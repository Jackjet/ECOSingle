using System;
namespace EcoDevice.AccessAPI
{
	internal class EatonGroupCurrentMib_M2
	{
		private int index = 1;
		public static string Entry = "1.3.6.1.4.1.534.6.6.7.5.4.1";
		public string CurrentValue
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".3.0." + this.index;
			}
		}
		public string CurrentStatus
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".4.0." + this.index;
			}
		}
		public string CurrentLowerWarning
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string CurrentLowerCritical
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".6.0." + this.index;
			}
		}
		public string CurrentUpperWarning
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public string CurrentUpperCritical
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".8.0." + this.index;
			}
		}
		public string MinCurrentMt
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".5.0." + this.index;
			}
		}
		public string MaxCurrentMT
		{
			get
			{
				return EatonGroupCurrentMib_M2.Entry + ".7.0." + this.index;
			}
		}
		public EatonGroupCurrentMib_M2(int groupNum)
		{
			this.index = groupNum;
		}
	}
}
