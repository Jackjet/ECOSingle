using Dispatcher;
using ecoProtocols;
using System;
using System.IO;
using System.IO.Compression;
namespace Packing
{
	public class Compression
	{
		public delegate void CompressCallback(int type, int algorithm, byte[] result);
		public struct CompressContext
		{
			public int msgType;
			public int algorithm;
			public byte[] sourceData;
			public Compression.CompressCallback cbCompress;
		}
		public static void CompressThread(object threadInfo)
		{
			DispatchAttribute dispatchAttribute = (DispatchAttribute)threadInfo;
			byte[] data = dispatchAttribute.data;
			byte[] array = null;
			if (DispatchAPI.IsLocalConnection())
			{
				dispatchAttribute.algorithm = 0;
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					if (dispatchAttribute.algorithm != 2)
					{
						if (dispatchAttribute.algorithm == 1)
						{
							using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
							{
								gZipStream.Write(data, 0, data.Length);
								gZipStream.Close();
								memoryStream.Close();
								array = memoryStream.ToArray();
								gZipStream.Dispose();
								memoryStream.Dispose();
								goto IL_8E;
							}
						}
						if (dispatchAttribute.algorithm == 0)
						{
							array = dispatchAttribute.data;
						}
					}
					IL_8E:;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("CompressThread: " + ex.Message, new string[0]);
				array = null;
			}
			if (array == null || array.Length == 0)
			{
				return;
			}
			double num = (double)array.Length;
			num /= (double)data.Length;
			if (array != null)
			{
				if (dispatchAttribute.type == 1024)
				{
					byte[] bytes = BitConverter.GetBytes(dispatchAttribute.cid);
					byte[] array2 = new byte[8 + array.Length];
					Array.Copy(bytes, 0, array2, 0, bytes.Length);
					Array.Copy(array, 0, array2, 8, array.Length);
					array = array2;
				}
				dispatchAttribute.data = array;
				if (dispatchAttribute.cbCacheProcess != null)
				{
					dispatchAttribute.cbCacheProcess(dispatchAttribute);
				}
				if (dispatchAttribute.cbCallBack != null)
				{
					dispatchAttribute.cbCallBack(dispatchAttribute);
				}
			}
		}
		public static void DecompressThread(object threadInfo)
		{
			DispatchAttribute dispatchAttribute = (DispatchAttribute)threadInfo;
			byte[] data = dispatchAttribute.data;
			byte[] array = null;
			try
			{
				int num = 0;
				int num2 = dispatchAttribute.data.Length;
				if (dispatchAttribute.type == 1024)
				{
					long cid = BitConverter.ToInt64(dispatchAttribute.data, 0);
					dispatchAttribute.cid = cid;
					num = 8;
					num2 -= 8;
				}
				if (dispatchAttribute.algorithm != 2)
				{
					if (dispatchAttribute.algorithm == 1)
					{
						try
						{
							using (MemoryStream memoryStream = new MemoryStream(data, num, num2))
							{
								GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
								MemoryStream memoryStream2 = new MemoryStream();
								gZipStream.CopyTo(memoryStream2);
								memoryStream.Close();
								memoryStream2.Close();
								memoryStream2.Close();
								array = memoryStream2.ToArray();
								gZipStream.Dispose();
								memoryStream.Dispose();
								memoryStream2.Dispose();
							}
							goto IL_E7;
						}
						catch (Exception)
						{
							array = null;
							goto IL_E7;
						}
					}
					if (dispatchAttribute.algorithm == 0)
					{
						array = new byte[num2];
						Array.Copy(data, num, array, 0, num2);
					}
				}
				IL_E7:;
			}
			catch (Exception ex)
			{
				Common.WriteLine("DecompressThread: {0}", new string[]
				{
					ex.Message
				});
				array = null;
			}
			if (array == null || array.Length == 0)
			{
				return;
			}
			double num3 = (double)array.Length;
			num3 /= (double)data.Length;
			if (dispatchAttribute.cbCallBack != null)
			{
				dispatchAttribute.data = array;
				dispatchAttribute.cbCallBack(dispatchAttribute);
			}
		}
	}
}
