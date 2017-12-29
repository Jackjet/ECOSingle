using System;
namespace PDFjet.NET
{
	public class QRUtil
	{
		private const int G15 = 1335;
		private const int G15_MASK = 21522;
		internal static Polynomial GetErrorCorrectPolynomial(int errorCorrectLength)
		{
			Polynomial polynomial = new Polynomial(new int[]
			{
				1
			});
			for (int i = 0; i < errorCorrectLength; i++)
			{
				polynomial = polynomial.Multiply(new Polynomial(new int[]
				{
					1,
					QRMath.Gexp(i)
				}));
			}
			return polynomial;
		}
		internal static bool GetMask(int maskPattern, int i, int j)
		{
			switch (maskPattern)
			{
			case 0:
				return (i + j) % 2 == 0;
			case 1:
				return i % 2 == 0;
			case 2:
				return j % 3 == 0;
			case 3:
				return (i + j) % 3 == 0;
			case 4:
				return (i / 2 + j / 3) % 2 == 0;
			case 5:
				return i * j % 2 + i * j % 3 == 0;
			case 6:
				return (i * j % 2 + i * j % 3) % 2 == 0;
			case 7:
				return (i * j % 3 + (i + j) % 2) % 2 == 0;
			default:
				throw new ArgumentException("mask: " + maskPattern);
			}
		}
		internal static int GetLostPoint(QRCode qrCode)
		{
			int moduleCount = qrCode.GetModuleCount();
			int num = 0;
			for (int i = 0; i < moduleCount; i++)
			{
				for (int j = 0; j < moduleCount; j++)
				{
					int num2 = 0;
					bool flag = qrCode.IsDark(i, j);
					for (int k = -1; k <= 1; k++)
					{
						if (i + k >= 0 && moduleCount > i + k)
						{
							for (int l = -1; l <= 1; l++)
							{
								if (j + l >= 0 && moduleCount > j + l && (k != 0 || l != 0) && flag == qrCode.IsDark(i + k, j + l))
								{
									num2++;
								}
							}
						}
					}
					if (num2 > 5)
					{
						num += 3 + num2 - 5;
					}
				}
			}
			for (int m = 0; m < moduleCount - 1; m++)
			{
				for (int n = 0; n < moduleCount - 1; n++)
				{
					int num3 = 0;
					if (qrCode.IsDark(m, n))
					{
						num3++;
					}
					if (qrCode.IsDark(m + 1, n))
					{
						num3++;
					}
					if (qrCode.IsDark(m, n + 1))
					{
						num3++;
					}
					if (qrCode.IsDark(m + 1, n + 1))
					{
						num3++;
					}
					if (num3 == 0 || num3 == 4)
					{
						num += 3;
					}
				}
			}
			for (int num4 = 0; num4 < moduleCount; num4++)
			{
				for (int num5 = 0; num5 < moduleCount - 6; num5++)
				{
					if (qrCode.IsDark(num4, num5) && !qrCode.IsDark(num4, num5 + 1) && qrCode.IsDark(num4, num5 + 2) && qrCode.IsDark(num4, num5 + 3) && qrCode.IsDark(num4, num5 + 4) && !qrCode.IsDark(num4, num5 + 5) && qrCode.IsDark(num4, num5 + 6))
					{
						num += 40;
					}
				}
			}
			for (int num6 = 0; num6 < moduleCount; num6++)
			{
				for (int num7 = 0; num7 < moduleCount - 6; num7++)
				{
					if (qrCode.IsDark(num7, num6) && !qrCode.IsDark(num7 + 1, num6) && qrCode.IsDark(num7 + 2, num6) && qrCode.IsDark(num7 + 3, num6) && qrCode.IsDark(num7 + 4, num6) && !qrCode.IsDark(num7 + 5, num6) && qrCode.IsDark(num7 + 6, num6))
					{
						num += 40;
					}
				}
			}
			int num8 = 0;
			for (int num9 = 0; num9 < moduleCount; num9++)
			{
				for (int num10 = 0; num10 < moduleCount; num10++)
				{
					if (qrCode.IsDark(num10, num9))
					{
						num8++;
					}
				}
			}
			int num11 = Math.Abs(100 * num8 / moduleCount / moduleCount - 50) / 5;
			return num + num11 * 10;
		}
		public static int GetBCHTypeInfo(int data)
		{
			int num = data << 10;
			while (QRUtil.GetBCHDigit(num) - QRUtil.GetBCHDigit(1335) >= 0)
			{
				num ^= 1335 << QRUtil.GetBCHDigit(num) - QRUtil.GetBCHDigit(1335);
			}
			return (data << 10 | num) ^ 21522;
		}
		private static int GetBCHDigit(int data)
		{
			int num = 0;
			while (data != 0)
			{
				num++;
				data = (int)((uint)data >> 1);
			}
			return num;
		}
	}
}
