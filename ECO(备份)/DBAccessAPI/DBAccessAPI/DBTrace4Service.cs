using System;
using System.IO;
using System.Text;
namespace DBAccessAPI
{
	public class DBTrace4Service
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
		private static DBTrace4Service instance = new DBTrace4Service();
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
		public static DBTrace4Service GetInstance()
		{
			return DBTrace4Service.instance;
		}
		private DBTrace4Service()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string text = baseDirectory + "debuglog" + Path.DirectorySeparatorChar;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			else
			{
				if (File.Exists(text + "DB4Service.log"))
				{
					File.Delete(text + "DB4Service.log");
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
					File.AppendAllText(this.log_path + "DB4Service.log", str + msg.ToString() + "\n", Encoding.ASCII);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
