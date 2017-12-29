using System;
using System.Text;
namespace PDFjet.NET
{
	public class Bidi
	{
		public static string ReorderVisually(string str)
		{
			StringBuilder stringBuilder = new StringBuilder(Bidi.Reverse(str));
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int i = 0; i < stringBuilder.Length; i++)
			{
				int num = (int)stringBuilder[i];
				if (num >= 1425 && num <= 1524)
				{
					if (stringBuilder2.Length != 0)
					{
						stringBuilder3.Append(Bidi.Reverse(stringBuilder2.ToString()));
						stringBuilder2 = new StringBuilder();
					}
					stringBuilder3.Append((char)num);
				}
				else
				{
					stringBuilder2.Append((char)num);
				}
			}
			stringBuilder3.Append(Bidi.Reverse(stringBuilder2.ToString()));
			return stringBuilder3.ToString();
		}
		private static string Reverse(string s)
		{
			char[] array = s.ToCharArray();
			Array.Reverse(array);
			return new string(array);
		}
	}
}
