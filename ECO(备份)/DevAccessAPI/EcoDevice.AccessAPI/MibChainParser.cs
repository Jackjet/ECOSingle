using DBAccessAPI;
using System;
using System.Collections.Generic;
using System.Net;
namespace EcoDevice.AccessAPI
{
	internal class MibChainParser
	{
		public static LeafVarBinding GetChainPropertiesRequest()
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.DeviceName);
			leafVarBinding.Add(DeviceBaseMib.FWversion);
			leafVarBinding.Add(DeviceBaseMib.Mac);
			leafVarBinding.Add(DeviceBaseMib.ModelName);
			leafVarBinding.Add(DashboardMib.DashboradRackname);
			leafVarBinding.Add(DeviceBaseMib.ChainNumber);
			return leafVarBinding;
		}
		public static System.Collections.Generic.List<PropertiesMessage> GetChainPropertiesMessage(System.Collections.Generic.Dictionary<string, string> variables)
		{
			if (variables == null || variables.Count < 1)
			{
				return null;
			}
			System.Collections.Generic.List<PropertiesMessage> list = new System.Collections.Generic.List<PropertiesMessage>();
			PropertiesMessage propertiesMessage = new PropertiesMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = variables.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = variables[current];
				if ("\0".Equals(text) || string.IsNullOrEmpty(text) || "Null".Equals(text))
				{
					text = string.Empty;
				}
				if (current.StartsWith(DeviceBaseMib.DeviceName))
				{
					propertiesMessage.DeviceName = text;
				}
				else
				{
					if (current.StartsWith(DeviceBaseMib.FWversion))
					{
						propertiesMessage.FirwWareVersion = text;
					}
					else
					{
						if (current.StartsWith(DeviceBaseMib.Mac))
						{
							propertiesMessage.MacAddress = text.Replace(" ", ":").Replace("-", ":");
						}
						else
						{
							if (current.StartsWith(DeviceBaseMib.ModelName))
							{
								propertiesMessage.ModelName = text;
							}
							else
							{
								if (current.StartsWith(DashboardMib.DashboradRackname))
								{
									propertiesMessage.DashboardRackname = text;
								}
								else
								{
									if (!current.StartsWith(DeviceBaseMib.ChainNumber))
									{
										return null;
									}
									propertiesMessage.ChainNums = (string.IsNullOrEmpty(text) ? 0 : System.Convert.ToInt32(text));
								}
							}
						}
					}
				}
			}
			propertiesMessage.CreateTime = System.DateTime.Now;
			list.Add(propertiesMessage);
			return list;
		}
		public static LeafVarBinding GetSlavePropertiesRequest(int index)
		{
			SlaveBaseMib slaveBaseMib = new SlaveBaseMib(index);
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(slaveBaseMib.SlaveDeviceName);
			leafVarBinding.Add(slaveBaseMib.SlaveDeviceMac);
			leafVarBinding.Add(slaveBaseMib.SlaveModelName);
			return leafVarBinding;
		}
		public static PropertiesMessage GetSlavePropertiesMessage(System.Collections.Generic.Dictionary<string, string> variables, int index)
		{
			if (variables == null || variables.Count < 1)
			{
				return null;
			}
			SlaveBaseMib slaveBaseMib = new SlaveBaseMib(index);
			PropertiesMessage propertiesMessage = new PropertiesMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = variables.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = variables[current];
				if ("\0".Equals(text) || string.IsNullOrEmpty(text) || "Null".Equals(text))
				{
					text = string.Empty;
				}
				if (current.StartsWith(slaveBaseMib.SlaveDeviceName))
				{
					propertiesMessage.DeviceName = text;
				}
				else
				{
					if (current.StartsWith(slaveBaseMib.SlaveDeviceMac))
					{
						propertiesMessage.MacAddress = text;
					}
					else
					{
						if (!current.StartsWith(slaveBaseMib.SlaveModelName))
						{
							return null;
						}
						propertiesMessage.ModelName = text;
					}
				}
			}
			propertiesMessage.FirwWareVersion = "";
			propertiesMessage.DashboardRackname = "";
			propertiesMessage.CreateTime = System.DateTime.Now;
			return propertiesMessage;
		}
		public static LeafVarBinding SetSlaveDeviceThresholdVariables(DeviceThreshold threshold, DevModelConfig modelcfg, int index)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveDeviceEnvironmentMib slaveDeviceEnvironmentMib = new SlaveDeviceEnvironmentMib(index);
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
			}
			if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold)
			{
				if (threshold.MaxPowerDissMT != -500f)
				{
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerDissMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
				}
				if (threshold.MaxPowerMT != -500f)
				{
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
				}
				if (threshold.MaxVoltageMT != -500f)
				{
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
				}
				if (threshold.MinVoltageMT != -500f)
				{
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMinVoltageMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
				}
			}
			else
			{
				if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					if (threshold.MaxPowerDissMT != -500f)
					{
						leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerDissMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
					}
					if (threshold.MaxPowerMT != -500f)
					{
						leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
					}
				}
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetSlaveSensorThresholdVariables(SensorThreshold threshold, DevModelConfig modelcfg, int index)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveSensorEnvironmentMib slaveSensorEnvironmentMib = new SlaveSensorEnvironmentMib(index, threshold.SensorNumber);
			if (threshold.MaxHumidityMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxHumidityMt, System.Convert.ToInt32(threshold.MaxHumidityMT * 10f));
			}
			if (threshold.MaxPressMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxPressMt, System.Convert.ToInt32(threshold.MaxPressMT * 10f));
			}
			if (threshold.MaxTemperatureMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxTemperatureMt, System.Convert.ToInt32(threshold.MaxTemperatureMT * 10f));
			}
			if (threshold.MinHumidityMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinHumidityMt, System.Convert.ToInt32(threshold.MinHumidityMT * 10f));
			}
			if (threshold.MinPressMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinPressMt, System.Convert.ToInt32(threshold.MinPressMT * 10f));
			}
			if (threshold.MinTemperatureMT != -500f)
			{
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinTemperatureMt, System.Convert.ToInt32(threshold.MinTemperatureMT * 10f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetSlaveOutletThresholdVariables(OutletThreshold threshold, DevModelConfig modelcfg, int index)
		{
			int switchable = modelcfg.switchable;
			int perportreading = modelcfg.perportreading;
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveOutletEnvironmentMib slaveOutletEnvironmentMib = new SlaveOutletEnvironmentMib(index, threshold.OutletNumber);
			if (!string.IsNullOrEmpty(threshold.OutletName))
			{
				leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveOutletName, threshold.OutletName);
			}
			else
			{
				leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveOutletName, "/empty");
			}
			if (switchable == 2 && modelcfg.isOutletSwitchable(threshold.OutletNumber - 1))
			{
				if (!string.IsNullOrEmpty(threshold.MacAddress))
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMacAddress, threshold.MacAddress);
				}
				if ((float)threshold.OffDelayTime != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveOffDelayTime, threshold.OffDelayTime);
				}
				if ((float)threshold.OnDelayTime != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveOnDelayTime, threshold.OnDelayTime);
				}
				leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveConfirmation, System.Convert.ToInt32(threshold.Confirmation));
				leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveShutdownMethod, System.Convert.ToInt32(threshold.ShutdownMethod));
			}
			if (perportreading == 2)
			{
				if (threshold.MaxCurrentMT != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
				}
				if (threshold.MaxPowerDissMT != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMaxPowerDissMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
				}
				if (threshold.MaxPowerMT != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMaxPowerMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
				}
				if (threshold.MaxVoltageMT != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
				}
				if (threshold.MinVoltageMT != -500f)
				{
					leafVarBinding.Add(slaveOutletEnvironmentMib.SlaveMinVoltageMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
				}
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetSlaveBankThresholdVariables(BankThreshold threshold, DevModelConfig modelcfg, int index)
		{
			int perbankReading = modelcfg.perbankReading;
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveBankEnvironmentMib slaveBankEnvironmentMib = new SlaveBankEnvironmentMib(index, threshold.BankNumber);
			if (!string.IsNullOrEmpty(threshold.BankName))
			{
				leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankName, threshold.BankName);
			}
			else
			{
				leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankName, "/empty");
			}
			if (perbankReading == 2)
			{
				if (threshold.MaxCurrentMT != -500f)
				{
					leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankMaxCurMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
				}
				if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold || modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					if (threshold.MaxVoltageMT != -500f)
					{
						leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankMaxVolMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
					}
					if (threshold.MaxPowerMT != -500f)
					{
						leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankMaxPMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
					}
					if (threshold.MaxPowerDissMT != -500f)
					{
						leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankMaxPDMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
					}
					if (threshold.MinVoltageMT != -500f)
					{
						leafVarBinding.Add(slaveBankEnvironmentMib.SlaveBankMinVolMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
					}
				}
			}
			return leafVarBinding;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetChainValuesRequest(DevModelConfig modelcfg)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum2 = modelcfg.bankNum;
			int switchable = modelcfg.switchable;
			int perportreading = modelcfg.perportreading;
			int perbankReading = modelcfg.perbankReading;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.Mac);
			leafVarBinding.Add(DeviceBaseMib.ChainNumber);
			leafVarBinding.Add(DeviceValueMib.Current);
			if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold)
			{
				leafVarBinding.Add(DeviceValueMib.Power);
				leafVarBinding.Add(DeviceValueMib.PowerDissipation);
				leafVarBinding.Add(DeviceValueMib.Voltage);
			}
			else
			{
				if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding.Add(DeviceValueMib.Power);
					leafVarBinding.Add(DeviceValueMib.PowerDissipation);
				}
			}
			for (int i = 1; i <= sensorNum; i++)
			{
				SensorValueMib sensorValueMib = new SensorValueMib(i);
				leafVarBinding.Add(sensorValueMib.Humidity);
				leafVarBinding.Add(sensorValueMib.Pressure);
				leafVarBinding.Add(sensorValueMib.Temperature);
			}
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			if (portNum > 0 && (switchable == 2 || perportreading == 2))
			{
				if (switchable == 2)
				{
					for (int j = 1; j <= portNum; j++)
					{
						OutletStatusValueMib outletStatusValueMib = new OutletStatusValueMib(j);
						leafVarBinding2.Add(outletStatusValueMib.OutletStatus);
					}
				}
				if (perportreading == 2)
				{
					LeafVBBuilder leafVBBuilder = new LeafVBBuilder(4, portNum);
					leafVBBuilder.BuildVbByIndex(list, delegate(int outletNum, LeafVarBinding leafVb)
					{
						OutletValueMib outletValueMib = new OutletValueMib(outletNum);
						leafVb.Add(outletValueMib.Current);
						leafVb.Add(outletValueMib.Power);
						leafVb.Add(outletValueMib.PowerDissipation);
						leafVb.Add(outletValueMib.Voltage);
					});
				}
			}
			if (bankNum2 > 0 && perbankReading == 2)
			{
				if (modelcfg.commonThresholdFlag == Constant.WithoutCommonThreshold)
				{
					for (int k = 1; k <= bankNum2; k++)
					{
						BankStatusValueMib bankStatusValueMib = new BankStatusValueMib(k);
						leafVarBinding2.Add(bankStatusValueMib.BankStatus);
					}
					LeafVBBuilder leafVBBuilder2 = new LeafVBBuilder(1, bankNum2);
					leafVBBuilder2.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
					{
						BankValueMib bankValueMib = new BankValueMib(bankNum);
						leafVb.Add(bankValueMib.Current);
					});
				}
				else
				{
					if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
					{
						for (int l = 1; l <= bankNum2; l++)
						{
							BankStatusValueMib bankStatusValueMib2 = new BankStatusValueMib(l);
							leafVarBinding2.Add(bankStatusValueMib2.BankStatus);
						}
						LeafVBBuilder leafVBBuilder3 = new LeafVBBuilder(4, bankNum2);
						leafVBBuilder3.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
						{
							BankValueMib bankValueMib = new BankValueMib(bankNum);
							leafVb.Add(bankValueMib.Current);
							leafVb.Add(bankValueMib.Voltage);
							leafVb.Add(bankValueMib.Power);
							leafVb.Add(bankValueMib.PowerDissipation);
						});
					}
					else
					{
						LeafVBBuilder leafVBBuilder4 = new LeafVBBuilder(4, bankNum2);
						leafVBBuilder4.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
						{
							BankValueMib bankValueMib = new BankValueMib(bankNum);
							leafVb.Add(bankValueMib.Current);
							leafVb.Add(bankValueMib.Voltage);
							leafVb.Add(bankValueMib.Power);
							leafVb.Add(bankValueMib.PowerDissipation);
						});
					}
				}
			}
			if (leafVarBinding2.VarBindings.Count > 0)
			{
				list.Add(leafVarBinding2);
			}
			return list;
		}
		public static System.Collections.Generic.List<ValueMessage> GetChainValueMessage(DevModelConfig modelCfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.List<ValueMessage> list = new System.Collections.Generic.List<ValueMessage>();
			ValueMessage valueMessage = new ValueMessage();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text) || string.IsNullOrEmpty(text) || "Null".Equals(text))
				{
					text = string.Empty;
				}
				if (current.StartsWith(DeviceValueMib.Entry))
				{
					dictionary.Add(current, text);
				}
				else
				{
					if (current.StartsWith(SensorValueMib.Entry))
					{
						dictionary2.Add(current, text);
					}
					else
					{
						if (current.StartsWith(OutletValueMib.Entry) || current.StartsWith(OutletStatusValueMib.Entry))
						{
							dictionary3.Add(current, text);
						}
						else
						{
							if (current.StartsWith(BankValueMib.Entry) || current.StartsWith(BankStatusValueMib.Entry))
							{
								dictionary4.Add(current, text);
							}
							else
							{
								if (current.StartsWith(DeviceBaseMib.Mac))
								{
									valueMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
								}
								else
								{
									if (current.StartsWith(DeviceBaseMib.ChainNumber))
									{
										valueMessage.ChainNums = (string.IsNullOrEmpty(text) ? 0 : System.Convert.ToInt32(text));
									}
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				valueMessage.DeviceValue = MibChainParser.GetDeviceValue(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				valueMessage.OutletValue = MibChainParser.GetOutletValue(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				valueMessage.SensorValue = MibChainParser.GetSensorValue(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				valueMessage.BankValue = MibChainParser.GetBankValue(dictionary4);
			}
			if (modelCfg.commonThresholdFlag == Constant.WithoutCommonThreshold)
			{
				float energyValue = Sys_Para.GetEnergyValue();
				float num = float.Parse(valueMessage.DeviceValue.Current);
				float num2 = energyValue * num;
				valueMessage.DeviceValue.Voltage = energyValue.ToString("F2");
				valueMessage.DeviceValue.Power = num2.ToString("F4");
				valueMessage.DeviceValue.PowerDissipation = "N/A";
				if (valueMessage.OutletValue != null && valueMessage.OutletValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, OutletValueEntry> current2 in valueMessage.OutletValue)
					{
						OutletValueEntry value = current2.Value;
						num = float.Parse(value.Current);
						num2 = energyValue * num;
						value.Voltage = energyValue.ToString("F2");
						value.Power = num2.ToString("F4");
						value.PowerDissipation = "N/A";
					}
				}
				if (valueMessage.BankValue == null || valueMessage.BankValue.Count <= 0)
				{
					goto IL_4C1;
				}
				using (System.Collections.Generic.Dictionary<int, BankValueEntry>.Enumerator enumerator3 = valueMessage.BankValue.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						System.Collections.Generic.KeyValuePair<int, BankValueEntry> current3 = enumerator3.Current;
						BankValueEntry value2 = current3.Value;
						bool flag = float.TryParse(value2.Current, out num);
						if (flag)
						{
							num2 = energyValue * num;
						}
						else
						{
							num2 = 0f;
							num = 0f;
							value2.Current = num.ToString("F2");
						}
						value2.Voltage = energyValue.ToString("F2");
						value2.Power = num2.ToString("F4");
						value2.PowerDissipation = "N/A";
					}
					goto IL_4C1;
				}
			}
			if (modelCfg.commonThresholdFlag == Constant.WithSpecialThreshold)
			{
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				if (valueMessage.BankValue != null && valueMessage.BankValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, BankValueEntry> current4 in valueMessage.BankValue)
					{
						BankValueEntry value3 = current4.Value;
						if (!float.TryParse(value3.Current, out num4))
						{
							num4 = 0f;
							value3.Current = num4.ToString("F2");
						}
						if (!float.TryParse(value3.Voltage, out num3))
						{
							num3 = 0f;
							value3.Voltage = num3.ToString("F2");
						}
						if (!float.TryParse(value3.Power, out num5))
						{
							num5 = 0f;
							value3.Power = num5.ToString("F4");
						}
					}
				}
			}
			IL_4C1:
			list.Add(valueMessage);
			return list;
		}
		private static System.Collections.Generic.Dictionary<int, OutletValueEntry> GetOutletValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, OutletValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, OutletValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			OutletValueMib outletValueMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					OutletValueEntry value = new OutletValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					outletValueMib = new OutletValueMib(num2);
					num = num2;
				}
				OutletValueEntry outletValueEntry = dictionary[num2];
				if (text.StartsWith(outletValueMib.Current))
				{
					outletValueEntry.Current = text2;
				}
				else
				{
					if (text.StartsWith(outletValueMib.Voltage))
					{
						outletValueEntry.Voltage = text2;
					}
					else
					{
						if (text.StartsWith(outletValueMib.Power))
						{
							outletValueEntry.Power = text2;
						}
						else
						{
							if (text.StartsWith(outletValueMib.PowerDissipation))
							{
								outletValueEntry.PowerDissipation = text2;
							}
							else
							{
								outletValueEntry.OutletStatus = (OutletStatus)System.Convert.ToInt32(text2);
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, BankValueEntry> GetBankValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, BankValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			BankValueMib bankValueMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					BankValueEntry value = new BankValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					bankValueMib = new BankValueMib(num2);
					num = num2;
				}
				BankValueEntry bankValueEntry = dictionary[num2];
				if (text.StartsWith(bankValueMib.Current))
				{
					bankValueEntry.Current = text2;
				}
				else
				{
					if (text.StartsWith(bankValueMib.Voltage))
					{
						bankValueEntry.Voltage = text2;
					}
					else
					{
						if (text.StartsWith(bankValueMib.Power))
						{
							bankValueEntry.Power = text2;
						}
						else
						{
							if (text.StartsWith(bankValueMib.PowerDissipation))
							{
								bankValueEntry.PowerDissipation = text2;
							}
							else
							{
								bankValueEntry.BankStatus = (BankStatus)System.Convert.ToInt32(text2);
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorValueEntry> GetSensorValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SensorValueMib sensorValueMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					SensorValueEntry value = new SensorValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					sensorValueMib = new SensorValueMib(num2);
					num = num2;
				}
				SensorValueEntry sensorValueEntry = dictionary[num2];
				if (text.StartsWith(sensorValueMib.Humidity))
				{
					sensorValueEntry.Humidity = text2;
				}
				else
				{
					if (text.StartsWith(sensorValueMib.Pressure))
					{
						sensorValueEntry.Pressure = text2;
					}
					else
					{
						if (text.StartsWith(sensorValueMib.Temperature))
						{
							sensorValueEntry.Temperature = text2;
						}
					}
				}
			}
			return dictionary;
		}
		private static DeviceValueEntry GetDeviceValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			DeviceValueEntry deviceValueEntry = new DeviceValueEntry();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
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
				if (current.StartsWith(DeviceValueMib.Current))
				{
					deviceValueEntry.Current = text;
				}
				else
				{
					if (current.StartsWith(DeviceValueMib.Power))
					{
						deviceValueEntry.Power = text;
					}
					else
					{
						if (current.StartsWith(DeviceValueMib.PowerDissipation))
						{
							deviceValueEntry.PowerDissipation = text;
						}
						else
						{
							if (current.StartsWith(DeviceValueMib.Voltage))
							{
								deviceValueEntry.Voltage = text;
							}
						}
					}
				}
			}
			return deviceValueEntry;
		}
		internal static LeafVarBinding GetTrapReceiverRequest(int index)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			TrapReceiverMib trapReceiverMib = new TrapReceiverMib(index);
			leafVarBinding.Add(trapReceiverMib.TrapEnabled);
			leafVarBinding.Add(trapReceiverMib.TrapVersion);
			leafVarBinding.Add(trapReceiverMib.ReceiverIp);
			leafVarBinding.Add(trapReceiverMib.TrapPort);
			return leafVarBinding;
		}
		internal static TrapReceiverConfiguration GetTrapReceiverMessage(System.Collections.Generic.Dictionary<string, string> result)
		{
			if (result == null || result.Count < 1)
			{
				throw new System.ArgumentNullException("[GetTrapReceiver] result is null");
			}
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			TrapReceiverConfiguration trapReceiverConfiguration = null;
			TrapReceiverMib trapReceiverMib = null;
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				int index = System.Convert.ToInt32(current.Substring(current.Length - 1));
				if (trapReceiverConfiguration == null)
				{
					trapReceiverConfiguration = new TrapReceiverConfiguration(index);
				}
				if (trapReceiverMib == null)
				{
					trapReceiverMib = new TrapReceiverMib(index);
				}
				if (current.StartsWith(trapReceiverMib.TrapEnabled))
				{
					trapReceiverConfiguration.Enabled = (TrapEnabled)System.Convert.ToInt32(text);
				}
				else
				{
					if (current.StartsWith(trapReceiverMib.TrapVersion))
					{
						trapReceiverConfiguration.TrapVersion = System.Convert.ToInt32(text);
					}
					else
					{
						if (current.StartsWith(trapReceiverMib.ReceiverIp))
						{
							trapReceiverConfiguration.ReceiverIp = System.Net.IPAddress.Parse(text);
						}
						else
						{
							if (current.StartsWith(trapReceiverMib.TrapPort))
							{
								trapReceiverConfiguration.TrapPort = System.Convert.ToInt32(text);
							}
						}
					}
				}
			}
			return trapReceiverConfiguration;
		}
		internal static LeafVarBinding ConfigTrapReceiverVariables(System.Collections.Generic.List<TrapReceiverConfiguration> traps)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			foreach (TrapReceiverConfiguration current in traps)
			{
				TrapReceiverMib trapReceiverMib = new TrapReceiverMib(current.Index);
				leafVarBinding.Add(trapReceiverMib.TrapEnabled, (int)current.Enabled);
				if (current.ReceiverIp != null)
				{
					leafVarBinding.Add(trapReceiverMib.ReceiverIp, current.ReceiverIp);
				}
				if (current.TrapPort != -1)
				{
					leafVarBinding.Add(trapReceiverMib.TrapPort, current.TrapPort);
				}
				if (current.AgentVersion == 3)
				{
					if (current.TrapVersion == 2 && current.AuthPassword.Length > 0 && current.PrivPassword.Length > 0)
					{
						leafVarBinding.Add(trapReceiverMib.TrapVersion, current.AgentVersion);
						leafVarBinding.Add(trapReceiverMib.Username, current.Username);
						leafVarBinding.Add(trapReceiverMib.AuthPassword, current.AuthPassword);
						leafVarBinding.Add(trapReceiverMib.PrivPassword, current.PrivPassword);
					}
					else
					{
						leafVarBinding.Add(trapReceiverMib.TrapVersion, 1);
						leafVarBinding.Add(trapReceiverMib.Community, current.Community);
					}
				}
				else
				{
					leafVarBinding.Add(trapReceiverMib.TrapVersion, current.AgentVersion + 1);
					leafVarBinding.Add(trapReceiverMib.Community, current.Community);
				}
			}
			return leafVarBinding;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetDeviceStatusRequest(System.Collections.Generic.List<int> outlets)
		{
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			foreach (int current in outlets)
			{
				OutletStatusValueMib outletStatusValueMib = new OutletStatusValueMib(current);
				leafVarBinding.Add(outletStatusValueMib.OutletStatus);
			}
			list.Add(leafVarBinding);
			return list;
		}
		public static System.Collections.Generic.Dictionary<int, OutletStatus> GetDeviceStatusValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, OutletStatus> dictionary = new System.Collections.Generic.Dictionary<int, OutletStatus>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string value = result[text];
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
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int key = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, (OutletStatus)System.Convert.ToInt32(value));
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetSlaveValuesRequest(DevModelConfig modelcfg, int index)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum2 = modelcfg.bankNum;
			int switchable = modelcfg.switchable;
			int perportreading = modelcfg.perportreading;
			int perbankReading = modelcfg.perbankReading;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveDeviceValueMib slaveDeviceValueMib = new SlaveDeviceValueMib(index);
			leafVarBinding.Add(slaveDeviceValueMib.SlaveCurrent);
			if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold)
			{
				leafVarBinding.Add(slaveDeviceValueMib.SlavePower);
				leafVarBinding.Add(slaveDeviceValueMib.SlavePowerDissipation);
				leafVarBinding.Add(slaveDeviceValueMib.SlaveVoltage);
			}
			else
			{
				if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding.Add(slaveDeviceValueMib.SlavePower);
					leafVarBinding.Add(slaveDeviceValueMib.SlavePowerDissipation);
				}
			}
			for (int i = 1; i <= sensorNum; i++)
			{
				SlaveSensorValueMib slaveSensorValueMib = new SlaveSensorValueMib(index, i);
				leafVarBinding.Add(slaveSensorValueMib.SlaveHumidity);
				leafVarBinding.Add(slaveSensorValueMib.SlavePressure);
				leafVarBinding.Add(slaveSensorValueMib.SlaveTemperature);
			}
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			if (portNum > 0 && (switchable == 2 || perportreading == 2))
			{
				if (switchable == 2)
				{
					for (int j = 1; j <= portNum; j++)
					{
						SlaveOutletStatusValueMib slaveOutletStatusValueMib = new SlaveOutletStatusValueMib(index, j);
						leafVarBinding2.Add(slaveOutletStatusValueMib.SlaveOutletStatus);
					}
				}
				if (perportreading == 2)
				{
					LeafVBBuilder leafVBBuilder = new LeafVBBuilder(4, portNum);
					leafVBBuilder.BuildVbByIndex(list, delegate(int outletNum, LeafVarBinding leafVb)
					{
						SlaveOutletValueMib slaveOutletValueMib = new SlaveOutletValueMib(index, outletNum);
						leafVb.Add(slaveOutletValueMib.SlaveCurrent);
						leafVb.Add(slaveOutletValueMib.SlavePower);
						leafVb.Add(slaveOutletValueMib.SlavePowerDissipation);
						leafVb.Add(slaveOutletValueMib.SlaveVoltage);
					});
				}
			}
			if (bankNum2 > 0 && perbankReading == 2)
			{
				if (modelcfg.commonThresholdFlag == Constant.WithoutCommonThreshold)
				{
					for (int k = 1; k <= bankNum2; k++)
					{
						SlaveBankStatusValueMib slaveBankStatusValueMib = new SlaveBankStatusValueMib(index, k);
						leafVarBinding2.Add(slaveBankStatusValueMib.SlaveBankStatus);
					}
					LeafVBBuilder leafVBBuilder2 = new LeafVBBuilder(1, bankNum2);
					leafVBBuilder2.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
					{
						SlaveBankValueMib slaveBankValueMib = new SlaveBankValueMib(index, bankNum);
						leafVb.Add(slaveBankValueMib.SlaveCurrent);
					});
				}
				else
				{
					if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
					{
						for (int l = 1; l <= bankNum2; l++)
						{
							SlaveBankStatusValueMib slaveBankStatusValueMib2 = new SlaveBankStatusValueMib(index, l);
							leafVarBinding2.Add(slaveBankStatusValueMib2.SlaveBankStatus);
						}
						LeafVBBuilder leafVBBuilder3 = new LeafVBBuilder(4, bankNum2);
						leafVBBuilder3.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
						{
							SlaveBankValueMib slaveBankValueMib = new SlaveBankValueMib(index, bankNum);
							leafVb.Add(slaveBankValueMib.SlaveCurrent);
							leafVb.Add(slaveBankValueMib.SlaveVoltage);
							leafVb.Add(slaveBankValueMib.SlavePower);
							leafVb.Add(slaveBankValueMib.SlavePowerDissipation);
						});
					}
					else
					{
						LeafVBBuilder leafVBBuilder4 = new LeafVBBuilder(4, bankNum2);
						leafVBBuilder4.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
						{
							SlaveBankValueMib slaveBankValueMib = new SlaveBankValueMib(index, bankNum);
							leafVb.Add(slaveBankValueMib.SlaveCurrent);
							leafVb.Add(slaveBankValueMib.SlaveVoltage);
							leafVb.Add(slaveBankValueMib.SlavePower);
							leafVb.Add(slaveBankValueMib.SlavePowerDissipation);
						});
					}
				}
			}
			if (leafVarBinding2.VarBindings.Count > 0)
			{
				list.Add(leafVarBinding2);
			}
			return list;
		}
		public static ValueMessage GetSlaveValuesMessage(DevModelConfig modelCfg, System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			ValueMessage valueMessage = new ValueMessage();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveDeviceValueMib slaveDeviceValueMib = new SlaveDeviceValueMib(index);
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string value = result[current];
				if ("\0".Equals(value) || string.IsNullOrEmpty(value) || "Null".Equals(value))
				{
					value = string.Empty;
				}
				if (current.StartsWith(slaveDeviceValueMib.DeviceValueEntry))
				{
					dictionary.Add(current, value);
				}
				else
				{
					if (current.StartsWith(slaveDeviceValueMib.SensorValueEntry))
					{
						dictionary2.Add(current, value);
					}
					else
					{
						if (current.StartsWith(slaveDeviceValueMib.OutletValueEntry) || current.StartsWith(slaveDeviceValueMib.OutletStatusValueEntry))
						{
							dictionary3.Add(current, value);
						}
						else
						{
							if (current.StartsWith(slaveDeviceValueMib.BankValueEntry) || current.StartsWith(slaveDeviceValueMib.BankStatusValueEntry))
							{
								dictionary4.Add(current, value);
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				valueMessage.DeviceValue = MibChainParser.GetSlaveDeviceValue(dictionary, index);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				valueMessage.OutletValue = MibChainParser.GetSlaveOutletValue(dictionary3, index);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				valueMessage.SensorValue = MibChainParser.GetSlaveSensorValue(dictionary2, index);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				valueMessage.BankValue = MibChainParser.GetSlaveBankValue(dictionary4, index);
			}
			if (modelCfg.commonThresholdFlag == Constant.WithoutCommonThreshold)
			{
				float energyValue = Sys_Para.GetEnergyValue();
				float num = float.Parse(valueMessage.DeviceValue.Current);
				float num2 = energyValue * num;
				valueMessage.DeviceValue.Voltage = energyValue.ToString("F2");
				valueMessage.DeviceValue.Power = num2.ToString("F4");
				valueMessage.DeviceValue.PowerDissipation = "N/A";
				if (valueMessage.OutletValue != null && valueMessage.OutletValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, OutletValueEntry> current2 in valueMessage.OutletValue)
					{
						OutletValueEntry value2 = current2.Value;
						num = float.Parse(value2.Current);
						num2 = energyValue * num;
						value2.Voltage = energyValue.ToString("F2");
						value2.Power = num2.ToString("F4");
						value2.PowerDissipation = "N/A";
					}
				}
				if (valueMessage.BankValue == null || valueMessage.BankValue.Count <= 0)
				{
					return valueMessage;
				}
				using (System.Collections.Generic.Dictionary<int, BankValueEntry>.Enumerator enumerator3 = valueMessage.BankValue.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						System.Collections.Generic.KeyValuePair<int, BankValueEntry> current3 = enumerator3.Current;
						BankValueEntry value3 = current3.Value;
						bool flag = float.TryParse(value3.Current, out num);
						if (flag)
						{
							num2 = energyValue * num;
						}
						else
						{
							num2 = 0f;
							num = 0f;
							value3.Current = num.ToString("F2");
						}
						value3.Voltage = energyValue.ToString("F2");
						value3.Power = num2.ToString("F4");
						value3.PowerDissipation = "N/A";
					}
					return valueMessage;
				}
			}
			if (modelCfg.commonThresholdFlag == Constant.WithSpecialThreshold)
			{
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				if (valueMessage.BankValue != null && valueMessage.BankValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, BankValueEntry> current4 in valueMessage.BankValue)
					{
						BankValueEntry value4 = current4.Value;
						if (!float.TryParse(value4.Current, out num4))
						{
							num4 = 0f;
							value4.Current = num4.ToString("F2");
						}
						if (!float.TryParse(value4.Voltage, out num3))
						{
							num3 = 0f;
							value4.Voltage = num3.ToString("F2");
						}
						if (!float.TryParse(value4.Power, out num5))
						{
							num5 = 0f;
							value4.Power = num5.ToString("F4");
						}
					}
				}
			}
			return valueMessage;
		}
		private static DeviceValueEntry GetSlaveDeviceValue(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			DeviceValueEntry deviceValueEntry = new DeviceValueEntry();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveDeviceValueMib slaveDeviceValueMib = new SlaveDeviceValueMib(index);
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
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
				if (current.StartsWith(slaveDeviceValueMib.SlaveCurrent))
				{
					deviceValueEntry.Current = text;
				}
				else
				{
					if (current.StartsWith(slaveDeviceValueMib.SlavePower))
					{
						deviceValueEntry.Power = text;
					}
					else
					{
						if (current.StartsWith(slaveDeviceValueMib.SlavePowerDissipation))
						{
							deviceValueEntry.PowerDissipation = text;
						}
						else
						{
							if (current.StartsWith(slaveDeviceValueMib.SlaveVoltage))
							{
								deviceValueEntry.Voltage = text;
							}
						}
					}
				}
			}
			return deviceValueEntry;
		}
		private static System.Collections.Generic.Dictionary<int, OutletValueEntry> GetSlaveOutletValue(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, OutletValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, OutletValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveOutletValueMib slaveOutletValueMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					OutletValueEntry value = new OutletValueEntry(num);
					dictionary.Add(num, value);
					slaveOutletValueMib = new SlaveOutletValueMib(index, num);
				}
				OutletValueEntry outletValueEntry = dictionary[num];
				if (text.StartsWith(slaveOutletValueMib.SlaveCurrent))
				{
					outletValueEntry.Current = text2;
				}
				else
				{
					if (text.StartsWith(slaveOutletValueMib.SlaveVoltage))
					{
						outletValueEntry.Voltage = text2;
					}
					else
					{
						if (text.StartsWith(slaveOutletValueMib.SlavePower))
						{
							outletValueEntry.Power = text2;
						}
						else
						{
							if (text.StartsWith(slaveOutletValueMib.SlavePowerDissipation))
							{
								outletValueEntry.PowerDissipation = text2;
							}
							else
							{
								outletValueEntry.OutletStatus = (OutletStatus)System.Convert.ToInt32(text2);
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, BankValueEntry> GetSlaveBankValue(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, BankValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveBankValueMib slaveBankValueMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					BankValueEntry value = new BankValueEntry(num);
					dictionary.Add(num, value);
					slaveBankValueMib = new SlaveBankValueMib(index, num);
				}
				BankValueEntry bankValueEntry = dictionary[num];
				if (text.StartsWith(slaveBankValueMib.SlaveCurrent))
				{
					bankValueEntry.Current = text2;
				}
				else
				{
					if (text.StartsWith(slaveBankValueMib.SlaveVoltage))
					{
						bankValueEntry.Voltage = text2;
					}
					else
					{
						if (text.StartsWith(slaveBankValueMib.SlavePower))
						{
							bankValueEntry.Power = text2;
						}
						else
						{
							if (text.StartsWith(slaveBankValueMib.SlavePowerDissipation))
							{
								bankValueEntry.PowerDissipation = text2;
							}
							else
							{
								bankValueEntry.BankStatus = (BankStatus)System.Convert.ToInt32(text2);
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorValueEntry> GetSlaveSensorValue(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, SensorValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveSensorValueMib slaveSensorValueMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-500);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					SensorValueEntry value = new SensorValueEntry(num);
					dictionary.Add(num, value);
					slaveSensorValueMib = new SlaveSensorValueMib(index, num);
				}
				SensorValueEntry sensorValueEntry = dictionary[num];
				if (text.StartsWith(slaveSensorValueMib.SlaveHumidity))
				{
					sensorValueEntry.Humidity = text2;
				}
				else
				{
					if (text.StartsWith(slaveSensorValueMib.SlavePressure))
					{
						sensorValueEntry.Pressure = text2;
					}
					else
					{
						if (text.StartsWith(slaveSensorValueMib.SlaveTemperature))
						{
							sensorValueEntry.Temperature = text2;
						}
					}
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.List<VarBinding> GetChainThresholdsRequest(DevModelConfig modelcfg)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum = modelcfg.bankNum;
			int swichable = modelcfg.switchable;
			int perPortReading = modelcfg.perportreading;
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.Mac);
			leafVarBinding.Add(DeviceBaseMib.ChainNumber);
			leafVarBinding.Add(DeviceEnvironmentMib.MaxCurrentMT);
			if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MinVoltageMT);
				leafVarBinding.Add(DeviceEnvironmentMib.MaxVoltageMT);
				leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerMT);
				leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerDissMT);
			}
			else
			{
				if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerMT);
					leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerDissMT);
				}
			}
			for (int i = 1; i <= sensorNum; i++)
			{
				SensorEnvironmentMib sensorEnvironmentMib = new SensorEnvironmentMib(i);
				leafVarBinding.Add(sensorEnvironmentMib.MaxHumidityMt);
				leafVarBinding.Add(sensorEnvironmentMib.MaxPressMt);
				leafVarBinding.Add(sensorEnvironmentMib.MaxTemperatureMt);
				leafVarBinding.Add(sensorEnvironmentMib.MinHumidityMt);
				leafVarBinding.Add(sensorEnvironmentMib.MinPressMt);
				leafVarBinding.Add(sensorEnvironmentMib.MinTemperatureMt);
			}
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			for (int j = 1; j <= bankNum; j++)
			{
				BankEnvironmentMib bankEnvironmentMib = new BankEnvironmentMib(j);
				leafVarBinding2.Add(bankEnvironmentMib.BankName);
				leafVarBinding2.Add(bankEnvironmentMib.BankMaxCurMT);
				if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold || modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding2.Add(bankEnvironmentMib.BankMinVolMT);
					leafVarBinding2.Add(bankEnvironmentMib.BankMaxVolMT);
					leafVarBinding2.Add(bankEnvironmentMib.BankMaxPMT);
					leafVarBinding2.Add(bankEnvironmentMib.BankMaxPDMT);
				}
			}
			if (bankNum > 0)
			{
				list.Add(leafVarBinding2);
			}
			if (portNum > 0)
			{
				int num = 1;
				if (swichable == 2)
				{
					num += 5;
				}
				if (perPortReading == 2)
				{
					num += 5;
				}
				LeafVBBuilder leafVBBuilder = new LeafVBBuilder(num, portNum);
				System.Collections.Generic.List<LeafVarBinding> list2 = new System.Collections.Generic.List<LeafVarBinding>();
				leafVBBuilder.BuildVbByIndex(list2, delegate(int index, LeafVarBinding leafVb)
				{
					OutletEnvironmentMib outletEnvironmentMib = new OutletEnvironmentMib(index);
					leafVb.Add(outletEnvironmentMib.OutletName);
					if (swichable == 2)
					{
						leafVb.Add(outletEnvironmentMib.Confirmation);
						leafVb.Add(outletEnvironmentMib.MacAddress);
						leafVb.Add(outletEnvironmentMib.OffDelayTime);
						leafVb.Add(outletEnvironmentMib.OnDelayTime);
						leafVb.Add(outletEnvironmentMib.ShutdownMethod);
					}
					if (perPortReading == 2)
					{
						leafVb.Add(outletEnvironmentMib.MaxCurrentMT);
						leafVb.Add(outletEnvironmentMib.MaxPowerDissMT);
						leafVb.Add(outletEnvironmentMib.MaxPowerMT);
						leafVb.Add(outletEnvironmentMib.MaxVoltageMT);
						leafVb.Add(outletEnvironmentMib.MinVoltageMT);
					}
				});
				foreach (LeafVarBinding current in list2)
				{
					list.Add(current);
				}
			}
			return list;
		}
		public static System.Collections.Generic.List<ThresholdMessage> GetChainThresholdMessage(DevModelConfig modelcfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.List<ThresholdMessage> list = new System.Collections.Generic.List<ThresholdMessage>();
			ThresholdMessage thresholdMessage = new ThresholdMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text) || string.IsNullOrEmpty(text) || "Null".Equals(text))
				{
					text = string.Empty;
				}
				if (current.StartsWith(DeviceEnvironmentMib.Entry))
				{
					dictionary.Add(current, text);
				}
				else
				{
					if (current.StartsWith(SensorEnvironmentMib.Entry))
					{
						dictionary2.Add(current, text);
					}
					else
					{
						if (current.StartsWith(OutletEnvironmentMib.Entry))
						{
							dictionary3.Add(current, text);
						}
						else
						{
							if (current.StartsWith(BankEnvironmentMib.Entry))
							{
								dictionary4.Add(current, text);
							}
							else
							{
								if (current.StartsWith(DeviceBaseMib.Mac))
								{
									thresholdMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
								}
								else
								{
									if (current.StartsWith(DeviceBaseMib.ChainNumber))
									{
										thresholdMessage.ChainNums = (string.IsNullOrEmpty(text) ? 0 : System.Convert.ToInt32(text));
									}
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				thresholdMessage.DeviceThreshold = MibChainParser.GetDeviceThreshold(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				thresholdMessage.OutletThreshold = MibChainParser.GetOutletThreshold(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				thresholdMessage.SensorThreshold = MibChainParser.GetSensorThreshold(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				thresholdMessage.BankThreshold = MibChainParser.GetBankThreshold(dictionary4);
			}
			list.Add(thresholdMessage);
			return list;
		}
		private static DeviceThreshold GetDeviceThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			DeviceThreshold deviceThreshold = new DeviceThreshold();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text))
				{
					text = System.Convert.ToString(-10000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-5000);
					}
				}
				float num = (float)System.Convert.ToInt32(text) / 10f;
				if (current.StartsWith(DeviceEnvironmentMib.MaxCurrentMT))
				{
					deviceThreshold.MaxCurrentMT = num;
				}
				else
				{
					if (current.StartsWith(DeviceEnvironmentMib.MaxPowerDissMT))
					{
						deviceThreshold.MaxPowerDissMT = num;
					}
					else
					{
						if (current.StartsWith(DeviceEnvironmentMib.MaxPowerMT))
						{
							deviceThreshold.MaxPowerMT = num;
						}
						else
						{
							if (current.StartsWith(DeviceEnvironmentMib.MaxVoltageMT))
							{
								deviceThreshold.MaxVoltageMT = num;
							}
							else
							{
								if (current.StartsWith(DeviceEnvironmentMib.MinCurrentMt))
								{
									deviceThreshold.MinCurrentMT = num;
								}
								else
								{
									if (current.StartsWith(DeviceEnvironmentMib.MinPowerMT))
									{
										deviceThreshold.MinPowerMT = num;
									}
									else
									{
										if (!current.StartsWith(DeviceEnvironmentMib.MinVoltageMT))
										{
											return null;
										}
										deviceThreshold.MinVoltageMT = num;
									}
								}
							}
						}
					}
				}
				if (deviceThreshold.MinCurrentMT == -500f)
				{
					deviceThreshold.MinCurrentMT = -300f;
				}
				if (deviceThreshold.MinPowerMT == -500f)
				{
					deviceThreshold.MinPowerMT = -300f;
				}
			}
			return deviceThreshold;
		}
		private static System.Collections.Generic.Dictionary<int, SensorThreshold> GetSensorThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorThreshold> dictionary = new System.Collections.Generic.Dictionary<int, SensorThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SensorEnvironmentMib sensorEnvironmentMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					SensorThreshold value = new SensorThreshold(num2);
					dictionary.Add(num2, value);
				}
				SensorThreshold sensorThreshold = dictionary[num2];
				if (num != num2)
				{
					num = num2;
					sensorEnvironmentMib = new SensorEnvironmentMib(num2);
				}
				float num3 = (float)System.Convert.ToInt32(text2) / 10f;
				if (text.StartsWith(sensorEnvironmentMib.MaxHumidityMt))
				{
					sensorThreshold.MaxHumidityMT = num3;
				}
				else
				{
					if (text.StartsWith(sensorEnvironmentMib.MaxPressMt))
					{
						sensorThreshold.MaxPressMT = num3;
					}
					else
					{
						if (text.StartsWith(sensorEnvironmentMib.MaxTemperatureMt))
						{
							sensorThreshold.MaxTemperatureMT = num3;
						}
						else
						{
							if (text.StartsWith(sensorEnvironmentMib.MinHumidityMt))
							{
								sensorThreshold.MinHumidityMT = num3;
							}
							else
							{
								if (text.StartsWith(sensorEnvironmentMib.MinPressMt))
								{
									sensorThreshold.MinPressMT = num3;
								}
								else
								{
									if (!text.StartsWith(sensorEnvironmentMib.MinTemperatureMt))
									{
										return new System.Collections.Generic.Dictionary<int, SensorThreshold>();
									}
									sensorThreshold.MinTemperatureMT = num3;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, OutletThreshold> GetOutletThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, OutletThreshold> dictionary = new System.Collections.Generic.Dictionary<int, OutletThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			OutletEnvironmentMib outletEnvironmentMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					OutletThreshold value = new OutletThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					outletEnvironmentMib = new OutletEnvironmentMib(num2);
					num = num2;
				}
				OutletThreshold outletThreshold = dictionary[num2];
				if (text.StartsWith(outletEnvironmentMib.Confirmation))
				{
					outletThreshold.Confirmation = (Confirmation)System.Convert.ToInt32(text2);
				}
				else
				{
					if (text.StartsWith(outletEnvironmentMib.MacAddress))
					{
						outletThreshold.MacAddress = text2;
					}
					else
					{
						if (text.StartsWith(outletEnvironmentMib.MaxCurrentMT))
						{
							outletThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text2) / 10f;
						}
						else
						{
							if (text.StartsWith(outletEnvironmentMib.MaxPowerDissMT))
							{
								outletThreshold.MaxPowerDissMT = (float)System.Convert.ToInt32(text2) / 10f;
							}
							else
							{
								if (text.StartsWith(outletEnvironmentMib.MaxPowerMT))
								{
									outletThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
								else
								{
									if (text.StartsWith(outletEnvironmentMib.MaxVoltageMT))
									{
										outletThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
									}
									else
									{
										if (text.StartsWith(outletEnvironmentMib.MinCurrentMt))
										{
											outletThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text2) / 10f;
										}
										else
										{
											if (text.StartsWith(outletEnvironmentMib.MinPowerMT))
											{
												outletThreshold.MinPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
											}
											else
											{
												if (text.StartsWith(outletEnvironmentMib.MinVoltageMT))
												{
													outletThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
												}
												else
												{
													if (text.StartsWith(outletEnvironmentMib.OffDelayTime))
													{
														outletThreshold.OffDelayTime = System.Convert.ToInt32(text2);
													}
													else
													{
														if (text.StartsWith(outletEnvironmentMib.OnDelayTime))
														{
															outletThreshold.OnDelayTime = System.Convert.ToInt32(text2);
														}
														else
														{
															if (text.StartsWith(outletEnvironmentMib.OutletName))
															{
																outletThreshold.OutletName = (text2.Equals(System.Convert.ToString(-10000)) ? string.Empty : text2);
															}
															else
															{
																if (!text.StartsWith(outletEnvironmentMib.ShutdownMethod))
																{
																	return new System.Collections.Generic.Dictionary<int, OutletThreshold>();
																}
																outletThreshold.ShutdownMethod = (ShutDownMethod)System.Convert.ToInt32(text2);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (outletThreshold.MinCurrentMt == -500f)
				{
					outletThreshold.MinCurrentMt = -300f;
				}
				if (outletThreshold.MinPowerMT == -500f)
				{
					outletThreshold.MinPowerMT = -300f;
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, BankThreshold> GetBankThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, BankThreshold> dictionary = new System.Collections.Generic.Dictionary<int, BankThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			BankEnvironmentMib bankEnvironmentMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					BankThreshold value = new BankThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					bankEnvironmentMib = new BankEnvironmentMib(num2);
					num = num2;
				}
				BankThreshold bankThreshold = dictionary[num2];
				if (text.StartsWith(bankEnvironmentMib.BankName))
				{
					bankThreshold.BankName = (text2.Equals(System.Convert.ToString(-10000)) ? string.Empty : text2);
				}
				else
				{
					if (text.StartsWith(bankEnvironmentMib.BankMaxCurMT))
					{
						bankThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text2) / 10f;
					}
					else
					{
						if (text.StartsWith(bankEnvironmentMib.BankMinVolMT))
						{
							bankThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
						}
						else
						{
							if (text.StartsWith(bankEnvironmentMib.BankMaxVolMT))
							{
								bankThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
							}
							else
							{
								if (text.StartsWith(bankEnvironmentMib.BankMaxPMT))
								{
									bankThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
								else
								{
									if (!text.StartsWith(bankEnvironmentMib.BankMaxPDMT))
									{
										return new System.Collections.Generic.Dictionary<int, BankThreshold>();
									}
									bankThreshold.MaxPowerDissMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.List<VarBinding> GetSlaveThresholdsRequest(DevModelConfig modelcfg, int index)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum = modelcfg.bankNum;
			int swichable = modelcfg.switchable;
			int perPortReading = modelcfg.perportreading;
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SlaveDeviceEnvironmentMib slaveDeviceEnvironmentMib = new SlaveDeviceEnvironmentMib(index);
			leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxCurrentMT);
			if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold)
			{
				leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerMT);
				leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxVoltageMT);
				leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMinVoltageMT);
				leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerDissMT);
			}
			else
			{
				if (modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerMT);
					leafVarBinding.Add(slaveDeviceEnvironmentMib.SlaveMaxPowerDissMT);
				}
			}
			for (int i = 1; i <= sensorNum; i++)
			{
				SlaveSensorEnvironmentMib slaveSensorEnvironmentMib = new SlaveSensorEnvironmentMib(index, i);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxHumidityMt);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxPressMt);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMaxTemperatureMt);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinHumidityMt);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinPressMt);
				leafVarBinding.Add(slaveSensorEnvironmentMib.SlaveMinTemperatureMt);
			}
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			for (int j = 1; j <= bankNum; j++)
			{
				SlaveBankEnvironmentMib slaveBankEnvironmentMib = new SlaveBankEnvironmentMib(index, j);
				leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankName);
				leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankMaxCurMT);
				if (modelcfg.commonThresholdFlag == Constant.WithCommonThreshold || modelcfg.commonThresholdFlag == Constant.WithSpecialThreshold)
				{
					leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankMinVolMT);
					leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankMaxVolMT);
					leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankMaxPMT);
					leafVarBinding2.Add(slaveBankEnvironmentMib.SlaveBankMaxPDMT);
				}
			}
			if (bankNum > 0)
			{
				list.Add(leafVarBinding2);
			}
			if (portNum > 0)
			{
				int num = 1;
				if (swichable == 2)
				{
					num += 5;
				}
				if (perPortReading == 2)
				{
					num += 5;
				}
				LeafVBBuilder leafVBBuilder = new LeafVBBuilder(num, portNum);
				System.Collections.Generic.List<LeafVarBinding> list2 = new System.Collections.Generic.List<LeafVarBinding>();
				leafVBBuilder.BuildVbByIndex(list2, delegate(int idx, LeafVarBinding leafVb)
				{
					SlaveOutletEnvironmentMib slaveOutletEnvironmentMib = new SlaveOutletEnvironmentMib(index, idx);
					leafVb.Add(slaveOutletEnvironmentMib.SlaveOutletName);
					if (swichable == 2)
					{
						leafVb.Add(slaveOutletEnvironmentMib.SlaveConfirmation);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMacAddress);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveOffDelayTime);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveOnDelayTime);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveShutdownMethod);
					}
					if (perPortReading == 2)
					{
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMaxCurrentMT);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMaxPowerDissMT);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMaxPowerMT);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMaxVoltageMT);
						leafVb.Add(slaveOutletEnvironmentMib.SlaveMinVoltageMT);
					}
				});
				foreach (LeafVarBinding current in list2)
				{
					list.Add(current);
				}
			}
			return list;
		}
		public static ThresholdMessage GetSlaveThresholdsMessage(DevModelConfig modelcfg, System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			ThresholdMessage thresholdMessage = new ThresholdMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			SlaveDeviceEnvironmentMib slaveDeviceEnvironmentMib = new SlaveDeviceEnvironmentMib(index);
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string value = result[current];
				if ("\0".Equals(value) || string.IsNullOrEmpty(value) || "Null".Equals(value))
				{
					value = string.Empty;
				}
				if (current.StartsWith(slaveDeviceEnvironmentMib.DeviceConfigEntry))
				{
					dictionary.Add(current, value);
				}
				else
				{
					if (current.StartsWith(slaveDeviceEnvironmentMib.SensorConfigEntry))
					{
						dictionary2.Add(current, value);
					}
					else
					{
						if (current.StartsWith(slaveDeviceEnvironmentMib.OutletConfigEntry))
						{
							dictionary3.Add(current, value);
						}
						else
						{
							if (current.StartsWith(slaveDeviceEnvironmentMib.BankConfigEntry))
							{
								dictionary4.Add(current, value);
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				thresholdMessage.DeviceThreshold = MibChainParser.GetSlaveDeviceThreshold(dictionary, index);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				thresholdMessage.OutletThreshold = MibChainParser.GetSlaveOutletThreshold(dictionary3, index);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				thresholdMessage.SensorThreshold = MibChainParser.GetSlaveSensorThreshold(dictionary2, index);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				thresholdMessage.BankThreshold = MibChainParser.GetSlaveBankThreshold(dictionary4, index);
			}
			return thresholdMessage;
		}
		private static DeviceThreshold GetSlaveDeviceThreshold(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			DeviceThreshold deviceThreshold = new DeviceThreshold();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text))
				{
					text = System.Convert.ToString(-10000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-5000);
					}
				}
				float num = (float)System.Convert.ToInt32(text) / 10f;
				SlaveDeviceEnvironmentMib slaveDeviceEnvironmentMib = new SlaveDeviceEnvironmentMib(index);
				if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMaxCurrentMT))
				{
					deviceThreshold.MaxCurrentMT = num;
				}
				else
				{
					if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMaxPowerDissMT))
					{
						deviceThreshold.MaxPowerDissMT = num;
					}
					else
					{
						if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMaxPowerMT))
						{
							deviceThreshold.MaxPowerMT = num;
						}
						else
						{
							if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMaxVoltageMT))
							{
								deviceThreshold.MaxVoltageMT = num;
							}
							else
							{
								if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMinCurrentMt))
								{
									deviceThreshold.MinCurrentMT = num;
								}
								else
								{
									if (current.StartsWith(slaveDeviceEnvironmentMib.SlaveMinPowerMT))
									{
										deviceThreshold.MinPowerMT = num;
									}
									else
									{
										if (!current.StartsWith(slaveDeviceEnvironmentMib.SlaveMinVoltageMT))
										{
											return null;
										}
										deviceThreshold.MinVoltageMT = num;
									}
								}
							}
						}
					}
				}
				if (deviceThreshold.MinCurrentMT == -500f)
				{
					deviceThreshold.MinCurrentMT = -300f;
				}
				if (deviceThreshold.MinPowerMT == -500f)
				{
					deviceThreshold.MinPowerMT = -300f;
				}
			}
			return deviceThreshold;
		}
		private static System.Collections.Generic.Dictionary<int, SensorThreshold> GetSlaveSensorThreshold(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, SensorThreshold> dictionary = new System.Collections.Generic.Dictionary<int, SensorThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveSensorEnvironmentMib slaveSensorEnvironmentMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					SensorThreshold value = new SensorThreshold(num);
					dictionary.Add(num, value);
					slaveSensorEnvironmentMib = new SlaveSensorEnvironmentMib(index, num);
				}
				SensorThreshold sensorThreshold = dictionary[num];
				float num2 = (float)System.Convert.ToInt32(text2) / 10f;
				if (text.StartsWith(slaveSensorEnvironmentMib.SlaveMaxHumidityMt))
				{
					sensorThreshold.MaxHumidityMT = num2;
				}
				else
				{
					if (text.StartsWith(slaveSensorEnvironmentMib.SlaveMaxPressMt))
					{
						sensorThreshold.MaxPressMT = num2;
					}
					else
					{
						if (text.StartsWith(slaveSensorEnvironmentMib.SlaveMaxTemperatureMt))
						{
							sensorThreshold.MaxTemperatureMT = num2;
						}
						else
						{
							if (text.StartsWith(slaveSensorEnvironmentMib.SlaveMinHumidityMt))
							{
								sensorThreshold.MinHumidityMT = num2;
							}
							else
							{
								if (text.StartsWith(slaveSensorEnvironmentMib.SlaveMinPressMt))
								{
									sensorThreshold.MinPressMT = num2;
								}
								else
								{
									if (!text.StartsWith(slaveSensorEnvironmentMib.SlaveMinTemperatureMt))
									{
										return new System.Collections.Generic.Dictionary<int, SensorThreshold>();
									}
									sensorThreshold.MinTemperatureMT = num2;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, OutletThreshold> GetSlaveOutletThreshold(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, OutletThreshold> dictionary = new System.Collections.Generic.Dictionary<int, OutletThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveOutletEnvironmentMib slaveOutletEnvironmentMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					OutletThreshold value = new OutletThreshold(num);
					dictionary.Add(num, value);
					slaveOutletEnvironmentMib = new SlaveOutletEnvironmentMib(index, num);
				}
				OutletThreshold outletThreshold = dictionary[num];
				if (text.StartsWith(slaveOutletEnvironmentMib.SlaveConfirmation))
				{
					outletThreshold.Confirmation = (Confirmation)System.Convert.ToInt32(text2);
				}
				else
				{
					if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMacAddress))
					{
						outletThreshold.MacAddress = text2;
					}
					else
					{
						if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMaxCurrentMT))
						{
							outletThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text2) / 10f;
						}
						else
						{
							if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMaxPowerDissMT))
							{
								outletThreshold.MaxPowerDissMT = (float)System.Convert.ToInt32(text2) / 10f;
							}
							else
							{
								if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMaxPowerMT))
								{
									outletThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
								else
								{
									if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMaxVoltageMT))
									{
										outletThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
									}
									else
									{
										if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMinCurrentMt))
										{
											outletThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text2) / 10f;
										}
										else
										{
											if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMinPowerMT))
											{
												outletThreshold.MinPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
											}
											else
											{
												if (text.StartsWith(slaveOutletEnvironmentMib.SlaveMinVoltageMT))
												{
													outletThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
												}
												else
												{
													if (text.StartsWith(slaveOutletEnvironmentMib.SlaveOffDelayTime))
													{
														outletThreshold.OffDelayTime = System.Convert.ToInt32(text2);
													}
													else
													{
														if (text.StartsWith(slaveOutletEnvironmentMib.SlaveOnDelayTime))
														{
															outletThreshold.OnDelayTime = System.Convert.ToInt32(text2);
														}
														else
														{
															if (text.StartsWith(slaveOutletEnvironmentMib.SlaveOutletName))
															{
																outletThreshold.OutletName = (text2.Equals(System.Convert.ToString(-10000)) ? string.Empty : text2);
															}
															else
															{
																if (!text.StartsWith(slaveOutletEnvironmentMib.SlaveShutdownMethod))
																{
																	return new System.Collections.Generic.Dictionary<int, OutletThreshold>();
																}
																outletThreshold.ShutdownMethod = (ShutDownMethod)System.Convert.ToInt32(text2);
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (outletThreshold.MinCurrentMt == -500f)
				{
					outletThreshold.MinCurrentMt = -300f;
				}
				if (outletThreshold.MinPowerMT == -500f)
				{
					outletThreshold.MinPowerMT = -300f;
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, BankThreshold> GetSlaveBankThreshold(System.Collections.Generic.Dictionary<string, string> result, int index)
		{
			System.Collections.Generic.Dictionary<int, BankThreshold> dictionary = new System.Collections.Generic.Dictionary<int, BankThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			SlaveBankEnvironmentMib slaveBankEnvironmentMib = null;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2))
				{
					text2 = System.Convert.ToString(-10000);
				}
				else
				{
					if (text2 == null || string.IsNullOrEmpty(text2))
					{
						text2 = System.Convert.ToString(-5000);
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					BankThreshold value = new BankThreshold(num);
					dictionary.Add(num, value);
					slaveBankEnvironmentMib = new SlaveBankEnvironmentMib(index, num);
				}
				BankThreshold bankThreshold = dictionary[num];
				if (text.StartsWith(slaveBankEnvironmentMib.SlaveBankName))
				{
					bankThreshold.BankName = (text2.Equals(System.Convert.ToString(-10000)) ? string.Empty : text2);
				}
				else
				{
					if (text.StartsWith(slaveBankEnvironmentMib.SlaveBankMaxCurMT))
					{
						bankThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text2) / 10f;
					}
					else
					{
						if (text.StartsWith(slaveBankEnvironmentMib.SlaveBankMinVolMT))
						{
							bankThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
						}
						else
						{
							if (text.StartsWith(slaveBankEnvironmentMib.SlaveBankMaxVolMT))
							{
								bankThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
							}
							else
							{
								if (text.StartsWith(slaveBankEnvironmentMib.SlaveBankMaxPMT))
								{
									bankThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
								else
								{
									if (!text.StartsWith(slaveBankEnvironmentMib.SlaveBankMaxPDMT))
									{
										return new System.Collections.Generic.Dictionary<int, BankThreshold>();
									}
									bankThreshold.MaxPowerDissMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
	}
}
