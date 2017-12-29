using CommonAPI.CultureTransfer;
using EcoDevice.AccessAPI;
using System;
using System.Collections;
using System.Collections.Generic;
namespace EcoSensors.Common.DevAccess
{
	internal class AppSnmpExecutors
	{
		private System.Collections.Generic.List<SnmpConfiger> snmpConfigs;
		public AppSnmpExecutors(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			this.snmpConfigs = snmpConfigs;
		}
		public System.Collections.Generic.List<string> SetDevThresholds(DeviceThreshold deviceThreshold)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			SystemThreadPool<SnmpConfiger, string> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, string>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string arg = CultureTransfer.ToString(snmpConfiger.DeviceID);
				bool flag = false;
				try
				{
					flag = snmpExecutor.SetDeviceThreshold(deviceThreshold);
				}
				catch (System.Exception)
				{
				}
				lock (col)
				{
					((System.Collections.Generic.List<string>)col).Add(arg + ":" + flag);
				}
			});
		}
		public System.Collections.Generic.List<string> SetDevPOPs(DevicePOPSettings pop2dev)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			SystemThreadPool<SnmpConfiger, string> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, string>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string arg = CultureTransfer.ToString(snmpConfiger.DeviceID);
				bool flag = false;
				try
				{
					flag = snmpExecutor.SetDevicePOPSettings(pop2dev);
				}
				catch (System.Exception)
				{
				}
				lock (col)
				{
					((System.Collections.Generic.List<string>)col).Add(arg + ":" + flag);
				}
			});
		}
		public System.Collections.Generic.List<string> SetSensorThreshold(SensorThreshold SSThreshold)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			SystemThreadPool<SnmpConfiger, string> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, string>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string arg = CultureTransfer.ToString(snmpConfiger.DeviceID);
				bool flag = false;
				try
				{
					flag = snmpExecutor.SetSensorThreshold(SSThreshold);
				}
				catch (System.Exception)
				{
				}
				lock (col)
				{
					((System.Collections.Generic.List<string>)col).Add(arg + ":" + flag);
				}
			});
		}
	}
}
