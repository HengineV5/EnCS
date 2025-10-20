namespace EnCS
{
	public interface IContainer<TSelf, TArch>
		where TSelf : IContainer<TSelf, TArch>, allows ref struct
		where TArch : unmanaged
	{
		nint Entities { get; }
    }
}
