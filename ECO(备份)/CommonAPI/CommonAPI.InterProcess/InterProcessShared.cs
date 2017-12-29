using CustomXmlSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
namespace CommonAPI.InterProcess
{
	public class InterProcessShared<T> : InterProcessBase where T : new()
	{
		public const string KEY_ServiceStatus = "ServiceStatus";
		public const string VALUE_ServiceStatus_Waiting = "waiting";
		public const string VALUE_ServiceStatus_Failed = "failed";
		public const string VALUE_ServiceStatus_Ready = "ready";
		public const string KEY_ServiceStDBMaintain = "ServiceStDBMaintain";
		public const string VALUE_ServiceStDBMaintain = "DBMaintain";
		public const string VALUE_ServiceStDBMaintainFinish = "DBMaintainFinish";
		public const int _maxMemorySize = 8192;
		private object _synLock;
		private long _shareMemSize;
		private string _strMapFileName;
		private IntPtr _hMapFile;
		private IntPtr _pMapBuf;
		private string _semaphoreName;
		private Semaphore _semaphoreEcoInstance;
		private static object _pairLock = new object();
		private static InterProcessShared<Dictionary<string, string>> _sharePairs = null;
		public static string getInterProcessKeyValue(string key)
		{
			string result;
			lock (InterProcessShared<T>._pairLock)
			{
				if (InterProcessShared<T>._sharePairs == null)
				{
					InterProcessShared<T>._sharePairs = new InterProcessShared<Dictionary<string, string>>("SharedValuePairs", 8192L);
				}
				if (InterProcessShared<T>._sharePairs == null)
				{
					result = null;
				}
				else
				{
					Dictionary<string, string> sharedObject = InterProcessShared<T>._sharePairs.getSharedObject();
					if (sharedObject == null)
					{
						result = null;
					}
					else
					{
						if (!sharedObject.ContainsKey(key))
						{
							result = null;
						}
						else
						{
							result = sharedObject[key];
						}
					}
				}
			}
			return result;
		}
		public static void setInterProcessKeyValue(string key, string value)
		{
			lock (InterProcessShared<T>._pairLock)
			{
				if (InterProcessShared<T>._sharePairs == null)
				{
					InterProcessShared<T>._sharePairs = new InterProcessShared<Dictionary<string, string>>("SharedValuePairs", 8192L);
				}
				if (InterProcessShared<T>._sharePairs != null)
				{
					Dictionary<string, string> sharedObject = InterProcessShared<T>._sharePairs.getSharedObject();
					if (sharedObject != null)
					{
						sharedObject[key] = value;
						InterProcessShared<T>._sharePairs.setSharedObject(sharedObject);
					}
				}
			}
		}
		public InterProcessShared(string shareName, long maxMemInBytes)
		{
			this._synLock = new object();
			this._shareMemSize = maxMemInBytes;
			this._strMapFileName = "Global\\" + shareName + "Name";
			this._semaphoreName = "Global\\" + shareName + "Semaphore";
			this._hMapFile = IntPtr.Zero;
			this._pMapBuf = IntPtr.Zero;
			this._semaphoreEcoInstance = null;
			this.OpenInterProcessShared();
		}
		public bool IsAvailable()
		{
			bool result;
			lock (this._synLock)
			{
				result = (this._semaphoreEcoInstance != null);
			}
			return result;
		}
		private bool OpenInterProcessShared()
		{
			Semaphore semaphore = InterProcessBase.OpenGlobalSemaphore(this._semaphoreName, 1, 1);
			if (semaphore == null)
			{
				return false;
			}
			this._semaphoreEcoInstance = semaphore;
			int num = (int)this._shareMemSize + 8;
			this._semaphoreEcoInstance.WaitOne();
			int num2 = InterProcessBase.SetupShareMemoryWithSercurity(this._strMapFileName, num, ref this._hMapFile, ref this._pMapBuf);
			if (num2 < 0)
			{
				this._semaphoreEcoInstance.Release();
				this._semaphoreEcoInstance = null;
				return false;
			}
			if (num2 <= 0)
			{
				byte[] array = new byte[num];
				Array.Clear(array, 0, array.Length);
				this.WriteShareArray(0L, array);
				T t = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
				XmlDocument xmlDocument = CustomXmlSerializer.Serialize(t, 8, "SharedObject");
				string outerXml = xmlDocument.OuterXml;
				byte[] bytes = Encoding.UTF8.GetBytes(outerXml);
				if ((long)bytes.Length < this._shareMemSize)
				{
					long value = (long)bytes.Length;
					byte[] bytes2 = BitConverter.GetBytes(value);
					byte[] array2 = new byte[8 + bytes.Length];
					Array.Copy(bytes2, 0, array2, 0, 8);
					Array.Copy(bytes, 0, array2, 8, bytes.Length);
					this.WriteShareArray(0L, array2);
				}
			}
			this._semaphoreEcoInstance.Release();
			return true;
		}
		public void CloseShared()
		{
			lock (this._synLock)
			{
				if (this._semaphoreEcoInstance != null)
				{
					this._semaphoreEcoInstance.WaitOne();
					if (this._pMapBuf != IntPtr.Zero)
					{
						Win32.UnmapViewOfFile(this._pMapBuf);
					}
					if (this._hMapFile != IntPtr.Zero)
					{
						Win32.CloseHandle(this._hMapFile);
					}
					this._semaphoreEcoInstance.Release();
					this._semaphoreEcoInstance.Close();
                    //this._semaphoreEcoInstance.Dispose();
					this._semaphoreEcoInstance = null;
					this._hMapFile = IntPtr.Zero;
					this._pMapBuf = IntPtr.Zero;
				}
			}
		}
		private void WriteShareArray(long nOffset, byte[] data)
		{
			IntPtr destination = new IntPtr(this._pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(data, 0, destination, data.Length);
		}
		private byte[] ReadShareArray(long nOffset, long nSize)
		{
			byte[] array = new byte[nSize];
			IntPtr source = new IntPtr(this._pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(source, array, 0, array.Length);
			return array;
		}
		public T getSharedObject()
		{
			try
			{
				lock (this._synLock)
				{
					byte[] array = this.ReadShareArray(0L, 8L + this._shareMemSize);
					long num = BitConverter.ToInt64(array, 0);
					if (num <= this._shareMemSize)
					{
						byte[] array2 = new byte[num];
						Array.Copy(array, 8L, array2, 0L, num);
						string @string = Encoding.UTF8.GetString(array2);
						return (T)((object)CustomXmlDeserializer.Deserialize(@string, 8, new TestMeTypeConverter()));
					}
				}
			}
			catch (Exception)
			{
			}
			return default(T);
		}
		public bool setSharedObject(T obj)
		{
			try
			{
				lock (this._synLock)
				{
					XmlDocument xmlDocument = CustomXmlSerializer.Serialize(obj, 8, "SharedObject");
					string outerXml = xmlDocument.OuterXml;
					byte[] bytes = Encoding.UTF8.GetBytes(outerXml);
					if ((long)bytes.Length < this._shareMemSize)
					{
						long value = (long)bytes.Length;
						byte[] bytes2 = BitConverter.GetBytes(value);
						byte[] array = new byte[8 + bytes.Length];
						Array.Copy(bytes2, 0, array, 0, 8);
						Array.Copy(bytes, 0, array, 8, bytes.Length);
						this.WriteShareArray(0L, array);
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
