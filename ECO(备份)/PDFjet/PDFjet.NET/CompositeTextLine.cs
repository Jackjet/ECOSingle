using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	public class CompositeTextLine : IDrawable
	{
		private const int X = 0;
		private const int Y = 1;
		private List<TextLine> textLines = new List<TextLine>();
		private double[] position = new double[2];
		private double[] current = new double[2];
		private double subscript_size_factor = 0.583;
		private double superscript_size_factor = 0.583;
		private double superscript_position = 0.35;
		private double subscript_position = 0.141;
		private double fontSize = 12.0;
		public CompositeTextLine(double x, double y)
		{
			this.position[0] = x;
			this.position[1] = y;
			this.current[0] = x;
			this.current[1] = y;
		}
		public void SetFontSize(double fontSize)
		{
			this.fontSize = fontSize;
		}
		public double GetFontSize()
		{
			return this.fontSize;
		}
		public void SetSuperscriptFactor(double superscript)
		{
			this.superscript_size_factor = superscript;
		}
		public double GetSuperscriptFactor()
		{
			return this.superscript_size_factor;
		}
		public void SetSubscriptFactor(double subscript)
		{
			this.subscript_size_factor = subscript;
		}
		public double GetSubscriptFactor()
		{
			return this.subscript_size_factor;
		}
		public void SetSuperscriptPosition(double superscript_position)
		{
			this.superscript_position = superscript_position;
		}
		public double GetSuperscriptPosition()
		{
			return this.superscript_position;
		}
		public void SetSubscriptPosition(double subscript_position)
		{
			this.subscript_position = subscript_position;
		}
		public double GetSubscriptPosition()
		{
			return this.subscript_position;
		}
		public void AddComponent(TextLine component)
		{
			if (component.GetTextEffect() == 2)
			{
				component.GetFont().SetSize(this.fontSize * this.superscript_size_factor);
				component.SetPosition(this.current[0], this.current[1] - this.fontSize * this.superscript_position);
			}
			else
			{
				if (component.GetTextEffect() == 1)
				{
					component.GetFont().SetSize(this.fontSize * this.subscript_size_factor);
					component.SetPosition(this.current[0], this.current[1] + this.fontSize * this.subscript_position);
				}
				else
				{
					component.GetFont().SetSize(this.fontSize);
					component.SetPosition(this.current[0], this.current[1]);
				}
			}
			this.current[0] += (double)component.GetWidth();
			this.textLines.Add(component);
		}
		public void SetPosition(double x, double y)
		{
			this.SetLocation((float)x, (float)y);
		}
		public void SetPosition(float x, float y)
		{
			this.SetLocation(x, y);
		}
		public void SetLocation(float x, float y)
		{
			this.position[0] = (double)x;
			this.position[1] = (double)y;
			this.current[0] = (double)x;
			this.current[1] = (double)y;
			if (this.textLines == null)
			{
				return;
			}
			if (this.textLines.Count == 0)
			{
				return;
			}
			foreach (TextLine textLine in this.textLines)
			{
				if (textLine.GetTextEffect() == 2)
				{
					textLine.SetPosition(this.current[0], this.current[1] - this.fontSize * this.superscript_position);
				}
				else
				{
					if (textLine.GetTextEffect() == 1)
					{
						textLine.SetPosition(this.current[0], this.current[1] + this.fontSize * this.subscript_position);
					}
					else
					{
						textLine.SetPosition(this.current[0], this.current[1]);
					}
				}
				this.current[0] += (double)textLine.GetWidth();
			}
		}
		public double[] GetPosition()
		{
			return this.position;
		}
		public TextLine Get(int index)
		{
			if (this.textLines == null)
			{
				return null;
			}
			int count = this.textLines.Count;
			if (count == 0)
			{
				return null;
			}
			if (index < 0 || index > count - 1)
			{
				return null;
			}
			return this.textLines[index];
		}
		public int Size()
		{
			return this.textLines.Count;
		}
		public double[] GetMinMax()
		{
			double num = this.position[1];
			double num2 = this.position[1];
			foreach (TextLine textLine in this.textLines)
			{
				if (textLine.GetTextEffect() == 2)
				{
					double num3 = this.position[1] - (double)textLine.GetFont().ascent - this.fontSize * this.superscript_position;
					if (num3 < num)
					{
						num = num3;
					}
				}
				else
				{
					if (textLine.GetTextEffect() == 1)
					{
						double num3 = this.position[1] - (double)textLine.GetFont().descent + this.fontSize * this.subscript_position;
						if (num3 > num2)
						{
							num2 = num3;
						}
					}
					else
					{
						double num3 = this.position[1] - (double)textLine.GetFont().ascent;
						if (num3 < num)
						{
							num = num3;
						}
						num3 = this.position[1] - (double)textLine.GetFont().descent;
						if (num3 > num2)
						{
							num2 = num3;
						}
					}
				}
			}
			return new double[]
			{
				num,
				num2
			};
		}
		public double GetHeight()
		{
			double[] minMax = this.GetMinMax();
			return minMax[1] - minMax[0];
		}
		public double GetWidth()
		{
			return this.current[0] - this.position[0];
		}
		public void DrawOn(Page page)
		{
			foreach (TextLine textLine in this.textLines)
			{
				textLine.DrawOn(page);
			}
		}
	}
}
