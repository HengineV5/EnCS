namespace EnCS
{
	public static partial class Looper<TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		public static void LoopMapped<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1, scoped ReadOnlySpan<int> map)
			where TContainer1 : IIndexedContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
		{
			var @enum = EnumeratorCreator<TVec, TSingle>.CreateMapped(ref container1, map);
			@enum.Reset();

			for (int i = 0; i < map.Length; i++)
			{
				@enum.MoveNext();

				updater1.Invoke(@enum.Current);
			}
		}

		public static void LoopMapped<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, scoped ReadOnlySpan<int> map, ref TContext context)
			where TContainer1 : IIndexedContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
		{
			var @enum = EnumeratorCreator<TVec, TSingle>.CreateMapped(ref container1, map);
			@enum.Reset();

			for (int i = 0; i < map.Length; i++)
			{
				@enum.MoveNext();

				updater1.Invoke(@enum.Current, ref context);
			}
		}
	}
}
