using ADODB;
using ADOX;
using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace DBAccessAPI
{
	public class ThreadWithParameter
	{
		public const string MANAGE_DB = "history.mdb";
		private string str_dbfile;
		private string str_time;
		private List<string> al_DBdate = new List<string>();
		private int createdb()
		{
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!File.Exists(text + "history.mdb"))
				{
					Catalog catalog = (Catalog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000602-0000-0010-8000-00AA006D2EA4")));
					catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root");
					Connection connection = (Connection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000514-0000-0010-8000-00AA006D2EA4")));
					if (File.Exists(text + "history.mdb"))
					{
						connection.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + text + "history.mdb;Jet OLEDB:Database Password=root", null, null, -1);
						catalog.ActiveConnection = connection;
						Table table = (Table)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000609-0000-0010-8000-00AA006D2EA4")));
						table.ParentCatalog = catalog;
						table.Name = "compactdb";
						Column column = (Column)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0000061B-0000-0010-8000-00AA006D2EA4")));
						column.ParentCatalog = catalog;
						column.Name = "dbname";
						column.Type = DataTypeEnum.adVarWChar;
						table.Columns.Append(column, DataTypeEnum.adVarWChar, 128);
						table.Keys.Append("dbnamePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
						table.Columns.Append("dbsize", DataTypeEnum.adVarWChar, 128);
						catalog.Tables.Append(table);
						Table table2 = (Table)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00000609-0000-0010-8000-00AA006D2EA4")));
						table2.ParentCatalog = catalog;
						table2.Name = "mysqldb";
						Column column2 = (Column)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0000061B-0000-0010-8000-00AA006D2EA4")));
						column2.ParentCatalog = catalog;
						column2.Name = "dbname";
						column2.Type = DataTypeEnum.adVarWChar;
						table2.Columns.Append(column2, DataTypeEnum.adVarWChar, 128);
						table2.Keys.Append("dbnamePrimaryKey", KeyTypeEnum.adKeyPrimary, column2, null, null);
						catalog.Tables.Append(table2);
						connection.Close();
						return 1;
					}
				}
			}
			catch
			{
			}
			return -1;
		}
		public ThreadWithParameter(string pstr_dbfile, string pstr_time)
		{
			this.str_dbfile = pstr_dbfile;
			this.str_time = pstr_time;
		}
		public ThreadWithParameter(List<string> lt_s)
		{
			this.str_dbfile = "";
			this.str_time = "";
			this.al_DBdate = lt_s;
		}
		public void CleanUPDataSingleThread()
		{
			bool flag = true;
			string path = AppDomain.CurrentDomain.BaseDirectory + "datadb.mdb";
			if (!File.Exists(path))
			{
				return;
			}
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!File.Exists(text + "history.mdb") && this.createdb() < 0)
			{
				return;
			}
			Hashtable historyDataStatus = DBTools.GetHistoryDataStatus();
			if (historyDataStatus == null)
			{
				return;
			}
			try
			{
				if (flag)
				{
					if (historyDataStatus.ContainsKey("thermaldb.mdb"))
					{
						DBTools.UpdateHistoryDataStatus("thermaldb.mdb", "-1");
					}
					else
					{
						DBTools.InsertHistoryDataStatus("thermaldb.mdb", "-1");
					}
					int num = DBTools.FilterData(text + "thermaldb.mdb", DateTime.Now.ToString("yyyy-MM-dd"), true);
					if (num >= 0)
					{
						DBTools.UpdateHistoryDataStatus("thermaldb.mdb", "0");
					}
					num = DBTools.CompactAccess(text + "thermaldb.mdb");
					if (num > 0 && File.Exists(text + "thermaldb.mdb"))
					{
						FileInfo fileInfo = new FileInfo(text + "thermaldb.mdb");
						long num2 = fileInfo.Length * 3L;
						DBTools.UpdateHistoryDataStatus("thermaldb.mdb", string.Concat(num2));
					}
				}
			}
			catch
			{
			}
			if (this.al_DBdate != null && this.al_DBdate.Count > 0)
			{
				foreach (string current in this.al_DBdate)
				{
					try
					{
						DateTime dateTime = Convert.ToDateTime(current);
						string text2 = text + "datadb_" + dateTime.ToString("yyyyMMdd") + ".mdb";
						if (File.Exists(text2))
						{
							FileInfo fileInfo2 = new FileInfo(text2);
							if (historyDataStatus.ContainsKey(fileInfo2.Name))
							{
								DBTools.UpdateHistoryDataStatus(fileInfo2.Name, "-1");
							}
							else
							{
								DBTools.InsertHistoryDataStatus(fileInfo2.Name, "-1");
							}
							int num3 = DBTools.FilterData(text2, dateTime.ToString("yyyy-MM-dd"), false);
							if (num3 >= 0)
							{
								DBTools.UpdateHistoryDataStatus(fileInfo2.Name, "0");
							}
							num3 = DBTools.CompactAccess(text2);
							if (num3 > 0 && File.Exists(text2))
							{
								FileInfo fileInfo3 = new FileInfo(text2);
								long num4 = fileInfo3.Length * 3L;
								DBTools.UpdateHistoryDataStatus(fileInfo2.Name, string.Concat(num4));
							}
						}
					}
					catch (Exception ex)
					{
						DebugCenter.GetInstance().appendToFile("Transfer one day data error : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			DBTools.b_up = false;
		}
	}
}
