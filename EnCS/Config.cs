using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnCS
{
	internal static class Config
    {
        // How many component types an ECS instance supports. (Must be divisable by 32)
        public const int MAX_COMPONENTS = 64;

        // How many components each entity can have assigned to it.
        public const int MAX_COMPONENTS_PER_ARCHTYPE = 16;

        // How many arrays the memory manager can store.
        public const uint MEMORY_MANAGER_ITEMS = 1024;

        // Start amount of archtypes.
        public const uint ARCHTYPE_COUNT = 32;

        // Start amount of entities
        public const uint ENTITY_COUNT = 64;

        // Start amound of worlds
        public const uint WORLD_COUNT = 2;

#if RELEASE
        // How large each arch type will be by default.
        public const uint ARCHTYPE_SIZE = 1_000_000 / 8;
#else
        //public const uint ARCHTYPE_MANAGER_SIZE = 10_000_000;
        public const uint ARCHTYPE_SIZE = 4;
#endif
    }
}
