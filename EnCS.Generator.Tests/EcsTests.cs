using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace EnCS.Generator.Tests
{
	public static class TestHelper
	{
		public static Task Verify(params string[] source)
		{
			List<SyntaxTree> trees = new List<SyntaxTree>();
			foreach (var item in source)
			{
				trees.Add(CSharpSyntaxTree.ParseText(item));
			}

			var r = new MetadataReference[1];
			r[0] = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

			CSharpCompilation compilation = CSharpCompilation.Create("Tests", trees, references: r);
			var diag = compilation.GetDiagnostics();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(new TemplateGenerator());
			driver = driver.RunGenerators(compilation);
			
			return Verifier.Verify(driver);
		}
	}

	[UsesVerify]
	public class EcsTests
	{
		[Fact]
		public Task ComponentTest()
		{
			string fixedArraySource = @"
using System.Runtime.CompilerServices;

namespace EnCS
{
	[InlineArray(2)]
	public struct FixedArray2<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(4)]
	public struct FixedArray4<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(8)]
	public struct FixedArray8<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(16)]
	public struct FixedArray16<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(32)]
	public struct FixedArray32<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(64)]
	public struct FixedArray64<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}
}

";

			string interfaceSource = @"
namespace EnCS
{
	public interface IResourceManager<TResource>
	{
		public uint Store(in TResource resource);

		public ref TResource Get(uint id);
	}

	public interface IResourceManager<TIn, TOut>
	{
		public uint Store(in TIn resource);

		public ref TOut Get(uint id);
	}
}
";

			string attribSource = @"
using System;

namespace EnCS.Attributes
{
	public class ComponentAttribute : System.Attribute
	{

	}

	public class ArchTypeAttribute : System.Attribute
	{

	}

	public class ResourceManagerAttribute : System.Attribute
	{

	}

	public class SystemAttribute : System.Attribute
	{

	}

	public class SystemContextAttribute<T1> : System.Attribute where T1 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2> : System.Attribute where T1 : unmanaged where T2 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3> : System.Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3, T4> : System.Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
	{

	}

	public class SystemLayerAttribute : System.Attribute
	{
		public SystemLayerAttribute(int layer)
		{
            
		}

		public SystemLayerAttribute(int layer, int chunk)
		{

		}
	}

	public class SystemUpdateAttribute : System.Attribute
	{

	}

	public class SystemPreLoopAttribute : System.Attribute
	{

	}

	public class SystemPostLoopAttribute : System.Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class UsingResourceAttribute<T> : System.Attribute
	{

	}
}
";

			string source = @"
using EnCS;
using EnCS.Attributes;
using namespace Project.Primitives;

public enum CompEnum
{
	Val1,
	Val2
}

public struct TestContext
{
	public float data;
}

public struct TestContext2
{
	public float data;
}

public struct Mesh
{
	public string name;
}

public struct MeshId
{
	public uint id;
}

public struct Kaki
{
	public string name;
}

public struct KakiId
{
	public uint id;
}

[ResourceManager]
public partial class TestResourceManager : IResourceManager<Kaki, KakiId>
{

}

[ResourceManager]
public partial class MeshResourceManager : IResourceManager<Mesh, MeshId>
{

}

public struct TestResource
{
	public string name;
}

public struct TestResourceId
{
	public uint id;
}

[ResourceManager]
public partial class MeshResourceManager2 : IResourceManager<TestResource, TestResourceId>
{
	Memory<Runner.TestResource> resource = new Runner.TestResource[8];
	Memory<Runner.TestResourceId> resourceids = new Runner.TestResourceId[8];

    public MeshResourceManager()
    {
		resource.Span[0] = new() { name = ""yay"" };
		resourceids.Span[0] = new() { id = 0 };
		resource.Span[1] = new() { name = ""nay"" };
		resourceids.Span[1] = new() { id = 1 };
    }

    public ref Runner.TestResourceId Get(uint id)
	{
		return ref resourceids.Span[(int)id];
	}

	public uint Store(in Runner.TestResource resource)
	{
		return resource.name == ""yay"" ? 0u : 1u;
	}
}

[Component]
public partial struct InvalidComp
{
	public string x;
}

