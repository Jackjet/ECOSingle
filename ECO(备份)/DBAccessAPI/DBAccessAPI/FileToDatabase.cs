using Microsoft.Office.Interop.Access.Dao;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
namespace DBAccessAPI
{
	public class FileToDatabase
	{
		public const int RACK_COL_NAME = 0;
		public const int RACK_COL_SX = 1;
		public const int RACK_COL_SY = 2;
		public const int RACK_COL_EX = 3;
		public const int RACK_COL_EY = 4;
		public const int DEVCFG_COL_MODNM = 0;
		public const int DEVCFG_COL_PORTCOUNT = 1;
		public const int DEVCFG_COL_SENSORCOUNT = 2;
		public const int DEVCFG_COL_BANKCOUNT = 3;
		public const int DEVCFG_COL_SWITCH = 4;
		public const int DEVCFG_COL_PO = 5;
		public const int DEVCFG_COL_MAXC = 6;
		public const int DEVCFG_COL_MINV = 7;
		public const int DEVCFG_COL_MAXV = 8;
		public const int DEVICE_COL_SNMPPORT = 0;
		public const int DEVICE_COL_MAC = 1;
		public const int DEVICE_COL_MODNAME = 2;
		public const int DEVICE_COL_NAME = 3;
		public const int DEVICE_COL_RACKNAME = 4;
		public const int DEVICE_COL_FW = 5;
		public const int DEVICE_COL_SEN_I_TYPE = 6;
		public const int DEVICE_COL_SEN_I_L = 7;
		public const int DEVICE_COL_SEN_II_TYPE = 8;
		public const int DEVICE_COL_SEN_II_L = 9;
		public const int DEVICE_COL_SEN_III_TYPE = 10;
		public const int DEVICE_COL_SEN_III_L = 11;
		public const int DEVICE_COL_SEN_IV_TYPE = 12;
		public const int DEVICE_COL_SEN_IV_L = 13;
		public const int DEVICE_COL_IP = 14;
		public static bool b_deleting = false;
		public static bool b_backuping = false;
		public static bool isBreak = false;
		public static bool b_DAO = false;
		public static ConsoleColor colorBack = Console.BackgroundColor;
		public static ConsoleColor colorFore = Console.ForegroundColor;
		public static void RefreshOutput4Backup()
		{
			string text = "";
			while (FileToDatabase.b_backuping)
			{
				Thread.Sleep(200);
				if (text.Length < 6)
				{
					text += ".";
				}
				else
				{
					text = "";
				}
				Console.Write("\r");
				Console.Write("{0} ", "Backup database " + text);
			}
		}
		public static void RefreshOutput()
		{
			string text = "";
			while (FileToDatabase.b_deleting)
			{
				Thread.Sleep(200);
				if (text.Length < 6)
				{
					text += ".";
				}
				else
				{
					text = "";
				}
				Console.Write("\r");
				Console.Write("{0} ", "Deleting old data " + text);
			}
		}
		public static void CleanDatabase(string str_dbpath)
		{
			if (DBUrl.SERVERMODE)
			{
				DBConn dBConn = null;
				DbCommand dbCommand = null;
				try
				{
					try
					{
						dBConn = DBConnPool.getConnection();
						if (dBConn.con != null)
						{
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "TRUNCATE TABLE  backuptask ";
							dbCommand.ExecuteNonQuery();
							dbCommand = dBConn.con.CreateCommand();
							dbCommand.CommandText = "TRUNCATE TABLE  bank_auto_info ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE bank_data_daily  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE bank_data_hourly  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE bank_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "delete from data_group where grouptype <> 'allrack' and grouptype <> 'alldev' and grouptype <> 'alloutlet'  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_addr_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_auto_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_base_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_data_daily  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_data_hourly  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_sensor_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE device_voltage  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE group_detail  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE groupcontroltask  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE port_auto_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE port_data_daily  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE port_data_hourly  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE port_info  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE rack_effect  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE rackthermal_daily  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE rackthermal_hourly  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE rci_daily  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE rci_hourly  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE taskschedule  ";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "TRUNCATE TABLE zone_info  ";
							dbCommand.ExecuteNonQuery();
							FileToDatabase.b_deleting = false;
						}
					}
					catch
					{
					}
					return;
				}
				finally
				{
					if (dbCommand != null)
					{
						dbCommand.Dispose();
					}
					if (dBConn != null)
					{
						dBConn.Close();
					}
				}
			}
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			DBConn dBConn2 = null;
			DBConn dBConn3 = null;
			DbCommand dbCommand2 = new OleDbCommand();
			DbCommand dbCommand3 = new OleDbCommand();
			try
			{
				dBConn2 = DBConnPool.getConnection();
				if (dBConn2.con != null)
				{
					FileToDatabase.b_deleting = true;
					Thread thread = new Thread(new ThreadStart(FileToDatabase.RefreshOutput));
					thread.Start();
					dbCommand2 = DBConn.GetCommandObject(dBConn2.con);
					dbCommand2.CommandType = CommandType.Text;
					dbCommand2.CommandText = "delete from group_detail ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from zone_info ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from device_addr_info ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from data_group where grouptype not in ('allrack','alldev','alloutlet')";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from bank_info ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from device_sensor_info ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from port_info ";
					dbCommand2.ExecuteNonQuery();
					dbCommand2.CommandText = "delete from device_base_info ";
					dbCommand2.ExecuteNonQuery();
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
					{
						dBConn3 = DBConnPool.getDynaConnection();
						if (dBConn3.con != null)
						{
							dbCommand3 = DBConn.GetCommandObject(dBConn3.con);
							dbCommand3.CommandText = "TRUNCATE TABLE bank_auto_info ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE bank_data_daily ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE port_auto_info ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE port_data_daily ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE device_auto_info ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE device_data_daily ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE device_data_hourly ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE port_data_hourly ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE bank_data_hourly ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE rackthermal_hourly ";
							dbCommand3.ExecuteNonQuery();
							dbCommand3.CommandText = "TRUNCATE TABLE rackthermal_daily ";
							dbCommand3.ExecuteNonQuery();
						}
					}
					else
					{
						try
						{
							FileInfo fileInfo = new FileInfo(str_dbpath);
							string text = fileInfo.DirectoryName + Path.DirectorySeparatorChar + "datadb.mdb";
							if (File.Exists(text))
							{
								FileInfo fileInfo2 = new FileInfo(text);
								string text2 = fileInfo2.DirectoryName;
								if (text2[text2.Length - 1] != Path.DirectorySeparatorChar)
								{
									text2 += Path.DirectorySeparatorChar;
								}
								string text3 = text2 + "datadb.mdb";
								string sourceFileName = text2 + "datadb.org";
								FileInfo fileInfo3 = new FileInfo(text3);
								fileInfo3.Delete();
								File.Copy(sourceFileName, text3, true);
							}
						}
						catch (Exception)
						{
						}
					}
				}
				FileToDatabase.b_deleting = false;
			}
			catch (Exception)
			{
			}
			finally
			{
				FileToDatabase.b_deleting = false;
				if (dBConn2 != null)
				{
					dBConn2.close();
				}
				dbCommand2.Dispose();
				if (dBConn3 != null)
				{
					dBConn3.close();
				}
				dbCommand3.Dispose();
			}
			FileToDatabase.b_deleting = false;
		}
		public static DataTable ReadFileToTable(string str_file, bool b_head)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			DataTable dataTable = new DataTable();
			try
			{
				string[] array = File.ReadAllLines(str_file, Encoding.UTF8);
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (!string.IsNullOrEmpty(text) && text.Trim().Length != 0 && text.IndexOf(",") != -1)
					{
						if (i == num)
						{
							string[] array2 = text.Split(new string[]
							{
								","
							}, StringSplitOptions.RemoveEmptyEntries);
							if (b_head)
							{
								for (int j = 0; j < array2.Length; j++)
								{
									dataTable.Columns.Add(array2[j]);
								}
								goto IL_120;
							}
							for (int k = 0; k < array2.Length; k++)
							{
								dataTable.Columns.Add();
							}
						}
						string[] array3 = text.Split(new string[]
						{
							","
						}, StringSplitOptions.RemoveEmptyEntries);
						DataRow dataRow = dataTable.NewRow();
						for (int l = 0; l < array3.Length; l++)
						{
							dataRow[l] = array3[l];
						}
						dataTable.Rows.Add(dataRow);
					}
					IL_120:;
				}
			}
			catch (Exception)
			{
			}
			return dataTable;
		}
		public static DataTable ReadFileToTable(string str_file)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			DataTable dataTable = new DataTable();
			try
			{
				string[] array = File.ReadAllLines(str_file, Encoding.UTF8);
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (!string.IsNullOrEmpty(text) && text.Trim().Length != 0 && text.IndexOf(",") != -1)
					{
						if (i == num)
						{
							string[] array2 = text.Split(new string[]
							{
								","
							}, StringSplitOptions.RemoveEmptyEntries);
							for (int j = 0; j < array2.Length; j++)
							{
								dataTable.Columns.Add(array2[j]);
							}
						}
						else
						{
							string[] array3 = text.Split(new string[]
							{
								","
							}, StringSplitOptions.RemoveEmptyEntries);
							DataRow dataRow = dataTable.NewRow();
							for (int k = 0; k < array3.Length; k++)
							{
								dataRow[k] = array3[k];
							}
							dataTable.Rows.Add(dataRow);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return dataTable;
		}
		public static DataTable ReadFileToTable(string str_filepath, string FilsPath)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			OleDbConnection oleDbConnection = new OleDbConnection();
			OleDbCommand oleDbCommand = new OleDbCommand();
			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			DataTable dataTable = new DataTable();
			oleDbConnection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + str_filepath + ";Extended Properties='Text;FMT=Delimited;HDR=YES;'";
			oleDbConnection.Open();
			oleDbCommand.Connection = oleDbConnection;
			oleDbCommand.CommandText = "select * From [" + FilsPath + "]";
			oleDbDataAdapter.SelectCommand = oleDbCommand;
			try
			{
				oleDbDataAdapter.Fill(dataTable);
			}
			catch (Exception)
			{
			}
			finally
			{
				oleDbConnection.Close();
				oleDbCommand.Dispose();
				oleDbDataAdapter.Dispose();
				oleDbConnection.Dispose();
			}
			return dataTable;
		}
		public static int ImportRackInfo(DataTable dt_rackfile, ref Dictionary<string, long> RackIDNameMap)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			int num = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					foreach (DataRow dataRow in dt_rackfile.Rows)
					{
						string text = Convert.ToString(dataRow[0]);
						if (!list2.Contains(text))
						{
							string text2 = Convert.ToString(dataRow[1]);
							string text3 = Convert.ToString(dataRow[2]);
							string text4 = Convert.ToString(dataRow[3]);
							string text5 = Convert.ToString(dataRow[4]);
							if (!list.Contains(string.Concat(new string[]
							{
								"(",
								text2,
								",",
								text3,
								")"
							})) && !list.Contains(string.Concat(new string[]
							{
								"(",
								text4,
								",",
								text5,
								")"
							})))
							{
								string commandText = string.Concat(new string[]
								{
									"insert into device_addr_info (rack_nm,sx,sy,ex,ey,reserve ) values('",
									text,
									"',",
									text2,
									",",
									text3,
									",",
									text4,
									",",
									text5,
									",'' )"
								});
								dbCommand.CommandText = commandText;
								num = dbCommand.ExecuteNonQuery();
								dbCommand.Parameters.Clear();
								if (DBUrl.SERVERMODE)
								{
									dbCommand.CommandText = "SELECT LAST_INSERT_ID()";
								}
								else
								{
									dbCommand.CommandText = "SELECT @@IDENTITY";
								}
								long value = Convert.ToInt64(dbCommand.ExecuteScalar());
								if (num > 0)
								{
									list2.Add(text);
									list.Add(string.Concat(new string[]
									{
										"(",
										text2,
										",",
										text3,
										")"
									}));
									list.Add(string.Concat(new string[]
									{
										"(",
										text4,
										",",
										text5,
										")"
									}));
									RackIDNameMap.Add(text, value);
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return num;
		}
		public static int ImportDeviceInfo(DataTable dt_simu, DataTable dt_device, Dictionary<string, long> RackIDNameMap, string str_device_ip, int i_dc)
		{
			try
			{
				Sys_Para.SetResolution(i_dc);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error : " + ex.Message + "\n" + ex.StackTrace);
			}
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			int num = 1;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary7 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary8 = new Dictionary<string, string>();
			foreach (DataRow dataRow in dt_device.Rows)
			{
				try
				{
					string key = Convert.ToString(dataRow[0]);
					string value = Convert.ToString(dataRow[1]);
					string value2 = Convert.ToString(dataRow[2]);
					string value3 = Convert.ToString(dataRow[3]);
					string value4 = Convert.ToString(dataRow[4]);
					string value5 = Convert.ToString(dataRow[6]);
					string value6 = Convert.ToString(dataRow[7]);
					string value7 = Convert.ToString(dataRow[8]);
					dictionary2.Add(key, value);
					dictionary3.Add(key, value2);
					dictionary4.Add(key, value3);
					dictionary5.Add(key, value4);
					dictionary6.Add(key, value5);
					dictionary7.Add(key, value7);
					dictionary8.Add(key, value6);
				}
				catch (Exception)
				{
				}
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			try
			{
				int num2 = 1;
				foreach (DataRow dataRow2 in dt_simu.Rows)
				{
					Console.SetCursorPosition(0, 22);
					int num3 = num2 * 100 / dt_simu.Rows.Count;
					if (num3 < 1)
					{
						num3 = 1;
					}
					Console.Write(" {0}% ", num3);
					num2++;
					int num4 = 0;
					try
					{
						num4 = Convert.ToInt32(dataRow2[0]);
					}
					catch (Exception)
					{
					}
					if (num4 != 0)
					{
						string text = Convert.ToString(dataRow2[3]);
						if (list2.Contains(text))
						{
							dictionary.Add(text, "Name repeat");
						}
						else
						{
							string text2 = Convert.ToString(dataRow2[1]);
							if (list.Contains(text2))
							{
								dictionary.Add(text, "Mac repeat");
							}
							else
							{
								string text3 = Convert.ToString(dataRow2[2]);
								string key2 = Convert.ToString(dataRow2[4]);
								long num5 = 0L;
								if (RackIDNameMap.TryGetValue(key2, out num5))
								{
									if (num5 == 0L)
									{
										dictionary.Add(text, "Rack name is not correct");
									}
									else
									{
										int num6 = 0;
										int num7 = 0;
										string s = "";
										if (dictionary2.TryGetValue(text3, out s))
										{
											int num8;
											try
											{
												num8 = int.Parse(s);
											}
											catch (Exception)
											{
												continue;
											}
											string s2 = "";
											if (dictionary3.TryGetValue(text3, out s2))
											{
												try
												{
													num6 = int.Parse(s2);
												}
												catch (Exception)
												{
													continue;
												}
												string s3 = "";
												if (dictionary4.TryGetValue(text3, out s3))
												{
													try
													{
														num7 = int.Parse(s3);
													}
													catch (Exception)
													{
														continue;
													}
													string s4 = "";
													if (dictionary5.TryGetValue(text3, out s4))
													{
														try
														{
															int.Parse(s4);
														}
														catch (Exception)
														{
															continue;
														}
														string s5 = "";
														if (dictionary6.TryGetValue(text3, out s5))
														{
															float f_max_c;
															try
															{
																f_max_c = float.Parse(s5);
															}
															catch (Exception)
															{
																continue;
															}
															string s6 = "";
															if (dictionary7.TryGetValue(text3, out s6))
															{
																float f_max_v;
																try
																{
																	f_max_v = float.Parse(s6);
																}
																catch (Exception)
																{
																	continue;
																}
																string s7 = "";
																if (dictionary8.TryGetValue(text3, out s7))
																{
																	float f_min_v;
																	try
																	{
																		f_min_v = float.Parse(s7);
																	}
																	catch (Exception)
																	{
																		continue;
																	}
																	string str_ip = Convert.ToString(dataRow2[14]);
																	string str_version = Convert.ToString(dataRow2[5]);
																	Convert.ToString(dataRow2[6]);
																	string text4 = Convert.ToString(dataRow2[7]);
																	Convert.ToString(dataRow2[8]);
																	string text5 = Convert.ToString(dataRow2[9]);
																	Convert.ToString(dataRow2[10]);
																	string text6 = Convert.ToString(dataRow2[11]);
																	Convert.ToString(dataRow2[12]);
																	string text7 = Convert.ToString(dataRow2[13]);
																	List<PortInfo> list3 = new List<PortInfo>();
																	List<SensorInfo> list4 = new List<SensorInfo>();
																	List<BankInfo> list5 = new List<BankInfo>();
																	for (int i = 0; i < num8; i++)
																	{
																		PortInfo item = new PortInfo(-1, -1, i + 1, "Outlet_" + (i + 1), f_max_v, f_min_v, 99999f, -300f, 9999f, -300f, f_max_c, -300f, 1, 0, 0, 1, "000000000000");
																		list3.Add(item);
																	}
																	for (int j = 0; j < num6; j++)
																	{
																		int num9 = -1;
																		switch (j)
																		{
																		case 0:
																			if (text4.ToUpper().Equals("INTAKE"))
																			{
																				num9 = 0;
																			}
																			if (text4.ToUpper().Equals("EXHAUST"))
																			{
																				num9 = 1;
																			}
																			if (text4.ToUpper().Equals("FLOOR"))
																			{
																				num9 = 2;
																			}
																			break;
																		case 1:
																			if (text5.ToUpper().Equals("INTAKE"))
																			{
																				num9 = 0;
																			}
																			if (text5.ToUpper().Equals("EXHAUST"))
																			{
																				num9 = 1;
																			}
																			if (text5.ToUpper().Equals("FLOOR"))
																			{
																				num9 = 2;
																			}
																			break;
																		case 2:
																			if (text6.ToUpper().Equals("INTAKE"))
																			{
																				num9 = 0;
																			}
																			if (text6.ToUpper().Equals("EXHAUST"))
																			{
																				num9 = 1;
																			}
																			if (text6.ToUpper().Equals("FLOOR"))
																			{
																				num9 = 2;
																			}
																			break;
																		case 3:
																			if (text7.ToUpper().Equals("INTAKE"))
																			{
																				num9 = 0;
																			}
																			if (text7.ToUpper().Equals("EXHAUST"))
																			{
																				num9 = 1;
																			}
																			if (text7.ToUpper().Equals("FLOOR"))
																			{
																				num9 = 2;
																			}
																			break;
																		}
																		SensorInfo item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), num9, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																		if (num9 == -1)
																		{
																			if (num6 <= 2)
																			{
																				if (j == 0)
																				{
																					item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 0, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																				}
																				else
																				{
																					item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 1, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																				}
																			}
																			else
																			{
																				if (num6 == 4)
																				{
																					if (j == 0)
																					{
																						item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 0, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																					}
																					else
																					{
																						if (j == 1)
																						{
																							item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 0, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																						}
																						else
																						{
																							if (j == 2)
																							{
																								item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 1, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																							}
																							else
																							{
																								if (j == 3)
																								{
																									item2 = new SensorInfo(-1, -1, "Sensor_" + (j + 1), 2, j + 1, 94.9f, 15.1f, 59.9f, -19.9f, 249.9f, -249.9f);
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																		list4.Add(item2);
																	}
																	for (int k = 0; k < num7; k++)
																	{
																		BankInfo item3 = new BankInfo(-1, -1, 110, string.Concat(k + 1), "Bank_" + (k + 1), f_max_v, f_min_v, 99999f, -300f, 9999.9f, -300f, 10f, -300f);
																		list5.Add(item3);
																	}
																	if (str_device_ip.Length > 0)
																	{
																		str_ip = str_device_ip;
																	}
																	DeviceInfo dev_info;
																	if (text3.IndexOf("EC2004") > -1)
																	{
																		dev_info = new DeviceInfo(-1, str_ip, text, text2, "", "", "AES", "MD5", 100, 1, "administrator", num4, 1, text3, -500f, -500f, 99998f, -300f, 9998f, -300f, f_max_c, -300f, num5, str_version, 1, -1f, 0, 0f, 0, 0, 0, "0", -300f);
																	}
																	else
																	{
																		dev_info = new DeviceInfo(-1, str_ip, text, text2, "", "", "AES", "MD5", 100, 1, "administrator", num4, 1, text3, f_max_v, f_min_v, 99998f, -300f, 9998f, -300f, f_max_c, -300f, num5, str_version, 1, -1f, 0, 0f, 0, 0, 0, "0", -300f);
																	}
																	int num10 = DeviceOperation.AddNewDevice(dev_info, list3, list4, list5, null);
																	if (num10 < 1)
																	{
																		num--;
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
								else
								{
									dictionary.Add(text, "Rack name is not correct");
								}
							}
						}
					}
				}
				Console.WriteLine("");
			}
			catch (Exception)
			{
				num = -100000;
			}
			return num;
		}
		public static int BackupDatabase(string str_file)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			int result = -1;
			try
			{
				FileToDatabase.b_backuping = true;
				Thread thread = new Thread(new ThreadStart(FileToDatabase.RefreshOutput4Backup));
				thread.Start();
				string str = DateTime.Now.ToString("yyyyMMddHHmmss");
				"Databasebackup" + str + "." + "zip";
				new FileInfo(str_file);
				FileToDatabase.b_backuping = false;
				return 1;
			}
			catch (Exception)
			{
			}
			FileToDatabase.b_backuping = false;
			return result;
		}
		private static string GetPropertyValue(DataTable dt_source, int i_index, string str_search)
		{
			try
			{
				DataRow[] array = dt_source.Select(str_search);
				if (array != null && array.Length > 0)
				{
					return Convert.ToString(array[0][i_index]);
				}
			}
			catch (Exception)
			{
			}
			return null;
		}
		public static string CreateSerialNumber(int strLength)
		{
			if (strLength < 1)
			{
				strLength = 4;
			}
			string text = string.Empty;
			string arg_12_0 = string.Empty;
			string text2 = Guid.NewGuid().ToString().Replace("-", "");
			int length = text2.Length;
			Random random = new Random(int.Parse(DateTime.Now.ToString("MMddHHmmsss")));
			for (int i = 0; i < strLength; i++)
			{
				int startIndex = random.Next(0, length - 1);
				text += text2.Substring(startIndex, 1);
			}
			return text.ToUpper();
		}
		public static long SimulatePower(int i_day, DbCommand command, long l_count)
		{
			string name = AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb";
			try
			{
				DBEngine dBEngine = (DBEngine)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("CD7791B9-43FD-42C5-AE42-8DD2811F0419")));
				Database database = dBEngine.OpenDatabase(name, false, false, "MS Access;PWD=^tenec0Sensor");
				database.Close();
				FileToDatabase.b_DAO = true;
			}
			catch (Exception)
			{
				FileToDatabase.b_DAO = false;
			}
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			DataTable dataTable = new DataTable();
			DataTable dt_port = new DataTable();
			DataTable dt_bank = new DataTable();
			DataTable dt_pdsource = new DataTable();
			DataTable dt_dbsource = new DataTable();
			DBConn connection = DBConnPool.getConnection();
			OleDbCommand oleDbCommand = new OleDbCommand();
			oleDbCommand.Connection = (OleDbConnection)connection.con;
			command.CommandText = "select * from datasource ";
			DbDataReader dbDataReader = command.ExecuteReader();
			try
			{
				if (dbDataReader.HasRows)
				{
					dt_dbsource = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader.Close();
				dbDataReader.Dispose();
			}
			command.CommandText = "select * from pdsource ";
			DbDataReader dbDataReader2 = command.ExecuteReader();
			try
			{
				if (dbDataReader2.HasRows)
				{
					dt_pdsource = DBConn.ConvertOleDbReaderToDataTable(dbDataReader2);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader2.Close();
				dbDataReader2.Dispose();
			}
			oleDbCommand.CommandText = "select * from device_base_info ";
			DbDataReader dbDataReader3 = oleDbCommand.ExecuteReader();
			try
			{
				if (dbDataReader3.HasRows)
				{
					dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader3);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader3.Close();
				dbDataReader3.Dispose();
			}
			oleDbCommand.CommandText = "select * from port_info ";
			DbDataReader dbDataReader4 = oleDbCommand.ExecuteReader();
			try
			{
				if (dbDataReader4.HasRows)
				{
					dt_port = DBConn.ConvertOleDbReaderToDataTable(dbDataReader4);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader4.Close();
				dbDataReader4.Dispose();
			}
			oleDbCommand.CommandText = "select * from bank_info ";
			DbDataReader dbDataReader5 = oleDbCommand.ExecuteReader();
			try
			{
				if (dbDataReader5.HasRows)
				{
					dt_bank = DBConn.ConvertOleDbReaderToDataTable(dbDataReader5);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader5.Close();
				dbDataReader5.Dispose();
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
			dictionary2.Add("PE5324B", 2);
			dictionary2.Add("PE5324G", 2);
			dictionary2.Add("PE5324J", 2);
			dictionary2.Add("PE5324kJA", 2);
			dictionary2.Add("PE5340sB", 2);
			dictionary2.Add("PE5340sG", 2);
			dictionary2.Add("PE5340sJ", 2);
			dictionary2.Add("PE6324B", 2);
			dictionary2.Add("PE6324G", 2);
			dictionary2.Add("PE6324J", 2);
			dictionary2.Add("PE7324kJA", 2);
			dictionary2.Add("PE7324B", 2);
			dictionary2.Add("PE7324G", 2);
			dictionary2.Add("PE7324J", 2);
			dictionary2.Add("PE7328B", 2);
			dictionary2.Add("PE7328J", 2);
			dictionary2.Add("PE7328G", 2);
			dictionary2.Add("PE8316A", 2);
			dictionary2.Add("PE8316B", 2);
			dictionary2.Add("PE8316G", 2);
			dictionary2.Add("PE8316J", 2);
			dictionary2.Add("PE8324A", 2);
			dictionary2.Add("PE8324JA", 2);
			dictionary2.Add("PE8324B", 2);
			dictionary2.Add("PE8324G", 2);
			dictionary2.Add("PE8324J", 2);
			dictionary2.Add("PE9324B", 2);
			dictionary2.Add("PE9324G", 2);
			dictionary2.Add("PE9324J", 2);
			dictionary2.Add("PE9330B", 2);
			dictionary2.Add("PE9330J", 2);
			dictionary2.Add("PE9330G", 2);
			dictionary2.Add("PE7324rB", 2);
			dictionary2.Add("PE7324rG", 2);
			dictionary2.Add("PE7324rJ", 2);
			dictionary2.Add("PE8324rB", 2);
			dictionary2.Add("PE8324rG", 2);
			dictionary2.Add("PE8324rJ", 2);
			dictionary2.Add("PE9324rB", 2);
			dictionary2.Add("PE9324rG", 2);
			dictionary2.Add("PE9324rJ", 2);
			dictionary2.Add("EC1000", 2);
			dictionary2.Add("EC2004", 2);
			dictionary3.Add("PE3108A", 24);
			dictionary3.Add("PE3108B", 24);
			dictionary3.Add("PE3108G", 24);
			dictionary3.Add("PE3208A", 24);
			dictionary3.Add("PE3208B", 24);
			dictionary3.Add("PE3208G", 24);
			dictionary3.Add("PE8108A", 24);
			dictionary3.Add("PE8108B", 24);
			dictionary3.Add("PE8108G", 24);
			dictionary3.Add("PE8208A", 24);
			dictionary3.Add("PE8208B", 24);
			dictionary3.Add("PE8208BSKX", 24);
			dictionary3.Add("PE8208G", 24);
			dictionary3.Add("PE7214B", 24);
			dictionary3.Add("PE7214J", 24);
			dictionary3.Add("PE7214G", 24);
			dictionary3.Add("PE7216B", 24);
			dictionary3.Add("PE7216G", 24);
			dictionary3.Add("PE7324kJA", 24);
			dictionary3.Add("PE7324B", 24);
			dictionary3.Add("PE7324G", 24);
			dictionary3.Add("PE7324J", 24);
			dictionary3.Add("PE7328B", 24);
			dictionary3.Add("PE7328J", 24);
			dictionary3.Add("PE7328G", 24);
			dictionary3.Add("PE8121kJ", 24);
			dictionary3.Add("PE8216B", 24);
			dictionary3.Add("PE8216G", 24);
			dictionary3.Add("PE8316A", 24);
			dictionary3.Add("PE8316B", 24);
			dictionary3.Add("PE8316G", 24);
			dictionary3.Add("PE8316J", 24);
			dictionary3.Add("PE8324A", 24);
			dictionary3.Add("PE8324JA", 24);
			dictionary3.Add("PE8324B", 24);
			dictionary3.Add("PE8324G", 24);
			dictionary3.Add("PE8324J", 24);
			dictionary3.Add("PE9216B", 24);
			dictionary3.Add("PE9216G", 24);
			dictionary3.Add("PE9222B", 24);
			dictionary3.Add("PE9222J", 24);
			dictionary3.Add("PE9222G", 24);
			dictionary3.Add("PE9324B", 24);
			dictionary3.Add("PE9324G", 24);
			dictionary3.Add("PE9324J", 24);
			dictionary3.Add("PE9330B", 24);
			dictionary3.Add("PE9330J", 24);
			dictionary3.Add("PE9330G", 24);
			dictionary3.Add("PE7216rB", 24);
			dictionary3.Add("PE7216rG", 24);
			dictionary3.Add("PE7324rB", 24);
			dictionary3.Add("PE7324rG", 24);
			dictionary3.Add("PE7324rJ", 24);
			dictionary3.Add("PE8216rB", 24);
			dictionary3.Add("PE8216rG", 24);
			dictionary3.Add("PE8324rB", 24);
			dictionary3.Add("PE8324rG", 24);
			dictionary3.Add("PE8324rJ", 24);
			dictionary3.Add("PE9216rB", 24);
			dictionary3.Add("PE9216rG", 24);
			dictionary3.Add("PE9324rB", 24);
			dictionary3.Add("PE9324rG", 24);
			dictionary3.Add("PE9324rJ", 24);
			foreach (DataRow dataRow in dataTable.Rows)
			{
				new Random((int)DateTime.Now.Ticks);
				int num = Convert.ToInt32(dataRow[0]);
				string text = Convert.ToString(dataRow[13]);
				try
				{
					if (dictionary.ContainsKey(text))
					{
						int sampleid = 0;
						if (dictionary.TryGetValue(text, out sampleid))
						{
							l_count = FileToDatabase.InsertSameModelPower4port(command, i_day, num, sampleid, dt_port, l_count);
						}
					}
					else
					{
						if (dictionary3.ContainsKey(text))
						{
							l_count = FileToDatabase.InsertCellPower4port(command, i_day, num, text, l_count, dt_port, dt_dbsource);
						}
						if (dictionary2.ContainsKey(text))
						{
							l_count = FileToDatabase.InsertCellPower4bank(command, i_day, num, text, l_count, dt_bank, dt_dbsource);
						}
						if (!dictionary3.ContainsKey(text) && !dictionary2.ContainsKey(text))
						{
							l_count = FileToDatabase.InsertCellPower(command, i_day, num, text, l_count, dt_dbsource);
						}
						dictionary.Add(text, num);
					}
					if (dictionary3.ContainsKey(text))
					{
						l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, num, text, l_count, dt_port, dt_pdsource);
					}
					if (dictionary2.ContainsKey(text))
					{
						l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, num, text, l_count, dt_bank, dt_pdsource);
					}
					if (!dictionary3.ContainsKey(text) && !dictionary2.ContainsKey(text))
					{
						l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, num, text, l_count, dt_pdsource);
					}
				}
				catch (Exception ex)
				{
					string arg_8F0_0 = ex.Message;
				}
			}
			command.Dispose();
			oleDbCommand.Dispose();
			connection.con.Close();
			return l_count;
		}
		public static long SimulatePDNew(int i_day, DbCommand command, long l_cont)
		{
			DataTable dataTable = new DataTable();
			DataTable dt_port = new DataTable();
			DataTable dt_bank = new DataTable();
			DataTable dt_pdsource = new DataTable();
			command.CommandText = "select * from pdsource ";
			DbDataReader dbDataReader = command.ExecuteReader();
			try
			{
				if (dbDataReader.HasRows)
				{
					dt_pdsource = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader.Close();
				dbDataReader.Dispose();
			}
			command.CommandText = "select * from device_base_info ";
			DbDataReader dbDataReader2 = command.ExecuteReader();
			try
			{
				if (dbDataReader2.HasRows)
				{
					dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader2);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader2.Close();
				dbDataReader2.Dispose();
			}
			command.CommandText = "select * from port_info ";
			DbDataReader dbDataReader3 = command.ExecuteReader();
			try
			{
				if (dbDataReader3.HasRows)
				{
					dt_port = DBConn.ConvertOleDbReaderToDataTable(dbDataReader3);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader3.Close();
				dbDataReader3.Dispose();
			}
			command.CommandText = "select * from bank_info ";
			DbDataReader dbDataReader4 = command.ExecuteReader();
			try
			{
				if (dbDataReader4.HasRows)
				{
					dt_bank = DBConn.ConvertOleDbReaderToDataTable(dbDataReader4);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader4.Close();
				dbDataReader4.Dispose();
			}
			long l_count = 0L;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				int i_deviceid = Convert.ToInt32(dataRow[0]);
				string text = Convert.ToString(dataRow[13]);
				string key;
				switch (key = text)
				{
				case "EC1000":
					l_count = FileToDatabase.InsertDeviceAndBankAutoInfo(command, i_day, i_deviceid, text, l_count, dt_bank);
					l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, i_deviceid, text, l_count, dt_bank, dt_pdsource);
					break;
				case "PE1108A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE1108B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE1108G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE1208A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE1208B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE1208G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE3108A":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE3108B":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE3108G":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE3208A":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE3208B":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE3208G":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE5216A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE5216B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE5216G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE5324B":
					l_count = FileToDatabase.InsertDeviceAndBankAutoInfo(command, i_day, i_deviceid, text, l_count, dt_bank);
					l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, i_deviceid, text, l_count, dt_bank, dt_pdsource);
					break;
				case "PE5324G":
					l_count = FileToDatabase.InsertDeviceAndBankAutoInfo(command, i_day, i_deviceid, text, l_count, dt_bank);
					l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, i_deviceid, text, l_count, dt_bank, dt_pdsource);
					break;
				case "PE6108A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6108B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6108G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6208A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6208B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6208G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6216A":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6216B":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6216G":
					l_count = FileToDatabase.InsertDeviceAutoInfo(command, i_day, i_deviceid, text, l_count);
					l_count = FileToDatabase.InsertDeviceDataDaily(command, i_day, i_deviceid, text, l_count, dt_pdsource);
					break;
				case "PE6324B":
					l_count = FileToDatabase.InsertDeviceAndBankAutoInfo(command, i_day, i_deviceid, text, l_count, dt_bank);
					l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, i_deviceid, text, l_count, dt_bank, dt_pdsource);
					break;
				case "PE6324G":
					l_count = FileToDatabase.InsertDeviceAndBankAutoInfo(command, i_day, i_deviceid, text, l_count, dt_bank);
					l_count = FileToDatabase.InsertDeviceAndBankDataDaily(command, i_day, i_deviceid, text, l_count, dt_bank, dt_pdsource);
					break;
				case "PE8108A":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE8108B":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE8108G":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE8208A":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE8208B":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				case "PE8208G":
					l_count = FileToDatabase.InsertDeviceAndPortAutoInfo(command, i_day, i_deviceid, text, l_count, dt_port);
					l_count = FileToDatabase.InsertDeviceAndPortDataDaily(command, i_day, i_deviceid, text, l_count, dt_port, dt_pdsource);
					break;
				}
			}
			return l_cont;
		}
		public static int SimulatePD(int i_day, DbCommand command)
		{
			DataTable dataTable = new DataTable();
			DataTable dataTable2 = new DataTable();
			DataTable dataTable3 = new DataTable();
			command.CommandText = "select * from device_base_info ";
			DbDataReader dbDataReader = command.ExecuteReader();
			try
			{
				if (dbDataReader.HasRows)
				{
					dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader.Close();
				dbDataReader.Dispose();
			}
			command.CommandText = "select * from port_info ";
			DbDataReader dbDataReader2 = command.ExecuteReader();
			try
			{
				if (dbDataReader2.HasRows)
				{
					dataTable2 = DBConn.ConvertOleDbReaderToDataTable(dbDataReader2);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader2.Close();
				dbDataReader2.Dispose();
			}
			command.CommandText = "select * from bank_info ";
			DbDataReader dbDataReader3 = command.ExecuteReader();
			try
			{
				if (dbDataReader3.HasRows)
				{
					dataTable3 = DBConn.ConvertOleDbReaderToDataTable(dbDataReader3);
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader3.Close();
				dbDataReader3.Dispose();
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				int num = Convert.ToInt32(dataRow[0]);
				string text = Convert.ToString(dataRow[13]);
				int num2 = 0;
				int num3 = 0;
				string key;
				switch (key = text)
				{
				case "EC1000":
					num2 = 169729;
					num3 = 1630;
					break;
				case "PE1108A":
					num2 = 41732;
					num3 = 398;
					break;
				case "PE1108B":
					num2 = 41645;
					num3 = 422;
					break;
				case "PE1108G":
					num2 = 41681;
					num3 = 415;
					break;
				case "PE1208A":
					num2 = 41798;
					num3 = 1630;
					break;
				case "PE1208B":
					num2 = 41984;
					num3 = 1630;
					break;
				case "PE1208G":
					num2 = 41422;
					num3 = 1630;
					break;
				case "PE3108A":
					num2 = 41422;
					num3 = 4693;
					break;
				case "PE3108B":
					num2 = 41422;
					num3 = 4781;
					break;
				case "PE3108G":
					num2 = 41422;
					num3 = 4580;
					break;
				case "PE3208A":
					num2 = 41422;
					num3 = 4591;
					break;
				case "PE3208B":
					num2 = 41422;
					num3 = 4231;
					break;
				case "PE3208G":
					num2 = 41422;
					num3 = 4231;
					break;
				case "PE5216A":
					num2 = 83441;
					num3 = 793;
					break;
				case "PE5216B":
					num2 = 86820;
					num3 = 812;
					break;
				case "PE5216G":
					num2 = 130748;
					num3 = 766;
					break;
				case "PE5324B":
					num2 = 124970;
					num3 = 1261;
					break;
				case "PE5324G":
					num2 = 124970;
					num3 = 1197;
					break;
				case "PE6108A":
					num2 = 41846;
					num3 = 410;
					break;
				case "PE6108B":
					num2 = 41846;
					num3 = 410;
					break;
				case "PE6108G":
					num2 = 41846;
					num3 = 410;
					break;
				case "PE6208A":
					num2 = 41846;
					num3 = 410;
					break;
				case "PE6208B":
					num2 = 87403;
					num3 = 410;
					break;
				case "PE6208G":
					num2 = 87403;
					num3 = 410;
					break;
				case "PE6216A":
					num2 = 87403;
					num3 = 910;
					break;
				case "PE6216B":
					num2 = 87403;
					num3 = 780;
					break;
				case "PE6216G":
					num2 = 87403;
					num3 = 1630;
					break;
				case "PE6324B":
					num2 = 124072;
					num3 = 13899;
					break;
				case "PE6324G":
					num2 = 124072;
					num3 = 1110;
					break;
				case "PE8108A":
					num2 = 41573;
					num3 = 1630;
					break;
				case "PE8108B":
					num2 = 41573;
					num3 = 4710;
					break;
				case "PE8108G":
					num2 = 41573;
					num3 = 4508;
					break;
				case "PE8208A":
					num2 = 41573;
					num3 = 394;
					break;
				case "PE8208B":
					num2 = 41573;
					num3 = 4626;
					break;
				case "PE8208G":
					num2 = 41573;
					num3 = 4801;
					break;
				}
				int num5 = 5000;
				int num6 = 550;
				int num7 = 6000;
				int num8 = 600;
				for (int i = 0; i < i_day; i++)
				{
					DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
					Random random = new Random((int)DateTime.Now.Ticks);
					num2 += random.Next(-500, 500);
					num3 += random.Next(-50, 50);
					num5 += random.Next(-1000, 1000);
					num6 += random.Next(-50, 50);
					num7 += random.Next(-1000, 1000);
					num8 += random.Next(-60, 60);
					command.CommandText = string.Concat(new object[]
					{
						"insert into device_data_daily (device_id,power_consumption,insert_time) values(",
						num,
						",",
						num2,
						",'",
						dateTime.ToString("yyyy-MM-dd"),
						"')"
					});
					command.ExecuteNonQuery();
					DataRow[] array = dataTable2.Select(" device_id = " + num);
					DataRow[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						DataRow dataRow2 = array2[j];
						int num9 = Convert.ToInt32(dataRow2[0]);
						command.CommandText = string.Concat(new object[]
						{
							"insert into port_data_daily (port_id,power_consumption,insert_time) values(",
							num9,
							",",
							num5,
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							"')"
						});
						command.ExecuteNonQuery();
					}
					DataRow[] array3 = dataTable3.Select(" device_id = " + num);
					DataRow[] array4 = array3;
					for (int k = 0; k < array4.Length; k++)
					{
						DataRow dataRow3 = array4[k];
						int num10 = Convert.ToInt32(dataRow3[0]);
						command.CommandText = string.Concat(new object[]
						{
							"insert into bank_data_daily (bank_id,power_consumption,insert_time)  values(",
							num10,
							",",
							num7,
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							"')"
						});
						command.ExecuteNonQuery();
					}
					for (int l = 0; l < 24; l++)
					{
						DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, l, 30, 0);
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_data_hourly  (device_id,power_consumption,insert_time) values(",
							num,
							",",
							num3,
							",'",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"')"
						});
						command.ExecuteNonQuery();
						DataRow[] array5 = array;
						for (int m = 0; m < array5.Length; m++)
						{
							DataRow dataRow4 = array5[m];
							int num11 = Convert.ToInt32(dataRow4[0]);
							command.CommandText = string.Concat(new object[]
							{
								"insert into port_data_hourly  (port_id,power_consumption,insert_time) values(",
								num11,
								",",
								num6,
								",'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"')"
							});
							command.ExecuteNonQuery();
						}
						DataRow[] array6 = array3;
						for (int n = 0; n < array6.Length; n++)
						{
							DataRow dataRow5 = array6[n];
							int num12 = Convert.ToInt32(dataRow5[0]);
							command.CommandText = string.Concat(new object[]
							{
								"insert into bank_data_hourly (bank_id,power_consumption,insert_time)  values(",
								num12,
								",",
								num8,
								",'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"')"
							});
							command.ExecuteNonQuery();
						}
					}
				}
			}
			return 0;
		}
		public static long SimulateRCI(int i_day, DbCommand command, long l_cont)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			try
			{
				for (int i = 0; i < i_day; i++)
				{
					DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
					Random random = new Random((int)DateTime.Now.Ticks);
					double num = 69999.0;
					double num2 = 69999.0;
					for (int j = 0; j < 24; j++)
					{
						DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 30, 0);
						double num3 = (double)(random.Next(-20, 6) + 90);
						if (num > num3)
						{
							num = num3;
						}
						double num4 = (double)(random.Next(-30, 6) + 90);
						if (num2 > num3)
						{
							num2 = num3;
						}
						command.CommandText = string.Concat(new object[]
						{
							"insert into rci_hourly  (rci_high,rci_lo,insert_time)  values(",
							num3,
							",",
							num4,
							",'",
							dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
							"')"
						});
						int num5 = command.ExecuteNonQuery();
						l_cont += (long)num5;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_cont);
					}
					random.Next(-20, 5);
					random.Next(-30, 5);
					command.CommandText = string.Concat(new object[]
					{
						"insert into rci_daily (rci_high,rci_lo,insert_time) values(",
						num,
						",",
						num2,
						",'",
						dateTime.ToString("yyyy-MM-dd"),
						"')"
					});
					int num6 = command.ExecuteNonQuery();
					l_cont += (long)num6;
					Console.SetCursorPosition(0, 15);
					Console.Write("Generate {0} rows history data ", l_cont);
				}
			}
			catch
			{
			}
			return l_cont;
		}
		public static long SimulateRackThermal(int i_day, DbCommand command, long l_cont)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			DBConn connection = DBConnPool.getConnection();
			OleDbCommand oleDbCommand = new OleDbCommand();
			oleDbCommand.Connection = (OleDbConnection)connection.con;
			oleDbCommand.CommandText = "select id from device_addr_info";
			DbDataReader dbDataReader = oleDbCommand.ExecuteReader();
			try
			{
				DataTable dataTable = new DataTable();
				if (dbDataReader.HasRows)
				{
					dataTable = DBConn.ConvertOleDbReaderToDataTable(dbDataReader);
				}
				dbDataReader.Close();
				foreach (DataRow dataRow in dataTable.Rows)
				{
					int num = Convert.ToInt32(dataRow[0]);
					for (int i = 0; i < i_day; i++)
					{
						DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
						Random random = new Random((int)DateTime.Now.Ticks);
						double num2 = (double)random.Next(-10, 10) + 24.96;
						double num3 = (double)random.Next(-10, 10) + 39.96;
						double num4 = (double)random.Next(-12, 12) + 38.54;
						command.CommandText = string.Concat(new object[]
						{
							"insert into rackthermal_daily (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
							num,
							",",
							num2,
							",",
							num3,
							",",
							num4,
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							"')"
						});
						int num5 = command.ExecuteNonQuery();
						l_cont += (long)num5;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_cont);
						for (int j = 0; j < 24; j++)
						{
							DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 30, 0);
							double num6 = (double)random.Next(-10, 10) + 20.96;
							double num7 = (double)random.Next(-10, 10) + 39.96;
							double num8 = (double)random.Next(-12, 12) + 38.54;
							command.CommandText = string.Concat(new object[]
							{
								"insert into rackthermal_hourly  (rack_id,intakepeak,exhaustpeak,differencepeak,insert_time) values(",
								num,
								",",
								num6,
								",",
								num7,
								",",
								num8,
								",'",
								dateTime2.ToString("yyyy-MM-dd HH:mm:ss"),
								"')"
							});
							int num9 = command.ExecuteNonQuery();
							l_cont += (long)num9;
							Console.SetCursorPosition(0, 15);
							Console.Write("Generate {0} rows history data ", l_cont);
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				dbDataReader.Dispose();
				oleDbCommand.Dispose();
				connection.close();
			}
			return l_cont;
		}
		public static void SimulateData(int i_day)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getDynaConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "select id from device_addr_info";
					DbDataReader dbDataReader = dbCommand.ExecuteReader();
					while (dbDataReader.Read())
					{
					}
					dbDataReader.Close();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				dbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
		}
		public static long InsertPortDataDaily(DataRow[] found_rows, DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < found_rows.Length; j++)
				{
					DataRow dataRow = found_rows[j];
					int num = Convert.ToInt32(dataRow[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into port_data_daily select ",
							num,
							" as port_id,pdvalue/",
							found_rows.Length,
							" as power_consumption,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' as insert_time from pdsource where model_nm = '",
							str_model,
							"' and type = 'daily' "
						});
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertPortAutoInfo(DataRow[] found_rows, DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < found_rows.Length; j++)
				{
					DataRow dataRow = found_rows[j];
					int num = Convert.ToInt32(dataRow[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into port_auto_info select ",
							num,
							" as port_id,d.powervalue as power,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + d.insert_time as insert_time from datasource d where d.model_nm = '",
							str_model,
							"' and d.type = 'port' "
						});
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 27);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertBankDataHourly(DataRow[] found_rows, DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < found_rows.Length; j++)
				{
					DataRow dataRow = found_rows[j];
					int num = Convert.ToInt32(dataRow[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into bank_data_hourly select ",
							num,
							" as bank_id,p.pdvalue/",
							found_rows.Length,
							" + ",
							random.Next(-900, 900),
							" as power_consumption,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + p.insert_time as as insert_time from pdsource p where p.model_nm = '",
							str_model,
							"' and p.type = 'hourly' "
						});
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertBankDataDaily(DataRow[] found_rows, DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < found_rows.Length; j++)
				{
					DataRow dataRow = found_rows[j];
					int num = Convert.ToInt32(dataRow[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into bank_data_daily select ",
							num,
							" as bank_id,p.pdvalue/",
							found_rows.Length,
							" + ",
							random.Next(-5000, 5000),
							" as power_consumption,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' as insert_time from pdsource p where p.model_nm = '",
							str_model,
							"' and p.type = 'daily' "
						});
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertBankAutoInfo(DataRow[] found_rows, DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < found_rows.Length; j++)
				{
					DataRow dataRow = found_rows[j];
					int num = Convert.ToInt32(dataRow[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into bank_auto_info select ",
							num,
							" as bank_id,d.powervalue as power,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + d.insert_time as insert_time from datasource d where d.model_nm = '",
							str_model,
							"' and d.type = 'port' "
						});
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceDateHourly(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				try
				{
					command.CommandText = string.Concat(new object[]
					{
						"insert into device_data_hourly select ",
						i_deviceid,
						" as device_id,p.pdvalue + ",
						random.Next(-6000, 6000),
						" as power_consumption,'",
						dateTime.ToString("yyyy-MM-dd"),
						"' + ' ' + p.insert_time as insert_time from pdsource p where p.model_nm = '",
						str_model,
						"' and p.type = 'hourly' "
					});
					long num = (long)command.ExecuteNonQuery();
					l_count += num;
					Console.SetCursorPosition(0, 15);
					Console.Write("Generate {0} rows history data ", l_count);
				}
				catch (Exception ex)
				{
					Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
					Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
				}
			}
			return l_count;
		}
		public static long InsertDeviceDataDaily(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_pdsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			int num = 0;
			try
			{
				DataRow[] array = dt_pdsource.Select(" model_nm = '" + str_model + "' and type = 'daily'");
				num = Convert.ToInt32(array[0][2]);
			}
			catch
			{
			}
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				int num2 = random.Next(-23456, 99874);
				num += num2;
				int num3 = num / 24;
				int[] array2 = new int[24];
				for (int j = 0; j < 12; j++)
				{
					int num4 = random.Next(-4000, 4000);
					array2[j] = num3 + num4;
					array2[23 - j] = num3 - num4;
				}
				try
				{
					command.CommandText = string.Concat(new object[]
					{
						"insert into device_data_daily select ",
						i_deviceid,
						" as device_id,p.pdvalue+ ",
						num2,
						" as power_consumption,'",
						dateTime.ToString("yyyy-MM-dd"),
						"' as insert_time from pdsource p where p.model_nm = '",
						str_model,
						"' and p.type = 'daily' "
					});
					long num5 = (long)command.ExecuteNonQuery();
					l_count += num5;
					Console.SetCursorPosition(0, 15);
					Console.Write("Generate {0} rows history data ", l_count);
				}
				catch (Exception ex)
				{
					Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
					Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
				}
				for (int k = 0; k < 24; k++)
				{
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_data_hourly values (",
							i_deviceid,
							",",
							array2[k],
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							" ",
							k,
							":30:00' )"
						});
						long num6 = (long)command.ExecuteNonQuery();
						l_count += num6;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex2)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceAndBankDataDaily(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_bank, DataTable dt_pdsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			int num = 0;
			try
			{
				DataRow[] array = dt_pdsource.Select(" model_nm = '" + str_model + "' and type = 'daily'");
				num = Convert.ToInt32(array[0][2]);
			}
			catch
			{
			}
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				int num2 = random.Next(-23456, 99874);
				num += num2;
				int num3 = num / 24;
				int[] array2 = new int[24];
				for (int j = 0; j < 12; j++)
				{
					int num4 = random.Next(-4000, 4000);
					array2[j] = num3 + num4;
					array2[23 - j] = num3 - num4;
				}
				try
				{
					command.CommandText = string.Concat(new object[]
					{
						"insert into device_data_daily select ",
						i_deviceid,
						" as device_id,p.pdvalue+ ",
						num2,
						" as power_consumption,'",
						dateTime.ToString("yyyy-MM-dd"),
						"' as insert_time from pdsource p where p.model_nm = '",
						str_model,
						"' and p.type = 'daily' "
					});
					long num5 = (long)command.ExecuteNonQuery();
					l_count += num5;
					Console.SetCursorPosition(0, 15);
					Console.Write("Generate {0} rows history data ", l_count);
				}
				catch (Exception ex)
				{
					Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
					Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
				}
				DataRow[] array3 = dt_bank.Select(" device_id = " + i_deviceid);
				int num6 = array3.Length;
				if (num6 > 0)
				{
					int num7 = 0;
					int num8 = num / num6;
					int[] array4 = new int[num6];
					for (int k = 0; k < num6 / 2; k++)
					{
						int num9 = random.Next(-4000, 4000);
						array4[k] = num8 + num9;
						array4[num6 - k - 1] = num8 - num9;
					}
					DataRow[] array5 = array3;
					for (int l = 0; l < array5.Length; l++)
					{
						DataRow dataRow = array5[l];
						int num10 = Convert.ToInt32(dataRow[0]);
						try
						{
							command.CommandText = string.Concat(new object[]
							{
								" insert into bank_data_daily values( ",
								num10,
								",",
								array4[num7],
								",'",
								dateTime.ToString("yyyy-MM-dd"),
								"' )"
							});
							long num11 = (long)command.ExecuteNonQuery();
							l_count += num11;
							Console.SetCursorPosition(0, 15);
							Console.Write("Generate {0} rows history data ", l_count);
						}
						catch (Exception ex2)
						{
							Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
							Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
						}
						num7++;
						for (int m = 0; m < 24; m++)
						{
							try
							{
								command.CommandText = string.Concat(new object[]
								{
									"insert into bank_data_hourly values( ",
									num10,
									",",
									array2[m] / num6,
									",'",
									dateTime.ToString("yyyy-MM-dd"),
									" ",
									m,
									":30:00' )"
								});
								long num12 = (long)command.ExecuteNonQuery();
								l_count += num12;
								Console.SetCursorPosition(0, 15);
								Console.Write("Generate {0} rows history data ", l_count);
							}
							catch (Exception ex3)
							{
								Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
								Console.WriteLine("&&&&&&&& Error Message : " + ex3.Message + "\n" + ex3.StackTrace);
							}
						}
					}
				}
				for (int n = 0; n < 24; n++)
				{
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_data_hourly values (",
							i_deviceid,
							",",
							array2[n],
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							" ",
							n,
							":30:00' )"
						});
						long num13 = (long)command.ExecuteNonQuery();
						l_count += num13;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex4)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex4.Message + "\n" + ex4.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceAndPortDataDaily(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_port, DataTable dt_pdsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			int num = 0;
			try
			{
				DataRow[] array = dt_pdsource.Select(" model_nm = '" + str_model + "' and type = 'daily'");
				num = Convert.ToInt32(array[0][2]);
			}
			catch
			{
			}
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				int num2 = random.Next(-23456, 99874);
				num += num2;
				int num3 = num / 24;
				int[] array2 = new int[24];
				for (int j = 0; j < 12; j++)
				{
					int num4 = random.Next(-4000, 4000);
					array2[j] = num3 + num4;
					array2[23 - j] = num3 - num4;
				}
				try
				{
					command.CommandText = string.Concat(new object[]
					{
						"insert into device_data_daily select ",
						i_deviceid,
						" as device_id,p.pdvalue+ ",
						num2,
						" as power_consumption,'",
						dateTime.ToString("yyyy-MM-dd"),
						"' as insert_time from pdsource p where p.model_nm = '",
						str_model,
						"' and p.type = 'daily' "
					});
					if (command.Connection is MySqlConnection)
					{
						string text = command.CommandText;
						text = text.Replace("#", "'");
						command.CommandText = text;
					}
					long num5 = (long)command.ExecuteNonQuery();
					l_count += num5;
					Console.SetCursorPosition(0, 15);
					Console.Write("Generate {0} rows history data ", l_count);
				}
				catch (Exception ex)
				{
					Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
					Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
				}
				DataRow[] array3 = dt_port.Select(" device_id = " + i_deviceid);
				int num6 = array3.Length;
				if (num6 > 0)
				{
					int num7 = 0;
					int num8 = num / num6;
					int[] array4 = new int[num6];
					for (int k = 0; k < num6 / 2; k++)
					{
						int num9 = random.Next(-4000, 4000);
						array4[k] = num8 + num9;
						array4[num6 - k - 1] = num8 - num9;
					}
					DataRow[] array5 = array3;
					for (int l = 0; l < array5.Length; l++)
					{
						DataRow dataRow = array5[l];
						int num10 = Convert.ToInt32(dataRow[0]);
						try
						{
							command.CommandText = string.Concat(new object[]
							{
								"insert into port_data_daily values (",
								num10,
								",",
								array4[num7],
								",'",
								dateTime.ToString("yyyy-MM-dd"),
								"' )"
							});
							if (command.Connection is MySqlConnection)
							{
								string text2 = command.CommandText;
								text2 = text2.Replace("#", "'");
								command.CommandText = text2;
							}
							long num11 = (long)command.ExecuteNonQuery();
							l_count += num11;
							Console.SetCursorPosition(0, 15);
							Console.Write("Generate {0} rows history data ", l_count);
						}
						catch (Exception ex2)
						{
							Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
							Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
						}
						num7++;
						for (int m = 0; m < 24; m++)
						{
							try
							{
								command.CommandText = string.Concat(new object[]
								{
									"insert into port_data_hourly values (",
									num10,
									",",
									array2[m] / num6,
									",'",
									dateTime.ToString("yyyy-MM-dd"),
									" ",
									m,
									":30:00' )"
								});
								if (command.Connection is MySqlConnection)
								{
									string text3 = command.CommandText;
									text3 = text3.Replace("#", "'");
									command.CommandText = text3;
								}
								long num12 = (long)command.ExecuteNonQuery();
								l_count += num12;
								Console.SetCursorPosition(0, 15);
								Console.Write("Generate {0} rows history data ", l_count);
							}
							catch (Exception ex3)
							{
								Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
								Console.WriteLine("&&&&&&&& Error Message : " + ex3.Message + "\n" + ex3.StackTrace);
							}
						}
					}
				}
				for (int n = 0; n < 24; n++)
				{
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_data_hourly values (",
							i_deviceid,
							",",
							array2[n],
							",'",
							dateTime.ToString("yyyy-MM-dd"),
							" ",
							n,
							":30:00' )"
						});
						if (command.Connection is MySqlConnection)
						{
							string text4 = command.CommandText;
							text4 = text4.Replace("#", "'");
							command.CommandText = text4;
						}
						long num13 = (long)command.ExecuteNonQuery();
						l_count += num13;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex4)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex4.Message + "\n" + ex4.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceAutoInfo(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 23; j++)
				{
					int num = random.Next(0, 24);
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info select ",
							i_deviceid,
							" as device_id,d.powervalue as power,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + '",
							text,
							"' + right(d.insert_time,6) as insert_time from datasource d where d.model_nm = '",
							str_model,
							"' and d.type = 'dev' and d.insert_time like '",
							num,
							":%'"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceAndBankAutoInfo(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_bank)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 23; j++)
				{
					int num = random.Next(0, 24);
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info select ",
							i_deviceid,
							" as device_id,d.powervalue as power,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + '",
							text,
							"' + right(d.insert_time,6) as insert_time from datasource d where d.model_nm = '",
							str_model,
							"' and d.type = 'dev' and d.insert_time like '",
							num,
							":%'"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
					DataRow[] array = dt_bank.Select(" device_id = " + i_deviceid);
					DataRow[] array2 = array;
					for (int k = 0; k < array2.Length; k++)
					{
						DataRow dataRow = array2[k];
						int num3 = Convert.ToInt32(dataRow[0]);
						try
						{
							command.CommandText = string.Concat(new object[]
							{
								"insert into bank_auto_info select ",
								num3,
								" as bank_id,d.powervalue + ",
								random.Next(-99345, 99345),
								" as power,'",
								dateTime.ToString("yyyy-MM-dd"),
								"' + ' ' + '",
								text,
								"' + right(d.insert_time,6) as insert_time from datasource d where d.model_nm = '",
								str_model,
								"' and d.type = 'port'  and d.insert_time like '",
								num,
								":%'"
							});
							if (command.Connection is MySqlConnection)
							{
								string text3 = command.CommandText;
								text3 = text3.Replace("#", "'");
								command.CommandText = text3;
							}
							long num4 = (long)command.ExecuteNonQuery();
							l_count += num4;
							Console.SetCursorPosition(0, 15);
							Console.Write("Generate {0} rows history data ", l_count);
						}
						catch (Exception ex2)
						{
							Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
							Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
						}
					}
				}
			}
			return l_count;
		}
		public static long InsertDeviceAndPortAutoInfo(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_port)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 23; j++)
				{
					int num = random.Next(0, 24);
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info select ",
							i_deviceid,
							" as device_id,d.powervalue as power,'",
							dateTime.ToString("yyyy-MM-dd"),
							"' + ' ' + '",
							text,
							"' + right(d.insert_time,6) as insert_time from datasource d where d.model_nm = '",
							str_model,
							"' and d.type = 'dev' and d.insert_time like '",
							num,
							":%'"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num2 = (long)command.ExecuteNonQuery();
						l_count += num2;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
					DataRow[] array = dt_port.Select(" device_id = " + i_deviceid);
					DataRow[] array2 = array;
					for (int k = 0; k < array2.Length; k++)
					{
						DataRow dataRow = array2[k];
						int num3 = Convert.ToInt32(dataRow[0]);
						try
						{
							command.CommandText = string.Concat(new object[]
							{
								"insert into port_auto_info select ",
								num3,
								" as port_id,d.powervalue + ",
								random.Next(-99345, 99345),
								" as power,'",
								dateTime.ToString("yyyy-MM-dd"),
								"' + ' ' + '",
								text,
								"' + right(d.insert_time,6) as insert_time from datasource d where d.model_nm = '",
								str_model,
								"' and d.type = 'port'  and d.insert_time like '",
								num,
								":%'"
							});
							if (command.Connection is MySqlConnection)
							{
								string text3 = command.CommandText;
								text3 = text3.Replace("#", "'");
								command.CommandText = text3;
							}
							long num4 = (long)command.ExecuteNonQuery();
							l_count += num4;
							Console.SetCursorPosition(0, 15);
							Console.Write("Generate {0} rows history data ", l_count);
						}
						catch (Exception ex2)
						{
							Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
							Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
						}
					}
				}
			}
			return l_count;
		}
		public static long InsertCellPower4port(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_port, DataTable dt_dbsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 24; j++)
				{
					int num = 0;
					num = random.Next(3900000, 5000000);
					int num2 = 0;
					try
					{
						DataRow[] array = dt_dbsource.Select(string.Concat(new object[]
						{
							" model_nm = '",
							str_model,
							"' and type = 'dev' and insert_time = '",
							j,
							":00:00'"
						}));
						num2 = Convert.ToInt32(array[0]["powervalue"]);
					}
					catch
					{
					}
					num2 += num;
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info values( ",
							i_deviceid,
							",",
							num2,
							",'",
							new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"),
							"')"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num3 = (long)command.ExecuteNonQuery();
						l_count += num3;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
					DataRow[] array2 = dt_port.Select(" device_id = " + i_deviceid);
					int num4 = array2.Length;
					if (num4 > 0)
					{
						int num5 = 0;
						int num6 = num2 / num4;
						int[] array3 = new int[num4];
						for (int k = 0; k < num4 / 2; k++)
						{
							int num7 = random.Next(-8000, 8000);
							array3[k] = num6 + num7;
							array3[num4 - k - 1] = num6 - num7;
						}
						DataRow[] array4 = array2;
						for (int l = 0; l < array4.Length; l++)
						{
							DataRow dataRow = array4[l];
							int num8 = Convert.ToInt32(dataRow[0]);
							try
							{
								command.CommandText = string.Concat(new object[]
								{
									"insert into port_auto_info values(",
									num8,
									",",
									array3[num5],
									",'",
									new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"),
									"')"
								});
								if (command.Connection is MySqlConnection)
								{
									string text3 = command.CommandText;
									text3 = text3.Replace("#", "'");
									command.CommandText = text3;
								}
								long num9 = (long)command.ExecuteNonQuery();
								l_count += num9;
								Console.SetCursorPosition(0, 15);
								Console.Write("Generate {0} rows history data ", l_count);
							}
							catch (Exception ex2)
							{
								Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
								Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
							}
							num5++;
						}
					}
				}
			}
			return l_count;
		}
		public static long InsertSameModelPower4port(DbCommand command, int i_day, int i_deviceid, int sampleid, DataTable dt_port, long l_count)
		{
			DateTime dateTime = DateTime.Now.AddDays(-1.0);
			DateTime dateTime2 = DateTime.Now.AddDays((double)(-(double)i_day - 1));
			DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
			DateTime dateTime4 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
			try
			{
				command.CommandText = string.Concat(new object[]
				{
					"insert into device_auto_info select ",
					i_deviceid,
					" as device_id,d.power as power,d.insert_time as insert_time from device_auto_info d where d.device_id = ",
					sampleid,
					" and d.insert_time >=#",
					dateTime4.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and d.insert_time <= #",
					dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
					"#"
				});
				if (command.Connection is MySqlConnection)
				{
					string text = command.CommandText;
					text = text.Replace("#", "'");
					command.CommandText = text;
				}
				long num = (long)command.ExecuteNonQuery();
				l_count += num;
				Console.SetCursorPosition(0, 15);
				Console.Write("Generate {0} rows history data ", l_count);
			}
			catch (Exception ex)
			{
				Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
				Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
			}
			DataRow[] array = dt_port.Select(" device_id = " + sampleid);
			int num2 = array.Length;
			int[] array2 = new int[num2];
			int num3 = 0;
			DataRow[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				DataRow dataRow = array3[i];
				int num4 = Convert.ToInt32(dataRow[0]);
				array2[num3] = num4;
				num3++;
			}
			DataRow[] array4 = dt_port.Select(" device_id = " + i_deviceid);
			int num5 = array4.Length;
			if (num5 > 0)
			{
				int num6 = 0;
				DataRow[] array5 = array4;
				for (int j = 0; j < array5.Length; j++)
				{
					DataRow dataRow2 = array5[j];
					int num7 = Convert.ToInt32(dataRow2[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into port_auto_info select ",
							num7,
							" as port_id,d.power as power,d.insert_time as insert_time from port_auto_info d where d.port_id = ",
							array2[num6],
							" and d.insert_time >=#",
							dateTime4.ToString("yyyy-MM-dd HH:mm:ss"),
							"# and d.insert_time <= #",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"#"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num8 = (long)command.ExecuteNonQuery();
						l_count += num8;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex2)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
					}
					num6++;
				}
			}
			return l_count;
		}
		public static long InsertCellPower4bank(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_bank, DataTable dt_dbsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 24; j++)
				{
					int num = 0;
					num = random.Next(3900000, 5000000);
					int num2 = 0;
					try
					{
						DataRow[] array = dt_dbsource.Select(string.Concat(new object[]
						{
							" model_nm = '",
							str_model,
							"' and type = 'dev' and insert_time = '",
							j,
							":00:00'"
						}));
						num2 = Convert.ToInt32(array[0]["powervalue"]);
					}
					catch
					{
					}
					num2 += num;
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info values( ",
							i_deviceid,
							",",
							num2,
							",#",
							new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"),
							"#)"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num3 = (long)command.ExecuteNonQuery();
						l_count += num3;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
					DataRow[] array2 = dt_bank.Select(" device_id = " + i_deviceid);
					int num4 = array2.Length;
					if (num4 > 0)
					{
						int num5 = 0;
						int num6 = num2 / num4;
						int[] array3 = new int[num4];
						for (int k = 0; k < num4 / 2; k++)
						{
							int num7 = random.Next(-90000, 90000);
							array3[k] = num6 + num7;
							array3[num4 - k - 1] = num6 - num7;
						}
						DataRow[] array4 = array2;
						for (int l = 0; l < array4.Length; l++)
						{
							DataRow dataRow = array4[l];
							int num8 = Convert.ToInt32(dataRow[0]);
							try
							{
								command.CommandText = string.Concat(new object[]
								{
									"insert into bank_auto_info values(",
									num8,
									",",
									array3[num5],
									",#",
									new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"),
									"#)"
								});
								if (command.Connection is MySqlConnection)
								{
									string text3 = command.CommandText;
									text3 = text3.Replace("#", "'");
									command.CommandText = text3;
								}
								long num9 = (long)command.ExecuteNonQuery();
								l_count += num9;
								Console.SetCursorPosition(0, 15);
								Console.Write("Generate {0} rows history data ", l_count);
							}
							catch (Exception ex2)
							{
								Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
								Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
							}
							num5++;
						}
					}
				}
			}
			return l_count;
		}
		public static long InsertSameModelPower4bank(DbCommand command, int i_day, int i_deviceid, int sampleid, DataTable dt_bank, long l_count)
		{
			DateTime dateTime = DateTime.Now.AddDays(-1.0);
			DateTime dateTime2 = DateTime.Now.AddDays((double)(-(double)i_day - 1));
			DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
			DateTime dateTime4 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
			try
			{
				command.CommandText = string.Concat(new object[]
				{
					"insert into device_auto_info select ",
					i_deviceid,
					" as device_id,d.power as power,d.insert_time as insert_time from device_auto_info d where d.device_id = ",
					sampleid,
					" and d.insert_time >=#",
					dateTime4.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and d.insert_time <= #",
					dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
					"#"
				});
				if (command.Connection is MySqlConnection)
				{
					string text = command.CommandText;
					text = text.Replace("#", "'");
					command.CommandText = text;
				}
				long num = (long)command.ExecuteNonQuery();
				l_count += num;
				Console.SetCursorPosition(0, 15);
				Console.Write("Generate {0} rows history data ", l_count);
			}
			catch (Exception ex)
			{
				Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
				Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
			}
			DataRow[] array = dt_bank.Select(" device_id = " + sampleid);
			int num2 = array.Length;
			int[] array2 = new int[num2];
			int num3 = 0;
			DataRow[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				DataRow dataRow = array3[i];
				int num4 = Convert.ToInt32(dataRow[0]);
				array2[num3] = num4;
				num3++;
			}
			DataRow[] array4 = dt_bank.Select(" device_id = " + i_deviceid);
			int num5 = array4.Length;
			if (num5 > 0)
			{
				int num6 = 0;
				DataRow[] array5 = array4;
				for (int j = 0; j < array5.Length; j++)
				{
					DataRow dataRow2 = array5[j];
					int num7 = Convert.ToInt32(dataRow2[0]);
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into bank_auto_info select ",
							num7,
							" as bank_id,d.power as power,d.insert_time as insert_time from bank_auto_info d where d.bank_id = ",
							array2[num6],
							" and d.insert_time >=#",
							dateTime4.ToString("yyyy-MM-dd HH:mm:ss"),
							"# and d.insert_time <= #",
							dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
							"#"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num8 = (long)command.ExecuteNonQuery();
						l_count += num8;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex2)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex2.Message + "\n" + ex2.StackTrace);
					}
					num6++;
				}
			}
			return l_count;
		}
		public static long InsertCellPower(DbCommand command, int i_day, int i_deviceid, string str_model, long l_count, DataTable dt_dbsource)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < i_day; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays((double)(-(double)(i + 1)));
				for (int j = 0; j < 24; j++)
				{
					int num = 0;
					num = random.Next(3900000, 5000000);
					int num2 = 0;
					try
					{
						DataRow[] array = dt_dbsource.Select(string.Concat(new object[]
						{
							" model_nm = '",
							str_model,
							"' and type = 'dev' and insert_time = '",
							j,
							":00:00'"
						}));
						num2 = Convert.ToInt32(array[0]["powervalue"]);
					}
					catch
					{
					}
					num2 += num;
					string text = string.Concat(j);
					if (text.Length < 2)
					{
						text = "0" + j;
					}
					try
					{
						command.CommandText = string.Concat(new object[]
						{
							"insert into device_auto_info values( ",
							i_deviceid,
							",",
							num2,
							",#",
							new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, j, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"),
							"#)"
						});
						if (command.Connection is MySqlConnection)
						{
							string text2 = command.CommandText;
							text2 = text2.Replace("#", "'");
							command.CommandText = text2;
						}
						long num3 = (long)command.ExecuteNonQuery();
						l_count += num3;
						Console.SetCursorPosition(0, 15);
						Console.Write("Generate {0} rows history data ", l_count);
					}
					catch (Exception ex)
					{
						Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
						Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
			return l_count;
		}
		public static long InsertSameModelPower(DbCommand command, int i_day, int i_deviceid, int sampleid, long l_count)
		{
			DateTime dateTime = DateTime.Now.AddDays(-1.0);
			DateTime dateTime2 = DateTime.Now.AddDays((double)(-(double)i_day - 1));
			DateTime dateTime3 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
			DateTime dateTime4 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
			try
			{
				command.CommandText = string.Concat(new object[]
				{
					"insert into device_auto_info select ",
					i_deviceid,
					" as device_id,d.power as power,d.insert_time as insert_time from device_auto_info d where d.device_id = ",
					sampleid,
					" and d.insert_time >=#",
					dateTime4.ToString("yyyy-MM-dd HH:mm:ss"),
					"# and d.insert_time <= #",
					dateTime3.ToString("yyyy-MM-dd HH:mm:ss"),
					"#"
				});
				if (command.Connection is MySqlConnection)
				{
					string text = command.CommandText;
					text = text.Replace("#", "'");
					command.CommandText = text;
				}
				long num = (long)command.ExecuteNonQuery();
				l_count += num;
				Console.SetCursorPosition(0, 15);
				Console.Write("Generate {0} rows history data ", l_count);
			}
			catch (Exception ex)
			{
				Console.WriteLine("&&&&&&&& SQL : " + command.CommandText);
				Console.WriteLine("&&&&&&&& Error Message : " + ex.Message + "\n" + ex.StackTrace);
			}
			return l_count;
		}
	}
}
