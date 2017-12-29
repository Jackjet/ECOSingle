using System;
using System.Collections.Generic;
using System.Xml;
namespace EcoDevice.AccessAPI
{
	public class XMLParserAPI
	{
		private System.Collections.Generic.Dictionary<string, DevModelConfig> devList = new System.Collections.Generic.Dictionary<string, DevModelConfig>();
		public System.Collections.Generic.Dictionary<string, DevModelConfig> LoadDevInfoXml(string xmlDir, ref string m_Vesion, ref int m_maxZoneNum, ref int m_maxRackNum, ref int m_maxDevNum, ref bool m_supportISG, ref int m_ISGtimeout, ref bool m_supportBillprot, ref bool m_isdebuglogExport, ref int m_supportOEMDev, ref int m_runEnv_type, ref int m_runEnv_dbusage, ref int m_PeakPowerMethod, ref int m_MySQLUseMajorVersionOnly)
		{
			this.ReadDevModelInfo(xmlDir, ref m_Vesion, ref m_maxZoneNum, ref m_maxRackNum, ref m_maxDevNum, ref m_supportISG, ref m_ISGtimeout, ref m_supportBillprot, ref m_isdebuglogExport, ref m_supportOEMDev, ref m_runEnv_type, ref m_runEnv_dbusage, ref m_PeakPowerMethod, ref m_MySQLUseMajorVersionOnly);
			return this.devList;
		}
		public void clearDevList()
		{
			this.devList.Clear();
		}
		private void ReadDevModelInfo(string xmlDir, ref string m_Vesion, ref int m_maxZoneNum, ref int m_maxRackNum, ref int m_maxDevNum, ref bool m_supportISG, ref int m_ISGtimeout, ref bool m_supportBillprot, ref bool m_isdebuglogExport, ref int m_supportOEMDev, ref int m_runEnv_type, ref int m_runEnv_dbusage, ref int m_PeakPowerMethod, ref int m_MySQLUseMajorVersionOnly)
		{
			DevModelConfig devModelConfig = new DevModelConfig(string.Empty);
			DevCommonThreshold src = default(DevCommonThreshold);
			string text = "";
			XmlTextReader xmlTextReader = new XmlTextReader(xmlDir);
			while (xmlTextReader.Read())
			{
				XmlNodeType nodeType = xmlTextReader.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					if (xmlTextReader.Name.Equals("version"))
					{
						m_Vesion = xmlTextReader.GetAttribute("value");
					}
					else
					{
						if (xmlTextReader.Name.Equals("maxZoneNum"))
						{
							m_maxZoneNum = System.Convert.ToInt32(xmlTextReader.GetAttribute("value"));
						}
						else
						{
							if (xmlTextReader.Name.Equals("maxRackNum"))
							{
								m_maxRackNum = System.Convert.ToInt32(xmlTextReader.GetAttribute("value"));
							}
							else
							{
								if (xmlTextReader.Name.Equals("maxDevNum"))
								{
									m_maxDevNum = System.Convert.ToInt32(xmlTextReader.GetAttribute("value"));
								}
								else
								{
									if (xmlTextReader.Name.Equals("isdebuglogExportTRUE"))
									{
										m_isdebuglogExport = true;
									}
									else
									{
										if (xmlTextReader.Name.Equals("wantISGsupport"))
										{
											int num = 0;
											int num2 = 0;
											string attribute = xmlTextReader.GetAttribute("value");
											try
											{
												string[] array = attribute.Split(new char[]
												{
													','
												});
												int.TryParse(array[0], out num);
												int.TryParse(array[1], out num2);
												if (num == 13122)
												{
													m_supportISG = true;
													m_ISGtimeout = num2;
												}
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("wantBillProtsupport"))
										{
											int num = 0;
											string attribute2 = xmlTextReader.GetAttribute("value");
											try
											{
												int.TryParse(attribute2, out num);
												if (num == 1)
												{
													m_supportBillprot = true;
												}
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("supportOEMDev"))
										{
											int num = 0;
											string attribute3 = xmlTextReader.GetAttribute("value");
											try
											{
												int.TryParse(attribute3, out num);
												m_supportOEMDev = num;
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("PowerMethod"))
										{
											int num = 0;
											string attribute4 = xmlTextReader.GetAttribute("value");
											try
											{
												int.TryParse(attribute4, out num);
												m_PeakPowerMethod = num;
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("MySQLUseMajorVersionOnly"))
										{
											int num = 0;
											string attribute5 = xmlTextReader.GetAttribute("value");
											try
											{
												int.TryParse(attribute5, out num);
												m_MySQLUseMajorVersionOnly = num;
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("runenv"))
										{
											try
											{
												string attribute6 = xmlTextReader.GetAttribute("type");
												if (attribute6 != null)
												{
													int num;
													int.TryParse(attribute6, out num);
													m_runEnv_type = num;
												}
											}
											catch (System.Exception)
											{
											}
											try
											{
												string attribute7 = xmlTextReader.GetAttribute("dbusage");
												if (attribute7 != null)
												{
													int num;
													int.TryParse(attribute7, out num);
													m_runEnv_dbusage = num;
												}
												goto IL_35B;
											}
											catch (System.Exception)
											{
												goto IL_35B;
											}
										}
										if (xmlTextReader.Name.Equals("public"))
										{
											src = this.getCommonThresholds(xmlTextReader);
										}
										else
										{
											if (xmlTextReader.Name.Equals("device"))
											{
												devModelConfig = new DevModelConfig(string.Empty);
												devModelConfig.devThresholds.commonThresholds.copy(src);
												devModelConfig.modelName = xmlTextReader.GetAttribute("name");
												devModelConfig.modelName = devModelConfig.modelName.Trim();
											}
										}
									}
								}
							}
						}
					}
					IL_35B:
					text = xmlTextReader.Name;
					if (text.Equals("threshold"))
					{
						this.getThresholds(ref devModelConfig, xmlTextReader);
					}
					else
					{
						if (text.Equals("ampcapacity"))
						{
							stru_CommRange item = default(stru_CommRange);
							item.type = xmlTextReader.GetAttribute("type");
							item.id = xmlTextReader.GetAttribute("id");
							item.range = xmlTextReader.GetAttribute("range");
							devModelConfig.ampcapicity.Add(item);
						}
						else
						{
							if (text.Equals("fwupgrade"))
							{
								int num;
								if (int.TryParse(xmlTextReader.GetAttribute("validate"), out num))
								{
									devModelConfig.FWvalidate = num;
								}
								devModelConfig.FWnms = ((xmlTextReader.GetAttribute("nms") == null) ? "" : xmlTextReader.GetAttribute("nms"));
								devModelConfig.FWext = ((xmlTextReader.GetAttribute("ext") == null) ? "" : xmlTextReader.GetAttribute("ext"));
							}
						}
					}
					break;
				case XmlNodeType.Attribute:
					break;
				case XmlNodeType.Text:
					if (text.Equals("outletNumber"))
					{
						devModelConfig.portNum = System.Convert.ToInt32(xmlTextReader.Value);
					}
					else
					{
						if (text.Equals("sensorNumber"))
						{
							devModelConfig.sensorNum = System.Convert.ToInt32(xmlTextReader.Value);
						}
						else
						{
							if (text.Equals("bankNumber"))
							{
								this.getbankNumber(ref devModelConfig, xmlTextReader.Value);
							}
							else
							{
								if (text.Equals("bankOpt"))
								{
									this.getbankOpt(ref devModelConfig, xmlTextReader.Value);
								}
								else
								{
									if (text.Equals("outletCtrl"))
									{
										this.getoutletCtrl(ref devModelConfig, xmlTextReader.Value);
									}
									else
									{
										if (text.Equals("outletReading"))
										{
											devModelConfig.perportreadingOutlets = (ulong)System.Convert.ToUInt32(xmlTextReader.Value, 16);
											if (devModelConfig.perportreadingOutlets > 0uL)
											{
												devModelConfig.perportreading = 2;
											}
											else
											{
												devModelConfig.perportreading = 1;
											}
										}
										else
										{
											if (text.Equals("bankReading"))
											{
												this.getbankReading(ref devModelConfig, xmlTextReader.Value);
											}
											else
											{
												if (text.Equals("popReading"))
												{
													this.getPopInfo(ref devModelConfig, xmlTextReader.Value);
												}
												else
												{
													if (text.Equals("doorReading"))
													{
														this.getDoorInfo(ref devModelConfig, xmlTextReader.Value);
													}
													else
													{
														if (text.Equals("devcapacity"))
														{
															devModelConfig.devcapacity = xmlTextReader.Value;
														}
														else
														{
															if (text.Equals("commonThresholdFlag"))
															{
																devModelConfig.commonThresholdFlag = System.Convert.ToInt32(xmlTextReader.Value);
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
					break;
				default:
					if (nodeType == XmlNodeType.EndElement)
					{
						if (xmlTextReader.Name.Equals("device"))
						{
							string modelName = devModelConfig.modelName;
							string[] array2 = modelName.Split(new string[]
							{
								","
							}, System.StringSplitOptions.RemoveEmptyEntries);
							if (array2.Length == 1)
							{
								this.devList.Add(devModelConfig.modelName, devModelConfig);
							}
							else
							{
								for (int i = 0; i < array2.Length; i++)
								{
									array2[i] = array2[i].Trim();
									DevModelConfig value = new DevModelConfig(array2[i]);
									value.copy(devModelConfig);
									value.modelName = array2[i];
									this.devList.Add(value.modelName, value);
								}
							}
							devModelConfig = new DevModelConfig(string.Empty);
						}
					}
					break;
				}
			}
		}
		private DevCommonThreshold getCommonThresholds(XmlTextReader reader)
		{
			DevCommonThreshold result = default(DevCommonThreshold);
			while (reader.Read())
			{
				if (reader.Name.Equals("threshold"))
				{
					while (reader.Read())
					{
						XmlNodeType nodeType = reader.NodeType;
						if (nodeType != XmlNodeType.Element)
						{
							if (nodeType == XmlNodeType.EndElement)
							{
								if (reader.Name.Equals("public"))
								{
									return result;
								}
							}
						}
						else
						{
							if (reader.Name.Equals("voltage"))
							{
								result.voltage = reader.GetAttribute("range");
							}
							else
							{
								if (reader.Name.Equals("power"))
								{
									result.power = reader.GetAttribute("range");
								}
								else
								{
									if (reader.Name.Equals("powerDissipation"))
									{
										result.powerDissipation = reader.GetAttribute("range");
									}
									else
									{
										if (reader.Name.Equals("temperature"))
										{
											result.temperature = reader.GetAttribute("range");
										}
										else
										{
											if (reader.Name.Equals("humidity"))
											{
												result.humidity = reader.GetAttribute("range");
											}
											else
											{
												if (reader.Name.Equals("pressure"))
												{
													result.pressure = reader.GetAttribute("range");
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
			return result;
		}
		private void getThresholds(ref DevModelConfig devinfo, XmlTextReader reader)
		{
			devinfo.devThresholds.ThresholdsFlg = new System.Collections.Generic.List<ThresholdFlg>();
			devinfo.devThresholds.UIEditFlg = new System.Collections.Generic.List<ThresholdFlg>();
			devinfo.devThresholds.currentThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.voltageThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.powerThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.powerDissThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.tempThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.HumiThresholds = new System.Collections.Generic.List<stru_CommRange>();
			devinfo.devThresholds.PressThresholds = new System.Collections.Generic.List<stru_CommRange>();
			while (reader.Read())
			{
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.EndElement)
					{
						if (reader.Name.Equals("threshold"))
						{
							return;
						}
					}
				}
				else
				{
					if (reader.Name.Equals("readVflag"))
					{
						ThresholdFlg item = default(ThresholdFlg);
						item.type = reader.GetAttribute("type");
						item.flg = System.Convert.ToInt32(reader.GetAttribute("flag"), 16);
						devinfo.devThresholds.ThresholdsFlg.Add(item);
					}
					else
					{
						if (reader.Name.Equals("UIeditflag"))
						{
							ThresholdFlg item2 = default(ThresholdFlg);
							item2.type = reader.GetAttribute("type");
							item2.flg = System.Convert.ToInt32(reader.GetAttribute("flag"), 16);
							devinfo.devThresholds.UIEditFlg.Add(item2);
						}
						else
						{
							if (reader.Name.Equals("current"))
							{
								stru_CommRange item3 = default(stru_CommRange);
								item3.type = reader.GetAttribute("type");
								item3.id = reader.GetAttribute("id");
								item3.range = reader.GetAttribute("range");
								devinfo.devThresholds.currentThresholds.Add(item3);
							}
							else
							{
								if (reader.Name.Equals("voltage"))
								{
									stru_CommRange item4 = default(stru_CommRange);
									item4.type = reader.GetAttribute("type");
									item4.id = reader.GetAttribute("id");
									item4.range = reader.GetAttribute("range");
									devinfo.devThresholds.voltageThresholds.Add(item4);
								}
								else
								{
									if (reader.Name.Equals("power"))
									{
										stru_CommRange item5 = default(stru_CommRange);
										item5.type = reader.GetAttribute("type");
										item5.id = reader.GetAttribute("id");
										item5.range = reader.GetAttribute("range");
										devinfo.devThresholds.powerThresholds.Add(item5);
									}
									else
									{
										if (reader.Name.Equals("powerDissipation"))
										{
											stru_CommRange item6 = default(stru_CommRange);
											item6.type = reader.GetAttribute("type");
											item6.id = reader.GetAttribute("id");
											item6.range = reader.GetAttribute("range");
											devinfo.devThresholds.powerDissThresholds.Add(item6);
										}
										else
										{
											if (reader.Name.Equals("temperature"))
											{
												stru_CommRange item7 = default(stru_CommRange);
												item7.type = reader.GetAttribute("type");
												item7.id = reader.GetAttribute("id");
												item7.range = reader.GetAttribute("range");
												devinfo.devThresholds.tempThresholds.Add(item7);
											}
											else
											{
												if (reader.Name.Equals("humidity"))
												{
													stru_CommRange item8 = default(stru_CommRange);
													item8.type = reader.GetAttribute("type");
													item8.id = reader.GetAttribute("id");
													item8.range = reader.GetAttribute("range");
													devinfo.devThresholds.HumiThresholds.Add(item8);
												}
												else
												{
													if (reader.Name.Equals("pressure"))
													{
														stru_CommRange item9 = default(stru_CommRange);
														item9.type = reader.GetAttribute("type");
														item9.id = reader.GetAttribute("id");
														item9.range = reader.GetAttribute("range");
														devinfo.devThresholds.PressThresholds.Add(item9);
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
		private void getbankNumber(ref DevModelConfig devinfo, string str)
		{
			string[] array = str.Split(new char[]
			{
				'@'
			});
			devinfo.bankNum = System.Convert.ToInt32(array[0]);
			if (array.Length > 1)
			{
				devinfo.bankCtrlflg = System.Convert.ToUInt32(array[1], 16);
			}
			if (array.Length > 2)
			{
				string[] array2 = array[2].Split(new char[]
				{
					','
				});
				if (array2 != null && array2.Length > 1)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[]
						{
							'~'
						});
						if (array3 != null && array3.Length > 1)
						{
							BankOutlets item = default(BankOutlets);
							item.fromPort = System.Convert.ToInt32(array3[0]);
							item.toPort = System.Convert.ToInt32(array3[1]);
							devinfo.bankOutlets.Add(item);
						}
					}
				}
			}
		}
		private void getbankOpt(ref DevModelConfig devinfo, string str)
		{
			devinfo.bankOpt_nameempty = System.Convert.ToUInt32(str, 16);
		}
		private void getoutletCtrl(ref DevModelConfig devinfo, string str)
		{
			string[] array = str.Split(new char[]
			{
				'@'
			});
			devinfo.switchableOutlets = System.Convert.ToUInt64(array[0], 16);
			if (devinfo.switchableOutlets > 0uL)
			{
				devinfo.switchable = 2;
			}
			else
			{
				devinfo.switchable = 1;
			}
			if (array.Length > 1)
			{
				devinfo.killPowerDisableRebootOutlets = System.Convert.ToUInt64(array[1], 16);
			}
		}
		private void getPopInfo(ref DevModelConfig devinfo, string str)
		{
			string[] array = str.Split(new char[]
			{
				'@'
			});
			devinfo.popReading = System.Convert.ToInt32(array[0]) + 1;
			devinfo.popDefault = System.Convert.ToInt32(array[1]);
			devinfo.popUdefmax = System.Convert.ToInt32(array[2]);
		}
		private void getDoorInfo(ref DevModelConfig devinfo, string str)
		{
			devinfo.doorReading = System.Convert.ToInt32(str) + 1;
		}
		private void getbankReading(ref DevModelConfig devinfo, string str)
		{
			string[] array = str.Split(new char[]
			{
				'@'
			});
			devinfo.perbankReading = System.Convert.ToInt32(array[0]) + 1;
		}
	}
}
