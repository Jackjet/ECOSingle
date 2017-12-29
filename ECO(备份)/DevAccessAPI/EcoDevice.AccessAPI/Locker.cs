using System;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class Locker
	{
		private const int waitTime = 100;
		private bool isLock;
		public void TryLock()
		{
			while (this.isLock)
			{
				System.Threading.Thread.Sleep(100);
			}
			this.isLock = true;
		}
		public void UnLock()
		{
			this.isLock = false;
		}
	}
}
