//HintName: ResourceSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial class ResourceSystem
	{
		public void Update<T0Arch, T1Arch>(ref ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>.Enumerator<T0Arch> en0, ref ComponentEnumerableNew<Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>.Enumerator<T1Arch> en1, Project.Primitives.MeshResourceManager MeshResourceManager, Project.Primitives.TestResourceManager TestResourceManager)
			where T0Arch : unmanaged, IArchType<T0Arch, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<T0Arch, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>, IArchType<T0Arch, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>, IArchType<T0Arch, Project.Primitives.TestResourceManager.Kaki, Project.Primitives.TestResourceManager.Kaki.Vectorized, Project.Primitives.TestResourceManager.Kaki.Array>
			where T1Arch : unmanaged, IArchType<T1Arch, Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>
		{
			TestContext contextTestContext = new TestContext();

			// Not the best, but my templating language does not handle recusion the best atm
			PreLoop1();
			int chunkCounter0 = 0;
			while (en0.MoveNext())
			{
				var item0 = en0.Current;
				var remaining0 = en0.Remaining;
				if (chunkCounter0 == 16) PreLoop1();
				for (int i = 0; i < remaining0; i++)
				{
					Update(ref contextTestContext, Project.Primitives.Position.Ref.FromArray(ref item0.item1Single, i), Project.Primitives.Velocity.Ref.FromArray(ref item0.item2Single, i), ref Project.Primitives.MeshResourceManager.Mesh.Ref.FromArray(ref item0.item3Single, i, MeshResourceManager).Mesh, ref Project.Primitives.TestResourceManager.Kaki.Ref.FromArray(ref item0.item4Single, i, TestResourceManager).Kaki);
				}
				Update(ref contextTestContext, ref item0.item1Vec, ref item0.item2Vec);
				if (chunkCounter0 == 16) PostLoop1();
				chunkCounter0 = chunkCounter0 == 16 ? 0 : chunkCounter0 + 1;

			PreLoop();
			
			while (en1.MoveNext())
			{
				var item1 = en1.Current;
				var remaining1 = en1.Remaining;
				
				Update(ref item1.item1Vec);
				for (int i = 0; i < remaining1; i++)
				{
					Update(Project.Primitives.Scale.Ref.FromArray(ref item1.item1Single, i));
				}
				
				
			}
			PostLoop();
			en1.Reset();
			}
			PostLoop1();
			en0.Reset();
		}
	}
}