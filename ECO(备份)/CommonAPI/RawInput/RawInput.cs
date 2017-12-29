using CommonAPI;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
namespace RawInput
{
	public class RawInput : NativeWindow
	{
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private class PreMessageFilter : IMessageFilter
		{
			public bool PreFilterMessage(ref Message m)
			{
				return m.Msg == 255 && RawInput._rawinputDriver.ProcessRawInput(m.LParam);
			}
		}
		private static RawInputDriver _rawinputDriver;
		private readonly IntPtr _devNotifyHandle;
		private static readonly Guid AllUsbDeviceInterfaceHid = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");
		private RawInput.PreMessageFilter _filter;
		public event RawInputDriver.DeviceEventHandler InputEvent
		{
			add
			{
				RawInput._rawinputDriver.InputEvent += value;
			}
			remove
			{
				RawInput._rawinputDriver.InputEvent -= value;
			}
		}
		public int NumberOfKeyboards
		{
			get
			{
				return RawInput._rawinputDriver.NumberOfKeyboards;
			}
		}
		public bool CaptureOnlyIfTopMostWindow
		{
			get
			{
				return RawInput._rawinputDriver.CaptureOnlyIfTopMostWindow;
			}
			set
			{
				RawInput._rawinputDriver.CaptureOnlyIfTopMostWindow = value;
			}
		}
		public void AddMessageFilter()
		{
			if (this._filter != null)
			{
				return;
			}
			this._filter = new RawInput.PreMessageFilter();
			Application.AddMessageFilter(this._filter);
		}
		public void RemoveMessageFilter()
		{
			if (this._filter == null)
			{
				return;
			}
			Application.RemoveMessageFilter(this._filter);
		}
		public RawInput()
		{
			throw new NotSupportedException("Call the overloaded contructor with a Window handle.");
		}
		public RawInput(IntPtr parentHandle)
		{
			try
			{
				base.AssignHandle(parentHandle);
				RawInput._rawinputDriver = new RawInputDriver(parentHandle);
				RawInput._rawinputDriver.EnumerateDevices();
				this._devNotifyHandle = RawInput.RegisterForDeviceNotifications(parentHandle);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message + "\r\n" + ex.StackTrace);
			}
		}
		private static IntPtr RegisterForDeviceNotifications(IntPtr parent)
		{
			IntPtr intPtr = IntPtr.Zero;
			BroadcastDeviceInterface broadcastDeviceInterface = default(BroadcastDeviceInterface);
			broadcastDeviceInterface.dbcc_size = Marshal.SizeOf(broadcastDeviceInterface);
			broadcastDeviceInterface.BroadcastDeviceType = BroadcastDeviceType.DBT_DEVTYP_DEVICEINTERFACE;
			broadcastDeviceInterface.dbcc_classguid = RawInput.AllUsbDeviceInterfaceHid;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BroadcastDeviceInterface)));
				Marshal.StructureToPtr(broadcastDeviceInterface, intPtr2, false);
				intPtr = Win32.RegisterDeviceNotification(parent, intPtr2, DeviceNotification.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
			}
			catch (Exception ex)
			{
				DebugCenter.GetInstance().appendToFile(ex.Message + "\r\n" + ex.StackTrace);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr == IntPtr.Zero)
			{
				DebugCenter.GetInstance().appendToFile("Registration for device notifications Failed. Error: " + Marshal.GetLastWin32Error().ToString());
			}
			return intPtr;
		}
		protected override void WndProc(ref Message message)
		{
			int msg = message.Msg;
			if (msg != 255)
			{
				if (msg == 537)
				{
					RawInput._rawinputDriver.EnumerateDevices();
				}
			}
			else
			{
				RawInput._rawinputDriver.ProcessRawInput(message.LParam);
			}
			base.WndProc(ref message);
		}
		~RawInput()
		{
			Win32.UnregisterDeviceNotification(this._devNotifyHandle);
		}
	}
}
