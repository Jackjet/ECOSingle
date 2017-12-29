using DBAccessAPI;
using System;
using System.IO;
namespace EcoSensors.SysManDB
{
	internal class DbExportHanler
	{
		private string path;
		public DbExportHanler(string path)
		{
			this.path = path;
		}
		public bool HandleEvennt()
		{
			try
			{
				string text = this.path;
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				if (!System.IO.Directory.Exists(text))
				{
					System.IO.Directory.CreateDirectory(text);
				}
				string str = System.DateTime.Now.ToString("yyyyMMddHHmm");
				text += "db.zip";
				text = text.Insert(text.LastIndexOf("."), "_" + str);
				return DBTools.ExportDatabase(text);
			}
			catch (System.Exception)
			{
			}
			return false;
		}
	}
}
