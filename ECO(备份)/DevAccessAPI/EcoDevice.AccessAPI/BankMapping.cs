using System;
namespace EcoDevice.AccessAPI
{
	public class BankMapping
	{
		private int bankId;
		private int bankNum;
		private string bankName;
		public int BankNumber
		{
			get
			{
				return this.bankNum;
			}
			set
			{
				this.bankNum = value;
			}
		}
		public int BankId
		{
			get
			{
				return this.bankId;
			}
			set
			{
				this.bankId = value;
			}
		}
		public string BankName
		{
			get
			{
				return this.bankName;
			}
			set
			{
				this.bankName = value;
			}
		}
		public BankMapping(int bankNum)
		{
			this.bankNum = bankNum;
		}
	}
}
