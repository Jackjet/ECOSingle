using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
namespace ecoProtocols
{
	public class ConnectContext : PacketReceiver, IDisposable
	{
		public class SerializeContext
		{
			public int _state;
			public int _compress;
			public long _tPacketStart = (long)Environment.TickCount;
			public byte[] _hashDataSet;
			public int _dataSize;
			public byte[] _dataBuffer;
			public SerializeContext()
			{
				this._state = -1;
				this._compress = 0;
				this._tPacketStart = (long)Environment.TickCount;
				this._hashDataSet = null;
				this._dataSize = 0;
				this._dataBuffer = null;
			}
		}
		public IConnectInterface _owner;
		protected int _loginState;
		public long _uid;
		public string _userName;
		public long _vid;
		public ConnectContext.SerializeContext _dsContext;
		public ConnectContext.SerializeContext _sqlContext;
		public Socket _sock;
		protected IPEndPoint RemoteEndPoint;
		public static int g_uid = 1;
		protected ulong _requestSN;
		protected int _lastRequesdtType;
		protected char[] chCRLF = new char[]
		{
			'\r',
			'\n'
		};
		protected object _lockContext = new object();
		public Queue<DispatchAttribute> _datasetRequestQueue;
		public DispatchAttribute _curDispatchAttrib;
		public bool _isRealTimePending;
		public ConcurrentQueue<BufferState> _sslPendingQ;
		public bool _sslSending;
		public bool _sslConnected;
		public NetworkStream _netStream;
		public SslStream _sslStream;
		public long _tLastReceived;
		public long _tLastSent;
		public bool _bKicked;
		public bool _bServiceWillDown;
		public ConnectContext()
		{
			this._sock = null;
			this._loginState = 0;
			this.RemoteEndPoint = null;
			this._requestSN = 0uL;
			this._dsContext = new ConnectContext.SerializeContext();
			this._sqlContext = new ConnectContext.SerializeContext();
			this._datasetRequestQueue = new Queue<DispatchAttribute>();
			this._curDispatchAttrib = null;
			this._isRealTimePending = false;
			this._vid = -1L;
			this._bKicked = false;
			this._bServiceWillDown = false;
			this._lastRequesdtType = -1;
			this._sslPendingQ = new ConcurrentQueue<BufferState>();
			this._sslSending = false;
		}
		public void ResetSSL()
		{
			this._sslConnected = false;
			this._netStream = null;
			this._sslStream = null;
			while (!this._sslPendingQ.IsEmpty)
			{
				BufferState bufferState;
				this._sslPendingQ.TryDequeue(out bufferState);
			}
			this._sslSending = false;
		}
		public ConnectContext(Socket sock)
		{
			this._sock = sock;
			this._loginState = 0;
			this.RemoteEndPoint = (sock.RemoteEndPoint as IPEndPoint);
			this._requestSN = 0uL;
			this._dsContext = new ConnectContext.SerializeContext();
			this._sqlContext = new ConnectContext.SerializeContext();
			this._datasetRequestQueue = new Queue<DispatchAttribute>();
			this._curDispatchAttrib = null;
			this._vid = -1L;
			this._bKicked = false;
			this._bServiceWillDown = false;
			this._lastRequesdtType = -1;
			this._sslPendingQ = new ConcurrentQueue<BufferState>();
			this._sslSending = false;
		}
		public void setKicked(bool kick)
		{
			lock (this._lockContext)
			{
				this._bKicked = kick;
			}
		}
		public void setServiceWillDown(bool willdown)
		{
			lock (this._lockContext)
			{
				this._bServiceWillDown = willdown;
			}
		}
		public virtual void setSocket(Socket sock)
		{
			lock (this._lockContext)
			{
				this._sock = sock;
				this._dsContext = null;
				this._sqlContext = null;
				if (this._datasetRequestQueue != null)
				{
					this._datasetRequestQueue.Clear();
				}
				this._curDispatchAttrib = null;
			}
		}
		public virtual Socket getSocket()
		{
			Socket sock;
			lock (this._lockContext)
			{
				sock = this._sock;
			}
			return sock;
		}
		public IPEndPoint getRemoteEndPoint()
		{
			IPEndPoint remoteEndPoint;
			lock (this._lockContext)
			{
				remoteEndPoint = this.RemoteEndPoint;
			}
			return remoteEndPoint;
		}
		public void setConnected(Socket sock)
		{
			this._sock = sock;
			this._loginState = 0;
			this.RemoteEndPoint = (sock.RemoteEndPoint as IPEndPoint);
			this._requestSN = 0uL;
			this._dsContext = new ConnectContext.SerializeContext();
			this._sqlContext = new ConnectContext.SerializeContext();
			this._vid = -1L;
			this._lastRequesdtType = -1;
			if (this._curDispatchAttrib != null)
			{
				this._curDispatchAttrib.dispatchPointer = -1;
			}
		}
		public void setLastRequest(int dataType)
		{
			lock (this._lockContext)
			{
				this._lastRequesdtType = dataType;
			}
		}
		public int getLastRequest()
		{
			int lastRequesdtType;
			lock (this._lockContext)
			{
				lastRequesdtType = this._lastRequesdtType;
			}
			return lastRequesdtType;
		}
		public void setLoginState(int status)
		{
			lock (this._lockContext)
			{
				this._loginState = status;
			}
		}
		public int getLoginState()
		{
			int loginState;
			lock (this._lockContext)
			{
				loginState = this._loginState;
			}
			return loginState;
		}
		public void setAttribute(Socket sock)
		{
			lock (this._lockContext)
			{
				this._sock = sock;
			}
		}
		public void Dispose()
		{
		}
		public void Reset(int used)
		{
		}
		public ulong getNextSeq()
		{
			ulong requestSN;
			lock (this._lockContext)
			{
				this._requestSN += 1uL;
				requestSN = this._requestSN;
			}
			return requestSN;
		}
		public bool setDataSet(DispatchAttribute attrib)
		{
			bool flag = (attrib.type & 1) != 0;
			bool result;
			lock (this._lockContext)
			{
				if (this._curDispatchAttrib != null)
				{
					if (this._isRealTimePending && flag && attrib.discard_if_jam > 0)
					{
						string text = "";
						if (this._curDispatchAttrib != null)
						{
							text += this._curDispatchAttrib.type.ToString();
						}
						int num = 0;
						foreach (DispatchAttribute current in this._datasetRequestQueue)
						{
							if (num == 0)
							{
								text += ":";
							}
							else
							{
								text += ",";
							}
							text += current.type;
							num++;
						}
						Common.WriteLine("XXXXXXXXXXXXXX Realtime data is already on the way @setDataSet({0}), data discarded", new string[]
						{
							text
						});
					}
					else
					{
						if (flag)
						{
							this._isRealTimePending = true;
						}
						this._datasetRequestQueue.Enqueue(attrib);
						string text2 = "";
						if (this._curDispatchAttrib != null)
						{
							text2 += this._curDispatchAttrib.type.ToString();
						}
						int num2 = 0;
						foreach (DispatchAttribute current2 in this._datasetRequestQueue)
						{
							if (num2 == 0)
							{
								text2 += ":";
							}
							else
							{
								text2 += ",";
							}
							text2 += current2.type;
							num2++;
						}
						DateTime arg_1A6_0 = DateTime.Now;
						Common.WriteLine("(S)Dispatch pending @setDataSet, push data in queue({0}): type={1}, uid={2}, {3} bytes", new string[]
						{
							text2,
							this._curDispatchAttrib.type.ToString("X8"),
							this._curDispatchAttrib.uid.ToString(),
							this._curDispatchAttrib.data.Length.ToString()
						});
					}
					result = false;
				}
				else
				{
					this._curDispatchAttrib = attrib;
					if (this._curDispatchAttrib.data != null)
					{
						if (flag)
						{
							this._isRealTimePending = true;
						}
						long num3 = Common.ElapsedTime(this._curDispatchAttrib.tStart);
						Common.WriteLine("    New Dispatch: type={0}, uid={1}, {2} bytes, Elapsed={3}", new string[]
						{
							this._curDispatchAttrib.type.ToString("X8"),
							this._curDispatchAttrib.uid.ToString(),
							this._curDispatchAttrib.data.Length.ToString(),
							num3.ToString()
						});
					}
					else
					{
						this._curDispatchAttrib = null;
					}
					result = (this._curDispatchAttrib != null);
				}
			}
			return result;
		}
		public bool IsDataSetDone()
		{
			bool result;
			lock (this._lockContext)
			{
				result = (this._curDispatchAttrib == null);
			}
			return result;
		}
	}
}
