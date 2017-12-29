using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonSensorContactMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.7.3.1";
		public string ContactStatus
		{
			get
			{
				return EatonSensorContactMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public EatonSensorContactMib_M2(int sensorNum)
		{
			this.index = sensorNum;
		}
	}
}
