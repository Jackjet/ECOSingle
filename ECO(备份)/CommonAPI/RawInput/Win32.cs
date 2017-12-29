using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
namespace RawInput
{
	public static class Win32
	{
		public struct KeyAndState
		{
			public int Key;
			public byte[] KeyboardState;
			public KeyAndState(int key, byte[] state)
			{
				this.Key = key;
				this.KeyboardState = state;
			}
		}
		public const int KEYBOARD_OVERRUN_MAKE_CODE = 255;
		public const int WM_APPCOMMAND = 793;
		private const int FAPPCOMMANDMASK = 61440;
		internal const int FAPPCOMMANDMOUSE = 32768;
		internal const int FAPPCOMMANDOEM = 4096;
		public const int WM_NCMOUSEMOVE = 160;
		public const int WM_NCLBUTTONDOWN = 161;
		public const int WM_NCLBUTTONUP = 162;
		public const int WM_NCLBUTTONDBLCLK = 163;
		public const int WM_NCRBUTTONDOWN = 164;
		public const int WM_NCRBUTTONUP = 165;
		public const int WM_NCRBUTTONDBLCLK = 166;
		public const int WM_NCMBUTTONDOWN = 167;
		public const int WM_NCMBUTTONUP = 168;
		public const int WM_NCMBUTTONDBLCLK = 169;
		public const int WM_MOUSEFIRST = 512;
		public const int WM_MOUSEMOVE = 512;
		public const int WM_LBUTTONDOWN = 513;
		public const int WM_LBUTTONUP = 514;
		public const int WM_LBUTTONDBLCLK = 515;
		public const int WM_RBUTTONDOWN = 516;
		public const int WM_RBUTTONUP = 517;
		public const int WM_RBUTTONDBLCLK = 518;
		public const int WM_MBUTTONDOWN = 519;
		public const int WM_MBUTTONUP = 520;
		public const int WM_MBUTTONDBLCLK = 521;
		public const int WM_MOUSEWHEEL = 522;
		public const int WM_XBUTTONDOWN = 523;
		public const int WM_XBUTTONUP = 524;
		public const int WM_XBUTTONDBLCLK = 525;
		public const int WM_MOUSEHWHEEL = 526;
		public const int WM_KEYDOWN = 256;
		public const int WM_KEYUP = 257;
		internal const int WM_SYSKEYDOWN = 260;
		public const int WM_INPUT = 255;
		public const int WM_USB_DEVICECHANGE = 537;
		internal const int VK_SHIFT = 16;
		internal const int RI_KEY_MAKE = 0;
		internal const int RI_KEY_BREAK = 1;
		internal const int RI_KEY_E0 = 2;
		internal const int RI_KEY_E1 = 4;
		internal const int VK_CONTROL = 17;
		internal const int VK_MENU = 18;
		internal const int VK_ZOOM = 251;
		internal const int VK_LSHIFT = 160;
		internal const int VK_RSHIFT = 161;
		internal const int VK_LCONTROL = 162;
		internal const int VK_RCONTROL = 163;
		internal const int VK_LMENU = 164;
		internal const int VK_RMENU = 165;
		internal const int SC_SHIFT_R = 54;
		internal const int SC_SHIFT_L = 42;
		internal const int RIM_INPUT = 0;
		public static int LoWord(int dwValue)
		{
			return dwValue & 65535;
		}
		public static int HiWord(long dwValue)
		{
			return (int)(dwValue >> 16) & -61441;
		}
		public static ushort LowWord(uint val)
		{
			return (ushort)val;
		}
		public static ushort HighWord(uint val)
		{
			return (ushort)(val >> 16);
		}
		public static uint BuildWParam(ushort low, ushort high)
		{
			return (uint)((int)high << 16 | (int)low);
		}
		[DllImport("User32.dll", SetLastError = true)]
		public static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, out InputData buffer, [In] [Out] ref int size, int cbSizeHeader);
		[DllImport("User32.dll", SetLastError = true)]
		public static extern int GetRawInputData(IntPtr hRawInput, DataCommand command, [Out] IntPtr pData, [In] [Out] ref int size, int sizeHeader);
		[DllImport("User32.dll", SetLastError = true)]
		internal static extern uint GetRawInputDeviceInfo(IntPtr hDevice, RawInputDeviceInfo command, IntPtr pData, ref uint size);
		[DllImport("user32.dll")]
		public static extern uint GetRawInputDeviceInfo(IntPtr hDevice, uint command, ref DeviceInfo data, ref uint dataSize);
		[DllImport("User32.dll", SetLastError = true)]
		public static extern uint GetRawInputDeviceList(IntPtr pRawInputDeviceList, ref uint numberDevices, uint size);
		[DllImport("User32.dll", SetLastError = true)]
		internal static extern bool RegisterRawInputDevices(RawInputDevice[] pRawInputDevice, uint numberDevices, uint size);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr notificationFilter, DeviceNotification flags);
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool UnregisterDeviceNotification(IntPtr handle);
		[DllImport("user32.dll")]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool GetKeyboardState(byte[] lpKeyState);
		[DllImport("user32.dll")]
		public static extern bool SetKeyboardState(byte[] lpKeyState);
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
		public static string DeviceAudit()
		{
			string text = "";
			FileStream fileStream = new FileStream("DeviceAudit.txt", FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			int num = 0;
			uint num2 = 0u;
			int num3 = Marshal.SizeOf(typeof(Rawinputdevicelist));
			if (Win32.GetRawInputDeviceList(IntPtr.Zero, ref num2, (uint)num3) == 0u)
			{
				IntPtr intPtr = Marshal.AllocHGlobal((int)((long)num3 * (long)((ulong)num2)));
				Win32.GetRawInputDeviceList(intPtr, ref num2, (uint)num3);
				int num4 = 0;
				while ((long)num4 < (long)((ulong)num2))
				{
					uint num5 = 0u;
					Rawinputdevicelist rawinputdevicelist = (Rawinputdevicelist)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(num3 * num4)), typeof(Rawinputdevicelist));
					Win32.GetRawInputDeviceInfo(rawinputdevicelist.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref num5);
					if (num5 <= 0u)
					{
						streamWriter.WriteLine("pcbSize: " + num5);
						streamWriter.WriteLine(Marshal.GetLastWin32Error().ToString());
						streamWriter.Flush();
						streamWriter.Close();
						fileStream.Close();
						return text;
					}
					uint num6 = (uint)Marshal.SizeOf(typeof(DeviceInfo));
					DeviceInfo deviceInfo = new DeviceInfo
					{
						Size = Marshal.SizeOf(typeof(DeviceInfo))
					};
					if (Win32.GetRawInputDeviceInfo(rawinputdevicelist.hDevice, 536870923u, ref deviceInfo, ref num6) <= 0u)
					{
						streamWriter.WriteLine(Marshal.GetLastWin32Error());
						streamWriter.Flush();
						streamWriter.Close();
						fileStream.Close();
						return text;
					}
					IntPtr intPtr2 = Marshal.AllocHGlobal((int)num5);
					Win32.GetRawInputDeviceInfo(rawinputdevicelist.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, intPtr2, ref num5);
					string device = Marshal.PtrToStringAnsi(intPtr2);
					if (rawinputdevicelist.dwType == 1u || rawinputdevicelist.dwType == 2u)
					{
						string deviceDescription = Win32.GetDeviceDescription(device);
						RawInputEvent rawInputEvent = new RawInputEvent
						{
							DeviceName = Marshal.PtrToStringAnsi(intPtr2),
							DeviceHandle = rawinputdevicelist.hDevice,
							DeviceType = Win32.GetDeviceType(rawinputdevicelist.dwType),
							Name = deviceDescription,
							Source = num++.ToString(CultureInfo.InvariantCulture)
						};
						streamWriter.WriteLine(rawInputEvent.ToString());
						streamWriter.WriteLine(deviceInfo.ToString());
						streamWriter.WriteLine(deviceInfo.KeyboardInfo.ToString());
						streamWriter.WriteLine(deviceInfo.HIDInfo.ToString());
						streamWriter.WriteLine("=========================================================================================================");
						text += string.Format("{0}\r\n", rawInputEvent.ToString());
						text += string.Format("{0}\r\n", deviceInfo.ToString());
						text += string.Format("{0}\r\n", deviceInfo.KeyboardInfo.ToString());
						text += string.Format("{0}\r\n", deviceInfo.HIDInfo.ToString());
						text += "=========================================================================================================\r\n";
					}
					Marshal.FreeHGlobal(intPtr2);
					num4++;
				}
				Marshal.FreeHGlobal(intPtr);
				streamWriter.Flush();
				streamWriter.Close();
				fileStream.Close();
				return text;
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		public static string GetDeviceType(uint device)
		{
			string result;
			switch (device)
			{
			case 0u:
				result = "MOUSE";
				break;
			case 1u:
				result = "KEYBOARD";
				break;
			case 2u:
				result = "HID";
				break;
			default:
				result = "UNKNOWN";
				break;
			}
			return result;
		}
		public static string GetDeviceDescription(string device)
		{
			RegistryKey deviceKey = RegistryAccess.GetDeviceKey(device);
			if (deviceKey != null)
			{
				string text = deviceKey.GetValue("DeviceDesc").ToString();
				if (text != null)
				{
					int num = text.IndexOf(';');
					if (num >= 0 && num < text.Length - 1)
					{
						return text.Substring(num + 1);
					}
					return string.Empty;
				}
			}
			return string.Empty;
		}
		public static bool InputInForeground(IntPtr wparam)
		{
			return wparam == Win32.GetForegroundWindow();
		}
	}
}
