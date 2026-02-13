using System;

namespace EnCS
{
	public interface IArchType<TSelf, TType>
		where TSelf : IArchType<TSelf, TType>, allows ref struct
		where TType : allows ref struct
	{
		static abstract ref TType Get(ref TSelf arch);
	}
}
