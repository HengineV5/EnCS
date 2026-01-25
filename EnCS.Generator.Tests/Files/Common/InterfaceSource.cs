
namespace EnCS
{
	public interface IResourceManager<TResource>
	{
		public uint Store(in TResource resource);

		public ref TResource Get(uint id);
	}

	public interface IResourceManager<TIn, TOut>
	{
		public uint Store(in TIn resource);

		public ref TOut Get(uint id);
	}
}
