using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
namespace CommonAPI
{
	public class CommonAPI
	{
		public delegate void DelegateOnConnected(Socket sock);
		public delegate void DelegateOnClosed(int reason);
		public delegate void DelegateOnBroadcast(int infoType, object info, object carried);
		public static string ReportException(int level, Exception e, bool bWithLineFlag = true, string prefix = "    ")
		{
			string fullName = e.GetType().FullName;
			string key;
			switch (key = fullName)
			{
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\r\n");
			stringBuilder.Append(fullName + ": " + e.Message + "\r\n");
			string text = prefix;
			StackTrace stackTrace = new StackTrace(true);
			for (int i = stackTrace.FrameCount - 1; i >= 0; i--)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				if (CommonAPI.IsMethodToBeIncluded(frame))
				{
					stringBuilder.Append(text);
					if (i != stackTrace.FrameCount - 1)
					{
						stringBuilder.Append("> ");
					}
					stringBuilder.AppendLine(CommonAPI.MethodCallLog(frame));
					text += "-";
				}
			}
			if (!bWithLineFlag)
			{
				stringBuilder.Replace("\r\n", "");
			}
			return stringBuilder.ToString();
		}
		private static bool IsMethodToBeIncluded(StackFrame p_StackMethod)
		{
			MethodBase method = p_StackMethod.GetMethod();
			return !(method.DeclaringType == typeof(CommonAPI));
		}
		private static string MethodCallLog(StackFrame p_MethodCall)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MethodBase method = p_MethodCall.GetMethod();
			stringBuilder.Append(method.DeclaringType.ToString());
			stringBuilder.Append(".");
			stringBuilder.Append(p_MethodCall.GetMethod().Name);
			ParameterInfo[] parameters = method.GetParameters();
			stringBuilder.Append("(");
			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				ParameterInfo parameterInfo = parameters[i];
				stringBuilder.Append(parameterInfo.ParameterType.Name);
				stringBuilder.Append(" ");
				stringBuilder.Append(parameterInfo.Name);
			}
			stringBuilder.Append(")");
			string fileName = p_MethodCall.GetFileName();
			if (!string.IsNullOrEmpty(fileName))
			{
				stringBuilder.Append(" in ");
				stringBuilder.Append(fileName);
				stringBuilder.Append(": line ");
				stringBuilder.Append(p_MethodCall.GetFileLineNumber().ToString());
			}
			return stringBuilder.ToString();
		}
		public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception e2 = (Exception)e.ExceptionObject;
			CommonAPI.ReportException(0, e2, true, "    ");
		}
		public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Exception exception = e.Exception;
			CommonAPI.ReportException(0, exception, true, "    ");
		}
	}
}
