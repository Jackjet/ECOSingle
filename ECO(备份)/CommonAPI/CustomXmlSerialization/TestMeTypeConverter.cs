using System;
namespace CustomXmlSerialization
{
	public class TestMeTypeConverter : CustomXmlDeserializer.ITypeConverter
	{
		public void ProcessType(ref string assemblyFullName, ref string typeFullName)
		{
			string a;
			if ((a = assemblyFullName) != null)
			{
				if (!(a == "OldTestMe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"))
				{
					return;
				}
				assemblyFullName = "TestMe, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null";
				if (typeFullName.StartsWith("OldTestMe.VirtualList"))
				{
					assemblyFullName = "CommonTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
					typeFullName = typeFullName.Replace("OldTestMe", "CommonTools.Lists");
				}
			}
		}
	}
}
