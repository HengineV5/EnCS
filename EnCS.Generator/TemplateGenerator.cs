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

			new TemplateGeneratorBuilder()
				.WithLogging(context)
				.WithAttributeGenerator("EnCS.Attributes.ComponentAttribute", context, compGenerator)
				.WithGenerator(context, archTypeGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute", context, systemGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute<T1, T2>", context, systemGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute<T1,T2>", context, systemGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute<T,T>", context, systemGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute<,>", context, systemGenerator)
				.WithAttributeGenerator("EnCS.Attributes.SystemAttribute<>", context, systemGenerator)
				.WithGenerator(context, worldGenerator)
				.WithGenerator(context, ecsGenerator)
				.WithAttributeGenerator("EnCS.Attributes.ResourceManagerAttribute", context, resourceManagerGenerator)
				.WithInfoFile(context);
		}
	}
}
