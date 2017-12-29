using System;
namespace EcoDevice.AccessAPI
{
	public class TrapV3Message : TrapMessage
	{
		private string securityName = string.Empty;
		private string engineId = string.Empty;
		private TrapDesc trapDesc = new TrapDesc();
		public string SecurityName
		{
			get
			{
				return this.securityName;
			}
			set
			{
				this.securityName = value;
			}
		}
		public string EngineId
		{
			get
			{
				return this.engineId;
			}
			set
			{
				this.engineId = value;
			}
		}
		public TrapDesc TrapDesc
		{
			get
			{
				return this.trapDesc;
			}
			set
			{
				this.trapDesc = value;
			}
		}
	}
}
