using System;
namespace EcoDevice.AccessAPI
{
	public abstract class AbstractSocketData
	{
		private string type;
		private string target;
		private int port;
		private int dataLenth;
		private byte[] dataBytes;
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public int DataLenth
		{
			get
			{
				return this.dataLenth;
			}
			set
			{
				this.dataLenth = value;
			}
		}
		public byte[] DataBytes
		{
			get
			{
				return this.dataBytes;
			}
			set
			{
				this.dataBytes = value;
			}
		}
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public abstract string getAssamble();
	}
}
