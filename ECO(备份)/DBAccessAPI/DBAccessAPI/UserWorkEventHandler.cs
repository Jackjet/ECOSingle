using System;
namespace DBAccessAPI
{
	public delegate void UserWorkEventHandler<T>(object sender, WorkQueue<T>.EnqueueEventArgs e);
}
