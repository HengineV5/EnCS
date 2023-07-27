using System;

namespace EnCS
{
	public interface IArchType<T1, T1Vec, T1Single>
		where T1 : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
	}

	public interface IArchType<T1, T1Vec, T1Single, T2, T2Vec, T2Single>
		where T1 : unmanaged, IComponent<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2 : unmanaged, IComponent<T2Vec, T2Single>
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
	}
}
