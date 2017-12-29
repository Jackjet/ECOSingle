using System;
namespace PDFjet.NET
{
	public class QRMath
	{
		private static int[] EXP_TABLE;
		private static int[] LOG_TABLE;
		static QRMath()
		{
			QRMath.EXP_TABLE = new int[256];
			QRMath.LOG_TABLE = new int[256];
			for (int i = 0; i < 8; i++)
			{
				QRMath.EXP_TABLE[i] = 1 << i;
			}
			for (int j = 8; j < 256; j++)
			{
				QRMath.EXP_TABLE[j] = (QRMath.EXP_TABLE[j - 4] ^ QRMath.EXP_TABLE[j - 5] ^ QRMath.EXP_TABLE[j - 6] ^ QRMath.EXP_TABLE[j - 8]);
			}
			for (int k = 0; k < 255; k++)
			{
				QRMath.LOG_TABLE[QRMath.EXP_TABLE[k]] = k;
			}
		}
		public static int Glog(int n)
		{
			if (n < 1)
			{
				throw new ArithmeticException("log(" + n + ")");
			}
			return QRMath.LOG_TABLE[n];
		}
		public static int Gexp(int n)
		{
			while (n < 0)
			{
				n += 255;
			}
			while (n >= 256)
			{
				n -= 255;
			}
			return QRMath.EXP_TABLE[n];
		}
	}
}
