//HintName: PerfSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PerfSystem
	{
		public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Position.Vectorized, Runner.Position.Array>

        {
            PerfSystem system;

			

            public SystemUpdater_0(PerfSystem system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				

                ref Runner.Position.Vectorized vec1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref slice);
                ref Runner.Position.Array single1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref slice);// Components

                // Resource

                for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Position.FromArray(ref single1, i);// Components

					// Resource Managers

                    system.Update(ref comp1);
                }

				system.Update(ref vec1);

				
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