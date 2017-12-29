using System;
namespace EcoDevice.AccessAPI
{
	internal class SlaveOutletEnvironmentMib
	{
		private int outletIndex = 1;
		private int slave = 7;
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.";
		public static string outletConfigEntry = ".2.2.1";
		public static string OutletName = ".2.2.1.2.";
		public static string Confirmation = ".2.2.1.3.";
		public static string OnDelayTime = ".2.2.1.4.";
		public static string OffDelayTime = ".2.2.1.5.";
		public static string ShutdownMethod = ".2.2.1.6.";
		public static string MacAddress = ".2.2.1.7.";
		public static string MinCurrentMt = ".2.2.1.8.";
		public static string MaxCurrentMT = ".2.2.1.9.";
		public static string MinVoltageMT = ".2.2.1.10.";
		public static string MaxVoltageMT = ".2.2.1.11.";
		public static string MinPowerMT = ".2.2.1.12.";
		public static string MaxPowerMT = ".2.2.1.13.";
		public static string MaxPowerDissMT = ".2.2.1.14.";
		public string SlaveOutletName
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.OutletName,
					this.outletIndex
				});
			}
		}
		public string SlaveConfirmation
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.Confirmation,
					this.outletIndex
				});
			}
		}
		public string SlaveOnDelayTime
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.OnDelayTime,
					this.outletIndex
				});
			}
		}
		public string SlaveOffDelayTime
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.OffDelayTime,
					this.outletIndex
				});
			}
		}
		public string SlaveShutdownMethod
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.ShutdownMethod,
					this.outletIndex
				});
			}
		}
		public string SlaveMacAddress
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MacAddress,
					this.outletIndex
				});
			}
		}
		public string SlaveMinCurrentMt
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MinCurrentMt,
					this.outletIndex
				});
			}
		}
		public string SlaveMaxCurrentMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MaxCurrentMT,
					this.outletIndex
				});
			}
		}
		public string SlaveMinVoltageMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MinVoltageMT,
					this.outletIndex
				});
			}
		}
		public string SlaveMaxVoltageMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MaxVoltageMT,
					this.outletIndex
				});
			}
		}
		public string SlaveMinPowerMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MinPowerMT,
					this.outletIndex
				});
			}
		}
		public string SlaveMaxPowerMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MaxPowerMT,
					this.outletIndex
				});
			}
		}
		public string SlaveMaxPowerDissMT
		{
			get
			{
				return string.Concat(new object[]
				{
					SlaveOutletEnvironmentMib.Entry,
					this.slave,
					SlaveOutletEnvironmentMib.MaxPowerDissMT,
					this.outletIndex
				});
			}
		}
		public SlaveOutletEnvironmentMib(int index, int outletnumber)
		{
			this.slave += index;
			this.outletIndex = outletnumber;
		}
		public int SlaveOutletNumber(string oid)
		{
			if (string.IsNullOrEmpty(oid))
			{
				throw new System.ArgumentNullException("The oid is null or empty.");
			}
			if (!oid.StartsWith(SlaveOutletEnvironmentMib.outletConfigEntry))
			{
				throw new System.ArgumentException("This oid is not of the outletConfigEntry");
			}
			return System.Convert.ToInt32(oid.Substring(oid.Length - 1));
		}
	}
}
