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
	static class ArchTypeGeneratorDiagnostics
	{
		public static readonly DiagnosticDescriptor ArchTypeMustBeValidComponent = new("ECS003", "Archtype can only contain valid components", "Archtype member is not valid component", "ArchTypeGenerator", DiagnosticSeverity.Error, true);
	}

	class ArchTypeGenerator : ITemplateSourceGenerator<IdentifierNameSyntax>
	{
		public Guid Id { get; } = Guid.NewGuid();

		public string Template => ResourceReader.GetResource("ArchType.tcs");

        public bool TryCreateModel(Compilation compilation, IdentifierNameSyntax node, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			var builderRoot = EcsGenerator.GetBuilderRoot(node);

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");

			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), Parameter.Create(node.GetNamespace()));
			model.Set("ecsName".AsSpan(), new Parameter<string>(EcsGenerator.GetEcsName(node)));

			var archTypeSuccess = TryGetArchTypes(compilation, archTypeStep, diagnostics, out List<ArchType> archTypes);
			model.Set("archTypes".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(archTypes.Select(x => x.GetModel())));

			return true;
		}

		public bool Filter(IdentifierNameSyntax node)
		{
			return node.Identifier.Text == "EcsBuilder";
		}

		public string GetName(IdentifierNameSyntax node)
		{
			return $"{EcsGenerator.GetEcsName(node)}_ArchType";
		}

		public static bool TryGetArchTypes(Compilation compilation, MemberAccessExpressionSyntax step, List<Diagnostic> diagnostics, out List<ArchType> models)
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

				if (!TryGetComponents(compilation, genericName, diagnostics, out List<Component> components))
					continue;

				models.Add(new ArchType()
				{
					name = nameToken,
					components = components
				});
			}

			return models.Count > 0;
		}

		static bool TryGetComponents(Compilation compilation, GenericNameSyntax name, List<Diagnostic> diagnostics, out List<Component> models)
		{
			var nodes = compilation.SyntaxTrees.SelectMany(x => x.GetRoot().DescendantNodesAndSelf());
			models = new List<Component>();

			foreach (IdentifierNameSyntax comp in name.TypeArgumentList.Arguments)
			{
				var compNode = nodes.FindNode<StructDeclarationSyntax>(x => x.Identifier.Text == comp.Identifier.Text);

				if (!ComponentGenerator.IsValidComponent(compNode))
				{
					diagnostics.Add(Diagnostic.Create(ComponentGeneratorDiagnostics.InvalidComponentMemberType, comp.GetLocation(), ""));
					continue;
				}	

				models.Add(new Component()
				{
					name = $"{compNode.GetNamespace()}.{comp.Identifier.Text}",
					varName = comp.Identifier.Text
				});
			}

			return models.Count > 0;
		}
	}

	struct ArchType
	{
		public string name;
		public List<Component> components;

		public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("archTypeName".AsSpan(), Parameter.Create(name));
			model.Set("archTypeComponents".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(components.Select(x => x.GetModel())));

			return model;
		}
	}

	struct Component
	{
		public string name;
		public string varName;

		public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("compName".AsSpan(), Parameter.Create(name));
			model.Set("compVarName".AsSpan(), Parameter.Create(varName));

			return model;
		}
	}
}
