using CommonAPI;
using DBAccessAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class DevDiscoverAPI
	{
		public delegate void DelegateOnAutoModeBatch(System.Collections.Generic.List<DevModelConfig> newModels);
		private DevSnmpConfig snmpCfg;
		private SnmpConfig sc;
		private DevAccessCfg cfg;
		private int total;
		public static DevDiscoverAPI.DelegateOnAutoModeBatch _cbOnAutoModePost;
		public DevDiscoverAPI(DevSnmpConfig snmpSettings)
		{
			this.cfg = DevAccessCfg.GetInstance();
			this.snmpCfg = snmpSettings;
			this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
		}
		public System.Collections.Generic.List<PropertiesMessage> Scan_ALL(string startIp, string endIp)
		{
			System.Collections.Generic.List<string> list = this.countIp(startIp, endIp);
			if (list == null || list.Count < 1)
			{
				return null;
			}
			this.total = list.Count;
			System.Collections.Generic.List<SnmpConfiger> snmpConfigs = this.createSnmpConfig(list);
			return new DefaultSnmpExecutors(snmpConfigs).GetProperties_ALL();
		}
		public System.Collections.Generic.List<PropertiesMessage> Scan_ATEN(string startIp, string endIp)
		{
			System.Collections.Generic.List<string> list = this.countIp(startIp, endIp);
			if (list == null || list.Count < 1)
			{
				return null;
			}
			this.total = list.Count;
			System.Collections.Generic.List<SnmpConfiger> snmpConfigs = this.createSnmpConfig(list);
			System.Collections.Generic.List<PropertiesMessage> properties_ATEN = new DefaultSnmpExecutors(snmpConfigs).GetProperties_ATEN();
			System.Collections.Generic.List<PropertiesMessage> list2 = new System.Collections.Generic.List<PropertiesMessage>();
			System.Collections.Generic.List<DevModelConfig> list3 = new System.Collections.Generic.List<DevModelConfig>();
			if (properties_ATEN != null && properties_ATEN.Count > 0)
			{
				DBConn connection = DBConnPool.getConnection();
				foreach (PropertiesMessage current in properties_ATEN)
				{
					if (!current.ModelName.Equals("Null"))
					{
						if (!string.IsNullOrEmpty(current.AutoBasicInfo) && !string.IsNullOrEmpty(current.AutoRatingInfo))
						{
							try
							{
								int num = this.cfg.updateAutoModelList2Database(connection, current.ModelName, current.FirwWareVersion, current.AutoBasicInfo, current.AutoRatingInfo);
								if (num > 0)
								{
									list3.Add(new DevModelConfig(current.ModelName)
									{
										modelName = current.ModelName,
										firmwareVer = current.FirwWareVersion,
										autoBasicInfo = current.AutoBasicInfo,
										autoRatingInfo = current.AutoRatingInfo
									});
								}
								else
								{
									if (num != 0 && num < 0)
									{
										DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
										{
											"Failed to update auto-detect info: ",
											current.ModelName,
											", ",
											current.AutoBasicInfo,
											current.AutoRatingInfo
										}));
										continue;
									}
								}
							}
							catch (System.Exception)
							{
								DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
								{
									"Exception when parsing auto-detect info: ",
									current.ModelName,
									", ",
									current.AutoBasicInfo,
									current.AutoRatingInfo
								}));
								continue;
							}
						}
						list2.Add(current);
					}
				}
				try
				{
					connection.Close();
				}
				catch
				{
				}
				if (DevDiscoverAPI._cbOnAutoModePost != null && list3.Count > 0)
				{
					DevDiscoverAPI._cbOnAutoModePost(list3);
				}
			}
			return list2;
		}
		public System.Collections.Generic.List<PropertiesMessage> Scan_EATON(string startIp, string endIp)
		{
			System.Collections.Generic.List<PropertiesMessage> list = new System.Collections.Generic.List<PropertiesMessage>();
			System.Collections.Generic.List<string> list2 = this.countIp(startIp, endIp);
			if (list2 == null || list2.Count < 1)
			{
				return null;
			}
			this.total = list2.Count;
			System.Collections.Generic.List<SnmpConfiger> snmpConfigs = this.createSnmpConfig(list2);
			System.Collections.Generic.List<PropertiesMessage> eatonPDUProperties = new DefaultSnmpExecutors(snmpConfigs).GetEatonPDUProperties();
			System.Collections.Generic.List<PropertiesMessage> eatonPDUProperties_M = new DefaultSnmpExecutors(snmpConfigs).GetEatonPDUProperties_M2();
			if (eatonPDUProperties != null)
			{
				foreach (PropertiesMessage current in eatonPDUProperties)
				{
					list.Add(current);
				}
			}
			if (eatonPDUProperties_M != null)
			{
				foreach (PropertiesMessage current2 in eatonPDUProperties_M)
				{
					list.Add(current2);
				}
			}
			return list;
		}
		public System.Collections.Generic.List<PropertiesMessage> Scan_APC(string startIp, string endIp)
		{
			System.Collections.Generic.List<string> list = this.countIp(startIp, endIp);
			if (list == null || list.Count < 1)
			{
				return null;
			}
			this.total = list.Count;
			System.Collections.Generic.List<SnmpConfiger> snmpConfigs = this.createSnmpConfig(list);
			return new DefaultSnmpExecutors(snmpConfigs).GetApcPDUProperties();
		}
		private System.Collections.Generic.List<SnmpConfiger> createSnmpConfig(System.Collections.Generic.List<string> ips)
		{
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (string current in ips)
			{
				SnmpConfig snmpConfig;
				if (this.snmpCfg.snmpVer == 0)
				{
					snmpConfig = new SnmpV1Config(current);
					((SnmpV1Config)snmpConfig).Community = this.snmpCfg.userName;
				}
				else
				{
					if (this.snmpCfg.snmpVer == 1)
					{
						snmpConfig = new SnmpV2Config(current);
						((SnmpV2Config)snmpConfig).Community = this.snmpCfg.userName;
					}
					else
					{
						snmpConfig = new SnmpV3Config(current);
						SnmpV3Config snmpV3Config = snmpConfig as SnmpV3Config;
						snmpV3Config.UserName = this.snmpCfg.userName;
						snmpV3Config.Authentication = (Authentication)System.Enum.Parse(typeof(Authentication), this.snmpCfg.authType);
						snmpV3Config.AuthSecret = this.snmpCfg.authPSW;
						snmpV3Config.Privacy = (Privacy)System.Enum.Parse(typeof(Privacy), this.snmpCfg.privType);
						snmpV3Config.PrivacySecret = this.snmpCfg.privPSW;
					}
				}
				snmpConfig.Port = this.snmpCfg.devPort;
				snmpConfig.Retry = this.snmpCfg.retry;
				snmpConfig.Timeout = this.snmpCfg.timeout;
				SnmpConfiger item = new SnmpConfiger(snmpConfig);
				list.Add(item);
			}
			return list;
		}
		private System.Collections.Generic.List<string> countIp(string startIp, string endIp)
		{
			if (startIp == null || "".Equals(startIp))
			{
				throw new System.ArgumentNullException("StartIp can not be null or empty.");
			}
			int num = startIp.LastIndexOf(".");
			int num2 = System.Convert.ToInt32(startIp.Substring(num + 1));
			string text = startIp.Substring(0, num + 1);
			int num3 = 255;
			if (!string.IsNullOrEmpty(endIp))
			{
				int num4 = endIp.LastIndexOf(".");
				num3 = System.Convert.ToInt32(endIp.Substring(num4 + 1));
				string text2 = endIp.Substring(0, num4 + 1);
				if (!text2.Equals(text))
				{
					throw new System.ArgumentException("StartIp and endIp must be in the same subnet.");
				}
			}
			if (num2 > num3 || num3 > 255)
			{
				throw new System.Exception("Invalid startIp and endIp");
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			for (int i = num2; i <= num3; i++)
			{
				list.Add(text + i);
			}
			return list;
		}
	}
}
