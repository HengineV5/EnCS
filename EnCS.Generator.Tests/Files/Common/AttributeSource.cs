
using System;

namespace EnCS.Attributes
{
	public class ComponentAttribute : System.Attribute
	{

	}

	public class ArchTypeAttribute : System.Attribute
	{

	}

	public class ResourceManagerAttribute : System.Attribute
	{

	}

	public class SystemAttribute : System.Attribute
	{

	}

	public class SystemContextAttribute<T1> : System.Attribute where T1 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2> : System.Attribute where T1 : unmanaged where T2 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3> : System.Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
	{

	}

	public class SystemContextAttribute<T1, T2, T3, T4> : System.Attribute where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
	{

	}

	public class SystemLayerAttribute : System.Attribute
	{
		public SystemLayerAttribute(int layer)
		{
            
		}

		public SystemLayerAttribute(int layer, int chunk)
		{

		}
	}

	public class SystemUpdateAttribute : System.Attribute
	{

	}

	public class SystemPreLoopAttribute : System.Attribute
	{

	}

	public class SystemPostLoopAttribute : System.Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class UsingResourceAttribute<T> : System.Attribute
	{

	}
}
