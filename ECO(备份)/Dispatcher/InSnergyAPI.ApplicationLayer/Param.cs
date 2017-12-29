using System;
namespace InSnergyAPI.ApplicationLayer
{
	public class Param
	{
		public const int TIMEOUT_CHANGE_COLOR = 1000;
		public string aID;
		public double dvalue;
		public string svalue;
		public bool bChanged;
		public int lastChanged;
		public DateTime time;
		public Param DeepClone()
		{
			return new Param(this.aID, this.dvalue, this.time, this.svalue)
			{
				bChanged = this.bChanged,
				lastChanged = this.lastChanged
			};
		}
		public Param(string aid, double d, DateTime t, string s)
		{
			this.aID = aid;
			this.dvalue = d;
			this.time = t;
			this.svalue = s;
			this.bChanged = true;
			this.lastChanged = Environment.TickCount;
		}
		public void Update(double v1, DateTime t, string v2)
		{
			if (v1 != this.dvalue || v2 != this.svalue)
			{
				this.bChanged = true;
				this.lastChanged = Environment.TickCount;
			}
			this.dvalue = v1;
			this.svalue = v2;
			this.time = t;
		}
		public bool IsChanged()
		{
			return this.bChanged;
		}
		public void ResetChange()
		{
			this.bChanged = false;
		}
		public bool IsHighlight()
		{
			if (this.lastChanged != 0)
			{
				int num = Environment.TickCount;
				if (num > this.lastChanged)
				{
					num -= this.lastChanged;
					if (num < 1000)
					{
						return true;
					}
				}
			}
			this.lastChanged = 0;
			return false;
		}
	}
}
