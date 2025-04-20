//HintName: TestComp123.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public ref partial struct TestComp123 : IComponent<TestComp123, TestComp123.Vectorized, TestComp123.Array>
	{
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

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<int> tag;
		}

		/*
		public ref struct Ref
		{
			public ref int tag;
			
			public Ref(ref int tag)
			{
				this.tag = ref tag;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Set(in TestComp123 data)
			{
				this.tag = data.tag;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Ref FromArray(ref Array array, int idx)
			{
				return new Ref(ref array.tag[idx]);
			}
		}
		*/

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
	}
}