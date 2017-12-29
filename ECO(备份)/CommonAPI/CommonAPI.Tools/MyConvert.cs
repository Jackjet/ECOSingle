using System;
using System.Globalization;
using System.Threading;
namespace CommonAPI.Tools
{
	public class MyConvert
	{
		private static CultureInfo cultureInfo = new CultureInfo("en-US");
		public static string ToString(object f_value)
		{
			return Convert.ToString(f_value, MyConvert.cultureInfo);
		}
		public static float ToSingle(object str)
		{
			return Convert.ToSingle(str, MyConvert.cultureInfo);
		}
		public static float ToSingle(string str)
		{
			return Convert.ToSingle(str, MyConvert.cultureInfo);
		}
		public static int ToInt32(object str)
		{
			return Convert.ToInt32(str, MyConvert.cultureInfo);
		}
		public static int ToInt32(string str)
		{
			return Convert.ToInt32(str, MyConvert.cultureInfo);
		}
		public static string ToString(float f_value)
		{
			return Convert.ToString(f_value, MyConvert.cultureInfo);
		}
		public static string ToString(int i_value)
		{
			return Convert.ToString(i_value, MyConvert.cultureInfo);
		}
		public static string ToString(float f_value, string style)
		{
			return f_value.ToString(style, MyConvert.cultureInfo);
		}
		public static string ToString(int i_value, string style)
		{
			return i_value.ToString(style, MyConvert.cultureInfo);
		}
		public static float floatParse(string szInput)
		{
			return float.Parse(szInput, MyConvert.cultureInfo);
		}
		public static double doubleParse(string szInput)
		{
			return double.Parse(szInput, MyConvert.cultureInfo);
		}
		public static void MyTest()
		{
			Console.Write("\r\n\r\nCurrentCulture - Before:\r\n");
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			Console.Write(currentCulture.Name);
			Console.Write(currentCulture.TwoLetterISOLanguageName);
			Console.Write(currentCulture.ThreeLetterISOLanguageName);
			Console.Write(currentCulture.ThreeLetterWindowsLanguageName);
			Console.Write(currentCulture.DisplayName);
			Console.Write(currentCulture.EnglishName);
			Console.Write("\r\n\r\nCurrentUICulture - Before:\r\n");
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			Console.Write(currentUICulture.Name);
			Console.Write(currentUICulture.TwoLetterISOLanguageName);
			Console.Write(currentUICulture.ThreeLetterISOLanguageName);
			Console.Write(currentUICulture.ThreeLetterWindowsLanguageName);
			Console.Write(currentUICulture.DisplayName);
			Console.Write(currentUICulture.EnglishName);
			Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-BE");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-BE");
			Convert.ToString(123.567886f);
			Convert.ToString(123.567886f, MyConvert.cultureInfo);
			123.567886f.ToString("F2");
			123.567886f.ToString("F4", new CultureInfo("fr-BE"));
			string text = 15.789f.ToString(CultureInfo.InvariantCulture);
			float.Parse(text, CultureInfo.InvariantCulture);
			string s = DateTime.Now.ToString(CultureInfo.InvariantCulture);
			DateTime.Parse(s, CultureInfo.InvariantCulture);
			Console.Write("InvariantCulture 15.798 -> " + text);
			string str = 15.789f.ToString();
			Console.Write("Default 15.798 -> " + str);
			float value = float.Parse("123,45");
			Convert.ToString(value);
			float value2 = float.Parse("123.45");
			Convert.ToString(value2);
			double.Parse("123.45");
			double.Parse("123,45");
			"123,45".Replace(",", ".");
			"123.45".Replace(",", ".");
			"123450".Replace(",", ".");
		}
	}
}
