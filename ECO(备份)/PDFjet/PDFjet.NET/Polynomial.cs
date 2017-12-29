using System;
namespace PDFjet.NET
{
	public class Polynomial
	{
		private int[] num;
		public Polynomial(int[] num) : this(num, 0)
		{
		}
		public Polynomial(int[] num, int shift)
		{
			int num2 = 0;
			while (num2 < num.Length && num[num2] == 0)
			{
				num2++;
			}
			this.num = new int[num.Length - num2 + shift];
			Array.Copy(num, num2, this.num, 0, num.Length - num2);
		}
		public int Get(int index)
		{
			return this.num[index];
		}
		public int GetLength()
		{
			return this.num.Length;
		}
		public Polynomial Multiply(Polynomial e)
		{
			int[] array = new int[this.GetLength() + e.GetLength() - 1];
			for (int i = 0; i < this.GetLength(); i++)
			{
				for (int j = 0; j < e.GetLength(); j++)
				{
					array[i + j] ^= QRMath.Gexp(QRMath.Glog(this.Get(i)) + QRMath.Glog(e.Get(j)));
				}
			}
			return new Polynomial(array);
		}
		public Polynomial Mod(Polynomial e)
		{
			if (this.GetLength() - e.GetLength() < 0)
			{
				return this;
			}
			int num = QRMath.Glog(this.Get(0)) - QRMath.Glog(e.Get(0));
			int[] array = new int[this.GetLength()];
			for (int i = 0; i < this.GetLength(); i++)
			{
				array[i] = this.Get(i);
			}
			for (int j = 0; j < e.GetLength(); j++)
			{
				array[j] ^= QRMath.Gexp(QRMath.Glog(e.Get(j)) + num);
			}
			return new Polynomial(array).Mod(e);
		}
	}
}
