using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class MessageCacheList<T> : MessageCache<T>
	{
		private System.Collections.Generic.List<T> cache = new System.Collections.Generic.List<T>();
		private Locker locker = new Locker();
		public override bool reWriteMessage(System.Collections.Generic.ICollection<T> message)
		{
			bool result;
			try
			{
				this.locker.TryLock();
				this.cache.Clear();
				if (message == null || message.Count < 1)
				{
					result = true;
				}
				else
				{
					this.cache.AddRange(message);
					result = true;
				}
			}
			catch (System.Exception)
			{
				result = false;
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override bool putMessage(T message)
		{
			if (message == null)
			{
				return false;
			}
			bool result;
			try
			{
				this.locker.TryLock();
				this.cache.Add(message);
				result = true;
			}
			catch (System.Exception)
			{
				result = false;
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override bool putMessage(System.Collections.Generic.ICollection<T> messages)
		{
			if (messages == null || messages.Count < 1)
			{
				return false;
			}
			bool result;
			try
			{
				this.locker.TryLock();
				this.cache.AddRange(messages);
				result = true;
			}
			catch (System.Exception)
			{
				result = false;
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override System.Collections.Generic.ICollection<T> removeMessage()
		{
			if (this.cache.Count < 1)
			{
				return new System.Collections.Generic.List<T>();
			}
			System.Collections.Generic.ICollection<T> result;
			try
			{
				this.locker.TryLock();
				System.Collections.Generic.List<T> range = this.cache.GetRange(0, this.cache.Count);
				this.cache.Clear();
				result = range;
			}
			catch (System.Exception)
			{
				result = new System.Collections.Generic.List<T>();
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override System.Collections.Generic.ICollection<T> removeMessage(System.Collections.Generic.ICollection<T> items)
		{
			if (items == null || items.Count < 1)
			{
				return new System.Collections.Generic.List<T>();
			}
			if (this.cache.Count < 1)
			{
				return new System.Collections.Generic.List<T>();
			}
			System.Collections.Generic.ICollection<T> result;
			try
			{
				this.locker.TryLock();
				System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
				foreach (T current in items)
				{
					if (this.cache.Contains(current))
					{
						this.cache.Remove(current);
						list.Add(current);
					}
				}
				result = list;
			}
			catch (System.Exception)
			{
				result = new System.Collections.Generic.List<T>();
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override T removeMessage(T item)
		{
			if (item == null)
			{
				return default(T);
			}
			if (this.cache == null || this.cache.Count < 1)
			{
				return default(T);
			}
			T result;
			try
			{
				this.locker.TryLock();
				if (this.cache.Contains(item))
				{
					this.cache.Remove(item);
				}
				result = item;
			}
			catch (System.Exception)
			{
				result = default(T);
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
		public override System.Collections.Generic.ICollection<T> getMessage()
		{
			System.Collections.Generic.ICollection<T> result;
			try
			{
				this.locker.TryLock();
				if (this.cache.Count < 1)
				{
					result = new System.Collections.Generic.List<T>();
				}
				else
				{
					System.Collections.Generic.List<T> list = new System.Collections.Generic.List<T>();
					list.AddRange(this.cache);
					result = list;
				}
			}
			finally
			{
				this.locker.UnLock();
			}
			return result;
		}
	}
}
