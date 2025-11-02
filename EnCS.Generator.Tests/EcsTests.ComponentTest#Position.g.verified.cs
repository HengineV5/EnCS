//HintName: Position.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public ref partial struct Position
	{
		public Position()
		{
			throw new NotImplementedException("Position should be created with Comp struct, not directly.");
		}

		public Position(ref float x, ref float y, ref float z)
		{
			this.x = ref x;
			this.y = ref y;
			this.z = ref z;
		}

		public void Set(Comp c)
		{
			this.x = c.x;
			this.y = c.y;
			this.z = c.z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Position FromArray(ref Array array, int idx)
		{
			return new Position(ref array.x[idx], ref array.y[idx], ref array.z[idx]);
		}

		public struct Vectorized
		{
			public Vector256<float> x;
			public Vector256<float> y;
			public Vector256<float> z;
		}

		public struct Comp
		{
			public float x;
			public float y;
			public float z;

			public Comp(float x, float y, float z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedBuffer8<float> x;
			public FixedBuffer8<float> y;
			public FixedBuffer8<float> z;
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