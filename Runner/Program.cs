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

	//[ResourceManager]
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

        public ref struct Slice : IArchTypeSlice<Slice, Vectorized, Array>
        {
            public ref Vectorized item1Vec;
            public ref Array item1Single;

            public Slice(ref Vectorized item1Vec, ref Array item1Single)
            {
                this.item1Vec = ref item1Vec;
                this.item1Single = ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Slice Create(ref Vectorized item1Vec, ref Array item1Single)
            {
                return new(ref item1Vec, ref item1Single);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Array GetSingle()
			{
				return ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Vectorized GetVec()
			{
				return ref item1Vec;
            }
		}
    }

	[Component]
	ref partial struct Position : IUnslicer<Position.Slice, Position.Vectorized, Position.Array>
    {
		public ref float x;
		public ref float y;
		public ref float z;

		public static ref Array GetSingle(ref Slice slice)
		{
			return ref slice.item1Single;
        }

		public static ref Vectorized GetVec(ref Slice slice)
		{
			return ref slice.item1Vec;
        }

		public ref struct Slice : IArchTypeSlice<Slice, Vectorized, Array>
        {
            public ref Vectorized item1Vec;
            public ref Array item1Single;

            public Slice(ref Vectorized item1Vec, ref Array item1Single)
            {
                this.item1Vec = ref item1Vec;
                this.item1Single = ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Array GetSingle()
            {
                return ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Vectorized GetVec()
            {
                return ref item1Vec;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Slice Create(ref Vectorized item1Vec, ref Array item1Single)
            {
                return new(ref item1Vec, ref item1Single);
            }
        }

        public class Slicer<TArch> : IArchSlicer<Slice, TArch>
			where TArch : unmanaged, IArchType<TArch, Position, Vectorized, Array>
        {
            public static Slice Slice(ref TArch arch)
				=> Slicer<Slice, TArch, Position, Vectorized, Array>.Slice(ref arch);
        }
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

        public ref struct Slice : IArchTypeSlice<Slice, Vectorized, Array>
        {
            public ref Vectorized item1Vec;
            public ref Array item1Single;

            public Slice(ref Vectorized item1Vec, ref Array item1Single)
            {
                this.item1Vec = ref item1Vec;
                this.item1Single = ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Slice Create(ref Vectorized item1Vec, ref Array item1Single)
            {
                return new(ref item1Vec, ref item1Single);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Array GetSingle()
            {
                return ref item1Single;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref Vectorized GetVec()
            {
                return ref item1Vec;
            }
        }

        public class Slicer<TArch> : IArchSlicer<Slice, TArch>
            where TArch : unmanaged, IArchType<TArch, Velocity, Vectorized, Array>
        {
            public static Slice Slice(ref TArch arch)
                => Slicer<Slice, TArch, Velocity, Vectorized, Array>.Slice(ref arch);
        }
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

	//[System]
	//[UsingResource<MeshResourceManager>]
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

        public ref struct SystemUpdater_0<TSlice> : ISystemUpdater<SystemUpdater_0<TSlice>, TSlice>
            where TSlice : IArchTypeSlice<TSlice, Position.Vectorized, Position.Array>, allows ref struct
        {
            PrintSystem system;

            public SystemUpdater_0(PrintSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, TSlice slice)
            {
				ref Position.Vectorized vec = ref slice.GetVec();
				ref Position.Array single = ref slice.GetSingle();

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

        public ref struct SystemUpdater_0<TSlice> : ISystemUpdater<SystemUpdater_0<TSlice>, TSlice>
            where TSlice : IArchTypeSlice<TSlice, Position.Vectorized, Position.Array>, IArchTypeSlice<TSlice, Velocity.Vectorized, Velocity.Array>, allows ref struct
        {
            PrintSystem_2 system;

            public SystemUpdater_0(PrintSystem_2 system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, TSlice slice)
            {
                ref Position.Vectorized vec1 = ref SliceGetter<TSlice, Position.Vectorized, Position.Array>.GetVec(ref slice);
                ref Position.Array single1 = ref SliceGetter<TSlice, Position.Vectorized, Position.Array>.GetSingle(ref slice);
                ref Velocity.Vectorized vec2 = ref SliceGetter<TSlice, Velocity.Vectorized, Velocity.Array>.GetVec(ref slice);
                ref Velocity.Array single2 = ref SliceGetter<TSlice, Velocity.Vectorized, Velocity.Array>.GetSingle(ref slice);

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

        public ref struct SystemUpdater_0<TSlice> : ISystemUpdater<SystemUpdater_0<TSlice>, TSlice>
            where TSlice : IArchTypeSlice<TSlice, Velocity.Vectorized, Velocity.Array>, allows ref struct
        {
            LayerSystem system;

            public SystemUpdater_0(LayerSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, TSlice slice)
            {
                ref Velocity.Vectorized vec = ref slice.GetVec();
                ref Velocity.Array single = ref slice.GetSingle();

                for (int i = 0; i < remaining; i++)
                {
                    Velocity comp = Velocity.FromArray(ref single, i);
                    system.Update1(ref comp);
                }
            }
		}

        public ref struct SystemUpdater_1<TSlice> : ISystemUpdater<SystemUpdater_1<TSlice>, TSlice>
            where TSlice : IArchTypeSlice<TSlice, Position.Vectorized, Position.Array>, allows ref struct
        {
            LayerSystem system;

            public SystemUpdater_1(LayerSystem system)
            {
                this.system = system;
            }

            public void Invoke(nint remaining, TSlice slice)
            {
                ref Position.Vectorized vec = ref slice.GetVec();
                ref Position.Array single = ref slice.GetSingle();

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
					//x.ResourceManager<MeshResourceManager>();
				})
				.Build<Ecs>();
			/*
			*/

			/*
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
			*/

			ArchTypeContainer<Wall.Vectorized, Wall> containerWall = new();
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

			Wall wall = containerWall.Get(r);
			wall.Position.Set(new Vector3(5, 0, 0));

            //ComponentEnumerable<Position, Position.Vectorized, Position.Array> testEnumerable = new();
            //var s = testEnumerable.GetEnumerator(containerWall.AsSpan(), (int)containerWall.Entities);

            //SequentialEnumerator<ArchTypeSlice<Position.Vectorized, Position>, ArchTypeContainer<Wall.Vectorized, Wall>, Wall.Vectorized, > s = new(containerWall);

            //SequentialEnumerator<ArchTypeSlice<Position.Vectorized, Position.Array>, ArchTypeContainer<Wall.Vectorized, Wall>, Wall.Vectorized, Slicer<Wall.Vectorized, Position, Position.Vectorized, Position.Array>> s = new(ref containerWall);
            var posEnum = EnumeratorCreator<Wall.Vectorized, Position.Slice, Position.Slicer<Wall.Vectorized>>.CreateSequential(ref containerWall);
			var velEnum = EnumeratorCreator<Wall.Vectorized, Velocity.Slice, Velocity.Slicer<Wall.Vectorized>>.CreateSequential(ref containerWall);

            PrintSystem printSystem = new();
			PrintSystem.SystemUpdater_0<Position.Slice> positionUpdater = new(printSystem);

            LayerSystem layerSystem = new();
            LayerSystem.SystemUpdater_0<Velocity.Slice> layerUpdater_0 = new(layerSystem);
            LayerSystem.SystemUpdater_1<Position.Slice> layerUpdater_1 = new(layerSystem);

            Looper<Position.Slice>.Loop(ref posEnum, positionUpdater);
            Looper<Velocity.Slice, Position.Slice>.Loop(ref velEnum, layerUpdater_0, ref posEnum, layerUpdater_1);
			/*
			*/
        }

		static void LoopGeneric<T, TSystem0>(Ecs ecs, TSystem0 system)
			where T : IWorld<Ecs, TSystem0>, allows ref struct
			where TSystem0 : class
		{
			T.Loop(ecs, system);
		}

		static void PositionAction(Position pos)
		{
        }

		static void PositionActionVec(ref Position.Vectorized pos)
		{
        }

		static void VelocityAction(Velocity vel)
		{
        }
    }

    public ref struct Slicer<TSlice, TArch, TComp, TVec, TSingle> : IArchSlicer<TSlice, TArch>
        where TSlice : IArchTypeSlice<TSlice, TVec, TSingle>, allows ref struct
        where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>
        where TComp : IComponent<TComp, TVec, TSingle>, allows ref struct
        where TVec : unmanaged
        where TSingle : unmanaged
    {
		public static TSlice Slice(ref TArch arch)
        {
            return TSlice.Create(ref TComp.GetVec(ref arch), ref TComp.GetSingle(ref arch));
        }
    }

	public interface ISystemUpdater<TSelf, TSlice>
		where TSelf : ISystemUpdater<TSelf, TSlice>, allows ref struct
        where TSlice : allows ref struct
    {
		void Invoke(nint remaining, TSlice slice);
    }

    public delegate void LoopAction<TVec>(ref TVec vec) where TVec : unmanaged;

    static class Looper<TSlice1>
        where TSlice1 : allows ref struct
    {
        public static void Loop<TEnumerator1, TUpdater1>(ref TEnumerator1 enum1, TUpdater1 updater1)
            where TEnumerator1 : IArchEnumerator<TEnumerator1, TSlice1>, allows ref struct
            where TUpdater1 : ISystemUpdater<TUpdater1, TSlice1>, allows ref struct
        {
            enum1.Reset();
            while (enum1.MoveNext())
            {
				updater1.Invoke(enum1.Remaining, enum1.Current);
            }
        }
    }

    static class Looper<TSlice1, TSlice2>
        where TSlice1 : allows ref struct
        where TSlice2 : allows ref struct
    {
        public static void Loop<TEnumerator1, TUpdater1, TEnumerator2, TUpdater2>(ref TEnumerator1 enum1, TUpdater1 updater1, ref TEnumerator2 enum2, TUpdater2 updater2)
            where TEnumerator1 : IArchEnumerator<TEnumerator1, TSlice1>, allows ref struct
            where TUpdater1 : ISystemUpdater<TUpdater1, TSlice1>, allows ref struct
            where TEnumerator2 : IArchEnumerator<TEnumerator2, TSlice2>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TSlice2>, allows ref struct
        {
            enum1.Reset();
            while (enum1.MoveNext())
            {
                updater1.Invoke(enum1.Remaining, enum1.Current);

                Looper<TSlice2>.Loop(ref enum2, updater2);
            }
        }
    }

    /*
	static class Looper<TSlice1>
        where TSlice1 : allows ref struct
    {
        public static void Loop<TEnumerator1>(ref TEnumerator1 enum1, Action<nint, TSlice1> layer1Action)
            where TEnumerator1 : IArchEnumerator<TEnumerator1, TSlice1>, allows ref struct
        {
            enum1.Reset();
            while (enum1.MoveNext())
            {
                layer1Action(enum1.Remaining, enum1.Current);
            }
        }
    }

    static class Looper<TSlice1, TSlice2>
        where TSlice1 : allows ref struct
        where TSlice2 : allows ref struct
    {
        public static void Loop<TEnumerator1, TEnumerator2>(ref TEnumerator1 enum1, ref TEnumerator2 enum2, Action<nint, TSlice1> layer1Action, Action<nint, TSlice2> layer2Action)
            where TEnumerator1 : IArchEnumerator<TEnumerator1, TSlice1>, allows ref struct
            where TEnumerator2 : IArchEnumerator<TEnumerator2, TSlice2>, allows ref struct
        {
            enum1.Reset();
            while (enum1.MoveNext())
            {
                layer1Action(enum1.Remaining, enum1.Current);

                Looper<TSlice2>.Loop(ref enum2, layer2Action);
            }
        }
    }
	*/

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
}