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

	public class SystemContextAttribute<T1> : Attribute where T1 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2> : Attribute where T1 : unmanaged where T2 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3> : Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3, T4> : Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
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
