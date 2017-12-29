using System;
namespace CommonAPI.Email
{
	public abstract class MailSender
	{
		protected MailConfig config;
		public MailConfig MailConfig
		{
			get
			{
				return this.config;
			}
			set
			{
				this.config = value;
			}
		}
		public MailSender(MailConfig config)
		{
			this.config = config;
		}
		public abstract bool Send(MailMsg message);
	}
}
