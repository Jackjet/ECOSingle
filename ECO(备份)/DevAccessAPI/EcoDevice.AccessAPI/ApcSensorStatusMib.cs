using System;
namespace EcoDevice.AccessAPI
{
	internal class ApcSensorStatusMib
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.318.1.1.26.10.2.2.1";
		public string Temperature
		{
			get
			{
				return ApcSensorStatusMib.Entry + ".8." + this.index;
			}
		}
		public string Humidity
		{
			get
			{
				return ApcSensorStatusMib.Entry + ".10." + this.index;
			}
		}
		public ApcSensorStatusMib(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
