using System;
using System.Runtime.CompilerServices;

namespace EnCS
{
	public interface IComponent<TComp, TVec, TSingle>
		where TComp : unmanaged, IComponent<TComp, TVec, TSingle>
		where TVec : unmanaged
		where TSingle : unmanaged
	{
		static abstract ref TVec GetVec<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>;

		static abstract ref TSingle GetSingle<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TComp, TVec, TSingle>;
	}
}
