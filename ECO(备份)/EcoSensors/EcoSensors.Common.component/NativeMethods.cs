using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
namespace EcoSensors.Common.component
{
	internal static class NativeMethods
	{
		[System.Flags]
		internal enum AnimationFlags
		{
			Roll = 0,
			HorizontalPositive = 1,
			HorizontalNegative = 2,
			VerticalPositive = 4,
			VerticalNegative = 8,
			Center = 16,
			Hide = 65536,
			Activate = 131072,
			Slide = 262144,
			Blend = 524288,
			Mask = 1048575
		}
		internal struct MINMAXINFO
		{
			public Point reserved;
			public Size maxSize;
			public Point maxPosition;
			public Size minTrackSize;
			public Size maxTrackSize;
		}
		internal const int WM_NCHITTEST = 132;
		internal const int WM_NCACTIVATE = 134;
		internal const int WS_EX_TRANSPARENT = 32;
		internal const int WS_EX_TOOLWINDOW = 128;
		internal const int WS_EX_LAYERED = 524288;
		internal const int WS_EX_NOACTIVATE = 134217728;
		internal const int HTTRANSPARENT = -1;
		internal const int HTLEFT = 10;
		internal const int HTRIGHT = 11;
		internal const int HTTOP = 12;
		internal const int HTTOPLEFT = 13;
		internal const int HTTOPRIGHT = 14;
		internal const int HTBOTTOM = 15;
		internal const int HTBOTTOMLEFT = 16;
		internal const int HTBOTTOMRIGHT = 17;
		internal const int WM_PRINT = 791;
		internal const int WM_USER = 1024;
		internal const int WM_REFLECT = 8192;
		internal const int WM_COMMAND = 273;
		internal const int CBN_DROPDOWN = 7;
		internal const int WM_GETMINMAXINFO = 36;
		private static System.Runtime.InteropServices.HandleRef HWND_TOPMOST = new System.Runtime.InteropServices.HandleRef(null, new System.IntPtr(-1));
		private static bool? _isRunningOnMono;
		public static bool IsRunningOnMono
		{
			get
			{
				if (!NativeMethods._isRunningOnMono.HasValue)
				{
					NativeMethods._isRunningOnMono = new bool?(System.Type.GetType("Mono.Runtime") != null);
				}
				return NativeMethods._isRunningOnMono.Value;
			}
		}
		[System.Security.SuppressUnmanagedCodeSecurity]
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern int AnimateWindow(System.Runtime.InteropServices.HandleRef windowHandle, int time, NativeMethods.AnimationFlags flags);
		internal static void AnimateWindow(Control control, int time, NativeMethods.AnimationFlags flags)
		{
			try
			{
				System.Security.Permissions.SecurityPermission securityPermission = new System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				NativeMethods.AnimateWindow(new System.Runtime.InteropServices.HandleRef(control, control.Handle), time, flags);
			}
			catch (System.Security.SecurityException)
			{
			}
		}
		[System.Security.SuppressUnmanagedCodeSecurity]
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern bool SetWindowPos(System.Runtime.InteropServices.HandleRef hWnd, System.Runtime.InteropServices.HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);
		internal static void SetTopMost(Control control)
		{
			try
			{
				System.Security.Permissions.SecurityPermission securityPermission = new System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				NativeMethods.SetWindowPos(new System.Runtime.InteropServices.HandleRef(control, control.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
			}
			catch (System.Security.SecurityException)
			{
			}
		}
		internal static int HIWORD(int n)
		{
			return (int)((short)(n >> 16 & 65535));
		}
		internal static int HIWORD(System.IntPtr n)
		{
			return NativeMethods.HIWORD((int)((long)n));
		}
		internal static int LOWORD(int n)
		{
			return (int)((short)(n & 65535));
		}
		internal static int LOWORD(System.IntPtr n)
		{
			return NativeMethods.LOWORD((int)((long)n));
		}
	}
}
