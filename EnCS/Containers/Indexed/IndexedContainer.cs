using System.Buffers;
using System.Formats.Tar;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnCS
{
	public unsafe struct IndexedContainer<TArch, TPtr> : IIndexedContainer<IndexedContainer<TArch, TPtr>, TArch, TPtr>
		where TArch : unmanaged
		where TPtr : allows ref struct
	{
		const int INITIAL_CONTAINER_SIZE = 1024;

		static nuint DataSize = (nuint)sizeof(TArch);

		public readonly nint Entities => entityCount;

		TArch* buff;
		nuint length;

		nint entityCount;

		nint[] map;
		Stack<nint> deleted;

		public IndexedContainer()
		{
			buff = (TArch*)NativeMemory.AllocZeroed(INITIAL_CONTAINER_SIZE * DataSize);
			map = new nint[INITIAL_CONTAINER_SIZE];
			length = INITIAL_CONTAINER_SIZE;
			entityCount = 0;

			deleted = new();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArchRef<TPtr> Create(in TArch data)
		{
			nint idx = entityCount;

			if (deleted.Count > 0)
				idx = deleted.Pop();

			map[idx] = entityCount >> 3;

			entityCount++;

			return new ArchRef<TPtr>(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Delete(in ArchRef<TPtr> ptr)
		{
			entityCount--;

			// Replace deleted element with last element
			buff[ptr.idx] = buff[entityCount];
			map[entityCount] = ptr.idx;

			deleted.Push(ptr.idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref TArch GetValue(ArchRef<TPtr> ptr)
		{
			return ref buff[map[ptr.idx]];
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TArch GetValue(nint idx)
        {
            return ref buff[idx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<TArch> AsSpan()
		{
            return new Span<TArch>(buff, ((int)entityCount >> 3) + 1);
		}
	}
}
