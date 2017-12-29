using DBAccessAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class DevMonitorAPI
	{
		private System.Collections.Generic.List<SnmpConfiger> retryConfigs = new System.Collections.Generic.List<SnmpConfiger>();
		public System.Collections.Generic.List<SnmpConfiger> GetThresholdRetryList()
		{
			return this.retryConfigs;
		}
		public System.Collections.Generic.List<ValueMessage> GetMonitorValues(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			return new DefaultSnmpExecutors(snmpConfigs).GetVaules();
		}
		public System.Collections.Generic.List<ValueMessage> GetMonitorValues(System.Collections.Generic.List<DevSnmpConfig> configs)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			return new DefaultSnmpExecutors(list).GetVaules();
		}
		public System.Collections.Generic.List<ThresholdMessage> GetMonitorThresholds(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(snmpConfigs);
			System.Collections.Generic.List<ThresholdMessage> thresholds = defaultSnmpExecutors.GetThresholds();
			this.retryConfigs = defaultSnmpExecutors.getRetryList();
			return thresholds;
		}
		public System.Collections.Generic.List<ThresholdMessage> GetMonitorThresholds(System.Collections.Generic.List<DevSnmpConfig> configs)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(list);
			System.Collections.Generic.List<ThresholdMessage> thresholds = defaultSnmpExecutors.GetThresholds();
			this.retryConfigs = defaultSnmpExecutors.getRetryList();
			return thresholds;
		}
		public void SetTrapReceiver(System.Collections.Generic.List<DevSnmpConfig> configs, Sys_Para pSys)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(list);
			defaultSnmpExecutors.UpdateTrapReceiver(pSys, TrapEnabled.Yes);
		}
		public void SetTrapReceiver(System.Collections.Generic.List<SnmpConfiger> snmpConfigs, Sys_Para pSys)
		{
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(snmpConfigs);
			defaultSnmpExecutors.UpdateTrapReceiver(pSys, TrapEnabled.Yes);
		}
		public string[] RestoreThresholds2Device(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(snmpConfigs);
			return defaultSnmpExecutors.RestoreThresholds2Device();
		}
		public System.Collections.Generic.List<SnmpConfiger> SetDeviceVoltages(System.Collections.Generic.List<SnmpConfiger> snmpConfigs, float voltage)
		{
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (SnmpConfiger current in snmpConfigs)
			{
				if (current.DevModel.Equals("EC1000"))
				{
					list.Add(current);
				}
			}
			DefaultSnmpExecutors defaultSnmpExecutors = new DefaultSnmpExecutors(list);
			return defaultSnmpExecutors.UpdateDeviceVoltages(voltage);
		}
		public int GetEatonPDUBankNumber(DevSnmpConfig cfg)
		{
			this.GetEatonPDUNumber_M2(cfg);
			DevAccessAPI devAccessAPI = new DevAccessAPI(cfg);
			return devAccessAPI.GetEatonPDUBankNumber();
		}
		public DevRealConfig GetEatonPDUNumber_M2(DevSnmpConfig cfg)
		{
			DevAccessAPI devAccessAPI = new DevAccessAPI(cfg);
			return devAccessAPI.GetEatonPDUNumber_M2();
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsEatonPDU_M2(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			return new DefaultSnmpExecutors(snmpConfigs).GetThresholdsEatonPDU_M2();
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsEatonPDU_M2(System.Collections.Generic.List<DevSnmpConfig> configs)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevAccessAPI devAccessAPI = new DevAccessAPI(current);
				DevRealConfig eatonPDUNumber_M = devAccessAPI.GetEatonPDUNumber_M2();
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID, eatonPDUNumber_M);
				list.Add(item);
			}
			return new DefaultSnmpExecutors(list).GetThresholdsEatonPDU_M2();
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsApcPDU(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			return new DefaultSnmpExecutors(snmpConfigs).GetThresholdsApcPDU();
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsApcPDU(System.Collections.Generic.List<DevSnmpConfig> configs)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			return new DefaultSnmpExecutors(list).GetThresholdsApcPDU();
		}
	}
}
