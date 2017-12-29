using EcoMessenger;
using ecoProtocols;
using SessionManager;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace SocketServer
{
	public abstract class BaseServer<C, T> : IConnectInterface, IDisposable where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		protected volatile bool isRunning;
		protected ServerConfig _config;
		protected int _totalRequests;
		protected SpeedMeter _speedMeter = new SpeedMeter();
		public object _lockServer;
		public T _handler;
		public int _connected;
		public Dictionary<Socket, C> _connections;
		protected List<string> _tokenList = new List<string>();
		protected ServerEndPoint realEndPoint;
		private Random random = new Random(DateTime.Now.Millisecond);
		protected long _tLastPacket = (long)Environment.TickCount;
		public ServerEventHandlerByVal<BaseServer<C, T>, C, SocketAsyncEventArgs> cbSent;
		public ServerEventHandlerByRef<BaseServer<C, T>, C, SocketAsyncEventArgs, bool> cbReceived;
		public ServerEventHandlerByVal<BaseServer<C, T>, C, SocketAsyncEventArgs> cbBeforeSend;
		public ServerEventHandlerByVal<BaseServer<C, T>, C> cbNewConnection;
		public ServerEventHandlerByVal<BaseServer<C, T>, C> cbEndConnection;
		public ServerEventHandlerByVal<BaseServer<C, T>, C> cbFailed;
		public ServerEndPoint LocalEndPoint
		{
			get
			{
				return this.realEndPoint;
			}
		}
		public virtual void AsyncSend(object c, byte[] data)
		{
		}
		public virtual void ReportMessage(IConnectInterface from, object c, ulong header, byte[] message)
		{
		}
		public virtual void setLoginState(int status)
		{
		}
		public virtual int getLoginState()
		{
			return 0;
		}
		public virtual void ResetLastReceive()
		{
			this._tLastPacket = (long)Environment.TickCount;
		}
		public virtual long GetLastReceive()
		{
			return this._tLastPacket;
		}
		public virtual void UpdateUID(int uid, int vid)
		{
		}
		public virtual void UpdateVID(int vid)
		{
		}
		public virtual void RequestDataset(int nDataType)
		{
		}
		public virtual void SendUrgency(Socket sock, int uid, byte[] packet)
		{
		}
		public virtual void DispatchDataset(DispatchAttribute attr)
		{
		}
		public bool IsAuthorized(string token)
		{
			return this._tokenList.Count > 0 && this._tokenList.Contains(token);
		}
		public bool IsValidToken(string token)
		{
			return this._tokenList.Contains(token);
		}
		public void setAuthorizedString(bool bOverWrite, string token)
		{
			string[] array = token.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			if (bOverWrite)
			{
				this._tokenList.Clear();
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!string.IsNullOrEmpty(text))
				{
					this._tokenList.Add(text);
				}
			}
		}
		public BaseServer(ServerConfig config)
		{
			this._config = config;
			this._lockServer = new object();
			this._connections = new Dictionary<Socket, C>();
			this._totalRequests = 0;
			this._tLastPacket = (long)Environment.TickCount;
		}
		public abstract bool Start(T handler, int usePort = -1);
		public abstract void SendAsync(C c, SocketAsyncEventArgs e);
		public abstract void Dispose();
		public abstract int GetContextCount();
		public ServerEndPoint GetLocalEndpoint(IPAddress addr)
		{
			return this.realEndPoint;
		}
		protected virtual void OnBaseNewConnection(C connection)
		{
			if (this.cbNewConnection != null)
			{
				this.cbNewConnection(this, connection);
			}
		}
		protected virtual void OnBaseEndConnection(C connection)
		{
			if (this.cbEndConnection != null)
			{
				this.cbEndConnection(this, connection);
			}
		}
		protected virtual bool OnBaseReceived(C c, ref SocketAsyncEventArgs e)
		{
			return this.cbReceived == null || this.cbReceived(this, (c != null) ? c : default(C), ref e);
		}
		protected void OnBaseBeforeSend(C c, SocketAsyncEventArgs e)
		{
			if (this.cbBeforeSend != null)
			{
				this.cbBeforeSend(this, (c != null) ? c : default(C), e);
			}
		}
		protected virtual void OnBaseSent(C c, SocketAsyncEventArgs e)
		{
			if (this.cbSent != null)
			{
				this.cbSent(this, (c != null) ? c : default(C), e);
			}
		}
		protected virtual void OnBaseFailed(C c)
		{
			if (this.cbFailed != null)
			{
				this.cbFailed(this, c);
			}
		}
		private static long GetIPv4Long(IPAddress address)
		{
			return address.Address;
		}
		public int GetTotalRequests()
		{
			int totalRequests;
			lock (this._lockServer)
			{
				totalRequests = this._totalRequests;
			}
			return totalRequests;
		}
		public void getSpeed(ref double recvSpeed, ref double sendSpeed)
		{
			recvSpeed = this._speedMeter.getReceiveSpeed();
			sendSpeed = this._speedMeter.getSendSpeed();
		}
		protected C CreateConnection(Socket sock)
		{
			C result;
			lock (this._lockServer)
			{
				C c = Activator.CreateInstance<C>();
				c.setSocket(sock);
				this._connections.Add(sock, c);
				Interlocked.Increment(ref this._connected);
				result = c;
			}
			return result;
		}
		public int GetConnectionCount()
		{
			int count;
			lock (this._lockServer)
			{
				count = this._connections.Count;
			}
			return count;
		}
		protected C GetConnection(Socket sock)
		{
			C result;
			lock (this._lockServer)
			{
				if (this._connections.ContainsKey(sock))
				{
					C c = this._connections[sock];
					result = c;
				}
				else
				{
					result = default(C);
				}
			}
			return result;
		}
		protected void RemoveConnection(Socket sock)
		{
			long num = -1L;
			lock (this._lockServer)
			{
				if (this._connections.ContainsKey(sock))
				{
					C c = this._connections[sock];
					if (c != null)
					{
						num = c._uid;
					}
					this._connections.Remove(sock);
					Interlocked.Decrement(ref this._connected);
				}
			}
			if (num > 0L)
			{
				SessionAPI.Delete(num, false);
			}
		}
		public void KillConnection(int nKillCount)
		{
			lock (this._lockServer)
			{
				List<Socket> list = new List<Socket>();
				List<int> list2 = new List<int>();
				if (nKillCount > this._connections.Count)
				{
					nKillCount = this._connections.Count;
				}
				if (this._connections.Count == nKillCount)
				{
					for (int i = 0; i < nKillCount; i++)
					{
						list2.Add(i);
					}
				}
				else
				{
					while (list2.Count < nKillCount)
					{
						int item = this.random.Next(0, this._connections.Count - 1);
						if (!list2.Contains(item))
						{
							list2.Add(item);
						}
					}
				}
				int num = 0;
				foreach (KeyValuePair<Socket, C> current in this._connections)
				{
					if (list2.Contains(num))
					{
						list.Add(current.Key);
					}
					num++;
				}
				foreach (Socket current2 in list)
				{
					if (current2.Connected)
					{
						current2.Shutdown(SocketShutdown.Both);
						current2.Disconnect(false);
					}
					current2.Close();
				}
			}
		}
	}
}
