using System;
using System.IO;
using System.Text;
namespace PDFjet.NET
{
	public class Font
	{
		internal string name;
		internal int objNumber;
		internal int fileObjNumber = -1;
		internal int unitsPerEm = 1000;
		internal float size = 12f;
		internal float ascent;
		internal float descent;
		internal float capHeight;
		internal float body_height;
		internal int[][] metrics;
		internal bool isStandard = true;
		internal bool kernPairs;
		internal bool isComposite;
		internal int firstChar = 32;
		internal int lastChar = 255;
		internal bool skew15;
		internal bool isCJK;
		internal float bBoxLLx;
		internal float bBoxLLy;
		internal float bBoxURx;
		internal float bBoxURy;
		internal float underlinePosition;
		internal float underlineThickness;
		internal int compressed_size;
		internal int uncompressed_size;
		internal int[] advanceWidth;
		internal int[] glyphWidth;
		internal int[] unicodeToGID;
		private int fontDescriptorObjNumber = -1;
		private int cMapObjNumber = -1;
		private int cidFontDictObjNumber = -1;
		private int toUnicodeCMapObjNumber = -1;
		private int widthsArrayObjNumber = -1;
		private int encodingObjNumber = -1;
		private int codePage = CodePage.CP1252;
		private int fontUnderlinePosition;
		private int fontUnderlineThickness;
		public Font(PDF pdf, PDFobj obj)
		{
			string text = obj.GetValue("/BaseFont").Substring(1);
			if (text.Equals(Helvetica.name))
			{
				this.AddCoreFont(pdf, CoreFont.HELVETICA);
				return;
			}
			if (text.Equals(Helvetica_Bold.name))
			{
				this.AddCoreFont(pdf, CoreFont.HELVETICA_BOLD);
				return;
			}
			if (text.Equals(Helvetica_Oblique.name))
			{
				this.AddCoreFont(pdf, CoreFont.HELVETICA_OBLIQUE);
				return;
			}
			if (text.Equals(Helvetica_BoldOblique.name))
			{
				this.AddCoreFont(pdf, CoreFont.HELVETICA_BOLD_OBLIQUE);
				return;
			}
			if (text.Equals(Times_Roman.name))
			{
				this.AddCoreFont(pdf, CoreFont.TIMES_ROMAN);
				return;
			}
			if (text.Equals(Times_Bold.name))
			{
				this.AddCoreFont(pdf, CoreFont.TIMES_BOLD);
				return;
			}
			if (text.Equals(Times_Italic.name))
			{
				this.AddCoreFont(pdf, CoreFont.TIMES_ITALIC);
				return;
			}
			if (text.Equals(Times_BoldItalic.name))
			{
				this.AddCoreFont(pdf, CoreFont.TIMES_BOLD_ITALIC);
				return;
			}
			if (text.Equals(Courier.name))
			{
				this.AddCoreFont(pdf, CoreFont.COURIER);
				return;
			}
			if (text.Equals(Courier_Bold.name))
			{
				this.AddCoreFont(pdf, CoreFont.COURIER_BOLD);
				return;
			}
			if (text.Equals(Courier_Oblique.name))
			{
				this.AddCoreFont(pdf, CoreFont.COURIER_OBLIQUE);
				return;
			}
			if (text.Equals(Courier_BoldOblique.name))
			{
				this.AddCoreFont(pdf, CoreFont.COURIER_BOLD_OBLIQUE);
				return;
			}
			if (text.Equals(Symbol.name))
			{
				this.AddCoreFont(pdf, CoreFont.SYMBOL);
				return;
			}
			if (text.Equals(ZapfDingbats.name))
			{
				this.AddCoreFont(pdf, CoreFont.ZAPF_DINGBATS);
			}
		}
		public Font(PDF pdf, CoreFont coreFont)
		{
			this.AddCoreFont(pdf, coreFont);
		}
		private void AddCoreFont(PDF pdf, CoreFont coreFont)
		{
			StandardFont instance = StandardFont.GetInstance(coreFont);
			this.name = instance.name;
			this.bBoxLLx = (float)instance.bBoxLLx;
			this.bBoxLLy = (float)instance.bBoxLLy;
			this.bBoxURx = (float)instance.bBoxURx;
			this.bBoxURy = (float)instance.bBoxURy;
			this.fontUnderlinePosition = instance.underlinePosition;
			this.fontUnderlineThickness = instance.underlineThickness;
			this.metrics = instance.metrics;
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /Font\n");
			pdf.Append("/Subtype /Type1\n");
			pdf.Append("/BaseFont /");
			pdf.Append(this.name);
			pdf.Append('\n');
			if (!this.name.Equals("Symbol") && !this.name.Equals("ZapfDingbats"))
			{
				pdf.Append("/Encoding /WinAnsiEncoding\n");
			}
			pdf.Append(">>\n");
			pdf.Endobj();
			this.objNumber = pdf.objNumber;
			this.ascent = this.bBoxURy * this.size / (float)this.unitsPerEm;
			this.descent = this.bBoxLLy * this.size / (float)this.unitsPerEm;
			this.body_height = this.ascent - this.descent;
			this.underlineThickness = (float)this.fontUnderlineThickness * this.size / (float)this.unitsPerEm;
			this.underlinePosition = (float)this.fontUnderlinePosition * this.size / (float)(-(float)this.unitsPerEm) + this.underlineThickness / 2f;
			pdf.fonts.Add(this);
		}
		public Font(PDF pdf, string fontName, int codePage)
		{
			this.name = fontName;
			this.codePage = codePage;
			this.isCJK = true;
			this.isStandard = false;
			this.isComposite = true;
			this.firstChar = 32;
			this.lastChar = 65518;
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /FontDescriptor\n");
			pdf.Append("/FontName /");
			pdf.Append(fontName);
			pdf.Append('\n');
			pdf.Append("/Flags 4\n");
			pdf.Append("/FontBBox [0 0 0 0]\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /Font\n");
			pdf.Append("/Subtype /CIDFontType0\n");
			pdf.Append("/BaseFont /");
			pdf.Append(fontName);
			pdf.Append('\n');
			pdf.Append("/FontDescriptor ");
			pdf.Append(pdf.objNumber - 1);
			pdf.Append(" 0 R\n");
			pdf.Append("/CIDSystemInfo <<\n");
			pdf.Append("/Registry (Adobe)\n");
			if (fontName.StartsWith("AdobeMingStd"))
			{
				pdf.Append("/Ordering (CNS1)\n");
				pdf.Append("/Supplement 4\n");
			}
			else
			{
				if (fontName.StartsWith("AdobeSongStd") || fontName.StartsWith("STHeitiSC"))
				{
					pdf.Append("/Ordering (GB1)\n");
					pdf.Append("/Supplement 4\n");
				}
				else
				{
					if (fontName.StartsWith("KozMinPro"))
					{
						pdf.Append("/Ordering (Japan1)\n");
						pdf.Append("/Supplement 4\n");
					}
					else
					{
						if (!fontName.StartsWith("AdobeMyungjoStd"))
						{
							throw new Exception("Unsupported font: " + fontName);
						}
						pdf.Append("/Ordering (Korea1)\n");
						pdf.Append("/Supplement 1\n");
					}
				}
			}
			pdf.Append(">>\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /Font\n");
			pdf.Append("/Subtype /Type0\n");
			pdf.Append("/BaseFont /");
			if (fontName.StartsWith("AdobeMingStd"))
			{
				pdf.Append(fontName + "-UniCNS-UTF16-H\n");
				pdf.Append("/Encoding /UniCNS-UTF16-H\n");
			}
			else
			{
				if (fontName.StartsWith("AdobeSongStd") || fontName.StartsWith("STHeitiSC"))
				{
					pdf.Append(fontName + "-UniGB-UTF16-H\n");
					pdf.Append("/Encoding /UniGB-UTF16-H\n");
				}
				else
				{
					if (fontName.StartsWith("KozMinPro"))
					{
						pdf.Append(fontName + "-UniJIS-UCS2-H\n");
						pdf.Append("/Encoding /UniJIS-UCS2-H\n");
					}
					else
					{
						if (!fontName.StartsWith("AdobeMyungjoStd"))
						{
							throw new Exception("Unsupported font: " + fontName);
						}
						pdf.Append(fontName + "-UniKS-UCS2-H\n");
						pdf.Append("/Encoding /UniKS-UCS2-H\n");
					}
				}
			}
			pdf.Append("/DescendantFonts [");
			pdf.Append(pdf.objNumber - 1);
			pdf.Append(" 0 R]\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			this.objNumber = pdf.objNumber;
			this.ascent = this.size;
			this.descent = -this.ascent / 4f;
			this.body_height = this.ascent - this.descent;
			pdf.fonts.Add(this);
		}
		public Font(PDF pdf, Stream inputStream)
		{
			this.isStandard = false;
			this.isComposite = true;
			this.codePage = CodePage.UNICODE;
			FastFont.Register(pdf, this, inputStream);
			this.ascent = this.bBoxURy * this.size / (float)this.unitsPerEm;
			this.descent = this.bBoxLLy * this.size / (float)this.unitsPerEm;
			this.body_height = this.ascent - this.descent;
			this.underlineThickness = (float)this.fontUnderlineThickness * this.size / (float)this.unitsPerEm;
			this.underlinePosition = (float)this.fontUnderlinePosition * this.size / (float)(-(float)this.unitsPerEm) + this.underlineThickness / 2f;
			pdf.fonts.Add(this);
		}
		internal int GetFontDescriptorObjNumber()
		{
			return this.fontDescriptorObjNumber;
		}
		internal int GetCMapObjNumber()
		{
			return this.cMapObjNumber;
		}
		internal int GetCidFontDictObjNumber()
		{
			return this.cidFontDictObjNumber;
		}
		internal int GetToUnicodeCMapObjNumber()
		{
			return this.toUnicodeCMapObjNumber;
		}
		internal int GetWidthsArrayObjNumber()
		{
			return this.widthsArrayObjNumber;
		}
		internal int GetEncodingObjNumber()
		{
			return this.encodingObjNumber;
		}
		internal void SetFontDescriptorObjNumber(int objNumber)
		{
			this.fontDescriptorObjNumber = objNumber;
		}
		internal void SetCMapObjNumber(int objNumber)
		{
			this.cMapObjNumber = objNumber;
		}
		internal void SetCidFontDictObjNumber(int objNumber)
		{
			this.cidFontDictObjNumber = objNumber;
		}
		internal void SetToUnicodeCMapObjNumber(int objNumber)
		{
			this.toUnicodeCMapObjNumber = objNumber;
		}
		internal void SetWidthsArrayObjNumber(int objNumber)
		{
			this.widthsArrayObjNumber = objNumber;
		}
		internal void SetEncodingObjNumber(int objNumber)
		{
			this.encodingObjNumber = objNumber;
		}
		public void SetSize(double fontSize)
		{
			this.SetSize((float)fontSize);
		}
		public void SetSize(float fontSize)
		{
			this.size = fontSize;
			if (this.isCJK)
			{
				this.ascent = this.size;
				this.descent = -this.ascent / 4f;
				return;
			}
			this.ascent = this.bBoxURy * this.size / (float)this.unitsPerEm;
			this.descent = this.bBoxLLy * this.size / (float)this.unitsPerEm;
			this.body_height = this.ascent - this.descent;
			this.underlineThickness = (float)this.fontUnderlineThickness * this.size / (float)this.unitsPerEm;
			this.underlinePosition = (float)this.fontUnderlinePosition * this.size / (float)(-(float)this.unitsPerEm) + this.underlineThickness / 2f;
		}
		public float GetSize()
		{
			return this.size;
		}
		public void SetKernPairs(bool kernPairs)
		{
			this.kernPairs = kernPairs;
		}
		public float StringWidth(string str)
		{
			if (str == null)
			{
				return 0f;
			}
			int num = 0;
			if (this.isCJK)
			{
				return (float)str.Length * this.ascent;
			}
			for (int i = 0; i < str.Length; i++)
			{
				int num2 = (int)str[i];
				if (this.isStandard)
				{
					if (num2 < this.firstChar || num2 > this.lastChar)
					{
						num2 = this.MapUnicodeChar(num2);
					}
					num2 -= 32;
					num += this.metrics[num2][1];
					if (this.kernPairs && i < str.Length - 1)
					{
						int num3 = (int)str[i + 1];
						if (num3 < this.firstChar || num3 > this.lastChar)
						{
							num3 = 32;
						}
						for (int j = 2; j < this.metrics[num2].Length; j += 2)
						{
							if (this.metrics[num2][j] == num3)
							{
								num += this.metrics[num2][j + 1];
								break;
							}
						}
					}
				}
				else
				{
					if (num2 < this.firstChar || num2 > this.lastChar)
					{
						num += this.advanceWidth[0];
					}
					else
					{
						num += this.nonStandardFontGlyphWidth(num2);
					}
				}
			}
			return (float)num * this.size / (float)this.unitsPerEm;
		}
		private int nonStandardFontGlyphWidth(int c1)
		{
			int result = 0;
			if (this.isComposite)
			{
				result = this.glyphWidth[c1];
			}
			else
			{
				if (c1 < 127)
				{
					result = this.glyphWidth[c1];
				}
				else
				{
					if (this.codePage == 0)
					{
						result = this.glyphWidth[CP1250.codes[c1 - 127]];
					}
					else
					{
						if (this.codePage == 1)
						{
							result = this.glyphWidth[CP1251.codes[c1 - 127]];
						}
						else
						{
							if (this.codePage == 2)
							{
								result = this.glyphWidth[CP1252.codes[c1 - 127]];
							}
							else
							{
								if (this.codePage == 3)
								{
									result = this.glyphWidth[CP1253.codes[c1 - 127]];
								}
								else
								{
									if (this.codePage == 4)
									{
										result = this.glyphWidth[CP1254.codes[c1 - 127]];
									}
									else
									{
										if (this.codePage == 7)
										{
											result = this.glyphWidth[CP1257.codes[c1 - 127]];
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
		public float GetAscent()
		{
			return this.ascent;
		}
		public float GetDescent()
		{
			return -this.descent;
		}
		public float GetHeight()
		{
			return this.ascent - this.descent;
		}
		public float GetBodyHeight()
		{
			return this.body_height;
		}
		public int GetFitChars(string str, double width)
		{
			return this.GetFitChars(str, (float)width);
		}
		public int GetFitChars(string str, float width)
		{
			float num = width * (float)this.unitsPerEm / this.size;
			if (this.isCJK)
			{
				return (int)(num / this.ascent);
			}
			if (this.isStandard)
			{
				return this.GetStandardFontFitChars(str, num);
			}
			int i;
			for (i = 0; i < str.Length; i++)
			{
				int num2 = (int)str[i];
				if (num2 < this.firstChar || num2 > this.lastChar)
				{
					num -= (float)this.advanceWidth[0];
				}
				else
				{
					num -= (float)this.nonStandardFontGlyphWidth(num2);
				}
				if (num < 0f)
				{
					break;
				}
			}
			return i;
		}
		private int GetStandardFontFitChars(string str, double width)
		{
			return this.GetStandardFontFitChars(str, (float)width);
		}
		private int GetStandardFontFitChars(string str, float width)
		{
			float num = width;
			int i;
			for (i = 0; i < str.Length; i++)
			{
				int num2 = (int)str[i];
				if (num2 < this.firstChar || num2 > this.lastChar)
				{
					num2 = 32;
				}
				num2 -= 32;
				num -= (float)this.metrics[num2][1];
				if (num < 0f)
				{
					return i;
				}
				if (this.kernPairs && i < str.Length - 1)
				{
					int num3 = (int)str[i + 1];
					if (num3 < this.firstChar || num3 > this.lastChar)
					{
						num3 = 32;
					}
					int j = 2;
					while (j < this.metrics[num2].Length)
					{
						if (this.metrics[num2][j] == num3)
						{
							num -= (float)this.metrics[num2][j + 1];
							if (num < 0f)
							{
								return i;
							}
							break;
						}
						else
						{
							j += 2;
						}
					}
				}
			}
			return i;
		}
		public int MapUnicodeChar(int c1)
		{
			int[] array = null;
			if (this.codePage == CodePage.CP1250)
			{
				array = CP1250.codes;
			}
			else
			{
				if (this.codePage == CodePage.CP1251)
				{
					array = CP1251.codes;
				}
				else
				{
					if (this.codePage == CodePage.CP1252)
					{
						array = CP1252.codes;
					}
					else
					{
						if (this.codePage == CodePage.CP1253)
						{
							array = CP1253.codes;
						}
						else
						{
							if (this.codePage == CodePage.CP1254)
							{
								array = CP1254.codes;
							}
							else
							{
								if (this.codePage == CodePage.CP1257)
								{
									array = CP1257.codes;
								}
							}
						}
					}
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == c1)
					{
						return 127 + i;
					}
				}
			}
			return 32;
		}
		public void SetItalic(bool skew15)
		{
			this.skew15 = skew15;
		}
		public float StringWidth(Font font2, string str)
		{
			float num = 0f;
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				int num2 = (int)str[i];
				if ((this.isCJK && num2 >= 19968 && num2 <= 40908) || (!this.isCJK && this.unicodeToGID[num2] != 0))
				{
					if (!flag)
					{
						num += font2.StringWidth(stringBuilder.ToString());
						stringBuilder = new StringBuilder();
						flag = true;
					}
				}
				else
				{
					if (flag)
					{
						num += this.StringWidth(stringBuilder.ToString());
						stringBuilder = new StringBuilder();
						flag = false;
					}
				}
				stringBuilder.Append((char)num2);
			}
			if (flag)
			{
				num += this.StringWidth(stringBuilder.ToString());
			}
			else
			{
				num += font2.StringWidth(stringBuilder.ToString());
			}
			return num;
		}
	}
}
