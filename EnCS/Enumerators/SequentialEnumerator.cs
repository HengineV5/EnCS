using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EnCS
{
	public ref struct SequentialEnumerator<TContainer, TArch> : IArchEnumerator<SequentialEnumerator<TContainer, TArch>, TArch>
		where TContainer : IIndexedContainer<TContainer, TArch>
		where TArch : unmanaged
    {
		public ref TArch Current => ref container.GetValue(index);

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

	public ref struct MappedEnumerator<TContainer, TArch> : IArchEnumerator<SequentialEnumerator<TContainer, TArch>, TArch>
		where TContainer : IIndexedContainer<TContainer, TArch>
		where TArch : unmanaged
	{
		public ref TArch Current => ref container.GetValue(map[index] / 8);

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

			if (map.Length - index <= 0)
				return false;

			return true;
		}

		public void Reset()
		{
			index = -1;
		}
	}
}
