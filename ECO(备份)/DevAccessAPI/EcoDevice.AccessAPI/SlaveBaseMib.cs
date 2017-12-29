using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveBaseMib
	{
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string ModelName = ".1.1.0";
		public static string DeviceName = ".1.2.0";
		public static string SlaveMac = ".1.8.0";
		public string SlaveModelName
		{
			get
			{
				return SlaveBaseMib.Entry + this.slave + SlaveBaseMib.ModelName;
			}
		}
		public string SlaveDeviceName
		{
			get
			{
				return SlaveBaseMib.Entry + this.slave + SlaveBaseMib.DeviceName;
			}
		}
		public string SlaveDeviceMac
		{
			get
			{
				return SlaveBaseMib.Entry + this.slave + SlaveBaseMib.SlaveMac;
			}
		}
		public SlaveBaseMib(int index)
		{
			this.slave += index;
		}
	}
}
