using System;
using System.Globalization;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class ThreadCreator
	{
		private ThreadCreator()
		{
		}
		public static System.Threading.Thread StartThread(System.Threading.ThreadStart start, bool isBackground)
		{
			System.Threading.Thread thread = new System.Threading.Thread(start);
			thread.IsBackground = isBackground;
			thread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			thread.Start();
			return thread;
		}
		public static System.Threading.Thread StartThread(System.Threading.ParameterizedThreadStart start, object obj, bool isBackground)
		{
			System.Threading.Thread thread = new System.Threading.Thread(start);
			thread.IsBackground = isBackground;
			thread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
			thread.Start(obj);
			return thread;
		}
	}
}
