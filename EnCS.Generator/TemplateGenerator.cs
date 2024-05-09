using Microsoft.CodeAnalysis;
using TemplateGenerator;

namespace EnCS.Generator
{
	[Generator]
	public class TemplateGenerator : IIncrementalGenerator
	{
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			var ecsGenerator = new EcsGenerator();
			var compGenerator = new ComponentGenerator();
			var archTypeGenerator = new ArchTypeGenerator();
			var systemGenerator = new SystemGenerator();
			var worldGenerator = new WorldGenerator();
			var resourceManagerGenerator = new ResourceManagerGenerator();

			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.ComponentAttribute", context, compGenerator);
			TemplateGeneratorHelpers.RegisterTemplateGenerator(context, archTypeGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute<T1, T2>", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute<T1,T2>", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute<T,T>", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute<,>", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.SystemAttribute<>", context, systemGenerator);
			TemplateGeneratorHelpers.RegisterTemplateGenerator(context, worldGenerator);
			TemplateGeneratorHelpers.RegisterTemplateGenerator(context, ecsGenerator);
			TemplateGeneratorHelpers.RegisterAttributeTemplateGenerator("EnCS.Attributes.ResourceManagerAttribute", context, resourceManagerGenerator);
		}
	}
}
