using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	internal static class EatonMibParser
	{
		public static LeafVarBinding GetEatonPDUPropertiesRequest_M2()
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(EatonPDUBaseMib_M2.DeviceName);
			leafVarBinding.Add(EatonPDUBaseMib_M2.FWversion);
			leafVarBinding.Add(EatonPDUBaseMib_M2.Mac);
			leafVarBinding.Add(EatonPDUBaseMib_M2.ModelName);
			return leafVarBinding;
		}
		public static PropertiesMessage GetEatonPDUPropertiesMessage_M2(System.Collections.Generic.Dictionary<string, string> variables)
		{
			if (variables == null || variables.Count < 1)
			{
				return null;
			}
			PropertiesMessage propertiesMessage = new PropertiesMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = variables.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = variables[current];
				if (current.StartsWith(EatonPDUBaseMib_M2.DeviceName))
				{
					if ("\0".Equals(text) || string.IsNullOrEmpty(text))
					{
						text = string.Empty;
					}
					propertiesMessage.DeviceName = text;
				}
				else
				{
					if (current.StartsWith(EatonPDUBaseMib_M2.FWversion))
					{
						propertiesMessage.FirwWareVersion = text;
					}
					else
					{
						if (current.StartsWith(EatonPDUBaseMib_M2.Mac))
						{
							propertiesMessage.MacAddress = text.Replace(" ", ":").Replace("-", ":");
						}
						else
						{
							if (!current.StartsWith(EatonPDUBaseMib_M2.ModelName))
							{
								return null;
							}
							if ("\0".Equals(text) || string.IsNullOrEmpty(text))
							{
								return null;
							}
							propertiesMessage.ModelName = text;
						}
					}
				}
			}
			propertiesMessage.DashboardRackname = "";
			propertiesMessage.CreateTime = System.DateTime.Now;
			return propertiesMessage;
		}
		public static System.Collections.Generic.List<VarBinding> GetThresholdsRequestEatonPDU_M2(DevModelConfig modelcfg, DevRealConfig realcfg)
		{
			int num = 1;
			int portNum = realcfg.portNum;
			int bankNum = realcfg.bankNum;
			int sensorNum = realcfg.sensorNum;
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(EatonPDUBaseMib_M2.Mac);
			leafVarBinding.Add(EatonPDUBaseMib_M2.FWversion);
			leafVarBinding.Add(EatonPDUBaseMib_M2.DeviceName);
			for (int i = 1; i <= num; i++)
			{
				EatonInputCurrentMib_M2 eatonInputCurrentMib_M = new EatonInputCurrentMib_M2(i);
				leafVarBinding.Add(eatonInputCurrentMib_M.MaxCurrentMT);
				EatonInputVoltageMib_M2 eatonInputVoltageMib_M = new EatonInputVoltageMib_M2(i);
				leafVarBinding.Add(eatonInputVoltageMib_M.MinVoltageMt);
				leafVarBinding.Add(eatonInputVoltageMib_M.MaxVoltageMT);
			}
			for (int j = 1; j <= sensorNum; j++)
			{
				EatonSensorTemperatureMib_M2 eatonSensorTemperatureMib_M = new EatonSensorTemperatureMib_M2(j);
				leafVarBinding.Add(eatonSensorTemperatureMib_M.MinTemperatureMt);
				leafVarBinding.Add(eatonSensorTemperatureMib_M.MaxTemperatureMT);
				EatonSensorHumidityMib_M2 eatonSensorHumidityMib_M = new EatonSensorHumidityMib_M2(j);
				leafVarBinding.Add(eatonSensorHumidityMib_M.MinHumidityMt);
				leafVarBinding.Add(eatonSensorHumidityMib_M.MaxHumidityMT);
			}
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			for (int k = 1; k <= bankNum; k++)
			{
				EatonGroupEntryMib_M2 eatonGroupEntryMib_M = new EatonGroupEntryMib_M2(k);
				leafVarBinding2.Add(eatonGroupEntryMib_M.GroupName);
				EatonGroupCurrentMib_M2 eatonGroupCurrentMib_M = new EatonGroupCurrentMib_M2(k);
				leafVarBinding2.Add(eatonGroupCurrentMib_M.MaxCurrentMT);
			}
			if (bankNum > 0)
			{
				list.Add(leafVarBinding2);
			}
			if (portNum > 0)
			{
				int nodes = 4;
				LeafVBBuilder leafVBBuilder = new LeafVBBuilder(nodes, portNum);
				System.Collections.Generic.List<LeafVarBinding> list2 = new System.Collections.Generic.List<LeafVarBinding>();
				leafVBBuilder.BuildVbByIndex(list2, delegate(int index, LeafVarBinding leafVb)
				{
					EatonOutletEntryMib_M2 eatonOutletEntryMib_M = new EatonOutletEntryMib_M2(index);
					leafVb.Add(eatonOutletEntryMib_M.OutletName);
					EatonOutletVoltageMib_M2 eatonOutletVoltageMib_M = new EatonOutletVoltageMib_M2(index);
					leafVb.Add(eatonOutletVoltageMib_M.MinVoltageMt);
					leafVb.Add(eatonOutletVoltageMib_M.MaxVoltageMT);
					EatonOutletCurrentMib_M2 eatonOutletCurrentMib_M = new EatonOutletCurrentMib_M2(index);
					leafVb.Add(eatonOutletCurrentMib_M.MaxCurrentMT);
				});
				foreach (LeafVarBinding current in list2)
				{
					list.Add(current);
				}
			}
			return list;
		}
		public static ThresholdMessage GetThresholdMessageEatonPDU_M2(DevModelConfig modelcfg, System.Collections.Generic.Dictionary<string, string> result)
		{
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
				if (current.StartsWith(EatonInputCurrentMib_M2.Entry) || current.StartsWith(EatonInputVoltageMib_M2.Entry))
				{
					dictionary.Add(current, text);
				}
				else
				{
					if (current.StartsWith(EatonSensorTemperatureMib_M2.Entry) || current.StartsWith(EatonSensorHumidityMib_M2.Entry))
					{
						dictionary2.Add(current, text);
					}
					else
					{
						if (current.StartsWith(EatonOutletCurrentMib_M2.Entry) || current.StartsWith(EatonOutletVoltageMib_M2.Entry) || current.StartsWith(EatonOutletEntryMib_M2.Entry) || current.StartsWith(EatonOutletControlMib_M2.Entry))
						{
							dictionary3.Add(current, text);
						}
						else
						{
							if (current.StartsWith(EatonGroupCurrentMib_M2.Entry) || current.StartsWith(EatonGroupVoltageMib_M2.Entry) || current.StartsWith(EatonGroupEntryMib_M2.Entry))
							{
								dictionary4.Add(current, text);
							}
							else
							{
								if (current.StartsWith(EatonPDUBaseMib_M2.Mac))
								{
									thresholdMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
								}
								else
								{
									if (current.StartsWith(EatonPDUBaseMib_M2.FWversion))
									{
										thresholdMessage.DeviceFWVer = text;
									}
									else
									{
										if (current.StartsWith(EatonPDUBaseMib_M2.DeviceName))
										{
											if ("\0".Equals(text) || string.IsNullOrEmpty(text))
											{
												text = string.Empty;
											}
											thresholdMessage.DeviceName = text;
										}
									}
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				thresholdMessage.DeviceThreshold = EatonMibParser.GetDeviceThreshold(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				thresholdMessage.OutletThreshold = EatonMibParser.GetOutletThreshold(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				thresholdMessage.SensorThreshold = EatonMibParser.GetSensorThreshold(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				thresholdMessage.BankThreshold = EatonMibParser.GetBankThreshold(dictionary4);
			}
			return thresholdMessage;
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
					text = System.Convert.ToString(-1000000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-500000);
					}
					else
					{
						if (text.Equals("Null"))
						{
							text = System.Convert.ToString(-500000);
						}
					}
				}
				int num = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (num == 1)
				{
					EatonInputCurrentMib_M2 eatonInputCurrentMib_M = new EatonInputCurrentMib_M2(num);
					EatonInputVoltageMib_M2 eatonInputVoltageMib_M = new EatonInputVoltageMib_M2(num);
					float num2 = (float)System.Convert.ToInt32(text) / 1000f;
					if (current.StartsWith(eatonInputCurrentMib_M.MaxCurrentMT))
					{
						deviceThreshold.MaxCurrentMT = num2;
					}
					else
					{
						if (current.StartsWith(eatonInputVoltageMib_M.MaxVoltageMT))
						{
							deviceThreshold.MaxVoltageMT = num2;
						}
						else
						{
							if (current.StartsWith(eatonInputCurrentMib_M.MinCurrentMt))
							{
								deviceThreshold.MinCurrentMT = num2;
							}
							else
							{
								if (!current.StartsWith(eatonInputVoltageMib_M.MinVoltageMt))
								{
									return null;
								}
								deviceThreshold.MinVoltageMT = num2;
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
			}
			return deviceThreshold;
		}
		private static System.Collections.Generic.Dictionary<int, SensorThreshold> GetSensorThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorThreshold> dictionary = new System.Collections.Generic.Dictionary<int, SensorThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			EatonSensorTemperatureMib_M2 eatonSensorTemperatureMib_M = null;
			EatonSensorHumidityMib_M2 eatonSensorHumidityMib_M = null;
			int num = 0;
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
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					SensorThreshold value = new SensorThreshold(num2);
					dictionary.Add(num2, value);
				}
				SensorThreshold sensorThreshold = dictionary[num2];
				if (num != num2)
				{
					num = num2;
					eatonSensorTemperatureMib_M = new EatonSensorTemperatureMib_M2(num2);
					eatonSensorHumidityMib_M = new EatonSensorHumidityMib_M2(num2);
				}
				float num3 = (float)System.Convert.ToInt32(text) / 10f;
				if (current.StartsWith(eatonSensorHumidityMib_M.MaxHumidityMT))
				{
					sensorThreshold.MaxHumidityMT = num3;
				}
				else
				{
					if (current.StartsWith(eatonSensorTemperatureMib_M.MaxTemperatureMT))
					{
						sensorThreshold.MaxTemperatureMT = num3;
					}
					else
					{
						if (current.StartsWith(eatonSensorHumidityMib_M.MinHumidityMt))
						{
							sensorThreshold.MinHumidityMT = num3;
						}
						else
						{
							if (!current.StartsWith(eatonSensorTemperatureMib_M.MinTemperatureMt))
							{
								return new System.Collections.Generic.Dictionary<int, SensorThreshold>();
							}
							sensorThreshold.MinTemperatureMT = num3;
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
			EatonOutletEntryMib_M2 eatonOutletEntryMib_M = null;
			EatonOutletCurrentMib_M2 eatonOutletCurrentMib_M = null;
			EatonOutletVoltageMib_M2 eatonOutletVoltageMib_M = null;
			EatonOutletControlMib_M2 eatonOutletControlMib_M = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text))
				{
					text = System.Convert.ToString(-1000000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-500000);
					}
				}
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					OutletThreshold value = new OutletThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					eatonOutletEntryMib_M = new EatonOutletEntryMib_M2(num2);
					eatonOutletCurrentMib_M = new EatonOutletCurrentMib_M2(num2);
					eatonOutletVoltageMib_M = new EatonOutletVoltageMib_M2(num2);
					num = num2;
				}
				OutletThreshold outletThreshold = dictionary[num2];
				if (current.StartsWith(eatonOutletCurrentMib_M.MaxCurrentMT))
				{
					outletThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text) / 1000f;
				}
				else
				{
					if (current.StartsWith(eatonOutletVoltageMib_M.MaxVoltageMT))
					{
						outletThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text) / 1000f;
					}
					else
					{
						if (current.StartsWith(eatonOutletCurrentMib_M.MinCurrentMt))
						{
							outletThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text) / 1000f;
						}
						else
						{
							if (current.StartsWith(eatonOutletVoltageMib_M.MinVoltageMt))
							{
								outletThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text) / 1000f;
							}
							else
							{
								if (current.StartsWith(eatonOutletEntryMib_M.OutletName))
								{
									outletThreshold.OutletName = (text.Equals(System.Convert.ToString(-1000000)) ? string.Empty : text);
								}
								else
								{
									if (!current.StartsWith(eatonOutletControlMib_M.ControlSequenceDelay))
									{
										return new System.Collections.Generic.Dictionary<int, OutletThreshold>();
									}
									outletThreshold.OffDelayTime = System.Convert.ToInt32(text);
									outletThreshold.OnDelayTime = System.Convert.ToInt32(text);
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
			EatonGroupEntryMib_M2 eatonGroupEntryMib_M = null;
			EatonGroupVoltageMib_M2 eatonGroupVoltageMib_M = null;
			EatonGroupCurrentMib_M2 eatonGroupCurrentMib_M = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if ("\0".Equals(text))
				{
					text = System.Convert.ToString(-1000000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-500000);
					}
				}
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					BankThreshold value = new BankThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					eatonGroupEntryMib_M = new EatonGroupEntryMib_M2(num2);
					eatonGroupCurrentMib_M = new EatonGroupCurrentMib_M2(num2);
					eatonGroupVoltageMib_M = new EatonGroupVoltageMib_M2(num2);
					num = num2;
				}
				BankThreshold bankThreshold = dictionary[num2];
				if (current.StartsWith(eatonGroupEntryMib_M.GroupName))
				{
					bankThreshold.BankName = (text.Equals(System.Convert.ToString(-1000000)) ? string.Empty : text);
				}
				else
				{
					if (current.StartsWith(eatonGroupCurrentMib_M.MinCurrentMt))
					{
						bankThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text) / 1000f;
					}
					else
					{
						if (current.StartsWith(eatonGroupCurrentMib_M.MaxCurrentMT))
						{
							bankThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text) / 1000f;
						}
						else
						{
							if (current.StartsWith(eatonGroupVoltageMib_M.MinVoltageMt))
							{
								bankThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text) / 1000f;
							}
							else
							{
								if (!current.StartsWith(eatonGroupVoltageMib_M.MaxVoltageMT))
								{
									return new System.Collections.Generic.Dictionary<int, BankThreshold>();
								}
								bankThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text) / 1000f;
							}
						}
					}
				}
				if (bankThreshold.MinCurrentMt == -500f)
				{
					bankThreshold.MinCurrentMt = -300f;
				}
				if (bankThreshold.MinPowerMT == -500f)
				{
					bankThreshold.MinPowerMT = -300f;
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetValuesRequestEatonPDU_M2(DevModelConfig modelcfg, DevRealConfig realcfg)
		{
			int num = 1;
			int portNum = realcfg.portNum;
			int bankNum = realcfg.bankNum;
			int sensorNum = realcfg.sensorNum;
			int contactNum = realcfg.contactNum;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(EatonPDUBaseMib_M2.Mac);
			for (int l = 1; l <= num; l++)
			{
				EatonInputCurrentMib_M2 eatonInputCurrentMib_M = new EatonInputCurrentMib_M2(l);
				leafVarBinding.Add(eatonInputCurrentMib_M.CurrentValue);
				EatonInputVoltageMib_M2 eatonInputVoltageMib_M = new EatonInputVoltageMib_M2(l);
				leafVarBinding.Add(eatonInputVoltageMib_M.VoltageValue);
				EatonInputPowerMib_M2 eatonInputPowerMib_M = new EatonInputPowerMib_M2(1);
				leafVarBinding.Add(eatonInputPowerMib_M.PowerValue);
				leafVarBinding.Add(eatonInputPowerMib_M.PowerValue_VA);
			}
			for (int j = 1; j <= contactNum; j++)
			{
				EatonSensorContactMib_M2 eatonSensorContactMib_M = new EatonSensorContactMib_M2(j);
				leafVarBinding.Add(eatonSensorContactMib_M.ContactStatus);
			}
			for (int k = 1; k <= sensorNum; k++)
			{
				EatonSensorTemperatureMib_M2 eatonSensorTemperatureMib_M = new EatonSensorTemperatureMib_M2(k);
				leafVarBinding.Add(eatonSensorTemperatureMib_M.TemperatureValue);
				EatonSensorHumidityMib_M2 eatonSensorHumidityMib_M = new EatonSensorHumidityMib_M2(k);
				leafVarBinding.Add(eatonSensorHumidityMib_M.HumidityValue);
			}
			list.Add(leafVarBinding);
			if (portNum > 0)
			{
				LeafVBBuilder leafVBBuilder = new LeafVBBuilder(3, portNum);
				leafVBBuilder.BuildVbByIndex(list, delegate(int i, LeafVarBinding leafVb)
				{
					EatonOutletVoltageMib_M2 eatonOutletVoltageMib_M = new EatonOutletVoltageMib_M2(i);
					leafVb.Add(eatonOutletVoltageMib_M.VoltageValue);
					EatonOutletCurrentMib_M2 eatonOutletCurrentMib_M = new EatonOutletCurrentMib_M2(i);
					leafVb.Add(eatonOutletCurrentMib_M.CurrentValue);
					EatonOutletPowerMib_M2 eatonOutletPowerMib_M = new EatonOutletPowerMib_M2(i);
					leafVb.Add(eatonOutletPowerMib_M.PowerValue);
				});
			}
			if (bankNum > 0)
			{
				LeafVBBuilder leafVBBuilder2 = new LeafVBBuilder(2, bankNum);
				leafVBBuilder2.BuildVbByIndex(list, delegate(int i, LeafVarBinding leafVb)
				{
					EatonGroupCurrentMib_M2 eatonGroupCurrentMib_M = new EatonGroupCurrentMib_M2(i);
					leafVb.Add(eatonGroupCurrentMib_M.CurrentValue);
					EatonGroupPowerMib_M2 eatonGroupPowerMib_M = new EatonGroupPowerMib_M2(i);
					leafVb.Add(eatonGroupPowerMib_M.PowerValue);
				});
			}
			return list;
		}
		public static ValueMessage GetValueMessageEatonPDU_M2(DevModelConfig modelCfg, System.Collections.Generic.Dictionary<string, string> result)
		{
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
				if (current.StartsWith(EatonInputCurrentMib_M2.Entry) || current.StartsWith(EatonInputVoltageMib_M2.Entry) || current.StartsWith(EatonInputPowerMib_M2.Entry))
				{
					dictionary.Add(current, text);
				}
				else
				{
					if (current.StartsWith(EatonSensorTemperatureMib_M2.Entry) || current.StartsWith(EatonSensorHumidityMib_M2.Entry))
					{
						dictionary2.Add(current, text);
					}
					else
					{
						if (current.StartsWith(EatonOutletCurrentMib_M2.Entry) || current.StartsWith(EatonOutletVoltageMib_M2.Entry) || current.StartsWith(EatonOutletPowerMib_M2.Entry))
						{
							dictionary3.Add(current, text);
						}
						else
						{
							if (current.StartsWith(EatonGroupCurrentMib_M2.Entry) || current.StartsWith(EatonGroupVoltageMib_M2.Entry) || current.StartsWith(EatonGroupPowerMib_M2.Entry))
							{
								dictionary4.Add(current, text);
							}
							else
							{
								if (current.StartsWith(EatonPDUBaseMib_M2.Mac))
								{
									valueMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				valueMessage.DeviceValue = EatonMibParser.GetDeviceValue(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				valueMessage.OutletValue = EatonMibParser.GetOutletValue(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				valueMessage.SensorValue = EatonMibParser.GetSensorValue(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				valueMessage.BankValue = EatonMibParser.GetBankValue(dictionary4);
			}
			if (modelCfg.doorReading == Constant.DoorSensorNo)
			{
				valueMessage.DeviceValue.DoorSensorStatus = -500;
			}
			if (modelCfg.commonThresholdFlag == Constant.EatonPDU_M2)
			{
				valueMessage.DeviceValue.PowerDissipation = "N/A";
				if (valueMessage.BankValue != null && valueMessage.BankValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, BankValueEntry> current2 in valueMessage.BankValue)
					{
						BankValueEntry value = current2.Value;
						value.PowerDissipation = "N/A";
						value.Voltage = valueMessage.DeviceValue.Voltage;
					}
				}
				if (valueMessage.OutletValue != null && valueMessage.OutletValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, OutletValueEntry> current3 in valueMessage.OutletValue)
					{
						OutletValueEntry value2 = current3.Value;
						value2.PowerDissipation = "N/A";
					}
				}
			}
			return valueMessage;
		}
		private static DeviceValueEntry GetDeviceValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			string s = "0";
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
				int num = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (num == 1)
				{
					EatonInputCurrentMib_M2 eatonInputCurrentMib_M = new EatonInputCurrentMib_M2(num);
					EatonInputVoltageMib_M2 eatonInputVoltageMib_M = new EatonInputVoltageMib_M2(num);
					EatonInputPowerMib_M2 eatonInputPowerMib_M = new EatonInputPowerMib_M2(num);
					float arg_B2_0 = (float)System.Convert.ToInt32(text) / 1000f;
					if (current.StartsWith(eatonInputCurrentMib_M.CurrentValue))
					{
						deviceValueEntry.Current = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
					}
					else
					{
						if (current.StartsWith(eatonInputVoltageMib_M.VoltageValue))
						{
							deviceValueEntry.Voltage = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
						}
						else
						{
							if (current.StartsWith(eatonInputPowerMib_M.PowerValue))
							{
								deviceValueEntry.Power = text;
							}
							else
							{
								if (current.StartsWith(eatonInputPowerMib_M.PowerValue_VA))
								{
									s = text;
								}
							}
						}
					}
				}
			}
			if (double.Parse(deviceValueEntry.Current) <= 0.0)
			{
				deviceValueEntry.Current = (double.Parse(s) / double.Parse(deviceValueEntry.Voltage)).ToString("F2");
			}
			return deviceValueEntry;
		}
		private static System.Collections.Generic.Dictionary<int, OutletValueEntry> GetOutletValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, OutletValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, OutletValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			int num = 0;
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
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					OutletValueEntry value = new OutletValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					new OutletValueMib(num2);
					num = num2;
				}
				OutletValueEntry outletValueEntry = dictionary[num2];
				EatonOutletCurrentMib_M2 eatonOutletCurrentMib_M = new EatonOutletCurrentMib_M2(num2);
				EatonOutletVoltageMib_M2 eatonOutletVoltageMib_M = new EatonOutletVoltageMib_M2(num2);
				EatonOutletPowerMib_M2 eatonOutletPowerMib_M = new EatonOutletPowerMib_M2(num2);
				if (current.StartsWith(eatonOutletCurrentMib_M.CurrentValue))
				{
					outletValueEntry.Current = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
				}
				else
				{
					if (current.StartsWith(eatonOutletVoltageMib_M.VoltageValue))
					{
						outletValueEntry.Voltage = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
					}
					else
					{
						if (current.StartsWith(eatonOutletPowerMib_M.PowerValue))
						{
							outletValueEntry.Power = text;
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
			int num = 0;
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
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					BankValueEntry value = new BankValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					new BankValueMib(num2);
					num = num2;
				}
				BankValueEntry bankValueEntry = dictionary[num2];
				EatonGroupCurrentMib_M2 eatonGroupCurrentMib_M = new EatonGroupCurrentMib_M2(num2);
				EatonGroupVoltageMib_M2 eatonGroupVoltageMib_M = new EatonGroupVoltageMib_M2(num2);
				EatonGroupPowerMib_M2 eatonGroupPowerMib_M = new EatonGroupPowerMib_M2(num2);
				if (current.StartsWith(eatonGroupCurrentMib_M.CurrentValue))
				{
					bankValueEntry.Current = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
				}
				else
				{
					if (current.StartsWith(eatonGroupVoltageMib_M.VoltageValue))
					{
						bankValueEntry.Voltage = System.Convert.ToString((float)System.Convert.ToInt32(text) / 1000f);
					}
					else
					{
						if (current.StartsWith(eatonGroupPowerMib_M.PowerValue))
						{
							bankValueEntry.Power = text;
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
			int num = 0;
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string value = result[current];
				if ("\0".Equals(value) || "n/a".Equals(value) || "N/A".Equals(value))
				{
					value = System.Convert.ToString(-10000);
				}
				else
				{
					if (string.IsNullOrEmpty(value))
					{
						value = System.Convert.ToString(-5000);
					}
				}
				string text = ((float)System.Convert.ToInt32(value) / 10f).ToString();
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					SensorValueEntry value2 = new SensorValueEntry(num2);
					dictionary.Add(num2, value2);
				}
				if (num != num2)
				{
					new SensorValueMib(num2);
					num = num2;
				}
				SensorValueEntry sensorValueEntry = dictionary[num2];
				EatonSensorTemperatureMib_M2 eatonSensorTemperatureMib_M = new EatonSensorTemperatureMib_M2(num2);
				EatonSensorHumidityMib_M2 eatonSensorHumidityMib_M = new EatonSensorHumidityMib_M2(num2);
				if (current.StartsWith(eatonSensorTemperatureMib_M.TemperatureValue))
				{
					sensorValueEntry.Temperature = text;
				}
				else
				{
					if (current.StartsWith(eatonSensorHumidityMib_M.HumidityValue))
					{
						sensorValueEntry.Humidity = text;
					}
				}
			}
			return dictionary;
		}
		public static LeafVarBinding SetDeviceThresholdVariablesEatonPDU_M2(DeviceThreshold threshold, DevModelConfig modelcfg)
		{
			int inputNum = 1;
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			EatonInputCurrentMib_M2 eatonInputCurrentMib_M = new EatonInputCurrentMib_M2(inputNum);
			EatonInputVoltageMib_M2 eatonInputVoltageMib_M = new EatonInputVoltageMib_M2(inputNum);
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonInputCurrentMib_M.MaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			if (threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonInputVoltageMib_M.MaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 1000f));
			}
			if (threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonInputVoltageMib_M.MinVoltageMt, System.Convert.ToInt32(threshold.MinVoltageMT * 1000f));
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonInputCurrentMib_M.CurrentUpperCritical, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			if (threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonInputVoltageMib_M.VoltageUpperCritical, System.Convert.ToInt32(threshold.MaxVoltageMT * 1000f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetOutletThresholdVariablesEatonPDU_M2(OutletThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			int outletNumber = threshold.OutletNumber;
			EatonOutletEntryMib_M2 eatonOutletEntryMib_M = new EatonOutletEntryMib_M2(outletNumber);
			EatonOutletVoltageMib_M2 eatonOutletVoltageMib_M = new EatonOutletVoltageMib_M2(outletNumber);
			EatonOutletCurrentMib_M2 eatonOutletCurrentMib_M = new EatonOutletCurrentMib_M2(outletNumber);
			if (!string.IsNullOrEmpty(threshold.OutletName))
			{
				leafVarBinding.Add(eatonOutletEntryMib_M.OutletName, threshold.OutletName);
			}
			else
			{
				leafVarBinding.Add(eatonOutletEntryMib_M.OutletName, "/empty");
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonOutletCurrentMib_M.MaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			if (threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonOutletVoltageMib_M.MaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 1000f));
			}
			if (threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonOutletVoltageMib_M.MinVoltageMt, System.Convert.ToInt32(threshold.MinVoltageMT * 1000f));
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonOutletCurrentMib_M.CurrentUpperCritical, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			if (threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(eatonOutletVoltageMib_M.VoltageUpperCritical, System.Convert.ToInt32(threshold.MaxVoltageMT * 1000f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetBankThresholdVariablesEatonPDU_M2(BankThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			int bankNumber = threshold.BankNumber;
			EatonGroupEntryMib_M2 eatonGroupEntryMib_M = new EatonGroupEntryMib_M2(bankNumber);
			EatonGroupCurrentMib_M2 eatonGroupCurrentMib_M = new EatonGroupCurrentMib_M2(bankNumber);
			if (!string.IsNullOrEmpty(threshold.BankName))
			{
				leafVarBinding.Add(eatonGroupEntryMib_M.GroupName, threshold.BankName);
			}
			else
			{
				leafVarBinding.Add(eatonGroupEntryMib_M.GroupName, "/empty");
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonGroupCurrentMib_M.MaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(eatonGroupCurrentMib_M.CurrentUpperCritical, System.Convert.ToInt32(threshold.MaxCurrentMT * 1000f));
			}
			return leafVarBinding;
		}
	}
}
