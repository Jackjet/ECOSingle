using System;
using System.IO;
namespace PDFjet.NET
{
	public class BMPImage
	{
		private const int m10000000 = 128;
		private const int m01000000 = 64;
		private const int m00100000 = 32;
		private const int m00010000 = 16;
		private const int m00001000 = 8;
		private const int m00000100 = 4;
		private const int m00000010 = 2;
		private const int m00000001 = 1;
		private const int m11110000 = 240;
		private const int m00001111 = 15;
		private int w;
		private int h;
		private byte[] image;
		private byte[] deflated;
		private int bpp;
		private byte[][] palette;
		private bool r5g6b5;
		public BMPImage(Stream stream)
		{
			byte[] bytes = this.getBytes(stream, 2);
			if ((bytes[0] == 66 && bytes[1] == 77) || (bytes[0] == 66 && bytes[1] == 65) || (bytes[0] == 67 && bytes[1] == 73) || (bytes[0] == 67 && bytes[1] == 80) || (bytes[0] == 73 && bytes[1] == 67) || (bytes[0] == 80 && bytes[1] == 84))
			{
				this.skipNBytes(stream, 8);
				int num = this.readSignedInt(stream);
				this.readSignedInt(stream);
				this.w = this.readSignedInt(stream);
				this.h = this.readSignedInt(stream);
				this.skipNBytes(stream, 2);
				this.bpp = this.read2BytesLE(stream);
				int num2 = this.readSignedInt(stream);
				if (this.bpp > 8)
				{
					this.r5g6b5 = (num2 == 3);
					this.skipNBytes(stream, 20);
					if (num > 54)
					{
						this.skipNBytes(stream, num - 54);
					}
				}
				else
				{
					this.skipNBytes(stream, 12);
					int num3 = this.readSignedInt(stream);
					if (num3 == 0)
					{
						num3 = (int)Math.Pow(2.0, (double)this.bpp);
					}
					this.skipNBytes(stream, 4);
					this.parsePalette(stream, num3);
				}
				this.parseData(stream);
				return;
			}
			throw new Exception("BMP data could not be parsed!");
		}
		private void parseData(Stream stream)
		{
			this.image = new byte[this.w * this.h * 3];
			int length = 4 * (int)Math.Ceiling((double)(this.bpp * this.w) / 32.0);
			try
			{
				int i = 0;
				while (i < this.h)
				{
					byte[] array = this.getBytes(stream, length);
					int num = this.bpp;
					if (num <= 8)
					{
						if (num != 1)
						{
							if (num != 4)
							{
								if (num != 8)
								{
									goto IL_D0;
								}
							}
							else
							{
								array = BMPImage.bit4to8(array, this.w);
							}
						}
						else
						{
							array = BMPImage.bit1to8(array, this.w);
						}
					}
					else
					{
						if (num != 16)
						{
							if (num != 24)
							{
								if (num != 32)
								{
									goto IL_D0;
								}
								array = BMPImage.bit32to24(array, this.w);
							}
						}
						else
						{
							if (this.r5g6b5)
							{
								array = BMPImage.bit16to24(array, this.w);
							}
							else
							{
								array = BMPImage.bit16to24b(array, this.w);
							}
						}
					}
					int num2 = this.w * (this.h - i - 1) * 3;
					if (this.palette != null)
					{
						for (int j = 0; j < this.w; j++)
						{
							this.image[num2++] = this.palette[(array[j] < 0) ? ((int)array[j] + 256) : ((int)array[j])][2];
							this.image[num2++] = this.palette[(array[j] < 0) ? ((int)array[j] + 256) : ((int)array[j])][1];
							this.image[num2++] = this.palette[(array[j] < 0) ? ((int)array[j] + 256) : ((int)array[j])][0];
						}
					}
					else
					{
						for (int k = 0; k < this.w * 3; k += 3)
						{
							this.image[num2++] = array[k + 2];
							this.image[num2++] = array[k + 1];
							this.image[num2++] = array[k];
						}
					}
					i++;
					continue;
					IL_D0:
					throw new Exception("Can only parse 1 bit, 4bit, 8bit, 16bit, 24bit and 32bit images");
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString() + " : BMP parse error: imagedata not correct");
			}
			MemoryStream memoryStream = new MemoryStream(32768);
			DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream);
			deflaterOutputStream.Write(this.image, 0, this.image.Length);
			deflaterOutputStream.Finish();
			this.deflated = memoryStream.ToArray();
		}
		private static byte[] bit16to24(byte[] row, int width)
		{
			byte[] array = new byte[width * 3];
			int num = 0;
			for (int i = 0; i < width * 2; i += 2)
			{
				array[num++] = (byte)((row[i] & 31) << 3);
				array[num++] = (byte)(((int)(row[i + 1] & 7) << 5) + ((row[i] & 224) >> 3));
				array[num++] = Convert.ToByte((row[i + 1] & 248));
			}
			return array;
		}
		private static byte[] bit16to24b(byte[] row, int width)
		{
			byte[] array = new byte[width * 3];
			int num = 0;
			for (int i = 0; i < width * 2; i += 2)
			{
				array[num++] = (byte)((row[i] & 31) << 3);
				array[num++] = (byte)(((int)(row[i + 1] & 3) << 6) + ((row[i] & 224) >> 2));
				array[num++] = (byte)((row[i + 1] & 124) << 1);
			}
			return array;
		}
		private static byte[] bit32to24(byte[] row, int width)
		{
			byte[] array = new byte[width * 3];
			int num = 0;
			for (int i = 0; i < width * 4; i += 4)
			{
				array[num++] = row[i + 1];
				array[num++] = row[i + 2];
				array[num++] = row[i + 3];
			}
			return array;
		}
		private static byte[] bit4to8(byte[] row, int width)
		{
			byte[] array = new byte[width];
			for (int i = 0; i < width; i++)
			{
				if (i % 2 == 0)
				{
					array[i] = (byte)((row[i / 2] & 240) >> 4);
				}
				else
				{
					array[i] = Convert.ToByte((row[i / 2] & 15));
				}
			}
			return array;
		}
		private static byte[] bit1to8(byte[] row, int width)
		{
			byte[] array = new byte[width];
			for (int i = 0; i < width; i++)
			{
				switch (i % 8)
				{
				case 0:
					array[i] = (byte)((row[i / 8] & 128) >> 7);
					break;
				case 1:
					array[i] = (byte)((row[i / 8] & 64) >> 6);
					break;
				case 2:
					array[i] = (byte)((row[i / 8] & 32) >> 5);
					break;
				case 3:
					array[i] = (byte)((row[i / 8] & 16) >> 4);
					break;
				case 4:
					array[i] = (byte)((row[i / 8] & 8) >> 3);
					break;
				case 5:
					array[i] = (byte)((row[i / 8] & 4) >> 2);
					break;
				case 6:
					array[i] = (byte)((row[i / 8] & 2) >> 1);
					break;
				case 7:
					array[i] = Convert.ToByte((row[i / 8] & 1));
					break;
				}
			}
			return array;
		}
		private void parsePalette(Stream stream, int size)
		{
			this.palette = new byte[size][];
			for (int i = 0; i < size; i++)
			{
				this.palette[i] = this.getBytes(stream, 4);
			}
		}
		private void skipNBytes(Stream inputStream, int n)
		{
			this.getBytes(inputStream, n);
		}
		private byte[] getBytes(Stream inputStream, int length)
		{
			byte[] array = new byte[length];
			inputStream.Read(array, 0, array.Length);
			return array;
		}
		private int read2BytesLE(Stream inputStream)
		{
			byte[] bytes = this.getBytes(inputStream, 2);
			int num = 0;
			num |= (int)(bytes[1] & 255);
			num <<= 8;
			return num | (int)(bytes[0] & 255);
		}
		private int readSignedInt(Stream inputStream)
		{
			byte[] bytes = this.getBytes(inputStream, 4);
			long num = 0L;
			num |= (long)((ulong)(bytes[3] & 255));
			num <<= 8;
			num |= (long)((ulong)(bytes[2] & 255));
			num <<= 8;
			num |= (long)((ulong)(bytes[1] & 255));
			num <<= 8;
			num |= (long)((ulong)(bytes[0] & 255));
			return (int)num;
		}
		public int GetWidth()
		{
			return this.w;
		}
		public int GetHeight()
		{
			return this.h;
		}
		public byte[] GetData()
		{
			return this.deflated;
		}
	}
}
