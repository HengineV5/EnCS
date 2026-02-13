using System.Buffers;
using System.Formats.Tar;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UtilLib.Memory;

namespace EnCS
{
	public struct IndexedContainer<TArchMem, TVec, TSingle> : IIndexedContainer<TVec, TSingle>
		where TArchMem : IArchMemory<TArchMem, TVec, TSingle>
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		const int INITIAL_CONTAINER_SIZE = 1024;

		public readonly nint Entities => entityCount;

		TArchMem memory;

		nint entityCount;

		nint[] map;
		Stack<nint> deleted;

		public IndexedContainer()
		{
			memory = TArchMem.Create(INITIAL_CONTAINER_SIZE);
			entityCount = 0;
			map = new nint[INITIAL_CONTAINER_SIZE];
			deleted = new();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArchRef<TSingle> Create()
		{
			nint idx = entityCount;

			if (deleted.Count > 0)
				idx = deleted.Pop();

			map[idx] = entityCount >> 3;

			entityCount++;

			return new ArchRef<TSingle>(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Delete(ref readonly ArchRef<TSingle> ptr)
		{
			entityCount--;

			// Replace deleted element with last element

			map[ptr.idx] = map[entityCount];
			deleted.Push(ptr.idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TSingle GetSingle(nint idx)
		{
			return memory.GetSingle(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TSingle GetSingle(ref readonly ArchRef<TSingle> ptr)
		{
			return memory.GetSingle(map[ptr.idx]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TVec GetVec(nint idx)
		{
			return memory.GetVec(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TVec GetVec(ref readonly ArchRef<TSingle> ptr)
		{
			return memory.GetVec(map[ptr.idx]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedRefBuffer8<TSingle> GetSingleArray(nint idx)
		{
			return memory.GetSingleArray(idx);
		}
	}
}
