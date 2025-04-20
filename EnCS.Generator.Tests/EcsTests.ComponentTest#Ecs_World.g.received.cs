//HintName: Ecs_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct MainWorld : IWorld<Ecs, PositionSystem  >, IWorld<Ecs, PrintSystem  >, IWorld<Ecs, PerfSystem  >, IWorld<Ecs, LayerSystem  >
		{
			ref ArchTypeContainer<Wall.Vectorized, Wall> containerWall;
			ref ArchTypeContainer<Tile.Vectorized, Tile> containerTile;
			ref ArchTypeContainer<Cam.Vectorized, Cam> containerCam;

			Runner.MeshResourceManager MeshResourceManager;

			public MainWorld(ref ArchTypeContainer<Wall.Vectorized, Wall> containerWall, ref ArchTypeContainer<Tile.Vectorized, Tile> containerTile, ref ArchTypeContainer<Cam.Vectorized, Cam> containerCam, Runner.MeshResourceManager MeshResourceManager)
			{
				this.containerWall = ref containerWall;
				this.containerTile = ref containerTile;
				this.containerCam = ref containerCam;

				this.MeshResourceManager = MeshResourceManager;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Wall> Create(ref readonly Wall.Vectorized data)
			{
				return containerWall.Create(data);
			}

			public void Delete(ref readonly ArchRef<Wall> ptr)
			{
				containerWall.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Wall Get(ref readonly ArchRef<Wall> ptr)
			{
				return containerWall.Get(ptr, MeshResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Tile> Create(ref readonly Tile.Vectorized data)
			{
				return containerTile.Create(data);
			}

			public void Delete(ref readonly ArchRef<Tile> ptr)
			{
				containerTile.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Tile Get(ref readonly ArchRef<Tile> ptr)
			{
				return containerTile.Get(ptr, MeshResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Cam> Create(ref readonly Cam.Vectorized data)
			{
				return containerCam.Create(data);
			}

			public void Delete(ref readonly ArchRef<Cam> ptr)
			{
				containerCam.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Cam Get(ref readonly ArchRef<Cam> ptr)
			{
				return containerCam.Get(ptr);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PositionSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array, Runner.MeshResourceManager.TestResource, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.Enumerator<Wall.Vectorized>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array, Runner.MeshResourceManager.TestResource, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.Enumerator<Tile.Vectorized>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall, MeshResourceManager);
				system.Update(ref enumTile, MeshResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PrintSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall.Vectorized>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile.Vectorized>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PerfSystem system)
			{
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall.Vectorized>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile.Vectorized>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumWall);
				system.Update(ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(LayerSystem system)
			{
				var enumCam = new ComponentEnumerableNew<Runner.TestComp123, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>.Enumerator<Cam.Vectorized>(containerCam.AsSpan(), (int)containerCam.Entities);
				var enumWall = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Wall.Vectorized>(containerWall.AsSpan(), (int)containerWall.Entities);
				var enumTile = new ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<Tile.Vectorized>(containerTile.AsSpan(), (int)containerTile.Entities);
				
				system.Update(ref enumCam, ref enumWall);
				system.Update(ref enumCam, ref enumTile);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, PositionSystem system)
					=> ecs.GetMainWorld().Loop(system);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, PrintSystem system)
					=> ecs.GetMainWorld().Loop(system);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, PerfSystem system)
					=> ecs.GetMainWorld().Loop(system);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void Loop(Ecs ecs, LayerSystem system)
					=> ecs.GetMainWorld().Loop(system);
		}
	}
}