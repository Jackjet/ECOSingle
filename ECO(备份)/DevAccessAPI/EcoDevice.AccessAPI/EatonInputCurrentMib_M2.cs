using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonInputCurrentMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.3.3.1";
		public string CurrentValue
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".4.0.1." + this.index;
			}
		}
		public string CurrentStatus
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".5.0.1." + this.index;
			}
		}
		public string CurrentLowerWarning
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".6.0.1." + this.index;
			}
		}
		public string CurrentLowerCritical
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".7.0.1." + this.index;
			}
		}
		public string CurrentUpperWarning
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".8.0.1." + this.index;
			}
		}
		public string CurrentUpperCritical
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".9.0.1." + this.index;
			}
		}
		public string MinCurrentMt
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".6.0.1." + this.index;
			}
		}
		public string MaxCurrentMT
		{
			get
			{
				return EatonInputCurrentMib_M2.Entry + ".8.0.1." + this.index;
			}
		}
		public EatonInputCurrentMib_M2(int inputNum)
		{
			this.index = inputNum;
		}
	}
}
