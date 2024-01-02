namespace EnCS.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	public class UsingResourceAttribute<T> : Attribute
	{

	}
}
