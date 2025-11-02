namespace EnCS
{
	public interface ISystemUpdater<TSelf, TArch>
		where TSelf : ISystemUpdater<TSelf, TArch>, allows ref struct
        where TArch : unmanaged
    {
		void Invoke(nint remaining, ref TArch arch);
    }

	public interface ISystemUpdater<TSelf, TArch, TContext>
		where TSelf : ISystemUpdater<TSelf, TArch, TContext>, allows ref struct
        where TArch : unmanaged
    {
        void Invoke(nint remaining, ref TArch arch, ref TContext context);
    }
}