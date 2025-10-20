namespace EnCS
{
	public interface IIndexedContainer<TSelf, TArch> : IContainer<TSelf, TArch>
		where TSelf : IIndexedContainer<TSelf, TArch>, allows ref struct
		where TArch : unmanaged
	{
		ref TArch Get(nint idx);
    }
}
