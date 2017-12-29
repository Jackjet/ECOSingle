using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
namespace CommonAPI.InterProcess
{
	public class InterProcessBase
	{
		public static EventWaitHandle OpenGlobalEvent(string ewhName, bool defaultValue)
		{
			EventWaitHandle eventWaitHandle = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				eventWaitHandle = EventWaitHandle.OpenExisting(ewhName);
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				flag = true;
			}
			catch (UnauthorizedAccessException)
			{
				flag2 = true;
			}
			if (flag)
			{
                //Environment.UserDomainName + "\\" + Environment.UserName;
				SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				EventWaitHandleSecurity eventWaitHandleSecurity = new EventWaitHandleSecurity();
				eventWaitHandleSecurity.AddAccessRule(new EventWaitHandleAccessRule(identity, EventWaitHandleRights.FullControl, AccessControlType.Allow));
				bool flag3;
				eventWaitHandle = new EventWaitHandle(defaultValue, EventResetMode.ManualReset, ewhName, out flag3, eventWaitHandleSecurity);
				if (!flag3)
				{
					return null;
				}
			}
			else
			{
				if (flag2)
				{
					try
					{
						eventWaitHandle = EventWaitHandle.OpenExisting(ewhName, EventWaitHandleRights.ReadPermissions | EventWaitHandleRights.ChangePermissions);
						EventWaitHandleSecurity accessControl = eventWaitHandle.GetAccessControl();
                        //Environment.UserDomainName + "\\" + Environment.UserName;
						SecurityIdentifier identity2 = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
						new EventWaitHandleSecurity();
						accessControl.AddAccessRule(new EventWaitHandleAccessRule(identity2, EventWaitHandleRights.FullControl, AccessControlType.Allow));
						eventWaitHandle.SetAccessControl(accessControl);
						eventWaitHandle = EventWaitHandle.OpenExisting(ewhName);
					}
					catch (UnauthorizedAccessException)
					{
						return null;
					}
					return eventWaitHandle;
				}
			}
			return eventWaitHandle;
		}
		public static Semaphore OpenGlobalSemaphore(string semName, int sInit, int sMax)
		{
			Semaphore semaphore = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				semaphore = Semaphore.OpenExisting(semName);
			}
			catch (WaitHandleCannotBeOpenedException)
			{
				flag = true;
			}
			catch (UnauthorizedAccessException)
			{
				flag2 = true;
			}
			if (flag)
			{
                //Environment.UserDomainName + "\\" + Environment.UserName;
				SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				SemaphoreSecurity semaphoreSecurity = new SemaphoreSecurity();
				semaphoreSecurity.AddAccessRule(new SemaphoreAccessRule(identity, SemaphoreRights.FullControl, AccessControlType.Allow));
				bool flag3;
                semaphore = new Semaphore(sInit, sMax, semName, out flag3, semaphoreSecurity);
				if (!flag3)
				{
					return null;
				}
			}
			else
			{
				if (flag2)
				{
					try
					{
                        using (semaphore = Semaphore.OpenExisting(semName))
                        {
                            //Environment.UserDomainName + "\\" + Environment.UserName;
                            SecurityIdentifier identity2 = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                            SemaphoreSecurity semaphoreSecurity2 = new SemaphoreSecurity();
                            semaphoreSecurity2.AddAccessRule(new SemaphoreAccessRule(identity2, SemaphoreRights.FullControl, AccessControlType.Allow));
                            semaphore.SetAccessControl(semaphoreSecurity2);
                            semaphore = Semaphore.OpenExisting(semName);
                        }
						
					}
					catch (UnauthorizedAccessException)
					{
                        //if (semaphore != null)
                        //{
                        //    semaphore.Close();
                        //    //semaphore.Dispose();
                        //}
						return null;
					}
					return semaphore;
				}
			}
			return semaphore;
		}
		public static int SetupShareMemoryWithSercurity(string strMapFileName, int nSize, ref IntPtr _hMapFile, ref IntPtr _pMapBuf)
		{
			bool flag = false;
			IntPtr intPtr = IntPtr.Zero;
			int cb = 68;
			IntPtr arg_12_0 = IntPtr.Zero;
			Win32.SECURITY_ATTRIBUTES sECURITY_ATTRIBUTES = default(Win32.SECURITY_ATTRIBUTES);
			try
			{
				intPtr = Marshal.AllocHGlobal(cb);
				if (!Win32.CreateWellKnownSid(WellKnownSidType.BuiltinAdministratorsSid, IntPtr.Zero, intPtr, ref cb))
				{
					int result = -1;
					return result;
				}
				if (!Win32.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;;GA;;;RD)(A;;GA;;;S-1-5-32-544)(A;;GA;;;S-1-5-4)", 1, out sECURITY_ATTRIBUTES.lpSecurityDescriptor, IntPtr.Zero))
				{
					int result = -1;
					return result;
				}
				sECURITY_ATTRIBUTES.nLength = Marshal.SizeOf(sECURITY_ATTRIBUTES);
				sECURITY_ATTRIBUTES.bInheritHandle = false;
				_hMapFile = Win32.CreateFileMapping(4294967295u, ref sECURITY_ATTRIBUTES, 4, 0, nSize + 1024, strMapFileName);
				flag = ((long)Marshal.GetLastWin32Error() == 183L);
				if (_hMapFile == IntPtr.Zero)
				{
					int result = -1;
					return result;
				}
				_pMapBuf = Win32.MapViewOfFile(_hMapFile, 983071, 0, 0, nSize + 128);
				if (_pMapBuf == IntPtr.Zero)
				{
					int result = -1;
					return result;
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (sECURITY_ATTRIBUTES.lpSecurityDescriptor != IntPtr.Zero)
				{
					Win32.LocalFree(sECURITY_ATTRIBUTES.lpSecurityDescriptor);
				}
			}
			if (!flag)
			{
				return 0;
			}
			return 1;
		}
	}
}
