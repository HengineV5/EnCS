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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
			//position.x = Vector256.Sqrt(position.x);
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
            Console.WriteLine(position.x);
        }

		[SystemUpdate]
		public void Update(ref Position.Vectorized position)
		{
		}
	}

	[System]
	partial class LayerSystem
	{
		[SystemUpdate, SystemLayer(0)]
		public void Update1(ref TestComp123 position)
		{
			Console.WriteLine($"Tag: {position.tag}");
		}

		[SystemUpdate, SystemLayer(1)]
		public void Update2(ref Position position)
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
					x.ArchType<Position, TestResource>("Tile");
					x.ArchType<TestComp123>("Cam");
				})
				.System(x =>
				{
					x.System<PositionSystem>();
					x.System<PrintSystem>();
					x.System<PerfSystem>();
					x.System<LayerSystem>();
				})
				.World(x =>
				{
					x.World<Ecs.Wall, Ecs.Tile, Ecs.Cam>("MainWorld");
				})
				.Resource(x =>
				{
					x.ResourceManager<MeshResourceManager>();
				})
				.Build<Ecs>();

			PositionSystem position = new();
			LayerSystem layerSystem = new();

			MeshResourceManager meshResourceManager = new();
			Ecs ecs = new(meshResourceManager);

			Ecs.MainWorld mainWorld = ecs.GetMainWorld();
			ArchRef<Ecs.Tile> tile1 = mainWorld.Create(new Ecs.Tile.Vectorized());
			ArchRef<Ecs.Tile> tile2 = mainWorld.Create(new Ecs.Tile.Vectorized());

			ArchRef<Ecs.Wall> wall1 = mainWorld.Create(new Ecs.Wall.Vectorized());
			ArchRef<Ecs.Wall> wall2 = mainWorld.Create(new Ecs.Wall.Vectorized());

			var camRef1 = mainWorld.Get(mainWorld.Create(new Ecs.Cam.Vectorized()));
			var camRef2 = mainWorld.Get(mainWorld.Create(new Ecs.Cam.Vectorized()));

			camRef1.TestComp123.tag = 1;
			camRef1.TestComp123.tag = 2;

			Ecs.Tile tile1Ref = mainWorld.Get(in tile1);
			Ecs.Tile tile2Ref = mainWorld.Get(in tile2);
			Ecs.Wall wall1Ref = mainWorld.Get(in wall1);
			Ecs.Wall wall2Ref = mainWorld.Get(in wall2);

			var r = new TestResource()
			{
				name = "nay"
			};

			tile1Ref.TestResource.Set(r);
			tile2Ref.TestResource.Set(r);

            Console.WriteLine("Single:");
            Console.WriteLine(tile1Ref.Position.x);
			wall1Ref.Position.x = 1;
			wall2Ref.Position.x = 3;
			wall1Ref.Position.Set(new Vector3(2, 0, 0));
			tile1Ref.Position.Set(new Vector3(2, 0, 0));
			Console.WriteLine(tile1Ref.Position.x);

            Console.WriteLine("Systems:");

            mainWorld.Loop(position);
			tile1Ref.Position.x = 20;
			tile2Ref.Position.x = 12;
            Console.WriteLine("PrintSystem:");
			LoopGeneric<Ecs.MainWorld, PrintSystem>(ecs, new PrintSystem());
			mainWorld.Loop(layerSystem);

			//ArchTypeContainerNew<Ecs.Tile, Position, Position.Vectorized, Position.Array> testContainer = new();
			//testContainer.Set(r, new Position());

			/*
			PositionSystem system = new();
			var e = containerManager.GetEnumerator(system);

			system.Update(ref e);

			//containerManager.GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> e2);

			var e2 = containerManager.GetEnumerator(system);
			while (e2.MoveNext())
			{
				var item = e2.Current;

				for (int i = 0; i < Position.Array.Size; i++)
				{
					Console.WriteLine($"Actual: {item.item1Single.x[i]}");
				}
			}
			foreach (var item in e2)
			{
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Console.WriteLine($"Actual: {item.item1Single.x[i]}");
				}
			}
			*/
		}

		static void LoopGeneric<T, TSystem0>(Ecs ecs, TSystem0 system)
			where T : IWorld<Ecs, TSystem0>, allows ref struct
			where TSystem0 : class
		{
			T.Loop(ecs, system);
		}
	}

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
			ecs = new(meshResourceManager);

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
}