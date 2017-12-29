using System;
namespace EcoSensors.Common
{
	internal class Combobox_item
	{
		private string m_key;
		private string m_value;
		public Combobox_item(string key, string value)
		{
			this.m_key = key;
			this.m_value = value;
		}
		public string getKey()
		{
			return this.m_key;
		}
		public string getValue()
		{
			return this.m_value;
		}
		public override string ToString()
		{
			return this.m_value;
		}
	}
}
