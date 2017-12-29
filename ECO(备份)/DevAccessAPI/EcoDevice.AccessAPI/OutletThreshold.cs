using System;
namespace EcoDevice.AccessAPI
{
	public class OutletThreshold
	{
		private int index = 1;
		private string outletName = string.Empty;
		private Confirmation confirmation = Confirmation.No;
		private int onDelayTime;
		private int offDelayTime;
		private ShutDownMethod shutdownMethod = ShutDownMethod.KillThePower;
		private string macAddress = string.Empty;
		private float minCurrentMT = -500f;
		private float maxCurrentMT = -500f;
		private float minVoltageMT = -500f;
		private float maxVoltageMT = -500f;
		private float minPowerMT = -500f;
		private float maxPowerMT = -500f;
		private float maxPowerDissMT = -500f;
		public int OutletNumber
		{
			get
			{
				return this.index;
			}
		}
		public ShutDownMethod ShutdownMethod
		{
			get
			{
				return this.shutdownMethod;
			}
			set
			{
				this.shutdownMethod = value;
			}
		}
		public int OffDelayTime
		{
			get
			{
				return this.offDelayTime;
			}
			set
			{
				this.offDelayTime = value;
			}
		}
		public int OnDelayTime
		{
			get
			{
				return this.onDelayTime;
			}
			set
			{
				this.onDelayTime = value;
			}
		}
		public Confirmation Confirmation
		{
			get
			{
				return this.confirmation;
			}
			set
			{
				this.confirmation = value;
			}
		}
		public string OutletName
		{
			get
			{
				return this.outletName;
			}
			set
			{
				this.outletName = value;
			}
		}
		public string MacAddress
		{
			get
			{
				return this.macAddress;
			}
			set
			{
				this.macAddress = value;
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
		public OutletThreshold(int index)
		{
			this.index = index;
		}
	}
}
