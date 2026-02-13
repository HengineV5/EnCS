//HintName: MeshResourceManager.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
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

			public TestResource(ref uint id)
			{
				this.id = ref id;
			}

			public Runner.TestResourceId Get(MeshResourceManager resourceManager)
			{
				return resourceManager.Get(id);
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

					return new TestResource.Vectorized
					{
						id = ref MemoryMarshal.AsRef<Vector256<uint>>(this.id.Span.Slice(idx, 8)),
					};
				}

				public TestResource GetSingle(int idx)
				{
					return new TestResource(
						ref this.id.Span[idx]
					);
				}

				public TestResource.Span AsSpan()
				{
					return new TestResource.Span(in this);
				}
			}

			public ref struct Span
			{
				public Span<uint> id;

				public Span(ref readonly Memory<TestResource.Memory> memory)
				{
					this.id = memory.Span.id;
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