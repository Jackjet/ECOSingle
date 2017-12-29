using System;
using System.Collections.Generic;
namespace InSnergyAPI
{
	public class IBranch
	{
		public string sDid;
		public Dictionary<string, IMeter> listChannel = new Dictionary<string, IMeter>();
		public IBranch DeepClone()
		{
			IBranch branch = new IBranch(this.sDid);
			branch.listChannel = new Dictionary<string, IMeter>();
			foreach (KeyValuePair<string, IMeter> current in this.listChannel)
			{
				branch.listChannel.Add(current.Key, current.Value.DeepClone());
			}
			return branch;
		}
		public IBranch(string did)
		{
			this.sDid = did;
		}
		public void Update(IBranch device)
		{
			if (this.sDid != device.sDid)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IMeter> current in this.listChannel)
			{
				list.Add(current.Key);
			}
			foreach (KeyValuePair<string, IMeter> current2 in device.listChannel)
			{
				if (this.listChannel.ContainsKey(current2.Key))
				{
					list.Remove(current2.Key);
					this.listChannel[current2.Key].Update(current2.Value);
				}
				else
				{
					this.listChannel.Add(current2.Key, current2.Value);
				}
			}
			foreach (string current3 in list)
			{
				this.listChannel.Remove(current3);
			}
		}
	}
}
