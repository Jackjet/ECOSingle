using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonInputPowerMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.3.4.1";
		public string PowerValue
		{
			get
			{
				return EatonInputPowerMib_M2.Entry + ".4.0.1." + this.index;
			}
		}
		public string PowerValue_VA
		{
			get
			{
				return EatonInputPowerMib_M2.Entry + ".3.0.1." + this.index;
			}
		}
		public EatonInputPowerMib_M2(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
