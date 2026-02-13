using UtilLib.Memory;

namespace EnCS
{
	public interface IArchEnumerator<TSingle>
		where TSingle : allows ref struct
	{
		TSingle Current { get; }

		nint Remaining { get; }

		bool MoveNext();

		void Reset();
	}

	public interface IArchEnumerator<TVec, TSingle> : IArchEnumerator<TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		TVec CurrentVec { get; } // Will only move every 8 moves

		FixedRefBuffer8<TSingle> CurrentArray { get; }
    }
}
