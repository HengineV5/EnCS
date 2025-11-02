//HintName: Ecs_ArchType.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct Wall
		{
			public Runner.Position Position;
			public Runner.Velocity Velocity;
			public Runner.MeshResourceManager.TestResource TestResource;

			public Wall(Runner.Position Position, Runner.Velocity Velocity, Runner.MeshResourceManager.TestResource TestResource)
			{
				this.Position = Position;
				this.Velocity = Velocity;
				this.TestResource = TestResource;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Wall FromArchType(ref Wall.Vectorized archType, int idx, Runner.MeshResourceManager MeshResourceManager)
			{
				return new Wall(Runner.Position.FromArray(ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref archType._Position), idx), 
					Runner.Velocity.FromArray(ref Unsafe.As<Runner.Velocity.Vectorized, Runner.Velocity.Array>(ref archType._Velocity), idx), 
					Runner.MeshResourceManager.TestResource.FromArray(ref Unsafe.As<Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>(ref archType._TestResource), idx, MeshResourceManager));
			}

			public struct Vectorized : IArchType<Wall.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>, IArchType<Wall.Vectorized, Runner.Velocity.Vectorized, Runner.Velocity.Array>, IArchType<Wall.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>
			{
				public Runner.Position.Vectorized _Position;
				public Runner.Velocity.Vectorized _Velocity;
				public Runner.MeshResourceManager.TestResource.Vectorized _TestResource;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Position.Array IArchType<Wall.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref Wall.Vectorized arch)
				{
					return ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref arch._Position);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Position.Vectorized IArchType<Wall.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref Wall.Vectorized arch)
				{
					return ref arch._Position;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Velocity.Array IArchType<Wall.Vectorized, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref Wall.Vectorized arch)
				{
					return ref Unsafe.As<Runner.Velocity.Vectorized, Runner.Velocity.Array>(ref arch._Velocity);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Velocity.Vectorized IArchType<Wall.Vectorized, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref Wall.Vectorized arch)
				{
					return ref arch._Velocity;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.MeshResourceManager.TestResource.Array IArchType<Wall.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetSingle(ref Wall.Vectorized arch)
				{
					return ref Unsafe.As<Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>(ref arch._TestResource);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.MeshResourceManager.TestResource.Vectorized IArchType<Wall.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetVec(ref Wall.Vectorized arch)
				{
					return ref arch._TestResource;
				}
			}
		}

		public ref struct Tile
		{
			public Runner.Position Position;
			public Runner.MeshResourceManager.TestResource TestResource;

			public Tile(Runner.Position Position, Runner.MeshResourceManager.TestResource TestResource)
			{
				this.Position = Position;
				this.TestResource = TestResource;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Tile FromArchType(ref Tile.Vectorized archType, int idx, Runner.MeshResourceManager MeshResourceManager)
			{
				return new Tile(Runner.Position.FromArray(ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref archType._Position), idx), 
					Runner.MeshResourceManager.TestResource.FromArray(ref Unsafe.As<Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>(ref archType._TestResource), idx, MeshResourceManager));
			}

			public struct Vectorized : IArchType<Tile.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>, IArchType<Tile.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>
			{
				public Runner.Position.Vectorized _Position;
				public Runner.MeshResourceManager.TestResource.Vectorized _TestResource;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Position.Array IArchType<Tile.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref Tile.Vectorized arch)
				{
					return ref Unsafe.As<Runner.Position.Vectorized, Runner.Position.Array>(ref arch._Position);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.Position.Vectorized IArchType<Tile.Vectorized, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref Tile.Vectorized arch)
				{
					return ref arch._Position;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.MeshResourceManager.TestResource.Array IArchType<Tile.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetSingle(ref Tile.Vectorized arch)
				{
					return ref Unsafe.As<Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>(ref arch._TestResource);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.MeshResourceManager.TestResource.Vectorized IArchType<Tile.Vectorized, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetVec(ref Tile.Vectorized arch)
				{
					return ref arch._TestResource;
				}
			}
		}

		public ref struct Cam
		{
			public Runner.TestComp123 TestComp123;

			public Cam(Runner.TestComp123 TestComp123)
			{
				this.TestComp123 = TestComp123;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Cam FromArchType(ref Cam.Vectorized archType, int idx)
			{
				return new Cam(Runner.TestComp123.FromArray(ref Unsafe.As<Runner.TestComp123.Vectorized, Runner.TestComp123.Array>(ref archType._TestComp123), idx));
			}

			public struct Vectorized : IArchType<Cam.Vectorized, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>
			{
				public Runner.TestComp123.Vectorized _TestComp123;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.TestComp123.Array IArchType<Cam.Vectorized, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>.GetSingle(ref Cam.Vectorized arch)
				{
					return ref Unsafe.As<Runner.TestComp123.Vectorized, Runner.TestComp123.Array>(ref arch._TestComp123);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				static ref Runner.TestComp123.Vectorized IArchType<Cam.Vectorized, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>.GetVec(ref Cam.Vectorized arch)
				{
					return ref arch._TestComp123;
				}
			}
		}
	}

	public static class Ecs_ContainerExtensions
	{
		// TODO: Generate create method

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Wall Get(this ref IndexedContainer<Ecs.Wall.Vectorized, Ecs.Wall> container, ArchRef<Ecs.Wall> ptr, Runner.MeshResourceManager MeshResourceManager)
		{
			return Ecs.Wall.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7, MeshResourceManager);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Tile Get(this ref IndexedContainer<Ecs.Tile.Vectorized, Ecs.Tile> container, ArchRef<Ecs.Tile> ptr, Runner.MeshResourceManager MeshResourceManager)
		{
			return Ecs.Tile.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7, MeshResourceManager);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Ecs.Cam Get(this ref IndexedContainer<Ecs.Cam.Vectorized, Ecs.Cam> container, ArchRef<Ecs.Cam> ptr)
		{
			return Ecs.Cam.FromArchType(ref container.GetVec(ptr), (int)ptr.idx & 7);
		}
	}
}