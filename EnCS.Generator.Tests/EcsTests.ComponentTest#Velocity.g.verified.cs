//HintName: Velocity.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
				idx *= 8;

				return new Velocity.Vectorized
				{
					 x = ref Unsafe.As<int, Vector256<int>>(ref this.x.Span[idx]),
					 y = ref Unsafe.As<int, Vector256<int>>(ref this.y.Span[idx]),
					 z = ref Unsafe.As<int, Vector256<int>>(ref this.z.Span[idx]),//
				};
			}

			public Velocity GetSingle(int idx)
			{
				return new Velocity
				(
					ref this.x.Span[idx], 
					ref this.y.Span[idx], 
					ref this.z.Span[idx]
				);
			}
		}
	}
}