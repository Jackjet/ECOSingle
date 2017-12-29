using System;
using System.Collections.Generic;
using System.Linq;
namespace InSnergyAPI.ApplicationLayer
{
	public class Device
	{
		public string sDID;
		public string linkType;
		public string slaveID;
		public int[] mapChannel;
		public bool bListReceived;
		public List<string> duplicatedIDList;
		public Dictionary<string, string> channelIDList;
		public Dictionary<string, Channel> listChannel;
		public Dictionary<string, Param> measurePair;
		public Device DeepClone()
		{
			Device device = new Device(this.sDID, this.linkType);
			device.slaveID = this.slaveID;
			device.bListReceived = this.bListReceived;
			device.mapChannel = new int[this.mapChannel.Length];
			int num = 0;
			int[] array = this.mapChannel;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = array[i];
				device.mapChannel[num++] = num2;
			}
			device.duplicatedIDList = new List<string>();
			foreach (string current in this.duplicatedIDList)
			{
				device.duplicatedIDList.Add(current);
			}
			device.channelIDList = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> current2 in this.channelIDList)
			{
				device.channelIDList.Add(current2.Key, current2.Value);
			}
			device.listChannel = new Dictionary<string, Channel>();
			foreach (KeyValuePair<string, Channel> current3 in this.listChannel)
			{
				device.listChannel.Add(current3.Key, current3.Value.DeepClone());
			}
			device.measurePair = new Dictionary<string, Param>();
			foreach (KeyValuePair<string, Param> current4 in this.measurePair)
			{
				device.measurePair.Add(current4.Key, current4.Value.DeepClone());
			}
			return device;
		}
		public Device(string did, string link)
		{
			int[] array = new int[12];
			this.mapChannel = array;
			base..ctor();
			this.sDID = did;
			this.linkType = link;
			this.bListReceived = false;
			this.duplicatedIDList = new List<string>();
			this.listChannel = new Dictionary<string, Channel>();
			this.measurePair = new Dictionary<string, Param>();
			this.channelIDList = new Dictionary<string, string>();
		}
		public int UpdateChannelMap(string map, string sid)
		{
			this.listChannel.Clear();
			string[] array = map.Split(new char[]
			{
				','
			});
			bool flag = false;
			this.slaveID = sid;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, string> current in this.channelIDList)
			{
				list.Add(current.Key);
			}
			this.bListReceived = true;
			for (int i = 0; i < array.Count<string>(); i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'_'
				});
				if (array2.Length == 3 && (array2[0].Trim() != "0" || array2[1].Trim() != "0" || array2[2].Trim() != "0"))
				{
					Channel value = new Channel((i + 1).ToString());
					this.listChannel.Add((i + 1).ToString(), value);
					if (i < 12)
					{
						this.mapChannel[i] = (Convert.ToInt32(array2[0].Trim()) << 16) + (Convert.ToInt32(array2[1].Trim()) << 8) + Convert.ToInt32(array2[2].Trim());
					}
					string text = (i + 1).ToString();
					while (text.Length < 2)
					{
						text = "0" + text;
					}
					for (int j = 0; j < 3; j++)
					{
						if (Convert.ToInt32(array2[j].Trim()) > 0)
						{
							string text2 = array2[j].Trim();
							while (text2.Length < 2)
							{
								text2 = "0" + text2;
							}
							string text3 = text + "0" + (j + 1).ToString();
							if (this.channelIDList.ContainsKey(text3))
							{
								string text4 = this.channelIDList[text3];
								if (text4 != text2)
								{
									this.channelIDList[text3] = text2;
									if (this.duplicatedIDList.Contains(text3 + text2))
									{
										this.duplicatedIDList.Remove(text3 + text2);
										this.duplicatedIDList.Add(text3 + text4);
									}
									else
									{
										flag = true;
									}
								}
								list.Remove(text3);
							}
							else
							{
								this.channelIDList.Add(text3, text2);
								flag = true;
							}
						}
					}
				}
			}
			foreach (string current2 in list)
			{
				this.channelIDList.Remove(current2);
			}
			if (flag)
			{
				return -1 * this.channelIDList.Count;
			}
			return this.channelIDList.Count;
		}
		public int UpdateChannelList(string list, string sid)
		{
			string[] array = list.Split(new char[]
			{
				','
			});
			this.slaveID = sid;
			this.channelIDList.Clear();
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, Channel> current in this.listChannel)
			{
				list2.Add(current.Key);
			}
			for (int i = 0; i < array.Count<string>(); i++)
			{
				if (!this.listChannel.ContainsKey(array[i]))
				{
					Channel value = new Channel(array[i]);
					this.listChannel.Add(array[i], value);
					string text = array[i];
					while (text.Length < 6)
					{
						text = "0" + text;
					}
					this.channelIDList.Add(text, "");
				}
				else
				{
					list2.Remove(array[i]);
				}
			}
			foreach (string current2 in list2)
			{
				this.listChannel.Remove(current2);
			}
			return array.Count<string>();
		}
		public bool IsChannelExisted(string ch)
		{
			return this.listChannel.ContainsKey(ch);
		}
		public static DateTime GetTime(string timeMeasure)
		{
			DateTime result = DateTime.Now;
			string text = timeMeasure.Trim();
			if (text == "")
			{
				return result;
			}
			char[] anyOf = new char[]
			{
				'-',
				':'
			};
			if (text.IndexOfAny(anyOf) >= 0)
			{
				char[] separator = new char[]
				{
					'-',
					':',
					' '
				};
				string[] array = text.Split(separator);
				if (array.Length != 6)
				{
					return result;
				}
				result = new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), Convert.ToInt32(array[3]), Convert.ToInt32(array[4]), Convert.ToInt32(array[5]));
			}
			else
			{
				DateTime d = new DateTime(1970, 1, 1, 0, 0, 0);
				TimeSpan t = new TimeSpan(0, 0, Convert.ToInt32(text));
				result = d + t;
			}
			return result;
		}
		public int UpdateDeviceAttributeA01(string attrib_list)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Param> current in this.measurePair)
			{
				list.Add(current.Key);
			}
			string[] array = attrib_list.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'_'
				});
				if (array2.Length == 3 && array2[1] == "0")
				{
					if (this.measurePair.ContainsKey(array2[0]))
					{
						this.measurePair[array2[0]].Update(Convert.ToDouble(array2[2]), DateTime.Now, "");
						list.Remove(array2[0]);
					}
					else
					{
						Param value = new Param(array2[0], Convert.ToDouble(array2[2]), DateTime.Now, "");
						this.measurePair.Add(array2[0], value);
					}
				}
			}
			foreach (string current2 in list)
			{
				this.measurePair.Remove(current2);
			}
			return this.measurePair.Count;
		}
		public int UpdateAttributeS16(string attrib_list, string time)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Param> current in this.measurePair)
			{
				list.Add(current.Key);
			}
			string[] array = attrib_list.Split(new char[]
			{
				','
			});
			DateTime time2 = Device.GetTime(time);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'_'
				});
				if (array2.Length == 2)
				{
					if (this.measurePair.ContainsKey(array2[0]))
					{
						this.measurePair[array2[0]].Update(Convert.ToDouble(array2[1]), time2, "");
						list.Remove(array2[0]);
					}
					else
					{
						Param value = new Param(array2[0], Convert.ToDouble(array2[1]), time2, "");
						this.measurePair.Add(array2[0], value);
					}
				}
			}
			foreach (string current2 in list)
			{
				this.measurePair.Remove(current2);
			}
			return this.measurePair.Count;
		}
		public int UpdateChannelAttribute26(string channel, string attrib_value, string time, string load)
		{
			if (!this.listChannel.ContainsKey(channel))
			{
				return 0;
			}
			DateTime time2 = Device.GetTime(time);
			return this.listChannel[channel].UpdateValuePair(attrib_value, time2, load);
		}
	}
}
