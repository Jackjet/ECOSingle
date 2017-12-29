using System;
namespace PDFjet.NET
{
	internal class Round
	{
		internal double value;
		internal int exponent = 1;
		internal int num_of_grid_lines;
		public Round(double value, int exponent, int num_of_grid_lines)
		{
			this.value = value;
			this.exponent = exponent;
			this.num_of_grid_lines = num_of_grid_lines;
		}
	}
}
