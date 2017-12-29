using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	public class SimpleDateFormat
	{
		private string format;
		public SimpleDateFormat(string format)
		{
			this.format = format;
		}
		public string Format(DateTime now)
		{
			string text = now.Year.ToString();
			if (this.format[4] == '-')
			{
				List<string> list = new List<string>();
				list.Add("-");
				list.Add(now.Month.ToString());
				list.Add("-");
				list.Add(now.Day.ToString());
				list.Add("T");
				list.Add(now.Hour.ToString());
				list.Add(":");
				list.Add(now.Minute.ToString());
				list.Add(":");
				list.Add(now.Second.ToString());
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = list[i];
					if (text2.Length == 1 && char.IsDigit(text2[0]))
					{
						text += "0";
					}
					text += text2;
				}
			}
			else
			{
				List<int> list2 = new List<int>();
				list2.Add(now.Month);
				list2.Add(now.Day);
				list2.Add(now.Hour);
				list2.Add(now.Minute);
				list2.Add(now.Second);
				for (int j = 0; j < list2.Count; j++)
				{
					string text3 = list2[j].ToString();
					if (text3.Length == 1)
					{
						text += "0";
					}
					text += text3;
				}
				text += "Z";
			}
			return text;
		}
	}
}
