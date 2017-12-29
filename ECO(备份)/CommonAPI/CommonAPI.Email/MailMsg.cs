using System;
using System.Collections.Generic;
using System.Text;
namespace CommonAPI.Email
{
	public class MailMsg
	{
		public int _SerialNo;
		public int _ItemsInBody;
		public StringBuilder _mergedBody;
		private string body;
		private string subject = "eco Sensors Event Notification";
		private string from;
		private string receiver;
		private List<string> to = new List<string>();
		private List<string> cc = new List<string>();
		private MailConfig mailCfg = new MailConfig();
		public string Body
		{
			get
			{
				return this.body;
			}
			set
			{
				this.body = value;
			}
		}
		public string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				this.subject = value;
			}
		}
		public string From
		{
			get
			{
				return this.from;
			}
			set
			{
				this.from = value;
			}
		}
		public List<string> To
		{
			get
			{
				return this.to;
			}
			set
			{
				this.to = value;
			}
		}
		public List<string> Cc
		{
			get
			{
				return this.cc;
			}
			set
			{
				this.cc = value;
			}
		}
		public MailConfig MailCfg
		{
			get
			{
				return this.mailCfg;
			}
			set
			{
				this.mailCfg = value;
			}
		}
		public string Receiver
		{
			get
			{
				return this.receiver;
			}
			set
			{
				this.receiver = value;
			}
		}
		public MailMsg()
		{
			this._SerialNo = 0;
			this._ItemsInBody = 0;
			this._mergedBody = new StringBuilder();
		}
		public MailMsg(string host, int port, string from, List<string> to, int authflag, string username, string password)
		{
			this._SerialNo = 0;
			this._ItemsInBody = 0;
			this._mergedBody = new StringBuilder();
			this.mailCfg.Host = host;
			this.mailCfg.Port = port;
			this.mailCfg.Authentication = authflag;
			if (authflag > 0)
			{
				this.mailCfg.Username = username;
				this.mailCfg.Password = password;
			}
			else
			{
				this.mailCfg.Username = string.Empty;
				this.mailCfg.Password = string.Empty;
			}
			this.mailCfg.Timeout = 30000;
			this.from = from;
			this.to = to;
		}
	}
}
