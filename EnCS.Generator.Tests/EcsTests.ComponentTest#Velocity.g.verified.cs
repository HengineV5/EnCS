//HintName: Velocity.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public ref partial struct Velocity : IComponent<Velocity, Velocity.Vectorized, Velocity.Array>
	{
		public Velocity()
		{
			throw new NotImplementedException("Velocity should be created with Comp struct, not directly.");
		}

		public Velocity(ref int x, ref int y, ref int z)
		{
			this.x = ref x;
			this.y = ref y;
			this.z = ref z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Velocity FromArray(ref Array array, int idx)
		{
			return new Velocity(ref array.x[idx], ref array.y[idx], ref array.z[idx]);
		}

		public struct Vectorized
		{
			public Vector256<int> x;
			public Vector256<int> y;
			public Vector256<int> z;
		}

		public struct Comp
		{
			public int x;
			public int y;
			public int z;

			public Comp(int x, int y, int z)
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

			public FixedArray8<int> x;
			public FixedArray8<int> y;
			public FixedArray8<int> z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Velocity, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Velocity, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}

		public static implicit operator Velocity(Comp c) => new(ref Unsafe.AsRef(c.x), ref Unsafe.AsRef(c.y), ref Unsafe.AsRef(c.z));
	}
}