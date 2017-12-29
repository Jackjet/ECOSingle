using System;
namespace EcoDevice.AccessAPI
{
	public class LineThreshold
	{
		private int index = 1;
		private float minCurrentMT = -500f;
		private float maxCurrentMT = -500f;
		private float minVoltageMT = -500f;
		private float maxVoltageMT = -500f;
		private float minPowerMT = -500f;
		private float maxPowerMT = -500f;
		public int LineNumber
		{
			get
			{
				return this.index;
			}
		}
		public float MaxPowerMT
		{
			get
			{
				return this.maxPowerMT;
			}
			set
			{
				this.maxPowerMT = value;
			}
		}
		public float MinPowerMT
		{
			get
			{
				return this.minPowerMT;
			}
			set
			{
				this.minPowerMT = value;
			}
		}
		public float MinVoltageMT
		{
			get
			{
				return this.minVoltageMT;
			}
			set
			{
				this.minVoltageMT = value;
			}
		}
		public float MaxVoltageMT
		{
			get
			{
				return this.maxVoltageMT;
			}
			set
			{
				this.maxVoltageMT = value;
			}
		}
		public float MinCurrentMt
		{
			get
			{
				return this.minCurrentMT;
			}
			set
			{
				this.minCurrentMT = value;
			}
		}
		public float MaxCurrentMT
		{
			get
			{
				return this.maxCurrentMT;
			}
			set
			{
				this.maxCurrentMT = value;
			}
		}
		public LineThreshold(int index)
		{
			this.index = index;
		}
	}
}
