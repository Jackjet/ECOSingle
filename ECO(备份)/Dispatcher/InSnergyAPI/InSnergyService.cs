using CommonAPI;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using ecoProtocols;
using EventLogAPI;
using InSnergyAPI.ApplicationLayer;
using InSnergyAPI.ConnectionLayer;
using InSnergyAPI.DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
namespace InSnergyAPI
{
	public class InSnergyService
	{
		public const long PARAM_VOLTAGE_ID = 1L;
		public const long PARAM_CURRENT_ID = 2L;
		public const long PARAM_FREQUENCY_ID = 3L;
		public const long PARAM_FACTOR_ID = 4L;
		public const long PARAM_POWER_ID = 5L;
		public const long PARAM_METER_ID = 8L;
		public const long INSNERGY_COMMAND_START = 1L;
		public const long INSNERGY_COMMAND_STOP = 2L;
		public const long INSNERGY_COMMAND_MANAGE = 3L;
		public const long INSNERGY_COMMAND_UNMANAGE = 4L;
		public const long INSNERGY_COMMAND_FILTER = 5L;
		public const long INSNERGY_COMMAND_EVENT = 6L;
		public const long INSNERGY_COMMAND_BILLING_START = 7L;
		public const long INSNERGY_COMMAND_BILLING_STOP = 8L;
		public const long INSNERGY_COMMAND_BILLING_TOKEN = 9L;
		public const long INSNERGY_STATUS_STARTED = 1L;
		public const long INSNERGY_STATUS_OUTOFMEM = 2L;
		public const long INSNERGY_STATUS_DATACHANGED = 8L;
		public const long INSNERGY_STATUS_BILLING_STARTED = 16L;
		private const int TIMEOUT_AFTER_DOWN = 120;
		private const long nPollingTimeout = 1000L;
		public static int[] paramFilter = new int[]
		{
			1001,
			2001,
			3001,
			1002,
			2002,
			3002,
			1003,
			2003,
			3003,
			1004,
			2004,
			3004,
			1005,
			2005,
			3005,
			1008,
			2008,
			3008
		};
		private static int m_nListenPort = 8500;
		private static int m_nMaxListen = 100;
		private static int m_nConnectionTimeout = 120000;
		private static int m_nBufferSize = 1600;
		private static int m_nMaxConnections = 5000;
		private static int m_nMaxGateways = 3000;
		private static int m_nMaxDevices = 3000;
		private static int m_nMaxChannels = 3000;
		private static int m_nMaxParameters = 24;
		private static bool bServiceOwner = false;
		private static bool bInSnergyConnectRunning = false;
		private static string sCodebase = InSnergyService.sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static long m_nLastGatewayCount = 0L;
		private static long m_nTotalGatewayCount = 0L;
		private static Dictionary<string, IGateway> listGateway = new Dictionary<string, IGateway>();
		private static object thisInSnergyLock = new object();
		private static List<string> gwManaged = null;
		private static bool m_bManagedLoaded = false;
		private static Queue<string> gwManaged2Service = new Queue<string>();
		private static Queue<string> gwUnmanaged2Service = new Queue<string>();
		private static byte[] managedID = new byte[20];
		private static Queue<string> gwToken2Service = new Queue<string>();
		private static byte[] billingToken = new byte[128];
		private static string securityString = "";
		private static Queue<long> queueTrigger2Service = new Queue<long>();
		private static Dictionary<int, int> eventTriggers = new Dictionary<int, int>();
		private static SocketListener Listener = null;
		private static Thread listenerThread = null;
		private static Queue<string> log = new Queue<string>();
		private static System.Timers.Timer updateTimer = null;
		private static System.Timers.Timer fastTimer = null;
		private static Thread thEventThread = null;
		private static Semaphore semEventQueue = null;
		private static Semaphore semThreadAbort = null;
		private static WaitHandle[] semHandles;
		private static bool _isFirstTime;
		public static SocketListener GetListener()
		{
			return InSnergyService.Listener;
		}
		public static void LoadManagedGatewaysFromDatabase()
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.gwManaged = new List<string>();
				List<InSnergyGateway> allGateWay = InSnergyGateway.GetAllGateWay();
				foreach (InSnergyGateway current in allGateWay)
				{
					InSnergyService.gwManaged.Add(current.GatewayID);
				}
			}
		}
		public static void RestartBillingProtocol(bool bEnable, int port, string security)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					string text = "";
					if (bEnable)
					{
						text += "1\t";
					}
					else
					{
						text += "0\t";
					}
					text += port;
					text += "\t";
					text += security;
					InSnergyService.gwToken2Service.Enqueue(text);
				}
			}
		}
		public static void Manage(string gw)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.gwManaged != null && !InSnergyService.gwManaged.Contains(gw))
				{
					InSnergyService.gwManaged.Add(gw);
					InSnergyService.gwManaged2Service.Enqueue(gw);
				}
			}
		}
		public static bool IsManaged(string gw)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.gwManaged != null && InSnergyService.gwManaged.Contains(gw))
				{
					return true;
				}
			}
			return false;
		}
		public static void Unmanage(string gw)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.gwManaged != null && InSnergyService.gwManaged.Contains(gw))
				{
					InSnergyService.gwManaged.Remove(gw);
					InSnergyService.gwUnmanaged2Service.Enqueue(gw);
				}
			}
		}
		private static void StartBillingProtocolLocal(bool bEnable, int port, string token)
		{
			if (bEnable)
			{
				long num = 9L;
				byte[] bytes = Encoding.ASCII.GetBytes(token);
				byte[] array = new byte[8];
				int num2 = (bytes.Length + 4) / 5;
				int num3 = bytes.Length;
				for (int i = 0; i < Math.Min(15, num2); i++)
				{
					Array.Clear(array, 0, 8);
					Buffer.BlockCopy(bytes, i * 5, array, 0, Math.Min(5, num3));
					long num4 = num << 48;
					num4 += BitConverter.ToInt64(array, 0) << 8;
					num4 += (long)((i + 1 << 4) + num2);
					SharedData.QPushLong(num4);
					if (num3 >= 5)
					{
						num3 -= 5;
					}
					else
					{
						num3 = 0;
					}
				}
			}
			SharedData.QPushLong(2251799813685248L + (long)port);
			if (bEnable)
			{
				SharedData.QPushLong(1970324837040128L + (long)port);
			}
		}
		private static void OnFastTimer(object sender, EventArgs e)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.gwManaged2Service.Count != 0 || InSnergyService.gwUnmanaged2Service.Count != 0 || InSnergyService.gwToken2Service.Count != 0 || InSnergyService.queueTrigger2Service.Count != 0)
				{
					if (InSnergyService.fastTimer != null)
					{
						InSnergyService.fastTimer.Stop();
					}
					while (SharedData.getQueueFree() > 9L)
					{
						if (InSnergyService.gwToken2Service.Count <= 0)
						{
							IL_1E4:
							while (SharedData.getQueueFree() > 4L && (InSnergyService.gwManaged2Service.Count != 0 || InSnergyService.gwUnmanaged2Service.Count != 0 || InSnergyService.queueTrigger2Service.Count != 0))
							{
								if (InSnergyService.queueTrigger2Service.Count > 0)
								{
									long num = InSnergyService.queueTrigger2Service.Dequeue();
									long num2 = 6L;
									long num3 = num2 << 48;
									num3 += (num >> 32 & 65535L) << 32;
									num3 += (num & (long)((ulong)-1));
									SharedData.QPushLong(num3);
								}
								else
								{
									long num2 = 0L;
									string text = "";
									if (InSnergyService.gwManaged2Service.Count > 0)
									{
										text = InSnergyService.gwManaged2Service.Dequeue();
										num2 = 3L;
									}
									else
									{
										if (InSnergyService.gwUnmanaged2Service.Count > 0)
										{
											text = InSnergyService.gwUnmanaged2Service.Dequeue();
											num2 = 4L;
										}
									}
									if (text.Length == 20)
									{
										byte[] bytes = Encoding.ASCII.GetBytes(text);
										byte[] array = new byte[8];
										for (int i = 0; i < 4; i++)
										{
											Array.Clear(array, 0, 8);
											Buffer.BlockCopy(bytes, i * 5, array, 0, 5);
											long num3 = num2 << 48;
											num3 += BitConverter.ToInt64(array, 0) << 8;
											num3 += (long)(i + 1);
											SharedData.QPushLong(num3);
										}
									}
								}
							}
							if (InSnergyService.fastTimer != null)
							{
								InSnergyService.fastTimer.Start();
							}
							return;
						}
						string text2 = InSnergyService.gwToken2Service.Dequeue();
						string[] array2 = text2.Split(new char[]
						{
							'\t'
						});
						if (array2.Length == 3)
						{
							InSnergyService.StartBillingProtocolLocal(int.Parse(array2[0]) > 0, int.Parse(array2[1]), array2[2]);
						}
					}
					goto IL_1E4;
				}
			}
		}
		private static void OnUpdateTimer(object sender, EventArgs e)
		{
			DateTime.Now.ToString("MMMdd HH:mm:ss ");
			if (InSnergyService.updateTimer != null)
			{
				InSnergyService.updateTimer.Stop();
			}
			if (InSnergyService.sCodebase == "")
			{
				InSnergyService.sCodebase = AppDomain.CurrentDomain.BaseDirectory;
				InSnergyService.PostLog("Codebase:" + InSnergyService.sCodebase);
			}
			if (!InSnergyService.m_bManagedLoaded)
			{
				InSnergyService.LoadManagedGatewaysFromDatabase();
				InSnergyService.m_bManagedLoaded = true;
			}
			while (InSnergyService.log.Count > 0)
			{
				string text = InSnergyService.log.Dequeue();
				if (text != null)
				{
					text = text.Replace("\r\n", "").Trim();
					text != "";
				}
			}
			if (InSnergyService.bServiceOwner)
			{
				if (InSnergyService.Listener != null)
				{
					ConnectionManager.GetConnections();
					ConnectionManager.GetTotalReported();
					if (InSnergyService.Listener != null)
					{
						InSnergyService.Listener.GetPoolFree();
					}
					if (InSnergyService.Listener != null)
					{
						InSnergyService.Listener.CheckTimeout();
					}
					ApplicationHandler.RemoveTimeoutGateways(120L);
					Dictionary<string, Gateway> gateways = ApplicationHandler.GetGateways();
					if (gateways != null && (gateways.Count > 0 || (long)gateways.Count != InSnergyService.m_nLastGatewayCount))
					{
						InSnergyService.m_nLastGatewayCount = (long)gateways.Count;
						SharedData.LocalToShareEx(gateways, false);
					}
				}
				int dataQueueSize = GateWayRefreshQueue.GetDataQueueSize();
				if (dataQueueSize > 0)
				{
					InSnergyService.PostLog("Data Queue: " + dataQueueSize);
				}
			}
			if (InSnergyService.updateTimer != null)
			{
				InSnergyService.updateTimer.Start();
			}
		}
		public static void UpdateLocalTree()
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				InSnergyService.m_nTotalGatewayCount = SharedData.ShareToLocalEx(InSnergyService.listGateway);
			}
		}
		private static bool CheckCommandQueue()
		{
			List<long> list = SharedData.QPopLong(1);
			if (list.Count <= 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				long num = list[i] >> 48;
				if (num <= 9L && num >= 1L)
				{
					switch ((int)(num - 1L))
					{
					case 0:
						lock (InSnergyService.thisInSnergyLock)
						{
							if (InSnergyService.bServiceOwner)
							{
								InSnergyService.m_nConnectionTimeout = 1000 * DevAccessCfg.GetInstance().getISGtimeout();
								if (InSnergyService.m_nConnectionTimeout < 90000)
								{
									InSnergyService.m_nConnectionTimeout = 90000;
								}
								if (InSnergyService.m_nConnectionTimeout > 300000)
								{
									InSnergyService.m_nConnectionTimeout = 300000;
								}
								long num2 = list[i] >> 16 & 65535L;
								long num3 = list[i] & 65535L;
								InSnergyService.PostLog(string.Concat(new object[]
								{
									"INSNERGY_COMMAND_START:",
									num2,
									",port:",
									num3,
									",timeout:",
									InSnergyService.m_nConnectionTimeout
								}));
								DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_GateWayPortNA, true);
								if (num2 > 0L && !SharedData.IsServiceStarted())
								{
									if (DevAccessCfg.GetInstance().isISGsupport())
									{
										InSnergyService.m_nListenPort = (int)num3;
										if (InSnergyService.m_nListenPort == 0)
										{
											InSnergyService.m_nListenPort = 8500;
										}
										if (InSnergyService.StartListen(InSnergyService.m_nListenPort, InSnergyService.m_nMaxConnections, InSnergyService.m_nMaxListen, InSnergyService.m_nConnectionTimeout, InSnergyService.m_nBufferSize))
										{
											SharedData.WriteStatus(1L, true);
											SharedData.SetProducer(true);
											SharedData.ServiceStarted(1L);
											InSnergyService.PostLog("InSnergy StartListen Started Successfully\r\n");
										}
										else
										{
											LogAPI.writeEventLog("0110090", new string[]
											{
												InSnergyService.m_nListenPort.ToString()
											});
											DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_GateWayPortNA, true);
											InSnergyService.PostLog("Failed to StartListen InSnergy\r\n");
											DebugCenter.GetInstance().appendToFile("ISG port is already in use: " + InSnergyService.m_nListenPort);
										}
									}
									else
									{
										InSnergyService.PostLog("InSnergy Engine is disabled in xml file\r\n");
									}
								}
							}
							goto IL_81F;
						}
						goto IL_55F;
					case 1:
						goto IL_55F;
					case 2:
					case 3:
						goto IL_689;
					case 4:
						goto IL_616;
					case 5:
						lock (InSnergyService.thisInSnergyLock)
						{
							if (InSnergyService.bServiceOwner)
							{
								int num4 = (int)(list[i] >> 32 & 65535L);
								int num5 = (int)(list[i] & (long)((ulong)-1));
								if (InSnergyService.eventTriggers.ContainsKey(num4))
								{
									InSnergyService.eventTriggers[num4] = num5;
									InSnergyService.PostLog(string.Concat(new object[]
									{
										"INSNERGY_COMMAND_EVENT: update index=",
										num4,
										",value=",
										num5
									}));
								}
								else
								{
									InSnergyService.eventTriggers.Add(num4, num5);
									InSnergyService.PostLog(string.Concat(new object[]
									{
										"INSNERGY_COMMAND_EVENT: new index=",
										num4,
										",value=",
										num5
									}));
								}
							}
						}
						goto IL_81F;
					case 6:
						lock (InSnergyService.thisInSnergyLock)
						{
							if (InSnergyService.bServiceOwner)
							{
								long num6 = list[i] >> 16 & 65535L;
								long num7 = list[i] & 65535L;
								InSnergyService.PostLog(string.Concat(new object[]
								{
									"INSNERGY_COMMAND_BILLING_START:",
									num6,
									",port:",
									num7,
									",security:",
									InSnergyService.securityString
								}));
								DebugCenter.GetInstance().clearStatusCode(DebugCenter.ST_BillingPortNA, true);
								if (num6 > 0L && (SharedData.ReadStatus() & 16L) == 0L)
								{
									if (DevAccessCfg.GetInstance().isBillprotsupport())
									{
										int num8 = (int)num7;
										if (num8 == 0)
										{
											num8 = 8888;
										}
										if (ServicesAPI.StartBillingService(null, num8, InSnergyService.securityString))
										{
											SharedData.WriteStatus(16L, true);
											InSnergyService.PostLog(string.Concat(new object[]
											{
												"Billing Protocol Started Successfully,port=",
												num8,
												",security=",
												InSnergyService.securityString
											}));
										}
										else
										{
											LogAPI.writeEventLog("0110090", new string[]
											{
												num8.ToString()
											});
											DebugCenter.GetInstance().setLastStatusCode(DebugCenter.ST_BillingPortNA, true);
											InSnergyService.PostLog("Failed to start Billing Protocol\r\n");
											DebugCenter.GetInstance().appendToFile("BP port is already in use: " + num8);
										}
									}
									else
									{
										InSnergyService.PostLog("Billing Protocol is disabled in xml file\r\n");
									}
								}
							}
							goto IL_81F;
						}
						break;
					case 7:
						break;
					case 8:
						goto IL_2B2;
					default:
						goto IL_81F;
					}
					lock (InSnergyService.thisInSnergyLock)
					{
						if (InSnergyService.bServiceOwner)
						{
							long num9 = list[i] >> 16 & 65535L;
							long num10 = list[i] & 65535L;
							InSnergyService.PostLog(string.Concat(new object[]
							{
								"INSNERGY_COMMAND_BILLING_STOP:",
								num9,
								",port:",
								num10
							}));
							if ((SharedData.ReadStatus() & 16L) != 0L)
							{
								ServicesAPI.StopBillingService();
								SharedData.WriteStatus(16L, false);
								InSnergyService.PostLog("Billing Protocol Stopped\r\n");
							}
						}
						goto IL_81F;
					}
					IL_2B2:
					byte[] array = new byte[20];
					Array.Clear(array, 0, 20);
					long num11 = list[i] & 281474976710655L;
					int num12 = (int)(num11 & 255L);
					int num13 = num12 >> 4;
					num12 &= 15;
					num11 >>= 8;
					byte[] bytes = BitConverter.GetBytes(num11);
					Buffer.BlockCopy(bytes, 0, InSnergyService.billingToken, (num13 - 1) * 5, 5);
					Array.Clear(InSnergyService.billingToken, num13 * 5, InSnergyService.billingToken.Length - num13 * 5);
					if (num13 == num12)
					{
						InSnergyService.securityString = "";
						byte[] array2 = InSnergyService.billingToken;
						for (int j = 0; j < array2.Length; j++)
						{
							byte b = array2[j];
							char c = (char)b;
							if (c == '\0')
							{
								break;
							}
							InSnergyService.securityString += c;
						}
						goto IL_81F;
					}
					goto IL_81F;
					IL_55F:
					lock (InSnergyService.thisInSnergyLock)
					{
						if (InSnergyService.bServiceOwner)
						{
							long num14 = list[i] >> 16 & 65535L;
							long num15 = list[i] & 65535L;
							InSnergyService.PostLog(string.Concat(new object[]
							{
								"INSNERGY_COMMAND_STOP:",
								num14,
								",port:",
								num15
							}));
							if (SharedData.IsServiceStarted())
							{
								InSnergyService.StopListen();
								SharedData.WriteStatus(1L, false);
								SharedData.ServiceStarted(0L);
								SharedData.SetProducer(false);
								InSnergyService.PostLog("InSnergy Stopped\r\n");
							}
						}
						goto IL_81F;
					}
					IL_616:
					lock (InSnergyService.thisInSnergyLock)
					{
						int iSGFlag = Sys_Para.GetISGFlag();
						int iSGPort = Sys_Para.GetISGPort();
						InSnergyService.PostLog(string.Concat(new object[]
						{
							"INSNERGY_COMMAND_FILTER: flag=",
							iSGFlag,
							",port=",
							iSGPort
						}));
						bool arg_677_0 = InSnergyService.bServiceOwner;
						goto IL_81F;
					}
					IL_689:
					byte[] array3 = new byte[20];
					long num16 = list[i] & 281474976710655L;
					Array.Clear(array3, 0, 20);
					int num17 = (int)(num16 & 255L);
					num16 >>= 8;
					byte[] bytes2 = BitConverter.GetBytes(num16);
					Buffer.BlockCopy(bytes2, 0, InSnergyService.managedID, (num17 - 1) * 5, 5);
					if (num17 == 4)
					{
						string @string = Encoding.ASCII.GetString(InSnergyService.managedID, 0, 20);
						if (list[i] >> 48 == 3L)
						{
							InSnergyService.Manage(@string);
							ApplicationHandler.SetManaged(@string, true);
						}
						else
						{
							InSnergyService.Unmanage(@string);
							ApplicationHandler.SetManaged(@string, false);
						}
					}
				}
				IL_81F:;
			}
			return true;
		}
		private static bool IsProducer()
		{
			bool result;
			lock (InSnergyService.thisInSnergyLock)
			{
				result = SharedData.IsProducer();
			}
			return result;
		}
		public static void Restart(bool bEnable, int port)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					SharedData.QPushLong(562949953421312L + (bEnable ? 65536L : 0L) + (long)port);
					SharedData.QPushLong(281474976710656L + (bEnable ? 65536L : 0L) + (long)port);
				}
			}
		}
		public static bool StartInSnergyService(bool IsService)
		{
			Common.WriteLine("StartInSnergyService", new string[0]);
			InSnergyService.bServiceOwner = IsService;
			if (InSnergyService.bServiceOwner)
			{
				InSnergyService.m_nListenPort = Sys_Para.GetISGPort();
				if (InSnergyService.m_nListenPort == 0)
				{
					InSnergyService.m_nListenPort = 8500;
				}
				GateWayRefreshQueue.CBWork = new CallBackEventHandler(ApplicationHandler.DBRegisterDone);
			}
			InSnergyService.CreateEventQueue();
			bool result = false;
			lock (InSnergyService.thisInSnergyLock)
			{
				if (SharedData.CreateShare((long)InSnergyService.m_nMaxGateways, (long)InSnergyService.m_nMaxDevices, (long)InSnergyService.m_nMaxChannels, (long)InSnergyService.m_nMaxParameters))
				{
					result = true;
					if (InSnergyService.bServiceOwner)
					{
						SharedData.SetParamFilter(InSnergyService.paramFilter);
						ApplicationHandler.SetParamFilter(InSnergyService.paramFilter);
						ApplicationHandler.LoadGatewayListFromDatabase();
						int num = Sys_Para.GetISGFlag();
						if (num == 1)
						{
							InSnergyService.PostLog("Starting InSnergy Engine\r\n");
							SharedData.QPushLong(281474976710656L + (long)((long)num << 16) + (long)InSnergyService.m_nListenPort);
						}
						else
						{
							InSnergyService.PostLog("Stopping InSnergy Engine\r\n");
							SharedData.QPushLong(562949953421312L + (long)((long)num << 16) + (long)InSnergyService.m_nListenPort);
						}
						num = Sys_Para.GetBPFlag();
						int bPPort = Sys_Para.GetBPPort();
						string bPSecurity = Sys_Para.GetBPSecurity();
						if (num > 0 && bPPort > 0)
						{
							InSnergyService.StartBillingProtocolLocal(num > 0, bPPort, bPSecurity);
						}
					}
					InSnergyService.updateTimer = new System.Timers.Timer();
					InSnergyService.updateTimer.Elapsed += new ElapsedEventHandler(InSnergyService.OnUpdateTimer);
					InSnergyService.updateTimer.Interval = 1000.0;
					InSnergyService.updateTimer.Start();
					InSnergyService.PostLog("Update Timer Ready\r\n");
					if (!InSnergyService.bServiceOwner)
					{
						InSnergyService.fastTimer = new System.Timers.Timer();
						InSnergyService.fastTimer.Elapsed += new ElapsedEventHandler(InSnergyService.OnFastTimer);
						InSnergyService.fastTimer.Interval = 100.0;
						InSnergyService.fastTimer.Start();
						InSnergyService.PostLog("UI Event Timer Ready\r\n");
					}
				}
			}
			Common.WriteLine("StartInSnergyService Done", new string[0]);
			return result;
		}
		public static bool StopInSnergyService()
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.bServiceOwner)
				{
					InSnergyService.PostLog("Stopping gateway deamon\r\n");
					InSnergyService.StopListen();
					InSnergyService.PostLog("Stopping billing protocol deamon\r\n");
					if ((SharedData.ReadStatus() & 16L) != 0L)
					{
						ServicesAPI.StopBillingService();
					}
					SharedData.ServiceStarted(0L);
					SharedData.SetProducer(false);
					InSnergyService.PostLog("Stopping Clear DataTask\r\n");
					GateWayRefreshQueue.CleanDataTask();
					InSnergyService.PostLog("Clear DataTask stopped\r\n");
				}
				if (InSnergyService.updateTimer != null)
				{
					InSnergyService.updateTimer.Close();
					InSnergyService.updateTimer.Dispose();
					InSnergyService.updateTimer = null;
				}
				if (InSnergyService.fastTimer != null)
				{
					InSnergyService.fastTimer.Close();
					InSnergyService.fastTimer.Dispose();
					InSnergyService.fastTimer = null;
				}
				SharedData.CloseShare();
				InSnergyService.PostLog("CloseEventQueue\r\n");
				InSnergyService.CloseEventQueue();
				InSnergyService.PostLog("CloseEventQueue done\r\n");
				InSnergyService.bServiceOwner = false;
			}
			return true;
		}
		private static void DelegateLog(string strLog)
		{
			Common.WriteLine(strLog, new string[0]);
		}
		public static void PostLog(string strLog)
		{
			if (strLog == "")
			{
				return;
			}
			if (strLog == "\r\n")
			{
				return;
			}
			if (strLog == "\r")
			{
				return;
			}
			if (strLog == "\n")
			{
				return;
			}
			string text = strLog;
			while (text.EndsWith("\r") || text.EndsWith("\n"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			Common.WriteLine(text, new string[0]);
		}
		private static bool CreateEventQueue()
		{
			InSnergyService.semEventQueue = SharedData.OpenGlobalSemaphore("Global\\EventQueueAvailable", 0, 1023);
			if (InSnergyService.semEventQueue == null)
			{
				return false;
			}
			InSnergyService.semThreadAbort = SharedData.OpenGlobalSemaphore("Global\\KillEventQueueThread", 0, 1);
			if (InSnergyService.semThreadAbort == null)
			{
				InSnergyService.semEventQueue.Close();
				InSnergyService.semEventQueue.Dispose();
				InSnergyService.semEventQueue = null;
				return false;
			}
			if (InSnergyService.bServiceOwner)
			{
				InSnergyService.semHandles[0] = InSnergyService.semEventQueue;
				InSnergyService.semHandles[1] = InSnergyService.semThreadAbort;
				InSnergyService.thEventThread = new Thread(new ParameterizedThreadStart(InSnergyService.EventThread));
				InSnergyService.thEventThread.Name = "Event Exchanging Thread";
				InSnergyService.thEventThread.CurrentCulture = CultureInfo.InvariantCulture;
				InSnergyService.thEventThread.IsBackground = true;
				InSnergyService.thEventThread.Start(1);
			}
			return true;
		}
		private static void CloseEventQueue()
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.bServiceOwner)
				{
					try
					{
						if (InSnergyService.semThreadAbort != null)
						{
							InSnergyService.semThreadAbort.Release();
						}
						InSnergyService.thEventThread.Join(500);
					}
					catch (Exception ex)
					{
						InSnergyService.PostLog(ex.Message);
					}
				}
				InSnergyService.semEventQueue.Close();
				InSnergyService.semEventQueue.Dispose();
				InSnergyService.semThreadAbort.Close();
				InSnergyService.semThreadAbort.Dispose();
				InSnergyService.semHandles[0] = null;
				InSnergyService.semHandles[1] = null;
			}
		}
		public static void EventAdded()
		{
			try
			{
				if (InSnergyService.semEventQueue != null)
				{
					InSnergyService.semEventQueue.Release();
				}
			}
			catch (Exception ex)
			{
				InSnergyService.PostLog("Addd Event: " + ex.Message);
			}
		}
		private static void EventThread(object id)
		{
			InSnergyService.PostLog("Event queue is ready");
			while (true)
			{
				int num = WaitHandle.WaitAny(InSnergyService.semHandles, 2000);
				if (num != 258)
				{
					if (num != 0)
					{
						break;
					}
					while (InSnergyService.CheckCommandQueue())
					{
					}
				}
			}
			InSnergyService.PostLog("Event thread End");
		}
		private static bool StartListen(int nPort, int maxConnections, int nMaxListen, int nTimeout, int bufferSize)
		{
			if (InSnergyService.bInSnergyConnectRunning)
			{
				InSnergyService.PostLog("InSnergy is already running");
				return false;
			}
			if (nPort == 0 || nMaxListen == 0 || maxConnections < 1 || nTimeout < 10000 || bufferSize < 256)
			{
				InSnergyService.PostLog(string.Concat(new object[]
				{
					"Parameters error port=",
					nPort,
					",maxListen=",
					nMaxListen,
					",maxConnections=",
					maxConnections,
					",timeout=",
					nTimeout,
					",buffer=",
					bufferSize
				}));
				return false;
			}
			IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, nPort);
			InSnergyService.Listener = new SocketListener(maxConnections, nMaxListen, nTimeout, bufferSize, localEndPoint);
			InSnergyService.Listener.SetLogCallback(new SocketListener.DelegateLog(InSnergyService.DelegateLog));
			ApplicationHandler.SetLogCallback(new ApplicationHandler.DelegateLog(InSnergyService.DelegateLog));
			try
			{
				bool result;
				if (InSnergyService.Listener.Create())
				{
					ParameterizedThreadStart start = new ParameterizedThreadStart(SocketListener.ThreadProc);
					InSnergyService.listenerThread = new Thread(start);
					InSnergyService.listenerThread.Name = "InSnergy Listen Thread";
					InSnergyService.listenerThread.CurrentCulture = CultureInfo.InvariantCulture;
					InSnergyService.listenerThread.IsBackground = true;
					InSnergyService.listenerThread.Start(InSnergyService.Listener);
					InSnergyService.bInSnergyConnectRunning = true;
					result = true;
					return result;
				}
				result = false;
				return result;
			}
			catch (ThreadStateException ex)
			{
				InSnergyService.PostLog(ex.Message);
			}
			catch (ThreadInterruptedException ex2)
			{
				InSnergyService.PostLog(ex2.Message);
			}
			catch (Exception)
			{
			}
			return false;
		}
		private static void StopListen()
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.bInSnergyConnectRunning)
				{
					try
					{
						bool flag2 = true;
						if (InSnergyService.Listener != null)
						{
							flag2 = InSnergyService.Listener.NotifyQuit();
						}
						if (flag2)
						{
							InSnergyService.listenerThread.Abort();
						}
						InSnergyService.PostLog("InSnergy Joining");
						if (InSnergyService.listenerThread != null)
						{
							InSnergyService.listenerThread.Join(500);
						}
						InSnergyService.PostLog("InSnergy Joining Done");
						InSnergyService.Listener = null;
						InSnergyService.listenerThread = null;
						InSnergyService.bInSnergyConnectRunning = false;
					}
					catch (Exception ex)
					{
						InSnergyService.PostLog("Error: " + ex.Message);
					}
				}
			}
		}
		public static bool IsRunning()
		{
			bool result;
			lock (InSnergyService.thisInSnergyLock)
			{
				result = ((SharedData.ReadStatus() & 1L) > 0L);
			}
			return result;
		}
		public static bool IsOutOfSharedMemory()
		{
			bool result;
			lock (InSnergyService.thisInSnergyLock)
			{
				result = ((SharedData.ReadStatus() & 2L) > 0L);
			}
			return result;
		}
		public static long GetTotalGatewayCount()
		{
			long nTotalGatewayCount;
			lock (InSnergyService.thisInSnergyLock)
			{
				nTotalGatewayCount = InSnergyService.m_nTotalGatewayCount;
			}
			return nTotalGatewayCount;
		}
		public static void ConsumerCheck()
		{
			if (!SharedData.IsAvailable())
			{
				InSnergyService.StartInSnergyService(false);
				if (!InSnergyService.m_bManagedLoaded)
				{
					InSnergyService.LoadManagedGatewaysFromDatabase();
					InSnergyService.m_bManagedLoaded = true;
				}
				InSnergyService.m_nTotalGatewayCount = SharedData.ShareToLocalEx(InSnergyService.listGateway);
			}
		}
		public static string getGatewayIP(string gw)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null)
					{
						string result = "";
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw))
					{
						string result = InSnergyService.listGateway[gw].sIP;
						return result;
					}
				}
			}
			return "";
		}
		public static bool IsGatewayOnline(string gw)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null)
					{
						bool result = false;
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw))
					{
						bool result;
						if (InSnergyService.listGateway[gw].sIP == "0.0.0.0:0" || InSnergyService.listGateway[gw].sIP.IndexOf(":0") >= 0)
						{
							result = false;
							return result;
						}
						result = true;
						return result;
					}
				}
			}
			return false;
		}
		public static bool IsBranchOnline(string gw, string branch)
		{
			if (!InSnergyService.IsGatewayOnline(gw))
			{
				return false;
			}
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null || branch == null)
					{
						bool result = false;
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw) && InSnergyService.listGateway[gw].listDevice.ContainsKey(branch))
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}
		public static bool IsGatewayOnlineEx(string gw)
		{
			return ApplicationHandler.IsGatewayOnline(gw);
		}
		public static bool IsBranchOnlineEx(string gw, string branch)
		{
			return ApplicationHandler.IsBranchOnline(gw, branch);
		}
		public static Dictionary<string, IMeter> GetBranchEx(string gw, string branch)
		{
			return ApplicationHandler.GetBranch(gw, branch);
		}
		public static bool IsMeterOnline(string gw, string branch, string meter)
		{
			if (!InSnergyService.IsGatewayOnline(gw))
			{
				return false;
			}
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null || branch == null || meter == null)
					{
						bool result = false;
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw) && InSnergyService.listGateway[gw].listDevice.ContainsKey(branch) && InSnergyService.listGateway[gw].listDevice[branch].listChannel.ContainsKey(meter))
					{
						bool result = true;
						return result;
					}
				}
			}
			return false;
		}
		public static Dictionary<string, string> GetGatewayList(bool bUnmanagedOnly = true)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					foreach (KeyValuePair<string, IGateway> current in InSnergyService.listGateway)
					{
						if (!bUnmanagedOnly || InSnergyService.gwManaged == null || !InSnergyService.gwManaged.Contains(current.Key))
						{
							if (current.Value.sIP == "0.0.0.0:0")
							{
								dictionary.Add(current.Key, "");
							}
							else
							{
								dictionary.Add(current.Key, current.Value.sIP);
							}
						}
					}
				}
			}
			return dictionary;
		}
		public static List<string> GetBranchList(string gw)
		{
			List<string> list = new List<string>();
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null)
					{
						return list;
					}
					if (InSnergyService.listGateway.ContainsKey(gw))
					{
						foreach (KeyValuePair<string, IBranch> current in InSnergyService.listGateway[gw].listDevice)
						{
							list.Add(current.Key);
						}
					}
				}
			}
			return list;
		}
		public static List<string> GetMeterList(string gw, string branch)
		{
			List<string> list = new List<string>();
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null || branch == null)
					{
						return list;
					}
					if (InSnergyService.listGateway.ContainsKey(gw) && InSnergyService.listGateway[gw].listDevice.ContainsKey(branch))
					{
						foreach (KeyValuePair<string, IMeter> current in InSnergyService.listGateway[gw].listDevice[branch].listChannel)
						{
							list.Add(current.Key);
						}
					}
				}
			}
			return list;
		}
		public static Dictionary<string, IMeter> GetBranch(string gw, string branch)
		{
			Dictionary<string, IMeter> dictionary = new Dictionary<string, IMeter>();
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null || branch == null)
					{
						Dictionary<string, IMeter> result = dictionary;
						return result;
					}
					if (!InSnergyService.IsGatewayOnline(gw))
					{
						Dictionary<string, IMeter> result = dictionary;
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw) && InSnergyService.listGateway[gw].listDevice.ContainsKey(branch))
					{
						foreach (KeyValuePair<string, IMeter> current in InSnergyService.listGateway[gw].listDevice[branch].listChannel)
						{
							IMeter value = current.Value.DeepClone();
							dictionary.Add(current.Key, value);
						}
					}
				}
			}
			return dictionary;
		}
		public static List<IParam> GetMeterParams(string gw, string branch, string meter, int paramID)
		{
			List<IParam> list = new List<IParam>();
			lock (InSnergyService.thisInSnergyLock)
			{
				InSnergyService.ConsumerCheck();
				if (!InSnergyService.bServiceOwner)
				{
					if (gw == null || branch == null || meter == null)
					{
						List<IParam> result = list;
						return result;
					}
					if (!InSnergyService.IsGatewayOnline(gw))
					{
						List<IParam> result = list;
						return result;
					}
					if (InSnergyService.listGateway.ContainsKey(gw) && InSnergyService.listGateway[gw].listDevice.ContainsKey(branch) && InSnergyService.listGateway[gw].listDevice[branch].listChannel.ContainsKey(meter))
					{
						if (paramID == 0)
						{
							using (Dictionary<int, IParam>.Enumerator enumerator = InSnergyService.listGateway[gw].listDevice[branch].listChannel[meter].listParam.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<int, IParam> current = enumerator.Current;
									IParam item = new IParam(current.Value);
									list.Add(item);
								}
								goto IL_167;
							}
						}
						if (InSnergyService.listGateway[gw].listDevice[branch].listChannel[meter].listParam.ContainsKey(paramID))
						{
							IParam item2 = new IParam(InSnergyService.listGateway[gw].listDevice[branch].listChannel[meter].listParam[paramID]);
							list.Add(item2);
						}
					}
				}
				IL_167:;
			}
			return list;
		}
		public static void setEvent2Service(int index, int value)
		{
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.queueTrigger2Service != null)
				{
					InSnergyService.queueTrigger2Service.Enqueue(((long)index << 32) + (long)value);
				}
			}
		}
		public static int getEvent(int index, bool autoclear = true)
		{
			int result = 0;
			lock (InSnergyService.thisInSnergyLock)
			{
				if (InSnergyService.bServiceOwner && InSnergyService.eventTriggers.ContainsKey(index))
				{
					result = InSnergyService.eventTriggers[index];
					if (autoclear)
					{
						InSnergyService.eventTriggers[index] = 0;
					}
				}
			}
			return result;
		}
		static InSnergyService()
		{
			// Note: this type is marked as 'beforefieldinit'.
			WaitHandle[] array = new WaitHandle[2];
			InSnergyService.semHandles = array;
			InSnergyService._isFirstTime = true;
		}
	}
}
