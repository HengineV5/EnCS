//HintName: TestSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Project.Primitives
{
	public partial class TestSystem
	{
		public void Update<T0Arch>(
			ref ComponentEnumerableNew<Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>.Enumerator<T0Arch> en0)
			where T0Arch : unmanaged, IArchType<T0Arch, Project.Primitives.Scale, Project.Primitives.Scale.Vectorized, Project.Primitives.Scale.Array>
		{
			// Not the best, but my templating language does not handle recusion the best atm
			
			
			while (en0.MoveNext())
			{
				var item0 = en0.Current;
				var remaining0 = en0.Remaining;
				
				
				for (int i = 0; i < remaining0; i++)
				{
					Update(Project.Primitives.Scale.Ref.FromArray(ref item0.item1Single, i));
				}
				for (int i = 0; i < remaining0; i++)
				{
					Update(Project.Primitives.Scale.Ref.FromArray(ref item0.item1Single, i));
				}
				
			}
			
			en0.Reset();
		}
	}
}