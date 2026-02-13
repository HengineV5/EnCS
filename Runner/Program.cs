using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using EnCS;
using EnCS.Attributes;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.Json;
using static Runner.Ecs;

namespace Runner
{
	public struct TestResource
	{
		public string name;
	}

	public struct TestResourceId
	{
		public uint id;
	}

	public struct TestContext
	{
		public float id;
	}

	[ResourceManager]
	public partial class MeshResourceManager : IResourceManager<TestResource, TestResourceId>
	{
		Memory<Runner.TestResource> resource = new Runner.TestResource[8];
		Memory<Runner.TestResourceId> resourceids = new Runner.TestResourceId[8];

        public MeshResourceManager()
        {
			resource.Span[0] = new() { name = "yay" };
			resourceids.Span[0] = new() { id = 0 };
			resource.Span[1] = new() { name = "nay" };
			resourceids.Span[1] = new() { id = 1 };
        }

        public ref Runner.TestResourceId Get(uint id)
		{
			return ref resourceids.Span[(int)id];
		}

		public uint Store(in Runner.TestResource resource)
		{
			return resource.name == "yay" ? 0u : 1u;
		}
	}

	[Component]
	ref partial struct TestComp123
	{
		//public string wow;
		public ref int tag;
    }

	[Component]
	ref partial struct Position
    {
		public ref float x;
		public ref float y;
		public ref float z;
    }

	static class Comp_Extensions
	{
		public static void Set(this ref Position position, Vector3 value)
		{
			position.x = value.X;
			position.y = value.Y;
			position.z = value.Z;
		}
	}

	[Component]
	ref partial struct Velocity
	{
		public ref int x;
		public ref int y;
		public ref int z;
    }

	[System]
	partial class PerfSystem
	{
		static Vector256<float> vf = Vector256.Create(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);

		[SystemUpdate]
		public void Update(ref Position position)
		{
			position.x = Random.Shared.Next(0, 100);
			position.x = MathF.Sqrt(position.x);
		}

		[SystemUpdate]
		public void Update(ref Position.Vectorized position)
		{
			position.x = Vector256.Sqrt(position.x);
		}
	}

	[System]
	[UsingResource<MeshResourceManager>]
	[SystemContext<TestContext>]
	partial class PositionSystem
	{
		static Vector256<float> vf = Vector256.Create(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);

		[SystemUpdate]
		public void Update(ref TestContext context, ref Position position, ref TestResourceId resource)
		{
            position.x = Random.Shared.Next(0, 100);
            Console.WriteLine(resource.id);
            //position.x = MathF.Sqrt(position.x);
        }

		[SystemUpdate]
		public void Update(ref TestContext context, ref Position.Vectorized position)
		{
            position.x = Vector256.Sqrt(position.x);
		}
	}

	[System]
	partial class PrintSystem
	{
		[SystemUpdate]
		public void Update(ref Position position)
		{
            Console.WriteLine($"Print System: {position.x}");
        }

		[SystemUpdate]
		public void Update(ref Position.Vectorized position)
		{
            Console.WriteLine($"Vec Print System: {position.x}");
        }
    }

	[System]
	partial class PrintSystem_2
	{
		[SystemUpdate]
		public void Update(ref Position position, ref Velocity velocity)
		{
            Console.WriteLine($"Print System: {position.x}");
        }

		[SystemUpdate]
		public void Update(ref Position.Vectorized position, ref Velocity.Vectorized velocity)
		{
            Console.WriteLine($"Vec Print System: {position.x}");
        }
	}

	[System]
	partial class LayerSystem
    {
		[SystemUpdate, SystemLayer(0)]
		public void Update1(scoped ref Velocity velocity)
		{
			Console.WriteLine($"Tag: {velocity.x}");
		}

		[SystemUpdate, SystemLayer(1)]
		public void Update2(scoped ref Position position)
		{
			Console.WriteLine($"\t{position.x}");
		}
	}

