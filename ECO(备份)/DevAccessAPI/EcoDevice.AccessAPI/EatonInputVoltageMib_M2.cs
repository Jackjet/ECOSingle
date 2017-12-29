using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonInputVoltageMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.3.2.1";
		public string VoltageValue
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".3.0.1." + this.index;
			}
		}
		public string VoltageStatus
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".4.0.1." + this.index;
			}
		}
		public string VoltageLowerWarning
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".5.0.1." + this.index;
			}
		}
		public string VoltageLowerCritical
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".6.0.1." + this.index;
			}
		}
		public string VoltageUpperWarning
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".7.0.1." + this.index;
			}
		}
		public string VoltageUpperCritical
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".8.0.1." + this.index;
			}
		}
		public string MinVoltageMt
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".5.0.1." + this.index;
			}
		}
		public string MaxVoltageMT
		{
			get
			{
				return EatonInputVoltageMib_M2.Entry + ".7.0.1." + this.index;
			}
		}
		public EatonInputVoltageMib_M2(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
