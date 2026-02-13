using System.Runtime.CompilerServices;
using UtilLib.Memory;

namespace EnCS
{
	public interface IIndexedContainer<TVec, TSingle> : IContainer
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		TSingle GetSingle(nint idx);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		TSingle GetSingle(ref readonly ArchRef<TSingle> ptr);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		FixedRefBuffer8<TSingle> GetSingleArray(nint idx);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		TVec GetVec(nint idx);
	}
}
