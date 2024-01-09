using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnCS.Attributes
{
	public class SystemAttribute : Attribute
	{

	}

	public class SystemAttribute<T> : Attribute where T : unmanaged
	{

	}

	public class SystemUpdateAttribute : Attribute
	{

	}

	public class SystemLayerAttribute : Attribute
	{
        public SystemLayerAttribute(int layer)
        {
            
        }

		public SystemLayerAttribute(int layer, int chunk)
		{

		}
	}

	public class SystemPreLoopAttribute : Attribute
	{

	}

	public class SystemPostLoopAttribute : Attribute
	{

	}
}
