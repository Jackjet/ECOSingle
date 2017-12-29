using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonOutletControlMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.6.6.1";
		public string ControlStatus
		{
			get
			{
				return EatonOutletControlMib_M2.Entry + ".2.0." + this.index;
			}
		}
		public string ControlOffCmd
		{
			get
			{
				return EatonOutletControlMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public string ControlOnCmd
		{
			get
			{
				return EatonOutletControlMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string ControlRebootCmd
		{
			get
			{
				return EatonOutletControlMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string ControlSequenceDelay
		{
			get
			{
				return EatonOutletControlMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public EatonOutletControlMib_M2(int outletNum)
		{
			this.index = outletNum;
		}
	}
}
