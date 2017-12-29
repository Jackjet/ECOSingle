using System;
using System.IO;
namespace CommonAPI.Tools
{
	public class DiskInfo
	{
		private string path;
		private bool isFile;
		private DriveInfo _drive;
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
					throw new ArgumentNullException("The path is null or emtpy.");
				}
				if (File.Exists(path))
				{
					this.isFile = true;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public long SizeOfCurrent()
		{
			if (this.isFile)
			{
				return new FileInfo(this.path).Length;
			}
			return this.getSizeOfDir(this.path);
		}
		private long getSizeOfDir(string path)
		{
			long num = 0L;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (!directoryInfo.Exists)
				{
					return 0L;
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				DirectoryInfo[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					DirectoryInfo directoryInfo2 = array[i];
					num += this.getSizeOfDir(directoryInfo2.FullName);
				}
				FileInfo[] files = directoryInfo.GetFiles();
				FileInfo[] array2 = files;
				for (int j = 0; j < array2.Length; j++)
				{
					FileInfo fileInfo = array2[j];
					num += fileInfo.Length;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return num;
		}
		public long DiskTotalSpace()
		{
			DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.TotalSize;
		}
		private DriveInfo getCurrentDrive()
		{
			if (this._drive == null)
			{
				try
				{
					string pathRoot = Path.GetPathRoot(this.path);
					if (string.IsNullOrEmpty(pathRoot))
					{
						throw new Exception("The path \"" + this.path + "\" is invalid.");
					}
					DriveInfo[] drives = DriveInfo.GetDrives();
					DriveInfo[] array = drives;
					for (int i = 0; i < array.Length; i++)
					{
						DriveInfo driveInfo = array[i];
						if (driveInfo.Name.StartsWith(pathRoot.ToUpper()) || driveInfo.Name.StartsWith(pathRoot))
						{
							this._drive = driveInfo;
							break;
						}
					}
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			if (this._drive == null)
			{
				throw new Exception("The path \"" + this.path + "\" is invalid.");
			}
			return this._drive;
		}
		public long DiskFreeSpace()
		{
			DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.TotalFreeSpace;
		}
		public long DiskAvailableFreeSpace()
		{
			DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.AvailableFreeSpace;
		}
		public string DriverNm()
		{
			DriveInfo currentDrive = this.getCurrentDrive();
			return currentDrive.Name;
		}
	}
}
