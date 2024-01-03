using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices
{
	//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	//public sealed class ModuleInitializerAttribute : Attribute { }
}

namespace EnCS.Generator.Tests
{
	public static class TestModuleInitializer
	{
		[ModuleInitializer]
		public static void Init()
		{
			VerifySourceGenerators.Initialize();
		}
	}
}