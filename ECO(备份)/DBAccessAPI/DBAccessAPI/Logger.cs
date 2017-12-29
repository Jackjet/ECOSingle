using CommonAPI;
using System;
using System.IO;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class Logger
	{
		private const string logFile = ".log";
		private bool open;
		private string path;
		public bool Open
		{
			get
			{
				return this.open;
			}
			set
			{
				this.open = value;
			}
		}
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}
		public Logger() : this(false, null)
		{
		}
		public Logger(bool open, string path)
		{
			this.open = open;
			if (string.IsNullOrEmpty(path))
			{
				path = Environment.CurrentDirectory + "\\log\\";
			}
			if (!path.EndsWith("\\"))
			{
				path += "\\";
			}
			this.path = path;
			this.createDirection();
		}
		private void createDirection()
		{
			try
			{
				if (!Directory.Exists(this.path))
				{
					Directory.CreateDirectory(this.path);
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
		public Logger(bool open) : this(open, null)
		{
		}
		public void Info(string message)
		{
			if (this.open)
			{
				try
				{
					bool flag = false;
					try
					{
						Monitor.Enter(this, ref flag);
						string str = DateTime.Now.ToString("HH:mm:ss");
						File.AppendAllText(this.path + DateTime.Now.ToString("yyyy-MM-dd") + ".log", str + " '" + message + "'\r\n", Encoding.Default);
					}
					finally
					{
						if (flag)
						{
							Monitor.Exit(this);
						}
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}
	}
}
