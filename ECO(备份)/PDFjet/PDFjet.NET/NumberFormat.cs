using System;
namespace PDFjet.NET
{
	public class NumberFormat
	{
		private int minFractionDigits;
		private int maxFractionDigits;
		public static NumberFormat getInstance()
		{
			return new NumberFormat();
		}
		public void SetMinimumFractionDigits(int minFractionDigits)
		{
			this.minFractionDigits = minFractionDigits;
		}
		public void SetMaximumFractionDigits(int maxFractionDigits)
		{
			this.maxFractionDigits = maxFractionDigits;
		}
		public string Format(double value)
		{
			string text = "0.";
			for (int i = 0; i < this.maxFractionDigits; i++)
			{
				text += "0";
			}
			return value.ToString(text);
		}
	}
}
