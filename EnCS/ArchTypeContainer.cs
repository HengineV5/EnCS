using System.Buffers;
using System.Formats.Tar;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnCS
{
	public struct ArchRef<TArch> where TArch : allows ref struct
	{
		public readonly nuint idx;

		internal ArchRef(nuint idx)
		{
			this.idx = idx;
		}
	}

	public unsafe struct ArchTypeContainer<TArch, TPtr> : IContainer<TArch> where TArch : unmanaged where TPtr : allows ref struct
	{
		const int INITIAL_CONTAINER_SIZE = 1_000_000;

		static nuint DataSize = (nuint)sizeof(TArch);

		public readonly nuint Entities => entityCount;

		TArch* buff;
		nuint length;

		nuint entityCount;

		nuint[] map;
		Stack<nuint> deleted;

		public ArchTypeContainer()
		{
			buff = (TArch*)NativeMemory.AllocZeroed(INITIAL_CONTAINER_SIZE * DataSize);
			map = new nuint[INITIAL_CONTAINER_SIZE];
			length = INITIAL_CONTAINER_SIZE;
			entityCount = 0;

			deleted = new();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArchRef<TPtr> Create(in TArch data)
		{
			nuint idx = entityCount;

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
		public ref TArch GetVec(ArchRef<TPtr> ptr)
		{
			return ref buff[map[ptr.idx]];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<TArch> AsSpan()
		{
            return new Span<TArch>(buff, ((int)entityCount >> 3) + 1);
		}
	}
}
