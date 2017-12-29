using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class SystemSmallThreadPool<Bean, Result> : SystemThreadPool<Bean, Result>
	{
		private class ThreadPoolHandler : ThreadPoolHandlerTemplate
		{
			private System.Threading.EventWaitHandle eventWaitHandle;
			private HandleThread handler;
			private System.Collections.ICollection collection;
			public ThreadPoolHandler(HandleThread handler, System.Collections.ICollection collection, System.Threading.EventWaitHandle mre)
			{
				this.eventWaitHandle = mre;
				this.collection = collection;
				this.handler = handler;
			}
			public override void handle(object obj)
			{
				this.handler(this.collection, obj);
				this.eventWaitHandle.Set();
			}
		}
		private System.Collections.Generic.List<Bean> beans = new System.Collections.Generic.List<Bean>();
		private System.Collections.Generic.List<Result> result = new System.Collections.Generic.List<Result>();
		public SystemSmallThreadPool(System.Collections.Generic.List<Bean> beans)
		{
			this.beans = beans;
		}
		public System.Collections.Generic.List<Result> GetResults(HandleThread handler)
		{
			if (handler == null)
			{
				throw new System.Exception("The instance of HandleThread is null.");
			}
			if (this.beans == null || this.beans.Count < 1)
			{
				return this.result;
			}
			if (this.beans.Count > 64)
			{
				throw new System.Exception("The count of " + typeof(Bean) + " is more than 64.");
			}
			System.Threading.ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum, ThreadUtil.CompletionPortNum);
			System.Threading.EventWaitHandle[] eventWaitHandle = ThreadUtil.GetEventWaitHandle(this.beans.Count);
			int num = 0;
			foreach (Bean current in this.beans)
			{
				eventWaitHandle[num] = new System.Threading.ManualResetEvent(false);
				ThreadPoolHandlerTemplate @object = new SystemSmallThreadPool<Bean, Result>.ThreadPoolHandler(handler, this.result, eventWaitHandle[num]);
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(@object.handle), current);
				num++;
			}
			System.Threading.WaitHandle.WaitAll(eventWaitHandle);
			return this.result;
		}
	}
}
