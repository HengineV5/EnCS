//HintName: Scale.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial struct Scale : IComponent<Scale, Scale.Vectorized, Scale.Array>
	{
		public struct Vectorized
		{
			public Vector256<float> x;
			public Vector256<float> y;
			public Vector256<float> z;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<float> x;
			public FixedArray8<float> y;
			public FixedArray8<float> z;
		}

		public ref struct Ref
		{
			public ref float x;
			public ref float y;
			public ref float z;
			
			public Ref(ref float x, ref float y, ref float z)
			{
				this.x = ref x;
				this.y = ref y;
				this.z = ref z;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Set(in Scale data)
			{
				this.x = data.x;
				this.y = data.y;
				this.z = data.z;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Ref FromArray(ref Array array, int idx)
			{
				return new Ref(ref array.x[idx], ref array.y[idx], ref array.z[idx]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Scale, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Scale, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}
}