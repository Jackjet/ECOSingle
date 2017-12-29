using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
namespace CustomXmlSerialization
{
	public abstract class CustomXmlSerializerBase
	{
		protected class TypeInfo
		{
			internal int TypeId;
			internal XmlElement OnlyElement;
			internal void WriteTypeId(XmlElement element)
			{
				element.SetAttribute("typeid", this.TypeId.ToString());
			}
		}
		private static Dictionary<string, IDictionary<string, FieldInfo>> fieldInfoCache = new Dictionary<string, IDictionary<string, FieldInfo>>();
		protected XmlDocument doc = new XmlDocument();
		protected static IDictionary<string, FieldInfo> GetTypeFieldInfo(Type objType)
		{
			string fullName = objType.FullName;
			IDictionary<string, FieldInfo> dictionary;
			if (!CustomXmlSerializerBase.fieldInfoCache.TryGetValue(fullName, out dictionary))
			{
				FieldInfo[] fields = objType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				Dictionary<string, FieldInfo> dictionary2 = new Dictionary<string, FieldInfo>(fields.Length);
				FieldInfo[] array = fields;
				for (int i = 0; i < array.Length; i++)
				{
					FieldInfo fieldInfo = array[i];
					if (!fieldInfo.FieldType.IsSubclassOf(typeof(MulticastDelegate)))
					{
						object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);
						if (customAttributes.Length == 0)
						{
							dictionary2.Add(fieldInfo.Name, fieldInfo);
						}
					}
				}
				Type baseType = objType.BaseType;
				if (baseType != null && baseType != typeof(object))
				{
					object[] customAttributes2 = baseType.GetCustomAttributes(typeof(XmlIgnoreBaseTypeAttribute), false);
					if (customAttributes2.Length == 0)
					{
						IDictionary<string, FieldInfo> typeFieldInfo = CustomXmlSerializerBase.GetTypeFieldInfo(baseType);
						foreach (KeyValuePair<string, FieldInfo> current in typeFieldInfo)
						{
							string text = current.Key;
							if (dictionary2.ContainsKey(text))
							{
								text = "base." + text;
							}
							dictionary2.Add(text, current.Value);
						}
					}
				}
				dictionary = dictionary2;
				CustomXmlSerializerBase.fieldInfoCache.Add(fullName, dictionary);
			}
			return dictionary;
		}
	}
}
