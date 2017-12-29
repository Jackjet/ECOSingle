using System;
using System.Text;
namespace PDFjet.NET
{
	public class QRCode : IDrawable
	{
		private const int PAD0 = 236;
		private const int PAD1 = 17;
		private bool?[][] modules;
		private int moduleCount = 33;
		private int errorCorrectLevel;
		private float x;
		private float y;
		private byte[] qrData;
		private float m1 = 2f;
		public QRCode(string str, int errorCorrectLevel)
		{
			this.qrData = Encoding.GetEncoding("utf-8").GetBytes(str);
			this.errorCorrectLevel = errorCorrectLevel;
			this.Make(false, this.GetBestMaskPattern());
		}
		public void SetPosition(double x, double y)
		{
			this.SetPosition((float)x, (float)y);
		}
		public void SetPosition(float x, float y)
		{
			this.SetLocation(x, y);
		}
		public void SetLocation(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
		public void SetModuleLength(double moduleLength)
		{
			this.m1 = (float)moduleLength;
		}
		public void SetModuleLength(float moduleLength)
		{
			this.m1 = moduleLength;
		}
		public void DrawOn(Page page)
		{
			for (int i = 0; i < this.modules.Length; i++)
			{
				for (int j = 0; j < this.modules.Length; j++)
				{
					if (this.IsDark(i, j))
					{
						page.FillRect((double)(this.x + (float)j * this.m1), (double)(this.y + (float)i * this.m1), (double)this.m1, (double)this.m1);
					}
				}
			}
		}
		internal bool IsDark(int row, int col)
		{
			return this.modules[row][col].HasValue && this.modules[row][col].Value;
		}
		internal int GetModuleCount()
		{
			return this.moduleCount;
		}
		private int GetBestMaskPattern()
		{
			int num = 0;
			int result = 0;
			for (int i = 0; i < 8; i++)
			{
				this.Make(true, i);
				int lostPoint = QRUtil.GetLostPoint(this);
				if (i == 0 || num > lostPoint)
				{
					num = lostPoint;
					result = i;
				}
			}
			return result;
		}
		private void Make(bool test, int maskPattern)
		{
			this.modules = new bool?[this.moduleCount][];
			for (int i = 0; i < this.modules.Length; i++)
			{
				this.modules[i] = new bool?[this.moduleCount];
			}
			this.SetupPositionProbePattern(0, 0);
			this.SetupPositionProbePattern(this.moduleCount - 7, 0);
			this.SetupPositionProbePattern(0, this.moduleCount - 7);
			this.SetupPositionAdjustPattern();
			this.SetupTimingPattern();
			this.SetupTypeInfo(test, maskPattern);
			this.MapData(this.CreateData(this.errorCorrectLevel), maskPattern);
		}
		private void MapData(byte[] data, int maskPattern)
		{
			int num = -1;
			int num2 = this.moduleCount - 1;
			int num3 = 7;
			int num4 = 0;
			for (int i = this.moduleCount - 1; i > 0; i -= 2)
			{
				if (i == 6)
				{
					i--;
				}
				do
				{
					for (int j = 0; j < 2; j++)
					{
						if (!this.modules[num2][i - j].HasValue)
						{
							bool flag = false;
							if (num4 < data.Length)
							{
								flag = (((uint)data[num4] >> num3 & 1u) == 1u);
							}
							bool mask = QRUtil.GetMask(maskPattern, num2, i - j);
							if (mask)
							{
								flag = !flag;
							}
							this.modules[num2][i - j] = new bool?(flag);
							num3--;
							if (num3 == -1)
							{
								num4++;
								num3 = 7;
							}
						}
					}
					num2 += num;
				}
				while (num2 >= 0 && this.moduleCount > num2);
				num2 -= num;
				num = -num;
			}
		}
		private void SetupPositionAdjustPattern()
		{
			int[] array = new int[]
			{
				6,
				26
			};
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					int num = array[i];
					int num2 = array[j];
					if (!this.modules[num][num2].HasValue)
					{
						for (int k = -2; k <= 2; k++)
						{
							for (int l = -2; l <= 2; l++)
							{
								this.modules[num + k][num2 + l] = new bool?(k == -2 || k == 2 || l == -2 || l == 2 || (k == 0 && l == 0));
							}
						}
					}
				}
			}
		}
		private void SetupPositionProbePattern(int row, int col)
		{
			for (int i = -1; i <= 7; i++)
			{
				for (int j = -1; j <= 7; j++)
				{
					if (row + i > -1 && this.moduleCount > row + i && col + j > -1 && this.moduleCount > col + j)
					{
						this.modules[row + i][col + j] = new bool?((0 <= i && i <= 6 && (j == 0 || j == 6)) || (0 <= j && j <= 6 && (i == 0 || i == 6)) || (2 <= i && i <= 4 && 2 <= j && j <= 4));
					}
				}
			}
		}
		private void SetupTimingPattern()
		{
			for (int i = 8; i < this.moduleCount - 8; i++)
			{
				if (!this.modules[i][6].HasValue)
				{
					this.modules[i][6] = new bool?(i % 2 == 0);
				}
			}
			for (int j = 8; j < this.moduleCount - 8; j++)
			{
				if (!this.modules[6][j].HasValue)
				{
					this.modules[6][j] = new bool?(j % 2 == 0);
				}
			}
		}
		private void SetupTypeInfo(bool test, int maskPattern)
		{
			int data = this.errorCorrectLevel << 3 | maskPattern;
			int bCHTypeInfo = QRUtil.GetBCHTypeInfo(data);
			for (int i = 0; i < 15; i++)
			{
				bool value = !test && (bCHTypeInfo >> i & 1) == 1;
				if (i < 6)
				{
					this.modules[i][8] = new bool?(value);
				}
				else
				{
					if (i < 8)
					{
						this.modules[i + 1][8] = new bool?(value);
					}
					else
					{
						this.modules[this.moduleCount - 15 + i][8] = new bool?(value);
					}
				}
			}
			for (int j = 0; j < 15; j++)
			{
				bool value2 = !test && (bCHTypeInfo >> j & 1) == 1;
				if (j < 8)
				{
					this.modules[8][this.moduleCount - j - 1] = new bool?(value2);
				}
				else
				{
					if (j < 9)
					{
						this.modules[8][15 - j - 1 + 1] = new bool?(value2);
					}
					else
					{
						this.modules[8][15 - j - 1] = new bool?(value2);
					}
				}
			}
			this.modules[this.moduleCount - 8][8] = new bool?(!test);
		}
		private byte[] CreateData(int errorCorrectLevel)
		{
			RSBlock[] rSBlocks = RSBlock.GetRSBlocks(errorCorrectLevel);
			BitBuffer bitBuffer = new BitBuffer();
			bitBuffer.Put(4, 4);
			bitBuffer.Put(this.qrData.Length, 8);
			for (int i = 0; i < this.qrData.Length; i++)
			{
				bitBuffer.Put((int)this.qrData[i], 8);
			}
			int num = 0;
			for (int j = 0; j < rSBlocks.Length; j++)
			{
				num += rSBlocks[j].GetDataCount();
			}
			if (bitBuffer.GetLengthInBits() > num * 8)
			{
				throw new ArgumentException(string.Concat(new object[]
				{
					"String length overflow. (",
					bitBuffer.GetLengthInBits(),
					">",
					num * 8,
					")"
				}));
			}
			if (bitBuffer.GetLengthInBits() + 4 <= num * 8)
			{
				bitBuffer.Put(0, 4);
			}
			while (bitBuffer.GetLengthInBits() % 8 != 0)
			{
				bitBuffer.Put(false);
			}
			while (bitBuffer.GetLengthInBits() < num * 8)
			{
				bitBuffer.Put(236, 8);
				if (bitBuffer.GetLengthInBits() >= num * 8)
				{
					break;
				}
				bitBuffer.Put(17, 8);
			}
			return QRCode.CreateBytes(bitBuffer, rSBlocks);
		}
		private static byte[] CreateBytes(BitBuffer buffer, RSBlock[] rsBlocks)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int[][] array = new int[rsBlocks.Length][];
			int[][] array2 = new int[rsBlocks.Length][];
			for (int i = 0; i < rsBlocks.Length; i++)
			{
				int dataCount = rsBlocks[i].GetDataCount();
				int num4 = rsBlocks[i].GetTotalCount() - dataCount;
				num2 = Math.Max(num2, dataCount);
				num3 = Math.Max(num3, num4);
				array[i] = new int[dataCount];
				for (int j = 0; j < array[i].Length; j++)
				{
					array[i][j] = (int)(255 & buffer.GetBuffer()[j + num]);
				}
				num += dataCount;
				Polynomial errorCorrectPolynomial = QRUtil.GetErrorCorrectPolynomial(num4);
				Polynomial polynomial = new Polynomial(array[i], errorCorrectPolynomial.GetLength() - 1);
				Polynomial polynomial2 = polynomial.Mod(errorCorrectPolynomial);
				array2[i] = new int[errorCorrectPolynomial.GetLength() - 1];
				for (int k = 0; k < array2[i].Length; k++)
				{
					int num5 = k + polynomial2.GetLength() - array2[i].Length;
					array2[i][k] = ((num5 >= 0) ? polynomial2.Get(num5) : 0);
				}
			}
			int num6 = 0;
			for (int l = 0; l < rsBlocks.Length; l++)
			{
				num6 += rsBlocks[l].GetTotalCount();
			}
			byte[] array3 = new byte[num6];
			int num7 = 0;
			for (int m = 0; m < num2; m++)
			{
				for (int n = 0; n < rsBlocks.Length; n++)
				{
					if (m < array[n].Length)
					{
						array3[num7++] = (byte)array[n][m];
					}
				}
			}
			for (int num8 = 0; num8 < num3; num8++)
			{
				for (int num9 = 0; num9 < rsBlocks.Length; num9++)
				{
					if (num8 < array2[num9].Length)
					{
						array3[num7++] = (byte)array2[num9][num8];
					}
				}
			}
			return array3;
		}
	}
}
