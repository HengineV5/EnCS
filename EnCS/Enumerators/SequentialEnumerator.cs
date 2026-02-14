using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UtilLib.Memory;

namespace EnCS
{
	public ref struct SequentialEnumerator<TContainer, TVec, TSingle> : IArchEnumerator<TVec, TSingle>
		where TContainer : IIndexedContainer<TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		public TSingle Current => container.GetSingle(index);

		public FixedRefBuffer8<TSingle> CurrentArray => container.GetSingleArray(index);

		public TVec CurrentVec => container.GetVec(index);

		public int Remaining => int.Max(0, (int)container.Entities - index);

		ref TContainer container;
		int index;

        public SequentialEnumerator(ref TContainer container)
		{
			this.container = ref container;
			this.index = -1;
		}

		public bool MoveNext()
		{
			index++;

			if (index >= container.Entities)
				return false;

			return true;
		}

		public bool MoveNextArray()
		{
			index += 8;

			if (index >= container.Entities)
				return false;

			return true;
		}

		public void Reset()
		{
			index = -1;
        }
	}
}
