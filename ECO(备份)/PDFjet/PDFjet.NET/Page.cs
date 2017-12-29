using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace PDFjet.NET
{
	public class Page
	{
		internal PDF pdf;
		internal MemoryStream buf;
		internal float[] tm = new float[]
		{
			1f,
			0f,
			0f,
			1f
		};
		internal int renderingMode;
		internal float width;
		internal float height;
		internal List<Annotation> annots;
		internal List<Destination> destinations;
		internal float[] cropBox;
		internal float[] bleedBox;
		internal float[] trimBox;
		internal float[] artBox;
		private float[] pen;
		private float[] brush;
		private float pen_width;
		private int line_cap_style;
		private int line_join_style;
		private string linePattern;
		private Font font;
		private List<State> savedStates;
		public Page(float[] pageSize) : this(null, pageSize)
		{
		}
		public Page(PDF pdf, float[] pageSize)
		{
			float[] array = new float[3];
			this.pen = array;
			float[] array2 = new float[3];
			this.brush = array2;
			this.pen_width = -1f;
			this.linePattern = "[] 0";
			this.savedStates = new List<State>();
            //base..ctor();
			this.pdf = pdf;
			this.annots = new List<Annotation>();
			this.destinations = new List<Destination>();
			this.width = pageSize[0];
			this.height = pageSize[1];
			this.buf = new MemoryStream(8192);
			if (pdf != null)
			{
				pdf.AddPage(this);
			}
		}
		public void AddDestination(string name, double yPosition)
		{
			this.destinations.Add(new Destination(name, (double)this.height - yPosition));
		}
		internal void SetDestinationsPageObjNumber(int pageObjNumber)
		{
			foreach (Destination current in this.destinations)
			{
				current.SetPageObjNumber(pageObjNumber);
				this.pdf.destinations.Add(current.name, current);
			}
		}
		public float GetWidth()
		{
			return this.width;
		}
		public float GetHeight()
		{
			return this.height;
		}
		public void DrawLine(double x1, double y1, double x2, double y2)
		{
			this.MoveTo(x1, y1);
			this.LineTo(x2, y2);
			this.StrokePath();
		}
		public void DrawString(Font font1, Font font2, string str, float x, float y)
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				int num = (int)str[i];
				if ((font1.isCJK && num >= 19968 && num <= 40908) || (!font1.isCJK && font1.unicodeToGID[num] != 0))
				{
					if (!flag)
					{
						string str2 = stringBuilder.ToString();
						this.DrawString(font2, str2, x, y);
						x += font2.StringWidth(str2);
						stringBuilder = new StringBuilder();
						flag = true;
					}
				}
				else
				{
					if (flag)
					{
						string str3 = stringBuilder.ToString();
						this.DrawString(font1, str3, x, y);
						x += font1.StringWidth(str3);
						stringBuilder = new StringBuilder();
						flag = false;
					}
				}
				stringBuilder.Append((char)num);
			}
			if (flag)
			{
				this.DrawString(font1, stringBuilder.ToString(), x, y);
				return;
			}
			this.DrawString(font2, stringBuilder.ToString(), x, y);
		}
		public void DrawString(Font font, string str, double x, double y)
		{
			this.DrawString(font, str, (float)x, (float)y);
		}
		public void DrawString(Font font, string str, float x, float y)
		{
			if (str == null || str.Equals(""))
			{
				return;
			}
			this.Append("q\n");
			this.Append("BT\n");
			this.SetTextFont(font);
			if (this.renderingMode != 0)
			{
				this.Append(this.renderingMode);
				this.Append(" Tr\n");
			}
			float num = 0f;
			if (font.skew15 && this.tm[0] == 1f && this.tm[1] == 0f && this.tm[2] == 0f && this.tm[3] == 1f)
			{
				num = 0.26f;
			}
			this.Append(this.tm[0]);
			this.Append(' ');
			this.Append(this.tm[1]);
			this.Append(' ');
			this.Append(this.tm[2] + num);
			this.Append(' ');
			this.Append(this.tm[3]);
			this.Append(' ');
			this.Append(x);
			this.Append(' ');
			this.Append(this.height - y);
			this.Append(" cm\n");
			this.Append("[ (");
			this.DrawString(font, str);
			this.Append(") ] TJ\n");
			this.Append("ET\n");
			this.Append("Q\n");
		}
		private void DrawString(Font font, string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				int c = (int)str[i];
				if (font.isComposite)
				{
					this.DrawTwoByteChar(c, font);
				}
				else
				{
					this.DrawOneByteChar(c, font, str, i);
				}
			}
		}
		private void DrawTwoByteChar(int c1, Font font)
		{
			if (c1 < font.firstChar || c1 > font.lastChar)
			{
				if (font.isCJK)
				{
					this.Append(0);
					this.Append(32);
					return;
				}
				this.Append((byte)font.unicodeToGID[0]);
				this.Append((byte)font.unicodeToGID[32]);
				return;
			}
			else
			{
				byte b;
				byte b2;
				if (font.isCJK)
				{
					b = (byte)(c1 >> 8);
					b2 = (byte)c1;
				}
				else
				{
					int num = font.unicodeToGID[c1];
					b = (byte)(num >> 8);
					b2 = (byte)num;
				}
				if (b == 40 || b == 41 || b == 92)
				{
					this.Append(92);
				}
				this.Append(b);
				if (b2 == 13)
				{
					this.Append("\\015");
					return;
				}
				if (b2 == 40 || b2 == 41 || b2 == 92)
				{
					this.Append(92);
				}
				this.Append(b2);
				return;
			}
		}
		public void DrawOneByteChar(int c1, Font font, string str, int i)
		{
			if (c1 < font.firstChar || c1 > font.lastChar)
			{
				c1 = font.MapUnicodeChar(c1);
			}
			if (c1 == 40 || c1 == 41 || c1 == 92)
			{
				this.Append(92);
			}
			this.Append((byte)c1);
			if (font.isStandard && font.kernPairs && i < str.Length - 1)
			{
				c1 -= 32;
				int num = (int)str[i + 1];
				if (num < font.firstChar || num > font.lastChar)
				{
					num = 32;
				}
				for (int j = 2; j < font.metrics[c1].Length; j += 2)
				{
					if (font.metrics[c1][j] == num)
					{
						this.Append(") ");
						this.Append(-font.metrics[c1][j + 1]);
						this.Append(" (");
						return;
					}
				}
			}
		}
		public void SetPenColor(double r, double g, double b)
		{
			this.SetPenColor((float)r, (float)g, (float)b);
		}
		public void SetPenColor(float r, float g, float b)
		{
			if (this.pen[0] != r || this.pen[1] != g || this.pen[2] != b)
			{
				this.SetColor(r, g, b);
				this.Append(" RG\n");
				this.pen[0] = r;
				this.pen[1] = g;
				this.pen[2] = b;
			}
		}
		public void SetBrushColor(double r, double g, double b)
		{
			this.SetBrushColor((float)r, (float)g, (float)b);
		}
		public void SetBrushColor(float r, float g, float b)
		{
			if (this.brush[0] != r || this.brush[1] != g || this.brush[2] != b)
			{
				this.SetColor(r, g, b);
				this.Append(" rg\n");
				this.brush[0] = r;
				this.brush[1] = g;
				this.brush[2] = b;
			}
		}
		public void SetBrushColor(float[] color)
		{
			this.SetBrushColor(color[0], color[1], color[2]);
		}
		public float[] GetBrushColor()
		{
			return this.brush;
		}
		private void SetColor(float r, float g, float b)
		{
			this.Append(r);
			this.Append(' ');
			this.Append(g);
			this.Append(' ');
			this.Append(b);
		}
		public void SetPenColor(int color)
		{
			float r = (float)(color >> 16 & 255) / 255f;
			float g = (float)(color >> 8 & 255) / 255f;
			float b = (float)(color & 255) / 255f;
			this.SetPenColor(r, g, b);
		}
		[Obsolete("")]
		public void SetPenColor(int[] color)
		{
			float r = (float)(color[0] >> 16 & 255) / 255f;
			float g = (float)(color[1] >> 8 & 255) / 255f;
			float b = (float)(color[2] & 255) / 255f;
			this.SetPenColor(r, g, b);
		}
		public void SetBrushColor(int color)
		{
			float r = (float)(color >> 16 & 255) / 255f;
			float g = (float)(color >> 8 & 255) / 255f;
			float b = (float)(color & 255) / 255f;
			this.SetBrushColor(r, g, b);
		}
		[Obsolete("")]
		public void SetBrushColor(int[] color)
		{
			float r = (float)(color[0] >> 16 & 255) / 255f;
			float g = (float)(color[1] >> 8 & 255) / 255f;
			float b = (float)(color[2] & 255) / 255f;
			this.SetBrushColor(r, g, b);
		}
		public void SetDefaultLineWidth()
		{
			this.Append(0f);
			this.Append(" w\n");
		}
		public void SetLinePattern(string pattern)
		{
			if (!pattern.Equals(this.linePattern))
			{
				this.linePattern = pattern;
				this.Append(this.linePattern);
				this.Append(" d\n");
			}
		}
		public void SetDefaultLinePattern()
		{
			this.Append("[] 0");
			this.Append(" d\n");
		}
		public void SetPenWidth(double width)
		{
			this.SetPenWidth((float)width);
		}
		public void SetPenWidth(float width)
		{
			if (this.pen_width != width)
			{
				this.pen_width = width;
				this.Append(this.pen_width);
				this.Append(" w\n");
			}
		}
		public void SetLineCapStyle(int style)
		{
			if (this.line_cap_style != style)
			{
				this.line_cap_style = style;
				this.Append(this.line_cap_style);
				this.Append(" J\n");
			}
		}
		public void SetLineJoinStyle(int style)
		{
			if (this.line_join_style != style)
			{
				this.line_join_style = style;
				this.Append(this.line_join_style);
				this.Append(" j\n");
			}
		}
		public void MoveTo(double x, double y)
		{
			this.MoveTo((float)x, (float)y);
		}
		public void MoveTo(float x, float y)
		{
			this.Append(x);
			this.Append(' ');
			this.Append(this.height - y);
			this.Append(" m\n");
		}
		public void LineTo(double x, double y)
		{
			this.LineTo((float)x, (float)y);
		}
		public void LineTo(float x, float y)
		{
			this.Append(x);
			this.Append(' ');
			this.Append(this.height - y);
			this.Append(" l\n");
		}
		public void StrokePath()
		{
			this.Append("S\n");
		}
		public void ClosePath()
		{
			this.Append("s\n");
		}
		public void FillPath()
		{
			this.Append("f\n");
		}
		public void DrawRect(double x, double y, double w, double h)
		{
			this.MoveTo(x, y);
			this.LineTo(x + w, y);
			this.LineTo(x + w, y + h);
			this.LineTo(x, y + h);
			this.ClosePath();
		}
		public void FillRect(double x, double y, double w, double h)
		{
			this.MoveTo(x, y);
			this.LineTo(x + w, y);
			this.LineTo(x + w, y + h);
			this.LineTo(x, y + h);
			this.FillPath();
		}
		public void DrawPath(List<Point> path, char operation)
		{
			if (path.Count < 2)
			{
				throw new Exception("The Path object must contain at least 2 points");
			}
			Point point = path[0];
			this.MoveTo(point.x, point.y);
			bool flag = false;
			for (int i = 1; i < path.Count; i++)
			{
				point = path[i];
				if (point.isControlPoint)
				{
					flag = true;
					this.Append(point);
				}
				else
				{
					if (flag)
					{
						flag = false;
						this.Append(point);
						this.Append("c\n");
					}
					else
					{
						this.LineTo(point.x, point.y);
					}
				}
			}
			this.Append(operation);
			this.Append('\n');
		}
		public void DrawBezierCurve(List<Point> list)
		{
			this.DrawBezierCurve(list, Operation.STROKE);
		}
		public void DrawBezierCurve(List<Point> list, char operation)
		{
			Point point = list[0];
			this.MoveTo(point.x, point.y);
			for (int i = 1; i < list.Count; i++)
			{
				point = list[i];
				this.Append(point);
				if (i % 3 == 0)
				{
					this.Append("c\n");
				}
			}
			this.Append(operation);
			this.Append('\n');
		}
		public void DrawCircle(double x, double y, double r)
		{
			this.DrawEllipse((float)x, (float)y, (float)r, (float)r, Operation.STROKE);
		}
		public void DrawCircle(double x, double y, double r, char operation)
		{
			this.DrawEllipse((float)x, (float)y, (float)r, (float)r, operation);
		}
		public void DrawEllipse(double x, double y, double r1, double r2)
		{
			this.DrawEllipse((float)x, (float)y, (float)r1, (float)r2, Operation.STROKE);
		}
		public void DrawEllipse(float x, float y, float r1, float r2)
		{
			this.DrawEllipse(x, y, r1, r2, Operation.STROKE);
		}
		public void FillEllipse(double x, double y, double r1, double r2)
		{
			this.DrawEllipse((float)x, (float)y, (float)r1, (float)r2, Operation.FILL);
		}
		public void FillEllipse(float x, float y, float r1, float r2)
		{
			this.DrawEllipse(x, y, r1, r2, Operation.FILL);
		}
		private void DrawEllipse(float x, float y, float r1, float r2, char operation)
		{
			float num = 0.551784f;
			this.DrawPath(new List<Point>
			{
				new Point(x, y - r2),
				new Point(x + num * r1, y - r2, Point.CONTROL_POINT),
				new Point(x + r1, y - num * r2, Point.CONTROL_POINT),
				new Point(x + r1, y),
				new Point(x + r1, y + num * r2, Point.CONTROL_POINT),
				new Point(x + num * r1, y + r2, Point.CONTROL_POINT),
				new Point(x, y + r2),
				new Point(x - num * r1, y + r2, Point.CONTROL_POINT),
				new Point(x - r1, y + num * r2, Point.CONTROL_POINT),
				new Point(x - r1, y),
				new Point(x - r1, y - num * r2, Point.CONTROL_POINT),
				new Point(x - num * r1, y - r2, Point.CONTROL_POINT),
				new Point(x, y - r2)
			}, operation);
		}
		public void DrawPoint(Point p)
		{
			if (p.shape != Point.INVISIBLE)
			{
				if (p.shape == Point.CIRCLE)
				{
					if (p.fillShape)
					{
						this.DrawCircle((double)p.x, (double)p.y, (double)p.r, 'f');
						return;
					}
					this.DrawCircle((double)p.x, (double)p.y, (double)p.r, 'S');
					return;
				}
				else
				{
					if (p.shape == Point.DIAMOND)
					{
						List<Point> list = new List<Point>();
						list.Add(new Point(p.x, p.y - p.r));
						list.Add(new Point(p.x + p.r, p.y));
						list.Add(new Point(p.x, p.y + p.r));
						list.Add(new Point(p.x - p.r, p.y));
						if (p.fillShape)
						{
							this.DrawPath(list, 'f');
							return;
						}
						this.DrawPath(list, 's');
						return;
					}
					else
					{
						if (p.shape == Point.BOX)
						{
							List<Point> list = new List<Point>();
							list.Add(new Point(p.x - p.r, p.y - p.r));
							list.Add(new Point(p.x + p.r, p.y - p.r));
							list.Add(new Point(p.x + p.r, p.y + p.r));
							list.Add(new Point(p.x - p.r, p.y + p.r));
							if (p.fillShape)
							{
								this.DrawPath(list, 'f');
								return;
							}
							this.DrawPath(list, 's');
							return;
						}
						else
						{
							if (p.shape == Point.PLUS)
							{
								this.DrawLine((double)(p.x - p.r), (double)p.y, (double)(p.x + p.r), (double)p.y);
								this.DrawLine((double)p.x, (double)(p.y - p.r), (double)p.x, (double)(p.y + p.r));
								return;
							}
							if (p.shape == Point.UP_ARROW)
							{
								List<Point> list = new List<Point>();
								list.Add(new Point(p.x, p.y - p.r));
								list.Add(new Point(p.x + p.r, p.y + p.r));
								list.Add(new Point(p.x - p.r, p.y + p.r));
								if (p.fillShape)
								{
									this.DrawPath(list, 'f');
									return;
								}
								this.DrawPath(list, 's');
								return;
							}
							else
							{
								if (p.shape == Point.DOWN_ARROW)
								{
									List<Point> list = new List<Point>();
									list.Add(new Point(p.x - p.r, p.y - p.r));
									list.Add(new Point(p.x + p.r, p.y - p.r));
									list.Add(new Point(p.x, p.y + p.r));
									if (p.fillShape)
									{
										this.DrawPath(list, 'f');
										return;
									}
									this.DrawPath(list, 's');
									return;
								}
								else
								{
									if (p.shape == Point.LEFT_ARROW)
									{
										List<Point> list = new List<Point>();
										list.Add(new Point(p.x + p.r, p.y + p.r));
										list.Add(new Point(p.x - p.r, p.y));
										list.Add(new Point(p.x + p.r, p.y - p.r));
										if (p.fillShape)
										{
											this.DrawPath(list, 'f');
											return;
										}
										this.DrawPath(list, 's');
										return;
									}
									else
									{
										if (p.shape == Point.RIGHT_ARROW)
										{
											List<Point> list = new List<Point>();
											list.Add(new Point(p.x - p.r, p.y - p.r));
											list.Add(new Point(p.x + p.r, p.y));
											list.Add(new Point(p.x - p.r, p.y + p.r));
											if (p.fillShape)
											{
												this.DrawPath(list, 'f');
												return;
											}
											this.DrawPath(list, 's');
											return;
										}
										else
										{
											if (p.shape == Point.H_DASH)
											{
												this.DrawLine((double)(p.x - p.r), (double)p.y, (double)(p.x + p.r), (double)p.y);
												return;
											}
											if (p.shape == Point.V_DASH)
											{
												this.DrawLine((double)p.x, (double)(p.y - p.r), (double)p.x, (double)(p.y + p.r));
												return;
											}
											if (p.shape == Point.X_MARK)
											{
												this.DrawLine((double)(p.x - p.r), (double)(p.y - p.r), (double)(p.x + p.r), (double)(p.y + p.r));
												this.DrawLine((double)(p.x - p.r), (double)(p.y + p.r), (double)(p.x + p.r), (double)(p.y - p.r));
												return;
											}
											if (p.shape == Point.MULTIPLY)
											{
												this.DrawLine((double)(p.x - p.r), (double)(p.y - p.r), (double)(p.x + p.r), (double)(p.y + p.r));
												this.DrawLine((double)(p.x - p.r), (double)(p.y + p.r), (double)(p.x + p.r), (double)(p.y - p.r));
												this.DrawLine((double)(p.x - p.r), (double)p.y, (double)(p.x + p.r), (double)p.y);
												this.DrawLine((double)p.x, (double)(p.y - p.r), (double)p.x, (double)(p.y + p.r));
												return;
											}
											if (p.shape == Point.STAR)
											{
												double num = 0.31415926535897931;
												double num2 = Math.Sin(num);
												double num3 = Math.Cos(num);
												double num4 = (double)p.r * num3;
												double num5 = (double)p.r * num2;
												double num6 = 2.0 * num4 * num2;
												double num7 = 2.0 * num4 * num3 - (double)p.r;
												List<Point> list = new List<Point>();
												list.Add(new Point(p.x, p.y - p.r));
												list.Add(new Point((double)p.x + num6, (double)p.y + num7));
												list.Add(new Point((double)p.x - num4, (double)p.y - num5));
												list.Add(new Point((double)p.x + num4, (double)p.y - num5));
												list.Add(new Point((double)p.x - num6, (double)p.y + num7));
												if (p.fillShape)
												{
													this.DrawPath(list, 'f');
													return;
												}
												this.DrawPath(list, 's');
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		public void SetTextRenderingMode(int mode)
		{
			if (mode >= 0 && mode <= 7)
			{
				this.renderingMode = mode;
				return;
			}
			throw new Exception("Invalid text rendering mode: " + mode);
		}
		public void SetTextDirection(int degrees)
		{
			if (degrees > 360)
			{
				degrees %= 360;
			}
			if (degrees == 0)
			{
				this.tm = new float[]
				{
					1f,
					0f,
					0f,
					1f
				};
				return;
			}
			if (degrees == 90)
			{
				float[] array = new float[4];
				array[1] = 1f;
				array[2] = -1f;
				this.tm = array;
				return;
			}
			if (degrees == 180)
			{
				this.tm = new float[]
				{
					-1f,
					0f,
					0f,
					-1f
				};
				return;
			}
			if (degrees == 270)
			{
				float[] array2 = new float[4];
				array2[1] = -1f;
				array2[2] = 1f;
				this.tm = array2;
				return;
			}
			if (degrees == 360)
			{
				this.tm = new float[]
				{
					1f,
					0f,
					0f,
					1f
				};
				return;
			}
			float num = (float)Math.Sin((double)degrees * 0.017453292519943295);
			float num2 = (float)Math.Cos((double)degrees * 0.017453292519943295);
			this.tm = new float[]
			{
				num2,
				num,
				-num,
				num2
			};
		}
		public void BezierCurveTo(Point p1, Point p2, Point p3)
		{
			this.Append(p1);
			this.Append(p2);
			this.Append(p3);
			this.Append("c\n");
		}
		public void SetTextStart()
		{
			this.Append("BT\n");
		}
		public void SetTextLocation(float x, float y)
		{
			this.Append(x);
			this.Append(' ');
			this.Append(this.height - y);
			this.Append(" Td\n");
		}
		public void SetTextBegin(float x, float y)
		{
			this.Append("BT\n");
			this.Append(x);
			this.Append(' ');
			this.Append(this.height - y);
			this.Append(" Td\n");
		}
		public void SetTextLeading(float leading)
		{
			this.Append(leading);
			this.Append(" TL\n");
		}
		public void SetCharSpacing(float spacing)
		{
			this.Append(spacing);
			this.Append(" Tc\n");
		}
		public void SetWordSpacing(float spacing)
		{
			this.Append(spacing);
			this.Append(" Tw\n");
		}
		public void SetTextScaling(float scaling)
		{
			this.Append(scaling);
			this.Append(" Tz\n");
		}
		public void SetTextRise(float rise)
		{
			this.Append(rise);
			this.Append(" Ts\n");
		}
		public void SetTextFont(Font font)
		{
			this.font = font;
			this.Append("/F");
			this.Append(font.objNumber);
			this.Append(' ');
			this.Append(font.size);
			this.Append(" Tf\n");
		}
		public void Println(string str)
		{
			this.Print(str);
			this.Println();
		}
		public void Print(string str)
		{
			this.Append('(');
			if (this.font != null && this.font.isComposite)
			{
				for (int i = 0; i < str.Length; i++)
				{
					int c = (int)str[i];
					this.DrawTwoByteChar(c, this.font);
				}
			}
			else
			{
				for (int j = 0; j < str.Length; j++)
				{
					int num = (int)str[j];
					if (num == 40 || num == 41 || num == 92)
					{
						this.Append('\\');
						this.Append((byte)num);
					}
					else
					{
						if (num == 9)
						{
							this.Append(' ');
							this.Append(' ');
							this.Append(' ');
							this.Append(' ');
						}
						else
						{
							this.Append((byte)num);
						}
					}
				}
			}
			this.Append(") Tj\n");
		}
		public void Println()
		{
			this.Append("T*\n");
		}
		public void SetTextEnd()
		{
			this.Append("ET\n");
		}
		public void DrawRectRoundCorners(float x, float y, float w, float h, float r1, float r2, char operation)
		{
			float num = 0.551784f;
			this.DrawPath(new List<Point>
			{
				new Point(x + w - r1, y),
				new Point(x + w - r1 + num * r1, y, Point.CONTROL_POINT),
				new Point(x + w, y + r2 - num * r2, Point.CONTROL_POINT),
				new Point(x + w, y + r2),
				new Point(x + w, y + h - r2),
				new Point(x + w, y + h - r2 + num * r2, Point.CONTROL_POINT),
				new Point(x + w - num * r1, y + h, Point.CONTROL_POINT),
				new Point(x + w - r1, y + h),
				new Point(x + r1, y + h),
				new Point(x + r1 - num * r1, y + h, Point.CONTROL_POINT),
				new Point(x, y + h - num * r2, Point.CONTROL_POINT),
				new Point(x, y + h - r2),
				new Point(x, y + r2),
				new Point(x, y + r2 - num * r2, Point.CONTROL_POINT),
				new Point(x + num * r1, y, Point.CONTROL_POINT),
				new Point(x + r1, y),
				new Point(x + w - r1, y)
			}, operation);
		}
		public void ClipPath()
		{
			this.Append("W\n");
			this.Append("n\n");
		}
		public void ClipRect(float x, float y, float w, float h)
		{
			this.MoveTo(x, y);
			this.LineTo(x + w, y);
			this.LineTo(x + w, y + h);
			this.LineTo(x, y + h);
			this.ClipPath();
		}
		public void Save()
		{
			this.Append("q\n");
			this.savedStates.Add(new State(this.pen, this.brush, this.pen_width, this.line_cap_style, this.line_join_style, this.linePattern));
		}
		public void Restore()
		{
			this.Append("Q\n");
			if (this.savedStates.Count > 0)
			{
				this.savedStates.RemoveAt(this.savedStates.Count - 1);
				State state = this.savedStates[this.savedStates.Count - 1];
				this.pen = state.GetPen();
				this.brush = state.GetBrush();
				this.pen_width = state.GetPenWidth();
				this.line_cap_style = state.GetLineCapStyle();
				this.line_join_style = state.GetLineJoinStyle();
				this.linePattern = state.GetLinePattern();
			}
		}
		public void SetCropBox(float upperLeftX, float upperLeftY, float lowerRightX, float lowerRightY)
		{
			this.cropBox = new float[]
			{
				upperLeftX,
				upperLeftY,
				lowerRightX,
				lowerRightY
			};
		}
		public void SetBleedBox(float upperLeftX, float upperLeftY, float lowerRightX, float lowerRightY)
		{
			this.bleedBox = new float[]
			{
				upperLeftX,
				upperLeftY,
				lowerRightX,
				lowerRightY
			};
		}
		public void SetTrimBox(float upperLeftX, float upperLeftY, float lowerRightX, float lowerRightY)
		{
			this.trimBox = new float[]
			{
				upperLeftX,
				upperLeftY,
				lowerRightX,
				lowerRightY
			};
		}
		public void SetArtBox(float upperLeftX, float upperLeftY, float lowerRightX, float lowerRightY)
		{
			this.artBox = new float[]
			{
				upperLeftX,
				upperLeftY,
				lowerRightX,
				lowerRightY
			};
		}
		private void Append(Point point)
		{
			this.Append(point.x);
			this.Append(' ');
			this.Append(this.height - point.y);
			this.Append(' ');
		}
		internal void Append(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				this.buf.WriteByte((byte)str[i]);
			}
		}
		internal void Append(int num)
		{
			this.Append(num.ToString());
		}
		internal void Append(float val)
		{
			this.Append(val.ToString().Replace(',', '.'));
		}
		internal void Append(double val)
		{
			this.Append(val.ToString().Replace(',', '.'));
		}
		internal void Append(char ch)
		{
			this.buf.WriteByte((byte)ch);
		}
		internal void Append(byte b)
		{
			this.buf.WriteByte(b);
		}
		public void Append(byte[] buffer)
		{
			this.buf.Write(buffer, 0, buffer.Length);
		}
		internal void DrawString(Font font, string str, float x, float y, Dictionary<string, int> colors)
		{
			this.SetTextBegin(x, y);
			this.SetTextFont(font);
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (char.IsLetterOrDigit(c))
				{
					this.PrintBuffer(stringBuilder2, colors);
					stringBuilder.Append(c);
				}
				else
				{
					this.PrintBuffer(stringBuilder, colors);
					stringBuilder2.Append(c);
				}
			}
			this.PrintBuffer(stringBuilder, colors);
			this.PrintBuffer(stringBuilder2, colors);
			this.SetTextEnd();
		}
		private void PrintBuffer(StringBuilder buf, Dictionary<string, int> colors)
		{
			string text = buf.ToString();
			if (text.Length > 0)
			{
				if (colors.ContainsKey(text))
				{
					this.SetBrushColor(colors[text]);
				}
				else
				{
					this.SetBrushColor(0);
				}
			}
			this.Print(text);
			buf.Length = 0;
		}
	}
}
