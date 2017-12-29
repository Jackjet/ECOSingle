using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveDeviceEnvironmentMib
	{
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string sensorConfigEntry = ".1.7.1";
		public static string outletConfigEntry = ".2.2.1";
		public static string bankConfigEntry = ".3.3.1";
		public static string deviceConfigEntry = ".1.6.1";
		public static string MinCurrentMt = ".1.6.1.2";
		public static string MaxCurrentMT = ".1.6.1.3";
		public static string MinVoltageMT = ".1.6.1.4";
		public static string MaxVoltageMT = ".1.6.1.5";
		public static string MinPowerMT = ".1.6.1.6";
		public static string MaxPowerMT = ".1.6.1.7";
		public static string MaxPowerDissMT = ".1.6.1.8";
		public string SlaveMinCurrentMt
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MinCurrentMt;
			}
		}
		public string SlaveMaxCurrentMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MaxCurrentMT;
			}
		}
		public string SlaveMinVoltageMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MinVoltageMT;
			}
		}
		public string SlaveMaxVoltageMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MaxVoltageMT;
			}
		}
		public string SlaveMinPowerMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MinPowerMT;
			}
		}
		public string SlaveMaxPowerMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MaxPowerMT;
			}
		}
		public string SlaveMaxPowerDissMT
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.MaxPowerDissMT;
			}
		}
		public string DeviceConfigEntry
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.deviceConfigEntry;
			}
		}
		public string SensorConfigEntry
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.sensorConfigEntry;
			}
		}
		public string OutletConfigEntry
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.outletConfigEntry;
			}
		}
		public string BankConfigEntry
		{
			get
			{
				return SlaveDeviceEnvironmentMib.Entry + this.slave + SlaveDeviceEnvironmentMib.bankConfigEntry;
			}
		}
		public SlaveDeviceEnvironmentMib(int index)
		{
			this.slave += index;
		}
	}
}
