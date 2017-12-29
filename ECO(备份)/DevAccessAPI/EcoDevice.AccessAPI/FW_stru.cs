using System;
namespace EcoDevice.AccessAPI
{
	public struct FW_stru
	{
		public const int FWvalidate_no = 0;
		public const int FWvalidate_1 = 1;
		public const int FWvalidate_2 = 2;
		private int m_validate;
		private string m_NMStart;
		private string m_ext;
		public int FWvalidate
		{
			get
			{
				return this.m_validate;
			}
			set
			{
				this.m_validate = value;
			}
		}
		public string FWnms
		{
			get
			{
				return this.m_NMStart;
			}
			set
			{
				this.m_NMStart = value.ToUpper();
			}
		}
		public string FWext
		{
			get
			{
				return this.m_ext;
			}
			set
			{
				this.m_ext = value.ToUpper();
			}
		}
		public void init()
		{
			this.m_validate = 0;
			this.m_NMStart = "";
			this.m_ext = "";
		}
		public void copy(FW_stru src)
		{
			this.m_validate = src.m_validate;
			this.m_NMStart = src.m_NMStart;
			this.m_ext = src.m_ext;
		}
	}
}
