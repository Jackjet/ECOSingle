using EcoSensors._Lang;
using PDFjet.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
namespace EcoSensors.Common
{
	internal class pdfUtil
	{
		public static float PDFleft_margin = 70f;
		public static float PDFright_margin = 40f;
		public static float PDFtop_margin = 30f;
		public static float PDFbottom_margin = 30f;
		public static float PDFLetter_maxWidth = 612f;
		public static float PDFLetter_maxHeight = 792f;
		public static float PDFpageeffwidth = pdfUtil.PDFLetter_maxWidth - pdfUtil.PDFleft_margin - pdfUtil.PDFright_margin;
		public static float PDFpageeffHeight = pdfUtil.PDFLetter_maxHeight - pdfUtil.PDFtop_margin - pdfUtil.PDFbottom_margin;
		private static float PDFLoctx_pageNo = 300f;
		private static float PDFLocty_pageNo = 780f;
		public static float DrawImg(PDF pdf, Page page, string imgfile, float offsetY)
		{
			System.Drawing.Image image = System.Drawing.Image.FromFile(imgfile);
			double val = (double)pdfUtil.PDFpageeffwidth / (double)image.Width;
			double val2 = (double)(pdfUtil.PDFpageeffHeight - offsetY) / (double)image.Height;
			double factor = System.Math.Min(val, val2);
			PDFjet.NET.Image image2 = new PDFjet.NET.Image(pdf, new System.IO.BufferedStream(new System.IO.FileStream(imgfile, System.IO.FileMode.Open, System.IO.FileAccess.Read)), ImageType.JPG);
			image2.SetLocation(pdfUtil.PDFleft_margin, pdfUtil.PDFtop_margin + offsetY);
			image2.ScaleBy(factor);
			image2.DrawOn(page);
			return image2.GetHeight();
		}
		public static void DrawTitle(PDF pdf, Page page, string title, float offsetY, float fontsize, int txtAlign)
		{
			PDFjet.NET.Font font = new PDFjet.NET.Font(pdf, CoreFont.TIMES_BOLD);
			font.SetSize(fontsize);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append(title);
			TextBox textBox = new TextBox(font, stringBuilder.ToString(), pdfUtil.PDFpageeffwidth, 20f);
			textBox.SetLocation(pdfUtil.PDFleft_margin, pdfUtil.PDFtop_margin + offsetY);
			textBox.SetVerticalAlignment(0);
			textBox.SetTextAlignment(txtAlign);
			textBox.SetBorder(65536, false);
			textBox.SetBorder(131072, false);
			textBox.SetBorder(262144, false);
			textBox.SetBorder(524288, false);
			textBox.DrawOn(page);
		}
		public static void DrawPageNumber(PDF pdf, Page page, int pageNumber)
		{
			PDFjet.NET.Font font = new PDFjet.NET.Font(pdf, CoreFont.HELVETICA_BOLD);
			font.SetSize(7f);
			TextLine textLine = new TextLine(font);
			string text = string.Format(LangRes.PageNo, pageNumber);
			textLine.SetText(text);
			textLine.SetLocation(pdfUtil.PDFLoctx_pageNo, pdfUtil.PDFLocty_pageNo);
			textLine.DrawOn(page);
		}
		public static Page DrawTable(PDF pdf, Page page, ref int pageNumber, System.Collections.Generic.List<System.Collections.Generic.List<Cell>> tableData, int headrownum, float offsetY, System.Collections.Generic.List<float> arrycolWidth)
		{
			Table table = new Table();
			table.SetData(tableData, headrownum);
			table.SetCellBordersWidth(0.2f);
			table.SetLocation(pdfUtil.PDFleft_margin, pdfUtil.PDFtop_margin + offsetY);
			for (int i = 0; i < arrycolWidth.Count; i++)
			{
				table.SetColumnWidth(i, (double)arrycolWidth[i]);
			}
			table.WrapAroundCellText();
			table.GetNumberOfPages(page);
			while (true)
			{
				table.DrawOn(page);
				if (!table.HasMoreData())
				{
					break;
				}
				pdfUtil.DrawPageNumber(pdf, page, pageNumber++);
				page = new Page(pdf, Letter.PORTRAIT);
				table.SetLocation(pdfUtil.PDFleft_margin, pdfUtil.PDFtop_margin);
			}
			return page;
		}
	}
}
