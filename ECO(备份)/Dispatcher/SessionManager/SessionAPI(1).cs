using DBAccessAPI;
using DBAccessAPI.user;
using Dispatcher;
using ecoProtocols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;
namespace SessionManager
{
	public static class SessionAPI
	{
		private struct ColumnDefine
		{
			public string colName;
			public string colType;
			public ColumnDefine(string name, string type)
			{
				this.colName = name;
				this.colType = type;
			}
		}
		public static readonly string session_uid = "uid";
		public static readonly string session_user = "user_name";
		public static readonly string session_ip = "user_ip";
		public static readonly string session_stype = "system_type";
		public static readonly string session_ctype = "client_type";
		public static readonly string session_login = "login_time";
		public static readonly string session_sn = "serial_no";
		public static string _sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static int SESSION_LIMIT = 8;
		private static object _lockSession = new object();
		private static string _threadName = "Session Manager";
		private static Thread _thread = null;
		private static ManualResetEvent _stoppingEvent = new ManualResetEvent(false);
		private static ManualResetEvent _stoppedEvent = new ManualResetEvent(true);
		private static WaitHandle[] _waitHandles;
		private static ManualResetEvent _eventMessage;
		private static Queue<string> _queueMessage;
		private static long _next_uid;
		private static Dictionary<long, Session> _sessions;
		private static readonly SessionAPI.ColumnDefine[] colSessions;
		public static bool StartSessionManager()
		{
			Common.WriteLine("StartSessionManager", new string[0]);
			lock (SessionAPI._lockSession)
			{
				SessionAPI._stoppingEvent.Reset();
				SessionAPI._stoppedEvent.Reset();
				SessionAPI._eventMessage.Reset();
				SessionAPI._waitHandles[0] = SessionAPI._stoppingEvent;
				SessionAPI._waitHandles[1] = SessionAPI._eventMessage;
				SessionAPI._queueMessage.Clear();
				SessionAPI._next_uid = 1L;
				SessionAPI._thread = new Thread(new ParameterizedThreadStart(SessionAPI.SessionThread));
				SessionAPI._thread.Name = SessionAPI._threadName;
				SessionAPI._thread.CurrentCulture = CultureInfo.InvariantCulture;
				SessionAPI._thread.IsBackground = true;
				SessionAPI._thread.Start();
			}
			return true;
		}
		public static void StopSessionManager()
		{
			Common.WriteLine("Stopping Session Manager " + SessionAPI._threadName + " thread", new string[0]);
			SessionAPI._stoppingEvent.Set();
			try
			{
				if (!SessionAPI._stoppedEvent.WaitOne(1000))
				{
					Common.WriteLine("Abort a dead " + SessionAPI._threadName + " thread", new string[0]);
					SessionAPI._thread.Abort();
				}
				SessionAPI._thread.Join(500);
			}
			catch (Exception ex)
			{
				Common.WriteLine("StopSessionManager: " + ex.Message, new string[0]);
			}
			Common.WriteLine(SessionAPI._threadName + " stopped", new string[0]);
		}
		public static string getRandomString(int nMaxLength, bool bNumber, bool bSymbol, bool bLowercase, bool bUppercase)
		{
			if (nMaxLength == 0)
			{
				return "";
			}
			ArrayList arrayList = new ArrayList();
			if (bNumber)
			{
				arrayList.Add("0123456789");
			}
			if (bSymbol)
			{
				arrayList.Add("~!@#$%^&*()_+{}[]|/?;:<>,.");
			}
			if (bLowercase)
			{
				arrayList.Add("abcdefghijklmnopqrstuvwxyz");
			}
			if (bUppercase)
			{
				arrayList.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
			}
			if (arrayList.Count == 0)
			{
				return "";
			}
			int i = 0;
			string text = "";
			Random random = new Random(DateTime.Now.Millisecond);
			while (i < nMaxLength)
			{
				int index = random.Next() % arrayList.Count;
				string text2 = (string)arrayList[index];
				int index2 = random.Next() % text2.Length;
				text += text2[index2];
				i++;
			}
			return text;
		}
		public static int getSessionCount()
		{
			int count;
			lock (SessionAPI._lockSession)
			{
				count = SessionAPI._sessions.Count;
			}
			return count;
		}
		public static DataTable getAllSessions()
		{
			DataTable dataTable = new DataTable();
			dataTable.TableName = "Sessions";
			lock (SessionAPI._lockSession)
			{
				try
				{
					SessionAPI.ColumnDefine[] array = SessionAPI.colSessions;
					for (int i = 0; i < array.Length; i++)
					{
						SessionAPI.ColumnDefine columnDefine = array[i];
						DataColumn dataColumn = new DataColumn();
						dataColumn.DataType = Type.GetType(columnDefine.colType);
						dataColumn.ColumnName = columnDefine.colName;
						dataTable.Columns.Add(dataColumn);
					}
					dataTable.PrimaryKey = new DataColumn[]
					{
						dataTable.Columns[SessionAPI.session_uid]
					};
					foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
					{
						try
						{
							if (!string.IsNullOrEmpty(current.Value._remote))
							{
								string arg_DC_0 = current.Value._remote;
								if (current.Value._remote.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
								{
									DataRow dataRow = dataTable.NewRow();
									dataRow[SessionAPI.session_uid] = current.Value._uid;
									dataRow[SessionAPI.session_user] = current.Value._userName;
									dataRow[SessionAPI.session_ip] = current.Value._remoteIP;
									dataRow[SessionAPI.session_stype] = current.Value._master;
									dataRow[SessionAPI.session_ctype] = current.Value._remote;
									dataRow[SessionAPI.session_login] = current.Value._tLoginTime;
									dataRow[SessionAPI.session_sn] = current.Value._serialno;
									dataTable.Rows.Add(dataRow);
								}
							}
						}
						catch (Exception ex)
						{
							Common.WriteLine("Error in AddSessions: {0}", new string[]
							{
								ex.Message
							});
						}
					}
				}
				catch (Exception ex2)
				{
					Common.WriteLine("Error in AddSessions: {0}", new string[]
					{
						ex2.Message
					});
				}
			}
			Common.WriteLine("    Get All Sessions: {0}", new string[]
			{
				dataTable.Rows.Count.ToString()
			});
			return dataTable;
		}
		public static Session getSessionByCookie(string cookie)
		{
			Session result = null;
			lock (SessionAPI._lockSession)
			{
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					try
					{
						if (!string.IsNullOrEmpty(current.Value._Cookie))
						{
							if (cookie.Equals(current.Value._Cookie))
							{
								result = current.Value;
								break;
							}
						}
					}
					catch (Exception ex)
					{
						Common.WriteLine("Error in getSessionByCookie(): {0}", new string[]
						{
							ex.Message
						});
					}
				}
			}
			return result;
		}
		public static void ServiceWillStop()
		{
			lock (SessionAPI._lockSession)
			{
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					current.Value._service_stop = true;
				}
			}
		}
		public static bool KillSessions(int fromUID, string listToKill)
		{
			List<long> list = new List<long>();
			List<Socket> list2 = new List<Socket>();
			List<long> list3 = new List<long>();
			lock (SessionAPI._lockSession)
			{
				try
				{
					if (SessionAPI._sessions.ContainsKey((long)fromUID))
					{
						string arg_43_0 = SessionAPI._sessions[(long)fromUID]._userName;
					}
					Common.WriteLine("    Killing sessions: {0}", new string[]
					{
						listToKill
					});
					string[] array = listToKill.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = array[i].Trim();
						if (!string.IsNullOrEmpty(array[i]))
						{
							list.Add((long)Convert.ToInt32(array[i]));
						}
					}
					foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
					{
						if (list.Contains(current.Value._uid))
						{
							current.Value._killed = true;
							if (current.Value._sock.Connected && current.Value._remote.Equals("remote", StringComparison.InvariantCultureIgnoreCase))
							{
								try
								{
									list2.Add(current.Value._sock);
									list3.Add(current.Value._uid);
								}
								catch (Exception)
								{
								}
							}
							Common.WriteLine("Session killed: user={0}, uid={1}", new string[]
							{
								current.Value._userName,
								current.Value._uid.ToString()
							});
							current.Value._remote.Equals("remote", StringComparison.InvariantCultureIgnoreCase);
						}
					}
					using (List<long>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int num = (int)enumerator2.Current;
							SessionAPI._sessions.Remove((long)num);
						}
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("Error in KillSessions: {0}", new string[]
					{
						ex.Message
					});
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				DispatchAPI.SendUrgency(list2[j], (int)list3[j], "kick");
			}
			return true;
		}
		public static int GetRemoteSessionCount()
		{
			int num = 0;
			int result;
			lock (SessionAPI._lockSession)
			{
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					current.Value._master.Equals("master", StringComparison.InvariantCultureIgnoreCase);
					if (current.Value._remote.Equals("remote", StringComparison.InvariantCultureIgnoreCase))
					{
						num++;
					}
				}
				result = num;
			}
			return result;
		}
		private static void SessionThread(object state)
		{
			Common.WriteLine(SessionAPI._threadName + " thread started", new string[0]);
			while (true)
			{
				int num = WaitHandle.WaitAny(SessionAPI._waitHandles, 500);
				if (num == 258)
				{
					SessionAPI.IdleProcessing();
				}
				else
				{
					if (num == 0)
					{
						break;
					}
					if (num == 1)
					{
						string text = null;
						lock (SessionAPI._lockSession)
						{
							if (SessionAPI._queueMessage.Count > 0)
							{
								text = SessionAPI._queueMessage.Dequeue();
							}
							else
							{
								SessionAPI._eventMessage.Reset();
							}
						}
						if (text != null)
						{
							SessionAPI.Processing(text);
						}
					}
				}
			}
			Common.WriteLine("[" + SessionAPI._threadName + "] thread End", new string[0]);
			SessionAPI._stoppedEvent.Set();
		}
		private static void IdleProcessing()
		{
			lock (SessionAPI._lockSession)
			{
				List<long> list = new List<long>();
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					if (!current.Value._remote.Equals("remote", StringComparison.InvariantCultureIgnoreCase) && current.Value._enableTimeout)
					{
						long num = Common.ElapsedTime(current.Value._tLastUpdateTime);
						if (num > (long)(current.Value._timeoutSeconds * 1000))
						{
							if (current.Value._sock.Connected)
							{
								current.Value._sock.Shutdown(SocketShutdown.Both);
								current.Value._sock.Disconnect(false);
							}
							Common.WriteLine("Session timeout: user={0}, uid={1}", new string[]
							{
								current.Value._userName,
								current.Value._uid.ToString()
							});
							current.Value._remote.Equals("remote", StringComparison.InvariantCultureIgnoreCase);
							list.Add(current.Key);
						}
					}
					if (current.Value._Delay2Delete != DateTime.MinValue)
					{
						TimeSpan timeSpan = DateTime.Now - current.Value._Delay2Delete;
						if (timeSpan.TotalMilliseconds < 0.0 || timeSpan.TotalMilliseconds > 10000.0)
						{
							if (current.Value._sock.Connected)
							{
								current.Value._sock.Shutdown(SocketShutdown.Both);
								current.Value._sock.Disconnect(false);
							}
							list.Add(current.Key);
						}
					}
				}
				using (List<long>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int num2 = (int)enumerator2.Current;
						SessionAPI._sessions.Remove((long)num2);
					}
				}
			}
		}
		private static void Processing(string msg)
		{
			lock (SessionAPI._lockSession)
			{
			}
		}
		private static long CheckSerialNo(string serialno, int snType)
		{
			return 0L;
		}
		public static long getUID(string user, string password, string serial_no, string client_cookie, string master, string remote, Socket sock)
		{
			long result;
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI.GetRemoteSessionCount() >= SessionAPI.SESSION_LIMIT)
				{
					result = -7L;
				}
				else
				{
					string text = serial_no.Trim();
					foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
					{
						if (text.Equals(current.Value._serialno.Trim(), StringComparison.InvariantCultureIgnoreCase) && (string.IsNullOrEmpty(client_cookie) || !current.Value._Cookie.Equals(client_cookie.Trim())))
						{
							result = -6L;
							return result;
						}
					}
					bool trial = false;
					int num = 0;
					long num2 = -1L;
					if (!string.IsNullOrEmpty(serial_no))
					{
						if (remote.Equals("Local", StringComparison.InvariantCultureIgnoreCase))
						{
							num2 = SessionAPI.CheckSerialNo(serial_no, 1);
						}
						else
						{
							num2 = SessionAPI.CheckSerialNo(serial_no, 2);
						}
					}
					if (num2 == 1L)
					{
						trial = true;
						num = ExpiryCheck.isOverdue();
						if (num <= 0)
						{
							result = -8L;
							return result;
						}
					}
					else
					{
						if (num2 != 0L)
						{
							result = -5L;
							return result;
						}
					}
					bool flag2 = true;
					Session session = null;
					if (!string.IsNullOrEmpty(client_cookie))
					{
						session = SessionAPI.getSessionByCookie(client_cookie.Trim());
					}
					if (session == null)
					{
						flag2 = false;
						string randomString = SessionAPI.getRandomString(32, true, false, false, true);
						session = new Session(user, password, serial_no, randomString, true, 180);
						session._uid = SessionAPI._next_uid;
						UserInfo userByName = UserMaintain.getUserByName(user);
						session._userType = userByName.UserType;
						Interlocked.Increment(ref SessionAPI._next_uid);
					}
					session._sock = sock;
					session._remoteIP = sock.RemoteEndPoint.ToString();
					int num3 = session._remoteIP.IndexOf(":");
					if (num3 >= 0)
					{
						session._remoteIP = session._remoteIP.Substring(0, num3);
					}
					session._serialno = serial_no.Trim();
					session._master = master;
					session._remote = remote;
					session._trial = trial;
					session._remaining_days = num;
					session._tRenewTime = DateTime.Now;
					session._Delay2Delete = DateTime.MinValue;
					if (flag2)
					{
						session._nRenewCount += 1L;
						Common.WriteLine("Renew an existing session for user: {0}, uid={1}, cookie={2}, retry={3}", new string[]
						{
							user,
							session._uid.ToString(),
							session._Cookie,
							session._nRenewCount.ToString()
						});
					}
					else
					{
						DispatchAttribute dispatchAttribute = new DispatchAttribute();
						dispatchAttribute.type |= 8;
						dispatchAttribute.type |= 1;
						dispatchAttribute.type |= 64;
						dispatchAttribute.type |= 2;
						dispatchAttribute.type |= 128;
						dispatchAttribute.type |= 32;
						dispatchAttribute.type |= 512;
						dispatchAttribute.type |= 256;
						dispatchAttribute.type |= 4;
						dispatchAttribute.type |= 2048;
						dispatchAttribute.type |= 4096;
						dispatchAttribute.uid = (int)session._uid;
						session._updateTracker.AddBindings(dispatchAttribute);
						SessionAPI._sessions.Add(session._uid, session);
						Common.WriteLine("Created a new session for user: {0}, uid={1}, cookie={2}", new string[]
						{
							user,
							session._uid.ToString(),
							session._Cookie
						});
					}
					result = session._uid;
				}
			}
			return result;
		}
		public static void Logout(long uid, long vid)
		{
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					SessionAPI._sessions[uid]._logout = true;
				}
			}
		}
		public static bool isTrial(long uid)
		{
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					return SessionAPI._sessions[uid]._trial;
				}
			}
			return false;
		}
		public static int getRemainingDays(long uid)
		{
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					return SessionAPI._sessions[uid]._remaining_days;
				}
			}
			return 30;
		}
		private static void UpdateBaseRight(UserAccessRights auth, string userName, long _uid, long usrerType, string devices, string groups)
		{
			auth._userName = userName;
			auth._userID = _uid;
			auth._usrerType = usrerType;
			auth._DeviceList = devices;
			auth._GroupList = groups;
		}
		private static void AppendDeviceMembers(Hashtable ht_devCache, long did, UserAccessRights auth)
		{
			try
			{
				if (ht_devCache != null && ht_devCache.ContainsKey((int)did))
				{
					if (auth._authDevicePortList.ContainsKey(did))
					{
						auth._authDevicePortList[did].Clear();
					}
					else
					{
						List<long> value = new List<long>();
						auth._authDevicePortList.Add(did, value);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendDeviceMembers: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		private static void AppendRackMembers(Hashtable ht_rackDeviceMap, long rid, UserAccessRights auth)
		{
			try
			{
				List<long> list = new List<long>();
				if (ht_rackDeviceMap != null && ht_rackDeviceMap.ContainsKey((int)rid))
				{
					List<int> list2 = (List<int>)ht_rackDeviceMap[(int)rid];
					if (list2 != null && list2.Count > 0)
					{
						Hashtable deviceCache = DBCache.GetDeviceCache();
						foreach (int current in list2)
						{
							SessionAPI.AppendDeviceMembers(deviceCache, (long)current, auth);
							list.Add((long)current);
						}
					}
				}
				if (!auth._authRackDeviceList.ContainsKey(rid))
				{
					auth._authRackDeviceList.Add(rid, list);
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendRackMembers: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		private static void AppendZoneMembers(Hashtable ht_zoneDetail, long zid, UserAccessRights auth)
		{
			try
			{
				if (ht_zoneDetail != null && ht_zoneDetail.ContainsKey(zid))
				{
					ZoneInfo zoneInfo = (ZoneInfo)ht_zoneDetail[zid];
					string rackInfo = zoneInfo.RackInfo;
					if (rackInfo != null && rackInfo.Length > 0)
					{
						string[] separator = new string[]
						{
							","
						};
						string[] array = rackInfo.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (array != null && array.Length > 0)
						{
							Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string value = array2[i];
								long rid = (long)Convert.ToInt32(value);
								SessionAPI.AppendRackMembers(rackDeviceMap, rid, auth);
							}
						}
					}
					if (!auth._authZoneList.ContainsKey(zid))
					{
						auth._authZoneList.Add(zid, zid);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendZoneMembers: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		private static void AppendPort(Hashtable ht_ports, long pid, UserAccessRights auth)
		{
			try
			{
				if (ht_ports != null && ht_ports.ContainsKey((int)pid))
				{
					PortInfo portInfo = (PortInfo)ht_ports[(int)pid];
					if (auth._authDevicePortList.ContainsKey((long)portInfo.DeviceID))
					{
						if (auth._authDevicePortList[(long)portInfo.DeviceID].Count > 0 && !auth._authDevicePortList[(long)portInfo.DeviceID].Contains(pid))
						{
							auth._authDevicePortList[(long)portInfo.DeviceID].Add(pid);
						}
					}
					else
					{
						List<long> list = new List<long>();
						list.Add(pid);
						auth._authDevicePortList.Add((long)portInfo.DeviceID, list);
					}
				}
				if (!auth._authPortList.ContainsKey(pid))
				{
					auth._authPortList.Add(pid, pid);
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendPort: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		private static void AppendGroupMembers(long gid, UserAccessRights auth)
		{
			try
			{
				Common.WriteLine("AppendGroupMembers: gid={0}", new string[]
				{
					gid.ToString()
				});
				GroupInfo groupByID = GroupInfo.GetGroupByID(gid);
				if (groupByID != null)
				{
					string members = groupByID.Members;
					if (!string.IsNullOrEmpty(members))
					{
						int num = 0;
						string[] array = members.Split(new char[]
						{
							','
						}, StringSplitOptions.RemoveEmptyEntries);
						long[] array2 = new long[array.Length];
						string[] array3 = array;
						for (int i = 0; i < array3.Length; i++)
						{
							string value = array3[i];
							long num2 = (long)Convert.ToInt32(value);
							array2[num++] = num2;
						}
						if (array2 == null || num <= 0)
						{
							return;
						}
						if (groupByID.GroupType.Equals("zone"))
						{
							Common.WriteLine("Adding Zone Group ...", new string[0]);
							Hashtable zoneCache = DBCache.GetZoneCache();
							long[] array4 = array2;
							for (int j = 0; j < array4.Length; j++)
							{
								long zid = array4[j];
								SessionAPI.AppendZoneMembers(zoneCache, zid, auth);
							}
						}
						else
						{
							if (groupByID.GroupType.Equals("rack") || groupByID.GroupType.Equals("allrack"))
							{
								Common.WriteLine("Adding Rack Group ...", new string[0]);
								Hashtable rackDeviceMap = DBCache.GetRackDeviceMap();
								long[] array5 = array2;
								for (int k = 0; k < array5.Length; k++)
								{
									long rid = array5[k];
									SessionAPI.AppendRackMembers(rackDeviceMap, rid, auth);
								}
							}
							else
							{
								if (groupByID.GroupType.Equals("dev") || groupByID.GroupType.Equals("alldev"))
								{
									Common.WriteLine("Adding Device Group ...", new string[0]);
									Hashtable deviceCache = DBCache.GetDeviceCache();
									long[] array6 = array2;
									for (int l = 0; l < array6.Length; l++)
									{
										long did = array6[l];
										SessionAPI.AppendDeviceMembers(deviceCache, did, auth);
									}
								}
								else
								{
									if (groupByID.GroupType.Equals("outlet") || groupByID.GroupType.Equals("alloutlet"))
									{
										Common.WriteLine("Adding Port Group ...", new string[0]);
										Hashtable portCache = DBCache.GetPortCache();
										long[] array7 = array2;
										for (int m = 0; m < array7.Length; m++)
										{
											long pid = array7[m];
											SessionAPI.AppendPort(portCache, pid, auth);
										}
									}
								}
							}
						}
					}
					if (!auth._authGroupList.ContainsKey(gid))
					{
						auth._authGroupList.Add(gid, gid);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendGroupMembers: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		private static void AppendRackByDevices(UserAccessRights auth)
		{
			try
			{
				Common.WriteLine("AppendRackByDevices ...", new string[0]);
				foreach (long current in auth._authDevicePortList.Keys)
				{
					Hashtable deviceCache = DBCache.GetDeviceCache();
					if (deviceCache != null && deviceCache.ContainsKey((int)current))
					{
						DeviceInfo deviceInfo = (DeviceInfo)deviceCache[(int)current];
						long rackID = deviceInfo.RackID;
						if (!auth._authRackDeviceList.ContainsKey(rackID))
						{
							List<long> list = new List<long>();
							list.Add(current);
							auth._authRackDeviceList.Add(rackID, list);
						}
						else
						{
							if (!auth._authRackDeviceList[rackID].Contains(current))
							{
								auth._authRackDeviceList[rackID].Add(current);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("AppendRackByDevices: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		public static void Authorized(string userName, long uid, long userType, string groups, string devices)
		{
			lock (SessionAPI._lockSession)
			{
				Common.WriteLine("Authorized for: user={0}, uid={1}", new string[]
				{
					userName,
					uid.ToString()
				});
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					Common.WriteLine("Building UAC for: user={0}, uid={1}", new string[]
					{
						userName,
						uid.ToString()
					});
					SessionAPI.UpdateBaseRight(SessionAPI._sessions[uid]._userRights, userName, uid, userType, devices, groups);
					string[] separator = new string[]
					{
						","
					};
					string[] array = groups.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array != null && array.Length > 0)
					{
						Common.WriteLine("Building Group UAC for: user={0}, uid={1}", new string[]
						{
							userName,
							uid.ToString()
						});
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string value = array2[i];
							long gid = (long)Convert.ToInt32(value);
							SessionAPI.AppendGroupMembers(gid, SessionAPI._sessions[uid]._userRights);
						}
					}
					string[] array3 = devices.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array3 != null && array3.Length > 0)
					{
						Common.WriteLine("Building Device UAC for: user={0}, uid={1}", new string[]
						{
							userName,
							uid.ToString()
						});
						Hashtable deviceCache = DBCache.GetDeviceCache();
						string[] array4 = array3;
						for (int j = 0; j < array4.Length; j++)
						{
							string value2 = array4[j];
							long did = (long)Convert.ToInt32(value2);
							SessionAPI.AppendDeviceMembers(deviceCache, did, SessionAPI._sessions[uid]._userRights);
						}
					}
					Common.WriteLine("Check Racks UAC by Device for: user={0}, uid={1}", new string[]
					{
						userName,
						uid.ToString()
					});
					SessionAPI.AppendRackByDevices(SessionAPI._sessions[uid]._userRights);
					Common.WriteLine("Build UAC done for: user={0}, uid={1}", new string[]
					{
						userName,
						uid.ToString()
					});
				}
			}
		}
		public static void AuthorizationUpdate(string userName)
		{
			lock (SessionAPI._lockSession)
			{
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					if (1 == current.Value._userType && (string.IsNullOrEmpty(userName) || userName == current.Value._userName))
					{
						UserAccessRights clone = current.Value._userRights.getClone();
						current.Value._userRights.Clear();
						UserInfo userByName = UserMaintain.getUserByName(current.Value._userName);
						string userDevice = userByName.UserDevice;
						string userGroup = userByName.UserGroup;
						SessionAPI.UpdateBaseRight(current.Value._userRights, current.Value._userName, current.Value._uid, (long)current.Value._userType, userDevice, userGroup);
						string[] separator = new string[]
						{
							","
						};
						string[] array = userGroup.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (array != null && array.Length > 0)
						{
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string value = array2[i];
								long gid = (long)Convert.ToInt32(value);
								SessionAPI.AppendGroupMembers(gid, current.Value._userRights);
							}
						}
						string[] array3 = userDevice.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (array3 != null && array3.Length > 0)
						{
							Hashtable deviceCache = DBCache.GetDeviceCache();
							string[] array4 = array3;
							for (int j = 0; j < array4.Length; j++)
							{
								string value2 = array4[j];
								long did = (long)Convert.ToInt32(value2);
								SessionAPI.AppendDeviceMembers(deviceCache, did, current.Value._userRights);
							}
						}
						SessionAPI.AppendRackByDevices(current.Value._userRights);
						uint num = SessionAPI.CheckChangedUAC(current.Value._userRights, clone);
						if (num != 0u)
						{
							current.Value._updateTracker._modifiedSinceLastRequest |= num;
						}
						Common.WriteLine("Authorization reloaded for: user={0}, uid={1}", new string[]
						{
							userName,
							current.Key.ToString()
						});
					}
				}
			}
		}
		public static uint CheckChangedUAC(UserAccessRights lastUAC, UserAccessRights nextUAC)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (nextUAC._authGroupList.Count != lastUAC._authGroupList.Count)
			{
				flag = true;
			}
			else
			{
				foreach (KeyValuePair<long, long> current in nextUAC._authGroupList)
				{
					if (!lastUAC._authGroupList.ContainsKey(current.Key))
					{
						flag = true;
						break;
					}
				}
			}
			if (nextUAC._authZoneList.Count != lastUAC._authZoneList.Count)
			{
				flag2 = true;
			}
			else
			{
				foreach (KeyValuePair<long, long> current2 in nextUAC._authZoneList)
				{
					if (!lastUAC._authZoneList.ContainsKey(current2.Key))
					{
						flag2 = true;
						break;
					}
				}
			}
			if (nextUAC._authPortList.Count != lastUAC._authPortList.Count)
			{
				flag4 = true;
			}
			else
			{
				foreach (KeyValuePair<long, long> current3 in nextUAC._authPortList)
				{
					if (!lastUAC._authPortList.ContainsKey(current3.Key))
					{
						flag4 = true;
						break;
					}
				}
			}
			if (nextUAC._authRackDeviceList.Count != lastUAC._authRackDeviceList.Count)
			{
				flag3 = true;
			}
			else
			{
				foreach (KeyValuePair<long, List<long>> current4 in nextUAC._authRackDeviceList)
				{
					if (!lastUAC._authRackDeviceList.ContainsKey(current4.Key))
					{
						flag3 = true;
						break;
					}
					if (lastUAC._authRackDeviceList[current4.Key].Count != nextUAC._authRackDeviceList[current4.Key].Count)
					{
						flag3 = true;
						break;
					}
					foreach (long current5 in nextUAC._authRackDeviceList[current4.Key])
					{
						if (!lastUAC._authRackDeviceList[current4.Key].Contains(current5))
						{
							flag3 = true;
							break;
						}
					}
					if (flag3)
					{
						break;
					}
				}
			}
			if (nextUAC._authDevicePortList.Count != lastUAC._authDevicePortList.Count)
			{
				flag5 = true;
			}
			else
			{
				foreach (KeyValuePair<long, List<long>> current6 in nextUAC._authDevicePortList)
				{
					if (!lastUAC._authDevicePortList.ContainsKey(current6.Key))
					{
						flag5 = true;
						break;
					}
					foreach (long current7 in nextUAC._authDevicePortList[current6.Key])
					{
						if (!lastUAC._authDevicePortList[current6.Key].Contains(current7))
						{
							flag5 = true;
							break;
						}
					}
					if (flag5)
					{
						break;
					}
				}
			}
			uint num = 0u;
			if (flag)
			{
				num |= 512u;
			}
			if (flag2)
			{
				num |= 256u;
			}
			if (flag3)
			{
				num |= 4u;
			}
			if (flag4 || flag5)
			{
				num |= 2u;
				num |= 1u;
			}
			Common.WriteLine("Changed types by UAC, user:{0}, type={1}", new string[]
			{
				nextUAC._userName,
				num.ToString("X8")
			});
			return num;
		}
		public static bool IsUserUAC(long uid)
		{
			bool result = false;
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = (1 == SessionAPI._sessions[uid]._userType);
				}
			}
			return result;
		}
		public static void toTracker(DispatchAttribute attrib)
		{
			lock (SessionAPI._lockSession)
			{
				foreach (KeyValuePair<long, Session> current in SessionAPI._sessions)
				{
					if ((long)attrib.uid == current.Value._uid && 1 == current.Value._userType)
					{
						if ((attrib.type & 8) != 0)
						{
							current.Value._updateTracker._modifiedSinceLastRequest |= 8u;
							if ((attrib.type & 2) != 0)
							{
								current.Value._updateTracker._modifiedSinceLastRequest |= 1u;
							}
							attrib.type |= (int)current.Value._updateTracker._modifiedSinceLastRequest;
						}
						current.Value._updateTracker.AddBindings(attrib);
						Common.WriteLine("toTracker: type={0}, uid={1}", new string[]
						{
							attrib.type.ToString("X8"),
							attrib.uid.ToString()
						});
					}
				}
			}
		}
		public static Dictionary<long, List<long>> getDeviceListClone(long uid)
		{
			Dictionary<long, List<long>> result = new Dictionary<long, List<long>>();
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = SessionAPI._sessions[uid]._userRights.getDeviceListClone();
					return result;
				}
			}
			return result;
		}
		public static UpdateTracker getTrackerClone(long uid, bool bReset)
		{
			UpdateTracker result;
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					UpdateTracker clone = SessionAPI._sessions[uid]._updateTracker.getClone(bReset);
					result = clone;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public static UserAccessRights getUserRights(long uid)
		{
			UserAccessRights userAccessRights = null;
			UserAccessRights result;
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					userAccessRights = SessionAPI._sessions[uid]._userRights.getClone();
				}
				result = userAccessRights;
			}
			return result;
		}
		public static string getUser(long uid)
		{
			string result = "";
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = SessionAPI._sessions[uid]._userName;
				}
			}
			return result;
		}
		public static string getCookie(long uid)
		{
			string result = "";
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = SessionAPI._sessions[uid]._Cookie;
				}
			}
			return result;
		}
		public static string getRemoteIP(long uid)
		{
			string result = "";
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = SessionAPI._sessions[uid]._remoteIP;
				}
			}
			return result;
		}
		public static string getRemoteType(long uid)
		{
			string result = "";
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					result = SessionAPI._sessions[uid]._remote;
				}
			}
			return result;
		}
		public static void Update(long uid)
		{
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					SessionAPI._sessions[uid]._tLastUpdateTime = (long)Environment.TickCount;
				}
			}
		}
		public static void Delete(long uid, bool bSilent = false)
		{
			lock (SessionAPI._lockSession)
			{
				if (SessionAPI._sessions.ContainsKey(uid))
				{
					if (!bSilent)
					{
						if (!SessionAPI._sessions[uid]._logout && !SessionAPI._sessions[uid]._killed && !SessionAPI._sessions[uid]._service_stop)
						{
							SessionAPI._sessions[uid]._Delay2Delete = DateTime.Now;
							Common.WriteLine("Deletion delayed: {0}, uid={1}", new string[]
							{
								SessionAPI._sessions[uid]._userName,
								SessionAPI._sessions[uid]._uid.ToString()
							});
							return;
						}
						Common.WriteLine("Close a session for user: {0}, uid={1}", new string[]
						{
							SessionAPI._sessions[uid]._userName,
							SessionAPI._sessions[uid]._uid.ToString()
						});
					}
					SessionAPI._sessions.Remove(uid);
				}
			}
		}
		static SessionAPI()
		{
			// Note: this type is marked as 'beforefieldinit'.
			WaitHandle[] waitHandles = new WaitHandle[2];
			SessionAPI._waitHandles = waitHandles;
			SessionAPI._eventMessage = new ManualResetEvent(false);
			SessionAPI._queueMessage = new Queue<string>();
			SessionAPI._next_uid = 1L;
			SessionAPI._sessions = new Dictionary<long, Session>();
			SessionAPI.colSessions = new SessionAPI.ColumnDefine[]
			{
				new SessionAPI.ColumnDefine(SessionAPI.session_uid, "System.Int32"),
				new SessionAPI.ColumnDefine(SessionAPI.session_user, "System.String"),
				new SessionAPI.ColumnDefine(SessionAPI.session_ip, "System.String"),
				new SessionAPI.ColumnDefine(SessionAPI.session_stype, "System.String"),
				new SessionAPI.ColumnDefine(SessionAPI.session_ctype, "System.String"),
				new SessionAPI.ColumnDefine(SessionAPI.session_login, "System.DateTime"),
				new SessionAPI.ColumnDefine(SessionAPI.session_sn, "System.String")
			};
		}
	}
}
