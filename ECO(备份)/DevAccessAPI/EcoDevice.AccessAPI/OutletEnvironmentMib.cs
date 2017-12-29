using System;
namespace EcoDevice.AccessAPI
{
	internal class OutletEnvironmentMib
	{
		private int outletnumber = 1;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.2.10.1";
		public static string Column = "1.3.6.1.4.1.21317.1.3.2.2.2.2.10.1.2";
		public string OutletName
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".2." + this.outletnumber;
			}
		}
		public string Confirmation
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".3." + this.outletnumber;
			}
		}
		public string OnDelayTime
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".4." + this.outletnumber;
			}
		}
		public string OffDelayTime
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".5." + this.outletnumber;
			}
		}
		public string ShutdownMethod
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".6." + this.outletnumber;
			}
		}
		public string MacAddress
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".7." + this.outletnumber;
			}
		}
		public string MinCurrentMt
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".8." + this.outletnumber;
			}
		}
		public string MaxCurrentMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".9." + this.outletnumber;
			}
		}
		public string MinVoltageMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".10." + this.outletnumber;
			}
		}
		public string MaxVoltageMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".11." + this.outletnumber;
			}
		}
		public string MinPowerMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".12." + this.outletnumber;
			}
		}
		public string MaxPowerMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".13." + this.outletnumber;
			}
		}
		public string MaxPowerDissMT
		{
			get
			{
				return OutletEnvironmentMib.Entry + ".14." + this.outletnumber;
			}
		}
		public OutletEnvironmentMib(int outletnumber)
		{
			this.outletnumber = outletnumber;
		}
		public int OutletNumber(string oid)
		{
			if (string.IsNullOrEmpty(oid))
			{
				throw new System.ArgumentNullException("The oid is null or empty.");
			}
			if (!oid.StartsWith(OutletEnvironmentMib.Entry))
			{
				throw new System.ArgumentException("This oid is not of the outletConfigEntry");
			}
			this.outletnumber = System.Convert.ToInt32(oid.Substring(oid.Length - 1));
			return this.outletnumber;
		}
	}
}
