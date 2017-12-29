using Dispatcher;
using EcoMessenger;
using ecoProtocols;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
namespace SocketServer
{
	public class ServerSSL<C, T> : BaseTCPServer<C, T> where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		public ServerSSL(ServerConfig config) : base(config)
		{
		}
		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}
			Common.WriteLine("Certificate error: {0}", new string[]
			{
				sslPolicyErrors.ToString()
			});
			return true;
		}
		protected override void OnSocketAccepted(SocketAsyncEventArgs e)
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
					e.Completed -= new EventHandler<SocketAsyncEventArgs>(base.IO_Completed);
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
			lock (this._lockServer)
			{
				e.AcceptSocket = null;
				e.Completed -= new EventHandler<SocketAsyncEventArgs>(base.IO_Completed);
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
				}
				else
				{
					if (acceptSocket.Connected)
					{
						Common.WriteLine(this._threadName + ": SSL accepting...", new string[0]);
						try
						{
							c._netStream = new NetworkStream(acceptSocket);
							c._sslStream = new SslStream(c._netStream, false, new RemoteCertificateValidationCallback(ServerSSL<C, T>.ValidateServerCertificate), null);
							c._sslStream.BeginAuthenticateAsServer(this._config.TlsCertificate, new AsyncCallback(this.OnSslConnected), c);
							return;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Exception: {0}", ex.Message);
							if (ex.InnerException != null)
							{
								Console.WriteLine("Inner exception: {0}", ex.InnerException.Message);
							}
							Console.WriteLine("Authentication failed - closing the connection.");
							if (acceptSocket.Connected)
							{
								acceptSocket.Shutdown(SocketShutdown.Both);
							}
							acceptSocket.Close();
							return;
						}
					}
					ServicesAPI.OnDisconnected(acceptSocket);
					acceptSocket.Close();
				}
			}
		}
		public void OnSslConnected(IAsyncResult ar)
		{
			lock (this._lockServer)
			{
				C c = default(C);
				try
				{
					Common.WriteLine("    OnSslConnected", new string[0]);
					c = (ar.AsyncState as C);
					c._sslStream.EndAuthenticateAsServer(ar);
					c._sslConnected = true;
					BufferState bufferState = new BufferState();
					bufferState._ctx = c;
					bufferState._buffer = new byte[4096];
					if (bufferState._buffer != null)
					{
						c._sslStream.BeginRead(bufferState._buffer, 0, bufferState._buffer.Length, new AsyncCallback(this.ReadCallback), bufferState);
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    OnSslConnected: {0}", new string[]
					{
						ex.Message
					});
					if (c != null)
					{
						if (c._sslStream != null)
						{
							c._sslStream.Dispose();
							c._sslStream = null;
						}
						if (c._sock.Connected)
						{
							c._sock.Shutdown(SocketShutdown.Both);
						}
						c._sock.Close();
					}
				}
			}
		}
		private void ReadCallback(IAsyncResult ar)
		{
			lock (this._lockServer)
			{
				BufferState bufferState = ar.AsyncState as BufferState;
				C c = (C)((object)bufferState._ctx);
				try
				{
					int num = c._sslStream.EndRead(ar);
					if (num > 0)
					{
						this._speedMeter.Received(num);
						c._tLastReceived = (long)Environment.TickCount;
						int num2 = 0;
						List<byte[]> list = c.ReceivedBuffer(bufferState, num, ref num2);
						if (list != null)
						{
							foreach (byte[] current in list)
							{
								if (!c._sock.Connected)
								{
									break;
								}
								lock (this._lockSend)
								{
									this.EnqueueDataForWrite(new BufferState
									{
										_ctx = c,
										_buffer = current
									});
								}
							}
						}
						BufferState bufferState2 = new BufferState();
						bufferState2._ctx = c;
						bufferState2._buffer = new byte[4096];
						if (bufferState2._buffer != null)
						{
							c._sslStream.BeginRead(bufferState2._buffer, 0, bufferState2._buffer.Length, new AsyncCallback(this.ReadCallback), bufferState2);
						}
					}
					else
					{
						Common.WriteLine("    Peer Closed", new string[0]);
						this.AbortOnError(c);
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    ReadCallback: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(c);
				}
			}
		}
		private void WriteCallback(IAsyncResult ar)
		{
			lock (this._lockServer)
			{
				BufferState bufferState = ar.AsyncState as BufferState;
				C c = (C)((object)bufferState._ctx);
				try
				{
					this._speedMeter.Sent(bufferState._buffer.Length);
					c._sslStream.EndWrite(ar);
					c._tLastSent = (long)Environment.TickCount;
					ulong num = BitConverter.ToUInt64(bufferState._buffer, 0);
					num = ecoServerProtocol.swap64(num);
					uint packetType = ecoServerProtocol.getPacketType(num);
					bufferState._buffer = null;
					if (packetType == 32u)
					{
						try
						{
							Thread.Sleep(200);
						}
						catch (Exception)
						{
						}
						this.AbortOnError(c);
					}
					else
					{
						this.IsDispatch(c);
						this.CheckSslSending(c);
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    WriteCallback: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(c);
				}
			}
		}
		public override void AsyncSend(object c, byte[] data)
		{
			this.StartSend((C)((object)c), data);
		}
		public override void SendAsync(C connection, SocketAsyncEventArgs e)
		{
		}
		public override void StartSend(C c, byte[] sendBuffer)
		{
			lock (this._lockServer)
			{
				try
				{
					if (c._sock.Connected)
					{
						this.EnqueueDataForWrite(new BufferState
						{
							_ctx = c,
							_buffer = sendBuffer
						});
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    StartSend: {0}", new string[]
					{
						ex.Message
					});
					this.AbortOnError(c);
				}
			}
		}
		private void EnqueueDataForWrite(BufferState toSend)
		{
			lock (this._lockSend)
			{
				if (toSend != null)
				{
					C c = (C)((object)toSend._ctx);
					lock (c._sslPendingQ)
					{
						c._sslPendingQ.Enqueue(toSend);
						if (!c._sslSending)
						{
							try
							{
								BufferState bufferState = null;
								if (c._sslPendingQ.Count > 0 && c._sslPendingQ.TryDequeue(out bufferState))
								{
									c = (C)((object)bufferState._ctx);
									c._sslSending = true;
									c._sslStream.BeginWrite(bufferState._buffer, 0, bufferState._buffer.Length, new AsyncCallback(this.WriteCallback), bufferState);
								}
								else
								{
									c._sslSending = false;
								}
							}
							catch (Exception ex)
							{
								c._sslSending = false;
								Common.WriteLine("    EnqueueDataForWrite: {0}", new string[]
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
		private void CheckSslSending(C c)
		{
			if (c == null)
			{
				return;
			}
			BufferState bufferState = null;
			lock (c._sslPendingQ)
			{
				try
				{
					if (c._sslPendingQ.Count > 0 && c._sslPendingQ.TryDequeue(out bufferState))
					{
						c = (C)((object)bufferState._ctx);
						c._sslSending = true;
						c._sslStream.BeginWrite(bufferState._buffer, 0, bufferState._buffer.Length, new AsyncCallback(this.WriteCallback), bufferState);
					}
					else
					{
						c._sslSending = false;
					}
				}
				catch (Exception)
				{
					c._sslSending = false;
				}
			}
		}
	}
}
