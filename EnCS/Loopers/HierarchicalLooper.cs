namespace EnCS
{
	public static partial class Looper<TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		public static void LoopHierarchical<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TSingle> parentPtr)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
		{
			scoped var self = container1.GetSingle(in parentPtr);
			updater1.Invoke(self);

			LoopHierarchical_NotSelf(ref container1, updater1, parentPtr);
		}

		static void LoopHierarchical_NotSelf<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TSingle> parentPtr)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
		{
			var childrenEnum = container1.GetChildren(in parentPtr);
			while (childrenEnum.MoveNext())
			{
				scoped var child = container1.GetSingle(childrenEnum.Current);

				updater1.Invoke(child);
				LoopHierarchical_NotSelf(ref container1, updater1, childrenEnum.Current);
			}
		}

		public static void LoopHierarchical<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TSingle> parentPtr, ref TContext context)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
		{
			scoped var self = container1.GetSingle(in parentPtr);
			updater1.Invoke(self, ref context);

			LoopHierarchical_NotSelf(ref container1, updater1, parentPtr, ref context);
		}

		static void LoopHierarchical_NotSelf<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, in ArchRef<TSingle> parentPtr, ref TContext context)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
		{
			var childrenEnum = container1.GetChildren(in parentPtr);
			while (childrenEnum.MoveNext())
			{
				scoped var child = container1.GetSingle(childrenEnum.Current);
				updater1.Invoke(child, ref context);

				LoopHierarchical_NotSelf(ref container1, updater1, childrenEnum.Current, ref context);
			}
		}

		public static void LoopHierarchical<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
		 => LoopHierarchical(ref container1, updater1, container1.GetRoot());

		public static void LoopHierarchical<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContext context)
			where TContainer1 : IHierarchicalContainer<TVec, TSingle>
			where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
		 => LoopHierarchical(ref container1, updater1, container1.GetRoot(), ref context);
	}
}
