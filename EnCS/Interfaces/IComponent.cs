using System;
using System.Runtime.CompilerServices;

namespace EnCS
{
	public interface IComponent<TSelf, TVec, TSingle>
		where TSelf : IComponent<TSelf, TVec, TSingle>, allows ref struct
		where TVec : unmanaged
		where TSingle : unmanaged
	{
		static abstract ref TVec GetVec<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TSelf, TVec, TSingle>;

		static abstract ref TSingle GetSingle<TArch>(ref TArch arch)
			where TArch : unmanaged, IArchType<TArch, TSelf, TVec, TSingle>;

        static abstract TSelf FromArray(ref TSingle single, int idx);
    }
}
