//HintName: MeshResourceManager.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using EnCS.Attributes;

// TODO: Refactor when template import is supported in language
namespace Runner
{
	public partial class MeshResourceManager
	{
		[Component]
		public ref struct TestResource : IComponent<TestResource, TestResource.Vectorized, TestResource.Array>
		{
			public ref Runner.TestResourceId TestResourceProp => ref resourceManager.Get(id);

			ref uint id;
			MeshResourceManager resourceManager;

			public TestResource(ref uint id, MeshResourceManager resourceManager)
			{
				this.id = ref id;
				this.resourceManager = resourceManager;
			}

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

			/*
			public ref struct Ref
			{
				public ref Runner.TestResourceId TestResource => ref resourceManager.Get(id);

				ref uint id;
				MeshResourceManager resourceManager;

				public Ref(ref uint id, MeshResourceManager resourceManager)
				{
					this.id = ref id;
					this.resourceManager = resourceManager;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Set(in Runner.TestResource data)
				{
					this.id = resourceManager.Store(data);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArray(ref Array array, int idx, MeshResourceManager resourceManager)
				{
					return new Ref(ref array.id[idx], resourceManager);
				}
			}
			*/

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

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Set(in Runner.TestResource data)
			{
				this.id = resourceManager.Store(data);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static TestResource FromArray(ref Array array, int idx, MeshResourceManager resourceManager)
			{
				return new TestResource(ref array.id[idx], resourceManager);
			}
		}
	}
}