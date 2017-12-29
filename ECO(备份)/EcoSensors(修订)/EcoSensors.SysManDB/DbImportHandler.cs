using DBAccessAPI;
using System;
namespace EcoSensors.SysManDB
{
	internal class DbImportHandler
	{
		private string path;
		public DbImportHandler(string path)
		{
			this.path = path;
		}
		public bool HandleEvennt()
		{
			try
			{
				return DBTools.ImportDatabase(this.path);
			}
			catch (System.Exception)
			{
			}
			return false;
		}
	}
}
