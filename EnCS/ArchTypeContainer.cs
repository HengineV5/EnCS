using System.Runtime.InteropServices;

namespace EnCS
{
	public unsafe struct ArchTypeContainer<TArch, T1Comp, T1Vec, T1Single> : IContainer<TArch, T1Comp, T1Vec, T1Single>
		where TArch : unmanaged, IArchType<TArch, T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		static nuint DataSize = (nuint)sizeof(T1Vec);

		TArch* data;
		nuint length;

        public ArchTypeContainer(nuint size)
        {
			data = (TArch*)NativeMemory.AllocZeroed(size * DataSize);
			length = size;
        }

		public Span<TArch> AsSpan()
		{
			return new Span<TArch>(data, (int)length);
		}
    }

	public unsafe struct ArchTypeContainer<TArch, T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single> : IContainer<TArch, T1Comp, T1Vec, T1Single, T2Comp, T2Vec, T2Single>
		where TArch : unmanaged, IArchType<TArch, T1Comp, T1Vec, T1Single>, IArchType<TArch, T2Comp, T2Vec, T2Single>
		where T1Comp : unmanaged, IComponent<T1Comp, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Comp : unmanaged, IComponent<T2Comp, T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		static nuint DataSize = (nuint)sizeof(TArch);

		TArch* data;
		nuint length;

        public ArchTypeContainer(nuint size)
        {
			data = (TArch*)NativeMemory.AllocZeroed(size * DataSize);
			length = size;
        }

		public Span<TArch> AsSpan()
		{
			return new Span<TArch>(data, (int)length);
		}
    }
}
