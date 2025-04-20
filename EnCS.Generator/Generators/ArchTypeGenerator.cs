using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	static class ArchTypeGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor ArchTypeMustBeValidComponent = new("ECS003", "Archtype can only contain valid components", "Archtype member is not valid component", "ArchTypeGenerator", DiagnosticSeverity.Error, true);
	}

	struct ArchTypeGeneratorData : IEquatable<ArchTypeGeneratorData>
	{
		public string ns;
		public string ecsName;
		public Location location;

		public EquatableArray<ArchType> archTypes = EquatableArray<ArchType>.Empty;

        public ArchTypeGeneratorData(string ns, string ecsName, Location location, EquatableArray<ArchType> archTypes)
		{
			this.ns = ns;
			this.ecsName = ecsName;
			this.location = location;
			this.archTypes = archTypes;
		}

		public bool Equals(ArchTypeGeneratorData other)
		{
			return archTypes.Equals(other.archTypes);
		}
	}

	class ArchTypeGenerator : ITemplateSourceGenerator<IdentifierNameSyntax, ArchTypeGeneratorData>
	{
		public Guid Id { get; } = Guid.NewGuid();

		public string Template => ResourceReader.GetResource("ArchType.tcs");

        public bool TryCreateModel(ArchTypeGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();

			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), Parameter.Create(data.ns));
			model.Set("ecsName".AsSpan(), new Parameter<string>(data.ecsName));

			model.Set("archTypes".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.archTypes.Select(x => x.GetModel())));

			return true;
		}

		public ArchTypeGeneratorData? Filter(IdentifierNameSyntax node, SemanticModel semanticModel)
		{
			if (node.Identifier.Text != "EcsBuilder")
				return null;

			if (!EcsGenerator.TryGetBuilderRoot(node, out var builderRoot))
				return null;

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");
			var resourceStep = builderSteps.First(x => x.Name.Identifier.Text == "Resource");

			if (!TryGetResourceManagers(semanticModel, resourceStep, out List<ResourceManager> resourceManagers))
				return null;

			if (!TryGetArchTypes(semanticModel, archTypeStep, resourceManagers, new(), out List<ArchType> archTypes))
				return null;

			return new ArchTypeGeneratorData(builderRoot.GetNamespace(), EcsGenerator.GetEcsName(node), builderRoot.GetLocation(), new(archTypes.ToArray()));
		}

		public string GetName(ArchTypeGeneratorData data)
			=> $"{data.ecsName}_ArchType";

		public Location GetLocation(ArchTypeGeneratorData data)
			=> data.location;

		public static bool TryGetResourceManagers(SemanticModel semanticModel, MemberAccessExpressionSyntax step, out List<ResourceManager> resourceManagers)
		{
			resourceManagers = new List<ResourceManager>();

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

				if (genericName.Identifier.Text != "ResourceManager")
					continue;

				var resourceManagerType = genericName.TypeArgumentList.Arguments[0] as IdentifierNameSyntax;

				var foundSymbol = semanticModel.Compilation.GetSymbolsWithName(resourceManagerType.ToFullString(), SymbolFilter.Type).Single();
				if (foundSymbol is not INamedTypeSymbol typeSymbol)
					throw new Exception();

				ResourceManagerGenerator.TryGetResourceManagers(typeSymbol, out List<ResourceManager> localResourceManagers);
				resourceManagers.AddRange(localResourceManagers);
			}

			return true;
		}

		public static bool TryGetArchTypes(SemanticModel semanticModel, MemberAccessExpressionSyntax step, List<ResourceManager> resourceManagers, List<Diagnostic> diagnostics, out List<ArchType> models)
		{
			models = new List<ArchType>();

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

				if (genericName.Identifier.Text != "ArchType")
					continue;

				var nameArg = invocation.ArgumentList.Arguments[0].Expression as LiteralExpressionSyntax;
				var nameToken = nameArg.Token.ValueText;

				bool compSuccess = TryGetComponents(semanticModel, genericName, resourceManagers, diagnostics, out List<Component> components);
				bool resourceCompSuccess = TryGetResourceComponents(semanticModel, genericName, resourceManagers, out List<ResourceComponent> resourceComponents);

				if (!compSuccess && !resourceCompSuccess)
					continue;

				var uniqueResourcManagers = resourceComponents.Select(x => x.resourceManager).GroupBy(x => x.name).Select(x => x.First()).ToList();

				models.Add(new ArchType()
				{
					name = nameToken,
					components = new(components.ToArray()),
					resourceComponents = new(resourceComponents.ToArray()),
					resourceManagers = new(uniqueResourcManagers.ToArray())
				});
			}

			return models.Count > 0;
		}

		static bool TryGetComponents(SemanticModel semanticModel, GenericNameSyntax name, List<ResourceManager> resourceManagers, List<Diagnostic> diagnostics, out List<Component> models)
		{
			models = new List<Component>();

			foreach (IdentifierNameSyntax comp in name.TypeArgumentList.Arguments)
			{
				var foundSymbol = semanticModel.Compilation.GetSymbolsWithName(comp.ToFullString(), SymbolFilter.Type).Single();
				if (foundSymbol is not INamedTypeSymbol typeSymbol)
					throw new Exception();

				if (!ComponentGenerator.IsValidComponent(typeSymbol))
				{
					// Only show error if struct is not valid component and not a registerd resource.
					if (!IsResource(typeSymbol, resourceManagers))
						diagnostics.Add(Diagnostic.Create(ArchTypeGeneratorDiagnostics.ArchTypeMustBeValidComponent, comp.GetLocation(), ""));

					continue;
				}	

				models.Add(new Component()
				{
					name = $"{typeSymbol.ContainingNamespace}.{comp.Identifier.Text}",
					varName = comp.Identifier.Text
				});
			}

			return models.Count > 0;
		}

		static bool TryGetResourceComponents(SemanticModel semanticModel, GenericNameSyntax name, List<ResourceManager> resourceManagers, out List<ResourceComponent> models)
		{
			models = new List<ResourceComponent>();

			foreach (IdentifierNameSyntax comp in name.TypeArgumentList.Arguments)
			{
				var foundSymbol = semanticModel.Compilation.GetSymbolsWithName(comp.ToFullString(), SymbolFilter.Type).Single();
				if (foundSymbol is not INamedTypeSymbol typeSymbol)
					throw new Exception();

				if (!TryGetResourceManager(typeSymbol, resourceManagers, out ResourceManager resourceManager))
					continue;

				models.Add(new ResourceComponent()
				{
					name = $"{resourceManager.ns}.{resourceManager.name}.{resourceManager.inType}",
					varName = comp.Identifier.Text,
					resourceManager = resourceManager
				});
			}

			return models.Count > 0;
		}

		static bool IsResource(INamedTypeSymbol comp, List<ResourceManager> resourceManagers)
		{
			return resourceManagers.Any(x => x.inType == comp.Name);
		}

		static bool TryGetResourceManager(INamedTypeSymbol comp, List<ResourceManager> resourceManagers, out ResourceManager resourceManager)
		{
			resourceManager = resourceManagers.FirstOrDefault(x => x.inType == comp.Name);
			return IsResource(comp, resourceManagers);
		}
	}

	struct ArchType : IEquatable<ArchType>
	{
		public string name;
		public EquatableArray<Component> components;
		public EquatableArray<ResourceComponent> resourceComponents;
		public EquatableArray<ResourceManager> resourceManagers;

        public ArchType()
        {
			name = "";
            components = EquatableArray<Component>.Empty;
			resourceComponents = EquatableArray<ResourceComponent>.Empty;
			resourceManagers = EquatableArray<ResourceManager>.Empty;
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("archTypeName".AsSpan(), Parameter.Create(name));
			model.Set("archTypeComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel()).Concat(resourceComponents.Select(x => x.GetModel()))));
			model.Set("archTypeResourceComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceComponents.Select(x => x.GetModel())));
			model.Set("archTypeResourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(resourceManagers.Select(x => x.GetModel())));

			return model;
		}

		public bool Equals(ArchType other)
		{
			return name.Equals(other.name)
				&& components.Equals(other.components)
				&& resourceComponents.Equals(other.resourceComponents)
				&& resourceManagers.Equals(other.resourceManagers);
		}
	}

	struct Component : IEquatable<Component>
	{
		public string name;
		public string varName;

        public Component()
        {
			name = "";
			varName = "";
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("compName".AsSpan(), Parameter.Create(name));
			model.Set("compVarName".AsSpan(), Parameter.Create(varName));
			model.Set("compType".AsSpan(), Parameter.Create("Component"));
				
			return model;
		}

		public bool Equals(Component other)
		{
			return name.Equals(other.name)
				&& varName.Equals(other.varName);
		}
	}

	struct ResourceComponent : IEquatable<ResourceComponent>
	{
		public string name;
		public string varName;
		public ResourceManager resourceManager;

        public ResourceComponent()
        {
			name = "";
			varName = "";
			resourceManager = new();
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("compName".AsSpan(), Parameter.Create(name));
			model.Set("compVarName".AsSpan(), Parameter.Create(varName));
			model.Set("compResourceManager".AsSpan(), Parameter.Create((IModel<ReturnType>)resourceManager.GetModel()));
			model.Set("compType".AsSpan(), Parameter.Create("Resource"));

			return model;
		}

		public bool Equals(ResourceComponent other)
		{
			return name.Equals(other.name)
				&& varName.Equals(other.varName)
				&& resourceManager.Equals(other.resourceManager);
		}
	}
}
