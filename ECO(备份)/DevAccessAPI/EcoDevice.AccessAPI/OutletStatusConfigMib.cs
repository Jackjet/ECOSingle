using System;
namespace EcoDevice.AccessAPI
{
	internal class OutletStatusConfigMib
	{
		public static string Entry = "1.3.6.1.4.1.21317.1.3.2.2.2.2.";
		public static string OutletStatus(int outletIndex)
		{
			int num = 1;
			if (outletIndex > 8)
			{
				num = 2;
			}
			return OutletStatusConfigMib.Entry + (outletIndex + num) + ".0";
		}
		public static int OutletIndex(string oid)
		{
			if (oid == null || "".Equals(oid))
			{
				throw new System.ArgumentNullException("OID");
			}
			if (!oid.StartsWith(OutletStatusConfigMib.Entry))
			{
				throw new System.ArgumentException("This oid is not of Outlet.");
			}
			return System.Convert.ToInt32(oid.Substring(OutletStatusConfigMib.Entry.Length, 1));
		}
	}
}
