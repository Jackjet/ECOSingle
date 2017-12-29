using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
namespace CommonAPI.network
{
	public class NetworkShareAccesser : IDisposable
	{
		private struct ErrorClass
		{
			public int num;
			public string message;
			public ErrorClass(int num, string message)
			{
				this.num = num;
				this.message = message;
			}
		}
		[StructLayout(LayoutKind.Sequential)]
		private class NETRESOURCE
		{
			public int dwScope;
			public int dwType;
			public int dwDisplayType;
			public int dwUsage;
			public string lpLocalName = "";
			public string lpRemoteName = "";
			public string lpComment = "";
			public string lpProvider = "";
		}
		private const int RESOURCE_CONNECTED = 1;
		private const int RESOURCE_GLOBALNET = 2;
		private const int RESOURCE_REMEMBERED = 3;
		private const int RESOURCETYPE_ANY = 0;
		private const int RESOURCETYPE_DISK = 1;
		private const int RESOURCETYPE_PRINT = 2;
		private const int RESOURCEDISPLAYTYPE_GENERIC = 0;
		private const int RESOURCEDISPLAYTYPE_DOMAIN = 1;
		private const int RESOURCEDISPLAYTYPE_SERVER = 2;
		private const int RESOURCEDISPLAYTYPE_SHARE = 3;
		private const int RESOURCEDISPLAYTYPE_FILE = 4;
		private const int RESOURCEDISPLAYTYPE_GROUP = 5;
		private const int RESOURCEUSAGE_CONNECTABLE = 1;
		private const int RESOURCEUSAGE_CONTAINER = 2;
		private const int CONNECT_INTERACTIVE = 8;
		private const int CONNECT_PROMPT = 16;
		private const int CONNECT_REDIRECT = 128;
		private const int CONNECT_UPDATE_PROFILE = 1;
		private const int CONNECT_COMMANDLINE = 2048;
		private const int CONNECT_CMD_SAVECRED = 4096;
		private const int CONNECT_LOCALDRIVE = 256;
		private const int NO_ERROR = 0;
		private const int ERROR_ACCESS_DENIED = 5;
		private const int ERROR_ALREADY_ASSIGNED = 85;
		private const int ERROR_BAD_DEVICE = 1200;
		private const int ERROR_BAD_NET_NAME = 67;
		private const int ERROR_BAD_PROVIDER = 1204;
		private const int ERROR_CANCELLED = 1223;
		private const int ERROR_EXTENDED_ERROR = 1208;
		private const int ERROR_INVALID_ADDRESS = 487;
		private const int ERROR_INVALID_PARAMETER = 87;
		private const int ERROR_INVALID_PASSWORD = 1216;
		private const int ERROR_MORE_DATA = 234;
		private const int ERROR_NO_MORE_ITEMS = 259;
		private const int ERROR_NO_NET_OR_BAD_PATH = 1203;
		private const int ERROR_NO_NETWORK = 1222;
		private const int ERROR_BAD_PROFILE = 1206;
		private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
		private const int ERROR_DEVICE_IN_USE = 2404;
		private const int ERROR_NOT_CONNECTED = 2250;
		private const int ERROR_OPEN_FILES = 2401;
		private string _remoteUncName;
		private string _remoteComputerName;
		private static NetworkShareAccesser.ErrorClass[] ERROR_LIST = new NetworkShareAccesser.ErrorClass[]
		{
			new NetworkShareAccesser.ErrorClass(5, "Error: Access Denied"),
			new NetworkShareAccesser.ErrorClass(85, "Error: Already Assigned"),
			new NetworkShareAccesser.ErrorClass(1200, "Error: Bad Device"),
			new NetworkShareAccesser.ErrorClass(67, "Error: Bad Net Name"),
			new NetworkShareAccesser.ErrorClass(1204, "Error: Bad Provider"),
			new NetworkShareAccesser.ErrorClass(1223, "Error: Cancelled"),
			new NetworkShareAccesser.ErrorClass(1208, "Error: Extended Error"),
			new NetworkShareAccesser.ErrorClass(487, "Error: Invalid Address"),
			new NetworkShareAccesser.ErrorClass(87, "Error: Invalid Parameter"),
			new NetworkShareAccesser.ErrorClass(1216, "Error: Invalid Password"),
			new NetworkShareAccesser.ErrorClass(234, "Error: More Data"),
			new NetworkShareAccesser.ErrorClass(259, "Error: No More Items"),
			new NetworkShareAccesser.ErrorClass(1203, "Error: No Net Or Bad Path"),
			new NetworkShareAccesser.ErrorClass(1222, "Error: No Network"),
			new NetworkShareAccesser.ErrorClass(1206, "Error: Bad Profile"),
			new NetworkShareAccesser.ErrorClass(1205, "Error: Cannot Open Profile"),
			new NetworkShareAccesser.ErrorClass(2404, "Error: Device In Use"),
			new NetworkShareAccesser.ErrorClass(1208, "Error: Extended Error"),
			new NetworkShareAccesser.ErrorClass(2250, "Error: Not Connected"),
			new NetworkShareAccesser.ErrorClass(2401, "Error: Open Files")
		};
		public string RemoteComputerName
		{
			get
			{
				return this._remoteComputerName;
			}
			set
			{
				this._remoteComputerName = value;
				this._remoteUncName = "\\\\" + this._remoteComputerName;
			}
		}
		public string UserName
		{
			get;
			set;
		}
		public string Password
		{
			get;
			set;
		}
		public string Result
		{
			get;
			set;
		}
		[DllImport("Mpr.dll")]
		private static extern int WNetUseConnection(IntPtr hwndOwner, NetworkShareAccesser.NETRESOURCE lpNetResource, string lpPassword, string lpUserID, int dwFlags, string lpAccessName, string lpBufferSize, string lpResult);
		[DllImport("Mpr.dll")]
		private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);
		public static NetworkShareAccesser Access(string remoteComputerName)
		{
			return new NetworkShareAccesser(remoteComputerName);
		}
		public static NetworkShareAccesser Access(string remoteComputerName, string domainOrComuterName, string userName, string password)
		{
			return new NetworkShareAccesser(remoteComputerName, domainOrComuterName + "\\" + userName, password);
		}
		public static NetworkShareAccesser Access(string remoteComputerName, string userName, string password)
		{
			return new NetworkShareAccesser(remoteComputerName, userName, password);
		}
		private NetworkShareAccesser(string remoteComputerName)
		{
			this.RemoteComputerName = remoteComputerName;
			this.ConnectToShare(this._remoteUncName, null, null, true);
		}
		private NetworkShareAccesser(string remoteComputerName, string userName, string password)
		{
			this.RemoteComputerName = remoteComputerName;
			this.UserName = userName;
			this.Password = password;
			this.ConnectToShare(this._remoteUncName, this.UserName, this.Password, false);
		}
		private void ConnectToShare(string remoteUnc, string username, string password, bool promptUser)
		{
			NetworkShareAccesser.NETRESOURCE lpNetResource = new NetworkShareAccesser.NETRESOURCE
			{
				dwType = 1,
				lpRemoteName = remoteUnc
			};
			int num;
			if (promptUser)
			{
				num = NetworkShareAccesser.WNetUseConnection(IntPtr.Zero, lpNetResource, "", "", 24, null, null, null);
			}
			else
			{
				num = NetworkShareAccesser.WNetUseConnection(IntPtr.Zero, lpNetResource, password, username, 0, null, null, null);
			}
			if (num != 0)
			{
				this.Result = this.getErrorForNumber(num);
				return;
			}
			this.Result = "";
		}
		private string getErrorForNumber(int errNum)
		{
			NetworkShareAccesser.ErrorClass[] eRROR_LIST = NetworkShareAccesser.ERROR_LIST;
			for (int i = 0; i < eRROR_LIST.Length; i++)
			{
				NetworkShareAccesser.ErrorClass errorClass = eRROR_LIST[i];
				if (errorClass.num == errNum)
				{
					return errorClass.message;
				}
			}
			return "Error: Unknown, " + errNum;
		}
		private void DisconnectFromShare(string remoteUnc)
		{
			int num = NetworkShareAccesser.WNetCancelConnection2(remoteUnc, 1, false);
			this.Result = "";
			if (num != 0)
			{
				this.Result = this.getErrorForNumber(num);
			}
		}
		public void Dispose()
		{
			this.DisconnectFromShare(this._remoteUncName);
		}
		public static bool TcpPortInUse(int port)
		{
			Socket socket = null;
			try
			{
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Bind(localEP);
				socket.Listen(5);
				socket.Close();
				return false;
			}
			catch (SocketException)
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return true;
		}
		public static bool UDPPortInUse(int port)
		{
			Socket socket = null;
			try
			{
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Bind(localEP);
				socket.Close();
				return false;
			}
			catch (SocketException)
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return true;
		}
	}
}
