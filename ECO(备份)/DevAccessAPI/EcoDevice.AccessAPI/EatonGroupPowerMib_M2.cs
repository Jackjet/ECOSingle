using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonGroupPowerMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.5.5.1";
		public string PowerValue
		{
			get
			{
				return EatonGroupPowerMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public EatonGroupPowerMib_M2(int groupNum)
		{
			this.index = groupNum;
		}
	}
}
