using System;
namespace EcoDevice.AccessAPI
{
	public class DeviceThreshold
	{
		private float minCurrentMT = -500f;
		private float maxCurrentMT = -500f;
		private float minVoltageMT = -500f;
		private float maxVoltageMT = -500f;
		private float minPowerMT = -500f;
		private float maxPowerMT = -500f;
		private float maxPowerDissMT = -500f;
		private int popEnableMode;
		private float popThreshold = -500f;
		private int popModeOutlet;
		private int popModeLIFO;
		private int popModePriority;
		private string popPriorityList = "";
		private float devRefVoltage = -500f;
		private int doorSensorType = -500;
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
		public float MinCurrentMT
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
		public float PopThreshold
		{
			get
			{
				return this.popThreshold;
			}
			set
			{
				this.popThreshold = value;
			}
		}
		public int PopEnableMode
		{
			get
			{
				return this.popEnableMode;
			}
			set
			{
				this.popEnableMode = value;
			}
		}
		public int PopModeOutlet
		{
			get
			{
				return this.popModeOutlet;
			}
			set
			{
				this.popModeOutlet = value;
			}
		}
		public int PopModeLIFO
		{
			get
			{
				return this.popModeLIFO;
			}
			set
			{
				this.popModeLIFO = value;
			}
		}
		public int PopModePriority
		{
			get
			{
				return this.popModePriority;
			}
			set
			{
				this.popModePriority = value;
			}
		}
		public string PopPriorityList
		{
			get
			{
				return this.popPriorityList;
			}
			set
			{
				this.popPriorityList = value;
			}
		}
		public float DevReferenceVoltage
		{
			get
			{
				return this.devRefVoltage;
			}
			set
			{
				this.devRefVoltage = value;
			}
		}
		public int DoorSensorType
		{
			get
			{
				return this.doorSensorType;
			}
			set
			{
				this.doorSensorType = value;
			}
		}
	}
}
