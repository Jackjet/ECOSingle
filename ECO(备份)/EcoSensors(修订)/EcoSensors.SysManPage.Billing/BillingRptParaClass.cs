using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class BillingRptParaClass
	{
		private string m_Txttitle;
		private string m_Txtwriter;
		private string m_Dtptime;
		private int m_RptType;
		private string m_BeginText;
		private int m_Cboduration;
		private DataTable dataTableRst;
		private int m_tableType_index;
		private System.Collections.ArrayList m_ret_list;
		public string Txttitle
		{
			get
			{
				return this.m_Txttitle;
			}
		}
		public string Txtwriter
		{
			get
			{
				return this.m_Txtwriter;
			}
		}
		public string Dtptime
		{
			get
			{
				return this.m_Dtptime;
			}
		}
		public int RptType
		{
			get
			{
				return this.m_RptType;
			}
		}
		public DataTable DataTableRst
		{
			get
			{
				return this.dataTableRst;
			}
		}
		public int tableType_index
		{
			get
			{
				return this.m_tableType_index;
			}
		}
		public BillingRptParaClass(BillingRptPara pPara)
		{
			this.m_Txttitle = pPara.Txttitle;
			this.m_Txtwriter = pPara.Txtwriter;
			this.m_Dtptime = pPara.Dtptime;
			this.m_RptType = pPara.RptType;
			this.m_BeginText = pPara.BeginText;
			this.m_Cboduration = pPara.Cboduration;
			this.m_ret_list = this.getAnalysisGroup(pPara);
		}
		public void pageInit()
		{
			System.DateTime dateTime = default(System.DateTime);
			System.DateTime dateTime2 = default(System.DateTime);
			string strBegin = "";
			string strEnd = "";
			dateTime = System.Convert.ToDateTime(this.m_BeginText);
			dateTime2 = dateTime.AddMonths(this.m_Cboduration);
			strBegin = dateTime.ToString("yyyy-MM-dd");
			strEnd = dateTime2.ToString("yyyy-MM-dd");
			this.dataTableRst = new DataTable();
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_GroupName, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Month, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_AvgAmp, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Voltage, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_AvgKW, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_KWH, new string[0]), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Rate, new string[]
			{
				EcoGlobalVar.CurCurrency
			}), typeof(string));
			this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Fee, new string[]
			{
				EcoGlobalVar.CurCurrency
			}), typeof(string));
			if (this.m_RptType == 1)
			{
				if (Sys_Para.GetBill_ratetype() != 0)
				{
					this.m_tableType_index = 1;
					DataColumn dataColumn = this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Period, new string[0]), typeof(string));
					dataColumn.SetOrdinal(2);
				}
			}
			else
			{
				if (this.m_RptType == 2)
				{
					if (Sys_Para.GetBill_ratetype() == 0)
					{
						this.m_tableType_index = 2;
						DataColumn dataColumn2 = this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_RackName, new string[0]), typeof(string));
						dataColumn2.SetOrdinal(1);
					}
					else
					{
						this.m_tableType_index = 3;
						DataColumn dataColumn3 = this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_RackName, new string[0]), typeof(string));
						dataColumn3.SetOrdinal(1);
						dataColumn3 = this.dataTableRst.Columns.Add(EcoLanguage.getMsg(LangRes.BILLTB_Period, new string[0]), typeof(string));
						dataColumn3.SetOrdinal(3);
					}
				}
			}
			for (int i = 0; i < this.m_ret_list.Count; i++)
			{
				string[] array = this.m_ret_list[i].ToString().Split(new char[]
				{
					'|'
				});
				string text = array[BillingRptShow.AnalysisIndex_devIDs];
				string value = array[BillingRptShow.AnalysisIndex_gpID];
				long l_gid = System.Convert.ToInt64(value);
				string text2 = array[BillingRptShow.AnalysisIndex_gpNM];
				string text3 = dateTime.ToString("MMM yyyy");
				string text4 = array[BillingRptShow.AnalysisIndex_gpTP];
				string text5 = array[BillingRptShow.AnalysisIndex_portIDs];
				int bill_ratetype = Sys_Para.GetBill_ratetype();
				float bill_1rate = Sys_Para.GetBill_1rate();
				float bill_2rate = Sys_Para.GetBill_2rate1();
				float bill_2rate2 = Sys_Para.GetBill_2rate2();
				if (text5.Equals("0"))
				{
					text5 = "";
				}
				if (text.Equals("0"))
				{
					text = "";
				}
				double num = 0.0;
				double num2 = 0.0;
				if (this.m_RptType == 1)
				{
					this.filldata(this.dataTableRst, text2, -1L, text3, strBegin, strEnd, text, text5, bill_ratetype, bill_1rate, bill_2rate, bill_2rate2, ref num, ref num2);
				}
				else
				{
					if (this.m_RptType == 2)
					{
						num = 0.0;
						num2 = 0.0;
						System.Collections.Generic.Dictionary<long, string> rackDeviceMapByGroupID = DBTools.GetRackDeviceMapByGroupID(l_gid, text4);
						foreach (System.Collections.Generic.KeyValuePair<long, string> current in rackDeviceMapByGroupID)
						{
							text = "";
							text5 = "";
							if (text4 == "alloutlet" || text4 == "outlet")
							{
								text5 = current.Value;
							}
							else
							{
								text = current.Value;
							}
							if (this.filldata(this.dataTableRst, text2, current.Key, text3, strBegin, strEnd, text, text5, bill_ratetype, bill_1rate, bill_2rate, bill_2rate2, ref num, ref num2) > 0)
							{
								text2 = "";
							}
						}
						if (rackDeviceMapByGroupID.Count <= 0)
						{
							if (bill_ratetype == 0)
							{
								this.dataTableRst.Rows.Add(new object[]
								{
									text2,
									"",
									text3,
									"N/A",
									"N/A",
									"0",
									"0",
									bill_1rate.ToString("F2"),
									"0"
								});
							}
							else
							{
								this.dataTableRst.Rows.Add(new object[]
								{
									text2,
									"",
									text3,
									EcoLanguage.getMsg(LangRes.Rpt_BillPEAK, new string[0]),
									"N/A",
									"N/A",
									"0",
									"0",
									bill_2rate.ToString("F2"),
									"0"
								});
								this.dataTableRst.Rows.Add(new object[]
								{
									"",
									"",
									text3,
									EcoLanguage.getMsg(LangRes.Rpt_BillnonPEAK, new string[0]),
									"N/A",
									"N/A",
									"0",
									"0",
									bill_2rate2.ToString("F2"),
									"0"
								});
							}
						}
						if (bill_ratetype == 0)
						{
							this.dataTableRst.Rows.Add(new object[]
							{
								EcoLanguage.getMsg(LangRes.BILLTB_Subtotal, new string[0]),
								EcoLanguage.getMsg(LangRes.Rpt_BillALL, new string[0]),
								"",
								"",
								"",
								"",
								num.ToString("F4"),
								"",
								num2.ToString("F2")
							});
						}
						else
						{
							this.dataTableRst.Rows.Add(new object[]
							{
								EcoLanguage.getMsg(LangRes.BILLTB_Subtotal, new string[0]),
								EcoLanguage.getMsg(LangRes.Rpt_BillALL, new string[0]),
								"",
								"",
								"",
								"",
								"",
								num.ToString("F4"),
								"",
								num2.ToString("F2")
							});
						}
					}
				}
			}
		}
		private System.Collections.ArrayList getAnalysisGroup(BillingRptPara pPara)
		{
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			foreach (ListViewItem listViewItem in pPara.Grouplist)
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
				string text4 = listViewItem.SubItems[1].Text;
				string text5 = "0,";
				string text6 = "0,";
				string text7 = "0,";
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string key;
				switch (key = text3)
				{
				case "zone":
				{
					string str_sql = "select racks from zone_info where id in (" + text2 + ") and racks <>''";
					DataTable dataTable = DBTools.CreateDataTable4SysDB(str_sql);
					text7 = "0,";
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string str_sql2 = "select id as device_ids from device_base_info where rack_id in (" + System.Convert.ToString(dataTable.Rows[i]["racks"]) + ") ";
						DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
						string text8 = "";
						stringBuilder = new System.Text.StringBuilder();
						for (int j = 0; j < dataTable2.Rows.Count; j++)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[j]["device_ids"]) + ",");
						}
						text8 += stringBuilder.ToString();
						if (text8.Length > 0)
						{
							text8 = text8.Substring(0, text8.Length - 1);
						}
						text7 = text7 + text8 + ",";
					}
					text7 = commUtil.uniqueIDs(text7);
					if (text7.Length > 0)
					{
						text5 = text7.Substring(0, text7.Length - 1);
					}
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					break;
				}
				case "rack":
				case "allrack":
				{
					string str_sql2 = "select id as device_ids from device_base_info where rack_id in (" + text2 + ")";
					DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
					stringBuilder = new System.Text.StringBuilder();
					for (int k = 0; k < dataTable2.Rows.Count; k++)
					{
						stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[k]["device_ids"]) + ",");
					}
					text7 += stringBuilder.ToString();
					if (text7.Length > 0)
					{
						text5 = text7.Substring(0, text7.Length - 1);
					}
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					break;
				}
				case "dev":
				case "alldev":
				{
					string str_sql2 = "select id,model_nm from device_base_info where id in (" + text2 + ")";
					DataTable dataTable2 = DBTools.CreateDataTable4SysDB(str_sql2);
					stringBuilder = new System.Text.StringBuilder();
					for (int l = 0; l < dataTable2.Rows.Count; l++)
					{
						stringBuilder.Append(System.Convert.ToString(dataTable2.Rows[l]["id"]) + ",");
					}
					text5 += stringBuilder.ToString();
					if (text5.Length > 0)
					{
						text5 = text5.Substring(0, text5.Length - 1);
					}
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					break;
				}
				case "alloutlet":
				{
					text5 = "0";
					string str_sql3 = "select a.id,b.model_nm,b.fw_version from port_info a  left join device_base_info b on a.device_id=b.id where a.id in (select id from port_info)";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int m = 0; m < dataTable3.Rows.Count; m++)
					{
						string modelname = System.Convert.ToString(dataTable3.Rows[m]["model_nm"]);
						string fmwareVer = System.Convert.ToString(dataTable3.Rows[m]["fw_version"]);
						if (DevAccessCfg.GetInstance().getDeviceModelConfig(modelname, fmwareVer).perportreading == 2)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[m]["id"]) + ",");
						}
					}
					text6 += stringBuilder.ToString();
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					break;
				}
				case "outlet":
				{
					text5 = "0";
					string str_sql3 = "select a.id,b.model_nm,b.fw_version from port_info a  left join device_base_info b on a.device_id=b.id where a.id in (select dest_id from group_detail where grouptype='outlet' and group_id=" + text + ")";
					DataTable dataTable3 = DBTools.CreateDataTable4SysDB(str_sql3);
					stringBuilder = new System.Text.StringBuilder();
					for (int n = 0; n < dataTable3.Rows.Count; n++)
					{
						string modelname2 = System.Convert.ToString(dataTable3.Rows[n]["model_nm"]);
						string fmwareVer2 = System.Convert.ToString(dataTable3.Rows[n]["fw_version"]);
						if (DevAccessCfg.GetInstance().getDeviceModelConfig(modelname2, fmwareVer2).perportreading == 2)
						{
							stringBuilder.Append(System.Convert.ToString(dataTable3.Rows[n]["id"]) + ",");
						}
					}
					text6 += stringBuilder.ToString();
					if (text6.Length > 0)
					{
						text6 = text6.Substring(0, text6.Length - 1);
					}
					break;
				}
				}
				arrayList.Add(string.Concat(new string[]
				{
					text5,
					"|",
					text,
					"|",
					text4,
					"|",
					text3.ToString(),
					"|",
					text6
				}));
			}
			return arrayList;
		}
		private int filldata(DataTable data, string groupname, long rackID, string showmonth, string strBegin, string strEnd, string device_id, string portid, int BillrateType, float Bill_1rate, float Bill_2rate1, float Bill_2rate2, ref double totalKWH, ref double totalFee)
		{
			string text = "";
			RackInfo rackInfo = null;
			if (rackID > 0L)
			{
				rackInfo = RackInfo.getRackByID(rackID);
				text = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			}
			float voltage = DeviceOperation.GetVoltage(device_id, portid);
			string text2 = "N/A";
			string text3 = "N/A";
			if ((double)voltage > 0.01)
			{
				text2 = voltage.ToString("F2") + "V";
			}
			System.Collections.Generic.List<BillReportInfo> billReportInfo = DBTools.GetBillReportInfo(strBegin, strEnd, device_id, portid);
			double num = 0.0;
			double num2 = 0.0;
			foreach (BillReportInfo current in billReportInfo)
			{
				double num3 = (double)current.KWH / 10000.0;
				double num4 = num3 / (double)current.TIMESPAN_HOUR;
				if ((double)voltage > 0.01)
				{
					text3 = (num4 * 1000.0 / (double)voltage).ToString("F2") + "A";
				}
				if (BillrateType == 0)
				{
					float num5 = Bill_1rate;
					double num6 = num3 * (double)num5;
					num2 += num6;
					totalFee += num6;
					totalKWH += num3;
					if (rackInfo == null)
					{
						data.Rows.Add(new object[]
						{
							groupname,
							showmonth,
							text3,
							text2,
							num4.ToString("F2"),
							num3.ToString("F4"),
							num5.ToString("F2"),
							num6.ToString("F2")
						});
					}
					else
					{
						data.Rows.Add(new object[]
						{
							groupname,
							text,
							showmonth,
							text3,
							text2,
							num4.ToString("F2"),
							num3.ToString("F4"),
							num5.ToString("F2"),
							num6.ToString("F2")
						});
					}
				}
				else
				{
					float num5;
					double num6;
					string msg;
					if (current.PERIOD.Equals("PEAK"))
					{
						msg = EcoLanguage.getMsg(LangRes.Rpt_BillPEAK, new string[0]);
						num5 = Bill_2rate1;
						num6 = num3 * (double)num5;
						num2 += num6;
						totalFee += num6;
					}
					else
					{
						msg = EcoLanguage.getMsg(LangRes.Rpt_BillnonPEAK, new string[0]);
						num5 = Bill_2rate2;
						num6 = num3 * (double)num5;
						num2 += num6;
						totalFee += num6;
					}
					num += num3;
					totalKWH += num3;
					if (rackInfo == null)
					{
						data.Rows.Add(new object[]
						{
							groupname,
							showmonth,
							msg,
							text3,
							text2,
							num4.ToString("F2"),
							num3.ToString("F4"),
							num5.ToString("F2"),
							num6.ToString("F2")
						});
					}
					else
					{
						data.Rows.Add(new object[]
						{
							groupname,
							text,
							showmonth,
							msg,
							text3,
							text2,
							num4.ToString("F2"),
							num3.ToString("F4"),
							num5.ToString("F2"),
							num6.ToString("F2")
						});
					}
				}
				groupname = "";
				showmonth = "";
				text = "";
			}
			if (BillrateType == 1 && billReportInfo.Count > 0)
			{
				string msg = EcoLanguage.getMsg(LangRes.Rpt_BillALL, new string[0]);
				if (rackInfo == null)
				{
					data.Rows.Add(new object[]
					{
						groupname,
						showmonth,
						msg,
						"",
						"",
						"",
						num.ToString("F4"),
						"",
						num2.ToString("F2")
					});
				}
				else
				{
					data.Rows.Add(new object[]
					{
						groupname,
						text,
						showmonth,
						msg,
						"",
						"",
						"",
						num.ToString("F4"),
						"",
						num2.ToString("F2")
					});
				}
			}
			if (billReportInfo.Count > 0)
			{
				return 1;
			}
			return 0;
		}
	}
}
