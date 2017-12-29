using System;
namespace EcoDevice.AccessAPI
{
	public struct ThresholdFlg
	{
		public string type;
		public int flg;
		public void copy(ThresholdFlg src)
		{
			this.type = src.type;
			this.flg = src.flg;
		}
	}
}
