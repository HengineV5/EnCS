namespace EnCS
{
	public interface IHierarchicalContainer<TSelf, TArch, TPtr> : IContainer<TSelf, TArch>
		where TSelf : IHierarchicalContainer<TSelf, TArch, TPtr>, allows ref struct
		where TArch : unmanaged
		where TPtr : allows ref struct
	{
		ChildrenEnumerator<TPtr, TArch> GetChildren(in ArchRef<TPtr> parentPtr);

		Span<TArch> GetChildrenValues(ArchRef<TPtr> parentPtr);

		ArchRef<TPtr> GetRoot();

		ref TArch GetValue(ArchRef<TPtr> ptr);
	}
}