[ComponentAttribute]
public partial struct Position
{
	public MeshId mesh;
	public int x;
	public int y;
	public FixedArray4<int> z;
	public CompEnum e;
}

[ComponentAttribute]
public partial struct Velocity
{
	public float x;
	public double y;
	public decimal z;
}

[ComponentAttribute]
public partial struct Scale
{
	public float x;
	public float y;
	public float z;

	public static implicit operator Scale(Vector3 v) => new Scale(v.X, v.Y, v.Z);
	public static implicit operator Scale(Vector2 v) => new Scale(v.X, v.Y, 0);
}

[SystemAttribute]
[SystemContext<TestContext, TestContext2>]
[UsingResource<TestResourceManager>]
[UsingResource<MeshResourceManager>]
public partial class ResourceSystem
{
	[SystemPreLoop, SystemLayer(0)]
	public void PreLoop1(ref TestContext context)
	{

	}

	[SystemUpdate, SystemLayer(1)]
	public void Update(ref Scale.Vectorized scale)
	{
    }

	[SystemUpdate, SystemLayer(1)]
	public void Update(Scale.Ref scale)
	{
    }

	[SystemPostLoop, SystemLayer(0)]
	public void PostLoop1()
	{

	}

	[SystemPreLoop, SystemLayer(1)]
	public void PreLoop()
	{

	}

	[SystemUpdate, SystemLayer(0, 16)]
	public void Update(ref TestContext context, Position.Ref position, Velocity.Ref velocity, ref MeshId mesh, ref KakiId kaki)
	{
    }

	[SystemUpdate, SystemLayer(0, 16)]
	public void Update(ref TestContext context, ref Position.Vectorized position, ref Velocity.Vectorized velocity)
	{
	}

	[SystemPostLoop, SystemLayer(1)]
	public void PostLoop()
	{

	}
}

[SystemAttribute]
public partial class TestSystem
{
	[SystemUpdate]
	public void Update(Scale.Ref scale)
	{
	}

	[SystemUpdate]
	public void Update(Position.Ref position, Velocity.Ref velocity)
	{
	}
}

[SystemAttribute]
public partial class PositionSystem
{
	[SystemUpdate]
	public void Update(Position.Ref position)
	{
    }

	[SystemUpdate]
	public void Update(ref Position.Vectorized position)
	{
	}

	[SystemUpdate]
	public void UpdateAfter(Position.Ref position)
	{
    }
}

[SystemAttribute]
public partial class VelocitySystem
{
	[SystemUpdate]
	public void Update(Position.Ref position, Velocity.Ref velocity)
	{
    }

	[SystemUpdate]
	public void Update(ref Position.Vectorized position,ref Velocity.Vectorized velocity)
	{
	}
}

public partial struct Ecs
{

}

namespace Test
{
	static void Main()
	{
		new EcsBuilder()
			.ArchType(x =>
			{
				x.ArchType<InvalidComp>(""IsWrong"");
				x.ArchType<Position, Velocity, Mesh, Kaki>(""Wall"");
				x.ArchType<Position, Scale>(""Tile"");
			})
			.System(x =>
			{
				x.System<PositionSystem>();
				x.System<VelocitySystem>();
				x.System<ResourceSystem>();
			})
			.World(x =>
			{
				x.World<Ecs.Wall, Ecs.Tile>(""Main"");
				x.World<Ecs.Wall>(""World2"");
				x.World<Ecs.Wall>();
			})
			.Resource(x =>
			{
				x.ResourceManager<MeshResourceManager>();
				x.ResourceManager<TestResourceManager>();
			})
			.Build<Ecs>();

		var ecs = new EcsBuilder()
			.ArchType(x =>
			{
				x.ArchType<Position>(""Tile"");
			})
			.System(x =>
			{
				x.System<PositionSystem>();
			})
			.World(x =>
			{
				x.World<Ecs.Tile>(""Main"");
			})
			.Resource(x =>
			{
			})
			.Build<Ecs2>();
	}
}
";


			var source3 = File.ReadAllText("Files/TestFile.txt");

			return TestHelper.Verify(source, attribSource, interfaceSource, fixedArraySource);
		}
	}
}