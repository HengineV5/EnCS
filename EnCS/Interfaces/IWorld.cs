namespace EnCS
{
	public interface IWorld<TSystem, TEcs>
		where TSystem : class
		where TEcs : class
	{
		static abstract void Loop(TSystem system, TEcs ecs);
	}

	public interface IWorld<TSystem, TEcs, TContext0>
		where TSystem : class
		where TContext0 : struct
		where TEcs : class
	{
		static abstract void Loop(TSystem system, TEcs ecs, ref TContext0 context0);
	}

	public interface IWorld<TSystem, TEcs, TContext0, TContext1>
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
		where TEcs : class
	{
		static abstract void Loop(TSystem system, TEcs ecs, ref TContext0 context0, ref TContext1 context1);
	}

	public interface IWorld<TSystem, TEcs, TContext0, TContext1, TContext2>
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
		where TContext2 : struct
		where TEcs : class
	{
		static abstract void Loop(TSystem system, TEcs ecs, ref TContext0 context0, ref TContext1 context1, ref TContext2 context2);
	}

	public interface IWorld<TSystem, TEcs, TContext0, TContext1, TContext2, TContext3>
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
		where TContext2 : struct
		where TContext3 : struct
		where TEcs : class
	{
		static abstract void Loop(TSystem system, TEcs ecs, ref TContext0 context0, ref TContext1 context1, ref TContext2 context2, ref TContext3 context3);
	}
}