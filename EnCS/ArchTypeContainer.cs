using System.Buffers;
using System.Formats.Tar;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnCS
{
	public struct ArchRef<TArch> where TArch : unmanaged
	{
		public readonly nuint idx;

		internal ArchRef(nuint idx)
		{
			this.idx = idx;
		}
	}

	public static class ContainerExtensions
	{
		public static void Set<T, TType, TTypeVec, TTypeSingle>()
		{

		}
	}

	public unsafe struct ArchTypeContainer<TArch> : IContainer<TArch> where TArch : unmanaged
	{
		static nuint DataSize = (nuint)sizeof(TArch);

		TArch* buff;
		nuint length;

		nuint entityCount;

		nuint[] map;
		Stack<nuint> deleted;

		public ArchTypeContainer()
		{
			buff = (TArch*)NativeMemory.AllocZeroed(10 * DataSize);
			map = new nuint[10];
			length = 10;
			entityCount = 0;

			deleted = new();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArchRef<TArch> Create(in TArch data)
		{
			nuint idx = entityCount;

			if (deleted.Count > 0)
				idx = deleted.Pop();

			//buff[entityCount] = data;
			map[idx] = entityCount >> 3;

			entityCount++;

			return new ArchRef<TArch>(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Delete(ref ArchRef<TArch> ptr)
		{
			entityCount--;

			// Replace deleted element with last element
			buff[ptr.idx] = buff[entityCount];
			map[entityCount] = ptr.idx;

			deleted.Push(ptr.idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref TArch GetVec(ArchRef<TArch> ptr)
		{
			return ref buff[map[ptr.idx]];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<TArch> AsSpan()
		{
            return new Span<TArch>(buff, ((int)entityCount >> 3) + 1);
		}
	}

	public unsafe struct ArchTypeContainerNew<TArch, TType1, TType1Vec, TType1Single> : IContainer<TArch>
		where TArch : unmanaged, IArchType<TArch, TType1, TType1Vec, TType1Single>
		where TType1 : unmanaged, IComponent<TType1, TType1Vec, TType1Single>
		where TType1Vec : unmanaged
		where TType1Single : unmanaged
	{
		static nuint DataSize = (nuint)sizeof(TArch);
		//static int EntitiesPerArch = TArch.GetSize();

		TArch* buff;
		nuint length;

		nuint entityCount; // Amount of entities in this container

		nuint[] map;
		Stack<nuint> deleted;

		public ArchTypeContainerNew(nuint startSize = 8)
		{
			buff = (TArch*)NativeMemory.AllocZeroed(startSize * DataSize);
			map = new nuint[startSize];
			length = startSize;
			entityCount = 0;

			deleted = new();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArchRef<TArch> Create()
		{
			nuint idx = entityCount >> 3;

			if (deleted.Count > 0)
				idx = deleted.Pop();

			buff[entityCount & 7] = default;
			map[idx] = entityCount;

			entityCount++;

			return new ArchRef<TArch>(idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Delete(ref ArchRef<TArch> ptr)
		{
			entityCount--;

			// Replace deleted element with last element
			buff[ptr.idx] = buff[entityCount];
			map[entityCount] = ptr.idx;

			deleted.Push(ptr.idx);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref TArch Get(ref ArchRef<TArch> ptr)
		{
			return ref buff[map[ptr.idx]];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<TArch> AsSpan()
		{
			return new Span<TArch>(buff, (int)entityCount);
		}

		nuint GetIdx()
		{
			if (deleted.Count > 0)
				return deleted.Pop();

			return entityCount;
		}
	}
}
