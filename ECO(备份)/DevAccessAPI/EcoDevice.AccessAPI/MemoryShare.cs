using System;
using System.Runtime.InteropServices;
namespace EcoDevice.AccessAPI
{
	public class MemoryShare : System.IDisposable
	{
		private const int ERROR_ALREADY_EXISTS = 183;
		private const int FILE_MAP_COPY = 1;
		private const int FILE_MAP_WRITE = 2;
		private const int FILE_MAP_READ = 4;
		private const int FILE_MAP_ALL_ACCESS = 6;
		private const int PAGE_READONLY = 2;
		private const int PAGE_READWRITE = 4;
		private const int PAGE_WRITECOPY = 8;
		private const int PAGE_EXECUTE = 16;
		private const int PAGE_EXECUTE_READ = 32;
		private const int PAGE_EXECUTE_READWRITE = 64;
		private const int SEC_COMMIT = 134217728;
		private const int SEC_IMAGE = 16777216;
		private const int SEC_NOCACHE = 268435456;
		private const int SEC_RESERVE = 67108864;
		private const int INVALID_HANDLE_VALUE = -1;
		private const int maxSize = 209715200;
		private System.IntPtr fileHanlder = System.IntPtr.Zero;
		private System.IntPtr m_pwData = System.IntPtr.Zero;
		private bool existed;
		private int memorySize;
		private string name;
		public int MemorySize
		{
			get
			{
				return this.memorySize;
			}
			set
			{
				if (value <= 0)
				{
					throw new System.ArgumentException("The size should be greater than 0.");
				}
				if (value > 209715200)
				{
					throw new System.ArgumentException("The size should be less than " + 209715200 + ".");
				}
				this.memorySize = value;
			}
		}
		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr SendMessage(System.IntPtr hWnd, int Msg, int wParam, System.IntPtr lParam);
		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr CreateFileMapping(int hFile, System.IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);
		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr OpenFileMapping(int dwDesiredAccess, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool bInheritHandle, string lpName);
		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr MapViewOfFile(System.IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool UnmapViewOfFile(System.IntPtr pvBaseAddress);
		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(System.IntPtr handle);
		[System.Runtime.InteropServices.DllImport("Kernel32")]
		public static extern int GetLastError();
		public MemoryShare(string name)
		{
			if (string.IsNullOrEmpty(name) || name.Length < 1)
			{
				throw new System.ArgumentException("The name can not be null or empty.");
			}
			this.name = name;
		}
		public int Create()
		{
			this.fileHanlder = MemoryShare.CreateFileMapping(-1, System.IntPtr.Zero, 4u, 0u, (uint)this.memorySize, this.name);
			if (this.fileHanlder == System.IntPtr.Zero)
			{
				this.existed = false;
				return 0;
			}
			if (MemoryShare.GetLastError() == 183)
			{
				this.existed = true;
			}
			else
			{
				this.existed = false;
			}
			if (!this.existed)
			{
				this.m_pwData = MemoryShare.MapViewOfFile(this.fileHanlder, 2u, 0u, 0u, (uint)this.memorySize);
				if (this.m_pwData == System.IntPtr.Zero)
				{
					MemoryShare.CloseHandle(this.fileHanlder);
					return 0;
				}
				this.existed = true;
			}
			return 1;
		}
		private int open()
		{
			this.fileHanlder = MemoryShare.OpenFileMapping(4, false, this.name);
			if (this.fileHanlder == System.IntPtr.Zero)
			{
				MemoryShare.GetLastError();
				return 0;
			}
			this.existed = true;
			this.m_pwData = MemoryShare.MapViewOfFile(this.fileHanlder, 4u, 0u, 0u, (uint)this.memorySize);
			if (this.m_pwData == System.IntPtr.Zero)
			{
				MemoryShare.GetLastError();
				return 0;
			}
			return 1;
		}
		public void Close()
		{
			try
			{
				if (this.existed)
				{
					MemoryShare.UnmapViewOfFile(this.m_pwData);
					MemoryShare.CloseHandle(this.fileHanlder);
					this.existed = false;
				}
			}
			catch (System.Exception)
			{
			}
		}
		public void Dispose()
		{
			this.Close();
		}
		public int Read(byte[] data, int startAddr, int length)
		{
			if (startAddr + length > this.memorySize)
			{
				return 2;
			}
			if (this.open() == 1)
			{
				try
				{
					System.Runtime.InteropServices.Marshal.Copy(this.m_pwData, data, startAddr, length);
					return 1;
				}
				catch (System.Exception ex)
				{
					string arg_2F_0 = ex.Message;
				}
				finally
				{
					this.Dispose();
				}
				return 0;
			}
			return 0;
		}
		public int Write(byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				throw new System.ArgumentException("The data is null or empty.");
			}
			if (data.Length > this.memorySize)
			{
				throw new System.ArgumentException("The length is larger than the memory size(" + this.memorySize + ")");
			}
			byte[] array = new byte[this.memorySize];
			System.Array.Copy(data, array, data.Length);
			return this.Write(array, 0, this.memorySize);
		}
		public int Write(byte[] data, int startAddr, int length)
		{
			if (startAddr + length > this.memorySize)
			{
				return 3;
			}
			if (!this.existed)
			{
				return 2;
			}
			try
			{
				System.Runtime.InteropServices.Marshal.Copy(data, startAddr, this.m_pwData, length);
				return 1;
			}
			catch (System.Exception)
			{
			}
			return 0;
		}
	}
}
