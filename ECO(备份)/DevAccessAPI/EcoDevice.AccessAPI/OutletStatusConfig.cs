using System;
namespace EcoDevice.AccessAPI
{
	public class OutletStatusConfig
	{
		private int index;
		private int status;
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}
		public int OutletIndex
		{
			get
			{
				return this.index;
			}
		}
		public OutletStatusConfig(int index)
		{
			this.index = index;
		}
	}
}
