using System.Runtime.CompilerServices;

namespace EnCS.Generator.Tests
{
	public static class ModuleInitializer
	{
		[ModuleInitializer]
		public static void Init()
		{
			VerifySourceGenerators.Initialize();
		}
	}
}