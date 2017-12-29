using System;
namespace CustomXmlSerialization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class CustomXmlSerializationOptionsAttribute : Attribute
	{
		public CustomXmlSerializer.SerializationOptions SerializationOptions = new CustomXmlSerializer.SerializationOptions();
		public CustomXmlSerializationOptionsAttribute(bool useTypeCache, bool useGraphSerialization)
		{
			this.SerializationOptions.UseTypeCache = useTypeCache;
			this.SerializationOptions.UseGraphSerialization = useGraphSerialization;
		}
	}
}
