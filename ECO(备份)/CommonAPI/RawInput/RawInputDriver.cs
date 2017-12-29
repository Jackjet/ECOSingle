using CommonAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
namespace RawInput
{
	public sealed class RawInputDriver
	{
		public delegate void DeviceEventHandler(object sender, InputEventArg e);
		private readonly Dictionary<IntPtr, RawInputEvent> _deviceList = new Dictionary<IntPtr, RawInputEvent>();
		private readonly object _padLock = new object();
		private static InputData _rawBuffer;
		private IntPtr _parentWnd = (IntPtr)0;
		private uint _parentProcessID;
		public event RawInputDriver.DeviceEventHandler InputEvent;
		public int NumberOfKeyboards
		{
			get;
			private set;
		}
		public bool CaptureOnlyIfTopMostWindow
		{
			get;
			set;
		}
		public RawInputDriver(IntPtr hwnd)
		{
			RawInputDevice[] array = new RawInputDevice[2];
			array[0].UsagePage = HidUsagePage.GENERIC;
			array[0].Usage = HidUsage.Keyboard;
			array[0].Flags = RawInputDeviceFlags.INPUTSINK;
			array[0].Target = hwnd;
			array[1].UsagePage = HidUsagePage.GENERIC;
			array[1].Usage = HidUsage.Mouse;
			array[1].Flags = RawInputDeviceFlags.INPUTSINK;
			array[1].Target = hwnd;
			if (!Win32.RegisterRawInputDevices(array, (uint)array.Length, (uint)Marshal.SizeOf(array[0])))
			{
				DebugCenter.GetInstance().appendToFile("Failed to register raw input device(s).");
				return;
			}
			this._parentWnd = hwnd;
			Win32.GetWindowThreadProcessId(this._parentWnd, out this._parentProcessID);
		}
		public void EnumerateDevices()
		{
			lock (this._padLock)
			{
				try
				{
					this._deviceList.Clear();
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					uint num5 = 0u;
					int num6 = Marshal.SizeOf(typeof(Rawinputdevicelist));
					if (Win32.GetRawInputDeviceList(IntPtr.Zero, ref num5, (uint)num6) == 0u)
					{
						IntPtr intPtr = Marshal.AllocHGlobal((int)((long)num6 * (long)((ulong)num5)));
						Win32.GetRawInputDeviceList(intPtr, ref num5, (uint)num6);
						int num7 = 0;
						while ((long)num7 < (long)((ulong)num5))
						{
							uint num8 = 0u;
							Rawinputdevicelist rawinputdevicelist = (Rawinputdevicelist)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(num6 * num7)), typeof(Rawinputdevicelist));
							Win32.GetRawInputDeviceInfo(rawinputdevicelist.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref num8);
							if (num8 > 0u)
							{
								IntPtr intPtr2 = Marshal.AllocHGlobal((int)num8);
								Win32.GetRawInputDeviceInfo(rawinputdevicelist.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, intPtr2, ref num8);
								string device = Marshal.PtrToStringAnsi(intPtr2);
								if (rawinputdevicelist.dwType == 1u)
								{
									string deviceDescription = Win32.GetDeviceDescription(device);
									RawInputEvent value = new RawInputEvent
									{
										DeviceName = Marshal.PtrToStringAnsi(intPtr2),
										DeviceHandle = rawinputdevicelist.hDevice,
										DeviceType = Win32.GetDeviceType(rawinputdevicelist.dwType),
										Name = deviceDescription,
										Source = num++.ToString(CultureInfo.InvariantCulture)
									};
									if (!this._deviceList.ContainsKey(rawinputdevicelist.hDevice))
									{
										num4++;
										this._deviceList.Add(rawinputdevicelist.hDevice, value);
									}
								}
								else
								{
									if (rawinputdevicelist.dwType == 0u)
									{
										string deviceDescription2 = Win32.GetDeviceDescription(device);
										RawInputEvent value2 = new RawInputEvent
										{
											DeviceName = Marshal.PtrToStringAnsi(intPtr2),
											DeviceHandle = rawinputdevicelist.hDevice,
											DeviceType = Win32.GetDeviceType(rawinputdevicelist.dwType),
											Name = deviceDescription2,
											Source = num2++.ToString(CultureInfo.InvariantCulture)
										};
										if (!this._deviceList.ContainsKey(rawinputdevicelist.hDevice))
										{
											num4++;
											this._deviceList.Add(rawinputdevicelist.hDevice, value2);
										}
									}
									else
									{
										if (rawinputdevicelist.dwType == 2u)
										{
											string deviceDescription3 = Win32.GetDeviceDescription(device);
											RawInputEvent value3 = new RawInputEvent
											{
												DeviceName = Marshal.PtrToStringAnsi(intPtr2),
												DeviceHandle = rawinputdevicelist.hDevice,
												DeviceType = Win32.GetDeviceType(rawinputdevicelist.dwType),
												Name = deviceDescription3,
												Source = num3++.ToString(CultureInfo.InvariantCulture)
											};
											if (!this._deviceList.ContainsKey(rawinputdevicelist.hDevice))
											{
												num4++;
												this._deviceList.Add(rawinputdevicelist.hDevice, value3);
											}
										}
									}
								}
								Marshal.FreeHGlobal(intPtr2);
							}
							num7++;
						}
						Marshal.FreeHGlobal(intPtr);
						this.NumberOfKeyboards = num4;
						string msg = string.Format("EnumerateDevices() found {0}, Keyboard({1}), Mouse({2}), HID({3})", new object[]
						{
							this.NumberOfKeyboards.ToString(),
							num.ToString(),
							num2.ToString(),
							num3.ToString()
						});
						DebugCenter.GetInstance().appendToFile(msg);
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile(ex.Message + "\r\n" + ex.StackTrace);
				}
			}
		}
		public bool isForegroundProcess()
		{
			IntPtr foregroundWindow = Win32.GetForegroundWindow();
			uint num = 0u;
			Win32.GetWindowThreadProcessId(foregroundWindow, out num);
			return num == this._parentProcessID;
		}
		public bool ProcessRawInput(IntPtr hdevice)
		{
			if (this._deviceList.Count == 0)
			{
				return false;
			}
			if (this.CaptureOnlyIfTopMostWindow && !this.isForegroundProcess())
			{
				return false;
			}
			int num = 0;
			Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref num, Marshal.SizeOf(typeof(Rawinputheader)));
			if (num != Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, out RawInputDriver._rawBuffer, ref num, Marshal.SizeOf(typeof(Rawinputheader))))
			{
				return false;
			}
			if (RawInputDriver._rawBuffer.header.dwType != 2u)
			{
				if (RawInputDriver._rawBuffer.header.dwType == 1u)
				{
					int vKey = (int)RawInputDriver._rawBuffer.data.keyboard.VKey;
					int makecode = (int)RawInputDriver._rawBuffer.data.keyboard.Makecode;
					int flags = (int)RawInputDriver._rawBuffer.data.keyboard.Flags;
					if (vKey == 255)
					{
						return false;
					}
					bool isE0BitSet = (flags & 2) != 0;
					if (this._deviceList.ContainsKey(RawInputDriver._rawBuffer.header.hDevice))
					{
						RawInputEvent rawInputEvent;
						lock (this._padLock)
						{
							rawInputEvent = this._deviceList[RawInputDriver._rawBuffer.header.hDevice];
							goto IL_144;
						}
						return false;
						IL_144:
						bool flag2 = (flags & 1) != 0;
						rawInputEvent.RawInputData = RawInputDriver._rawBuffer;
						rawInputEvent.KeyPressState = (flag2 ? "BREAK" : "MAKE");
						rawInputEvent.Message = RawInputDriver._rawBuffer.data.keyboard.Message;
						rawInputEvent.VKeyName = KeyMapper.GetKeyName(RawInputDriver.VirtualKeyCorrection(vKey, isE0BitSet, makecode)).ToUpper();
						rawInputEvent.VKey = vKey;
						if (this.InputEvent != null)
						{
							this.InputEvent(this, new InputEventArg(rawInputEvent));
							return true;
						}
						return true;
					}
					return false;
				}
				else
				{
					if (RawInputDriver._rawBuffer.header.dwType == 0u)
					{
						if (this._deviceList.ContainsKey(RawInputDriver._rawBuffer.header.hDevice))
						{
							RawInputEvent rawInputEvent2;
							lock (this._padLock)
							{
								rawInputEvent2 = this._deviceList[RawInputDriver._rawBuffer.header.hDevice];
								goto IL_241;
							}
							return false;
							IL_241:
							rawInputEvent2.RawInputData = RawInputDriver._rawBuffer;
							rawInputEvent2.mouseMessage = (ulong)RawInputDriver._rawBuffer.data.mouse.usButtonFlags;
							if (this.InputEvent != null)
							{
								this.InputEvent(this, new InputEventArg(rawInputEvent2));
								return true;
							}
							return true;
						}
						return false;
					}
				}
			}
			return true;
		}
		public bool ProcessRawInput(IntPtr hdevice, out RawInputEvent inputEvent)
		{
			inputEvent = new RawInputEvent();
			inputEvent.Filter = false;
			if (this._deviceList.Count == 0)
			{
				return false;
			}
			if (this.CaptureOnlyIfTopMostWindow && !this.isForegroundProcess())
			{
				return false;
			}
			int num = 0;
			Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref num, Marshal.SizeOf(typeof(Rawinputheader)));
			if (num != Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, out RawInputDriver._rawBuffer, ref num, Marshal.SizeOf(typeof(Rawinputheader))))
			{
				return false;
			}
			int vKey = (int)RawInputDriver._rawBuffer.data.keyboard.VKey;
			int makecode = (int)RawInputDriver._rawBuffer.data.keyboard.Makecode;
			int flags = (int)RawInputDriver._rawBuffer.data.keyboard.Flags;
			if (vKey == 255)
			{
				return false;
			}
			bool isE0BitSet = (flags & 2) != 0;
			if (this._deviceList.ContainsKey(RawInputDriver._rawBuffer.header.hDevice))
			{
				lock (this._padLock)
				{
					inputEvent = this._deviceList[RawInputDriver._rawBuffer.header.hDevice];
					goto IL_129;
				}
				return false;
				IL_129:
				inputEvent.RawInputData = RawInputDriver._rawBuffer;
				if (inputEvent.DeviceType.Equals("KEYBOARD"))
				{
					bool flag2 = (flags & 1) != 0;
					inputEvent.KeyPressState = (flag2 ? "BREAK" : "MAKE");
					inputEvent.Message = RawInputDriver._rawBuffer.data.keyboard.Message;
					inputEvent.VKeyName = KeyMapper.GetKeyName(RawInputDriver.VirtualKeyCorrection(vKey, isE0BitSet, makecode)).ToUpper();
					inputEvent.VKey = vKey;
				}
				if (this.InputEvent != null)
				{
					this.InputEvent(this, new InputEventArg(inputEvent));
				}
				return true;
			}
			return false;
		}
		private static int VirtualKeyCorrection(int virtualKey, bool isE0BitSet, int makeCode)
		{
			int result = virtualKey;
			if (RawInputDriver._rawBuffer.header.hDevice == IntPtr.Zero)
			{
				if (RawInputDriver._rawBuffer.data.keyboard.VKey == 17)
				{
					result = 251;
				}
			}
			else
			{
				switch (virtualKey)
				{
				case 16:
					result = ((makeCode == 54) ? 161 : 160);
					break;
				case 17:
					result = (isE0BitSet ? 163 : 162);
					break;
				case 18:
					result = (isE0BitSet ? 165 : 164);
					break;
				default:
					result = virtualKey;
					break;
				}
			}
			return result;
		}
	}
}
