using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	class ArchTypeGenerator : ITemplateSourceGenerator<IdentifierNameSyntax>
	{
		public Guid Id { get; } = Guid.NewGuid();

		public string Template => ResourceReader.GetResource("ArchType.tcs");

        public Model<ReturnType> CreateModel(Compilation compilation, IdentifierNameSyntax node)
		{
			var builderRoot = EcsGenerator.GetBuilderRoot(node);

			var builderSteps = builderRoot.DescendantNodes()
				.Where(x => x is MemberAccessExpressionSyntax)
				.Cast<MemberAccessExpressionSyntax>();

			var archTypeStep = builderSteps.First(x => x.Name.Identifier.Text == "ArchType");

			var model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), Parameter.Create(node.GetNamespace()));
			model.Set("ecsName".AsSpan(), new Parameter<string>(EcsGenerator.GetEcsName(node)));

			var archTypes = GetArchTypes(compilation, archTypeStep);
			model.Set("archTypes".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(archTypes.Select(x => x.GetModel())));

			return model;
		}

		public bool Filter(IdentifierNameSyntax node)
		{
			return node.Identifier.Text == "EcsBuilder";
		}

		public string GetName(IdentifierNameSyntax node)
		{
			return $"{EcsGenerator.GetEcsName(node)}_ArchType";
		}

		public static List<ArchType> GetArchTypes(Compilation compilation, MemberAccessExpressionSyntax step)
		{
			var models = new List<ArchType>();

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

				models.Add(new ArchType()
				{
					name = nameToken,
					components = GetComponents(compilation, genericName)
				});
			}

			return models;
		}

		static List<Component> GetComponents(Compilation compilation, GenericNameSyntax name)
		{
			var nodes = compilation.SyntaxTrees.SelectMany(x => x.GetRoot().DescendantNodesAndSelf());
			var models = new List<Component>();

			foreach (IdentifierNameSyntax comp in name.TypeArgumentList.Arguments)
			{
				var compNode = nodes.FindNode<StructDeclarationSyntax>(x => x.Identifier.Text == comp.Identifier.Text);

				models.Add(new Component()
				{
					name = $"{compNode.GetNamespace()}.{comp.Identifier.Text}",
					varName = comp.Identifier.Text
				});
			}

			return models;
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
