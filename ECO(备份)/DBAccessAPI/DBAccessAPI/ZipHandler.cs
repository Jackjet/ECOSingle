using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
namespace DBAccessAPI
{
	public class ZipHandler
	{
		public static bool CompressByFiles(FileInfo fi, List<FileInfo> source_fi)
		{
			bool result = false;
			Package package = null;
			try
			{
				if (fi.Exists)
				{
					fi.Delete();
				}
				package = Package.Open(fi.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				foreach (FileInfo current in source_fi)
				{
					string fullName = current.FullName;
					Uri partUri = PackUriHelper.CreatePartUri(new Uri(current.Name, UriKind.Relative));
					PackagePart packagePart = package.CreatePart(partUri, "text/xml");
					FileStream fileStream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
					fileStream.CopyTo(packagePart.GetStream(), 8192);
					fileStream.Close();
				}
				package.Close();
				return true;
			}
			catch
			{
			}
			finally
			{
				if (package != null)
				{
					package.Close();
				}
			}
			return result;
		}
		public static void Compress(FileInfo fi, DirectoryInfo dir)
		{
			if (fi.Exists)
			{
				fi.Delete();
			}
			Package package = Package.Open(fi.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			FileInfo[] files = dir.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				FileInfo fileInfo = files[i];
				string fullName = fileInfo.FullName;
				Uri partUri = PackUriHelper.CreatePartUri(new Uri(fileInfo.Name, UriKind.Relative));
				PackagePart packagePart = package.CreatePart(partUri, "text/xml");
				byte[] array = File.ReadAllBytes(fullName);
				packagePart.GetStream().Write(array, 0, array.Length);
			}
			DirectoryInfo[] directories = dir.GetDirectories();
			for (int j = 0; j < directories.Length; j++)
			{
				DirectoryInfo directoryInfo = directories[j];
				FileInfo[] files2 = directoryInfo.GetFiles();
				for (int k = 0; k < files2.Length; k++)
				{
					FileInfo fileInfo2 = files2[k];
					string fullName2 = fileInfo2.FullName;
					Uri partUri2 = PackUriHelper.CreatePartUri(new Uri(directoryInfo.Name + "/" + fileInfo2.Name, UriKind.Relative));
					PackagePart packagePart2 = package.CreatePart(partUri2, "text/xml");
					byte[] array2 = File.ReadAllBytes(fullName2);
					packagePart2.GetStream().Write(array2, 0, array2.Length);
				}
			}
			package.Close();
		}
		public static bool Decompress(FileInfo fi, string origName)
		{
			Package package = null;
			bool result = false;
			try
			{
				string fullName = fi.FullName;
				package = Package.Open(fullName, FileMode.Open, FileAccess.ReadWrite);
				using (IEnumerator<PackagePart> enumerator = package.GetParts().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ZipPackagePart contentFile = (ZipPackagePart)enumerator.Current;
						ZipHandler.CreateFile(origName, contentFile);
						result = true;
					}
				}
				package.Close();
			}
			catch
			{
				return false;
			}
			finally
			{
				if (package != null)
				{
					package.Close();
				}
			}
			return result;
		}
		private static void CreateFile(string rootFolder, ZipPackagePart contentFile)
		{
			string text = string.Empty;
			text = contentFile.Uri.OriginalString.Replace('/', Path.DirectorySeparatorChar);
			if (text.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{
				text = text.TrimStart(new char[]
				{
					Path.DirectorySeparatorChar
				});
			}
			text = Path.Combine(rootFolder, text);
			if (!Directory.Exists(Path.GetDirectoryName(text)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(text));
			}
			FileStream fileStream = File.Create(text);
			contentFile.GetStream().CopyTo(fileStream, 8192);
			fileStream.Close();
		}
	}
}
