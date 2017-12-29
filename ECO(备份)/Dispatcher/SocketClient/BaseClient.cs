using CommonAPI.Timers;
using EcoMessenger;
using ecoProtocols;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace SocketClient
{
	public abstract class BaseClient<C, T> : IConnectInterface, IDisposable where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		protected ClientConfig _clientConfig;
		protected int _stopping;
		protected ManualResetEvent _stoppedEvent;
		protected Thread _clientThread;
		protected Socket _sockClient;
		protected long _nConnectState;
		protected TickTimer _tDelayConnectFrom = new TickTimer();
		protected long _tLastTransfer = (long)Environment.TickCount;
		protected long _tLastReceivedPacket = (long)Environment.TickCount;
		protected SpeedMeter _speedMeter = new SpeedMeter();
		protected object thisClientLock = new object();
		protected object thisSendLock = new object();
		protected Random random = new Random(DateTime.Now.Millisecond);
		protected int totalRequested;
		protected IPEndPoint hostEndPoint;
		protected int nRequestInterval = 5000;
		protected int nMaxConnections = 2000;
		protected int nBufferSize = 4096;
		protected SocketAsyncEventArgs _connectArgs = new SocketAsyncEventArgs();
		protected SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
		protected C _context = Activator.CreateInstance<C>();
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		public abstract void Dispose();
		public BaseClient()
		{
			this._context.ResetSSL();
			this._context._owner = this;
			this._tLastReceivedPacket = (long)Environment.TickCount;
		}
		public BaseClient(ClientConfig config)
		{
			this._context.ResetSSL();
			this._context._owner = this;
			this._clientConfig = config;
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent = new ManualResetEvent(true);
			this.nRequestInterval = this._clientConfig.interval;
			this.nMaxConnections = this._clientConfig.maxConnections;
			IPAddress[] hostAddresses = Dns.GetHostAddresses(this._clientConfig.hostName);
			this.hostEndPoint = new IPEndPoint(hostAddresses[hostAddresses.Length - 1], this._clientConfig.port);
		}
		public virtual bool IsConnected()
		{
			bool result = false;
			if (this._context != null && !this._context._bKicked && !this._context._bServiceWillDown && this._context._sock != null && this._context._sock.Connected)
			{
				result = true;
			}
			return result;
		}
		public void getSpeed(ref double recvSpeed, ref double sendSpeed)
		{
			recvSpeed = this._speedMeter.getReceiveSpeed();
			sendSpeed = this._speedMeter.getSendSpeed();
		}
		public virtual void ResetLastReceive()
		{
			this._tLastReceivedPacket = (long)Environment.TickCount;
		}
		public virtual long GetLastReceive()
		{
			return this._tLastReceivedPacket;
		}
		public virtual void AsyncSend(object c, byte[] data)
		{
		}
		public virtual void ReportMessage(IConnectInterface from, object c, ulong header, byte[] message)
		{
		}
		public virtual bool IsValidToken(string token)
		{
			return false;
		}
		public virtual void setLoginState(int status)
		{
		}
		public virtual int getLoginState()
		{
			return 0;
		}
		public virtual void UpdateUID(int uid, int vid)
		{
		}
		public virtual void UpdateVID(int vid)
		{
		}
		public virtual void SendUrgency(Socket sock, int uid, byte[] packet)
		{
		}
		public virtual void RequestDataset(int nDataType)
		{
		}
		public virtual void DispatchDataset(DispatchAttribute attr)
		{
		}
	}
}
