using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public abstract class MessageCache<T>
	{
		public abstract bool reWriteMessage(System.Collections.Generic.ICollection<T> message);
		public abstract bool putMessage(T message);
		public abstract bool putMessage(System.Collections.Generic.ICollection<T> messages);
		public abstract System.Collections.Generic.ICollection<T> removeMessage();
		public abstract System.Collections.Generic.ICollection<T> removeMessage(System.Collections.Generic.ICollection<T> items);
		public abstract T removeMessage(T item);
		public abstract System.Collections.Generic.ICollection<T> getMessage();
	}
}
