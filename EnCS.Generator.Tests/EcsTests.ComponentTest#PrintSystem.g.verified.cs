//HintName: PrintSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

namespace Runner
{
	public partial class PrintSystem
	{
		public ref struct SystemUpdater_0<TVec, TSingle> : ISystemUpdater<TVec, TSingle, Context>
            where TVec : IArchType<TVec, Runner.Position.Vectorized>, allows ref struct
            where TSingle : IArchType<TSingle, Runner.Position>, allows ref struct
        {
            PrintSystem system;

			

            public SystemUpdater_0(PrintSystem system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref Context context)
            {
				

                for (int i = 0; i < remaining; i++)
                {
                    system.Update(ref ArchGetter<TSingle, Runner.Position>.Get(ref single.Get(i)));
                }

				system.Update(ref ArchGetter<TVec, Runner.Position.Vectorized>.Get(ref vec));

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update(ref ArchGetter<TSingle, Runner.Position>.Get(ref single));

				
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