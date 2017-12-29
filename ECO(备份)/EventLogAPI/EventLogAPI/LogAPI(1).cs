using CommonAPI.Email;
using CommonAPI.Global;
using CommonAPI.ThreadWrapper;
using DBAccessAPI;
using EventLogAPI._Lang;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
namespace EventLogAPI
{
	public class LogAPI
	{
		private static string s_myKeyWord;
		private static int i_myRangeType;
		private static int i_foundLogs;
		private static int i_matchedLogs;
		private static DateTime dt_myStart;
		private static DateTime dt_myEnd;
		private static string[][] s_matchedLogs;
		private static object _lockEmail = new object();
		private static MailMsg _mergedMail = null;
		private static DateTime _lastEmailMerge = DateTime.Now;
		private static ConcurrentQueue<MailMsg> _emailQueue = new ConcurrentQueue<MailMsg>();
		private static ManualResetEvent _emailEvent = new ManualResetEvent(false);
		private static ManualResetEvent _stopMailEvent = new ManualResetEvent(false);
		private static WaitHandle[] _waitEmailHandles;
		private static Thread _threadEmail;
		private static int _NextEmailSerialNo;
		private static int _mailerCounter;
		private static string[] getOneRow(string keyword, DataRow logData)
		{
			string[] array = new string[5];
			string text = "L" + logData["eventid"].ToString();
			string text2 = logData["insert_time"].ToString();
			string[] separator = new string[]
			{
				"^|^"
			};
			string[] argv = logData["parametervalue"].ToString().Split(separator, StringSplitOptions.None);
			int num = Convert.ToInt32(text.Substring(1));
			int num2 = num / 10000 % 10;
			int num3 = num / 100000;
			string text3 = "";
			switch (num2)
			{
			case 1:
				text3 = LangRes.Sev1Critical;
				break;
			case 2:
				text3 = LangRes.Sev2Warning;
				break;
			case 3:
				text3 = LangRes.Sev3Information;
				break;
			}
			string text4 = "";
			switch (num3)
			{
			case 1:
				text4 = LangRes.Cat1System;
				break;
			case 2:
				text4 = LangRes.Cat2Authentication;
				break;
			case 3:
				text4 = LangRes.Cat3Usermanagement;
				break;
			case 4:
				text4 = LangRes.Cat4Devicemanagement;
				break;
			case 5:
				text4 = LangRes.Cat5Systemtask;
				break;
			case 6:
				text4 = LangRes.Cat6Device;
				break;
			case 7:
				text4 = LangRes.Cat7DevTrap;
				break;
			}
			string msg = EventLogLang.getMsg("S" + text, new string[0]);
			string msg2 = EventLogLang.getMsg(text, argv);
			array[0] = text2;
			array[1] = text4;
			array[2] = text3;
			array[3] = msg;
			array[4] = msg2;
			if (keyword != null && !text2.Contains(keyword) && !text4.Contains(keyword) && !text3.Contains(keyword) && !msg.Contains(keyword) && !msg2.Contains(keyword))
			{
				return null;
			}
			return array;
		}
		public static int writeEventLog(string logKey, params string[] logPrar)
		{
			ValuePairs.getValuePair("uid");
			ValuePairs.getValuePair("Username");
			int num = 1;
			int result = LogAPI.processNewLog(ref num, logKey, logPrar);
			if (num > 0)
			{
				result = LogAPI.emailNewLog(logKey, logPrar);
			}
			return result;
		}
		private static int emailNewLog(string logKey, params string[] logPrar)
		{
			SMTPSetting sMTPSetting = new SMTPSetting();
			string[] array = sMTPSetting.Receiver.Split(new string[]
			{
				"\r\n",
				";",
				",",
				" "
			}, StringSplitOptions.RemoveEmptyEntries);
			List<string> list = new List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string item = array2[i];
				list.Add(item);
			}
			if (sMTPSetting.Status <= 0 || list.Count <= 0)
			{
				return 0;
			}
			string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			string msg = EventLogLang.getMsg("L" + logKey, logPrar);
			string msg2 = EventLogLang.getMsg("SL" + logKey, logPrar);
			string value = str + "<br/>" + msg;
			MailMsg mailMsg = new MailMsg(sMTPSetting.ServerIP, sMTPSetting.Port, sMTPSetting.Sender, list, sMTPSetting.AuthenticationFlag, sMTPSetting.AccountName, sMTPSetting.AccountPwd);
			mailMsg.Subject = "eco Sensors Event Notification - " + msg2;
			mailMsg.Body = new StringBuilder().AppendLine(value).ToString();
			mailMsg.Receiver = sMTPSetting.Receiver;
			if (logKey.Equals("0230002") || logKey.Equals("0110100") || logKey.Equals("0110090") || logKey.Equals("0130001"))
			{
				LogAPI.sendMailThread(mailMsg);
				return ErrorCode.SUCCESS;
			}
			lock (LogAPI._lockEmail)
			{
				if (LogAPI._mergedMail != null)
				{
					if (mailMsg.Subject.Equals(LogAPI._mergedMail.Subject, StringComparison.InvariantCultureIgnoreCase) && mailMsg.Receiver.Equals(LogAPI._mergedMail.Receiver, StringComparison.InvariantCultureIgnoreCase))
					{
						LogAPI._mergedMail._ItemsInBody++;
						LogAPI._mergedMail._mergedBody.Append("<br /><br />");
						LogAPI._mergedMail._mergedBody.Append(mailMsg.Body);
						LogAPI._lastEmailMerge = DateTime.Now;
					}
					else
					{
						LogAPI._emailQueue.Enqueue(LogAPI._mergedMail);
						LogAPI._emailEvent.Set();
						LogAPI._mergedMail = null;
					}
				}
				if (LogAPI._mergedMail == null)
				{
					LogAPI._mergedMail = mailMsg;
					LogAPI._mergedMail._SerialNo = LogAPI._NextEmailSerialNo++;
					LogAPI._mergedMail._ItemsInBody++;
					LogAPI._mergedMail._mergedBody.Append(mailMsg.Body);
					LogAPI._lastEmailMerge = DateTime.Now;
				}
			}
			if (LogAPI._threadEmail == null)
			{
				LogAPI._waitEmailHandles[0] = LogAPI._stopMailEvent;
				LogAPI._waitEmailHandles[1] = LogAPI._emailEvent;
				LogAPI._threadEmail = new Thread(new ParameterizedThreadStart(LogAPI.MailThreadScheduler));
				LogAPI._threadEmail.Name = "Email Scheduler Thread";
				LogAPI._threadEmail.CurrentCulture = CultureInfo.InvariantCulture;
				LogAPI._threadEmail.IsBackground = true;
				LogAPI._threadEmail.Start();
			}
			return ErrorCode.SUCCESS;
		}
		private static void MailThreadScheduler(object obj)
		{
			MailMsg mailMsg = null;
			while (true)
			{
				int num = WaitHandle.WaitAny(LogAPI._waitEmailHandles, 100);
				if (num == 258)
				{
					lock (LogAPI._lockEmail)
					{
						TimeSpan timeSpan = DateTime.Now - LogAPI._lastEmailMerge;
						if (timeSpan.TotalSeconds < 0.0)
						{
							LogAPI._lastEmailMerge = DateTime.Now;
							timeSpan = DateTime.Now - LogAPI._lastEmailMerge;
						}
						if (timeSpan.TotalSeconds <= 5.0)
						{
							continue;
						}
						if (LogAPI._mergedMail == null)
						{
							continue;
						}
						mailMsg = LogAPI._mergedMail;
						LogAPI._mergedMail = null;
					}
				}
				if (num == 0)
				{
					break;
				}
				try
				{
					if (LogAPI._mailerCounter < 10)
					{
						lock (LogAPI._lockEmail)
						{
							if (mailMsg == null)
							{
								if (LogAPI._emailQueue.Count > 0)
								{
									if (LogAPI._emailQueue.TryDequeue(out mailMsg))
									{
									}
								}
								else
								{
									LogAPI._emailEvent.Reset();
								}
							}
						}
						if (mailMsg != null)
						{
							Interlocked.Increment(ref LogAPI._mailerCounter);
							mailMsg.Body = new StringBuilder("Dear User, ").AppendLine("<br /><br />").Append(mailMsg._mergedBody).ToString();
							ThreadCreator.StartThread(new ParameterizedThreadStart(LogAPI.sendMailThread), mailMsg, true);
							mailMsg = null;
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}
		private static void sendMailThread(object obj)
		{
			MailMsg mailMsg = (MailMsg)obj;
			try
			{
				int num = MailAPI.GetInstance().SendEmail(mailMsg);
				if (num != ErrorCode.SUCCESS)
				{
					int num2 = 0;
					LogInfo.InsertNewLog(ref num2, "0120062", new string[]
					{
						mailMsg.Receiver,
						mailMsg.Subject.Replace("\r\n", "")
					});
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (LogAPI._mailerCounter > 0)
				{
					Interlocked.Decrement(ref LogAPI._mailerCounter);
				}
			}
		}
		private static int processNewLog(ref int mailflag, string logKey, params string[] logPrar)
		{
			return LogInfo.InsertNewLog(ref mailflag, logKey, logPrar);
		}
		public static int searchLogs(string s_keyword, int i_range_type, DateTime dt_start, DateTime dt_end)
		{
			LogAPI.i_myRangeType = i_range_type;
			LogAPI.dt_myStart = dt_start;
			if (i_range_type == 0)
			{
				LogAPI.dt_myEnd = DateTime.Now;
			}
			else
			{
				LogAPI.dt_myEnd = dt_end;
			}
			LogAPI.s_myKeyWord = null;
			LogAPI.i_matchedLogs = 0;
			LogAPI.i_foundLogs = LogInfo.GetRowCount(i_range_type, dt_start, dt_end);
			if (LogAPI.i_foundLogs > 0)
			{
				if (s_keyword != null)
				{
					LogAPI.s_myKeyWord = s_keyword;
					LogAPI.s_matchedLogs = new string[LogAPI.i_foundLogs][];
					DataTable searchResult = LogInfo.GetSearchResult(i_range_type, dt_start, dt_end, 0, 0, LogAPI.i_foundLogs);
					for (int i = 0; i < searchResult.Rows.Count; i++)
					{
						string[] oneRow = LogAPI.getOneRow(s_keyword, searchResult.Rows[i]);
						if (oneRow != null)
						{
							LogAPI.s_matchedLogs[LogAPI.i_matchedLogs] = oneRow;
							LogAPI.i_matchedLogs++;
						}
					}
				}
				else
				{
					LogAPI.i_matchedLogs = LogAPI.i_foundLogs;
				}
			}
			return LogAPI.i_matchedLogs;
		}
		public static DataTable dispOnePageLogs(int i_pagenum, int i_pagesize)
		{
			if (LogAPI.i_matchedLogs == 0)
			{
				return null;
			}
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T1No, new string[0]), typeof(int));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T2DateTime, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T3Category, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T4Severity, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T5Event, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T6LogInfo, new string[0]), typeof(string));
			if (LogAPI.s_myKeyWord != null)
			{
				int num = (i_pagenum - 1) * i_pagesize;
				int num2 = num + i_pagesize;
				if (num2 > LogAPI.i_matchedLogs)
				{
					i_pagenum = LogAPI.i_matchedLogs / i_pagesize;
					num = i_pagenum * i_pagesize;
					num2 = LogAPI.i_matchedLogs;
				}
				for (int i = num; i < num2; i++)
				{
					dataTable.Rows.Add(new object[]
					{
						i - num + 1,
						LogAPI.s_matchedLogs[i][0],
						LogAPI.s_matchedLogs[i][1],
						LogAPI.s_matchedLogs[i][2],
						LogAPI.s_matchedLogs[i][3],
						LogAPI.s_matchedLogs[i][4]
					});
				}
			}
			else
			{
				DataTable searchResult = LogInfo.GetSearchResult(LogAPI.i_myRangeType, LogAPI.dt_myStart, LogAPI.dt_myEnd, i_pagenum, i_pagesize, LogAPI.i_foundLogs);
				for (int i = 0; i < searchResult.Rows.Count; i++)
				{
					string[] oneRow = LogAPI.getOneRow(null, searchResult.Rows[i]);
					if (oneRow != null)
					{
						dataTable.Rows.Add(new object[]
						{
							i + 1,
							oneRow[0],
							oneRow[1],
							oneRow[2],
							oneRow[3],
							oneRow[4]
						});
					}
				}
			}
			return dataTable;
		}
		public static DataTable getAllLogs()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T1No, new string[0]), typeof(int));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T2DateTime, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T3Category, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T4Severity, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T5Event, new string[0]), typeof(string));
			dataTable.Columns.Add(EventLogLang.getMsg(LangRes.T6LogInfo, new string[0]), typeof(string));
			if (LogAPI.i_matchedLogs == 0)
			{
				return dataTable;
			}
			if (LogAPI.s_myKeyWord != null)
			{
				for (int i = 0; i < LogAPI.i_matchedLogs; i++)
				{
					dataTable.Rows.Add(new object[]
					{
						i + 1,
						LogAPI.s_matchedLogs[i][0],
						LogAPI.s_matchedLogs[i][1],
						LogAPI.s_matchedLogs[i][2],
						LogAPI.s_matchedLogs[i][3],
						LogAPI.s_matchedLogs[i][4]
					});
				}
			}
			else
			{
				DataTable searchResult = LogInfo.GetSearchResult(LogAPI.i_myRangeType, LogAPI.dt_myStart, LogAPI.dt_myEnd, 0, 0, LogAPI.i_foundLogs);
				int num = 0;
				foreach (DataRow logData in searchResult.Rows)
				{
					string[] oneRow = LogAPI.getOneRow(null, logData);
					if (oneRow != null)
					{
						dataTable.Rows.Add(new object[]
						{
							num + 1,
							oneRow[0],
							oneRow[1],
							oneRow[2],
							oneRow[3],
							oneRow[4]
						});
						num++;
					}
				}
			}
			return dataTable;
		}
		static LogAPI()
		{
			// Note: this type is marked as 'beforefieldinit'.
			WaitHandle[] waitEmailHandles = new WaitHandle[2];
			LogAPI._waitEmailHandles = waitEmailHandles;
			LogAPI._threadEmail = null;
			LogAPI._NextEmailSerialNo = 0;
			LogAPI._mailerCounter = 0;
		}
	}
}
