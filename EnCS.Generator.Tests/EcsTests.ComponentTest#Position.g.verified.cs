//HintName: Position.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public ref partial struct Position
	{
		public Position()
		{
			throw new NotImplementedException("Position should be created with Comp struct, not directly.");
		}

		public Position(ref float x, ref float y, ref float z)
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
			public ref Vector256<float> x;
			public ref Vector256<float> y;
			public ref Vector256<float> z;
		}

		public struct Comp
		{
			public float x;
			public float y;
			public float z;

			public Comp(float x, float y, float z)
			{
				this.x = x;
				this.y = y;
				this.z = z;
			}
		}

		public struct Memory
		{
			public Memory<float> x;
			public Memory<float> y;
			public Memory<float> z;

			public Memory(int length)
			{
				this.x = new float[length];
				this.y = new float[length];
				this.z = new float[length];
			}

			public Position.Vectorized GetVec(int idx)
			{
				idx /= 8;

				return new Position.Vectorized
				{
					 x = ref MemoryMarshal.AsRef<Vector256<float>>(this.x.Span.Slice(idx, 8)),
					 y = ref MemoryMarshal.AsRef<Vector256<float>>(this.y.Span.Slice(idx, 8)),
					 z = ref MemoryMarshal.AsRef<Vector256<float>>(this.z.Span.Slice(idx, 8)),//
				};
			}

			public Position GetSingle(int idx)
			{
				return new Position(
					ref this.x.Span[idx],
					ref this.y.Span[idx],
					ref this.z.Span[idx],
				);
			}

			public Position.Span AsSpan()
			{
				return new Position.Span(in this);
			}
		}

		public ref struct Span
		{
			public Span<float> x;
			public Span<float> y;
			public Span<float> z;

			public Span(ref readonly Memory<Position.Memory> memory)
			{
				this.x = memory.Span.x;
				this.y = memory.Span.y;
				this.z = memory.Span.z;
			}
		}
	}
}