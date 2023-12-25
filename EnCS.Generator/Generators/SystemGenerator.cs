using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	static class SystemGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor SystemUpdateMethodsMustBeEqual = new("ECS004", "All system update methods must be equal", "", "SystemGenerator", DiagnosticSeverity.Error, true);

		public static readonly DiagnosticDescriptor MethodArgumentsMustBeConcistent = new("ECS005", "All system update methods must only use Vector or Single types", "", "SystemGenerator", DiagnosticSeverity.Error, true);
	}

	class SystemGenerator : ITemplateSourceGenerator<ClassDeclarationSyntax>
	{
		public string Template => ResourceReader.GetResource("System.tcs");

		public bool TryCreateModel(Compilation compilation, ClassDeclarationSyntax node, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), new Parameter<string>(node.GetNamespace()));
			model.Set("name".AsSpan(), new Parameter<string>(node.Identifier.ToString()));

			TryGetResourceManagers(compilation, node, diagnostics, out List<ResourceManager> resourceManagers);
			var uniqueResourceManagers = resourceManagers.GroupBy(x => x.name).Select(x => x.First());
			model.Set("resourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(uniqueResourceManagers.Select(x => x.GetModel())));

			bool componentSuccess = TryGetComponents(compilation, node, resourceManagers, diagnostics, out List<SystemComponent> components);
			model.Set("components".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel())));

			bool methodSuccess = TryGetMethods(node, diagnostics, out List<SystemMethod> methods);
			model.Set("methods".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(methods.Select(x => x.GetModel())));

			return componentSuccess && methodSuccess;
		}

		public bool Filter(ClassDeclarationSyntax node)
		{
			foreach (AttributeListSyntax attributeListSyntax in node.AttributeLists)
			{
				foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
				{
					if ((attributeSyntax.Name as SimpleNameSyntax).Identifier.Text == "SystemAttribute")
						return true;

					if ((attributeSyntax.Name as SimpleNameSyntax).Identifier.Text == "System")
						return true;
				}
			}

			return false;
		}

		public string GetName(ClassDeclarationSyntax node)
		{
			return node.Identifier.ToString();
		}

		static bool TryGetResourceManagers(Compilation compilation, ClassDeclarationSyntax node, List<Diagnostic> diagnostics, out List<ResourceManager> resourceManagers)
		{
			resourceManagers = new List<ResourceManager>();

			var nodes = compilation.SyntaxTrees.SelectMany(x => x.GetRoot().DescendantNodesAndSelf());

			foreach (AttributeListSyntax attributeListSyntax in node.AttributeLists)
			{
				foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
				{
					if (attributeSyntax.Name is not GenericNameSyntax g)
						continue;

					if (g.Identifier.Text != "UsingResourceAttribute" && g.Identifier.Text != "UsingResource")
						continue;

					if (g.TypeArgumentList.Arguments.Count == 0)
						continue;

					var resourceManagerType = g.TypeArgumentList.Arguments[0] as IdentifierNameSyntax;
					var resourceManagerNode = nodes.FindNode<ClassDeclarationSyntax>(x => x.Identifier.Text == resourceManagerType.Identifier.Text);

					ResourceManagerGenerator.TryGetResourceManagers(compilation, resourceManagerNode, out resourceManagers);
				}
			}

			return resourceManagers.Count > 0;
		}

		public static bool TryGetComponents(Compilation compilation, ClassDeclarationSyntax node, List<ResourceManager> resourceManagers, List<Diagnostic> diagnostics, out List<SystemComponent> components)
		{
			components = new List<SystemComponent>();

			var nodes = compilation.SyntaxTrees.SelectMany(x => x.GetRoot().DescendantNodesAndSelf());
			var systemUpdateMethods = node.Members.Where(x => x is MethodDeclarationSyntax).Select(x => x as MethodDeclarationSyntax).Where(x => x.Identifier.Text == "Update" || x.Identifier.Text.StartsWith("Update"));

			if (systemUpdateMethods.Count() == 0)
				return false;

			var firstMethod = systemUpdateMethods.First();

			if (!IsMethodArgumentsEqual(firstMethod, systemUpdateMethods))
			{
				diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.SystemUpdateMethodsMustBeEqual, node.GetLocation(), ""));
				return false;
			}

			int idx = 1;
			foreach (var parameter in firstMethod.ParameterList.Parameters)
			{
				if (parameter.Type is QualifiedNameSyntax qualifiedType)
				{
					if (!TryGetNormalComponent(qualifiedType, nodes, idx, diagnostics, out SystemComponent component))
						continue;

					components.Add(component);
					idx++;
				}
				else if (parameter.Type is IdentifierNameSyntax identifierType) // Assume this is a resource
				{
					if (!TryGetResourceComponent(identifierType, resourceManagers, idx, diagnostics, out SystemComponent component))
						continue;

					components.Add(component);
					idx++;
				}

			}

			return components.Count > 0;
		}

		static bool TryGetNormalComponent(QualifiedNameSyntax type, IEnumerable<SyntaxNode> nodes, int idx, List<Diagnostic> diagnostics, out SystemComponent component)
		{
			var paramName = (type.Left as IdentifierNameSyntax).Identifier.Text;
			var componentNode = nodes.FindNode<StructDeclarationSyntax>(x => x.Identifier.Text == paramName);

			if (!ComponentGenerator.IsValidComponent(componentNode, diagnostics))
			{
				component = default;
				return false;
			}

			component = new SystemComponent()
			{
				name = $"{componentNode.GetNamespace()}.{paramName}",
				idx = idx,
				type = "Component"
			};

			return true;
		}

		static bool TryGetResourceComponent(IdentifierNameSyntax type, List<ResourceManager> resourceManagers, int idx, List<Diagnostic> diagnostics, out SystemComponent component)
		{
			var resourceManager = resourceManagers.First(x => x.type == type.Identifier.Text);

			component = new SystemComponent()
			{
				name = $"{resourceManager.ns}.{resourceManager.name}.{resourceManager.type}",
				idx = idx,
				type = "Resource",
				resourceManager = resourceManager
			};

			return true;
		}

		static bool IsMethodArgumentsEqual(MethodDeclarationSyntax method, IEnumerable<MethodDeclarationSyntax> methods)
		{
			foreach (var item in methods)
			{
				for (int i = 0; i < method.ParameterList.Parameters.Count; i++)
				{
					// Non qualidfied names are assumed to be resources, and they are not equal to vectorized versions.
					if (method.ParameterList.Parameters[i].Type is not QualifiedNameSyntax)
						continue;

					if (method.ParameterList.Parameters[i].Identifier.Text != item.ParameterList.Parameters[i].Identifier.Text)
						return false;
				}
			}

			return true;
		}

		static bool TryGetMethods(ClassDeclarationSyntax node, List<Diagnostic> diagnostics, out List<SystemMethod> methods)
		{
			methods = new List<SystemMethod>();

			foreach (var method in node.Members.Where(x => x is MethodDeclarationSyntax).Select(x => x as MethodDeclarationSyntax))
			{
				if (method.Identifier.Text != "Update" && !method.Identifier.Text.StartsWith("Update"))
					continue;

				if (method.ParameterList.Parameters.Count == 0)
					return false;

				var paramType = method.ParameterList.Parameters[0].Type as QualifiedNameSyntax;
				var name = paramType.Right as IdentifierNameSyntax;

				if (!IsMethodArgumentsConsistent(method))
				{
					diagnostics.Add(Diagnostic.Create(SystemGeneratorDiagnostics.MethodArgumentsMustBeConcistent, method.GetLocation(), ""));
					return false;
				}

				methods.Add(new SystemMethod()
				{
					name = method.Identifier.Text,
					type = name.Identifier.Text == "Ref" ? "Single" : "Vector"
				});
			}

			return methods.Count > 0;
		}

		static bool IsMethodArgumentsConsistent(MethodDeclarationSyntax method)
		{
			var firstParamType = method.ParameterList.Parameters[0].Type as QualifiedNameSyntax;
			var firstName = firstParamType.Right as IdentifierNameSyntax;

			for (int i = 0; i < method.ParameterList.Parameters.Count; i++)
			{
				if (method.ParameterList.Parameters[i].Type is not QualifiedNameSyntax paramType)
					continue;

				var name = paramType.Right as IdentifierNameSyntax;

				if (name.Identifier.Text != firstName.Identifier.Text)
					return false;
			}

			return true;
		}
	}

	struct SystemMethod
	{
		public string name;
		public string type;

		public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("methodName".AsSpan(), Parameter.Create(name));
			model.Set("methodType".AsSpan(), Parameter.Create(type));

			return model;
		}
	}

	struct SystemComponent
	{
		public string name;
		public int idx;
		public string type;
		public ResourceManager resourceManager;

		public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("compName".AsSpan(), Parameter.Create(name));
			model.Set("compIdx".AsSpan(), Parameter.Create<float>(idx));
			model.Set("compType".AsSpan(), Parameter.Create(type));
			model.Set("compResourceManager".AsSpan(), Parameter.Create((IModel<ReturnType>)resourceManager.GetModel()));

			return model;
		}
	}
}
