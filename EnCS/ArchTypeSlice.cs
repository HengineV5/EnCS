using System.Runtime.CompilerServices;

namespace EnCS
{
	public ref struct ArchTypeSliceNew<T1Vec, T1Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ArchTypeSliceNew(ref T1Vec item1Vec, ref T1Single item1Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
		}
	}

	public ref struct ArchTypeSlice<T1Vec, T1Single, T2Vec, T2Single>
		where T1Vec : unmanaged
		where T1Single : unmanaged
		where T2Vec : unmanaged
		where T2Single : unmanaged
	{
		public ref T1Vec item1Vec;
		public ref T1Single item1Single;

		public ref T2Vec item2Vec;
		public ref T2Single item2Single;

		public ArchTypeSlice(ref T1Vec item1Vec, ref T1Single item1Single, ref T2Vec item2Vec, ref T2Single item2Single)
		{
			this.item1Vec = ref item1Vec;
			this.item1Single = ref item1Single;
			this.item2Vec = ref item2Vec;
			this.item2Single = ref item2Single;
		}
	}
}
