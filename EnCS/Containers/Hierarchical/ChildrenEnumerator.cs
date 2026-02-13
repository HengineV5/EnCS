namespace EnCS
{
	public ref struct ChildrenEnumerator<TPtr>
		where TPtr : allows ref struct
	{
		public readonly ArchRef<TPtr> Current => new ArchRef<TPtr>(currentIdx);

		Range range;
		int currentIdx;

		public ChildrenEnumerator(Range range)
		{
			this.range = range;
			this.currentIdx = range.Start.Value - 1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			currentIdx++;

			if (currentIdx >= range.End.Value)
				return false;

			return true;
		}

		public void Reset()
		{
			currentIdx = range.Start.Value - 1;
		}
	}
}
