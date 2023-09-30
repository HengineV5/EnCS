//HintName: PrintSystem.g.cs
using System.Runtime.Intrinsics;
using System.Runtime.CompilerServices;
using EnCS;

namespace Runner
{
	public partial class PrintSystem
	{
		public void Update<T1Arch>(ref ComponentEnumerableNew<Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>.Enumerator<T1Arch> en)
			where T1Arch : unmanaged, IArchType<T1Arch, Runner.Position, Runner.Position.Vectorized, Runner.Position.Array>
		{
			while (en.MoveNext())
			{
				var item = en.Current;
				var remaining = en.Remaining;
				for (int i = 0; i < remaining; i++)
				{
					Update(Runner.Position.Ref.FromArray(ref item.item1Single, i));
				}
				Update(ref item.item1Vec);
			}
		}
	}
}