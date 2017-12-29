using System;
namespace EcoDevice.AccessAPI
{
	internal class DeviceValueMib
	{
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.1.3.1";
		public static string Current = DeviceValueMib.Entry + ".2.1";
		public static string Voltage = DeviceValueMib.Entry + ".3.1";
		public static string Power = DeviceValueMib.Entry + ".4.1";
		public static string PowerDissipation = DeviceValueMib.Entry + ".5.1";
		public static string DoorSensorStatus = "1.3.6.1.4.1.21317.1.3.2.2.2.1.16.1.0";
		public static string LeakCurrentStatus = "1.3.6.1.4.1.21317.1.3.2.2.2.4.4.0";
	}
}
