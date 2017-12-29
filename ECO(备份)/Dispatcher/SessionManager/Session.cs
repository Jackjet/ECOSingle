using System;
using System.Net.Sockets;
namespace SessionManager
{
	public class Session
	{
		public DateTime _tLoginTime;
		public DateTime _tRenewTime;
		public DateTime _Delay2Delete;
		public long _tLastUpdateTime;
		public int _timeoutSeconds;
		public bool _enableTimeout;
		public bool _logout;
		public bool _killed;
		public bool _service_stop;
		public string _userName;
		public string _userPassword;
		public int _userType;
		public string _serialno;
		public string _master;
		public string _remote;
		public bool _trial;
		public int _remaining_days;
		public long _uid;
		public string _remoteIP;
		public Socket _sock;
		public long _nRenewCount;
		public string _Cookie;
		public UserAccessRights _userRights;
		public UpdateTracker _updateTracker;
		public Session DeepCopy()
		{
			Session session = (Session)base.MemberwiseClone();
			session._userRights = this._userRights.getClone();
			return session;
		}
		public Session(string user, string password, string serialno, string cookie, bool enableTO, int nTimeout)
		{
			this._tLoginTime = DateTime.Now;
			this._tRenewTime = DateTime.Now;
			this._tLastUpdateTime = (long)Environment.TickCount;
			this._timeoutSeconds = nTimeout;
			this._enableTimeout = enableTO;
			this._userName = user;
			this._userPassword = password;
			this._userType = 1;
			this._serialno = serialno;
			this._master = "";
			this._remote = "";
			this._logout = false;
			this._killed = false;
			this._service_stop = false;
			this._uid = -1L;
			this._remoteIP = "";
			this._Cookie = cookie;
			this._Delay2Delete = DateTime.MinValue;
			this._nRenewCount = 0L;
			this._userRights = new UserAccessRights();
			this._updateTracker = new UpdateTracker();
		}
	}
}
