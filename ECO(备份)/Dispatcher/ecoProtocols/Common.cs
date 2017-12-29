using CommonAPI.Global;
using Dispatcher;
using System;
using System.Collections.Generic;
using System.IO;
namespace ecoProtocols
{
	public static class Common
	{
		public static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		public static string _xmlFile = "Config.xml";
		public static Dictionary<string, string> _cfgFile = null;
		public static string _logFile = "";
		public static int _monitorRole = 1;
		public static string _monitorIP = "127.0.0.1";
		public static int _monitorPort = 7979;
		public static int _viewerPort = 7777;
		public static bool _dataCompress = false;
		public static bool _useSSL = false;
		public static int _maxClients = 2000;
		public static int _tKeepaliveTimer = 10000;
		public static int _tConnectingTimeout = 20000;
		public static int _tMaxRetryTimer = 5000;
		public static object _lockLog = new object();
		public static long ElapsedTime(long tLast)
		{
			long num = (long)Environment.TickCount;
			if (num >= tLast)
			{
				return num - tLast;
			}
			long num2 = 2147483647L - tLast + 1L;
			return num2 + (num - -2147483648L);
		}
		public static void WriteRawData(string filename, bool bCreate, byte[] data, int offset, int count)
		{
		}
		public static void WritePacket(string filename, bool bCreate, byte[] header, byte[] data)
		{
		}
		public static void WriteLine(string format, params string[] list)
		{
			lock (Common._lockLog)
			{
				if (Common._cfgFile == null)
				{
					Common._logFile = "";
					Common._cfgFile = ValuePairs.LoadValueKeyFromXML(Common._xmlFile);
					if (Common._cfgFile.ContainsKey("LogFile"))
					{
						Common._logFile = Common._cfgFile["LogFile"];
					}
				}
				if (Common._logFile != null && !(Common._logFile == ""))
				{
					try
					{
						string str = string.Format(format, list);
						DateTime now = DateTime.Now;
						string str2 = now.ToString("MM-dd HH:mm:ss.fff");
						string text = Common.sCodebase + "\\debuglog\\" + Common._logFile;
						int num = text.LastIndexOf(".");
						if (num >= 0)
						{
							text = text.Substring(0, num);
						}
						if (DispatchAPI.IsServerRole())
						{
							text = text + "(" + now.ToString("yyyy-MM-dd") + ")s.log";
						}
						else
						{
							text = text + "(" + now.ToString("yyyy-MM-dd") + ")c.log";
						}
						using (StreamWriter streamWriter = File.AppendText(text))
						{
							if (DispatchAPI.IsServerRole())
							{
								streamWriter.WriteLine(str2 + " [S] " + str, streamWriter);
							}
							else
							{
								streamWriter.WriteLine(str2 + " [-] " + str, streamWriter);
							}
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
