using System;
namespace EcoDevice.AccessAPI
{
	public struct stru_CommRange
	{
		public string type;
		public string id;
		public string range;
		public void copy(stru_CommRange src)
		{
			this.type = src.type;
			this.id = src.id;
			this.range = src.range;
		}
	}
}
