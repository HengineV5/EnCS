namespace EnCS
{
	public interface IContainer<TArch, T1Component, T1Vec, T1Single>
		where TArch : unmanaged, IArchType<TArch, T1Component, T1Vec, T1Single>
		where T1Component : unmanaged, IComponent<T1Component, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
	}

	public interface IContainer<TArch, T1Component, T1Vec, T1Single, T2Component, T2Vec, T2Single>
		where TArch : unmanaged, IArchType<TArch, T1Component, T1Vec, T1Single>, IArchType<TArch, T2Component, T2Vec, T2Single>
		where T1Component : unmanaged, IComponent<T1Component, T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Component : unmanaged, IComponent<T2Component, T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
	}
}
