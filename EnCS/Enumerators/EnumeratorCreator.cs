namespace EnCS
{
	public static class EnumeratorCreator<TArch>
        where TArch : unmanaged
    {
		public static SequentialEnumerator<TContainer, TArch> CreateSequential<TContainer>(ref TContainer container)
            where TContainer : IIndexedContainer<TContainer, TArch>
        {
			return new SequentialEnumerator<TContainer, TArch>(ref container);
        }
	}
}
