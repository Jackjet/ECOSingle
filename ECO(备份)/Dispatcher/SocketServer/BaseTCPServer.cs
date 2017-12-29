using Dispatcher;
using EcoMessenger;
using ecoProtocols;
using SessionManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace SocketServer
{
	public class BaseTCPServer<C, T> : BaseServer<C, T> where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		public int _stopping;
		public string _threadName;
		public Thread _serverThread;
		public ManualResetEvent _stoppedEvent;
		protected Semaphore semListenQueue;
		protected Semaphore semThreadAbort;
		protected WaitHandle[] semHandles;
		public int _maxListen;
		public int _port;
		public Socket _mainSocket;
		public readonly object _lockSend;
		public Queue<SocketAsyncEventArgs> _listenSocketArgs;
		public Queue<SocketAsyncEventArgs> _receiveSocketArgs;
		public BaseTCPServer(ServerConfig config)
		{
			WaitHandle[] array = new WaitHandle[2];
			this.semHandles = array;
			this._lockSend = new object();
			base..ctor(config);
			this._threadName = config.serverName;
			this._port = config.port;
			this._maxListen = config.MaxListen;
			this._stoppedEvent = new ManualResetEvent(true);
			this._listenSocketArgs = new Queue<SocketAsyncEventArgs>();
			this._receiveSocketArgs = new Queue<SocketAsyncEventArgs>();
		}
		public override void Dispose()
		{
			this.isRunning = false;
			lock (this._lockServer)
			{
				if (this._mainSocket != null)
				{
					this._mainSocket.Close();
					this._mainSocket = null;
				}
			}
		}
		public override void ReportMessage(IConnectInterface from, object c, ulong header, byte[] message)
		{
			if (this._handler != null)
			{
				this._handler.PutMesssage(from, c, 7, header, message);
			}
		}
		private bool CreateListenEvents()
		{
			lock (this._lockServer)
			{
				this.semListenQueue = new Semaphore(0, this._maxListen);
				if (this.semListenQueue == null)
				{
					bool result = false;
					return result;
				}
				this.semThreadAbort = new Semaphore(0, 1);
				if (this.semThreadAbort == null)
				{
					this.semListenQueue.Close();
					this.semListenQueue.Dispose();
					this.semListenQueue = null;
					bool result = false;
					return result;
				}
				this.semHandles[0] = this.semListenQueue;
				this.semHandles[1] = this.semThreadAbort;
			}
			return true;
		}
		private void CloseListenEvents()
		{
			lock (this._lockServer)
			{
				this.semListenQueue.Close();
				this.semListenQueue.Dispose();
				this.semThreadAbort.Close();
				this.semThreadAbort.Dispose();
				for (int i = 0; i < this.semHandles.Length; i++)
				{
					this.semHandles[i] = null;
				}
			}
		}
		public virtual void AbortOnError(C c)
		{
			lock (this._lockServer)
			{
				if (c != null)
				{
					this._handler.OnClose(c);
					base.RemoveConnection(c._sock);
					if (!c._sock.Connected)
					{
						c._datasetRequestQueue.Clear();
						c._curDispatchAttrib = null;
					}
					SessionAPI.Delete(c._uid, false);
					if (c._sslStream != null)
					{
						c._sslStream.Dispose();
						c._sslStream = null;
					}
					if (c._sock != null)
					{
						Common.WriteLine("Socket Aborted: #" + c._sock.Handle, new string[0]);
						if (c._sock.Connected)
						{
							c._sock.Shutdown(SocketShutdown.Both);
							c._sock.Disconnect(false);
						}
						c._sock.Close();
					}
				}
			}
		}
		public override bool Start(T handler, int usePort = -1)
		{
			this._handler = handler;
			if (this._handler != null)
			{
				this._handler.AttachDispatcher(this);
				ecoMessage ecoMessage = new ecoMessage();
				ecoMessage._from = this;
			}
			if (usePort > 0)
			{
				this._config.port = usePort;
				this._port = usePort;
			}
			Interlocked.Exchange(ref this._stopping, 0);
			Interlocked.Exchange(ref this._totalRequests, 0);
			try
			{
				this.CreateListenEvents();
				IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, this._port);
				for (int i = 0; i < this._maxListen; i++)
				{
					SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
					socketAsyncEventArgs.UserToken = null;
					this._listenSocketArgs.Enqueue(socketAsyncEventArgs);
					try
					{
						if (this.semListenQueue != null)
						{
							this.semListenQueue.Release();
						}
					}
					catch (Exception)
					{
					}
				}
				for (int j = 0; j < this._config.MaxConnect; j++)
				{
					SocketAsyncEventArgs socketAsyncEventArgs2 = new SocketAsyncEventArgs();
					socketAsyncEventArgs2.UserToken = null;
					byte[] array = new byte[this._config.BufferSize];
					socketAsyncEventArgs2.SetBuffer(array, 0, array.Length);
					this._receiveSocketArgs.Enqueue(socketAsyncEventArgs2);
				}
				this._mainSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				this._mainSocket.Bind(iPEndPoint);
				this._mainSocket.Listen(this._maxListen);
				Common.WriteLine("Thread[{0}] is listening @ {1}", new string[]
				{
					this._threadName,
					this._config.port.ToString()
				});
				this._stoppedEvent.Reset();
				this._serverThread = new Thread(new ParameterizedThreadStart(this.ServerThread));
				this._serverThread.Name = this._config.serverName;
				this._serverThread.CurrentCulture = CultureInfo.InvariantCulture;
				this._serverThread.IsBackground = true;
				this._serverThread.Start();
			}
			catch (Exception ex)
			{
				Common.WriteLine("Thread[{0}] {1}", new string[]
				{
					this._threadName,
					ex.Message
				});
				if (this._mainSocket != null)
				{
					this._mainSocket.Close();
				}
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
				foreach (SocketAsyncEventArgs current in this._listenSocketArgs)
				{
					current.Dispose();
				}
				this._listenSocketArgs.Clear();
				this._listenSocketArgs = null;
				foreach (SocketAsyncEventArgs current2 in this._receiveSocketArgs)
				{
					current2.Dispose();
				}
				this._receiveSocketArgs.Clear();
				this._receiveSocketArgs = null;
				Interlocked.Exchange(ref this._stopping, 0);
				this.CloseListenEvents();
				return false;
			}
			return true;
		}
		private void ShutdownAll()
		{
			base.KillConnection(base.GetConnectionCount());
		}
		public void Stop()
		{
			Common.WriteLine("Stopping " + this._threadName + " thread", new string[0]);
			this._handler.DetachDispatcher(this);
			if (this._mainSocket != null)
			{
				this._mainSocket.Close();
			}
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
				if (!this._stoppedEvent.WaitOne(1000))
				{
					Common.WriteLine("Abort a dead " + this._threadName + " thread", new string[0]);
					this._serverThread.Abort();
				}
				this._serverThread.Join(500);
				while (this._listenSocketArgs.Count < this._maxListen || this._receiveSocketArgs.Count < this._config.MaxConnect)
				{
					Thread.Sleep(100);
					this.ShutdownAll();
				}
				this._mainSocket.Close();
				this._mainSocket = null;
				foreach (SocketAsyncEventArgs current in this._listenSocketArgs)
				{
					current.Dispose();
				}
				this._listenSocketArgs.Clear();
				this._listenSocketArgs = null;
				foreach (SocketAsyncEventArgs current2 in this._receiveSocketArgs)
				{
					current2.Dispose();
				}
				this._receiveSocketArgs.Clear();
				this._receiveSocketArgs = null;
			}
			catch (Exception ex)
			{
				Common.WriteLine("Stop TCP Thread: " + ex.Message, new string[0]);
			}
			Interlocked.Exchange(ref this._stopping, 0);
			this.CloseListenEvents();
			Common.WriteLine(this._threadName + " stopped", new string[0]);
		}
		public override int GetContextCount()
		{
			int result;
			lock (this._lockServer)
			{
				if (this._receiveSocketArgs == null)
				{
					result = 0;
				}
				else
				{
					result = this._receiveSocketArgs.Count;
				}
			}
			return result;
		}
		private void ServerThread(object state)
		{
			int num = 0;
			Common.WriteLine(this._threadName + " thread started", new string[0]);
			while (this._stopping == 0)
			{
				try
				{
					num = WaitHandle.WaitAny(this.semHandles, 200);
				}
				catch (Exception ex)
				{
					Common.WriteLine("ServerThread: " + ex.Message, new string[0]);
					continue;
				}
				if (num != 258)
				{
					if (num == 1)
					{
						break;
					}
					SocketAsyncEventArgs socketAsyncEventArgs = null;
					lock (this._lockServer)
					{
						if (this._listenSocketArgs.Count > 0)
						{
							socketAsyncEventArgs = this._listenSocketArgs.Dequeue();
						}
					}
					if (socketAsyncEventArgs != null)
					{
						try
						{
							socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
							if (!this._mainSocket.AcceptAsync(socketAsyncEventArgs))
							{
								if (socketAsyncEventArgs.SocketError == SocketError.Success)
								{
									this.OnSocketAccepted(socketAsyncEventArgs);
								}
								else
								{
									if (socketAsyncEventArgs.AcceptSocket != null)
									{
										socketAsyncEventArgs.AcceptSocket.Close();
									}
									lock (this._lockServer)
									{
										socketAsyncEventArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
										this._listenSocketArgs.Enqueue(socketAsyncEventArgs);
										try
										{
											if (this.semListenQueue != null)
											{
												this.semListenQueue.Release();
											}
										}
										catch (Exception)
										{
										}
									}
								}
							}
						}
						catch
						{
							if (socketAsyncEventArgs.AcceptSocket != null)
							{
								socketAsyncEventArgs.AcceptSocket.Close();
							}
							lock (this._lockServer)
							{
								socketAsyncEventArgs.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
								this._listenSocketArgs.Enqueue(socketAsyncEventArgs);
								try
								{
									if (this.semListenQueue != null)
									{
										this.semListenQueue.Release();
									}
								}
								catch (Exception)
								{
								}
							}
						}
					}
				}
			}
			lock (this._lockServer)
			{
				try
				{
					List<C> list = new List<C>();
					foreach (KeyValuePair<Socket, C> current in this._connections)
					{
						list.Add(current.Value);
					}
					foreach (C current2 in list)
					{
						this.AbortOnError(current2);
					}
				}
				catch (Exception)
				{
					Common.WriteLine("Error when killing all sockets: {0}", new string[]
					{
						this._connections.Count.ToString()
					});
				}
			}
			Common.WriteLine("[" + this._threadName + "] thread End", new string[0]);
			this._stoppedEvent.Set();
		}
		protected void IO_Completed(object sender, SocketAsyncEventArgs e)
		{
			SocketAsyncOperation lastOperation = e.LastOperation;
			if (lastOperation == SocketAsyncOperation.Accept)
			{
				this.OnSocketAccepted(e);
				return;
			}
			if (lastOperation == SocketAsyncOperation.Receive)
			{
				this.OnSocketReceived(e);
				return;
			}
			if (lastOperation != SocketAsyncOperation.Send)
			{
				throw new ArgumentException("The last operation completed on the socket was not a receive or send");
			}
			this.OnSocketSent(e);
		}
		protected virtual void OnSocketAccepted(SocketAsyncEventArgs e)
		{
			Socket acceptSocket = e.AcceptSocket;
			if (e.SocketError != SocketError.Success)
			{
				lock (this._lockServer)
				{
					if (acceptSocket != null)
					{
						if (acceptSocket.Connected)
						{
							acceptSocket.Shutdown(SocketShutdown.Both);
							acceptSocket.Disconnect(false);
						}
						acceptSocket.Close();
					}
					e.AcceptSocket = null;
					e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
					this._listenSocketArgs.Enqueue(e);
					try
					{
						if (this.semListenQueue != null)
						{
							this.semListenQueue.Release();
						}
					}
					catch (Exception)
					{
					}
				}
				return;
			}
			this._tLastPacket = (long)Environment.TickCount;
			C c = default(C);
			lock (this._lockServer)
			{
				c = base.CreateConnection(acceptSocket);
				if (c != null)
				{
					c._owner = this;
					ServicesAPI.OnAccepted(acceptSocket);
					Common.WriteLine(this._threadName + ": connection accepted: " + this._connected, new string[0]);
				}
				else
				{
					Common.WriteLine(this._threadName + ": failed to craete connection: " + this._connected, new string[0]);
				}
			}
			if (e.BytesTransferred > 0 && acceptSocket.Connected)
			{
				this.OnSocketReceived(e);
			}
			lock (this._lockServer)
			{
				e.AcceptSocket = null;
				e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
				this._listenSocketArgs.Enqueue(e);
				try
				{
					if (this.semListenQueue != null)
					{
						this.semListenQueue.Release();
					}
				}
				catch (Exception)
				{
				}
			}
			SocketAsyncEventArgs socketAsyncEventArgs = null;
			bool flag4 = true;
			lock (this._lockServer)
			{
				if (this._receiveSocketArgs.Count == 0)
				{
					c = base.GetConnection(acceptSocket);
					if (c != null)
					{
						base.RemoveConnection(acceptSocket);
					}
					ServicesAPI.OnDisconnected(acceptSocket);
					lock (this._lockServer)
					{
						if (acceptSocket.Connected)
						{
							acceptSocket.Shutdown(SocketShutdown.Both);
							acceptSocket.Disconnect(false);
						}
						acceptSocket.Close();
					}
					Common.WriteLine(this._threadName + ": connection limited to: " + this._connected, new string[0]);
					return;
				}
				if (acceptSocket.Connected)
				{
					socketAsyncEventArgs = this._receiveSocketArgs.Dequeue();
					socketAsyncEventArgs.UserToken = acceptSocket;
					socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
					flag4 = acceptSocket.ReceiveAsync(socketAsyncEventArgs);
				}
				else
				{
					ServicesAPI.OnDisconnected(acceptSocket);
					acceptSocket.Close();
				}
			}
			if (!flag4 && socketAsyncEventArgs != null)
			{
				this.OnSocketReceived(socketAsyncEventArgs);
			}
		}
		private void OnSocketReceived(SocketAsyncEventArgs e)
		{
			bool flag = false;
			bool flag2 = true;
			Socket socket = e.AcceptSocket;
			if (e.LastOperation == SocketAsyncOperation.Receive)
			{
				socket = (e.UserToken as Socket);
			}
			C connection = base.GetConnection(socket);
			lock (this._lockServer)
			{
				if (e.SocketError != SocketError.Success || e.BytesTransferred == 0 || !socket.Connected)
				{
					if (connection != null)
					{
						base.RemoveConnection(socket);
					}
					ServicesAPI.OnDisconnected(socket);
					if (e.LastOperation == SocketAsyncOperation.Receive)
					{
						if (socket.Connected)
						{
							socket.Shutdown(SocketShutdown.Both);
							socket.Disconnect(false);
						}
						socket.Close();
						e.AcceptSocket = null;
						e.UserToken = null;
						e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
						this._receiveSocketArgs.Enqueue(e);
					}
					Common.WriteLine(this._threadName + ": connection down: " + this._connected, new string[0]);
					return;
				}
				this._speedMeter.Received(e.BytesTransferred);
				if (connection != null)
				{
					BufferState bufferState = new BufferState();
					bufferState._ctx = connection;
					bufferState._buffer = e.Buffer;
					if (bufferState._buffer != null)
					{
						Interlocked.Increment(ref this._totalRequests);
						SessionAPI.Update(connection._uid);
						int num = 0;
						List<byte[]> list = connection.ReceivedBuffer(bufferState, e.BytesTransferred, ref num);
						if (list != null)
						{
							flag2 = false;
							foreach (byte[] current in list)
							{
								if (!socket.Connected)
								{
									break;
								}
								this.StartSend(connection, current);
							}
						}
					}
				}
			}
			if (!flag2)
			{
				if (e.LastOperation == SocketAsyncOperation.Receive)
				{
					flag = true;
					lock (this._lockServer)
					{
						if (socket.Connected)
						{
							flag = socket.ReceiveAsync(e);
						}
						else
						{
							ServicesAPI.OnDisconnected(socket);
							if (connection != null)
							{
								base.RemoveConnection(socket);
							}
							e.AcceptSocket = null;
							e.UserToken = null;
							e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
							this._receiveSocketArgs.Enqueue(e);
						}
					}
					if (!flag)
					{
						this.OnSocketReceived(e);
					}
				}
				return;
			}
			if (connection != null)
			{
				base.RemoveConnection(socket);
			}
			ServicesAPI.OnDisconnected(socket);
			lock (this._lockServer)
			{
				if (socket.Connected)
				{
					socket.Shutdown(SocketShutdown.Both);
					socket.Disconnect(false);
				}
				socket.Close();
			}
			if (e.LastOperation == SocketAsyncOperation.Receive)
			{
				e.AcceptSocket = null;
				e.UserToken = null;
				e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
				this._receiveSocketArgs.Enqueue(e);
			}
			Common.WriteLine("Receive Processing Error", new string[0]);
		}
		protected void OnSocketSent(SocketAsyncEventArgs e)
		{
			if (e.SocketError != SocketError.Success)
			{
				lock (this._lockServer)
				{
					ServicesAPI.OnDisconnected(e.AcceptSocket);
					if (e.AcceptSocket.Connected)
					{
						e.AcceptSocket.Shutdown(SocketShutdown.Both);
						e.AcceptSocket.Disconnect(false);
					}
					e.AcceptSocket.Close();
					Common.WriteLine(this._threadName + ": send error: " + this._connected, new string[0]);
					goto IL_11A;
				}
			}
			this._speedMeter.Sent(e.BytesTransferred);
			Socket acceptSocket = e.AcceptSocket;
			C connection = base.GetConnection(acceptSocket);
			if (connection != null)
			{
				ulong num = BitConverter.ToUInt64(e.Buffer, 0);
				num = ecoServerProtocol.swap64(num);
				uint packetType = ecoServerProtocol.getPacketType(num);
				if (packetType == 32u)
				{
					try
					{
						Thread.Sleep(200);
					}
					catch (Exception)
					{
					}
					if (acceptSocket.Connected)
					{
						acceptSocket.Shutdown(SocketShutdown.Both);
						acceptSocket.Disconnect(false);
					}
					acceptSocket.Close();
					e.Dispose();
					return;
				}
				this.IsDispatch(connection);
				this.OnBaseSent(connection, e);
			}
			IL_11A:
			e.Dispose();
		}
		public override void AsyncSend(object c, byte[] data)
		{
			this.StartSend((C)((object)c), data);
		}
		public virtual void StartSend(C c, byte[] sendBuffer)
		{
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			socketAsyncEventArgs.AcceptSocket = c._sock;
			socketAsyncEventArgs.RemoteEndPoint = c.getRemoteEndPoint();
			socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
			lock (this._lockServer)
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
		public override void SendAsync(C connection, SocketAsyncEventArgs e)
		{
			Socket socket = connection.getSocket();
			e.AcceptSocket = connection.getSocket();
			e.Completed += new EventHandler<SocketAsyncEventArgs>(this.IO_Completed);
			lock (this._lockServer)
			{
				bool flag2 = true;
				if (socket.Connected)
				{
					lock (this._lockSend)
					{
						flag2 = socket.SendAsync(e);
					}
				}
				if (!flag2)
				{
					this.OnSocketSent(e);
				}
			}
		}
		public override void SendUrgency(Socket sock, int uid, byte[] packet)
		{
			List<C> list = new List<C>();
			lock (this._lockServer)
			{
				if (this._connections.Count > 0)
				{
					using (Dictionary<Socket, C>.Enumerator enumerator = this._connections.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Socket, C> current = enumerator.Current;
							if (uid == 0)
							{
								list.Add(current.Value);
							}
							else
							{
								if ((long)uid == current.Value._uid)
								{
									list.Add(current.Value);
									break;
								}
							}
						}
						goto IL_9D;
					}
				}
				Common.WriteLine("Urgency to Nobody", new string[0]);
				IL_9D:;
			}
			foreach (C current2 in list)
			{
				if (current2 != null)
				{
					byte[] sendBuffer = (byte[])packet.Clone();
					this.StartSend(current2, sendBuffer);
				}
			}
		}
		public override void DispatchDataset(DispatchAttribute attrib)
		{
			lock (this._lockServer)
			{
				if (attrib != null)
				{
					if (this._connections.Count > 0)
					{
						if ((attrib.type & 8) != 0)
						{
							SessionAPI.AuthorizationUpdate("");
						}
						using (Dictionary<Socket, C>.Enumerator enumerator = this._connections.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<Socket, C> current = enumerator.Current;
								C value = current.Value;
								if ((attrib.uid == 0 && value._vid == 0L) || ((long)attrib.uid == value._uid && (long)attrib.vid == value._vid))
								{
									if (attrib.uid == 0 && (attrib.type & 8192) == 0 && SessionAPI.IsUserUAC(value._uid))
									{
										DispatchAttribute copy = attrib.getCopy();
										copy.uid = (int)value._uid;
										SessionAPI.toTracker(copy);
										if (this._handler != null)
										{
											this._handler.AutoPull4UAC(value);
										}
									}
									else
									{
										if ((attrib.type & 8192) != 0)
										{
											bool flag2 = true;
											if (attrib.uid == 0 && attrib.operation.StartsWith("USER:"))
											{
												int num = attrib.operation.IndexOf(":");
												if (num >= 0)
												{
													string text = attrib.operation.Substring(num + 1).Trim();
													if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(value._userName) || !text.Equals(value._userName, StringComparison.CurrentCultureIgnoreCase))
													{
														flag2 = false;
													}
												}
											}
											if (flag2)
											{
												this.SendBroadcast(value, attrib);
											}
										}
										else
										{
											if (attrib.data != null)
											{
												bool flag3 = false;
												if (attrib.type == 1 && value._isRealTimePending)
												{
													if (attrib.discard_if_jam <= 0)
													{
														Common.WriteLine("XXXXXXXXXXXXXX Important Realtime data can not be discarded", new string[0]);
													}
													else
													{
														flag3 = true;
														string text2 = "";
														if (value._curDispatchAttrib != null)
														{
															text2 += value._curDispatchAttrib.type.ToString();
														}
														int num2 = 0;
														foreach (DispatchAttribute current2 in value._datasetRequestQueue)
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
														Common.WriteLine("XXXXXXXXXXXXXX Realtime data is already on the way @DispatchDataset({0}), data discarded", new string[]
														{
															text2
														});
													}
												}
												if (!flag3)
												{
													DispatchAttribute copy2 = attrib.getCopy();
													if (value.setDataSet(copy2))
													{
														this.StartDispatch(value);
													}
												}
											}
										}
									}
								}
							}
							goto IL_315;
						}
					}
					Common.WriteLine("Dispatch to Nobody", new string[0]);
				}
				IL_315:;
			}
		}
		public void SendBroadcast(C ctx, DispatchAttribute attr)
		{
			lock (this._lockServer)
			{
				if (this._handler != null)
				{
					try
					{
						byte[] array = this._handler.BuildBroadcast(attr);
						if (array != null)
						{
							this.StartSend(ctx, array);
						}
					}
					catch (Exception ex)
					{
						if (ctx._sock != null && ctx._sock.Connected)
						{
							ctx._sock.Close();
						}
						Common.WriteLine("SendBroadcast failed: {0}", new string[]
						{
							ex.Message
						});
					}
				}
			}
		}
		public bool StartDispatch(C ctx)
		{
			lock (this._lockServer)
			{
				if (ctx._curDispatchAttrib == null)
				{
					bool result = false;
					return result;
				}
				if (this._handler == null)
				{
					bool result = false;
					return result;
				}
				try
				{
					byte[] array = this._handler.BuildFirstDispatch(ctx._curDispatchAttrib);
					bool result;
					if (array == null)
					{
						result = false;
						return result;
					}
					this.StartSend(ctx, array);
					result = true;
					return result;
				}
				catch (Exception ex)
				{
					if (ctx._sock != null && ctx._sock.Connected)
					{
						ctx._sock.Close();
					}
					Common.WriteLine("DisptachDataSetStart failed: {0}", new string[]
					{
						ex.Message
					});
				}
			}
			return false;
		}
		private bool DispatchNext(C ctx)
		{
			bool result;
			lock (this._lockServer)
			{
				if (ctx._curDispatchAttrib == null)
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
							byte[] array = this._handler.BuildNextDispatch(ctx._curDispatchAttrib);
							if (array == null)
							{
								result = false;
								return result;
							}
							this.StartSend(ctx, array);
							result = true;
							return result;
						}
						catch (Exception ex)
						{
							this.AbortOnError(ctx);
							Common.WriteLine("DispatchNext failed: {0}", new string[]
							{
								ex.Message
							});
						}
						result = false;
					}
				}
			}
			return result;
		}
		protected virtual bool IsDispatch(C ctx)
		{
			lock (this._lockServer)
			{
				if (ctx._curDispatchAttrib == null)
				{
					bool result = true;
					return result;
				}
				if (this._handler == null)
				{
					bool result = false;
					return result;
				}
				if (ctx._curDispatchAttrib.dispatchPointer >= 0 && ctx._curDispatchAttrib.dispatchPointer >= ctx._curDispatchAttrib.data.Length)
				{
					try
					{
						if ((ctx._curDispatchAttrib.type & 1) != 0)
						{
							ctx._isRealTimePending = false;
						}
						ctx._curDispatchAttrib.data = null;
						ctx._curDispatchAttrib.dispatchPointer = -1;
						if (ctx._datasetRequestQueue.Count > 0)
						{
							ctx._curDispatchAttrib = ctx._datasetRequestQueue.Dequeue();
							bool result = this.StartDispatch(ctx);
							return result;
						}
						ctx._curDispatchAttrib = null;
					}
					catch (Exception ex)
					{
						Common.WriteLine("IsDispatch:" + ex.Message, new string[0]);
					}
				}
				if (ctx._curDispatchAttrib == null || ctx._curDispatchAttrib.dispatchPointer < 0)
				{
					ctx._curDispatchAttrib = null;
					bool result = false;
					return result;
				}
			}
			return this.DispatchNext(ctx);
		}
	}
}
