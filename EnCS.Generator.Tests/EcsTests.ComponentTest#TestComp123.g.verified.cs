//HintName: TestComp123.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public ref partial struct TestComp123
	{
		public TestComp123()
		{
			throw new NotImplementedException("TestComp123 should be created with Comp struct, not directly.");
		}

		public TestComp123(ref int tag)
		{
			this.tag = ref tag;
		}

		public void Set(Comp c)
		{
			this.tag = c.tag;
		}

		public ref struct Vectorized
		{
			public ref Vector256<int> tag;
		}

		public struct Comp
		{
			public int tag;

			public Comp(int tag)
			{
				this.tag = tag;
			}
		}

		public struct Memory
		{
			public Memory<int> tag;

			public Memory(int length)
			{
				this.tag = new int[length];
			}

			public TestComp123.Vectorized GetVec(int idx)
			{
				idx /= 8;

				return new TestComp123.Vectorized
				{
					 tag = ref MemoryMarshal.AsRef<Vector256<int>>(this.tag.Span.Slice(idx, 8)),//
				};
			}

			public TestComp123 GetSingle(int idx)
			{
				return new TestComp123(
					ref this.tag.Span[idx],
				);
			}

			public TestComp123.Span AsSpan()
			{
				return new TestComp123.Span(in this);
			}
		}

		public ref struct Span
		{
			public Span<int> tag;

			public Span(ref readonly Memory<TestComp123.Memory> memory)
			{
				this.tag = memory.Span.tag;
			}
		}
	}
}