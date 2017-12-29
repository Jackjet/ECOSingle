using System;
namespace CommonAPI.Email
{
	public class MailConfig
	{
		private string host;
		private int authentication;
		private string username;
		private string password;
		private int port = 25;
		private int timeout = 100000;
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = value;
			}
		}
		public int Authentication
		{
			get
			{
				return this.authentication;
			}
			set
			{
				this.authentication = value;
			}
		}
		public string Username
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = value;
			}
		}
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.timeout = value;
			}
		}
	}
}
