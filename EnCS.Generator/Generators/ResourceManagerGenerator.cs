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
	struct ResourceManagerGeneratorData : IEquatable<ResourceManagerGeneratorData>
	{
		public INamedTypeSymbol node;
		public Location location;

		public EquatableArray<ResourceManager> resourceManagers = EquatableArray<ResourceManager>.Empty;

		public ResourceManagerGeneratorData(INamedTypeSymbol node, Location location, EquatableArray<ResourceManager> resourceManagers)
		{
			this.node = node;
			this.resourceManagers = resourceManagers;
		}

		public bool Equals(ResourceManagerGeneratorData other)
		{
			return resourceManagers.Equals(other.resourceManagers);
		}
	}

	class ResourceManagerGenerator : ITemplateSourceGenerator<ClassDeclarationSyntax, ResourceManagerGeneratorData>
	{
		public string Template => ResourceReader.GetResource("ResourceManager.tcs");

		public bool TryCreateModel(ResourceManagerGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			model = new Model<ReturnType>();
			model.Set("namespace".AsSpan(), new Parameter<string>(data.node.ContainingNamespace.ToString()));
			model.Set("name".AsSpan(), new Parameter<string>(data.node.Name));

			model.Set("resourceManagers".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.resourceManagers.Select(x => x.GetModel())));

			return true;
		}

		public ResourceManagerGeneratorData? Filter(ClassDeclarationSyntax node, SemanticModel semanticModel)
		{
			if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol typeSymbol)
				return null;

			if (!TryGetResourceManagers(typeSymbol, out var resourceManagers))
				return null;

			return new ResourceManagerGeneratorData(typeSymbol, node.GetLocation(), new(resourceManagers.ToArray()));
		}

		public string GetName(ResourceManagerGeneratorData data)
			=> data.node.Name;

		public Location GetLocation(ResourceManagerGeneratorData data)
			=> data.location;

		public static bool TryGetResourceManagers(INamedTypeSymbol resourceManager, out List<ResourceManager> resourceManagers)
		{
			resourceManagers = new List<ResourceManager>();

			foreach (var type in resourceManager.Interfaces)
			{
				if (type.Name != "IResourceManager")
					continue;

				var typeArguments = type.TypeArguments;

				// Dont throw if not filled in yet.
				if (typeArguments.Length == 0)
					continue;

				if (typeArguments.Length > 2)
					throw new Exception("Invalid number of type arguments for resource managers, must be 1 or 2");

				string inType = typeArguments[0].Name;
				string outType = typeArguments.Length == 2 ? typeArguments[1].Name : inType;

				resourceManagers.Add(new ResourceManager()
				{
					name = resourceManager.Name,
					ns = resourceManager.ContainingNamespace.ToString(),
					inType = inType,
					outType = outType,
					typeNs = typeArguments[0].ContainingNamespace.ToString()
				});
			}

			return resourceManagers.Count > 0;
		}
	}

	struct ResourceManager : IEquatable<ResourceManager>
	{
		public string name;
		public string ns;
		public string inType;
		public string outType;
		public string typeNs;

        public ResourceManager()
        {
			name = "";
			ns = "";
			inType = "";
			outType = "";
			typeNs = "";
        }

        public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("resourceManagerName".AsSpan(), Parameter.Create(name));
			model.Set("resourceManagerNamespace".AsSpan(), Parameter.Create(ns));
			model.Set("resourceManagerInType".AsSpan(), Parameter.Create(inType));
			model.Set("resourceManagerOutType".AsSpan(), Parameter.Create(outType));
			model.Set("resourceManagerTypeNamespace".AsSpan(), Parameter.Create(typeNs));

			return model;
		}

		public bool Equals(ResourceManager other)
		{
			return name.Equals(other.name)
				&& ns.Equals(other.ns)
				&& inType.Equals(other.inType)
				&& outType.Equals(other.outType)
				&& typeNs.Equals(other.typeNs);
		}
	}
}
