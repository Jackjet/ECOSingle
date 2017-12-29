using System;
using System.Reflection;
namespace EcoDevice.AccessAPI
{
	public class AttributeHelper
	{
		public static System.Reflection.FieldInfo[] getFieldInfos(System.Type type)
		{
			System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;
			return type.GetFields(bindingAttr);
		}
		public static bool needEncoded(System.Reflection.FieldInfo fieldInfo)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(Encode), false);
			return AttributeHelper.needEncoded(customAttributes);
		}
		public static bool needEncoded(System.Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(false);
			return AttributeHelper.needEncoded(customAttributes);
		}
		private static bool needEncoded(object[] attributes)
		{
			if (attributes == null || attributes.Length <= 0)
			{
				return true;
			}
			for (int i = 0; i < attributes.Length; i++)
			{
				System.Attribute attribute = (System.Attribute)attributes[i];
				if (attribute is Encode)
				{
					Encode encode = attribute as Encode;
					bool result;
					if (encode.NeedEncoded)
					{
						result = true;
					}
					else
					{
						result = false;
					}
					return result;
				}
			}
			return false;
		}
	}
}
