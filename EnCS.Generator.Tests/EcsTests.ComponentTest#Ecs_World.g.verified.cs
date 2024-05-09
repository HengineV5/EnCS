//HintName: Ecs_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Test
{
	public partial class Ecs
	{
		public ref struct Main
		{
			public struct Interface : IWorld<Ecs, PositionSystem  >, IWorld<Ecs, VelocitySystem  >, IWorld<Ecs, ResourceSystem , TestContext, TestContext2 >
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, PositionSystem system)
					=> ecs.GetMain().Loop(system);

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, VelocitySystem system)
					=> ecs.GetMain().Loop(system);

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, ResourceSystem system, ref TestContext contextTestContext, ref TestContext2 contextTestContext2)
					=> ecs.GetMain().Loop(system, ref contextTestContext, ref contextTestContext2);
			}

			ref ArchTypeContainer<Wall> containerWall;
			ref ArchTypeContainer<Tile> containerTile;

			Project.Primitives.MeshResourceManager MeshResourceManager;
			Project.Primitives.TestResourceManager TestResourceManager;

			public Main(ref ArchTypeContainer<Wall> containerWall, ref ArchTypeContainer<Tile> containerTile, Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
			{
				this.containerWall = ref containerWall;
				this.containerTile = ref containerTile;

				this.MeshResourceManager = MeshResourceManager;
				this.TestResourceManager = TestResourceManager;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Wall> Create(in Wall data)
			{
				return containerWall.Create(data);
			}

			public void Delete(in ArchRef<Wall> ptr)
			{
				containerWall.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Wall.Ref Get(in ArchRef<Wall> ptr)
			{
				return containerWall.Get(ptr, MeshResourceManager, TestResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Tile> Create(in Tile data)
			{
				return containerTile.Create(data);
			}

			public void Delete(in ArchRef<Tile> ptr)
			{
				containerTile.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Tile.Ref Get(in ArchRef<Tile> ptr)
			{
				return containerTile.Get(ptr);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PositionSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(VelocitySystem system)
			{
				var enumWall = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				
				system.Update(ref enumWall);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(ResourceSystem system, ref TestContext contextTestContext, ref TestContext2 contextTestContext2)
			{
				var enumWall = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>.Enumerator<Tile>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall, ref enumTile, MeshResourceManager, TestResourceManager, ref contextTestContext, ref contextTestContext2);
			}
		}

		public ref struct World2
		{
			public struct Interface : IWorld<Ecs, PositionSystem  >, IWorld<Ecs, VelocitySystem  >
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, PositionSystem system)
					=> ecs.GetWorld2().Loop(system);

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, VelocitySystem system)
					=> ecs.GetWorld2().Loop(system);
			}

			ref ArchTypeContainer<Wall> containerWall;

			Project.Primitives.MeshResourceManager MeshResourceManager;
			Project.Primitives.TestResourceManager TestResourceManager;

			public World2(ref ArchTypeContainer<Wall> containerWall, Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
			{
				this.containerWall = ref containerWall;

				this.MeshResourceManager = MeshResourceManager;
				this.TestResourceManager = TestResourceManager;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Wall> Create(in Wall data)
			{
				return containerWall.Create(data);
			}

			public void Delete(in ArchRef<Wall> ptr)
			{
				containerWall.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Wall.Ref Get(in ArchRef<Wall> ptr)
			{
				return containerWall.Get(ptr, MeshResourceManager, TestResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PositionSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				
				system.Update(ref enumWall);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(VelocitySystem system)
			{
				var enumWall = new ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.Enumerator<Wall>(containerWall.AsSpan(), (int)containerWall.Entities);
				
				system.Update(ref enumWall);
			}
		}
	}
}