//HintName: Ecs_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs
	{
		public struct Wall : IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>, IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>, IArchType<Wall, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>
		{
			public Project.Primitives.Position.Vectorized _Position;
			public Project.Primitives.Velocity.Vectorized _Velocity;
			public Project.Primitives.MeshResourceManager.Mesh.Vectorized _Mesh;
			public Project.Primitives.TestResourceManager.Kaki.Vectorized _Kaki;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Array IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref arch._Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Vectorized IArchType<Wall, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetVec(ref Wall arch)
			{
				return ref arch._Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Velocity.Array IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>(ref arch._Velocity);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Velocity.Vectorized IArchType<Wall, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.GetVec(ref Wall arch)
			{
				return ref arch._Velocity;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.MeshResourceManager.Mesh.Array IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>(ref arch._Mesh);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.MeshResourceManager.Mesh.Vectorized IArchType<Wall, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>.GetVec(ref Wall arch)
			{
				return ref arch._Mesh;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.TestResourceManager.Kaki.Array IArchType<Wall, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>.GetSingle(ref Wall arch)
			{
				return ref Unsafe.As<Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>(ref arch._Kaki);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.TestResourceManager.Kaki.Vectorized IArchType<Wall, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>.GetVec(ref Wall arch)
			{
				return ref arch._Kaki;
			}

			public ref struct Ref
			{
				public Project.Primitives.Position.Ref _Position;
				public Project.Primitives.Velocity.Ref _Velocity;
				public Project.Primitives.MeshResourceManager.Mesh.Ref _Mesh;
				public Project.Primitives.TestResourceManager.Kaki.Ref _Kaki;

				public Ref(Project.Primitives.Position.Ref Position, Project.Primitives.Velocity.Ref Velocity, Project.Primitives.MeshResourceManager.Mesh.Ref Mesh, Project.Primitives.TestResourceManager.Kaki.Ref Kaki)
				{
					this._Position = Position;
					this._Velocity = Velocity;
					this._Mesh = Mesh;
					this._Kaki = Kaki;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Wall archType, int idx, Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
				{
					return new Ref(Project.Primitives.Position.Ref.FromArray(ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref archType._Position), idx), 
						Project.Primitives.Velocity.Ref.FromArray(ref Unsafe.As<Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>(ref archType._Velocity), idx), 
						Project.Primitives.MeshResourceManager.Mesh.Ref.FromArray(ref Unsafe.As<Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>(ref archType._Mesh), idx, MeshResourceManager), 
						Project.Primitives.TestResourceManager.Kaki.Ref.FromArray(ref Unsafe.As<Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>(ref archType._Kaki), idx, TestResourceManager));
				}
			}
		}

		public struct Tile : IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<Tile, Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>
		{
			public Project.Primitives.Position.Vectorized _Position;
			public Project.Primitives.Scale.Vectorized _Scale;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Array IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetSingle(ref Tile arch)
			{
				return ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref arch._Position);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Position.Vectorized IArchType<Tile, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.GetVec(ref Tile arch)
			{
				return ref arch._Position;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Scale.Array IArchType<Tile, Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>.GetSingle(ref Tile arch)
			{
				return ref Unsafe.As<Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>(ref arch._Scale);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static ref Project.Primitives.Scale.Vectorized IArchType<Tile, Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>.GetVec(ref Tile arch)
			{
				return ref arch._Scale;
			}

			public ref struct Ref
			{
				public Project.Primitives.Position.Ref _Position;
				public Project.Primitives.Scale.Ref _Scale;

				public Ref(Project.Primitives.Position.Ref Position, Project.Primitives.Scale.Ref Scale)
				{
					this._Position = Position;
					this._Scale = Scale;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Ref FromArchType(ref Tile archType, int idx)
				{
					return new Ref(Project.Primitives.Position.Ref.FromArray(ref Unsafe.As<Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>(ref archType._Position), idx), 
						Project.Primitives.Scale.Ref.FromArray(ref Unsafe.As<Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>(ref archType._Scale), idx));
				}
			}
		}
	}

	public static class Ecs_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Wall.Ref Get(this ref ArchTypeContainer<Ecs.Wall> container, ArchRef<Ecs.Wall> ptr, Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
		{
			return Ecs.Wall.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7, MeshResourceManager, TestResourceManager);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Tile.Ref Get(this ref ArchTypeContainer<Ecs.Tile> container, ArchRef<Ecs.Tile> ptr)
		{
			return Ecs.Tile.Ref.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}
	}
}