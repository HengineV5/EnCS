namespace EnCS
{
	public struct ContainerManager<T1Arch, T11Comp, T11Vec, T11Single> : IContainerManager<T1Arch, T11Comp, T11Vec, T11Single>
		where T1Arch : unmanaged, IArchType<T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single> container1;

        public ContainerManager(nuint size1)
        {
			container1 = new(size1);
        }
    }

	public struct ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single> : IContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		where T1Arch : unmanaged, IArchType<T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		where T11Comp : unmanaged, IComponent<T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T12Comp : unmanaged, IComponent<T12Vec, T12Single>
		where T12Vec : unmanaged
		where T12Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single> container1;

		public ContainerManager(nuint size1)
		{
			container1 = new(size1);
		}
	}

	public struct ContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T21Arch, T21Comp, T21Vec, T21Single> : IContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T21Arch, T21Comp, T21Vec, T21Single>
		where T1Arch : unmanaged, IArchType<T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T21Arch : unmanaged, IArchType<T21Comp, T21Vec, T21Single>
		where T21Comp : unmanaged, IComponent<T21Vec, T21Single>
		where T21Vec : unmanaged
		where T21Single : unmanaged
	{
		public ArchTypeContainer<T1Arch, T11Comp, T11Vec, T11Single> container1;
		public ArchTypeContainer<T21Arch, T21Comp, T21Vec, T21Single> container2;

		public ContainerManager(nuint size1, nuint size2)
		{
			container1 = new(size1);
			container2 = new(size2);
		}
	}
}
