using System;
using System.Collections.Generic;
using System.Drawing;
namespace EcoSensors.EnegManPage.DashBoard
{
	public static class RackStatusAll
	{
		public const string VALUE_Error = "Error";
		private static double _total = 0.0;
		private static string _boardTag;
		private static bool _detection = false;
		public static System.Collections.Generic.Dictionary<long, string> SingleValue_List;
		private static double[] Valuevalid = new double[EcoGlobalVar.gl_maxRackNum];
		private static int _index = 0;
		private static double _max = 0.0;
		private static double _min = 0.0;
		private static int _pd_period = 0;
		private static int _PUE_period = 0;
		public static string Board_Tag
		{
			get
			{
				return RackStatusAll._boardTag;
			}
			set
			{
				RackStatusAll._boardTag = value;
			}
		}
		public static double Total_Value
		{
			get
			{
				return RackStatusAll._total;
			}
			set
			{
				RackStatusAll._total = value;
			}
		}
		public static double MaxValue
		{
			get
			{
				return RackStatusAll._max;
			}
			set
			{
				RackStatusAll._max = value;
			}
		}
		public static double MinValue
		{
			get
			{
				return RackStatusAll._min;
			}
			set
			{
				RackStatusAll._min = value;
			}
		}
		public static int Power_dissipation_period
		{
			get
			{
				return RackStatusAll._pd_period;
			}
			set
			{
				RackStatusAll._pd_period = value;
			}
		}
		public static int PUE_period
		{
			get
			{
				return RackStatusAll._PUE_period;
			}
			set
			{
				RackStatusAll._PUE_period = value;
			}
		}
		public static bool fetal_error
		{
			get
			{
				return RackStatusAll._detection;
			}
			set
			{
				RackStatusAll._detection = value;
			}
		}
		public static System.Collections.Generic.Dictionary<long, Color> RackColor(System.Collections.Generic.Dictionary<long, string> dict)
		{
			RackStatusAll._index = 0;
			System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
			RackStatusAll.SingleValue_List = new System.Collections.Generic.Dictionary<long, string>();
			foreach (System.Collections.Generic.KeyValuePair<long, string> current in dict)
			{
				Color value = Color.Silver;
				long key = current.Key;
				string value2 = current.Value;
				RackStatusAll.SingleValue_List.Add(key, value2);
				if (value2.Equals("Error"))
				{
					dictionary.Add(key, value);
				}
				else
				{
					double num = System.Convert.ToDouble(value2);
					RackStatusAll.Valuevalid[RackStatusAll._index] = num;
					RackStatusAll._index++;
					double num2 = 0.0;
					double num3 = (RackStatusAll._max - RackStatusAll._min) / 20.0;
					string boardTag;
					switch (boardTag = RackStatusAll._boardTag)
					{
					case "0:1":
						num2 = System.Math.Floor((RackStatusAll._max - num) / num3);
						break;
					case "1:0":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "1:1":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "2:0":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "2:1":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "2:2":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "2:3":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "2:4":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "3:0":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "3:1":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "3:2":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "3:4":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "3:5":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "4:0":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					case "4:1":
						num2 = System.Math.Floor((num - RackStatusAll._min) / num3);
						break;
					}
					switch (System.Convert.ToInt32(num2))
					{
					case 0:
						value = Color.FromArgb(38, 39, 109);
						break;
					case 1:
						value = Color.FromArgb(39, 58, 146);
						break;
					case 2:
						value = Color.FromArgb(48, 89, 167);
						break;
					case 3:
						value = Color.FromArgb(62, 115, 184);
						break;
					case 4:
						value = Color.FromArgb(166, 196, 242);
						break;
					case 5:
						value = Color.FromArgb(98, 204, 243);
						break;
					case 6:
						value = Color.FromArgb(137, 212, 238);
						break;
					case 7:
						value = Color.FromArgb(172, 223, 237);
						break;
					case 8:
						value = Color.FromArgb(206, 235, 241);
						break;
					case 9:
						value = Color.FromArgb(222, 241, 246);
						break;
					case 10:
						value = Color.FromArgb(251, 247, 201);
						break;
					case 11:
						value = Color.FromArgb(253, 239, 82);
						break;
					case 12:
						value = Color.FromArgb(251, 179, 21);
						break;
					case 13:
						value = Color.FromArgb(245, 139, 31);
						break;
					case 14:
						value = Color.FromArgb(243, 119, 33);
						break;
					case 15:
						value = Color.FromArgb(239, 83, 35);
						break;
					case 16:
						value = Color.FromArgb(236, 28, 36);
						break;
					case 17:
						value = Color.FromArgb(198, 32, 38);
						break;
					case 18:
						value = Color.FromArgb(158, 28, 32);
						break;
					case 19:
						value = Color.FromArgb(118, 17, 19);
						break;
					default:
						if (num2 >= 20.0)
						{
							value = Color.FromArgb(118, 17, 19);
						}
						else
						{
							if (num2 < 0.0)
							{
								value = Color.FromArgb(38, 39, 109);
							}
						}
						break;
					}
					dictionary.Add(key, value);
				}
			}
			return dictionary;
		}
		public static double AvgValue()
		{
			double result = 0.0;
			if (RackStatusAll._index > 0)
			{
				result = RackStatusAll._total / (double)RackStatusAll._index;
			}
			return result;
		}
		public static double VarValue()
		{
			double num = 0.0;
			double num2 = RackStatusAll.AvgValue();
			if (RackStatusAll._index > 0)
			{
				for (int i = 0; i < RackStatusAll._index; i++)
				{
					num += System.Math.Pow(RackStatusAll.Valuevalid[i] - num2, 2.0);
				}
				num = System.Math.Sqrt(num / (double)RackStatusAll._index);
			}
			return num;
		}
		public static double FtoCdegrees(double degrees)
		{
			return (degrees - 32.0) / 1.8;
		}
		public static double CtoFdegrees(double degrees)
		{
			return degrees * 1.8 + 32.0;
		}
		public static double MaxWarningthreshold(double maxthreshold, double minthreshold)
		{
			return maxthreshold - (maxthreshold - minthreshold) * 0.15;
		}
		public static double MinWarningthreshold(double maxthreshold, double minthreshold)
		{
			return minthreshold + (maxthreshold - minthreshold) * 0.15;
		}
	}
}
