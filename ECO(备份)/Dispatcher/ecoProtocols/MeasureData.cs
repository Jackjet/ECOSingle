using System;
namespace ecoProtocols
{
	public class MeasureData
	{
		public byte type;
		public long id;
		public string value_list;
		public string time;
		public MeasureData()
		{
			this.type = 0;
			this.id = 0L;
			this.value_list = "";
			this.time = "";
		}
	}
}
