using System;
namespace EcoDevice.AccessAPI
{
	public class TrapV1Message : TrapMessage
	{
		private string enterprise;
		private string genericTrap;
		private string specific;
		private string timeStamp;
		private string community;
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
		public string TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
			set
			{
				this.timeStamp = value;
			}
		}
		public string Specific
		{
			get
			{
				return this.specific;
			}
			set
			{
				this.specific = value;
			}
		}
		public string Enterprise
		{
			get
			{
				return this.enterprise;
			}
			set
			{
				this.enterprise = value;
			}
		}
		public string GenericTrap
		{
			get
			{
				return this.genericTrap;
			}
			set
			{
				this.genericTrap = value;
			}
		}
	}
}
