using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
namespace EcoSensors._Lang
{
	internal class EcoLanguage
	{
		public static string[] strLang = new string[]
		{
			"English",
			"Deutsch",
			"Español",
			"Français",
			"Italiano",
			"日本語",
			"한국어",
			"Português",
			"Русский",
			"中文(简体)",
			"中文(繁體)"
		};
		private static int _Language = 0;
		private static System.Globalization.CultureInfo _SelectedCulture = null;
		private static System.Reflection.Assembly ass = null;
		public EcoLanguage()
		{
			EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("en");
			System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
			EcoLanguage.ass = System.Reflection.Assembly.GetExecutingAssembly();
		}
		public static int getLang()
		{
			return EcoLanguage._Language;
		}
		public static void ChangeLang(int Language)
		{
			EcoLanguage._Language = Language;
			switch (Language)
			{
			case 0:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 1:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("de");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 2:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("es");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 3:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("fr");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 4:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("it");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 5:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("ja");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 6:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("ko");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 7:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("pt");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 8:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 9:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHS");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			case 10:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHT");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				return;
			default:
				EcoLanguage._SelectedCulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				System.Threading.Thread.CurrentThread.CurrentUICulture = EcoLanguage._SelectedCulture;
				EcoLanguage._Language = 0;
				return;
			}
		}
		public static string getMsg(string skey, params string[] argv)
		{
			return string.Format(skey, argv);
		}
		public static string getMsginThread(string skey, params string[] argv)
		{
			if (EcoLanguage.ass == null)
			{
				EcoLanguage.ass = System.Reflection.Assembly.GetExecutingAssembly();
			}
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("EcoSensors._Lang.LangRes", EcoLanguage.ass);
			string @string = resourceManager.GetString(skey, EcoLanguage._SelectedCulture);
			return string.Format(@string, argv);
		}
	}
}
