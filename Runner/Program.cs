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

namespace Runner
{
	public struct TestResource
	{
		public string name;
	}

	[ResourceManager]
	public partial class MeshResourceManager : IResourceManager<TestResource>
	{
		Memory<Runner.TestResource> resource = new Runner.TestResource[8];

        public MeshResourceManager()
        {
			resource.Span[0] = new() { name = "yay" };
			resource.Span[1] = new() { name = "nay" };
        }

        public ref Runner.TestResource Get(uint id)
		{
			return ref resource.Span[(int)id];
		}

		public uint Store(in Runner.TestResource resource)
		{
			return resource.name == "yay" ? 0u : 1u;
		}
	}

	[Component]
	partial struct TestComp123
	{
		//public string wow;
	}

	[Component]
	partial struct Position
	{
		public float x;
		public float y;
		public float z;

		public Position(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static implicit operator Position(Vector3 v) => new Position(v.X, v.Y, v.Z);
	}

	[Component]
	partial struct Velocity
	{
		public int x;
		public int y;
		public int z;

		public Velocity(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	[System]
	partial class PerfSystem
	{
		static Vector256<float> vf = Vector256.Create(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);

		public void Update(Position.Ref position)
		{
			position.x = Random.Shared.Next(0, 100);
			//position.x = MathF.Sqrt(position.x);
		}

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

		public void Update(Position.Ref position, ref TestResource resource)
		{
            position.x = Random.Shared.Next(0, 100);
            Console.WriteLine(resource.name);
            //position.x = MathF.Sqrt(position.x);
        }

		public void Update(ref Position.Vectorized position)
		{
            position.x = Vector256.Sqrt(position.x);
		}
	}

	[System]
	partial class PrintSystem
	{
		public void Update(Position.Ref position)
		{
            Console.WriteLine(position.x);
        }

		public void Update(ref Position.Vectorized position)
		{
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
				})
				.System(x =>
				{
					x.System<PositionSystem>();
					x.System<PrintSystem>();
					x.System<PerfSystem>();
				})
				.World(x =>
				{
					x.World<Ecs.Wall, Ecs.Tile>("MainWorld");
				})
				.Resource(x =>
				{
					x.ResourceManager<MeshResourceManager>();
				})
				.Build<Ecs>();

			PositionSystem position = new();

			MeshResourceManager meshResourceManager = new();
			Ecs ecs = new(meshResourceManager);

			Ecs.MainWorld mainWorld = ecs.GetMainWorld();
			ArchRef<Ecs.Tile> tile1 = mainWorld.Create(new Ecs.Tile());
			ArchRef<Ecs.Tile> tile2 = mainWorld.Create(new Ecs.Tile());

			ArchRef<Ecs.Wall> wall1 = mainWorld.Create(new Ecs.Wall());
			ArchRef<Ecs.Wall> wall2 = mainWorld.Create(new Ecs.Wall());

			Ecs.Tile.Ref tile1Ref = mainWorld.Get(tile1);
			Ecs.Tile.Ref tile2Ref = mainWorld.Get(tile2);
			Ecs.Wall.Ref wall1Ref = mainWorld.Get(wall1);
			Ecs.Wall.Ref wall2Ref = mainWorld.Get(wall2);

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
			wall1Ref.Position.Set(new Position(2, 0, 0));
			tile1Ref.Position.Set(new Position(2, 0, 0));
            Console.WriteLine(tile1Ref.Position.x);

            Console.WriteLine("Systems:");

            mainWorld.Loop(position);
			tile1Ref.Position.x = 20;
			tile2Ref.Position.x = 12;
			mainWorld.Loop(new PrintSystem());

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
	}

	[SimpleJob(RuntimeMoniker.Net80)]
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
				mainWorld.Create(new Ecs.Wall());
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