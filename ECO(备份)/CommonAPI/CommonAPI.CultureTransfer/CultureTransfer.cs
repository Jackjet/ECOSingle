using System;
using System.Globalization;
namespace CommonAPI.CultureTransfer
{
	public class CultureTransfer
	{
		private static CultureInfo en_CultureInfo = new CultureInfo("en");
		public static float ToSingle(string str)
		{
			return Convert.ToSingle(str, CultureTransfer.en_CultureInfo);
		}
		public static int ToInt32(string str)
		{
			return Convert.ToInt32(str, CultureTransfer.en_CultureInfo);
		}
		public static string ToString(float f_value)
		{
			return Convert.ToString(f_value, CultureTransfer.en_CultureInfo);
		}
		public static string ToString(int i_value)
		{
			return Convert.ToString(i_value, CultureTransfer.en_CultureInfo);
		}
		public static double ToDouble(string str)
		{
			return Convert.ToDouble(str, CultureTransfer.en_CultureInfo);
		}
	}
}
