using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace PDFjet.NET
{
	public class PDF
	{
		internal int objNumber;
		internal int metadataObjNumber;
		internal int outputIntentObjNumber;
		internal List<Font> fonts = new List<Font>();
		internal List<Image> images = new List<Image>();
		internal List<Page> pages = new List<Page>();
		internal Dictionary<string, Destination> destinations = new Dictionary<string, Destination>();
		private static int CR_LF = 0;
		private static int CR = 1;
		private static int LF = 2;
		private int compliance;
		private Stream os;
		private List<int> objOffset = new List<int>();
		private List<int> contents = new List<int>();
		private string producer = "PDFjet v4.92 (http://pdfjet.com)";
		private string creationDate;
		private string createDate;
		private string title = "";
		private string subject = "";
		private string author = "";
		private int byte_count;
		private int endOfLine = PDF.CR_LF;
		internal List<OptionalContentGroup> groups = new List<OptionalContentGroup>();
		public PDF()
		{
		}
		public PDF(Stream os) : this(os, 0)
		{
		}
		public PDF(Stream os, int compliance)
		{
			this.os = os;
			this.compliance = compliance;
			DateTime now = new DateTime(DateTime.Now.Ticks);
			SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyyMMddHHmmss'Z'");
			SimpleDateFormat simpleDateFormat2 = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
			this.creationDate = simpleDateFormat.Format(now);
			this.createDate = simpleDateFormat2.Format(now);
			this.Append("%PDF-1.4\n");
			this.Append('%');
			this.Append(242);
			this.Append(243);
			this.Append(244);
			this.Append(245);
			this.Append(246);
			this.Append('\n');
			if (compliance == Compliance.PDF_A_1B)
			{
				this.metadataObjNumber = this.AddMetadataObject("", true);
				this.outputIntentObjNumber = this.AddOutputIntentObject();
			}
		}
		internal void Newobj()
		{
			this.objOffset.Add(this.byte_count);
			this.Append(++this.objNumber);
			this.Append(" 0 obj\n");
		}
		internal void Endobj()
		{
			this.Append("endobj\n");
		}
		internal int AddMetadataObject(string notice, bool pad)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xpacket begin='﻿' id=\"W5M0MpCehiHzreSzNTczkc9d\"?>\n");
			stringBuilder.Append("<x:xmpmeta xmlns:x=\"adobe:ns:meta/\">\n");
			stringBuilder.Append("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">\n");
			stringBuilder.Append("<rdf:Description rdf:about=\"\" xmlns:pdf=\"http://ns.adobe.com/pdf/1.3/\" pdf:Producer=\"");
			stringBuilder.Append(this.producer);
			stringBuilder.Append("\"></rdf:Description>\n");
			stringBuilder.Append("<rdf:Description rdf:about=\"\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\">\n");
			stringBuilder.Append("<dc:format>application/pdf</dc:format>\n");
			stringBuilder.Append("<dc:title><rdf:Alt><rdf:li xml:lang=\"x-default\">");
			stringBuilder.Append(this.title);
			stringBuilder.Append("</rdf:li></rdf:Alt></dc:title>\n");
			stringBuilder.Append("<dc:creator><rdf:Seq><rdf:li>");
			stringBuilder.Append(this.author);
			stringBuilder.Append("</rdf:li></rdf:Seq></dc:creator>\n");
			stringBuilder.Append("<dc:description><rdf:Alt><rdf:li xml:lang=\"en-US\">");
			stringBuilder.Append(notice);
			stringBuilder.Append("</rdf:li></rdf:Alt></dc:description>\n");
			stringBuilder.Append("</rdf:Description>\n");
			stringBuilder.Append("<rdf:Description rdf:about=\"\" xmlns:pdfaid=\"http://www.aiim.org/pdfa/ns/id/\">");
			stringBuilder.Append("<pdfaid:part>1</pdfaid:part>");
			stringBuilder.Append("<pdfaid:conformance>B</pdfaid:conformance>");
			stringBuilder.Append("</rdf:Description>");
			stringBuilder.Append("<rdf:Description rdf:about=\"\" xmlns:xmp=\"http://ns.adobe.com/xap/1.0/\">\n");
			stringBuilder.Append("<xmp:CreateDate>");
			stringBuilder.Append(this.createDate);
			stringBuilder.Append("</xmp:CreateDate>\n");
			stringBuilder.Append("</rdf:Description>\n");
			stringBuilder.Append("</rdf:RDF>\n");
			stringBuilder.Append("</x:xmpmeta>\n");
			if (pad)
			{
				for (int i = 0; i < 20; i++)
				{
					for (int j = 0; j < 10; j++)
					{
						stringBuilder.Append("          ");
					}
					stringBuilder.Append("\n");
				}
			}
			stringBuilder.Append("<?xpacket end=\"w\"?>");
			byte[] bytes = new UTF8Encoding().GetBytes(stringBuilder.ToString());
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Type /Metadata\n");
			this.Append("/Subtype /XML\n");
			this.Append("/Length ");
			this.Append(bytes.Length);
			this.Append("\n");
			this.Append(">>\n");
			this.Append("stream\n");
			for (int k = 0; k < bytes.Length; k++)
			{
				this.Append(bytes[k]);
			}
			this.Append("\nendstream\n");
			this.Endobj();
			return this.objNumber;
		}
		private int AddOutputIntentObject()
		{
			this.Newobj();
			this.Append("<<\n");
			this.Append("/N 3\n");
			this.Append("/Length ");
			this.Append(ICCBlackScaled.profile.Length);
			this.Append("\n");
			this.Append("/Filter /FlateDecode\n");
			this.Append(">>\n");
			this.Append("stream\n");
			this.Append(ICCBlackScaled.profile, 0, ICCBlackScaled.profile.Length);
			this.Append("\nendstream\n");
			this.Endobj();
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Type /OutputIntent\n");
			this.Append("/S /GTS_PDFA1\n");
			this.Append("/OutputCondition (sRGB IEC61966-2.1)\n");
			this.Append("/OutputConditionIdentifier (sRGB IEC61966-2.1)\n");
			this.Append("/Info (sRGB IEC61966-2.1)\n");
			this.Append("/DestOutputProfile ");
			this.Append(this.objNumber - 1);
			this.Append(" 0 R\n");
			this.Append(">>\n");
			this.Endobj();
			return this.objNumber;
		}
		private int AddResourcesObject()
		{
			this.Newobj();
			this.Append("<<\n");
			if (this.fonts.Count > 0)
			{
				this.Append("/Font\n");
				this.Append("<<\n");
				for (int i = 0; i < this.fonts.Count; i++)
				{
					Font font = this.fonts[i];
					this.Append("/F");
					this.Append(font.objNumber);
					this.Append(' ');
					this.Append(font.objNumber);
					this.Append(" 0 R\n");
				}
				this.Append(">>\n");
			}
			if (this.images.Count > 0)
			{
				this.Append("/XObject\n");
				this.Append("<<\n");
				for (int j = 0; j < this.images.Count; j++)
				{
					Image image = this.images[j];
					this.Append("/Im");
					this.Append(image.objNumber);
					this.Append(' ');
					this.Append(image.objNumber);
					this.Append(" 0 R\n");
				}
				this.Append(">>\n");
			}
			if (this.groups.Count > 0)
			{
				this.Append("/Properties\n");
				this.Append("<<\n");
				for (int k = 0; k < this.groups.Count; k++)
				{
					OptionalContentGroup optionalContentGroup = this.groups[k];
					this.Append("/OC");
					this.Append(k + 1);
					this.Append(' ');
					this.Append(optionalContentGroup.objNumber);
					this.Append(" 0 R\n");
				}
				this.Append(">>\n");
			}
			this.Append(">>\n");
			this.Endobj();
			return this.objNumber;
		}
		private int AddPagesObject()
		{
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Type /Pages\n");
			this.Append("/Kids [ ");
			int num = this.objNumber + 1;
			for (int i = 0; i < this.pages.Count; i++)
			{
				Page page = this.pages[i];
				page.SetDestinationsPageObjNumber(num);
				this.Append(num);
				this.Append(" 0 R ");
				num += page.annots.Count + 1;
			}
			this.Append("]\n");
			this.Append("/Count ");
			this.Append(this.pages.Count);
			this.Append('\n');
			this.Append(">>\n");
			this.Endobj();
			return this.objNumber;
		}
		private int AddInfoObject()
		{
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Title (");
			this.Append(this.title);
			this.Append(")\n");
			this.Append("/Subject (");
			this.Append(this.subject);
			this.Append(")\n");
			this.Append("/Author (");
			this.Append(this.author);
			this.Append(")\n");
			this.Append("/Producer (");
			this.Append(this.producer);
			this.Append(")\n");
			if (this.compliance != Compliance.PDF_A_1B)
			{
				this.Append("/CreationDate (D:");
				this.Append(this.creationDate);
				this.Append(")\n");
			}
			this.Append(">>\n");
			this.Endobj();
			return this.objNumber;
		}
		private void AddPageBox(string boxName, Page page, float[] rect)
		{
			this.Append("/");
			this.Append(boxName);
			this.Append(" [");
			this.Append((double)rect[0]);
			this.Append(' ');
			this.Append((double)(page.height - rect[3]));
			this.Append(' ');
			this.Append((double)rect[2]);
			this.Append(' ');
			this.Append((double)(page.height - rect[1]));
			this.Append("]\n");
		}
		private void AddAllPages(int pagesObjNumber, int resObjNumber)
		{
			for (int i = 0; i < this.pages.Count; i++)
			{
				Page page = this.pages[i];
				this.Newobj();
				this.Append("<<\n");
				this.Append("/Type /Page\n");
				this.Append("/Parent ");
				this.Append(pagesObjNumber);
				this.Append(" 0 R\n");
				this.Append("/MediaBox [0.0 0.0 ");
				this.Append((double)page.width);
				this.Append(' ');
				this.Append((double)page.height);
				this.Append("]\n");
				if (page.cropBox != null)
				{
					this.AddPageBox("CropBox", page, page.cropBox);
				}
				if (page.bleedBox != null)
				{
					this.AddPageBox("BleedBox", page, page.bleedBox);
				}
				if (page.trimBox != null)
				{
					this.AddPageBox("TrimBox", page, page.trimBox);
				}
				if (page.artBox != null)
				{
					this.AddPageBox("ArtBox", page, page.artBox);
				}
				this.Append("/Resources ");
				this.Append(resObjNumber);
				this.Append(" 0 R\n");
				this.Append("/Contents ");
				this.Append(this.contents[i]);
				this.Append(" 0 R\n");
				if (page.annots.Count > 0)
				{
					this.Append("/Annots [ ");
					for (int j = 0; j < page.annots.Count; j++)
					{
						this.Append(this.objNumber + j + 1);
						this.Append(" 0 R ");
					}
					this.Append("]\n");
				}
				this.Append(">>\n");
				this.Endobj();
				this.AddAnnotDictionaries(page);
			}
		}
		internal void AddPageContent(Page page)
		{
			MemoryStream memoryStream = new MemoryStream();
			DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream);
			byte[] array = page.buf.ToArray();
			deflaterOutputStream.Write(array, 0, array.Length);
			deflaterOutputStream.Finish();
			page.buf = null;
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Filter /FlateDecode\n");
			this.Append("/Length ");
			this.Append((double)memoryStream.Length);
			this.Append("\n");
			this.Append(">>\n");
			this.Append("stream\n");
			this.Append(memoryStream);
			this.Append("\nendstream\n");
			this.Endobj();
			this.contents.Add(this.objNumber);
		}
		private void AddAnnotDictionaries(Page page)
		{
			for (int i = 0; i < page.annots.Count; i++)
			{
				Annotation annotation = page.annots[i];
				this.Newobj();
				this.Append("<<\n");
				this.Append("/Type /Annot\n");
				this.Append("/Subtype /Link\n");
				this.Append("/Rect [");
				this.Append(annotation.x1);
				this.Append(' ');
				this.Append(annotation.y1);
				this.Append(' ');
				this.Append(annotation.x2);
				this.Append(' ');
				this.Append(annotation.y2);
				this.Append("]\n");
				this.Append("/Border[0 0 0]\n");
				if (annotation.uri != null)
				{
					this.Append("/F 4\n");
					this.Append("/A <<\n");
					this.Append("/S /URI\n");
					this.Append("/URI (");
					this.Append(annotation.uri);
					this.Append(")\n");
					this.Append(">>\n");
				}
				else
				{
					if (annotation.key != null)
					{
						Destination destination = this.destinations[annotation.key];
						if (destination != null)
						{
							this.Append("/Dest [");
							this.Append(destination.pageObjNumber);
							this.Append(" 0 R /XYZ 0 ");
							this.Append((double)destination.yPosition);
							this.Append(" 0]\n");
						}
					}
				}
				this.Append(">>\n");
				this.Endobj();
			}
		}
		private void AddOCProperties()
		{
			if (this.groups.Count > 0)
			{
				this.Append("/OCProperties\n");
				this.Append("<<\n");
				this.Append("/OCGs [");
				foreach (OptionalContentGroup current in this.groups)
				{
					this.Append(' ');
					this.Append(current.objNumber);
					this.Append(" 0 R");
				}
				this.Append(" ]\n");
				this.Append("/D <<\n");
				this.Append("/BaseState /OFF\n");
				this.Append("/ON [");
				foreach (OptionalContentGroup current2 in this.groups)
				{
					if (current2.visible)
					{
						this.Append(' ');
						this.Append(current2.objNumber);
						this.Append(" 0 R");
					}
				}
				this.Append(" ]\n");
				this.Append("/AS [\n");
				this.Append("<< /Event /Print /Category [/Print] /OCGs [");
				foreach (OptionalContentGroup current3 in this.groups)
				{
					if (current3.printable)
					{
						this.Append(' ');
						this.Append(current3.objNumber);
						this.Append(" 0 R");
					}
				}
				this.Append(" ] >>\n");
				this.Append("<< /Event /Export /Category [/Export] /OCGs [");
				foreach (OptionalContentGroup current4 in this.groups)
				{
					if (current4.exportable)
					{
						this.Append(' ');
						this.Append(current4.objNumber);
						this.Append(" 0 R");
					}
				}
				this.Append(" ] >>\n");
				this.Append("]\n");
				this.Append("/Order [[ ()");
				foreach (OptionalContentGroup current5 in this.groups)
				{
					this.Append(' ');
					this.Append(current5.objNumber);
					this.Append(" 0 R");
				}
				this.Append(" ]]\n");
				this.Append(">>\n");
				this.Append(">>\n");
			}
		}
		public void AddPage(Page page)
		{
			int count = this.pages.Count;
			if (count > 0)
			{
				this.AddPageContent(this.pages[count - 1]);
			}
			this.pages.Add(page);
		}
		public void Flush()
		{
			this.Flush(false);
		}
		public void Close()
		{
			this.Flush(true);
		}
		private void Flush(bool close)
		{
			this.AddPageContent(this.pages[this.pages.Count - 1]);
			int resObjNumber = this.AddResourcesObject();
			int num = this.AddInfoObject();
			int num2 = this.AddPagesObject();
			this.AddAllPages(num2, resObjNumber);
			this.Newobj();
			this.Append("<<\n");
			this.Append("/Type /Catalog\n");
			this.AddOCProperties();
			this.Append("/Pages ");
			this.Append(num2);
			this.Append(" 0 R\n");
			if (this.compliance == Compliance.PDF_A_1B)
			{
				this.Append("/Metadata ");
				this.Append(this.metadataObjNumber);
				this.Append(" 0 R\n");
				this.Append("/OutputIntents [");
				this.Append(this.outputIntentObjNumber);
				this.Append(" 0 R]\n");
			}
			this.Append(">>\n");
			this.Endobj();
			int num3 = this.byte_count;
			this.Append("xref\n");
			this.Append("0 ");
			this.Append(this.objNumber + 1);
			this.Append('\n');
			this.Append("0000000000 65535 f \n");
			for (int i = 0; i < this.objOffset.Count; i++)
			{
				string text = this.objOffset[i].ToString();
				for (int j = 0; j < 10 - text.Length; j++)
				{
					this.Append('0');
				}
				this.Append(text);
				this.Append(" 00000 n \n");
			}
			this.Append("trailer\n");
			this.Append("<<\n");
			this.Append("/Size ");
			this.Append(this.objNumber + 1);
			this.Append('\n');
			string iD = new Salsa20().getID();
			this.Append("/ID[<");
			this.Append(iD);
			this.Append("><");
			this.Append(iD);
			this.Append(">]\n");
			this.Append("/Root ");
			this.Append(this.objNumber);
			this.Append(" 0 R\n");
			this.Append("/Info ");
			this.Append(num);
			this.Append(" 0 R\n");
			this.Append(">>\n");
			this.Append("startxref\n");
			this.Append(num3);
			this.Append('\n');
			this.Append("%%EOF\n");
			this.os.Flush();
			if (close)
			{
				this.os.Dispose();
			}
		}
		public void SetTitle(string title)
		{
			this.title = title;
		}
		public void SetSubject(string subject)
		{
			this.subject = subject;
		}
		public void SetAuthor(string author)
		{
			this.author = author;
		}
		internal void Append(int num)
		{
			this.Append(num.ToString());
		}
		internal void Append(double val)
		{
			this.Append(val.ToString().Replace(',', '.'));
		}
		internal void Append(string str)
		{
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				this.os.WriteByte((byte)str[i]);
			}
			this.byte_count += length;
		}
		internal void Append(char ch)
		{
			this.Append((byte)ch);
		}
		internal void Append(byte b)
		{
			this.os.WriteByte(b);
			this.byte_count++;
		}
		internal void Append(byte[] buf, int off, int len)
		{
			this.os.Write(buf, off, len);
			this.byte_count += len;
		}
		internal void Append(MemoryStream baos)
		{
			baos.WriteTo(this.os);
			this.byte_count += (int)baos.Length;
		}
		public Dictionary<int, PDFobj> Read(Stream inputStream)
		{
			List<PDFobj> list = new List<PDFobj>();
			MemoryStream memoryStream = new MemoryStream();
			int num;
			while ((num = inputStream.ReadByte()) != -1)
			{
				memoryStream.WriteByte((byte)num);
			}
			byte[] array = memoryStream.ToArray();
			int startXRef = this.GetStartXRef(array);
			PDFobj pDFobj = this.GetObject(array, startXRef);
			if (pDFobj.dict[0].Equals("xref"))
			{
				this.GetPdfObjects1(array, pDFobj, list);
			}
			else
			{
				this.GetPdfObjects2(array, pDFobj, list);
			}
			Dictionary<int, PDFobj> dictionary = new Dictionary<int, PDFobj>();
			for (int i = 0; i < list.Count; i++)
			{
				pDFobj = list[i];
				pDFobj.number = int.Parse(pDFobj.dict[0]);
				if (pDFobj.dict.Contains("stream"))
				{
					pDFobj.SetStream(array, pDFobj.GetLength(list));
				}
				if (pDFobj.GetValue("/Filter").Equals("/FlateDecode"))
				{
					Decompressor decompressor = new Decompressor(pDFobj.stream);
					pDFobj.data = decompressor.getDecompressedData();
				}
				if (pDFobj.GetValue("/Type").Equals("/ObjStm"))
				{
					int num2 = int.Parse(pDFobj.GetValue("/First"));
					int.Parse(pDFobj.GetValue("/N"));
					PDFobj @object = this.GetObject(pDFobj.data, 0, num2);
					for (int j = 0; j < @object.dict.Count; j += 2)
					{
						int key = int.Parse(@object.dict[j]);
						int num3 = int.Parse(@object.dict[j + 1]);
						int len = pDFobj.data.Length;
						if (j <= @object.dict.Count - 4)
						{
							len = num2 + int.Parse(@object.dict[j + 3]);
						}
						PDFobj object2 = this.GetObject(pDFobj.data, num2 + num3, len);
						object2.dict.Insert(0, "obj");
						object2.dict.Insert(0, "0");
						object2.dict.Insert(0, key.ToString());
						dictionary[key] = object2;
					}
				}
				else
				{
					dictionary[pDFobj.number] = pDFobj;
				}
			}
			return dictionary;
		}
		private bool Process(PDFobj obj, StringBuilder buf, int off)
		{
			string text = buf.ToString().Trim();
			if (!text.Equals(""))
			{
				obj.dict.Add(text);
			}
			buf.Length = 0;
			if (text.Equals("stream") || text.Equals("endobj") || text.Equals("startxref"))
			{
				if (text.Equals("stream"))
				{
					if (this.endOfLine == PDF.CR_LF)
					{
						obj.stream_offset = off + 1;
					}
					else
					{
						if (this.endOfLine == PDF.CR || this.endOfLine == PDF.LF)
						{
							obj.stream_offset = off;
						}
					}
				}
				return true;
			}
			return false;
		}
		private PDFobj GetObject(byte[] buf, int off)
		{
			return this.GetObject(buf, off, buf.Length);
		}
		private PDFobj GetObject(byte[] buf, int off, int len)
		{
			PDFobj pDFobj = new PDFobj(off);
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			char c = ' ';
			bool flag = false;
			while (!flag && off < len)
			{
				char c2 = (char)buf[off++];
				if (c2 == '(')
				{
					if (num == 0)
					{
						flag = this.Process(pDFobj, stringBuilder, off);
					}
					if (!flag)
					{
						stringBuilder.Append(c2);
						num++;
					}
				}
				else
				{
					if (c2 == ')')
					{
						stringBuilder.Append(c2);
						num--;
						if (num == 0)
						{
							flag = this.Process(pDFobj, stringBuilder, off);
						}
					}
					else
					{
						if (c2 == '\0' || c2 == '\t' || c2 == '\n' || c2 == '\f' || c2 == '\r' || c2 == ' ')
						{
							flag = this.Process(pDFobj, stringBuilder, off);
							if (!flag)
							{
								c = ' ';
							}
						}
						else
						{
							if (c2 == '/')
							{
								flag = this.Process(pDFobj, stringBuilder, off);
								if (!flag)
								{
									stringBuilder.Append(c2);
									c = c2;
								}
							}
							else
							{
								if (c2 == '<' || c2 == '>' || c2 == '%')
								{
									if (c2 != c)
									{
										flag = this.Process(pDFobj, stringBuilder, off);
										if (!flag)
										{
											stringBuilder.Append(c2);
											c = c2;
										}
									}
									else
									{
										stringBuilder.Append(c2);
										flag = this.Process(pDFobj, stringBuilder, off);
										if (!flag)
										{
											c = ' ';
										}
									}
								}
								else
								{
									if (c2 == '[' || c2 == ']' || c2 == '{' || c2 == '}')
									{
										flag = this.Process(pDFobj, stringBuilder, off);
										if (!flag)
										{
											pDFobj.dict.Add(c2.ToString());
											c = c2;
										}
									}
									else
									{
										stringBuilder.Append(c2);
										if (num == 0)
										{
											c = c2;
										}
									}
								}
							}
						}
					}
				}
			}
			return pDFobj;
		}
		private int ToInt(byte[] buf, int off, int len)
		{
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				num |= (int)(buf[off + i] & 255);
				if (i < len - 1)
				{
					num <<= 8;
				}
			}
			return num;
		}
		private void GetPdfObjects1(byte[] pdf, PDFobj obj, List<PDFobj> pdfObjects)
		{
			string value = obj.GetValue("/Prev");
			if (!value.Equals(""))
			{
				this.GetPdfObjects1(pdf, this.GetObject(pdf, int.Parse(value)), pdfObjects);
			}
			int num = 1;
			while (true)
			{
				string text = obj.dict[num++];
				if (text.Equals("trailer"))
				{
					break;
				}
				int num2 = int.Parse(obj.dict[num++]);
				for (int i = 0; i < num2; i++)
				{
					string s = obj.dict[num++];
					string arg_8C_0 = obj.dict[num++];
					string text2 = obj.dict[num++];
					if (!text2.Equals("f"))
					{
						int num3 = int.Parse(s);
						if (num3 != 0)
						{
							pdfObjects.Add(this.GetObject(pdf, num3));
						}
					}
				}
			}
		}
		private void GetPdfObjects2(byte[] pdf, PDFobj obj, List<PDFobj> pdfObjects)
		{
			string value = obj.GetValue("/Prev");
			if (!value.Equals(""))
			{
				this.GetPdfObjects2(pdf, this.GetObject(pdf, int.Parse(value)), pdfObjects);
			}
			obj.SetStream(pdf, int.Parse(obj.GetValue("/Length")));
			Decompressor decompressor = new Decompressor(obj.stream);
			byte[] decompressedData = decompressor.getDecompressedData();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < obj.dict.Count; i++)
			{
				string text = obj.dict[i];
				if (text.Equals("/Predictor") && obj.dict[i + 1].Equals("12"))
				{
					num = 1;
				}
				if (text.Equals("/W"))
				{
					num2 = int.Parse(obj.dict[i + 2]);
					num3 = int.Parse(obj.dict[i + 3]);
					num4 = int.Parse(obj.dict[i + 4]);
				}
			}
			int num5 = num + num2 + num3 + num4;
			byte[] array = new byte[num5];
			for (int j = 0; j < decompressedData.Length; j += num5)
			{
				for (int k = 0; k < num5; k++)
				{
					byte[] expr_135_cp_0 = array;
					int expr_135_cp_1 = k;
					expr_135_cp_0[expr_135_cp_1] += decompressedData[j + k];
				}
				if (array[1] == 1)
				{
					int off = this.ToInt(array, num + num2, num3);
					pdfObjects.Add(this.GetObject(pdf, off, pdf.Length));
				}
			}
		}
		private int GetStartXRef(byte[] buf)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = buf.Length - 10; i > 10; i--)
			{
				if (buf[i] == 115 && buf[i + 1] == 116 && buf[i + 2] == 97 && buf[i + 3] == 114 && buf[i + 4] == 116 && buf[i + 5] == 120 && buf[i + 6] == 114 && buf[i + 7] == 101 && buf[i + 8] == 102)
				{
					if (buf[i + 9] == 13)
					{
						if (buf[i + 10] == 10)
						{
							this.endOfLine = PDF.CR_LF;
						}
						else
						{
							this.endOfLine = PDF.CR;
						}
					}
					else
					{
						if (buf[i + 9] == 10)
						{
							this.endOfLine = PDF.LF;
						}
					}
					int num = (this.endOfLine == PDF.CR_LF) ? (i + 11) : (i + 10);
					char c = (char)buf[num];
					while (c == ' ' || char.IsDigit(c))
					{
						stringBuilder.Append(c);
						c = (char)buf[++num];
					}
					break;
				}
			}
			return int.Parse(stringBuilder.ToString().Trim());
		}
	}
}
