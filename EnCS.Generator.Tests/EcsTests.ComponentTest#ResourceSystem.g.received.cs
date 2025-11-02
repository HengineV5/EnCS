//HintName: ResourceSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class ResourceSystem
	{
		public ref struct SystemUpdater_0<TArch> : ISystemUpdater<SystemUpdater_0<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, TestContext.Vectorized, TestContext.Array>, IArchType<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>, IArchType<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>, IArchType<TArch, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array> 
        {
            ResourceSystem system;

			Runner.MeshResourceManager MeshResourceManager;

            public SystemUpdater_0(ResourceSystem system, Runner.MeshResourceManager MeshResourceManager)
            {
                this.system = system;
				this.MeshResourceManager = MeshResourceManager;
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				PreLoop1(ref contextTestContext);

                ref Runner.Ref.Vectorized vec1 = ref ArchGetter<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>.GetVec(ref slice);
                ref Runner.Ref.Array single1 = ref ArchGetter<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>.GetSingle(ref slice);
				ref Runner.Ref.Vectorized vec2 = ref ArchGetter<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>.GetVec(ref slice);
                ref Runner.Ref.Array single2 = ref ArchGetter<TArch, Runner.Ref.Vectorized, Runner.Ref.Array>.GetSingle(ref slice);// Components

                ref Runner.MeshResourceManager.TestResource.Array single3 = ref ArchGetter<TArch, Runner.MeshResourceManager.TestResource.Vectorized, Runner.MeshResourceManager.TestResource.Array>.GetSingle(ref slice);// Resource

                for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Ref.FromArray(ref single1, i);
					var comp2 = Runner.Ref.FromArray(ref single2, i);// Components

					ref var comp3 = ref Runner.MeshResourceManager.TestResource.FromArray(ref single3, i, MeshResourceManager).TestResourceProp;// Resource Managers

                    system.Update(ref context.TestContext, ref comp1, ref comp2, ref comp3);
                }

				PostLoop1();
            }
        }

		public ref struct SystemUpdater_1<TArch> : ISystemUpdater<SystemUpdater_1<TArch>, TArch, Context>
            where TArch : unmanaged, IArchType<TArch, Runner.Vectorized.Vectorized, Runner.Vectorized.Array> 
        {
            ResourceSystem system;

			Runner.MeshResourceManager MeshResourceManager;

            public SystemUpdater_1(ResourceSystem system, Runner.MeshResourceManager MeshResourceManager)
            {
                this.system = system;
				this.MeshResourceManager = MeshResourceManager;
            }

            public void Invoke(nint remaining, ref TArch slice, ref Context context)
            {
				PreLoop();

                ref Runner.Vectorized.Vectorized vec1 = ref ArchGetter<TArch, Runner.Vectorized.Vectorized, Runner.Vectorized.Array>.GetVec(ref slice);
                ref Runner.Vectorized.Array single1 = ref ArchGetter<TArch, Runner.Vectorized.Vectorized, Runner.Vectorized.Array>.GetSingle(ref slice);// Components

                // Resource

                system.Update(ref vec1);

				for (int i = 0; i < remaining; i++)
                {
                    var comp1 = Runner.Vectorized.FromArray(ref single1, i);// Components

					// Resource Managers

                    system.Update(ref comp1);
                }

				PostLoop();
            }
        } 

		public ref struct Context
		{
			ref TestContext TestContext;
			ref TestContext2 TestContext2;

			public Context(ref TestContext TestContextref TestContext2 TestContext2)
			{
				this.TestContext = ref TestContext;
				this.TestContext2 = ref TestContext2;
			}
		}
	}
}