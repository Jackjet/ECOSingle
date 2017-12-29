using DBAccessAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Xml;
namespace ecoProtocols
{
	public class BillingContext : ConnectContext, IDisposable
	{
		private const string errorResponse = "*2;000;00000000;2001;\n";
		protected int _Id
		{
			get;
			set;
		}
		protected ServerEndPoint LocalEndPoint
		{
			get;
			set;
		}
		public BillingContext()
		{
			if (this._receiveBuffer == null)
			{
				this._receiveBuffer = new byte[4096];
			}
		}
		public override List<byte[]> ReceivedBuffer(BufferState rState, int nSize, ref int nRequestCount)
		{
			List<byte[]> result;
			lock (this._lockContext)
			{
				nRequestCount = 0;
				if (this._receivedSize + nSize > this._receiveBuffer.Length)
				{
					Common.WriteLine("Buffer Overlapped", new string[0]);
					result = null;
				}
				else
				{
					List<byte[]> list = new List<byte[]>();
					Array.Copy(rState._buffer, 0, this._receiveBuffer, this._receivedSize, nSize);
					this._receivedSize += nSize;
					string text = Encoding.ASCII.GetString(this._receiveBuffer, 0, this._receivedSize);
					while (true)
					{
						text = Encoding.ASCII.GetString(this._receiveBuffer, 0, this._receivedSize);
						string text2 = text.ToLower();
						int num = text2.IndexOf("<bp");
						int num2 = text2.IndexOf("</bp>");
						if (num >= 0 && num2 < 0)
						{
							break;
						}
						if (num >= 0 && num2 >= 0 && num < num2)
						{
							nRequestCount++;
							text = Encoding.ASCII.GetString(this._receiveBuffer, num, num2 - num + 5);
							this._receivedSize -= num2 + 5;
							if (this._receivedSize > 0)
							{
								Array.Copy(this._receiveBuffer, num2 + 5, this._receiveBuffer, 0, this._receivedSize);
							}
						}
						else
						{
							if (num > 0)
							{
								this._receivedSize -= num;
								if (this._receivedSize > 0)
								{
									Array.Copy(this._receiveBuffer, num, this._receiveBuffer, 0, this._receivedSize);
								}
							}
							else
							{
								this._receivedSize = 0;
							}
							text = "";
						}
						if (text == string.Empty)
						{
							break;
						}
						string text3 = this.RequestParser(text);
						if (text3 != string.Empty)
						{
							byte[] bytes = Encoding.ASCII.GetBytes(text3);
							list.Add(bytes);
						}
					}
					result = list;
				}
			}
			return result;
		}
		private bool IsValidToken(string token)
		{
			return this._owner != null && this._owner.IsValidToken(token);
		}
		private string RequestParser(string bp)
		{
			string text = "";
			string text2 = "";
			Common.WriteLine("Billing Request: \r\n" + bp, new string[0]);
			string text3 = "<?xml version='1.0'?>\r\n";
			text3 += bp;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(new StringReader(text3));
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/bp");
			string text4 = "";
			if (xmlNode.Attributes.GetNamedItem("seq") != null)
			{
				text4 = xmlNode.Attributes.GetNamedItem("seq").Value;
			}
			if (xmlNode.Attributes.GetNamedItem("authorizes") != null)
			{
				text2 = xmlNode.Attributes.GetNamedItem("authorizes").Value;
			}
			text = string.Concat(new string[]
			{
				"<bp seq=\"",
				text4,
				"\" authorizes=\"",
				text2,
				"\">\r\n"
			});
			string text5 = "";
			string text6 = "";
			List<int> list = new List<int>();
			string text7 = "";
			for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = xmlNode.ChildNodes[i];
				if (string.Compare(xmlNode2.Name, "request") == 0)
				{
					string text8 = "";
					text5 = "";
					text6 = "";
					list.Clear();
					text7 = "";
					int num = 1;
					string text9 = "";
					if (xmlNode2.Attributes.GetNamedItem("cmd") != null)
					{
						text8 = xmlNode2.Attributes.GetNamedItem("cmd").Value;
					}
					if (!(text8 == string.Empty))
					{
						if (xmlNode2.Attributes.GetNamedItem("type") != null)
						{
							text5 = xmlNode2.Attributes.GetNamedItem("type").Value;
						}
						if (xmlNode2.Attributes.GetNamedItem("id") != null)
						{
							text6 = xmlNode2.Attributes.GetNamedItem("id").Value;
							string[] array = text6.Split(new char[]
							{
								','
							}, StringSplitOptions.RemoveEmptyEntries);
							if (array != null)
							{
								string[] array2 = array;
								for (int j = 0; j < array2.Length; j++)
								{
									string text10 = array2[j];
									if (text10 != string.Empty)
									{
										list.Add(int.Parse(text10));
									}
								}
							}
						}
						if (xmlNode2.Attributes.GetNamedItem("period") != null)
						{
							string value = xmlNode2.Attributes.GetNamedItem("period").Value;
							string[] array3 = value.Split(new char[]
							{
								','
							}, StringSplitOptions.RemoveEmptyEntries);
							if (array3.Length > 0)
							{
								text7 = array3[0].Trim();
							}
							num = 1;
							if (array3.Length > 1)
							{
								num = int.Parse(array3[1]);
							}
						}
						if (xmlNode2.Attributes.GetNamedItem("date") != null)
						{
							text9 = xmlNode2.Attributes.GetNamedItem("date").Value;
						}
						text8 = text8.ToUpper();
						if (text8 == "LIV")
						{
							text = "<reply cmd=\"LIV\"/>\r\n";
						}
						else
						{
							if (text8 == "ZON")
							{
								text += "<reply cmd=\"ZON\">\r\n";
								if (this.IsValidToken(text2))
								{
									ArrayList allZone = ZoneInfo.getAllZone();
									IEnumerator enumerator = allZone.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											ZoneInfo zoneInfo = (ZoneInfo)enumerator.Current;
											string str = string.Concat(new object[]
											{
												"<ZON id=\"",
												zoneInfo.ZoneID,
												"\" name=\"",
												zoneInfo.ZoneName,
												"\"/>\r\n"
											});
											text += str;
										}
										goto IL_3D5;
									}
									finally
									{
										IDisposable disposable = enumerator as IDisposable;
										if (disposable != null)
										{
											disposable.Dispose();
										}
									}
									goto IL_3C5;
								}
								goto IL_3C5;
								IL_3D5:
								text += "</reply>\r\n";
								goto IL_197C;
								IL_3C5:
								string str2 = "<ERR>wrong authorizes</ERR>\r\n";
								text += str2;
								goto IL_3D5;
							}
							if (text8 == "RAK")
							{
								ArrayList allRack = RackInfo.getAllRack();
								if (text5 == null || string.Compare(text5, "ZON", true) != 0)
								{
									text += "<reply cmd=\"RAK\" type=\"ALL\">\r\n";
									if (this.IsValidToken(text2))
									{
										IEnumerator enumerator = allRack.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												RackInfo rackInfo = (RackInfo)enumerator.Current;
												string str3 = string.Concat(new object[]
												{
													"<RAK id=\"",
													rackInfo.RackID,
													"\" name=\"",
													rackInfo.OriginalName,
													"\"/>\r\n"
												});
												text += str3;
											}
											goto IL_65C;
										}
										finally
										{
											IDisposable disposable = enumerator as IDisposable;
											if (disposable != null)
											{
												disposable.Dispose();
											}
										}
										goto IL_64C;
									}
									goto IL_64C;
									IL_65C:
									text += "</reply>\r\n";
									goto IL_197C;
									IL_64C:
									string str4 = "<ERR>wrong authorizes</ERR>\r\n";
									text += str4;
									goto IL_65C;
								}
								if (text6 != null)
								{
									Dictionary<long, string> dictionary = new Dictionary<long, string>();
									foreach (RackInfo rackInfo2 in allRack)
									{
										dictionary.Add(rackInfo2.RackID, rackInfo2.OriginalName);
									}
									text = text + "<reply cmd=\"RAK\" type=\"ZON\" id=\"" + text6 + "\">\r\n";
									if (this.IsValidToken(text2))
									{
										using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												int current = enumerator2.Current;
												ZoneInfo zoneByID = ZoneInfo.getZoneByID((long)current);
												if (zoneByID != null)
												{
													string rackInfo3 = zoneByID.RackInfo;
													string[] array4 = rackInfo3.Split(new char[]
													{
														','
													}, StringSplitOptions.RemoveEmptyEntries);
													string[] array2 = array4;
													for (int j = 0; j < array2.Length; j++)
													{
														string text11 = array2[j];
														if (!(text11 == string.Empty))
														{
															long num2 = (long)int.Parse(text11);
															if (dictionary.ContainsKey(num2))
															{
																string str5 = string.Concat(new object[]
																{
																	"<RAK id=\"",
																	num2,
																	"\" name=\"",
																	dictionary[num2],
																	"\"/>\r\n"
																});
																text += str5;
															}
														}
													}
												}
											}
											goto IL_59A;
										}
										goto IL_58A;
									}
									goto IL_58A;
									IL_59A:
									text += "</reply>\r\n";
									goto IL_197C;
									IL_58A:
									string str6 = "<ERR>wrong authorizes</ERR>\r\n";
									text += str6;
									goto IL_59A;
								}
							}
							else
							{
								if (text8 == "DEV")
								{
									List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
									if (text5 != null && string.Compare(text5, "ZON", true) == 0)
									{
										text = text + "<reply cmd=\"DEV\" type=\"ZON\" id=\"" + text6 + "\">\r\n";
										if (this.IsValidToken(text2))
										{
											using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
											{
												while (enumerator2.MoveNext())
												{
													int current2 = enumerator2.Current;
													ZoneInfo zoneByID2 = ZoneInfo.getZoneByID((long)current2);
													if (zoneByID2 != null)
													{
														string rackInfo4 = zoneByID2.RackInfo;
														string[] array5 = rackInfo4.Split(new char[]
														{
															','
														}, StringSplitOptions.RemoveEmptyEntries);
														if (array5 != null && array5.Length > 0)
														{
															Dictionary<long, string> dictionary2 = new Dictionary<long, string>();
															string[] array2 = array5;
															for (int j = 0; j < array2.Length; j++)
															{
																string text12 = array2[j];
																if (text12 != string.Empty)
																{
																	dictionary2.Add((long)int.Parse(text12), text12);
																}
															}
															if (dictionary2.Count > 0)
															{
																foreach (DeviceInfo current3 in allDevice)
																{
																	if (dictionary2.ContainsKey(current3.RackID))
																	{
																		string str7 = string.Concat(new object[]
																		{
																			"<DEV id=\"",
																			current3.DeviceID,
																			"\" name=\"",
																			current3.DeviceName,
																			"\"/>\r\n"
																		});
																		text += str7;
																	}
																}
															}
														}
													}
												}
												goto IL_823;
											}
											goto IL_813;
										}
										goto IL_813;
										IL_823:
										text += "</reply>\r\n";
										goto IL_197C;
										IL_813:
										string str8 = "<ERR>wrong authorizes</ERR>\r\n";
										text += str8;
										goto IL_823;
									}
									if (text5 != null && string.Compare(text5, "RAK", true) == 0)
									{
										text = text + "<reply cmd=\"DEV\" type=\"RAK\" id=\"" + text6 + "\">\r\n";
										if (this.IsValidToken(text2))
										{
											using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
											{
												while (enumerator2.MoveNext())
												{
													int current4 = enumerator2.Current;
													foreach (DeviceInfo current5 in allDevice)
													{
														if (current5.RackID == (long)current4)
														{
															string str9 = string.Concat(new object[]
															{
																"<DEV id=\"",
																current5.DeviceID,
																"\" name=\"",
																current5.DeviceName,
																"\"/>\r\n"
															});
															text += str9;
														}
													}
												}
												goto IL_938;
											}
											goto IL_928;
										}
										goto IL_928;
										IL_938:
										text += "</reply>\r\n";
										goto IL_197C;
										IL_928:
										string str10 = "<ERR>wrong authorizes</ERR>\r\n";
										text += str10;
										goto IL_938;
									}
									text += "<reply cmd=\"DEV\" type=\"ALL\">\r\n";
									if (this.IsValidToken(text2))
									{
										using (List<DeviceInfo>.Enumerator enumerator3 = allDevice.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												DeviceInfo current6 = enumerator3.Current;
												string str11 = string.Concat(new object[]
												{
													"<DEV id=\"",
													current6.DeviceID,
													"\" name=\"",
													current6.DeviceName,
													"\"/>\r\n"
												});
												text += str11;
											}
											goto IL_9EB;
										}
										goto IL_9DB;
									}
									goto IL_9DB;
									IL_9EB:
									text += "</reply>\r\n";
									goto IL_197C;
									IL_9DB:
									string str12 = "<ERR>wrong authorizes</ERR>\r\n";
									text += str12;
									goto IL_9EB;
								}
								else
								{
									if (text8 == "OLT")
									{
										if (text5 == null || string.Compare(text5, "DEV", true) != 0)
										{
											text += "<reply cmd=\"OLT\" type=\"ALL\">\r\n";
											if (this.IsValidToken(text2))
											{
												List<PortInfo> allPort = DeviceOperation.getAllPort();
												using (List<PortInfo>.Enumerator enumerator4 = allPort.GetEnumerator())
												{
													while (enumerator4.MoveNext())
													{
														PortInfo current7 = enumerator4.Current;
														string str13 = string.Concat(new object[]
														{
															"<OLT id=\"",
															current7.ID,
															"\" name=\"",
															current7.PortName,
															"\" dev=\"",
															current7.DeviceID,
															"\"/>\r\n"
														});
														text += str13;
													}
													goto IL_BFF;
												}
												goto IL_BEF;
											}
											goto IL_BEF;
											IL_BFF:
											text += "</reply>\r\n";
											goto IL_197C;
											IL_BEF:
											string str14 = "<ERR>wrong authorizes</ERR>\r\n";
											text += str14;
											goto IL_BFF;
										}
										if (text6 != null)
										{
											text = text + "<reply cmd=\"OLT\" type=\"DEV\" id=\"" + text6 + "\">\r\n";
											if (this.IsValidToken(text2))
											{
												using (List<int>.Enumerator enumerator2 = list.GetEnumerator())
												{
													while (enumerator2.MoveNext())
													{
														int current8 = enumerator2.Current;
														List<PortInfo> allPortByDeviceID = DeviceOperation.getAllPortByDeviceID(current8);
														foreach (PortInfo current9 in allPortByDeviceID)
														{
															string str15 = string.Concat(new object[]
															{
																"<OLT id=\"",
																current9.ID,
																"\" name=\"",
																current9.PortName,
																"\" dev=\"",
																current8,
																"\"/>\r\n"
															});
															text += str15;
														}
													}
													goto IL_B29;
												}
												goto IL_B19;
											}
											goto IL_B19;
											IL_B29:
											text += "</reply>\r\n";
											goto IL_197C;
											IL_B19:
											string str16 = "<ERR>wrong authorizes</ERR>\r\n";
											text += str16;
											goto IL_B29;
										}
									}
									else
									{
										if (text8 == "GRP")
										{
											List<GroupInfo> allGroup = GroupInfo.GetAllGroup();
											text += "<reply cmd=\"GRP\">\r\n";
											if (this.IsValidToken(text2))
											{
												using (List<GroupInfo>.Enumerator enumerator5 = allGroup.GetEnumerator())
												{
													while (enumerator5.MoveNext())
													{
														GroupInfo current10 = enumerator5.Current;
														string text13 = string.Empty;
														if (string.Compare(current10.GroupType, "allrack", true) == 0)
														{
															text13 = "AR";
														}
														else
														{
															if (string.Compare(current10.GroupType, "alldev", true) == 0)
															{
																text13 = "AD";
															}
															else
															{
																if (string.Compare(current10.GroupType, "alloutlet", true) == 0)
																{
																	text13 = "AO";
																}
																else
																{
																	if (string.Compare(current10.GroupType, "zone", true) == 0)
																	{
																		text13 = "Z";
																	}
																	else
																	{
																		if (string.Compare(current10.GroupType, "rack", true) == 0)
																		{
																			text13 = "R";
																		}
																		else
																		{
																			if (string.Compare(current10.GroupType, "dev", true) == 0)
																			{
																				text13 = "D";
																			}
																			else
																			{
																				if (string.Compare(current10.GroupType, "outlet", true) == 0)
																				{
																					text13 = "O";
																				}
																			}
																		}
																	}
																}
															}
														}
														if (text13 != string.Empty)
														{
															string str17 = string.Concat(new object[]
															{
																"<GRP id=\"",
																current10.ID,
																"\" name=\"",
																current10.GroupName,
																"\" type=\"",
																text13,
																"\"/>\r\n"
															});
															text += str17;
														}
													}
													goto IL_DC6;
												}
												goto IL_DB6;
											}
											goto IL_DB6;
											IL_DC6:
											text += "</reply>\r\n";
											goto IL_197C;
											IL_DB6:
											string str18 = "<ERR>wrong authorizes</ERR>\r\n";
											text += str18;
											goto IL_DC6;
										}
										if (text8 == "GMB")
										{
											if (text6 != null)
											{
												string text14 = "";
												string text15 = "";
												text14 = string.Empty;
												if (this.IsValidToken(text2))
												{
													foreach (int current11 in list)
													{
														GroupInfo groupByID = GroupInfo.GetGroupByID((long)current11);
														if (groupByID.GroupType != null && groupByID.Members != null)
														{
															text14 = groupByID.GroupType.Trim();
															Dictionary<long, string> dictionary3 = new Dictionary<long, string>();
															string text16 = groupByID.Members.Trim();
															string[] array6 = text16.Split(new char[]
															{
																','
															}, StringSplitOptions.RemoveEmptyEntries);
															if (array6 != null)
															{
																string[] array2 = array6;
																for (int j = 0; j < array2.Length; j++)
																{
																	string text17 = array2[j];
																	if (text17 != string.Empty)
																	{
																		dictionary3.Add((long)int.Parse(text17), text17);
																	}
																}
															}
															if (string.Compare(groupByID.GroupType, "allrack", true) == 0 || string.Compare(groupByID.GroupType, "rack", true) == 0)
															{
																if (string.Compare(groupByID.GroupType, "rack", true) == 0)
																{
																	text14 = "R";
																}
																else
																{
																	text14 = "AR";
																}
																ArrayList allRack2 = RackInfo.getAllRack();
																IEnumerator enumerator = allRack2.GetEnumerator();
																try
																{
																	while (enumerator.MoveNext())
																	{
																		RackInfo rackInfo5 = (RackInfo)enumerator.Current;
																		if (dictionary3.ContainsKey(rackInfo5.RackID))
																		{
																			string str19 = string.Concat(new object[]
																			{
																				"<GMB id=\"",
																				rackInfo5.RackID,
																				"\" name=\"",
																				rackInfo5.OriginalName,
																				"\"/>\r\n"
																			});
																			text15 += str19;
																		}
																	}
																	continue;
																}
																finally
																{
																	IDisposable disposable = enumerator as IDisposable;
																	if (disposable != null)
																	{
																		disposable.Dispose();
																	}
																}
															}
															if (string.Compare(groupByID.GroupType, "alldev", true) == 0 || string.Compare(groupByID.GroupType, "dev", true) == 0)
															{
																if (string.Compare(groupByID.GroupType, "dev", true) == 0)
																{
																	text14 = "D";
																}
																else
																{
																	text14 = "AD";
																}
																List<DeviceInfo> allDevice2 = DeviceOperation.GetAllDevice();
																using (List<DeviceInfo>.Enumerator enumerator3 = allDevice2.GetEnumerator())
																{
																	while (enumerator3.MoveNext())
																	{
																		DeviceInfo current12 = enumerator3.Current;
																		if (dictionary3.ContainsKey((long)current12.DeviceID))
																		{
																			string str20 = string.Concat(new object[]
																			{
																				"<GMB id=\"",
																				current12.DeviceID,
																				"\" name=\"",
																				current12.DeviceName,
																				"\"/>\r\n"
																			});
																			text15 += str20;
																		}
																	}
																	continue;
																}
															}
															if (string.Compare(groupByID.GroupType, "alloutlet", true) == 0 || string.Compare(groupByID.GroupType, "outlet", true) == 0)
															{
																if (string.Compare(groupByID.GroupType, "outlet", true) == 0)
																{
																	text14 = "O";
																}
																else
																{
																	text14 = "AO";
																}
																List<PortInfo> allPort2 = DeviceOperation.getAllPort();
																using (List<PortInfo>.Enumerator enumerator4 = allPort2.GetEnumerator())
																{
																	while (enumerator4.MoveNext())
																	{
																		PortInfo current13 = enumerator4.Current;
																		if (dictionary3.ContainsKey((long)current13.ID))
																		{
																			string str21 = string.Concat(new object[]
																			{
																				"<GMB id=\"",
																				current13.ID,
																				"\" name=\"",
																				current13.PortName,
																				"\" dev=\"",
																				current13.DeviceID,
																				"\"/>\r\n"
																			});
																			text15 += str21;
																		}
																	}
																	continue;
																}
															}
															if (string.Compare(groupByID.GroupType, "zone", true) == 0)
															{
																text14 = "Z";
																ArrayList allZone2 = ZoneInfo.getAllZone();
																foreach (ZoneInfo zoneInfo2 in allZone2)
																{
																	if (dictionary3.ContainsKey(zoneInfo2.ZoneID))
																	{
																		string str22 = string.Concat(new object[]
																		{
																			"<GMB id=\"",
																			zoneInfo2.ZoneID,
																			"\" name=\"",
																			zoneInfo2.ZoneName,
																			"\"/>\r\n"
																		});
																		text15 += str22;
																	}
																}
															}
														}
													}
												}
												if (this.IsValidToken(text2))
												{
													if (text14 != string.Empty)
													{
														string text18 = text;
														text = string.Concat(new string[]
														{
															text18,
															"<reply cmd=\"GMB\" type=\"",
															text14,
															"\" id=\"",
															text6,
															"\">\r\n"
														});
														text += text15;
														text += "</reply>\r\n";
													}
												}
												else
												{
													text = text + "<reply cmd=\"GMB\" type=\"\" id=\"" + text6 + "\">\r\n";
													text += "<ERR>wrong authorizes</ERR>\r\n";
													text += "</reply>\r\n";
												}
											}
										}
										else
										{
											if (text8 == "PCO")
											{
												DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 0);
												DateTime dateTime2 = dateTime;
												if (num < 1)
												{
													num = 1;
												}
												object obj = text;
												text = string.Concat(new object[]
												{
													obj,
													"<reply cmd=\"PCO\" type=\"",
													text5,
													"\" id=\"",
													text6,
													"\" period=\"",
													text7,
													",",
													num,
													"\" date=\"",
													text9,
													"\">\r\n"
												});
												if (this.IsValidToken(text2))
												{
													bool flag = false;
													if (text9 != string.Empty)
													{
														if (string.Compare(text7, "Y", true) == 0)
														{
															try
															{
																string[] array7 = text9.Split(new char[]
																{
																	'-'
																});
																if (array7.Length == 1)
																{
																	int num3 = int.Parse(array7[0]);
																	dateTime = new DateTime(num3, 1, 1, 0, 0, 0);
																	dateTime2 = new DateTime(num3 + num, 1, 1, 0, 0, 0);
																}
																else
																{
																	flag = true;
																}
																goto IL_17A2;
															}
															catch (Exception)
															{
																goto IL_17A2;
															}
															goto IL_1459;
														}
														goto IL_1459;
														IL_17A2:
														if (!flag && dateTime.Year > 2000 && dateTime2.Year > 2000)
														{
															string format = "yyyy-MM-dd HH:mm:ss";
															string time_start = dateTime.ToString(format);
															string time_end = dateTime2.ToString(format);
															string text19 = "";
															string text20 = "";
															string a = "";
															if (text5 != null)
															{
																if (string.Compare(text5, "GRP", true) == 0)
																{
																	text19 = BillingContext.GetMembersByGroup(list, ref a);
																	if (a == "AO" || a == "O")
																	{
																		text20 = text19;
																		text19 = "";
																	}
																}
																else
																{
																	if (string.Compare(text5, "ZON", true) == 0)
																	{
																		text19 = BillingContext.GetDevicesByZone(list);
																	}
																	else
																	{
																		if (string.Compare(text5, "RAK", true) == 0)
																		{
																			text19 = BillingContext.GetDevicesByRack(list);
																		}
																		else
																		{
																			if (string.Compare(text5, "DEV", true) == 0)
																			{
																				text19 = text6;
																			}
																			else
																			{
																				if (string.Compare(text5, "OLT", true) == 0)
																				{
																					text20 = text6;
																				}
																			}
																		}
																	}
																}
															}
															if (!(text19 != string.Empty) && !(text20 != string.Empty))
															{
																goto IL_1970;
															}
															List<MeasureData> measureData = BillingContext.getMeasureData(time_start, time_end, text7, text19, text20);
															if (measureData.Count <= 0)
															{
																goto IL_1970;
															}
															using (List<MeasureData>.Enumerator enumerator6 = measureData.GetEnumerator())
															{
																while (enumerator6.MoveNext())
																{
																	MeasureData current14 = enumerator6.Current;
																	if (current14.type == 0)
																	{
																		text = text + "<PCO type=\"DEV\" value=\"" + current14.value_list + "\"/>\r\n";
																	}
																	else
																	{
																		text = text + "<PCO type=\"OLT\" value=\"" + current14.value_list + "\"/>\r\n";
																	}
																}
																goto IL_1970;
															}
														}
														text += "<ERR>bad date format</ERR>\r\n";
														goto IL_1970;
														IL_1459:
														if (string.Compare(text7, "Q", true) == 0)
														{
															try
															{
																string[] array8 = text9.Split(new char[]
																{
																	'-'
																});
																if (array8.Length == 2)
																{
																	int num3 = int.Parse(array8[0]);
																	int num4 = int.Parse(array8[1]);
																	if (num4 < 1)
																	{
																		num4 = 1;
																	}
																	if (num4 > 4)
																	{
																		num4 = 4;
																	}
																	int num5 = 1 + 3 * (num4 - 1);
																	dateTime = new DateTime(num3, num5, 1, 0, 0, 0);
																	for (int k = 0; k < num; k++)
																	{
																		num5 += 3;
																		if (num5 > 12)
																		{
																			num3++;
																			num5 = 1;
																		}
																	}
																	dateTime2 = new DateTime(num3, num5, 1, 0, 0, 0);
																}
																else
																{
																	flag = true;
																}
																goto IL_17A2;
															}
															catch (Exception)
															{
																goto IL_17A2;
															}
														}
														if (string.Compare(text7, "M", true) == 0)
														{
															try
															{
																string[] array9 = text9.Split(new char[]
																{
																	'-'
																});
																if (array9.Length == 2)
																{
																	int num3 = int.Parse(array9[0]);
																	int num5 = int.Parse(array9[1]);
																	dateTime = new DateTime(num3, num5, 1, 0, 0, 0);
																	for (int l = 0; l < num; l++)
																	{
																		num5++;
																		if (num5 > 12)
																		{
																			num3++;
																			num5 = 1;
																		}
																	}
																	dateTime2 = new DateTime(num3, num5, 1, 0, 0, 0);
																}
																else
																{
																	flag = true;
																}
																goto IL_17A2;
															}
															catch (Exception)
															{
																goto IL_17A2;
															}
														}
														if (string.Compare(text7, "W", true) == 0)
														{
															try
															{
																string[] array10 = text9.Split(new char[]
																{
																	'-'
																});
																if (array10.Length == 2)
																{
																	int num3 = int.Parse(array10[0]);
																	int num6 = int.Parse(array10[1]);
																	if (num6 < 1)
																	{
																		num6 = 1;
																	}
																	dateTime = new DateTime(num3, 1, 1, 0, 0, 0);
																	DayOfWeek dayOfWeek = dateTime.DayOfWeek;
																	int num7 = (int)((DayOfWeek)7 - dayOfWeek);
																	if (num6 == 1)
																	{
																		dateTime = new DateTime(num3, 1, 1, 0, 0, 0);
																		dateTime2 = dateTime + new TimeSpan(num7 + 7 * (num - 1), 0, 0, 0);
																	}
																	else
																	{
																		dateTime = new DateTime(num3, 1, 1, 0, 0, 0);
																		dateTime += new TimeSpan(num7 + 7 * (num6 - 2), 0, 0, 0);
																		dateTime2 = dateTime + new TimeSpan(7 * num, 0, 0, 0);
																	}
																}
																else
																{
																	flag = true;
																}
																goto IL_17A2;
															}
															catch (Exception)
															{
																goto IL_17A2;
															}
														}
														if (string.Compare(text7, "D", true) == 0)
														{
															try
															{
																string[] array11 = text9.Split(new char[]
																{
																	'-'
																});
																if (array11.Length == 3)
																{
																	int num3 = int.Parse(array11[0]);
																	int num5 = int.Parse(array11[1]);
																	int day = int.Parse(array11[2]);
																	dateTime = new DateTime(num3, num5, day, 0, 0, 0);
																	dateTime2 = dateTime + new TimeSpan(num, 0, 0, 0);
																}
																else
																{
																	flag = true;
																}
																goto IL_17A2;
															}
															catch (Exception)
															{
																goto IL_17A2;
															}
														}
														if (string.Compare(text7, "H", true) == 0)
														{
															try
															{
																string[] array12 = text9.Split(new char[]
																{
																	'-'
																});
																if (array12.Length == 4)
																{
																	int num3 = int.Parse(array12[0]);
																	int num5 = int.Parse(array12[1]);
																	int day = int.Parse(array12[2]);
																	int hour = int.Parse(array12[3]);
																	dateTime = new DateTime(num3, num5, day, hour, 0, 0);
																	dateTime2 = dateTime + new TimeSpan(num, 0, 0);
																}
																else
																{
																	flag = true;
																}
															}
															catch (Exception)
															{
															}
															goto IL_17A2;
														}
														goto IL_17A2;
													}
													else
													{
														text += "<ERR>bad date format</ERR>\r\n";
													}
												}
												else
												{
													text += "<ERR>wrong authorizes</ERR>\r\n";
												}
												IL_1970:
												text += "</reply>\r\n";
											}
										}
									}
								}
							}
						}
					}
				}
				IL_197C:;
			}
			text += "</bp>";
			return text;
		}
		public static string GetDevicesByZone(List<int> zoneIDs)
		{
			string text = "";
			Dictionary<long, long> dictionary = new Dictionary<long, long>();
			foreach (int current in zoneIDs)
			{
				ZoneInfo zoneByID = ZoneInfo.getZoneByID((long)current);
				string[] array = zoneByID.RackInfo.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (!(text2 == string.Empty))
					{
						int num = int.Parse(text2);
						if (!dictionary.ContainsKey((long)num))
						{
							dictionary.Add((long)num, 0L);
						}
					}
				}
			}
			List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			foreach (DeviceInfo current2 in allDevice)
			{
				if (dictionary.ContainsKey(current2.RackID))
				{
					if (text != string.Empty)
					{
						text += ",";
					}
					text += current2.DeviceID.ToString();
				}
			}
			return text;
		}
		public static string GetDevicesByRack(List<int> rackIDs)
		{
			string text = "";
			List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			foreach (DeviceInfo current in allDevice)
			{
				if (rackIDs.Contains((int)current.RackID))
				{
					if (text != string.Empty)
					{
						text += ",";
					}
					text += current.DeviceID.ToString();
				}
			}
			return text;
		}
		public static string GetMembersByGroup(List<int> groupIDs, ref string groupType)
		{
			string text = "";
			groupType = string.Empty;
			foreach (int current in groupIDs)
			{
				GroupInfo groupByID = GroupInfo.GetGroupByID((long)current);
				if (groupByID.GroupType != null && groupByID.Members != null)
				{
					groupType = groupByID.GroupType.Trim();
					Dictionary<long, string> dictionary = new Dictionary<long, string>();
					string text2 = groupByID.Members.Trim();
					string[] array = text2.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array != null)
					{
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text3 = array2[i];
							if (text3 != string.Empty)
							{
								dictionary.Add((long)int.Parse(text3), text3);
							}
						}
					}
					if (string.Compare(groupByID.GroupType, "allrack", true) == 0 || string.Compare(groupByID.GroupType, "rack", true) == 0)
					{
						if (string.Compare(groupByID.GroupType, "rack", true) == 0)
						{
							groupType = "R";
						}
						else
						{
							groupType = "AR";
						}
						List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
						using (List<DeviceInfo>.Enumerator enumerator2 = allDevice.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								DeviceInfo current2 = enumerator2.Current;
								if (dictionary.ContainsKey(current2.RackID))
								{
									if (text != string.Empty)
									{
										text += ",";
									}
									text += current2.DeviceID.ToString();
								}
							}
							continue;
						}
					}
					if (string.Compare(groupByID.GroupType, "alldev", true) == 0 || string.Compare(groupByID.GroupType, "dev", true) == 0)
					{
						if (string.Compare(groupByID.GroupType, "dev", true) == 0)
						{
							groupType = "D";
						}
						else
						{
							groupType = "AD";
						}
						text = groupByID.Members;
					}
					else
					{
						if (string.Compare(groupByID.GroupType, "alloutlet", true) == 0 || string.Compare(groupByID.GroupType, "outlet", true) == 0)
						{
							if (string.Compare(groupByID.GroupType, "outlet", true) == 0)
							{
								groupType = "O";
							}
							else
							{
								groupType = "AO";
							}
							text = groupByID.Members;
						}
						else
						{
							if (string.Compare(groupByID.GroupType, "zone", true) == 0)
							{
								groupType = "Z";
								Dictionary<long, long> dictionary2 = new Dictionary<long, long>();
								foreach (KeyValuePair<long, string> current3 in dictionary)
								{
									ZoneInfo zoneByID = ZoneInfo.getZoneByID(current3.Key);
									string[] array3 = zoneByID.RackInfo.Split(new char[]
									{
										','
									}, StringSplitOptions.RemoveEmptyEntries);
									string[] array4 = array3;
									for (int j = 0; j < array4.Length; j++)
									{
										string text4 = array4[j];
										if (!(text4 == string.Empty))
										{
											int num = int.Parse(text4);
											if (!dictionary2.ContainsKey((long)num))
											{
												dictionary2.Add((long)num, 0L);
											}
										}
									}
								}
								List<DeviceInfo> allDevice2 = DeviceOperation.GetAllDevice();
								foreach (DeviceInfo current4 in allDevice2)
								{
									if (dictionary2.ContainsKey(current4.RackID))
									{
										if (text != string.Empty)
										{
											text += ",";
										}
										text += current4.DeviceID.ToString();
									}
								}
							}
						}
					}
				}
			}
			return text;
		}
		public static int getWeekOfYear(DateTime t)
		{
			int year = t.Year;
			int dayOfWeek = (int)new DateTime(year, 1, 1).DayOfWeek;
			int num = 7 - dayOfWeek;
			int dayOfYear = t.DayOfYear;
			if (dayOfYear <= num)
			{
				return 1;
			}
			int num2 = dayOfYear - num;
			int num3 = 1 + num2 / 7;
			if (num2 % 7 > 0)
			{
				num3++;
			}
			return num3;
		}
		public static List<MeasureData> getMeasureData(string time_start, string time_end, string type, string device_list, string port_list)
		{
			List<MeasureData> list = new List<MeasureData>();
			if (type == string.Empty)
			{
				return list;
			}
			if (port_list == string.Empty && device_list == string.Empty)
			{
				return list;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			DateTime d = Convert.ToDateTime(time_start);
			DateTime d2 = Convert.ToDateTime(time_end);
			d2 - d;
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			string text = device_list.Trim();
			if (text != string.Empty)
			{
				string[] array = text.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array != null)
				{
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						if (text2 != string.Empty)
						{
							dictionary.Add((long)int.Parse(text2), text2);
						}
					}
				}
			}
			Dictionary<long, string> dictionary2 = new Dictionary<long, string>();
			string text3 = port_list.Trim();
			if (text3 != string.Empty)
			{
				string[] array3 = text3.Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array3 != null)
				{
					string[] array4 = array3;
					for (int j = 0; j < array4.Length; j++)
					{
						string text4 = array4[j];
						if (text4 != string.Empty)
						{
							dictionary2.Add((long)int.Parse(text4), text4);
						}
					}
				}
			}
			try
			{
				dBConn = DBConnPool.getDynaConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					bool flag = true;
					byte b = 0;
					string text5 = "";
					string text6 = "";
					string text7 = "";
					string text8 = "";
					if (type == "H")
					{
						if (device_list != string.Empty)
						{
							text6 = " device_data_hourly";
						}
						else
						{
							if (port_list != string.Empty)
							{
								text6 = " port_data_hourly";
							}
						}
					}
					else
					{
						if (device_list != string.Empty)
						{
							text6 = " device_data_daily";
						}
						else
						{
							if (port_list != string.Empty)
							{
								text6 = " port_data_daily";
							}
						}
					}
					int num = 20000;
					string text9 = string.Concat(new string[]
					{
						" where insert_time >=#",
						time_start,
						"# and insert_time <#",
						time_end,
						"#"
					});
					if (device_list != string.Empty)
					{
						b = 0;
						if (dictionary.Count > num)
						{
							flag = false;
						}
						text5 = "select device_id, sum(power_consumption) from";
						if (flag)
						{
							text9 = text9 + " and device_id in (" + device_list + ")";
						}
						text7 = " group by device_id";
						text8 = " order by device_id";
					}
					else
					{
						if (port_list != string.Empty)
						{
							b = 1;
							if (dictionary2.Count > num)
							{
								flag = false;
							}
							text5 = "select port_id, sum(power_consumption) from";
							if (flag)
							{
								text9 = text9 + " and port_id in (" + port_list + ")";
							}
							text7 = " group by port_id";
							text8 = " order by port_id";
						}
					}
					if (flag)
					{
						text5 = "select sum(power_consumption) from";
						text7 = "";
						text8 = "";
					}
					if (text6 == string.Empty)
					{
						return list;
					}
					dbCommand.CommandText = string.Concat(new string[]
					{
						text5,
						text6,
						text9,
						text7,
						text8
					});
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					double num2 = 0.0;
					while (dbDataReader.Read())
					{
						if (flag)
						{
							num2 += dbDataReader.GetDouble(0);
						}
						else
						{
							int @int = dbDataReader.GetInt32(0);
							if (b == 0)
							{
								if (!dictionary.ContainsKey((long)@int))
								{
									continue;
								}
							}
							else
							{
								if (!dictionary2.ContainsKey((long)@int))
								{
									continue;
								}
							}
							num2 += dbDataReader.GetDouble(1);
						}
					}
					list.Add(new MeasureData
					{
						type = b,
						id = 0L,
						value_list = num2.ToString()
					});
					dbDataReader.Close();
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("getMeasureData: " + ex.Message, new string[0]);
			}
			finally
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return list;
		}
	}
}
