using System;
namespace EcoDevice.AccessAPI
{
	public class TrapDesc
	{
		private string trapSysUpTime0 = string.Empty;
		private string trapObjectID0 = string.Empty;
		private string errorIndex = string.Empty;
		private string errorStatus = string.Empty;
		private string errorStatusString = string.Empty;
		public string ErrorStatusString
		{
			get
			{
				return this.errorStatusString;
			}
			set
			{
				this.errorStatusString = value;
			}
		}
		public string ErrorStatus
		{
			get
			{
				return this.errorStatus;
			}
			set
			{
				this.errorStatus = value;
			}
		}
		public string ErrorIndex
		{
			get
			{
				return this.errorIndex;
			}
			set
			{
				this.errorIndex = value;
			}
		}
		public string TrapObjectID
		{
			get
			{
				return this.trapObjectID0;
			}
			set
			{
				this.trapObjectID0 = value;
			}
		}
		public string TrapSysUpTime
		{
			get
			{
				return this.trapSysUpTime0;
			}
			set
			{
				this.trapSysUpTime0 = value;
			}
		}
	}
}
