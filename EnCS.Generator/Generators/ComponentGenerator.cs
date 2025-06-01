using LightParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using TemplateGenerator;

namespace EnCS.Generator
{
	struct ComponentGeneratorData : IEquatable<ComponentGeneratorData>, ITemplateData
	{
		public INamedTypeSymbol node;
		public Location location;

		public EquatableArray<ComponentMember> members = EquatableArray<ComponentMember>.Empty;

		public ComponentGeneratorData(INamedTypeSymbol node, Location location, EquatableArray<ComponentMember> members)
		{
			this.node = node;
			this.location = location;
			this.members = members;
		}

		public bool Equals(ComponentGeneratorData other)
		{
			return members.Equals(other.members);
		}

		public string GetIdentifier()
			=> $"Component Generator ({node.Name}) ({location})";
	}

	class ComponentGenerator : ITemplateSourceGenerator<StructDeclarationSyntax, ComponentGeneratorData>
	{
		const int MAX_SIMD_BUFFER_BITS = 512;
		const int ARRAY_ELEMENTS = 8;
		const int BITS_PER_BYTE = 8;

		public string Template => ResourceReader.GetResource("Component.tcs");

		public bool TryCreateModel(ComponentGeneratorData data, out Model<ReturnType> model, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			model = new Model<ReturnType>();

			model.Set("namespace".AsSpan(), new Parameter<string>(data.node.ContainingNamespace.ToString()));
			model.Set("compName".AsSpan(), new Parameter<string>(data.node.Name));
			model.Set("arraySize".AsSpan(), new Parameter<float>(ARRAY_ELEMENTS));

			model.Set("members".AsSpan(), Parameter.CreateEnum<IModel<ReturnType>>(data.members.Select(x => x.GetModel())));

			return true;
		}

		public bool TryGetData(StructDeclarationSyntax node, SemanticModel semanticModel, out ComponentGeneratorData data, out List<Diagnostic> diagnostics)
		{
			diagnostics = new List<Diagnostic>();
			Unsafe.SkipInit(out data);

			if (semanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol typeSymbol)
				return false;

			var isPartial = node.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
			if (!isPartial)
			{
				diagnostics.Add(Diagnostic.Create(ComponentGeneratorDiagnostics.ComponentMustBePartial, node.GetLocation(), node.ToFullString()));
				return false;
			}

			if (!TryGetMembers(typeSymbol, diagnostics, out var members))
				return false;

			data = new ComponentGeneratorData(typeSymbol, node.GetLocation(), new(members.ToArray()));
			return true;
		}

		public string GetName(ComponentGeneratorData data)
			=> data.node.Name;

		public Location GetLocation(ComponentGeneratorData data)
			=> data.location;

		static bool TryGetMembers(INamedTypeSymbol comp, List<Diagnostic> diagnostics, out List<ComponentMember> members)
		{
			members = new List<ComponentMember>();

			bool hasProperties = false;
			foreach (var member in comp.GetMembers())
			{
				if (member is IPropertySymbol)
					hasProperties = true;

				if (member is not IFieldSymbol field)
					continue;

				if (!TryGetTypeName(field.Type, out string typeName))
					continue;

				if (!TryGetTypeSize(field.Type, out int size))
				{
					diagnostics.Add(Diagnostic.Create(ComponentGeneratorDiagnostics.InvalidComponentMemberType, comp.Locations.FirstOrDefault(), typeName));
					return false;
				}

				int bitsPerVector = Math.Min(size * BITS_PER_BYTE * ARRAY_ELEMENTS, MAX_SIMD_BUFFER_BITS);
				int vectorArraySize = (size * BITS_PER_BYTE * ARRAY_ELEMENTS) / MAX_SIMD_BUFFER_BITS;

				members.Add(new ComponentMember()
				{
					name = member.Name,
					type = ClassToNative(typeName),
					bits = bitsPerVector,
					arraySize = vectorArraySize
				});
			}

			return !hasProperties && members.Count > 0;
		}

		static bool TryGetTypeName(ITypeSymbol type, out string name)
		{
			name = type.ToString();
			return true;
		}

