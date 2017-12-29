using System;
namespace DBAccessAPI
{
	public class CommParaClass
	{
		private long long_1;
		private int int_1;
		private string str_1 = "";
		private string str_2 = "";
		public long Long_First
		{
			get
			{
				return this.long_1;
			}
			set
			{
				this.long_1 = value;
			}
		}
		public int Integer_First
		{
			get
			{
				return this.int_1;
			}
			set
			{
				this.int_1 = value;
			}
		}
		public string String_First
		{
			get
			{
				return this.str_1;
			}
			set
			{
				this.str_1 = value;
			}
		}
		public string String_2
		{
			get
			{
				return this.str_2;
			}
			set
			{
				this.str_2 = value;
			}
		}
		public CommParaClass()
		{
			this.long_1 = 0L;
			this.int_1 = 0;
			this.str_1 = "";
			this.str_2 = "";
		}
	}
}
