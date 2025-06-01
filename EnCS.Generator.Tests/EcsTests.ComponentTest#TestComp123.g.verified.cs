//HintName: TestComp123.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public ref partial struct TestComp123 : IComponent<TestComp123, TestComp123.Vectorized, TestComp123.Array>
	{
		public TestComp123()
		{
			throw new NotImplementedException("TestComp123 should be created with Comp struct, not directly.");
		}

		public TestComp123(ref int tag)
		{
			this.tag = ref tag;
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

			public FixedArray8<int> tag;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, TestComp123, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, TestComp123, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}

		public static implicit operator TestComp123(Comp c) => new(ref Unsafe.AsRef(c.tag));
	}
}