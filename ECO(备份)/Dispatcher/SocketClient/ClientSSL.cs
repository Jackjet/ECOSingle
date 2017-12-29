using EcoMessenger;
using ecoProtocols;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
namespace SocketClient
{
	public class ClientSSL<C, T> : BaseTCPClient<C, T> where C : ConnectContext, IDisposable, new() where T : MessageBase, IDisposable, new()
	{
		public ClientSSL(ClientConfig config) : base(config)
		{
			this._context.ResetSSL();
		}
		public void ResetSSL()
		{
			this._context.ResetSSL();
		}
		public override void AsyncSend(object c, byte[] data)
		{
			this.StartSend((C)((object)c), data);
		}
		public override void Socket_Completed(object sender, SocketAsyncEventArgs e)
		{
			SocketAsyncOperation lastOperation = e.LastOperation;
			if (lastOperation != SocketAsyncOperation.Connect)
			{
				return;
			}
			this.OnConnected(sender, e);
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
		public override void OnConnected(object sender, SocketAsyncEventArgs e)
		{
			lock (this.thisClientLock)
			{
				this.ResetSSL();
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
							if (socket.Connected)
							{
								socket.Shutdown(SocketShutdown.Both);
							}
							socket.Close();
						}
						e.UserToken = null;
						e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						this._tDelayConnectFrom.Start();
						Interlocked.Exchange(ref this._nConnectState, -1L);
						Common.WriteLine("    Failed to connect", new string[0]);
					}
					else
					{
						e.Completed -= new EventHandler<SocketAsyncEventArgs>(this.Socket_Completed);
						this._context.setConnected(socket);
						this._tLastTransfer = (long)Environment.TickCount;
						this._tDelayConnectFrom.Stop();
						Interlocked.Exchange(ref this._nConnectState, 2L);
						Common.WriteLine(string.Concat(new object[]
						{
							"[#",
							socket.Handle,
							"]Connected: ssl=",
							this._clientConfig.ssl.ToString()
						}), new string[0]);
						if (this._clientConfig.ssl)
						{
							Common.WriteLine("[#" + socket.Handle + "]SSL Authenticating", new string[0]);
							try
							{
								this._context._netStream = new NetworkStream(socket);
								this._context._sslStream = new SslStream(this._context._netStream, false, new RemoteCertificateValidationCallback(ClientSSL<C, T>.ValidateServerCertificate), null);
								this._context._sslStream.BeginAuthenticateAsClient(this._clientConfig.hostName, new AsyncCallback(this.OnAuthenticate), this._context);
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
								if (socket.Connected)
								{
									socket.Shutdown(SocketShutdown.Both);
								}
								socket.Close();
								return;
							}
						}
						if (socket != null && socket.Connected)
						{
							socket.Close();
						}
						Common.WriteLine("non-SSL not supported: {0}", new string[0]);
					}
				}
				catch (Exception ex2)
				{
					if (socket != null && socket.Connected)
					{
						socket.Close();
					}
					Common.WriteLine("OnConnected failed: {0}", new string[]
					{
						ex2.Message
					});
				}
			}
		}
		public void OnAuthenticate(IAsyncResult ar)
		{
			lock (this.thisClientLock)
			{
				C c = ar.AsyncState as C;
				try
				{
					Common.WriteLine("    OnAuthenticate done", new string[0]);
					if (!c._sock.Connected)
					{
						Common.WriteLine("    Socket is down while authenticating", new string[0]);
						base.AbortOnError(c);
					}
					else
					{
						c._sslConnected = true;
						BufferState bufferState = new BufferState();
						bufferState._ctx = c;
						bufferState._buffer = new byte[4096];
						if (bufferState._buffer != null)
						{
							c._sslStream.BeginRead(bufferState._buffer, 0, bufferState._buffer.Length, new AsyncCallback(this.ReadCallback), bufferState);
							this.OnSayHello();
						}
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    OnAuthenticate: {0}", new string[]
					{
						ex.Message
					});
					base.AbortOnError(c);
				}
			}
		}
		private void ReadCallback(IAsyncResult ar)
		{
			lock (this.thisClientLock)
			{
				BufferState bufferState = ar.AsyncState as BufferState;
				C c = (C)((object)bufferState._ctx);
				try
				{
					int num = c._sslStream.EndRead(ar);
					if (num > 0)
					{
						this._speedMeter.Received(num);
						int num2 = 0;
						List<byte[]> list = c.ReceivedBuffer(bufferState, num, ref num2);
						if (num2 > 0)
						{
							this._tLastReceivedPacket = (long)Environment.TickCount;
						}
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
						base.AbortOnError(c);
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    ReadCallback: {0}", new string[]
					{
						ex.Message
					});
					base.AbortOnError(c);
				}
			}
		}
		private void WriteCallback(IAsyncResult ar)
		{
			lock (this.thisClientLock)
			{
				BufferState bufferState = ar.AsyncState as BufferState;
				C c = (C)((object)bufferState._ctx);
				try
				{
					this._speedMeter.Sent(bufferState._buffer.Length);
					c._sslStream.EndWrite(ar);
					c._tLastSent = (long)Environment.TickCount;
					this._tLastTransfer = (long)Environment.TickCount;
					ulong num = BitConverter.ToUInt64(bufferState._buffer, 0);
					num = ecoServerProtocol.swap64(num);
					uint packetType = ecoServerProtocol.getPacketType(num);
					if (packetType == 258u)
					{
						this.IsDispatch(c);
					}
					this.CheckSslSending(c);
				}
				catch (Exception ex)
				{
					Common.WriteLine("    WriteCallback: {0}", new string[]
					{
						ex.Message
					});
					base.AbortOnError(c);
				}
			}
		}
		public override void StartSend(C c, byte[] sendBuffer)
		{
			lock (this._lockSend)
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
					base.AbortOnError(c);
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
						if (c._sslSending)
						{
							return;
						}
					}
					this.CheckSslSending(c);
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
					lock (c._sslPendingQ)
					{
						c._sslSending = false;
					}
				}
			}
			catch (Exception)
			{
				base.AbortOnError(c);
				lock (c._sslPendingQ)
				{
					c._sslSending = false;
				}
			}
		}
	}
}
