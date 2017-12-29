using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
namespace DBAccessAPI
{
	public class ZipArchive : IDisposable
	{
		public enum CompressionMethodEnum
		{
			Stored,
			Deflated
		}
		public enum DeflateOptionEnum
		{
			Normal,
			Maximum,
			Fast,
			SuperFast
		}
		public struct ZipFileInfo
		{
			internal object external;
			public string Name
			{
				get
				{
					return (string)this.GetProperty("Name");
				}
			}
			public DateTime LastModFileDateTime
			{
				get
				{
					return (DateTime)this.GetProperty("LastModFileDateTime");
				}
			}
			public bool FolderFlag
			{
				get
				{
					return (bool)this.GetProperty("FolderFlag");
				}
			}
			public bool VolumeLabelFlag
			{
				get
				{
					return (bool)this.GetProperty("VolumeLabelFlag");
				}
			}
			public object CompressionMethod
			{
				get
				{
					return this.GetProperty("CompressionMethod");
				}
			}
			public object DeflateOption
			{
				get
				{
					return this.GetProperty("DeflateOption");
				}
			}
			private object GetProperty(string name)
			{
				return this.external.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(this.external, null);
			}
			public override string ToString()
			{
				return this.Name;
			}
			public Stream GetStream()
			{
				FileMode fileMode = FileMode.Open;
				FileAccess fileAccess = FileAccess.Read;
				MethodInfo method = this.external.GetType().GetMethod("GetStream", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				return (Stream)method.Invoke(this.external, new object[]
				{
					fileMode,
					fileAccess
				});
			}
			public Stream SetStream()
			{
				FileMode fileMode = FileMode.Open;
				FileAccess fileAccess = FileAccess.ReadWrite;
				MethodInfo method = this.external.GetType().GetMethod("GetStream", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				return (Stream)method.Invoke(this.external, new object[]
				{
					fileMode,
					fileAccess
				});
			}
		}
		private object external;
		public IEnumerable<ZipArchive.ZipFileInfo> Files
		{
			get
			{
				MethodInfo method = this.external.GetType().GetMethod("GetFiles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				IEnumerable enumerable = method.Invoke(this.external, null) as IEnumerable;
				foreach (object current in enumerable)
				{
					yield return new ZipArchive.ZipFileInfo
					{
						external = current
					};
				}
				yield break;
			}
		}
		public IEnumerable<string> FileNames
		{
			get
			{
				return 
					from p in this.Files
					select p.Name into p
					orderby p
					select p;
			}
		}
		private ZipArchive()
		{
		}
		public static ZipArchive CreateZipFile(string path)
		{
			FileMode fileMode = FileMode.OpenOrCreate;
			FileAccess fileAccess = FileAccess.ReadWrite;
			FileShare fileShare = FileShare.None;
			bool flag = false;
			Type type = typeof(Package).Assembly.GetType("MS.Internal.IO.Zip.ZipArchive");
			MethodInfo method = type.GetMethod("OpenOnFile", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			return new ZipArchive
			{
				external = method.Invoke(null, new object[]
				{
					path,
					fileMode,
					fileAccess,
					fileShare,
					flag
				})
			};
		}
		public static ZipArchive OpenOnFile(string path)
		{
			FileMode fileMode = FileMode.Open;
			FileAccess fileAccess = FileAccess.Read;
			FileShare fileShare = FileShare.Read;
			bool flag = false;
			Type type = typeof(Package).Assembly.GetType("MS.Internal.IO.Zip.ZipArchive");
			MethodInfo method = type.GetMethod("OpenOnFile", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			return new ZipArchive
			{
				external = method.Invoke(null, new object[]
				{
					path,
					fileMode,
					fileAccess,
					fileShare,
					flag
				})
			};
		}
		public static ZipArchive OpenOnStream(Stream stream)
		{
			FileMode fileMode = FileMode.OpenOrCreate;
			FileAccess fileAccess = FileAccess.ReadWrite;
			bool flag = false;
			Type type = typeof(Package).Assembly.GetType("MS.Internal.IO.Zip.ZipArchive");
			MethodInfo method = type.GetMethod("OpenOnStream", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			return new ZipArchive
			{
				external = method.Invoke(null, new object[]
				{
					stream,
					fileMode,
					fileAccess,
					flag
				})
			};
		}
		public ZipArchive.ZipFileInfo AddFile(string path)
		{
			ZipArchive.CompressionMethodEnum compressionMethodEnum = ZipArchive.CompressionMethodEnum.Deflated;
			ZipArchive.DeflateOptionEnum deflateOptionEnum = ZipArchive.DeflateOptionEnum.Normal;
			Type type = this.external.GetType();
			MethodInfo method = type.GetMethod("AddFile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object value = type.Assembly.GetType("MS.Internal.IO.Zip.CompressionMethodEnum").GetField(compressionMethodEnum.ToString()).GetValue(null);
			object value2 = type.Assembly.GetType("MS.Internal.IO.Zip.DeflateOptionEnum").GetField(deflateOptionEnum.ToString()).GetValue(null);
			return new ZipArchive.ZipFileInfo
			{
				external = method.Invoke(this.external, new object[]
				{
					path,
					value,
					value2
				})
			};
		}
		public void DeleteFile(string name)
		{
			MethodInfo method = this.external.GetType().GetMethod("DeleteFile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			method.Invoke(this.external, new object[]
			{
				name
			});
		}
		public void Dispose()
		{
			((IDisposable)this.external).Dispose();
		}
		public ZipArchive.ZipFileInfo GetFile(string name)
		{
			MethodInfo method = this.external.GetType().GetMethod("GetFile", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return new ZipArchive.ZipFileInfo
			{
				external = method.Invoke(this.external, new object[]
				{
					name
				})
			};
		}
	}
}
