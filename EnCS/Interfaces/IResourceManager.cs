namespace EnCS
{
	public interface IResourceManager<TResource>
	{
		public uint Store(in TResource resource);

		public ref TResource Get(uint id);
	}
}
