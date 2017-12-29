using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
namespace InSnergyAPI.ApplicationLayer
{
	public class Gateway
	{
		public GatewayStatus status;
		private object thisGateLock = new object();
		public Dictionary<string, Device> listDevice;
		public Gateway DeepClone()
		{
			Gateway gateway = new Gateway();
			gateway.thisGateLock = new object();
			gateway.status = this.status.DeepClone();
			gateway.listDevice = new Dictionary<string, Device>();
			foreach (KeyValuePair<string, Device> current in this.listDevice)
			{
				gateway.listDevice.Add(current.Key, current.Value.DeepClone());
			}
			return gateway;
		}
		public Gateway()
		{
		}
		public Gateway(string gid, Socket sock, bool managed, int nTZone)
		{
			this.status = new GatewayStatus(gid, sock, managed, nTZone);
			this.listDevice = new Dictionary<string, Device>();
		}
		public void ResetTopology()
		{
			foreach (KeyValuePair<string, Device> current in this.listDevice)
			{
				this.listDevice[current.Key].bListReceived = false;
			}
		}
		public int UpdateDeviceChannelMap(string did, string map, string sid)
		{
			int result = 0;
			lock (this.thisGateLock)
			{
				if (!this.IsDeviceExisted(did))
				{
					result = 0;
				}
				else
				{
					result = this.listDevice[did].UpdateChannelMap(map, sid);
				}
			}
			return result;
		}
		public int UpdateDeviceChannelList(string did, string list, string sid)
		{
			int result = 0;
			lock (this.thisGateLock)
			{
				if (!this.IsDeviceExisted(did))
				{
					result = 0;
				}
				else
				{
					result = this.listDevice[did].UpdateChannelList(list, sid);
				}
			}
			return result;
		}
		public int UpdateDeviceAttributeA01(string device, string attr_list)
		{
			int result = 0;
			lock (this.thisGateLock)
			{
				if (!this.IsDeviceExisted(device))
				{
					result = 0;
				}
				else
				{
					result = this.listDevice[device].UpdateDeviceAttributeA01(attr_list);
				}
			}
			return result;
		}
		public int UpdateDeviceList(string list)
		{
			int result = 0;
			lock (this.thisGateLock)
			{
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, Device> current in this.listDevice)
				{
					list2.Add(current.Key);
				}
				string[] array = list.Split(new char[]
				{
					'/'
				});
				for (int i = 0; i < array.Count<string>(); i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						','
					});
					if (!this.listDevice.ContainsKey(array2[0]))
					{
						Device value = new Device(array2[0], array2[1]);
						this.listDevice.Add(array2[0], value);
					}
					else
					{
						this.listDevice[array2[0]].linkType = array2[1];
						list2.Remove(array2[0]);
					}
				}
				foreach (string current2 in list2)
				{
					this.listDevice.Remove(current2);
				}
				result = array.Count<string>();
			}
			return result;
		}
		public bool IsDeviceExisted(string did)
		{
			bool result = false;
			lock (this.thisGateLock)
			{
				result = this.listDevice.ContainsKey(did);
			}
			return result;
		}
		public bool IsChannelExisted(string did, string ch)
		{
			bool result = false;
			lock (this.thisGateLock)
			{
				result = (this.listDevice.ContainsKey(did) && this.listDevice[did].IsChannelExisted(ch));
			}
			return result;
		}
		public int UpdateDeviceAttributeS16(string did, string attrib_list, string time)
		{
			int result = 0;
			lock (this.thisGateLock)
			{
				if (!this.listDevice.ContainsKey(did))
				{
					result = 0;
				}
				else
				{
					result = this.listDevice[did].UpdateAttributeS16(attrib_list, time);
				}
			}
			return result;
		}
		public bool UpdateDeviceChannelAttribute26(string device, string channel, string attrib_value, string time, string load)
		{
			bool result = false;
			lock (this.thisGateLock)
			{
				if (!this.listDevice.ContainsKey(device))
				{
					result = false;
				}
				else
				{
					this.listDevice[device].UpdateChannelAttribute26(channel, attrib_value, time, load);
					result = true;
				}
			}
			return result;
		}
	}
}
