using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonOutletCurrentMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.6.4.1";
		public string CurrentValue
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public string CurrentStatus
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string CurrentLowerWarning
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string CurrentLowerCritical
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string CurrentUpperWarning
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public string CurrentUpperCritical
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public string MinCurrentMt
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string MaxCurrentMT
		{
			get
			{
				return EatonOutletCurrentMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public EatonOutletCurrentMib_M2(int outletNum)
		{
			this.index = outletNum;
		}
	}
}
