using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	internal static class ApcMibParser
	{
		public static LeafVarBinding GetApcPDUPropertiesRequest()
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(ApcPDUBaseMib.DeviceName);
			leafVarBinding.Add(ApcPDUBaseMib.FWversion);
			leafVarBinding.Add(ApcPDUBaseMib.Mac);
			leafVarBinding.Add(ApcPDUBaseMib.ModelName);
			return leafVarBinding;
		}
		public static PropertiesMessage GetApcPDUPropertiesMessage(System.Collections.Generic.Dictionary<string, string> variables)
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
				if (current.StartsWith(ApcPDUBaseMib.DeviceName))
				{
					if ("\0".Equals(text) || string.IsNullOrEmpty(text))
					{
						text = string.Empty;
					}
					propertiesMessage.DeviceName = text;
				}
				else
				{
					if (current.StartsWith(ApcPDUBaseMib.FWversion))
					{
						text = text.Replace("v", "");
						text = text.Replace("V", "");
						propertiesMessage.FirwWareVersion = text;
					}
					else
					{
						if (current.StartsWith(ApcPDUBaseMib.Mac))
						{
							propertiesMessage.MacAddress = text.Replace(" ", ":").Replace("-", ":");
						}
						else
						{
							if (!current.StartsWith(ApcPDUBaseMib.ModelName))
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
		public static System.Collections.Generic.List<VarBinding> GetThresholdsRequestApcPDU(DevModelConfig modelcfg, DevRealConfig realcfg, int sensorNum)
		{
			int num = 1;
			int bankNum = modelcfg.bankNum;
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(ApcPDUBaseMib.Mac);
			leafVarBinding.Add(ApcPDUBaseMib.FWversion);
			leafVarBinding.Add(ApcPDUBaseMib.DeviceName);
			for (int i = 1; i <= num; i++)
			{
				ApcDeviceConfigMib apcDeviceConfigMib = new ApcDeviceConfigMib(i);
				leafVarBinding.Add(apcDeviceConfigMib.MinPowerConfig);
				leafVarBinding.Add(apcDeviceConfigMib.MaxPowerConfig);
				ApcPhaseConfigMib apcPhaseConfigMib = new ApcPhaseConfigMib(i);
				leafVarBinding.Add(apcPhaseConfigMib.MinCurrentConfig);
				leafVarBinding.Add(apcPhaseConfigMib.MaxCurrentConfig);
			}
			for (int j = 1; j <= bankNum; j++)
			{
				ApcBankConfigMib apcBankConfigMib = new ApcBankConfigMib(j);
				leafVarBinding.Add(apcBankConfigMib.MinCurrentConfig);
				leafVarBinding.Add(apcBankConfigMib.MaxCurrentConfig);
			}
			for (int k = 1; k <= sensorNum; k++)
			{
				ApcSensorConfigMib apcSensorConfigMib = new ApcSensorConfigMib(k);
				leafVarBinding.Add(apcSensorConfigMib.MaxTemperature);
				leafVarBinding.Add(apcSensorConfigMib.MinHumidity);
			}
			list.Add(leafVarBinding);
			return list;
		}
		public static ThresholdMessage GetThresholdMessageApcPDU(DevModelConfig modelcfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			ThresholdMessage thresholdMessage = new ThresholdMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if (current.StartsWith(ApcPDUBaseMib.Mac))
				{
					thresholdMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
				}
				else
				{
					if (current.StartsWith(ApcPDUBaseMib.FWversion))
					{
						text = text.Replace("v", "");
						text = text.Replace("V", "");
						thresholdMessage.DeviceFWVer = text;
					}
					else
					{
						if (current.StartsWith(ApcPDUBaseMib.DeviceName))
						{
							if ("\0".Equals(text) || string.IsNullOrEmpty(text))
							{
								text = string.Empty;
							}
							thresholdMessage.DeviceName = text;
						}
						else
						{
							if (current.StartsWith(ApcDeviceConfigMib.Entry) || current.StartsWith(ApcPhaseConfigMib.Entry))
							{
								dictionary.Add(current, text);
							}
							else
							{
								if (current.StartsWith(ApcBankConfigMib.Entry))
								{
									dictionary3.Add(current, text);
								}
								else
								{
									if (current.StartsWith(ApcSensorConfigMib.Entry))
									{
										dictionary2.Add(current, text);
									}
								}
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				thresholdMessage.DeviceThreshold = ApcMibParser.GetDeviceThreshold(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				thresholdMessage.BankThreshold = ApcMibParser.GetBankThreshold(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				thresholdMessage.SensorThreshold = ApcMibParser.GetSensorThreshold(dictionary2);
			}
			else
			{
				thresholdMessage.SensorThreshold = ApcMibParser.GetSensorThreshold_Empty(dictionary2);
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
					text = System.Convert.ToString(-10000);
				}
				else
				{
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-5000);
					}
					else
					{
						if (text.Equals("Null"))
						{
							text = System.Convert.ToString(-5000);
						}
					}
				}
				int num = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (num == 1)
				{
					ApcDeviceConfigMib apcDeviceConfigMib = new ApcDeviceConfigMib(num);
					ApcPhaseConfigMib apcPhaseConfigMib = new ApcPhaseConfigMib(num);
					if (current.StartsWith(apcDeviceConfigMib.MinPowerConfig))
					{
						deviceThreshold.MinPowerMT = (float)System.Convert.ToInt32(text) / 10f;
						deviceThreshold.MinPowerMT *= 1000f;
					}
					else
					{
						if (current.StartsWith(apcDeviceConfigMib.MaxPowerConfig))
						{
							deviceThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text) / 10f;
							deviceThreshold.MaxPowerMT *= 1000f;
						}
						else
						{
							if (current.StartsWith(apcPhaseConfigMib.MinCurrentConfig))
							{
								deviceThreshold.MinCurrentMT = (float)System.Convert.ToInt32(text);
							}
							else
							{
								if (current.StartsWith(apcPhaseConfigMib.MaxCurrentConfig))
								{
									deviceThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text);
								}
							}
						}
					}
				}
			}
			return deviceThreshold;
		}
		private static System.Collections.Generic.Dictionary<int, BankThreshold> GetBankThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, BankThreshold> dictionary = new System.Collections.Generic.Dictionary<int, BankThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			ApcBankConfigMib apcBankConfigMib = null;
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
					if (text == null || string.IsNullOrEmpty(text))
					{
						text = System.Convert.ToString(-500);
					}
				}
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					dictionary.Add(num2, new BankThreshold(num2)
					{
						BankName = "\r\n"
					});
				}
				if (num != num2)
				{
					apcBankConfigMib = new ApcBankConfigMib(num2);
					num = num2;
				}
				BankThreshold bankThreshold = dictionary[num2];
				if (current.StartsWith(apcBankConfigMib.MinCurrentConfig))
				{
					bankThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text);
				}
				else
				{
					if (current.StartsWith(apcBankConfigMib.MaxCurrentConfig))
					{
						bankThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text);
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorThreshold> GetSensorThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorThreshold> dictionary = new System.Collections.Generic.Dictionary<int, SensorThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			ApcSensorConfigMib apcSensorConfigMib = null;
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
					if (text == null || string.IsNullOrEmpty(text) || text.Equals("Null"))
					{
						text = System.Convert.ToString(-500);
					}
					else
					{
						if (text.Equals("-1"))
						{
							text = System.Convert.ToString(-600);
						}
					}
				}
				int num2 = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					SensorThreshold value = new SensorThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					apcSensorConfigMib = new ApcSensorConfigMib(num2);
					num = num2;
				}
				SensorThreshold sensorThreshold = dictionary[num2];
				if (current.StartsWith(apcSensorConfigMib.MaxTemperature))
				{
					sensorThreshold.MaxTemperatureMT = (float)System.Convert.ToInt32(text);
				}
				else
				{
					if (current.StartsWith(apcSensorConfigMib.MinHumidity))
					{
						sensorThreshold.MinHumidityMT = (float)System.Convert.ToInt32(text);
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorThreshold> GetSensorThreshold_Empty(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorThreshold> dictionary = new System.Collections.Generic.Dictionary<int, SensorThreshold>();
			int num = 1;
			SensorThreshold value = new SensorThreshold(num);
			dictionary.Add(num, value);
			SensorThreshold sensorThreshold = dictionary[num];
			string value2 = System.Convert.ToString(-600);
			sensorThreshold.MaxTemperatureMT = (float)System.Convert.ToInt32(value2);
			sensorThreshold.MinHumidityMT = (float)System.Convert.ToInt32(value2);
			return dictionary;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetValuesRequestApcPDU(DevModelConfig modelcfg, DevRealConfig realcfg, int sensorNum)
		{
			int num = 1;
			int bankNum = modelcfg.bankNum;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(ApcPDUBaseMib.Mac);
			for (int i = 1; i <= num; i++)
			{
				ApcDeviceStatusMib apcDeviceStatusMib = new ApcDeviceStatusMib(i);
				leafVarBinding.Add(apcDeviceStatusMib.PowerStatus);
				leafVarBinding.Add(apcDeviceStatusMib.PowerDsptStatus);
				ApcPhaseStatusMib apcPhaseStatusMib = new ApcPhaseStatusMib(i);
				leafVarBinding.Add(apcPhaseStatusMib.CurrentStatus);
				leafVarBinding.Add(apcPhaseStatusMib.VoltageStatus);
			}
			for (int j = 1; j <= bankNum; j++)
			{
				ApcBankStatusMib apcBankStatusMib = new ApcBankStatusMib(j);
				leafVarBinding.Add(apcBankStatusMib.CurrentStatus);
			}
			for (int k = 1; k <= sensorNum; k++)
			{
				ApcSensorStatusMib apcSensorStatusMib = new ApcSensorStatusMib(k);
				leafVarBinding.Add(apcSensorStatusMib.Temperature);
				leafVarBinding.Add(apcSensorStatusMib.Humidity);
			}
			list.Add(leafVarBinding);
			return list;
		}
		public static ValueMessage GetValueMessageApcPDU(DevModelConfig modelCfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			ValueMessage valueMessage = new ValueMessage();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if (current.StartsWith(ApcDeviceStatusMib.Entry) || current.StartsWith(ApcPhaseStatusMib.Entry))
				{
					dictionary.Add(current, text);
				}
				else
				{
					if (current.StartsWith(ApcBankStatusMib.Entry))
					{
						dictionary2.Add(current, text);
					}
					else
					{
						if (current.StartsWith(ApcPDUBaseMib.Mac))
						{
							valueMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
						}
						else
						{
							if (current.StartsWith(ApcSensorStatusMib.Entry))
							{
								dictionary3.Add(current, text);
							}
						}
					}
				}
			}
			if (dictionary != null && dictionary.Count > 0)
			{
				valueMessage.DeviceValue = ApcMibParser.GetDeviceValue(dictionary);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				valueMessage.BankValue = ApcMibParser.GetBankValue(dictionary2);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				valueMessage.SensorValue = ApcMibParser.GetSensorValue(dictionary3);
			}
			else
			{
				valueMessage.SensorValue = ApcMibParser.GetSensorValue_Empty(dictionary3);
			}
			return valueMessage;
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
				int num = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (num == 1)
				{
					ApcDeviceStatusMib apcDeviceStatusMib = new ApcDeviceStatusMib(num);
					ApcPhaseStatusMib apcPhaseStatusMib = new ApcPhaseStatusMib(num);
					System.Convert.ToInt32(text);
					if (current.StartsWith(apcDeviceStatusMib.PowerStatus))
					{
						deviceValueEntry.Power = System.Convert.ToString((float)System.Convert.ToInt32(text) / 100f * 1000f);
					}
					else
					{
						if (current.StartsWith(apcDeviceStatusMib.PowerDsptStatus))
						{
							deviceValueEntry.PowerDissipation = System.Convert.ToString((float)System.Convert.ToInt32(text) / 10f);
						}
						else
						{
							if (current.StartsWith(apcPhaseStatusMib.CurrentStatus))
							{
								deviceValueEntry.Current = System.Convert.ToString((float)System.Convert.ToInt32(text) / 10f);
							}
							else
							{
								if (current.StartsWith(apcPhaseStatusMib.VoltageStatus))
								{
									deviceValueEntry.Voltage = text;
								}
							}
						}
					}
				}
			}
			return deviceValueEntry;
		}
		private static System.Collections.Generic.Dictionary<int, BankValueEntry> GetBankValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, BankValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string value = result[current];
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
				int num = System.Convert.ToInt32(current.Substring(current.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num))
				{
					BankValueEntry value2 = new BankValueEntry(num);
					dictionary.Add(num, value2);
				}
				BankValueEntry bankValueEntry = dictionary[num];
				ApcBankStatusMib apcBankStatusMib = new ApcBankStatusMib(num);
				if (current.StartsWith(apcBankStatusMib.CurrentStatus))
				{
					bankValueEntry.Current = System.Convert.ToString((float)System.Convert.ToInt32(value) / 10f);
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorValueEntry> GetSensorValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			ApcSensorStatusMib apcSensorStatusMib = null;
			int num = 0;
			while (enumerator.MoveNext())
			{
				string text = enumerator.Current;
				string text2 = result[text];
				if ("\0".Equals(text2) || "n/a".Equals(text2) || "N/A".Equals(text2) || "-1".Equals(text2))
				{
					text2 = System.Convert.ToString(-1000);
				}
				else
				{
					if (string.IsNullOrEmpty(text2) || text2.Equals("Null"))
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
					apcSensorStatusMib = new ApcSensorStatusMib(num2);
					num = num2;
				}
				SensorValueEntry sensorValueEntry = dictionary[num2];
				if (text.StartsWith(apcSensorStatusMib.Humidity))
				{
					sensorValueEntry.Humidity = text2;
				}
				else
				{
					if (text.StartsWith(apcSensorStatusMib.Temperature))
					{
						if (text2.Equals(System.Convert.ToString(-1000)) || text2.Equals(System.Convert.ToString(-500)))
						{
							sensorValueEntry.Temperature = text2;
						}
						else
						{
							sensorValueEntry.Temperature = ((float)System.Convert.ToInt32(text2) / 10f).ToString();
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, SensorValueEntry> GetSensorValue_Empty(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, SensorValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
			int num = 1;
			SensorValueEntry sensorValueEntry = new SensorValueEntry(num);
			string text = System.Convert.ToString(-1000);
			sensorValueEntry.Humidity = text;
			sensorValueEntry.Temperature = text;
			dictionary.Add(num, sensorValueEntry);
			return dictionary;
		}
		public static System.Collections.Generic.List<LeafVarBinding> SetDeviceThresholdVariablesApcPDU(DeviceThreshold threshold, DevModelConfig modelcfg)
		{
			int inputNum = 1;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			ApcDeviceConfigMib apcDeviceConfigMib = new ApcDeviceConfigMib(inputNum);
			ApcPhaseConfigMib apcPhaseConfigMib = new ApcPhaseConfigMib(inputNum);
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			if (threshold.MaxPowerMT != -500f)
			{
				leafVarBinding.Add(apcDeviceConfigMib.MinPowerConfig, 0);
				leafVarBinding.Add(apcDeviceConfigMib.NearPowerConfig, 1);
				leafVarBinding2.Add(apcDeviceConfigMib.MaxPowerConfig, System.Convert.ToInt32(threshold.MaxPowerMT * 10f / 1000f));
				leafVarBinding2.Add(apcDeviceConfigMib.NearPowerConfig, System.Convert.ToInt32(threshold.MaxPowerMT * 10f / 1000f));
			}
			if (threshold.MinPowerMT != -500f)
			{
				leafVarBinding2.Add(apcDeviceConfigMib.MinPowerConfig, System.Convert.ToInt32(threshold.MinPowerMT * 10f / 1000f));
			}
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(apcPhaseConfigMib.MinCurrentConfig, 0);
				leafVarBinding.Add(apcPhaseConfigMib.NearCurrentConfig, 1);
				leafVarBinding2.Add(apcPhaseConfigMib.MaxCurrentConfig, System.Convert.ToInt32(threshold.MaxCurrentMT));
				leafVarBinding2.Add(apcPhaseConfigMib.NearCurrentConfig, System.Convert.ToInt32(threshold.MaxCurrentMT));
			}
			if (threshold.MinCurrentMT != -500f)
			{
				leafVarBinding2.Add(apcPhaseConfigMib.MinCurrentConfig, System.Convert.ToInt32(threshold.MinCurrentMT));
			}
			list.Add(leafVarBinding);
			list.Add(leafVarBinding2);
			return list;
		}
		public static System.Collections.Generic.List<LeafVarBinding> SetBankThresholdVariablesApcPDU(BankThreshold threshold, DevModelConfig modelcfg)
		{
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			int bankNumber = threshold.BankNumber;
			ApcBankConfigMib apcBankConfigMib = new ApcBankConfigMib(bankNumber);
			if (threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(apcBankConfigMib.MinCurrentConfig, 0);
				leafVarBinding.Add(apcBankConfigMib.NearCurrentConfig, 1);
				leafVarBinding2.Add(apcBankConfigMib.MaxCurrentConfig, System.Convert.ToInt32(threshold.MaxCurrentMT));
				leafVarBinding2.Add(apcBankConfigMib.NearCurrentConfig, System.Convert.ToInt32(threshold.MaxCurrentMT));
			}
			if (threshold.MinCurrentMt != -500f)
			{
				leafVarBinding2.Add(apcBankConfigMib.MinCurrentConfig, System.Convert.ToInt32(threshold.MinCurrentMt));
			}
			list.Add(leafVarBinding);
			list.Add(leafVarBinding2);
			return list;
		}
		public static System.Collections.Generic.List<LeafVarBinding> SetSensorThresholdVariablesApcPDU(SensorThreshold threshold, DevModelConfig modelcfg)
		{
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			int sensorNumber = threshold.SensorNumber;
			ApcSensorConfigMib apcSensorConfigMib = new ApcSensorConfigMib(sensorNumber);
			if (threshold.MaxTemperatureMT != -500f && threshold.MaxTemperatureMT != -600f)
			{
				leafVarBinding.Add(apcSensorConfigMib.HighTemperature, 0);
				leafVarBinding2.Add(apcSensorConfigMib.MaxTemperature, System.Convert.ToInt32(threshold.MaxTemperatureMT));
				leafVarBinding2.Add(apcSensorConfigMib.HighTemperature, System.Convert.ToInt32(threshold.MaxTemperatureMT) - 1);
			}
			if (threshold.MinHumidityMT != -500f && threshold.MinHumidityMT != -600f)
			{
				leafVarBinding.Add(apcSensorConfigMib.LowHumidity, 99);
				leafVarBinding2.Add(apcSensorConfigMib.MinHumidity, System.Convert.ToInt32(threshold.MinHumidityMT));
				leafVarBinding2.Add(apcSensorConfigMib.LowHumidity, System.Convert.ToInt32(threshold.MinHumidityMT) + 1);
			}
			list.Add(leafVarBinding);
			list.Add(leafVarBinding2);
			return list;
		}
	}
}
