using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveBankValueMib
	{
		private int bankIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string bankValueEntry = ".3.2.1";
		public static string Current = ".3.2.1.2.";
		public static string Voltage = ".3.2.1.3.";
		public static string Power = ".3.2.1.4.";
		public static string PowerDissipation = ".3.2.1.5.";
		public string SlaveCurrent
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankValueMib.Entry,
					this.slave,
					SlaveBankValueMib.Current,
					this.bankIndex
				});
			}
		}
		public string SlaveVoltage
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankValueMib.Entry,
					this.slave,
					SlaveBankValueMib.Voltage,
					this.bankIndex
				});
			}
		}
		public string SlavePower
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankValueMib.Entry,
					this.slave,
					SlaveBankValueMib.Power,
					this.bankIndex
				});
			}
		}
		public string SlavePowerDissipation
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankValueMib.Entry,
					this.slave,
					SlaveBankValueMib.PowerDissipation,
					this.bankIndex
				});
			}
		}
		public SlaveBankValueMib(int index, int banknumber)
		{
			this.slave += index;
			this.bankIndex = banknumber;
		}
	}
}
