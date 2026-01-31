namespace EnCS
{
	public interface IIndexedContainer<TSelf, TArch> : IContainer<TSelf, TArch>
		where TSelf : IIndexedContainer<TSelf, TArch>, allows ref struct
		where TArch : unmanaged
	{
		ref TArch GetValue(nint idx);
    }

	public interface IIndexedContainer<TSelf, TArch, TPtr> : IIndexedContainer<TSelf, TArch>
		where TSelf : IIndexedContainer<TSelf, TArch>, allows ref struct
		where TArch : unmanaged
		where TPtr : allows ref struct
	{
		ref TArch GetValue(ArchRef<TPtr> ptr);
    }
}
