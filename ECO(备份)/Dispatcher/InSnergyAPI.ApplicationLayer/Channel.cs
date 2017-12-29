using System;
using System.Collections.Generic;
using System.Linq;
namespace InSnergyAPI.ApplicationLayer
{
	public class Channel
	{
		public string sCID;
		public string loadType;
		public Dictionary<string, Param> measurePair;
		public Channel DeepClone()
		{
			Channel channel = new Channel(this.sCID);
			channel.loadType = this.loadType;
			channel.measurePair = new Dictionary<string, Param>();
			foreach (KeyValuePair<string, Param> current in this.measurePair)
			{
				channel.measurePair.Add(current.Key, current.Value.DeepClone());
			}
			return channel;
		}
		public Channel(string cid)
		{
			this.sCID = cid;
			this.measurePair = new Dictionary<string, Param>();
		}
		public Param GetValue(string attrib)
		{
			if (this.measurePair.ContainsKey(attrib))
			{
				return this.measurePair[attrib];
			}
			return null;
		}
		public int UpdateValuePair(string attrib_list, DateTime time, string load_type)
		{
			this.loadType = load_type;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, Param> current in this.measurePair)
			{
				list.Add(current.Key);
			}
			string[] array = attrib_list.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Count<string>(); i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'_'
				});
				if (array2.Length >= 2)
				{
					if (ApplicationHandler.listParamEnabled != null)
					{
						int item = Convert.ToInt32(array2[0]);
						if (!ApplicationHandler.listParamEnabled.Contains(item))
						{
							goto IL_118;
						}
					}
					if (this.measurePair.ContainsKey(array2[0]))
					{
						this.measurePair[array2[0]].Update(Convert.ToDouble(array2[1]), time, "");
						list.Remove(array2[0]);
					}
					else
					{
						Param value = new Param(array2[0], Convert.ToDouble(array2[1]), time, "");
						this.measurePair.Add(array2[0], value);
					}
				}
				IL_118:;
			}
			foreach (string current2 in list)
			{
				this.measurePair.Remove(current2);
			}
			return this.measurePair.Count;
		}
	}
}
