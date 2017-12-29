using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveOutletValueMib
	{
		private int outletIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string outletValueEntry = ".2.1.1";
		public static string Current = ".2.1.1.2.";
		public static string Voltage = ".2.1.1.3.";
		public static string Power = ".2.1.1.4.";
		public static string PowerDissipation = ".2.1.1.5.";
		public string SlaveCurrent
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletValueMib.Entry,
					this.slave,
					SlaveOutletValueMib.Current,
					this.outletIndex
				});
			}
		}
		public string SlaveVoltage
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletValueMib.Entry,
					this.slave,
					SlaveOutletValueMib.Voltage,
					this.outletIndex
				});
			}
		}
		public string SlavePower
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletValueMib.Entry,
					this.slave,
					SlaveOutletValueMib.Power,
					this.outletIndex
				});
			}
		}
		public string SlavePowerDissipation
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletValueMib.Entry,
					this.slave,
					SlaveOutletValueMib.PowerDissipation,
					this.outletIndex
				});
			}
		}
		public SlaveOutletValueMib(int index, int outletnumber)
		{
			this.slave += index;
			this.outletIndex = outletnumber;
		}
	}
}
