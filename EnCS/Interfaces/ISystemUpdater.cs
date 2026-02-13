using UtilLib.Memory;

namespace EnCS
{
	public interface ISystemUpdater<TVec, TSingle>
        where TVec : allows ref struct
        where TSingle : allows ref struct
    {
		void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single);

		void Invoke(scoped TSingle single);
    }

	public interface ISystemUpdater<TVec, TSingle, TContext>
        where TVec : allows ref struct
        where TSingle : allows ref struct
        where TContext : allows ref struct
    {
        void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref TContext context);

		void Invoke(scoped TSingle single, ref TContext context);
	}
}