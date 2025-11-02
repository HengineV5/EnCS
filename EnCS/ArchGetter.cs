using System.Runtime.CompilerServices;

namespace EnCS
{
	public static class ArchGetter<TArch, TVec, TSingle>
        where TArch : unmanaged, IArchType<TArch, TVec, TSingle>
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
}
