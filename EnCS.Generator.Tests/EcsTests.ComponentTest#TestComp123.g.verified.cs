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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TestComp123 FromArray(ref Array array, int idx)
		{
			return new TestComp123(ref array.tag[idx]);
		}

		public struct Vectorized
		{
			public Vector256<int> tag;
		}

		public struct Comp
		{
			public int tag;

			public Comp(int tag)
			{
				this.tag = tag;
			}
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedBuffer8<int> tag;
		}

		public struct Memory
		{
			public Memory<int> tag;

			public Memory(int length)
			{
				this.tag = new int[length];
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


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}
}