using Dispatcher;
using EcoMessenger;
using ecoProtocols;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace SocketClient
{
	public class BaseTCPClient<C, T> : BaseClient<C, T> where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		protected int nConnectSN;
		public readonly object _lockSend = new object();
		protected ManualResetEvent semDispatchDataSet;
		protected Semaphore semDispatchDataSetDone;
		public ManualResetEvent _eventDispatch;
		public Queue<byte[]> _dispatchMessage;
		private Semaphore semThreadAbort;
		private WaitHandle[] semHandles;
		protected T _handler;
		public BaseTCPClient(ClientConfig config)
		{
			WaitHandle[] array = new WaitHandle[4];
			this.semHandles = array;
			this._handler = default(T);
			base..ctor(config);
			this.nConnectSN = 0;
		}
		public override void Dispose()
		{
		}
		public bool canSend()
		{
			if (this._clientConfig.ssl)
			{
				return this._sockClient != null && this._sockClient.Connected && this._context._sslConnected;
			}
			return this._sockClient != null && this._sockClient.Connected;
		}
		public override void AsyncSend(object c, byte[] data)
		{
			this.StartSend((C)((object)c), data);
		}
		public override void ReportMessage(IConnectInterface from, object c, ulong header, byte[] message)
		{
			if (this._handler != null)
			{
				this._handler.PutMesssage(from, c, 6, header, message);
			}
		}
		public override void UpdateUID(int uid, int vid)
		{
			this._clientConfig.uid = uid;
			this._clientConfig.vid = vid;
			this._context._uid = (long)uid;
			this._context._vid = (long)vid;
		}
		public override void UpdateVID(int vid)
		{
			this._clientConfig.vid = vid;
			this._context._vid = (long)vid;
		}
		public override void RequestDataset(int dataType)
		{
			lock (this.thisClientLock)
			{
				if (this._clientConfig.uid >= 0)
				{
					if (this._handler != null)
					{
						try
						{
							this._context.setLastRequest(dataType);
							byte[] array = this._handler.BuildRequest(this._clientConfig.uid, this._clientConfig.vid, dataType);
							if (array != null && array.Length > 0)
							{
								Common.WriteLine("Send request: uid={0}, type={1}", new string[]
								{
									this._clientConfig.uid.ToString(),
									dataType.ToString("X8")
								});
								this.StartSend(this._context, array);
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("SendRequest failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(this._context);
						}
					}
				}
			}
		}
		public override void DispatchDataset(DispatchAttribute attrib)
		{
			lock (this.thisClientLock)
			{
				if (!this._context._sock.Connected)
				{
					this._context._datasetRequestQueue.Clear();
					this._context._curDispatchAttrib = null;
				}
				else
				{
					if (this._context._curDispatchAttrib != null)
					{
						this._context._datasetRequestQueue.Enqueue(attrib);
						Common.WriteLine("    (C)Dispatch pending, push data in queue: type={0}, uid={1}, {2} bytes", new string[]
						{
							this._context._curDispatchAttrib.type.ToString("X8"),
							this._context._curDispatchAttrib.uid.ToString(),
							this._context._curDispatchAttrib.data.Length.ToString()
						});
					}
					else
					{
						this._context._curDispatchAttrib = attrib;
						if (this._context._curDispatchAttrib.data != null)
						{
							if (this.semDispatchDataSet != null)
							{
								this.semDispatchDataSet.Set();
							}
							long num = Common.ElapsedTime(this._context._curDispatchAttrib.tStart);
							Common.WriteLine("    New Dispatch: type={0}, uid={1}, {2} bytes, Elapsed={3}", new string[]
							{
								this._context._curDispatchAttrib.type.ToString("X8"),
								this._context._curDispatchAttrib.uid.ToString(),
								this._context._curDispatchAttrib.data.Length.ToString(),
								num.ToString()
							});
						}
						else
						{
							this._context._curDispatchAttrib = null;
						}
					}
				}
			}
		}
		public virtual bool Start(T handler)
		{
			this._handler = handler;
			if (this._handler != null)
			{
				this._handler.AttachDispatcher(this);
			}
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this.semThreadAbort = new Semaphore(0, 1);
			if (this.semThreadAbort == null)
			{
				return false;
			}
			this.semDispatchDataSet = new ManualResetEvent(false);
			if (this.semDispatchDataSet == null)
			{
				this.semThreadAbort.Close();
				this.semThreadAbort.Dispose();
				this.semThreadAbort = null;
				return false;
			}
			this.semDispatchDataSetDone = new Semaphore(0, 50);
			if (this.semDispatchDataSetDone == null)
			{
				this.semThreadAbort.Close();
				this.semThreadAbort.Dispose();
				this.semThreadAbort = null;
				this.semDispatchDataSet.Close();
				this.semDispatchDataSet.Dispose();
				this.semDispatchDataSet = null;
				return false;
			}
			this._eventDispatch = new ManualResetEvent(false);
			this._dispatchMessage = new Queue<byte[]>();
			this.semHandles[0] = this.semThreadAbort;
			this.semHandles[1] = this.semDispatchDataSet;
			this.semHandles[2] = this.semDispatchDataSetDone;
			this.semHandles[3] = this._eventDispatch;
			this._tDelayConnectFrom.Stop();
			Interlocked.Exchange(ref this._nConnectState, 0L);
			this._clientThread = new Thread(new ParameterizedThreadStart(this.ClientThread));
			this._clientThread.Name = "SocketClient";
			this._clientThread.CurrentCulture = CultureInfo.InvariantCulture;
			this._clientThread.IsBackground = true;
			this._clientThread.Start();
			return true;
		}
		public virtual void Stop()
		{
			this._handler.DetachDispatcher(this);
			Interlocked.Exchange(ref this._stopping, 1);
			try
			{
				if (this.semThreadAbort != null)
				{
					this.semThreadAbort.Release();
				}
			}
			catch (Exception)
			{
			}
			try
			{
				if (this._sockClient != null)
				{
					this._sockClient.Close();
				}
				if (!this._stoppedEvent.WaitOne(1000))
				{
					Common.WriteLine("    Abort a dead thread", new string[0]);
					this._clientThread.Abort();
				}
				this._clientThread.Join(500);
			}
			catch (Exception ex)
			{
				Common.WriteLine("Stop ClientTcp: " + ex.Message, new string[0]);
			}
			Interlocked.Exchange(ref this._stopping, 0);
		}
		public void AbortOnError(C c)
		{
			this._clientConfig.uid = 0;
			this._clientConfig.vid = 0;
			this._handler.OnClose(c);
			if (c._sock != null)
			{
				if (!c._sock.Connected)
				{
					c._datasetRequestQueue.Clear();
					c._curDispatchAttrib = null;
				}
				Common.WriteLine("AbortOnError:" + this._sockClient.Handle, new string[0]);
				if (c._sock.Connected)
				{
					c._sock.Shutdown(SocketShutdown.Both);
					c._sock.Disconnect(false);
				}
				c._sock.Close();
				c._sslConnected = false;
				c.setLoginState(0);
			}
			c._datasetRequestQueue.Clear();
			c._curDispatchAttrib = null;
			c._sslConnected = false;
			c.setLoginState(0);
			this._tDelayConnectFrom.Start();
			Interlocked.Exchange(ref this._nConnectState, -1L);
		}
		public override void setLoginState(int status)
		{
			this._context.setLoginState(status);
		}
		public override int getLoginState()
		{
			return this._context.getLoginState();
		}
		public virtual void OnSayHello()
		{
			this.SendLogin();
		}
		public virtual void SendLogin()
		{
			lock (this.thisClientLock)
			{
				if (this.canSend())
				{
					if (this._handler != null)
					{
						try
						{
							byte[] array = this._handler.BuildLogin(this._clientConfig);
							if (array != null)
							{
								this.StartSend(this._context, array);
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("SendLogin failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(this._context);
						}
					}
				}
			}
		}
		public virtual void SendLogout()
		{
			lock (this.thisClientLock)
			{
				if (this.canSend())
				{
					if (this._clientConfig.uid >= 0)
					{
						if (this._handler != null)
						{
							try
							{
								byte[] array = this._handler.BuildLogout(this._clientConfig.uid, this._clientConfig.vid);
								if (array != null)
								{
									this.StartSend(this._context, array);
								}
							}
							catch (Exception ex)
							{
								Common.WriteLine("SendLogout failed: {0}", new string[]
								{
									ex.Message
								});
								this.AbortOnError(this._context);
							}
						}
					}
				}
			}
		}
		public virtual void SendMsg4Service(string msg4Service)
		{
			lock (this.thisClientLock)
			{
				if (this.canSend())
				{
					if (this._handler != null)
					{
						try
						{
							byte[] array = this._handler.BuildMsg4Service(this._clientConfig.uid, this._clientConfig.vid, msg4Service);
							if (array != null && array.Length > 0)
							{
								this.StartSend(this._context, array);
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("SendMsg4Service failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(this._context);
						}
					}
				}
			}
		}
		public virtual void SendKeepAlive()
		{
			lock (this.thisClientLock)
			{
				if (this.canSend())
				{
					if (this._handler != null)
					{
						try
						{
							byte[] array = this._handler.BuildKeepAlive(this._clientConfig.uid, this._clientConfig.vid);
							if (array != null && array.Length > 0)
							{
								this.StartSend(this._context, array);
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("SendKeepAlive failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(this._context);
						}
					}
				}
			}
		}
		public virtual void SendRemoteCall(long cid, int pid, int compress, string parameter)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<RemoteCall>\r\n";
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"<cid>",
				cid,
				"</cid>\r\n"
			});
			object obj2 = text;
			text = string.Concat(new object[]
			{
				obj2,
				"<pid>",
				pid,
				"</pid>\r\n"
			});
			object obj3 = text;
			text = string.Concat(new object[]
			{
				obj3,
				"<compress>",
				compress,
				"</compress>\r\n"
			});
			text = text + "<parameter>" + parameter + "</parameter>\r\n";
			text += "</RemoteCall>\r\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			this.SendMessage(1024, false, bytes);
		}
		public virtual void SendMessage(int type, bool broadcast, byte[] message)
		{
			lock (this.thisClientLock)
			{
				if (message.Length > 65471)
				{
					Common.WriteLine("Message is oversize: {0}", new string[]
					{
						message.Length.ToString()
					});
				}
				else
				{
					if (this.canSend())
					{
						if (this._clientConfig.uid >= 0)
						{
							if (this._handler != null)
							{
								try
								{
									byte[] array = this._handler.BuildMessage(this._clientConfig.uid, this._clientConfig.vid, type, broadcast, message);
									if (array.Length > 0)
									{
										int num = this._clientConfig.uid;
										int num2 = this._clientConfig.vid;
										if (broadcast)
										{
											num = 0;
											num2 = 0;
										}
										int num3 = 0;
										if (message != null && message.Length > 0)
										{
											num3 += message.Length;
										}
										Common.WriteLine("Send message: type={0}, uid={1}, vid={2}, len={3}, broadcast={4}", new string[]
										{
											type.ToString("X8"),
											num.ToString(),
											num2.ToString(),
											num3.ToString(),
											broadcast.ToString()
										});
										this.StartSend(this._context, array);
									}
								}
								catch (Exception ex)
								{
									Common.WriteLine("SendMessage failed: {0}", new string[]
									{
										ex.Message
									});
									this.AbortOnError(this._context);
								}
							}
						}
					}
				}
			}
		}
		protected virtual void DisptachDataSetStart()
		{
			lock (this.thisClientLock)
			{
				if (this._context._curDispatchAttrib != null)
				{
					if (this._handler != null)
					{
						try
						{
							byte[] array = this._handler.BuildFirstDispatch(this._context._curDispatchAttrib);
							if (array != null)
							{
								this.StartSend(this._context, array);
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("DisptachDataSetStart failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(this._context);
						}
					}
				}
			}
		}
		protected virtual bool DispatchNext(C c)
		{
			bool result;
			lock (this.thisClientLock)
			{
				if (c._curDispatchAttrib == null)
				{
					result = false;
				}
				else
				{
					if (this._handler == null)
					{
						result = false;
					}
					else
					{
						try
						{
							byte[] array = this._handler.BuildNextDispatch(c._curDispatchAttrib);
							if (array == null)
							{
								result = false;
								return result;
							}
							this.StartSend(c, array);
							result = true;
							return result;
						}
						catch (Exception ex)
						{
							Common.WriteLine("DispatchNext failed: {0}", new string[]
							{
								ex.Message
							});
							this.AbortOnError(c);
						}
						result = false;
					}
				}
			}
			return result;
		}
		protected virtual bool IsDispatch(C c)
		{
			lock (this.thisClientLock)
			{
				if (c._curDispatchAttrib == null)
				{
					bool result = true;
					return result;
				}
				if (this._handler == null)
				{
					bool result = false;
					return result;
				}
				if (c._curDispatchAttrib.dispatchPointer >= 0 && c._curDispatchAttrib.dispatchPointer >= c._curDispatchAttrib.data.Length)
				{
					try
					{
						c._curDispatchAttrib.data = null;
						c._curDispatchAttrib.dispatchPointer = -1;
						if (this.semDispatchDataSetDone != null)
						{
							this.semDispatchDataSetDone.Release();
						}
					}
					catch (Exception ex)
					{
						Common.WriteLine("IsDispatch: " + ex.Message, new string[0]);
					}
				}
				if (c._curDispatchAttrib.dispatchPointer < 0)
				{
					bool result = false;
					return result;
				}
			}
			return this.DispatchNext(c);
		}
		private void ClientThread(object state)
		{
			int num = 0;
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			long tLast = (long)Environment.TickCount;
			Common.WriteLine("    Client thread begins", new string[0]);
			while (this._stopping == 0)
			{
				try
				{
					num = WaitHandle.WaitAny(this.semHandles, 100);
				}
				catch (Exception ex)
				{
					Common.WriteLine("ClientThread: " + ex.Message, new string[0]);
				}
				if (num == 0)
				{
					break;
				}
				if (num != 258)
				{
					if (num == 1)
					{
						if (this._nConnectState == 2L)
						{
							this.DisptachDataSetStart();
						}
						this.semDispatchDataSet.Reset();
					}
					else
					{
						if (num == 2)
						{
							lock (this.thisClientLock)
							{
								if (this._context._curDispatchAttrib != null)
								{
									Common.WriteLine("    Disptach end: type={0}, uid={1}", new string[]
									{
										this._context._curDispatchAttrib.type.ToString("X8"),
										this._context._curDispatchAttrib.uid.ToString()
									});
									this._context._curDispatchAttrib.dispatchPointer = -1;
								}
								this._context._curDispatchAttrib = null;
								if (this._context._datasetRequestQueue.Count > 0)
								{
									this._context._curDispatchAttrib = this._context._datasetRequestQueue.Dequeue();
								}
								if (this._context._curDispatchAttrib != null)
								{
									this.semDispatchDataSet.Set();
								}
							}
						}
					}
				}
				long num2 = Common.ElapsedTime(tLast);
				if (num2 >= 1000L)
				{
					tLast = (long)Environment.TickCount;
					if (this._nConnectState == 2L)
					{
						if (Common.ElapsedTime(this._tLastTransfer) >= (long)Common._tKeepaliveTimer)
						{
							this._tLastTransfer = (long)Environment.TickCount;
							this.SendKeepAlive();
						}
					}
					else
					{
						lock (this.thisClientLock)
						{
							this._context._curDispatchAttrib = null;
						}
					}
				}
				if (this._nConnectState == -1L)
				{
					lock (this.thisClientLock)
					{
						if (this._context._datasetRequestQueue.Count > 0)
						{
							this._context._datasetRequestQueue.Clear();
						}
						if (this._context._curDispatchAttrib != null)
						{
							this._context._curDispatchAttrib = null;
						}
						if (this._eventDispatch != null)
						{
							this._eventDispatch.Reset();
						}
						if (this._dispatchMessage.Count > 0)
						{
							this._dispatchMessage.Clear();
						}
					}
					if (this._tDelayConnectFrom.isRunning() && this._tDelayConnectFrom.getElapsed() >= (long)Common._tMaxRetryTimer)
					{
						this._tDelayConnectFrom.Stop();
						Interlocked.Exchange(ref this._nConnectState, 0L);
						Common.WriteLine("    Ready to re-connect", new string[0]);
					}
				}
				if (this._nConnectState == 1L && this._tDelayConnectFrom.isRunning() && this._tDelayConnectFrom.getElapsed() >= (long)Common._tConnectingTimeout)
				{
					Common.WriteLine("    Connecting timeout", new string[0]);
					lock (this.thisClientLock)
					{
						if (this._connectArgs != null)
						{
							this._connectArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
							if (this._connectArgs.UserToken != null)
							{
								((Socket)this._connectArgs.UserToken).Close();
							}
							this._connectArgs.UserToken = null;
							this._connectArgs.RemoteEndPoint = null;
						}
						this.AbortOnError(this._context);
					}
				}
				if (this._nConnectState == 0L && !this._context._bKicked && !this._context._bServiceWillDown)
				{
					this._sockClient = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					if (this._sockClient == null)
					{
						this._tDelayConnectFrom.Start();
						Interlocked.Exchange(ref this._nConnectState, -1L);
						Common.WriteLine("    Can't create socket object", new string[0]);
					}
					else
					{
						if (this._connectArgs != null)
						{
							this._connectArgs.UserToken = this._sockClient;
							this._connectArgs.RemoteEndPoint = this.hostEndPoint;
							this._connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						}
						this._tDelayConnectFrom.Start();
						Interlocked.Exchange(ref this._nConnectState, 1L);
						Common.WriteLine("    Connecting {0}:{1}", new string[]
						{
							this.hostEndPoint.Address.ToString(),
							this.hostEndPoint.Port.ToString()
						});
						if (!this._sockClient.ConnectAsync(this._connectArgs))
						{
							this.OnConnected(null, this._connectArgs);
						}
					}
				}
				long arg_535_0 = this._nConnectState;
			}
			lock (this.thisClientLock)
			{
				try
				{
					if (this._sockClient != null)
					{
						if (this._sockClient.Connected)
						{
							this._sockClient.Shutdown(SocketShutdown.Both);
						}
						this._sockClient.Close();
					}
				}
				catch (Exception)
				{
				}
			}
			this._stoppedEvent.Set();
			Common.WriteLine("    Client thread Ends", new string[0]);
		}
		public virtual void Socket_Completed(object sender, SocketAsyncEventArgs e)
		{
			SocketAsyncOperation lastOperation = e.LastOperation;
			switch (lastOperation)
			{
			case SocketAsyncOperation.Connect:
				this.OnConnected(sender, e);
				return;
			case SocketAsyncOperation.Disconnect:
				break;
			case SocketAsyncOperation.Receive:
				this.OnReceived(sender, e);
				return;
			default:
				if (lastOperation != SocketAsyncOperation.Send)
				{
					return;
				}
				this.OnSent(sender, e);
				break;
			}
		}
		public virtual void OnConnected(object sender, SocketAsyncEventArgs e)
		{
			lock (this.thisClientLock)
			{
				Socket socket = e.UserToken as Socket;
				try
				{
					if (e.SocketError != SocketError.Success || socket == null || !socket.Connected)
					{
						if (socket != null)
						{
							Common.WriteLine(string.Concat(new object[]
							{
								"[#",
								socket.Handle,
								"]Connect timeout or failed: ",
								e.SocketError
							}), new string[0]);
							this.AbortOnError(this._context);
						}
						e.UserToken = null;
						e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						this._tDelayConnectFrom.Start();
						Interlocked.Exchange(ref this._nConnectState, -1L);
						Common.WriteLine("    Failed to connect", new string[0]);
						return;
					}
					e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
					this._context.setConnected(socket);
					this._tLastTransfer = (long)Environment.TickCount;
					this._tDelayConnectFrom.Stop();
					Interlocked.Exchange(ref this._nConnectState, 2L);
					Common.WriteLine("[#" + socket.Handle + "]Connected", new string[0]);
					byte[] array = new byte[this.nBufferSize];
					this._recvArgs.UserToken = socket;
					this._recvArgs.SetBuffer(array, 0, array.Length);
					this._recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
					this.StartReceive(this._recvArgs);
				}
				catch (Exception ex)
				{
					Common.WriteLine("OnConnected failed: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(this._context);
					return;
				}
				this.OnSayHello();
			}
		}
		private void OnReceived(object sender, SocketAsyncEventArgs e)
		{
			lock (this.thisClientLock)
			{
				Socket socket = e.UserToken as Socket;
				try
				{
					if (e.SocketError != SocketError.Success || socket == null || e.BytesTransferred == 0)
					{
						ClientAPI.OnClosed(this._context, -1);
						if (e.LastOperation == SocketAsyncOperation.Receive)
						{
							if (socket != null)
							{
								Common.WriteLine(string.Concat(new object[]
								{
									"[#",
									socket.Handle,
									"]OnReceived Closed, bytes=",
									e.BytesTransferred,
									", err=",
									e.SocketError.ToString()
								}), new string[0]);
								this.AbortOnError(this._context);
							}
							e.UserToken = null;
							e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						}
						this._tDelayConnectFrom.Start();
						Interlocked.Exchange(ref this._nConnectState, -1L);
						return;
					}
					this._speedMeter.Received(e.BytesTransferred);
					BufferState bufferState = new BufferState();
					bufferState._ctx = this._context;
					bufferState._buffer = e.Buffer;
					if (bufferState._buffer != null)
					{
						int num = 0;
						List<byte[]> list = this._context.ReceivedBuffer(bufferState, e.BytesTransferred, ref num);
						if (num > 0)
						{
							this._tLastReceivedPacket = (long)Environment.TickCount;
						}
						if (list != null)
						{
							foreach (byte[] current in list)
							{
								if (!socket.Connected)
								{
									break;
								}
								this.StartSend(this._context, current);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("OnReceived failed: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(this._context);
				}
			}
			this.StartReceive(e);
		}
		private void OnSent(object sender, SocketAsyncEventArgs e)
		{
			if (e.SocketError != SocketError.Success)
			{
				lock (this.thisClientLock)
				{
					Socket socket = e.UserToken as Socket;
					if (socket != null)
					{
						Common.WriteLine("[#" + socket.Handle + "]Send error, disconnecting ...", new string[0]);
						this.AbortOnError(this._context);
					}
					this._tDelayConnectFrom.Start();
					Interlocked.Exchange(ref this._nConnectState, -1L);
					goto IL_C1;
				}
			}
			this._speedMeter.Sent(e.BytesTransferred);
			ulong num = BitConverter.ToUInt64(e.Buffer, 0);
			num = ecoServerProtocol.swap64(num);
			uint packetType = ecoServerProtocol.getPacketType(num);
			if (packetType == 258u)
			{
				this.IsDispatch(this._context);
			}
			IL_C1:
			lock (this.thisClientLock)
			{
				this._tLastTransfer = (long)Environment.TickCount;
				e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
				e.Dispose();
			}
		}
		private void StartReceive(SocketAsyncEventArgs e)
		{
			bool flag = true;
			lock (this.thisClientLock)
			{
				Socket socket = e.UserToken as Socket;
				try
				{
					if (!socket.Connected)
					{
						Common.WriteLine("[#" + socket.Handle + "]Failed to receive", new string[0]);
						this.AbortOnError(this._context);
						e.UserToken = null;
						e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						return;
					}
					flag = socket.ReceiveAsync(e);
				}
				catch (Exception ex)
				{
					Common.WriteLine("StartReceive failed: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(this._context);
					return;
				}
			}
			if (!flag)
			{
				this.OnReceived(null, e);
			}
		}
		public virtual void StartSend(C c, byte[] sendBuffer)
		{
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			socketAsyncEventArgs.UserToken = c._sock;
			socketAsyncEventArgs.RemoteEndPoint = this.hostEndPoint;
			socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
			lock (this.thisSendLock)
			{
				if (c._sock != null && c._sock.Connected)
				{
					try
					{
						if (c._sock.Connected)
						{
							bool flag2 = false;
							lock (this._lockSend)
							{
								flag2 = c._sock.SendAsync(socketAsyncEventArgs);
							}
							if (!flag2)
							{
								socketAsyncEventArgs.Dispose();
								if (socketAsyncEventArgs.SocketError != SocketError.Success)
								{
									this.AbortOnError(c);
								}
							}
						}
						else
						{
							socketAsyncEventArgs.Dispose();
							this.AbortOnError(c);
						}
					}
					catch (Exception ex)
					{
						Common.WriteLine("StartSend failed: {0}", new string[]
						{
							ex.Message
						});
						this.AbortOnError(c);
					}
				}
			}
		}
	}
}
