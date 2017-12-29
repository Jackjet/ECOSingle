using System;
using System.Data;
using System.Threading;
namespace Packing
{
	public class TrunkToTable
	{
		public string[] separators;
		public DataTable dt;
		public byte[] data;
		public int nextReadPos;
		public int nEndPos;
		public ManualResetEvent evtDone;
		public TrunkToTable()
		{
			this.dt = null;
			this.data = null;
			this.nEndPos = 0;
			this.nextReadPos = 0;
			this.evtDone = null;
		}
	}
}