		static bool TryGetTypeSize(ITypeSymbol type, out int size)
		{
			size = 0;
			switch (type.TypeKind)
			{
				case TypeKind.Enum:
					{
						if (type is not INamedTypeSymbol namedType)
							return false;

						return TryGetTypeSize(namedType.EnumUnderlyingType, out size);
					}
				case TypeKind.Struct:
					{
						if (TryGetSize(type.Name, out size))
							return true;

						if (TryHandleInlineArray(type, out size))
							return true;

						if (!type.IsUnmanagedType)
							return false;

						foreach (var member in type.GetMembers())
						{
							if (member is not IFieldSymbol fieldSymbol)
								continue;

							if (!TryGetTypeSize(fieldSymbol.Type, out int typeSize))
								return false;

							size += typeSize;
						}

						return size != 0;
					}
				default:
					return false;
			}
		}

		static bool TryHandleInlineArray(ITypeSymbol type, out int size)
		{
			size = 0;

			var attributes = type.GetAttributes();
			foreach (var attribute in attributes)
			{
				if (attribute.AttributeClass.Name != "InlineArrayAttribute")
					continue;

				if (type is not INamedTypeSymbol namedType)
					return false;

				if (namedType.TypeArguments.Length != 1)
					return false;

				if (!TryGetTypeSize(namedType.TypeArguments[0], out size))
					return false;

				size *= int.Parse(attribute.ConstructorArguments[0].Value.ToString());
				return true;
			}

			return false;
		}

		public static bool IsValidComponent(ITypeSymbol node)
		{
			if (node.Name == "Ref" || node.Name == "Vectorized") // TODO: Better component detection
				return true;

			if (!node.GetAttributes().Any(x => x.AttributeClass.Name == "ComponentAttribute" || x.AttributeClass.Name == "Component"))
				return false;

			foreach (var member in node.GetMembers())
			{
				if (member is IPropertySymbol)
					return false;

				if (member is not IFieldSymbol field)
					continue;

				if (!TryGetTypeName(field.Type, out string typeName))
					return false;

				if (!TryGetTypeSize(field.Type, out int size))
					return false;
			}

			return true;
		}

		static bool TryGetSize(string numericTypeName, out int size)
		{
			switch (numericTypeName)
			{
				case "sbyte":
				case "Byte":
				case "Char":
					size = 1;
					return true;
				case "Int16":
				case "UInt16":
					size = 2;
					return true;
				case "Int32":
				case "UInt32":
					size = 4;
					return true;
				case "Int64":
				case "UIin64":
					size = 8;
					return true;
				case "Single":
					size = 4;
					return true;
				case "Double":
					size = 8;
					return true;
				case "Decimal":
					size = 16;
					return true;
				default:
					size = 0;
					return false;
			}
		}

		static bool IsFixedNumericType(string name)
		{
			switch (name)
			{
				case "sbyte":
				case "byte":
				case "char":
				case "short":
				case "ushort":
				case "int":
				case "uint":
				case "long":
				case "ulong":
				case "float":
				case "double":
				case "decimal":
					return true;
				default:
					return false;

			}
		}

		static string ClassToNative(string name)
		{
			switch (name)
			{
				case "Char":
					return "char";
				case "Byte":
					return "byte";
				case "Int16":
					return "short";
				case "UInt16":
					return "ushort";
				case "Int32":
					return "int";
				case "UInt32":
					return "uint";
				case "Int64":
					return "long";
				case "UInt64":
					return "ulong";
				case "Single":
					return "float";
				case "Double":
					return "double";
				case "Decimal":
					return "decimal";
				default:
					return name;
			}
		}
	}

	struct ComponentMember : IEquatable<ComponentMember>
	{
		public string name;
		public string type;
		public int bits;
		public int arraySize;

		public ComponentMember()
		{
			name = "";
			type = "";
			bits = 0;
			arraySize = 0;
		}

		public Model<ReturnType> GetModel()
		{
			var model = new Model<ReturnType>();

			model.Set("name".AsSpan(), Parameter.Create(name));
			model.Set("type".AsSpan(), Parameter.Create(type));
			model.Set("bits".AsSpan(), Parameter.Create<float>(bits));
			model.Set("arraySize".AsSpan(), Parameter.Create<float>(arraySize));

			return model;
		}

		public bool Equals(ComponentMember other)
		{
			return name.Equals(other.name)
				&& type.Equals(other.type)
				&& bits.Equals(other.bits)
				&& arraySize.Equals(other.arraySize);
		}
	}
}