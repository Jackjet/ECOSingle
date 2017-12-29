using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace InSnergyAPI.ConnectionLayer
{
	public class SocketListener
	{
		public delegate void DelegateLog(string message);
		private Process theProcess;
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		internal volatile int nRunning;
		private object thisListenerLock = new object();
		private static Random random = new Random(DateTime.Now.Millisecond);
		private BufferManager theBufferManager;
		private Socket listenSocket;
		private Semaphore theAcceptLimit;
		protected ManualResetEvent _stoppedEvent;
		public IPEndPoint m_LocalEndPoint;
		public int m_nKillCounter;
		public int m_nMaxListen;
		public int m_nMaxBufferSize;
		public int m_nMaxConnections;
		public int m_nConnectedSockets;
		private SocketAsyncEventArgsPool poolOfReceiveEventArgs;
		private List<SocketAsyncEventArgs> poolOfBusyEventArgs;
		private SocketListener.DelegateLog logOut;
		public void SetLogCallback(SocketListener.DelegateLog log)
		{
			this.logOut = log;
		}
		public void DebugLog(string sLog)
		{
			if (this.logOut != null)
			{
				this.logOut(sLog);
			}
		}
		public SocketListener(int nMaxConnections, int nMaxListen, int nTimeout, int nMaxBufferSize, IPEndPoint localEndPoint)
		{
			this.theProcess = Process.GetCurrentProcess();
			Interlocked.Exchange(ref this.nRunning, 0);
			Interlocked.Exchange(ref this.m_nConnectedSockets, 0);
			ConnectionContext.TIMEOUT_RECEIVE = nTimeout;
			this.m_nKillCounter = 0;
			this.m_LocalEndPoint = localEndPoint;
			this.m_nMaxConnections = Math.Max(1, nMaxConnections);
			this.m_nMaxListen = Math.Max(1, nMaxListen);
			this.m_nMaxBufferSize = Math.Max(256, nMaxBufferSize);
			this.theBufferManager = new BufferManager(this.m_nMaxBufferSize * (this.m_nMaxConnections + this.m_nMaxListen), this.m_nMaxBufferSize);
			this.poolOfReceiveEventArgs = new SocketAsyncEventArgsPool(this.m_nMaxConnections + this.m_nMaxListen);
			this.poolOfBusyEventArgs = new List<SocketAsyncEventArgs>();
			this.theAcceptLimit = new Semaphore(this.m_nMaxListen, this.m_nMaxListen);
			this._stoppedEvent = new ManualResetEvent(true);
		}
		public bool NotifyQuit()
		{
			this.DebugLog("Quit Listening Notify\r\n");
			lock (this.thisListenerLock)
			{
				Interlocked.Exchange(ref this.nRunning, 0);
				this.listenSocket.Close();
			}
			if (!this._stoppedEvent.WaitOne(2000))
			{
				this.DebugLog("Quit Timeout\r\n");
				return true;
			}
			return false;
		}
		public int GetPoolFree()
		{
			int count;
			lock (this.thisListenerLock)
			{
				count = this.poolOfReceiveEventArgs.Count;
			}
			return count;
		}
		public bool Create()
		{
			this.Init();
			if (this.startListen())
			{
				Interlocked.Exchange(ref this.nRunning, 1);
				this._stoppedEvent.Reset();
				return true;
			}
			return false;
		}
		public void Run()
		{
			int num = 1000;
			this.DebugLog("InSnergy is listening");
			while (this.nRunning > 0)
			{
				try
				{
					this.theAcceptLimit.WaitOne();
				}
				catch (Exception)
				{
				}
				num = 0;
				if (this.nRunning <= 0)
				{
					break;
				}
				lock (this.thisListenerLock)
				{
					if (this.poolOfReceiveEventArgs.Count > 0)
					{
						bool flag2 = true;
						SocketAsyncEventArgs socketAsyncEventArgs = null;
						lock (this.thisListenerLock)
						{
							socketAsyncEventArgs = this.poolOfReceiveEventArgs.Pop();
							this.poolOfBusyEventArgs.Add(socketAsyncEventArgs);
							socketAsyncEventArgs.AcceptSocket = null;
						}
						try
						{
							if (!this.listenSocket.AcceptAsync(socketAsyncEventArgs))
							{
								if (socketAsyncEventArgs.SocketError == SocketError.Success)
								{
									this.ProcessAccepted(socketAsyncEventArgs);
									this.DebugLog("New connection accepted #1\r\n");
								}
								else
								{
									this.DebugLog("Failed to accept a new connection\r\n");
									flag2 = false;
								}
								this.theAcceptLimit.Release();
							}
						}
						catch (Exception ex)
						{
							flag2 = false;
							this.theAcceptLimit.Release();
							this.DebugLog("Accept error: [" + ex.Message + "]\r\n");
						}
						if (flag2)
						{
							goto IL_1AA;
						}
						num = 1000;
						if (socketAsyncEventArgs.AcceptSocket != null)
						{
							this.DebugLog("Accept error\r\n");
						}
						socketAsyncEventArgs.AcceptSocket.Close();
						socketAsyncEventArgs.AcceptSocket = null;
						lock (this.thisListenerLock)
						{
							this.poolOfReceiveEventArgs.Push(socketAsyncEventArgs);
							if (this.poolOfBusyEventArgs.Contains(socketAsyncEventArgs))
							{
								this.poolOfBusyEventArgs.Remove(socketAsyncEventArgs);
							}
							goto IL_1AA;
						}
					}
					num = 1000;
					this.theAcceptLimit.Release();
					this.DebugLog("Connection Limited\r\n");
					IL_1AA:;
				}
				if (num > 0)
				{
					Thread.Sleep(num);
				}
			}
			this.listenSocket.Close();
			this.DebugLog("Listen is Closed");
			ConnectionManager.DisconnectAll();
			this.DebugLog("InSnergy listen thread End");
			this.theBufferManager = null;
			this._stoppedEvent.Set();
		}
		public static void ThreadProc(object info)
		{
			SocketListener socketListener = (SocketListener)info;
			socketListener.Run();
		}
		public string GetPeerAddress(Socket socket)
		{
			string result;
			lock (this.thisListenerLock)
			{
				string text = "";
				if (socket != null)
				{
					text = socket.RemoteEndPoint.ToString();
				}
				result = text;
			}
			return result;
		}
		public void CheckTimeout()
		{
			List<Socket> list = ConnectionManager.CheckTimeout();
			lock (this.thisListenerLock)
			{
				foreach (Socket current in list)
				{
					if (current.Connected)
					{
						current.Shutdown(SocketShutdown.Both);
						current.Disconnect(false);
					}
				}
			}
		}
		internal void Init()
		{
			this.theBufferManager.InitBuffer();
			ConnectionManager.DisconnectAll();
			for (int i = 0; i < this.m_nMaxConnections + this.m_nMaxListen; i++)
			{
				SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
				if (this.theBufferManager.SetBuffer(socketAsyncEventArgs))
				{
					int num = this.poolOfReceiveEventArgs.AssignTokenId() + 1000000;
					socketAsyncEventArgs.UserToken = num;
					socketAsyncEventArgs.DisconnectReuseSocket = true;
					socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
					socketAsyncEventArgs.AcceptSocket = null;
					this.poolOfReceiveEventArgs.Push(socketAsyncEventArgs);
					if (this.poolOfBusyEventArgs.Contains(socketAsyncEventArgs))
					{
						this.poolOfBusyEventArgs.Remove(socketAsyncEventArgs);
					}
				}
				else
				{
					socketAsyncEventArgs.Dispose();
				}
			}
		}
		public void KillDevice(int nKillCount)
		{
			lock (this.thisListenerLock)
			{
				List<Socket> list = new List<Socket>();
				List<int> list2 = new List<int>();
				if (nKillCount > this.poolOfBusyEventArgs.Count)
				{
					nKillCount = this.poolOfBusyEventArgs.Count;
				}
				this.m_nKillCounter = nKillCount;
				if (this.poolOfBusyEventArgs.Count == nKillCount)
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
						int item = SocketListener.random.Next(0, this.poolOfBusyEventArgs.Count - 1);
						if (!list2.Contains(item))
						{
							list2.Add(item);
						}
					}
				}
				int num = 0;
				foreach (SocketAsyncEventArgs current in this.poolOfBusyEventArgs)
				{
					if (list2.Contains(num))
					{
						list.Add(current.AcceptSocket);
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
				}
			}
		}
		internal bool startListen()
		{
			this.listenSocket = new Socket(this.m_LocalEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				this.listenSocket.Bind(this.m_LocalEndPoint);
				this.listenSocket.Listen(this.m_nMaxListen);
				return true;
			}
			catch (SocketException ex)
			{
				this.DebugLog("Listen error: [" + ex.Message + "]\r\n");
			}
			catch (Exception)
			{
			}
			return false;
		}
		private void ProcessAccepted(SocketAsyncEventArgs acceptedEventArgs)
		{
			lock (this.thisListenerLock)
			{
				if (acceptedEventArgs.SocketError != SocketError.Success)
				{
					acceptedEventArgs.AcceptSocket.Close();
					this.RecycleConnections(acceptedEventArgs);
				}
				else
				{
					Interlocked.Increment(ref this.m_nConnectedSockets);
					Socket acceptSocket = acceptedEventArgs.AcceptSocket;
					if (acceptSocket != null)
					{
						ConnectionManager.UpdateState(acceptSocket, 1, 0, "");
					}
					this.ProcessReceived(acceptedEventArgs);
				}
			}
		}
		private void StartReceive(SocketAsyncEventArgs receiveEventArgs)
		{
			lock (this.thisListenerLock)
			{
				try
				{
					if (receiveEventArgs.AcceptSocket.Connected)
					{
						if (!receiveEventArgs.AcceptSocket.ReceiveAsync(receiveEventArgs))
						{
							this.ProcessReceived(receiveEventArgs);
						}
					}
					else
					{
						if (receiveEventArgs.AcceptSocket.Connected)
						{
							receiveEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
							receiveEventArgs.AcceptSocket.Disconnect(false);
						}
						receiveEventArgs.AcceptSocket.Close();
						this.RecycleConnections(receiveEventArgs);
					}
				}
				catch (Exception ex)
				{
					this.DebugLog("StartReceive error: [" + ex.Message + "]\r\n");
					if (receiveEventArgs.AcceptSocket.Connected)
					{
						receiveEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
						receiveEventArgs.AcceptSocket.Disconnect(false);
					}
					receiveEventArgs.AcceptSocket.Close();
					this.RecycleConnections(receiveEventArgs);
				}
			}
		}
		internal void StartDisconnect(Socket sock)
		{
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.AcceptSocket = sock;
			socketAsyncEventArgs.UserToken = null;
			socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
			if (sock.Connected)
			{
				sock.Shutdown(SocketShutdown.Both);
				if (!sock.DisconnectAsync(socketAsyncEventArgs))
				{
					this.ProcessDisconnected(socketAsyncEventArgs);
				}
			}
		}
		public void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
			switch (e.LastOperation)
			{
			case SocketAsyncOperation.Accept:
				this.ProcessAccepted(e);
				this.theAcceptLimit.Release();
				return;
			case SocketAsyncOperation.Disconnect:
				Interlocked.Decrement(ref this.m_nKillCounter);
				this.ProcessDisconnected(e);
				return;
			case SocketAsyncOperation.Receive:
				this.ProcessReceived(e);
				return;
			case SocketAsyncOperation.Send:
				this.ProcessSent(e);
				return;
			}
			SocketError arg_6B_0 = e.SocketError;
			this.CloseClientSocket(e);
		}
		private void RecycleConnections(SocketAsyncEventArgs receiveEventArg)
		{
			if (receiveEventArg.LastOperation == SocketAsyncOperation.Accept || receiveEventArg.LastOperation == SocketAsyncOperation.Receive)
			{
				ConnectionManager.UpdateState(receiveEventArg.AcceptSocket, 0, 0, "");
				ConnectionManager.LinkDownNotify(receiveEventArg.AcceptSocket);
				Interlocked.Decrement(ref this.m_nConnectedSockets);
				receiveEventArg.AcceptSocket.Close();
				receiveEventArg.AcceptSocket = null;
				this.poolOfReceiveEventArgs.Push(receiveEventArg);
				if (this.poolOfBusyEventArgs.Contains(receiveEventArg))
				{
					this.poolOfBusyEventArgs.Remove(receiveEventArg);
				}
			}
		}
		private void ProcessReceived(SocketAsyncEventArgs receiveEventArg)
		{
			lock (this.thisListenerLock)
			{
				if (receiveEventArg.SocketError != SocketError.Success)
				{
					if (receiveEventArg.AcceptSocket.Connected)
					{
						receiveEventArg.AcceptSocket.Shutdown(SocketShutdown.Both);
						receiveEventArg.AcceptSocket.Disconnect(false);
					}
					receiveEventArg.AcceptSocket.Close();
					this.RecycleConnections(receiveEventArg);
				}
				else
				{
					if (receiveEventArg.BytesTransferred == 0 && receiveEventArg.LastOperation == SocketAsyncOperation.Receive)
					{
						if (receiveEventArg.AcceptSocket.Connected)
						{
							receiveEventArg.AcceptSocket.Shutdown(SocketShutdown.Both);
							receiveEventArg.AcceptSocket.Disconnect(false);
						}
						receiveEventArg.AcceptSocket.Close();
						this.RecycleConnections(receiveEventArg);
					}
					else
					{
						List<SocketAsyncEventArgs> list = ConnectionManager.IncomingHandler(receiveEventArg);
						if (list == null)
						{
							this.DebugLog("App buffer is full [" + receiveEventArg.AcceptSocket.Handle + "]\r\n");
							if (receiveEventArg.AcceptSocket.Connected)
							{
								receiveEventArg.AcceptSocket.Shutdown(SocketShutdown.Both);
								receiveEventArg.AcceptSocket.Disconnect(false);
							}
							receiveEventArg.AcceptSocket.Close();
							this.RecycleConnections(receiveEventArg);
						}
						else
						{
							for (int i = 0; i < list.Count; i++)
							{
								if (list[i] != null)
								{
									list[i].Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
									this.StartSend(list[i]);
								}
							}
							ConnectionManager.UpdateState(receiveEventArg.AcceptSocket, 4, receiveEventArg.BytesTransferred, "");
							this.StartReceive(receiveEventArg);
						}
					}
				}
			}
		}
		private void StartSend(SocketAsyncEventArgs sendEventArg)
		{
			lock (this.thisListenerLock)
			{
				if (sendEventArg.AcceptSocket.Connected)
				{
					if (!sendEventArg.AcceptSocket.SendAsync(sendEventArg))
					{
						this.ProcessSent(sendEventArg);
					}
				}
				else
				{
					sendEventArg.Dispose();
				}
			}
		}
		private void ProcessSent(SocketAsyncEventArgs sendEventArg)
		{
			lock (this.thisListenerLock)
			{
				if (sendEventArg.SocketError == SocketError.Success)
				{
					ConnectionManager.UpdateState(sendEventArg.AcceptSocket, 5, sendEventArg.BytesTransferred, "");
				}
				sendEventArg.Dispose();
			}
		}
		private void ProcessDisconnected(SocketAsyncEventArgs e)
		{
			lock (this.thisListenerLock)
			{
				e.AcceptSocket.Close();
				e.Dispose();
			}
		}
		private void CloseClientSocket(SocketAsyncEventArgs e)
		{
			lock (this.thisListenerLock)
			{
				if (e.UserToken == null)
				{
					e.Dispose();
				}
			}
		}
		private void DisposeAllObjects()
		{
			lock (this.thisListenerLock)
			{
				while (this.poolOfReceiveEventArgs.Count > 0)
				{
					SocketAsyncEventArgs socketAsyncEventArgs = this.poolOfReceiveEventArgs.Pop();
					socketAsyncEventArgs.Dispose();
				}
			}
		}
		public void SendPoll(Socket socket, List<string> pollList)
		{
			lock (this.thisListenerLock)
			{
				if (pollList.Count > 0)
				{
					foreach (string current in pollList)
					{
						byte[] bytes = Encoding.ASCII.GetBytes(current);
						SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
						if (socketAsyncEventArgs != null)
						{
							socketAsyncEventArgs.AcceptSocket = socket;
							socketAsyncEventArgs.SetBuffer(bytes, 0, bytes.Length);
							socketAsyncEventArgs.UserToken = null;
							socketAsyncEventArgs.DisconnectReuseSocket = true;
							socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
							this.StartSend(socketAsyncEventArgs);
						}
					}
				}
			}
		}
	}
}
