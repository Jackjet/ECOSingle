using System;
namespace ecoProtocols
{
	public class DispatchAttribute
	{
		public const int DATASET_MONITOR = 1;
		public const int DATASET_THRESHOLD = 2;
		public const int DATASET_RACKINFO = 4;
		public const int DATASET_UAC = 8;
		public const int DATASET_RESERVED = 16;
		public const int DATASET_SYS_PARAM = 32;
		public const int DATASET_DEV_INCR = 64;
		public const int DATASET_BASE_INCR = 128;
		public const int DATASET_ZONE_INFO = 256;
		public const int DATASET_GROUP_INFO = 512;
		public const int DATASET_REMOTE_CALL = 1024;
		public const int DATASET_AUTO_MODEL = 2048;
		public const int DATASET_PUE = 4096;
		public const int DATASET_BROADCAST = 8192;
		public const int DATASET_PULL = 16384;
		public const int COMPRESS_NONE = 0;
		public const int COMPRESS_GZIP = 1;
		public const int COMPRESS_LZ4 = 2;
		public int uid;
		public int vid;
		public int type;
		public int algorithm;
		public long cid;
		public byte[] data;
		public DispatchCallback cbCallBack;
		public DispatchCallback cbCacheProcess;
		public IConnectInterface owner;
		public int block_no;
		public int dispatchPointer = -1;
		public int reloadDB = 1;
		public string request;
		public long tStart;
		public int discard_if_jam;
		public string guid;
		public string alltype;
		public string attached;
		public string operation;
		public object _msgHandler;
		public DispatchAttribute getCopy()
		{
			DispatchAttribute dispatchAttribute = new DispatchAttribute();
			if (dispatchAttribute != null)
			{
				dispatchAttribute.uid = this.uid;
				dispatchAttribute.vid = this.vid;
				dispatchAttribute.type = this.type;
				dispatchAttribute.algorithm = this.algorithm;
				dispatchAttribute.data = this.data;
				dispatchAttribute.cbCallBack = this.cbCallBack;
				dispatchAttribute.cbCacheProcess = this.cbCacheProcess;
				dispatchAttribute.owner = this.owner;
				dispatchAttribute.block_no = 0;
				dispatchAttribute.dispatchPointer = -1;
				dispatchAttribute.request = this.request;
				dispatchAttribute.tStart = (long)Environment.TickCount;
				dispatchAttribute.reloadDB = 1;
				dispatchAttribute.discard_if_jam = this.discard_if_jam;
				dispatchAttribute.cid = this.cid;
				dispatchAttribute.guid = this.guid;
				dispatchAttribute.alltype = this.alltype;
				dispatchAttribute.attached = this.attached;
				dispatchAttribute.operation = this.operation;
				dispatchAttribute._msgHandler = this._msgHandler;
			}
			return dispatchAttribute;
		}
		public DispatchAttribute()
		{
			this.uid = 0;
			this.vid = 0;
			this.type = 0;
			this.cid = 0L;
			this.algorithm = 1;
			this.data = null;
			this.cbCallBack = null;
			this.cbCacheProcess = null;
			this.owner = null;
			this.block_no = 0;
			this.dispatchPointer = -1;
			this.request = "";
			this.reloadDB = 1;
			this.discard_if_jam = 0;
			this.guid = "";
			this.alltype = "";
			this.attached = "";
			this.operation = "";
			this.tStart = (long)Environment.TickCount;
			this._msgHandler = null;
		}
		public DispatchAttribute(DispatchAttribute attrib)
		{
			this.uid = attrib.uid;
			this.vid = attrib.vid;
			this.cid = attrib.cid;
			this.type = attrib.type;
			this.algorithm = attrib.algorithm;
			this.data = attrib.data;
			this.cbCallBack = attrib.cbCallBack;
			this.cbCacheProcess = attrib.cbCacheProcess;
			this.reloadDB = attrib.reloadDB;
			this.discard_if_jam = attrib.discard_if_jam;
			this.guid = attrib.guid;
			this.alltype = attrib.alltype;
			this.attached = attrib.attached;
			this.operation = attrib.operation;
			this.owner = null;
			this._msgHandler = attrib._msgHandler;
			this.block_no = 0;
			this.dispatchPointer = -1;
			this.request = "";
			this.tStart = (long)Environment.TickCount;
		}
	}
}
