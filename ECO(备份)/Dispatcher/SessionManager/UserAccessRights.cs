using System;
using System.Collections.Generic;
using System.Text;
namespace SessionManager
{
	public class UserAccessRights
	{
		public string _userName;
		public long _userID;
		public long _usrerType;
		public string _DeviceList;
		public string _GroupList;
		public Dictionary<long, long> _authGroupList;
		public Dictionary<long, long> _authZoneList;
		public Dictionary<long, long> _authPortList;
		public Dictionary<long, List<long>> _authRackDeviceList;
		public Dictionary<long, List<long>> _authDevicePortList;
		public UserAccessRights()
		{
			this._userName = "";
			this._userID = -1L;
			this._usrerType = -1L;
			this._DeviceList = "";
			this._GroupList = "";
			this._authGroupList = new Dictionary<long, long>();
			this._authZoneList = new Dictionary<long, long>();
			this._authPortList = new Dictionary<long, long>();
			this._authRackDeviceList = new Dictionary<long, List<long>>();
			this._authDevicePortList = new Dictionary<long, List<long>>();
		}
		public void Clear()
		{
			this._userName = "";
			this._userID = -1L;
			this._usrerType = -1L;
			this._DeviceList = "";
			this._GroupList = "";
			this._authGroupList.Clear();
			this._authZoneList.Clear();
			this._authPortList.Clear();
			this._authRackDeviceList.Clear();
			this._authDevicePortList.Clear();
		}
		public UserAccessRights getClone()
		{
			UserAccessRights userAccessRights = new UserAccessRights();
			userAccessRights._userName = this._userName;
			userAccessRights._userID = this._userID;
			userAccessRights._usrerType = this._usrerType;
			userAccessRights._DeviceList = this._DeviceList;
			userAccessRights._GroupList = this._GroupList;
			userAccessRights._authGroupList = new Dictionary<long, long>();
			foreach (KeyValuePair<long, long> current in this._authGroupList)
			{
				userAccessRights._authGroupList.Add(current.Key, current.Value);
			}
			userAccessRights._authZoneList = new Dictionary<long, long>();
			foreach (KeyValuePair<long, long> current2 in this._authZoneList)
			{
				userAccessRights._authZoneList.Add(current2.Key, current2.Value);
			}
			userAccessRights._authPortList = new Dictionary<long, long>();
			foreach (KeyValuePair<long, long> current3 in this._authPortList)
			{
				userAccessRights._authPortList.Add(current3.Key, current3.Value);
			}
			userAccessRights._authRackDeviceList = this.getRackDeviceListClone();
			userAccessRights._authDevicePortList = this.getDeviceListClone();
			return userAccessRights;
		}
		public Dictionary<long, List<long>> getRackDeviceListClone()
		{
			Dictionary<long, List<long>> dictionary = new Dictionary<long, List<long>>();
			foreach (KeyValuePair<long, List<long>> current in this._authRackDeviceList)
			{
				List<long> portList = new List<long>();
				current.Value.ForEach(delegate(long item)
				{
					portList.Add(item);
				});
				dictionary.Add(current.Key, portList);
			}
			return dictionary;
		}
		public Dictionary<long, List<long>> getDeviceListClone()
		{
			Dictionary<long, List<long>> dictionary = new Dictionary<long, List<long>>();
			foreach (KeyValuePair<long, List<long>> current in this._authDevicePortList)
			{
				List<long> portList = new List<long>();
				current.Value.ForEach(delegate(long item)
				{
					portList.Add(item);
				});
				dictionary.Add(current.Key, portList);
			}
			return dictionary;
		}
		public string getAuthorized(string listType, string strList)
		{
			string[] array = strList.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			int num = 0;
			if (listType.Equals("group", StringComparison.InvariantCultureIgnoreCase))
			{
				num = 1;
			}
			else
			{
				if (listType.Equals("zone", StringComparison.InvariantCultureIgnoreCase))
				{
					num = 2;
				}
				else
				{
					if (listType.Equals("rack", StringComparison.InvariantCultureIgnoreCase))
					{
						num = 3;
					}
					else
					{
						if (listType.Equals("device", StringComparison.InvariantCultureIgnoreCase))
						{
							num = 4;
						}
						else
						{
							if (listType.Equals("port", StringComparison.InvariantCultureIgnoreCase))
							{
								num = 5;
							}
						}
					}
				}
			}
			for (int i = 0; i < array.Length; i++)
			{
				long key = (long)Convert.ToInt32(array[i]);
				switch (num)
				{
				case 1:
					if (!this._authGroupList.ContainsKey(key))
					{
						array[i] = "";
					}
					break;
				case 2:
					if (!this._authZoneList.ContainsKey(key))
					{
						array[i] = "";
					}
					break;
				case 3:
					if (!this._authRackDeviceList.ContainsKey(key))
					{
						array[i] = "";
					}
					break;
				case 4:
					if (!this._authDevicePortList.ContainsKey(key))
					{
						array[i] = "";
					}
					break;
				case 5:
					if (!this._authPortList.ContainsKey(key))
					{
						array[i] = "";
					}
					break;
				default:
					array[i] = "";
					break;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array.Length; j++)
			{
				if (!string.IsNullOrEmpty(array[j]))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(array[j]);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
