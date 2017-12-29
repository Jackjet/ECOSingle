using System;
namespace EcoDevice.AccessAPI
{
	public class DevicePOPSettings
	{
		private int popEnableMode;
		private float popThreshold = -500f;
		private int popModeOutlet;
		private int popModeLIFO;
		private int popModePriority;
		private string popPriorityList = "";
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
		public void copyPOPSetings(DeviceThreshold devThreshold)
		{
			this.popThreshold = devThreshold.PopThreshold;
			this.popEnableMode = devThreshold.PopEnableMode;
			this.popThreshold = devThreshold.PopThreshold;
			this.popModeOutlet = devThreshold.PopModeOutlet;
			this.popModeLIFO = devThreshold.PopModeLIFO;
			this.popModePriority = devThreshold.PopModePriority;
			this.popPriorityList = devThreshold.PopPriorityList;
		}
	}
}
