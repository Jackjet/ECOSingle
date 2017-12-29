using System;
using System.Data.Common;
namespace DBAccessAPI
{
	public class ExpiryCheck
	{
		public static void SyncExpiredDate()
		{
			try
			{
				string clearText = DateTime.Now.ToString("yyyy-MM-dd");
				clearText = AESEncryptionUtility.Encrypt(clearText, "evaluate_date-time");
				string text = DateTime.Now.AddDays(-14.0).ToString("yyyy-MM-dd");
				text = AESEncryptionUtility.Encrypt(text, "evaluate_date-time");
				string expiryfromlog = ExpiryCheck.getExpiryfromlog();
				string expiryfromthermal = ExpiryCheck.getExpiryfromthermal();
				if (!expiryfromlog.Equals(expiryfromthermal))
				{
					if (expiryfromlog.Length == 10 && expiryfromthermal.Length == 10)
					{
						try
						{
							DateTime d = new DateTime(Convert.ToInt32(expiryfromlog.Substring(0, 4)), Convert.ToInt32(expiryfromlog.Substring(5, 2)), Convert.ToInt32(expiryfromlog.Substring(8, 2)));
							DateTime dateTime = new DateTime(Convert.ToInt32(expiryfromthermal.Substring(0, 4)), Convert.ToInt32(expiryfromthermal.Substring(5, 2)), Convert.ToInt32(expiryfromthermal.Substring(8, 2)));
							if (d.CompareTo(dateTime) > 0)
							{
								if ((DateTime.Now - dateTime).TotalDays < 0.0)
								{
									ExpiryCheck.setExpiry4log(text, 3);
									ExpiryCheck.setExpiry4thermal(text, 3);
									return;
								}
								string text2 = dateTime.ToString("yyyy-MM-dd");
								text2 = AESEncryptionUtility.Encrypt(text2, "evaluate_date-time");
								ExpiryCheck.setExpiry4log(text2, 3);
								return;
							}
							else
							{
								if ((DateTime.Now - d).TotalDays < 0.0)
								{
									ExpiryCheck.setExpiry4log(text, 3);
									ExpiryCheck.setExpiry4thermal(text, 3);
									return;
								}
								string text3 = d.ToString("yyyy-MM-dd");
								text3 = AESEncryptionUtility.Encrypt(text3, "evaluate_date-time");
								ExpiryCheck.setExpiry4thermal(text3, 3);
								return;
							}
						}
						catch
						{
							return;
						}
					}
					if (expiryfromlog.Length == 10 && expiryfromthermal.Length != 10)
					{
						try
						{
							DateTime d2 = new DateTime(Convert.ToInt32(expiryfromlog.Substring(0, 4)), Convert.ToInt32(expiryfromlog.Substring(5, 2)), Convert.ToInt32(expiryfromlog.Substring(8, 2)));
							string a;
							int iType;
							if ((a = expiryfromthermal) != null)
							{
								if (a == "NOTABLE")
								{
									iType = 0;
									goto IL_253;
								}
								if (a == "NOVALUE")
								{
									iType = 1;
									goto IL_253;
								}
								if (a == "VALUENULL")
								{
									iType = 2;
									goto IL_253;
								}
							}
							iType = 3;
							IL_253:
							if ((DateTime.Now - d2).TotalDays < 0.0)
							{
								ExpiryCheck.setExpiry4log(text, 3);
								ExpiryCheck.setExpiry4thermal(text, iType);
								return;
							}
							string text4 = d2.ToString("yyyy-MM-dd");
							text4 = AESEncryptionUtility.Encrypt(text4, "evaluate_date-time");
							ExpiryCheck.setExpiry4thermal(text4, iType);
							return;
						}
						catch
						{
							return;
						}
					}
					if (expiryfromlog.Length != 10 && expiryfromthermal.Length == 10)
					{
						try
						{
							DateTime d3 = new DateTime(Convert.ToInt32(expiryfromthermal.Substring(0, 4)), Convert.ToInt32(expiryfromthermal.Substring(5, 2)), Convert.ToInt32(expiryfromthermal.Substring(8, 2)));
							string a2;
							int iType2;
							if ((a2 = expiryfromlog) != null)
							{
								if (a2 == "NOTABLE")
								{
									iType2 = 0;
									goto IL_34D;
								}
								if (a2 == "NOVALUE")
								{
									iType2 = 1;
									goto IL_34D;
								}
								if (a2 == "VALUENULL")
								{
									iType2 = 2;
									goto IL_34D;
								}
							}
							iType2 = 3;
							IL_34D:
							if ((DateTime.Now - d3).TotalDays < 0.0)
							{
								ExpiryCheck.setExpiry4log(text, iType2);
								ExpiryCheck.setExpiry4thermal(text, 3);
							}
							else
							{
								string text5 = d3.ToString("yyyy-MM-dd");
								text5 = AESEncryptionUtility.Encrypt(text5, "evaluate_date-time");
								ExpiryCheck.setExpiry4log(text5, iType2);
							}
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}
		public static int isOverdue()
		{
			int result;
			try
			{
				string text = DateTime.Now.ToString("yyyy-MM-dd");
				text = AESEncryptionUtility.Encrypt(text, "evaluate_date-time");
				string text2 = DateTime.Now.AddDays(-14.0).ToString("yyyy-MM-dd");
				text2 = AESEncryptionUtility.Encrypt(text2, "evaluate_date-time");
				string expiryfromlog = ExpiryCheck.getExpiryfromlog();
				string expiryfromthermal = ExpiryCheck.getExpiryfromthermal();
				if (expiryfromlog.Equals(expiryfromthermal))
				{
					if (expiryfromlog.Length == 0)
					{
						result = 0;
					}
					else
					{
						if (expiryfromlog.Equals("NOTABLE"))
						{
							ExpiryCheck.setExpiry4log(text, 0);
							ExpiryCheck.setExpiry4thermal(text, 0);
							result = 30;
						}
						else
						{
							if (expiryfromlog.Equals("NOVALUE"))
							{
								result = 0;
							}
							else
							{
								if (expiryfromlog.Equals("VALUENULL"))
								{
									result = 0;
								}
								else
								{
									if (expiryfromlog.Length == 10)
									{
										try
										{
											DateTime d = new DateTime(Convert.ToInt32(expiryfromlog.Substring(0, 4)), Convert.ToInt32(expiryfromlog.Substring(5, 2)), Convert.ToInt32(expiryfromlog.Substring(8, 2)));
											TimeSpan timeSpan = DateTime.Now - d;
											if (timeSpan.TotalDays < 0.0)
											{
												ExpiryCheck.setExpiry4log(text2, 3);
												ExpiryCheck.setExpiry4thermal(text2, 3);
												result = 14;
												return result;
											}
											if (timeSpan.TotalDays > 30.0)
											{
												result = 0;
												return result;
											}
											result = 30 - Convert.ToInt32(timeSpan.TotalDays);
											return result;
										}
										catch
										{
											result = 0;
											return result;
										}
									}
									result = 0;
								}
							}
						}
					}
				}
				else
				{
					if (expiryfromlog.Length == 0 || expiryfromthermal.Length == 0)
					{
						result = 0;
					}
					else
					{
						if (expiryfromlog.Length == 10 && expiryfromthermal.Length == 10)
						{
							try
							{
								DateTime d2 = new DateTime(Convert.ToInt32(expiryfromlog.Substring(0, 4)), Convert.ToInt32(expiryfromlog.Substring(5, 2)), Convert.ToInt32(expiryfromlog.Substring(8, 2)));
								DateTime dateTime = new DateTime(Convert.ToInt32(expiryfromthermal.Substring(0, 4)), Convert.ToInt32(expiryfromthermal.Substring(5, 2)), Convert.ToInt32(expiryfromthermal.Substring(8, 2)));
								if (d2.CompareTo(dateTime) > 0)
								{
									TimeSpan timeSpan2 = DateTime.Now - dateTime;
									if (timeSpan2.TotalDays < 0.0)
									{
										ExpiryCheck.setExpiry4log(text2, 3);
										ExpiryCheck.setExpiry4thermal(text2, 3);
										result = 14;
										return result;
									}
									if (timeSpan2.TotalDays > 30.0)
									{
										result = 0;
										return result;
									}
									string text3 = dateTime.ToString("yyyy-MM-dd");
									text3 = AESEncryptionUtility.Encrypt(text3, "evaluate_date-time");
									ExpiryCheck.setExpiry4log(text3, 3);
									result = 30 - Convert.ToInt32(timeSpan2.TotalDays);
									return result;
								}
								else
								{
									TimeSpan timeSpan3 = DateTime.Now - d2;
									if (timeSpan3.TotalDays < 0.0)
									{
										ExpiryCheck.setExpiry4log(text2, 3);
										ExpiryCheck.setExpiry4thermal(text2, 3);
										result = 14;
										return result;
									}
									if (timeSpan3.TotalDays > 30.0)
									{
										result = 0;
										return result;
									}
									string text4 = d2.ToString("yyyy-MM-dd");
									text4 = AESEncryptionUtility.Encrypt(text4, "evaluate_date-time");
									ExpiryCheck.setExpiry4thermal(text4, 3);
									result = 30 - Convert.ToInt32(timeSpan3.TotalDays);
									return result;
								}
							}
							catch
							{
								result = 0;
								return result;
							}
						}
						if (expiryfromlog.Length == 10 && expiryfromthermal.Length != 10)
						{
							try
							{
								DateTime d3 = new DateTime(Convert.ToInt32(expiryfromlog.Substring(0, 4)), Convert.ToInt32(expiryfromlog.Substring(5, 2)), Convert.ToInt32(expiryfromlog.Substring(8, 2)));
								string a;
								int iType;
								if ((a = expiryfromthermal) != null)
								{
									if (a == "NOTABLE")
									{
										iType = 0;
										goto IL_3E4;
									}
									if (a == "NOVALUE")
									{
										iType = 1;
										goto IL_3E4;
									}
									if (a == "VALUENULL")
									{
										iType = 2;
										goto IL_3E4;
									}
								}
								iType = 3;
								IL_3E4:
								TimeSpan timeSpan4 = DateTime.Now - d3;
								if (timeSpan4.TotalDays < 0.0)
								{
									ExpiryCheck.setExpiry4log(text2, 3);
									ExpiryCheck.setExpiry4thermal(text2, iType);
									result = 14;
									return result;
								}
								if (timeSpan4.TotalDays > 30.0)
								{
									result = 0;
									return result;
								}
								string text5 = d3.ToString("yyyy-MM-dd");
								text5 = AESEncryptionUtility.Encrypt(text5, "evaluate_date-time");
								ExpiryCheck.setExpiry4thermal(text5, iType);
								result = 30 - Convert.ToInt32(timeSpan4.TotalDays);
								return result;
							}
							catch
							{
								result = 0;
								return result;
							}
						}
						if (expiryfromlog.Length != 10 && expiryfromthermal.Length == 10)
						{
							try
							{
								DateTime d4 = new DateTime(Convert.ToInt32(expiryfromthermal.Substring(0, 4)), Convert.ToInt32(expiryfromthermal.Substring(5, 2)), Convert.ToInt32(expiryfromthermal.Substring(8, 2)));
								string a2;
								int iType2;
								if ((a2 = expiryfromlog) != null)
								{
									if (a2 == "NOTABLE")
									{
										iType2 = 0;
										goto IL_50E;
									}
									if (a2 == "NOVALUE")
									{
										iType2 = 1;
										goto IL_50E;
									}
									if (a2 == "VALUENULL")
									{
										iType2 = 2;
										goto IL_50E;
									}
								}
								iType2 = 3;
								IL_50E:
								TimeSpan timeSpan5 = DateTime.Now - d4;
								if (timeSpan5.TotalDays < 0.0)
								{
									ExpiryCheck.setExpiry4log(text2, iType2);
									ExpiryCheck.setExpiry4thermal(text2, 3);
									result = 14;
									return result;
								}
								if (timeSpan5.TotalDays > 30.0)
								{
									result = 0;
									return result;
								}
								string text6 = d4.ToString("yyyy-MM-dd");
								text6 = AESEncryptionUtility.Encrypt(text6, "evaluate_date-time");
								ExpiryCheck.setExpiry4log(text6, iType2);
								result = 30 - Convert.ToInt32(timeSpan5.TotalDays);
								return result;
							}
							catch
							{
								result = 0;
								return result;
							}
						}
						result = 0;
					}
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		private static string getExpiryfromthermal()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getThermalConnection();
				if (dBConn != null && dBConn.con != null)
				{
					string result;
					if (DBUtil.DetermineTableExist(dBConn.con, "sys_info"))
					{
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "select * from sys_info ";
						object obj = dbCommand.ExecuteScalar();
						if (obj != null && obj != DBNull.Value)
						{
							string text = Convert.ToString(obj);
							if (text.Length == 0)
							{
								result = "VALUENULL";
								return result;
							}
							try
							{
								text = AESEncryptionUtility.Decrypt(text, "evaluate_date-time");
								result = text;
								return result;
							}
							catch
							{
								result = "FATAL";
								return result;
							}
						}
						result = "NOVALUE";
						return result;
					}
					result = "NOTABLE";
					return result;
				}
			}
			catch (Exception)
			{
				string result = "";
				return result;
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.close();
					}
					catch
					{
					}
				}
			}
			return "";
		}
		private static int setExpiry4thermal(string strV, int iType)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getThermalConnection();
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					switch (iType)
					{
					case 0:
						try
						{
							dbCommand.CommandText = "create table sys_info (sysid varchar(255) )";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "insert into sys_info (sysid ) values ('" + strV + "' )";
							dbCommand.ExecuteNonQuery();
							int result = 1;
							return result;
						}
						catch
						{
							int result = -1;
							return result;
						}
						break;
					case 1:
						break;
					case 2:
						goto IL_A8;
					case 3:
						goto IL_CE;
					default:
						goto IL_F4;
					}
					try
					{
						dbCommand.CommandText = "insert into sys_info (sysid ) values ('" + strV + "' )";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
					try
					{
						IL_A8:
						dbCommand.CommandText = "update sys_info set sysid = '" + strV + "'";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
					try
					{
						IL_CE:
						dbCommand.CommandText = "update sys_info set sysid = '" + strV + "'";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
				}
				IL_F4:;
			}
			catch
			{
				int result = -1;
				return result;
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
		private static string getExpiryfromlog()
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn != null && dBConn.con != null)
				{
					string result;
					if (DBUtil.DetermineTableExist(dBConn.con, "sys_info"))
					{
						dbCommand = dBConn.con.CreateCommand();
						dbCommand.CommandText = "select * from sys_info ";
						object obj = dbCommand.ExecuteScalar();
						if (obj != null && obj != DBNull.Value)
						{
							string text = Convert.ToString(obj);
							if (text.Length == 0)
							{
								result = "VALUENULL";
								return result;
							}
							try
							{
								text = AESEncryptionUtility.Decrypt(text, "evaluate_date-time");
								result = text;
								return result;
							}
							catch
							{
								result = "FATAL";
								return result;
							}
						}
						result = "NOVALUE";
						return result;
					}
					result = "NOTABLE";
					return result;
				}
			}
			catch (Exception)
			{
				string result = "";
				return result;
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.close();
					}
					catch
					{
					}
				}
			}
			return "";
		}
		private static int setExpiry4log(string strV, int iType)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getLogConnection();
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					switch (iType)
					{
					case 0:
						try
						{
							dbCommand.CommandText = "create table sys_info (sysid varchar(255) )";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "insert into sys_info (sysid ) values ('" + strV + "' )";
							dbCommand.ExecuteNonQuery();
							int result = 1;
							return result;
						}
						catch
						{
							int result = -1;
							return result;
						}
						break;
					case 1:
						break;
					case 2:
						goto IL_A8;
					case 3:
						goto IL_CE;
					default:
						goto IL_F4;
					}
					try
					{
						dbCommand.CommandText = "insert into sys_info (sysid ) values ('" + strV + "' )";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
					try
					{
						IL_A8:
						dbCommand.CommandText = "update sys_info set sysid = '" + strV + "'";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
					try
					{
						IL_CE:
						dbCommand.CommandText = "update sys_info set sysid = '" + strV + "'";
						dbCommand.ExecuteNonQuery();
						int result = 1;
						return result;
					}
					catch
					{
						int result = -1;
						return result;
					}
				}
				IL_F4:;
			}
			catch
			{
				int result = -1;
				return result;
			}
			finally
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				if (dBConn != null)
				{
					try
					{
						dBConn.close();
					}
					catch
					{
					}
				}
			}
			return -1;
		}
	}
}
