using System;
using System.Collections.Generic;
namespace ecoProtocols
{
	public class SpeedMeter
	{
		private class OnewaySpeedMeter
		{
			private List<long> _SecondBytes;
			private DateTime _tFirstSecond;
			private DateTime _tLastSecond;
			private int _posLastSecond;
			private object _lock;
			public OnewaySpeedMeter()
			{
				this._lock = new object();
				this._posLastSecond = 0;
				this._tFirstSecond = DateTime.MinValue;
				this._tLastSecond = DateTime.Now;
				this._SecondBytes = new List<long>();
				for (int i = 0; i < 10; i++)
				{
					this._SecondBytes.Add(0L);
				}
			}
			public void addBytes(int nBytes)
			{
				lock (this._lock)
				{
					if (this._tFirstSecond == DateTime.MinValue)
					{
						this._tFirstSecond = DateTime.Now;
					}
					DateTime now = DateTime.Now;
					TimeSpan timeSpan = now - this._tLastSecond;
					if (timeSpan.TotalSeconds < 0.0)
					{
						this._tLastSecond = DateTime.Now;
						timeSpan = DateTime.Now - this._tLastSecond;
					}
					long num = (long)timeSpan.TotalSeconds;
					if (num == 0L)
					{
						List<long> secondBytes;
						int posLastSecond;
						(secondBytes = this._SecondBytes)[posLastSecond = this._posLastSecond] = secondBytes[posLastSecond] + (long)nBytes;
					}
					else
					{
						for (long num2 = 1L; num2 <= num; num2 += 1L)
						{
							this._posLastSecond++;
							if (this._posLastSecond >= 10)
							{
								this._posLastSecond = 0;
							}
							if (num2 == num)
							{
								this._SecondBytes[this._posLastSecond] = (long)nBytes;
							}
							else
							{
								this._SecondBytes[this._posLastSecond] = 0L;
							}
						}
					}
					this._tLastSecond = DateTime.Now;
				}
			}
			public double getSpeed()
			{
				this.addBytes(0);
				double result;
				lock (this._lock)
				{
					double num = 0.0;
					TimeSpan timeSpan = DateTime.Now - this._tFirstSecond;
					if (timeSpan.TotalSeconds < 0.0)
					{
						this._tFirstSecond = DateTime.Now;
						timeSpan = DateTime.Now - this._tFirstSecond;
					}
					long num2 = (long)timeSpan.TotalSeconds;
					if (num2 == 0L)
					{
						result = num;
					}
					else
					{
						foreach (long current in this._SecondBytes)
						{
							num += (double)current;
						}
						if (num2 > 10L)
						{
							num2 = 10L;
						}
						result = num / (double)num2;
					}
				}
				return result;
			}
		}
		private const int CountSeconds = 10;
		private SpeedMeter.OnewaySpeedMeter _send;
		private SpeedMeter.OnewaySpeedMeter _recv;
		public SpeedMeter()
		{
			this._send = new SpeedMeter.OnewaySpeedMeter();
			this._recv = new SpeedMeter.OnewaySpeedMeter();
		}
		public void Received(int nBytes)
		{
			this._recv.addBytes(nBytes);
		}
		public void Sent(int nBytes)
		{
			this._send.addBytes(nBytes);
		}
		public double getSendSpeed()
		{
			return this._send.getSpeed();
		}
		public double getReceiveSpeed()
		{
			return this._recv.getSpeed();
		}
	}
}
