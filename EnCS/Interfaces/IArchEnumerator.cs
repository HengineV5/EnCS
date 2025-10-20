namespace EnCS
{
	public interface IArchEnumerator<TSelf, TArch>
		where TSelf : allows ref struct
		where TArch : unmanaged
    {
		ref TArch Current { get; }

		nint Remaining { get; }

		bool MoveNext();

		void Reset();
    }
}
