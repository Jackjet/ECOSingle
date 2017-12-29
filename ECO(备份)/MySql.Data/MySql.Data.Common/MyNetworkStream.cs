using MySql.Data.MySqlClient;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
namespace MySql.Data.Common
{
	internal class MyNetworkStream : NetworkStream
	{
		private const int MaxRetryCount = 2;
		private Socket socket;
		public MyNetworkStream(Socket socket, bool ownsSocket) : base(socket, ownsSocket)
		{
			this.socket = socket;
		}
		private bool IsTimeoutException(SocketException e)
		{
			return e.SocketErrorCode == SocketError.TimedOut;
		}
		private bool IsWouldBlockException(SocketException e)
		{
			return e.SocketErrorCode == SocketError.WouldBlock;
		}
		private void HandleOrRethrowException(Exception e)
		{
			for (Exception ex = e; ex != null; ex = ex.InnerException)
			{
				if (ex is SocketException)
				{
					SocketException e2 = (SocketException)ex;
					if (this.IsWouldBlockException(e2))
					{
						this.socket.Blocking = true;
						return;
					}
					if (this.IsTimeoutException(e2))
					{
						return;
					}
				}
			}
			throw e;
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			Exception ex = null;
			do
			{
				try
				{
					int result = base.Read(buffer, offset, count);
					return result;
				}
				catch (Exception ex2)
				{
					ex = ex2;
					this.HandleOrRethrowException(ex2);
				}
			}
			while (++num < 2);
			if (ex.GetBaseException() is SocketException && this.IsTimeoutException((SocketException)ex.GetBaseException()))
			{
				throw new TimeoutException(ex.Message, ex);
			}
			throw ex;
		}
		public override int ReadByte()
		{
			int num = 0;
			Exception ex = null;
			do
			{
				try
				{
					return base.ReadByte();
				}
				catch (Exception ex2)
				{
					ex = ex2;
					this.HandleOrRethrowException(ex2);
				}
			}
			while (++num < 2);
			throw ex;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = 0;
			Exception ex = null;
			do
			{
				try
				{
					base.Write(buffer, offset, count);
					return;
				}
				catch (Exception ex2)
				{
					ex = ex2;
					this.HandleOrRethrowException(ex2);
				}
			}
			while (++num < 2);
			throw ex;
		}
		public override void Flush()
		{
			int num = 0;
			Exception ex = null;
			do
			{
				try
				{
					base.Flush();
					return;
				}
				catch (Exception ex2)
				{
					ex = ex2;
					this.HandleOrRethrowException(ex2);
				}
			}
			while (++num < 2);
			throw ex;
		}
		public static MyNetworkStream CreateStream(MySqlConnectionStringBuilder settings, bool unix)
		{
			MyNetworkStream myNetworkStream = null;
			IPHostEntry hostEntry = MyNetworkStream.GetHostEntry(settings.Server);
			IPAddress[] addressList = hostEntry.AddressList;
			for (int i = 0; i < addressList.Length; i++)
			{
				IPAddress ip = addressList[i];
				try
				{
					myNetworkStream = MyNetworkStream.CreateSocketStream(settings, ip, unix);
					if (myNetworkStream != null)
					{
						break;
					}
				}
				catch (Exception ex2)
				{
					SocketException ex = ex2 as SocketException;
					if (ex == null)
					{
						throw;
					}
					if (ex.SocketErrorCode != SocketError.ConnectionRefused)
					{
						throw;
					}
				}
			}
			return myNetworkStream;
		}
		private static IPHostEntry ParseIPAddress(string hostname)
		{
			IPHostEntry iPHostEntry = null;
			IPAddress iPAddress;
			if (IPAddress.TryParse(hostname, out iPAddress))
			{
				iPHostEntry = new IPHostEntry();
				iPHostEntry.AddressList = new IPAddress[1];
				iPHostEntry.AddressList[0] = iPAddress;
			}
			return iPHostEntry;
		}
		private static IPHostEntry GetHostEntry(string hostname)
		{
			IPHostEntry iPHostEntry = MyNetworkStream.ParseIPAddress(hostname);
			if (iPHostEntry != null)
			{
				return iPHostEntry;
			}
			return Dns.GetHostEntry(hostname);
		}
		private static EndPoint CreateUnixEndPoint(string host)
		{
			Assembly assembly = Assembly.Load("Mono.Posix, Version=2.0.0.0, \t\t\t\t\r\n                Culture=neutral, PublicKeyToken=0738eb9f132ed756");
			return (EndPoint)assembly.CreateInstance("Mono.Posix.UnixEndPoint", false, BindingFlags.CreateInstance, null, new object[]
			{
				host
			}, null, null);
		}
		private static MyNetworkStream CreateSocketStream(MySqlConnectionStringBuilder settings, IPAddress ip, bool unix)
		{
			EndPoint remoteEP;
			if (!Platform.IsWindows() && unix)
			{
				remoteEP = MyNetworkStream.CreateUnixEndPoint(settings.Server);
			}
			else
			{
				remoteEP = new IPEndPoint(ip, (int)settings.Port);
			}
			Socket socket = unix ? new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP) : new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (settings.Keepalive > 0u)
			{
				MyNetworkStream.SetKeepAlive(socket, settings.Keepalive);
			}
			IAsyncResult asyncResult = socket.BeginConnect(remoteEP, null, null);
			if (!asyncResult.AsyncWaitHandle.WaitOne((int)(settings.ConnectionTimeout * 1000u), false))
			{
				socket.Close();
				return null;
			}
			try
			{
				socket.EndConnect(asyncResult);
			}
			catch (Exception)
			{
				socket.Close();
				throw;
			}
			MyNetworkStream myNetworkStream = new MyNetworkStream(socket, true);
			GC.SuppressFinalize(socket);
			GC.SuppressFinalize(myNetworkStream);
			return myNetworkStream;
		}
		private static void SetKeepAlive(Socket s, uint time)
		{
			uint value = 1u;
			uint value2 = 1000u;
			byte[] array = new byte[12];
			BitConverter.GetBytes(value).CopyTo(array, 0);
			BitConverter.GetBytes(time).CopyTo(array, 4);
			BitConverter.GetBytes(value2).CopyTo(array, 8);
			try
			{
				s.IOControl((IOControlCode)((ulong)-1744830460), array, null);
				return;
			}
			catch (NotImplementedException)
			{
			}
			s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
		}
	}
}
