//HintName: Ecs2_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs2
	{
		public struct Tile : IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>
		{
			public Project.Primitives.Position.Vectorized Position;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Array IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetSingle(ref Tile arch)
			{
				return ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref arch.Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Vectorized IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetVec(ref Tile arch)
			{
				return ref arch.Position;
			}

			public ref struct Ref
			{
				public Project.Primitives.Position.Ref Position;

				public Ref(Project.Primitives.Position.Ref Position)
				{
					this.Position = Position;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Tile archType, int idx)
				{
					return new Ref(Project.Primitives.Position.Ref.FromArray(ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref archType.Position), idx));
				}
			}
		}
	}

	public static class Ecs2_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs2.Tile.Ref Get(this ref ArchTypeContainer<Ecs2.Tile> container, ArchRef<Ecs2.Tile> ptr)
		{
			return Ecs2.Tile.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}
	}
}