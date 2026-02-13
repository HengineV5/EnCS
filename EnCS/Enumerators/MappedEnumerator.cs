namespace EnCS
{
	public ref struct MappedEnumerator<TContainer, TVec, TSingle> : IArchEnumerator<TSingle>
		where TContainer : IIndexedContainer<TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		public TSingle Current => container.GetSingle(map[index]);

		public nint Remaining => map.Length - index;

		ref TContainer container;
		int index;
		ReadOnlySpan<int> map;

		public MappedEnumerator(ref TContainer container, ReadOnlySpan<int> map)
		{
			this.container = ref container;
			this.index = -1;
			this.map = map;
		}

		public bool MoveNext()
		{
			index++;

			if (index >= map.Length)
				return false;

			return true;
		}

		public void Reset()
		{
			index = -1;
		}
	}
}
