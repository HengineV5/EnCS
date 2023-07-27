using System;

namespace EnCS
{
	public interface IArchTypeEnumerable<TArch, TType, TTypeVec, TTypeSingle>
		where TArch : unmanaged
		where TType : unmanaged, IComponent<TTypeVec, TTypeSingle>
		where TTypeVec : unmanaged
		where TTypeSingle : unmanaged
	{
		static abstract ref TTypeVec GetVec(ref TArch arch);

		static abstract ref TTypeSingle GetSingle(ref TArch arch);
	}
}
