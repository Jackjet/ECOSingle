using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
namespace EventLogAPI._Lang
{
	internal class EventLogLang
	{
		private static Assembly ass;
		private static ResourceManager m_LocRM;
		public static string getMsg(string skey, params string[] argv)
		{
			if (EventLogLang.ass == null)
			{
				EventLogLang.ass = Assembly.GetExecutingAssembly();
			}
			if (EventLogLang.m_LocRM == null)
			{
				EventLogLang.m_LocRM = new ResourceManager("EventLogAPI._Lang.LangRes", EventLogLang.ass);
			}
			string text = EventLogLang.m_LocRM.GetString(skey);
			if (text == null || text == "")
			{
				text = skey;
			}
			if (text.IndexOf("}") < 1)
			{
				text = string.Format(text, argv);
			}
			else
			{
				int num = Regex.Matches(text, "(?<!\\{)\\{([0-9]+).*?\\}(?!})").Cast<Match>().Max((Match m) => int.Parse(m.Groups[1].Value)) + 1;
				if (num > argv.Length)
				{
					string[] array = new string[num];
					for (int i = 0; i < num; i++)
					{
						if (i < argv.Length)
						{
							array[i] = argv[i];
						}
						else
						{
							array[i] = "N/A";
						}
					}
					text = string.Format(text, array);
				}
				else
				{
					if (num < argv.Length)
					{
						string[] array2 = new string[num];
						for (int j = 0; j < num; j++)
						{
							array2[j] = argv[j];
						}
						text = string.Format(text, array2);
					}
					else
					{
						if (num == argv.Length)
						{
							text = string.Format(text, argv);
						}
					}
				}
			}
			return text;
		}
	}
}
