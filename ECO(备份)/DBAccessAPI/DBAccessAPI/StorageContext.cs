using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace DBAccessAPI
{
	public class StorageContext
	{
		public Dictionary<int, string> _deviceVoltageList;
		public Dictionary<int, Dictionary<int, double>> _lastSensorMaxValue;
		public Hashtable _AllData4Access;
		public Hashtable _htExisted;
		public Hashtable _htData;
		public DateTime _tDataTime;
		public string _sDataTime;
		public string _tablePrefix;
		public string _tableSuffix;
		public double _dataRatio;
		public Semaphore _finishSignal;
		public SemaphoreSlim _finishCounter;
		public StorageContext()
		{
			this._htExisted = null;
			this._htData = null;
			this._dataRatio = 1.0;
			this._tDataTime = DateTime.Now;
			this._tablePrefix = "";
			this._tableSuffix = "";
			this._finishSignal = null;
			this._finishCounter = null;
			this._deviceVoltageList = null;
			this._lastSensorMaxValue = null;
			this._AllData4Access = null;
		}
	}
}
