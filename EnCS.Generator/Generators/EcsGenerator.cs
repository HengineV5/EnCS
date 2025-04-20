using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	struct EcsGeneratorData : IEquatable<EcsGeneratorData>
	{
		public string ns;
		public string ecsName;
		public Location location;

		public EquatableArray<ResourceManager> resourceManagers = EquatableArray<ResourceManager>.Empty;
		public EquatableArray<ArchType> archTypes = EquatableArray<ArchType>.Empty;
		public EquatableArray<World> worlds = EquatableArray<World>.Empty;

		public string path;
		public int line;
		public int character;

		public EcsGeneratorData(string ns, string ecsName, Location location, EquatableArray<ResourceManager> resourceManagers, EquatableArray<ArchType> archTypes, EquatableArray<World> worlds, string path, int line, int character)
		{
			this.ns = ns;
			this.ecsName = ecsName;
			this.location = location;
			this.resourceManagers = resourceManagers;
			this.archTypes = archTypes;
			this.worlds = worlds;
			this.path = path;
			this.line = line;
			this.character = character;
		}

		public bool Equals(EcsGeneratorData other)
		{
			return resourceManagers.Equals(other.resourceManagers)
				&& archTypes.Equals(other.archTypes)
				&& worlds.Equals(other.worlds);
		}
	}

	class EcsGenerator : ITemplateSourceGenerator<IdentifierNameSyntax, EcsGeneratorData>
	{
		public string Template => ResourceReader.GetResource("Ecs.tcs");

		public bool TryCreateModel(EcsGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();

			// Ecs Info
			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), Parameter.Create(data.ns));
			model.Set("name".AsSpan(), Parameter.Create(data.ecsName));

			model.Set("resourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.resourceManagers.Select(x => x.GetModel())));
			model.Set("archTypes".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.archTypes.Select(x => x.GetModel())));
			model.Set("worlds".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.worlds.Select(x => x.GetModel())));

			// Intercept info
			model.Set("path".AsSpan(), Parameter.Create(data.path));
			model.Set("line".AsSpan(), Parameter.Create<float>(data.line));
			model.Set("character".AsSpan(), Parameter.Create<float>(data.character));

			return true;
		}

		public EcsGeneratorData? Filter(IdentifierNameSyntax node, SemanticModel semanticModel)
		{
			if (node.Identifier.Text != "EcsBuilder")
				return null;

			if (!TryGetBuilderRoot(node, out var builderRoot))
				return null;

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var buildStep = builderSteps.Single(x => x.Name.Identifier.Text == "Build");
			var worldStep = builderSteps.First(x => x.Name.Identifier.Text == "World");
			var systemStep = builderSteps.First(x => x.Name.Identifier.Text == "System");
			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");
			var resourceStep = builderSteps.First(x => x.Name.Identifier.Text == "Resource");

			if (!ArchTypeGenerator.TryGetResourceManagers(semanticModel, resourceStep, out List<ResourceManager> resourceManagers))
				return null;

			if (!ArchTypeGenerator.TryGetArchTypes(semanticModel, archTypeStep, resourceManagers, new(), out List<ArchType> archTypes))
				return null;

			if (!WorldGenerator.TryGetSystems(semanticModel, systemStep, out List<System> systems))
				return null;

			if (!WorldGenerator.TryGetWorlds(semanticModel, worldStep, systems, archTypes, new(), out List<World> worlds))
				return null;

			// Intercept info
			var builderLocation = buildStep.Name.GetLocation();
			var loc = builderLocation.GetMappedLineSpan();

			return new EcsGeneratorData(builderRoot.GetNamespace(), GetEcsName(node), builderRoot.GetLocation(), new(resourceManagers.ToArray()), new(archTypes.ToArray()), new(worlds.ToArray()), loc.Path, loc.StartLinePosition.Line + 1, loc.StartLinePosition.Character);
		}

		public string GetName(EcsGeneratorData data)
			=> data.ecsName;

		public Location GetLocation(EcsGeneratorData data)
			=> data.location;

		public static bool TryGetBuilderRoot(SyntaxNode node, out SyntaxNode root)
		{
			if (node is StatementSyntax)
			{
				root = node;
				return true;
			}

			if (node.Parent == null)
			{
				root = null;
				return false;
			}

			return TryGetBuilderRoot(node.Parent, out root);
		}

		public static string GetEcsName(IdentifierNameSyntax node)
		{
			if (!TryGetBuilderRoot(node, out var builderRoot))
				return null;

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var buildStep = builderSteps.Single(x => x.Name.Identifier.Text == "Build");
			var genricName = buildStep.Name as GenericNameSyntax;

			return genricName.TypeArgumentList.Arguments[0].ToString();
		}
	}
}
