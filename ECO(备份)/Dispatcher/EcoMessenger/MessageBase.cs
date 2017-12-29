using ecoProtocols;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
namespace EcoMessenger
{
	public class MessageBase : IDisposable
	{
		protected object _lockHandler = new object();
		protected object _lockDataset = new object();
		private string _threadName = "Message Processor";
		protected bool _isServer;
		private int _stopping;
		private Thread _procThread;
		private ManualResetEvent _stoppedEvent = new ManualResetEvent(true);
		private ManualResetEvent _abortEvent = new ManualResetEvent(false);
		private WaitHandle[] _waitHandles;
		private ManualResetEvent _eventMessage;
		private Queue<ecoMessage> _queueEcoMessage;
		protected List<IConnectInterface> _lstConnectors;
		public void Dispose()
		{
		}
		public MessageBase()
		{
			WaitHandle[] waitHandles = new WaitHandle[2];
			this._waitHandles = waitHandles;
			this._eventMessage = new ManualResetEvent(false);
			this._queueEcoMessage = new Queue<ecoMessage>();
			this._lstConnectors = new List<IConnectInterface>();
			base..ctor();
			this._isServer = false;
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._queueEcoMessage.Clear();
		}
		public bool Start(bool asServer)
		{
			this._isServer = asServer;
			Interlocked.Exchange(ref this._stopping, 0);
			this._stoppedEvent.Reset();
			this._abortEvent.Reset();
			this._eventMessage.Reset();
			this._waitHandles[0] = this._abortEvent;
			this._waitHandles[1] = this._eventMessage;
			this._queueEcoMessage.Clear();
			this._procThread = new Thread(new ParameterizedThreadStart(this.MessageThread));
			this._procThread.Name = this._threadName;
			this._procThread.CurrentCulture = CultureInfo.InvariantCulture;
			this._procThread.IsBackground = true;
			this._procThread.Start();
			return true;
		}
		public void Stop()
		{
			Common.WriteLine("Stopping Message Processor " + this._threadName + " thread", new string[0]);
			Interlocked.Exchange(ref this._stopping, 1);
			if (this._abortEvent != null)
			{
				this._abortEvent.Set();
			}
			try
			{
				if (!this._stoppedEvent.WaitOne(1000))
				{
					Common.WriteLine("Abort a dead " + this._threadName + " thread", new string[0]);
					this._procThread.Abort();
				}
				this._procThread.Join(500);
			}
			catch (Exception ex)
			{
				Common.WriteLine("Stopping Message base: " + ex.Message, new string[0]);
			}
			Interlocked.Exchange(ref this._stopping, 0);
			Common.WriteLine(this._threadName + " stopped", new string[0]);
		}
		public virtual void PutMesssage(IConnectInterface receiver, object c, int type, ulong header, object message)
		{
			lock (this._lockHandler)
			{
				ecoMessage ecoMessage = new ecoMessage(receiver, c, type, header, message);
				if (ecoMessage != null && this._queueEcoMessage != null)
				{
					this._queueEcoMessage.Enqueue(ecoMessage);
					this._eventMessage.Set();
				}
			}
		}
		private void MessageThread(object state)
		{
			Common.WriteLine(this._threadName + " thread started", new string[0]);
			while (this._stopping == 0)
			{
				int num = WaitHandle.WaitAny(this._waitHandles, 100);
				if (num == 0)
				{
					break;
				}
				ecoMessage ecoMessage = null;
				lock (this._lockHandler)
				{
					if (this._queueEcoMessage.Count > 0)
					{
						ecoMessage = this._queueEcoMessage.Dequeue();
					}
					else
					{
						this._eventMessage.Reset();
					}
				}
				if (ecoMessage != null)
				{
					if (this._isServer)
					{
						this.ServerProcessing(ecoMessage);
					}
					else
					{
						this.ClientProcessing(ecoMessage);
					}
				}
			}
			Common.WriteLine("[" + this._threadName + "] thread end", new string[0]);
			this._stoppedEvent.Set();
		}
		public virtual void OnDBReady()
		{
		}
		public virtual void OnClose(ConnectContext ctx)
		{
		}
		public virtual byte[] BuildMsg4Service(int uid, int vid, string msg)
		{
			return null;
		}
		public virtual byte[] BuildRequest(int uid, int vid, int dataType)
		{
			return null;
		}
		public virtual byte[] BuildLogin(object cfgClient)
		{
			return null;
		}
		public virtual byte[] BuildLogout(int uid, int vid)
		{
			return null;
		}
		public virtual byte[] BuildKeepAlive(int uid, int vid)
		{
			return null;
		}
		public virtual byte[] BuildMessage(int uid, int vid, int type, bool broadcast, byte[] message)
		{
			return null;
		}
		public virtual byte[] BuildBroadcast(DispatchAttribute attr)
		{
			return null;
		}
		public virtual byte[] BuildFirstDispatch(DispatchAttribute attr)
		{
			return null;
		}
		public virtual byte[] BuildNextDispatch(DispatchAttribute attr)
		{
			return null;
		}
		public virtual void AttachDispatcher(IConnectInterface comObj)
		{
		}
		public virtual void DetachDispatcher(IConnectInterface comObj)
		{
		}
		public virtual void ToAllDispatcher(DispatchAttribute attrib)
		{
		}
		public virtual void FirstDispatch(ecoMessage msg)
		{
		}
		protected virtual void ClientProcessing(ecoMessage msg)
		{
		}
		protected virtual void ServerProcessing(ecoMessage msg)
		{
		}
		public virtual void AutoPull4UAC(ConnectContext comObj)
		{
		}
	}
}
