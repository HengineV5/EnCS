//HintName: LayerSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class LayerSystem
	{
		public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>

        {
            LayerSystem system;

			

            public SystemUpdater_0(LayerSystem system)
            {
                this.system = system;
				
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				

                ref Runner.Velocity.Vectorized vec1 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref slice);
                ref Runner.Velocity.Array single1 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref slice);// Components

                // Resource

                for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Velocity.FromArray(ref single1, i);// Components

					// Resource Managers

                    system.Update1(ref comp1);
                }

				
            }

            public void Invoke(int idx, ref TArch slice, ref Context context)
            {
				

                ref Runner.Velocity.Vectorized vec1 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetVec(ref slice);
                ref Runner.Velocity.Array single1 = ref ArchGetter<TArch, Runner.Velocity.Vectorized, Runner.Velocity.Array>.GetSingle(ref slice);// Components

                // Resource

                
                {
                    var comp1 = Runner.Velocity.FromArray(ref single1, idx);// Components

					// Resource Managers

                    system.Update1(ref comp1);
                }

				
            }
        }

		public ref struct SystemUpdater_1<TArch> : ISystemUpdater<SystemUpdater_1<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Position.Vectorized, Runner.Position.Array>

        {
            LayerSystem system;

			

            public SystemUpdater_1(LayerSystem system)
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

                    system.Update2(ref comp1);
                }

				
            }

            public void Invoke(int idx, ref TArch slice, ref Context context)
            {
				

                ref Runner.Position.Vectorized vec1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref slice);
                ref Runner.Position.Array single1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref slice);// Components

                // Resource

                
                {
                    var comp1 = Runner.Position.FromArray(ref single1, idx);// Components

					// Resource Managers

                    system.Update2(ref comp1);
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