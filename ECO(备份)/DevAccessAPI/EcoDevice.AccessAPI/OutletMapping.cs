using System;
namespace EcoDevice.AccessAPI
{
	public class OutletMapping
	{
		private int outletId;
		private int outletNum;
		private string outletName;
		public int OutletNumber
		{
			get
			{
				return this.outletNum;
			}
			set
			{
				this.outletNum = value;
			}
		}
		public int OutletId
		{
			get
			{
				return this.outletId;
			}
			set
			{
				this.outletId = value;
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
		public OutletMapping(int outletNum)
		{
			this.outletNum = outletNum;
		}
	}
}
