using EventLogAPI._Lang;
using System;
namespace EventLogAPI
{
	public static class LogKey
	{
		public const string AllLogKeys = "0130000;0130001;0130010;0130011;0130020;0130021;0130022;0130023;0130030;0120062;0120063;0110070;0110071;0130080;0130081;0110090;0110100;0132010;0132011;0230000;0220001;0230002;0230003;0330000;0330001;0330002;0430000;0430001;0430010;0430011;0430012;0430020;0430021;0430022;0430030;0430031;0430032;0432000;0432010;0432011;0432012;0432013;0530000;0530001;0530002;0530020;0630000;0630001;0630005;0630010;0630011;0630015;0630016;0630020;0630021;0630030;0630031;0630050;0630051;0630052;0630053;0630054;0630055;0730010";
		public const string L0130000 = "0130000";
		public const string L0130001 = "0130001";
		public const string L0130010 = "0130010";
		public const string L0130011 = "0130011";
		public const string L0130020 = "0130020";
		public const string L0130021 = "0130021";
		public const string L0130022 = "0130022";
		public const string L0130023 = "0130023";
		public const string L0130030 = "0130030";
		public const string L0120060 = "0120060";
		public const string L0120061 = "0120061";
		public const string L0120062 = "0120062";
		public const string L0120063 = "0120063";
		public const string L0110070 = "0110070";
		public const string L0110071 = "0110071";
		public const string L0130080 = "0130080";
		public const string L0130081 = "0130081";
		public const string L0110090 = "0110090";
		public const string L0110100 = "0110100";
		public const string L0132010 = "0132010";
		public const string L0132011 = "0132011";
		public const string L0230000 = "0230000";
		public const string L0220001 = "0220001";
		public const string L0230002 = "0230002";
		public const string L0230003 = "0230003";
		public const string L0330000 = "0330000";
		public const string L0330001 = "0330001";
		public const string L0330002 = "0330002";
		public const string L0430000 = "0430000";
		public const string L0430001 = "0430001";
		public const string L0430010 = "0430010";
		public const string L0430011 = "0430011";
		public const string L0430012 = "0430012";
		public const string L0430020 = "0430020";
		public const string L0430021 = "0430021";
		public const string L0430022 = "0430022";
		public const string L0430030 = "0430030";
		public const string L0430031 = "0430031";
		public const string L0430032 = "0430032";
		public const string L0432000 = "0432000";
		public const string L0432010 = "0432010";
		public const string L0432011 = "0432011";
		public const string L0432012 = "0432012";
		public const string L0432013 = "0432013";
		public const string L0530000 = "0530000";
		public const string L0530001 = "0530001";
		public const string L0530002 = "0530002";
		public const string L0530020 = "0530020";
		public const string L0630000 = "0630000";
		public const string L0630001 = "0630001";
		public const string L0630005 = "0630005";
		public const string L0630010 = "0630010";
		public const string L0630011 = "0630011";
		public const string L0630015 = "0630015";
		public const string L0630016 = "0630016";
		public const string L0630020 = "0630020";
		public const string L0630021 = "0630021";
		public const string L0630030 = "0630030";
		public const string L0630031 = "0630031";
		public const string L0630050 = "0630050";
		public const string L0630051 = "0630051";
		public const string L0630052 = "0630052";
		public const string L0630053 = "0630053";
		public const string L0630054 = "0630054";
		public const string L0630055 = "0630055";
		public const string L0730000 = "0730000";
		public const string L0720001 = "0720001";
		public const string L0710002 = "0710002";
		public const string L0730010 = "0730010";
		public static string strCategory(string strLogKey)
		{
			int num = Convert.ToInt32(strLogKey);
			int num2 = num / 100000;
			string result = "";
			switch (num2)
			{
			case 1:
				result = LangRes.Cat1System;
				break;
			case 2:
				result = LangRes.Cat2Authentication;
				break;
			case 3:
				result = LangRes.Cat3Usermanagement;
				break;
			case 4:
				result = LangRes.Cat4Devicemanagement;
				break;
			case 5:
				result = LangRes.Cat5Systemtask;
				break;
			case 6:
				result = LangRes.Cat6Device;
				break;
			case 7:
				result = LangRes.Cat7DevTrap;
				break;
			}
			return result;
		}
		public static string strSeverity(string strLogKey)
		{
			int num = Convert.ToInt32(strLogKey);
			int num2 = num / 10000 % 10;
			string result = "";
			switch (num2)
			{
			case 1:
				result = LangRes.Sev1Critical;
				break;
			case 2:
				result = LangRes.Sev2Warning;
				break;
			case 3:
				result = LangRes.Sev3Information;
				break;
			}
			return result;
		}
		public static string strEvent(string strLogKey)
		{
			return EventLogLang.getMsg("SL" + strLogKey, new string[0]);
		}
		public static bool isISGEvent(string strLogKey)
		{
			int num = Convert.ToInt32(strLogKey);
			int num2 = num % 10000 / 1000;
			return num2 == 2;
		}
	}
}
