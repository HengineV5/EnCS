namespace EnCS
{
	public interface IContainerManager<T1Arch, T11Comp, T11Vec, T11Single>
		where T1Arch : unmanaged, IArchType<T11Comp, T11Vec, T11Single>
		where T11Comp : unmanaged, IComponent<T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
	{

	}

	public interface IContainerManager<T1Arch, T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		where T1Arch : unmanaged, IArchType<T11Comp, T11Vec, T11Single, T12Comp, T12Vec, T12Single>
		where T11Comp : unmanaged, IComponent<T11Vec, T11Single>
		where T11Vec : unmanaged
		where T11Single : unmanaged
		where T12Comp : unmanaged, IComponent<T12Vec, T12Single>
		where T12Vec : unmanaged
		where T12Single : unmanaged
	{

	}

	public interface IContainerManager<T1Arch, T1Comp, T1Vec, T1Single, T2Arch, T2Comp, T2Vec, T2Single>
		where T1Arch : unmanaged, IArchType<T1Comp, T1Vec, T1Single>
		where T1Comp : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Arch : unmanaged, IArchType<T2Comp, T2Vec, T2Single>
		where T2Comp : unmanaged, IComponent<T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{

	}
}
