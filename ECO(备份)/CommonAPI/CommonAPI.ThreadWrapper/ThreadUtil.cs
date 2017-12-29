using System;
using System.Threading;
namespace CommonAPI.ThreadWrapper
{
	public class ThreadUtil
	{
		public static int WorkThreadNum = 60;
		public static int CompletionPortNum = ThreadUtil.WorkThreadNum * 4;
		private ThreadUtil()
		{
		}
		public static EventWaitHandle[] GetEventWaitHandle(int taskNum)
		{
			if (taskNum <= ThreadUtil.WorkThreadNum / 2)
			{
				return new ManualResetEvent[taskNum];
			}
			return new ManualResetEvent[ThreadUtil.WorkThreadNum];
		}
		public static void InitThreadPool()
		{
			ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum, ThreadUtil.CompletionPortNum);
		}
	}
}
