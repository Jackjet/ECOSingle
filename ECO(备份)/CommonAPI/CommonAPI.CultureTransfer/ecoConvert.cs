using System;
namespace CommonAPI.CultureTransfer
{
	public class ecoConvert
	{
		public static double f2d(object value)
		{
			decimal value2 = Convert.ToDecimal(value);
			return Convert.ToDouble(value2);
		}
	}
}
