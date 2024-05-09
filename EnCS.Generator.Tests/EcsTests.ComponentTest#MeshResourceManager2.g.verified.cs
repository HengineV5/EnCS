//HintName: MeshResourceManager2.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using EnCS.Attributes;

// TODO: Refactor when template import is supported in language
namespace Project.Primitives
{
	public partial class MeshResourceManager2
	{
		[Component]
		public struct TestResource : IComponent<TestResource, TestResource.Vectorized, TestResource.Array>
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
				public ref Project.Primitives.TestResourceId TestResource => ref resourceManager.Get(id);

				ref uint id;
				MeshResourceManager2 resourceManager;

				public Ref(ref uint id, MeshResourceManager2 resourceManager)
				{
					this.id = ref id;
					this.resourceManager = resourceManager;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Set(in Project.Primitives.TestResource data)
				{
					this.id = resourceManager.Store(data);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArray(ref Array array, int idx, MeshResourceManager2 resourceManager)
				{
					return new Ref(ref array.id[idx], resourceManager);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, TestResource, Vectorized, Array>
			{
				return ref TArch.GetVec(ref arch);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, TestResource, Vectorized, Array>
			{
				return ref TArch.GetSingle(ref arch);
			}
		}
	}
}