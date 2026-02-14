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

		TVec GetVec(nint index);
	}

	public static class ArchMemoryExtensions
	{
		public static FixedRefBuffer8<TSingle> GetSingleArray<TSelf, TVec, TSingle>(this TSelf self, nint idx)
			where TSelf : IArchMemory<TSelf, TVec, TSingle>
			where TVec : allows ref struct
			where TSingle : allows ref struct
		{
			FixedRefBuffer8<TSingle> buffer = new();
			nint start = idx / 8;
			start *= 8;

			buffer.Get(0) = self.GetSingle(start + 0);
			buffer.Get(1) = self.GetSingle(start + 1);
			buffer.Get(2) = self.GetSingle(start + 2);
			buffer.Get(3) = self.GetSingle(start + 3);
			buffer.Get(4) = self.GetSingle(start + 4);
			buffer.Get(5) = self.GetSingle(start + 5);
			buffer.Get(6) = self.GetSingle(start + 6);
			buffer.Get(7) = self.GetSingle(start + 7);

			return buffer;
		}
	}
}
