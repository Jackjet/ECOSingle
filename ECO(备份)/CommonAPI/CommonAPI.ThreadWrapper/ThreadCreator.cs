using System;
using System.Globalization;
using System.Threading;
namespace CommonAPI.ThreadWrapper
{
	public class ThreadCreator
	{
		private ThreadCreator()
		{
		}
		public static Thread StartThread(ThreadStart start, bool isBackground)
		{
			Thread thread = new Thread(start);
			thread.IsBackground = isBackground;
			thread.CurrentCulture = new CultureInfo("en-US");
			thread.Start();
			return thread;
		}
		public static Thread StartThread(ParameterizedThreadStart start, object obj, bool isBackground)
		{
			Thread thread = new Thread(start);
			thread.IsBackground = isBackground;
			thread.CurrentCulture = new CultureInfo("en-US");
			thread.Start(obj);
			return thread;
		}
	}
}
