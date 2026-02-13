using System.Runtime.CompilerServices;
using UtilLib.Memory;

namespace EnCS
{
	public interface IArchMemory<TSelf, TVec, TSingle>
		where TSelf : IArchMemory<TSelf, TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static abstract TSelf Create(int size);

		TSingle GetSingle(nint index);

		FixedRefBuffer8<TSingle> GetSingleArray(nint index);

		TVec GetVec(nint index);
	}
}
