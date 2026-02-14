using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EnCS
{

	public static partial class Looper<TVec, TSingle>
        where TVec : allows ref struct
        where TSingle : allows ref struct
    {
        public static void LoopIndexed<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
        {
            var @enum = EnumeratorCreator<TVec, TSingle>.CreateSequential(ref container1);
			@enum.Reset();
			@enum.MoveNext();

			do
			{
				updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray);
			}
			while (@enum.MoveNextArray());
		}

        public static void LoopIndexed<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContext context)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TContext : allows ref struct
		{
            var @enum = EnumeratorCreator<TVec, TSingle>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

            do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray, ref context);
            }
            while (@enum.MoveNextArray());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2>(ref TContainer1 enum1, TUpdater1 updater1, TUpdater2 updater2)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle>, allows ref struct
            => Looper<TVec, TSingle, TVec, TSingle>.LoopIndexed(ref enum1, updater1, ref enum1, updater2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TContext>(ref TContainer1 enum1, TUpdater1 updater1, TUpdater2 updater2, ref TContext context)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
			=> Looper<TVec, TSingle, TVec, TSingle>.LoopIndexed(ref enum1, updater1, ref enum1, updater2, ref context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater3 : ISystemUpdater<TVec, TSingle>, allows ref struct
            => Looper<TVec, TSingle, TVec, TSingle, TVec, TSingle>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TContext>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, ref TContext context)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater3 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
			=> Looper<TVec, TSingle, TVec, TSingle, TVec, TSingle>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TUpdater4>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, TUpdater4 updater4)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater3 : ISystemUpdater<TVec, TSingle>, allows ref struct
            where TUpdater4 : ISystemUpdater<TVec, TSingle>, allows ref struct
            => IndexedLooper<TVec, TSingle, TVec, TSingle, TVec, TSingle, TVec, TSingle>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref container1, updater4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TUpdater4, TContext>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, TUpdater4 updater4, ref TContext context)
            where TContainer1 : IIndexedContainer<TVec, TSingle>
            where TUpdater1 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater3 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
            where TUpdater4 : ISystemUpdater<TVec, TSingle, TContext>, allows ref struct
			where TContext : allows ref struct
			=> IndexedLooper<TVec, TSingle, TVec, TSingle, TVec, TSingle, TVec, TSingle>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref container1, updater4, ref context);
    }

    public static partial class Looper<T1Vec, T1Single, T2Vec, T2Single>
        where T1Vec : allows ref struct
        where T1Single : allows ref struct
		where T2Vec : allows ref struct
		where T2Single : allows ref struct
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single>, allows ref struct
        {
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

			do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray);

                Looper<T2Vec, T2Single>.LoopIndexed(ref container2, updater2);
            }
            while (@enum.MoveNextArray());
		}

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContext context)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single, TContext>, allows ref struct
			where TContext : allows ref struct
		{
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

			do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray, ref context);

                Looper<T2Vec, T2Single>.LoopIndexed(ref container2, updater2, ref context);
            }
            while (@enum.MoveNextArray());
        }
    }

    public static partial class Looper<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single>
        where T1Vec : allows ref struct
		where T1Single : allows ref struct
		where T2Vec : allows ref struct
		where T2Single : allows ref struct
		where T3Vec : allows ref struct
		where T3Single : allows ref struct
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single>, allows ref struct
            where TContainer3 : IIndexedContainer<T3Vec, T3Single>
            where TUpdater3 : ISystemUpdater<T3Vec, T3Single>, allows ref struct
        {
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

			do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray);

                Looper<T2Vec, T2Single, T3Vec, T3Single>.LoopIndexed(ref container2, updater2, ref container3, updater3);
            }
            while (@enum.MoveNextArray());
		}

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContext context)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single, TContext>, allows ref struct
            where TContainer3 : IIndexedContainer<T3Vec, T3Single>
            where TUpdater3 : ISystemUpdater<T3Vec, T3Single, TContext>, allows ref struct
			where TContext : allows ref struct
		{
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

            do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray, ref context);

                Looper<T2Vec, T2Single, T3Vec, T3Single>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref context);
            }
            while (@enum.MoveNextArray());
		}
    }

    public static partial class IndexedLooper<T1Vec, T1Single, T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single>
        where T1Vec : allows ref struct
		where T1Single : allows ref struct
		where T2Vec : allows ref struct
		where T2Single : allows ref struct
		where T3Vec : allows ref struct
		where T3Single : allows ref struct
		where T4Vec : allows ref struct
		where T4Single : allows ref struct
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContainer4, TUpdater4>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContainer4 container4, TUpdater4 updater4)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single>, allows ref struct
            where TContainer3 : IIndexedContainer<T3Vec, T3Single>
            where TUpdater3 : ISystemUpdater<T3Vec, T3Single>, allows ref struct
            where TContainer4 : IIndexedContainer<T4Vec, T4Single>
            where TUpdater4 : ISystemUpdater<T4Vec, T4Single>, allows ref struct
        {
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

            do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray);
                Looper<T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref container4, updater4);
            }
            while (@enum.MoveNextArray());
		}

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContainer4, TUpdater4, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContainer4 container4, TUpdater4 updater4, ref TContext context)
            where TContainer1 : IIndexedContainer<T1Vec, T1Single>
            where TUpdater1 : ISystemUpdater<T1Vec, T1Single, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<T2Vec, T2Single>
            where TUpdater2 : ISystemUpdater<T2Vec, T2Single, TContext>, allows ref struct
            where TContainer3 : IIndexedContainer<T3Vec, T3Single>
            where TUpdater3 : ISystemUpdater<T3Vec, T3Single, TContext>, allows ref struct
            where TContainer4 : IIndexedContainer<T4Vec, T4Single>
            where TUpdater4 : ISystemUpdater<T4Vec, T4Single, TContext>, allows ref struct
			where TContext : allows ref struct
		{
            var @enum = EnumeratorCreator<T1Vec, T1Single>.CreateSequential(ref container1);
            @enum.Reset();
            @enum.MoveNext();

			do
            {
                updater1.Invoke(int.Min(8, @enum.Remaining), @enum.CurrentVec, @enum.CurrentArray, ref context);
                Looper<T2Vec, T2Single, T3Vec, T3Single, T4Vec, T4Single>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref container4, updater4, ref context);
            }
            while (@enum.MoveNextArray());
		}
    }
}
