using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
namespace CommonAPI.ThreadWrapper
{
	public class SystemLargeThreadPool<Bean, Result> : SystemThreadPool<Bean, Result>
	{
		private List<Bean> beans = new List<Bean>();
		private List<Result> result = new List<Result>();
		private ManualResetEvent doneEvent;
		private int decrementIndex;
		private HandleThread handler;
		public SystemLargeThreadPool(List<Bean> beans)
		{
			this.beans = beans;
		}
		public List<Result> GetResults(HandleThread handler)
		{
			if (this.beans == null || this.beans.Count < 1)
			{
				return this.result;
			}
			this.decrementIndex = this.beans.Count;
			this.handler = handler;
			this.doneEvent = new ManualResetEvent(false);
			foreach (Bean current in this.beans)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoWork), current);
			}
			this.doneEvent.WaitOne();
			return this.result;
		}
		private void DoWork(object obj)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			try
			{
				this.handler(this.result, obj);
			}
			catch (Exception)
			{
			}
			finally
			{
				if (Interlocked.Decrement(ref this.decrementIndex) == 0)
				{
					this.doneEvent.Set();
				}
			}
		}
	}
}
