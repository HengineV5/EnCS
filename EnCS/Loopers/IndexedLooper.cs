using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EnCS
{
    public static partial class Looper<TArch1>
        where TArch1 : unmanaged
    {
        public static void LoopIndexed<TContainer1, TUpdater1>(ref TContainer1 container1, TUpdater1 updater1)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current);
            }
        }

        public static void LoopIndexed<TContainer1, TUpdater1, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current, ref context);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2>(ref TContainer1 enum1, TUpdater1 updater1, TUpdater2 updater2)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1>, allows ref struct
            => Looper<TArch1, TArch1>.LoopIndexed(ref enum1, updater1, ref enum1, updater2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TContext>(ref TContainer1 enum1, TUpdater1 updater1, TUpdater2 updater2, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1, TContext>, allows ref struct
            => Looper<TArch1, TArch1>.LoopIndexed(ref enum1, updater1, ref enum1, updater2, ref context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1>, allows ref struct
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch1>, allows ref struct
            => Looper<TArch1, TArch1, TArch1>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TContext>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1, TContext>, allows ref struct
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch1, TContext>, allows ref struct
            => Looper<TArch1, TArch1, TArch1>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref context);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TUpdater4>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, TUpdater4 updater4)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1>, allows ref struct
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch1>, allows ref struct
            where TUpdater4 : ISystemUpdater<TUpdater4, TArch1>, allows ref struct
            => IndexedLooper<TArch1, TArch1, TArch1, TArch1>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref container1, updater4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LoopIndexed<TContainer1, TUpdater1, TUpdater2, TUpdater3, TUpdater4, TContext>(ref TContainer1 container1, TUpdater1 updater1, TUpdater2 updater2, TUpdater3 updater3, TUpdater4 updater4, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch1, TContext>, allows ref struct
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch1, TContext>, allows ref struct
            where TUpdater4 : ISystemUpdater<TUpdater4, TArch1, TContext>, allows ref struct
            => IndexedLooper<TArch1, TArch1, TArch1, TArch1>.LoopIndexed(ref container1, updater1, ref container1, updater2, ref container1, updater3, ref container1, updater4, ref context);
    }

    public static partial class Looper<TArch1, TArch2>
        where TArch1 : unmanaged
        where TArch2 : unmanaged
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current);

                Looper<TArch2>.LoopIndexed(ref container2, updater2);
            }
        }

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2, TContext>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current, ref context);

                Looper<TArch2>.LoopIndexed(ref container2, updater2, ref context);
            }
        }
    }

    public static partial class Looper<TArch1, TArch2, TArch3>
        where TArch1 : unmanaged
        where TArch2 : unmanaged
        where TArch3 : unmanaged
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2>, allows ref struct
            where TContainer3 : IIndexedContainer<TContainer3, TArch3>
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch3>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current);

                Looper<TArch2, TArch3>.LoopIndexed(ref container2, updater2, ref container3, updater3);
            }
        }

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2, TContext>, allows ref struct
            where TContainer3 : IIndexedContainer<TContainer3, TArch3>
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch3, TContext>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current, ref context);

                Looper<TArch2, TArch3>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref context);
            }
        }
    }

    public static partial class IndexedLooper<TArch1, TArch2, TArch3, TArch4>
        where TArch1 : unmanaged
        where TArch2 : unmanaged
        where TArch3 : unmanaged
        where TArch4 : unmanaged
    {
        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContainer4, TUpdater4>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContainer4 container4, TUpdater4 updater4)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2>, allows ref struct
            where TContainer3 : IIndexedContainer<TContainer3, TArch3>
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch3>, allows ref struct
            where TContainer4 : IIndexedContainer<TContainer4, TArch4>
            where TUpdater4 : ISystemUpdater<TUpdater4, TArch4>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current);
                Looper<TArch2, TArch3, TArch4>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref container4, updater4);
            }
        }

        public static void LoopIndexed<TContainer1, TUpdater1, TContainer2, TUpdater2, TContainer3, TUpdater3, TContainer4, TUpdater4, TContext>(ref TContainer1 container1, TUpdater1 updater1, ref TContainer2 container2, TUpdater2 updater2, ref TContainer3 container3, TUpdater3 updater3, ref TContainer4 container4, TUpdater4 updater4, ref TContext context)
            where TContainer1 : IIndexedContainer<TContainer1, TArch1>
            where TUpdater1 : ISystemUpdater<TUpdater1, TArch1, TContext>, allows ref struct
            where TContainer2 : IIndexedContainer<TContainer2, TArch2>
            where TUpdater2 : ISystemUpdater<TUpdater2, TArch2, TContext>, allows ref struct
            where TContainer3 : IIndexedContainer<TContainer3, TArch3>
            where TUpdater3 : ISystemUpdater<TUpdater3, TArch3, TContext>, allows ref struct
            where TContainer4 : IIndexedContainer<TContainer4, TArch4>
            where TUpdater4 : ISystemUpdater<TUpdater4, TArch4, TContext>, allows ref struct
        {
            var @enum = EnumeratorCreator<TArch1>.CreateSequential(ref container1);
            @enum.Reset();

            while (@enum.MoveNext())
            {
                updater1.Invoke(@enum.Remaining, ref @enum.Current, ref context);
                Looper<TArch2, TArch3, TArch4>.LoopIndexed(ref container2, updater2, ref container3, updater3, ref container4, updater4, ref context);
            }
        }
    }
}
