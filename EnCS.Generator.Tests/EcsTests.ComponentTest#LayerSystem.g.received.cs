//HintName: LayerSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class LayerSystem
	{
		public ref struct SystemUpdater_0<TVec, TSingle> : ISystemUpdater<TVec, TSingle, Context>
            where TVec : IArchType<TVec, Runner.Velocity.Vectorized>, allows ref struct
            where TSingle : IArchType<TSingle, Runner.Velocity>, allows ref struct
        {
            LayerSystem system;

			

            public SystemUpdater_0(LayerSystem system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref Context context)
            {
				

                for (int i = 0; i < remaining; i++)
                {
					// Resource Managers

                    system.Update1(ref ArchGetter<TVec, Runner.Velocity>.Get(ref single[i]));
                }

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update1(ref ArchGetter<TSingle, Runner.Velocity>.Get(ref single));

				
            }
        }

		public ref struct SystemUpdater_1<TVec, TSingle> : ISystemUpdater<TVec, TSingle, Context>
            where TVec : IArchType<TVec, Runner.Position.Vectorized>, allows ref struct
            where TSingle : IArchType<TSingle, Runner.Position>, allows ref struct
        {
            LayerSystem system;

			

            public SystemUpdater_1(LayerSystem system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref Context context)
            {
				

                for (int i = 0; i < remaining; i++)
                {
					// Resource Managers

                    system.Update2(ref ArchGetter<TVec, Runner.Position>.Get(ref single[i]));
                }

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update2(ref ArchGetter<TSingle, Runner.Position>.Get(ref single));

				
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