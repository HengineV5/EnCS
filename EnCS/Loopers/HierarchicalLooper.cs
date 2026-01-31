namespace EnCS
{
	public static partial class Looper<TArch1>
		where TArch1 : unmanaged
	{
		public static void LoopHierarchical<TContainer1, TUpdater1, TPtr1>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TPtr1> parentPtr)
			where TContainer1 : IHierarchicalContainer<TContainer1, TArch1, TPtr1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
			where TPtr1 : allows ref struct
		{
			var childrenEnum = container1.GetChildren(parentPtr);
			while (childrenEnum.MoveNext())
			{
				ref var child = ref container1.GetValue(childrenEnum.Current);

				updater1.Invoke(childrenEnum.Current.idx % 8, ref child);
				LoopHierarchical(ref container1, updater1, childrenEnum.Current);
			}
		}

		public static void LoopHierarchical<TContainer1, TUpdater1, TPtr1, TContext>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TPtr1> parentPtr, ref TContext context)
			where TContainer1 : IHierarchicalContainer<TContainer1, TArch1, TPtr1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
			where TPtr1 : allows ref struct
			where TContext : allows ref struct
		{
			var childrenEnum = container1.GetChildren(parentPtr);
			while (childrenEnum.MoveNext())
			{
				ref var child = ref container1.GetValue(childrenEnum.Current);
				updater1.Invoke(childrenEnum.Current.idx % 8, ref child, ref context);
				LoopHierarchical(ref container1, updater1, childrenEnum.Current, ref context);
			}
		}

		public static void LoopHierarchical<TContainer1, TUpdater1, TPtr1>(ref TContainer1 container1, TUpdater1 updater1)
			where TContainer1 : IHierarchicalContainer<TContainer1, TArch1, TPtr1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
			where TPtr1 : allows ref struct
		 => LoopHierarchical(ref container1, updater1, container1.GetRoot());

		public static void LoopHierarchical<TContainer1, TUpdater1, TPtr1, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContext context)
			where TContainer1 : IHierarchicalContainer<TContainer1, TArch1, TPtr1>
			where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
			where TPtr1 : allows ref struct
			where TContext : allows ref struct
		 => LoopHierarchical(ref container1, updater1, container1.GetRoot(), ref context);
	}
}
