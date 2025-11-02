//HintName: PositionSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PositionSystem
	{
		public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Position.Vectorized, Runner.Position.Array>, IArchType<TArch, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array> 
        {
            PositionSystem system;

			Runner.MeshResourceManager MeshResourceManager;

            public SystemUpdater_0(PositionSystem system, Runner.MeshResourceManager MeshResourceManager)
            {
                this.system = system;
				this.MeshResourceManager = MeshResourceManager;
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				

                ref Runner.Position.Vectorized vec1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetVec(ref slice);
                ref Runner.Position.Array single1 = ref ArchGetter<TArch, Runner.Position.Vectorized, Runner.Position.Array>.GetSingle(ref slice);// Components

                ref Runner.MeshResourceManager.TestResource.Array single2 = ref ArchGetter<TArch, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetSingle(ref slice);// Resource

                for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Position.FromArray(ref single1, i);// Components

					ref var comp2 = ref Runner.MeshResourceManager.TestResource.FromArray(ref single2, i, MeshResourceManager).TestResourceProp;// Resource Managers

                    system.Update(ref comp1, ref comp2);
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