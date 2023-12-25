namespace EnCS
{
	public class EcsBuilder
	{
		public delegate void ArchTypeAction(EcsArchType archType);
		public delegate void SystemAction(EcsSystem system);
		public delegate void WorldAction(EcsWorld world);
		public delegate void HookAction(EcsHook hook);
		public delegate void ResourceAction(EcsResource resource);

		public ref struct EcsArchType
		{
			public EcsArchType ArchType<T1>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5, T6>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5, T6, T7>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5, T6, T7, T8>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name)
			{
				return this;
			}

			public EcsArchType ArchType<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name)
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

			public EcsWorld World<T1, T2, T3>(string name) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
			{
				return this;
			}

			public EcsWorld World<T1, T2, T3, T4>(string name) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
			{
				return this;
			}

			public EcsWorld World<T1, T2, T3, T4, T5>(string name) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged
			{
				return this;
			}
		}

		public ref struct EcsHook
		{
			public EcsHook Hook<TIn, TOut>() where TIn : unmanaged where TOut : unmanaged
			{
				return this;
			}
		}

		public ref struct EcsResource
		{
			public EcsResource ResourceManager<T>() where T : class
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

		public EcsBuilder Resource(ResourceAction resource)
		{
			return this;
		}

		EcsBuilder Hook(HookAction hook)
		{
			return this;
		}

		public T Build<T>() where T : class
		{
			return default;
			//throw new NotImplementedException();
		}
	}
}