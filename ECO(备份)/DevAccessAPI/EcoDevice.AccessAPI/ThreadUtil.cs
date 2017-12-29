using System;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class ThreadUtil
	{
		public static int WorkThreadNum = 60;
		public static int CompletionPortNum = ThreadUtil.WorkThreadNum * 4;
		private ThreadUtil()
		{
		}
		public static System.Threading.EventWaitHandle[] GetEventWaitHandle(int taskNum)
		{
			if (taskNum <= ThreadUtil.WorkThreadNum / 2)
			{
				return new System.Threading.ManualResetEvent[taskNum];
			}
			return new System.Threading.ManualResetEvent[ThreadUtil.WorkThreadNum];
		}
		public static void InitThreadPool()
		{
			System.Threading.ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum, ThreadUtil.CompletionPortNum);
		}
	}
}
