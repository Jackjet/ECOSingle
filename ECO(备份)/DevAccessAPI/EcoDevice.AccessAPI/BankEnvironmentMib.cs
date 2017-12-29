using System;
namespace EcoDevice.AccessAPI
{
	internal class BankEnvironmentMib
	{
		private int banknumber = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.3.3.1";
		public static string valueEntry = "1.3.6.1.4.1.21317.1.3.2.2.2.3.2.1";
		public string BankName
		{
			get
			{
				return BankEnvironmentMib.Entry + ".2." + this.banknumber;
			}
		}
		public string BankMinCurMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".3." + this.banknumber;
			}
		}
		public string BankMaxCurMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".4." + this.banknumber;
			}
		}
		public string BankMinVolMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".5." + this.banknumber;
			}
		}
		public string BankMaxVolMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".6." + this.banknumber;
			}
		}
		public string BankMaxPMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".8." + this.banknumber;
			}
		}
		public string BankMaxPDMT
		{
			get
			{
				return BankEnvironmentMib.Entry + ".9." + this.banknumber;
			}
		}
		public string BankVoltageValue
		{
			get
			{
				return BankEnvironmentMib.valueEntry + ".3." + this.banknumber;
			}
		}
		public BankEnvironmentMib(int banknumber)
		{
			this.banknumber = banknumber;
		}
		public int BankNumber(string oid)
		{
			if (string.IsNullOrEmpty(oid))
			{
				throw new System.ArgumentNullException("The oid is null or empty.");
			}
			if (!oid.StartsWith(BankEnvironmentMib.Entry))
			{
				throw new System.ArgumentException("This oid is not of the bankConfigEntry");
			}
			this.banknumber = System.Convert.ToInt32(oid.Substring(oid.Length - 1));
			return this.banknumber;
		}
	}
}
