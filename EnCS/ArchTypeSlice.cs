using System.Runtime.CompilerServices;

namespace EnCS
{
	public static class ArchGetter<TArch, TComp, TVec, TSingle>
        where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>
		where TComp : IComponent<TComp, TVec, TSingle>, allows ref struct
        where TVec : unmanaged
        where TSingle : unmanaged
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TVec GetVec(ref TArch slice)
		{
			return ref TArch.GetVec(ref slice);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TSingle GetSingle(ref TArch slice)
        {
            return ref TArch.GetSingle(ref slice);
        }
    }

    public ref struct ArchTypeSlice<T1Vec, T1Single>
        where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Vec : unmanaged
		where T6Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ref T6Vec item6Vec;
		public ref T6Single item6Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single, ref T6Vec item6Vec, ref T6Single item6Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
			this.item6Vec = ref item6Vec;
			this.item6Single = ref item6Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Vec : unmanaged
		where T7Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ref T6Vec item6Vec;
		public ref T6Single item6Single;

		public ref T7Vec item7Vec;
		public ref T7Single item7Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single, ref T6Vec item6Vec, ref T6Single item6Single, ref T7Vec item7Vec, ref T7Single item7Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
			this.item6Vec = ref item6Vec;
			this.item6Single = ref item6Single;
			this.item7Vec = ref item7Vec;
			this.item7Single = ref item7Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Vec : unmanaged
		where T8Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ref T6Vec item6Vec;
		public ref T6Single item6Single;

		public ref T7Vec item7Vec;
		public ref T7Single item7Single;

		public ref T8Vec item8Vec;
		public ref T8Single item8Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single, ref T6Vec item6Vec, ref T6Single item6Single, ref T7Vec item7Vec, ref T7Single item7Single, ref T8Vec item8Vec, ref T8Single item8Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
			this.item6Vec = ref item6Vec;
			this.item6Single = ref item6Single;
			this.item7Vec = ref item7Vec;
			this.item7Single = ref item7Single;
			this.item8Vec = ref item8Vec;
			this.item8Single = ref item8Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Vec : unmanaged
		where T8Single : unmanaged
		where T9Vec : unmanaged
		where T9Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ref T6Vec item6Vec;
		public ref T6Single item6Single;

		public ref T7Vec item7Vec;
		public ref T7Single item7Single;

		public ref T8Vec item8Vec;
		public ref T8Single item8Single;

		public ref T9Vec item9Vec;
		public ref T9Single item9Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single, ref T6Vec item6Vec, ref T6Single item6Single, ref T7Vec item7Vec, ref T7Single item7Single, ref T8Vec item8Vec, ref T8Single item8Single, ref T9Vec item9Vec, ref T9Single item9Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
			this.item6Vec = ref item6Vec;
			this.item6Single = ref item6Single;
			this.item7Vec = ref item7Vec;
			this.item7Single = ref item7Single;
			this.item8Vec = ref item8Vec;
			this.item8Single = ref item8Single;
			this.item9Vec = ref item9Vec;
			this.item9Single = ref item9Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single, T5Vec, T5Single, T6Vec, T6Single, T7Vec, T7Single, T8Vec, T8Single, T9Vec, T9Single, T10Vec, T10Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
		where T3Vec : unmanaged
		where T3Single : unmanaged
		where T4Vec : unmanaged
		where T4Single : unmanaged
		where T5Vec : unmanaged
		where T5Single : unmanaged
		where T6Vec : unmanaged
		where T6Single : unmanaged
		where T7Vec : unmanaged
		where T7Single : unmanaged
		where T8Vec : unmanaged
		where T8Single : unmanaged
		where T9Vec : unmanaged
		where T9Single : unmanaged
		where T10Vec : unmanaged
		where T10Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ref T3Vec item3Vec;
		public ref T3Single item3Single;

		public ref T4Vec item4Vec;
		public ref T4Single item4Single;

		public ref T5Vec item5Vec;
		public ref T5Single item5Single;

		public ref T6Vec item6Vec;
		public ref T6Single item6Single;

		public ref T7Vec item7Vec;
		public ref T7Single item7Single;

		public ref T8Vec item8Vec;
		public ref T8Single item8Single;

		public ref T9Vec item9Vec;
		public ref T9Single item9Single;

		public ref T10Vec item10Vec;
		public ref T10Single item10Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single, ref T3Vec item3Vec, ref T3Single item3Single, ref T4Vec item4Vec, ref T4Single item4Single, ref T5Vec item5Vec, ref T5Single item5Single, ref T6Vec item6Vec, ref T6Single item6Single, ref T7Vec item7Vec, ref T7Single item7Single, ref T8Vec item8Vec, ref T8Single item8Single, ref T9Vec item9Vec, ref T9Single item9Single, ref T10Vec item10Vec, ref T10Single item10Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
			this.item3Vec = ref item3Vec;
			this.item3Single = ref item3Single;
			this.item4Vec = ref item4Vec;
			this.item4Single = ref item4Single;
			this.item5Vec = ref item5Vec;
			this.item5Single = ref item5Single;
			this.item6Vec = ref item6Vec;
			this.item6Single = ref item6Single;
			this.item7Vec = ref item7Vec;
			this.item7Single = ref item7Single;
			this.item8Vec = ref item8Vec;
			this.item8Single = ref item8Single;
			this.item9Vec = ref item9Vec;
			this.item9Single = ref item9Single;
			this.item10Vec = ref item10Vec;
			this.item10Single = ref item10Single;
		}
	}
}
