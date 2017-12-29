using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
namespace CustomXmlSerialization
{
	public class CustomXmlSerializer : CustomXmlSerializerBase
	{
		private class ObjInfo
		{
			internal int Id;
			internal XmlElement OnlyElement;
			internal void WriteObjId(XmlElement element)
			{
				element.SetAttribute("id", this.Id.ToString());
			}
		}
		private struct ObjKeyForCache : IEquatable<CustomXmlSerializer.ObjKeyForCache>
		{
			private object m_obj;
			public ObjKeyForCache(object obj)
			{
				this.m_obj = obj;
			}
			public bool Equals(CustomXmlSerializer.ObjKeyForCache other)
			{
				return object.ReferenceEquals(this.m_obj, other.m_obj);
			}
		}
		public class SerializationOptions
		{
			public bool UseTypeCache = true;
			public bool UseGraphSerialization = true;
		}
		private Dictionary<Type, CustomXmlSerializerBase.TypeInfo> typeCache = new Dictionary<Type, CustomXmlSerializerBase.TypeInfo>();
		private Dictionary<Type, IDictionary<CustomXmlSerializer.ObjKeyForCache, CustomXmlSerializer.ObjInfo>> objCache = new Dictionary<Type, IDictionary<CustomXmlSerializer.ObjKeyForCache, CustomXmlSerializer.ObjInfo>>();
		private int objCacheNextId;
		private CustomXmlSerializer.SerializationOptions options;
		protected CustomXmlSerializer(CustomXmlSerializer.SerializationOptions opt)
		{
			this.options = opt;
		}
		private void SetTypeInfo(Type objType, XmlElement element)
		{
			if (!this.options.UseTypeCache)
			{
				CustomXmlSerializer.WriteTypeToNode(element, objType);
				return;
			}
			CustomXmlSerializerBase.TypeInfo typeInfo;
			if (this.typeCache.TryGetValue(objType, out typeInfo))
			{
				XmlElement onlyElement = typeInfo.OnlyElement;
				if (onlyElement != null)
				{
					typeInfo.WriteTypeId(onlyElement);
					onlyElement.RemoveAttribute("type");
					onlyElement.RemoveAttribute("assembly");
					typeInfo.OnlyElement = null;
				}
				typeInfo.WriteTypeId(element);
				return;
			}
			typeInfo = new CustomXmlSerializerBase.TypeInfo();
			typeInfo.TypeId = this.typeCache.Count;
			typeInfo.OnlyElement = element;
			this.typeCache.Add(objType, typeInfo);
			CustomXmlSerializer.WriteTypeToNode(element, objType);
		}
		private static void WriteTypeToNode(XmlElement element, Type objType)
		{
			element.SetAttribute("type", objType.FullName);
			element.SetAttribute("assembly", objType.Assembly.FullName);
		}
		private XmlElement GetTypeInfoNode()
		{
			XmlElement xmlElement = this.doc.CreateElement("TypeCache");
			foreach (KeyValuePair<Type, CustomXmlSerializerBase.TypeInfo> current in this.typeCache)
			{
				if (current.Value.OnlyElement == null)
				{
					XmlElement xmlElement2 = this.doc.CreateElement("TypeInfo");
					current.Value.WriteTypeId(xmlElement2);
					CustomXmlSerializer.WriteTypeToNode(xmlElement2, current.Key);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (!xmlElement.HasChildNodes)
			{
				return null;
			}
			return xmlElement;
		}
		public static XmlDocument Serialize(object obj, int ver, string rootName)
		{
			CustomXmlSerializer.SerializationOptions opt = new CustomXmlSerializer.SerializationOptions();
			if (obj != null)
			{
				Type type = obj.GetType();
				object[] customAttributes = type.GetCustomAttributes(typeof(CustomXmlSerializationOptionsAttribute), false);
				if (customAttributes.Length > 0)
				{
					opt = ((CustomXmlSerializationOptionsAttribute)customAttributes[0]).SerializationOptions;
				}
			}
			CustomXmlSerializer customXmlSerializer = new CustomXmlSerializer(opt);
			XmlElement xmlElement = customXmlSerializer.SerializeCore(rootName, obj);
			xmlElement.SetAttribute("version", ver.ToString());
			xmlElement.SetAttribute("culture", Thread.CurrentThread.CurrentCulture.ToString());
			XmlElement typeInfoNode = customXmlSerializer.GetTypeInfoNode();
			if (typeInfoNode != null)
			{
				xmlElement.PrependChild(typeInfoNode);
				xmlElement.SetAttribute("hasTypeCache", "true");
			}
			customXmlSerializer.doc.AppendChild(xmlElement);
			return customXmlSerializer.doc;
		}
		private bool AddObjToCache(Type objType, object obj, XmlElement element)
		{
			CustomXmlSerializer.ObjKeyForCache key = new CustomXmlSerializer.ObjKeyForCache(obj);
			IDictionary<CustomXmlSerializer.ObjKeyForCache, CustomXmlSerializer.ObjInfo> dictionary;
			if (this.objCache.TryGetValue(objType, out dictionary))
			{
				CustomXmlSerializer.ObjInfo objInfo;
				if (dictionary.TryGetValue(key, out objInfo))
				{
					if (objInfo.OnlyElement != null)
					{
						objInfo.WriteObjId(objInfo.OnlyElement);
						objInfo.OnlyElement = null;
					}
					objInfo.WriteObjId(element);
					return false;
				}
			}
			else
			{
				dictionary = new Dictionary<CustomXmlSerializer.ObjKeyForCache, CustomXmlSerializer.ObjInfo>(1);
				this.objCache.Add(objType, dictionary);
			}
			CustomXmlSerializer.ObjInfo objInfo2 = new CustomXmlSerializer.ObjInfo();
			objInfo2.Id = this.objCacheNextId;
			objInfo2.OnlyElement = element;
			dictionary.Add(key, objInfo2);
			this.objCacheNextId++;
			return true;
		}
		private static bool CheckForcedSerialization(Type objType)
		{
			object[] customAttributes = objType.GetCustomAttributes(typeof(XmlSerializeAsCustomTypeAttribute), false);
			return customAttributes.Length > 0;
		}
		private XmlElement SerializeCore(string name, object obj)
		{
			XmlElement xmlElement = this.doc.CreateElement(name);
			if (obj == null)
			{
				xmlElement.SetAttribute("value", "null");
				return xmlElement;
			}
			Type type = obj.GetType();
			if (type.IsClass && type != typeof(string))
			{
				if (this.options.UseGraphSerialization && !this.AddObjToCache(type, obj, xmlElement))
				{
					return xmlElement;
				}
				this.SetTypeInfo(type, xmlElement);
				if (CustomXmlSerializer.CheckForcedSerialization(type))
				{
					this.SerializeComplexType(obj, xmlElement);
					return xmlElement;
				}
				IXmlSerializable xmlSerializable = obj as IXmlSerializable;
				if (xmlSerializable == null)
				{
					IEnumerable enumerable = obj as IEnumerable;
					if (enumerable == null)
					{
						this.SerializeComplexType(obj, xmlElement);
						return xmlElement;
					}
					IEnumerator enumerator = enumerable.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object current = enumerator.Current;
							XmlElement newChild = this.SerializeCore(name, current);
							xmlElement.AppendChild(newChild);
						}
						return xmlElement;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
					Encoding = Encoding.UTF8,
					OmitXmlDeclaration = true
				});
				xmlWriter.WriteStartElement("value");
				xmlSerializable.WriteXml(xmlWriter);
				xmlWriter.WriteEndElement();
				xmlWriter.Close();
				xmlElement.InnerXml = stringBuilder.ToString();
			}
			else
			{
				this.SetTypeInfo(type, xmlElement);
				if (CustomXmlSerializer.CheckForcedSerialization(type))
				{
					this.SerializeComplexType(obj, xmlElement);
					return xmlElement;
				}
				if (type.IsEnum)
				{
					object obj2 = Enum.Format(type, obj, "d");
					xmlElement.SetAttribute("value", obj2.ToString());
				}
				else
				{
					if (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal))
					{
						xmlElement.SetAttribute("value", obj.ToString());
					}
					else
					{
						this.SerializeComplexType(obj, xmlElement);
					}
				}
			}
			return xmlElement;
		}
		private void SerializeComplexType(object obj, XmlElement element)
		{
			Type type = obj.GetType();
			IDictionary<string, FieldInfo> typeFieldInfo = CustomXmlSerializerBase.GetTypeFieldInfo(type);
			foreach (KeyValuePair<string, FieldInfo> current in typeFieldInfo)
			{
				XmlElement newChild = this.SerializeCore(current.Key, current.Value.GetValue(obj));
				element.AppendChild(newChild);
			}
		}
	}
}
