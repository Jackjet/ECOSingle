using System;
using System.Collections.Generic;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class DefaultSnmpExecutor : SnmpExecutor
	{
		private SnmpConfig snmpConfig;
		private SnmpSession snmpSession;
		private string devModel;
		private int portNums;
		private int switchable;
		private int perportreading;
		private int sensornumber;
		private int banknumber;
		private int linenumber;
		private DevModelConfig modelConfig;
		private DevRealConfig realConfig;
		private int perbankreading;
		private int perdoorreading;
		private string devMac = "";
		private int devID;
		public SnmpSession SnmpSession
		{
			get
			{
				return this.snmpSession;
			}
		}
		public SnmpConfig SnmpConfig
		{
			get
			{
				return this.snmpConfig;
			}
		}
		public bool Online
		{
			get
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(DeviceBaseMib.ModelName);
				bool result;
				try
				{
					this.snmpSession.Get(leafVarBinding);
					result = true;
				}
				catch (System.Exception)
				{
					result = false;
				}
				return result;
			}
		}
		public int DeviceHttpPort
		{
			get
			{
				int result = 0;
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(ServicePorts.HttpPort);
				try
				{
					System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
					if (dictionary == null || dictionary.Count < 1)
					{
						int result2 = 0;
						return result2;
					}
					System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string value = dictionary[current];
						if (current.StartsWith(ServicePorts.HttpPort))
						{
							result = System.Convert.ToInt32(value);
						}
					}
				}
				catch (System.Exception)
				{
					int result2 = 0;
					return result2;
				}
				return result;
			}
		}
		public DefaultSnmpExecutor(SnmpConfiger snmpConfig)
		{
			this.devModel = snmpConfig.DevModel;
			this.sensornumber = snmpConfig.SensorNumber;
			this.perportreading = snmpConfig.PerPortReading;
			this.switchable = snmpConfig.Switchable;
			this.portNums = snmpConfig.PortNumbers;
			this.snmpConfig = snmpConfig.SnmpConfig;
			this.modelConfig = snmpConfig.DevModelConfig;
			this.banknumber = snmpConfig.BankNumber;
			this.linenumber = snmpConfig.LineNumber;
			this.perbankreading = snmpConfig.PerBankReading;
			this.perdoorreading = snmpConfig.PerDoorReading;
			this.devMac = snmpConfig.DeviceMac;
			this.devID = snmpConfig.DeviceID;
			this.snmpSession = SnmpSessionFactory.CreateSession(this.snmpConfig);
			this.realConfig = snmpConfig.DevRealConfig;
		}
		public int CheckDeviceMac(string myMac)
		{
			if (myMac == null || myMac.Length <= 0)
			{
				return 0;
			}
			string value = "";
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				leafVarBinding.Add(EatonPDUBaseMib_M2.Mac);
			}
			else
			{
				if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
				{
					leafVarBinding.Add(ApcPDUBaseMib.Mac);
				}
				else
				{
					leafVarBinding.Add(DeviceBaseMib.Mac);
				}
			}
			int result;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = -2;
				}
				else
				{
					System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string text = dictionary[current];
						if (current.StartsWith(DeviceBaseMib.Mac) || current.StartsWith(EatonPDUBaseMib_M2.Mac) || current.StartsWith(ApcPDUBaseMib.Mac))
						{
							value = text.Replace(" ", ":").Replace("-", ":");
							break;
						}
					}
					if (myMac.Equals(value))
					{
						result = 1;
					}
					else
					{
						result = -1;
					}
				}
			}
			catch (System.Exception)
			{
				result = -3;
			}
			return result;
		}
		public bool CheckDeviceMac_Slave(string myMac, int index)
		{
			if (myMac == null || myMac.Length <= 0)
			{
				return true;
			}
			string value = "";
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveBaseMib slaveBaseMib = new SlaveBaseMib(index);
			leafVarBinding.Add(slaveBaseMib.SlaveDeviceMac);
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					bool result = false;
					return result;
				}
				System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					string text = dictionary[current];
					if (current.StartsWith(slaveBaseMib.SlaveDeviceMac))
					{
						value = text;
						break;
					}
				}
				if (myMac.Equals(value))
				{
					bool result = true;
					return result;
				}
			}
			catch (System.Exception)
			{
				bool result = false;
				return result;
			}
			return false;
		}
		public bool SetDevicePOPSettings(DevicePOPSettings popSettings)
		{
			if (this.modelConfig.popReading != Constant.YES)
			{
				return true;
			}
			if (popSettings == null)
			{
				throw new SnmpException("The instance of DeviceThreshold is null.");
			}
			LeafVarBinding varBindings;
			if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
			{
				varBindings = AutoMibParser.SetDevicePOPSettingsVariables(popSettings, this.modelConfig);
			}
			else
			{
				varBindings = MibParser.SetDevicePOPSettingsVariables(popSettings, this.modelConfig);
			}
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				bool result;
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			return true;
		}
		public bool SetDeviceThreshold(DeviceThreshold deviceThreshold)
		{
			if (deviceThreshold == null)
			{
				throw new SnmpException("The instance of DeviceThreshold is null.");
			}
			LeafVarBinding varBindings;
			if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				varBindings = EatonMibParser.SetDeviceThresholdVariablesEatonPDU_M2(deviceThreshold, this.modelConfig);
			}
			else
			{
				if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
				{
					System.Collections.Generic.List<LeafVarBinding> list = ApcMibParser.SetDeviceThresholdVariablesApcPDU(deviceThreshold, this.modelConfig);
					foreach (LeafVarBinding current in list)
					{
						try
						{
							this.snmpSession.Set(current);
						}
						catch (System.Exception)
						{
							bool result = false;
							return result;
						}
					}
					return true;
				}
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					varBindings = AutoMibParser.SetDeviceThresholdVariables(deviceThreshold, this.modelConfig);
				}
				else
				{
					varBindings = MibParser.SetDeviceThresholdVariables(deviceThreshold, this.modelConfig);
				}
			}
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				bool result;
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			return true;
		}
		public bool SetDeviceThreshold_Slave(DeviceThreshold deviceThreshold, int index)
		{
			if (deviceThreshold == null)
			{
				throw new SnmpException("The instance of DeviceThreshold is null.");
			}
			LeafVarBinding varBindings = MibChainParser.SetSlaveDeviceThresholdVariables(deviceThreshold, this.modelConfig, index);
			bool result;
			try
			{
				this.snmpSession.Set(varBindings);
				result = true;
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public bool SetOutletThreshold(OutletThreshold outletThreshold)
		{
			if (outletThreshold == null)
			{
				throw new SnmpException("The instance of OutletThreshold is null.");
			}
			LeafVarBinding varBindings;
			if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				varBindings = EatonMibParser.SetOutletThresholdVariablesEatonPDU_M2(outletThreshold, this.modelConfig);
			}
			else
			{
				if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
				{
					return true;
				}
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					varBindings = AutoMibParser.SetOutletThresholdVariables(outletThreshold, this.modelConfig);
				}
				else
				{
					varBindings = MibParser.SetOutletThresholdVariables(outletThreshold, this.modelConfig);
				}
			}
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				bool result;
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			return true;
		}
		public bool SetOutletThreshold_Slave(OutletThreshold outletThreshold, int index)
		{
			if (outletThreshold == null)
			{
				throw new SnmpException("The instance of OutletThreshold is null.");
			}
			LeafVarBinding varBindings = MibChainParser.SetSlaveOutletThresholdVariables(outletThreshold, this.modelConfig, index);
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool SetSensorThreshold(SensorThreshold sensorsThreshold)
		{
			if (sensorsThreshold == null)
			{
				throw new SnmpException("The instance of SensorThreshold is null.");
			}
			if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				return false;
			}
			if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
			{
				System.Collections.Generic.List<LeafVarBinding> list = ApcMibParser.SetSensorThresholdVariablesApcPDU(sensorsThreshold, this.modelConfig);
				foreach (LeafVarBinding current in list)
				{
					try
					{
						this.snmpSession.Set(current);
					}
					catch (System.Exception)
					{
						bool result = false;
						return result;
					}
				}
				return true;
			}
			LeafVarBinding varBindings;
			if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
			{
				varBindings = AutoMibParser.SetSensorThresholdVariables(sensorsThreshold, this.modelConfig);
			}
			else
			{
				varBindings = MibParser.SetSensorThresholdVariables(sensorsThreshold, this.modelConfig);
			}
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				bool result;
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			return true;
		}
		public bool SetSensorThreshold_Slave(SensorThreshold sensorsThreshold, int index)
		{
			if (sensorsThreshold == null)
			{
				throw new SnmpException("The instance of SensorThreshold is null.");
			}
			LeafVarBinding varBindings = MibChainParser.SetSlaveSensorThresholdVariables(sensorsThreshold, this.modelConfig, index);
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool SetBankThreshold(BankThreshold banksThreshold)
		{
			if (banksThreshold == null)
			{
				throw new SnmpException("The instance of BankThreshold is null.");
			}
			LeafVarBinding varBindings;
			if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				varBindings = EatonMibParser.SetBankThresholdVariablesEatonPDU_M2(banksThreshold, this.modelConfig);
			}
			else
			{
				if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
				{
					System.Collections.Generic.List<LeafVarBinding> list = ApcMibParser.SetBankThresholdVariablesApcPDU(banksThreshold, this.modelConfig);
					foreach (LeafVarBinding current in list)
					{
						try
						{
							this.snmpSession.Set(current);
						}
						catch (System.Exception)
						{
							bool result = false;
							return result;
						}
					}
					return true;
				}
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					varBindings = AutoMibParser.SetBankThresholdVariables(banksThreshold, this.modelConfig);
				}
				else
				{
					varBindings = MibParser.SetBankThresholdVariables(banksThreshold, this.modelConfig);
				}
			}
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				bool result;
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					result = false;
					return result;
				}
				result = true;
				return result;
			}
			return true;
		}
		public bool SetBankThreshold_Slave(BankThreshold banksThreshold, int index)
		{
			if (banksThreshold == null)
			{
				throw new SnmpException("The instance of BankThreshold is null.");
			}
			LeafVarBinding varBindings = MibChainParser.SetSlaveBankThresholdVariables(banksThreshold, this.modelConfig, index);
			try
			{
				this.snmpSession.Set(varBindings);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool SetLineThreshold(LineThreshold lineThreshold)
		{
			if (lineThreshold == null)
			{
				throw new SnmpException("The instance of LineThreshold is null.");
			}
			if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
			{
				LeafVarBinding varBindings = AutoMibParser.SetLineThresholdVariables(lineThreshold, this.modelConfig);
				try
				{
					this.snmpSession.Set(varBindings);
				}
				catch (System.Exception)
				{
					bool result;
					try
					{
						this.snmpSession.Set(varBindings);
					}
					catch (System.Exception)
					{
						result = false;
						return result;
					}
					result = true;
					return result;
				}
				return true;
			}
			return false;
		}
		public void GetProperties(ExecutorCallBack callback)
		{
			if (callback == null)
			{
				throw new SnmpException("[GetProperties]ExecutorCallBack can not be null.");
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.makeAsyncGetProperties), callback);
		}
		private void makeAsyncGetProperties(object obj)
		{
			PropertiesMessage obj2 = this.doGetProperties_ATEN();
			ExecutorCallBack executorCallBack = obj as ExecutorCallBack;
			executorCallBack(obj2);
		}
		public PropertiesMessage GetProperties_ALL()
		{
			return this.doGetProperties_ALL();
		}
		private PropertiesMessage doGetProperties_ALL()
		{
			PropertiesMessage propertiesMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(MibManager.GetDiscoveryRequest_ALL());
				if (dictionary == null || dictionary.Count < 1)
				{
					return null;
				}
				propertiesMessage = MibManager.GetDiscoveryMessage_ALL(dictionary);
				if (propertiesMessage != null)
				{
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
				}
			}
			catch (System.Exception)
			{
			}
			return propertiesMessage;
		}
		public PropertiesMessage GetProperties_ATEN()
		{
			return this.doGetProperties_ATEN();
		}
		private PropertiesMessage doGetProperties_ATEN()
		{
			PropertiesMessage propertiesMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(MibParser.GetDevicePropertiesRequest());
				if (dictionary == null || dictionary.Count < 1)
				{
					return null;
				}
				propertiesMessage = MibParser.GetDevicePropertiesMessage(dictionary);
				if (propertiesMessage != null)
				{
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
				}
			}
			catch (System.Exception)
			{
			}
			return propertiesMessage;
		}
		public PropertiesMessage GetEatonPDUProperties()
		{
			return this.doGetEatonPDUProperties();
		}
		private PropertiesMessage doGetEatonPDUProperties()
		{
			PropertiesMessage propertiesMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(MibParser.GetEatonPDUPropertiesRequest());
				if (dictionary == null || dictionary.Count < 1)
				{
					return null;
				}
				propertiesMessage = MibParser.GetEatonPDUPropertiesMessage(dictionary);
				if (propertiesMessage != null)
				{
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
				}
			}
			catch (System.Exception)
			{
			}
			return propertiesMessage;
		}
		public PropertiesMessage GetEatonPDUProperties_M2()
		{
			return this.doGetEatonPDUProperties_M2();
		}
		private PropertiesMessage doGetEatonPDUProperties_M2()
		{
			PropertiesMessage propertiesMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(EatonMibParser.GetEatonPDUPropertiesRequest_M2());
				if (dictionary == null || dictionary.Count < 1)
				{
					return null;
				}
				propertiesMessage = EatonMibParser.GetEatonPDUPropertiesMessage_M2(dictionary);
				if (propertiesMessage != null)
				{
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
				}
			}
			catch (System.Exception)
			{
			}
			return propertiesMessage;
		}
		public PropertiesMessage GetApcPDUProperties()
		{
			return this.doGetApcPDUProperties();
		}
		private PropertiesMessage doGetApcPDUProperties()
		{
			PropertiesMessage propertiesMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(ApcMibParser.GetApcPDUPropertiesRequest());
				if (dictionary == null || dictionary.Count < 1)
				{
					return null;
				}
				propertiesMessage = ApcMibParser.GetApcPDUPropertiesMessage(dictionary);
				if (propertiesMessage != null)
				{
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
				}
			}
			catch (System.Exception)
			{
			}
			return propertiesMessage;
		}
		public System.Collections.Generic.List<PropertiesMessage> GetChainProperties()
		{
			return this.doGetChainProperties();
		}
		private System.Collections.Generic.List<PropertiesMessage> doGetChainProperties()
		{
			System.Collections.Generic.List<PropertiesMessage> list = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(MibChainParser.GetChainPropertiesRequest());
				if (dictionary == null || dictionary.Count < 1)
				{
					System.Collections.Generic.List<PropertiesMessage> result = null;
					return result;
				}
				list = MibChainParser.GetChainPropertiesMessage(dictionary);
				if (list != null && list.Count > 0)
				{
					PropertiesMessage propertiesMessage = list[0];
					propertiesMessage.IpAddress = this.snmpConfig.AgentIp;
					propertiesMessage.MasterMac = string.Empty;
					if (propertiesMessage.ChainNums > 0)
					{
						for (int i = 0; i < propertiesMessage.ChainNums; i++)
						{
							dictionary = this.snmpSession.Get(MibChainParser.GetSlavePropertiesRequest(i));
							if (dictionary == null || dictionary.Count < 1)
							{
								System.Collections.Generic.List<PropertiesMessage> result = null;
								return result;
							}
							PropertiesMessage slavePropertiesMessage = MibChainParser.GetSlavePropertiesMessage(dictionary, i);
							if (slavePropertiesMessage != null)
							{
								slavePropertiesMessage.IpAddress = this.snmpConfig.AgentIp;
								slavePropertiesMessage.MasterMac = propertiesMessage.MacAddress;
								slavePropertiesMessage.SlaveIndex = i;
								list.Add(slavePropertiesMessage);
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
			return list;
		}
		public ThresholdMessage GetThresholds()
		{
			return this.doGetThreshold();
		}
		public void GetThresholds(ExecutorCallBack callback)
		{
			if (callback == null)
			{
				throw new SnmpException("[GetThresholds]ExecutorCallBack can not be null.");
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.makeAsyncGetTresholds), callback);
		}
		private void makeAsyncGetTresholds(object obj)
		{
			ThresholdMessage obj2 = this.doGetThreshold();
			ExecutorCallBack executorCallBack = obj as ExecutorCallBack;
			executorCallBack(obj2);
		}
		private ThresholdMessage doGetThreshold()
		{
			ThresholdMessage thresholdMessage = null;
			try
			{
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Walk(AutoMibParser.GetThresholdsRequest(this.modelConfig));
					thresholdMessage = AutoMibParser.GetThresholdMessage(this.modelConfig, result);
				}
				else
				{
					System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Walk(MibParser.GetThresholdsRequest(this.modelConfig));
					thresholdMessage = MibParser.GetThresholdMessage(this.modelConfig, result);
				}
				thresholdMessage.ModelName = this.devModel;
				thresholdMessage.IpAddress = this.snmpConfig.AgentIp;
				thresholdMessage.CreateTime = System.DateTime.Now;
				thresholdMessage.PortNums = this.portNums;
				thresholdMessage.SensorNums = this.sensornumber;
				thresholdMessage.BankNums = this.banknumber;
				thresholdMessage.LineNums = this.linenumber;
				thresholdMessage.PerPortReading = this.perportreading;
				thresholdMessage.Switchable = this.switchable;
				thresholdMessage.PerBankReading = this.perbankreading;
				thresholdMessage.PerDoorReading = this.perdoorreading;
				thresholdMessage.DeviceMac = this.devMac;
				thresholdMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return thresholdMessage;
		}
		public System.Collections.Generic.List<ThresholdMessage> GetChainThresholds()
		{
			return this.doGetChainThreshold();
		}
		private System.Collections.Generic.List<ThresholdMessage> doGetChainThreshold()
		{
			System.Collections.Generic.List<ThresholdMessage> list = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Walk(MibChainParser.GetChainThresholdsRequest(this.modelConfig));
				list = MibChainParser.GetChainThresholdMessage(this.modelConfig, dictionary);
				if (list != null && list.Count > 0)
				{
					ThresholdMessage thresholdMessage = list[0];
					thresholdMessage.ModelName = this.devModel;
					thresholdMessage.IpAddress = this.snmpConfig.AgentIp;
					thresholdMessage.CreateTime = System.DateTime.Now;
					thresholdMessage.PortNums = this.portNums;
					thresholdMessage.SensorNums = this.sensornumber;
					thresholdMessage.BankNums = this.banknumber;
					thresholdMessage.PerPortReading = this.perportreading;
					thresholdMessage.Switchable = this.switchable;
					thresholdMessage.PerBankReading = this.perbankreading;
					thresholdMessage.PerDoorReading = this.perdoorreading;
					thresholdMessage.DeviceMac = this.devMac;
					thresholdMessage.DeviceID = this.devID;
					if (thresholdMessage.ChainNums > 0)
					{
						for (int i = 0; i < thresholdMessage.ChainNums; i++)
						{
							dictionary = this.snmpSession.Get(MibChainParser.GetSlavePropertiesRequest(i));
							if (dictionary == null || dictionary.Count < 1)
							{
								return null;
							}
							PropertiesMessage slavePropertiesMessage = MibChainParser.GetSlavePropertiesMessage(dictionary, i);
							if (slavePropertiesMessage != null)
							{
								DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(slavePropertiesMessage.ModelName, slavePropertiesMessage.FirwWareVersion);
								dictionary = this.snmpSession.Walk(MibChainParser.GetSlaveThresholdsRequest(deviceModelConfig, i));
								ThresholdMessage slaveThresholdsMessage = MibChainParser.GetSlaveThresholdsMessage(deviceModelConfig, dictionary, i);
								if (slaveThresholdsMessage != null)
								{
									slaveThresholdsMessage.IpAddress = this.snmpConfig.AgentIp;
									slaveThresholdsMessage.CreateTime = System.DateTime.Now;
									slaveThresholdsMessage.PortNums = deviceModelConfig.portNum;
									slaveThresholdsMessage.SensorNums = deviceModelConfig.sensorNum;
									slaveThresholdsMessage.BankNums = deviceModelConfig.bankNum;
									slaveThresholdsMessage.PerPortReading = deviceModelConfig.perportreading;
									slaveThresholdsMessage.Switchable = deviceModelConfig.switchable;
									slaveThresholdsMessage.PerBankReading = deviceModelConfig.perbankReading;
									slaveThresholdsMessage.DeviceReplyMac = slavePropertiesMessage.MacAddress;
									slaveThresholdsMessage.DeviceMac = slavePropertiesMessage.MacAddress;
									slaveThresholdsMessage.DeviceID = this.GetSlaveDeviceID(slavePropertiesMessage.MacAddress);
									slaveThresholdsMessage.MasterMac = this.devMac;
									slaveThresholdsMessage.MasterID = this.devID;
									slaveThresholdsMessage.SlaveIndex = i;
									list.Add(slaveThresholdsMessage);
								}
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
			return list;
		}
		public ValueMessage GetValues()
		{
			return this.doGetValues();
		}
		public void GetValues(ExecutorCallBack callback)
		{
			if (callback == null)
			{
				throw new SnmpException("[GetValues] ExecutorCallBack can not be null.");
			}
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.makeAsyncGetValues), callback);
		}
		private void makeAsyncGetValues(object obj)
		{
			ValueMessage obj2 = this.doGetValues();
			ExecutorCallBack executorCallBack = obj as ExecutorCallBack;
			executorCallBack(obj2);
		}
		private ValueMessage doGetValues()
		{
			ValueMessage valueMessage = null;
			try
			{
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					System.Collections.Generic.List<LeafVarBinding> valuesRequest = AutoMibParser.GetValuesRequest(this.modelConfig);
					System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Get(valuesRequest);
					valueMessage = AutoMibParser.GetValueMessage(this.modelConfig, result);
				}
				else
				{
					System.Collections.Generic.List<LeafVarBinding> valuesRequest2 = MibParser.GetValuesRequest(this.modelConfig);
					System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Get(valuesRequest2);
					valueMessage = MibParser.GetValueMessage(this.modelConfig, result);
				}
				valueMessage.IpAddress = this.snmpConfig.AgentIp;
				valueMessage.CreateTime = System.DateTime.Now;
				valueMessage.PortNums = this.portNums;
				valueMessage.SensorNums = this.sensornumber;
				valueMessage.BankNums = this.banknumber;
				valueMessage.LineNums = this.linenumber;
				valueMessage.PerPortReading = this.perportreading;
				valueMessage.Switchable = this.switchable;
				valueMessage.PerBankReading = this.perbankreading;
				valueMessage.PerDoorReading = this.perdoorreading;
				valueMessage.DeviceMac = this.devMac;
				valueMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return valueMessage;
		}
		public System.Collections.Generic.List<ValueMessage> GetChainValues()
		{
			return this.doGetChainValues();
		}
		private System.Collections.Generic.List<ValueMessage> doGetChainValues()
		{
			System.Collections.Generic.List<ValueMessage> list = null;
			try
			{
				System.Collections.Generic.List<LeafVarBinding> chainValuesRequest = MibChainParser.GetChainValuesRequest(this.modelConfig);
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(chainValuesRequest);
				list = MibChainParser.GetChainValueMessage(this.modelConfig, dictionary);
				if (list != null && list.Count > 0)
				{
					ValueMessage valueMessage = list[0];
					valueMessage.IpAddress = this.snmpConfig.AgentIp;
					valueMessage.CreateTime = System.DateTime.Now;
					valueMessage.PortNums = this.portNums;
					valueMessage.SensorNums = this.sensornumber;
					valueMessage.BankNums = this.banknumber;
					valueMessage.PerPortReading = this.perportreading;
					valueMessage.Switchable = this.switchable;
					valueMessage.PerBankReading = this.perbankreading;
					valueMessage.PerDoorReading = this.perdoorreading;
					valueMessage.DeviceMac = this.devMac;
					valueMessage.DeviceID = this.devID;
					valueMessage.MasterMac = string.Empty;
					if (valueMessage.ChainNums > 0)
					{
						for (int i = 0; i < valueMessage.ChainNums; i++)
						{
							dictionary = this.snmpSession.Get(MibChainParser.GetSlavePropertiesRequest(i));
							if (dictionary == null || dictionary.Count < 1)
							{
								System.Collections.Generic.List<ValueMessage> result = null;
								return result;
							}
							PropertiesMessage slavePropertiesMessage = MibChainParser.GetSlavePropertiesMessage(dictionary, i);
							if (slavePropertiesMessage != null)
							{
								DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(slavePropertiesMessage.ModelName, slavePropertiesMessage.FirwWareVersion);
								dictionary = this.snmpSession.Get(MibChainParser.GetSlaveValuesRequest(deviceModelConfig, i));
								if (dictionary == null || dictionary.Count < 1)
								{
									System.Collections.Generic.List<ValueMessage> result = null;
									return result;
								}
								ValueMessage slaveValuesMessage = MibChainParser.GetSlaveValuesMessage(deviceModelConfig, dictionary, i);
								if (slaveValuesMessage != null)
								{
									slaveValuesMessage.IpAddress = this.snmpConfig.AgentIp;
									slaveValuesMessage.CreateTime = System.DateTime.Now;
									slaveValuesMessage.PortNums = deviceModelConfig.portNum;
									slaveValuesMessage.SensorNums = deviceModelConfig.sensorNum;
									slaveValuesMessage.BankNums = deviceModelConfig.bankNum;
									slaveValuesMessage.PerPortReading = deviceModelConfig.perportreading;
									slaveValuesMessage.Switchable = deviceModelConfig.switchable;
									slaveValuesMessage.PerBankReading = deviceModelConfig.perbankReading;
									slaveValuesMessage.DeviceReplyMac = slavePropertiesMessage.MacAddress;
									slaveValuesMessage.DeviceMac = slavePropertiesMessage.MacAddress;
									slaveValuesMessage.DeviceID = this.GetSlaveDeviceID(slavePropertiesMessage.MacAddress);
									slaveValuesMessage.MasterMac = this.devMac;
									slaveValuesMessage.MasterID = this.devID;
									slaveValuesMessage.SlaveIndex = i;
									list.Add(slaveValuesMessage);
								}
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
			}
			return list;
		}
		private int GetSlaveDeviceID(string mac)
		{
			return 0;
		}
		public bool ConfigTrapReceiver(System.Collections.Generic.List<TrapReceiverConfiguration> traps)
		{
			if (traps == null || traps.Count < 1)
			{
				throw new System.ArgumentNullException("[ConfigTrapReciver] parameter is null or empty.");
			}
			try
			{
				if (this.modelConfig.commonThresholdFlag == Constant.AtenAutoDetect)
				{
					LeafVarBinding varBindings = AutoMibParser.ConfigTrapReceiverVariables(traps);
					this.snmpSession.Set(varBindings);
				}
				else
				{
					LeafVarBinding varBindings2 = MibParser.ConfigTrapReceiverVariables(traps);
					this.snmpSession.Set(varBindings2);
				}
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool ConfigTrapReceiver(TrapReceiverConfiguration traps)
		{
			return this.ConfigTrapReceiver(new System.Collections.Generic.List<TrapReceiverConfiguration>
			{
				traps
			});
		}
		public TrapReceiverConfiguration GetTrapReceiverConfig(int index)
		{
			if (index < 1 || index > Constant.TrapReceiverNumber)
			{
				throw new System.ArgumentNullException("[GetTrapReceiver] Invalid parameter.");
			}
			TrapReceiverConfiguration result = null;
			try
			{
				LeafVarBinding trapReceiverRequest = MibParser.GetTrapReceiverRequest(index);
				System.Collections.Generic.Dictionary<string, string> result2 = this.snmpSession.Get(trapReceiverRequest);
				result = MibParser.GetTrapReceiverMessage(result2);
			}
			catch (System.Exception)
			{
			}
			return result;
		}
		public bool UpdateDeviceName(string devname)
		{
			string value = string.Empty;
			if (string.IsNullOrEmpty(devname))
			{
				value = "/empty";
			}
			else
			{
				value = devname;
			}
			bool result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
				{
					leafVarBinding.Add(EatonPDUBaseMib_M2.DeviceName, value);
				}
				else
				{
					if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						leafVarBinding.Add(ApcPDUBaseMib.DeviceName, value);
					}
					else
					{
						leafVarBinding.Add(DeviceBaseMib.DeviceName, value);
					}
				}
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Set(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public bool UpdateDeviceName_Slave(string devname, int index)
		{
			string value = string.Empty;
			if (string.IsNullOrEmpty(devname))
			{
				value = "/empty";
			}
			else
			{
				value = devname;
			}
			bool result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				SlaveBaseMib slaveBaseMib = new SlaveBaseMib(index);
				leafVarBinding.Add(slaveBaseMib.SlaveDeviceName, value);
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Set(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public System.Collections.Generic.Dictionary<int, OutletStatus> GetDeviceOutletsStatus(System.Collections.Generic.List<int> outlets)
		{
			if (outlets == null || outlets.Count <= 0)
			{
				throw new SnmpException("The outlet set is empty.");
			}
			System.Collections.Generic.Dictionary<int, OutletStatus> result2;
			try
			{
				System.Collections.Generic.List<LeafVarBinding> deviceStatusRequest = MibParser.GetDeviceStatusRequest(outlets);
				System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Get(deviceStatusRequest);
				System.Collections.Generic.Dictionary<int, OutletStatus> deviceStatusValue = MibParser.GetDeviceStatusValue(result);
				result2 = deviceStatusValue;
			}
			catch (System.Exception)
			{
				result2 = null;
			}
			return result2;
		}
		public OutletStatus GetOutletStatus(int outletNumber)
		{
			if (outletNumber < 1 || outletNumber > this.portNums)
			{
				throw new SnmpException("The outletNumber should be between 1 and " + this.portNums);
			}
			OutletStatus result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(OutletStatusConfigMib.OutletStatus(outletNumber));
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Values.GetEnumerator();
				enumerator.MoveNext();
				result = (OutletStatus)System.Convert.ToInt32(enumerator.Current);
			}
			catch (System.Exception)
			{
				result = (OutletStatus)0;
			}
			return result;
		}
		private bool changeOutletStatus(int outletNumber, OutletControl outletControl)
		{
			if (outletNumber < 1 || outletNumber > this.portNums)
			{
				throw new SnmpException("The outletNumber should be between 1 and " + this.portNums);
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				string variable = OutletStatusConfigMib.OutletStatus(outletNumber);
				leafVarBinding.Add(variable, (int)outletControl);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		private bool changeGroupOutletStatus(System.Collections.Generic.List<int> outlets, OutletControl outletControl)
		{
			if (outlets == null || outlets.Count < 1)
			{
				throw new SnmpException("Warning, the group outlets is empty!");
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				foreach (int current in outlets)
				{
					string variable = OutletStatusConfigMib.OutletStatus(current);
					leafVarBinding.Add(variable, (int)outletControl);
				}
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool TurnOnOutlet(int outletNumber)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeOutletStatus(outletNumber, OutletControl.ON);
		}
		public bool TurnOffOutlet(int outletNumber)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeOutletStatus(outletNumber, OutletControl.OFF);
		}
		public bool RebootOutlet(int outletNumber)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeOutletStatus(outletNumber, OutletControl.Reboot);
		}
		public bool TurnOnGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeGroupOutletStatus(outlets, OutletControl.ON);
		}
		public bool TurnOffGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeGroupOutletStatus(outlets, OutletControl.OFF);
		}
		public bool RebootGroupOutlets(System.Collections.Generic.List<int> outlets)
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			return this.changeGroupOutletStatus(outlets, OutletControl.Reboot);
		}
		public bool TurnOffOutlets()
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(PduControlMib.OutletControl, 1);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool TurnOnOutlets()
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(PduControlMib.OutletControl, 2);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool RebootOutlets()
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(PduControlMib.OutletReboot, 2);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool RebootDevice()
		{
			if (this.switchable != 2)
			{
				throw new UnSupportException("The current model doesn't surpport outlet control.");
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(PduControlMib.PduReboot, 2);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public bool UpdateRackName(string rackName)
		{
			string value = string.Empty;
			if (string.IsNullOrEmpty(rackName))
			{
				value = "/empty";
			}
			else
			{
				value = rackName;
			}
			bool result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				if (this.modelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
				{
					result = true;
				}
				else
				{
					if (this.modelConfig.commonThresholdFlag == Constant.APC_PDU)
					{
						result = true;
					}
					else
					{
						leafVarBinding.Add(DashboardMib.DashboradRackname, value);
						System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Set(leafVarBinding);
						if (dictionary == null || dictionary.Count < 1)
						{
							result = false;
						}
						else
						{
							result = true;
						}
					}
				}
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public bool TurnOffBank(int bankNumber)
		{
			return this.changeBankStatus(bankNumber, BankStatus.OFF);
		}
		public bool TurnOnBank(int bankNumber)
		{
			return this.changeBankStatus(bankNumber, BankStatus.ON);
		}
		public bool RebootBank(int bankNumber)
		{
			return this.changeBankStatus(bankNumber, BankStatus.Reboot);
		}
		private bool changeBankStatus(int bankNumber, BankStatus bankStatus)
		{
			if (bankNumber < 1 || bankNumber > this.banknumber)
			{
				throw new SnmpException("The bankNumber should be between 1 and " + this.banknumber);
			}
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				string variable = BankControlMib.BankStatus(bankNumber);
				leafVarBinding.Add(variable, (int)bankStatus);
				this.snmpSession.Set(leafVarBinding);
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
		public DevServiceInfo GetDevServiceInfo(string myMac)
		{
			DevServiceInfo devServiceInfo = new DevServiceInfo(myMac);
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(ServicePorts.HttpPort);
			leafVarBinding.Add(DeviceBaseMib.FWversion);
			try
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					DevServiceInfo result = devServiceInfo;
					return result;
				}
				System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					string text = dictionary[current];
					if (current.StartsWith(ServicePorts.HttpPort))
					{
						devServiceInfo.httpPort = System.Convert.ToInt32(text);
					}
					if (current.StartsWith(DeviceBaseMib.FWversion))
					{
						devServiceInfo.fwVersion = text;
					}
				}
			}
			catch (System.Exception)
			{
				DevServiceInfo result = devServiceInfo;
				return result;
			}
			return devServiceInfo;
		}
		public bool UpdateDeviceVoltage(float voltage)
		{
			bool result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(DeviceValueMib.Voltage, voltage.ToString("F2"));
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Set(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		public int GetEatonPDUBankNumber()
		{
			int num = 0;
			int result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(EatonPDUCurrentMib.CT1);
				leafVarBinding.Add(EatonPDUCurrentMib.CT2);
				leafVarBinding.Add(EatonPDUCurrentMib.CT3);
				leafVarBinding.Add(EatonPDUCurrentMib.CT4);
				leafVarBinding.Add(EatonPDUCurrentMib.CT5);
				leafVarBinding.Add(EatonPDUCurrentMib.CT6);
				leafVarBinding.Add(EatonPDUCurrentMib.CT7);
				leafVarBinding.Add(EatonPDUCurrentMib.CT8);
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = 0;
				}
				else
				{
					System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string text = dictionary[current];
						if ("\0".Equals(text))
						{
							text = System.Convert.ToString(-1000);
						}
						else
						{
							if (string.IsNullOrEmpty(text))
							{
								text = System.Convert.ToString(-500);
							}
						}
						int num2 = int.Parse(text);
						if (num2 >= 0)
						{
							num++;
						}
					}
					result = num;
				}
			}
			catch (System.Exception)
			{
				result = 0;
			}
			return result;
		}
		public DevRealConfig GetEatonPDUNumber_M2()
		{
			DevRealConfig devRealConfig = default(DevRealConfig);
			devRealConfig.init();
			DevRealConfig result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(EatonPDUUnitMib_M2.OutletCount);
				leafVarBinding.Add(EatonPDUUnitMib_M2.GroupCount);
				leafVarBinding.Add(EatonPDUUnitMib_M2.ContactCount);
				leafVarBinding.Add(EatonPDUUnitMib_M2.TemperatureCount);
				leafVarBinding.Add(EatonPDUUnitMib_M2.HumidityCount);
				leafVarBinding.Add(EatonPDUUnitMib_M2.InputCount);
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = devRealConfig;
				}
				else
				{
					System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string value = dictionary[current];
						if ("\0".Equals(value))
						{
							value = System.Convert.ToString(-1000);
						}
						else
						{
							if (string.IsNullOrEmpty(value))
							{
								value = System.Convert.ToString(-500);
							}
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.OutletCount))
						{
							devRealConfig.portNum = System.Convert.ToInt32(value);
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.GroupCount))
						{
							devRealConfig.bankNum = System.Convert.ToInt32(value);
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.ContactCount))
						{
							devRealConfig.contactNum = System.Convert.ToInt32(value);
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.TemperatureCount))
						{
							devRealConfig.temperatureNum = System.Convert.ToInt32(value);
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.HumidityCount))
						{
							devRealConfig.humidityNum = System.Convert.ToInt32(value);
						}
						if (current.StartsWith(EatonPDUUnitMib_M2.InputCount))
						{
							devRealConfig.inputNum = System.Convert.ToInt32(value);
						}
					}
					result = devRealConfig;
				}
			}
			catch (System.Exception)
			{
				result = devRealConfig;
			}
			return result;
		}
		public ThresholdMessage GetThresholdsEatonPDU_M2()
		{
			return this.doGetThresholdEatonPDU_M2();
		}
		private ThresholdMessage doGetThresholdEatonPDU_M2()
		{
			ThresholdMessage thresholdMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Walk(EatonMibParser.GetThresholdsRequestEatonPDU_M2(this.modelConfig, this.realConfig));
				thresholdMessage = EatonMibParser.GetThresholdMessageEatonPDU_M2(this.modelConfig, result);
				thresholdMessage.ModelName = this.devModel;
				thresholdMessage.IpAddress = this.snmpConfig.AgentIp;
				thresholdMessage.CreateTime = System.DateTime.Now;
				thresholdMessage.PortNums = this.portNums;
				thresholdMessage.SensorNums = this.sensornumber;
				thresholdMessage.BankNums = this.banknumber;
				thresholdMessage.PerPortReading = this.perportreading;
				thresholdMessage.Switchable = this.switchable;
				thresholdMessage.PerBankReading = this.perbankreading;
				thresholdMessage.PerDoorReading = this.perdoorreading;
				thresholdMessage.DeviceMac = this.devMac;
				thresholdMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return thresholdMessage;
		}
		public ValueMessage GetValuesEatonPDU_M2()
		{
			return this.doGetValuesEatonPDU_M2();
		}
		private ValueMessage doGetValuesEatonPDU_M2()
		{
			ValueMessage valueMessage = null;
			try
			{
				System.Collections.Generic.List<LeafVarBinding> valuesRequestEatonPDU_M = EatonMibParser.GetValuesRequestEatonPDU_M2(this.modelConfig, this.realConfig);
				System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Get(valuesRequestEatonPDU_M);
				valueMessage = EatonMibParser.GetValueMessageEatonPDU_M2(this.modelConfig, result);
				valueMessage.IpAddress = this.snmpConfig.AgentIp;
				valueMessage.CreateTime = System.DateTime.Now;
				valueMessage.PortNums = this.portNums;
				valueMessage.SensorNums = this.sensornumber;
				valueMessage.BankNums = this.banknumber;
				valueMessage.PerPortReading = this.perportreading;
				valueMessage.Switchable = this.switchable;
				valueMessage.PerBankReading = this.perbankreading;
				valueMessage.PerDoorReading = this.perdoorreading;
				valueMessage.DeviceMac = this.devMac;
				valueMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return valueMessage;
		}
		public ThresholdMessage GetThresholdsApcPDU()
		{
			return this.doGetThresholdApcPDU();
		}
		private ThresholdMessage doGetThresholdApcPDU()
		{
			int apcPDUSensorNumber = this.GetApcPDUSensorNumber();
			ThresholdMessage thresholdMessage = null;
			try
			{
				System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Walk(ApcMibParser.GetThresholdsRequestApcPDU(this.modelConfig, this.realConfig, apcPDUSensorNumber));
				thresholdMessage = ApcMibParser.GetThresholdMessageApcPDU(this.modelConfig, result);
				thresholdMessage.ModelName = this.devModel;
				thresholdMessage.IpAddress = this.snmpConfig.AgentIp;
				thresholdMessage.CreateTime = System.DateTime.Now;
				thresholdMessage.PortNums = this.portNums;
				thresholdMessage.SensorNums = this.sensornumber;
				thresholdMessage.BankNums = this.banknumber;
				thresholdMessage.PerPortReading = this.perportreading;
				thresholdMessage.Switchable = this.switchable;
				thresholdMessage.PerBankReading = this.perbankreading;
				thresholdMessage.PerDoorReading = this.perdoorreading;
				thresholdMessage.DeviceMac = this.devMac;
				thresholdMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return thresholdMessage;
		}
		public ValueMessage GetValuesApcPDU()
		{
			return this.doGetValuesApcPDU();
		}
		private ValueMessage doGetValuesApcPDU()
		{
			int apcPDUSensorNumber = this.GetApcPDUSensorNumber();
			ValueMessage valueMessage = null;
			try
			{
				System.Collections.Generic.List<LeafVarBinding> valuesRequestApcPDU = ApcMibParser.GetValuesRequestApcPDU(this.modelConfig, this.realConfig, apcPDUSensorNumber);
				System.Collections.Generic.Dictionary<string, string> result = this.snmpSession.Get(valuesRequestApcPDU);
				valueMessage = ApcMibParser.GetValueMessageApcPDU(this.modelConfig, result);
				valueMessage.IpAddress = this.snmpConfig.AgentIp;
				valueMessage.CreateTime = System.DateTime.Now;
				valueMessage.PortNums = this.portNums;
				valueMessage.SensorNums = this.sensornumber;
				valueMessage.BankNums = this.banknumber;
				valueMessage.PerPortReading = this.perportreading;
				valueMessage.Switchable = this.switchable;
				valueMessage.PerBankReading = this.perbankreading;
				valueMessage.PerDoorReading = this.perdoorreading;
				valueMessage.DeviceMac = this.devMac;
				valueMessage.DeviceID = this.devID;
			}
			catch (System.Exception)
			{
			}
			return valueMessage;
		}
		private int GetApcPDUSensorNumber()
		{
			int num = 0;
			int result;
			try
			{
				LeafVarBinding leafVarBinding = new LeafVarBinding();
				leafVarBinding.Add(ApcPDUBaseMib.SensorTableSize);
				System.Collections.Generic.Dictionary<string, string> dictionary = this.snmpSession.Get(leafVarBinding);
				if (dictionary == null || dictionary.Count < 1)
				{
					result = 0;
				}
				else
				{
					System.Collections.Generic.IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string text = dictionary[current];
						if (string.IsNullOrEmpty(text) || "\0".Equals(text) || text.Equals("Null"))
						{
							num = 0;
						}
						else
						{
							num = System.Convert.ToInt32(text);
						}
					}
					result = num;
				}
			}
			catch (System.Exception)
			{
				result = 0;
			}
			return result;
		}
	}
}
