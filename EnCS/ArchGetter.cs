using System.Runtime.CompilerServices;

namespace EnCS
{
	public static class ArchGetter<TArch, TSingle>
        where TArch : IArchType<TArch, TSingle>, allows ref struct
		where TSingle : allows ref struct
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TSingle Get(ref TArch slice)
		{
			return ref TArch.Get(ref slice);
        }
    }
}
