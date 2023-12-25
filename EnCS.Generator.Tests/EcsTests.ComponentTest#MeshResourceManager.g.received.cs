//HintName: MeshResourceManager.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using EnCS.Attributes;

// TODO: Refactor when template import is supported in language
namespace Project.Primitives
{
	public partial class MeshResourceManager
	{
		[Component]
		public struct Mesh : IComponent<Mesh, Mesh.Vectorized, Mesh.Array>
		{
			public uint id;

			public struct Vectorized
			{
				public Vector256<uint> id;
			}

			[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
			public struct Array
			{
				public const int Size = 8;

				public FixedArray8<uint> id;
			}

			public ref struct Ref
			{
				public ref Project.Primitives.Mesh Mesh => ref resourceManager.Get(id);

				ref uint id;
				MeshResourceManager resourceManager;

				public Ref(ref uint id, MeshResourceManager resourceManager)
				{
					this.id = ref id;
					this.resourceManager = resourceManager;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Set(in Project.Primitives.Mesh data)
				{
					this.id = resourceManager.Store(data);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArray(ref Array array, int idx, MeshResourceManager resourceManager)
				{
					return new Ref(ref array.id[idx], resourceManager);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Mesh, Vectorized, Array>
			{
				return ref TArch.GetVec(ref arch);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Mesh, Vectorized, Array>
			{
				return ref TArch.GetSingle(ref arch);
			}
		}
	}
}