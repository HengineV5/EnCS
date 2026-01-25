
using System.Runtime.CompilerServices;

namespace EnCS
{
	[InlineArray(2)]
	public struct FixedArray2<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(4)]
	public struct FixedArray4<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(8)]
	public struct FixedArray8<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(16)]
	public struct FixedArray16<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(32)]
	public struct FixedArray32<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}

	[InlineArray(64)]
	public struct FixedArray64<T>
	{
		T _element0;

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public ref T GetPinnableReference()
		{
			return ref Unsafe.AsRef(ref _element0);
		}
	}
}
