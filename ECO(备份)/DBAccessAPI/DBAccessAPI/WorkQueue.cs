using CommonAPI;
using System;
using System.Collections.Generic;
using System.Threading;
namespace DBAccessAPI
{
	public class WorkQueue<T>
	{
		public class EnqueueEventArgs : EventArgs
		{
			public T Item
			{
				get;
				private set;
			}
			public EnqueueEventArgs(object item)
			{
				try
				{
					this.Item = (T)((object)item);
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~DBERROR : " + ex.Message + "\n" + ex.StackTrace);
					throw new InvalidCastException("object to T failed");
				}
			}
		}
		private int qsize;
		private static WorkQueue<string> sqlQueue_power;
		private static WorkQueue<string> sqlQueue_pd;
		private static WorkQueue<string> sqlQueue_rackeffect;
		private static DBWorkThread db_power;
		private static DBWorkThread db_pd;
		private static DBWorkThread db_rackeffect;
		private bool IsWorking;
		private object lockIsWorking = new object();
		private DebugCenter debug;
		private string className = "";
		private long l_count;
		private DateTime DT_start;
		private DateTime DT_end;
		private Queue<T> queue;
		private object lockObj = new object();
		private bool isOneThread = true;
		public event UserWorkEventHandler<T> UserWork;
		public bool WorkSequential
		{
			get
			{
				return this.isOneThread;
			}
			set
			{
				this.isOneThread = value;
			}
		}
		public static int CloseDB()
		{
			if (WorkQueue<T>.db_power.CloseDBConnection() > 0 && WorkQueue<T>.db_pd.CloseDBConnection() > 0 && WorkQueue<T>.db_rackeffect.CloseDBConnection() > 0)
			{
				return 1;
			}
			return -1;
		}
		public int getSize()
		{
			return this.qsize;
		}
		public static WorkQueue<string> getInstance()
		{
			if (WorkQueue<T>.sqlQueue_power == null)
			{
				WorkQueue<T>.sqlQueue_power = new WorkQueue<string>(8000, "Power");
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					WorkQueue<T>.db_power = new DBWorkThread(1);
				}
				else
				{
					WorkQueue<T>.db_power = new DBWorkThread(0);
				}
				WorkQueue<T>.sqlQueue_power.UserWork = new UserWorkEventHandler<string>(WorkQueue<T>.db_power.workQueue_DBWork);
			}
			return WorkQueue<T>.sqlQueue_power;
		}
		public static WorkQueue<string> getInstance_pd()
		{
			if (WorkQueue<T>.sqlQueue_pd == null)
			{
				WorkQueue<T>.sqlQueue_pd = new WorkQueue<string>(8000, "PowerDisspation");
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					WorkQueue<T>.db_pd = new DBWorkThread(1);
				}
				else
				{
					WorkQueue<T>.db_pd = new DBWorkThread(0);
				}
				WorkQueue<T>.sqlQueue_pd.UserWork = new UserWorkEventHandler<string>(WorkQueue<T>.db_pd.workQueue_DBWork);
			}
			return WorkQueue<T>.sqlQueue_pd;
		}
		public static WorkQueue<string> getInstance_rackeffect()
		{
			if (WorkQueue<T>.sqlQueue_rackeffect == null)
			{
				WorkQueue<T>.sqlQueue_rackeffect = new WorkQueue<string>(100, "Rack");
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					WorkQueue<T>.db_rackeffect = new DBWorkThread(1);
				}
				else
				{
					WorkQueue<T>.db_rackeffect = new DBWorkThread(0);
				}
				WorkQueue<T>.sqlQueue_rackeffect.UserWork = new UserWorkEventHandler<string>(WorkQueue<T>.db_rackeffect.workQueue_DBWork);
			}
			return WorkQueue<T>.sqlQueue_rackeffect;
		}
		public int CloseDBConnection()
		{
			int num = -1;
			if (WorkQueue<T>.db_power != null)
			{
				num = WorkQueue<T>.db_power.CloseDBConnection();
			}
			if (num > 0 && WorkQueue<T>.db_pd != null)
			{
				num = WorkQueue<T>.db_pd.CloseDBConnection();
			}
			if (num > 0 && WorkQueue<T>.db_rackeffect != null)
			{
				num = WorkQueue<T>.db_rackeffect.CloseDBConnection();
			}
			return num;
		}
		public WorkQueue(int n, string name)
		{
			this.debug = DebugCenter.GetInstance();
			this.className = name;
			this.queue = new Queue<T>(n);
		}
		public WorkQueue()
		{
			this.queue = new Queue<T>();
		}
		public bool IsEmpty()
		{
			bool result;
			lock (this.lockObj)
			{
				result = (this.queue.Count == 0);
			}
			return result;
		}
		public void CleanQueue()
		{
			lock (this.lockObj)
			{
				this.queue.Clear();
				this.qsize = this.queue.Count;
			}
		}
		public void EnqueueItem(T item)
		{
			lock (this.lockObj)
			{
				this.queue.Enqueue(item);
				this.qsize = this.queue.Count;
			}
			lock (this.lockIsWorking)
			{
				if (!this.IsWorking)
				{
					this.IsWorking = true;
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.doUserWork));
				}
			}
		}
		private void doUserWork(object o)
		{
			try
			{
				while (!DBWorkThread.STOP_THREAD)
				{
					T t;
					lock (this.lockObj)
					{
						if (this.queue.Count <= 0)
						{
							this.qsize = 0;
							break;
						}
						t = this.queue.Dequeue();
						this.qsize = this.queue.Count;
						if (this.l_count == 0L)
						{
							this.DT_start = DateTime.Now;
						}
						if (this.className.Equals("Power"))
						{
							if (this.l_count > 26994L && this.qsize == 0)
							{
								this.l_count = 0L;
							}
							else
							{
								this.l_count += 1L;
							}
						}
						else
						{
							if (this.className.Equals("PowerDisspation"))
							{
								if (this.l_count > 53500L && this.qsize == 0)
								{
									this.l_count = 0L;
								}
								else
								{
									this.l_count += 1L;
								}
							}
							else
							{
								if (this.className.Equals("Rack"))
								{
									if (this.l_count > 1L && this.qsize == 0)
									{
										this.l_count = 0L;
									}
									else
									{
										this.l_count += 1L;
									}
								}
							}
						}
					}
					if (!t.Equals(default(T)))
					{
						if (this.isOneThread)
						{
							if (this.UserWork != null)
							{
								if (DBWorkThread.STOP_THREAD)
								{
									break;
								}
								this.UserWork(this, new WorkQueue<T>.EnqueueEventArgs(t));
							}
						}
						else
						{
							if (DBWorkThread.STOP_THREAD)
							{
								break;
							}
							ThreadPool.QueueUserWorkItem(delegate(object obj)
							{
								if (this.UserWork != null)
								{
									this.UserWork(this, new WorkQueue<T>.EnqueueEventArgs(obj));
								}
							}, t);
						}
					}
				}
			}
			finally
			{
				lock (this.lockIsWorking)
				{
					this.IsWorking = false;
				}
			}
		}
	}
}
