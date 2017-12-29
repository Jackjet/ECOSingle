using CommonAPI.CultureTransfer;
using EcoDevice.AccessAPI;
using EcoSensors.EnegManPage.DashBoard;
using System;
using System.Collections.Generic;
namespace EcoSensors.Common
{
	internal class devcfgUtil
	{
		public const string ThresHoldType_Dev = "dev";
		public const string ThresHoldType_Bank = "bank";
		public const string ThresHoldType_Port = "port";
		public const string ThresHoldType_SS = "ss";
		public const string ThresHoldType_Line = "line";
		public const int ThresHoldFlg_MinCurrent = 1;
		public const int ThresHoldFlg_MaxCurrent = 2;
		public const int ThresHoldFlg_MinVoltage = 4;
		public const int ThresHoldFlg_MaxVoltage = 8;
		public const int ThresHoldFlg_MinPower = 16;
		public const int ThresHoldFlg_MaxPower = 32;
		public const int ThresHoldFlg_MinPowerD = 64;
		public const int ThresHoldFlg_MaxPowerD = 128;
		public const int ThresHoldFlg_MinCurrentEmpty = 256;
		public const int ThresHoldFlg_MaxCurrentEmpty = 512;
		public const int ThresHoldFlg_MinVoltageEmpty = 1024;
		public const int ThresHoldFlg_MaxVoltageEmpty = 2048;
		public const int ThresHoldFlg_MinPowerEmpty = 4096;
		public const int ThresHoldFlg_MaxPowerEmpty = 8192;
		public const int ThresHoldFlg_MinPowerDEmpty = 16384;
		public const int ThresHoldFlg_MaxPowerDEmpty = 32768;
		public const int ThresHoldFlg_MinSST = 1;
		public const int ThresHoldFlg_MaxSST = 2;
		public const int ThresHoldFlg_MinSSH = 4;
		public const int ThresHoldFlg_MaxSSH = 8;
		public const int ThresHoldFlg_MinSSP = 16;
		public const int ThresHoldFlg_MaxSSP = 32;
		public const int ThresHoldFlg_MinSSTEmpty = 256;
		public const int ThresHoldFlg_MaxSSTEmpty = 512;
		public const int ThresHoldFlg_MinSSHEmpty = 1024;
		public const int ThresHoldFlg_MaxSSHEmpty = 2048;
		public const int ThresHoldFlg_MinSSPEmpty = 4096;
		public const int ThresHoldFlg_MaxSSPEmpty = 8192;
		public static string Rang_min(string range)
		{
			string[] array;
			if (range.IndexOf('~') >= 0)
			{
				array = range.Split(new char[]
				{
					'~'
				});
				return array[0];
			}
			array = range.Split(new char[]
			{
				'～'
			});
			return array[0];
		}
		public static string Rang_max(string range)
		{
			if (range.IndexOf('~') >= 0)
			{
				string[] array = range.Split(new char[]
				{
					'~'
				});
				if (array.Length == 1)
				{
					return array[0];
				}
				return array[1];
			}
			else
			{
				string[] array = range.Split(new char[]
				{
					'～'
				});
				if (array.Length == 1)
				{
					return array[0];
				}
				return array[1];
			}
		}
		public static string RangeCurrent(DevModelConfig devcfg, string sType, int selNo = 0, string fmt = "F1")
		{
			System.Collections.Generic.List<stru_CommRange> currentThresholds = devcfg.devThresholds.currentThresholds;
			string format = string.Concat(new string[]
			{
				"({0:",
				fmt,
				"}～{1:",
				fmt,
				"})"
			});
			if (currentThresholds != null)
			{
				foreach (stru_CommRange current in currentThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format(format, CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
						}
					}
				}
			}
			return string.Format(format, 0.0, 20.0);
		}
		public static string RangeVoltage(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> voltageThresholds = devcfg.devThresholds.voltageThresholds;
			if (voltageThresholds != null)
			{
				foreach (stru_CommRange current in voltageThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format("({0:F1}～{1:F1})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
						}
					}
				}
			}
			string voltage = devcfg.devThresholds.commonThresholds.voltage;
			return string.Format("({0:F1}～{1:F1})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(voltage)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(voltage)));
		}
		public static string RangePower(DevModelConfig devcfg, string sType, int selNo = 0, double rate = 1.0)
		{
			System.Collections.Generic.List<stru_CommRange> powerThresholds = devcfg.devThresholds.powerThresholds;
			if (powerThresholds != null)
			{
				foreach (stru_CommRange current in powerThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format("({0:F1}～{1:F1})", (double)CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)) / rate, (double)CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)) / rate);
						}
					}
				}
			}
			string power = devcfg.devThresholds.commonThresholds.power;
			return string.Format("({0:F1}～{1:F1})", (double)CultureTransfer.ToSingle(devcfgUtil.Rang_min(power)) / rate, (double)CultureTransfer.ToSingle(devcfgUtil.Rang_max(power)) / rate);
		}
		public static string RangePowerDiss(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> powerDissThresholds = devcfg.devThresholds.powerDissThresholds;
			if (powerDissThresholds != null)
			{
				foreach (stru_CommRange current in powerDissThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format("({0:F1}～{1:F0})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
						}
					}
				}
			}
			string powerDissipation = devcfg.devThresholds.commonThresholds.powerDissipation;
			return string.Format("({0:F1}～{1:F0})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(powerDissipation)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(powerDissipation)));
		}
		public static string RangeTemp(DevModelConfig devcfg, string sType, int selNo = 0, string fmt = "F1")
		{
			System.Collections.Generic.List<stru_CommRange> tempThresholds = devcfg.devThresholds.tempThresholds;
			string format = string.Concat(new string[]
			{
				"({0:",
				fmt,
				"}～{1:",
				fmt,
				"})"
			});
			if (tempThresholds != null)
			{
				foreach (stru_CommRange current in tempThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							string result;
							if (EcoGlobalVar.TempUnit == 0)
							{
								result = string.Format(format, CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
								return result;
							}
							result = string.Format(format, RackStatusAll.CtoFdegrees((double)CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range))), RackStatusAll.CtoFdegrees((double)CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range))));
							return result;
						}
					}
				}
			}
			string temperature = devcfg.devThresholds.commonThresholds.temperature;
			if (EcoGlobalVar.TempUnit == 0)
			{
				return string.Format(format, CultureTransfer.ToSingle(devcfgUtil.Rang_min(temperature)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(temperature)));
			}
			return string.Format(format, RackStatusAll.CtoFdegrees((double)CultureTransfer.ToSingle(devcfgUtil.Rang_min(temperature))), RackStatusAll.CtoFdegrees((double)CultureTransfer.ToSingle(devcfgUtil.Rang_max(temperature))));
		}
		public static string RangeHumi(DevModelConfig devcfg, string sType, int selNo = 0, string fmt = "F1")
		{
			System.Collections.Generic.List<stru_CommRange> humiThresholds = devcfg.devThresholds.HumiThresholds;
			string format = string.Concat(new string[]
			{
				"({0:",
				fmt,
				"}～{1:",
				fmt,
				"})"
			});
			if (humiThresholds != null)
			{
				foreach (stru_CommRange current in humiThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format(format, CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
						}
					}
				}
			}
			string humidity = devcfg.devThresholds.commonThresholds.humidity;
			return string.Format(format, CultureTransfer.ToSingle(devcfgUtil.Rang_min(humidity)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(humidity)));
		}
		public static string RangePress(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> pressThresholds = devcfg.devThresholds.PressThresholds;
			if (pressThresholds != null)
			{
				foreach (stru_CommRange current in pressThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return string.Format("({0:F1}～{1:F1})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range)));
						}
					}
				}
			}
			string pressure = devcfg.devThresholds.commonThresholds.pressure;
			return string.Format("({0:F1}～{1:F1})", CultureTransfer.ToSingle(devcfgUtil.Rang_min(pressure)), CultureTransfer.ToSingle(devcfgUtil.Rang_max(pressure)));
		}
		public static int ThresholdFlg(DevModelConfig devcfg, string sType)
		{
			System.Collections.Generic.List<ThresholdFlg> thresholdsFlg = devcfg.devThresholds.ThresholdsFlg;
			foreach (ThresholdFlg current in thresholdsFlg)
			{
				if (string.Compare(current.type, sType, true) == 0)
				{
					return current.flg;
				}
			}
			return 0;
		}
		public static int UIThresholdEditFlg(DevModelConfig devcfg, string sType)
		{
			System.Collections.Generic.List<ThresholdFlg> uIEditFlg = devcfg.devThresholds.UIEditFlg;
			if (uIEditFlg != null)
			{
				foreach (ThresholdFlg current in uIEditFlg)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						return current.flg;
					}
				}
			}
			if (sType.Equals("ss"))
			{
				return 0;
			}
			return 81;
		}
		public static float fmaxCurrent(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> currentThresholds = devcfg.devThresholds.currentThresholds;
			foreach (stru_CommRange current in currentThresholds)
			{
				if (string.Compare(current.type, sType, true) == 0)
				{
					int num = CultureTransfer.ToInt32(devcfgUtil.Rang_min(current.id));
					int num2 = CultureTransfer.ToInt32(devcfgUtil.Rang_max(current.id));
					if (num <= selNo && selNo <= num2)
					{
						return CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range));
					}
				}
			}
			return -300f;
		}
		public static float fMinVoltage(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> voltageThresholds = devcfg.devThresholds.voltageThresholds;
			if (voltageThresholds != null)
			{
				foreach (stru_CommRange current in voltageThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return CultureTransfer.ToSingle(devcfgUtil.Rang_min(current.range));
						}
					}
				}
			}
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_min(commonThresholds.voltage));
		}
		public static float fMaxVoltage(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> voltageThresholds = devcfg.devThresholds.voltageThresholds;
			if (voltageThresholds != null)
			{
				foreach (stru_CommRange current in voltageThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range));
						}
					}
				}
			}
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.voltage));
		}
		public static float fMaxPower(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> powerThresholds = devcfg.devThresholds.powerThresholds;
			if (powerThresholds != null)
			{
				foreach (stru_CommRange current in powerThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range));
						}
					}
				}
			}
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.power));
		}
		public static float fMaxPowerD(DevModelConfig devcfg, string sType, int selNo = 0)
		{
			System.Collections.Generic.List<stru_CommRange> powerDissThresholds = devcfg.devThresholds.powerDissThresholds;
			if (powerDissThresholds != null)
			{
				foreach (stru_CommRange current in powerDissThresholds)
				{
					if (string.Compare(current.type, sType, true) == 0)
					{
						int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
						int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
						if (num <= selNo && selNo <= num2)
						{
							return CultureTransfer.ToSingle(devcfgUtil.Rang_max(current.range));
						}
					}
				}
			}
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.powerDissipation));
		}
		public static float fMinHumidity(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_min(commonThresholds.humidity));
		}
		public static float fMaxHumidity(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.humidity));
		}
		public static float fMinTemperature(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_min(commonThresholds.temperature));
		}
		public static float fMaxTemperature(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.temperature));
		}
		public static float fMinPress(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_min(commonThresholds.pressure));
		}
		public static float fMaxPress(DevModelConfig devcfg)
		{
			DevCommonThreshold commonThresholds = devcfg.devThresholds.commonThresholds;
			return CultureTransfer.ToSingle(devcfgUtil.Rang_max(commonThresholds.pressure));
		}
		public static double ampMax(DevModelConfig devcfg, string sType)
		{
			System.Collections.Generic.List<stru_CommRange> ampcapicity = devcfg.ampcapicity;
			foreach (stru_CommRange current in ampcapicity)
			{
				if (string.Compare(current.type, sType, true) == 0)
				{
					return CultureTransfer.ToDouble(devcfgUtil.Rang_max(current.range));
				}
			}
			return 0.0;
		}
		public static double ampMax(DevModelConfig devcfg, string sType, int selNo)
		{
			System.Collections.Generic.List<stru_CommRange> ampcapicity = devcfg.ampcapicity;
			foreach (stru_CommRange current in ampcapicity)
			{
				if (string.Compare(current.type, sType, true) == 0)
				{
					int num = System.Convert.ToInt32(devcfgUtil.Rang_min(current.id));
					int num2 = System.Convert.ToInt32(devcfgUtil.Rang_max(current.id));
					if (num <= selNo && selNo <= num2)
					{
						return CultureTransfer.ToDouble(devcfgUtil.Rang_max(current.range));
					}
				}
			}
			return 0.0;
		}
		public static bool haveMeasureCurrent(int UIEditFlg)
		{
			return (UIEditFlg & 1) == 0 || (UIEditFlg & 2) == 0;
		}
		public static bool haveMeasureVoltage(int UIEditFlg)
		{
			return (UIEditFlg & 4) == 0 || (UIEditFlg & 8) == 0;
		}
		public static bool haveMeasurePower(int UIEditFlg)
		{
			return (UIEditFlg & 16) == 0 || (UIEditFlg & 32) == 0;
		}
		public static bool haveMeasurePowerD(int UIEditFlg)
		{
			return (UIEditFlg & 64) == 0 || (UIEditFlg & 128) == 0;
		}
		public static bool haveUIEditV(int UIEditFlg, int minmask, int maxmask, double minV, double maxV)
		{
			return ((UIEditFlg & minmask) == 0 && minV != -600.0) || ((UIEditFlg & maxmask) == 0 && maxV != -600.0);
		}
	}
}
