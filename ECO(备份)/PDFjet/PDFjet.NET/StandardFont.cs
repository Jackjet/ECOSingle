using System;
namespace PDFjet.NET
{
	internal class StandardFont
	{
		internal string name;
		internal int bBoxLLx;
		internal int bBoxLLy;
		internal int bBoxURx;
		internal int bBoxURy;
		internal int underlinePosition;
		internal int underlineThickness;
		internal int[][] metrics;
		internal static StandardFont GetInstance(CoreFont coreFont)
		{
			StandardFont standardFont = new StandardFont();
			switch (coreFont)
			{
			case CoreFont.COURIER:
				standardFont.name = Courier.name;
				standardFont.bBoxLLx = Courier.bBoxLLx;
				standardFont.bBoxLLy = Courier.bBoxLLy;
				standardFont.bBoxURx = Courier.bBoxURx;
				standardFont.bBoxURy = Courier.bBoxURy;
				standardFont.underlinePosition = Courier.underlinePosition;
				standardFont.underlineThickness = Courier.underlineThickness;
				standardFont.metrics = Courier.metrics;
				break;
			case CoreFont.COURIER_BOLD:
				standardFont.name = Courier_Bold.name;
				standardFont.bBoxLLx = Courier_Bold.bBoxLLx;
				standardFont.bBoxLLy = Courier_Bold.bBoxLLy;
				standardFont.bBoxURx = Courier_Bold.bBoxURx;
				standardFont.bBoxURy = Courier_Bold.bBoxURy;
				standardFont.underlinePosition = Courier_Bold.underlinePosition;
				standardFont.underlineThickness = Courier_Bold.underlineThickness;
				standardFont.metrics = Courier_Bold.metrics;
				break;
			case CoreFont.COURIER_OBLIQUE:
				standardFont.name = Courier_Oblique.name;
				standardFont.bBoxLLx = Courier_Oblique.bBoxLLx;
				standardFont.bBoxLLy = Courier_Oblique.bBoxLLy;
				standardFont.bBoxURx = Courier_Oblique.bBoxURx;
				standardFont.bBoxURy = Courier_Oblique.bBoxURy;
				standardFont.underlinePosition = Courier_Oblique.underlinePosition;
				standardFont.underlineThickness = Courier_Oblique.underlineThickness;
				standardFont.metrics = Courier_Oblique.metrics;
				break;
			case CoreFont.COURIER_BOLD_OBLIQUE:
				standardFont.name = Courier_BoldOblique.name;
				standardFont.bBoxLLx = Courier_BoldOblique.bBoxLLx;
				standardFont.bBoxLLy = Courier_BoldOblique.bBoxLLy;
				standardFont.bBoxURx = Courier_BoldOblique.bBoxURx;
				standardFont.bBoxURy = Courier_BoldOblique.bBoxURy;
				standardFont.underlinePosition = Courier_BoldOblique.underlinePosition;
				standardFont.underlineThickness = Courier_BoldOblique.underlineThickness;
				standardFont.metrics = Courier_BoldOblique.metrics;
				break;
			case CoreFont.HELVETICA:
				standardFont.name = Helvetica.name;
				standardFont.bBoxLLx = Helvetica.bBoxLLx;
				standardFont.bBoxLLy = Helvetica.bBoxLLy;
				standardFont.bBoxURx = Helvetica.bBoxURx;
				standardFont.bBoxURy = Helvetica.bBoxURy;
				standardFont.underlinePosition = Helvetica.underlinePosition;
				standardFont.underlineThickness = Helvetica.underlineThickness;
				standardFont.metrics = Helvetica.metrics;
				break;
			case CoreFont.HELVETICA_BOLD:
				standardFont.name = Helvetica_Bold.name;
				standardFont.bBoxLLx = Helvetica_Bold.bBoxLLx;
				standardFont.bBoxLLy = Helvetica_Bold.bBoxLLy;
				standardFont.bBoxURx = Helvetica_Bold.bBoxURx;
				standardFont.bBoxURy = Helvetica_Bold.bBoxURy;
				standardFont.underlinePosition = Helvetica_Bold.underlinePosition;
				standardFont.underlineThickness = Helvetica_Bold.underlineThickness;
				standardFont.metrics = Helvetica_Bold.metrics;
				break;
			case CoreFont.HELVETICA_OBLIQUE:
				standardFont.name = Helvetica_Oblique.name;
				standardFont.bBoxLLx = Helvetica_Oblique.bBoxLLx;
				standardFont.bBoxLLy = Helvetica_Oblique.bBoxLLy;
				standardFont.bBoxURx = Helvetica_Oblique.bBoxURx;
				standardFont.bBoxURy = Helvetica_Oblique.bBoxURy;
				standardFont.underlinePosition = Helvetica_Oblique.underlinePosition;
				standardFont.underlineThickness = Helvetica_Oblique.underlineThickness;
				standardFont.metrics = Helvetica_Oblique.metrics;
				break;
			case CoreFont.HELVETICA_BOLD_OBLIQUE:
				standardFont.name = Helvetica_BoldOblique.name;
				standardFont.bBoxLLx = Helvetica_BoldOblique.bBoxLLx;
				standardFont.bBoxLLy = Helvetica_BoldOblique.bBoxLLy;
				standardFont.bBoxURx = Helvetica_BoldOblique.bBoxURx;
				standardFont.bBoxURy = Helvetica_BoldOblique.bBoxURy;
				standardFont.underlinePosition = Helvetica_BoldOblique.underlinePosition;
				standardFont.underlineThickness = Helvetica_BoldOblique.underlineThickness;
				standardFont.metrics = Helvetica_BoldOblique.metrics;
				break;
			case CoreFont.TIMES_ROMAN:
				standardFont.name = Times_Roman.name;
				standardFont.bBoxLLx = Times_Roman.bBoxLLx;
				standardFont.bBoxLLy = Times_Roman.bBoxLLy;
				standardFont.bBoxURx = Times_Roman.bBoxURx;
				standardFont.bBoxURy = Times_Roman.bBoxURy;
				standardFont.underlinePosition = Times_Roman.underlinePosition;
				standardFont.underlineThickness = Times_Roman.underlineThickness;
				standardFont.metrics = Times_Roman.metrics;
				break;
			case CoreFont.TIMES_BOLD:
				standardFont.name = Times_Bold.name;
				standardFont.bBoxLLx = Times_Bold.bBoxLLx;
				standardFont.bBoxLLy = Times_Bold.bBoxLLy;
				standardFont.bBoxURx = Times_Bold.bBoxURx;
				standardFont.bBoxURy = Times_Bold.bBoxURy;
				standardFont.underlinePosition = Times_Bold.underlinePosition;
				standardFont.underlineThickness = Times_Bold.underlineThickness;
				standardFont.metrics = Times_Bold.metrics;
				break;
			case CoreFont.TIMES_ITALIC:
				standardFont.name = Times_Italic.name;
				standardFont.bBoxLLx = Times_Italic.bBoxLLx;
				standardFont.bBoxLLy = Times_Italic.bBoxLLy;
				standardFont.bBoxURx = Times_Italic.bBoxURx;
				standardFont.bBoxURy = Times_Italic.bBoxURy;
				standardFont.underlinePosition = Times_Italic.underlinePosition;
				standardFont.underlineThickness = Times_Italic.underlineThickness;
				standardFont.metrics = Times_Italic.metrics;
				break;
			case CoreFont.TIMES_BOLD_ITALIC:
				standardFont.name = Times_BoldItalic.name;
				standardFont.bBoxLLx = Times_BoldItalic.bBoxLLx;
				standardFont.bBoxLLy = Times_BoldItalic.bBoxLLy;
				standardFont.bBoxURx = Times_BoldItalic.bBoxURx;
				standardFont.bBoxURy = Times_BoldItalic.bBoxURy;
				standardFont.underlinePosition = Times_BoldItalic.underlinePosition;
				standardFont.underlineThickness = Times_BoldItalic.underlineThickness;
				standardFont.metrics = Times_BoldItalic.metrics;
				break;
			case CoreFont.SYMBOL:
				standardFont.name = Symbol.name;
				standardFont.bBoxLLx = Symbol.bBoxLLx;
				standardFont.bBoxLLy = Symbol.bBoxLLy;
				standardFont.bBoxURx = Symbol.bBoxURx;
				standardFont.bBoxURy = Symbol.bBoxURy;
				standardFont.underlinePosition = Symbol.underlinePosition;
				standardFont.underlineThickness = Symbol.underlineThickness;
				standardFont.metrics = Symbol.metrics;
				break;
			case CoreFont.ZAPF_DINGBATS:
				standardFont.name = ZapfDingbats.name;
				standardFont.bBoxLLx = ZapfDingbats.bBoxLLx;
				standardFont.bBoxLLy = ZapfDingbats.bBoxLLy;
				standardFont.bBoxURx = ZapfDingbats.bBoxURx;
				standardFont.bBoxURy = ZapfDingbats.bBoxURy;
				standardFont.underlinePosition = ZapfDingbats.underlinePosition;
				standardFont.underlineThickness = ZapfDingbats.underlineThickness;
				standardFont.metrics = ZapfDingbats.metrics;
				break;
			}
			return standardFont;
		}
	}
}
