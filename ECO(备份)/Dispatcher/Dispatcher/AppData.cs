using CommonAPI;
using CommonAPI.CultureTransfer;
using CustomXmlSerialization;
using DBAccessAPI;
using ecoProtocols;
using EventLogAPI;
using InSnergyAPI;
using Packing;
using SessionManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
namespace Dispatcher
{
	public class AppData
	{
		public const int RemoteCall_SQL_DataTable = 1;
		public const int RemoteCall_heatLoadDissipation = 2;
		public const int RemoteCall_getDeviceByID = 3;
		public const int RemoteCall_AllPort_in1Dev = 4;
		public const int RemoteCall_getDeviceInfoList = 7;
		public const int RemoteCall_UACDev2Port = 8;
		public const int RemoteCall_writeEventLog = 100;
		public const int RemoteCall_setEventFlag = 101;
		public const int RemoteCall_getSessions = 102;
		public const int RemoteCall_killSession = 103;
		public const int RemoteCall_getSrvDateTime = 104;
		public const string VALUE_Error = "Error";
		public static void LoadPUEData(ref double[] ret_v, ref int[] DB_Flg, ref string[] strTimePrompt)
		{
			try
			{
				int iSGFlag = Sys_Para.GetISGFlag();
				int iTPowerFlag = Sys_Para.GetITPowerFlag();
				DB_Flg[0] = iSGFlag;
				DB_Flg[1] = iTPowerFlag;
				if (iSGFlag != 0)
				{
					DateTime now = DateTime.Now;
					double num = 0.0;
					double num2 = 0.0;
					List<InSnergyGateway> allGateWay = InSnergyGateway.GetAllGateWay();
					foreach (InSnergyGateway current in allGateWay)
					{
						if (InSnergyService.IsGatewayOnlineEx(current.GatewayID))
						{
							foreach (Branch current2 in current.BranchList)
							{
								if (InSnergyService.IsBranchOnlineEx(current2.GatewayID, current2.BranchID))
								{
									Dictionary<string, IMeter> branchEx = InSnergyService.GetBranchEx(current.GatewayID, current2.BranchID);
									foreach (SubMeter current3 in current2.SubMeterList)
									{
										if (branchEx.ContainsKey(current3.SubmeterID) && current3.ElectricityUsage != 0)
										{
											IMeter meter = branchEx[current3.SubmeterID];
											switch (current3.ElectricityUsage)
											{
											case 1:
												if (meter.listParam.ContainsKey(5))
												{
													double dvalue = meter.listParam[5].dvalue;
													num += dvalue;
												}
												break;
											case 2:
												if (meter.listParam.ContainsKey(5))
												{
													double dvalue = meter.listParam[5].dvalue;
													num2 += dvalue;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					double num3;
					if (iTPowerFlag == 1)
					{
						num3 = num;
					}
					else
					{
						num3 = num;
					}
					double num4 = num3 + num2;
					ret_v[0] = num3;
					ret_v[1] = num4;
					strTimePrompt[0] = "";
					strTimePrompt[1] = "";
					if (InSnergyGateway.Need_Calculate_PUE)
					{
						DebugCenter.GetInstance().appendToFile("^^^ Begin to update PUE data ");
						DateTime now2 = DateTime.Now;
						num = InSnergyGateway.GetPUE(0);
						num2 = InSnergyGateway.GetPUE(1);
						num3 = num;
						if (iTPowerFlag == 1)
						{
							double dataCenterPDSum = DBTools.GetDataCenterPDSum(0);
							num3 = dataCenterPDSum + num;
						}
						num4 = num3 + num2;
						ret_v[2] = num3;
						ret_v[3] = num4;
						strTimePrompt[2] = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[3] = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
						InSnergyGateway.Last_IT_HOUR = num3;
						InSnergyGateway.Last_TT_HOUR = num4;
						num = InSnergyGateway.GetPUE(2);
						num2 = InSnergyGateway.GetPUE(3);
						num3 = num;
						if (iTPowerFlag == 1)
						{
							double dataCenterPDSum = DBTools.GetDataCenterPDSum(1);
							num3 = dataCenterPDSum + num;
						}
						num4 = num3 + num2;
						ret_v[4] = num3;
						ret_v[5] = num4;
						strTimePrompt[4] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[5] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						InSnergyGateway.Last_IT_DAY = num3;
						InSnergyGateway.Last_TT_DAY = num4;
						num = InSnergyGateway.GetPUE(4);
						num2 = InSnergyGateway.GetPUE(5);
						num3 = num;
						if (iTPowerFlag == 1)
						{
							double dataCenterPDSum = DBTools.GetDataCenterPDSum(2);
							num3 = dataCenterPDSum + num;
						}
						num4 = num3 + num2;
						ret_v[6] = num3;
						ret_v[7] = num4;
						DateTime dateTime = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))));
						if (now.DayOfWeek == DayOfWeek.Sunday)
						{
							dateTime = dateTime.AddDays(-7.0);
						}
						DateTime dateTime2 = dateTime.AddDays(6.0);
						strTimePrompt[6] = dateTime.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime2.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[7] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						InSnergyGateway.Last_IT_WEEK = num3;
						InSnergyGateway.Last_TT_WEEK = num4;
						InSnergyGateway.Need_Calculate_PUE = false;
						DebugCenter.GetInstance().appendToFile("^^^^ Finish to update PUE data " + (DateTime.Now - now2).TotalSeconds + " sec");
					}
					else
					{
						ret_v[2] = InSnergyGateway.Last_IT_HOUR;
						ret_v[3] = InSnergyGateway.Last_TT_HOUR;
						ret_v[4] = InSnergyGateway.Last_IT_DAY;
						ret_v[5] = InSnergyGateway.Last_TT_DAY;
						ret_v[6] = InSnergyGateway.Last_IT_WEEK;
						ret_v[7] = InSnergyGateway.Last_TT_WEEK;
						strTimePrompt[2] = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[4] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						DateTime dateTime3 = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))));
						if (now.DayOfWeek == DayOfWeek.Sunday)
						{
							dateTime3 = dateTime3.AddDays(-7.0);
						}
						DateTime dateTime4 = dateTime3.AddDays(6.0);
						strTimePrompt[6] = dateTime3.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime4.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[3] = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[5] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
						strTimePrompt[7] = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
					}
				}
			}
			catch (Exception ex)
			{
				string str = CommonAPI.ReportException(0, ex, true, "    ");
				Common.WriteLine("LoadPUEData: " + ex.Message + "\r\n" + str, new string[0]);
			}
		}
		public static int getDB_FlgISG()
		{
			return CultureTransfer.ToInt32(ClientAPI.getKeyValue("PUE_ISG"));
		}
		public static int getDB_flgAtenPDU()
		{
			return CultureTransfer.ToInt32(ClientAPI.getKeyValue("ATEN_PDU"));
		}
		public static bool getDB_flgEnablePower()
		{
			string keyValue = ClientAPI.getKeyValue("ENABLE_POWER_OP");
			return string.IsNullOrEmpty(keyValue) || CultureTransfer.ToInt32(keyValue) > 0;
		}
		public static double getPUE(int index)
		{
			double result = 0.0;
			switch (index)
			{
			case 0:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("CurrentIT"));
				break;
			case 1:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("CurrentTotal"));
				break;
			case 2:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("HourIT"));
				break;
			case 3:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("HourTotal"));
				break;
			case 4:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("DayIT"));
				break;
			case 5:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("DayTotal"));
				break;
			case 6:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("WeekIT"));
				break;
			case 7:
				result = CultureTransfer.ToDouble(ClientAPI.getPueValue("WeekTotal"));
				break;
			}
			return result;
		}
		public static string getPUETime(int index)
		{
			string result = "";
			switch (index)
			{
			case 0:
				result = ClientAPI.getPueValue("T_CurrentIT");
				break;
			case 1:
				result = ClientAPI.getPueValue("T_CurrentTotal");
				break;
			case 2:
				result = ClientAPI.getPueValue("T_HourIT");
				break;
			case 3:
				result = ClientAPI.getPueValue("T_HourTotal");
				break;
			case 4:
				result = ClientAPI.getPueValue("T_DayIT");
				break;
			case 5:
				result = ClientAPI.getPueValue("T_DayTotal");
				break;
			case 6:
				result = ClientAPI.getPueValue("T_WeekIT");
				break;
			case 7:
				result = ClientAPI.getPueValue("T_WeekTotal");
				break;
			}
			return result;
		}
		private static Dictionary<long, string> LoadheatLoadDissipation(ArrayList allRacks, int Power_dissipation_period)
		{
			DateTime now = DateTime.Now;
			string str = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
			string str2 = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
			switch (Power_dissipation_period)
			{
			case 0:
				str = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd HH", DateTimeFormatInfo.InvariantInfo);
				break;
			case 1:
				str = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				break;
			case 2:
			{
				DateTime dateTime = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfWeek.ToString("d"))));
				if (now.DayOfWeek == DayOfWeek.Sunday)
				{
					dateTime = dateTime.AddDays(-7.0);
				}
				DateTime dateTime2 = dateTime.AddDays(6.0);
				str = dateTime.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime2.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				break;
			}
			case 3:
			{
				DateTime dateTime = now.AddDays((double)(1 - now.Day));
				DateTime dateTime2 = dateTime.AddMonths(1).AddDays(-1.0);
				str = dateTime.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime2.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				break;
			}
			case 4:
			{
				DateTime dateTime = now.AddMonths(-((now.Month - 1) % 3)).AddDays((double)(1 - now.Day));
				DateTime dateTime2 = dateTime.AddMonths(3).AddDays(-1.0);
				str = dateTime.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime2.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				break;
			}
			case 5:
			{
				DateTime dateTime = now.AddDays((double)(1 - Convert.ToInt32(now.DayOfYear.ToString("d"))));
				DateTime dateTime2 = dateTime.AddYears(1).AddDays(-1.0);
				str = dateTime.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo) + " -- " + dateTime2.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				str2 = now.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo);
				break;
			}
			}
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			Hashtable rackPDSum = DBTools.GetRackPDSum(Power_dissipation_period, allRacks);
			for (int i = 0; i < allRacks.Count; i++)
			{
				double num = 0.0;
				bool flag = false;
				RackInfo rackInfo = (RackInfo)allRacks[i];
				if (rackPDSum != null && rackPDSum.Count > 0 && rackPDSum.ContainsKey(rackInfo.RackID))
				{
					flag = true;
					num = Convert.ToDouble(rackPDSum[rackInfo.RackID]);
				}
				if (flag)
				{
					dictionary.Add(rackInfo.RackID, num.ToString());
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			dictionary.Add(0L, str + "#" + str2);
			return dictionary;
		}
		public static byte[] AppProtResponse_Srv(int protocal_ID, string para, int fromUID)
		{
			XmlDocument xmlDocument = null;
			if (protocal_ID == 1)
			{
				try
				{
					List<int> list = new List<int>();
					DataTable dataTable = DBTools.CreateDataTable(para);
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						for (int j = 0; j < dataTable.Columns.Count; j++)
						{
							if (dataTable.Rows[i].ItemArray[j] == DBNull.Value)
							{
								list.Add(i);
								break;
							}
						}
					}
					if (list.Count > 0)
					{
						for (int k = list.Count - 1; k >= 0; k--)
						{
							dataTable.Rows.RemoveAt(list[k]);
						}
					}
					dataTable.TableName = "RCI";
					xmlDocument = CustomXmlSerializer.Serialize(dataTable, 8, "RCI");
					goto IL_59A;
				}
				catch (Exception ex)
				{
					Common.WriteLine("AppProtResponse_Srv---[RCI]: {0}", new string[]
					{
						ex.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 2)
			{
				try
				{
					ArrayList curRacks = DataSetManager.getCurRacks();
					Dictionary<long, string> obj = AppData.LoadheatLoadDissipation(curRacks, Convert.ToInt32(para));
					xmlDocument = CustomXmlSerializer.Serialize(obj, 8, "HEAT");
					goto IL_59A;
				}
				catch (Exception ex2)
				{
					Common.WriteLine("AppProtResponse_Srv---[HEAT]: {0}", new string[]
					{
						ex2.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 3)
			{
				try
				{
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(Convert.ToInt32(para));
					xmlDocument = CustomXmlSerializer.Serialize(deviceByID, 8, "RemoteCall_getDeviceByID");
					goto IL_59A;
				}
				catch (Exception ex3)
				{
					Common.WriteLine("AppProtResponse_Srv---[RemoteCall_getDeviceByID]: {0}", new string[]
					{
						ex3.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 4)
			{
				try
				{
					DeviceInfo deviceByID2 = DeviceOperation.getDeviceByID(Convert.ToInt32(para));
					List<PortInfo> obj2 = null;
					if (deviceByID2 != null)
					{
						obj2 = deviceByID2.GetPortInfo();
					}
					xmlDocument = CustomXmlSerializer.Serialize(obj2, 8, "RemoteCall_AllPort_in1Dev");
					goto IL_59A;
				}
				catch (Exception ex4)
				{
					Common.WriteLine("AppProtResponse_Srv---[RemoteCall_AllPort_in1Dev]: {0}", new string[]
					{
						ex4.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 7)
			{
				try
				{
					List<DeviceInfo> list2 = new List<DeviceInfo>();
					string[] array = para.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					for (int l = 0; l < array2.Length; l++)
					{
						string text = array2[l];
						string value = text.Trim();
						if (!string.IsNullOrEmpty(value))
						{
							DeviceInfo deviceByID3 = DeviceOperation.getDeviceByID(Convert.ToInt32(value));
							if (deviceByID3 != null)
							{
								list2.Add(deviceByID3);
							}
						}
					}
					xmlDocument = CustomXmlSerializer.Serialize(list2, 8, "RemoteCall_getDeviceInfoList");
					goto IL_59A;
				}
				catch (Exception ex5)
				{
					Common.WriteLine("AppProtResponse_Srv---[RemoteCall_getDeviceInfoList]: {0}", new string[]
					{
						ex5.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 100)
			{
				try
				{
					string[] array3 = para.Split(new char[]
					{
						'\n'
					});
					switch (array3.Length)
					{
					case 1:
						LogAPI.writeEventLog(array3[0], new string[0]);
						break;
					case 2:
					{
						string user = SessionAPI.getUser((long)fromUID);
						string remoteIP = SessionAPI.getRemoteIP((long)fromUID);
						string remoteType = SessionAPI.getRemoteType((long)fromUID);
						if (array3[0].Equals("0230003", StringComparison.InvariantCultureIgnoreCase))
						{
							LogAPI.writeEventLog("0230003", new string[]
							{
								user,
								remoteIP
							});
						}
						else
						{
							if (!remoteType.Equals("remote", StringComparison.InvariantCultureIgnoreCase))
							{
								LogAPI.writeEventLog(array3[0], new string[]
								{
									array3[1]
								});
							}
						}
						break;
					}
					case 3:
						LogAPI.writeEventLog(array3[0], new string[]
						{
							array3[1],
							array3[2]
						});
						break;
					case 4:
						LogAPI.writeEventLog(array3[0], new string[]
						{
							array3[1],
							array3[2],
							array3[3]
						});
						break;
					case 5:
						LogAPI.writeEventLog(array3[0], new string[]
						{
							array3[1],
							array3[2],
							array3[3],
							array3[4]
						});
						break;
					case 6:
						LogAPI.writeEventLog(array3[0], new string[]
						{
							array3[1],
							array3[2],
							array3[3],
							array3[4],
							array3[5]
						});
						break;
					case 7:
						LogAPI.writeEventLog(array3[0], new string[]
						{
							array3[1],
							array3[2],
							array3[3],
							array3[4],
							array3[5],
							array3[6]
						});
						break;
					}
					int num = 1;
					xmlDocument = CustomXmlSerializer.Serialize(num, 8, "RemoteCall_writeEventLog");
					goto IL_59A;
				}
				catch (Exception ex6)
				{
					Common.WriteLine("AppProtResponse_Srv---[RemoteCall_AllPort_in1Dev]: {0}", new string[]
					{
						ex6.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 101)
			{
				try
				{
					int num2 = 1;
					xmlDocument = CustomXmlSerializer.Serialize(num2, 8, "RemoteCall_setEventFlag");
					goto IL_59A;
				}
				catch (Exception ex7)
				{
					Common.WriteLine("AppProtResponse_Srv---[RemoteCall_setEventFlag]: {0}", new string[]
					{
						ex7.Message
					});
					goto IL_59A;
				}
			}
			if (protocal_ID == 102)
			{
				DataTable allSessions = SessionAPI.getAllSessions();
				xmlDocument = CustomXmlSerializer.Serialize(allSessions, 8, "Sessions");
			}
			else
			{
				if (protocal_ID == 103)
				{
					string obj3 = "Success";
					if (!SessionAPI.KillSessions(fromUID, para))
					{
						obj3 = "Failed";
					}
					xmlDocument = CustomXmlSerializer.Serialize(obj3, 8, "KillSession");
				}
				else
				{
					if (protocal_ID == 104)
					{
						DateTime now = DateTime.Now;
						xmlDocument = CustomXmlSerializer.Serialize(now, 8, "RemoteCall_getSrvDateTime");
					}
					else
					{
						if (protocal_ID == 8)
						{
							Dictionary<long, List<long>> dictionary = SessionAPI.getDeviceListClone((long)fromUID);
							if (dictionary == null)
							{
								dictionary = new Dictionary<long, List<long>>();
							}
							xmlDocument = CustomXmlSerializer.Serialize(dictionary, 8, "RemoteCall_UACDev2Port");
						}
					}
				}
			}
			IL_59A:
			if (xmlDocument == null)
			{
				return null;
			}
			byte[] result;
			try
			{
				string outerXml = xmlDocument.OuterXml;
				byte[] bytes = Encoding.UTF8.GetBytes(outerXml);
				result = bytes;
			}
			catch (Exception ex8)
			{
				Common.WriteLine("AppProtResponse_Srv: #{0}({1}), {2}", new string[]
				{
					protocal_ID.ToString(),
					para,
					ex8.Message
				});
				result = null;
			}
			return result;
		}
	}
}
