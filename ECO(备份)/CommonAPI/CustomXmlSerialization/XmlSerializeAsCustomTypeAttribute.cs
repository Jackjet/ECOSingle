using System;
namespace CustomXmlSerialization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class XmlSerializeAsCustomTypeAttribute : Attribute
	{
	}
}
