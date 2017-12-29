using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public interface SnmpExecutor
	{
		bool Online
		{
			get;
		}
		int DeviceHttpPort
		{
			get;
		}
		SnmpConfig SnmpConfig
		{
			get;
		}
		SnmpSession SnmpSession
		{
			get;
		}
		int CheckDeviceMac(string myMac);
		bool CheckDeviceMac_Slave(string myMac, int index);
		TrapReceiverConfiguration GetTrapReceiverConfig(int index);
		bool UpdateDeviceName(string name);
		bool UpdateDeviceName_Slave(string name, int index);
		bool UpdateRackName(string rackName);
		bool ConfigTrapReceiver(TrapReceiverConfiguration traps);
		bool ConfigTrapReceiver(System.Collections.Generic.List<TrapReceiverConfiguration> traps);
		bool SetDevicePOPSettings(DevicePOPSettings popSettings);
		bool SetDeviceThreshold(DeviceThreshold deviceThreshold);
		bool SetDeviceThreshold_Slave(DeviceThreshold deviceThreshold, int index);
		bool SetSensorThreshold(SensorThreshold sensorsThreshold);
		bool SetSensorThreshold_Slave(SensorThreshold sensorsThreshold, int index);
		bool SetBankThreshold(BankThreshold banksThreshold);
		bool SetBankThreshold_Slave(BankThreshold banksThreshold, int index);
		bool SetLineThreshold(LineThreshold lineThreshold);
		bool SetOutletThreshold(OutletThreshold outletThreshold);
		bool SetOutletThreshold_Slave(OutletThreshold outletThreshold, int index);
		OutletStatus GetOutletStatus(int outletNumber);
		System.Collections.Generic.Dictionary<int, OutletStatus> GetDeviceOutletsStatus(System.Collections.Generic.List<int> outlets);
		void GetProperties(ExecutorCallBack callback);
		PropertiesMessage GetProperties_ALL();
		PropertiesMessage GetProperties_ATEN();
		PropertiesMessage GetEatonPDUProperties();
		PropertiesMessage GetEatonPDUProperties_M2();
		PropertiesMessage GetApcPDUProperties();
		System.Collections.Generic.List<PropertiesMessage> GetChainProperties();
		ThresholdMessage GetThresholds();
		System.Collections.Generic.List<ThresholdMessage> GetChainThresholds();
		ThresholdMessage GetThresholdsEatonPDU_M2();
		ThresholdMessage GetThresholdsApcPDU();
		void GetThresholds(ExecutorCallBack callback);
		ValueMessage GetValues();
		System.Collections.Generic.List<ValueMessage> GetChainValues();
		ValueMessage GetValuesEatonPDU_M2();
		ValueMessage GetValuesApcPDU();
		void GetValues(ExecutorCallBack callback);
		bool TurnOffOutlet(int outletNumber);
		bool TurnOffOutlets();
		bool TurnOnOutlets();
		bool TurnOnOutlet(int outletNumber);
		bool RebootOutlet(int outletNumber);
		bool RebootOutlets();
		bool RebootDevice();
		bool TurnOnGroupOutlets(System.Collections.Generic.List<int> outlets);
		bool TurnOffGroupOutlets(System.Collections.Generic.List<int> outlets);
		bool RebootGroupOutlets(System.Collections.Generic.List<int> outlets);
		bool TurnOffBank(int bankNumber);
		bool TurnOnBank(int bankNumber);
		bool RebootBank(int bankNumber);
		DevServiceInfo GetDevServiceInfo(string myMac);
		bool UpdateDeviceVoltage(float voltage);
		int GetEatonPDUBankNumber();
		DevRealConfig GetEatonPDUNumber_M2();
	}
}
