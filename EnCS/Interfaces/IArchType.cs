using System;

namespace EnCS
{
	public interface IArchType<TArch, TType, TTypeVec, TTypeSingle>
		where TArch : unmanaged, IArchType<TArch, TType, TTypeVec, TTypeSingle>
		where TType : unmanaged, IComponent<TType, TTypeVec, TTypeSingle>
		where TTypeVec : unmanaged
		where TTypeSingle : unmanaged
	{
		static abstract ref TTypeVec GetVec(ref TArch arch);

		static abstract ref TTypeSingle GetSingle(ref TArch arch);
	}
}
