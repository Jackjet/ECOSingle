using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveBankEnvironmentMib
	{
		private int bankIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string bankConfigEntry = ".3.3.1";
		public static string BankName = ".3.3.1.2.";
		public static string BankMinCurMT = ".3.3.1.3.";
		public static string BankMaxCurMT = ".3.3.1.4.";
		public static string BankMinVolMT = ".3.3.1.5.";
		public static string BankMaxVolMT = ".3.3.1.6.";
		public static string BankMinPMT = ".3.3.1.7.";
		public static string BankMaxPMT = ".3.3.1.8.";
		public static string BankMaxPDMT = ".3.3.1.9.";
		public string SlaveBankName
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankName,
					this.bankIndex
				});
			}
		}
		public string SlaveBankMaxCurMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankMaxCurMT,
					this.bankIndex
				});
			}
		}
		public string SlaveBankMinVolMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankMinVolMT,
					this.bankIndex
				});
			}
		}
		public string SlaveBankMaxVolMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankMaxVolMT,
					this.bankIndex
				});
			}
		}
		public string SlaveBankMaxPMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankMaxPMT,
					this.bankIndex
				});
			}
		}
		public string SlaveBankMaxPDMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveBankEnvironmentMib.Entry,
					this.slave,
					SlaveBankEnvironmentMib.BankMaxPDMT,
					this.bankIndex
				});
			}
		}
		public SlaveBankEnvironmentMib(int index, int banknumber)
		{
			this.slave += index;
			this.bankIndex = banknumber;
		}
		public int SlaveBankNumber(string oid)
		{
			if (string.IsNullOrEmpty(oid))
			{
				throw new System.ArgumentNullException("The oid is null or empty.");
			}
			if (!oid.StartsWith(SlaveBankEnvironmentMib.bankConfigEntry))
			{
				throw new System.ArgumentException("This oid is not of the bankConfigEntry");
			}
			return System.Convert.ToInt32(oid.Substring(oid.Length - 1));
		}
	}
}
