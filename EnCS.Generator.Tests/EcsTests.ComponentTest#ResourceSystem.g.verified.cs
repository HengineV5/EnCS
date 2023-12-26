//HintName: ResourceSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial class ResourceSystem
	{
		public void Update<T1Arch>(ref ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>.Enumerator<T1Arch> en, Project.Primitives.MeshResourceManager MeshResourceManager)
			where T1Arch : unmanaged, IArchType<T1Arch, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<T1Arch, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>, IArchType<T1Arch, Project.Primitives.MeshResourceManager.Mesh, Project.Primitives.MeshResourceManager.Mesh.Vectorized, Project.Primitives.MeshResourceManager.Mesh.Array>
		{
			while (en.MoveNext())
			{
				var item = en.Current;
				var remaining = en.Remaining;
				for (int i = 0; i < remaining; i++)
				{
					Update(Project.Primitives.Position.Ref.FromArray(ref item.item1Single, i), Project.Primitives.Velocity.Ref.FromArray(ref item.item2Single, i), ref Project.Primitives.MeshResourceManager.Mesh.Ref.FromArray(ref item.item3Single, i, MeshResourceManager).MeshId);
				}
				Update(ref item.item1Vec, ref item.item2Vec);
			}
		}
	}
}