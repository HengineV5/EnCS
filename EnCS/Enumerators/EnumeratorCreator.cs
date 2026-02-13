using System.Runtime.CompilerServices;

namespace EnCS
{
	public static class EnumeratorCreator<TVec, TSingle>
        where TVec : allows ref struct
        where TSingle : allows ref struct
	{
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SequentialEnumerator<TContainer, TVec, TSingle> CreateSequential<TContainer>(ref TContainer container)
            where TContainer : IIndexedContainer<TVec, TSingle>
        {
			return new SequentialEnumerator<TContainer, TVec, TSingle>(ref container);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MappedEnumerator<TContainer, TVec, TSingle> CreateMapped<TContainer>(ref TContainer container, ReadOnlySpan<int> map)
            where TContainer : IIndexedContainer<TVec, TSingle>
        {
            return new MappedEnumerator<TContainer, TVec, TSingle>(ref container, map);
        }
	}
}
