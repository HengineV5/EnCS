using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using EnCS;
using EnCS.Attributes;
using System;
using System.Buffers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Runner
{
	[Component]
	partial struct Position
	{
		public float x;
		public int y;
		public int z;
	}

	[Component]
	partial struct Velocity
	{
		public int x;
		public int y;
		public int z;
	}

	[System]
	partial class PositionSystem
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
			//BenchmarkRunner.Run<PerfTests>();
			return;
#endif
			new EcsBuilder()
				.ArchType(x =>
				{
					x.ArchType<Position, Velocity>("Wall");
					x.ArchType<Position>("Tile");
				})
				.System(x =>
				{
					x.System<PositionSystem>();
					x.System<PrintSystem>();
				})
				.World(x =>
				{
					x.World<Ecs.Wall, Ecs.Tile>("MainWorld");
				})
				.Build<Ecs>();

			PositionSystem position = new();

			Ecs ecs = new();

			ref Ecs.MainWorld mainWorld = ref ecs.GetMainWorld();
			ArchRef<Ecs.Tile> tile1 = mainWorld.Create(new Ecs.Tile());
			ArchRef<Ecs.Tile> tile2 = mainWorld.Create(new Ecs.Tile());

			ArchRef<Ecs.Wall> wall1 = mainWorld.Create(new Ecs.Wall());

			Ecs.Tile.Ref tile1Ref = mainWorld.Get(tile1);
			Ecs.Tile.Ref tile2Ref = mainWorld.Get(tile2);
			Ecs.Wall.Ref wall1Ref = mainWorld.Get(wall1);

            Console.WriteLine("Single:");
            Console.WriteLine(tile1Ref.Position.x);
			wall1Ref.Position.x = 1;
            Console.WriteLine(tile1Ref.Position.x);

            Console.WriteLine("Systems:");

            //mainWorld.Loop(position);
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

		/*
	[SimpleJob(RuntimeMoniker.Net80)]
	[MemoryDiagnoser]
	public class PerfTests
	{
		EcsContainerManager container;
		float val;
		PositionSystem system = new();

		[GlobalSetup]
		public void Setup()
		{
			container = new(1_000_000 / 8);
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
		public void AddManual()
		{
			container.GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array> e);
			foreach (var vel in e)
			{
				for (int i = 0; i < Position.Array.Size; i++)
				{
					vel.item1Single.x[i] = Random.Shared.Next(0, 100);
					vel.item1Single.x[i] = MathF.Sqrt(vel.item1Single.x[i]);
				}
			}
        }

		[Benchmark]
		public void AddSystem()
		{
			container.GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array> e);
			system.Update(ref e);
        }
	}
		*/
}