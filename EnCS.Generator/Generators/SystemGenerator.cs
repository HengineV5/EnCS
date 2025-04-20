using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	static class SystemGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor SystemUpdateMethodsMustBeEqual = new("ECS004", "All system update methods within the group must be equal", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodArgumentsMustBeConcistent = new("ECS005", "All system update methods must only use Vector or Single types", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodArgumentMustBeComponentOrResourceOrContext = new("ECS007", "All system method arguments must be a valid component, resource or context parameter", "", "SystemGenerator", DiagnosticSeverity.Error, true);
		
		public static readonly DiagnosticDescriptor MethodCannotBeEmpty = new("ECS008", "System update method cannot be empty", "", "SystemGenerator", DiagnosticSeverity.Warning, true);

		public static readonly DiagnosticDescriptor MethodCannotBeInMoreThanOneGroup = new("ECS009", "System update method cannot be in more than one group", "", "SystemGenerator", DiagnosticSeverity.Error, true);
		
		public static readonly DiagnosticDescriptor MethodsWithinGroupMustHaveIdenticalChunk = new("ECS010", "Methods within a group must have identical chunk sizes", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor PreOrPostLoopCanOnlyHaveContextArgs = new("ECS011", "Pre and post loop methods can only have system contexts as arguments", "", "SystemGenerator", DiagnosticSeverity.Error, true);
	}

	struct SystemGeneratorData : IEquatable<SystemGeneratorData>
	{
		public INamedTypeSymbol node;
		public Location location;

		public System system;

		public SystemGeneratorData(INamedTypeSymbol node, Location location, System system)
		{
			this.node = node;
			this.location = location;
			this.system = system;
		}

		public bool Equals(SystemGeneratorData other)
		{
			return system.Equals(other.system);
		}
	}

	class SystemGenerator : ITemplateSourceGenerator<ClassDeclarationSyntax, SystemGeneratorData>
	{
		public string Template => ResourceReader.GetResource("System.tcs");

		public bool TryCreateModel(SystemGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), new Parameter<string>(data.node.ContainingNamespace.ToString()));
			model.Set("name".AsSpan(), new Parameter<string>(data.node.Name));

			model.Set("resourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.system.resourceManagers.Select(x => x.GetModel())));
			model.Set("groups".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.system.groups.Select(x => x.GetModel())));
			model.Set("reversedGroups".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.system.groups.AsEnumerable().Reverse().Select(x => x.GetModel())));
			model.Set("contexts".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.system.contexts.Select(x => x.GetModel())));

			return true;
		}

		public SystemGeneratorData? Filter(ClassDeclarationSyntax node, SemanticModel semanticModel)
		{
			if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol typeSymbol)
				return null;

			if (!TryGetSystem(typeSymbol, new(), out var system))
				return null;

			return new SystemGeneratorData(typeSymbol, node.GetLocation(), system);
		}

		public string GetName(SystemGeneratorData data)
			=> data.node.Name;

		public Location GetLocation(SystemGeneratorData data)
			=> data.location;

		public static bool TryGetSystem(INamedTypeSymbol node, List<Diagnostic> diagnostics, out System system)
		{
			TryGetResourceManagers(node, diagnostics, out List<ResourceManager> resourceManagers);

			TryGetSystemContexts(node, diagnostics, out List<SystemContext> contexts);
			TryGetPrePostLoopMethods(node, resourceManagers, contexts, diagnostics, out List<SystemMethod> preLoops, out List<SystemMethod> postLoops);

			bool methodSuccess = TryGetMethods(node, resourceManagers, contexts, diagnostics, out List<SystemMethod> methods);
			bool groupSuccess = TryGetGroups(methods, preLoops, postLoops, out List<SystemGroup> groups);

			var correctOrderResourceManagers = methods.SelectMany(x => x.components).Where(x => x.type == "Resource").Select(x => x.resourceManager);
			var uniqueResourceManagers = correctOrderResourceManagers.GroupBy(x => x.name).Select(x => x.First());

			system = new System()
			{
				name = node.Name,
				groups = new(groups.ToArray()),
				resourceManagers = new(uniqueResourceManagers.ToArray()),
				contexts = new(contexts.ToArray())
			};

			return methodSuccess && groupSuccess;
		}

		static bool TryGetSystemContexts(INamedTypeSymbol node, List<Diagnostic> diagnostics, out List<SystemContext> contexts)
		{
			contexts = new List<SystemContext>();

			foreach (var attribute in node.GetAttributes())
			{
				var attribName = attribute.AttributeClass.Name;
				if (attribName != "SystemContext" && attribName != "SystemContextAttribute") // TODO: Might not need reduncancy check for shorform
					continue;

				if (attribute.AttributeClass is null)
					continue;

				if (attribute.AttributeClass.TypeArguments.Length == 0)
					continue;

				foreach (var type in attribute.AttributeClass.TypeArguments)
				{
					contexts.Add(new SystemContext()
					{
						type = type.Name
					});
				}
			}

			return true;
		}

		static bool TryGetPrePostLoopMethods(INamedTypeSymbol node, List<ResourceManager> resourceManagers, List<SystemContext> contexts, List<Diagnostic> diagnostics, out List<SystemMethod> preLoops, out List<SystemMethod> postLoops)
		{
			preLoops = new List<SystemMethod>();
			postLoops = new List<SystemMethod>();

			foreach (var member in node.GetMembers())
			{
				if (member is not IMethodSymbol method)
					continue;

				bool preLoop = IsMethodPreLoop(method);
				bool postLoop = IsMethodPostLoop(method);

				if (!preLoop && !postLoop)
					continue;

				if (!TryGetMethodGroup(method, out int group))
				{
					//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodCannotBeInMoreThanOneGroup, preLoop.GetLocation(), ""));
					continue;
				}

				TryGetMethodComponents(method, resourceManagers, contexts, diagnostics, out List<MethodComponent> components);
				if (components.Count != 0 && components.Any(x => x.type != "Context"))
				{
					//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.PreOrPostLoopCanOnlyHaveContextArgs, preLoop.GetLocation(), ""));
					continue;
				}

				var systemMethod = new SystemMethod()
				{
					group = group,
					name = member.Name,
					type = preLoop ? "PreLoop" : "PostLoop",
					chunk = 0,
					components = new(components.ToArray())
				};

				if (preLoop)
					preLoops.Add(systemMethod);
				else if (postLoop)
					postLoops.Add(systemMethod);
			}

			return true;
		}

		public static bool TryGetResourceManagers(INamedTypeSymbol node, List<Diagnostic> diagnostics, out List<ResourceManager> resourceManagers)
		{
			resourceManagers = new List<ResourceManager>();

			foreach (var attribute in node.GetAttributes())
			{
				var attribName = attribute.AttributeClass.Name;
				if (attribName != "UsingResourceAttribute" && attribName != "UsingResource") // TODO: Might not need reduncancy check for shorform
					continue;

				if (attribute.AttributeClass is null)
					continue;

                foreach (var resourceManager in attribute.AttributeClass.TypeArguments)
                {
					ResourceManagerGenerator.TryGetResourceManagers(resourceManager as INamedTypeSymbol, out List<ResourceManager> foundResourceManagers);
					resourceManagers.AddRange(foundResourceManagers);
				}
			}

			return resourceManagers.Count > 0;
		}

		static bool TryGetGroups(List<SystemMethod> methods, List<SystemMethod> preLoops, List<SystemMethod> postLoops, out List<SystemGroup> groups)
		{
			groups = new List<SystemGroup>();
			var groupedMethods = methods.GroupBy(x => x.group);

			foreach (var methodGroup in groupedMethods)
			{
				int group = methodGroup.First().group;
				int chunk = methodGroup.First().chunk;

				if (!methodGroup.All(x => x.chunk == chunk))
				{
					// TODO: Add error here
					continue;
				}

				groups.Add(new SystemGroup()
				{
					idx = group,
					chunk = chunk,
					components = methodGroup.First().components,
					methods = new(methodGroup.ToArray()),
					preLoops = new(preLoops.Where(x => x.group == group).ToArray()),
					postLoops = new(postLoops.Where(x => x.group == group).ToArray())
				});
			}

			groups = groups.OrderBy(x => x.idx).ToList();
			return groups.Count > 0;
		}

		static bool TryGetGroupComponents(INamedTypeSymbol node, int group, List<ResourceManager> resourceManagers, List<SystemContext> contexts, List<Diagnostic> diagnostics, out List<MethodComponent> components)
		{
			components = new List<MethodComponent>();

			var systemUpdateMethods = new List<IMethodSymbol>();	
			foreach (var member in node.GetMembers())
			{
				if (member is not IMethodSymbol method)
					continue;

				if (!IsMethodSystemUpdate(method))
					continue;

				if (!TryGetMethodGroup(method, out int g))
					continue;

				if (g != group)
					continue;

				systemUpdateMethods.Add(method);
			}

			if (systemUpdateMethods.Count() == 0)
				return false;

			var firstMethod = systemUpdateMethods[0];

			if (!IsMethodArgumentsEqual(firstMethod, systemUpdateMethods))
			{
				//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.SystemUpdateMethodsMustBeEqual, node.GetLocation(), ""));
				return false;
			}

			TryGetMethodComponents(firstMethod, resourceManagers, contexts, diagnostics, out components);

			return components.Count > 0;
		}

		static bool TryGetMethodComponents(IMethodSymbol method, List<ResourceManager> resourceManagers, List<SystemContext> contexts, List<Diagnostic> diagnostics, out List<MethodComponent> components)
		{
			components = new List<MethodComponent>();

			int idx = 1;
			foreach (var parameter in method.Parameters)
			{
				bool resourceSuccess = TryGetResourceComponent(parameter.Type, resourceManagers, idx, diagnostics, out MethodComponent resourceComponent);
				bool contextSuccess = TryGetContextComponent(parameter.Type, contexts, out MethodComponent contextComponent);

				if (resourceSuccess || contextSuccess) // TODO: Improve generated componente detection.
				{
					if (resourceSuccess)
					{
						components.Add(resourceComponent);
					}
					else if (contextSuccess)
					{
						components.Add(contextComponent);
						continue;
					}
					else
					{
						//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodArgumentMustBeComponentOrResourceOrContext, identifierType.GetLocation(), ""));
						continue;
					}

					idx++;
				}
				else
				{
					if (!TryGetNormalComponent(parameter.Type, idx, out MethodComponent component))
						continue;

					components.Add(component);
					idx++;
				}

			}

			return components.Count > 0;
		}

		static bool TryGetNormalComponent(ITypeSymbol type, int idx, out MethodComponent component)
		{
			if (!ComponentGenerator.IsValidComponent(type))
			{
				component = default;
				return false;
			}

			component = new MethodComponent()
			{
				name = $"{type.ContainingNamespace.ToString()}.{type.OriginalDefinition.Name}",
				idx = idx,
				type = "Component"
			};

			return true;
		}

		static bool TryGetResourceComponent(ITypeSymbol type, List<ResourceManager> resourceManagers, int idx, List<Diagnostic> diagnostics, out MethodComponent component)
		{
			foreach (var resourceManager in resourceManagers)
			{
				if (resourceManager.outType != type.Name)
					continue;

				component = new MethodComponent()
				{
					name = $"{resourceManager.ns}.{resourceManager.name}.{resourceManager.inType}",
					idx = idx,
					type = "Resource",
					resourceManager = resourceManager
				};

				return true;
			}

			component = default;
			return false;
		}

		static bool TryGetContextComponent(ITypeSymbol type, List<SystemContext> contexts, out MethodComponent component)
		{
			foreach (var contextComponent in contexts)
			{
				if (contextComponent.type != type.Name)
					continue;

				component = new MethodComponent()
				{
					name = contextComponent.type,
					idx = 0,
					type = "Context"
				};

				return true;
			}

			component = default;
			return false;
		}

		static bool IsMethodArgumentsEqual(IMethodSymbol method, IEnumerable<IMethodSymbol> methods)
		{
			return true; // TODO: Fix

            foreach (var item in methods)
			{
				if (method.Parameters.Length != item.Parameters.Length)
					return false;

				for (int i = 0; i < method.Parameters.Length; i++)
				{
					if (method.Parameters[i].Name != item.Parameters[i].Name)
						return false;
				}
			}

			return true;
		}

		public static bool TryGetMethods(INamedTypeSymbol node, List<ResourceManager> resourceManagers, List<SystemContext> contexts, List<Diagnostic> diagnostics, out List<SystemMethod> methods)
		{
			methods = new List<SystemMethod>();

			foreach (var member in node.GetMembers())
			{
				if (member is not IMethodSymbol method)
					continue;

				if (!IsMethodSystemUpdate(method, diagnostics))
					continue;

				if (!TryGetMethodGroup(method, out int group))
				{
					//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodCannotBeInMoreThanOneGroup, method.GetLocation(), ""));
					continue;
				}

				if (!TryGetMethodChunk(method, out int chunk))
					continue;

				if (!TryGetGroupComponents(node, group, resourceManagers, contexts, diagnostics, out List<MethodComponent> components))
					continue;

				methods.Add(new SystemMethod()
				{
					name = method.Name,
					type = GetMethodType(method),
					group = group,
					chunk = chunk,
					components = new(components.ToArray())
				});
			}

            return methods.Count > 0;
		}

		static bool TryGetMethodGroup(IMethodSymbol method, out int group)
		{
			var attribs = method.GetAttributes();
			var groupAttribute = method.GetAttributes().Where(x => (x.AttributeClass.Name == "SystemLayer" || x.AttributeClass.Name == "SystemLayerAttribute"));
			group = 0;

			if (groupAttribute.Count() > 1)
				return false;


			if (groupAttribute.Count() == 1)
			{
				var gr = groupAttribute.Single();
				group = int.Parse(groupAttribute.Single().ConstructorArguments[0].Value?.ToString());
			}

			return true;
		}

		static bool TryGetMethodChunk(IMethodSymbol method, out int chunk)
		{
			var groupAttribute = method.GetAttributes().Where(x => x.AttributeClass.Name == "SystemLayer" || x.AttributeClass.Name == "SystemLayerAttribute");

			chunk = 0;
			if (groupAttribute.Count() > 1)
				return false;

			if (groupAttribute.Count() == 1)
			{
				var args = groupAttribute.Single().ConstructorArguments;

				if (args.Length > 1)
					chunk = int.Parse(args[1].Value.ToString());
			}	

			return true;
		}

		static bool IsMethodSystemUpdate(IMethodSymbol method, List<Diagnostic> diagnostics)
		{
			if (!IsMethodSystemUpdate(method))
				return false;

			if (method.Parameters.Length == 0)
			{
				//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodCannotBeEmpty, method.GetLocation(), ""));
				return false;
			}

			if (!IsMethodArgumentsConsistent(method))
			{
				//diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodArgumentsMustBeConcistent, method.GetLocation(), ""));
				return false;
			}

			return true;
		}

		static bool IsMethodSystemUpdate(IMethodSymbol method)
		{
			if (!method.GetAttributes().Any(x => x.AttributeClass.Name == "SystemUpdate" || x.AttributeClass.Name == "SystemUpdateAttribute"))
				return false;

			return true;
		}

		static bool IsMethodPreLoop(IMethodSymbol method)
		{
			if (!method.GetAttributes().Any(x => x.AttributeClass.Name == "SystemPreLoop" || x.AttributeClass.Name == "SystemPreLoopAttribute"))
				return false;

			return true;
		}

		static bool IsMethodPostLoop(IMethodSymbol method)
		{
			if (!method.GetAttributes().Any(x => x.AttributeClass.Name == "SystemPostLoop" || x.AttributeClass.Name == "SystemPostLoopAttribute"))
				return false;

			return true;
		}

		// Check if method has only vectorized or only single components.
		static bool IsMethodArgumentsConsistent(IMethodSymbol method)
		{
			string methodType = GetMethodType(method);
			foreach (var parameter in method.Parameters)
			{
				if (parameter.Type.OriginalDefinition is not INamedTypeSymbol) // TODO: Improve component detection
					continue;

				var parameterType = parameter.Type.Name == "Vectorized" ? "Vector" : "Single";
				if (parameterType != methodType)
					return false;
			}

			return true;
		}

		static string GetMethodType(IMethodSymbol method)
		{
			foreach (var parameter in method.Parameters)
			{
				if (parameter.Type.OriginalDefinition is not INamedTypeSymbol) // TODO: Improve component detection
					continue;

				return parameter.Type.Name == "Vectorized" ? "Vector" : "Single";
			}

			throw new Exception("Method neither vector or single");
		}
	}

	struct System : IEquatable<System>
	{
		public string name;
		public EquatableArray<ResourceManager> resourceManagers;
		public EquatableArray<SystemGroup> groups;
		public EquatableArray<SystemContext> contexts;

        public System()
        {
			name = "";
            resourceManagers = EquatableArray<ResourceManager>.Empty;
			groups = EquatableArray<SystemGroup>.Empty;
			contexts = EquatableArray<SystemContext>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("systemName".AsSpan(), Parameter.Create(name));
			model.Set("systemResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceManagers.Select(x => x.GetModel())));
			model.Set("systemGroups".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(groups.Select(x => x.GetModel())));
			model.Set("systemReversedGroups".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(groups.AsEnumerable().Reverse().Select(x => x.GetModel())));
			model.Set("systemContexts".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(contexts.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(System other)
		{
			return name.Equals(other.name)
				&& resourceManagers.Equals(other.resourceManagers)
				&& groups.Equals(other.groups)
				&& contexts.Equals(other.contexts);
		}
	}

	struct SystemMethod : IEquatable<SystemMethod>
	{
		public string name;
		public string type;
		public int group;
		public int chunk;
		public EquatableArray<MethodComponent> components;

        public SystemMethod()
        {
			name = "";
			type = "";
			group = 0;
			chunk = 0;
            components = EquatableArray<MethodComponent>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("methodName".AsSpan(), Parameter.Create(name));
			model.Set("methodType".AsSpan(), Parameter.Create(type));
			model.Set("methodGroup".AsSpan(), Parameter.Create<float>(group));
			model.Set("methodChunk".AsSpan(), Parameter.Create<float>(chunk));
			model.Set("methodComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(SystemMethod other)
		{
			return name.Equals(other.name)
				&& type.Equals(other.type)
				&& group.Equals(other.group)
				&& chunk.Equals(other.chunk)
				&& components.Equals(other.components);
		}
	}

	struct SystemGroup : IEquatable<SystemGroup>
	{
		public int idx;
		public int chunk;
		public EquatableArray<MethodComponent> components;
		public EquatableArray<SystemMethod> methods;
		public EquatableArray<SystemMethod> preLoops;
		public EquatableArray<SystemMethod> postLoops;

        public SystemGroup()
        {
			idx = 0;
			chunk = 0;
            components = EquatableArray<MethodComponent>.Empty;
			methods = EquatableArray<SystemMethod>.Empty;
			preLoops = EquatableArray<SystemMethod>.Empty;
			postLoops = EquatableArray<SystemMethod>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("groupIdx".AsSpan(), Parameter.Create<float>(idx));
			model.Set("groupChunk".AsSpan(), Parameter.Create<float>(chunk));
			model.Set("groupComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel())));
			model.Set("groupMethods".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(methods.Select(x => x.GetModel())));
			model.Set("groupPreLoops".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(preLoops.Select(x => x.GetModel())));
			model.Set("groupPostLoops".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(postLoops.Select(x => x.GetModel())));

			List<IModel<ReturnType>> modelList = new List<IModel<ReturnType>>();
			if (chunk != 0 && (preLoops.Count > 0 || postLoops.Count > 0))
				modelList.Add(new Model<ReturnType>());

			model.Set("groupHasPreOrPostLoop".AsSpan(), Parameter.CreateEnum(modelList));

			return model;
		}

		public bool Equals(SystemGroup other)
		{
			return idx.Equals(other.idx)
				&& chunk.Equals(other.chunk)
				&& components.Equals(other.components)
				&& methods.Equals(other.methods)
				&& preLoops.Equals(other.preLoops)
				&& postLoops.Equals(other.postLoops);
		}
	}

	struct MethodComponent : IEquatable<MethodComponent>
	{
		public string name;
		public int idx;
		public string type;
		public ResourceManager resourceManager;

        public MethodComponent()
        {
			name = "";
			idx = 0;
			type = "";
			resourceManager = new ResourceManager();
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("compName".AsSpan(), Parameter.Create(name));
			model.Set("compIdx".AsSpan(), Parameter.Create<float>(idx));
			model.Set("compType".AsSpan(), Parameter.Create(type));
			model.Set("compResourceManager".AsSpan(), Parameter.Create((IModel<ReturnType>)resourceManager.GetModel()));

			return model;
		}

		public bool Equals(MethodComponent other)
		{
			return name.Equals(other.name)
				&& idx.Equals(other.idx)
				&& type.Equals(other.type)
				&& resourceManager.Equals(other.resourceManager);
		}
	}

	struct SystemContext : IEquatable<SystemContext>
	{
		public string type;

        public SystemContext()
        {
			type = "";
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("contextType".AsSpan(), Parameter.Create(type));

			return model;
		}

		public bool Equals(SystemContext other)
		{
			return type.Equals(other.type);
		}
	}
}
