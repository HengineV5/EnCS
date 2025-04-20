//HintName: PerfSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PerfSystem
	{
		public void Update<T0Arch>(
			ref ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<T0Arch> en0)
			where T0Arch : unmanaged, IArchType<T0Arch, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>
		{
			// Not the best, but my templating language does not handle recusion the best atm
			
			
			while (en0.MoveNext())
			{
				var item0 = en0.Current;
				var remaining0 = en0.Remaining;
				
				
				for (int i = 0; i < remaining0; i++)
				{
					var arg0_1 = Runner.Position.FromArray(ref item0.item1Single, i);

					Update(ref arg0_1);
					//Update(Runner.Position.FromArray(ref item0.item1Single, i));
				}
				Update(ref item0.item1Vec);
				
			}
			
			en0.Reset();
		}
	}
}