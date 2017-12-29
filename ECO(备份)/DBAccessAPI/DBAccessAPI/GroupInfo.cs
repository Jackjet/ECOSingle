using CommonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class GroupInfo
	{
		public const int GROUP_SELECT_CHECK = 1;
		public const int GROUP_SELECT_UNCHECK = 0;
		public const int GROUP_SELECT_ALL = 2;
		public const string GROUP_TYPE_ZONE = "zone";
		public const string GROUP_TYPE_RACK = "rack";
		public const string GROUP_TYPE_DEV = "dev";
		public const string GROUP_BUILTIN_OUTLET = "alloutlet";
		public const string GROUP_BUILTIN_RACK = "allrack";
		public const string GROUP_BUILTIN_DEV = "alldev";
		public const string GROUP_TYPE_OUTLET = "outlet";
		public const string GROUP_MEMBER_ColNM = "name";
		public const string GROUP_MEMBER_ColParentNM = "parentname";
		public const string GROUP_MEMBER_ColID = "memberid";
		private long id = -1L;
		private string group_name = "";
		private string group_type = "";
		private string color = "";
		private int isselected;
		private int thermalflag;
		private int billflag;
		private string members;
		public long ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public string GroupName
		{
			get
			{
				return this.group_name;
			}
			set
			{
				this.group_name = value;
			}
		}
		public string GroupType
		{
			get
			{
				return this.group_type;
			}
			set
			{
				this.group_type = value;
			}
		}
		public string LineColor
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}
		public int SelectedFlag
		{
			get
			{
				return this.isselected;
			}
			set
			{
				this.isselected = value;
			}
		}
		public int ThermalFlag
		{
			get
			{
				return this.thermalflag;
			}
			set
			{
				this.thermalflag = value;
			}
		}
		public int BillFlag
		{
			get
			{
				return this.billflag;
			}
			set
			{
				this.billflag = value;
			}
		}
		public string Members
		{
			get
			{
				return this.members;
			}
			set
			{
				this.members = value;
			}
		}
		public string GetMemberList()
		{
			return this.members;
		}
		public GroupInfo()
		{
		}
		public GroupInfo(GroupInfo gp_scr)
		{
			this.id = gp_scr.id;
			this.group_name = gp_scr.group_name;
			this.group_type = gp_scr.group_type;
			this.color = gp_scr.color;
			this.isselected = gp_scr.isselected;
			this.thermalflag = gp_scr.thermalflag;
			try
			{
				this.billflag = gp_scr.billflag;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				this.billflag = 0;
			}
			this.members = gp_scr.members;
		}
		public GroupInfo(DataRow dr_group)
		{
			this.id = (long)Convert.ToInt32(dr_group["id"]);
			this.group_name = Convert.ToString(dr_group["groupname"]);
			this.group_type = Convert.ToString(dr_group["grouptype"]);
			this.color = Convert.ToString(dr_group["linecolor"]);
			this.isselected = Convert.ToInt32(dr_group["isselect"]);
			this.thermalflag = Convert.ToInt32(dr_group["thermalflag"]);
			try
			{
				this.billflag = Convert.ToInt32(dr_group["billflag"]);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				this.billflag = 0;
			}
		}
		public int Update()
		{
			DBConn dBConn = null;
			OleDbCommand oleDbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					OleDbTransaction transaction = (OleDbTransaction)dBConn.con.BeginTransaction();
					oleDbCommand = (OleDbCommand)dBConn.con.CreateCommand();
					oleDbCommand.CommandType = CommandType.Text;
					oleDbCommand.Transaction = transaction;
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = "delete from group_detail where group_id = ? ";
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Prepare();
					oleDbCommand.Parameters[0].Value = this.id;
					int result;
					try
					{
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Transaction.Rollback();
						result = -1;
						return result;
					}
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = string.Concat(new object[]
					{
						"insert into group_detail (group_id,grouptype,dest_id) values (",
						this.id,
						",'",
						this.group_type,
						"',?)"
					});
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Prepare();
					string[] array = this.members.Split(new string[]
					{
						","
					}, StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string value = array2[i];
						oleDbCommand.Parameters[0].Value = Convert.ToInt64(value);
						int num = oleDbCommand.ExecuteNonQuery();
						if (num < 0)
						{
							oleDbCommand.Transaction.Rollback();
							result = num;
							return result;
						}
					}
					oleDbCommand.Parameters.Clear();
					oleDbCommand.CommandText = "update data_group set groupname= ?, grouptype = ?, linecolor = ?, isselect = ?, thermalflag = ?, billflag = ? where id = " + this.id;
					oleDbCommand.Parameters.Add("?", OleDbType.VarChar);
					oleDbCommand.Parameters.Add("?", OleDbType.VarChar);
					oleDbCommand.Parameters.Add("?", OleDbType.VarChar);
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters.Add("?", OleDbType.Integer);
					oleDbCommand.Parameters[0].Value = this.group_name;
					oleDbCommand.Parameters[1].Value = this.group_type;
					oleDbCommand.Parameters[2].Value = this.color;
					oleDbCommand.Parameters[3].Value = Convert.ToInt32(this.isselected);
					oleDbCommand.Parameters[4].Value = Convert.ToInt64(this.thermalflag);
					oleDbCommand.Parameters[5].Value = Convert.ToInt64(this.billflag);
					try
					{
						oleDbCommand.ExecuteNonQuery();
					}
					catch
					{
						oleDbCommand.Transaction.Rollback();
						result = -1;
						return result;
					}
					oleDbCommand.Parameters.Clear();
					oleDbCommand.Transaction.Commit();
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
					result = 1;
					return result;
				}
			}
			catch (Exception ex)
			{
				oleDbCommand.Transaction.Rollback();
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
			}
			finally
			{
				oleDbCommand.Dispose();
				if (dBConn != null)
				{
					dBConn.close();
				}
			}
			return -1;
		}
		public static bool CheckGroupName(long l_gid, string str_gname)
		{
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo groupInfo in values)
					{
						if (groupInfo.group_name.Equals(str_gname))
						{
							bool result;
							if (groupInfo.id == l_gid)
							{
								result = true;
								return result;
							}
							result = false;
							return result;
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				bool result = false;
				return result;
			}
			return true;
		}
		public static GroupInfo GetGroupByName(string str_name)
		{
			GroupInfo result = null;
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo groupInfo in values)
					{
						if (groupInfo.GroupName.Equals(str_name))
						{
							return new GroupInfo(groupInfo);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
		public static GroupInfo GetGroupByID(long l_id)
		{
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.ContainsKey(l_id))
				{
					GroupInfo gp_scr = (GroupInfo)groupCache[l_id];
					return new GroupInfo(gp_scr);
				}
			}
			catch (Exception)
			{
			}
			return null;
		}
		public static int DeleteGroupByID(long l_id)
		{
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					string commandText = "delete from group_detail where group_id = " + l_id;
					dbCommand.CommandText = commandText;
					int result = dbCommand.ExecuteNonQuery();
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "delete from groupcontroltask where groupid = " + l_id;
					result = dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from taskschedule where groupid = " + l_id;
					result = dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from data_group where id = " + l_id;
					result = dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "delete from ugp where gid =" + l_id;
					dbCommand.ExecuteNonQuery();
					DBCacheStatus.Group = true;
					DBCacheStatus.GroupTask = true;
					DBCacheStatus.User = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_GroupTask"
					});
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_User"
					});
					return result;
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
					dBConn.close();
				}
			}
			return -1;
		}
		public static List<GroupInfo> GetGroupByThermalFlag(int i_selectflag)
		{
			List<GroupInfo> list = new List<GroupInfo>();
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo groupInfo in values)
					{
						if (i_selectflag == 2)
						{
							GroupInfo item = new GroupInfo(groupInfo);
							list.Add(item);
						}
						else
						{
							if (groupInfo.ThermalFlag == i_selectflag)
							{
								GroupInfo item2 = new GroupInfo(groupInfo);
								list.Add(item2);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public static List<GroupInfo> GetGroupByBillFlag(int i_selectflag)
		{
			List<GroupInfo> list = new List<GroupInfo>();
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo groupInfo in values)
					{
						if (i_selectflag == 2)
						{
							GroupInfo item = new GroupInfo(groupInfo);
							list.Add(item);
						}
						else
						{
							if (groupInfo.BillFlag == i_selectflag)
							{
								GroupInfo item2 = new GroupInfo(groupInfo);
								list.Add(item2);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public static List<GroupInfo> GetPartGroup(int i_selectflag)
		{
			List<GroupInfo> list = new List<GroupInfo>();
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo groupInfo in values)
					{
						if (i_selectflag == 2)
						{
							GroupInfo item = new GroupInfo(groupInfo);
							list.Add(item);
						}
						else
						{
							if (groupInfo.SelectedFlag == i_selectflag)
							{
								GroupInfo item2 = new GroupInfo(groupInfo);
								list.Add(item2);
							}
						}
					}
				}
				list.Sort((GroupInfo x, GroupInfo y) => x.group_name.CompareTo(y.group_name));
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public static int UpdateGroupThermalFlag(int i_selectflag, string str_groups)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					string commandText = string.Concat(new object[]
					{
						"update data_group set thermalflag = ",
						i_selectflag,
						" where id in (",
						str_groups,
						" )"
					});
					dbCommand.CommandText = commandText;
					result = dbCommand.ExecuteNonQuery();
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
					dBConn.close();
				}
			}
			return result;
		}
		public static int UpdateGroupBillFlag(int i_selectflag, string str_groups)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					string commandText = string.Concat(new object[]
					{
						"update data_group set billflag = ",
						i_selectflag,
						" where id in (",
						str_groups,
						" )"
					});
					dbCommand.CommandText = commandText;
					result = dbCommand.ExecuteNonQuery();
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
					dBConn.close();
				}
			}
			return result;
		}
		public static int UpdateGroupStatus(int i_selectflag, string str_groups)
		{
			int result = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					string commandText = string.Concat(new object[]
					{
						"update data_group set isselect = ",
						i_selectflag,
						" where id in (",
						str_groups,
						" )"
					});
					dbCommand.CommandText = commandText;
					result = dbCommand.ExecuteNonQuery();
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
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
					dBConn.close();
				}
			}
			return result;
		}
		public static List<GroupInfo> GetAllGroup()
		{
			List<GroupInfo> list = new List<GroupInfo>();
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0)
				{
					ICollection values = groupCache.Values;
					foreach (GroupInfo gp_scr in values)
					{
						GroupInfo item = new GroupInfo(gp_scr);
						list.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			return list;
		}
		public static DataTable GetMemberNameInfo(long l_gid)
		{
			DataTable dataTable = new DataTable();
			DataColumn dataColumn = new DataColumn("memberid");
			dataColumn.DataType = Type.GetType("System.Int64");
			dataTable.Columns.Add(dataColumn);
			DataColumn dataColumn2 = new DataColumn("name");
			dataColumn2.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn2);
			DataColumn dataColumn3 = new DataColumn("parentname");
			dataColumn3.DataType = Type.GetType("System.String");
			dataTable.Columns.Add(dataColumn3);
			try
			{
				Hashtable groupCache = DBCache.GetGroupCache();
				if (groupCache != null && groupCache.Count > 0 && groupCache.ContainsKey(l_gid))
				{
					GroupInfo groupInfo = (GroupInfo)groupCache[l_gid];
					string groupType;
					if ((groupType = groupInfo.GroupType) != null)
					{
						if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x60000f3-1 == null)
						{
							<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x60000f3-1 = new Dictionary<string, int>(7)
							{

								{
									"allrack",
									0
								},

								{
									"rack",
									1
								},

								{
									"alldev",
									2
								},

								{
									"dev",
									3
								},

								{
									"alloutlet",
									4
								},

								{
									"outlet",
									5
								},

								{
									"zone",
									6
								}
							};
						}
						int num;
						if (<PrivateImplementationDetails>{DC6F5227-DF66-41C5-8461-C47389EE7A9A}.$$method0x60000f3-1.TryGetValue(groupType, out num))
						{
							Hashtable rackCache;
							switch (num)
							{
							case 0:
							{
								rackCache = DBCache.GetRackCache();
								if (rackCache == null || rackCache.Count <= 0)
								{
									goto IL_92D;
								}
								int rackFullNameflag = Sys_Para.GetRackFullNameflag();
								ICollection values = rackCache.Values;
								IEnumerator enumerator = values.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										RackInfo rackInfo = (RackInfo)enumerator.Current;
										rackInfo.setNameFlag(rackFullNameflag);
										DataRow dataRow = dataTable.NewRow();
										dataRow["memberid"] = rackInfo.RackID;
										dataRow["name"] = rackInfo.RackName;
										dataRow["parentname"] = RackInfo.GetZoneNamesByRackID(rackInfo.RackID);
										dataTable.Rows.Add(dataRow);
									}
									goto IL_92D;
								}
								finally
								{
									IDisposable disposable = enumerator as IDisposable;
									if (disposable != null)
									{
										disposable.Dispose();
									}
								}
								break;
							}
							case 1:
								break;
							case 2:
								goto IL_356;
							case 3:
								goto IL_473;
							case 4:
								goto IL_5E1;
							case 5:
								goto IL_6E4;
							case 6:
								goto IL_838;
							default:
								goto IL_92D;
							}
							rackCache = DBCache.GetRackCache();
							Hashtable groupDestIDMap = DBCache.GetGroupDestIDMap();
							if (rackCache == null || rackCache.Count <= 0 || groupDestIDMap == null || !groupDestIDMap.ContainsKey(l_gid))
							{
								goto IL_92D;
							}
							int rackFullNameflag2 = Sys_Para.GetRackFullNameflag();
							List<long> list = (List<long>)groupDestIDMap[l_gid];
							using (List<long>.Enumerator enumerator2 = list.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									long current = enumerator2.Current;
									if (rackCache.ContainsKey(Convert.ToInt32(current)))
									{
										RackInfo rackInfo2 = (RackInfo)rackCache[Convert.ToInt32(current)];
										rackInfo2.setNameFlag(rackFullNameflag2);
										DataRow dataRow2 = dataTable.NewRow();
										dataRow2["memberid"] = rackInfo2.RackID;
										dataRow2["name"] = rackInfo2.RackName;
										dataRow2["parentname"] = RackInfo.GetZoneNamesByRackID(rackInfo2.RackID);
										dataTable.Rows.Add(dataRow2);
									}
								}
								goto IL_92D;
							}
							IL_356:
							Hashtable deviceCache = DBCache.GetDeviceCache();
							rackCache = DBCache.GetRackCache();
							if (deviceCache == null || deviceCache.Count <= 0 || rackCache == null || rackCache.Count <= 0)
							{
								goto IL_92D;
							}
							int rackFullNameflag3 = Sys_Para.GetRackFullNameflag();
							ICollection values2 = deviceCache.Values;
							IEnumerator enumerator3 = values2.GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									DeviceInfo deviceInfo = (DeviceInfo)enumerator3.Current;
									DataRow dataRow3 = dataTable.NewRow();
									dataRow3["memberid"] = deviceInfo.DeviceID;
									dataRow3["name"] = deviceInfo.DeviceName;
									if (rackCache.ContainsKey(Convert.ToInt32(deviceInfo.RackID)))
									{
										RackInfo rackInfo3 = (RackInfo)rackCache[Convert.ToInt32(deviceInfo.RackID)];
										rackInfo3.setNameFlag(rackFullNameflag3);
										dataRow3["parentname"] = rackInfo3.RackName;
									}
									dataTable.Rows.Add(dataRow3);
								}
								goto IL_92D;
							}
							finally
							{
								IDisposable disposable2 = enumerator3 as IDisposable;
								if (disposable2 != null)
								{
									disposable2.Dispose();
								}
							}
							IL_473:
							deviceCache = DBCache.GetDeviceCache();
							rackCache = DBCache.GetRackCache();
							groupDestIDMap = DBCache.GetGroupDestIDMap();
							if (deviceCache == null || deviceCache.Count <= 0 || rackCache == null || rackCache.Count <= 0 || groupDestIDMap == null || !groupDestIDMap.ContainsKey(l_gid))
							{
								goto IL_92D;
							}
							int rackFullNameflag4 = Sys_Para.GetRackFullNameflag();
							List<long> list2 = (List<long>)groupDestIDMap[l_gid];
							using (List<long>.Enumerator enumerator4 = list2.GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									long current2 = enumerator4.Current;
									if (deviceCache.ContainsKey(Convert.ToInt32(current2)))
									{
										DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[Convert.ToInt32(current2)];
										DataRow dataRow4 = dataTable.NewRow();
										dataRow4["memberid"] = deviceInfo2.DeviceID;
										dataRow4["name"] = deviceInfo2.DeviceName;
										if (rackCache.ContainsKey(Convert.ToInt32(deviceInfo2.RackID)))
										{
											RackInfo rackInfo4 = (RackInfo)rackCache[Convert.ToInt32(deviceInfo2.RackID)];
											rackInfo4.setNameFlag(rackFullNameflag4);
											dataRow4["parentname"] = rackInfo4.RackName;
										}
										dataTable.Rows.Add(dataRow4);
									}
								}
								goto IL_92D;
							}
							IL_5E1:
							deviceCache = DBCache.GetDeviceCache();
							Hashtable portCache = DBCache.GetPortCache();
							if (deviceCache == null || deviceCache.Count <= 0 || portCache == null || portCache.Count <= 0)
							{
								goto IL_92D;
							}
							ICollection values3 = portCache.Values;
							IEnumerator enumerator5 = values3.GetEnumerator();
							try
							{
								while (enumerator5.MoveNext())
								{
									PortInfo portInfo = (PortInfo)enumerator5.Current;
									DataRow dataRow5 = dataTable.NewRow();
									dataRow5["memberid"] = portInfo.ID;
									dataRow5["name"] = portInfo.PortName;
									if (deviceCache.ContainsKey(portInfo.DeviceID))
									{
										DeviceInfo deviceInfo3 = (DeviceInfo)deviceCache[portInfo.DeviceID];
										dataRow5["parentname"] = deviceInfo3.DeviceName;
									}
									dataTable.Rows.Add(dataRow5);
								}
								goto IL_92D;
							}
							finally
							{
								IDisposable disposable3 = enumerator5 as IDisposable;
								if (disposable3 != null)
								{
									disposable3.Dispose();
								}
							}
							IL_6E4:
							deviceCache = DBCache.GetDeviceCache();
							portCache = DBCache.GetPortCache();
							groupDestIDMap = DBCache.GetGroupDestIDMap();
							if (deviceCache == null || deviceCache.Count <= 0 || portCache == null || portCache.Count <= 0 || groupDestIDMap == null || !groupDestIDMap.ContainsKey(l_gid))
							{
								goto IL_92D;
							}
							List<long> list3 = (List<long>)groupDestIDMap[l_gid];
							using (List<long>.Enumerator enumerator6 = list3.GetEnumerator())
							{
								while (enumerator6.MoveNext())
								{
									long current3 = enumerator6.Current;
									if (portCache.ContainsKey(Convert.ToInt32(current3)))
									{
										PortInfo portInfo2 = (PortInfo)portCache[Convert.ToInt32(current3)];
										DataRow dataRow6 = dataTable.NewRow();
										dataRow6["memberid"] = portInfo2.ID;
										dataRow6["name"] = portInfo2.PortName;
										if (deviceCache.ContainsKey(portInfo2.DeviceID))
										{
											DeviceInfo deviceInfo4 = (DeviceInfo)deviceCache[portInfo2.DeviceID];
											dataRow6["parentname"] = deviceInfo4.DeviceName;
										}
										dataTable.Rows.Add(dataRow6);
									}
								}
								goto IL_92D;
							}
							IL_838:
							Hashtable zoneCache = DBCache.GetZoneCache();
							groupDestIDMap = DBCache.GetGroupDestIDMap();
							if (zoneCache != null && zoneCache.Count > 0 && groupDestIDMap != null && groupDestIDMap.ContainsKey(l_gid))
							{
								List<long> list4 = (List<long>)groupDestIDMap[l_gid];
								foreach (long current4 in list4)
								{
									if (zoneCache.ContainsKey(current4))
									{
										ZoneInfo zoneInfo = (ZoneInfo)zoneCache[current4];
										DataRow dataRow7 = dataTable.NewRow();
										dataRow7["memberid"] = zoneInfo.ZoneID;
										dataRow7["name"] = zoneInfo.ZoneName;
										dataRow7["parentname"] = "";
										dataTable.Rows.Add(dataRow7);
									}
								}
							}
						}
					}
				}
				IL_92D:;
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
			}
			dataTable = new DataView(dataTable)
			{
				Sort = "name ASC"
			}.ToTable();
			return dataTable;
		}
		public static int CreateNewGroup(string str_name, string str_type, string str_color, List<long> l_member)
		{
			int num = -1;
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getConnection();
				if (dBConn.con != null)
				{
					DbTransaction transaction = DBConn.GetTransaction(dBConn.con);
					dbCommand = DBConn.GetCommandObject(dBConn.con);
					dbCommand.CommandType = CommandType.Text;
					dbCommand.Transaction = transaction;
					if (DBUrl.SERVERMODE)
					{
						string commandText = string.Concat(new object[]
						{
							"insert into data_group (groupname,grouptype,linecolor,isselect,thermalflag,billflag) values (?groupname,?grouptype,?linecolor,?isselect,",
							0,
							",",
							0,
							") "
						});
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("?groupname", str_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?grouptype", str_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?linecolor", str_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("?isselect", 0, dbCommand));
					}
					else
					{
						string commandText = string.Concat(new object[]
						{
							"insert into data_group (groupname,grouptype,linecolor,isselect,thermalflag,billflag) values (?,?,?,?,",
							0,
							",",
							0,
							") "
						});
						dbCommand.CommandText = commandText;
						dbCommand.Parameters.Add(DBTools.GetParameter("@groupname", str_name, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@grouptype", str_type, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@linecolor", str_color, dbCommand));
						dbCommand.Parameters.Add(DBTools.GetParameter("@isselect", 0, dbCommand));
					}
					num = dbCommand.ExecuteNonQuery();
					if (num < 0)
					{
						dbCommand.Transaction.Rollback();
						int result = num;
						return result;
					}
					if (DBUrl.SERVERMODE)
					{
						dbCommand.CommandText = "SELECT LAST_INSERT_ID()";
					}
					else
					{
						dbCommand.CommandText = "SELECT @@IDENTITY";
					}
					dbCommand.Parameters.Clear();
					int num2 = Convert.ToInt32(dbCommand.ExecuteScalar());
					if (num2 < 1)
					{
						dbCommand.Transaction.Rollback();
						int result = -1;
						return result;
					}
					if (l_member != null && l_member.Count > 0)
					{
						foreach (long current in l_member)
						{
							dbCommand.Parameters.Clear();
							dbCommand.CommandType = CommandType.Text;
							if (DBUrl.SERVERMODE)
							{
								dbCommand.CommandText = "insert into group_detail (group_id,grouptype,dest_id) values (?group_id,?grouptype,?dest_id)";
								dbCommand.Parameters.Add(DBTools.GetParameter("?group_id", num2, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?grouptype", str_type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("?dest_id", current, dbCommand));
							}
							else
							{
								dbCommand.CommandText = "insert into group_detail (group_id,grouptype,dest_id) values (?,?,?)";
								dbCommand.Parameters.Add(DBTools.GetParameter("@group_id", num2, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@grouptype", str_type, dbCommand));
								dbCommand.Parameters.Add(DBTools.GetParameter("@dest_id", current, dbCommand));
							}
							num = dbCommand.ExecuteNonQuery();
							if (num < 0)
							{
								dbCommand.Transaction.Rollback();
								int result = num;
								return result;
							}
							dbCommand.Parameters.Clear();
						}
					}
					dbCommand.Transaction.Commit();
					DBCacheStatus.Group = true;
					DBCacheStatus.DBSyncEventSet(true, new string[]
					{
						"DBSyncEventName_Service_Group"
					});
				}
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					dbCommand.Transaction.Rollback();
				}
				catch
				{
				}
				if (ex.Message.IndexOf(" duplicate values ") > 0)
				{
					int result = -2;
					return result;
				}
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
					dBConn.close();
				}
			}
			return num;
		}
	}
}
