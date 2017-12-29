using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonGroupVoltageMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.5.3.1";
		public string VoltageValue
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public string VoltageStatus
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string VoltageLowerWarning
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string VoltageLowerCritical
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string VoltageUpperWarning
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public string VoltageUpperCritical
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public string MinVoltageMt
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string MaxVoltageMT
		{
			get
			{
				return EatonGroupVoltageMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public EatonGroupVoltageMib_M2(int groupNum)
		{
			this.index = groupNum;
		}
	}
}
