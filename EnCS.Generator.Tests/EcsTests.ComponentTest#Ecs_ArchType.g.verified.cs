//HintName: Ecs_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs
	{
		public struct Wall : IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>, IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>
		{
			public Project.Primitives.Position.Vectorized Position;
			public Project.Primitives.Velocity.Vectorized Velocity;
			public Project.Primitives.MeshResourceManager.Mesh.Vectorized Mesh;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Array IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref arch.Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Vectorized IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetVec(ref Wall arch)
			{
				return ref arch.Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Velocity.Array IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>(ref arch.Velocity);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Velocity.Vectorized IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.GetVec(ref Wall arch)
			{
				return ref arch.Velocity;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.MeshResourceManager.Mesh.Array IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>(ref arch.Mesh);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.MeshResourceManager.Mesh.Vectorized IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>.GetVec(ref Wall arch)
			{
				return ref arch.Mesh;
			}

			public ref struct Ref
			{
				public Project.Primitives.Position.Ref Position;
				public Project.Primitives.Velocity.Ref Velocity;
				public Project.Primitives.MeshResourceManager.Mesh.Ref Mesh;

				public Ref(Project.Primitives.Position.Ref Position, Project.Primitives.Velocity.Ref Velocity, Project.Primitives.MeshResourceManager.Mesh.Ref Mesh)
				{
					this.Position = Position;
					this.Velocity = Velocity;
					this.Mesh = Mesh;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Wall archType, int idx, Project.Primitives.MeshResourceManager MeshResourceManager)
				{
					return new Ref(Project.Primitives.Position.Ref.FromArray(ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref archType.Position), idx), 
						Project.Primitives.Velocity.Ref.FromArray(ref Unsafe.As<Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>(ref archType.Velocity), idx), 
						Project.Primitives.MeshResourceManager.Mesh.Ref.FromArray(ref Unsafe.As<Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>(ref archType.Mesh), idx, MeshResourceManager));
				}
			}
		}

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

	public static class Ecs_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Wall.Ref Get(this ref ArchTypeContainer<Ecs.Wall> container, ArchRef<Ecs.Wall> ptr, Project.Primitives.MeshResourceManager MeshResourceManager)
		{
			return Ecs.Wall.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7, MeshResourceManager);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Tile.Ref Get(this ref ArchTypeContainer<Ecs.Tile> container, ArchRef<Ecs.Tile> ptr)
		{
			return Ecs.Tile.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}
	}
}