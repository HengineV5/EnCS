namespace EnCS
{
	public class EcsBuilder
	{
		public delegate void ArchTypeAction(EcsArchType archType);
		public delegate void SystemAction(EcsSystem system);
		public delegate void WorldAction(EcsWorld world);

		public ref struct EcsArchType
		{
			public EcsArchType ArchType<T1>(string name) where T1 : unmanaged
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2>(string name) where T1 : unmanaged where T2 : unmanaged
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3>(string name) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
			{
				return this;
			}
		}

		public ref struct EcsSystem
		{
			public EcsSystem System<T>() where T : class
			{
				return this;
			}
		}

		public ref struct EcsWorld
		{
			public EcsWorld World<T1>(string name) where T1 : unmanaged
			{
				return this;
			}

			public EcsWorld World<T1, T2>(string name) where T1 : unmanaged where T2 : unmanaged
			{
				return this;
			}
		}

        public EcsBuilder()
        {
            
        }

		public EcsBuilder ArchType(ArchTypeAction archType)
		{
			return this;
		}

		public EcsBuilder System(SystemAction system)
		{
			return this;
		}

		public EcsBuilder World(WorldAction system)
		{
			return this;
		}

		public T Build<T>() where T : class
		{
			throw new NotImplementedException();
		}
	}
}