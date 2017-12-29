using CommonAPI;
using DBAccessAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class DevAccessCfg
	{
		public delegate void DelegateOnAutoModeUpdate(string modeKey, DevModelConfig cfg);
		public const int runEnv_type_standalone = 1;
		public const int runEnv_type_server = 2;
		public const int runEnv_dbusage_access = 1;
		public const int runEnv_dbusage_mysql = 2;
		public const int runEnv_dbusage_postgre = 3;
		private static System.Collections.Generic.Dictionary<string, DevModelConfig> devModelList = new System.Collections.Generic.Dictionary<string, DevModelConfig>();
		private static System.Collections.Generic.Dictionary<string, DevModelConfig> autoModelList = new System.Collections.Generic.Dictionary<string, DevModelConfig>();
		private static string m_version = "";
		private static int m_maxZoneNum = 128;
		private static int m_maxRackNum = 1250;
		private static int m_maxDevNum = 2500;
		private static bool m_supportISG = false;
		private static int m_ISGtimeout = 0;
		private static bool m_supportBillprot = false;
		private static int m_supportOEMDev = 0;
		private static bool m_isdebuglogExport = false;
		private static int m_PeakPowerMethod = 0;
		private static int m_MySQLUseMajorVersionOnly = 0;
		private static int m_runEnv_type = 1;
		private static int m_runEnv_dbusage = 1;
		private static DevCommonThreshold commonThreshold = default(DevCommonThreshold);
		private AutoDetectLoader autoLoader = AutoDetectLoader.GetInstance();
		private static DevAccessCfg instance = new DevAccessCfg();
		public static DevAccessCfg.DelegateOnAutoModeUpdate _cbOnAutoModeUpdated = null;
		public static DevAccessCfg GetInstance()
		{
			return DevAccessCfg.instance;
		}
		private DevAccessCfg()
		{
			try
			{
				string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
				XMLParserAPI xMLParserAPI = new XMLParserAPI();
				DevAccessCfg.devModelList = xMLParserAPI.LoadDevInfoXml(baseDirectory + "DevDefine.xml", ref DevAccessCfg.m_version, ref DevAccessCfg.m_maxZoneNum, ref DevAccessCfg.m_maxRackNum, ref DevAccessCfg.m_maxDevNum, ref DevAccessCfg.m_supportISG, ref DevAccessCfg.m_ISGtimeout, ref DevAccessCfg.m_supportBillprot, ref DevAccessCfg.m_isdebuglogExport, ref DevAccessCfg.m_supportOEMDev, ref DevAccessCfg.m_runEnv_type, ref DevAccessCfg.m_runEnv_dbusage, ref DevAccessCfg.m_PeakPowerMethod, ref DevAccessCfg.m_MySQLUseMajorVersionOnly);
				DevAccessCfg.commonThreshold.voltage = "90.0~260.0";
				DevAccessCfg.commonThreshold.power = "0~9999.9";
				DevAccessCfg.commonThreshold.powerDissipation = "0~99999";
				DevAccessCfg.commonThreshold.temperature = "-20.0~60.0";
				DevAccessCfg.commonThreshold.humidity = "15.0~95.0";
				DevAccessCfg.commonThreshold.pressure = "-250.0~250.0";
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("The process failed: {0}", ex.ToString());
			}
		}
		public void LoadAutoDeviceModelConfig()
		{
			try
			{
				DevAccessCfg.autoModelList = this.autoLoader.loadModelList();
			}
			catch (System.Exception)
			{
			}
		}
		private void ConvertXmlConfig2AutoConfig()
		{
			System.Collections.Generic.Dictionary<string, DevModelConfig> dictionary = new System.Collections.Generic.Dictionary<string, DevModelConfig>();
			System.Collections.Generic.IEnumerator<string> enumerator = DevAccessCfg.devModelList.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				DevModelConfig value = DevAccessCfg.devModelList[current];
				if (value.commonThresholdFlag == Constant.WithCommonThreshold)
				{
					value.maxDevThresholdOpt |= 15u;
					value.maxBankThresholdOpt |= 15u;
					value.maxPortThresholdOpt |= 15u;
					value.minDevThresholdOpt |= 2u;
					value.minBankThresholdOpt |= 2u;
					value.minPortThresholdOpt |= 2u;
					value.deviceMeasureOpt |= 15u;
				}
				else
				{
					if (value.commonThresholdFlag == Constant.WithoutCommonThreshold)
					{
						value.maxDevThresholdOpt |= 1u;
						value.maxBankThresholdOpt |= 1u;
						value.maxPortThresholdOpt = 0u;
						value.minDevThresholdOpt = 0u;
						value.minBankThresholdOpt = 0u;
						value.minPortThresholdOpt = 0u;
						value.deviceMeasureOpt |= 1u;
					}
					else
					{
						if (value.commonThresholdFlag == Constant.WithSpecialThreshold)
						{
							value.maxDevThresholdOpt |= 13u;
							value.maxBankThresholdOpt |= 15u;
							value.maxPortThresholdOpt = 0u;
							value.minDevThresholdOpt = 0u;
							value.minBankThresholdOpt |= 2u;
							value.minPortThresholdOpt = 0u;
							value.deviceMeasureOpt |= 15u;
						}
					}
				}
				if (value.perbankReading == Constant.YES)
				{
					if (value.commonThresholdFlag == Constant.WithCommonThreshold)
					{
						value.bankMeasureOpt = 15u;
					}
					else
					{
						if (value.commonThresholdFlag == Constant.WithoutCommonThreshold)
						{
							value.bankMeasureOpt = 1u;
							value.bankStatusSupport = Constant.YES;
						}
						else
						{
							if (value.commonThresholdFlag == Constant.WithSpecialThreshold)
							{
								value.bankMeasureOpt = 15u;
								value.bankStatusSupport = Constant.YES;
							}
						}
					}
				}
				if (value.perportreading == Constant.YES)
				{
					value.outletMeasureOpt = 15u;
					value.outletStatusSupport = Constant.YES;
				}
				dictionary.Add(current, value);
			}
			DevAccessCfg.devModelList.Clear();
			DevAccessCfg.devModelList = dictionary;
		}
		public SnmpConfig getSnmpConfig(DevSnmpConfig snmpCfg)
		{
			SnmpConfig snmpConfig = null;
			switch (snmpCfg.snmpVer)
			{
			case 0:
				snmpConfig = new SnmpV1Config(snmpCfg.devIP);
				((SnmpV1Config)snmpConfig).Community = snmpCfg.userName;
				((SnmpV1Config)snmpConfig).Retry = snmpCfg.retry;
				((SnmpV1Config)snmpConfig).Port = snmpCfg.devPort;
				((SnmpV1Config)snmpConfig).Timeout = snmpCfg.timeout;
				break;
			case 1:
				snmpConfig = new SnmpV2Config(snmpCfg.devIP);
				((SnmpV2Config)snmpConfig).Community = snmpCfg.userName;
				((SnmpV2Config)snmpConfig).Retry = snmpCfg.retry;
				((SnmpV2Config)snmpConfig).Port = snmpCfg.devPort;
				((SnmpV2Config)snmpConfig).Timeout = snmpCfg.timeout;
				break;
			case 3:
				snmpConfig = new SnmpV3Config(snmpCfg.devIP);
				snmpConfig.Retry = snmpCfg.retry;
				snmpConfig.Timeout = snmpCfg.timeout;
				snmpConfig.Port = snmpCfg.devPort;
				((SnmpV3Config)snmpConfig).Authentication = (Authentication)System.Enum.Parse(typeof(Authentication), snmpCfg.authType);
				((SnmpV3Config)snmpConfig).AuthSecret = snmpCfg.authPSW;
				((SnmpV3Config)snmpConfig).Privacy = (Privacy)System.Enum.Parse(typeof(Privacy), snmpCfg.privType);
				((SnmpV3Config)snmpConfig).PrivacySecret = snmpCfg.privPSW;
				((SnmpV3Config)snmpConfig).UserName = snmpCfg.userName;
				break;
			}
			return snmpConfig;
		}
		public string getVersion()
		{
			return DevAccessCfg.m_version;
		}
		public int getmaxZoneNum()
		{
			return DevAccessCfg.m_maxZoneNum;
		}
		public int getmaxRackNum()
		{
			return DevAccessCfg.m_maxRackNum;
		}
		public int getmaxDevNum()
		{
			return DevAccessCfg.m_maxDevNum;
		}
		public bool isISGsupport()
		{
			return DevAccessCfg.m_supportISG;
		}
		public int getISGtimeout()
		{
			return DevAccessCfg.m_ISGtimeout;
		}
		public bool isBillprotsupport()
		{
			return DevAccessCfg.m_supportBillprot;
		}
		public int supportOEMdev()
		{
			return DevAccessCfg.m_supportOEMDev;
		}
		public bool isDebugLogExport()
		{
			return DevAccessCfg.m_isdebuglogExport;
		}
		public int getPowerPeakMethod()
		{
			return DevAccessCfg.m_PeakPowerMethod;
		}
		public int getMySQLUseMajorVersionOnly()
		{
			return DevAccessCfg.m_MySQLUseMajorVersionOnly;
		}
		public int runEnv_type()
		{
			return DevAccessCfg.m_runEnv_type;
		}
		public int runEnv_dbusage()
		{
			return DevAccessCfg.m_runEnv_dbusage;
		}
		public bool isAutodectDev(string modelname, string fmwareVer)
		{
			if (modelname.StartsWith("Eaton ePDU"))
			{
				return false;
			}
			string key = modelname + "-" + fmwareVer;
			return DevAccessCfg.autoModelList.ContainsKey(key);
		}
		public DevModelConfig getDeviceModelConfig(string modelname, string fmwareVer)
		{
			string text = modelname;
			if (modelname.StartsWith("Eaton ePDU"))
			{
				text = "Eaton ePDU";
			}
			string key = text + "-" + fmwareVer;
			if (DevAccessCfg.autoModelList.ContainsKey(key))
			{
				return DevAccessCfg.autoModelList[key];
			}
			if (DevAccessCfg.devModelList.ContainsKey(text))
			{
				return DevAccessCfg.devModelList[text];
			}
			return new DevModelConfig(string.Empty);
		}
		public int updateAutoModelList(string modelName, string fmVersion, string autoBasicInfo, string autoRatingInfo)
		{
			string key = modelName + "-" + fmVersion;
			if (DevAccessCfg.autoModelList.ContainsKey(key))
			{
				DevModelConfig devModelConfig = DevAccessCfg.autoModelList[key];
				if (devModelConfig.autoBasicInfo.Equals(autoBasicInfo) && devModelConfig.autoRatingInfo.Equals(autoRatingInfo))
				{
					return -1;
				}
				DevAccessCfg.autoModelList.Remove(key);
			}
			try
			{
				DevModelConfig value = this.autoLoader.parseModelConfig(modelName, fmVersion, autoBasicInfo, autoRatingInfo);
				DevAccessCfg.autoModelList.Add(key, value);
			}
			catch (System.Exception)
			{
				return -1;
			}
			return 0;
		}
		public int updateAutoModelList2Database(DBConn conn, string modelName, string fmVersion, string autoBasicInfo, string autoRatingInfo)
		{
			DevModelConfig devModelConfig;
			try
			{
				devModelConfig = this.autoLoader.parseModelConfig(modelName, fmVersion, autoBasicInfo, autoRatingInfo);
			}
			catch (System.Exception)
			{
				int result = -1;
				return result;
			}
			int num = 0;
			string text = modelName + "-" + fmVersion;
			if (DevAccessCfg.autoModelList.ContainsKey(text))
			{
				DevModelConfig devModelConfig2 = DevAccessCfg.autoModelList[text];
				if (devModelConfig2.autoBasicInfo.Equals(autoBasicInfo) && devModelConfig2.autoRatingInfo.Equals(autoRatingInfo))
				{
					return 0;
				}
				num = 1;
			}
			int num2 = this.autoLoader.updateModelConfig2Database(conn, modelName, fmVersion, autoBasicInfo, autoRatingInfo, num);
			if (num2 <= 0)
			{
				DebugCenter.GetInstance().appendToFile(string.Concat(new object[]
				{
					"Failed to update auto-detect info: ",
					text,
					", opCode=",
					num
				}));
				return -1;
			}
			if (DevAccessCfg.autoModelList.ContainsKey(text))
			{
				DevAccessCfg.autoModelList.Remove(text);
			}
			DevAccessCfg.autoModelList.Add(text, devModelConfig);
			if (DevAccessCfg._cbOnAutoModeUpdated != null)
			{
				DevAccessCfg._cbOnAutoModeUpdated(text, devModelConfig);
			}
			return 1;
		}
	}
}
