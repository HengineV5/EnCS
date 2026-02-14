//HintName: MeshResourceManager.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EnCS;
using EnCS.Attributes;
using UtilLib.Memory;

// TODO: Refactor when template import is supported in language
namespace Runner
{
	public partial class MeshResourceManager
	{
		[Component]
		public ref struct TestResource
		{
			ref uint id;
			MeshResourceManager resourceManager;

			public TestResource(ref uint id)
			{
				this.id = ref id;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ref Runner.TestResourceId Get()
			{
				return ref resourceManager.Get(id);
			}

			public TestResource SetResourceManager(MeshResourceManager resourceManager)
			{
				this.resourceManager = resourceManager;
				return this;
			}

			public ref struct Vectorized
			{
				public ref Vector256<uint> id;
			}

			public struct Memory
			{
				public Memory<uint> id;

				public Memory(int length)
				{
					this.id = new uint[length];
				}

				public TestResource.Vectorized GetVec(int idx)
				{
					idx /= 8;
					idx *= 8;

					return new TestResource.Vectorized
					{
						id = ref Unsafe.As<uint, Vector256<uint>>(ref this.id.Span[idx]),
					};
				}

				public TestResource GetSingle(int idx)
				{
					return new TestResource
					(
						ref this.id.Span[idx]
					);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Set(in Runner.TestResource data)
			{
				this.id = resourceManager.Store(data);
			}
		}
	}
}