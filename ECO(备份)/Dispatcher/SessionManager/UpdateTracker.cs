using ecoProtocols;
using System;
using System.Collections.Generic;
namespace SessionManager
{
	public class UpdateTracker
	{
		public uint _modifiedSinceLastRequest;
		public long _tLastRequest;
		public List<DispatchAttribute> _UpdateBindings;
		public UpdateTracker()
		{
			this._tLastRequest = 0L;
			this._modifiedSinceLastRequest = 0u;
			this._UpdateBindings = new List<DispatchAttribute>();
		}
		public UpdateTracker getClone(bool bReset)
		{
			UpdateTracker updateTracker = new UpdateTracker();
			updateTracker._modifiedSinceLastRequest = this._modifiedSinceLastRequest;
			updateTracker._tLastRequest = this._tLastRequest;
			updateTracker._UpdateBindings = new List<DispatchAttribute>();
			foreach (DispatchAttribute current in this._UpdateBindings)
			{
				updateTracker._UpdateBindings.Add(current.getCopy());
			}
			if (bReset)
			{
				this._modifiedSinceLastRequest = 0u;
				this._tLastRequest = 0L;
				this._UpdateBindings.Clear();
			}
			return updateTracker;
		}
		public void AddBindings(DispatchAttribute attrib)
		{
			this._modifiedSinceLastRequest |= (uint)attrib.type;
			if (!string.IsNullOrEmpty(attrib.operation) || !string.IsNullOrEmpty(attrib.attached) || !string.IsNullOrEmpty(attrib.guid) || !string.IsNullOrEmpty(attrib.alltype))
			{
				this._UpdateBindings.Add(attrib);
			}
		}
	}
}
