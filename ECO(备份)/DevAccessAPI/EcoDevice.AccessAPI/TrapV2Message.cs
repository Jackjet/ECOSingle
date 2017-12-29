using System;
namespace EcoDevice.AccessAPI
{
	public class TrapV2Message : TrapMessage
	{
		private string community = string.Empty;
		private TrapDesc trapDesc = new TrapDesc();
		public string Community
		{
			get
			{
				return this.community;
			}
			set
			{
				this.community = value;
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
