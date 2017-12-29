using System;
using System.IO;
namespace EcoSensors.Common.Disk
{
	public class DiskInfo
	{
		private string path;
		private bool isFile;
		private System.IO.DriveInfo _drive;
		public DiskInfo(string path)
		{
			this.checkPath(path);
			this.path = path;
		}
		private void checkPath(string path)
		{
			try
			{
				if (string.IsNullOrEmpty(path))
				{
					throw new System.ArgumentNullException("The path is null or emtpy.");
				}
				if (System.IO.File.Exists(path))
				{
					this.isFile = true;
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		public long SizeOfCurrent()
		{
			if (this.isFile)
			{
				return new System.IO.FileInfo(this.path).Length;
			}
			return this.getSizeOfDir(this.path);
		}
		private long getSizeOfDir(string path)
		{
			long num = 0L;
			try
			{
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					return 0L;
				}
				System.IO.DirectoryInfo[] directories = directoryInfo.GetDirectories();
				System.IO.DirectoryInfo[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					System.IO.DirectoryInfo directoryInfo2 = array[i];
					num += this.getSizeOfDir(directoryInfo2.FullName);
				}
				System.IO.FileInfo[] files = directoryInfo.GetFiles();
				System.IO.FileInfo[] array2 = files;
				for (int j = 0; j < array2.Length; j++)
				{
					System.IO.FileInfo fileInfo = array2[j];
					num += fileInfo.Length;
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			return num;
		}
		public long DiskTotalSpace()
		{
			System.IO.DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.TotalSize;
		}
		private System.IO.DriveInfo getCurrentDrive()
		{
			if (this._drive == null)
			{
				try
				{
					string pathRoot = System.IO.Path.GetPathRoot(this.path);
					if (string.IsNullOrEmpty(pathRoot))
					{
						throw new System.Exception("The path \"" + this.path + "\" is invalid.");
					}
					System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
					System.IO.DriveInfo[] array = drives;
					for (int i = 0; i < array.Length; i++)
					{
						System.IO.DriveInfo driveInfo = array[i];
						if (driveInfo.Name.StartsWith(pathRoot.ToUpper()) || driveInfo.Name.StartsWith(pathRoot))
						{
							this._drive = driveInfo;
							break;
						}
					}
				}
				catch (System.Exception ex)
				{
					throw ex;
				}
			}
			if (this._drive == null)
			{
				throw new System.Exception("The path \"" + this.path + "\" is invalid.");
			}
			return this._drive;
		}
		public long DiskFreeSpace()
		{
			System.IO.DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.TotalFreeSpace;
		}
		public long DiskAvailableFreeSpace()
		{
			System.IO.DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.AvailableFreeSpace;
		}
		public string DriverNm()
		{
			System.IO.DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.Name;
		}
	}
}
