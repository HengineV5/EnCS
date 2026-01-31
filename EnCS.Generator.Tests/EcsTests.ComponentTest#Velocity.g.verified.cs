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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Velocity FromArray(ref Array array, int idx)
		{
			return new Velocity(ref array.x[idx], ref array.y[idx], ref array.z[idx]);
		}

		public struct Vectorized
		{
			public Vector256<int> x;
			public Vector256<int> y;
			public Vector256<int> z;
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

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public struct Array
		{
			public const int Size = 8;

			public FixedBuffer8<int> x;
			public FixedBuffer8<int> y;
			public FixedBuffer8<int> z;
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


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Vectorized GetVec<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Vectorized, Array>
		{
			return ref TArch.GetVec(ref arch);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref Array GetSingle<TArch>(ref TArch arch) where TArch : unmanaged, IArchType<TArch, Vectorized, Array>
		{
			return ref TArch.GetSingle(ref arch);
		}
	}
}