	partial class Ecs
	{

	}

    internal class Program
	{
		static void Main(string[] args)
		{
#if RELEASE
			BenchmarkRunner.Run<PerfTests>();
			return;
#endif
			new EcsBuilder()
				.ArchType(x =>
				{
					x.ArchType<Position, Velocity, TestResource>("Wall");
					//x.ArchType<Position, TestResource>("Tile");
					x.ArchType<TestComp123>("Cam");
				})
				.System(x =>
				{
					//x.System<PositionSystem>();
					x.System<PrintSystem>();
					x.System<PerfSystem>();
					x.System<LayerSystem>();
				})
				.World(x =>
				{
					//x.World<Ecs.Wall, Ecs.Tile, Ecs.Cam>("MainWorld");
					x.World<Ecs.Wall, Ecs.Cam>("MainWorld");
				})
				.Resource(x =>
				{
					x.ResourceManager<MeshResourceManager>();
				})
				.Build<Ecs>();
			/*
			*/

			IndexedContainer<Wall.Memory, Wall.Vectorized, Wall> containerWall = new();
			var r0 = containerWall.Create();

			{
				Wall w = containerWall.GetSingle(in r0);
				w.Position.Set(new Vector3(2, 0, 0));
			}

			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			containerWall.Create();
			var r = containerWall.Create();

			Wall wall = containerWall.GetSingle(r);
			wall.Position.Set(new Vector3(5, 0, 0));

			HierarchicalContainer<Wall.Memory, Wall.Vectorized, Wall> containerWallH = new();


			PrintSystem printSystem = new();
			PrintSystem.SystemUpdater_0<Wall.Vectorized, Wall> positionUpdater = new(printSystem);

            PrintSystem_2 printSystem2 = new();
			PrintSystem_2.SystemUpdater_0<Wall.Vectorized, Wall> position2Updater = new(printSystem2);
			PrintSystem_2.Context printSystemContext = new();

			LayerSystem layerSystem = new();
            LayerSystem.SystemUpdater_0<Wall.Vectorized, Wall> layerUpdater_0 = new(layerSystem);
            LayerSystem.SystemUpdater_1<Wall.Vectorized, Wall> layerUpdater_1 = new(layerSystem);
			
            //Looper<Wall.Vectorized>.Loop(ref posEnum, positionUpdater);
            //Looper<Wall.Vectorized>.Loop(ref posEnum, position2Updater);
            Looper<Wall.Vectorized, Wall>.LoopIndexed(ref containerWall, position2Updater, ref printSystemContext);
            Looper<Wall.Vectorized, Wall>.LoopMapped(ref containerWall, position2Updater, [0, 1, 8, 9, 10], ref printSystemContext);
            Looper<Wall.Vectorized, Wall>.LoopHierarchical(ref containerWallH, position2Updater, containerWallH.GetRoot(), ref printSystemContext);
            //Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, layerUpdater_0, layerUpdater_1);
		}
    }


    /*
    [SimpleJob(RuntimeMoniker.Net90)]
    [MemoryDiagnoser]
    public class PerfTests
    {
        Ecs ecs;
        float val;
        PerfSystem system = new();

        [GlobalSetup]
        public void Setup()
        {
            MeshResourceManager meshResourceManager = new();
            ecs = new();

            Ecs.MainWorld mainWorld = ecs.GetMainWorld();
            for (int i = 0; i < 1_000_000; i++)
            {
                mainWorld.Create(new Ecs.Wall.Vectorized());
            }
        }

        [Benchmark(Baseline = true)]
        public void Baseline()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                val = Random.Shared.Next(0, 100);
                val = MathF.Sqrt(val);
            }
        }

        [Benchmark]
        public void AddSystem()
        {
            Ecs.MainWorld mainWorld = ecs.GetMainWorld();
            mainWorld.Loop(system);
        }
    }
    */
}