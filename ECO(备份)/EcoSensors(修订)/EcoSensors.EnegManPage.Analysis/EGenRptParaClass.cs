using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace EcoSensors.EnegManPage.Analysis
{
	public class EGenRptParaClass
	{
		private string m_stxttitle;
		private string m_stxtwrite;
		private string m_sdtptime;
		private int m_period_SelectedIndex;
		private string m_sBeginText;
		private int m_iduration;
		private System.Collections.ArrayList m_gppara_list;
		private bool m_chkchart1_Checked;
		private bool m_chkchart2_Checked;
		private bool m_chkchart3_Checked;
		private bool m_chkchart4_Checked;
		private bool m_chkchart5_Checked;
		private bool m_chkchart6_Checked;
		private bool m_chkchart7_Checked;
		private bool m_chkchart8_Checked;
		private bool m_chkchart9_Checked;
		private double m_dCo2_elec;
		private double m_dprice_elec;
		private double m_dprice_Co2;
		public System.DateTime DTbegin = default(System.DateTime);
		public System.DateTime DTend = default(System.DateTime);
		public System.DateTime DTbegin_minus1 = default(System.DateTime);
		public string strBegin = "";
		public string strEnd = "";
		public string groupby = "";
		public string report_from = "";
		public string dblibnameDev = "device_data_daily";
		public string dblibnamePort = "port_data_daily";
		public string extra_str = "";
		public string AxisX_LabelStyleFormat = "yyyy-MM-dd";
		public double AxisX_Interval = 1.0;
		public DateTimeIntervalType AxisX_IntervalType = DateTimeIntervalType.Days;
		public string m_savepath = "";
		public string Txttitle
		{
			get
			{
				return this.m_stxttitle;
			}
		}
		public string Txtwriter
		{
			get
			{
				return this.m_stxtwrite;
			}
		}
		public string Dtptime
		{
			get
			{
				return this.m_sdtptime;
			}
		}
		public int Cboperiod_SelectedIndex
		{
			get
			{
				return this.m_period_SelectedIndex;
			}
		}
		public string BeginText
		{
			get
			{
				return this.m_sBeginText;
			}
		}
		public int Cboduration
		{
			get
			{
				return this.m_iduration;
			}
		}
		public System.Collections.ArrayList gppara_list
		{
			get
			{
				return this.m_gppara_list;
			}
		}
		public void pageInit(EGenRptPara pPara)
		{
			this.m_stxttitle = pPara.Txttitle;
			this.m_stxtwrite = pPara.Txtwriter;
			this.m_sdtptime = pPara.Dtptime;
			this.m_period_SelectedIndex = pPara.Cboperiod_SelectedIndex;
			this.m_sBeginText = pPara.BeginText;
			this.m_iduration = pPara.Cboduration;
			this.m_gppara_list = this.getAnalysisGroup(pPara);
			this.m_chkchart1_Checked = pPara.chkchart1_Checked();
			this.m_chkchart2_Checked = pPara.chkchart2_Checked();
			this.m_chkchart3_Checked = pPara.chkchart3_Checked();
			this.m_chkchart4_Checked = pPara.chkchart4_Checked();
			this.m_chkchart5_Checked = pPara.chkchart5_Checked();
			this.m_chkchart6_Checked = pPara.chkchart6_Checked();
			this.m_chkchart7_Checked = pPara.chkchart7_Checked();
			this.m_chkchart8_Checked = pPara.chkchart8_Checked();
			this.m_chkchart9_Checked = pPara.chkchart9_Checked();
			this.m_dCo2_elec = pPara.Co2_elec();
			this.m_dprice_elec = pPara.price_elec();
			this.m_dprice_Co2 = pPara.price_Co2();
			int num = 1;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") || DBUrl.SERVERMODE)
			{
				num = 2;
			}
			switch (this.Cboperiod_SelectedIndex)
			{
			case 0:
				this.DTbegin = System.Convert.ToDateTime(this.BeginText + ":0:0");
				this.DTend = this.DTbegin.AddHours((double)this.Cboduration);
				this.DTbegin_minus1 = this.DTbegin.AddHours(-1.0);
				this.report_from = EcoLanguage.getMsg(LangRes.Rpt_shfromHourly, new string[0]) + " " + this.Txtwriter;
				if (num == 1)
				{
					this.groupby = "FORMAT(insert_time, 'yyyy-MM-dd HH')";
				}
				else
				{
					this.groupby = "date_format(insert_time, '%Y-%m-%d %H')";
				}
				this.dblibnameDev = "device_data_hourly";
				this.dblibnamePort = "port_data_hourly";
				this.strBegin = this.DTbegin.ToString("yyyy-MM-dd HH:mm:ss");
				this.strEnd = this.DTend.ToString("yyyy-MM-dd HH:mm:ss");
				this.extra_str = ":0:0";
				this.AxisX_LabelStyleFormat = "yyyy-MM-dd HH";
				this.AxisX_IntervalType = DateTimeIntervalType.Hours;
				return;
			case 1:
				this.DTbegin = System.Convert.ToDateTime(this.BeginText);
				this.DTend = this.DTbegin.AddDays((double)this.Cboduration);
				this.DTbegin_minus1 = this.DTbegin.AddDays(-1.0);
				this.report_from = EcoLanguage.getMsg(LangRes.Rpt_shfromDaily, new string[0]) + " " + this.Txtwriter;
				if (num == 1)
				{
					this.groupby = "FORMAT(insert_time, 'yyyy-MM-dd')";
				}
				else
				{
					this.groupby = "date_format(insert_time, '%Y-%m-%d')";
				}
				this.dblibnameDev = "device_data_daily";
				this.dblibnamePort = "port_data_daily";
				this.strBegin = this.DTbegin.ToString("yyyy-MM-dd");
				this.strEnd = this.DTend.ToString("yyyy-MM-dd");
				this.AxisX_LabelStyleFormat = "yyyy-MM-dd";
				this.AxisX_IntervalType = DateTimeIntervalType.Days;
				return;
			case 2:
				this.DTbegin = System.Convert.ToDateTime(this.BeginText);
				this.DTend = this.DTbegin.AddMonths(this.Cboduration);
				this.DTbegin_minus1 = this.DTbegin.AddMonths(-1);
				this.report_from = EcoLanguage.getMsg(LangRes.Rpt_shfromMonthly, new string[0]) + " " + this.Txtwriter;
				if (num == 1)
				{
					this.groupby = "FORMAT(insert_time, 'yyyy-MM')";
				}
				else
				{
					this.groupby = "date_format(insert_time, '%Y-%m')";
				}
				this.dblibnameDev = "device_data_daily";
				this.dblibnamePort = "port_data_daily";
				this.strBegin = this.DTbegin.ToString("yyyy-MM-dd");
				this.strEnd = this.DTend.ToString("yyyy-MM-dd");
				this.AxisX_LabelStyleFormat = "yyyy-MM";
				this.AxisX_IntervalType = DateTimeIntervalType.Months;
				return;
			case 3:
				this.DTbegin = System.Convert.ToDateTime(this.BeginText + "-01");
				this.DTend = this.DTbegin.AddMonths(this.Cboduration * 3);
				this.DTbegin_minus1 = this.DTbegin.AddMonths(-3);
				this.report_from = EcoLanguage.getMsg(LangRes.Rpt_shfromQuarterly, new string[0]) + " " + this.Txtwriter;
				if (num == 1)
				{
					this.groupby = "FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')";
				}
				else
				{
					this.groupby = "concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time))";
				}
				this.dblibnameDev = "device_data_daily";
				this.dblibnamePort = "port_data_daily";
				this.strBegin = this.DTbegin.ToString("yyyy-MM-dd");
				this.strEnd = this.DTend.ToString("yyyy-MM-dd");
				this.AxisX_LabelStyleFormat = "yyyy-MM";
				this.AxisX_Interval = 3.0;
				this.AxisX_IntervalType = DateTimeIntervalType.Months;
				return;
			default:
				return;
			}
		}
		public bool chkchart1_Checked()
		{
			return this.m_chkchart1_Checked;
		}
		public bool chkchart2_Checked()
		{
			return this.m_chkchart2_Checked;
		}
		public bool chkchart3_Checked()
		{
			return this.m_chkchart3_Checked;
		}
		public bool chkchart4_Checked()
		{
			return this.m_chkchart4_Checked;
		}
		public bool chkchart5_Checked()
		{
			return this.m_chkchart5_Checked;
		}
		public bool chkchart6_Checked()
		{
			return this.m_chkchart6_Checked;
		}
		public bool chkchart7_Checked()
		{
			return this.m_chkchart7_Checked;
		}
		public bool chkchart8_Checked()
		{
			return this.m_chkchart8_Checked;
		}
		public bool chkchart9_Checked()
		{
			return this.m_chkchart9_Checked;
		}
		public double Co2_elec()
		{
			return this.m_dCo2_elec;
		}
		public double price_elec()
		{
			return this.m_dprice_elec;
		}
		public double price_Co2()
		{
			return this.m_dprice_Co2;
		}
		private System.Collections.ArrayList getAnalysisGroup(EGenRptPara m_pParaWindow)
		{
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			foreach (ListViewItem listViewItem in m_pParaWindow.Grouplist)
			{
				string text = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[0];
				string text2 = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[1];
				if (text2.Length == 0)
				{
					text2 = "0";
				}
				string text3 = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[2];
				string text4 = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[3];
				string text5 = listViewItem.SubItems[1].Text;
				string text6 = "0,";
				string text7 = "0,";
				string text8 = "0,";
				string text9 = "0,";
				string text10 = "0,";
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
				string key;
				switch (key = text3)
				{
				case "zone":
				{
					string str_sql = "select racks from zone_info where id in (" + text2 + ") and racks <>''";
					DataTable dataTable = DBTools.CreateDataTable4SysDB(str_sql);
					text9 = "0,";
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string str_sql2 = "select id,model_nm,fw_version from device_base_info where rack_id in (" + System.Convert.ToString(dataTable.Rows[i]["racks"]) + ") ";
						DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
						string text11 = "";
						text10 = "0,";
						stringBuilder2 = new System.Text.StringBuilder();
						stringBuilder3 = new System.Text.StringBuilder();
						stringBuilder = new System.Text.StringBuilder();
						for (int j = 0; j < dataTable2.Rows.Count; j++)
						{
							System.Convert.ToString(dataTable2.Rows[j]["id"]);
							string modelname = System.Convert.ToString(dataTable2.Rows[j]["model_nm"]);
							string fmwareVer = System.Convert.ToString(dataTable2.Rows[j]["fw_version"]);
							DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(modelname, fmwareVer);
							if (!deviceModelConfig.devcapacity.Equals("N/A"))
							{
								if (deviceModelConfig.perportreading == 2)
								{
									stringBuilder2.Append(System.Convert.ToString(dataTable2.Rows[j]["id"]) + ",");
								}
								else
								{
									stringBuilder3.Append(System.Convert.ToString(dataTable2.Rows[j]["id"]) + ",");
								}
								stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[j]["id"]) + ",");
							}
						}
						text11 += stringBuilder.ToString();
						text10 += stringBuilder2.ToString();
						text8 += stringBuilder3.ToString();
						if (text11.Length > 0)
						{
							text11 = text11.Substring(0, text11.Length - 1);
						}
						if (text10.Length > 0)
						{
							text10 = text10.Substring(0, text10.Length - 1);
						}
						string str_sql3 = "select id from port_info where device_id in (" + text10 + ")";
						DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
						stringBuilder = new System.Text.StringBuilder();
						for (int k = 0; k < dataTable3.Rows.Count; k++)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[k]["id"]) + ",");
						}
						text7 += stringBuilder.ToString();
						text9 = text9 + text11 + ",";
					}
					text9 = commUtil.uniqueIDs(text9);
					if (text9.Length > 0)
					{
						text6 = text9.Substring(0, text9.Length - 1);
					}
					text8 = commUtil.uniqueIDs(text8);
					if (text8.Length > 0)
					{
						text8 = text8.Substring(0, text8.Length - 1);
					}
					text7 = commUtil.uniqueIDs(text7);
					if (text7.Length > 0)
					{
						text7 = text7.Substring(0, text7.Length - 1);
					}
					break;
				}
				case "rack":
				case "allrack":
				{
					string str_sql2 = "select id,model_nm,fw_version from device_base_info where rack_id in (" + text2 + ")";
					DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
					stringBuilder2 = new System.Text.StringBuilder();
					stringBuilder3 = new System.Text.StringBuilder();
					stringBuilder = new System.Text.StringBuilder();
					for (int l = 0; l < dataTable2.Rows.Count; l++)
					{
						System.Convert.ToString(dataTable2.Rows[l]["id"]);
						string modelname2 = System.Convert.ToString(dataTable2.Rows[l]["model_nm"]);
						string fmwareVer2 = System.Convert.ToString(dataTable2.Rows[l]["fw_version"]);
						DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(modelname2, fmwareVer2);
						if (!deviceModelConfig2.devcapacity.Equals("N/A"))
						{
							if (deviceModelConfig2.perportreading == 2)
							{
								stringBuilder2.Append(System.Convert.ToString(dataTable2.Rows[l]["id"]) + ",");
							}
							else
							{
								stringBuilder3.Append(System.Convert.ToString(dataTable2.Rows[l]["id"]) + ",");
							}
							stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[l]["id"]) + ",");
						}
					}
					text9 += stringBuilder.ToString();
					text10 += stringBuilder2.ToString();
					text8 += stringBuilder3.ToString();
					if (text9.Length > 0)
					{
						text6 = text9.Substring(0, text9.Length - 1);
					}
					if (text10.Length > 0)
					{
						text10 = text10.Substring(0, text10.Length - 1);
					}
					if (text8.Length > 0)
					{
						text8 = text8.Substring(0, text8.Length - 1);
					}
					string str_sql3 = "select id from port_info where device_id in (" + text10 + ")";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int m = 0; m < dataTable3.Rows.Count; m++)
					{
						stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[m]["id"]) + ",");
					}
					text7 += stringBuilder.ToString();
					if (text7.Length > 0)
					{
						text7 = text7.Substring(0, text7.Length - 1);
					}
					break;
				}
				case "dev":
				case "alldev":
				{
					string str_sql2 = "select id,model_nm,fw_version from device_base_info where id in (" + text2 + ")";
					DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
					text10 = "0,";
					stringBuilder2 = new System.Text.StringBuilder();
					stringBuilder3 = new System.Text.StringBuilder();
					stringBuilder = new System.Text.StringBuilder();
					for (int n = 0; n < dataTable2.Rows.Count; n++)
					{
						System.Convert.ToString(dataTable2.Rows[n]["id"]);
						string modelname3 = System.Convert.ToString(dataTable2.Rows[n]["model_nm"]);
						string fmwareVer3 = System.Convert.ToString(dataTable2.Rows[n]["fw_version"]);
						DevModelConfig deviceModelConfig3 = DevAccessCfg.GetInstance().getDeviceModelConfig(modelname3, fmwareVer3);
						if (!deviceModelConfig3.devcapacity.Equals("N/A"))
						{
							if (deviceModelConfig3.perportreading == 2)
							{
								stringBuilder2.Append(System.Convert.ToString(dataTable2.Rows[n]["id"]) + ",");
							}
							else
							{
								stringBuilder3.Append(System.Convert.ToString(dataTable2.Rows[n]["id"]) + ",");
							}
							stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[n]["id"]) + ",");
						}
					}
					text10 += stringBuilder2.ToString();
					text8 += stringBuilder3.ToString();
					text6 += stringBuilder.ToString();
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					if (text10.Length > 0)
					{
						text10 = text10.Substring(0, text10.Length - 1);
					}
					if (text8.Length > 0)
					{
						text8 = text8.Substring(0, text8.Length - 1);
					}
					string str_sql3 = "select id from port_info where device_id in (" + text10 + ")";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int num2 = 0; num2 < dataTable3.Rows.Count; num2++)
					{
						stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[num2]["id"]) + ",");
					}
					text7 += stringBuilder.ToString();
					if (text7.Length > 0)
					{
						text7 = text7.Substring(0, text7.Length - 1);
					}
					break;
				}
				case "alloutlet":
				{
					text6 = "0";
					text8 = "0";
					string str_sql3 = "select a.id,b.model_nm,b.fw_version from port_info a  left join device_base_info b on a.device_id=b.id where a.id in (select id from port_info)";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int num3 = 0; num3 < dataTable3.Rows.Count; num3++)
					{
						string modelname4 = System.Convert.ToString(dataTable3.Rows[num3]["model_nm"]);
						string fmwareVer4 = System.Convert.ToString(dataTable3.Rows[num3]["fw_version"]);
						DevModelConfig deviceModelConfig4 = DevAccessCfg.GetInstance().getDeviceModelConfig(modelname4, fmwareVer4);
						if (!deviceModelConfig4.devcapacity.Equals("N/A") && deviceModelConfig4.perportreading == 2)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[num3]["id"]) + ",");
						}
					}
					text7 += stringBuilder.ToString();
					if (text7.Length > 0)
					{
						text7 = text7.Substring(0, text7.Length - 1);
					}
					break;
				}
				case "outlet":
				{
					text6 = "0";
					text8 = "0";
					string str_sql3 = "select a.id,b.model_nm,b.fw_version from port_info a  left join device_base_info b on a.device_id=b.id where a.id in (select dest_id from group_detail where grouptype='outlet' and group_id=" + text + ")";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int num4 = 0; num4 < dataTable3.Rows.Count; num4++)
					{
						string modelname5 = System.Convert.ToString(dataTable3.Rows[num4]["model_nm"]);
						string fmwareVer5 = System.Convert.ToString(dataTable3.Rows[num4]["fw_version"]);
						DevModelConfig deviceModelConfig5 = DevAccessCfg.GetInstance().getDeviceModelConfig(modelname5, fmwareVer5);
						if (!deviceModelConfig5.devcapacity.Equals("N/A") && deviceModelConfig5.perportreading == 2)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[num4]["id"]) + ",");
						}
					}
					text7 += stringBuilder.ToString();
					if (text7.Length > 0)
					{
						text7 = text7.Substring(0, text7.Length - 1);
					}
					break;
				}
				}
				arrayList.Add(string.Concat(new string[]
				{
					text6,
					"|",
					text,
					"|",
					text5,
					"|",
					text3.ToString(),
					"|",
					text7,
					"|",
					text8,
					"|",
					text4
				}));
			}
			return arrayList;
		}
	}
}
