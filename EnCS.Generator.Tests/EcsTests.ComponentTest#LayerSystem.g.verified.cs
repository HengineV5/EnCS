//HintName: LayerSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class LayerSystem
	{
		public void Update<T0Arch, T1Arch>(
			ref ComponentEnumerableNew<Runner.TestComp123, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>.Enumerator<T0Arch> en0, 
			ref ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<T1Arch> en1)
			where T0Arch : unmanaged, IArchType<T0Arch, Runner.TestComp123, Runner.TestComp123.Vectorized, Runner.TestComp123.Array>
			where T1Arch : unmanaged, IArchType<T1Arch, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>
		{
			// Not the best, but my templating language does not handle recusion the best atm
			
			
			while (en0.MoveNext())
			{
				var item0 = en0.Current;
				var remaining0 = en0.Remaining;
				
				
				for (int i = 0; i < remaining0; i++)
				{
					var arg0_1 = Runner.TestComp123.FromArray(ref item0.item1Single, i);

					Update1(ref arg0_1);
					//Update1(Runner.TestComp123.FromArray(ref item0.item1Single, i));
				}
				

			
			
			while (en1.MoveNext())
			{
				var item1 = en1.Current;
				var remaining1 = en1.Remaining;
				
				
				for (int i = 0; i < remaining1; i++)
				{
					var arg1_1 = Runner.Position.FromArray(ref item1.item1Single, i);

					Update2(ref arg1_1);
					//Update2(Runner.Position.FromArray(ref item1.item1Single, i));
				}
				
			}
			
			en1.Reset();
			}
			
			en0.Reset();
		}
	}
}