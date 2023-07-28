using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using EnCS;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Runner
{
	public class ArchTypeAttribute<T1> : Attribute
	{

	}

	public class ArchTypeAttribute<T1, T2> : Attribute
	{

	}

	public class ComponentAttribute : Attribute
	{

	}

	public class SystemAttribute : Attribute
	{

	}

	[ArchType<Position, Velocity>]
	partial struct TArchType
	{
	}

	[ArchType<Position>]
	partial struct TArchType2
	{
	}

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

	interface ISystem<TComp, TVector, TSingle>
		where TComp : unmanaged, IComponent<TComp, TVector, TSingle>
		where TVector : unmanaged
		where TSingle : unmanaged
	{
		void Update(ref TVector vector);

		void Update<TArch1>(ref ArchTypeEnumerable<TArch1, TComp, TVector, TSingle> loop)
			where TArch1 : unmanaged, IArchType<TArch1, TComp, TVector, TSingle>;
	}

	partial class PositionSystem : ISystem<Position, Position.Vectorized, Position.Array>
	{
		public void Update<TArch1>(ref ArchTypeEnumerable<TArch1, Position, Position.Vectorized, Position.Array> loop)
			where TArch1 : unmanaged, IArchType<TArch1, Position, Position.Vectorized, Position.Array>
		{
			var en = loop.GetEnumerator();
			while (en.MoveNext())
			{
				var item = en.Current;
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Update(new(ref item.item1Single.x[i], ref item.item1Single.y[i], ref item.item1Single.z[i]));
				}
				Update(ref item.item1Vec);
			}
		}

		public void Update<TArch1, TArch2>(ref ArchTypeEnumerable<TArch1, TArch2, Position, Position.Vectorized, Position.Array> loop)
			where TArch1 : unmanaged, IArchType<TArch1, Position, Position.Vectorized, Position.Array>
			where TArch2 : unmanaged, IArchType<TArch2, Position, Position.Vectorized, Position.Array>
		{
			var en = loop.GetEnumerator();
			while (en.MoveNext())
			{
				var item = en.Current;
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Update(new(ref item.item1Single.x[i], ref item.item1Single.y[i], ref item.item1Single.z[i]));
				}
				Update(ref item.item1Vec);
			}
		}
	}

	partial struct TArchType :
		IArchType<TArchType, Position, Position.Vectorized, Position.Array>,
		IArchType<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>
	{
		public Position.Vectorized position;
		public Velocity.Vectorized velocity;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Position.Array IArchType<TArchType, Position, Position.Vectorized, Position.Array>.GetSingle(ref TArchType arch)
		{
			return ref Unsafe.As<Position.Vectorized, Position.Array>(ref arch.position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Position.Vectorized IArchType<TArchType, Position, Position.Vectorized, Position.Array>.GetVec(ref TArchType arch)
		{
			return ref arch.position;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Velocity.Array IArchType<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>.GetSingle(ref TArchType arch)
		{
			return ref Unsafe.As<Velocity.Vectorized, Velocity.Array>(ref arch.velocity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Velocity.Vectorized IArchType<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>.GetVec(ref TArchType arch)
		{
			return ref arch.velocity;
		}
	}

	partial struct TArchType2
		: IArchType<TArchType2, Position, Position.Vectorized, Position.Array>
	{
		public Position.Vectorized position;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Position.Array IArchType<TArchType2, Position, Position.Vectorized, Position.Array>.GetSingle(ref TArchType2 arch)
		{
			return ref Unsafe.As<Position.Vectorized, Position.Array>(ref arch.position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ref Position.Vectorized IArchType<TArchType2, Position, Position.Vectorized, Position.Array>.GetVec(ref TArchType2 arch)
		{
			return ref arch.position;
		}
	}

	partial struct Position : IComponent<Position, Position.Vectorized, Position.Array>
	{
		public struct Vectorized
		{
			public Vector256<float> x;
			public Vector256<int> y;
			public Vector256<int> z;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<float> x;
			public FixedArray8<int> y;
			public FixedArray8<int> z;
		}

		public ref struct Ref
		{
			public ref float x;
			public ref int y;
			public ref int z;

			public Ref(ref float x, ref int y, ref int z)
			{
				this.x = ref x;
				this.y = ref y;
				this.z = ref z;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Position, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Position, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}

	partial struct Velocity : IComponent<Velocity, Velocity.Vectorized, Velocity.Array>
	{
		public static uint Id => 0;

		public static short Size => sizeof(int) * 3;

		public struct Vectorized
		{
			public Vector256<int> x;
			public Vector256<int> y;
			public Vector256<int> z;
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<int> x;
			public FixedArray8<int> y;
			public FixedArray8<int> z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Velocity, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Velocity, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}

	struct EcsContainerManager
	{
		ArchTypeContainer<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> container1;
		ArchTypeContainer<TArchType2, Position, Position.Vectorized, Position.Array> container2;

        public EcsContainerManager()
        {
			container1 = new(10);
			container2 = new(10);
        }

		public EcsContainerManager(nuint size)
		{
			container1 = new(size);
			container2 = new(size);
		}

		public bool GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array> e)
		{
			e = new(container1.AsSpan());
			return true;
		}

		public bool GetEnumerable(out ArchTypeEnumerable<TArchType, Velocity, Velocity.Vectorized, Velocity.Array> e)
		{
			e = new(container1.AsSpan());
			return true;
		}

		public bool GetEnumerable(out ArchTypeEnumerable<TArchType, TArchType2, Position, Position.Vectorized, Position.Array> e)
		{
			e = new(container1.AsSpan(), container2.AsSpan());
			return true;
		}

		public bool GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> e)
		{
			e = new(container1.AsSpan());
			return true;
		}
	}

	internal class Program
	{
		static void Main(string[] args)
		{
#if RELEASE
			BenchmarkRunner.Run<PerfTests>();
			return;
#endif
			EcsContainerManager containerManager = new(2);

			PositionSystem system = new();
			containerManager.GetEnumerable(out ArchTypeEnumerable<TArchType, TArchType2, Position, Position.Vectorized, Position.Array> e);

			system.Update(ref e);

			containerManager.GetEnumerable(out ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> e2);

			foreach (var item in e2)
			{
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Console.WriteLine($"Actual: {item.item1Single.x[i]}");
				}
			}
		}
	}

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

		/*
		[Benchmark]
		public void AddSystemParalell()
		{
			var span = container.container1.AsSpan();
			int chunk = span.Length / 4;
			Parallel.For(0, 4, x =>
			{
				var s = container.container1.AsSpan().Slice(x * chunk, chunk);

				var e = new ComponentEnumerable<TArchType, Position, Position.Vectorized, Position.Array>(s);
				system.Update(ref e);
			});
        }
		*/
	}
}