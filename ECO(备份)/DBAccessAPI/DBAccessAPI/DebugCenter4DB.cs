using System;
using System.IO;
using System.Text;
namespace DBAccessAPI
{
	public class DebugCenter4DB
	{
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
		private static DebugCenter4DB instance = new DebugCenter4DB();
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
		public static DebugCenter4DB GetInstance()
		{
			return DebugCenter4DB.instance;
		}
		private DebugCenter4DB()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text = baseDirectory + "debuglog" + Path.DirectorySeparatorChar;
			string text2 = text + "dberror.log";
			string destFileName = text + "dberror" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			else
			{
				if (File.Exists(text2))
				{
					try
					{
						FileInfo fileInfo = new FileInfo(text2);
						if (fileInfo.Length > 1048576L)
						{
							File.Copy(text2, destFileName);
							File.Delete(text2);
						}
						else
						{
							DateTime.Now.ToString() + " >>> ";
						}
					}
					catch (Exception)
					{
					}
				}
			}
			this.LogPath = text;
		}
		public void appendToFile(object msg)
		{
			if (msg == null)
			{
				return;
			}
			string str = DateTime.Now.ToString() + " >>> ";
			if (!this.isLog)
			{
				return;
			}
			lock (this.lockObj)
			{
				try
				{
					File.AppendAllText(this.log_path + "dberror.log", str + msg.ToString() + "\r\n", Encoding.ASCII);
				}
				catch (Exception)
				{
				}
			}
		}
		public static void writeLine(int logType, string logContent)
		{
			DebugCenter4DB.DebugLeve[logType] + logContent;
		}
	}
}
