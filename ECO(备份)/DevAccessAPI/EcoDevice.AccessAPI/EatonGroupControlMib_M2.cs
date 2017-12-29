using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonGroupControlMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.5.6.1";
		public string ControlStatus
		{
			get
			{
				return EatonGroupControlMib_M2.Entry + ".2.0." + this.index;
			}
		}
		public string ControlOffCmd
		{
			get
			{
				return EatonGroupControlMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public string ControlOnCmd
		{
			get
			{
				return EatonGroupControlMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string ControlRebootCmd
		{
			get
			{
				return EatonGroupControlMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public EatonGroupControlMib_M2(int groupNum)
		{
			this.index = groupNum;
		}
	}
}
