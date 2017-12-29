using System;
using System.Data;
namespace EcoSensors.Common
{
	internal class DBProcessUtil
	{
		public static void TransferRatio(DataTable dt, string cellNm, double ratio)
		{
			foreach (DataRow dataRow in dt.Rows)
			{
				double num = 0.0;
				double num2;
				if (double.TryParse(dataRow[cellNm].ToString(), out num2))
				{
					num = num2;
					num /= ratio;
				}
				dataRow[cellNm] = num;
			}
		}
	}
}
