//HintName: PositionSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PositionSystem
	{
		public ref struct SystemUpdater_0<TVec, TSingle> : ISystemUpdater<TVec, TSingle, Context>
            where TVec : IArchType<TVec, Runner.Position.Vectorized>, IArchType<TVec, Runner.MeshResourceManager.TestResource.Vectorized>, allows ref struct
            where TSingle : IArchType<TSingle, Runner.Position>, IArchType<TSingle, Runner.MeshResourceManager.TestResource>, allows ref struct
        {
            PositionSystem system;

			Runner.MeshResourceManager MeshResourceManager;

            public SystemUpdater_0(PositionSystem system, Runner.MeshResourceManager MeshResourceManager)
            {
                this.system = system;
				this.MeshResourceManager = MeshResourceManager;
            }

            public void Invoke(nint remaining, scoped TVec vec, scoped FixedRefBuffer8<TSingle> single, ref Context context)
            {
				

                for (int i = 0; i < remaining; i++)
                {
					ref var comp2 = ref Runner.MeshResourceManager.TestResource.FromArray(ref single2, i, MeshResourceManager).TestResourceProp;// Resource Managers

                    system.Update(ref context.TestContext, ref ArchGetter<TVec, Runner.Position>.Get(ref single[i]), ref ArchGetter<TVec, Runner.MeshResourceManager.TestResource>.Get(ref single[i]).Get(MeshResourceManager));
                }

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update(ref context.TestContext, ref ArchGetter<TSingle, Runner.Position>.Get(ref single), ref ArchGetter<TVec, Runner.MeshResourceManager.TestResource>.Get(ref single).Get(MeshResourceManager));

				
            }
        } 

		public ref struct Context
		{
			public ref TestContext TestContext;

			public Context(ref TestContext TestContext)
			{
				this.TestContext = ref TestContext;
			}
		}
	}
}