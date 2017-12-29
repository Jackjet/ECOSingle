using System;
namespace CommonAPI.Timers
{
	public class TickTimer
	{
		private bool _running;
		private long _lastTick;
		private object _lock;
		public TickTimer()
		{
			this._lock = new object();
			this._lastTick = (long)Environment.TickCount;
			this._running = false;
		}
		public bool isRunning()
		{
			bool running;
			lock (this._lock)
			{
				running = this._running;
			}
			return running;
		}
		public void Start()
		{
			lock (this._lock)
			{
				this._lastTick = (long)Environment.TickCount;
				this._running = true;
			}
		}
		public void Stop()
		{
			lock (this._lock)
			{
				this._running = false;
			}
		}
		public void Update()
		{
			lock (this._lock)
			{
				if (this._running)
				{
					this._lastTick = (long)Environment.TickCount;
				}
			}
		}
		public long getElapsed()
		{
			long result;
			lock (this._lock)
			{
				long num = (long)Environment.TickCount;
				if (num >= this._lastTick)
				{
					result = num - this._lastTick;
				}
				else
				{
					long num2 = 2147483647L - this._lastTick + 1L;
					num2 += num - -2147483648L;
					result = num2;
				}
			}
			return result;
		}
	}
}
