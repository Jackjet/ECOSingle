using InSnergyAPI.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
namespace InSnergyAPI.DataLayer
{
	public class SharedData
	{
		public const string strMapFileName = "Global\\InSnergySharedName";
		public const long MAX_QUEUE_ITEMS = 1024L;
		public const long ITEM_LEN_DEVICE_ID = 20L;
		public const long ITEM_SIZE_GATEWAY = 35L;
		public const long ITEM_SIZE_DEVICE = 21L;
		public const long ITEM_SIZE_CHANNEL = 7L;
		public const long ITEM_SIZE_ATTRIBUTE = 20L;
		public const long OFFSET_MAX_GATEWAYS = 0L;
		public const long OFFSET_MAX_DEVICES = 8L;
		public const long OFFSET_MAX_CHANNELS = 16L;
		public const long OFFSET_MAX_ATTRIBUTES = 24L;
		public const long OFFSET_GATEWAY_COUNT = 32L;
		public const long OFFSET_SERVICE_FLAG = 40L;
		public const long OFFSET_SHARE_SIZE = 48L;
		public const long OFFSET_SERVICE_STATUS = 56L;
		public const long OFFSET_QUEUE_SIZE = 64L;
		public const long OFFSET_QUEUE_HEAD = 72L;
		public const long OFFSET_QUEUE_TAIL = 80L;
		public const long OFFSET_QUEUE_BUFFER = 88L;
		public const long OFFSET_GATEWAY_LIST = 8280L;
		private const string semaphoreName = "Global\\InSnergySharedSemaphore";
		public static long MAX_SHARE_DATA_SIZE = 2000000L;
		private static bool bIsProducer = false;
		private static object thisSharedLock = new object();
		private static Semaphore semaphoreInSnergy = null;
		private static long nMaxGateways = 3000L;
		private static long nMaxDevices = 8L;
		private static long nMaxChannels = 8L;
		private static long nMaxAttributes = 32L;
		private static IntPtr hMapFile = IntPtr.Zero;
		private static IntPtr pMapBuf = IntPtr.Zero;
		private static long nLastShareMemoryErrorCode = 0L;
		private static List<int> listParamEnabled = null;
		public static void SetParamFilter(params int[] param)
		{
			lock (SharedData.thisSharedLock)
			{
				SharedData.listParamEnabled = new List<int>();
				for (int i = 0; i < param.Length; i++)
				{
					int item = param[i];
					SharedData.listParamEnabled.Add(item);
				}
			}
		}
		public static void SetProducer(bool producer)
		{
			lock (SharedData.thisSharedLock)
			{
				SharedData.bIsProducer = producer;
			}
		}
		public static bool IsProducer()
		{
			bool result;
			lock (SharedData.thisSharedLock)
			{
				result = SharedData.bIsProducer;
			}
			return result;
		}
		public static long getQueueFree()
		{
			long result;
			lock (SharedData.thisSharedLock)
			{
				if (SharedData.semaphoreInSnergy == null)
				{
					result = 0L;
				}
				else
				{
					SharedData.semaphoreInSnergy.WaitOne();
					long num = SharedData.ReadShareLong(64L);
					long num2 = SharedData.ReadShareLong(72L);
					long num3 = SharedData.ReadShareLong(80L);
					SharedData.semaphoreInSnergy.Release();
					if (num2 >= num3)
					{
						result = num - num2 + num3 - 1L;
					}
					else
					{
						result = num3 - num2 - 1L;
					}
				}
			}
			return result;
		}
		public static long getQueueLength()
		{
			long result;
			lock (SharedData.thisSharedLock)
			{
				if (SharedData.semaphoreInSnergy == null)
				{
					result = 0L;
				}
				else
				{
					SharedData.semaphoreInSnergy.WaitOne();
					long num = SharedData.ReadShareLong(64L);
					long num2 = SharedData.ReadShareLong(72L);
					long num3 = SharedData.ReadShareLong(80L);
					SharedData.semaphoreInSnergy.Release();
					if (num2 >= num3)
					{
						result = num2 - num3;
					}
					else
					{
						result = num - num3 + num2;
					}
				}
			}
			return result;
		}
		public static bool IsAvailable()
		{
			bool result;
			lock (SharedData.thisSharedLock)
			{
				result = (SharedData.semaphoreInSnergy != null);
			}
			return result;
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
				InSnergyService.PostLog("Semaphore does not exist: " + semName);
				flag = true;
			}
			catch (UnauthorizedAccessException ex)
			{
				InSnergyService.PostLog("Unauthorized access: " + ex.Message + "===>" + semName);
				flag2 = true;
			}
			catch (Exception)
			{
			}
			if (flag)
			{
				Environment.UserDomainName + "\\" + Environment.UserName;
				SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
				SemaphoreSecurity semaphoreSecurity = new SemaphoreSecurity();
				semaphoreSecurity.AddAccessRule(new SemaphoreAccessRule(identity, SemaphoreRights.FullControl, AccessControlType.Allow));
				bool flag3;
				semaphore = new Semaphore(sInit, sMax, semName, ref flag3, semaphoreSecurity);
				if (!flag3)
				{
					InSnergyService.PostLog("Unable to create the semaphore: " + semName);
					return null;
				}
				InSnergyService.PostLog("Created the semaphore: " + semName);
			}
			else
			{
				if (flag2)
				{
					try
					{
						semaphore = Semaphore.OpenExisting(semName);
						Environment.UserDomainName + "\\" + Environment.UserName;
						SecurityIdentifier identity2 = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
						SemaphoreSecurity semaphoreSecurity2 = new SemaphoreSecurity();
						semaphoreSecurity2.AddAccessRule(new SemaphoreAccessRule(identity2, SemaphoreRights.FullControl, AccessControlType.Allow));
						semaphore.SetAccessControl(semaphoreSecurity2);
						InSnergyService.PostLog("Updated semaphore security: " + semName);
						semaphore = Semaphore.OpenExisting(semName);
					}
					catch (UnauthorizedAccessException ex2)
					{
						InSnergyService.PostLog("Unable to change permissions: " + ex2.Message + "===>" + semName);
						if (semaphore != null)
						{
							semaphore.Close();
							semaphore.Dispose();
						}
						return null;
					}
					catch (Exception)
					{
					}
					return semaphore;
				}
			}
			return semaphore;
		}
		public static bool CreateShare(long maxGateways, long maxDevices, long maxChannels, long maxAttributes)
		{
			Semaphore semaphore = SharedData.OpenGlobalSemaphore("Global\\InSnergySharedSemaphore", 1, 1);
			if (semaphore == null)
			{
				return false;
			}
			SharedData.semaphoreInSnergy = semaphore;
			SharedData.nLastShareMemoryErrorCode = 0L;
			if (SharedData.CreateShareMemory(maxGateways, maxDevices, maxChannels, maxAttributes))
			{
				return true;
			}
			semaphore.Close();
			semaphore.Dispose();
			SharedData.semaphoreInSnergy = null;
			SharedData.hMapFile = IntPtr.Zero;
			SharedData.pMapBuf = IntPtr.Zero;
			return false;
		}
		public static void CloseShare()
		{
			lock (SharedData.thisSharedLock)
			{
				if (SharedData.semaphoreInSnergy != null)
				{
					SharedData.semaphoreInSnergy.WaitOne();
					if (SharedData.pMapBuf != IntPtr.Zero)
					{
						Win32.UnmapViewOfFile(SharedData.pMapBuf);
					}
					if (SharedData.hMapFile != IntPtr.Zero)
					{
						Win32.CloseHandle(SharedData.hMapFile);
					}
					SharedData.semaphoreInSnergy.Release();
					SharedData.semaphoreInSnergy.Close();
					SharedData.semaphoreInSnergy.Dispose();
					SharedData.semaphoreInSnergy = null;
					SharedData.hMapFile = IntPtr.Zero;
					SharedData.pMapBuf = IntPtr.Zero;
				}
			}
		}
		private static bool CreateShareMemory(long maxGateways, long maxDevices, long maxChannels, long maxAttributes)
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return false;
			}
			SharedData.nMaxGateways = maxGateways;
			SharedData.nMaxDevices = maxDevices;
			SharedData.nMaxChannels = maxChannels;
			SharedData.nMaxAttributes = maxAttributes;
			int num = 0;
			num += (int)(SharedData.nMaxGateways * 35L);
			num += (int)(SharedData.nMaxDevices * 21L);
			num += (int)(SharedData.nMaxChannels * (7L + SharedData.nMaxAttributes * 20L));
			SharedData.MAX_SHARE_DATA_SIZE = (long)num;
			InSnergyService.PostLog(string.Concat(new object[]
			{
				"Header + data = ",
				8280L,
				"+",
				num,
				"=",
				(long)num + 8280L
			}));
			num += 8280;
			SharedData.semaphoreInSnergy.WaitOne();
			int num2 = SharedData.SetupShareMemoryWithSercurity(num);
			if (num2 < 0)
			{
				InSnergyService.PostLog("SetupShareMemory failed");
				SharedData.semaphoreInSnergy.Release();
				return false;
			}
			if (num2 > 0)
			{
				SharedData.nMaxGateways = SharedData.ReadShareLong(0L);
				SharedData.nMaxDevices = SharedData.ReadShareLong(8L);
				SharedData.nMaxChannels = SharedData.ReadShareLong(16L);
				SharedData.nMaxAttributes = SharedData.ReadShareLong(24L);
				num = (int)(SharedData.nMaxGateways * 35L);
				num += (int)(SharedData.nMaxDevices * 21L);
				num += (int)(SharedData.nMaxChannels * (7L + SharedData.nMaxAttributes * 20L));
				SharedData.MAX_SHARE_DATA_SIZE = (long)num;
			}
			else
			{
				SharedData.WriteShareLong(0L, SharedData.nMaxGateways);
				SharedData.WriteShareLong(8L, SharedData.nMaxDevices);
				SharedData.WriteShareLong(16L, SharedData.nMaxChannels);
				SharedData.WriteShareLong(24L, SharedData.nMaxAttributes);
				SharedData.WriteShareLong(64L, 1024L);
				SharedData.WriteShareLong(72L, 0L);
				SharedData.WriteShareLong(80L, 0L);
				SharedData.WriteShareLong(32L, 0L);
				SharedData.WriteShareLong(40L, 0L);
				SharedData.WriteShareLong(56L, 0L);
			}
			SharedData.semaphoreInSnergy.Release();
			return true;
		}
		private static int SetupShareMemoryWithSercurity(int nSize)
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
					InSnergyService.PostLog("CreateWellKnownSid " + Marshal.GetLastWin32Error());
					int result = -1;
					return result;
				}
				if (!Win32.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;;GA;;;RD)(A;;GA;;;S-1-5-32-544)(A;;GA;;;S-1-5-4)", 1, out sECURITY_ATTRIBUTES.lpSecurityDescriptor, IntPtr.Zero))
				{
					InSnergyService.PostLog("ConvertStringSecurityDescriptorToSecurityDescriptor " + Marshal.GetLastWin32Error());
					int result = -1;
					return result;
				}
				sECURITY_ATTRIBUTES.nLength = Marshal.SizeOf(sECURITY_ATTRIBUTES);
				sECURITY_ATTRIBUTES.bInheritHandle = false;
				SharedData.hMapFile = Win32.CreateFileMapping(4294967295u, ref sECURITY_ATTRIBUTES, 4, 0, nSize + 1024, "Global\\InSnergySharedName");
				flag = ((long)Marshal.GetLastWin32Error() == 183L);
				if (SharedData.hMapFile == IntPtr.Zero)
				{
					InSnergyService.PostLog("CreateFileMapping " + Marshal.GetLastWin32Error());
					int result = -1;
					return result;
				}
				SharedData.pMapBuf = Win32.MapViewOfFile(SharedData.hMapFile, 983071, 0, 0, nSize + 128);
				if (SharedData.pMapBuf == IntPtr.Zero)
				{
					InSnergyService.PostLog("MapViewOfFile " + Marshal.GetLastWin32Error());
					int result = -1;
					return result;
				}
			}
			catch (Exception ex)
			{
				InSnergyService.PostLog(ex.Message + " failed with '" + ex.InnerException.Message);
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
		private static void WriteShareArray(long nOffset, byte[] data)
		{
			IntPtr destination = new IntPtr(SharedData.pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(data, 0, destination, data.Length);
		}
		private static byte[] ReadShareArray(long nOffset, long nSize)
		{
			byte[] array = new byte[nSize];
			IntPtr source = new IntPtr(SharedData.pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(source, array, 0, array.Length);
			return array;
		}
		private static void WriteShareLong(long nOffset, long value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			IntPtr destination = new IntPtr(SharedData.pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(bytes, 0, destination, bytes.Length);
		}
		private static long ReadShareLong(long nOffset)
		{
			byte[] array = new byte[8];
			IntPtr source = new IntPtr(SharedData.pMapBuf.ToInt64() + nOffset);
			Marshal.Copy(source, array, 0, array.Length);
			return BitConverter.ToInt64(array, 0);
		}
		public static void LocalToShareEx(Dictionary<string, Gateway> gts, bool bRemoveOffline)
		{
			if (!SharedData.IsProducer())
			{
				return;
			}
			if (SharedData.semaphoreInSnergy == null)
			{
				return;
			}
			if (gts.Count <= 0)
			{
				lock (SharedData.thisSharedLock)
				{
					if (SharedData.semaphoreInSnergy != null)
					{
						SharedData.semaphoreInSnergy.WaitOne();
						SharedData.WriteShareLong(32L, 0L);
						SharedData.WriteShareLong(48L, 0L);
						SharedData.semaphoreInSnergy.Release();
					}
				}
				return;
			}
			byte[] array = new byte[SharedData.MAX_SHARE_DATA_SIZE + 1024L];
			Array.Clear(array, 0, array.Length);
			long num = 0L;
			long num2 = 0L;
			long num3 = 0L;
			long num4 = 0L;
			long num5 = 0L;
			foreach (KeyValuePair<string, Gateway> current in gts)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(current.Key);
				string[] array2 = current.Value.status.gatewayIP.Split(new char[]
				{
					':'
				});
				DateTime tUptime = current.Value.status.tUptime;
				if (ApplicationHandler.IsTopologyNotEmpty(current.Key) && (!bRemoveOffline || current.Value.status.gatewayIP.IndexOf(":0") < 0))
				{
					num5 += 1L;
					if (num2 >= SharedData.MAX_SHARE_DATA_SIZE || num3 > 0L)
					{
						num3 = 1L;
					}
					else
					{
						Buffer.BlockCopy(bytes, 0, array, (int)num2, 20);
						num2 += 20L;
						byte[] array3 = new byte[6];
						Array.Clear(array3, 0, array3.Length);
						if (array2.Length == 2)
						{
							string[] array4 = array2[0].Split(new char[]
							{
								'.'
							});
							if (array4.Length == 4)
							{
								array3[0] = Convert.ToByte(array4[0]);
								array3[1] = Convert.ToByte(array4[1]);
								array3[2] = Convert.ToByte(array4[2]);
								array3[3] = Convert.ToByte(array4[3]);
								array3[4] = (byte)(Convert.ToInt32(array2[1]) & 255);
								array3[5] = (byte)(Convert.ToInt32(array2[1]) >> 8);
							}
						}
						Buffer.BlockCopy(array3, 0, array, (int)num2, 6);
						num2 += 6L;
						long value = ((long)tUptime.Year << 40) + ((long)tUptime.Month << 32) + ((long)tUptime.Day << 24) + (long)((long)tUptime.Hour << 16) + (long)((long)tUptime.Minute << 8) + (long)tUptime.Second;
						byte[] bytes2 = BitConverter.GetBytes(value);
						Buffer.BlockCopy(bytes2, 0, array, (int)num2, 8);
						num2 += 8L;
						Array.Clear(array3, 0, array3.Length);
						array3[0] = Convert.ToByte(current.Value.listDevice.Count);
						Buffer.BlockCopy(array3, 0, array, (int)num2, 1);
						num2 += 1L;
						foreach (KeyValuePair<string, Device> current2 in current.Value.listDevice)
						{
							if (num2 >= SharedData.MAX_SHARE_DATA_SIZE)
							{
								num3 = 1L;
								InSnergyService.PostLog("OutOfMem #2");
								break;
							}
							byte[] bytes3 = Encoding.ASCII.GetBytes(current2.Value.sDID);
							Buffer.BlockCopy(bytes3, 0, array, (int)num2, 20);
							num2 += 20L;
							long num6 = num2;
							Array.Clear(array3, 0, array3.Length);
							array3[0] = Convert.ToByte(current2.Value.listChannel.Count);
							Buffer.BlockCopy(array3, 0, array, (int)num2, 1);
							num2 += 1L;
							long num7 = 0L;
							foreach (KeyValuePair<string, Channel> current3 in current2.Value.listChannel)
							{
								if (num2 >= SharedData.MAX_SHARE_DATA_SIZE)
								{
									num3 = 1L;
									InSnergyService.PostLog("OutOfMem #3");
									break;
								}
								int num8 = Convert.ToInt32(current3.Value.sCID);
								if (num8 >= 1 && num8 <= 12)
								{
									int num9 = current2.Value.mapChannel[num8 - 1];
									if (num9 != 0)
									{
										string text = num8.ToString();
										while (text.Length < 2)
										{
											text = "0" + text;
										}
										for (int i = 0; i < 3; i++)
										{
											string str = "0" + (i + 1).ToString();
											string text2 = (num9 >> 8 * (2 - i) & 255).ToString();
											while (text2.Length < 2)
											{
												text2 = "0" + text2;
											}
											if (!(text2 == "00"))
											{
												byte[] bytes4 = Encoding.ASCII.GetBytes(text + str + text2);
												Buffer.BlockCopy(bytes4, 0, array, (int)num2, 6);
												num2 += 6L;
												long num10 = num2;
												Array.Clear(array3, 0, array3.Length);
												array3[0] = Convert.ToByte(current3.Value.measurePair.Count);
												Buffer.BlockCopy(array3, 0, array, (int)num2, 1);
												num2 += 1L;
												long num11 = 0L;
												foreach (KeyValuePair<string, Param> current4 in current3.Value.measurePair)
												{
													int num12 = Convert.ToInt32(current4.Value.aID);
													if ((SharedData.listParamEnabled == null || SharedData.listParamEnabled.Contains(num12)) && num12 > 1000 && num12 / 1000 == i + 1)
													{
														if (num2 >= SharedData.MAX_SHARE_DATA_SIZE)
														{
															num3 = 1L;
															InSnergyService.PostLog("OutOfMem #4");
															break;
														}
														byte[] bytes5 = BitConverter.GetBytes(num12);
														Buffer.BlockCopy(bytes5, 0, array, (int)num2, 4);
														num2 += 4L;
														byte[] bytes6 = BitConverter.GetBytes(current4.Value.dvalue);
														Buffer.BlockCopy(bytes6, 0, array, (int)num2, 8);
														num2 += 8L;
														value = ((long)current4.Value.time.Year << 40) + ((long)current4.Value.time.Month << 32) + ((long)current4.Value.time.Day << 24) + (long)((long)current4.Value.time.Hour << 16) + (long)((long)current4.Value.time.Minute << 8) + (long)current4.Value.time.Second;
														bytes2 = BitConverter.GetBytes(value);
														Buffer.BlockCopy(bytes2, 0, array, (int)num2, 8);
														num2 += 8L;
														num11 += 1L;
													}
												}
												if (num3 > 0L)
												{
													break;
												}
												Array.Clear(array3, 0, array3.Length);
												array3[0] = Convert.ToByte(num11);
												Buffer.BlockCopy(array3, 0, array, (int)num10, 1);
												num7 += 1L;
											}
										}
									}
								}
							}
							if (num3 > 0L)
							{
								break;
							}
							Array.Clear(array3, 0, array3.Length);
							array3[0] = Convert.ToByte(num7);
							Buffer.BlockCopy(array3, 0, array, (int)num6, 1);
						}
						if (num3 <= 0L)
						{
							num += 1L;
							num4 = num2;
						}
					}
				}
			}
			Array.Resize<byte>(ref array, (int)num4);
			lock (SharedData.thisSharedLock)
			{
				if (SharedData.semaphoreInSnergy != null)
				{
					num += num5 << 32;
					SharedData.semaphoreInSnergy.WaitOne();
					SharedData.WriteShareLong(32L, num);
					SharedData.WriteShareLong(48L, num4);
					SharedData.WriteShareArray(8280L, array);
					long num13 = SharedData.ReadShareLong(56L);
					num13 |= 8L;
					SharedData.WriteShareLong(56L, num13);
					SharedData.semaphoreInSnergy.Release();
				}
			}
			if (num3 > 0L)
			{
				if (num3 != SharedData.nLastShareMemoryErrorCode)
				{
					InSnergyService.PostLog("Out of share-memory, data truncated");
				}
				SharedData.WriteStatus(2L, true);
			}
			else
			{
				SharedData.WriteStatus(2L, false);
			}
			SharedData.nLastShareMemoryErrorCode = num3;
		}
		public static long ShareToLocalEx(Dictionary<string, IGateway> gts)
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IGateway> current in gts)
			{
				list.Add(current.Key);
			}
			long result = 0L;
			byte[] array = null;
			long num;
			lock (SharedData.thisSharedLock)
			{
				if (SharedData.semaphoreInSnergy == null)
				{
					return result;
				}
				SharedData.semaphoreInSnergy.WaitOne();
				num = SharedData.ReadShareLong(32L);
				result = num >> 32;
				num &= (long)((ulong)-1);
				long nSize = SharedData.ReadShareLong(48L);
				array = SharedData.ReadShareArray(8280L, nSize);
				long num2 = SharedData.ReadShareLong(56L);
				num2 &= -9L;
				SharedData.WriteShareLong(56L, num2);
				SharedData.semaphoreInSnergy.Release();
			}
			long num3 = 0L;
			for (long num4 = 0L; num4 < num; num4 += 1L)
			{
				string @string = Encoding.ASCII.GetString(array, (int)num3, 20);
				num3 += 20L;
				long num5 = (long)((int)array[(int)checked((IntPtr)unchecked(num3 + 4L))] + ((int)array[(int)checked((IntPtr)unchecked(num3 + 5L))] << 8));
				string ip = string.Concat(checked(new object[]
				{
					array[(int)((IntPtr)num3)].ToString(),
					".",
					array[(int)((IntPtr)unchecked(num3 + 1L))].ToString(),
					".",
					array[(int)((IntPtr)unchecked(num3 + 2L))].ToString(),
					".",
					array[(int)((IntPtr)unchecked(num3 + 3L))].ToString(),
					":",
					num5
				}));
				num3 += 6L;
				long num6 = BitConverter.ToInt64(array, (int)num3);
				DateTime t = new DateTime((int)(num6 >> 40), (int)(num6 >> 32 & 255L), (int)(num6 >> 24 & 255L), (int)(num6 >> 16 & 255L), (int)(num6 >> 8 & 255L), (int)(num6 & 255L));
				num3 += 8L;
				IGateway gateway = new IGateway(@string, ip, t);
				long num7 = (long)((ulong)array[(int)checked((IntPtr)num3)]);
				num3 += 1L;
				for (long num8 = 0L; num8 < num7; num8 += 1L)
				{
					string string2 = Encoding.ASCII.GetString(array, (int)num3, 20);
					num3 += 20L;
					IBranch branch = new IBranch(string2);
					long num9 = (long)((ulong)array[(int)checked((IntPtr)num3)]);
					num3 += 1L;
					int num10 = 0;
					while ((long)num10 < num9)
					{
						string string3 = Encoding.ASCII.GetString(array, (int)num3, 6);
						num3 += 6L;
						IMeter meter = new IMeter(string3);
						long num11 = (long)((ulong)array[(int)checked((IntPtr)num3)]);
						num3 += 1L;
						int num12 = 0;
						while ((long)num12 < num11)
						{
							int num13 = BitConverter.ToInt32(array, (int)num3);
							if (num13 > 1000)
							{
								num13 %= 100;
							}
							num3 += 4L;
							double d = BitConverter.ToDouble(array, (int)num3);
							num3 += 8L;
							num6 = BitConverter.ToInt64(array, (int)num3);
							t = new DateTime((int)(num6 >> 40), (int)(num6 >> 32 & 255L), (int)(num6 >> 24 & 255L), (int)(num6 >> 16 & 255L), (int)(num6 >> 8 & 255L), (int)(num6 & 255L));
							IParam value = new IParam(num13, d, t);
							num3 += 8L;
							meter.listParam.Add(num13, value);
							num12++;
						}
						branch.listChannel.Add(string3, meter);
						num10++;
					}
					gateway.listDevice.Add(string2, branch);
				}
				if (gts.ContainsKey(@string))
				{
					gts[@string].Update(gateway);
				}
				else
				{
					gts.Add(@string, gateway);
				}
				if (list.Contains(@string))
				{
					list.Remove(@string);
				}
			}
			foreach (string current2 in list)
			{
				gts.Remove(current2);
			}
			return result;
		}
		public static long ReadStatus()
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return 0L;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			long result = SharedData.ReadShareLong(56L);
			SharedData.semaphoreInSnergy.Release();
			return result;
		}
		public static void WriteStatus(long Event, bool bSet)
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			long num = SharedData.ReadShareLong(56L);
			if (bSet)
			{
				num |= Event;
			}
			else
			{
				num &= ~Event;
			}
			SharedData.WriteShareLong(56L, num);
			SharedData.semaphoreInSnergy.Release();
		}
		public static bool IsServiceStarted()
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return false;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			long num = SharedData.ReadShareLong(40L);
			SharedData.semaphoreInSnergy.Release();
			return num > 0L;
		}
		public static void ServiceStarted(long nStarted)
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			SharedData.WriteShareLong(40L, nStarted);
			if (nStarted == 0L)
			{
				SharedData.WriteShareLong(32L, 0L);
			}
			SharedData.semaphoreInSnergy.Release();
		}
		public static bool QPushLong(long nData)
		{
			if (SharedData.semaphoreInSnergy == null)
			{
				return false;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			long num = SharedData.ReadShareLong(64L);
			long num2 = SharedData.ReadShareLong(72L);
			long num3 = SharedData.ReadShareLong(80L);
			if ((num2 + 1L) % num == num3)
			{
				return false;
			}
			SharedData.WriteShareLong(88L + num2 * 8L, nData);
			SharedData.WriteShareLong(72L, (num2 + 1L) % num);
			SharedData.semaphoreInSnergy.Release();
			InSnergyService.EventAdded();
			return true;
		}
		public static List<long> QPopLong(int nMaxPop)
		{
			List<long> list = new List<long>();
			if (SharedData.semaphoreInSnergy == null)
			{
				return list;
			}
			SharedData.semaphoreInSnergy.WaitOne();
			long num = SharedData.ReadShareLong(64L);
			long num2 = SharedData.ReadShareLong(72L);
			long num3 = SharedData.ReadShareLong(80L);
			long num4 = 0L;
			while (num2 != num3)
			{
				long item = SharedData.ReadShareLong(88L + num3 * 8L);
				list.Add(item);
				num3 = (num3 + 1L) % num;
				num4 += 1L;
				if (nMaxPop != 0 && num4 >= (long)nMaxPop)
				{
					break;
				}
			}
			if (list.Count > 0)
			{
				SharedData.WriteShareLong(80L, num3);
			}
			SharedData.semaphoreInSnergy.Release();
			return list;
		}
	}
}
