using CommonAPI;
using DBAccessAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class AutoDetectLoader
	{
		private System.Collections.Generic.Dictionary<string, DevModelConfig> devList = new System.Collections.Generic.Dictionary<string, DevModelConfig>();
		private static AutoDetectLoader instance = new AutoDetectLoader();
		public static AutoDetectLoader GetInstance()
		{
			return AutoDetectLoader.instance;
		}
		public System.Collections.Generic.Dictionary<string, DevModelConfig> loadModelList()
		{
			System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> deviceDefine = DeviceOperation.GetDeviceDefine();
			foreach (System.Collections.Generic.Dictionary<string, string> current in deviceDefine)
			{
				string text = current["modelname"];
				string text2 = current["version"];
				string basicInfo = current["basic"];
				string ratingInfo = current["extra"];
				string key = text + "-" + text2;
				try
				{
					DevModelConfig value = this.parseModelConfig(text, text2, basicInfo, ratingInfo);
					this.devList[key] = value;
				}
				catch (System.Exception)
				{
				}
			}
			return this.devList;
		}
		public int updateModelConfig2Database(DBConn conn, string modelName, string fwVersion, string basicInfo, string ratingInfo, int opCode)
		{
			int result;
			if (opCode == 0)
			{
				result = DeviceOperation.InsertDeviceDefine(conn, modelName, fwVersion, basicInfo, ratingInfo);
			}
			else
			{
				result = DeviceOperation.UpdateDeviceDefine(conn, modelName, fwVersion, basicInfo, ratingInfo);
			}
			return result;
		}
		public DevModelConfig parseModelConfig(string modelName, string fwVersion, string basicInfo, string ratingInfo)
		{
			DevModelConfig result = new DevModelConfig(string.Empty);
			result.devThresholds.ThresholdsFlg = new System.Collections.Generic.List<ThresholdFlg>();
			result.devThresholds.UIEditFlg = new System.Collections.Generic.List<ThresholdFlg>();
			result.devThresholds.commonThresholds = default(DevCommonThreshold);
			result.devThresholds.currentThresholds = new System.Collections.Generic.List<stru_CommRange>();
			result.devThresholds.voltageThresholds = new System.Collections.Generic.List<stru_CommRange>();
			result.devThresholds.powerThresholds = new System.Collections.Generic.List<stru_CommRange>();
			result.devThresholds.powerDissThresholds = new System.Collections.Generic.List<stru_CommRange>();
			string[] array = basicInfo.Split(new char[]
			{
				';'
			});
			string[] array2 = ratingInfo.Split(new char[]
			{
				';'
			});
			result.commonThresholdFlag = Constant.AtenAutoDetect;
			result.FWvalidate = 0;
			result.FWnms = "n/a";
			result.FWext = "n/a";
			result.modelName = modelName;
			result.firmwareVer = fwVersion;
			result.autoBasicInfo = basicInfo;
			result.autoRatingInfo = ratingInfo;
			for (int i = 0; i < array.Length; i++)
			{
				string[] array3 = array[i].Split(new char[]
				{
					'='
				});
				if (array3[0].Equals("BN"))
				{
					result.bankNum = (int)System.Convert.ToInt16(array3[1]);
					if (result.bankNum == 1)
					{
						result.bankNum = 0;
					}
				}
				else
				{
					if (array3[0].Equals("ON"))
					{
						result.portNum = (int)System.Convert.ToInt16(array3[1]);
					}
					else
					{
						if (array3[0].Equals("SN"))
						{
							result.sensorNum = (int)System.Convert.ToInt16(array3[1]);
						}
						else
						{
							if (array3[0].Equals("LN"))
							{
								result.lineNum = (int)System.Convert.ToInt16(array3[1]);
							}
							else
							{
								if (array3[0].Equals("DMA"))
								{
									result.deviceMeasureOpt = System.Convert.ToUInt32(array3[1], 16);
								}
								else
								{
									if (array3[0].Equals("BMA"))
									{
										result.bankMeasureOpt = System.Convert.ToUInt32(array3[1], 16);
										if ((result.bankMeasureOpt & 15u) != 0u)
										{
											result.perbankReading = Constant.YES;
										}
										else
										{
											result.perbankReading = Constant.NO;
										}
									}
									else
									{
										if (array3[0].Equals("OMA"))
										{
											result.outletMeasureOpt = System.Convert.ToUInt32(array3[1], 16);
											if ((result.outletMeasureOpt & 15u) != 0u)
											{
												result.perportreading = Constant.YES;
											}
											else
											{
												result.perportreading = Constant.NO;
											}
										}
										else
										{
											if (array3[0].Equals("LMA"))
											{
												result.lineMeasureOpt = System.Convert.ToUInt32(array3[1], 16);
												if ((result.lineMeasureOpt & 15u) != 0u)
												{
													result.perlineReading = Constant.YES;
												}
												else
												{
													result.perlineReading = Constant.NO;
												}
												if (result.perlineReading == Constant.YES)
												{
													result.lineNum = 3;
												}
												else
												{
													result.lineNum = 0;
												}
											}
											else
											{
												if (array3[0].Equals("DNTH"))
												{
													result.minDevThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
												}
												else
												{
													if (array3[0].Equals("DXTH"))
													{
														result.maxDevThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
													}
													else
													{
														if (array3[0].Equals("BNTH"))
														{
															result.minBankThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
														}
														else
														{
															if (array3[0].Equals("BXTH"))
															{
																result.maxBankThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
															}
															else
															{
																if (array3[0].Equals("ONTH"))
																{
																	result.minPortThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
																}
																else
																{
																	if (array3[0].Equals("OXTH"))
																	{
																		result.maxPortThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
																	}
																	else
																	{
																		if (array3[0].Equals("LNTH"))
																		{
																			result.minLineThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
																		}
																		else
																		{
																			if (array3[0].Equals("LXTH"))
																			{
																				result.maxLineThresholdOpt = System.Convert.ToUInt32(array3[1], 16);
																			}
																			else
																			{
																				if (array3[0].Equals("MISC"))
																				{
																					uint num = System.Convert.ToUInt32(array3[1], 16);
																					if ((num & 1u) > 0u)
																					{
																						result.doorReading = Constant.YES;
																					}
																					else
																					{
																						result.doorReading = Constant.NO;
																					}
																					if ((num & 2u) > 0u)
																					{
																						result.popReading = Constant.YES;
																					}
																					else
																					{
																						result.popReading = Constant.NO;
																					}
																					if ((num & 8u) > 0u)
																					{
																						result.bankStatusSupport = Constant.YES;
																					}
																					else
																					{
																						result.bankStatusSupport = Constant.NO;
																					}
																					if ((num & 16u) > 0u)
																					{
																						result.popNewRule = Constant.YES;
																					}
																					else
																					{
																						result.popNewRule = Constant.NO;
																					}
																					if ((num & 32u) > 0u)
																					{
																						result.leakCurrent = Constant.YES;
																					}
																					else
																					{
																						result.leakCurrent = Constant.NO;
																					}
																					if ((num & 64u) > 0u)
																					{
																						result.popPrioritySupport = Constant.YES;
																					}
																					else
																					{
																						result.popPrioritySupport = Constant.NO;
																					}
																				}
																				else
																				{
																					if (array3[0].Equals("OLB"))
																					{
																						string[] array4 = array3[1].Split(new char[]
																						{
																							'#'
																						});
																						if (array4 != null && array4.Length > 1)
																						{
																							for (int j = 0; j < array4.Length; j++)
																							{
																								string[] array5 = array4[j].Split(new char[]
																								{
																									'~'
																								});
																								if (array5 != null && array5.Length > 1)
																								{
																									BankOutlets item = default(BankOutlets);
																									item.fromPort = System.Convert.ToInt32(array5[0]);
																									item.toPort = System.Convert.ToInt32(array5[1]);
																									result.bankOutlets.Add(item);
																								}
																							}
																						}
																					}
																					else
																					{
																						if (array3[0].Equals("DRA"))
																						{
																							uint num2 = System.Convert.ToUInt32(array3[1], 16);
																							if (num2 > 0u)
																							{
																								result.switchable = Constant.YES;
																							}
																							else
																							{
																								result.switchable = Constant.NO;
																							}
																						}
																						else
																						{
																							if (array3[0].Equals("BRA"))
																							{
																								result.bankCtrlflg = System.Convert.ToUInt32(array3[1], 16);
																								result.bankOpt_nameempty = 0u;
																							}
																							else
																							{
																								if (array3[0].Equals("ORA"))
																								{
																									result.switchableOutlets = System.Convert.ToUInt64(array3[1], 16);
																									if (result.switchableOutlets > 0uL)
																									{
																										result.switchable = Constant.YES;
																										result.outletStatusSupport = Constant.YES;
																									}
																									else
																									{
																										result.switchable = Constant.NO;
																										result.outletStatusSupport = Constant.NO;
																									}
																									result.killPowerDisableRebootOutlets = 0uL;
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
										}
									}
								}
							}
						}
					}
				}
			}
			if (basicInfo.IndexOf("BNTH=") < 0 && basicInfo.IndexOf("BXTH=") < 0)
			{
				result.minBankThresholdOpt = 0u;
				result.maxBankThresholdOpt = 0u;
			}
			if (basicInfo.IndexOf("ONTH=") < 0 && basicInfo.IndexOf("OXTH=") < 0)
			{
				result.minPortThresholdOpt = 0u;
				result.maxPortThresholdOpt = 0u;
			}
			if (basicInfo.IndexOf("LNTH=") < 0 && basicInfo.IndexOf("LXTH=") < 0)
			{
				result.minLineThresholdOpt = 0u;
				result.maxLineThresholdOpt = 0u;
			}
			if (basicInfo.IndexOf("BNTH=") < 0 || basicInfo.IndexOf("BXTH=") < 0 || basicInfo.IndexOf("ONTH=") < 0 || basicInfo.IndexOf("OXTH=") < 0)
			{
				DebugCenter.GetInstance().appendToFile(string.Concat(new string[]
				{
					"Info: ",
					modelName,
					"-",
					fwVersion,
					", BasicInfo: ",
					basicInfo
				}));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				string[] array6 = array2[k].Split(new char[]
				{
					'='
				});
				if (array6[0].Equals("DPC"))
				{
					result.devcapacity = array6[1];
				}
				else
				{
					if (array6[0].Equals("DAC"))
					{
						stru_CommRange item2 = default(stru_CommRange);
						item2.type = "dev";
						item2.id = "0";
						item2.range = array6[1];
						result.ampcapicity.Add(item2);
					}
					else
					{
						if (array6[0].Equals("BAC"))
						{
							stru_CommRange item3 = default(stru_CommRange);
							item3.type = "bank";
							item3.id = "1~" + result.bankNum;
							item3.range = array6[1];
							result.ampcapicity.Add(item3);
						}
						else
						{
							if (array6[0].Equals("LAC"))
							{
								stru_CommRange item4 = default(stru_CommRange);
								item4.type = "line";
								item4.id = "1~" + result.lineNum;
								item4.range = array6[1];
								result.ampcapicity.Add(item4);
							}
							else
							{
								if (array6[0].Equals("DCTR"))
								{
									stru_CommRange item5 = default(stru_CommRange);
									item5.type = "dev";
									item5.id = "0";
									item5.range = array6[1];
									result.devThresholds.currentThresholds.Add(item5);
								}
								else
								{
									if (array6[0].Equals("DVTR"))
									{
										result.devThresholds.commonThresholds.voltage = array6[1];
									}
									else
									{
										if (array6[0].Equals("DPTR"))
										{
											result.devThresholds.commonThresholds.power = array6[1];
										}
										else
										{
											if (array6[0].Equals("DPDTR"))
											{
												result.devThresholds.commonThresholds.powerDissipation = array6[1];
											}
											else
											{
												if (array6[0].Equals("STTR"))
												{
													result.devThresholds.commonThresholds.temperature = array6[1];
												}
												else
												{
													if (array6[0].Equals("SHTR"))
													{
														result.devThresholds.commonThresholds.humidity = array6[1];
													}
													else
													{
														if (array6[0].Equals("SPTR"))
														{
															result.devThresholds.commonThresholds.pressure = array6[1];
														}
														else
														{
															if (array6[0].Equals("BCTR"))
															{
																stru_CommRange item6 = default(stru_CommRange);
																item6.type = "bank";
																item6.id = "1~" + result.bankNum;
																item6.range = array6[1];
																result.devThresholds.currentThresholds.Add(item6);
																if (ratingInfo.IndexOf("LCTR=") < 0)
																{
																	stru_CommRange item7 = default(stru_CommRange);
																	item7.type = "line";
																	item7.id = "1~" + result.lineNum;
																	item7.range = array6[1];
																	result.devThresholds.currentThresholds.Add(item7);
																}
															}
															else
															{
																if (array6[0].Equals("BVTR"))
																{
																	stru_CommRange item8 = default(stru_CommRange);
																	item8.type = "bank";
																	item8.id = "1~" + result.bankNum;
																	item8.range = array6[1];
																	result.devThresholds.voltageThresholds.Add(item8);
																	if (ratingInfo.IndexOf("LVTR=") < 0)
																	{
																		stru_CommRange item9 = default(stru_CommRange);
																		item9.type = "line";
																		item9.id = "1~" + result.lineNum;
																		item9.range = array6[1];
																		result.devThresholds.voltageThresholds.Add(item9);
																	}
																}
																else
																{
																	if (array6[0].Equals("BPTR"))
																	{
																		stru_CommRange item10 = default(stru_CommRange);
																		item10.type = "bank";
																		item10.id = "1~" + result.bankNum;
																		item10.range = array6[1];
																		result.devThresholds.powerThresholds.Add(item10);
																		if (ratingInfo.IndexOf("LPTR=") < 0)
																		{
																			stru_CommRange item11 = default(stru_CommRange);
																			item11.type = "line";
																			item11.id = "1~" + result.lineNum;
																			item11.range = array6[1];
																			result.devThresholds.powerThresholds.Add(item11);
																		}
																	}
																	else
																	{
																		if (array6[0].Equals("BPDTR"))
																		{
																			stru_CommRange item12 = default(stru_CommRange);
																			item12.type = "bank";
																			item12.id = "1~" + result.bankNum;
																			item12.range = array6[1];
																			result.devThresholds.powerDissThresholds.Add(item12);
																		}
																		else
																		{
																			if (array6[0].Equals("OCTR"))
																			{
																				stru_CommRange item13 = default(stru_CommRange);
																				item13.type = "port";
																				item13.id = "1~" + result.portNum;
																				item13.range = array6[1];
																				result.devThresholds.currentThresholds.Add(item13);
																			}
																			else
																			{
																				if (array6[0].Equals("OVTR"))
																				{
																					stru_CommRange item14 = default(stru_CommRange);
																					item14.type = "port";
																					item14.id = "1~" + result.portNum;
																					item14.range = array6[1];
																					result.devThresholds.voltageThresholds.Add(item14);
																				}
																				else
																				{
																					if (array6[0].Equals("OPTR"))
																					{
																						stru_CommRange item15 = default(stru_CommRange);
																						item15.type = "port";
																						item15.id = "1~" + result.portNum;
																						item15.range = array6[1];
																						result.devThresholds.powerThresholds.Add(item15);
																					}
																					else
																					{
																						if (array6[0].Equals("OPDTR"))
																						{
																							stru_CommRange item16 = default(stru_CommRange);
																							item16.type = "port";
																							item16.id = "1~" + result.portNum;
																							item16.range = array6[1];
																							result.devThresholds.powerDissThresholds.Add(item16);
																						}
																						else
																						{
																							if (array6[0].Equals("LCTR"))
																							{
																								stru_CommRange item17 = default(stru_CommRange);
																								item17.type = "line";
																								item17.id = "1~" + result.lineNum;
																								item17.range = array6[1];
																								result.devThresholds.currentThresholds.Add(item17);
																							}
																							else
																							{
																								if (array6[0].Equals("LVTR"))
																								{
																									stru_CommRange item18 = default(stru_CommRange);
																									item18.type = "line";
																									item18.id = "1~" + result.lineNum;
																									item18.range = array6[1];
																									result.devThresholds.voltageThresholds.Add(item18);
																								}
																								else
																								{
																									if (array6[0].Equals("LPTR"))
																									{
																										stru_CommRange item19 = default(stru_CommRange);
																										item19.type = "line";
																										item19.id = "1~" + result.lineNum;
																										item19.range = array6[1];
																										result.devThresholds.powerThresholds.Add(item19);
																									}
																									else
																									{
																										if (array6[0].Equals("LPDTR"))
																										{
																											stru_CommRange item20 = default(stru_CommRange);
																											item20.type = "line";
																											item20.id = "1~" + result.lineNum;
																											item20.range = array6[1];
																											result.devThresholds.powerDissThresholds.Add(item20);
																										}
																										else
																										{
																											if (array6[0].Equals("PTHD"))
																											{
																												result.popDefault = System.Convert.ToInt32(array6[1]);
																											}
																											else
																											{
																												if (array6[0].Equals("PTHM"))
																												{
																													result.popUdefmax = System.Convert.ToInt32(array6[1]);
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
			int num3 = 0;
			if ((result.minDevThresholdOpt & 1u) == 0u)
			{
				num3 |= 1;
			}
			if ((result.minDevThresholdOpt & 2u) == 0u)
			{
				num3 |= 4;
			}
			if ((result.minDevThresholdOpt & 4u) == 0u)
			{
				num3 |= 16;
			}
			if ((result.minDevThresholdOpt & 8u) == 0u)
			{
				num3 |= 64;
			}
			if ((result.maxDevThresholdOpt & 1u) == 0u)
			{
				num3 |= 2;
			}
			if ((result.maxDevThresholdOpt & 2u) == 0u)
			{
				num3 |= 8;
			}
			if ((result.maxDevThresholdOpt & 4u) == 0u)
			{
				num3 |= 32;
			}
			if ((result.maxDevThresholdOpt & 8u) == 0u)
			{
				num3 |= 128;
			}
			ThresholdFlg item21 = default(ThresholdFlg);
			item21.type = "dev";
			item21.flg = num3;
			result.devThresholds.ThresholdsFlg.Add(item21);
			result.devThresholds.UIEditFlg.Add(item21);
			num3 = 0;
			if ((result.minBankThresholdOpt & 1u) == 0u)
			{
				num3 |= 1;
			}
			if ((result.minBankThresholdOpt & 2u) == 0u)
			{
				num3 |= 4;
			}
			if ((result.minBankThresholdOpt & 4u) == 0u)
			{
				num3 |= 16;
			}
			if ((result.minBankThresholdOpt & 8u) == 0u)
			{
				num3 |= 64;
			}
			if ((result.maxBankThresholdOpt & 1u) == 0u)
			{
				num3 |= 2;
			}
			if ((result.maxBankThresholdOpt & 2u) == 0u)
			{
				num3 |= 8;
			}
			if ((result.maxBankThresholdOpt & 4u) == 0u)
			{
				num3 |= 32;
			}
			if ((result.maxBankThresholdOpt & 8u) == 0u)
			{
				num3 |= 128;
			}
			ThresholdFlg item22 = default(ThresholdFlg);
			item22.type = "bank";
			item22.flg = num3;
			result.devThresholds.ThresholdsFlg.Add(item22);
			result.devThresholds.UIEditFlg.Add(item22);
			num3 = 0;
			if ((result.minPortThresholdOpt & 1u) == 0u)
			{
				num3 |= 1;
			}
			if ((result.minPortThresholdOpt & 2u) == 0u)
			{
				num3 |= 4;
			}
			if ((result.minPortThresholdOpt & 4u) == 0u)
			{
				num3 |= 16;
			}
			if ((result.minPortThresholdOpt & 8u) == 0u)
			{
				num3 |= 64;
			}
			if ((result.maxPortThresholdOpt & 1u) == 0u)
			{
				num3 |= 2;
			}
			if ((result.maxPortThresholdOpt & 2u) == 0u)
			{
				num3 |= 8;
			}
			if ((result.maxPortThresholdOpt & 4u) == 0u)
			{
				num3 |= 32;
			}
			if ((result.maxPortThresholdOpt & 8u) == 0u)
			{
				num3 |= 128;
			}
			ThresholdFlg item23 = default(ThresholdFlg);
			item23.type = "port";
			item23.flg = num3;
			result.devThresholds.ThresholdsFlg.Add(item23);
			result.devThresholds.UIEditFlg.Add(item23);
			num3 = 0;
			if ((result.minLineThresholdOpt & 1u) == 0u)
			{
				num3 |= 1;
			}
			if ((result.minLineThresholdOpt & 2u) == 0u)
			{
				num3 |= 4;
			}
			if ((result.minLineThresholdOpt & 4u) == 0u)
			{
				num3 |= 16;
			}
			if ((result.minLineThresholdOpt & 8u) == 0u)
			{
				num3 |= 64;
			}
			if ((result.maxLineThresholdOpt & 1u) == 0u)
			{
				num3 |= 2;
			}
			if ((result.maxLineThresholdOpt & 2u) == 0u)
			{
				num3 |= 8;
			}
			if ((result.maxLineThresholdOpt & 4u) == 0u)
			{
				num3 |= 32;
			}
			if ((result.maxLineThresholdOpt & 8u) == 0u)
			{
				num3 |= 128;
			}
			ThresholdFlg item24 = default(ThresholdFlg);
			item24.type = "line";
			item24.flg = num3;
			result.devThresholds.ThresholdsFlg.Add(item24);
			result.devThresholds.UIEditFlg.Add(item24);
			return result;
		}
	}
}
