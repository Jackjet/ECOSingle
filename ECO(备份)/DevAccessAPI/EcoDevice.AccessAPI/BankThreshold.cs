using System;
namespace EcoDevice.AccessAPI
{
	public class BankThreshold
	{
		private int index = 1;
		private string bankName = string.Empty;
		private float minCurrentMT = -500f;
		private float maxCurrentMT = -500f;
		private float minVoltageMT = -500f;
		private float maxVoltageMT = -500f;
		private float minPowerMT = -500f;
		private float maxPowerMT = -500f;
		private float maxPowerDissMT = -500f;
		public int BankNumber
		{
			get
			{
				return this.index;
			}
		}
		public string BankName
		{
			get
			{
				return this.bankName;
			}
			set
			{
				this.bankName = value;
			}
		}
		public float MaxPowerDissMT
		{
			get
			{
				return this.maxPowerDissMT;
			}
			set
			{
				this.maxPowerDissMT = value;
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
		public BankThreshold(int index)
		{
			this.index = index;
		}
	}
}
