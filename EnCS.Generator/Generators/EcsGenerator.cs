using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TemplateGenerator;
using TemplateGenerator.Helpers;

namespace EnCS.Generator
{
	struct EcsGeneratorData : IEquatable<EcsGeneratorData>, ITemplateData
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

		public string GetIdentifier()
			=> $"Component Generator ({ns}.{ecsName}) ({location})";
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

		public bool TryGetData(IdentifierNameSyntax node, SemanticModel semanticModel, out EcsGeneratorData data, out List<Diagnostic> diagnostics)
		{
			diagnostics = new();
			Unsafe.SkipInit(out data);

			if (node.Identifier.Text != "EcsBuilder")
				return false;

			if (!TryGetBuilderRoot(node, out var builderRoot))
				return false;

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var buildStep = builderSteps.Single(x => x.Name.Identifier.Text == "Build");
			var worldStep = builderSteps.First(x => x.Name.Identifier.Text == "World");
			var systemStep = builderSteps.First(x => x.Name.Identifier.Text == "System");
			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");
			var resourceStep = builderSteps.First(x => x.Name.Identifier.Text == "Resource");

			if (!ArchTypeGenerator.TryGetResourceManagers(semanticModel, resourceStep, diagnostics, out List<ResourceManager> resourceManagers))
				return false;

			if (!ArchTypeGenerator.TryGetArchTypes(semanticModel, archTypeStep, resourceManagers, diagnostics, out List<ArchType> archTypes))
				return false;

			if (!WorldGenerator.TryGetSystems(semanticModel, systemStep, diagnostics, out List<System> systems))
				return false;

			if (!WorldGenerator.TryGetWorlds(semanticModel, worldStep, systems, archTypes, diagnostics, out List<World> worlds))
				return false;

			// Intercept info
			var builderLocation = buildStep.Name.GetLocation();
			var loc = builderLocation.GetMappedLineSpan();

			data = new EcsGeneratorData(builderRoot.GetNamespace(), GetEcsName(node), builderRoot.GetLocation(), new(resourceManagers.ToArray()), new(archTypes.ToArray()), new(worlds.ToArray()), loc.Path, loc.StartLinePosition.Line + 1, loc.StartLinePosition.Character);
			return true;
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
