namespace EnCS
{
	public interface ISystemUpdater<TSelf, TArch>
		where TSelf : ISystemUpdater<TSelf, TArch>, allows ref struct
        where TArch : unmanaged
    {
		void Invoke(nint remaining, ref TArch arch);
    }
}