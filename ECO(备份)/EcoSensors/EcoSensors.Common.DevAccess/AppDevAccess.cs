using EcoDevice.AccessAPI;
using System;
using System.Collections.Generic;
namespace EcoSensors.Common.DevAccess
{
	internal class AppDevAccess
	{
		public System.Collections.Generic.Dictionary<string, bool> SetDeviceThresholds(System.Collections.Generic.List<DevSnmpConfig> configs, DeviceThreshold deviceThreshold)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			System.Collections.Generic.List<string> list2 = new AppSnmpExecutors(list).SetDevThresholds(deviceThreshold);
			System.Collections.Generic.Dictionary<string, bool> dictionary = new System.Collections.Generic.Dictionary<string, bool>();
			foreach (string current2 in list2)
			{
				string[] array = current2.Split(new char[]
				{
					':'
				});
				dictionary.Add(array[0], System.Convert.ToBoolean(array[1]));
			}
			return dictionary;
		}
		public System.Collections.Generic.Dictionary<string, bool> SetDevicePOPs(System.Collections.Generic.List<DevSnmpConfig> configs, DevicePOPSettings pop2dev)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			System.Collections.Generic.List<string> list2 = new AppSnmpExecutors(list).SetDevPOPs(pop2dev);
			System.Collections.Generic.Dictionary<string, bool> dictionary = new System.Collections.Generic.Dictionary<string, bool>();
			foreach (string current2 in list2)
			{
				string[] array = current2.Split(new char[]
				{
					':'
				});
				dictionary.Add(array[0], System.Convert.ToBoolean(array[1]));
			}
			return dictionary;
		}
		public System.Collections.Generic.Dictionary<string, bool> SetSensorThreshold(System.Collections.Generic.List<DevSnmpConfig> configs, SensorThreshold SSThreshold)
		{
			DevAccessCfg instance = DevAccessCfg.GetInstance();
			System.Collections.Generic.List<SnmpConfiger> list = new System.Collections.Generic.List<SnmpConfiger>();
			foreach (DevSnmpConfig current in configs)
			{
				DevModelConfig deviceModelConfig = instance.getDeviceModelConfig(current.modelName, current.fmwareVer);
				SnmpConfiger item = new SnmpConfiger(instance.getSnmpConfig(current), deviceModelConfig, current.devMac, current.devID);
				list.Add(item);
			}
			System.Collections.Generic.List<string> list2 = new AppSnmpExecutors(list).SetSensorThreshold(SSThreshold);
			System.Collections.Generic.Dictionary<string, bool> dictionary = new System.Collections.Generic.Dictionary<string, bool>();
			foreach (string current2 in list2)
			{
				string[] array = current2.Split(new char[]
				{
					':'
				});
				dictionary.Add(array[0], System.Convert.ToBoolean(array[1]));
			}
			return dictionary;
		}
	}
}
