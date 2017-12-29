using System;
using System.Collections.Generic;
namespace InSnergyAPI
{
	public class IMeter
	{
		public string sCid;
		public Dictionary<int, IParam> listParam = new Dictionary<int, IParam>();
		public IMeter DeepClone()
		{
			IMeter meter = new IMeter(this.sCid);
			meter.listParam = new Dictionary<int, IParam>();
			foreach (KeyValuePair<int, IParam> current in this.listParam)
			{
				meter.listParam.Add(current.Key, current.Value.DeepClone());
			}
			return meter;
		}
		public IMeter(string cid)
		{
			this.sCid = cid;
		}
		public void Update(IMeter channel)
		{
			if (this.sCid != channel.sCid)
			{
				return;
			}
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, IParam> current in this.listParam)
			{
				list.Add(current.Key);
			}
			foreach (KeyValuePair<int, IParam> current2 in channel.listParam)
			{
				if (this.listParam.ContainsKey(current2.Key))
				{
					list.Remove(current2.Key);
					this.listParam[current2.Key].Update(current2.Value);
				}
				else
				{
					this.listParam.Add(current2.Key, current2.Value);
				}
			}
			foreach (int current3 in list)
			{
				this.listParam.Remove(current3);
			}
		}
	}
}
