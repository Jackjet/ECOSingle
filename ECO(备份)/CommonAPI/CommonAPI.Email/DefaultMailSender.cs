using System;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace CommonAPI.Email
{
	public class DefaultMailSender : MailSender
	{
		public DefaultMailSender(MailConfig config) : base(config)
		{
		}
		public override bool Send(MailMsg message)
		{
			MailMessage message2 = this.parseMailMsg(message);
			SmtpClient smtpClient = this.parseSmtpClient(this.config);
			bool result;
			try
			{
				smtpClient.Send(message2);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		private MailMessage parseMailMsg(MailMsg message)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.Body = message.Body;
			mailMessage.BodyEncoding = Encoding.UTF8;
			foreach (string current in message.Cc)
			{
				mailMessage.CC.Add(current);
			}
			foreach (string current2 in message.To)
			{
				if (mailMessage.To.Count == 0)
				{
					mailMessage.To.Add(current2);
				}
				else
				{
					mailMessage.CC.Add(current2);
				}
			}
			mailMessage.IsBodyHtml = true;
			mailMessage.Subject = message.Subject;
			mailMessage.SubjectEncoding = Encoding.UTF8;
			mailMessage.From = new MailAddress(message.From);
			return mailMessage;
		}
		private SmtpClient parseSmtpClient(MailConfig config)
		{
			SmtpClient smtpClient = new SmtpClient();
			if (!string.IsNullOrEmpty(config.Username))
			{
				smtpClient.Credentials = new NetworkCredential(config.Username, config.Password);
			}
			smtpClient.Host = config.Host;
			smtpClient.Port = config.Port;
			smtpClient.Timeout = config.Timeout;
			return smtpClient;
		}
	}
}
