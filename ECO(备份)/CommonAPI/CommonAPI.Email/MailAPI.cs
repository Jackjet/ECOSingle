using CommonAPI.ThreadWrapper;
using System;
using System.Threading;
namespace CommonAPI.Email
{
	public class MailAPI
	{
		private int lastErrorCode;
		private static MailAPI instance = new MailAPI();
		private int debugcount;
		public static MailAPI GetInstance()
		{
			return MailAPI.instance;
		}
		private MailAPI()
		{
		}
		public int SendEmail(MailMsg mailMsg)
		{
			this.debugcount = 0;
			this.lastErrorCode = ErrorCode.ONGOING;
			ThreadCreator.StartThread(new ParameterizedThreadStart(this.sendMail), mailMsg, true);
			while (this.lastErrorCode == ErrorCode.ONGOING)
			{
				try
				{
					Thread.Sleep(300);
				}
				catch (Exception)
				{
					break;
				}
				this.debugcount++;
			}
			return this.lastErrorCode;
		}
		private void sendMail(object obj)
		{
			MailMsg mailMsg = (MailMsg)obj;
			try
			{
				MailSender mailSender = new DefaultMailSender(mailMsg.MailCfg);
				bool flag = mailSender.Send(mailMsg);
				if (flag)
				{
					this.lastErrorCode = ErrorCode.SUCCESS;
				}
				else
				{
					this.lastErrorCode = ErrorCode.FAIL;
				}
			}
			catch (Exception)
			{
				this.lastErrorCode = ErrorCode.EXCEPTION;
			}
		}
		public int GetLastErrorCode()
		{
			return this.lastErrorCode;
		}
	}
}
