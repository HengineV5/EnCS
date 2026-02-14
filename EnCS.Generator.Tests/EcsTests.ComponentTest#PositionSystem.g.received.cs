//HintName: PositionSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;
using UtilLib.Memory;

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
                    system.Update(ref context.TestContext, ref ArchGetter<TSingle, Runner.Position>.Get(ref single.Get(i)), ref ArchGetter<TSingle, Runner.MeshResourceManager.TestResource>.Get(ref single.Get(i)).SetResourceManager(MeshResourceManager).Get());
                }

				
            }

            public void Invoke(scoped TSingle single, ref Context context)
            {
				

                system.Update(ref context.TestContext, ref ArchGetter<TSingle, Runner.Position>.Get(ref single), ref ArchGetter<TSingle, Runner.MeshResourceManager.TestResource>.Get(ref single).SetResourceManager(MeshResourceManager).Get());

				
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