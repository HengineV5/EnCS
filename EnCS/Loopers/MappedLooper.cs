namespace EnCS
{
	public static partial class Looper<TArch1>
    {
		public static void LoopMapped<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1, scoped ReadOnlySpan<int> map)
			where TContainer1 : IIndexedContainer<TContainer1, TArch1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
		{
			var @enum = EnumeratorCreator<TArch1>.CreateMapped(ref container1, map);
			@enum.Reset();

			for (int i = 0; i < map.Length; i++)
			{
				@enum.MoveNext();

				updater1.Invoke(map[i] % 8, ref @enum.Current);
			}
		}

		public static void LoopMapped<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, scoped ReadOnlySpan<int> map, ref TContext context)
			where TContainer1 : IIndexedContainer<TContainer1, TArch1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
			where TContext : allows ref struct
		{
			var @enum = EnumeratorCreator<TArch1>.CreateMapped(ref container1, map);
			@enum.Reset();

			for (int i = 0; i < map.Length; i++)
			{
				@enum.MoveNext();

				updater1.Invoke(map[i] % 8, ref @enum.Current, ref context);
			}
		}
	}
}
