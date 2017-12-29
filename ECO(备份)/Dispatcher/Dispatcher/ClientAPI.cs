using CommonAPI;
using CommonAPI.Global;
using CommonAPI.Timers;
using CustomXmlSerialization;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoMessenger;
using ecoProtocols;
using Packing;
using SocketClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Management;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace Dispatcher
{
	public static class ClientAPI
	{
		public class DeviceWithZoneRackInfo
		{
			public int device_id;
			public string device_nm;
			public string device_model;
			public string fw_version;
			public int rack_id;
			public string rack_nm;
			public string zone_list;
		}
		public class RemoteCallContext
		{
			public long id;
			public long tStarted;
			public int protocal_ID;
			public int zipmethod;
			public string para;
			public bool isDone;
			public byte[] response;
			public RemoteCallContext()
			{
				this.id = -1L;
				this.protocal_ID = -1;
				this.zipmethod = 0;
				this.para = "";
				this.isDone = true;
				this.response = null;
			}
			public RemoteCallContext(long id, int protocal_ID, int zipmethod, string para)
			{
				this.id = id;
				this.tStarted = (long)Environment.TickCount;
				this.protocal_ID = protocal_ID;
				this.zipmethod = zipmethod;
				this.para = para;
				this.isDone = false;
				this.response = null;
			}
		}
		public class DeviceModelInfo
		{
			public string model_name;
			public string fw_version;
			public DeviceModelInfo()
			{
				this.model_name = "";
				this.fw_version = "";
			}
		}
		public class SqlQueryContext
		{
			public string identity;
			public string sql;
			public int nResultCode;
			public DataSet dsResult;
			public long tSend;
			public long tReceived;
			public SqlQueryContext()
			{
				this.identity = "";
				this.sql = "";
				this.nResultCode = 0;
				this.dsResult = null;
				this.tSend = (long)Environment.TickCount;
				this.tReceived = (long)Environment.TickCount;
			}
		}
		public class PendingStatusArgs : EventArgs
		{
			public PendingStatusArgs(object obj)
			{
			}
		}
		public class RealtimeDataArgs : EventArgs
		{
			public RealtimeDataArgs(object obj)
			{
			}
		}
		public class ThresholdArgs : EventArgs
		{
			public ThresholdArgs(object obj)
			{
			}
		}
		public class RackInfoArgs : EventArgs
		{
			public RackInfoArgs(object obj)
			{
			}
		}
		public class PUEChangedArgs : EventArgs
		{
			public PUEChangedArgs(object obj)
			{
			}
		}
		public class BroadcastArgs : EventArgs
		{
			public BroadcastArgs(object obj)
			{
			}
		}
		private static object _lockClient = new object();
		private static EcoHandler _MessageHandler = null;
		private static BaseTCPClient<EcoContext, EcoHandler> _pushChannel = null;
		private static BaseTCPClient<EcoContext, EcoHandler> _requestChannel = null;
		private static int _nRequestingFlag = 0;
		private static int _uid = 0;
		private static int _vid = 0;
		private static string _es_server = "";
		private static bool _usessl = true;
		private static int _mngr_service_port = 0;
		private static int _loginState = -1;
		private static long _loginItemMask = 0L;
		private static object _lockDataset = new object();
		private static DataSet _datasetFromRemote = null;
		private static DataSet _baseIncrementalRmote = null;
		private static DataSet _dsWorkRealtime = null;
		private static DataSet _dsWorkIncremental = null;
		private static bool _autoModelDone = false;
		private static int _dcLayout = 2;
		private static long _lastRealTimeSeq = 0L;
		private static Dictionary<long, CommParaClass> _devID2Rack = null;
		private static Dictionary<int, RackInfo> _aryRackInfo = null;
		private static Dictionary<long, List<long>> _UserUAC = null;
		private static object _lockLocalAccess = new object();
		private static DataSet _workingDataSet = null;
		private static DataTable _workingZoneInfo = null;
		private static DataTable _workingGroupInfo = null;
		private static Dictionary<string, string> _pueValuePair = null;
		private static Dictionary<string, string> _sysValuePair = null;
		private static Dictionary<string, ClientAPI.SqlQueryContext> _sqlQuery = null;
		private static Dictionary<int, int> _deviceOnlineStatus = null;
		private static int _pueCounter = 0;
		private static int[] _updateFlag;
		private static bool _login_use_SSL;
		private static string _login_string;
		private static string _last_cookie;
		private static Thread _WatchdogThread;
		private static ManualResetEvent[] evtWatchgog;
		private static long _syncTimerEnabled;
		private static long _syncTimer;
		private static Thread _syncThread;
		private static CommonAPI.DelegateOnBroadcast cbOnBroadcast;
		private static Dictionary<string, string> _macConflictList;
		public static CommonAPI.DelegateOnConnected cbOnConnected;
		private static bool _bOnClosedEnabled;
		private static CommonAPI.DelegateOnClosed cbOnClosed;
		private static Dictionary<long, ClientAPI.RemoteCallContext> _remoteCalls;
		public static event EventHandler<ClientAPI.PendingStatusArgs> PendingChanged;
		public static event EventHandler<ClientAPI.RealtimeDataArgs> RealtimeChanged;
		public static event EventHandler<ClientAPI.ThresholdArgs> ThresholdChanged;
		public static event EventHandler<ClientAPI.RackInfoArgs> RackInfoChanged;
		public static event EventHandler<ClientAPI.PUEChangedArgs> PUEChanged;
		public static event EventHandler<ClientAPI.BroadcastArgs> BroadcastChanged;
		public static void Sync_setTimer(long nInterval)
		{
			Interlocked.Exchange(ref ClientAPI._syncTimer, nInterval);
		}
		public static void Sync_Enable(long bEnable)
		{
			Interlocked.Exchange(ref ClientAPI._syncTimerEnabled, bEnable);
		}
		public static void Sync_Start()
		{
			if (ClientAPI._syncThread == null)
			{
				ClientAPI._syncThread = new Thread(new ThreadStart(ClientAPI.Sync_Thread));
				ClientAPI._syncThread.Name = string.Format("Dataset Sync Thread", new object[0]);
				ClientAPI._syncThread.IsBackground = true;
				ClientAPI._syncThread.Start();
			}
		}
		public static void Sync_Thread()
		{
			try
			{
				TickTimer tickTimer = new TickTimer();
				tickTimer.Start();
				while (true)
				{
					if (ClientAPI._loginState > 0 && ClientAPI._syncTimerEnabled > 0L)
					{
						if (tickTimer.getElapsed() > ClientAPI._syncTimer * 1000L)
						{
							if (ClientAPI._pushChannel != null)
							{
								ClientAPI._pushChannel.RequestDataset(16384);
							}
							tickTimer.Update();
						}
					}
					else
					{
						tickTimer.Update();
					}
					Thread.Sleep(100);
				}
			}
			catch (Exception)
			{
				Common.WriteLine("Sync_Thread Exited", new string[0]);
			}
		}
		public static void StartWatchdog()
		{
			if (ClientAPI._WatchdogThread == null)
			{
				ClientAPI._WatchdogThread = new Thread(new ThreadStart(ClientAPI.Watchdog4Connection));
				ClientAPI._WatchdogThread.Name = string.Format("Connection Watchdog", new object[0]);
				ClientAPI._WatchdogThread.IsBackground = true;
				ClientAPI._WatchdogThread.Start();
			}
		}
		public static void ReConnect()
		{
			if (ClientAPI._WatchdogThread != null && ClientAPI.evtWatchgog[2] != null)
			{
				ClientAPI.evtWatchgog[2].Set();
			}
		}
		public static void Watchdog4Connection()
		{
			try
			{
				while (true)
				{
					int num = WaitHandle.WaitAny(ClientAPI.evtWatchgog, 100);
					if (num != 258)
					{
						if (num == 0)
						{
							ClientAPI.WaitAuthentication(ClientAPI._login_use_SSL, ClientAPI._login_string, false, 30000u);
							ClientAPI.evtWatchgog[0].Reset();
						}
						else
						{
							if (num == 1)
							{
								ClientAPI.StopBroadcastChannel();
								ClientAPI._loginState = -1;
								ClientAPI.evtWatchgog[1].Reset();
							}
							else
							{
								if (num == 2)
								{
									ClientAPI.StopBroadcastChannel();
									ClientAPI._loginState = -1;
									try
									{
										Thread.Sleep(3000);
									}
									catch
									{
									}
									if (ClientAPI.WaitAuthentication(ClientAPI._login_use_SSL, ClientAPI._login_string, true, 30000u) < 0)
									{
										break;
									}
									if (ClientAPI.WaitDatasetReady(40000u) < 0)
									{
										goto Block_8;
									}
									ClientAPI.evtWatchgog[2].Reset();
								}
							}
						}
					}
				}
				if (ClientAPI.cbOnClosed != null)
				{
					ClientAPI.cbOnClosed(-1);
					goto IL_D8;
				}
				goto IL_D8;
				Block_8:
				if (ClientAPI.cbOnClosed != null)
				{
					ClientAPI.cbOnClosed(-1);
				}
				IL_D8:;
			}
			catch (Exception)
			{
				Common.WriteLine("Watchdog4Connection Exited", new string[0]);
			}
		}
		public static int getCpuUsage()
		{
			int num = 0;
			int num2 = 0;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					object value = managementObject["PercentProcessorTime"];
					object arg_43_0 = managementObject["Name"];
					num2 += Convert.ToInt32(value);
					num++;
				}
			}
			if (num > 0)
			{
				return num2 / num;
			}
			return 0;
		}
		public static int WaitAuthentication(bool useSSL, string user_pass_ver, bool bReconnect = false, uint tTimeout = 30000u)
		{
			lock (ClientAPI._lockClient)
			{
				if (!bReconnect)
				{
					ClientAPI.ResetAll();
				}
				Common.WriteLine("Waiting Authentication ..., ssl=" + useSSL.ToString(), new string[0]);
				ClientAPI._login_use_SSL = useSSL;
				ClientAPI._login_string = user_pass_ver;
				ClientAPI._usessl = useSSL;
				ClientAPI._uid = 0;
				ClientAPI._es_server = "";
				ClientAPI._mngr_service_port = 0;
				string valuePair = ValuePairs.getValuePair("uid");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._uid = Convert.ToInt32(valuePair);
				}
				valuePair = ValuePairs.getValuePair("MasterIP");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._es_server = ValuePairs.getValuePair("MasterIP");
				}
				valuePair = ValuePairs.getValuePair("ServicePort");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._mngr_service_port = Convert.ToInt32(ValuePairs.getValuePair("ServicePort"));
				}
				ClientAPI._MessageHandler = new EcoHandler();
				ClientAPI._MessageHandler.cbOnClosed = new EcoHandler.DelegateOnSocketClosed(ClientAPI.OnClosed);
				ClientAPI._MessageHandler.Start(false);
				DevAccessAPI.cbOnDeviceChanged = new DevAccessAPI.DelegateOnDeviceChanged(ClientAPI.SendDeviceOperation);
				DevAccessCfg._cbOnAutoModeUpdated = null;
				DevDiscoverAPI._cbOnAutoModePost = new DevDiscoverAPI.DelegateOnAutoModeBatch(ClientAPI.SendAutoModelBatch);
				ClientConfig clientConfig = new ClientConfig();
				clientConfig.uid = 0;
				clientConfig.vid = 0;
				clientConfig.bKeepAlive = true;
				clientConfig.ssl = ClientAPI._usessl;
				clientConfig.hostName = ClientAPI._es_server;
				clientConfig.port = ClientAPI._mngr_service_port;
				clientConfig.maxConnections = 1;
				clientConfig.interval = 20000;
				clientConfig.info4Login = user_pass_ver;
				ClientConfig expr_187 = clientConfig;
				expr_187.info4Login = expr_187.info4Login + "\n" + ClientAPI._last_cookie;
				if (clientConfig.ssl)
				{
					ClientAPI._pushChannel = new ClientSSL<EcoContext, EcoHandler>(clientConfig);
				}
				else
				{
					ClientAPI._pushChannel = new BaseTCPClient<EcoContext, EcoHandler>(clientConfig);
				}
				ClientAPI._bOnClosedEnabled = false;
				ClientAPI._pushChannel.Start(ClientAPI._MessageHandler);
			}
			long tLast = (long)Environment.TickCount;
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.ResetLastReceive();
			}
			int num = -11111111;
			while (num == -11111111)
			{
				if (ClientAPI._pushChannel != null)
				{
					tLast = ClientAPI._pushChannel.GetLastReceive();
				}
				if (Common.ElapsedTime(tLast) >= (long)((ulong)tTimeout))
				{
					num = -1;
					break;
				}
				int loginState = ClientAPI.getLoginState(true);
				if (loginState != 65535)
				{
					num = loginState;
				}
				Thread.Sleep(100);
			}
			if (num < 0)
			{
				ClientAPI.StopBroadcastChannel();
				ClientAPI._loginState = -1;
			}
			else
			{
				ClientAPI._bOnClosedEnabled = true;
				ClientAPI._uid = Convert.ToInt32(ValuePairs.getValuePair("uid"));
				ClientAPI._vid = Convert.ToInt32(ValuePairs.getValuePair("vid"));
				ClientAPI._last_cookie = ValuePairs.getValuePair("cookie");
				int num2 = Convert.ToInt32(ValuePairs.getValuePair("UserType"));
				if (ClientAPI._pushChannel != null)
				{
					if (num2 == 1)
					{
						ClientAPI._pushChannel.RequestDataset(16384);
					}
					else
					{
						ClientAPI._pushChannel.RequestDataset(4);
					}
				}
			}
			return num;
		}
		public static bool AsynAuthentication(bool useSSL, string user_pass_ver, bool bReconnect = false)
		{
			lock (ClientAPI._lockClient)
			{
				if (!bReconnect)
				{
					ClientAPI.ResetAll();
				}
				Common.WriteLine("Waiting Authentication ..., ssl=" + useSSL.ToString(), new string[0]);
				ClientAPI._login_use_SSL = useSSL;
				ClientAPI._login_string = user_pass_ver;
				ClientAPI._usessl = useSSL;
				ClientAPI._uid = 0;
				ClientAPI._es_server = "";
				ClientAPI._mngr_service_port = 0;
				string valuePair = ValuePairs.getValuePair("uid");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._uid = Convert.ToInt32(valuePair);
				}
				valuePair = ValuePairs.getValuePair("MasterIP");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._es_server = ValuePairs.getValuePair("MasterIP");
				}
				valuePair = ValuePairs.getValuePair("ServicePort");
				if (valuePair != null && valuePair != "")
				{
					ClientAPI._mngr_service_port = Convert.ToInt32(ValuePairs.getValuePair("ServicePort"));
				}
				ClientAPI._MessageHandler = new EcoHandler();
				ClientAPI._MessageHandler.cbOnClosed = new EcoHandler.DelegateOnSocketClosed(ClientAPI.OnClosed);
				ClientAPI._MessageHandler.Start(false);
				DevAccessAPI.cbOnDeviceChanged = null;
				DevAccessCfg._cbOnAutoModeUpdated = null;
				DevDiscoverAPI._cbOnAutoModePost = null;
				ClientConfig clientConfig = new ClientConfig();
				clientConfig.uid = 0;
				clientConfig.vid = 0;
				clientConfig.bKeepAlive = true;
				clientConfig.ssl = ClientAPI._usessl;
				clientConfig.hostName = ClientAPI._es_server;
				clientConfig.port = ClientAPI._mngr_service_port;
				clientConfig.maxConnections = 1;
				clientConfig.interval = 20000;
				clientConfig.info4Login = user_pass_ver;
				ClientConfig expr_16F = clientConfig;
				expr_16F.info4Login = expr_16F.info4Login + "\n" + ClientAPI._last_cookie;
				if (clientConfig.ssl)
				{
					ClientAPI._pushChannel = new ClientSSL<EcoContext, EcoHandler>(clientConfig);
				}
				else
				{
					ClientAPI._pushChannel = new BaseTCPClient<EcoContext, EcoHandler>(clientConfig);
				}
				ClientAPI._bOnClosedEnabled = false;
				ClientAPI._pushChannel.ResetLastReceive();
				ClientAPI._pushChannel.Start(ClientAPI._MessageHandler);
			}
			return true;
		}
		public static int GetConnectState()
		{
			int result;
			lock (ClientAPI._lockClient)
			{
				if (ClientAPI._loginState > 0)
				{
					result = 1;
				}
				else
				{
					int loginState = ClientAPI.getLoginState(true);
					if (loginState == 65535)
					{
						result = 0;
					}
					else
					{
						if (loginState < 0)
						{
							ClientAPI.StopBroadcastChannel();
							ClientAPI._loginState = -1;
							result = loginState;
						}
						else
						{
							ClientAPI._bOnClosedEnabled = true;
							ClientAPI._uid = Convert.ToInt32(ValuePairs.getValuePair("uid"));
							ClientAPI._vid = Convert.ToInt32(ValuePairs.getValuePair("vid"));
							ClientAPI._last_cookie = ValuePairs.getValuePair("cookie");
							result = 1;
						}
					}
				}
			}
			return result;
		}
		public static int WaitDatasetReady(uint tTimeout = 40000u)
		{
			int result;
			lock (ClientAPI._lockClient)
			{
				long tLast = (long)Environment.TickCount;
				if (ClientAPI._pushChannel != null)
				{
					ClientAPI._pushChannel.ResetLastReceive();
				}
				int num = -11111111;
				long num2 = 0L;
				while (num == -11111111)
				{
					if (ClientAPI._pushChannel != null)
					{
						tLast = ClientAPI._pushChannel.GetLastReceive();
					}
					if (Common.ElapsedTime(tLast) >= (long)((ulong)tTimeout))
					{
						num = -1;
						break;
					}
					int loginState = ClientAPI.getLoginState(false);
					if (loginState != 65535)
					{
						num = loginState;
					}
					if (ClientAPI._loginItemMask != num2)
					{
						num2 = ClientAPI._loginItemMask;
						tLast = (long)Environment.TickCount;
						Common.WriteLine("WaitDatasetReady: reset timer, itemmask={0}", new string[]
						{
							string.Format("{0:X02}", ClientAPI._loginItemMask)
						});
					}
					Thread.Sleep(100);
				}
				if (num < 0)
				{
					ClientAPI.StopBroadcastChannel();
					ClientAPI._loginState = -1;
				}
				else
				{
					ClientAPI._loginState = 1;
					ClientAPI._bOnClosedEnabled = true;
					ClientAPI._uid = Convert.ToInt32(ValuePairs.getValuePair("uid"));
					ClientAPI._vid = Convert.ToInt32(ValuePairs.getValuePair("vid"));
					ClientAPI.StartRequestChannel(ClientAPI._es_server, ClientAPI._mngr_service_port);
					tLast = (long)Environment.TickCount;
					while (Common.ElapsedTime(tLast) < (long)((ulong)tTimeout) && !ClientAPI.IsRequestChannelReady())
					{
						Thread.Sleep(100);
					}
					ClientAPI.StartWatchdog();
				}
				result = num;
			}
			return result;
		}
		public static void StopBroadcastChannel()
		{
			if (ClientAPI._loginState > 0)
			{
				ClientAPI.Logout();
			}
			ClientAPI.StopRequestChannel();
			ClientAPI._bOnClosedEnabled = false;
			Common.WriteLine("StopDataSetReceiver", new string[0]);
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.Stop();
			}
			ClientAPI._pushChannel = null;
		}
		private static void StartRequestChannel(string server, int port)
		{
			Common.WriteLine("StartRequestChannel ... ({0}:{1})", new string[]
			{
				server,
				port.ToString()
			});
			ClientConfig clientConfig = new ClientConfig();
			clientConfig.uid = ClientAPI._uid;
			clientConfig.vid = ClientAPI._vid;
			clientConfig.bKeepAlive = false;
			clientConfig.ssl = ClientAPI._usessl;
			clientConfig.hostName = server;
			clientConfig.port = port;
			clientConfig.maxConnections = 1;
			clientConfig.interval = 20000;
			clientConfig.info4Login = "";
			if (clientConfig.ssl)
			{
				ClientAPI._requestChannel = new ClientSSL<EcoContext, EcoHandler>(clientConfig);
			}
			else
			{
				ClientAPI._requestChannel = new BaseTCPClient<EcoContext, EcoHandler>(clientConfig);
			}
			ClientAPI._requestChannel.Start(ClientAPI._MessageHandler);
		}
		private static bool IsRequestChannelReady()
		{
			return ClientAPI._requestChannel.canSend();
		}
		private static void StopRequestChannel()
		{
			Common.WriteLine("StopRequestChannel...", new string[0]);
			if (ClientAPI._requestChannel != null)
			{
				ClientAPI._requestChannel.Stop();
			}
			ClientAPI._requestChannel = null;
		}
		private static void ResetAll()
		{
			ClientAPI._pueCounter = 0;
			ClientAPI._loginState = -1;
			ClientAPI._loginItemMask = 0L;
			ClientAPI._nRequestingFlag = 0;
			lock (ClientAPI._lockDataset)
			{
				ClientAPI._dcLayout = 2;
				if (ClientAPI._datasetFromRemote != null)
				{
					ClientAPI._datasetFromRemote.Dispose();
				}
				ClientAPI._datasetFromRemote = null;
				if (ClientAPI._baseIncrementalRmote != null)
				{
					ClientAPI._baseIncrementalRmote.Dispose();
				}
				ClientAPI._baseIncrementalRmote = null;
				if (ClientAPI._dsWorkRealtime != null)
				{
					ClientAPI._dsWorkRealtime.Dispose();
				}
				ClientAPI._dsWorkRealtime = null;
				if (ClientAPI._dsWorkIncremental != null)
				{
					ClientAPI._dsWorkIncremental.Dispose();
				}
				ClientAPI._dsWorkIncremental = null;
				if (ClientAPI._aryRackInfo != null)
				{
					ClientAPI._aryRackInfo.Clear();
				}
				ClientAPI._aryRackInfo = null;
				if (ClientAPI._devID2Rack != null)
				{
					ClientAPI._devID2Rack.Clear();
				}
				ClientAPI._devID2Rack = null;
				if (ClientAPI._UserUAC != null)
				{
					ClientAPI._UserUAC.Clear();
				}
				ClientAPI._UserUAC = null;
			}
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._pueValuePair != null)
				{
					ClientAPI._pueValuePair.Clear();
				}
				ClientAPI._pueValuePair = null;
				if (ClientAPI._sysValuePair != null)
				{
					ClientAPI._sysValuePair.Clear();
				}
				ClientAPI._sysValuePair = null;
				if (ClientAPI._sqlQuery != null)
				{
					ClientAPI._sqlQuery.Clear();
				}
				ClientAPI._sqlQuery = null;
				for (int i = 0; i < ClientAPI._updateFlag.Length; i++)
				{
					ClientAPI._updateFlag[i] = 0;
				}
				ClientAPI._autoModelDone = false;
				if (ClientAPI._workingDataSet != null)
				{
					ClientAPI._workingDataSet.Dispose();
				}
				ClientAPI._workingDataSet = null;
				if (ClientAPI._workingZoneInfo != null)
				{
					ClientAPI._workingZoneInfo.Dispose();
				}
				ClientAPI._workingZoneInfo = null;
				if (ClientAPI._workingGroupInfo != null)
				{
					ClientAPI._workingGroupInfo.Dispose();
				}
				ClientAPI._workingGroupInfo = null;
				if (ClientAPI._deviceOnlineStatus != null)
				{
					ClientAPI._deviceOnlineStatus.Clear();
				}
				ClientAPI._deviceOnlineStatus = null;
			}
		}
		public static void OnReceivedBroadcast(ulong idMessage, string operation, string carried, string guid, string alltype)
		{
			string text = "";
			int num = Convert.ToInt32(idMessage);
			if (!string.IsNullOrEmpty(alltype))
			{
				num = Convert.ToInt32(alltype);
				text = num.ToString("X8");
			}
			Common.WriteLine("Notify received: message_id={0}, operation={1}, attached={2}, guid={3}, alltype={4}", new string[]
			{
				idMessage.ToString("X8"),
				operation,
				carried,
				guid,
				text
			});
			Dictionary<string, string> dictionary = null;
			lock (ClientAPI._lockClient)
			{
				if (ClientAPI.cbOnBroadcast != null)
				{
					if ((num & 8192) != 0)
					{
						bool flag2 = true;
						if (operation.Equals("RestartListener", StringComparison.InvariantCultureIgnoreCase))
						{
							string[] array = carried.Split(new char[]
							{
								','
							});
							if (array.Length >= 2)
							{
								array[0].Trim();
								array[1].Trim();
							}
						}
						else
						{
							if (operation.Equals("MACConflict", StringComparison.InvariantCultureIgnoreCase))
							{
								dictionary = new Dictionary<string, string>();
								dictionary.Clear();
								string[] array2 = carried.Split(new char[]
								{
									';'
								}, StringSplitOptions.RemoveEmptyEntries);
								for (int i = 0; i < array2.Length; i++)
								{
									string[] array3 = array2[i].Split(new char[]
									{
										','
									});
									if (array3.Length == 2)
									{
										dictionary.Add(array3[0], array3[1]);
									}
								}
								flag2 = false;
							}
							else
							{
								if (operation.Equals("UAC:", StringComparison.InvariantCultureIgnoreCase))
								{
									flag2 = false;
								}
							}
						}
						if (flag2)
						{
							ClientAPI.cbOnBroadcast(8192, operation, carried);
						}
						num &= -8193;
					}
					if (num != 0)
					{
						ClientAPI.cbOnBroadcast(num, operation, carried);
					}
				}
			}
			lock (ClientAPI._lockLocalAccess)
			{
				if (dictionary != null)
				{
					ClientAPI._macConflictList = dictionary;
					for (int j = 0; j < ClientAPI._updateFlag.Length; j++)
					{
						ClientAPI._updateFlag[j] = 1;
					}
				}
			}
		}
		public static bool IsMACConflict(string strMac)
		{
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._macConflictList == null || ClientAPI._macConflictList.Count == 0)
				{
					bool result = false;
					return result;
				}
				if (ClientAPI._macConflictList.ContainsKey(strMac))
				{
					bool result = true;
					return result;
				}
			}
			return false;
		}
		private static void SetAllDeviceOffline()
		{
			if (ClientAPI._datasetFromRemote != null)
			{
				for (int i = 0; i < ClientAPI._datasetFromRemote.Tables.Count; i++)
				{
					if (i == 1)
					{
						if (ClientAPI._datasetFromRemote.Tables[i].Columns.Contains("humidity"))
						{
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("humidity");
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("temperature");
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("press_value");
						}
						DataColumn dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "humidity";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "temperature";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "press_value";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
					}
					else
					{
						if (ClientAPI._datasetFromRemote.Tables[i].Columns.Contains("voltage_value"))
						{
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("voltage_value");
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("current_value");
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("power_value");
							ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("power_consumption");
						}
						DataColumn dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "voltage_value";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "current_value";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "power_value";
						dataColumn.DefaultValue = DataSetManager.ValueEmpty;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Single");
						dataColumn.ColumnName = "power_consumption";
						dataColumn.DefaultValue = 0;
						ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						if (i == 0)
						{
							if (ClientAPI._datasetFromRemote.Tables[i].Columns.Contains("device_state"))
							{
								ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("device_state");
								ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("doorsensor_status");
							}
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Int32");
							dataColumn.ColumnName = "device_state";
							dataColumn.DefaultValue = 0;
							ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Int32");
							dataColumn.ColumnName = "doorsensor_status";
							dataColumn.DefaultValue = DataSetManager.PDUData_Doorst_noattach;
							ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
						}
						else
						{
							if (i == 2)
							{
								if (ClientAPI._datasetFromRemote.Tables[i].Columns.Contains("port_state"))
								{
									ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("port_state");
								}
								dataColumn = new DataColumn();
								dataColumn.DataType = Type.GetType("System.String");
								dataColumn.ColumnName = "port_state";
								dataColumn.DefaultValue = OutletStatus.OFF.ToString();
								ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
							}
							if (i == 3)
							{
								if (ClientAPI._datasetFromRemote.Tables[i].Columns.Contains("bank_state"))
								{
									ClientAPI._datasetFromRemote.Tables[i].Columns.Remove("bank_state");
								}
								dataColumn = new DataColumn();
								dataColumn.DataType = Type.GetType("System.String");
								dataColumn.ColumnName = "bank_state";
								dataColumn.DefaultValue = BankStatus.OFF.ToString();
								ClientAPI._datasetFromRemote.Tables[i].Columns.Add(dataColumn);
							}
						}
					}
				}
			}
		}
		public static int MergeToWorkingDataset(Serialization received, DispatchAttribute attrib)
		{
			long tLast = (long)Environment.TickCount;
			int num = 0;
			if (received == null)
			{
				return num;
			}
			if (received._dtAutoModel != null)
			{
				foreach (DataRow dataRow in received._dtAutoModel.Rows)
				{
					string text = dataRow.ItemArray[0].ToString();
					string text2 = dataRow.ItemArray[1].ToString();
					string text3 = dataRow.ItemArray[2].ToString();
					string text4 = dataRow.ItemArray[3].ToString();
					DevAccessCfg.GetInstance().updateAutoModelList(text, text2, text3, text4);
					Common.WriteLine(string.Format(" AutoModel Received: {0},{1},{2},{3}", new object[]
					{
						text,
						text2,
						text3,
						text4
					}), new string[0]);
				}
				ClientAPI._autoModelDone = true;
				num |= 2048;
			}
			if (received._dtZoneInfo != null)
			{
				lock (ClientAPI._lockLocalAccess)
				{
					if (ClientAPI._workingZoneInfo != null)
					{
						ClientAPI._workingZoneInfo.Dispose();
					}
					ClientAPI._workingZoneInfo = received._dtZoneInfo;
				}
				num |= 256;
			}
			if (received._dtGroupInfo != null)
			{
				lock (ClientAPI._lockLocalAccess)
				{
					if (ClientAPI._workingGroupInfo != null)
					{
						ClientAPI._workingGroupInfo.Dispose();
					}
					ClientAPI._workingGroupInfo = received._dtGroupInfo;
				}
				num |= 512;
			}
			if (received._tblUserUAC != null)
			{
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._UserUAC == null)
					{
						ClientAPI._UserUAC = new Dictionary<long, List<long>>();
					}
					else
					{
						ClientAPI._UserUAC.Clear();
					}
					ValuePairs.setValuePair("UserDevice", "");
					ValuePairs.setValuePair("UserGroup", "");
					foreach (DataRow dataRow2 in received._tblUserUAC.Rows)
					{
						int num2 = Convert.ToInt32(dataRow2.ItemArray[0].ToString());
						string text5 = dataRow2.ItemArray[1].ToString();
						if (num2 == 0)
						{
							string[] array = text5.Split(new char[]
							{
								';'
							}, StringSplitOptions.RemoveEmptyEntries);
							if (array.Length > 0)
							{
								string[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									string text6 = array2[i];
									string[] array3 = text6.Split(new char[]
									{
										':'
									}, StringSplitOptions.RemoveEmptyEntries);
									if (array3.Length > 1)
									{
										if (array3[0].Equals("UserType", StringComparison.InvariantCultureIgnoreCase))
										{
											ValuePairs.setValuePair("UserType", array3[1]);
										}
										else
										{
											if (array3[0].Equals("DeviceList", StringComparison.InvariantCultureIgnoreCase))
											{
												ValuePairs.setValuePair("UserDevice", array3[1]);
											}
											else
											{
												if (array3[0].Equals("GroupList", StringComparison.InvariantCultureIgnoreCase))
												{
													ValuePairs.setValuePair("UserGroup", array3[1]);
												}
											}
										}
									}
								}
							}
						}
						else
						{
							List<long> list = new List<long>();
							if (!string.IsNullOrEmpty(text5))
							{
								string[] array4 = text5.Split(new char[]
								{
									','
								});
								string[] array2 = array4;
								for (int i = 0; i < array2.Length; i++)
								{
									string value = array2[i];
									if (!string.IsNullOrEmpty(value))
									{
										long item = (long)((ulong)Convert.ToUInt32(value));
										if (!list.Contains(item))
										{
											list.Add(item);
										}
									}
								}
							}
							ClientAPI._UserUAC[(long)num2] = list;
						}
					}
				}
				received._tblUserUAC.Dispose();
				received._tblUserUAC = null;
				num |= 8;
			}
			if (received._tblDevice2Rack != null)
			{
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._devID2Rack == null)
					{
						ClientAPI._devID2Rack = new Dictionary<long, CommParaClass>();
					}
					else
					{
						ClientAPI._devID2Rack.Clear();
					}
					foreach (DataRow dataRow3 in received._tblDevice2Rack.Rows)
					{
						int num3 = Convert.ToInt32(dataRow3.ItemArray[0].ToString());
						if (!ClientAPI._devID2Rack.ContainsKey((long)num3))
						{
							CommParaClass commParaClass = new CommParaClass();
							commParaClass.Long_First = Convert.ToInt64(dataRow3.ItemArray[1].ToString());
							commParaClass.String_First = dataRow3.ItemArray[2].ToString();
							ClientAPI._devID2Rack.Add((long)num3, commParaClass);
						}
					}
				}
				received._tblDevice2Rack.Dispose();
				received._tblDevice2Rack = null;
				num |= 4;
			}
			if (received._tblRackInfo != null)
			{
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._aryRackInfo == null)
					{
						ClientAPI._aryRackInfo = new Dictionary<int, RackInfo>();
					}
					else
					{
						ClientAPI._aryRackInfo.Clear();
					}
					foreach (DataRow dataRow4 in received._tblRackInfo.Rows)
					{
						int num4 = Convert.ToInt32(dataRow4.ItemArray[0].ToString());
						RackInfo value2 = new RackInfo((long)num4, dataRow4.ItemArray[1].ToString(), (int)dataRow4.ItemArray[2], (int)dataRow4.ItemArray[3], (int)dataRow4.ItemArray[4], (int)dataRow4.ItemArray[5], dataRow4.ItemArray[6].ToString(), dataRow4.ItemArray[7].ToString());
						ClientAPI._aryRackInfo.Add(num4, value2);
					}
				}
				received._tblRackInfo.Dispose();
				received._tblRackInfo = null;
				num |= 4;
			}
			if (received._tblPuePair != null)
			{
				lock (ClientAPI._lockLocalAccess)
				{
					if (ClientAPI._pueValuePair == null)
					{
						ClientAPI._pueValuePair = new Dictionary<string, string>();
					}
					else
					{
						ClientAPI._pueValuePair.Clear();
					}
					if (received._tblPuePair.Columns.Count >= 2)
					{
						for (int j = 0; j < received._tblPuePair.Rows.Count; j++)
						{
							string text7 = "";
							if (received._tblPuePair.Rows[j].ItemArray[0] != DBNull.Value)
							{
								text7 = (string)received._tblPuePair.Rows[j].ItemArray[0];
							}
							string value3 = "";
							if (received._tblPuePair.Rows[j].ItemArray[1] != DBNull.Value)
							{
								value3 = (string)received._tblPuePair.Rows[j].ItemArray[1];
							}
							if (text7.Trim() != "")
							{
								ClientAPI._pueValuePair[text7.Trim()] = value3;
							}
						}
					}
				}
				received._tblPuePair.Dispose();
				received._tblPuePair = null;
				num |= 4096;
			}
			if (received._sysParamRequest != null)
			{
				lock (ClientAPI._lockLocalAccess)
				{
					if (ClientAPI._sysValuePair == null)
					{
						ClientAPI._sysValuePair = new Dictionary<string, string>();
					}
					else
					{
						ClientAPI._sysValuePair.Clear();
					}
					if (received._sysParamRequest.Columns.Count >= 2)
					{
						for (int k = 0; k < received._sysParamRequest.Rows.Count; k++)
						{
							string text8 = (string)received._sysParamRequest.Rows[k].ItemArray[0];
							string text9 = "";
							if (received._sysParamRequest.Rows[k].ItemArray[1] != DBNull.Value)
							{
								text9 = (string)received._sysParamRequest.Rows[k].ItemArray[1];
							}
							if (text8.Trim() != "")
							{
								text8 = text8.Trim();
								ClientAPI._sysValuePair[text8] = text9;
								if (text8.Equals("MAC_CONFLICT", StringComparison.InvariantCultureIgnoreCase))
								{
									Dictionary<string, string> dictionary = new Dictionary<string, string>();
									dictionary.Clear();
									string[] array5 = text9.Split(new char[]
									{
										'+'
									}, StringSplitOptions.RemoveEmptyEntries);
									for (int l = 0; l < array5.Length; l++)
									{
										string[] array6 = array5[l].Split(new char[]
										{
											'_'
										});
										if (array6.Length == 2)
										{
											dictionary.Add(array6[0], array6[1]);
										}
									}
									if (dictionary != null)
									{
										ClientAPI._macConflictList = dictionary;
									}
								}
							}
						}
					}
				}
				received._sysParamRequest.Dispose();
				num |= 32;
			}
			if (received._dsSqlRequest != null)
			{
				Common.WriteLine("Got SQL response @ {0}", new string[]
				{
					DateTime.Now.ToLongTimeString()
				});
				lock (ClientAPI._lockLocalAccess)
				{
					if (ClientAPI._sqlQuery == null)
					{
						ClientAPI._sqlQuery = new Dictionary<string, ClientAPI.SqlQueryContext>();
					}
					string[] array7 = received._dsSqlRequest.Tables[0].TableName.Split(new char[]
					{
						'/'
					});
					if (array7.Length > 0 && ClientAPI._sqlQuery.ContainsKey(array7[0]))
					{
						if (ClientAPI._sqlQuery[array7[0]].dsResult != null)
						{
							ClientAPI._sqlQuery[array7[0]].dsResult.Dispose();
						}
						ClientAPI._sqlQuery[array7[0]].nResultCode = 1;
						ClientAPI._sqlQuery[array7[0]].dsResult = received._dsSqlRequest;
						ClientAPI._sqlQuery[array7[0]].tReceived = (long)Environment.TickCount;
						Common.WriteLine("Remote SQL elapsed={0} ms", new string[]
						{
							Common.ElapsedTime(ClientAPI._sqlQuery[array7[0]].tSend).ToString()
						});
					}
					received._dsSqlRequest = null;
				}
			}
			if (received._dsDBBase != null)
			{
				for (int m = 0; m < received._dsDBBase.Tables.Count; m++)
				{
					if (!received._dsDBBase.Tables[m].TableName.Equals("device", StringComparison.CurrentCultureIgnoreCase))
					{
						DataColumn dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType("System.Int32");
						dataColumn.ColumnName = "rack_id";
						dataColumn.DefaultValue = 0;
						received._dsDBBase.Tables[m].Columns.Add(dataColumn);
					}
					if (received._dsDBBase.Tables[m].TableName.Equals("sensor", StringComparison.CurrentCultureIgnoreCase))
					{
						if (!received._dsDBBase.Tables[m].Columns.Contains("max_humidity"))
						{
							DataColumn dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_humidity";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_humidity";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_temperature";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_temperature";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_press";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_press";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
						}
					}
					else
					{
						if (!received._dsDBBase.Tables[m].Columns.Contains("max_voltage"))
						{
							DataColumn dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_voltage";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_voltage";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_power_diss";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_power_diss";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_power";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_power";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "max_current";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
							dataColumn = new DataColumn();
							dataColumn.DataType = Type.GetType("System.Single");
							dataColumn.ColumnName = "min_current";
							dataColumn.DefaultValue = DataSetManager.ValueEmpty;
							received._dsDBBase.Tables[m].Columns.Add(dataColumn);
						}
						if (!received._dsDBBase.Tables[m].TableName.Equals("device", StringComparison.CurrentCultureIgnoreCase))
						{
							received._dsDBBase.Tables[m].TableName.Equals("port", StringComparison.CurrentCultureIgnoreCase);
							received._dsDBBase.Tables[m].TableName.Equals("bank", StringComparison.CurrentCultureIgnoreCase);
							received._dsDBBase.Tables[m].TableName.Equals("line", StringComparison.CurrentCultureIgnoreCase);
						}
					}
				}
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._datasetFromRemote != null)
					{
						ClientAPI._datasetFromRemote.Dispose();
					}
					ClientAPI._datasetFromRemote = received._dsDBBase;
					if (ClientAPI._baseIncrementalRmote != null)
					{
						ClientAPI._baseIncrementalRmote.Clear();
					}
					else
					{
						ClientAPI._baseIncrementalRmote = ClientAPI._datasetFromRemote.Clone();
					}
				}
				received._dsDBBase = null;
				num |= 2;
			}
			if (received._dsDBIncremental != null)
			{
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._baseIncrementalRmote != null)
					{
						ClientAPI._baseIncrementalRmote.Dispose();
					}
					ClientAPI._baseIncrementalRmote = received._dsDBIncremental;
				}
				received._dsDBIncremental = null;
				num |= 128;
			}
			if (received._dsRealtime != null)
			{
				for (int n = 0; n < received._dsRealtime.Tables.Count; n++)
				{
					if (received._dsRealtime.Tables[n].TableName.Equals("bank", StringComparison.CurrentCultureIgnoreCase) && received._dsRealtime.Tables[n].Columns.Contains("bank_number"))
					{
						received._dsRealtime.Tables[n].Columns.Remove("bank_number");
					}
				}
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._dsWorkRealtime != null)
					{
						if (received._tblSequenceNo > ClientAPI._lastRealTimeSeq)
						{
							ClientAPI._dsWorkRealtime.Dispose();
						}
					}
					else
					{
						if (ClientAPI._pushChannel != null)
						{
							ClientAPI._pushChannel.RequestDataset(64);
						}
					}
					if (received._tblSequenceNo > ClientAPI._lastRealTimeSeq)
					{
						ClientAPI._dsWorkRealtime = received._dsRealtime;
						ClientAPI._lastRealTimeSeq = received._tblSequenceNo;
					}
					else
					{
						Common.WriteLine("        Current realtime data is old", new string[0]);
					}
					if (ClientAPI._dsWorkIncremental != null)
					{
						ClientAPI._dsWorkIncremental.Clear();
					}
				}
				received._dsRealtime = null;
				num |= 1;
			}
			if (received._dsIncremental != null)
			{
				for (int num5 = 0; num5 < received._dsIncremental.Tables.Count; num5++)
				{
					if (received._dsIncremental.Tables[num5].TableName.Equals(DataSetManager.tb_udBank, StringComparison.CurrentCultureIgnoreCase) && received._dsIncremental.Tables[num5].Columns.Contains("bank_number"))
					{
						received._dsIncremental.Tables[num5].Columns.Remove("bank_number");
					}
				}
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._dsWorkIncremental != null)
					{
						ClientAPI._dsWorkIncremental.Dispose();
					}
					ClientAPI._dsWorkIncremental = received._dsIncremental;
				}
				received._dsIncremental = null;
				num |= 64;
			}
			long num6 = Common.ElapsedTime(tLast);
			tLast = (long)Environment.TickCount;
			if ((num & 128) != 0)
			{
				lock (ClientAPI._lockDataset)
				{
					if (ClientAPI._datasetFromRemote != null && ClientAPI._baseIncrementalRmote != null && ClientAPI._baseIncrementalRmote.Tables.Count > 0)
					{
						for (int num7 = 0; num7 < ClientAPI._baseIncrementalRmote.Tables.Count; num7++)
						{
							ClientAPI._datasetFromRemote.Tables[num7].Merge(ClientAPI._baseIncrementalRmote.Tables[num7], false, MissingSchemaAction.AddWithKey);
						}
						Common.WriteLine("        Base merged with diff., elapsed={0}", new string[]
						{
							Common.ElapsedTime(tLast).ToString()
						});
						tLast = (long)Environment.TickCount;
					}
				}
			}
			if ((num & 130) != 0)
			{
				lock (ClientAPI._lockDataset)
				{
				}
			}
			if ((num & 134) != 0)
			{
				lock (ClientAPI._lockDataset)
				{
					Dictionary<long, long> dictionary2 = new Dictionary<long, long>();
					if (ClientAPI._datasetFromRemote != null)
					{
						for (int num8 = 0; num8 < ClientAPI._datasetFromRemote.Tables.Count; num8++)
						{
							int columnIndex = ClientAPI._datasetFromRemote.Tables[num8].Columns.IndexOf("device_id");
							int columnIndex2 = ClientAPI._datasetFromRemote.Tables[num8].Columns.IndexOf("rack_id");
							foreach (DataRow dataRow5 in ClientAPI._datasetFromRemote.Tables[num8].Rows)
							{
								long key = Convert.ToInt64(dataRow5[columnIndex]);
								if (num8 == 0)
								{
									long value4 = Convert.ToInt64(dataRow5[columnIndex2]);
									if (!dictionary2.ContainsKey(key))
									{
										dictionary2[key] = value4;
									}
								}
								else
								{
									if (dictionary2.ContainsKey(key))
									{
										dataRow5[columnIndex2] = dictionary2[key];
									}
								}
							}
						}
						num6 = Common.ElapsedTime(tLast);
						tLast = (long)Environment.TickCount;
					}
				}
			}
			if ((num & 199) != 0)
			{
				DataSet dataSet = null;
				Dictionary<int, int> dictionary3 = null;
				lock (ClientAPI._lockDataset)
				{
					ClientAPI.SetAllDeviceOffline();
					tLast = (long)Environment.TickCount;
					if (ClientAPI._datasetFromRemote != null && ClientAPI._dsWorkRealtime != null)
					{
						string text10 = "";
						try
						{
							for (int num9 = 0; num9 < ClientAPI._datasetFromRemote.Tables.Count; num9++)
							{
								int count = ClientAPI._datasetFromRemote.Tables[num9].Rows.Count;
								text10 = ClientAPI._datasetFromRemote.Tables[num9].TableName;
								if (num9 < ClientAPI._dsWorkRealtime.Tables.Count && (ClientAPI._dsWorkIncremental == null || num9 < ClientAPI._dsWorkIncremental.Tables.Count))
								{
									if (count != ClientAPI._dsWorkRealtime.Tables[num9].Rows.Count)
									{
										Common.WriteLine("    Before merge: WK={0}, RT={1} ", new string[]
										{
											ClientAPI._datasetFromRemote.Tables[num9].Rows.Count.ToString(),
											ClientAPI._dsWorkRealtime.Tables[num9].Rows.Count.ToString()
										});
									}
									ClientAPI._datasetFromRemote.Tables[num9].Merge(ClientAPI._dsWorkRealtime.Tables[num9], false, MissingSchemaAction.AddWithKey);
									if (ClientAPI._dsWorkIncremental != null && ClientAPI._dsWorkIncremental.Tables.Count > num9)
									{
										ClientAPI._datasetFromRemote.Tables[num9].Merge(ClientAPI._dsWorkIncremental.Tables[num9], false, MissingSchemaAction.AddWithKey);
									}
									DataRow[] array8 = ClientAPI._datasetFromRemote.Tables[num9].Select("rack_id is null or rack_id <= 0");
									if (array8 != null && array8.Length > 0)
									{
										for (int num10 = 0; num10 < array8.Length; num10++)
										{
											array8[num10].Delete();
										}
										ClientAPI._datasetFromRemote.Tables[num9].AcceptChanges();
										Common.WriteLine("    After merged: WK={0}, RT={1}, DEL={2} ", new string[]
										{
											ClientAPI._datasetFromRemote.Tables[num9].Rows.Count.ToString(),
											ClientAPI._dsWorkRealtime.Tables[num9].Rows.Count.ToString(),
											array8.Length.ToString()
										});
									}
								}
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("Failed to merge with [{0}]: {1}", new string[]
							{
								text10,
								ex.Message
							});
						}
						Common.WriteLine("        Merge with realtime data, elapsed={0}", new string[]
						{
							Common.ElapsedTime(tLast).ToString()
						});
						tLast = (long)Environment.TickCount;
					}
					if (ClientAPI._datasetFromRemote != null)
					{
						long tLast2 = (long)Environment.TickCount;
						int columnIndex3 = 0;
						int columnIndex4 = 0;
						dataSet = new DataSet();
						dictionary3 = new Dictionary<int, int>();
						for (int num11 = 0; num11 < ClientAPI._datasetFromRemote.Tables.Count; num11++)
						{
							DataTable dataTable = ClientAPI._datasetFromRemote.Tables[num11].Clone();
							dataTable.PrimaryKey = null;
							if (num11 == 0)
							{
								columnIndex3 = ClientAPI._datasetFromRemote.Tables[num11].Columns.IndexOf("device_id");
								columnIndex4 = ClientAPI._datasetFromRemote.Tables[num11].Columns.IndexOf("device_state");
							}
							foreach (DataRow dataRow6 in ClientAPI._datasetFromRemote.Tables[num11].Rows)
							{
								dataTable.ImportRow(dataRow6);
								if (num11 == 0)
								{
									dictionary3[Convert.ToInt32(dataRow6[columnIndex3])] = Convert.ToInt32(dataRow6[columnIndex4]);
								}
							}
							dataSet.Tables.Add(dataTable);
						}
						dataSet.Tables[0].PrimaryKey = new DataColumn[]
						{
							dataSet.Tables[0].Columns["device_id"]
						};
						dataSet.Tables[1].PrimaryKey = new DataColumn[]
						{
							dataSet.Tables[1].Columns["device_id"],
							dataSet.Tables[1].Columns["sensor_type"]
						};
						dataSet.Tables[2].PrimaryKey = new DataColumn[]
						{
							dataSet.Tables[2].Columns["device_id"],
							dataSet.Tables[2].Columns["port_number"],
							dataSet.Tables[2].Columns["port_id"]
						};
						dataSet.Tables[3].PrimaryKey = new DataColumn[]
						{
							dataSet.Tables[3].Columns["device_id"],
							dataSet.Tables[3].Columns["bank_id"]
						};
						if (dataSet.Tables.Count > 4)
						{
							dataSet.Tables[4].PrimaryKey = new DataColumn[]
							{
								dataSet.Tables[4].Columns["device_id"],
								dataSet.Tables[4].Columns["line_id"]
							};
						}
						Common.WriteLine("    Make Dataset Copy: elapsed={0}", new string[]
						{
							Common.ElapsedTime(tLast2).ToString()
						});
					}
				}
				if (dataSet != null)
				{
					lock (ClientAPI._lockLocalAccess)
					{
						Interlocked.Exchange<DataSet>(ref ClientAPI._workingDataSet, dataSet);
						Interlocked.Exchange<Dictionary<int, int>>(ref ClientAPI._deviceOnlineStatus, dictionary3);
						for (int num12 = 0; num12 < ClientAPI._updateFlag.Length; num12++)
						{
							ClientAPI._updateFlag[num12] = 1;
						}
					}
				}
				num6 = Common.ElapsedTime(tLast);
			}
			if (received._dcLayout >= 0)
			{
				ClientAPI._dcLayout = received._dcLayout;
			}
			int num13 = Convert.ToInt32(ValuePairs.getValuePair("UserType"));
			if (num13 != 1)
			{
				if (ClientAPI._sysValuePair == null)
				{
					if (attrib != null && (ClientAPI._nRequestingFlag & 32) == 0)
					{
						attrib.owner.RequestDataset(32);
						ClientAPI._nRequestingFlag |= 32;
					}
				}
				else
				{
					if (ClientAPI._aryRackInfo == null)
					{
						if (attrib != null && (ClientAPI._nRequestingFlag & 4) == 0)
						{
							attrib.owner.RequestDataset(4);
							ClientAPI._nRequestingFlag |= 4;
						}
					}
					else
					{
						if (ClientAPI._datasetFromRemote == null)
						{
							if (attrib != null && (ClientAPI._nRequestingFlag & 2) == 0)
							{
								attrib.owner.RequestDataset(2);
								ClientAPI._nRequestingFlag |= 2;
							}
						}
						else
						{
							if (!ClientAPI._autoModelDone)
							{
								if (attrib != null && (ClientAPI._nRequestingFlag & 2048) == 0)
								{
									attrib.owner.RequestDataset(2048);
									ClientAPI._nRequestingFlag |= 2048;
								}
							}
							else
							{
								if (ClientAPI._workingZoneInfo == null)
								{
									if (attrib != null && (ClientAPI._nRequestingFlag & 256) == 0)
									{
										attrib.owner.RequestDataset(256);
										ClientAPI._nRequestingFlag |= 256;
									}
								}
								else
								{
									if (ClientAPI._workingGroupInfo == null)
									{
										if (attrib != null && (ClientAPI._nRequestingFlag & 512) == 0)
										{
											attrib.owner.RequestDataset(512);
											ClientAPI._nRequestingFlag |= 512;
										}
									}
									else
									{
										if (ClientAPI._baseIncrementalRmote == null)
										{
											if (attrib != null && (ClientAPI._nRequestingFlag & 128) == 0)
											{
												attrib.owner.RequestDataset(128);
												ClientAPI._nRequestingFlag |= 128;
											}
										}
										else
										{
											if (ClientAPI._pueValuePair == null)
											{
												if (attrib != null && (ClientAPI._nRequestingFlag & 4096) == 0)
												{
													attrib.owner.RequestDataset(4096);
													ClientAPI._nRequestingFlag |= 4096;
												}
											}
											else
											{
												if (ClientAPI._dsWorkRealtime == null && (ClientAPI._nRequestingFlag & 1) == 0)
												{
													attrib.owner.RequestDataset(1);
													ClientAPI._nRequestingFlag |= 1;
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
			if ((num & 4096) != 0)
			{
				ClientAPI._pueCounter++;
				if (ClientAPI._pueCounter > 3)
				{
					ClientAPI._pueCounter = 0;
					lock (ClientAPI._lockDataset)
					{
						ClientAPI.SetAllDeviceOffline();
					}
					Common.WriteLine("Set all devices to OFFLINE", new string[0]);
				}
			}
			if ((num & 65) != 0)
			{
				ClientAPI._pueCounter = 0;
			}
			if (ClientAPI._pushChannel != null && ClientAPI._loginState > 0)
			{
				if ((num & 130) != 0)
				{
					ClientAPI.OnThresholdChanged(new ClientAPI.ThresholdArgs(null));
				}
				if ((num & 65) != 0)
				{
					ClientAPI.OnRealtimeChanged(new ClientAPI.RealtimeDataArgs(null));
				}
				if ((num & 64) != 0)
				{
					ClientAPI.OnPendingChanged(new ClientAPI.PendingStatusArgs("Pending List"));
				}
				if ((num & 4) != 0)
				{
					ClientAPI.OnRackInfoChanged(new ClientAPI.RackInfoArgs(null));
				}
				if ((num & 4096) != 0)
				{
					ClientAPI.OnPueChanged(new ClientAPI.PUEChangedArgs(null));
				}
				if ((num & 8192) != 0)
				{
					ClientAPI.OnBroadcastChanged(new ClientAPI.BroadcastArgs(null));
				}
			}
			double num14 = 0.0;
			double num15 = 0.0;
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.getSpeed(ref num14, ref num15);
			}
			if (ClientAPI._requestChannel != null)
			{
				ClientAPI._requestChannel.getSpeed(ref num14, ref num15);
			}
			return num;
		}
		private static bool IsNewDataSet(int from)
		{
			bool flag = ClientAPI._workingDataSet != null;
			if (from <= 0)
			{
				return flag;
			}
			flag = (flag && ClientAPI._updateFlag[from] > 0);
			ClientAPI._updateFlag[from] = 0;
			return flag;
		}
		private static int getLoginState(bool authOnly)
		{
			if (ClientAPI._pushChannel == null)
			{
				return -2;
			}
			int result;
			lock (ClientAPI._lockDataset)
			{
				int num = 65535;
				int loginState = ClientAPI._pushChannel.getLoginState();
				if (loginState < 0)
				{
					num = loginState;
				}
				else
				{
					if (loginState > 0)
					{
						if (authOnly)
						{
							num = 0;
						}
						else
						{
							if (ClientAPI._workingDataSet != null && ClientAPI._aryRackInfo != null && ClientAPI._sysValuePair != null && ClientAPI._autoModelDone && ClientAPI._workingZoneInfo != null && ClientAPI._workingGroupInfo != null)
							{
								num = 0;
							}
							if (ClientAPI._workingDataSet != null)
							{
								ClientAPI._loginItemMask |= 1L;
							}
							if (ClientAPI._aryRackInfo != null)
							{
								ClientAPI._loginItemMask |= 2L;
							}
							if (ClientAPI._sysValuePair != null)
							{
								ClientAPI._loginItemMask |= 4L;
							}
							if (ClientAPI._autoModelDone)
							{
								ClientAPI._loginItemMask |= 8L;
							}
							if (ClientAPI._workingZoneInfo != null)
							{
								ClientAPI._loginItemMask |= 16L;
							}
							if (ClientAPI._workingGroupInfo != null)
							{
								ClientAPI._loginItemMask |= 32L;
							}
						}
					}
				}
				result = num;
			}
			return result;
		}
		public static string getPueValue(string key)
		{
			string result;
			lock (ClientAPI._lockLocalAccess)
			{
				string text = "0";
				if (ClientAPI._pueValuePair != null && ClientAPI._pueValuePair.ContainsKey(key))
				{
					text = ClientAPI._pueValuePair[key];
				}
				result = text;
			}
			return result;
		}
		public static Dictionary<string, string> getSysValuePairs()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> current in ClientAPI._sysValuePair)
			{
				dictionary.Add(current.Key, current.Value);
			}
			return dictionary;
		}
		public static string getKeyValue(string key)
		{
			string result;
			lock (ClientAPI._lockLocalAccess)
			{
				string text = "";
				if (ClientAPI._sysValuePair != null && ClientAPI._sysValuePair.ContainsKey(key))
				{
					text = ClientAPI._sysValuePair[key];
				}
				result = text;
			}
			return result;
		}
		public static int getRackLayout()
		{
			int dcLayout;
			lock (ClientAPI._lockDataset)
			{
				dcLayout = ClientAPI._dcLayout;
			}
			return dcLayout;
		}
		public static void Logout()
		{
			lock (ClientAPI._lockClient)
			{
				if (ClientAPI._pushChannel != null)
				{
					ClientAPI._pushChannel.SendLogout();
					try
					{
						Thread.Sleep(500);
					}
					catch (Exception)
					{
					}
				}
			}
		}
		public static void NotifyDCReloadDS(ulong idMessage, string attached, string operation)
		{
			Common.WriteLine("Notify to Server: type={0}", new string[]
			{
				idMessage.ToString("X8")
			});
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<Message>\r\n";
			text = text + "<attached>" + attached + "</attached>\r\n";
			string text2 = Guid.NewGuid().ToString();
			text2 = text2.ToUpper();
			text = text + "<guid>" + text2 + "</guid>\r\n";
			text = text + "<alltype>" + idMessage.ToString() + "</alltype>\r\n";
			text = text + "<operation>" + operation + "</operation>\r\n";
			text += "</Message>\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			int num = Convert.ToInt32(idMessage);
			if ((num & 8192) != 0)
			{
				if (ClientAPI._pushChannel != null)
				{
					ClientAPI._pushChannel.SendMessage(8192, true, bytes);
				}
				num &= -8193;
			}
			if (num != 0 && ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.SendMessage(num, true, bytes);
			}
		}
		public static int getSQLCode(string strID)
		{
			int result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._sqlQuery == null)
				{
					result = -1;
				}
				else
				{
					if (!ClientAPI._sqlQuery.ContainsKey(strID))
					{
						result = -1;
					}
					else
					{
						result = ClientAPI._sqlQuery[strID].nResultCode;
					}
				}
			}
			return result;
		}
		public static DataSet getSQLResult(string strID)
		{
			DataSet result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._sqlQuery == null)
				{
					result = null;
				}
				else
				{
					if (!ClientAPI._sqlQuery.ContainsKey(strID))
					{
						result = null;
					}
					else
					{
						if (ClientAPI._sqlQuery[strID].nResultCode > 0)
						{
							DataSet dataSet = ClientAPI._sqlQuery[strID].dsResult.Copy();
							ClientAPI._sqlQuery[strID].dsResult.Dispose();
							ClientAPI._sqlQuery.Remove(strID);
							result = dataSet;
						}
						else
						{
							result = null;
						}
					}
				}
			}
			return result;
		}
		public static void OnConnected(Socket sock)
		{
			Common.WriteLine("OnConnected: {0}", new string[]
			{
				sock.LocalEndPoint.ToString()
			});
			lock (ClientAPI._lockClient)
			{
				if (ClientAPI.cbOnConnected != null)
				{
					ClientAPI.cbOnConnected(sock);
				}
			}
		}
		public static void OnClosed(ConnectContext context, int code)
		{
			lock (ClientAPI._lockDataset)
			{
				ClientAPI._loginState = -1;
				if (!context._bKicked)
				{
					if (!context._bServiceWillDown)
					{
						goto IL_FA;
					}
				}
				try
				{
					if (ClientAPI._workingDataSet != null && ClientAPI._dsWorkRealtime != null)
					{
						int columnIndex = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("device_state");
						int columnIndex2 = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("doorsensor_status");
						for (int i = 0; i < ClientAPI._workingDataSet.Tables[0].Rows.Count; i++)
						{
							ClientAPI._workingDataSet.Tables[0].Rows[i][columnIndex] = 0;
							ClientAPI._workingDataSet.Tables[0].Rows[i][columnIndex2] = DataSetManager.PDUData_Doorst_noattach;
						}
					}
				}
				catch (Exception)
				{
				}
				IL_FA:;
			}
			if (!ClientAPI._bOnClosedEnabled)
			{
				Common.WriteLine("    ClientAPI Onclosed(): disabled, uid={0}, enable={1}, ptr={2}, vid={3}", new string[]
				{
					context._uid.ToString(),
					ClientAPI._bOnClosedEnabled.ToString(),
					(ClientAPI.cbOnClosed != null).ToString(),
					context._vid.ToString()
				});
				return;
			}
			Common.WriteLine("    ClientAPI Onclosed(): enabled, uid={0}, enable={1}, ptr={2}, vid={3}", new string[]
			{
				context._uid.ToString(),
				ClientAPI._bOnClosedEnabled.ToString(),
				(ClientAPI.cbOnClosed != null).ToString(),
				context._vid.ToString()
			});
			if (context._bKicked)
			{
				Common.WriteLine("    ClientAPI Onclosed on kicked", new string[0]);
				if (ClientAPI.cbOnClosed != null)
				{
					ClientAPI.cbOnClosed(-2);
					return;
				}
			}
			else
			{
				if (context._bServiceWillDown)
				{
					Common.WriteLine("    ClientAPI Onclosed while service down", new string[0]);
					if (ClientAPI.cbOnClosed != null)
					{
						ClientAPI.cbOnClosed(-1);
						return;
					}
				}
				else
				{
					Common.WriteLine("    ClientAPI Onclosed unexpectedly", new string[0]);
					ClientAPI.ReConnect();
				}
			}
		}
		public static List<GroupInfo> getGroupCopy()
		{
			List<GroupInfo> list = new List<GroupInfo>();
			List<GroupInfo> result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingGroupInfo != null)
				{
					for (int i = 0; i < ClientAPI._workingGroupInfo.Rows.Count; i++)
					{
						DataRow dataRow = ClientAPI._workingGroupInfo.Rows[i];
						list.Add(new GroupInfo
						{
							ID = (long)((ulong)Convert.ToUInt32(dataRow["id"])),
							GroupName = Convert.ToString(dataRow["groupname"]),
							GroupType = Convert.ToString(dataRow["grouptype"]),
							LineColor = Convert.ToString(dataRow["linecolor"]),
							SelectedFlag = Convert.ToInt32(dataRow["isselect"]),
							ThermalFlag = Convert.ToInt32(dataRow["thermalflag"]),
							BillFlag = Convert.ToInt32(dataRow["billflag"]),
							Members = Convert.ToString(dataRow["members"])
						});
					}
				}
				result = list;
			}
			return result;
		}
		private static ArrayList getRackInfoCopy()
		{
			ArrayList result;
			lock (ClientAPI._lockDataset)
			{
				ArrayList arrayList = new ArrayList();
				if (ClientAPI._aryRackInfo != null)
				{
					foreach (KeyValuePair<int, RackInfo> current in ClientAPI._aryRackInfo)
					{
						RackInfo copy = current.Value.getCopy();
						arrayList.Add(copy);
					}
				}
				result = arrayList;
			}
			return result;
		}
		public static void SendAutoModelBatch(List<DevModelConfig> newModels)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<AutoModelUpdate>\r\n";
			foreach (DevModelConfig current in newModels)
			{
				text += "<ModelDefine>\r\n";
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"<model_key>",
					current.modelName,
					"-",
					current.firmwareVer,
					"</model_key>\r\n"
				});
				text = text + "<modelName>" + current.modelName + "</modelName>\r\n";
				text = text + "<firmwareVer>" + current.firmwareVer + "</firmwareVer>\r\n";
				text = text + "<autoBasicInfo>" + current.autoBasicInfo + "</autoBasicInfo>\r\n";
				text = text + "<autoRatingInfo>" + current.autoRatingInfo + "</autoRatingInfo>\r\n";
				text += "</ModelDefine>\r\n";
			}
			text += "</AutoModelUpdate>\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.SendMessage(2048, true, bytes);
			}
			Common.WriteLine("    Auto Model Operation Request:\r\n{0}", new string[]
			{
				text
			});
		}
		public static void SendAutoModelUpdated(string modeKey, DevModelConfig cfg)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<AutoModelUpdate>\r\n";
			text = text + "<model_key>" + modeKey + "</model_key>\r\n";
			text = text + "<modelName>" + cfg.modelName + "</modelName>\r\n";
			text = text + "<firmwareVer>" + cfg.firmwareVer + "</firmwareVer>\r\n";
			text = text + "<autoBasicInfo>" + cfg.autoBasicInfo + "</autoBasicInfo>\r\n";
			text = text + "<autoRatingInfo>" + cfg.autoRatingInfo + "</autoRatingInfo>\r\n";
			text += "</AutoModelUpdate>\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.SendMessage(2048, true, bytes);
			}
			Common.WriteLine("    Auto Model Operation Request:\r\n{0}", new string[]
			{
				text
			});
		}
		public static void SendDeviceOperation(string operation, string targetlist)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<DeviceOperation>\r\n";
			text = text + "<operation>" + operation + "</operation>\r\n";
			text = text + "<targets>" + targetlist + "</targets>\r\n";
			text += "</DeviceOperation>\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.SendMessage(64, true, bytes);
			}
			Common.WriteLine("    Device Operation Request:\r\n{0}", new string[]
			{
				text
			});
		}
		public static bool RestartListener(bool bSSL, int newPort)
		{
			string strCommand = string.Format("<RestartListener>{0},{1}</RestartListener>", bSSL ? "1" : "0", newPort);
			return ClientAPI.SendCommand4Service(strCommand);
		}
		private static bool SendCommand4Service(string strCommand)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<Command4Service>\r\n";
			text += strCommand;
			text += "\r\n";
			text += "</Command4Service>\r\n";
			if (ClientAPI._pushChannel != null)
			{
				ClientAPI._pushChannel.SendMsg4Service(text);
				Common.WriteLine("    SendCommand4Service:\r\n{0}", new string[]
				{
					strCommand
				});
				return true;
			}
			return false;
		}
		public static void SetBroadcastCallback(CommonAPI.DelegateOnBroadcast cb)
		{
			Common.WriteLine("Set Broadcast callback", new string[0]);
			ClientAPI.cbOnBroadcast = cb;
		}
		public static void SetConnectedCallback(CommonAPI.DelegateOnConnected cb)
		{
			Common.WriteLine("Set Connected callback", new string[0]);
			ClientAPI.cbOnConnected = cb;
		}
		public static void SetClosedCallback(CommonAPI.DelegateOnClosed cb)
		{
			Common.WriteLine("Set OnClose callback", new string[0]);
			ClientAPI.cbOnClosed = cb;
		}
		public static int getDeviceCount()
		{
			int result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._deviceOnlineStatus == null)
				{
					result = 0;
				}
				else
				{
					result = ClientAPI._deviceOnlineStatus.Count;
				}
			}
			return result;
		}
		public static bool IsDeviceExisted(int device_id)
		{
			bool result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._deviceOnlineStatus == null)
				{
					result = false;
				}
				else
				{
					result = ClientAPI._deviceOnlineStatus.ContainsKey(device_id);
				}
			}
			return result;
		}
		public static bool IsDeviceOnline(int device_id)
		{
			bool result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._deviceOnlineStatus == null)
				{
					result = false;
				}
				else
				{
					if (!ClientAPI._deviceOnlineStatus.ContainsKey(device_id))
					{
						result = false;
					}
					else
					{
						result = (ClientAPI._deviceOnlineStatus[device_id] != 0);
					}
				}
			}
			return result;
		}
		public static int updateDataSet(ref DataSet inDS, int from)
		{
			long tLast = (long)Environment.TickCount;
			int result;
			lock (ClientAPI._lockLocalAccess)
			{
				if (from > 0 && !ClientAPI.IsNewDataSet(from))
				{
					result = 2;
				}
				else
				{
					if (ClientAPI._workingDataSet == null)
					{
						ClientAPI._workingDataSet = new DataSet();
					}
					inDS = new DataSet();
					if (ClientAPI._workingDataSet.Tables.Count > 0)
					{
						for (int i = 0; i < ClientAPI._workingDataSet.Tables.Count; i++)
						{
							DataTable table = ClientAPI._workingDataSet.Tables[i].Copy();
							inDS.Tables.Add(table);
						}
					}
					long num = Common.ElapsedTime(tLast);
					Common.WriteLine("    updateDataSet from {0}: updated, elapsed={1}", new string[]
					{
						from.ToString(),
						num.ToString()
					});
					result = 1;
				}
			}
			return result;
		}
		public static DataRow[] getDataRows(int tableIndex, string SelectCondition, string sortStr)
		{
			DataRow[] result = new DataRow[0];
			lock (ClientAPI._lockLocalAccess)
			{
				try
				{
					if (tableIndex < ClientAPI._workingDataSet.Tables.Count)
					{
						DataRow[] array = new DataRow[0];
						if (ClientAPI._workingDataSet != null && ClientAPI._workingDataSet.Tables.Count > 0)
						{
							array = ClientAPI._workingDataSet.Tables[tableIndex].Select(SelectCondition, sortStr);
						}
						DataTable dataTable = ClientAPI._workingDataSet.Tables[tableIndex].Clone();
						for (int i = 0; i < array.Length; i++)
						{
							dataTable.ImportRow(array[i]);
						}
						result = dataTable.Select("", sortStr);
					}
				}
				catch (Exception)
				{
				}
			}
			return result;
		}
		private static Dictionary<int, string> getZoneNames()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingZoneInfo != null)
				{
					foreach (DataRow dataRow in ClientAPI._workingZoneInfo.Rows)
					{
						dictionary.Add(Convert.ToInt32(dataRow["id"]), (string)dataRow["zone_nm"]);
					}
				}
			}
			return dictionary;
		}
		public static List<ZoneInfo> getZoneCopy()
		{
			List<ZoneInfo> list = new List<ZoneInfo>();
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingZoneInfo != null)
				{
					foreach (DataRow tmp_dr_z in ClientAPI._workingZoneInfo.Rows)
					{
						ZoneInfo item = new ZoneInfo(tmp_dr_z);
						list.Add(item);
					}
				}
			}
			return list;
		}
		private static Dictionary<int, string> getZoneByRack()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingZoneInfo != null)
				{
					foreach (DataRow dataRow in ClientAPI._workingZoneInfo.Rows)
					{
						try
						{
							object obj = dataRow["racks"];
							if (obj != null && obj != DBNull.Value)
							{
								string text = (string)dataRow["racks"];
								string[] array = text.Split(new char[]
								{
									','
								}, StringSplitOptions.RemoveEmptyEntries);
								string[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									string text2 = array2[i];
									string value = text2.Trim();
									if (!string.IsNullOrEmpty(value))
									{
										int num = Convert.ToInt32(value);
										string text3 = dataRow["id"].ToString();
										if (dictionary.ContainsKey(num))
										{
											Dictionary<int, string> dictionary2;
											int key;
											(dictionary2 = dictionary)[key = num] = dictionary2[key] + "," + text3;
										}
										else
										{
											dictionary[num] = text3;
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("getZoneByRack: {0}", new string[]
							{
								ex.Message
							});
						}
					}
				}
			}
			return dictionary;
		}
		public static Dictionary<long, List<long>> getUserUAC()
		{
			Dictionary<long, List<long>> dictionary = new Dictionary<long, List<long>>();
			lock (ClientAPI._lockDataset)
			{
				if (ClientAPI._UserUAC != null)
				{
					foreach (KeyValuePair<long, List<long>> current in ClientAPI._UserUAC)
					{
						List<long> cloneList = new List<long>();
						current.Value.ForEach(delegate(long item)
						{
							cloneList.Add(item);
						});
						dictionary.Add(current.Key, cloneList);
					}
				}
			}
			return dictionary;
		}
		public static string getRackNameByID(int devid)
		{
			string result = "";
			lock (ClientAPI._lockDataset)
			{
				if (ClientAPI._devID2Rack != null && ClientAPI._devID2Rack.ContainsKey((long)devid))
				{
					return ClientAPI._devID2Rack[(long)devid].String_First;
				}
			}
			return result;
		}
		public static string getRacklistByZonelist(string zonelist)
		{
			StringBuilder stringBuilder = new StringBuilder();
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingZoneInfo != null)
				{
					string filterExpression = string.Format("id in ({0})", zonelist);
					DataRow[] array = ClientAPI._workingZoneInfo.Select(filterExpression);
					DataRow[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DataRow dataRow = array2[i];
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(dataRow["racks"]);
					}
				}
			}
			return stringBuilder.ToString();
		}
		public static Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> GetDevicRackZoneRelation()
		{
			long tLast = (long)Environment.TickCount;
			Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> dictionary = new Dictionary<int, ClientAPI.DeviceWithZoneRackInfo>();
			lock (ClientAPI._lockLocalAccess)
			{
				if (ClientAPI._workingDataSet == null)
				{
					Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> result = dictionary;
					return result;
				}
				if (ClientAPI._workingDataSet.Tables.Count <= 0)
				{
					Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> result = dictionary;
					return result;
				}
			}
			Dictionary<int, string> zoneByRack = ClientAPI.getZoneByRack();
			Dictionary<int, string> zoneNames = ClientAPI.getZoneNames();
			char[] separator = new char[]
			{
				','
			};
			lock (ClientAPI._lockLocalAccess)
			{
				int columnIndex = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("device_id");
				int columnIndex2 = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("device_nm");
				int columnIndex3 = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("model_nm");
				int columnIndex4 = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("fw_version");
				int columnIndex5 = ClientAPI._workingDataSet.Tables[0].Columns.IndexOf("rack_id");
				foreach (DataRow dataRow in ClientAPI._workingDataSet.Tables[0].Rows)
				{
					try
					{
						int num = Convert.ToInt32(dataRow[columnIndex]);
						string device_nm = (string)dataRow[columnIndex2];
						string device_model = (string)dataRow[columnIndex3];
						string fw_version = (string)dataRow[columnIndex4];
						int num2 = Convert.ToInt32(dataRow[columnIndex5]);
						string rackNameByID = ClientAPI.getRackNameByID(num);
						string[] array;
						if (zoneByRack.ContainsKey(num2))
						{
							string text = zoneByRack[num2];
							array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
							if (array == null)
							{
								array = new string[]
								{
									""
								};
							}
						}
						else
						{
							array = new string[]
							{
								""
							};
						}
						string text2 = "";
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string value = array2[i];
							if (!string.IsNullOrEmpty(value))
							{
								int key = Convert.ToInt32(value);
								if (zoneNames.ContainsKey(key))
								{
									if (text2 != "")
									{
										text2 += ",";
									}
									text2 += zoneNames[key];
								}
							}
						}
						dictionary.Add(num, new ClientAPI.DeviceWithZoneRackInfo
						{
							device_id = num,
							device_nm = device_nm,
							device_model = device_model,
							fw_version = fw_version,
							rack_id = num2,
							rack_nm = rackNameByID,
							zone_list = text2
						});
					}
					catch (Exception ex)
					{
						Common.WriteLine("GetDevicRackZoneRelation: {0}", new string[]
						{
							ex.Message
						});
					}
				}
			}
			Common.ElapsedTime(tLast);
			return dictionary;
		}
		public static DataSet getDataSet(int from)
		{
			DataSet result;
			lock (ClientAPI._lockLocalAccess)
			{
				long tLast = (long)Environment.TickCount;
				if (from > 0 && !ClientAPI.IsNewDataSet(from))
				{
					result = null;
				}
				else
				{
					if (ClientAPI._workingDataSet == null)
					{
						ClientAPI._workingDataSet = new DataSet();
					}
					DataSet dataSet = new DataSet();
					if (ClientAPI._workingDataSet.Tables.Count > 0)
					{
						for (int i = 0; i < ClientAPI._workingDataSet.Tables.Count; i++)
						{
							DataTable table = ClientAPI._workingDataSet.Tables[i].Copy();
							dataSet.Tables.Add(table);
						}
					}
					long num = Common.ElapsedTime(tLast);
					Common.WriteLine("    getDataSet from {0}: updated, elapsed={1}", new string[]
					{
						from.ToString(),
						num.ToString()
					});
					result = dataSet;
				}
			}
			return result;
		}
		public static ArrayList getRackInfo()
		{
			long tLast = (long)Environment.TickCount;
			ArrayList arrayList = ClientAPI.getRackInfoCopy();
			if (arrayList == null)
			{
				arrayList = new ArrayList();
			}
			Common.WriteLine("    getRackInfo: elapsed={0}", new string[]
			{
				Common.ElapsedTime(tLast).ToString()
			});
			return arrayList;
		}
		private static long RequestRemoteCall(int protocal_ID, int zipmethod, string para)
		{
			if (ClientAPI._requestChannel == null || !ClientAPI._requestChannel.IsConnected())
			{
				return -1L;
			}
			Random random = new Random();
			long num = 0L;
			lock (ClientAPI._remoteCalls)
			{
				do
				{
					num = (long)(random.NextDouble() * 10000000000.0);
				}
				while (ClientAPI._remoteCalls.ContainsKey(num));
				ClientAPI.RemoteCallContext remoteCallContext = new ClientAPI.RemoteCallContext(num, protocal_ID, zipmethod, para);
				if (ClientAPI._remoteCalls != null && remoteCallContext != null)
				{
					ClientAPI._remoteCalls.Add(num, remoteCallContext);
				}
			}
			ClientAPI._requestChannel.SendRemoteCall(num, protocal_ID, zipmethod, para);
			return num;
		}
		private static bool IsRemoteCallDone(long id)
		{
			bool result;
			lock (ClientAPI._remoteCalls)
			{
				if (!ClientAPI._remoteCalls.ContainsKey(id))
				{
					result = false;
				}
				else
				{
					result = ClientAPI._remoteCalls[id].isDone;
				}
			}
			return result;
		}
		private static object getRemoteCallResult(long id)
		{
			object result;
			lock (ClientAPI._remoteCalls)
			{
				if (!ClientAPI._remoteCalls.ContainsKey(id))
				{
					result = null;
				}
				else
				{
					if (!ClientAPI._remoteCalls[id].isDone)
					{
						result = null;
					}
					else
					{
						byte[] response = ClientAPI._remoteCalls[id].response;
						ClientAPI._remoteCalls.Remove(id);
						if (response == null || response.Length <= 0)
						{
							result = null;
						}
						else
						{
							string @string = Encoding.UTF8.GetString(response);
							object obj = CustomXmlDeserializer.Deserialize(@string, 8, new TestMeTypeConverter());
							result = obj;
						}
					}
				}
			}
			return result;
		}
		public static void setRemoteCallResult(long cid, byte[] data)
		{
			lock (ClientAPI._remoteCalls)
			{
				if (ClientAPI._remoteCalls.ContainsKey(cid))
				{
					ClientAPI._remoteCalls[cid].isDone = true;
					ClientAPI._remoteCalls[cid].response = data;
				}
			}
		}
		public static object RemoteCall(int protocal_ID, int zipmethod, string para, int timeout = 10000)
		{
			if (protocal_ID == 8)
			{
				return ClientAPI.getUserUAC();
			}
			object result = null;
			long num = ClientAPI.RequestRemoteCall(protocal_ID, zipmethod, para);
			if (num <= 0L)
			{
				return null;
			}
			Common.WriteLine("RemoteCall #{0}, pid={1}, compress={2}, parameter={3}", new string[]
			{
				num.ToString(),
				protocal_ID.ToString(),
				zipmethod.ToString(),
				para
			});
			long tLast = (long)Environment.TickCount;
			long num2;
			while (ClientAPI._requestChannel != null && ClientAPI._requestChannel.IsConnected())
			{
				num2 = Common.ElapsedTime(tLast);
				if (num2 > (long)timeout)
				{
					break;
				}
				if (ClientAPI.IsRemoteCallDone(num))
				{
					result = ClientAPI.getRemoteCallResult(num);
					break;
				}
				Thread.Sleep(100);
			}
			num2 = Common.ElapsedTime(tLast);
			Common.WriteLine("RemoteCall #{0} End, pid={1}, compress={2}, parameter={3}, Elapsed={4}", new string[]
			{
				num.ToString(),
				protocal_ID.ToString(),
				zipmethod.ToString(),
				para,
				num2.ToString()
			});
			return result;
		}
		public static void OnPendingChanged(ClientAPI.PendingStatusArgs e)
		{
			if (ClientAPI.PendingChanged != null)
			{
				ClientAPI.PendingChanged(null, e);
			}
		}
		public static void OnRealtimeChanged(ClientAPI.RealtimeDataArgs e)
		{
			if (ClientAPI.RealtimeChanged != null)
			{
				ClientAPI.RealtimeChanged(null, e);
			}
		}
		public static void OnThresholdChanged(ClientAPI.ThresholdArgs e)
		{
			if (ClientAPI.ThresholdChanged != null)
			{
				ClientAPI.ThresholdChanged(null, e);
			}
		}
		public static void OnRackInfoChanged(ClientAPI.RackInfoArgs e)
		{
			if (ClientAPI.RackInfoChanged != null)
			{
				ClientAPI.RackInfoChanged(null, e);
			}
		}
		public static void OnPueChanged(ClientAPI.PUEChangedArgs e)
		{
			if (ClientAPI.PUEChanged != null)
			{
				ClientAPI.PUEChanged(null, e);
			}
		}
		public static void OnBroadcastChanged(ClientAPI.BroadcastArgs e)
		{
			if (ClientAPI.BroadcastChanged != null)
			{
				ClientAPI.BroadcastChanged(null, e);
			}
		}
		static ClientAPI()
		{
			// Note: this type is marked as 'beforefieldinit'.
			int[] updateFlag = new int[10];
			ClientAPI._updateFlag = updateFlag;
			ClientAPI._login_use_SSL = false;
			ClientAPI._login_string = "";
			ClientAPI._last_cookie = "";
			ClientAPI._WatchdogThread = null;
			ClientAPI.evtWatchgog = new ManualResetEvent[]
			{
				new ManualResetEvent(false),
				new ManualResetEvent(false),
				new ManualResetEvent(false)
			};
			ClientAPI._syncTimerEnabled = 1L;
			ClientAPI._syncTimer = 10L;
			ClientAPI._syncThread = null;
			ClientAPI.cbOnBroadcast = null;
			ClientAPI._macConflictList = new Dictionary<string, string>();
			ClientAPI.cbOnConnected = null;
			ClientAPI._bOnClosedEnabled = true;
			ClientAPI.cbOnClosed = null;
			ClientAPI._remoteCalls = new Dictionary<long, ClientAPI.RemoteCallContext>();
			ClientAPI.PendingChanged = null;
			ClientAPI.RealtimeChanged = null;
			ClientAPI.ThresholdChanged = null;
			ClientAPI.RackInfoChanged = null;
			ClientAPI.PUEChanged = null;
			ClientAPI.BroadcastChanged = null;
		}
	}
}
