using System;
using System.Collections.Generic;
using System.Text;

namespace EnCS
{
	public interface IArchEnumerator<TSelf, TSlice>
		where TSelf : allows ref struct
		where TSlice : allows ref struct
    {
		TSlice Current { get; }

		nint Remaining { get; }

		bool MoveNext();

		void Reset();
    }

	public interface IArchSlicer<TSlice, TArch>
		where TSlice : allows ref struct
		where TArch : unmanaged
    {
        static abstract TSlice Slice(ref TArch arch);
    }

	public static class EnumeratorCreator<TArch, TSlice, TSlicer>
        where TSlice : allows ref struct
        where TArch : unmanaged
        where TSlicer : IArchSlicer<TSlice, TArch>, allows ref struct
    {
		public static SequentialEnumerator<TSlice, TContainer, TArch, TSlicer> CreateSequential<TContainer>(ref TContainer container)
            where TContainer : IIndexedContainer<TContainer, TArch>
        {
			return new SequentialEnumerator<TSlice, TContainer, TArch, TSlicer>(ref container);
        }
	}

	public ref struct SequentialEnumerator<TSlice, TContainer, TArch, TSlicer> : IArchEnumerator<SequentialEnumerator<TSlice, TContainer, TArch, TSlicer>, TSlice>
		where TSlice : allows ref struct
		where TContainer : IIndexedContainer<TContainer, TArch>
		where TArch : unmanaged
		where TSlicer : IArchSlicer<TSlice, TArch>, allows ref struct
    {
		public TSlice Current => TSlicer.Slice(ref container.Get(index));

		public nint Remaining => nint.Min(8, container.Entities - index * 8);

        ref TContainer container;
		nint index;

        public SequentialEnumerator(ref TContainer container)
		{
			this.container = ref container;
			this.index = -1;
		}

		public bool MoveNext()
		{
			index++;

			if (container.Entities - index * 8 <= 0)
				return false;

            return true;
        }

		public void Reset()
		{
			index = -1;
        }
	}
}
