using DBAccessAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public interface SnmpExecutors
	{
		bool OnLine
		{
			get;
		}
		void UpdateTrapReceiver(Sys_Para pSys, TrapEnabled open);
		System.Collections.Generic.List<PropertiesMessage> GetProperties_ALL();
		System.Collections.Generic.List<PropertiesMessage> GetProperties_ATEN();
		System.Collections.Generic.List<PropertiesMessage> GetEatonPDUProperties();
		System.Collections.Generic.List<PropertiesMessage> GetChainProperties();
		System.Collections.Generic.List<PropertiesMessage> GetEatonPDUProperties_M2();
		System.Collections.Generic.List<PropertiesMessage> GetApcPDUProperties();
		void GetProperties(ExecutorCallBack callBack);
		System.Collections.Generic.List<ThresholdMessage> GetThresholds();
		System.Collections.Generic.List<ThresholdMessage> GetChainThresholds();
		System.Collections.Generic.List<ThresholdMessage> GetThresholdsEatonPDU_M2();
		System.Collections.Generic.List<ThresholdMessage> GetThresholdsApcPDU();
		System.Collections.Generic.List<ValueMessage> GetVaules();
		System.Collections.Generic.List<ValueMessage> GetChainVaules();
		System.Collections.Generic.List<SnmpConfiger> UpdateDeviceVoltages(float voltage);
	}
}
