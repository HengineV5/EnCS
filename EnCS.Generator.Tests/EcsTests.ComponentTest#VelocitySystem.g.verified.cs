//HintName: VelocitySystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial class VelocitySystem
	{
		public void Update<T0Arch>(ref ComponentEnumerableNew<Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>.Enumerator<T0Arch> en0)
			where T0Arch : unmanaged, IArchType<T0Arch, Project.Primitives.Position, Project.Primitives.Position.Vectorized, Project.Primitives.Position.Array>, IArchType<T0Arch, Project.Primitives.Velocity, Project.Primitives.Velocity.Vectorized, Project.Primitives.Velocity.Array>
		{
			

			// Not the best, but my templating language does not handle recusion the best atm
			
			
			while (en0.MoveNext())
			{
				var item0 = en0.Current;
				var remaining0 = en0.Remaining;
				
				for (int i = 0; i < remaining0; i++)
				{
					Update(Project.Primitives.Position.Ref.FromArray(ref item0.item1Single, i), Project.Primitives.Velocity.Ref.FromArray(ref item0.item2Single, i));
				}
				Update(ref item0.item1Vec, ref item0.item2Vec);
				
				
			}
			
			en0.Reset();
		}
	}
}