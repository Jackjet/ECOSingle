using System;
using System.Collections.Generic;
using System.IO;
namespace PDFjet.NET
{
	public class PNGImage
	{
		private int w;
		private int h;
		private byte[] data;
		private byte[] inflated;
		private byte[] image;
		private byte[] deflated;
		private byte[] rgb;
		private byte[] alphaForPalette;
		private byte[] deflatedAlpha;
		public byte bitDepth = 8;
		public int colorType;
		public PNGImage(Stream inputStream)
		{
			this.ValidatePNG(inputStream);
			List<Chunk> list = new List<Chunk>();
			this.ProcessPNG(list, inputStream);
			for (int i = 0; i < list.Count; i++)
			{
				Chunk chunk = list[i];
				if (chunk.type[0] == 73 && chunk.type[1] == 72 && chunk.type[2] == 68 && chunk.type[3] == 82)
				{
					this.w = this.ToIntValue(chunk.GetData(), 0);
					this.h = this.ToIntValue(chunk.GetData(), 4);
					this.bitDepth = chunk.GetData()[8];
					this.colorType = (int)chunk.GetData()[9];
				}
				else
				{
					if (chunk.type[0] == 73 && chunk.type[1] == 68 && chunk.type[2] == 65 && chunk.type[3] == 84)
					{
						this.data = this.AppendIdatChunk(this.data, chunk.GetData());
					}
					else
					{
						if (chunk.type[0] == 80 && chunk.type[1] == 76 && chunk.type[2] == 84 && chunk.type[3] == 69)
						{
							this.rgb = chunk.GetData();
							if (this.rgb.Length % 3 != 0)
							{
								throw new Exception("Incorrect palette length.");
							}
						}
						else
						{
							if (chunk.type[0] != 103 || chunk.type[1] != 65 || chunk.type[2] != 77 || chunk.type[3] != 65)
							{
								if (chunk.type[0] == 116 && chunk.type[1] == 82 && chunk.type[2] == 78 && chunk.type[3] == 83)
								{
									if (this.colorType == 3)
									{
										this.alphaForPalette = new byte[this.w * this.h];
										for (int j = 0; j < this.alphaForPalette.Length; j++)
										{
											this.alphaForPalette[j] = 255;
										}
										byte[] array = chunk.GetData();
										for (int k = 0; k < array.Length; k++)
										{
											this.alphaForPalette[k] = array[k];
										}
									}
								}
								else
								{
									if ((chunk.type[0] != 99 || chunk.type[1] != 72 || chunk.type[2] != 82 || chunk.type[3] != 77) && (chunk.type[0] != 115 || chunk.type[1] != 66 || chunk.type[2] != 73 || chunk.type[3] != 84) && chunk.type[0] == 98 && chunk.type[1] == 75 && chunk.type[2] == 71)
									{
										byte arg_2B6_0 = chunk.type[3];
									}
								}
							}
						}
					}
				}
			}
			this.inflated = this.GetDecompressedData();
			if (this.colorType == 0)
			{
				if (this.bitDepth == 16)
				{
					this.image = this.getImageColorType0BitDepth16();
				}
				else
				{
					if (this.bitDepth == 8)
					{
						this.image = this.getImageColorType0BitDepth8();
					}
					else
					{
						if (this.bitDepth == 4)
						{
							this.image = this.getImageColorType0BitDepth4();
						}
						else
						{
							if (this.bitDepth == 2)
							{
								this.image = this.getImageColorType0BitDepth2();
							}
							else
							{
								if (this.bitDepth != 1)
								{
									throw new Exception("Image with unsupported bit depth == " + this.bitDepth);
								}
								this.image = this.getImageColorType0BitDepth1();
							}
						}
					}
				}
			}
			else
			{
				if (this.colorType == 6)
				{
					this.image = this.getImageColorType6BitDepth8();
				}
				else
				{
					if (this.rgb == null)
					{
						if (this.bitDepth == 16)
						{
							this.image = this.getImageColorType2BitDepth16();
						}
						else
						{
							this.image = this.getImageColorType2BitDepth8();
						}
					}
					else
					{
						if (this.bitDepth == 8)
						{
							this.image = this.getImageColorType3BitDepth8();
						}
						else
						{
							if (this.bitDepth == 4)
							{
								this.image = this.getImageColorType3BitDepth4();
							}
							else
							{
								if (this.bitDepth == 2)
								{
									this.image = this.getImageColorType3BitDepth2();
								}
								else
								{
									if (this.bitDepth != 1)
									{
										throw new Exception("Image with unsupported bit depth == " + this.bitDepth);
									}
									this.image = this.getImageColorType3BitDepth1();
								}
							}
						}
					}
				}
			}
			this.deflated = this.DeflateReconstructedData();
		}
		internal int GetWidth()
		{
			return this.w;
		}
		internal int GetHeight()
		{
			return this.h;
		}
		internal byte[] GetData()
		{
			return this.deflated;
		}
		internal byte[] GetAlpha()
		{
			return this.deflatedAlpha;
		}
		private void ProcessPNG(List<Chunk> chunks, Stream inputStream)
		{
			Chunk chunk;
			do
			{
				chunk = this.getChunk(inputStream);
				chunks.Add(chunk);
			}
			while (chunk.type[0] != 73 || chunk.type[1] != 69 || chunk.type[2] != 78 || chunk.type[3] != 68);
		}
		private void ValidatePNG(Stream inputStream)
		{
			byte[] array = new byte[8];
			if (inputStream.Read(array, 0, array.Length) == -1)
			{
				throw new Exception("File is too short!");
			}
			if ((array[0] & 255) == 137 && array[1] == 80 && array[2] == 78 && array[3] == 71 && array[4] == 13 && array[5] == 10 && array[6] == 26 && array[7] == 10)
			{
				return;
			}
			throw new Exception("Wrong PNG signature.");
		}
		private Chunk getChunk(Stream inputStream)
		{
			Chunk chunk = new Chunk();
			chunk.SetLength(this.GetLong(inputStream));
			chunk.SetType(this.GetBytes(inputStream, 4L));
			chunk.SetData(this.GetBytes(inputStream, chunk.GetLength()));
			chunk.SetCrc(this.GetLong(inputStream));
			if (!chunk.hasGoodCRC())
			{
				throw new Exception("Chunk has bad CRC.");
			}
			return chunk;
		}
		private long GetLong(Stream inputStream)
		{
			byte[] bytes = this.GetBytes(inputStream, 4L);
			return (long)this.ToIntValue(bytes, 0) & (long)((long)-1);
		}
		private byte[] GetBytes(Stream inputStream, long length)
		{
			byte[] array = new byte[(int)length];
			if (inputStream.Read(array, 0, array.Length) == -1)
			{
				throw new Exception("Error reading input stream!");
			}
			return array;
		}
		private int ToIntValue(byte[] buf, int off)
		{
			long num = 0L;
			num |= (long)((ulong)buf[off] & 255uL);
			num <<= 8;
			num |= (long)((ulong)buf[1 + off] & 255uL);
			num <<= 8;
			num |= (long)((ulong)buf[2 + off] & 255uL);
			num <<= 8;
			num |= (long)((ulong)buf[3 + off] & 255uL);
			return (int)num;
		}
		private byte[] getImageColorType2BitDepth16()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			byte filter = 0;
			int num2 = 6 * this.w;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % (num2 + 1) == 0)
				{
					filter = this.inflated[i];
				}
				else
				{
					array[num] = this.inflated[i];
					int a = 0;
					int b = 0;
					int c = 0;
					if (num % num2 >= 6)
					{
						a = (int)(array[num - 6] & 255);
					}
					if (num >= num2)
					{
						b = (int)(array[num - num2] & 255);
					}
					if (num % num2 >= 6 && num >= num2)
					{
						c = (int)(array[num - (num2 + 6)] & 255);
					}
					this.applyFilters(filter, array, num, a, b, c);
					num++;
				}
			}
			return array;
		}
		private byte[] getImageColorType2BitDepth8()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			byte filter = 0;
			int num2 = 3 * this.w;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % (num2 + 1) == 0)
				{
					filter = this.inflated[i];
				}
				else
				{
					array[num] = this.inflated[i];
					int a = 0;
					int b = 0;
					int c = 0;
					if (num % num2 >= 3)
					{
						a = (int)(array[num - 3] & 255);
					}
					if (num >= num2)
					{
						b = (int)(array[num - num2] & 255);
					}
					if (num % num2 >= 3 && num >= num2)
					{
						c = (int)(array[num - (num2 + 3)] & 255);
					}
					this.applyFilters(filter, array, num, a, b, c);
					num++;
				}
			}
			return array;
		}
		private byte[] getImageColorType6BitDepth8()
		{
			int num = 0;
			byte[] array = new byte[4 * this.w * this.h];
			byte filter = 0;
			int num2 = 4 * this.w;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % (num2 + 1) == 0)
				{
					filter = this.inflated[i];
				}
				else
				{
					array[num] = this.inflated[i];
					int a = 0;
					int b = 0;
					int c = 0;
					if (num % num2 >= 4)
					{
						a = (int)(array[num - 4] & 255);
					}
					if (num >= num2)
					{
						b = (int)(array[num - num2] & 255);
					}
					if (num % num2 >= 4 && num >= num2)
					{
						c = (int)(array[num - (num2 + 4)] & 255);
					}
					this.applyFilters(filter, array, num, a, b, c);
					num++;
				}
			}
			byte[] array2 = new byte[3 * this.w * this.h];
			byte[] array3 = new byte[this.w * this.h];
			int num3 = 0;
			int num4 = 0;
			for (int j = 0; j < array.Length; j += 4)
			{
				array2[num3] = array[j];
				array2[num3 + 1] = array[j + 1];
				array2[num3 + 2] = array[j + 2];
				array3[num4] = array[j + 3];
				num3 += 3;
				num4++;
			}
			Compressor compressor = new Compressor(array3);
			this.deflatedAlpha = compressor.GetCompressedData();
			return array2;
		}
		private byte[] getImageColorType3BitDepth8()
		{
			int num = 0;
			int num2 = 0;
			byte[] array = null;
			if (this.alphaForPalette != null)
			{
				array = new byte[this.w * this.h];
			}
			byte[] array2 = new byte[3 * (this.inflated.Length - this.h)];
			int num3 = this.w + 1;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num3 != 0)
				{
					int num4 = (int)(this.inflated[i] & 255);
					array2[num++] = this.rgb[3 * num4];
					array2[num++] = this.rgb[3 * num4 + 1];
					array2[num++] = this.rgb[3 * num4 + 2];
					if (this.alphaForPalette != null)
					{
						array[num2++] = this.alphaForPalette[num4];
					}
				}
			}
			if (this.alphaForPalette != null)
			{
				Compressor compressor = new Compressor(array);
				this.deflatedAlpha = compressor.GetCompressedData();
			}
			return array2;
		}
		private byte[] getImageColorType3BitDepth4()
		{
			int num = 0;
			byte[] array = new byte[6 * (this.inflated.Length - this.h)];
			int num2 = this.w / 2 + 1;
			if (this.w % 2 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					int num3 = (int)this.inflated[i];
					int num4 = 3 * (num3 >> 4 & 15);
					array[num++] = this.rgb[num4];
					array[num++] = this.rgb[num4 + 1];
					array[num++] = this.rgb[num4 + 2];
					if (num % (3 * this.w) != 0)
					{
						num4 = 3 * (num3 & 15);
						array[num++] = this.rgb[num4];
						array[num++] = this.rgb[num4 + 1];
						array[num++] = this.rgb[num4 + 2];
					}
				}
			}
			return array;
		}
		private byte[] getImageColorType3BitDepth2()
		{
			int num = 0;
			byte[] array = new byte[12 * (this.inflated.Length - this.h)];
			int num2 = this.w / 4 + 1;
			if (this.w % 4 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					int num3 = (int)this.inflated[i];
					int num4 = 3 * (num3 >> 6 & 3);
					array[num++] = this.rgb[num4];
					array[num++] = this.rgb[num4 + 1];
					array[num++] = this.rgb[num4 + 2];
					if (num % (3 * this.w) != 0)
					{
						num4 = 3 * (num3 >> 4 & 3);
						array[num++] = this.rgb[num4];
						array[num++] = this.rgb[num4 + 1];
						array[num++] = this.rgb[num4 + 2];
						if (num % (3 * this.w) != 0)
						{
							num4 = 3 * (num3 >> 2 & 3);
							array[num++] = this.rgb[num4];
							array[num++] = this.rgb[num4 + 1];
							array[num++] = this.rgb[num4 + 2];
							if (num % (3 * this.w) != 0)
							{
								num4 = 3 * (num3 & 3);
								array[num++] = this.rgb[num4];
								array[num++] = this.rgb[num4 + 1];
								array[num++] = this.rgb[num4 + 2];
							}
						}
					}
				}
			}
			return array;
		}
		private byte[] getImageColorType3BitDepth1()
		{
			int num = 0;
			byte[] array = new byte[24 * (this.inflated.Length - this.h)];
			int num2 = this.w / 8 + 1;
			if (this.w % 8 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					int num3 = (int)this.inflated[i];
					int num4 = 3 * (num3 >> 7 & 1);
					array[num++] = this.rgb[num4];
					array[num++] = this.rgb[num4 + 1];
					array[num++] = this.rgb[num4 + 2];
					if (num % (3 * this.w) != 0)
					{
						num4 = 3 * (num3 >> 6 & 1);
						array[num++] = this.rgb[num4];
						array[num++] = this.rgb[num4 + 1];
						array[num++] = this.rgb[num4 + 2];
						if (num % (3 * this.w) != 0)
						{
							num4 = 3 * (num3 >> 5 & 1);
							array[num++] = this.rgb[num4];
							array[num++] = this.rgb[num4 + 1];
							array[num++] = this.rgb[num4 + 2];
							if (num % (3 * this.w) != 0)
							{
								num4 = 3 * (num3 >> 4 & 1);
								array[num++] = this.rgb[num4];
								array[num++] = this.rgb[num4 + 1];
								array[num++] = this.rgb[num4 + 2];
								if (num % (3 * this.w) != 0)
								{
									num4 = 3 * (num3 >> 3 & 1);
									array[num++] = this.rgb[num4];
									array[num++] = this.rgb[num4 + 1];
									array[num++] = this.rgb[num4 + 2];
									if (num % (3 * this.w) != 0)
									{
										num4 = 3 * (num3 >> 2 & 1);
										array[num++] = this.rgb[num4];
										array[num++] = this.rgb[num4 + 1];
										array[num++] = this.rgb[num4 + 2];
										if (num % (3 * this.w) != 0)
										{
											num4 = 3 * (num3 >> 1 & 1);
											array[num++] = this.rgb[num4];
											array[num++] = this.rgb[num4 + 1];
											array[num++] = this.rgb[num4 + 2];
											if (num % (3 * this.w) != 0)
											{
												num4 = 3 * (num3 & 1);
												array[num++] = this.rgb[num4];
												array[num++] = this.rgb[num4 + 1];
												array[num++] = this.rgb[num4 + 2];
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return array;
		}
		private byte[] getImageColorType0BitDepth16()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			byte filter = 0;
			int num2 = 2 * this.w;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % (num2 + 1) == 0)
				{
					filter = this.inflated[i];
				}
				else
				{
					array[num] = this.inflated[i];
					int a = 0;
					int b = 0;
					int c = 0;
					if (num % num2 >= 2)
					{
						a = (int)(array[num - 2] & 255);
					}
					if (num >= num2)
					{
						b = (int)(array[num - num2] & 255);
					}
					if (num % num2 >= 2 && num >= num2)
					{
						c = (int)(array[num - (num2 + 2)] & 255);
					}
					this.applyFilters(filter, array, num, a, b, c);
					num++;
				}
			}
			return array;
		}
		private byte[] getImageColorType0BitDepth8()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			byte filter = 0;
			int num2 = this.w;
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % (num2 + 1) == 0)
				{
					filter = this.inflated[i];
				}
				else
				{
					array[num] = this.inflated[i];
					int a = 0;
					int b = 0;
					int c = 0;
					if (num % num2 >= 1)
					{
						a = (int)(array[num - 1] & 255);
					}
					if (num >= num2)
					{
						b = (int)(array[num - num2] & 255);
					}
					if (num % num2 >= 1 && num >= num2)
					{
						c = (int)(array[num - (num2 + 1)] & 255);
					}
					this.applyFilters(filter, array, num, a, b, c);
					num++;
				}
			}
			return array;
		}
		private byte[] getImageColorType0BitDepth4()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			int num2 = this.w / 2 + 1;
			if (this.w % 2 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					array[num++] = this.inflated[i];
				}
			}
			return array;
		}
		private byte[] getImageColorType0BitDepth2()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			int num2 = this.w / 4 + 1;
			if (this.w % 4 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					array[num++] = this.inflated[i];
				}
			}
			return array;
		}
		private byte[] getImageColorType0BitDepth1()
		{
			int num = 0;
			byte[] array = new byte[this.inflated.Length - this.h];
			int num2 = this.w / 8 + 1;
			if (this.w % 8 > 0)
			{
				num2++;
			}
			for (int i = 0; i < this.inflated.Length; i++)
			{
				if (i % num2 != 0)
				{
					array[num++] = this.inflated[i];
				}
			}
			return array;
		}
		private void applyFilters(byte filter, byte[] image, int j, int a, int b, int c)
		{
			if (filter == 0)
			{
				return;
			}
			if (filter == 1)
			{
				image[j] += (byte)a;
				return;
			}
			if (filter == 2)
			{
				image[j] += (byte)b;
				return;
			}
			if (filter == 3)
			{
				image[j] += (byte)Math.Floor((double)(a + b) / 2.0);
				return;
			}
			if (filter == 4)
			{
				int num = a + b - c;
				int num2 = Math.Abs(num - a);
				int num3 = Math.Abs(num - b);
				int num4 = Math.Abs(num - c);
				int num5;
				if (num2 <= num3 && num2 <= num4)
				{
					num5 = a;
				}
				else
				{
					if (num3 <= num4)
					{
						num5 = b;
					}
					else
					{
						num5 = c;
					}
				}
				image[j] += (byte)(num5 & 255);
			}
		}
		private byte[] GetDecompressedData()
		{
			Decompressor decompressor = new Decompressor(this.data);
			return decompressor.getDecompressedData();
		}
		private byte[] DeflateReconstructedData()
		{
			Compressor compressor = new Compressor(this.image);
			return compressor.GetCompressedData();
		}
		private byte[] AppendIdatChunk(byte[] array1, byte[] array2)
		{
			if (array1 == null)
			{
				return array2;
			}
			if (array2 == null)
			{
				return array1;
			}
			byte[] array3 = new byte[array1.Length + array2.Length];
			Array.Copy(array1, 0, array3, 0, array1.Length);
			Array.Copy(array2, 0, array3, array1.Length, array2.Length);
			return array3;
		}
	}
}
