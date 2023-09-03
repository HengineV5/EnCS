using System;

namespace EnCS
{
	public interface IComponent<TComp, TVec, TSingle>
		where TComp : unmanaged, IComponent<TComp, TVec, TSingle>
		where TVec : unmanaged
		where TSingle : unmanaged
	{
		static abstract ref TVec GetVec<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>;

		static abstract ref TSingle GetSingle<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>;
	}

	public interface IComponentNew<TVec>
		where TVec : unmanaged
	{
		static abstract ref TVec GetVec<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchTypeNew<TArch, TVec>;
	}

	public interface IComponentNew<TComp, TVec, T1> : IComponentNew<TVec>
		where TComp : unmanaged, IComponentNew<TComp, TVec, T1>
		where TVec : unmanaged
		where T1 : unmanaged
	{
		public ref struct Single
		{
			public ref T1 item1;

			public Single(T1 item1)
			{
				this.item1 = item1;
			}
		}

		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<T1> item;
		}
	}

	public interface IComponentNew<TComp, TVec, T1, T2> : IComponentNew<TVec>
		where TComp : unmanaged, IComponentNew<TComp, TVec, T1, T2>
		where TVec : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
	{
		public ref struct Single
		{
			public ref T1 item1;
			public ref T2 item2;

			public Single(T1 item1, T2 item2)
			{
				this.item1 = item1;
				this.item2 = item2;
			}
		}

		public struct Array
		{
			public const int Size = 8;

			public FixedArray8<T1> item1;
			public FixedArray8<T2> item2;
		}
	}
}
