using System;
using System.IO;
using System.Threading;
namespace CommonAPI
{
	public class SyslogMaintain
	{
		public const int KEEPDAYS = 15;
		public static void DeleteLog()
		{
			try
			{
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
				string[] files = Directory.GetFiles(text2, "*.log", SearchOption.TopDirectoryOnly);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string fileName = array[i];
					FileInfo fileInfo = new FileInfo(fileName);
					DateTime lastWriteTime = fileInfo.LastWriteTime;
					if ((DateTime.Now - lastWriteTime).TotalDays >= 15.0)
					{
						fileInfo.Delete();
					}
				}
			}
			catch (Exception)
			{
			}
		}
		public static void BackupLog()
		{
			try
			{
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
				string text3 = text2 + "service.log";
				if (File.Exists(text3))
				{
					int num = 1;
					while (num > 0 && num < 4)
					{
						try
						{
							FileInfo fileInfo = new FileInfo(text3);
							if (fileInfo.Length > 2097152L)
							{
								string destFileName = text2 + "service" + DateTime.Now.ToString("yyyyMMddHHmm") + ".log";
								File.Move(text3, destFileName);
							}
							num = 0;
						}
						catch (Exception)
						{
							num++;
						}
						Thread.Sleep(100);
					}
				}
			}
			catch (Exception)
			{
			}
			SyslogMaintain.DeleteLog();
		}
	}
}
