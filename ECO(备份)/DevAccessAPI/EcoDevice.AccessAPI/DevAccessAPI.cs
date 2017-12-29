using System;
using System.Collections.Generic;
using System.Text;
namespace EcoDevice.AccessAPI
{
	public class DevAccessAPI
	{
		public delegate void DelegateOnDeviceChanged(string operation, string targetlist);
		private DevSnmpConfig snmpCfg;
		private DevModelConfig mc;
		private SnmpConfig sc;
		private SnmpExecutor se;
		private DevAccessCfg cfg;
		public static DevAccessAPI.DelegateOnDeviceChanged cbOnDeviceChanged;
		public DevAccessAPI(DevSnmpConfig snmpSettings)
		{
			this.cfg = DevAccessCfg.GetInstance();
			this.snmpCfg = snmpSettings;
			this.sc = this.cfg.getSnmpConfig(this.snmpCfg);
			this.mc = this.cfg.getDeviceModelConfig(this.snmpCfg.modelName, this.snmpCfg.fmwareVer);
			this.se = new DefaultSnmpExecutor(new SnmpConfiger(this.sc, this.mc));
		}
		public bool SetDeviceName(string devName, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.UpdateDeviceName(devName);
		}
		public bool SetRackName(string rackName, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.UpdateRackName(rackName);
		}
		public bool SetDevicePOPSettings(DevicePOPSettings popSettings, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetDevicePOPSettings(popSettings);
		}
		public bool SetDeviceThreshold(DeviceThreshold deviceThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetDeviceThreshold(deviceThreshold);
		}
		public bool SetSensorThreshold(SensorThreshold ssrThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetSensorThreshold(ssrThreshold);
		}
		public bool SetOutletThreshold(OutletThreshold outletThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetOutletThreshold(outletThreshold);
		}
		public bool SetBankThreshold(BankThreshold bnkThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetBankThreshold(bnkThreshold);
		}
		public bool SetLineThreshold(LineThreshold lineThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetLineThreshold(lineThreshold);
		}
		public bool TurnOnOutlet(int outletIndex)
		{
			bool result = this.se.TurnOnOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool TurnOffOutlet(int outletIndex)
		{
			bool result = this.se.TurnOffOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool RebootOutlet(int outletIndex)
		{
			bool result = this.se.RebootOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool TurnOnAllOutlets()
		{
			bool result = this.se.TurnOnOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool TurnOffAllOutlets()
		{
			bool result = this.se.TurnOffOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootAllOutlets()
		{
			bool result = this.se.RebootOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootDevice()
		{
			return this.se.RebootDevice();
		}
		public bool TurnOnGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.TurnOnGroupOutlets(outlets);
			foreach (int current in outlets)
			{
				this.mc.isOutletSwitchable(current - 1);
			}
			return result;
		}
		public bool TurnOffGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.TurnOffGroupOutlets(outlets);
			foreach (int current in outlets)
			{
				this.mc.isOutletSwitchable(current - 1);
			}
			return result;
		}
		public bool RebootGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.RebootGroupOutlets(outlets);
			foreach (int current in outlets)
			{
				this.mc.isOutletSwitchable(current - 1);
			}
			return result;
		}
		public ValueMessage GetSingleDeviceValues()
		{
			return this.se.GetValues();
		}
		public ThresholdMessage GetSingleDeviceThresholds()
		{
			return this.se.GetThresholds();
		}
		public System.Collections.Generic.Dictionary<int, OutletStatus> GetDeviceOutletsStatus()
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					list.Add(i);
				}
			}
			return this.se.GetDeviceOutletsStatus(list);
		}
		public bool TurnOffBank(int bankIndex)
		{
			bool result = this.se.TurnOffBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool TurnOnBank(int bankIndex)
		{
			bool result = this.se.TurnOnBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootBank(int bankIndex)
		{
			bool result = this.se.RebootBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public DevServiceInfo GetDevServiceInfo(string myMac)
		{
			DevServiceInfo devServiceInfo = new DevServiceInfo(myMac);
			if (this.se.CheckDeviceMac(myMac) < 0)
			{
				return devServiceInfo;
			}
			devServiceInfo = this.se.GetDevServiceInfo(myMac);
			return devServiceInfo;
		}
		public bool SetDeviceName_Slave(string devName, string myMac, int index)
		{
			return this.se.CheckDeviceMac_Slave(myMac, index) && this.se.UpdateDeviceName_Slave(devName, index);
		}
		public bool SetDeviceThreshold_Slave(DeviceThreshold deviceThreshold, string myMac, int index)
		{
			return this.se.CheckDeviceMac_Slave(myMac, index) && this.se.SetDeviceThreshold_Slave(deviceThreshold, index);
		}
		public bool SetSensorThreshold_Slave(SensorThreshold ssrThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetSensorThreshold(ssrThreshold);
		}
		public bool SetOutletThreshold_Slave(OutletThreshold outletThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetOutletThreshold(outletThreshold);
		}
		public bool SetBankThreshold_Slave(BankThreshold bnkThreshold, string myMac)
		{
			return this.se.CheckDeviceMac(myMac) >= 0 && this.se.SetBankThreshold(bnkThreshold);
		}
		public bool TurnOnOutlet_Slave(int outletIndex)
		{
			bool result = this.se.TurnOnOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool TurnOffOutlet_Slave(int outletIndex)
		{
			bool result = this.se.TurnOffOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool RebootOutlet_Slave(int outletIndex)
		{
			bool result = this.se.RebootOutlet(outletIndex);
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + outletIndex);
			}
			return result;
		}
		public bool TurnOnAllOutlets_Slave()
		{
			bool result = this.se.TurnOnOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool TurnOffAllOutlets_Slave()
		{
			bool result = this.se.TurnOffOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootAllOutlets_Slave()
		{
			bool result = this.se.RebootOutlets();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootDevice_Slave()
		{
			return this.se.RebootDevice();
		}
		public bool TurnOnGroupOutlets_Slave(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.TurnOnGroupOutlets(outlets);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (int current in outlets)
			{
				if (this.mc.isOutletSwitchable(current - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(current.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool TurnOffGroupOutlets_Slave(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.TurnOffGroupOutlets(outlets);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (int current in outlets)
			{
				if (this.mc.isOutletSwitchable(current - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(current.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootGroupOutlets_Slave(System.Collections.Generic.List<int> outlets)
		{
			bool result = this.se.RebootGroupOutlets(outlets);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (int current in outlets)
			{
				if (this.mc.isOutletSwitchable(current - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(current.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public ValueMessage GetSingleDeviceValues_Slave()
		{
			return this.se.GetValues();
		}
		public ThresholdMessage GetSingleDeviceThresholds_Slave()
		{
			return this.se.GetThresholds();
		}
		public System.Collections.Generic.Dictionary<int, OutletStatus> GetDeviceOutletsStatus_Slave()
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			for (int i = 1; i <= this.mc.portNum; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					list.Add(i);
				}
			}
			return this.se.GetDeviceOutletsStatus(list);
		}
		public bool TurnOffBank_Slave(int bankIndex)
		{
			bool result = this.se.TurnOffBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool TurnOnBank_Slave(int bankIndex)
		{
			bool result = this.se.TurnOnBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public bool RebootBank_Slave(int bankIndex)
		{
			bool result = this.se.RebootBank(bankIndex);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = this.mc.bankOutlets[bankIndex - 1].fromPort; i <= this.mc.bankOutlets[bankIndex - 1].toPort; i++)
			{
				if (this.mc.isOutletSwitchable(i - 1))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(i.ToString());
				}
			}
			if (DevAccessAPI.cbOnDeviceChanged != null)
			{
				DevAccessAPI.cbOnDeviceChanged("pending", this.snmpCfg.devID + ":" + stringBuilder.ToString());
			}
			return result;
		}
		public int GetEatonPDUBankNumber()
		{
			return this.se.GetEatonPDUBankNumber();
		}
		public DevRealConfig GetEatonPDUNumber_M2()
		{
			return this.se.GetEatonPDUNumber_M2();
		}
	}
}
