using DBAccessAPI;
using ecoProtocols;
using EventLogAPI;
using InSnergyAPI.ConnectionLayer;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
namespace InSnergyAPI.ApplicationLayer
{
	public class ApplicationHandler
	{
		public delegate void DelegateLog(string message);
		private const string errorResponse = "*2;000;00000000;2001;\n";
		private static object thisAppLock = new object();
		private static bool g_TreeChanged = false;
		public static List<int> listParamEnabled = null;
		private static Dictionary<string, Gateway> listGateway = new Dictionary<string, Gateway>();
		private static ApplicationHandler.DelegateLog logOut = null;
		public static long nTotalParameters = 0L;
		public static int nPendingDBRegister = 0;
		public static void SetLogCallback(ApplicationHandler.DelegateLog log)
		{
			ApplicationHandler.logOut = log;
		}
		private static void DebugLog(string sLog)
		{
			if (ApplicationHandler.logOut != null)
			{
				ApplicationHandler.logOut(sLog);
			}
		}
		public static void SetParamFilter(params int[] param)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				ApplicationHandler.listParamEnabled = new List<int>();
				for (int i = 0; i < param.Length; i++)
				{
					int item = param[i];
					ApplicationHandler.listParamEnabled.Add(item);
				}
			}
		}
		public static bool IsGatewayOnline(string gw)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.ContainsKey(gw))
				{
					bool result;
					if (ApplicationHandler.listGateway[gw].status.gatewayIP == "0.0.0.0:0" || ApplicationHandler.listGateway[gw].status.gatewayIP.IndexOf(":0") >= 0)
					{
						result = false;
						return result;
					}
					result = true;
					return result;
				}
			}
			return false;
		}
		public static bool IsBranchOnline(string gw, string branch)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (gw == null || branch == null)
				{
					bool result = false;
					return result;
				}
				if (ApplicationHandler.listGateway.ContainsKey(gw) && ApplicationHandler.listGateway[gw].listDevice.ContainsKey(branch))
				{
					bool result = true;
					return result;
				}
			}
			return false;
		}
		public static Dictionary<string, IMeter> GetBranch(string gw, string branch)
		{
			Dictionary<string, IMeter> dictionary = new Dictionary<string, IMeter>();
			if (gw == null || branch == null)
			{
				return dictionary;
			}
			if (!ApplicationHandler.IsGatewayOnline(gw))
			{
				return dictionary;
			}
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.ContainsKey(gw) && ApplicationHandler.listGateway[gw].listDevice.ContainsKey(branch))
				{
					foreach (KeyValuePair<string, Channel> current in ApplicationHandler.listGateway[gw].listDevice[branch].listChannel)
					{
						int num = Convert.ToInt32(current.Value.sCID);
						if (num >= 1 && num <= 12)
						{
							int num2 = ApplicationHandler.listGateway[gw].listDevice[branch].mapChannel[num - 1];
							if (num2 != 0)
							{
								string text = num.ToString();
								while (text.Length < 2)
								{
									text = "0" + text;
								}
								for (int i = 0; i < 3; i++)
								{
									string str = "0" + (i + 1).ToString();
									string text2 = (num2 >> 8 * (2 - i) & 255).ToString();
									while (text2.Length < 2)
									{
										text2 = "0" + text2;
									}
									if (!(text2 == "00"))
									{
										string text3 = text + str + text2;
										IMeter meter = new IMeter(text3);
										foreach (KeyValuePair<string, Param> current2 in current.Value.measurePair)
										{
											int num3 = Convert.ToInt32(current2.Value.aID);
											if ((ApplicationHandler.listParamEnabled == null || ApplicationHandler.listParamEnabled.Contains(num3)) && num3 > 1000 && num3 / 1000 == i + 1)
											{
												IParam value = new IParam(num3 % 100, current2.Value.dvalue, current2.Value.time);
												meter.listParam.Add(num3 % 100, value);
											}
										}
										dictionary.Add(text3, meter);
									}
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		public static bool IsDataChanged()
		{
			bool result;
			lock (ApplicationHandler.thisAppLock)
			{
				result = ApplicationHandler.g_TreeChanged;
			}
			return result;
		}
		public static void RemoveTimeoutGateways(long toSeconds)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.Count > 0)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, Gateway> current in ApplicationHandler.listGateway)
					{
						if (current.Value.status.socket == null)
						{
							long num = Common.ElapsedTime(current.Value.status.tLastReceiveTime);
							if (num > toSeconds * 1000L)
							{
								list.Add(current.Key);
							}
						}
					}
					foreach (string current2 in list)
					{
						InSnergyService.PostLog("Gateway [" + current2 + "] delay removed");
						ApplicationHandler.listGateway.Remove(current2);
						ApplicationHandler.g_TreeChanged = true;
					}
				}
			}
		}
		public static void DelegateOnLinkDown(Socket socket, string reason)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.Count > 0)
				{
					foreach (KeyValuePair<string, Gateway> current in ApplicationHandler.listGateway)
					{
						if (current.Value.status.socket == socket)
						{
							if (reason != "")
							{
								string sLog = "Gateway[" + current.Key + "]: " + reason;
								ApplicationHandler.DebugLog(sLog);
								if (InSnergyService.IsManaged(current.Key))
								{
									string text = current.Value.status.gatewayIP;
									int num = text.IndexOf(":");
									if (num >= 0)
									{
										text = text.Substring(0, num);
									}
									LogAPI.writeEventLog("0132011", new string[]
									{
										current.Key,
										text
									});
								}
							}
							current.Value.status.socket = null;
							int num2 = current.Value.status.gatewayIP.IndexOf(":");
							if (num2 >= 0)
							{
								current.Value.status.gatewayIP = current.Value.status.gatewayIP.Substring(0, num2);
								GatewayStatus expr_145 = current.Value.status;
								expr_145.gatewayIP += ":0";
							}
							current.Value.status.tLoginTime = "";
							current.Value.status.tUptime = DateTime.Now;
							current.Value.status.tLastReceiveTime = (long)Environment.TickCount;
							ApplicationHandler.g_TreeChanged = true;
							break;
						}
					}
				}
			}
		}
		public static void SetManaged(string gwID, bool managed)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.ContainsKey(gwID))
				{
					ApplicationHandler.listGateway[gwID].status.bManaged = managed;
					InSnergyService.PostLog(string.Concat(new object[]
					{
						"Set gateway ",
						gwID,
						" managed=",
						managed
					}));
				}
			}
		}
		private static bool IsGatewayExisted(string gid)
		{
			return ApplicationHandler.listGateway.ContainsKey(gid);
		}
		private static bool IsDeviceExisted(string gid, string did)
		{
			return ApplicationHandler.listGateway.ContainsKey(gid) && ApplicationHandler.listGateway[gid].IsDeviceExisted(did);
		}
		private static bool IsChannelExisted(string gid, string did, string ch)
		{
			return ApplicationHandler.listGateway.ContainsKey(gid) && ApplicationHandler.listGateway[gid].IsChannelExisted(did, ch);
		}
		public static void LoadGatewayListFromDatabase()
		{
			InSnergyService.PostLog("Loading gateway from database");
			List<InSnergyGateway> allGateWay = InSnergyGateway.GetAllGateWay();
			lock (ApplicationHandler.thisAppLock)
			{
				foreach (InSnergyGateway current in allGateWay)
				{
					InSnergyService.PostLog("Load gateway:" + current.GatewayID.Trim());
					Gateway gateway = new Gateway(current.GatewayID.Trim(), null, true, 800);
					foreach (Branch current2 in current.BranchList)
					{
						Device device = new Device(current2.BranchID.Trim(), "2");
						foreach (SubMeter current3 in current2.SubMeterList)
						{
							string text = current3.SubmeterID.Trim();
							if (text.Length == 6)
							{
								if (device.channelIDList.ContainsKey(text.Substring(0, 4)))
								{
									device.duplicatedIDList.Add(text);
								}
								else
								{
									device.channelIDList.Add(text.Substring(0, 4), text.Substring(4, 2));
								}
								string text2 = text.Substring(0, 2);
								while (text2.Length > 0 && text2.Substring(0, 1) == "0")
								{
									text2 = text2.Substring(1);
								}
								if (!device.listChannel.ContainsKey(text2))
								{
									Channel value = new Channel(text2);
									device.listChannel.Add(text2, value);
								}
								string text3 = text.Substring(2, 2);
								while (text3.Length > 0 && text3.Substring(0, 1) == "0")
								{
									text3 = text3.Substring(1);
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "001"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "001", new Param(text3 + "001", 0.0, DateTime.Now, ""));
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "002"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "002", new Param(text3 + "002", 0.0, DateTime.Now, ""));
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "003"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "003", new Param(text3 + "003", 0.0, DateTime.Now, ""));
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "004"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "004", new Param(text3 + "004", 0.0, DateTime.Now, ""));
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "005"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "005", new Param(text3 + "005", 0.0, DateTime.Now, ""));
								}
								if (!device.listChannel[text2].measurePair.ContainsKey(text3 + "008"))
								{
									device.listChannel[text2].measurePair.Add(text3 + "008", new Param(text3 + "008", 0.0, DateTime.Now, ""));
								}
							}
						}
						gateway.listDevice.Add(current2.BranchID.Trim(), device);
						string text4 = "";
						foreach (KeyValuePair<string, string> current4 in device.channelIDList)
						{
							if (text4 != "")
							{
								text4 += ",";
							}
							text4 += current4.Key.Substring(0, 2);
							text4 += "_";
							text4 += current4.Key.Substring(2, 2);
							text4 += "_";
							text4 += current4.Value;
						}
						InSnergyService.PostLog("\tLoad channel map: " + text4);
					}
					ApplicationHandler.listGateway.Add(current.GatewayID.Trim(), gateway);
				}
				ApplicationHandler.g_TreeChanged = true;
			}
			InSnergyService.PostLog("Gateway loaded from database: count=" + allGateWay.Count);
		}
		private static bool GatewayLogin(Socket socket, string gatewayID, string password, string TZone)
		{
			string text = TZone.Trim();
			int num;
			if (text == "")
			{
				num = 800;
			}
			else
			{
				int num2 = (text.Substring(0, 1) == "-") ? -1 : 1;
				if (num2 < 0)
				{
					text = text.Substring(1);
				}
				num = 0;
				int num3 = text.IndexOf(":");
				if (num3 >= 0)
				{
					num += Convert.ToInt32(text.Substring(num3 + 1));
					text = text.Substring(0, num3);
				}
				num += Convert.ToInt32(text) * 100;
				num *= num2;
			}
			if (!ApplicationHandler.listGateway.ContainsKey(gatewayID))
			{
				Gateway value = new Gateway(gatewayID, socket, InSnergyService.IsManaged(gatewayID), num);
				ApplicationHandler.listGateway.Add(gatewayID, value);
				ApplicationHandler.listGateway[gatewayID].status.nLoginCount++;
			}
			else
			{
				ApplicationHandler.listGateway[gatewayID].status.nLoginCount++;
				ApplicationHandler.listGateway[gatewayID].status.socket = socket;
				ApplicationHandler.listGateway[gatewayID].status.tZone = num;
				ApplicationHandler.listGateway[gatewayID].status.bManaged = InSnergyService.IsManaged(gatewayID);
				ApplicationHandler.listGateway[gatewayID].status.gatewayIP = socket.RemoteEndPoint.ToString();
				ApplicationHandler.listGateway[gatewayID].status.tLoginTime = DateTime.Now.ToString("MMMdd HH:mm:ss");
				if (ApplicationHandler.listGateway[gatewayID].status.tUptime == new DateTime(1970, 1, 1))
				{
					ApplicationHandler.listGateway[gatewayID].status.tUptime = DateTime.Now;
				}
				ApplicationHandler.listGateway[gatewayID].status.tLastReceiveTime = (long)Environment.TickCount;
				if (ApplicationHandler.listGateway[gatewayID].status.nTopStatus == 2)
				{
					InSnergyService.PostLog("DBRegister pending: gateway [" + gatewayID + "] rejected");
					socket.Shutdown(SocketShutdown.Both);
					socket.Disconnect(false);
					return false;
				}
				ApplicationHandler.listGateway[gatewayID].ResetTopology();
			}
			ApplicationHandler.g_TreeChanged = true;
			ApplicationHandler.DebugLog(string.Concat(new object[]
			{
				"Gateway [",
				gatewayID,
				"] login @ [",
				socket.Handle,
				"], LoginCount=",
				ApplicationHandler.listGateway[gatewayID].status.nLoginCount
			}));
			if (InSnergyService.IsManaged(gatewayID))
			{
				string text2 = socket.RemoteEndPoint.ToString();
				int num4 = text2.IndexOf(":");
				if (num4 >= 0)
				{
					text2 = text2.Substring(0, num4);
				}
				LogAPI.writeEventLog("0132010", new string[]
				{
					gatewayID,
					text2
				});
			}
			return true;
		}
		private static int UpdateGatewayDeviceChannelMap(string gid, string did, string channelmap, string slaveID)
		{
			int result;
			if (!ApplicationHandler.IsGatewayExisted(gid))
			{
				result = 0;
			}
			else
			{
				result = ApplicationHandler.listGateway[gid].UpdateDeviceChannelMap(did, channelmap, slaveID);
				ApplicationHandler.g_TreeChanged = true;
			}
			return result;
		}
		private static int UpdateGatewayDeviceChannelList(string gid, string did, string channellist, string slaveID)
		{
			int result;
			if (!ApplicationHandler.IsGatewayExisted(gid))
			{
				result = 0;
			}
			else
			{
				result = ApplicationHandler.listGateway[gid].UpdateDeviceChannelList(did, channellist, slaveID);
				ApplicationHandler.g_TreeChanged = true;
			}
			return result;
		}
		private static int UpdateGatewayDeviceList(string gid, string devicelist)
		{
			int result;
			if (!ApplicationHandler.IsGatewayExisted(gid))
			{
				result = 0;
			}
			else
			{
				result = ApplicationHandler.listGateway[gid].UpdateDeviceList(devicelist);
				ApplicationHandler.g_TreeChanged = true;
			}
			return result;
		}
		public static Dictionary<string, Gateway> GetGateways()
		{
			Dictionary<string, Gateway> dictionary = new Dictionary<string, Gateway>();
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.g_TreeChanged)
				{
					if (ApplicationHandler.listGateway.Count > 0)
					{
						foreach (KeyValuePair<string, Gateway> current in ApplicationHandler.listGateway)
						{
							Gateway value = current.Value.DeepClone();
							dictionary.Add(current.Key, value);
						}
					}
					ApplicationHandler.g_TreeChanged = false;
					return dictionary;
				}
			}
			return null;
		}
		private static void RejectGateway(string gwID)
		{
			if (ApplicationHandler.listGateway.ContainsKey(gwID) && ApplicationHandler.listGateway[gwID].status.socket.Connected)
			{
				string text = ConnectionManager.GetRemainingBuffer(ApplicationHandler.listGateway[gwID].status.socket);
				text = "";
				ApplicationHandler.listGateway[gwID].status.socket.Shutdown(SocketShutdown.Both);
				ApplicationHandler.listGateway[gwID].status.socket.Disconnect(false);
				InSnergyService.PostLog(string.Concat(new string[]
				{
					"Gateway [",
					gwID,
					"] rejected ",
					text,
					", not registered, or not managed"
				}));
			}
		}
		public static bool IsTopologyNotEmpty(string gwID)
		{
			bool result;
			lock (ApplicationHandler.thisAppLock)
			{
				if (!ApplicationHandler.listGateway.ContainsKey(gwID))
				{
					result = false;
				}
				else
				{
					foreach (KeyValuePair<string, Device> current in ApplicationHandler.listGateway[gwID].listDevice)
					{
						if (current.Value.channelIDList.Count > 0)
						{
							result = true;
							return result;
						}
					}
					result = false;
				}
			}
			return result;
		}
		private static bool IsAllChannelsReceived(string gwID)
		{
			if (!ApplicationHandler.listGateway.ContainsKey(gwID))
			{
				return false;
			}
			foreach (KeyValuePair<string, Device> current in ApplicationHandler.listGateway[gwID].listDevice)
			{
				if (!current.Value.bListReceived)
				{
					return false;
				}
			}
			return true;
		}
		public static void DBRegisterDone(string gw)
		{
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.ContainsKey(gw))
				{
					ApplicationHandler.listGateway[gw].status.nTopStatus = 0;
					ApplicationHandler.nPendingDBRegister--;
					InSnergyService.PostLog(string.Concat(new object[]
					{
						"DBRegister Success ",
						gw,
						"(",
						ApplicationHandler.nPendingDBRegister,
						")"
					}));
				}
			}
		}
		private static string ProcessS11(string[] fields)
		{
			string text = "0";
			if (!ApplicationHandler.IsDeviceExisted(fields[3], fields[4]))
			{
				text = "1S1104";
			}
			else
			{
				ApplicationHandler.g_TreeChanged = true;
				if (ApplicationHandler.listGateway[fields[3]].status.nTopStatus == 2)
				{
					InSnergyService.PostLog("DBRegister pending: gateway=" + fields[3] + ", branch=" + fields[4]);
					ApplicationHandler.RejectGateway(fields[3]);
					return null;
				}
				int num = ApplicationHandler.UpdateGatewayDeviceChannelMap(fields[3], fields[4], fields[7], fields[5]);
				if (num < 0)
				{
					ApplicationHandler.listGateway[fields[3]].status.nTopStatus = 1;
					num = Math.Abs(num);
				}
				if (num != Convert.ToInt32(fields[6]))
				{
					text = "1S2202";
				}
				else
				{
					if (ApplicationHandler.IsAllChannelsReceived(fields[3]))
					{
						if (!ApplicationHandler.listGateway[fields[3]].status.bManaged)
						{
							ApplicationHandler.listGateway[fields[3]].status.nTopStatus = 0;
							ApplicationHandler.RejectGateway(fields[3]);
							return null;
						}
						if (ApplicationHandler.listGateway[fields[3]].status.nTopStatus == 1)
						{
							InSnergyService.PostLog(string.Concat(new object[]
							{
								"Map Changed: ",
								fields[7],
								", MeterCount=",
								ApplicationHandler.listGateway[fields[3]].listDevice[fields[4]].channelIDList.Count
							}));
							Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
							foreach (KeyValuePair<string, Device> current in ApplicationHandler.listGateway[fields[3]].listDevice)
							{
								Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
								foreach (KeyValuePair<string, string> current2 in current.Value.channelIDList)
								{
									dictionary2.Add(current2.Key + current2.Value, current.Key);
								}
								dictionary.Add(current.Key, dictionary2);
							}
							string text2 = "";
							foreach (KeyValuePair<string, Dictionary<string, string>> current3 in dictionary)
							{
								if (text2 != "")
								{
									text2 += ",";
								}
								text2 += current3.Key;
								text2 += ",";
								text2 += current3.Value.Keys.Count.ToString();
							}
							GateWayRefreshQueue.Insertnewtask(new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
							{

								{
									fields[3],
									dictionary
								}
							});
							ApplicationHandler.nPendingDBRegister++;
							ApplicationHandler.listGateway[fields[3]].status.nTopStatus = 2;
							InSnergyService.PostLog(string.Concat(new object[]
							{
								"Registering gateway ",
								fields[3],
								"(",
								ApplicationHandler.nPendingDBRegister,
								"): ",
								text2
							}));
							ApplicationHandler.RejectGateway(fields[3]);
							return null;
						}
					}
				}
			}
			return string.Concat(new string[]
			{
				"*2;",
				fields[1].ToUpper(),
				";",
				fields[2].ToUpper(),
				";",
				text,
				";\n"
			});
		}
		public static List<IMeterPower> MeterPowerChanged(string gid, string did, string cid = "")
		{
			List<IMeterPower> list = new List<IMeterPower>();
			if (!ApplicationHandler.listGateway.ContainsKey(gid))
			{
				return list;
			}
			if (!ApplicationHandler.listGateway[gid].listDevice.ContainsKey(did))
			{
				return list;
			}
			if (cid == "")
			{
				using (Dictionary<string, Param>.Enumerator enumerator = ApplicationHandler.listGateway[gid].listDevice[did].measurePair.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, Param> arg_67_0 = enumerator.Current;
					}
					return list;
				}
			}
			foreach (KeyValuePair<string, Channel> current in ApplicationHandler.listGateway[gid].listDevice[did].listChannel)
			{
				string text = current.Key;
				if (!(text != cid))
				{
					while (text.Length < 2)
					{
						text = "0" + text;
					}
					foreach (KeyValuePair<string, Param> current2 in current.Value.measurePair)
					{
						string text2;
						if (current2.Key == "1008")
						{
							text2 = text + "01";
						}
						else
						{
							if (current2.Key == "2008")
							{
								text2 = text + "02";
							}
							else
							{
								if (!(current2.Key == "3008"))
								{
									continue;
								}
								text2 = text + "03";
							}
						}
						if (current2.Value.IsChanged())
						{
							current2.Value.ResetChange();
							if (ApplicationHandler.listGateway[gid].listDevice[did].channelIDList.ContainsKey(text2))
							{
								string str = ApplicationHandler.listGateway[gid].listDevice[did].channelIDList[text2];
								IMeterPower item = new IMeterPower(text2 + str, current2.Value.dvalue, current2.Value.time);
								list.Add(item);
							}
						}
					}
				}
			}
			return list;
		}
		private static string ProcessS16(string[] fields)
		{
			string text = "0";
			if (!ApplicationHandler.IsGatewayExisted(fields[3]))
			{
				return null;
			}
			if (!ApplicationHandler.listGateway[fields[3]].status.bManaged)
			{
				ApplicationHandler.RejectGateway(fields[3]);
				return null;
			}
			if (!ApplicationHandler.IsDeviceExisted(fields[3], fields[4]))
			{
				text = "1S1603";
			}
			else
			{
				ApplicationHandler.g_TreeChanged = true;
				ApplicationHandler.listGateway[fields[3]].UpdateDeviceAttributeS16(fields[4], fields[5], fields[6]);
			}
			return string.Concat(new string[]
			{
				"*2;",
				fields[1].ToUpper(),
				";",
				fields[2].ToUpper(),
				";",
				text,
				";\n"
			});
		}
		private static string ProcessS26(string[] fields)
		{
			string text = "0";
			if (!ApplicationHandler.IsGatewayExisted(fields[3]))
			{
				return null;
			}
			if (!ApplicationHandler.listGateway[fields[3]].status.bManaged)
			{
				ApplicationHandler.RejectGateway(fields[3]);
				return null;
			}
			if (ApplicationHandler.listGateway[fields[3]].status.nTopStatus == 2)
			{
				ApplicationHandler.RejectGateway(fields[3]);
				return null;
			}
			if (!ApplicationHandler.IsDeviceExisted(fields[3], fields[4]))
			{
				text = "1S2603";
			}
			else
			{
				if (!ApplicationHandler.IsChannelExisted(fields[3], fields[4], fields[5]))
				{
					text = "1S2604";
				}
				else
				{
					ApplicationHandler.g_TreeChanged = true;
					ApplicationHandler.listGateway[fields[3]].UpdateDeviceChannelAttribute26(fields[4], fields[5], fields[7], fields[8], fields[6]);
					List<IMeterPower> list = ApplicationHandler.MeterPowerChanged(fields[3], fields[4], fields[5]);
					if (list.Count > 0)
					{
						foreach (IMeterPower current in list)
						{
							GateWayRefreshQueue.InsertDataTask(new GatewayDataCell
							{
								GatewayID = fields[3],
								BranchID = fields[4],
								SubmeterID = current.sMid,
								PD = current.value,
								RecordTime = current.timestamp
							});
						}
						ApplicationHandler.nTotalParameters += (long)list.Count;
					}
				}
			}
			return string.Concat(new string[]
			{
				"*2;",
				fields[1].ToUpper(),
				";",
				fields[2].ToUpper(),
				";",
				text,
				";\n"
			});
		}
		private static string ProcessS22(string[] fields)
		{
			string text = "0";
			if (!ApplicationHandler.IsGatewayExisted(fields[3]))
			{
				text = "1S2204";
			}
			else
			{
				if (!ApplicationHandler.IsDeviceExisted(fields[3], fields[4]))
				{
					text = "1S2204";
				}
				else
				{
					ApplicationHandler.g_TreeChanged = true;
					int num = ApplicationHandler.UpdateGatewayDeviceChannelList(fields[3], fields[4], fields[7], fields[5]);
					if (num != Convert.ToInt32(fields[6]))
					{
						text = "1S2202";
					}
					else
					{
						if (ApplicationHandler.IsAllChannelsReceived(fields[3]))
						{
							if (!ApplicationHandler.listGateway[fields[3]].status.bManaged)
							{
								ApplicationHandler.RejectGateway(fields[3]);
								return null;
							}
							Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
							foreach (KeyValuePair<string, Device> current in ApplicationHandler.listGateway[fields[3]].listDevice)
							{
								Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
								foreach (KeyValuePair<string, string> current2 in current.Value.channelIDList)
								{
									dictionary2.Add(current2.Key + current2.Value, current.Key);
								}
								dictionary.Add(current.Key, dictionary2);
							}
							string text2 = "";
							foreach (KeyValuePair<string, Dictionary<string, string>> current3 in dictionary)
							{
								if (text2 != "")
								{
									text2 += ",";
								}
								text2 += current3.Key;
								text2 += ",";
								text2 += current3.Value.Keys.Count.ToString();
							}
						}
					}
				}
			}
			return string.Concat(new string[]
			{
				"*2;",
				fields[1].ToUpper(),
				";",
				fields[2].ToUpper(),
				";",
				text,
				";\n"
			});
		}
		private static string ProcessS31(string[] fields)
		{
			string text = "0";
			if (!ApplicationHandler.IsGatewayExisted(fields[3]))
			{
				text = "1S3103";
			}
			else
			{
				ApplicationHandler.g_TreeChanged = true;
				int num = ApplicationHandler.UpdateGatewayDeviceList(fields[3], fields[5]);
				if (num != Convert.ToInt32(fields[4]))
				{
					text = "1S3102";
				}
			}
			return string.Concat(new string[]
			{
				"*2;",
				fields[1].ToUpper(),
				";",
				fields[2].ToUpper(),
				";",
				text,
				";\n"
			});
		}
		public static byte[] ProtocolParser(Socket socket, int nAuthorized, byte[] request, int offset, int size)
		{
			if (size == 0)
			{
				return null;
			}
			bool flag = false;
			string text = "";
			byte[] array = new byte[size];
			Buffer.BlockCopy(request, offset, array, 0, size);
			string @string = Encoding.ASCII.GetString(array);
			if (@string == "")
			{
				return null;
			}
			string[] array2 = @string.Split(new char[]
			{
				';'
			});
			byte[] result;
			lock (ApplicationHandler.thisAppLock)
			{
				if (array2[0] == "*1")
				{
					string key;
					switch (key = array2[1].ToUpper())
					{
					case "S01":
					{
						text = array2[1].ToUpper();
						string tZone = "";
						if (array2.Length >= 6)
						{
							tZone = array2[5].Trim();
						}
						string text2;
						if (ApplicationHandler.GatewayLogin(socket, array2[3], array2[4], tZone))
						{
							text2 = string.Concat(new string[]
							{
								"*2;",
								array2[1].ToUpper(),
								";",
								array2[2].ToUpper(),
								";0;\n"
							});
							result = Encoding.ASCII.GetBytes(text2);
							flag = true;
							goto IL_5FF;
						}
						text2 = string.Concat(new string[]
						{
							"*2;",
							array2[1].ToUpper(),
							";",
							array2[2].ToUpper(),
							";0;\n"
						});
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S07":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = string.Concat(new string[]
						{
							"*2;",
							array2[1].ToUpper(),
							";",
							array2[2].ToUpper(),
							";0;\n"
						});
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S10":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = string.Concat(new string[]
						{
							"*2;",
							array2[1].ToUpper(),
							";",
							array2[2].ToUpper(),
							";0;\n"
						});
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S11":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = ApplicationHandler.ProcessS11(array2);
						if (text2 == null)
						{
							result = null;
							goto IL_5FF;
						}
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S16":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = ApplicationHandler.ProcessS16(array2);
						if (text2 == null)
						{
							result = null;
							goto IL_5FF;
						}
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S20":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						DateTime d = new DateTime(1970, 1, 1, 0, 0, 0);
						DateTime now = DateTime.Now;
						TimeSpan timeSpan = now - d;
						string text2 = string.Concat(new object[]
						{
							"*2;",
							array2[1].ToUpper(),
							";",
							array2[2].ToUpper(),
							";0;",
							Convert.ToInt32(timeSpan.TotalSeconds),
							";\n"
						});
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S22":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = ApplicationHandler.ProcessS22(array2);
						if (text2 == null)
						{
							result = null;
							goto IL_5FF;
						}
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S26":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = ApplicationHandler.ProcessS26(array2);
						if (text2 == null)
						{
							result = null;
							goto IL_5FF;
						}
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					case "S31":
					{
						if (nAuthorized <= 0)
						{
							byte[] result2 = null;
							return result2;
						}
						text = array2[1].ToUpper();
						string text2 = ApplicationHandler.ProcessS31(array2);
						if (text2 == null)
						{
							result = null;
							goto IL_5FF;
						}
						result = Encoding.ASCII.GetBytes(text2);
						goto IL_5FF;
					}
					}
					if (nAuthorized <= 0)
					{
						byte[] result2 = null;
						return result2;
					}
					result = Encoding.ASCII.GetBytes("*2;000;00000000;2001;\n");
				}
				else
				{
					if (array2[0] == "*2")
					{
						string text3 = "";
						if (ApplicationHandler.listGateway.Count > 0)
						{
							foreach (KeyValuePair<string, Gateway> current in ApplicationHandler.listGateway)
							{
								if (current.Value.status.socket == socket)
								{
									text3 = current.Value.status.sGID;
								}
							}
						}
						string a;
						if ((a = array2[1].ToUpper()) != null && a == "A01")
						{
							text = array2[1].ToUpper();
							if (Convert.ToInt32(array2[3]) == 0)
							{
								ApplicationHandler.listGateway[text3].UpdateDeviceAttributeA01(array2[4], array2[6]);
							}
							else
							{
								string sLog = "Query[" + text3 + "]: A01 failed " + array2[3];
								ApplicationHandler.DebugLog(sLog);
							}
							result = null;
						}
						else
						{
							result = null;
						}
					}
					else
					{
						result = Encoding.ASCII.GetBytes("*2;000;00000000;2001;\n");
					}
				}
				IL_5FF:
				if (!string.IsNullOrEmpty(text) && ApplicationHandler.IsGatewayExisted(array2[3]))
				{
					ApplicationHandler.listGateway[array2[3]].status.sLastReport = text;
					ApplicationHandler.listGateway[array2[3]].status.tLastReceiveTime = (long)Environment.TickCount;
					ApplicationHandler.listGateway[array2[3]].status.nTotalReport++;
					ApplicationHandler.listGateway[array2[3]].status.TotalReceived += size + 1;
				}
			}
			if (flag)
			{
				ConnectionManager.SetAuthorized(socket, array2[3], true);
			}
			return result;
		}
		public static void Query(SocketListener listen, string gid, string did, string ch, string querylist)
		{
			Socket socket = null;
			List<string> list = new List<string>();
			lock (ApplicationHandler.thisAppLock)
			{
				if (ApplicationHandler.listGateway.Count > 0)
				{
					foreach (KeyValuePair<string, Gateway> current in ApplicationHandler.listGateway)
					{
						socket = current.Value.status.socket;
						if (gid == "" || gid == current.Key)
						{
							if (ApplicationHandler.IsGatewayExisted(current.Key) && ApplicationHandler.listGateway[current.Key].listDevice.Count > 0)
							{
								foreach (KeyValuePair<string, Device> current2 in ApplicationHandler.listGateway[current.Key].listDevice)
								{
									if ((did == "" || did == current2.Key) && ApplicationHandler.IsDeviceExisted(current.Key, current2.Key))
									{
										string text = "*1;A01;XXXXXXXX;" + current2.Value.sDID + ";";
										if (querylist == "")
										{
											text += "12;1,2,3,4,5,6,7,8,9,10,11,12;";
										}
										else
										{
											text += querylist;
										}
										text += "\n";
										list.Add(text);
										current.Value.status.tLastSendTime = DateTime.Now.ToString("MMMdd HH:mm:ss");
										current.Value.status.TotalSent += text.Length;
										current.Value.status.nTotalPoll++;
									}
								}
							}
							if (list.Count > 0 && socket != null && listen != null)
							{
								listen.SendPoll(socket, list);
							}
						}
					}
				}
			}
		}
	}
}
