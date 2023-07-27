using System.Runtime.CompilerServices;

namespace EnCS
{
	struct ItemGetter<TFrom, T1Get, T1GetVec, T1GetSingle>
		where TFrom : unmanaged, IArchTypeEnumerable<TFrom, T1Get, T1GetVec, T1GetSingle>
		where T1Get : unmanaged, IComponent<T1GetVec, T1GetSingle>
		where T1GetVec : unmanaged
		where T1GetSingle : unmanaged
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T1GetVec GetVec(ref TFrom archType)
		{
			return ref TFrom.GetVec(ref archType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T1GetSingle GetSingle(ref TFrom archType)
		{
			return ref TFrom.GetSingle(ref archType);
		}
	}

	public ref struct ArchTypeSlice<TArch, T1, T1Vec, T1Single>
		where TArch : unmanaged, IArchTypeEnumerable<TArch, T1, T1Vec, T1Single>
		where T1 : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref T1Vec item1;
		public ref T1Single item1s;

		public ArchTypeSlice(ref TArch archType)
		{
			item1 = ref ItemGetter<TArch, T1, T1Vec, T1Single>.GetVec(ref archType);
			item1s = ref ItemGetter<TArch, T1, T1Vec, T1Single>.GetSingle(ref archType);
		}
	}

	public ref struct ArchTypeSlice<TArch, T1, T1Vec, T1Single, T2, T2Vec, T2Single>
		where TArch : unmanaged, IArchTypeEnumerable<TArch, T1, T1Vec, T1Single>, IArchTypeEnumerable<TArch, T2, T2Vec, T2Single>
		where T1 : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2 : unmanaged, IComponent<T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref T1Vec item1;
		public ref T1Single item1s;

		public ref T2Vec item2;
		public ref T2Single item2s;

		public ArchTypeSlice(ref TArch archType)
		{
			item1 = ref ItemGetter<TArch, T1, T1Vec, T1Single>.GetVec(ref archType);
			item1s = ref ItemGetter<TArch, T1, T1Vec, T1Single>.GetSingle(ref archType);
			item2 = ref ItemGetter<TArch, T2, T2Vec, T2Single>.GetVec(ref archType);
			item2s = ref ItemGetter<TArch, T2, T2Vec, T2Single>.GetSingle(ref archType);
		}
	}
}
