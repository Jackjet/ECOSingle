using InSnergyAPI.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
namespace InSnergyAPI.ConnectionLayer
{
	public class ConnectionManager
	{
		public static int totalReported = 0;
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static object thisManagerLock = new object();
		private static Dictionary<Socket, ConnectionContext> allConnections = new Dictionary<Socket, ConnectionContext>();
		public static int GetTotalReported()
		{
			int result;
			lock (ConnectionManager.thisManagerLock)
			{
				result = ConnectionManager.totalReported;
			}
			return result;
		}
		public static int GetConnections()
		{
			int count;
			lock (ConnectionManager.thisManagerLock)
			{
				count = ConnectionManager.allConnections.Count;
			}
			return count;
		}
		public static string GetRemainingBuffer(Socket sock)
		{
			string result;
			lock (ConnectionManager.thisManagerLock)
			{
				string text = "";
				if (sock != null && ConnectionManager.allConnections.ContainsKey(sock) && ConnectionManager.allConnections[sock].nRemainingBytes > 0)
				{
					text = Encoding.ASCII.GetString(ConnectionManager.allConnections[sock].receiveBuffer, 0, ConnectionManager.allConnections[sock].nRemainingBytes);
				}
				result = text;
			}
			return result;
		}
		public static List<SocketAsyncEventArgs> IncomingHandler(SocketAsyncEventArgs receiveEventArg)
		{
			lock (ConnectionManager.thisManagerLock)
			{
				Socket acceptSocket = receiveEventArg.AcceptSocket;
				if (acceptSocket != null && ConnectionManager.allConnections.ContainsKey(acceptSocket))
				{
					List<SocketAsyncEventArgs> list = ConnectionManager.allConnections[acceptSocket].IncommingHandler(receiveEventArg);
					if (list != null)
					{
						ConnectionManager.totalReported += list.Count;
					}
					return list;
				}
			}
			return null;
		}
		public static void SetAuthorized(Socket socket, string strGID, bool bAuthorized)
		{
			lock (ConnectionManager.thisManagerLock)
			{
				if (ConnectionManager.allConnections.ContainsKey(socket))
				{
					ConnectionManager.allConnections[socket].UpdateState(6, 1, strGID);
				}
			}
		}
		public static string GetGatewayID(Socket socket)
		{
			string result = "";
			lock (ConnectionManager.thisManagerLock)
			{
				if (ConnectionManager.allConnections.ContainsKey(socket))
				{
					result = ConnectionManager.allConnections[socket].status.gid;
				}
			}
			return result;
		}
		public static bool IsConnected(Socket sock)
		{
			bool result;
			lock (ConnectionManager.thisManagerLock)
			{
				result = ConnectionManager.allConnections.ContainsKey(sock);
			}
			return result;
		}
		public static void UpdateState(Socket socket, int type, int nParam = 0, string strParam = "")
		{
			lock (ConnectionManager.thisManagerLock)
			{
				if (type == 1 && !ConnectionManager.allConnections.ContainsKey(socket))
				{
					ConnectionContext value = new ConnectionContext(socket);
					ConnectionManager.allConnections.Add(socket, value);
				}
				if (ConnectionManager.allConnections.ContainsKey(socket))
				{
					if (type == 0)
					{
						ConnectionManager.allConnections.Remove(socket);
					}
					else
					{
						ConnectionManager.allConnections[socket].UpdateState(type, nParam, strParam);
					}
				}
			}
		}
		public static void LinkDownNotify(Socket sock)
		{
			ApplicationHandler.DelegateOnLinkDown(sock, "link down");
		}
		public static List<Socket> CheckTimeout()
		{
			List<Socket> list = new List<Socket>();
			List<Socket> list2 = new List<Socket>();
			lock (ConnectionManager.thisManagerLock)
			{
				if (ConnectionManager.allConnections.Count > 0)
				{
					List<Socket> list3 = new List<Socket>();
					foreach (KeyValuePair<Socket, ConnectionContext> current in ConnectionManager.allConnections)
					{
						Socket key = current.Key;
						if (ConnectionManager.allConnections[key].IsTimeout())
						{
							list.Add(key);
							list3.Add(key);
						}
					}
					if (list3.Count > 0)
					{
						foreach (Socket current2 in list3)
						{
							list2.Add(current2);
							ConnectionManager.allConnections.Remove(current2);
						}
					}
				}
			}
			foreach (Socket current3 in list)
			{
				ApplicationHandler.DelegateOnLinkDown(current3, "timeout");
			}
			return list2;
		}
		public static void DisconnectAll()
		{
			List<Socket> list = new List<Socket>();
			lock (ConnectionManager.thisManagerLock)
			{
				if (ConnectionManager.allConnections.Count > 0)
				{
					foreach (KeyValuePair<Socket, ConnectionContext> current in ConnectionManager.allConnections)
					{
						Socket key = current.Key;
						list.Add(key);
						key.Close();
					}
				}
				ConnectionManager.allConnections.Clear();
			}
			foreach (Socket current2 in list)
			{
				ApplicationHandler.DelegateOnLinkDown(current2, "");
			}
		}
		public static Dictionary<Socket, ConnectionStatus> GetStatusClone()
		{
			Dictionary<Socket, ConnectionStatus> dictionary = new Dictionary<Socket, ConnectionStatus>();
			lock (ConnectionManager.thisManagerLock)
			{
				if (ConnectionManager.allConnections.Count > 0)
				{
					foreach (KeyValuePair<Socket, ConnectionContext> current in ConnectionManager.allConnections)
					{
						ConnectionStatus value = current.Value.status.DeepClone();
						dictionary.Add(current.Key, value);
					}
				}
			}
			return dictionary;
		}
	}
}
