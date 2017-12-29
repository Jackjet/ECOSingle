using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
namespace CustomXmlSerialization
{
	public class CustomXmlDeserializer : CustomXmlSerializerBase
	{
		public interface ITypeConverter
		{
			void ProcessType(ref string assemblyFullName, ref string typeFullName);
		}
		private CultureInfo cult;
		private Dictionary<int, Type> deserializationTypeCache;
		private Dictionary<int, object> deserializationObjCache = new Dictionary<int, object>();
		private CustomXmlDeserializer.ITypeConverter typeConverter;
		protected CustomXmlDeserializer(CustomXmlDeserializer.ITypeConverter typeConverter)
		{
			this.typeConverter = typeConverter;
		}
		public static object Deserialize(string xml, int maxSupportedVer)
		{
			return CustomXmlDeserializer.Deserialize(xml, maxSupportedVer, null);
		}
		public static object Deserialize(string xml, int maxSupportedVer, CustomXmlDeserializer.ITypeConverter typeConverter)
		{
			CustomXmlDeserializer customXmlDeserializer = new CustomXmlDeserializer(typeConverter);
			customXmlDeserializer.doc.LoadXml(xml);
			string attribute = customXmlDeserializer.doc.DocumentElement.GetAttribute("version");
			if (maxSupportedVer < Convert.ToInt32(attribute))
			{
				return null;
			}
			string attribute2 = customXmlDeserializer.doc.DocumentElement.GetAttribute("culture");
			customXmlDeserializer.cult = new CultureInfo(attribute2);
			return customXmlDeserializer.DeserializeCore(customXmlDeserializer.doc.DocumentElement);
		}
		private void DeserializeComplexType(object obj, Type objType, XmlNode firstChild)
		{
			IDictionary<string, FieldInfo> typeFieldInfo = CustomXmlSerializerBase.GetTypeFieldInfo(objType);
			for (XmlNode xmlNode = firstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				string name = xmlNode.Name;
				FieldInfo fieldInfo = null;
				if (typeFieldInfo.TryGetValue(name, out fieldInfo))
				{
					object value = this.DeserializeCore((XmlElement)xmlNode);
					fieldInfo.SetValue(obj, value);
				}
			}
		}
		private void LoadTypeCache(XmlElement element)
		{
			XmlNodeList elementsByTagName = element.GetElementsByTagName("TypeInfo");
			this.deserializationTypeCache = new Dictionary<int, Type>(elementsByTagName.Count);
			foreach (XmlElement xmlElement in elementsByTagName)
			{
				int key = Convert.ToInt32(xmlElement.GetAttribute("typeid"));
				Type value = this.InferTypeFromElement(xmlElement);
				this.deserializationTypeCache.Add(key, value);
			}
		}
		private object DeserializeCore(XmlElement element)
		{
			int num;
			if (int.TryParse(element.GetAttribute("id"), out num))
			{
				object objFromCache = this.GetObjFromCache(num);
				if (objFromCache != null)
				{
					return objFromCache;
				}
			}
			else
			{
				num = -1;
			}
			string attribute = element.GetAttribute("value");
			if (attribute == "null")
			{
				return null;
			}
			int num2 = element.ChildNodes.Count;
			XmlNode xmlNode = element.FirstChild;
			if (element.GetAttribute("hasTypeCache") == "true")
			{
				this.LoadTypeCache((XmlElement)xmlNode);
				num2--;
				xmlNode = xmlNode.NextSibling;
			}
			string attribute2 = element.GetAttribute("typeid");
			Type type;
			if (string.IsNullOrEmpty(attribute2))
			{
				type = this.InferTypeFromElement(element);
			}
			else
			{
				type = this.deserializationTypeCache[Convert.ToInt32(attribute2)];
			}
			if (type.IsEnum)
			{
				long value = Convert.ToInt64(attribute, this.cult);
				return Enum.ToObject(type, value);
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.DBNull:
				return DBNull.Value;
			case TypeCode.Boolean:
				return Convert.ToBoolean(attribute, this.cult);
			case TypeCode.Char:
				return Convert.ToChar(attribute, this.cult);
			case TypeCode.SByte:
				return Convert.ToSByte(attribute, this.cult);
			case TypeCode.Byte:
				return Convert.ToByte(attribute, this.cult);
			case TypeCode.Int16:
				return Convert.ToInt16(attribute, this.cult);
			case TypeCode.UInt16:
				return Convert.ToUInt16(attribute, this.cult);
			case TypeCode.Int32:
				return Convert.ToInt32(attribute, this.cult);
			case TypeCode.UInt32:
				return Convert.ToUInt32(attribute, this.cult);
			case TypeCode.Int64:
				return Convert.ToInt64(attribute, this.cult);
			case TypeCode.UInt64:
				return Convert.ToUInt64(attribute, this.cult);
			case TypeCode.Single:
				return Convert.ToSingle(attribute, this.cult);
			case TypeCode.Double:
				return Convert.ToDouble(attribute, this.cult);
			case TypeCode.Decimal:
				return Convert.ToDecimal(attribute, this.cult);
			case TypeCode.DateTime:
				return Convert.ToDateTime(attribute, this.cult);
			case TypeCode.String:
				return attribute;
			}
			object obj;
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				MethodInfo method = type.GetMethod("Set", new Type[]
				{
					typeof(int),
					elementType
				});
				ConstructorInfo constructor = type.GetConstructor(new Type[]
				{
					typeof(int)
				});
				obj = constructor.Invoke(new object[]
				{
					num2
				});
				if (num >= 0)
				{
					this.deserializationObjCache.Add(num, obj);
				}
				int num3 = 0;
				foreach (object current in this.ValuesFromNode(xmlNode))
				{
					method.Invoke(obj, new object[]
					{
						num3,
						current
					});
					num3++;
				}
				return obj;
			}
			obj = Activator.CreateInstance(type, true);
			if (num >= 0)
			{
				this.deserializationObjCache.Add(num, obj);
			}
			IXmlSerializable xmlSerializable = obj as IXmlSerializable;
			if (xmlSerializable == null)
			{
				IList list = obj as IList;
				if (list == null)
				{
					IDictionary dictionary = obj as IDictionary;
					if (dictionary == null)
					{
						if (type == typeof(DictionaryEntry) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >)))
						{
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>(element.ChildNodes.Count);
							for (XmlNode xmlNode2 = xmlNode; xmlNode2 != null; xmlNode2 = xmlNode2.NextSibling)
							{
								object value2 = this.DeserializeCore((XmlElement)xmlNode2);
								dictionary2.Add(xmlNode2.Name, value2);
							}
							return dictionary2;
						}
						this.DeserializeComplexType(obj, type, xmlNode);
						return obj;
					}
					else
					{
						IEnumerator enumerator2 = this.ValuesFromNode(xmlNode).GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object current2 = enumerator2.Current;
								Dictionary<string, object> dictionary3 = (Dictionary<string, object>)current2;
								if (dictionary3.ContainsKey("key"))
								{
									dictionary.Add(dictionary3["key"], dictionary3["value"]);
								}
								else
								{
									dictionary.Add(dictionary3["_key"], dictionary3["_value"]);
								}
							}
							return obj;
						}
						finally
						{
							IDisposable disposable2 = enumerator2 as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
						}
					}
				}
				IEnumerator enumerator3 = this.ValuesFromNode(xmlNode).GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object current3 = enumerator3.Current;
						list.Add(current3);
					}
					return obj;
				}
				finally
				{
					IDisposable disposable3 = enumerator3 as IDisposable;
					if (disposable3 != null)
					{
						disposable3.Dispose();
					}
				}
			}
			StringReader stringReader = new StringReader(element.InnerXml);
			XmlReader xmlReader = XmlReader.Create(stringReader);
			xmlSerializable.ReadXml(xmlReader);
			xmlReader.Close();
			stringReader.Close();
			return obj;
		}
		private IEnumerable ValuesFromNode(XmlNode firstChild)
		{
			for (XmlNode xmlNode = firstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				yield return this.DeserializeCore((XmlElement)xmlNode);
			}
			yield break;
		}
		private object GetObjFromCache(int objId)
		{
			object result;
			if (this.deserializationObjCache.TryGetValue(objId, out result))
			{
				return result;
			}
			return null;
		}
		private Type InferTypeFromElement(XmlElement element)
		{
			string attribute = element.GetAttribute("type");
			string attribute2 = element.GetAttribute("assembly");
			if (this.typeConverter != null)
			{
				this.typeConverter.ProcessType(ref attribute2, ref attribute);
			}
			Type type;
			if (string.IsNullOrEmpty(attribute2))
			{
				type = Type.GetType(attribute, true);
			}
			else
			{
				Assembly assembly = Assembly.Load(attribute2);
				type = assembly.GetType(attribute, true);
			}
			return type;
		}
	}
}
