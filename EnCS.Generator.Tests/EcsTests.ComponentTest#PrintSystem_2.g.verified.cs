//HintName: PrintSystem_2.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public partial class PrintSystem_2
	{
		public ref struct SystemUpdater_0<TVec, TSingle> : ISystemUpdater<TVec, TSingle, Context>
            where TVec : IArchType<TVec, Runner.Position.Vectorized>, IArchType<TVec, Runner.Velocity.Vectorized>, allows ref struct
            where TSingle : IArchType<TSingle, Runner.Position>, IArchType<TSingle, Runner.Velocity>, allows ref struct
        {
            PrintSystem_2 system;

			

            public SystemUpdater_0(PrintSystem_2 system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref Context context)
            {
				

                for (int i = 0; i < remaining; i++)
                {
                    system.Update(ref ArchGetter<TSingle, Runner.Position>.Get(ref single.Get(i)), ref ArchGetter<TSingle, Runner.Velocity>.Get(ref single.Get(i)));
                }

				system.Update(ref ArchGetter<TVec, Runner.Position.Vectorized>.Get(ref vec), ref ArchGetter<TVec, Runner.Velocity.Vectorized>.Get(ref vec));

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update(ref ArchGetter<TSingle, Runner.Position>.Get(ref single), ref ArchGetter<TSingle, Runner.Velocity>.Get(ref single));

				
            }
        } 

		public ref struct Context
		{
			

			public Context()
			{
				
			}
		}
	}
}