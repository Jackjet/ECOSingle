using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class SystemLargeThreadPool<Bean, Result> : SystemThreadPool<Bean, Result>
	{
		private System.Collections.Generic.List<Bean> beans = new System.Collections.Generic.List<Bean>();
		private System.Collections.Generic.List<Result> result = new System.Collections.Generic.List<Result>();
		private System.Threading.ManualResetEvent doneEvent;
		private int decrementIndex;
		private HandleThread handler;
		public SystemLargeThreadPool(System.Collections.Generic.List<Bean> beans)
		{
			this.beans = beans;
		}
		public System.Collections.Generic.List<Result> GetResults(HandleThread handler)
		{
			if (this.beans == null || this.beans.Count < 1)
			{
				return this.result;
			}
			this.decrementIndex = this.beans.Count;
			this.handler = handler;
			this.doneEvent = new System.Threading.ManualResetEvent(false);
			foreach (Bean current in this.beans)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.DoWork), current);
			}
			this.doneEvent.WaitOne();
			return this.result;
		}
		private void DoWork(object obj)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			try
			{
				this.handler(this.result, obj);
			}
			catch (System.Exception)
			{
			}
			finally
			{
				if (System.Threading.Interlocked.Decrement(ref this.decrementIndex) == 0)
				{
					this.doneEvent.Set();
				}
			}
		}
	}
}
