using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	static class WorldGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor TypeMustBeValidArchType = new("ECS006", "Type must be a valid arch type", "", "SystemGenerator", DiagnosticSeverity.Error, true);
	}

	struct WorldGeneratorData : IEquatable<WorldGeneratorData>
	{
		public string ns;
		public string ecsName;
		public Location location;

		public EquatableArray<World> worlds = EquatableArray<World>.Empty;

		public WorldGeneratorData(string ns, string ecsName, Location location, EquatableArray<World> worlds)
		{
			this.ns = ns;
			this.ecsName = ecsName;
			this.location = location;
			this.worlds = worlds;
		}

		public bool Equals(WorldGeneratorData other)
		{
			return worlds.Equals(other.worlds);
		}
	}

	class WorldGenerator : ITemplateSourceGenerator<IdentifierNameSyntax, WorldGeneratorData>
	{
		public string Template => ResourceReader.GetResource("World.tcs");

        public bool TryCreateModel(WorldGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();

			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), Parameter.Create(data.ns));
			model.Set("ecsName".AsSpan(), new Parameter<string>(data.ecsName));

			model.Set("worlds".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.worlds.Select(x => x.GetModel())));

			return true;
		}

		public WorldGeneratorData? Filter(IdentifierNameSyntax node, SemanticModel semanticModel)
		{
			if (node.Identifier.Text != "EcsBuilder")
				return null;

			var builderRoot = EcsGenerator.GetBuilderRoot(node);

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var worldStep = builderSteps.First(x => x.Name.Identifier.Text == "World");
			var systemStep = builderSteps.First(x => x.Name.Identifier.Text == "System");
			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");
			var resourceStep = builderSteps.First(x => x.Name.Identifier.Text == "Resource");

			if (!ArchTypeGenerator.TryGetResourceManagers(semanticModel, resourceStep, out List<ResourceManager> resourceManagers))
				return null;

			if (!TryGetSystems(semanticModel, systemStep, out List<System> systems))
				return null;

			if (!ArchTypeGenerator.TryGetArchTypes(semanticModel, archTypeStep, resourceManagers, new(), out List<ArchType> archTypes))
				return null;

			if (!TryGetWorlds(semanticModel, worldStep, systems, archTypes, new(), out List<World> worlds))
				return null;

			return new WorldGeneratorData(builderRoot.GetNamespace(), EcsGenerator.GetEcsName(node), builderRoot.GetLocation(), new(worlds.ToArray()));
		}

		public string GetName(WorldGeneratorData data)
			=> $"{data.ecsName}_World";

		public Location GetLocation(WorldGeneratorData data)
			=> data.location;

		public static bool TryGetWorlds(SemanticModel semanticModel, MemberAccessExpressionSyntax step, List<System> allSystems, List<ArchType> allArchTypes, List<Diagnostic> diagnostics, out List<World> worlds)
		{
			worlds = new List<World>();

			var parentExpression = step.Parent as InvocationExpressionSyntax;
			var lambda = parentExpression.ArgumentList.Arguments.Single().Expression as SimpleLambdaExpressionSyntax;

			int i = 1;
			foreach (var statement in lambda.Block.Statements.Where(x => x is ExpressionStatementSyntax).Cast<ExpressionStatementSyntax>())
			{
				if (statement.Expression is not InvocationExpressionSyntax invocation)
					continue;

				if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
					continue;

				if (memberAccess.Name is not GenericNameSyntax genericName)
					continue;

				if (genericName.Identifier.Text != "World")
					continue;

				if (invocation.ArgumentList.Arguments.Count == 0)
					continue;

				var nameArg = invocation.ArgumentList.Arguments[0].Expression as LiteralExpressionSyntax;
				var nameToken = nameArg.Token.ValueText;

				if (!TryGetWorldArchTypes(genericName, allArchTypes, diagnostics, out List<ArchType> worldArchTypes))
					continue;

				var worldSystems = GetWorldSystems(worldArchTypes, allSystems, diagnostics);
				var worldResourceManagers = worldSystems.SelectMany(x => x.resourceManagers).Concat(worldArchTypes.SelectMany(x => x.resourceManagers)).GroupBy(x => x.name).Select(x => x.First()).ToList();

				worlds.Add(new World()
				{
					name = nameToken,
					archTypes = new(worldArchTypes.ToArray()),
					systems = new(worldSystems.ToArray()),
					resourceManagers = new(worldResourceManagers.ToArray())
				});

				i++;
			}

			return worlds.Count > 0;
		}

		public static bool TryGetWorldArchTypes(GenericNameSyntax name, List<ArchType> validArchTypes, List<Diagnostic> diagnostics, out List<ArchType> archTypes)
		{
			archTypes = new List<ArchType>();

			foreach (TypeSyntax comp in name.TypeArgumentList.Arguments)
			{
				if (comp is IdentifierNameSyntax ident)
				{
					if (!validArchTypes.Any(x => x.name == ident.Identifier.Text))
					{
						diagnostics.Add(Diagnostic.Create(WorldGeneratorDiagnostics.TypeMustBeValidArchType, comp.GetLocation(), ""));
						return false;
					}

					archTypes.Add(validArchTypes.First(x => x.name == ident.Identifier.Text));
				}
				else if (comp is QualifiedNameSyntax qual)
				{
					var right = qual.Right as IdentifierNameSyntax;

					if (!validArchTypes.Any(x => x.name == right.Identifier.Text))
					{
						diagnostics.Add(Diagnostic.Create(WorldGeneratorDiagnostics.TypeMustBeValidArchType, comp.GetLocation(), ""));
						return false;
					}

					archTypes.Add(validArchTypes.First(x => x.name == right.Identifier.Text));
				}
				else
				{
					diagnostics.Add(Diagnostic.Create(WorldGeneratorDiagnostics.TypeMustBeValidArchType, comp.GetLocation(), ""));
					return false;
				}
			}

			return true;
		}

		static List<WorldSystem> GetWorldSystems(List<ArchType> worldArchTypes, List<System> allSystems, List<Diagnostic> diagnostics)
		{
			var models = new List<WorldSystem>();

			var worldComponentNames = worldArchTypes.SelectMany(x => x.components).Select(x => x.name);
			var resourceComponentNames = worldArchTypes.SelectMany(x => x.resourceComponents).Select(x => $"{x.resourceManager.ns}.{x.resourceManager.name}.{x.resourceManager.inType}");

			var names = worldComponentNames.Concat(resourceComponentNames);

			foreach (System system in allSystems)
			{
				// Filter out all systems wich this world cannot support
				if (!system.groups.SelectMany(x => x.components.Where(x => x.type == "Component" || x.type == "Resource")).Select(x => x.name).All(names.Contains))
					continue;

				List<ContainerGroup> systemGroupCompatibleContainers = new List<ContainerGroup>();
				foreach (var systemGroup in system.groups)
				{
					systemGroupCompatibleContainers.Add(new ContainerGroup()
					{
						containers = new(GetComptaibleContainers(worldArchTypes, systemGroup.components).ToArray()),
						contextArguments = system.contexts
					});
				}

				List<ContainerGroup> containerGroups = GetContainerCombinations(systemGroupCompatibleContainers.ToArray());

				var systemContainers = systemGroupCompatibleContainers.SelectMany(x => x.containers).GroupBy(x => x.name).Select(x => x.First()).ToList();
				var systemResourceManagers = systemContainers.SelectMany(x => x.components).Where(x => x.type == "Resource").Select(x => x.resourceManager).GroupBy(x => x.name).Select(x => x.First()).ToList();
				models.Add(new WorldSystem()
				{
					name = system.name,
					groups = new(containerGroups.ToArray()),
					containers = new(systemContainers.ToArray()),
					resourceManagers = new(systemResourceManagers.ToArray())
				});
			}

			return models;
		}

		static List<ContainerGroup> GetContainerCombinations(Span<ContainerGroup> groups)
		{
			if (groups == null || groups.Length == 0)
				return new List<ContainerGroup>();

			var combinations = new List<ContainerGroup>();
			if (groups.Length == 1)
			{
				foreach (var container in groups[0].containers)
				{
					var containers = new List<Container>();
					containers.Add(container);

					combinations.Add(new ContainerGroup()
					{
						containers = new(containers.ToArray()),
						contextArguments = groups[0].contextArguments
					});
				}
			}
			else
			{
				var nextCombinations = GetContainerCombinations(groups.Slice(1, groups.Length - 1));

				foreach (var container in groups[0].containers)
				{
					foreach (var nextContainer in nextCombinations)
					{
						var containers = new List<Container>();
						containers.Add(container);
						containers.AddRange(nextContainer.containers);

						combinations.Add(new ContainerGroup()
						{
							containers = new(containers.ToArray()),
							contextArguments = groups[0].contextArguments
						});
					}
				}
			}

			return combinations;
		}

		static List<Container> GetComptaibleContainers(List<ArchType> worldArchTypes, EquatableArray<MethodComponent> systemComps)
		{
			List<MethodComponent> filteredComponents = systemComps.Where(x => x.type == "Component" || x.type == "Resource").ToList();

			List<Container> models = new();
			foreach (var archType in worldArchTypes)
			{
				if (!IsArchTypeComatible(archType, filteredComponents))
					continue;

				var resourceManagers = filteredComponents.Where(x => x.type == "Resource").Select(x => x.resourceManager).GroupBy(x => x.name).Select(x => x.First()).ToList();

				models.Add(new Container()
				{
					name = archType.name,
					components = new(filteredComponents.ToArray()),
					resourceManagers = new(resourceManagers.ToArray())
				});
			}

			return models;
		}

		static bool IsArchTypeComatible(ArchType archType, List<MethodComponent> components)
		{
			foreach (var component in components)
			{
				string compResourceName = $"{component.resourceManager.ns}.{component.resourceManager.name}.{component.resourceManager.inType}";
				if (!archType.components.Any(x => x.name == component.name) && !archType.resourceComponents.Any(x => x.name == compResourceName))
					return false;
			}

			return true;
		}

		public static bool TryGetSystems(SemanticModel semanticModel, MemberAccessExpressionSyntax step, out List<System> systems)
		{
			systems = new List<System>();

			var parentExpression = step.Parent as InvocationExpressionSyntax;
			var lambda = parentExpression.ArgumentList.Arguments.Single().Expression as SimpleLambdaExpressionSyntax;

			foreach (var statement in lambda.Block.Statements.Where(x => x is ExpressionStatementSyntax).Cast<ExpressionStatementSyntax>())
			{
				if (statement.Expression is not InvocationExpressionSyntax invocation)
					continue;

				if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
					continue;

				if (memberAccess.Name is not GenericNameSyntax genericName)
					continue;

				if (genericName.Identifier.Text != "System")
					continue;

				foreach (IdentifierNameSyntax comp in genericName.TypeArgumentList.Arguments)
				{
					var foundSymbol = semanticModel.Compilation.GetSymbolsWithName(comp.ToFullString(), SymbolFilter.Type).Single();
					if (foundSymbol is not INamedTypeSymbol typeSymbol)
						throw new Exception();

					SystemGenerator.TryGetSystem(typeSymbol, new(), out System system);
					systems.Add(system);
				}
			}

			return systems.Count > 0;
		}
	}

	struct World : IEquatable<World>
	{
		public string name;
		public EquatableArray<ArchType> archTypes;
		public EquatableArray<WorldSystem> systems;
		public EquatableArray<ResourceManager> resourceManagers;

        public World()
        {
			archTypes = EquatableArray<ArchType>.Empty;
			systems = EquatableArray<WorldSystem>.Empty;
			resourceManagers = EquatableArray<ResourceManager>.Empty;
		}

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("worldName".AsSpan(), Parameter.Create(name));
			model.Set("worldArchTypes".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(archTypes.Select(x => x.GetModel())));
			model.Set("worldSystems".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(systems.Select(x => x.GetModel())));
			model.Set("worldResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceManagers.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(World other)
		{
			return name.Equals(other.name)
				&& archTypes.Equals(other.archTypes)
				&& systems.Equals(other.systems)
				&& resourceManagers.Equals(other.resourceManagers);
		}
	}

	struct WorldSystem : IEquatable<WorldSystem>
	{
		public string name;
		public EquatableArray<Container> containers;
		public EquatableArray<ContainerGroup> groups;
		public EquatableArray<ResourceManager> resourceManagers;

        public WorldSystem()
        {
			name = "";
			containers = EquatableArray<Container>.Empty;
			groups = EquatableArray<ContainerGroup>.Empty;
			resourceManagers = EquatableArray<ResourceManager>.Empty;

		}

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("systemName".AsSpan(), Parameter.Create(name));
			model.Set("systemContainers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(containers.Select(x => x.GetModel())));
			model.Set("systemGroups".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(groups.Select(x => x.GetModel())));
			model.Set("systemResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceManagers.Select(x => x.GetModel())));
			model.Set("systemContextArguments".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(groups.SelectMany(x => x.contextArguments).GroupBy(x => x.type).Select(x => x.First()).Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(WorldSystem other)
		{
			return name.Equals(other.name)
				&& containers.Equals(other.containers)
				&& groups.Equals(other.groups)
				&& resourceManagers.Equals(other.resourceManagers);
		}
	}

	struct ContainerGroup : IEquatable<ContainerGroup>
	{
		public EquatableArray<Container> containers;
		public EquatableArray<SystemContext> contextArguments;

        public ContainerGroup()
        {
			containers = EquatableArray<Container>.Empty;
			contextArguments = EquatableArray<SystemContext>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("groupContainers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(containers.Select(x => x.GetModel())));
			model.Set("groupResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(containers.SelectMany(x => x.resourceManagers).GroupBy(x => x.name).Select(x => x.First()).Select(x => x.GetModel())));
			model.Set("groupContextArguments".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(contextArguments.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(ContainerGroup other)
		{
			return containers.Equals(other.containers)
				&& contextArguments.Equals(other.contextArguments);
		}
	}

	struct Container : IEquatable<Container>
	{
		public string name;
		public EquatableArray<MethodComponent> components;
		public EquatableArray<ResourceManager> resourceManagers;

        public Container()
        {
			name = "";
            components = EquatableArray<MethodComponent>.Empty;
			resourceManagers = EquatableArray<ResourceManager>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("containerName".AsSpan(), Parameter.Create(name));
			model.Set("containerComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel())));
			model.Set("containerResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceManagers.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(Container other)
		{
			return name.Equals(other.name)
				&& components.Equals(other.components)
				&& resourceManagers.Equals(other.resourceManagers);
		}
	}
}
