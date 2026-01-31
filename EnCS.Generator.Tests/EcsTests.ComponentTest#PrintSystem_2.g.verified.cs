//HintName: PrintSystem_2.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PrintSystem_2
	{
		public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Position.Vectorized, Runner.Position.Array>, IArchType<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>

        {
            PrintSystem_2 system;

			

            public SystemUpdater_0(PrintSystem_2 system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				

                ref Runner.Position.Vectorized vec1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref slice);
                ref Runner.Position.Array single1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref slice);
				ref Runner.Velocity.Vectorized vec2 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref slice);
                ref Runner.Velocity.Array single2 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref slice);// Components

                // Resource

                for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Position.FromArray(ref single1, i);
					var comp2 = Runner.Velocity.FromArray(ref single2, i);// Components

					// Resource Managers

                    system.Update(ref comp1, ref comp2);
                }

				system.Update(ref vec1, ref vec2);

				
            }

            public void Invoke(int idx, ref TArch slice, ref Context context)
            {
				

                ref Runner.Position.Vectorized vec1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref slice);
                ref Runner.Position.Array single1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref slice);
				ref Runner.Velocity.Vectorized vec2 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref slice);
                ref Runner.Velocity.Array single2 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref slice);// Components

                // Resource

                
                {
                    var comp1 = Runner.Position.FromArray(ref single1, idx);
					var comp2 = Runner.Velocity.FromArray(ref single2, idx);// Components

					// Resource Managers

                    system.Update(ref comp1, ref comp2);
                }

				
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