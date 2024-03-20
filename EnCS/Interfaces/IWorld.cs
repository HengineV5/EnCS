using System.Runtime.CompilerServices;

namespace EnCS
{
	public interface IWorld<TEcs, TSystem>
		where TEcs : class
		where TSystem : class
	{
		static abstract void Loop(TEcs ecs, TSystem system);
	}

	public interface IWorld<TEcs, TSystem, TContext0>
		where TEcs : class
		where TSystem : class
		where TContext0 : struct
	{
		static abstract void Loop(TEcs ecs, TSystem system, ref TContext0 context0);
	}

	public interface IWorld<TEcs, TSystem, TContext0, TContext1>
		where TEcs : class
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
	{
		static abstract void Loop(TEcs ecs, TSystem system, ref TContext0 context0, ref TContext1 context1);
	}

	public interface IWorld<TEcs, TSystem, TContext0, TContext1, TContext2>
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
		where TContext2 : struct
	{
		static abstract void Loop(TEcs ecs, TSystem system, ref TContext0 context0, ref TContext1 context1, ref TContext2 context2);
	}

	public interface IWorld<TEcs, TSystem, TContext0, TContext1, TContext2, TContext3>
		where TEcs : class
		where TSystem : class
		where TContext0 : struct
		where TContext1 : struct
		where TContext2 : struct
		where TContext3 : struct
	{
		static abstract void Loop(TEcs ecs, TSystem system, ref TContext0 context0, ref TContext1 context1, ref TContext2 context2, ref TContext3 context3);
	}
}
