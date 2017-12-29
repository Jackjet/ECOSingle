using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace CommonAPI.ThreadWrapper
{
	public class SystemSmallThreadPool<Bean, Result> : SystemThreadPool<Bean, Result>
	{
		private class ThreadPoolHandler : ThreadPoolHandlerTemplate
		{
			private EventWaitHandle eventWaitHandle;
			private HandleThread handler;
			private ICollection collection;
			public ThreadPoolHandler(HandleThread handler, ICollection collection, EventWaitHandle mre)
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
		private List<Bean> beans = new List<Bean>();
		private List<Result> result = new List<Result>();
		public SystemSmallThreadPool(List<Bean> beans)
		{
			this.beans = beans;
		}
		public List<Result> GetResults(HandleThread handler)
		{
			if (handler == null)
			{
				throw new Exception("The instance of HandleThread is null.");
			}
			if (this.beans == null || this.beans.Count < 1)
			{
				return this.result;
			}
			if (this.beans.Count > 64)
			{
				throw new Exception("The count of " + typeof(Bean) + " is more than 64.");
			}
			ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum, ThreadUtil.CompletionPortNum);
			EventWaitHandle[] eventWaitHandle = ThreadUtil.GetEventWaitHandle(this.beans.Count);
			int num = 0;
			foreach (Bean current in this.beans)
			{
				eventWaitHandle[num] = new ManualResetEvent(false);
				ThreadPoolHandlerTemplate @object = new SystemSmallThreadPool<Bean, Result>.ThreadPoolHandler(handler, this.result, eventWaitHandle[num]);
				ThreadPool.QueueUserWorkItem(new WaitCallback(@object.handle), current);
				num++;
			}
			WaitHandle.WaitAll(eventWaitHandle);
			return this.result;
		}
	}
}
