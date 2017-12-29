using System;
using System.IO;
namespace PDFjet.NET
{
	public class JPGImage
	{
		private const char M_SOF0 = 'À';
		private const char M_SOF1 = 'Á';
		private const char M_SOF2 = 'Â';
		private const char M_SOF3 = 'Ã';
		private const char M_SOF5 = 'Å';
		private const char M_SOF6 = 'Æ';
		private const char M_SOF7 = 'Ç';
		private const char M_SOF9 = 'É';
		private const char M_SOF10 = 'Ê';
		private const char M_SOF11 = 'Ë';
		private const char M_SOF13 = 'Í';
		private const char M_SOF14 = 'Î';
		private const char M_SOF15 = 'Ï';
		private int width;
		private int height;
		private long size;
		private int colorComponents;
		private byte[] data;
		private Stream stream;
		public JPGImage(JPGImage jpg, Stream stream)
		{
			if (jpg == null)
			{
				this.ReadJPGImage(stream);
				stream.Dispose();
				return;
			}
			this.width = jpg.width;
			this.height = jpg.height;
			this.size = jpg.size;
			this.colorComponents = jpg.colorComponents;
			this.stream = stream;
		}
		public JPGImage(Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[2048];
			int count;
			while ((count = stream.Read(array, 0, array.Length)) != 0)
			{
				memoryStream.Write(array, 0, count);
			}
			stream.Dispose();
			this.data = memoryStream.ToArray();
			this.ReadJPGImage(new MemoryStream(this.data));
		}
		internal Stream GetInputStream()
		{
			return this.stream;
		}
		internal int GetWidth()
		{
			return this.width;
		}
		internal int GetHeight()
		{
			return this.height;
		}
		public long GetFileSize()
		{
			return this.size;
		}
		internal int GetColorComponents()
		{
			return this.colorComponents;
		}
		internal byte[] GetData()
		{
			return this.data;
		}
		private void ReadJPGImage(Stream stream)
		{
			char c = (char)stream.ReadByte();
			char c2 = (char)stream.ReadByte();
			this.size += 2L;
			if (c == 'ÿ' && c2 == 'Ø')
			{
				bool flag = false;
				while (true)
				{
					switch (this.NextMarker(stream))
					{
					case 'À':
					case 'Á':
					case 'Â':
					case 'Ã':
					case 'Å':
					case 'Æ':
					case 'Ç':
					case 'É':
					case 'Ê':
					case 'Ë':
					case 'Í':
					case 'Î':
					case 'Ï':
						stream.ReadByte();
						stream.ReadByte();
						stream.ReadByte();
						this.size += 3L;
						this.height = this.readTwoBytes(stream);
						this.width = this.readTwoBytes(stream);
						this.colorComponents = stream.ReadByte();
						this.size += 1L;
						flag = true;
						break;
					case 'Ä':
					case 'È':
					case 'Ì':
						goto IL_EE;
					default:
						goto IL_EE;
					}
					IL_F5:
					if (flag)
					{
						break;
					}
					continue;
					IL_EE:
					this.SkipVariable(stream);
					goto IL_F5;
				}
				while (stream.ReadByte() != -1)
				{
					this.size += 1L;
				}
				return;
			}
			throw new Exception();
		}
		private int readTwoBytes(Stream stream)
		{
			int num = stream.ReadByte();
			num <<= 8;
			num |= stream.ReadByte();
			this.size += 2L;
			return num;
		}
		private char NextMarker(Stream stream)
		{
			int num = 0;
			char c = (char)stream.ReadByte();
			this.size += 1L;
			while (c != 'ÿ')
			{
				num++;
				c = (char)stream.ReadByte();
				this.size += 1L;
			}
			do
			{
				c = (char)stream.ReadByte();
				this.size += 1L;
			}
			while (c == 'ÿ');
			if (num != 0)
			{
				throw new Exception();
			}
			return c;
		}
		private void SkipVariable(Stream stream)
		{
			int i = this.readTwoBytes(stream);
			if (i < 2)
			{
				throw new Exception();
			}
			for (i -= 2; i > 0; i--)
			{
				stream.ReadByte();
				this.size += 1L;
			}
		}
	}
}
