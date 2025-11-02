using System;

namespace EnCS
{
	public interface IArchType<TSelf, TTypeVec, TTypeSingle>
		where TSelf : unmanaged, IArchType<TSelf, TTypeVec, TTypeSingle>
		where TTypeVec : unmanaged
		where TTypeSingle : unmanaged
	{
		static abstract ref TTypeVec GetVec(ref TSelf arch);

		static abstract ref TTypeSingle GetSingle(ref TSelf arch);
	}
}
