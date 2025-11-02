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
	partial class PositionSystem
	{
		static Vector256<float> vf = Vector256.Create(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);

		[SystemUpdate]
		public void Update(ref Position position, ref TestResourceId resource)
		{
            position.x = Random.Shared.Next(0, 100);
            Console.WriteLine(resource.id);
            //position.x = MathF.Sqrt(position.x);
        }

		[SystemUpdate]
		public void Update(ref Position.Vectorized position)
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

        public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch>
            where TArch : unmanaged, IArchType<TArch, Position.Vectorized, Position.Array>
        {
            PrintSystem system;

            public SystemUpdater_0(PrintSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, ref TArch slice)
            {
                ref Position.Vectorized vec = ref TArch.GetVec(ref slice);
                ref Position.Array single = ref TArch.GetSingle(ref slice);

                for (int i = 0; i < remaining; i++)
                {
                    Position comp = Position.FromArray(ref single, i);
                    system.Update(ref comp);
                }
                system.Update(ref vec);
            }
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

        public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch>
            where TArch : unmanaged, IArchType<TArch, Position.Vectorized, Position.Array>, IArchType<TArch, Velocity.Vectorized, Velocity.Array>
        {
            PrintSystem_2 system;

            public SystemUpdater_0(PrintSystem_2 system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, ref TArch slice)
            {
                ref Position.Vectorized vec1 = ref ArchGetter<TArch, Position.Vectorized, Position.Array>.GetVec(ref slice);
                ref Position.Array single1 = ref ArchGetter<TArch, Position.Vectorized, Position.Array>.GetSingle(ref slice);
                ref Velocity.Vectorized vec2 = ref ArchGetter<TArch, Velocity.Vectorized, Velocity.Array>.GetVec(ref slice);
                ref Velocity.Array single2 = ref ArchGetter<TArch, Velocity.Vectorized, Velocity.Array>.GetSingle(ref slice);

                for (int i = 0; i < remaining; i++)
                {
                    var comp = Position.FromArray(ref single1, i);
                    var comp2 = Velocity.FromArray(ref single2, i);
                    system.Update(ref comp, ref comp2);
                }
                system.Update(ref vec1, ref vec2);
            }
        }
    }

	[System]
	partial class LayerSystem
    {
		[SystemUpdate, SystemLayer(0)]
		public void Update1(ref Velocity velocity)
		{
			Console.WriteLine($"Tag: {velocity.x}");
		}

		[SystemUpdate, SystemLayer(1)]
		public void Update2(ref Position position)
		{
			Console.WriteLine($"\t{position.x}");
		}

        public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch>
            where TArch : unmanaged, IArchType<TArch, Velocity.Vectorized, Velocity.Array>
        {
            LayerSystem system;

            public SystemUpdater_0(LayerSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, ref TArch slice)
            {
                ref Velocity.Vectorized vec = ref TArch.GetVec(ref slice);
                ref Velocity.Array single = ref TArch.GetSingle(ref slice);

                for (int i = 0; i < remaining; i++)
                {
                    Velocity comp = Velocity.FromArray(ref single, i);
                    system.Update1(ref comp);
                }
            }
		}

        public ref struct SystemUpdater_1<TArch> : ISystemUpdater<SystemUpdater_1<TArch>, TArch>
            where TArch : unmanaged, IArchType<TArch, Position.Vectorized, Position.Array>
        {
            LayerSystem system;

            public SystemUpdater_1(LayerSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, ref TArch slice)
            {
                ref Position.Vectorized vec = ref TArch.GetVec(ref slice);
                ref Position.Array single = ref TArch.GetSingle(ref slice);

                for (int i = 0; i < remaining; i++)
                {
                    Position comp = Position.FromArray(ref single, i);
                    system.Update2(ref comp);
                }
            }
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

			IndexedContainer<Wall.Vectorized, Wall> containerWall = new();
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			containerWall.Create(new Wall.Vectorized());
			var r = containerWall.Create(new Wall.Vectorized());

			Wall wall = containerWall.Get(r, new());
			wall.Position.Set(new Vector3(5, 0, 0));

            PrintSystem printSystem = new();
			PrintSystem.SystemUpdater_0<Wall.Vectorized> positionUpdater = new(printSystem);

            PrintSystem_2 printSystem2 = new();
			PrintSystem_2.SystemUpdater_0<Wall.Vectorized> position2Updater = new(printSystem2);

            LayerSystem layerSystem = new();
            LayerSystem.SystemUpdater_0<Wall.Vectorized> layerUpdater_0 = new(layerSystem);
            LayerSystem.SystemUpdater_1<Wall.Vectorized> layerUpdater_1 = new(layerSystem);

            //Looper<Wall.Vectorized>.Loop(ref posEnum, positionUpdater);
            //Looper<Wall.Vectorized>.Loop(ref posEnum, position2Updater);
            Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, position2Updater);
            //Looper<Wall.Vectorized>.LoopIndexed(ref containerWall, layerUpdater_0, layerUpdater_1);
			/*
			*/
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