using System;
using System.IO;
using System.Xml;
namespace CommonAPI
{
	public class XmlReader
	{
		public static XmlDocument GetXmlDocByXmlContent(string xmlFileContent)
		{
			if (string.IsNullOrEmpty(xmlFileContent))
			{
				return null;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.LoadXml(xmlFileContent);
			}
			catch
			{
				xmlDocument = null;
			}
			return xmlDocument;
		}
		public static XmlDocument GetXmlDocByFilePath(string xmlFilePath)
		{
			if (string.IsNullOrEmpty(xmlFilePath) || !File.Exists(xmlFilePath))
			{
				return null;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(xmlFilePath);
			}
			catch
			{
				throw new Exception(string.Format("Confirm file path is correct ：{0}", xmlFilePath));
			}
			return xmlDocument;
		}
		public static XmlNode GetFirstChildNodeByName(XmlNode parentXmlNode, string childNodeName)
		{
			XmlNodeList childNodesByName = XmlReader.GetChildNodesByName(parentXmlNode, childNodeName);
			if (childNodesByName != null && childNodesByName.Count > 0)
			{
				return childNodesByName[0];
			}
			return null;
		}
		public static XmlNodeList GetChildNodesByName(XmlNode parentXmlNode, string nodeName)
		{
			if (parentXmlNode == null || string.IsNullOrEmpty(nodeName))
			{
				return null;
			}
			return XmlReader.GetChildNodesByXPathExpr(parentXmlNode, string.Format(".//{0}", nodeName));
		}
		public static XmlNodeList GetChildNodesByXPathExpr(XmlNode parentXmlNode, string xpathExpr)
		{
			if (parentXmlNode == null || string.IsNullOrEmpty(xpathExpr))
			{
				return null;
			}
			return parentXmlNode.SelectNodes(xpathExpr);
		}
		public static XmlNode GetFirstChildNode(XmlNode parentXmlNode)
		{
			XmlNodeList childNodes = XmlReader.GetChildNodes(parentXmlNode);
			if (childNodes != null && childNodes.Count > 0)
			{
				return childNodes[0];
			}
			return null;
		}
		public static XmlNodeList GetChildNodes(XmlNode parentXmlNode)
		{
			if (parentXmlNode != null)
			{
				return parentXmlNode.ChildNodes;
			}
			return null;
		}
		public static string ReadAttrValue(XmlNode xmlNode, string attrName)
		{
			XmlElement xmlElement = xmlNode as XmlElement;
			if (xmlElement != null)
			{
				return xmlElement.GetAttribute(attrName);
			}
			return null;
		}
		public static string ReadFirstAttrValue(XmlNode parentXmlNode, string childNodeName, string attrName)
		{
			string[] array = XmlReader.ReadAttrValues(parentXmlNode, childNodeName, attrName);
			if (array != null && array.Length != 0)
			{
				return array[0];
			}
			return null;
		}
		public static string[] ReadAttrValues(XmlNode parentXmlNode, string childNodeName, string attrName)
		{
			if (parentXmlNode == null || string.IsNullOrEmpty(childNodeName) || string.IsNullOrEmpty(attrName))
			{
				return null;
			}
			string xpathExpr = string.Format("//{0}[@{1}]", childNodeName, attrName);
			XmlNodeList childNodesByXPathExpr = XmlReader.GetChildNodesByXPathExpr(parentXmlNode, xpathExpr);
			if (childNodesByXPathExpr != null && childNodesByXPathExpr.Count > 0)
			{
				int count = childNodesByXPathExpr.Count;
				string[] array = new string[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = ((XmlElement)childNodesByXPathExpr[i]).GetAttribute(attrName);
				}
				return array;
			}
			return null;
		}
		public static string ReadFirstChildNodeTextByName(XmlNode parentXmlNode, string childNodeName)
		{
			string[] array = XmlReader.ReadChildNodeTextsByName(parentXmlNode, childNodeName);
			if (array != null && array.Length > 0)
			{
				return array[0];
			}
			return null;
		}
		public static string[] ReadChildNodeTextsByName(XmlNode parentXmlNode, string childNodeName)
		{
			if (parentXmlNode == null || string.IsNullOrEmpty(childNodeName))
			{
				return null;
			}
			string xpathExpr = string.Format(".//{0}", childNodeName);
			XmlNodeList childNodesByXPathExpr = XmlReader.GetChildNodesByXPathExpr(parentXmlNode, xpathExpr);
			if (childNodesByXPathExpr != null && childNodesByXPathExpr.Count > 0)
			{
				int count = childNodesByXPathExpr.Count;
				string[] array = new string[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = childNodesByXPathExpr[i].InnerText;
				}
				return array;
			}
			return null;
		}
		public static string ReadFirstChildNodeText(XmlNode parentXmlNode)
		{
			string[] array = XmlReader.ReadChildNodeTexts(parentXmlNode);
			if (array != null && array.Length > 0)
			{
				return array[0];
			}
			return null;
		}
		public static string[] ReadChildNodeTexts(XmlNode parentXmlNode)
		{
			if (parentXmlNode == null)
			{
				return null;
			}
			XmlNodeList childNodes = XmlReader.GetChildNodes(parentXmlNode);
			if (childNodes != null && childNodes.Count > 0)
			{
				int count = childNodes.Count;
				string[] array = new string[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = childNodes[i].InnerText;
				}
				return array;
			}
			return null;
		}
		public static string ReadNodeText(XmlNode xmlNode)
		{
			if (xmlNode == null)
			{
				return null;
			}
			return xmlNode.InnerText;
		}
	}
}
