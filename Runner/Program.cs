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
            //position.x = Random.Shared.Next(0, 100);
            //position.x = MathF.Sqrt(position.x);
        }

		public void Update(ref Position.Vectorized position)
		{
            position.x = Vector256.Sqrt(position.x);
		}
	}

	interface ISystem<TType, TVector, TSingle>
		where TType : unmanaged, IComponent<TVector, TSingle>
		where TVector : unmanaged
		where TSingle : unmanaged
	{
		void Update(ref TVector vector);

		void Update<TEnum>(ref ArchTypeEnumerable<TEnum, TType, TVector, TSingle> loop)
			where TEnum : unmanaged, IArchTypeEnumerable<TEnum, TType, TVector, TSingle>;
	}

	partial class PositionSystem : ISystem<Position, Position.Vectorized, Position.Array>
	{
		public void Update<TEnum>(ref ArchTypeEnumerable<TEnum, Position, Position.Vectorized, Position.Array> loop)
			where TEnum : unmanaged, IArchTypeEnumerable<TEnum, Position, Position.Vectorized, Position.Array>
		{
			var en = loop.GetEnumerator();
			while (en.MoveNext())
			{
				var item = en.Current;
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Update(new(ref item.item1s.x[i], ref item.item1s.y[i], ref item.item1s.z[i]));
				}
				Update(ref item.item1);
			}
		}
	}

	partial struct TArchType :
		IArchType<Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array>,
		IArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>,
		IArchTypeEnumerable<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>
	{
		public Position.Vectorized position;
		public Velocity.Vectorized velocity;

		static ref Position.Array IArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>.GetSingle(ref TArchType arch)
		{
			return ref Unsafe.As<Position.Vectorized, Position.Array>(ref arch.position);
		}

		static ref Position.Vectorized IArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>.GetVec(ref TArchType arch)
		{
			return ref arch.position;
		}

		static ref Velocity.Array IArchTypeEnumerable<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>.GetSingle(ref TArchType arch)
		{
			return ref Unsafe.As<Velocity.Vectorized, Velocity.Array>(ref arch.velocity);
		}

		static ref Velocity.Vectorized IArchTypeEnumerable<TArchType, Velocity, Velocity.Vectorized, Velocity.Array>.GetVec(ref TArchType arch)
		{
			return ref arch.velocity;
		}
	}

	partial struct Position : IComponent<Position.Vectorized, Position.Array>
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
	}

	partial struct Velocity : IComponent<Velocity.Vectorized, Velocity.Array>
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
	}

	internal class Program
	{
		static void Main(string[] args)
		{
#if RELEASE
			BenchmarkRunner.Run<PerfTests>();
			return;
#endif

			Vector256<float> vf = Vector256.Create(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);

			Position.Vectorized t = new();
			ref Position.Array ta = ref Unsafe.As<Position.Vectorized, Position.Array>(ref t);

            ContainerManager<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> container = new(2);

			PositionSystem system = new();
			var e = new ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>(container.container1.AsSpan());
			system.Update(ref e);

			foreach (var item in e)
			{
				for (int i = 0; i < Position.Array.Size; i++)
				{
					Console.WriteLine($"Actual: {item.item1s.x[i]}");
				}
            }
		}
	}

	[SimpleJob(RuntimeMoniker.Net80)]
	[MemoryDiagnoser]
	public class PerfTests
	{
		ContainerManager<TArchType, Position, Position.Vectorized, Position.Array, Velocity, Velocity.Vectorized, Velocity.Array> container;
		float val;
		PositionSystem system = new();

		[GlobalSetup]
		public void Setup()
		{
			container = new(1_000_000);
		}

		//[Benchmark(Baseline = true)]
		public void Baseline()
		{
			for (int i = 0; i < 1_000_000 * 8; i++)
			{
				val = Random.Shared.Next(0, 100);
				val = MathF.Sqrt(val);
			}
		}

		//[Benchmark]
		public void AddManual()
		{
			foreach (var vel in new ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>(container.container1.AsSpan()))
			{
				for (int i = 0; i < Position.Array.Size; i++)
				{
					vel.item1s.x[i] = Random.Shared.Next(0, 100);
					vel.item1s.x[i] = MathF.Sqrt(vel.item1s.x[i]);
				}
			}
        }

		[Benchmark]
		public void AddSystem()
		{
			var e = new ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>(container.container1.AsSpan());
			system.Update(ref e);
        }

		[Benchmark]
		public void AddSystemParalell()
		{
			var span = container.container1.AsSpan();
			int chunk = span.Length / 4;
			Parallel.For(0, 4, x =>
			{
				var s = container.container1.AsSpan().Slice(x * chunk, chunk);

				var e = new ArchTypeEnumerable<TArchType, Position, Position.Vectorized, Position.Array>(s);
				system.Update(ref e);
			});
        }
	}
}