//HintName: Ecs_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct MainWorld
		{
			ref IndexedContainer<Wall.Vectorized, Wall> containerWall;
			ref IndexedContainer<Tile.Vectorized, Tile> containerTile;
			ref IndexedContainer<Cam.Vectorized, Cam> containerCam;

			Runner.MeshResourceManager MeshResourceManager;

			public MainWorld(ref IndexedContainer<Wall.Vectorized, Wall> containerWall, ref IndexedContainer<Tile.Vectorized, Tile> containerTile, ref IndexedContainer<Cam.Vectorized, Cam> containerCam, Runner.MeshResourceManager MeshResourceManager)
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
				PositionSystem.SystemUpdater_0<Wall.Vectorized> updaterWall_0 = new(systems, MeshResourceManager);

				PositionSystem.SystemUpdater_0<Tile.Vectorized> updaterTile_0 = new(systems, MeshResourceManager);

				PositionSystem.Context context = new();

				Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, updaterWall_0, ref context);
				Looper<Tile.Vectorized>.LoopIndexed(ref containerTile, updaterTile_0, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PrintSystem system)
			{
				PrintSystem.SystemUpdater_0<Wall.Vectorized> updaterWall_0 = new(systems);

				PrintSystem.SystemUpdater_0<Tile.Vectorized> updaterTile_0 = new(systems);

				PrintSystem.Context context = new();

				Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, updaterWall_0, ref context);
				Looper<Tile.Vectorized>.LoopIndexed(ref containerTile, updaterTile_0, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PerfSystem system)
			{
				PerfSystem.SystemUpdater_0<Wall.Vectorized> updaterWall_0 = new(systems);

				PerfSystem.SystemUpdater_0<Tile.Vectorized> updaterTile_0 = new(systems);

				PerfSystem.Context context = new();

				Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, updaterWall_0, ref context);
				Looper<Tile.Vectorized>.LoopIndexed(ref containerTile, updaterTile_0, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(LayerSystem system)
			{
				LayerSystem.SystemUpdater_0<Cam.Vectorized> updaterCam_0 = new(systems);
				LayerSystem.SystemUpdater_1<Cam.Vectorized> updaterCam_1 = new(systems);

				LayerSystem.SystemUpdater_0<Wall.Vectorized> updaterWall_0 = new(systems);
				LayerSystem.SystemUpdater_1<Wall.Vectorized> updaterWall_1 = new(systems);

				LayerSystem.SystemUpdater_0<Tile.Vectorized> updaterTile_0 = new(systems);
				LayerSystem.SystemUpdater_1<Tile.Vectorized> updaterTile_1 = new(systems);

				LayerSystem.Context context = new();

				Looper<Cam.Vectorized>.LoopIndexed(ref containerCam, updaterCam_0, updaterCam_1, ref context);
				Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, updaterWall_0, updaterWall_1, ref context);
				Looper<Tile.Vectorized>.LoopIndexed(ref containerTile, updaterTile_0, updaterTile_1, ref context);
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