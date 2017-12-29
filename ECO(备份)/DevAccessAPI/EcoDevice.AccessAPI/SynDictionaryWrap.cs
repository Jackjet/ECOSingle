using System;
using System.Collections;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class SynDictionaryWrap<Key, Value> : System.Collections.Generic.IDictionary<Key, Value>, System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Key, Value>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Key, Value>>, System.Collections.IEnumerable
	{
		private Locker lc = new Locker();
		private System.Collections.Generic.IDictionary<Key, Value> dic;
		public System.Collections.Generic.ICollection<Key> Keys
		{
			get
			{
				System.Collections.Generic.ICollection<Key> keys;
				try
				{
					this.lc.TryLock();
					keys = this.dic.Keys;
				}
				finally
				{
					this.lc.UnLock();
				}
				return keys;
			}
		}
		public System.Collections.Generic.ICollection<Value> Values
		{
			get
			{
				System.Collections.Generic.ICollection<Value> values;
				try
				{
					this.lc.TryLock();
					values = this.dic.Values;
				}
				finally
				{
					this.lc.UnLock();
				}
				return values;
			}
		}
		public Value this[Key key]
		{
			get
			{
				Value result;
				try
				{
					this.lc.TryLock();
					result = this.dic[key];
				}
				finally
				{
					this.lc.UnLock();
				}
				return result;
			}
			set
			{
				try
				{
					this.lc.TryLock();
					this.dic[key] = value;
				}
				finally
				{
					this.lc.UnLock();
				}
			}
		}
		public int Count
		{
			get
			{
				int count;
				try
				{
					this.lc.TryLock();
					count = this.dic.Count;
				}
				finally
				{
					this.lc.UnLock();
				}
				return count;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				bool isReadOnly;
				try
				{
					this.lc.TryLock();
					isReadOnly = this.dic.IsReadOnly;
				}
				finally
				{
					this.lc.UnLock();
				}
				return isReadOnly;
			}
		}
		public SynDictionaryWrap()
		{
			this.dic = new System.Collections.Generic.Dictionary<Key, Value>();
		}
		public SynDictionaryWrap(System.Collections.Generic.IDictionary<Key, Value> dic)
		{
			if (dic == null)
			{
				throw new System.ArgumentNullException("The parameter is null");
			}
			this.dic = dic;
		}
		public void Add(Key key, Value value)
		{
			try
			{
				this.lc.TryLock();
				this.dic.Add(key, value);
			}
			finally
			{
				this.lc.UnLock();
			}
		}
		public bool ContainsKey(Key key)
		{
			bool result;
			try
			{
				this.lc.TryLock();
				result = this.dic.ContainsKey(key);
			}
			finally
			{
				this.lc.UnLock();
			}
			return result;
		}
		public bool Remove(Key key)
		{
			bool result;
			try
			{
				this.lc.TryLock();
				result = this.dic.Remove(key);
			}
			finally
			{
				this.lc.UnLock();
			}
			return result;
		}
		public bool TryGetValue(Key key, out Value value)
		{
			bool result;
			try
			{
				this.lc.TryLock();
				result = this.dic.TryGetValue(key, out value);
			}
			finally
			{
				this.lc.UnLock();
			}
			return result;
		}
		public void Add(System.Collections.Generic.KeyValuePair<Key, Value> item)
		{
			try
			{
				this.lc.TryLock();
				this.dic.Add(item);
			}
			finally
			{
				this.lc.UnLock();
			}
		}
		public void Clear()
		{
			try
			{
				this.lc.TryLock();
				this.dic.Clear();
			}
			finally
			{
				this.lc.UnLock();
			}
		}
		public bool Contains(System.Collections.Generic.KeyValuePair<Key, Value> item)
		{
			bool result;
			try
			{
				this.lc.TryLock();
				result = this.dic.Contains(item);
			}
			finally
			{
				this.lc.UnLock();
			}
			return result;
		}
		public void CopyTo(System.Collections.Generic.KeyValuePair<Key, Value>[] array, int arrayIndex)
		{
			try
			{
				this.lc.TryLock();
				this.dic.CopyTo(array, arrayIndex);
			}
			finally
			{
				this.lc.UnLock();
			}
		}
		public bool Remove(System.Collections.Generic.KeyValuePair<Key, Value> item)
		{
			bool result;
			try
			{
				this.lc.TryLock();
				result = this.dic.Remove(item);
			}
			finally
			{
				this.lc.UnLock();
			}
			return result;
		}
		public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Key, Value>> GetEnumerator()
		{
			System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Key, Value>> enumerator;
			try
			{
				this.lc.TryLock();
				enumerator = this.dic.GetEnumerator();
			}
			finally
			{
				this.lc.UnLock();
			}
			return enumerator;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			System.Collections.IEnumerator enumerator;
			try
			{
				this.lc.TryLock();
				enumerator = this.dic.GetEnumerator();
			}
			finally
			{
				this.lc.UnLock();
			}
			return enumerator;
		}
	}
}
