using System;
using System.Collections.Generic;
using System.Threading;
namespace CommonAPI.InterProcess
{
	public class InterProcessEvent : InterProcessBase
	{
		private const string EVENT_PREFIX = "Global\\EcoSensorsGlobalEvent-";
		public static object _thisLock = new object();
		public static Dictionary<string, EventWaitHandle> _globalEvents = null;
		public static int waitGlobalEvent(string name, int timeout, bool defaultValue = false)
		{
			lock (InterProcessEvent._thisLock)
			{
				if (InterProcessEvent._globalEvents == null)
				{
					InterProcessEvent._globalEvents = new Dictionary<string, EventWaitHandle>();
				}
				string text = "Global\\EcoSensorsGlobalEvent-" + name;
				if (InterProcessEvent._globalEvents.ContainsKey(text))
				{
					if (InterProcessEvent._globalEvents[text].WaitOne(timeout))
					{
						int result = 1;
						return result;
					}
				}
				else
				{
					EventWaitHandle eventWaitHandle = InterProcessBase.OpenGlobalEvent(text, defaultValue);
					if (eventWaitHandle != null)
					{
						InterProcessEvent._globalEvents.Add(text, eventWaitHandle);
						if (InterProcessEvent._globalEvents[text].WaitOne(0))
						{
							int result = 1;
							return result;
						}
					}
				}
			}
			return -1;
		}
		public static bool getGlobalEvent(string name, bool defaultValue = false)
		{
			lock (InterProcessEvent._thisLock)
			{
				if (InterProcessEvent._globalEvents == null)
				{
					InterProcessEvent._globalEvents = new Dictionary<string, EventWaitHandle>();
				}
				string text = "Global\\EcoSensorsGlobalEvent-" + name;
				if (InterProcessEvent._globalEvents.ContainsKey(text))
				{
					if (InterProcessEvent._globalEvents[text].WaitOne(0))
					{
						bool result = true;
						return result;
					}
				}
				else
				{
					EventWaitHandle eventWaitHandle = InterProcessBase.OpenGlobalEvent(text, defaultValue);
					if (eventWaitHandle != null)
					{
						InterProcessEvent._globalEvents.Add(text, eventWaitHandle);
						if (InterProcessEvent._globalEvents[text].WaitOne(0))
						{
							bool result = true;
							return result;
						}
					}
				}
			}
			return false;
		}
		public static void setGlobalEvent(string name, bool defaultValue = true)
		{
			lock (InterProcessEvent._thisLock)
			{
				if (InterProcessEvent._globalEvents == null)
				{
					InterProcessEvent._globalEvents = new Dictionary<string, EventWaitHandle>();
				}
				string text = "Global\\EcoSensorsGlobalEvent-" + name;
				if (!InterProcessEvent._globalEvents.ContainsKey(text))
				{
					EventWaitHandle eventWaitHandle = InterProcessBase.OpenGlobalEvent(text, defaultValue);
					if (eventWaitHandle != null)
					{
						InterProcessEvent._globalEvents.Add(text, eventWaitHandle);
					}
				}
				if (InterProcessEvent._globalEvents.ContainsKey(text))
				{
					if (defaultValue)
					{
						InterProcessEvent._globalEvents[text].Set();
					}
					else
					{
						InterProcessEvent._globalEvents[text].Reset();
					}
				}
			}
		}
	}
}
