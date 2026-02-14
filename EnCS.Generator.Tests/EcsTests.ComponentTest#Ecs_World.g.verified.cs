//HintName: Ecs_World.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class Ecs
	{
		public ref struct MainWorld : IWorld<PrintSystem, Ecs >, IWorld<PerfSystem, Ecs >, IWorld<LayerSystem, Ecs >
		{
			ref IndexedContainer<Wall.Memory, Wall.Vectorized, Wall> containerWall;
			ref IndexedContainer<Cam.Memory, Cam.Vectorized, Cam> containerCam;

			Runner.MeshResourceManager MeshResourceManager;

			public MainWorld(ref IndexedContainer<Wall.Memory, Wall.Vectorized, Wall> containerWall, ref IndexedContainer<Cam.Memory, Cam.Vectorized, Cam> containerCam, Runner.MeshResourceManager MeshResourceManager)
			{
				this.containerWall = ref containerWall;
				this.containerCam = ref containerCam;

				this.MeshResourceManager = MeshResourceManager;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Wall> CreateWall()
			{
				return containerWall.Create();
			}

			public void Delete(ref readonly ArchRef<Wall> ptr)
			{
				containerWall.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Wall Get(ref readonly ArchRef<Wall> ptr)
			{
				return containerWall.GetSingle(ptr).SetResourceManager(MeshResourceManager);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ArchRef<Cam> CreateCam()
			{
				return containerCam.Create();
			}

			public void Delete(ref readonly ArchRef<Cam> ptr)
			{
				containerCam.Delete(ptr);
			}
			
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Cam Get(ref readonly ArchRef<Cam> ptr)
			{
				return containerCam.GetSingle(ptr).SetResourceManager();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PrintSystem system)
			{
				PrintSystem.SystemUpdater_0<Wall.Vectorized, Wall> updaterWall_0 = new(system);

				PrintSystem.Context context = new();

				Looper<Wall.Vectorized, Wall>.LoopIndexed(ref containerWall, updaterWall_0, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(PerfSystem system)
			{
				PerfSystem.SystemUpdater_0<Wall.Vectorized, Wall> updaterWall_0 = new(system);

				PerfSystem.Context context = new();

				Looper<Wall.Vectorized, Wall>.LoopIndexed(ref containerWall, updaterWall_0, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Loop(LayerSystem system)
			{
				LayerSystem.SystemUpdater_0<Wall.Vectorized, Wall> updaterWall_0 = new(system);
				LayerSystem.SystemUpdater_1<Wall.Vectorized, Wall> updaterWall_1 = new(system);

				LayerSystem.Context context = new();

				Looper<Wall.Vectorized, Wall>.LoopIndexed(ref containerWall, updaterWall_0, updaterWall_1, ref context);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Loop(PrintSystem system, Ecs ecs)
				=> ecs.GetMainWorld().Loop(system);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Loop(PerfSystem system, Ecs ecs)
				=> ecs.GetMainWorld().Loop(system);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Loop(LayerSystem system, Ecs ecs)
				=> ecs.GetMainWorld().Loop(system);
		}
	}
}