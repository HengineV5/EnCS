namespace EnCS
{
	public struct ContainerManager<T1Arch, T11Comp, T11Vec, T11Single>
		: IContainerManager<ContainerManager<T1Arch, T11Comp, T11Vec, T11Single>, T1Arch, T11Comp, T11Vec, T11Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Comp, T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single> container1;

        public ContainerManager(nuint size1)
        {
			container1 = new(size1);
        }

		public static ComponentEnumerable<T1Arch, T11Comp, T11Vec, T11Single> GetT1Enumerable(ref ContainerManager<T1Arch, T11Comp, T11Vec, T11Single> manager)
		{
			return new(manager.container1.AsSpan());
		}
	}

	public struct ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		: IContainerManager<ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>, T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T11Comp, T11Vec, T11Single>, IArchType<T1Arch, T12Comp, T12Vec, T12Single>
		where T11Comp : unmanaged, IComponent<T11Comp, T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T12Comp : unmanaged, IComponent<T12Comp, T12Vec, T12Single>
		where T12Vec : unmanaged
		where T12Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single> container1;

		public ContainerManager(nuint size1)
		{
			container1 = new(size1);
		}

		public static ComponentEnumerable<T1Arch, T11Comp, T11Vec, T11Single> GetT1Enumerable(ref ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single> manager)
		{
			return new(manager.container1.AsSpan());
		}

		public static ComponentEnumerable<T1Arch, T12Comp, T12Vec, T12Single> GetT2Enumerable(ref ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single> manager)
		{
			return new(manager.container1.AsSpan());
		}
	}

	public interface IC2<TC2, T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single>
		where TC2 : unmanaged, IC2<TC2, T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Comp, T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T2Arch : unmanaged, IArchType<T2Arch, T21Comp, T21Vec, T21Single>
		where T21Comp : unmanaged, IComponent<T21Comp, T21Vec, T21Single>
		where T21Vec : unmanaged
		where T21Single : unmanaged
	{
		static abstract ArchTypeEnumerable<T1Arch, T11Comp, T11Vec, T11Single> GetT11Enumerable(ref TC2 c2);

		static abstract ArchTypeEnumerable<T2Arch, T21Comp, T21Vec, T21Single> GetT21Enumerable(ref TC2 c2);
	}

	public struct C2Manager<T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single>
		: IC2<C2Manager<T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single>, T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single>
		where T1Arch : unmanaged, IArchType<T1Arch, T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Comp, T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T2Arch : unmanaged, IArchType<T2Arch, T21Comp, T21Vec, T21Single>
		where T21Comp : unmanaged, IComponent<T21Comp, T21Vec, T21Single>
		where T21Vec : unmanaged
		where T21Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single> container1;
		public ArchTypeContainer<T2Arch, T21Comp, T21Vec, T21Single> container2;

		public C2Manager(nuint size1)
		{
			container1 = new(size1);
			container2 = new(size1);
		}

		public static ArchTypeEnumerable<T1Arch, T11Comp, T11Vec, T11Single> GetT11Enumerable(ref C2Manager<T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single> c2)
		{
			return new(c2.container1.AsSpan());
		}

		public static ArchTypeEnumerable<T2Arch, T21Comp, T21Vec, T21Single> GetT21Enumerable(ref C2Manager<T1Arch, T11Comp, T11Vec, T11Single, T2Arch, T21Comp, T21Vec, T21Single> c2)
		{
			return new(c2.container2.AsSpan());
		}
	}
}
