namespace EnCS
{
	public interface IHierarchicalContainer<TVec, TSingle> : IContainer
		where TVec : allows ref struct
		where TSingle : allows ref struct
	{
		ChildrenEnumerator<TSingle> GetChildren(ref readonly ArchRef<TSingle> parentPtr);

		ChildrenEnumerator<TSingle> GetChildrenAndSelf(ref readonly ArchRef<TSingle> parentPtr);

		ArchRef<TSingle> GetRoot();

		TSingle GetSingle(ref readonly ArchRef<TSingle> ptr);
	}
}
