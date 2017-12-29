using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	public class PDFobj
	{
		public static string TYPE = "/Type";
		public static string SUBTYPE = "/Subtype";
		public static string FILTER = "/Filter";
		public static string WIDTH = "/Width";
		public static string HEIGHT = "/Height";
		public static string COLORSPACE = "/ColorSpace";
		public static string BITSPERCOMPONENT = "/BitsPerComponent";
		internal int offset;
		internal int number;
		internal List<string> dict;
		internal int stream_offset;
		internal byte[] stream;
		internal byte[] data;
		public PDFobj(int offset)
		{
			this.offset = offset;
			this.dict = new List<string>();
		}
		public void SetStream(byte[] pdf, int length)
		{
			this.stream = new byte[length];
			Array.Copy(pdf, this.stream_offset, this.stream, 0, length);
		}
		public byte[] GetData()
		{
			return this.data;
		}
		public string GetValue(string key)
		{
			for (int i = 0; i < this.dict.Count; i++)
			{
				string text = this.dict[i];
				if (text.Equals(key))
				{
					return this.dict[i + 1];
				}
			}
			return "";
		}
		public float[] GetPageSize()
		{
			for (int i = 0; i < this.dict.Count; i++)
			{
				if (this.dict[i].Equals("/MediaBox"))
				{
					return new float[]
					{
						Convert.ToSingle(this.dict[i + 4]),
						Convert.ToSingle(this.dict[i + 5])
					};
				}
			}
			return Letter.PORTRAIT;
		}
		internal int GetLength(List<PDFobj> objects)
		{
			int i = 0;
			while (i < this.dict.Count)
			{
				string text = this.dict[i];
				if (text.Equals("/Length"))
				{
					int result = int.Parse(this.dict[i + 1]);
					if (this.dict[i + 2].Equals("0") && this.dict[i + 3].Equals("R"))
					{
						return this.GetLength(objects, result);
					}
					return result;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}
		internal int GetLength(List<PDFobj> objects, int number)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				PDFobj pDFobj = objects[i];
				int num = int.Parse(pDFobj.dict[0]);
				if (num == number)
				{
					return int.Parse(pDFobj.dict[3]);
				}
			}
			return 0;
		}
	}
}
