using System;
namespace EcoDevice.AccessAPI
{
	internal class DeviceEnvironmentMib
	{
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.1.6.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.1.6.1.2";
		public static string MinCurrentMt = DeviceEnvironmentMib.Entry + ".2.1";
		public static string MaxCurrentMT = DeviceEnvironmentMib.Entry + ".3.1";
		public static string MinVoltageMT = DeviceEnvironmentMib.Entry + ".4.1";
		public static string MaxVoltageMT = DeviceEnvironmentMib.Entry + ".5.1";
		public static string MinPowerMT = DeviceEnvironmentMib.Entry + ".6.1";
		public static string MaxPowerMT = DeviceEnvironmentMib.Entry + ".7.1";
		public static string MaxPowerDissMT = DeviceEnvironmentMib.Entry + ".8.1";
		public static string PopEnableMode = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.1.0";
		public static string PopThreshold = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.2.0";
		public static string PopModeOutlet = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.3.0";
		public static string PopModeLIFO = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.4.0";
		public static string PopModePriority = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.5.0";
		public static string PopPriorityList = "1.3.6.1.4.1.21317.1.3.2.2.2.1.17.6.0";
		public static string devReferenceVoltage = "1.3.6.1.4.1.21317.1.3.2.2.2.3.2.1.3.1";
		public static string DoorSensorType = "1.3.6.1.4.1.21317.1.3.2.2.2.1.16.2.0";
	}
}
