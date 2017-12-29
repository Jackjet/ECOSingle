using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveDeviceValueMib
	{
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string bankStatusValueEntry = ".2.3";
		public static string outletStatusValueEntry = ".2.3";
		public static string bankValueEntry = ".3.2.1";
		public static string outletValueEntry = ".2.1.1";
		public static string sensorValueEntry = ".1.4.1";
		public static string deviceValueEntry = ".1.3.1";
		public static string Current = ".1.3.1.2";
		public static string Voltage = ".1.3.1.3";
		public static string Power = ".1.3.1.4";
		public static string PowerDissipation = ".1.3.1.5";
		public string SlaveCurrent
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.Current;
			}
		}
		public string SlaveVoltage
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.Voltage;
			}
		}
		public string SlavePower
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.Power;
			}
		}
		public string SlavePowerDissipation
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.PowerDissipation;
			}
		}
		public string DeviceValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.deviceValueEntry;
			}
		}
		public string SensorValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.sensorValueEntry;
			}
		}
		public string OutletValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.outletValueEntry;
			}
		}
		public string BankValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.bankValueEntry;
			}
		}
		public string OutletStatusValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.outletStatusValueEntry;
			}
		}
		public string BankStatusValueEntry
		{
			get
			{
				return SlaveDeviceValueMib.Entry + this.slave + SlaveDeviceValueMib.bankStatusValueEntry;
			}
		}
		public SlaveDeviceValueMib(int index)
		{
			this.slave += index;
		}
	}
}
