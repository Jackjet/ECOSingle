using System;
using System.Collections.Generic;
using System.Net;
namespace EcoDevice.AccessAPI
{
	internal static class AutoMibParser
	{
		public static LeafVarBinding SetDevicePOPSettingsVariables(DevicePOPSettings popSettings, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			if (modelcfg.popReading == Constant.PopReadingYes)
			{
				if (modelcfg.popNewRule == Constant.PopNewRuleYes)
				{
					if (modelcfg.perportreading == Constant.YES)
					{
						if (popSettings.PopModeOutlet > 0)
						{
							leafVarBinding.Add(DeviceEnvironmentMib.PopModeOutlet, System.Convert.ToInt32(popSettings.PopModeOutlet));
						}
						if (popSettings.PopModeLIFO > 0)
						{
							leafVarBinding.Add(DeviceEnvironmentMib.PopModeLIFO, System.Convert.ToInt32(popSettings.PopModeLIFO));
						}
						if (popSettings.PopModePriority > 0)
						{
							leafVarBinding.Add(DeviceEnvironmentMib.PopModePriority, System.Convert.ToInt32(popSettings.PopModePriority));
						}
					}
					else
					{
						if (popSettings.PopModePriority > 0)
						{
							leafVarBinding.Add(DeviceEnvironmentMib.PopModePriority, System.Convert.ToInt32(popSettings.PopModePriority));
						}
					}
					if (modelcfg.popPrioritySupport == Constant.YES)
					{
						leafVarBinding.Add(DeviceEnvironmentMib.PopPriorityList, popSettings.PopPriorityList);
					}
				}
				else
				{
					if (popSettings.PopEnableMode > 0)
					{
						leafVarBinding.Add(DeviceEnvironmentMib.PopEnableMode, System.Convert.ToInt32(popSettings.PopEnableMode));
					}
					if (popSettings.PopThreshold != -500f)
					{
						leafVarBinding.Add(DeviceEnvironmentMib.PopThreshold, System.Convert.ToInt32(popSettings.PopThreshold * 10f));
					}
				}
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetDeviceThresholdVariables(DeviceThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt && threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt && threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt && threshold.MaxPowerMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt && threshold.MaxPowerDissMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MaxPowerDissMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
			}
			if ((modelcfg.minDevThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt && threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MinVoltageMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
			}
			if ((modelcfg.minDevThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt && threshold.MinCurrentMT != -500f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.MinCurrentMt, System.Convert.ToInt32(threshold.MinCurrentMT * 10f));
			}
			if (modelcfg.doorReading == Constant.DoorSensorYes && threshold.DoorSensorType != -500)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.DoorSensorType, System.Convert.ToInt32(threshold.DoorSensorType));
			}
			if (modelcfg.modelName.Equals("EC1000") && threshold.DevReferenceVoltage != -500f && threshold.DevReferenceVoltage != -300f)
			{
				leafVarBinding.Add(DeviceEnvironmentMib.devReferenceVoltage, threshold.DevReferenceVoltage.ToString());
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetSensorThresholdVariables(SensorThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			SensorEnvironmentMib sensorEnvironmentMib = new SensorEnvironmentMib(threshold.SensorNumber);
			if (threshold.MaxHumidityMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MaxHumidityMt, System.Convert.ToInt32(threshold.MaxHumidityMT * 10f));
			}
			if (threshold.MaxPressMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MaxPressMt, System.Convert.ToInt32(threshold.MaxPressMT * 10f));
			}
			if (threshold.MaxTemperatureMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MaxTemperatureMt, System.Convert.ToInt32(threshold.MaxTemperatureMT * 10f));
			}
			if (threshold.MinHumidityMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MinHumidityMt, System.Convert.ToInt32(threshold.MinHumidityMT * 10f));
			}
			if (threshold.MinPressMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MinPressMt, System.Convert.ToInt32(threshold.MinPressMT * 10f));
			}
			if (threshold.MinTemperatureMT != -500f)
			{
				leafVarBinding.Add(sensorEnvironmentMib.MinTemperatureMt, System.Convert.ToInt32(threshold.MinTemperatureMT * 10f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetOutletThresholdVariables(OutletThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			OutletEnvironmentMib outletEnvironmentMib = new OutletEnvironmentMib(threshold.OutletNumber);
			if (!string.IsNullOrEmpty(threshold.OutletName))
			{
				leafVarBinding.Add(outletEnvironmentMib.OutletName, threshold.OutletName);
			}
			else
			{
				leafVarBinding.Add(outletEnvironmentMib.OutletName, "/empty");
			}
			if (modelcfg.switchable == 2 && modelcfg.isOutletSwitchable(threshold.OutletNumber - 1))
			{
				if (!string.IsNullOrEmpty(threshold.MacAddress))
				{
					leafVarBinding.Add(outletEnvironmentMib.MacAddress, threshold.MacAddress);
				}
				if ((float)threshold.OffDelayTime != -500f)
				{
					leafVarBinding.Add(outletEnvironmentMib.OffDelayTime, threshold.OffDelayTime);
				}
				if ((float)threshold.OnDelayTime != -500f)
				{
					leafVarBinding.Add(outletEnvironmentMib.OnDelayTime, threshold.OnDelayTime);
				}
				leafVarBinding.Add(outletEnvironmentMib.Confirmation, System.Convert.ToInt32(threshold.Confirmation));
				leafVarBinding.Add(outletEnvironmentMib.ShutdownMethod, System.Convert.ToInt32(threshold.ShutdownMethod));
			}
			if ((modelcfg.maxPortThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt && threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MaxCurrentMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
			}
			if ((modelcfg.maxPortThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt && threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MaxVoltageMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
			}
			if ((modelcfg.maxPortThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt && threshold.MaxPowerMT != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MaxPowerMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
			}
			if ((modelcfg.maxPortThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt && threshold.MaxPowerDissMT != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MaxPowerDissMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
			}
			if ((modelcfg.minPortThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt && threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MinVoltageMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
			}
			if ((modelcfg.minPortThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt && threshold.MinCurrentMt != -500f)
			{
				leafVarBinding.Add(outletEnvironmentMib.MinCurrentMt, System.Convert.ToInt32(threshold.MinCurrentMt * 10f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetBankThresholdVariables(BankThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			BankEnvironmentMib bankEnvironmentMib = new BankEnvironmentMib(threshold.BankNumber);
			if (!string.IsNullOrEmpty(threshold.BankName))
			{
				leafVarBinding.Add(bankEnvironmentMib.BankName, threshold.BankName);
			}
			else
			{
				leafVarBinding.Add(bankEnvironmentMib.BankName, "/empty");
			}
			if ((modelcfg.maxBankThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt && threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMaxCurMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
			}
			if ((modelcfg.maxBankThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt && threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMaxVolMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
			}
			if ((modelcfg.maxBankThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt && threshold.MaxPowerMT != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMaxPMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
			}
			if ((modelcfg.maxBankThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt && threshold.MaxPowerDissMT != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMaxPDMT, System.Convert.ToInt32(threshold.MaxPowerDissMT * 10f));
			}
			if ((modelcfg.minBankThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt && threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMinVolMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
			}
			if ((modelcfg.minBankThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt && threshold.MinCurrentMt != -500f)
			{
				leafVarBinding.Add(bankEnvironmentMib.BankMinCurMT, System.Convert.ToInt32(threshold.MinCurrentMt * 10f));
			}
			return leafVarBinding;
		}
		public static LeafVarBinding SetLineThresholdVariables(LineThreshold threshold, DevModelConfig modelcfg)
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			LineEnvironmentMib lineEnvironmentMib = new LineEnvironmentMib(threshold.LineNumber);
			if ((modelcfg.maxLineThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt && threshold.MaxCurrentMT != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMaxCurMT, System.Convert.ToInt32(threshold.MaxCurrentMT * 10f));
			}
			if ((modelcfg.maxLineThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt && threshold.MaxVoltageMT != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMaxVolMT, System.Convert.ToInt32(threshold.MaxVoltageMT * 10f));
			}
			if ((modelcfg.maxLineThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt && threshold.MaxPowerMT != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMaxPMT, System.Convert.ToInt32(threshold.MaxPowerMT * 10f));
			}
			if ((modelcfg.minLineThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt && threshold.MinVoltageMT != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMinVolMT, System.Convert.ToInt32(threshold.MinVoltageMT * 10f));
			}
			if ((modelcfg.minLineThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt && threshold.MinCurrentMt != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMinCurMT, System.Convert.ToInt32(threshold.MinCurrentMt * 10f));
			}
			if ((modelcfg.minLineThresholdOpt & Constant.MinPowOpt) == Constant.MinPowOpt && threshold.MinPowerMT != -500f)
			{
				leafVarBinding.Add(lineEnvironmentMib.LineMinPMT, System.Convert.ToInt32(threshold.MinPowerMT * 10f));
			}
			return leafVarBinding;
		}
		public static System.Collections.Generic.List<VarBinding> GetThresholdsRequest(DevModelConfig modelcfg)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum = modelcfg.bankNum;
			int lineNum = modelcfg.lineNum;
			int swichable = modelcfg.switchable;
			int perPortReading = modelcfg.perportreading;
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.AutoBasicInfoOID);
			leafVarBinding.Add(DeviceBaseMib.AutoRatingInfoOID);
			list.Add(leafVarBinding);
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			leafVarBinding2.Add(DeviceBaseMib.Mac);
			leafVarBinding2.Add(DeviceBaseMib.FWversion);
			leafVarBinding2.Add(DeviceBaseMib.DeviceName);
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MaxCurrentMT);
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MaxVoltageMT);
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MaxPowerMT);
			}
			if ((modelcfg.maxDevThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MaxPowerDissMT);
			}
			if ((modelcfg.minDevThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MinVoltageMT);
			}
			if ((modelcfg.minDevThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.MinCurrentMt);
			}
			if (modelcfg.doorReading == Constant.DoorSensorYes)
			{
				leafVarBinding2.Add(DeviceEnvironmentMib.DoorSensorType);
			}
			list.Add(leafVarBinding2);
			if (modelcfg.popReading == Constant.PopReadingYes)
			{
				LeafVarBinding leafVarBinding3 = new LeafVarBinding();
				if (modelcfg.popNewRule == Constant.PopNewRuleYes)
				{
					if (modelcfg.perportreading == Constant.YES)
					{
						leafVarBinding3.Add(DeviceEnvironmentMib.PopModeOutlet);
						leafVarBinding3.Add(DeviceEnvironmentMib.PopModeLIFO);
						leafVarBinding3.Add(DeviceEnvironmentMib.PopModePriority);
						list.Add(leafVarBinding3);
					}
					else
					{
						leafVarBinding3.Add(DeviceEnvironmentMib.PopModePriority);
						list.Add(leafVarBinding3);
					}
					if (modelcfg.popPrioritySupport == Constant.YES)
					{
						leafVarBinding3.Add(DeviceEnvironmentMib.PopPriorityList);
					}
				}
				else
				{
					leafVarBinding3.Add(DeviceEnvironmentMib.PopEnableMode);
					leafVarBinding3.Add(DeviceEnvironmentMib.PopThreshold);
					list.Add(leafVarBinding3);
				}
			}
			LeafVarBinding leafVarBinding4 = new LeafVarBinding();
			for (int i = 1; i <= sensorNum; i++)
			{
				SensorEnvironmentMib sensorEnvironmentMib = new SensorEnvironmentMib(i);
				leafVarBinding4.Add(sensorEnvironmentMib.MaxHumidityMt);
				leafVarBinding4.Add(sensorEnvironmentMib.MaxPressMt);
				leafVarBinding4.Add(sensorEnvironmentMib.MaxTemperatureMt);
				leafVarBinding4.Add(sensorEnvironmentMib.MinHumidityMt);
				leafVarBinding4.Add(sensorEnvironmentMib.MinPressMt);
				leafVarBinding4.Add(sensorEnvironmentMib.MinTemperatureMt);
			}
			if (sensorNum > 0)
			{
				list.Add(leafVarBinding4);
			}
			int num = bankNum / 4;
			int num2 = bankNum % 4;
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
					LeafVarBinding leafVarBinding5 = new LeafVarBinding();
					for (int k = 4 * j + 1; k <= 4 * j + 4; k++)
					{
						BankEnvironmentMib bankEnvironmentMib = new BankEnvironmentMib(k);
						leafVarBinding5.Add(bankEnvironmentMib.BankName);
						if ((modelcfg.maxBankThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMaxCurMT);
						}
						if ((modelcfg.maxBankThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMaxVolMT);
						}
						if ((modelcfg.maxBankThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMaxPMT);
						}
						if ((modelcfg.maxBankThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMaxPDMT);
						}
						if ((modelcfg.minBankThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMinCurMT);
						}
						if ((modelcfg.minBankThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt)
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankMinVolMT);
						}
						if (modelcfg.modelName.Equals("EC1000"))
						{
							leafVarBinding5.Add(bankEnvironmentMib.BankVoltageValue);
						}
					}
					list.Add(leafVarBinding5);
				}
			}
			LeafVarBinding leafVarBinding6 = new LeafVarBinding();
			for (int l = num * 4 + 1; l <= num * 4 + num2; l++)
			{
				BankEnvironmentMib bankEnvironmentMib2 = new BankEnvironmentMib(l);
				leafVarBinding6.Add(bankEnvironmentMib2.BankName);
				if ((modelcfg.maxBankThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMaxCurMT);
				}
				if ((modelcfg.maxBankThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMaxVolMT);
				}
				if ((modelcfg.maxBankThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMaxPMT);
				}
				if ((modelcfg.maxBankThresholdOpt & Constant.MaxPdpOpt) == Constant.MaxPdpOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMaxPDMT);
				}
				if ((modelcfg.minBankThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMinCurMT);
				}
				if ((modelcfg.minBankThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt)
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankMinVolMT);
				}
				if (modelcfg.modelName.Equals("EC1000"))
				{
					leafVarBinding6.Add(bankEnvironmentMib2.BankVoltageValue);
				}
			}
			if (num2 > 0)
			{
				list.Add(leafVarBinding6);
			}
			LeafVarBinding leafVarBinding7 = new LeafVarBinding();
			for (int m = 1; m <= lineNum; m++)
			{
				LineEnvironmentMib lineEnvironmentMib = new LineEnvironmentMib(m);
				if ((modelcfg.maxLineThresholdOpt & Constant.MaxCurOpt) == Constant.MaxCurOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMaxCurMT);
				}
				if ((modelcfg.maxLineThresholdOpt & Constant.MaxVolOpt) == Constant.MaxVolOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMaxVolMT);
				}
				if ((modelcfg.maxLineThresholdOpt & Constant.MaxPowOpt) == Constant.MaxPowOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMaxPMT);
				}
				if ((modelcfg.minLineThresholdOpt & Constant.MinCurOpt) == Constant.MinCurOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMinCurMT);
				}
				if ((modelcfg.minLineThresholdOpt & Constant.MinVolOpt) == Constant.MinVolOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMinVolMT);
				}
				if ((modelcfg.minLineThresholdOpt & Constant.MinPowOpt) == Constant.MinPowOpt)
				{
					leafVarBinding7.Add(lineEnvironmentMib.LineMinPMT);
				}
			}
			if (lineNum > 0)
			{
				list.Add(leafVarBinding7);
			}
			if (portNum > 0)
			{
				int num3 = 1;
				if (swichable == Constant.YES)
				{
					num3 += 5;
				}
				if (perPortReading == Constant.YES)
				{
					num3 += 6;
				}
				LeafVBBuilder leafVBBuilder = new LeafVBBuilder(num3, portNum);
				System.Collections.Generic.List<LeafVarBinding> list2 = new System.Collections.Generic.List<LeafVarBinding>();
				leafVBBuilder.BuildVbByIndex(list2, delegate(int index, LeafVarBinding leafVb)
				{
					OutletEnvironmentMib outletEnvironmentMib = new OutletEnvironmentMib(index);
					leafVb.Add(outletEnvironmentMib.OutletName);
					if (swichable == Constant.YES)
					{
						leafVb.Add(outletEnvironmentMib.Confirmation);
						leafVb.Add(outletEnvironmentMib.MacAddress);
						leafVb.Add(outletEnvironmentMib.OffDelayTime);
						leafVb.Add(outletEnvironmentMib.OnDelayTime);
						leafVb.Add(outletEnvironmentMib.ShutdownMethod);
					}
					if (perPortReading == Constant.YES)
					{
						leafVb.Add(outletEnvironmentMib.MaxCurrentMT);
						leafVb.Add(outletEnvironmentMib.MaxPowerDissMT);
						leafVb.Add(outletEnvironmentMib.MaxPowerMT);
						leafVb.Add(outletEnvironmentMib.MaxVoltageMT);
						leafVb.Add(outletEnvironmentMib.MinCurrentMt);
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
		public static ThresholdMessage GetThresholdMessage(DevModelConfig modelcfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			ThresholdMessage thresholdMessage = new ThresholdMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary5 = new System.Collections.Generic.Dictionary<string, string>();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if (current.StartsWith(DeviceEnvironmentMib.Entry) || current.StartsWith(DeviceEnvironmentMib.PopEnableMode) || current.StartsWith(DeviceEnvironmentMib.PopThreshold) || current.StartsWith(DeviceEnvironmentMib.DoorSensorType) || current.StartsWith(DeviceEnvironmentMib.PopModeOutlet) || current.StartsWith(DeviceEnvironmentMib.PopModeLIFO) || current.StartsWith(DeviceEnvironmentMib.PopModePriority) || current.StartsWith(DeviceEnvironmentMib.PopPriorityList))
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
							if (current.StartsWith(BankEnvironmentMib.Entry) || current.StartsWith(BankEnvironmentMib.valueEntry))
							{
								dictionary4.Add(current, text);
							}
							else
							{
								if (current.StartsWith(LineEnvironmentMib.Entry))
								{
									dictionary5.Add(current, text);
								}
								else
								{
									if (current.StartsWith(DeviceBaseMib.Mac))
									{
										thresholdMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
									}
									else
									{
										if (current.StartsWith(DeviceBaseMib.FWversion))
										{
											thresholdMessage.DeviceFWVer = text;
										}
										else
										{
											if (current.StartsWith(DeviceBaseMib.AutoBasicInfoOID))
											{
												thresholdMessage.AutoBasicInfo = (text.Equals("Null") ? string.Empty : text);
											}
											else
											{
												if (current.StartsWith(DeviceBaseMib.AutoRatingInfoOID))
												{
													thresholdMessage.AutoRatingInfo = (text.Equals("Null") ? string.Empty : text);
												}
												else
												{
													if (current.StartsWith(DeviceBaseMib.DeviceName))
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
					}
				}
			}
			string s = "N/A";
			float devReferenceVoltage = 0f;
			if (dictionary != null && dictionary.Count > 0)
			{
				thresholdMessage.DeviceThreshold = AutoMibParser.GetDeviceThreshold(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				thresholdMessage.OutletThreshold = AutoMibParser.GetOutletThreshold(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				thresholdMessage.SensorThreshold = AutoMibParser.GetSensorThreshold(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				thresholdMessage.BankThreshold = AutoMibParser.GetBankThreshold(dictionary4, ref s);
			}
			if (dictionary5 != null && dictionary5.Count > 0)
			{
				thresholdMessage.LineThreshold = AutoMibParser.GetLineThreshold(dictionary5);
			}
			if (modelcfg.modelName.Equals("EC1000"))
			{
				bool flag = float.TryParse(s, out devReferenceVoltage);
				if (flag)
				{
					thresholdMessage.DeviceThreshold.DevReferenceVoltage = devReferenceVoltage;
				}
				else
				{
					thresholdMessage.DeviceThreshold.DevReferenceVoltage = -300f;
				}
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
				if (current.StartsWith(DeviceEnvironmentMib.PopPriorityList))
				{
					deviceThreshold.PopPriorityList = text;
				}
				else
				{
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
											if (current.StartsWith(DeviceEnvironmentMib.MinVoltageMT))
											{
												deviceThreshold.MinVoltageMT = num;
											}
											else
											{
												if (current.StartsWith(DeviceEnvironmentMib.PopEnableMode))
												{
													deviceThreshold.PopEnableMode = (((int)num == -500) ? ((int)num) : System.Convert.ToInt32(text));
												}
												else
												{
													if (current.StartsWith(DeviceEnvironmentMib.PopThreshold))
													{
														deviceThreshold.PopThreshold = num;
													}
													else
													{
														if (current.StartsWith(DeviceEnvironmentMib.PopModeOutlet))
														{
															deviceThreshold.PopModeOutlet = (((int)num == -500) ? ((int)num) : System.Convert.ToInt32(text));
														}
														else
														{
															if (current.StartsWith(DeviceEnvironmentMib.PopModeLIFO))
															{
																deviceThreshold.PopModeLIFO = (((int)num == -500) ? ((int)num) : System.Convert.ToInt32(text));
															}
															else
															{
																if (current.StartsWith(DeviceEnvironmentMib.PopModePriority))
																{
																	deviceThreshold.PopModePriority = (((int)num == -500) ? ((int)num) : System.Convert.ToInt32(text));
																}
																else
																{
																	if (!current.StartsWith(DeviceEnvironmentMib.DoorSensorType))
																	{
																		return null;
																	}
																	deviceThreshold.DoorSensorType = System.Convert.ToInt32(text);
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
					else
					{
						if (text2.Equals("Null"))
						{
							text2 = System.Convert.ToString(-5000);
						}
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
					else
					{
						if (text2.Equals("Null"))
						{
							text2 = System.Convert.ToString(-5000);
						}
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
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, BankThreshold> GetBankThreshold(System.Collections.Generic.Dictionary<string, string> result, ref string devRefVoltage)
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
					else
					{
						if (text2.Equals("Null"))
						{
							text2 = System.Convert.ToString(-5000);
						}
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
					if (text.StartsWith(bankEnvironmentMib.BankMinCurMT))
					{
						bankThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text2) / 10f;
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
										if (text.StartsWith(bankEnvironmentMib.BankMaxPDMT))
										{
											bankThreshold.MaxPowerDissMT = (float)System.Convert.ToInt32(text2) / 10f;
										}
										else
										{
											if (!text.StartsWith(bankEnvironmentMib.BankVoltageValue))
											{
												return new System.Collections.Generic.Dictionary<int, BankThreshold>();
											}
											if (!text2.Equals("N/A"))
											{
												devRefVoltage = text2;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<int, LineThreshold> GetLineThreshold(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, LineThreshold> dictionary = new System.Collections.Generic.Dictionary<int, LineThreshold>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			LineEnvironmentMib lineEnvironmentMib = null;
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
					else
					{
						if (text2.Equals("Null"))
						{
							text2 = System.Convert.ToString(-5000);
						}
					}
				}
				if (text.LastIndexOf(".0") > 0)
				{
					text = text.Substring(0, text.LastIndexOf(".0"));
				}
				int num2 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
				if (!dictionary.ContainsKey(num2))
				{
					LineThreshold value = new LineThreshold(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					lineEnvironmentMib = new LineEnvironmentMib(num2);
					num = num2;
				}
				LineThreshold lineThreshold = dictionary[num2];
				if (text.StartsWith(lineEnvironmentMib.LineMinCurMT))
				{
					lineThreshold.MinCurrentMt = (float)System.Convert.ToInt32(text2) / 10f;
				}
				else
				{
					if (text.StartsWith(lineEnvironmentMib.LineMaxCurMT))
					{
						lineThreshold.MaxCurrentMT = (float)System.Convert.ToInt32(text2) / 10f;
					}
					else
					{
						if (text.StartsWith(lineEnvironmentMib.LineMinVolMT))
						{
							lineThreshold.MinVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
						}
						else
						{
							if (text.StartsWith(lineEnvironmentMib.LineMaxVolMT))
							{
								lineThreshold.MaxVoltageMT = (float)System.Convert.ToInt32(text2) / 10f;
							}
							else
							{
								if (text.StartsWith(lineEnvironmentMib.LineMaxPMT))
								{
									lineThreshold.MaxPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
								else
								{
									if (!text.StartsWith(lineEnvironmentMib.LineMinPMT))
									{
										return new System.Collections.Generic.Dictionary<int, LineThreshold>();
									}
									lineThreshold.MinPowerMT = (float)System.Convert.ToInt32(text2) / 10f;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.List<LeafVarBinding> GetValuesRequest(DevModelConfig modelcfg)
		{
			int portNum = modelcfg.portNum;
			int sensorNum = modelcfg.sensorNum;
			int bankNum2 = modelcfg.bankNum;
			int lineNum2 = modelcfg.lineNum;
			int switchable = modelcfg.switchable;
			int perportreading = modelcfg.perportreading;
			int perbankReading = modelcfg.perbankReading;
			int perlineReading = modelcfg.perlineReading;
			System.Collections.Generic.List<LeafVarBinding> list = new System.Collections.Generic.List<LeafVarBinding>();
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.Mac);
			if ((modelcfg.deviceMeasureOpt & Constant.CurBit) == Constant.CurBit)
			{
				leafVarBinding.Add(DeviceValueMib.Current);
			}
			if ((modelcfg.deviceMeasureOpt & Constant.VolBit) == Constant.VolBit)
			{
				leafVarBinding.Add(DeviceValueMib.Voltage);
			}
			if ((modelcfg.deviceMeasureOpt & Constant.PowBit) == Constant.PowBit)
			{
				leafVarBinding.Add(DeviceValueMib.Power);
			}
			if ((modelcfg.deviceMeasureOpt & Constant.PdpBit) == Constant.PdpBit)
			{
				leafVarBinding.Add(DeviceValueMib.PowerDissipation);
			}
			if (modelcfg.doorReading == Constant.DoorSensorYes)
			{
				leafVarBinding.Add(DeviceValueMib.DoorSensorStatus);
			}
			if (modelcfg.leakCurrent == Constant.YES)
			{
				leafVarBinding.Add(DeviceValueMib.LeakCurrentStatus);
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
			if (portNum > 0 && (switchable == Constant.YES || perportreading == Constant.YES))
			{
				if (switchable == Constant.YES)
				{
					for (int j = 1; j <= portNum; j++)
					{
						OutletStatusValueMib outletStatusValueMib = new OutletStatusValueMib(j);
						leafVarBinding2.Add(outletStatusValueMib.OutletStatus);
					}
				}
				if (perportreading == Constant.YES)
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
			if (bankNum2 > 0 && perbankReading == Constant.YES)
			{
				if ((modelcfg.bankStatusSupport & Constant.YES) == Constant.YES)
				{
					for (int k = 1; k <= bankNum2; k++)
					{
						BankStatusValueMib bankStatusValueMib = new BankStatusValueMib(k);
						leafVarBinding2.Add(bankStatusValueMib.BankStatus);
					}
				}
				if (modelcfg.bankMeasureOpt == 1u)
				{
					LeafVBBuilder leafVBBuilder2 = new LeafVBBuilder(1, bankNum2);
					leafVBBuilder2.BuildVbByIndex(list, delegate(int bankNum, LeafVarBinding leafVb)
					{
						BankValueMib bankValueMib = new BankValueMib(bankNum);
						leafVb.Add(bankValueMib.Current);
					});
				}
				else
				{
					if (modelcfg.bankMeasureOpt == 15u)
					{
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
				}
			}
			if (leafVarBinding2.VarBindings.Count > 0)
			{
				list.Add(leafVarBinding2);
			}
			if (lineNum2 > 0 && perlineReading == Constant.YES)
			{
				if (modelcfg.lineMeasureOpt == 1u)
				{
					LeafVBBuilder leafVBBuilder4 = new LeafVBBuilder(1, lineNum2);
					leafVBBuilder4.BuildVbByIndex(list, delegate(int lineNum, LeafVarBinding leafVb)
					{
						LineValueMib lineValueMib = new LineValueMib(lineNum);
						leafVb.Add(lineValueMib.Current);
					});
				}
				else
				{
					if (modelcfg.lineMeasureOpt == 7u)
					{
						LeafVBBuilder leafVBBuilder5 = new LeafVBBuilder(3, lineNum2);
						leafVBBuilder5.BuildVbByIndex(list, delegate(int lineNum, LeafVarBinding leafVb)
						{
							LineValueMib lineValueMib = new LineValueMib(lineNum);
							leafVb.Add(lineValueMib.Current);
							leafVb.Add(lineValueMib.Voltage);
							leafVb.Add(lineValueMib.Power);
						});
					}
				}
			}
			return list;
		}
		public static ValueMessage GetValueMessage(DevModelConfig modelCfg, System.Collections.Generic.Dictionary<string, string> result)
		{
			ValueMessage valueMessage = new ValueMessage();
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary4 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary5 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.Dictionary<string, string> dictionary6 = new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = result[current];
				if (current.StartsWith(DeviceValueMib.Entry) || current.StartsWith(DeviceValueMib.DoorSensorStatus) || current.StartsWith(DeviceValueMib.LeakCurrentStatus))
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
								if (current.StartsWith(LineValueMib.Entry))
								{
									dictionary5.Add(current, text);
								}
								else
								{
									if (current.StartsWith(EatonPDUCurrentMib.Entry))
									{
										dictionary6.Add(current, text);
									}
									else
									{
										if (current.StartsWith(DeviceBaseMib.Mac) || current.StartsWith(EatonPDUBaseMib.Mac))
										{
											valueMessage.DeviceReplyMac = text.Replace(" ", ":").Replace("-", ":");
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
				valueMessage.DeviceValue = AutoMibParser.GetDeviceValue(dictionary);
			}
			if (dictionary3 != null && dictionary3.Count > 0)
			{
				valueMessage.OutletValue = AutoMibParser.GetOutletValue(dictionary3);
			}
			if (dictionary2 != null && dictionary2.Count > 0)
			{
				valueMessage.SensorValue = AutoMibParser.GetSensorValue(dictionary2);
			}
			if (dictionary4 != null && dictionary4.Count > 0)
			{
				valueMessage.BankValue = AutoMibParser.GetBankValue(dictionary4);
			}
			if (dictionary5 != null && dictionary5.Count > 0)
			{
				valueMessage.LineValue = AutoMibParser.GetLineValue(dictionary5);
			}
			if (dictionary6 != null && dictionary6.Count > 0)
			{
				valueMessage.DeviceValue = AutoMibParser.GetCurrentValue(dictionary6);
				valueMessage.BankValue = AutoMibParser.GetEatonPDUBankValue(dictionary6);
			}
			if (modelCfg.doorReading == Constant.DoorSensorNo)
			{
				valueMessage.DeviceValue.DoorSensorStatus = -500;
			}
			if (modelCfg.modelName.Equals("EC1000") || modelCfg.modelName.Equals("EC2004"))
			{
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				if (valueMessage.BankValue != null && valueMessage.BankValue.Count > 0)
				{
					foreach (System.Collections.Generic.KeyValuePair<int, BankValueEntry> current2 in valueMessage.BankValue)
					{
						BankValueEntry value = current2.Value;
						if (!float.TryParse(value.Current, out num2))
						{
							value.Current = System.Convert.ToString(-1000);
						}
						if (!float.TryParse(value.Voltage, out num))
						{
							value.Voltage = System.Convert.ToString(-1000);
						}
						if (!float.TryParse(value.Power, out num3))
						{
							value.Power = System.Convert.ToString(-1000);
						}
					}
				}
			}
			return valueMessage;
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
		private static System.Collections.Generic.Dictionary<int, LineValueEntry> GetLineValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, LineValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, LineValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
			LineValueMib lineValueMib = null;
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
					LineValueEntry value = new LineValueEntry(num2);
					dictionary.Add(num2, value);
				}
				if (num != num2)
				{
					lineValueMib = new LineValueMib(num2);
					num = num2;
				}
				LineValueEntry lineValueEntry = dictionary[num2];
				if (text.StartsWith(lineValueMib.Current))
				{
					lineValueEntry.Current = text2;
				}
				else
				{
					if (text.StartsWith(lineValueMib.Voltage))
					{
						lineValueEntry.Voltage = text2;
					}
					else
					{
						if (text.StartsWith(lineValueMib.Power))
						{
							lineValueEntry.Power = text2;
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
				if ("\0".Equals(text2) || "n/a".Equals(text2) || "N/A".Equals(text2))
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
							else
							{
								if (current.StartsWith(DeviceValueMib.DoorSensorStatus))
								{
									deviceValueEntry.DoorSensorStatus = System.Convert.ToInt32(text);
								}
								else
								{
									if (current.StartsWith(DeviceValueMib.LeakCurrentStatus))
									{
										deviceValueEntry.LeakCurrentStatus = System.Convert.ToInt32(text);
									}
								}
							}
						}
					}
				}
			}
			return deviceValueEntry;
		}
		private static DeviceValueEntry GetCurrentValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			float num = 0f;
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
				int num2 = int.Parse(text);
				if (num2 >= 0)
				{
					num += (float)num2;
				}
			}
			deviceValueEntry.Current = (num / 10f).ToString();
			return deviceValueEntry;
		}
		private static System.Collections.Generic.Dictionary<int, BankValueEntry> GetEatonPDUBankValue(System.Collections.Generic.Dictionary<string, string> result)
		{
			System.Collections.Generic.Dictionary<int, BankValueEntry> dictionary = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
			System.Collections.Generic.IEnumerator<string> enumerator = result.Keys.GetEnumerator();
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
				int num = int.Parse(text2);
				if (num >= 0)
				{
					float num2 = (float)num;
					if (text.LastIndexOf(".0") > 0)
					{
						text = text.Substring(0, text.LastIndexOf(".0"));
					}
					int num3 = System.Convert.ToInt32(text.Substring(text.LastIndexOf(".") + 1));
					if (!dictionary.ContainsKey(num3))
					{
						BankValueEntry value = new BankValueEntry(num3);
						dictionary.Add(num3, value);
					}
					BankValueEntry bankValueEntry = dictionary[num3];
					bankValueEntry.Current = (num2 / 10f).ToString();
				}
			}
			return dictionary;
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
				int index = 1;
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
				leafVarBinding.Add(trapReceiverMib.configurationNotifyEnabled, 2);
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
	}
}
