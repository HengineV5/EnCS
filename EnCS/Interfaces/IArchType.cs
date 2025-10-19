using System;

namespace EnCS
{
	public interface IArchType<TSelf, TType, TTypeVec, TTypeSingle>
		where TSelf : unmanaged, IArchType<TSelf, TType, TTypeVec, TTypeSingle>
		where TType : IComponent<TType, TTypeVec, TTypeSingle>, allows ref struct
		where TTypeVec : unmanaged
		where TTypeSingle : unmanaged
	{
		static abstract ref TTypeVec GetVec(ref TSelf arch);

		static abstract ref TTypeSingle GetSingle(ref TSelf arch);
	}
}
