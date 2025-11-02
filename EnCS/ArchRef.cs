namespace EnCS
{
	public struct ArchRef<TArch> where TArch : allows ref struct
	{
		public readonly nint idx;

		internal ArchRef(nint idx)
		{
			this.idx = idx;
		}
	}
}
