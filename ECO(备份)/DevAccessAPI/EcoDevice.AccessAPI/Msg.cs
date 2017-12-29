using System;
namespace EcoDevice.AccessAPI
{
	public abstract class Msg
	{
		private string devName = string.Empty;
		private string devModel = string.Empty;
		private string ipAddress = string.Empty;
		private System.DateTime createTime;
		private int portNums;
		private int sensorNums;
		private int bankNums;
		private int lineNums;
		private int switchable;
		private int perportReading;
		private int perbankReading;
		private int perdoorReading;
		private string devFWVer = string.Empty;
		private string autoBasicInfo = string.Empty;
		private string autoRatingInfo = string.Empty;
		private string devReplyMac = string.Empty;
		private string devMac = string.Empty;
		private int devID;
		private int chainNums;
		private string masterMac = string.Empty;
		private int masterID;
		private int slaveIndex;
		public string DeviceName
		{
			get
			{
				return this.devName;
			}
			set
			{
				this.devName = value;
			}
		}
		public string ModelName
		{
			get
			{
				return this.devModel;
			}
			set
			{
				this.devModel = value;
			}
		}
		public string IpAddress
		{
			get
			{
				return this.ipAddress;
			}
			set
			{
				this.ipAddress = value;
			}
		}
		public string DeviceMac
		{
			get
			{
				return this.devMac;
			}
			set
			{
				this.devMac = value;
			}
		}
		public string DeviceReplyMac
		{
			get
			{
				return this.devReplyMac;
			}
			set
			{
				this.devReplyMac = value;
			}
		}
		public int DeviceID
		{
			get
			{
				return this.devID;
			}
			set
			{
				this.devID = value;
			}
		}
		public string DeviceFWVer
		{
			get
			{
				return this.devFWVer;
			}
			set
			{
				this.devFWVer = value;
			}
		}
		public string AutoBasicInfo
		{
			get
			{
				return this.autoBasicInfo;
			}
			set
			{
				this.autoBasicInfo = value;
			}
		}
		public string AutoRatingInfo
		{
			get
			{
				return this.autoRatingInfo;
			}
			set
			{
				this.autoRatingInfo = value;
			}
		}
		public int PortNums
		{
			get
			{
				return this.portNums;
			}
			set
			{
				this.portNums = value;
			}
		}
		public int Switchable
		{
			get
			{
				return this.switchable;
			}
			set
			{
				this.switchable = value;
			}
		}
		public int PerPortReading
		{
			get
			{
				return this.perportReading;
			}
			set
			{
				this.perportReading = value;
			}
		}
		public int PerBankReading
		{
			get
			{
				return this.perbankReading;
			}
			set
			{
				this.perbankReading = value;
			}
		}
		public int PerDoorReading
		{
			get
			{
				return this.perdoorReading;
			}
			set
			{
				this.perdoorReading = value;
			}
		}
		public int SensorNums
		{
			get
			{
				return this.sensorNums;
			}
			set
			{
				this.sensorNums = value;
			}
		}
		public int BankNums
		{
			get
			{
				return this.bankNums;
			}
			set
			{
				this.bankNums = value;
			}
		}
		public int LineNums
		{
			get
			{
				return this.lineNums;
			}
			set
			{
				this.lineNums = value;
			}
		}
		public System.DateTime CreateTime
		{
			get
			{
				return this.createTime;
			}
			set
			{
				this.createTime = value;
			}
		}
		public int ChainNums
		{
			get
			{
				return this.chainNums;
			}
			set
			{
				this.chainNums = value;
			}
		}
		public string MasterMac
		{
			get
			{
				return this.masterMac;
			}
			set
			{
				this.masterMac = value;
			}
		}
		public int MasterID
		{
			get
			{
				return this.masterID;
			}
			set
			{
				this.masterID = value;
			}
		}
		public int SlaveIndex
		{
			get
			{
				return this.slaveIndex;
			}
			set
			{
				this.slaveIndex = value;
			}
		}
	}
}
