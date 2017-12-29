using ADODB;
using ADOX;
using CommonAPI;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace DBAccessAPI
{
	public class CompactDBThread
	{
		public const string COMPACTPROGRAM_NAME = "AccessCompact.exe";
		public const string MANAGE_DB = "history.mdb";
		private Hashtable ht_alldbinfo = new Hashtable();
		public static void CheckDBFile()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			FileInfo[] files = directoryInfo.GetFiles();
			if (files.Length != 0)
			{
				FileInfo[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					FileInfo fileInfo = array[i];
					if (fileInfo.Name.IndexOf("temp_") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
					{
						try
						{
							fileInfo.Delete();
						}
						catch
						{
						}
					}
					if (fileInfo.Name.IndexOf("compacted_") == 0 && fileInfo.Extension.ToLower().Equals(".mdb"))
					{
						if (fileInfo.Name.IndexOf("thermaldb") > 0)
						{
							try
							{
								string arg_F1_0 = fileInfo.Name;
								string text2 = fileInfo.DirectoryName;
								if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
								{
									text2 += Path.DirectorySeparatorChar;
								}
								string text3 = text2 + "thermaldb.mdb";
								if (File.Exists(text3))
								{
									int j = 0;
									while (j < 5)
									{
										j++;
										File.Delete(text3);
										fileInfo.MoveTo(text3);
										if (File.Exists(text3))
										{
											break;
										}
										Thread.Sleep(100);
									}
								}
								goto IL_20D;
							}
							catch
							{
								goto IL_20D;
							}
						}
						try
						{
							string name = fileInfo.Name;
							string text4 = fileInfo.DirectoryName;
							if (text4[text4.Length - 1] != Path.DirectorySeparatorChar)
							{
								text4 += Path.DirectorySeparatorChar;
							}
							string str = name.Substring(17, 8);
							string text5 = text4 + "datadb_" + str + ".mdb";
							if (File.Exists(text5))
							{
								int k = 0;
								while (k < 5)
								{
									k++;
									File.Delete(text5);
									fileInfo.MoveTo(text5);
									if (File.Exists(text5))
									{
										break;
									}
									Thread.Sleep(100);
								}
							}
						}
						catch
						{
						}
					}
					IL_20D:;
				}
			}
		}
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
		private OleDbConnection getConnection()
		{
			OleDbConnection result;
			try
			{
				string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
				if (text[text.Length - 1] != Path.DirectorySeparatorChar)
				{
					text += Path.DirectorySeparatorChar;
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				string str = text + "history.mdb";
				OleDbConnection oleDbConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str + ";Jet OLEDB:Database Password=root");
				oleDbConnection.Open();
				result = oleDbConnection;
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
		private int CompactDB(string str_file)
		{
			try
			{
				Process process = new Process();
				string fileName = AppDomain.CurrentDomain.BaseDirectory + "CompactADB.exe";
				string arguments = "\"-src=" + str_file + "\"";
				process.StartInfo = new ProcessStartInfo(fileName, arguments)
				{
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					Verb = "runas"
				};
				process.StartInfo.UseShellExecute = false;
				process.Start();
				process.WaitForExit();
				string text = process.StandardOutput.ReadToEnd();
				int result;
				if (text.IndexOf("successfully") > 0)
				{
					result = 1;
					return result;
				}
				result = -1;
				return result;
			}
			catch
			{
			}
			return -1;
		}
		public void ComPactDBFile()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "datadb";
			if (text[text.Length - 1] != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			if (!File.Exists(text + "history.mdb") && this.createdb() < 0)
			{
				return;
			}
			OleDbConnection oleDbConnection = null;
			OleDbCommand oleDbCommand = null;
			OleDbDataAdapter oleDbDataAdapter = null;
			try
			{
				oleDbConnection = this.getConnection();
				if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
				{
					DataTable dataTable = new DataTable();
					oleDbDataAdapter = new OleDbDataAdapter();
					oleDbCommand = oleDbConnection.CreateCommand();
					oleDbCommand.CommandText = "select dbname,dbsize from compactdb ";
					oleDbDataAdapter.SelectCommand = oleDbCommand;
					oleDbDataAdapter.Fill(dataTable);
					oleDbDataAdapter.Dispose();
					if (dataTable != null)
					{
						this.ht_alldbinfo = new Hashtable();
						foreach (DataRow dataRow in dataTable.Rows)
						{
							string key = Convert.ToString(dataRow[0]);
							long num = Convert.ToInt64(dataRow[1]);
							if (this.ht_alldbinfo.ContainsKey(key))
							{
								this.ht_alldbinfo[key] = num;
							}
							else
							{
								this.ht_alldbinfo.Add(key, num);
							}
						}
					}
				}
			}
			catch (Exception)
			{
				return;
			}
			finally
			{
				if (oleDbDataAdapter != null)
				{
					try
					{
						oleDbDataAdapter.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbCommand != null)
				{
					try
					{
						oleDbCommand.Dispose();
					}
					catch
					{
					}
				}
				if (oleDbConnection != null)
				{
					try
					{
						oleDbConnection.Close();
					}
					catch
					{
					}
				}
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			FileInfo[] files = directoryInfo.GetFiles();
			if (files.Length != 0)
			{
				FileInfo[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					FileInfo fileInfo = array[i];
					if (((fileInfo.Name.IndexOf("datadb_") == 0 && fileInfo.Extension.ToLower().Equals(".mdb")) || fileInfo.Name.Equals("thermaldb.mdb")) && !fileInfo.Name.ToLower().Equals("datadb_" + DateTime.Now.ToString("yyyyMMdd") + ".mdb"))
					{
						try
						{
							string name = fileInfo.Name;
							string fullName = fileInfo.FullName;
							if (this.ht_alldbinfo.ContainsKey(name))
							{
								long num2 = Convert.ToInt64(this.ht_alldbinfo[name]);
								long length = fileInfo.Length;
								if (num2 >= length)
								{
									goto IL_4FE;
								}
							}
							DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Start compact file " + fullName);
							Process process = new Process();
							string fileName = AppDomain.CurrentDomain.BaseDirectory + "CompactADB.exe";
							string arguments = "\"-src=" + fullName + "\"";
							process.StartInfo = new ProcessStartInfo(fileName, arguments)
							{
								CreateNoWindow = true,
								WindowStyle = ProcessWindowStyle.Hidden,
								RedirectStandardOutput = true,
								RedirectStandardError = true,
								Verb = "runas"
							};
							process.StartInfo.UseShellExecute = false;
							process.Start();
							process.WaitForExit();
							string text2 = process.StandardOutput.ReadToEnd();
							if (text2.IndexOf("successfully") > 0)
							{
								DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Success Finish compact file " + fullName);
								try
								{
									oleDbConnection = this.getConnection();
									if (oleDbConnection != null && oleDbConnection.State == ConnectionState.Open)
									{
										oleDbCommand = oleDbConnection.CreateCommand();
										FileInfo fileInfo2 = new FileInfo(fullName);
										long num3 = fileInfo2.Length * 3L;
										if (this.ht_alldbinfo.ContainsKey(name))
										{
											oleDbCommand.CommandText = string.Concat(new object[]
											{
												"update compactdb set dbsize = '",
												num3,
												"' where dbname ='",
												name,
												"' "
											});
										}
										else
										{
											oleDbCommand.CommandText = string.Concat(new object[]
											{
												"insert into compactdb (dbname,dbsize) values ('",
												name,
												"','",
												num3,
												"') "
											});
										}
										oleDbCommand.ExecuteNonQuery();
										if (oleDbCommand != null)
										{
											try
											{
												oleDbCommand.Dispose();
											}
											catch
											{
											}
										}
										if (oleDbConnection != null)
										{
											try
											{
												oleDbConnection.Close();
											}
											catch
											{
											}
										}
									}
									goto IL_4F9;
								}
								catch
								{
									if (oleDbCommand != null)
									{
										try
										{
											oleDbCommand.Dispose();
										}
										catch
										{
										}
									}
									if (oleDbConnection != null)
									{
										try
										{
											oleDbConnection.Close();
										}
										catch
										{
										}
									}
									goto IL_4F9;
								}
							}
							if (fileInfo.Name.Equals("thermaldb.mdb"))
							{
								bool flag = false;
								int j = 0;
								while (j < 4)
								{
									j++;
									int num4 = this.CompactDB(fileInfo.FullName);
									Thread.Sleep(3000);
									if (num4 > 0)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Failed compact file " + fullName + "\r\n" + text2);
								}
							}
							else
							{
								DebugCenter.GetInstance().appendToFile("!!!!!!&&&&&&&!!!! Failed compact file " + fullName + "\r\n" + text2);
							}
							IL_4F9:;
						}
						catch
						{
						}
					}
					IL_4FE:;
				}
			}
		}
	}
}
