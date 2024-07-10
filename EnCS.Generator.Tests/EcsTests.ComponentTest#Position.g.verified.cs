//HintName: Position.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial struct Position : IComponent<Position, Position.Vectorized, Position.Array>
	{
		public struct Vectorized
		{
			public Vector256<Project.Primitives.MeshId> mesh;
			public Vector256<int> x;
			public Vector256<int> y;
			public FixedArray2<Vector512<EnCS.FixedArray4<int>>> z;
			public Vector256<Project.Primitives.CompEnum> e;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<Project.Primitives.MeshId> mesh;
			public FixedArray8<int> x;
			public FixedArray8<int> y;
			public FixedArray8<EnCS.FixedArray4<int>> z;
			public FixedArray8<Project.Primitives.CompEnum> e;
		}

		public ref struct Ref
		{
			public ref Project.Primitives.MeshId mesh;
			public ref int x;
			public ref int y;
			public ref EnCS.FixedArray4<int> z;
			public ref Project.Primitives.CompEnum e;
			
			public Ref(ref Project.Primitives.MeshId mesh, ref int x, ref int y, ref EnCS.FixedArray4<int> z, ref Project.Primitives.CompEnum e)
			{
				this.mesh = ref mesh;
				this.x = ref x;
				this.y = ref y;
				this.z = ref z;
				this.e = ref e;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Set(in Position data)
			{
				this.mesh = data.mesh;
				this.x = data.x;
				this.y = data.y;
				this.z = data.z;
				this.e = data.e;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Ref FromArray(ref Array array, int idx)
			{
				return new Ref(ref array.mesh[idx], ref array.x[idx], ref array.y[idx], ref array.z[idx], ref array.e[idx]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Position, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Position, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}
}