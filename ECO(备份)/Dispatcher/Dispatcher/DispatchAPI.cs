using DBAccessAPI;
using Dispatcher.Properties;
using EcoDevice.AccessAPI;
using EcoMessenger;
using ecoProtocols;
using SessionManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
namespace Dispatcher
{
	public class DispatchAPI
	{
		public delegate void DelegateSTOPService(int errCode);
		private static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static object _lockDispatch = new object();
		public static DispatchAPI.DelegateSTOPService cbStopOnError = null;
		private static bool _localConnection = false;
		private static EcoHandler _ecoHandler = null;
		private static bool _isServerRole = false;
		public static bool Command4Service(Dictionary<string, string> req)
		{
			if (req.Count == 0)
			{
				return false;
			}
			if (req.ContainsKey("RestartListener"))
			{
				string[] array = req["RestartListener"].Split(new char[]
				{
					','
				});
				if (array.Length >= 2)
				{
					bool newSSL = Convert.ToInt32(array[0].Trim()) > 0;
					int num = Convert.ToInt32(array[1].Trim());
					if (num > 0)
					{
						return ServicesAPI.RestartListener(newSSL, num, DispatchAPI._ecoHandler);
					}
				}
			}
			return false;
		}
		public static bool StartDispatcher(int port, bool useSSL = true)
		{
			bool result;
			lock (DispatchAPI._lockDispatch)
			{
				try
				{
					DispatchAPI._isServerRole = true;
					if (port == 0)
					{
						DispatchAPI._localConnection = true;
					}
					else
					{
						DispatchAPI.ExtractCertificate("MonitorServers.pfx");
					}
					SessionAPI.StartSessionManager();
					DispatchAPI._ecoHandler = new EcoHandler();
					DispatchAPI._ecoHandler.Start(true);
					DeviceInfo.cbOnDBUpdated = new DeviceInfo.DelegateOnDbUpdate(DispatchAPI._ecoHandler.OnReloadDBData);
					DevAccessCfg._cbOnAutoModeUpdated = new DevAccessCfg.DelegateOnAutoModeUpdate(DispatchAPI._ecoHandler.OnAutoDiscovery);
					bool flag2 = ServicesAPI.StartEcoService(useSSL, port, DispatchAPI._ecoHandler);
					result = flag2;
				}
				catch (Exception ex)
				{
					Common.WriteLine("StartDispatcher: {0}", new string[]
					{
						ex.Message
					});
					result = false;
				}
			}
			return result;
		}
		public static void StopDispatcher()
		{
			lock (DispatchAPI._lockDispatch)
			{
				Common.WriteLine("StopDispatcher ...", new string[0]);
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.Stop();
				}
				DispatchAPI._ecoHandler = null;
				ServicesAPI.StopEcoService();
				SessionAPI.StopSessionManager();
				Common.WriteLine("StopDispatcher Done", new string[0]);
			}
		}
		public static bool IsServerRole()
		{
			return DispatchAPI._isServerRole;
		}
		public static bool IsLocalConnection()
		{
			return DispatchAPI._localConnection;
		}
		public static bool ExtractCertificate(string dstfile)
		{
			string text = DispatchAPI.sCodebase;
			text += dstfile;
			try
			{
				if (File.Exists(text))
				{
					DateTime lastWriteTime = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
					DateTime lastWriteTime2 = File.GetLastWriteTime(text);
					if (lastWriteTime <= lastWriteTime2)
					{
						bool result = true;
						return result;
					}
					File.Delete(text);
				}
				MemoryStream memoryStream = new MemoryStream(Resources.MonitorServers);
				if (memoryStream == null)
				{
					bool result = false;
					return result;
				}
				FileStream fileStream = File.Open(text, FileMode.CreateNew);
				if (fileStream == null)
				{
					memoryStream.Dispose();
					bool result = false;
					return result;
				}
				DispatchAPI.CopyStream(memoryStream, fileStream);
				memoryStream.Close();
				fileStream.Close();
				memoryStream.Dispose();
				fileStream.Dispose();
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}
		public static void CopyStream(Stream input, Stream output)
		{
			try
			{
				byte[] array = new byte[32768];
				while (true)
				{
					int num = input.Read(array, 0, array.Length);
					if (num <= 0)
					{
						break;
					}
					output.Write(array, 0, num);
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
		}
		public static void UpdateDBStatus(long status)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.UpdateDBStatus(status);
				}
			}
		}
		public static void BroadcastMACConflict(IDictionary<string, string> macMismatch)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.BroadcastMACConflict(macMismatch);
				}
			}
		}
		public static void ServiceWillDown()
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					SessionAPI.ServiceWillStop();
					DispatchAPI._ecoHandler.SendUrgency(null, 0, "servicewilldown");
				}
			}
		}
		public static void SendUrgency(Socket sock, int uid, string op)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.SendUrgency(sock, uid, op);
				}
			}
		}
		public static void BroadcastPending(DataTable pendingOutlets)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.BroadcastPending(pendingOutlets);
				}
			}
		}
		public static void DifferentialValues(DataSet rtDiff)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.DifferentialValues(rtDiff);
				}
			}
		}
		public static void DispatchRealtimeData(DataSet rtData, List<int> keepLastValues)
		{
			lock (DispatchAPI._lockDispatch)
			{
				if (DispatchAPI._ecoHandler != null)
				{
					DispatchAPI._ecoHandler.DispatchRealtimeData(rtData, keepLastValues);
				}
			}
		}
	}
}
