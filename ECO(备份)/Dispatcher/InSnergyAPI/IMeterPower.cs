using System;
namespace InSnergyAPI
{
	public class IMeterPower
	{
		public string sMid;
		public double value;
		public DateTime timestamp;
		public IMeterPower DeepClone()
		{
			return new IMeterPower(this.sMid, this.value, this.timestamp);
		}
		public IMeterPower(string mid, double v, DateTime t)
		{
			this.sMid = mid;
			this.value = v;
			this.timestamp = t;
		}
	}
}
