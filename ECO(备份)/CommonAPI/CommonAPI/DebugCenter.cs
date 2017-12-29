using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Text;
namespace CommonAPI
{
	public class DebugCenter
	{
		public static readonly int ST_Success = 0;
		public static readonly int ST_fatalMask = 32768;
		public static readonly int ST_SevsPortNA = 32769;
		public static readonly int ST_DiskFull = 32770;
		public static readonly int ST_DbUpgrade = 32771;
		public static readonly int ST_SysdbNotExist = 33025;
		public static readonly int ST_LogdbNotExist = 33026;
		public static readonly int ST_DatadbNotExist = 33028;
		public static readonly int ST_SysdbNotMatch = 33281;
		public static readonly int ST_MYSQLCONNECT_ERROR = 36865;
		public static readonly int ST_MYSQLAUTH_ERROR = 36866;
		public static readonly int ST_MYSQLNotExist = 37121;
		public static readonly int ST_MYSQLSIDNotMatch = 37122;
		public static readonly int ST_MYSQLREPAIR_ERROR = 37124;
		public static readonly int ST_ImportDatabase_ERROR = 37377;
		public static readonly int ST_Unknown = 65535;
		public static readonly int ST_TrapPortNA = 1;
		public static readonly int ST_GateWayPortNA = 2;
		public static readonly int ST_BillingPortNA = 4;
		public static readonly int ST_MYSQLCONNECT_LOST = 256;
		public static readonly int DEBUG = 0;
		public static readonly int WARNING = 1;
		public static readonly int ERROR = 2;
		public static readonly string[] DebugLeve = new string[]
		{
			"Debug: ",
			"Warning: ",
			"Error: "
		};
		private string log_path = string.Empty;
		private object lockObj = new object();
		private bool isLog = true;
		private bool isService;
		private bool isEncode = true;
		private static DebugCenter instance = new DebugCenter();
		private static byte[] m_matrix = new byte[]
		{
			32,
			86,
			120,
			85,
			249,
			13,
			168,
			219,
			167,
			173,
			40,
			244,
			7,
			62,
			104,
			124
		};
		private Random mygenerator = new Random();
		private object _lockStatusCode = new object();
		public string LogPath
		{
			get
			{
				return this.log_path;
			}
			set
			{
				this.log_path = value;
				if (!Directory.Exists(this.log_path))
				{
					Directory.CreateDirectory(this.log_path);
				}
			}
		}
		public bool IsService
		{
			get
			{
				return this.isService;
			}
			set
			{
				this.isService = value;
			}
		}
		public bool Enabled
		{
			get
			{
				return this.isLog;
			}
			set
			{
				this.isLog = value;
			}
		}
		public static DebugCenter GetInstance()
		{
			return DebugCenter.instance;
		}
		private DebugCenter()
		{
			SyslogMaintain.DeleteLog();
			SyslogMaintain.BackupLog();
			string text = AppDomain.CurrentDomain.BaseDirectory;
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			string text2 = text + "debuglog" + Path.DirectorySeparatorChar;
			if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
			{
				text2 += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string path = text2 + "service.log";
			try
			{
				File.AppendAllText(path, "\r\n\r\n", Encoding.ASCII);
			}
			catch (Exception)
			{
			}
			this.LogPath = text2;
			if (Directory.Exists(text2 + "logencode_no"))
			{
				this.isEncode = false;
			}
		}
		public void appendToFile(object msg)
		{
			if (msg == null)
			{
				return;
			}
			DateTime now = DateTime.Now;
			string text;
			if (this.isService)
			{
				text = now.ToString(CultureInfo.InvariantCulture) + " <S> ";
			}
			else
			{
				text = now.ToString(CultureInfo.InvariantCulture) + " <C> ";
			}
			if (!this.isLog)
			{
				return;
			}
			lock (this.lockObj)
			{
				try
				{
					string contents = text + msg.ToString() + "\r\n";
					if (this.isEncode)
					{
						File.AppendAllText(this.log_path + "service.log", text, Encoding.UTF8);
						this.Write_byte(msg.ToString());
						File.AppendAllText(this.log_path + "service.log", "\r\n", Encoding.UTF8);
					}
					else
					{
						File.AppendAllText(this.log_path + "service.log", contents, Encoding.UTF8);
					}
				}
				catch (Exception)
				{
				}
			}
		}
		private void write2file(byte[] bytes)
		{
			using (FileStream fileStream = new FileStream(this.log_path + "service.log", FileMode.Append))
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
		}
		private void Write_byte(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			int num = bytes.Length;
			int num2 = 0;
			int srcOffset = 0;
			byte b = 0;
			while (b == 0 || b == 13 || b == 10)
			{
				b = (byte)this.mygenerator.Next();
			}
			byte b2 = b;
			bool flag = false;
			byte[] array;
			for (int i = 0; i < num; i++)
			{
				if (bytes[i] == 13)
				{
					flag = true;
					num2++;
				}
				else
				{
					if (bytes[i] == 10)
					{
						flag = true;
						num2++;
					}
					else
					{
						if (flag)
						{
							array = new byte[num2 + 3];
							array[0] = 255;
							array[1] = 254;
							array[2] = b;
							Buffer.BlockCopy(bytes, srcOffset, array, 3, num2);
							this.write2file(array);
							num2 = 0;
							srcOffset = i;
							while (b == 0 || b == 13 || b == 10)
							{
								b = (byte)this.mygenerator.Next();
							}
							b2 = b;
						}
						flag = false;
						byte[] expr_DA_cp_0 = bytes;
						int expr_DA_cp_1 = i;
						expr_DA_cp_0[expr_DA_cp_1] ^= b2;
						if (bytes[i] == 10 || bytes[i] == 13 || bytes[i] == 0)
						{
							byte[] expr_107_cp_0 = bytes;
							int expr_107_cp_1 = i;
							expr_107_cp_0[expr_107_cp_1] ^= b2;
						}
						b2 = bytes[i];
						b2 ^= DebugCenter.m_matrix[num2 % DebugCenter.m_matrix.Length];
						num2++;
					}
				}
			}
			array = new byte[num2 + 3];
			array[0] = 255;
			array[1] = 254;
			array[2] = b;
			Buffer.BlockCopy(bytes, srcOffset, array, 3, num2);
			this.write2file(array);
		}
		public static void writeLine(int logType, string logContent)
		{
            //DebugCenter.DebugLeve[logType] + logContent;
		}
		public void setLastStatusCode(int statusCode, bool bitwise)
		{
			lock (this._lockStatusCode)
			{
				int num = statusCode;
				try
				{
					string text = "SOFTWARE\\ATEN\\ecoSensors";
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text, true);
					if (registryKey == null)
					{
						registryKey = Registry.LocalMachine.CreateSubKey(text);
						if (registryKey != null)
						{
							registryKey.SetValue("ServiceStatus", num);
							DebugCenter.GetInstance().appendToFile("Failed to read register, create and set the register(set)");
						}
					}
					if (registryKey != null)
					{
						if (registryKey.GetValue("ServiceStatus") != null)
						{
							int num2 = (int)registryKey.GetValue("ServiceStatus");
							if (bitwise)
							{
								num |= num2;
							}
							if (num != num2)
							{
								registryKey.SetValue("ServiceStatus", num);
								DebugCenter.GetInstance().appendToFile("Service Status from 0x" + num2.ToString("X4") + " to 0x" + num.ToString("X4"));
							}
						}
						else
						{
							registryKey.SetValue("ServiceStatus", num);
							DebugCenter.GetInstance().appendToFile("Failed to read ServiceStatus, create and set the register(set)");
						}
						registryKey.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("setLastStatusCode: " + ex.Message);
				}
			}
		}
		public int getLastStatusCode()
		{
			int result;
			lock (this._lockStatusCode)
			{
				int num = 0;
				try
				{
					string text = "SOFTWARE\\ATEN\\ecoSensors";
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text, true);
					if (registryKey == null)
					{
						registryKey = Registry.LocalMachine.CreateSubKey(text);
						if (registryKey != null)
						{
							registryKey.SetValue("ServiceStatus", 0);
							DebugCenter.GetInstance().appendToFile("Failed to read registry, create and set the register(get)");
						}
					}
					if (registryKey != null)
					{
						if (registryKey.GetValue("ServiceStatus") != null)
						{
							num = (int)registryKey.GetValue("ServiceStatus");
							DebugCenter.GetInstance().appendToFile("Service Status: 0x" + num.ToString("X4"));
						}
						else
						{
							registryKey.SetValue("ServiceStatus", 0);
							DebugCenter.GetInstance().appendToFile("Failed to read ServiceStatus, create and set the register(get)");
						}
						registryKey.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("getLastStatusCode: " + ex.Message);
				}
				result = num;
			}
			return result;
		}
		public void clearStatusCode(int statusCode, bool bitwise)
		{
			lock (this._lockStatusCode)
			{
				int num = statusCode;
				try
				{
					string name = "SOFTWARE\\ATEN\\ecoSensors";
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, true);
					if (registryKey != null)
					{
						if (registryKey.GetValue("ServiceStatus") != null)
						{
							int num2 = (int)registryKey.GetValue("ServiceStatus");
							if (bitwise)
							{
								int num3 = ~statusCode;
								num = (num2 & num3);
							}
							if (num != num2)
							{
								registryKey.SetValue("ServiceStatus", num);
								DebugCenter.GetInstance().appendToFile("Service Status from 0x" + num2.ToString("X4") + " to 0x" + num.ToString("X4"));
							}
						}
						registryKey.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("clearStatusCode: " + ex.Message);
				}
			}
		}
		public void FileCheckDelete(string fname)
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string path = baseDirectory + fname;
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception)
			{
			}
		}
		public void FolderCheckDelete(string subDir)
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string path = baseDirectory + subDir;
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
