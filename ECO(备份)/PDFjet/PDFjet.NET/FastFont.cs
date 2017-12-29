using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace PDFjet.NET
{
	internal class FastFont
	{
		internal static void Register(PDF pdf, Font font, Stream inputStream)
		{
			font.name = DejaVuLGCSerif.name;
			font.unitsPerEm = DejaVuLGCSerif.unitsPerEm;
			font.bBoxLLx = (float)DejaVuLGCSerif.bBoxLLx;
			font.bBoxLLy = (float)DejaVuLGCSerif.bBoxLLy;
			font.bBoxURx = (float)DejaVuLGCSerif.bBoxURx;
			font.bBoxURy = (float)DejaVuLGCSerif.bBoxURy;
			font.ascent = (float)DejaVuLGCSerif.ascent;
			font.descent = (float)DejaVuLGCSerif.descent;
			font.capHeight = (float)DejaVuLGCSerif.capHeight;
			font.firstChar = DejaVuLGCSerif.firstChar;
			font.lastChar = DejaVuLGCSerif.lastChar;
			font.underlinePosition = (float)DejaVuLGCSerif.underlinePosition;
			font.underlineThickness = (float)DejaVuLGCSerif.underlineThickness;
			font.compressed_size = DejaVuLGCSerif.compressed_size;
			font.uncompressed_size = DejaVuLGCSerif.uncompressed_size;
			font.advanceWidth = FastFont.DecodeRLE(DejaVuLGCSerif.advanceWidth);
			font.glyphWidth = FastFont.DecodeRLE(DejaVuLGCSerif.glyphWidth);
			font.unicodeToGID = FastFont.DecodeRLE(DejaVuLGCSerif.unicodeToGID);
			FastFont.EmbedFontFile(pdf, font, inputStream);
			FastFont.AddFontDescriptorObject(pdf, font);
			FastFont.AddCIDFontDictionaryObject(pdf, font);
			FastFont.AddToUnicodeCMapObject(pdf, font);
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /Font\n");
			pdf.Append("/Subtype /Type0\n");
			pdf.Append("/BaseFont /");
			pdf.Append(font.name);
			pdf.Append('\n');
			pdf.Append("/Encoding /Identity-H\n");
			pdf.Append("/DescendantFonts [");
			pdf.Append(font.GetCidFontDictObjNumber());
			pdf.Append(" 0 R]\n");
			pdf.Append("/ToUnicode ");
			pdf.Append(font.GetToUnicodeCMapObjNumber());
			pdf.Append(" 0 R\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			font.objNumber = pdf.objNumber;
		}
		private static void EmbedFontFile(PDF pdf, Font font, Stream inputStream)
		{
			for (int i = 0; i < pdf.fonts.Count; i++)
			{
				Font font2 = pdf.fonts[i];
				if (font2.name.Equals(font.name) && font2.fileObjNumber != -1)
				{
					font.fileObjNumber = font2.fileObjNumber;
					return;
				}
			}
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Filter /FlateDecode\n");
			pdf.Append("/Length ");
			pdf.Append(font.compressed_size);
			pdf.Append("\n");
			pdf.Append("/Length1 ");
			pdf.Append(font.uncompressed_size);
			pdf.Append('\n');
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			int num;
			while ((num = inputStream.ReadByte()) != -1)
			{
				pdf.Append((byte)num);
			}
			inputStream.Dispose();
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			font.fileObjNumber = pdf.objNumber;
		}
		private static void AddFontDescriptorObject(PDF pdf, Font font)
		{
			float num = 1000f / (float)font.unitsPerEm;
			for (int i = 0; i < pdf.fonts.Count; i++)
			{
				Font font2 = pdf.fonts[i];
				if (font2.name.Equals(font.name) && font2.GetFontDescriptorObjNumber() != -1)
				{
					font.SetFontDescriptorObjNumber(font2.GetFontDescriptorObjNumber());
					return;
				}
			}
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /FontDescriptor\n");
			pdf.Append("/FontName /");
			pdf.Append(font.name);
			pdf.Append('\n');
			pdf.Append("/FontFile2 ");
			pdf.Append(font.fileObjNumber);
			pdf.Append(" 0 R\n");
			pdf.Append("/Flags 32\n");
			pdf.Append("/FontBBox [");
			pdf.Append((double)(font.bBoxLLx * num));
			pdf.Append(' ');
			pdf.Append((double)(font.bBoxLLy * num));
			pdf.Append(' ');
			pdf.Append((double)(font.bBoxURx * num));
			pdf.Append(' ');
			pdf.Append((double)(font.bBoxURy * num));
			pdf.Append("]\n");
			pdf.Append("/Ascent ");
			pdf.Append((double)(font.ascent * num));
			pdf.Append('\n');
			pdf.Append("/Descent ");
			pdf.Append((double)(font.descent * num));
			pdf.Append('\n');
			pdf.Append("/ItalicAngle 0\n");
			pdf.Append("/CapHeight ");
			pdf.Append((double)(font.capHeight * num));
			pdf.Append('\n');
			pdf.Append("/StemV 79\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			font.SetFontDescriptorObjNumber(pdf.objNumber);
		}
		private static void AddToUnicodeCMapObject(PDF pdf, Font font)
		{
			for (int i = 0; i < pdf.fonts.Count; i++)
			{
				Font font2 = pdf.fonts[i];
				if (font2.name.Equals(font.name) && font2.GetToUnicodeCMapObjNumber() != -1)
				{
					font.SetToUnicodeCMapObjNumber(font2.GetToUnicodeCMapObjNumber());
					return;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("/CIDInit /ProcSet findresource begin\n");
			stringBuilder.Append("12 dict begin\n");
			stringBuilder.Append("begincmap\n");
			stringBuilder.Append("/CIDSystemInfo <</Registry (Adobe) /Ordering (Identity) /Supplement 0>> def\n");
			stringBuilder.Append("/CMapName /Adobe-Identity def\n");
			stringBuilder.Append("/CMapType 2 def\n");
			stringBuilder.Append("1 begincodespacerange\n");
			stringBuilder.Append("<0000> <FFFF>\n");
			stringBuilder.Append("endcodespacerange\n");
			List<string> list = new List<string>();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j <= 65535; j++)
			{
				int num = font.unicodeToGID[j];
				if (num > 0)
				{
					stringBuilder2.Append('<');
					stringBuilder2.Append(FastFont.ToHexString(num));
					stringBuilder2.Append("> <");
					stringBuilder2.Append(FastFont.ToHexString(j));
					stringBuilder2.Append(">\n");
					list.Add(stringBuilder2.ToString());
					stringBuilder2.Length = 0;
					if (list.Count == 100)
					{
						FastFont.WriteListToBuffer(list, stringBuilder);
					}
				}
			}
			if (list.Count > 0)
			{
				FastFont.WriteListToBuffer(list, stringBuilder);
			}
			stringBuilder.Append("endcmap\n");
			stringBuilder.Append("CMapName currentdict /CMap defineresource pop\n");
			stringBuilder.Append("end\nend");
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Length ");
			pdf.Append(stringBuilder.Length);
			pdf.Append("\n");
			pdf.Append(">>\n");
			pdf.Append("stream\n");
			pdf.Append(stringBuilder.ToString());
			pdf.Append("\nendstream\n");
			pdf.Endobj();
			font.SetToUnicodeCMapObjNumber(pdf.objNumber);
		}
		private static void AddCIDFontDictionaryObject(PDF pdf, Font font)
		{
			for (int i = 0; i < pdf.fonts.Count; i++)
			{
				Font font2 = pdf.fonts[i];
				if (font2.name.Equals(font.name) && font2.GetCidFontDictObjNumber() != -1)
				{
					font.SetCidFontDictObjNumber(font2.GetCidFontDictObjNumber());
					return;
				}
			}
			pdf.Newobj();
			pdf.Append("<<\n");
			pdf.Append("/Type /Font\n");
			pdf.Append("/Subtype /CIDFontType2\n");
			pdf.Append("/BaseFont /");
			pdf.Append(font.name);
			pdf.Append('\n');
			pdf.Append("/CIDSystemInfo <</Registry (Adobe) /Ordering (Identity) /Supplement 0>>\n");
			pdf.Append("/FontDescriptor ");
			pdf.Append(font.GetFontDescriptorObjNumber());
			pdf.Append(" 0 R\n");
			pdf.Append("/DW ");
			pdf.Append((int)(1000f / (float)font.unitsPerEm * (float)font.advanceWidth[0]));
			pdf.Append('\n');
			pdf.Append("/W [0[\n");
			for (int j = 0; j < font.advanceWidth.Length; j++)
			{
				pdf.Append((int)(1000f / (float)font.unitsPerEm * (float)font.advanceWidth[j]));
				if ((j + 1) % 10 == 0)
				{
					pdf.Append('\n');
				}
				else
				{
					pdf.Append(' ');
				}
			}
			pdf.Append("]]\n");
			pdf.Append("/CIDToGIDMap /Identity\n");
			pdf.Append(">>\n");
			pdf.Endobj();
			font.SetCidFontDictObjNumber(pdf.objNumber);
		}
		private static string ToHexString(int code)
		{
			string text = Convert.ToString(code, 16);
			if (text.Length == 1)
			{
				return "000" + text;
			}
			if (text.Length == 2)
			{
				return "00" + text;
			}
			if (text.Length == 3)
			{
				return "0" + text;
			}
			return text;
		}
		private static void WriteListToBuffer(List<string> list, StringBuilder sb)
		{
			sb.Append(list.Count);
			sb.Append(" beginbfchar\n");
			foreach (string current in list)
			{
				sb.Append(current);
			}
			sb.Append("endbfchar\n");
			list.Clear();
		}
		private static int[] DecodeRLE(int[] buf)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < buf.Length; i++)
			{
				int num = buf[i];
				int num2 = num >> 16 & 65535;
				int item = num & 65535;
				for (int j = 0; j < num2; j++)
				{
					list.Add(item);
				}
			}
			int[] array = new int[list.Count];
			for (int k = 0; k < list.Count; k++)
			{
				array[k] = list[k];
			}
			return array;
		}
	}
}
