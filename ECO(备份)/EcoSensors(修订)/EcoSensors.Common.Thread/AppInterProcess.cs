using CommonAPI.InterProcess;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
namespace EcoSensors.Common.Thread
{
	internal class AppInterProcess
	{
		private const long MAX_INSTANCES = 100L;
		private const long ITEM_SIZE = 32L;
		private const string _strMapFileName = "Global\\ecoAppMapName";
		private const string _semaphoreName = "Global\\ecoAppMapSemaphore";
		private static object _thisSharedLock = new object();
		private static System.IntPtr _hMapFile = System.IntPtr.Zero;
		private static System.IntPtr _pMapBuf = System.IntPtr.Zero;
		private static Semaphore _semaphoreEcoInstance = null;
		public static bool IsAvailable()
		{
			bool result;
			lock (AppInterProcess._thisSharedLock)
			{
				result = (AppInterProcess._semaphoreEcoInstance != null);
			}
			return result;
		}
		private static Semaphore OpenGlobalSemaphore(string semName, int sInit, int sMax)
		{
			Semaphore semaphore = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				semaphore = Semaphore.OpenExisting(semName);
			}
			catch (System.Threading.WaitHandleCannotBeOpenedException)
			{
				flag = true;
			}
			catch (System.UnauthorizedAccessException)
			{
				flag2 = true;
			}
			if (flag)
			{
                //System.Environment.UserDomainName + "\\" + System.Environment.UserName;
				System.Security.Principal.SecurityIdentifier identity = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
				SemaphoreSecurity semaphoreSecurity = new SemaphoreSecurity();
				semaphoreSecurity.AddAccessRule(new SemaphoreAccessRule(identity, SemaphoreRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
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
						semaphore = Semaphore.OpenExisting(semName);
                      
                        //System.Environment.UserDomainName + "\\" + System.Environment.UserName;
						System.Security.Principal.SecurityIdentifier identity2 = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);
						SemaphoreSecurity semaphoreSecurity2 = new SemaphoreSecurity();
						semaphoreSecurity2.AddAccessRule(new SemaphoreAccessRule(identity2, SemaphoreRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
						semaphore.SetAccessControl(semaphoreSecurity2);
						semaphore = Semaphore.OpenExisting(semName);
					}
					catch (System.UnauthorizedAccessException)
					{
						if (semaphore != null)
						{
							semaphore.Close();
							semaphore.Dispose();
						}
						return null;
					}
					return semaphore;
				}
			}
			return semaphore;
		}
		public static bool OpenInterProcessShared()
		{
			Semaphore semaphore = AppInterProcess.OpenGlobalSemaphore("Global\\ecoAppMapSemaphore", 1, 1);
			if (semaphore == null)
			{
				return false;
			}
			AppInterProcess._semaphoreEcoInstance = semaphore;
			int num = 3208;
			AppInterProcess._semaphoreEcoInstance.WaitOne();
			int num2 = AppInterProcess.SetupShareMemoryWithSercurity(num);
			if (num2 <= 0)
			{
				byte[] array = new byte[num];
				System.Array.Clear(array, 0, array.Length);
				AppInterProcess.WriteShareArray(0L, array);
			}
			AppInterProcess._semaphoreEcoInstance.Release();
			return true;
		}
		public static void CloseShared()
		{
			lock (AppInterProcess._thisSharedLock)
			{
				if (AppInterProcess._semaphoreEcoInstance != null)
				{
					AppInterProcess._semaphoreEcoInstance.WaitOne();
					if (AppInterProcess._pMapBuf != System.IntPtr.Zero)
					{
						Win32.UnmapViewOfFile(AppInterProcess._pMapBuf);
					}
					if (AppInterProcess._hMapFile != System.IntPtr.Zero)
					{
						Win32.CloseHandle(AppInterProcess._hMapFile);
					}
					AppInterProcess._semaphoreEcoInstance.Release();
					AppInterProcess._semaphoreEcoInstance.Close();
					AppInterProcess._semaphoreEcoInstance.Dispose();
					AppInterProcess._semaphoreEcoInstance = null;
					AppInterProcess._hMapFile = System.IntPtr.Zero;
					AppInterProcess._pMapBuf = System.IntPtr.Zero;
				}
			}
		}
		private static long getValue64(byte[] shared, int index)
		{
			return System.BitConverter.ToInt64(shared, index * 8);
		}
		private static void setValue64(byte[] shared, int index, long value)
		{
			byte[] bytes = System.BitConverter.GetBytes(value);
			System.Array.Copy(bytes, 0, shared, index * 8, 8);
		}
		public static int getProcessID(int uid, string serverid, int isLocal)
		{
			int result;
			lock (AppInterProcess._thisSharedLock)
			{
				int num = 0;
				int num2 = 3208;
				byte[] array;
				if (serverid == null || serverid == "")
				{
					array = new byte[8];
					System.Array.Clear(array, 0, array.Length);
				}
				else
				{
					array = System.Text.Encoding.ASCII.GetBytes(serverid.ToUpper());
				}
				long num3 = System.BitConverter.ToInt64(array, 0);
				if (AppInterProcess._semaphoreEcoInstance != null)
				{
					AppInterProcess._semaphoreEcoInstance.WaitOne();
					byte[] shared = AppInterProcess.ReadShareArray(0L, (long)num2);
					long value = AppInterProcess.getValue64(shared, 0);
					int num4 = 0;
					while ((long)num4 < System.Math.Min(value, 100L))
					{
						int num5 = num4 * 32 + 8;
						AppInterProcess.getValue64(shared, num5);
						num5 += 8;
						long value2 = AppInterProcess.getValue64(shared, num5);
						num5 += 8;
						long value3 = AppInterProcess.getValue64(shared, num5);
						num5 += 8;
						long value4 = AppInterProcess.getValue64(shared, num5);
						num5 += 8;
						if (value2 == num3 && value3 == (long)isLocal && isLocal == 1)
						{
							num = (int)value4;
							break;
						}
						num4++;
					}
					AppInterProcess._semaphoreEcoInstance.Release();
				}
				result = num;
			}
			return result;
		}
		public static void setMyProcessID(int uid, string serverid, int isLocal)
		{
			lock (AppInterProcess._thisSharedLock)
			{
				int id = Process.GetCurrentProcess().Id;
				int num = 3208;
				byte[] array;
				if (serverid == null || serverid == "")
				{
					array = new byte[8];
					System.Array.Clear(array, 0, array.Length);
				}
				else
				{
					array = System.Text.Encoding.ASCII.GetBytes(serverid);
				}
				long num2 = System.BitConverter.ToInt64(array, 0);
				if (AppInterProcess._semaphoreEcoInstance != null)
				{
					AppInterProcess._semaphoreEcoInstance.WaitOne();
					byte[] array2 = AppInterProcess.ReadShareArray(0L, (long)num);
					long num3 = AppInterProcess.getValue64(array2, 0);
					bool flag2 = false;
					int num4 = 0;
					while ((long)num4 < System.Math.Min(num3, 100L))
					{
						int num5 = num4 * 32 + 8;
						long value = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						long value2 = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						long value3 = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						AppInterProcess.getValue64(array2, num5);
						if (value == (long)uid && value2 == num2 && value3 == (long)isLocal)
						{
							AppInterProcess.setValue64(array2, num5, (long)id);
							flag2 = true;
							break;
						}
						num4++;
					}
					if (!flag2 && num3 < 99L)
					{
						int num6 = (int)num3 * 32 + 8;
						AppInterProcess.setValue64(array2, num6, (long)uid);
						num6 += 8;
						AppInterProcess.setValue64(array2, num6, num2);
						num6 += 8;
						AppInterProcess.setValue64(array2, num6, (long)isLocal);
						num6 += 8;
						AppInterProcess.setValue64(array2, num6, (long)id);
						num3 += 1L;
						AppInterProcess.setValue64(array2, 0, num3);
					}
					AppInterProcess.WriteShareArray(0L, array2);
					AppInterProcess._semaphoreEcoInstance.Release();
				}
			}
		}
		public static void removeMyProcessID(int uid, string serverid, int isLocal)
		{
			lock (AppInterProcess._thisSharedLock)
			{
				int id = Process.GetCurrentProcess().Id;
				int num = 3208;
				byte[] array;
				if (serverid == null || serverid == "")
				{
					array = new byte[8];
					System.Array.Clear(array, 0, array.Length);
				}
				else
				{
					array = System.Text.Encoding.ASCII.GetBytes(serverid);
				}
				long num2 = System.BitConverter.ToInt64(array, 0);
				if (AppInterProcess._semaphoreEcoInstance != null)
				{
					AppInterProcess._semaphoreEcoInstance.WaitOne();
					byte[] array2 = AppInterProcess.ReadShareArray(0L, (long)num);
					long num3 = AppInterProcess.getValue64(array2, 0);
					int num4 = 0;
					while ((long)num4 < System.Math.Min(num3, 100L))
					{
						int num5 = num4 * 32 + 8;
						long value = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						long value2 = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						long value3 = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						long value4 = AppInterProcess.getValue64(array2, num5);
						num5 += 8;
						if (value == (long)uid && value2 == num2 && value3 == (long)isLocal && value4 == (long)id)
						{
							while ((long)num4 < num3 - 2L)
							{
								int num6 = num4 * 32 + 8;
								int num7 = (num4 + 1) * 32 + 8;
								AppInterProcess.getValue64(array2, num7);
								num7 += 8;
								AppInterProcess.setValue64(array2, num6, num3);
								num6 += 8;
								AppInterProcess.getValue64(array2, num7);
								num7 += 8;
								AppInterProcess.setValue64(array2, num6, num3);
								num6 += 8;
								AppInterProcess.getValue64(array2, num7);
								num7 += 8;
								AppInterProcess.setValue64(array2, num6, num3);
								num6 += 8;
								AppInterProcess.getValue64(array2, num7);
								num7 += 8;
								AppInterProcess.setValue64(array2, num6, num3);
								num6 += 8;
								num4++;
							}
							num3 -= 1L;
							AppInterProcess.setValue64(array2, 0, num3);
							break;
						}
						num4++;
					}
					AppInterProcess.WriteShareArray(0L, array2);
					AppInterProcess._semaphoreEcoInstance.Release();
				}
			}
		}
		private static int SetupShareMemoryWithSercurity(int nSize)
		{
			bool flag = false;
			System.IntPtr intPtr = System.IntPtr.Zero;
			int cb = 68;
			System.IntPtr arg_12_0 = System.IntPtr.Zero;
			Win32.SECURITY_ATTRIBUTES sECURITY_ATTRIBUTES = default(Win32.SECURITY_ATTRIBUTES);
			try
			{
				intPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(cb);
				if (!Win32.CreateWellKnownSid(System.Security.Principal.WellKnownSidType.BuiltinAdministratorsSid, System.IntPtr.Zero, intPtr, ref cb))
				{
					int result = -1;
					return result;
				}
				if (!Win32.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;;GA;;;RD)(A;;GA;;;S-1-5-32-544)(A;;GA;;;S-1-5-4)", 1, out sECURITY_ATTRIBUTES.lpSecurityDescriptor, System.IntPtr.Zero))
				{
					int result = -1;
					return result;
				}
				sECURITY_ATTRIBUTES.nLength = System.Runtime.InteropServices.Marshal.SizeOf(sECURITY_ATTRIBUTES);
				sECURITY_ATTRIBUTES.bInheritHandle = false;
				AppInterProcess._hMapFile = Win32.CreateFileMapping(4294967295u, ref sECURITY_ATTRIBUTES, 4, 0, nSize + 1024, "Global\\ecoAppMapName");
				flag = ((long)System.Runtime.InteropServices.Marshal.GetLastWin32Error() == 183L);
				if (AppInterProcess._hMapFile == System.IntPtr.Zero)
				{
					int result = -1;
					return result;
				}
				AppInterProcess._pMapBuf = Win32.MapViewOfFile(AppInterProcess._hMapFile, 983071, 0, 0, nSize + 128);
				if (AppInterProcess._pMapBuf == System.IntPtr.Zero)
				{
					int result = -1;
					return result;
				}
			}
			catch (System.Exception)
			{
			}
			finally
			{
				if (intPtr != System.IntPtr.Zero)
				{
					System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
				}
				if (sECURITY_ATTRIBUTES.lpSecurityDescriptor != System.IntPtr.Zero)
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
		private static void WriteShareArray(long nOffset, byte[] data)
		{
			System.IntPtr destination = new System.IntPtr(AppInterProcess._pMapBuf.ToInt64() + nOffset);
			System.Runtime.InteropServices.Marshal.Copy(data, 0, destination, data.Length);
		}
		private static byte[] ReadShareArray(long nOffset, long nSize)
		{
			byte[] array = new byte[nSize];
			System.IntPtr source = new System.IntPtr(AppInterProcess._pMapBuf.ToInt64() + nOffset);
			System.Runtime.InteropServices.Marshal.Copy(source, array, 0, array.Length);
			return array;
		}
		private static void WriteShareLong(long nOffset, long value)
		{
			byte[] bytes = System.BitConverter.GetBytes(value);
			System.IntPtr destination = new System.IntPtr(AppInterProcess._pMapBuf.ToInt64() + nOffset);
			System.Runtime.InteropServices.Marshal.Copy(bytes, 0, destination, bytes.Length);
		}
		private static long ReadShareLong(long nOffset)
		{
			byte[] array = new byte[8];
			System.IntPtr source = new System.IntPtr(AppInterProcess._pMapBuf.ToInt64() + nOffset);
			System.Runtime.InteropServices.Marshal.Copy(source, array, 0, array.Length);
			return System.BitConverter.ToInt64(array, 0);
		}
	}
}
