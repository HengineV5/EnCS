//HintName: Velocity.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public ref partial struct Velocity
	{
		public Velocity()
		{
			throw new NotImplementedException("Velocity should be created with Comp struct, not directly.");
		}

		public Velocity(ref int x, ref int y, ref int z)
		{
			this.x = ref x;
			this.y = ref y;
			this.z = ref z;
		}

		public void Set(Comp c)
		{
			this.x = c.x;
			this.y = c.y;
			this.z = c.z;
		}

		public ref struct Vectorized
		{
			public ref Vector256<int> x;
			public ref Vector256<int> y;
			public ref Vector256<int> z;
		}

		public struct Comp
		{
			public int x;
			public int y;
			public int z;

			public Comp(int x, int y, int z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		public struct Memory
		{
			public Memory<int> x;
			public Memory<int> y;
			public Memory<int> z;

			public Memory(int length)
			{
				this.x = new int[length];
				this.y = new int[length];
				this.z = new int[length];
			}

			public Velocity.Vectorized GetVec(int idx)
			{
				idx /= 8;

				return new Velocity.Vectorized
				{
					 x = ref MemoryMarshal.AsRef<Vector256<int>>(this.x.Span.Slice(idx, 8)),
					 y = ref MemoryMarshal.AsRef<Vector256<int>>(this.y.Span.Slice(idx, 8)),
					 z = ref MemoryMarshal.AsRef<Vector256<int>>(this.z.Span.Slice(idx, 8)),//
				};
			}

			public Velocity GetSingle(int idx)
			{
				return new Velocity(
					ref this.x.Span[idx],
					ref this.y.Span[idx],
					ref this.z.Span[idx],
				);
			}

			public Velocity.Span AsSpan()
			{
				return new Velocity.Span(in this);
			}
		}

		public ref struct Span
		{
			public Span<int> x;
			public Span<int> y;
			public Span<int> z;

			public Span(ref readonly Memory<Velocity.Memory> memory)
			{
				this.x = memory.Span.x;
				this.y = memory.Span.y;
				this.z = memory.Span.z;
			}
		}
	}
}