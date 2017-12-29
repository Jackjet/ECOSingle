using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.EnegManPage.DashBoard;
using PDFjet.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace EcoSensors.EnegManPage.AnalysisThermal
{
	public class ThermGenRptShow : UserControl
	{
		private ThermAnalysis m_parent;
		private ThermGenRptPara m_pParaWindow;
		private bool m_isRPTSaved;
		private Sys_Para m_syspara;
		private System.Drawing.Color[] m_lineColor;
		private System.Text.StringBuilder csv_chart1 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart2 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart3 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart4 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart5 = new System.Text.StringBuilder();
		private IContainer components;
		private Button btnPreview;
		private Button btnSave;
		private TabPage tpline;
		private Chart chart1;
		private TabControl tabchart;
		public ThermGenRptShow()
		{
			this.InitializeComponent();
			this.m_lineColor = new System.Drawing.Color[4];
			this.m_lineColor[0] = System.Drawing.Color.Red;
			this.m_lineColor[1] = System.Drawing.Color.Blue;
			this.m_lineColor[2] = System.Drawing.Color.Orange;
			this.m_lineColor[3] = System.Drawing.Color.FromArgb(162, 215, 48);
		}
		public void pageInit(ThermAnalysis parent, ThermGenRptPara pPara)
		{
			this.m_parent = parent;
			this.m_pParaWindow = pPara;
			this.m_isRPTSaved = false;
			this.m_syspara = new Sys_Para();
			this.chart1.Height = 55;
			this.chart1.Legends.Clear();
			this.chart1.Series.Clear();
			this.csv_chart1 = new System.Text.StringBuilder();
			this.csv_chart2 = new System.Text.StringBuilder();
			this.csv_chart3 = new System.Text.StringBuilder();
			this.csv_chart4 = new System.Text.StringBuilder();
			this.csv_chart5 = new System.Text.StringBuilder();
			this.CreateChart();
		}
		private void CreateChart()
		{
			this.tabchart.TabPages.Clear();
			System.DateTime dateTime = default(System.DateTime);
			System.DateTime t = default(System.DateTime);
			System.DateTime dateTime2 = default(System.DateTime);
			string text = "";
			string text2 = "";
			string txttitle = this.m_pParaWindow.Txttitle;
			string dtptime = this.m_pParaWindow.Dtptime;
			string txtwriter = this.m_pParaWindow.Txtwriter;
			string text3 = "";
			string text4 = "";
			string tablePrefix = "rackthermal_daily";
			string text5 = "rci_daily";
			string str = "";
			string format = "yyyy-MM-dd";
			double interval = 1.0;
			DateTimeIntervalType intervalType = DateTimeIntervalType.Days;
			int num = 1;
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				num = 2;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.Dictionary<string, double> dictionary = new System.Collections.Generic.Dictionary<string, double>();
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			switch (this.m_pParaWindow.Cboperiod_SelectedIndex)
			{
			case 0:
			{
				dateTime = System.Convert.ToDateTime(this.m_pParaWindow.BeginText + ":0:0");
				t = dateTime.AddHours((double)this.m_pParaWindow.Cboduration);
				dateTime2 = dateTime.AddHours(-1.0);
				text4 = EcoLanguage.getMsg(LangRes.Rpt_shfromHourly, new string[0]) + " " + txtwriter;
				if (num == 1)
				{
					text3 = "FORMAT(insert_time, 'yyyy-MM-dd HH')";
				}
				else
				{
					text3 = "date_format(insert_time, '%Y-%m-%d %H')";
				}
				tablePrefix = "rackthermal_hourly";
				text5 = "rci_hourly";
				text = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
				text2 = t.ToString("yyyy-MM-dd HH:mm:ss");
				str = ":0:0";
				format = "yyyy-MM-dd HH";
				intervalType = DateTimeIntervalType.Hours;
				System.DateTime t2 = dateTime;
				while (t2 < t)
				{
					string text6 = t2.ToString(format);
					stringBuilder.Append("\t\"" + text6 + "\"");
					dictionary.Add(text6, -100.0);
					list.Add(text6);
					t2 = t2.AddHours(1.0);
				}
				break;
			}
			case 1:
			{
				dateTime = System.Convert.ToDateTime(this.m_pParaWindow.BeginText);
				t = dateTime.AddDays((double)this.m_pParaWindow.Cboduration);
				dateTime2 = dateTime.AddDays(-1.0);
				text4 = EcoLanguage.getMsg(LangRes.Rpt_shfromDaily, new string[0]) + " " + txtwriter;
				if (num == 1)
				{
					text3 = "FORMAT(insert_time, 'yyyy-MM-dd')";
				}
				else
				{
					text3 = "date_format(insert_time, '%Y-%m-%d')";
				}
				tablePrefix = "rackthermal_daily";
				text5 = "rci_daily";
				text = dateTime.ToString("yyyy-MM-dd");
				text2 = t.ToString("yyyy-MM-dd");
				format = "yyyy-MM-dd";
				intervalType = DateTimeIntervalType.Days;
				System.DateTime t2 = dateTime;
				while (t2 < t)
				{
					string text7 = t2.ToString(format);
					stringBuilder.Append("\t\"" + text7 + "\"");
					dictionary.Add(text7, -100.0);
					list.Add(text7);
					t2 = t2.AddDays(1.0);
				}
				break;
			}
			case 2:
			{
				dateTime = System.Convert.ToDateTime(this.m_pParaWindow.BeginText);
				t = dateTime.AddMonths(this.m_pParaWindow.Cboduration);
				dateTime2 = dateTime.AddMonths(-1);
				text4 = EcoLanguage.getMsg(LangRes.Rpt_shfromMonthly, new string[0]) + " " + txtwriter;
				if (num == 1)
				{
					text3 = "FORMAT(insert_time, 'yyyy-MM')";
				}
				else
				{
					text3 = "date_format(insert_time, '%Y-%m')";
				}
				tablePrefix = "rackthermal_daily";
				text5 = "rci_daily";
				text = dateTime.ToString("yyyy-MM-dd");
				text2 = t.ToString("yyyy-MM-dd");
				format = "yyyy-MM";
				intervalType = DateTimeIntervalType.Months;
				System.DateTime t2 = dateTime;
				while (t2 < t)
				{
					string text8 = t2.ToString(format);
					stringBuilder.Append("\t\"" + text8 + "\"");
					dictionary.Add(text8, -100.0);
					list.Add(text8);
					t2 = t2.AddMonths(1);
				}
				break;
			}
			case 3:
			{
				dateTime = System.Convert.ToDateTime(this.m_pParaWindow.BeginText + "-01");
				t = dateTime.AddMonths(this.m_pParaWindow.Cboduration * 3);
				dateTime2 = dateTime.AddMonths(-3);
				text4 = EcoLanguage.getMsg(LangRes.Rpt_shfromQuarterly, new string[0]) + " " + txtwriter;
				if (num == 1)
				{
					text3 = "FORMAT(insert_time, 'yyyy')+'Q'+FORMAT(insert_time, 'q')";
				}
				else
				{
					text3 = "concat(date_format(insert_time, '%Y'),'Q',quarter(insert_time))";
				}
				tablePrefix = "rackthermal_daily";
				text5 = "rci_daily";
				text = dateTime.ToString("yyyy-MM-dd");
				text2 = t.ToString("yyyy-MM-dd");
				format = "yyyy-MM";
				interval = 3.0;
				intervalType = DateTimeIntervalType.Months;
				System.DateTime t2 = dateTime;
				while (t2 < t)
				{
					string text9 = t2.ToString(format);
					stringBuilder.Append("\t\"" + text9 + "\"");
					dictionary.Add(text9, -100.0);
					list.Add(text9);
					t2 = t2.AddMonths(3);
				}
				break;
			}
			}
			stringBuilder.AppendLine();
			dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			foreach (ListViewItem listViewItem in this.m_pParaWindow.Grouplist)
			{
				string text10 = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[1];
				if (text10.Length == 0)
				{
					text10 = "0";
				}
				string text11 = System.Convert.ToString(listViewItem.Tag).Split(new char[]
				{
					'|'
				})[2];
				string text12 = listViewItem.SubItems[1].Text;
				string text13 = "0,";
				int num2 = 0;
				string a;
				if ((a = text11) != null)
				{
					if (!(a == "zone"))
					{
						if (a == "rack" || a == "allrack")
						{
							text13 = text10;
							num2 = 1;
						}
					}
					else
					{
						string str_sql = "select racks from zone_info where id in (" + text10 + ") and racks <>''";
						DataTable dataTable = DBTools.CreateDataTable4SysDB(str_sql);
						text13 = "0,";
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							text13 = text13 + System.Convert.ToString(dataTable.Rows[i]["racks"]) + ",";
						}
						text13 = commUtil.uniqueIDs(text13);
						if (text13.Length > 0)
						{
							text13 = text13.Substring(0, text13.Length - 1);
						}
						num2 = 0;
					}
				}
				arrayList.Add(string.Concat(new string[]
				{
					text13,
					"|",
					text12,
					"|",
					num2.ToString()
				}));
			}
			if (this.m_pParaWindow.chkchart1_Checked() || this.m_pParaWindow.chkchart2_Checked() || this.m_pParaWindow.chkchart3_Checked() || this.m_pParaWindow.chkchart4_Checked() || this.m_pParaWindow.chkchart5_Checked())
			{
				bool isVisibleInLegend = true;
				this.tabchart.TabPages.Add(this.tpline);
				DataTable dataTable2 = new DataTable();
				this.chart1.Titles[0].Text = EcoLanguage.getMsg(LangRes.Rpt_shdate, new string[0]) + ":" + dtptime;
				this.chart1.Titles[1].Text = txttitle;
				this.chart1.Titles[2].Text = text4;
				this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
				this.chart1.Titles[1].Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, FontStyle.Regular);
				this.chart1.Titles[2].Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
				int num3 = 0;
				int[] array = new int[5];
				if (this.m_pParaWindow.chkchart1_Checked())
				{
					num3++;
					array[0] = num3;
				}
				else
				{
					array[0] = 0;
				}
				if (this.m_pParaWindow.chkchart2_Checked())
				{
					num3++;
					array[1] = num3;
				}
				else
				{
					array[1] = 0;
				}
				if (this.m_pParaWindow.chkchart3_Checked())
				{
					num3++;
					array[2] = num3;
				}
				else
				{
					array[2] = 0;
				}
				if (this.m_pParaWindow.chkchart4_Checked())
				{
					num3++;
					array[3] = num3;
				}
				else
				{
					array[3] = 0;
				}
				if (this.m_pParaWindow.chkchart5_Checked())
				{
					num3++;
					array[4] = num3;
				}
				else
				{
					array[4] = 0;
				}
				float num4 = (float)(80 + 400 * num3 + 70);
				float height = 40000f / num4;
				this.chart1.Titles[0].Position.Y = 600f / num4;
				this.chart1.Titles[1].Position.Y = 2400f / num4;
				this.chart1.Titles[1].Position.Height = 7000f / num4;
				this.chart1.Titles[2].Position.Y = 5000f / num4;
				this.chart1.Height = (int)num4;
				if (this.m_pParaWindow.chkchart1_Checked())
				{
					this.chart1.ChartAreas[0].Visible = true;
					this.chart1.ChartAreas[0].Position.X = 2f;
					this.chart1.ChartAreas[0].Position.Y = (float)((80 + 400 * (array[0] - 1)) * 100) / num4;
					this.chart1.ChartAreas[0].Position.Height = height;
					this.chart1.ChartAreas[0].Position.Width = 95f;
					this.chart1.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_ThmIntakePeak, new string[0]);
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.chart1.ChartAreas[0].AxisY.Title = "°C";
					}
					else
					{
						this.chart1.ChartAreas[0].AxisY.Title = "°F";
					}
					this.chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = format;
					this.chart1.ChartAreas[0].AxisX.Interval = interval;
					this.chart1.ChartAreas[0].AxisX.IntervalType = intervalType;
					this.chart1.ChartAreas[0].AxisX.Minimum = dateTime2.ToOADate();
					this.chart1.ChartAreas[0].AxisX.Maximum = t.ToOADate();
					Legend legend = new Legend();
					legend.Alignment = StringAlignment.Far;
					legend.Docking = Docking.Top;
					legend.LegendStyle = LegendStyle.Table;
					legend.BorderWidth = 2;
					legend.Name = "leg1";
					legend.DockedToChartArea = "ChartArea1";
					legend.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend);
					this.csv_chart1.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_shGroupName, new string[0]) + "\"");
					this.csv_chart1.Append(stringBuilder.ToString());
					for (int j = 0; j < arrayList.Count; j++)
					{
						System.Drawing.Color color = this.m_lineColor[j];
						string rack_ids = arrayList[j].ToString().Split(new char[]
						{
							'|'
						})[0];
						string text14 = arrayList[j].ToString().Split(new char[]
						{
							'|'
						})[1];
						for (int k = 0; k < list.Count; k++)
						{
							dictionary[list[k]] = -100.0;
						}
						this.csv_chart1.Append("\"" + text14 + "\"");
						dataTable2 = MultiThreadQuery.GetThermalData(1, text, text2, rack_ids, text3, tablePrefix);
						foreach (DataRow dataRow in dataTable2.Rows)
						{
							try
							{
								double num5 = System.Convert.ToDouble(dataRow["intakepeak"]);
								if (EcoGlobalVar.TempUnit == 1)
								{
									num5 = RackStatusAll.CtoFdegrees(num5);
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									string key = dataRow["period"].ToString();
									if (dictionary.ContainsKey(key))
									{
										dictionary[key] = num5;
									}
								}
								else
								{
									string text15 = dataRow["period"].ToString();
									int num6 = System.Convert.ToInt32(text15.Split(new char[]
									{
										'Q'
									})[1]);
									num6 = 3 * (num6 - 1) + 1;
									text15 = text15.Split(new char[]
									{
										'Q'
									})[0] + "-" + num6.ToString("D2");
									string key = text15;
									if (dictionary.ContainsKey(key))
									{
										dictionary[key] = num5;
									}
								}
							}
							catch
							{
							}
						}
						Series series = null;
						bool flag = false;
						for (int l = 0; l < list.Count; l++)
						{
							if (dictionary[list[l]] > -5.0)
							{
								if (series == null)
								{
									series = new Series();
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									series.Points.AddXY(System.DateTime.Parse(list[l] + str), new object[]
									{
										dictionary[list[l]]
									});
								}
								else
								{
									series.Points.AddXY(System.DateTime.Parse(list[l]), new object[]
									{
										dictionary[list[l]]
									});
								}
							}
							else
							{
								if (series != null)
								{
									series.XValueType = ChartValueType.DateTime;
									series.ChartArea = "ChartArea1";
									series.ChartType = SeriesChartType.Line;
									series.MarkerStyle = MarkerStyle.Circle;
									series.MarkerSize = 3;
									series.Color = color;
									series.LegendText = text14;
									series.IsValueShownAsLabel = true;
									series.LabelFormat = "F2";
									series.Legend = "leg1";
									if (!flag)
									{
										series.IsVisibleInLegend = isVisibleInLegend;
									}
									else
									{
										series.IsVisibleInLegend = false;
									}
									series.BorderWidth = 1;
									flag = true;
									this.chart1.Series.Add(series);
								}
								series = null;
							}
						}
						if (series != null || !flag)
						{
							if (series == null)
							{
								series = new Series();
							}
							series.XValueType = ChartValueType.DateTime;
							series.ChartArea = "ChartArea1";
							series.ChartType = SeriesChartType.Line;
							series.MarkerStyle = MarkerStyle.Circle;
							series.MarkerSize = 3;
							series.Color = color;
							series.LegendText = text14;
							series.IsValueShownAsLabel = true;
							series.LabelFormat = "F2";
							series.Legend = "leg1";
							if (!flag)
							{
								series.IsVisibleInLegend = isVisibleInLegend;
							}
							else
							{
								series.IsVisibleInLegend = false;
							}
							series.BorderWidth = 1;
							this.chart1.Series.Add(series);
						}
						for (int m = 0; m < list.Count; m++)
						{
							if (dictionary[list[m]] > -5.0)
							{
								this.csv_chart1.Append("\t\"" + dictionary[list[m]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart1.Append("\t\"NA\"");
							}
						}
						this.csv_chart1.AppendLine();
					}
					isVisibleInLegend = false;
				}
				else
				{
					this.chart1.ChartAreas[0].Visible = false;
				}
				if (this.m_pParaWindow.chkchart2_Checked())
				{
					this.chart1.ChartAreas[1].Visible = true;
					this.chart1.ChartAreas[1].Position.X = 2f;
					this.chart1.ChartAreas[1].Position.Y = (float)((80 + 420 * (array[1] - 1)) * 100) / num4;
					this.chart1.ChartAreas[1].Position.Height = height;
					this.chart1.ChartAreas[1].Position.Width = 95f;
					this.chart1.ChartAreas[1].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_ThmExhaustPeak, new string[0]);
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.chart1.ChartAreas[1].AxisY.Title = "°C";
					}
					else
					{
						this.chart1.ChartAreas[1].AxisY.Title = "°F";
					}
					this.chart1.ChartAreas[1].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[1].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[1].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[1].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[1].AxisX.LabelStyle.Format = format;
					this.chart1.ChartAreas[1].AxisX.Interval = interval;
					this.chart1.ChartAreas[1].AxisX.IntervalType = intervalType;
					this.chart1.ChartAreas[1].AxisX.Minimum = dateTime2.ToOADate();
					this.chart1.ChartAreas[1].AxisX.Maximum = t.ToOADate();
					Legend legend2 = new Legend();
					legend2.Alignment = StringAlignment.Far;
					legend2.Docking = Docking.Top;
					legend2.LegendStyle = LegendStyle.Table;
					legend2.Name = "leg2";
					legend2.DockedToChartArea = "ChartArea2";
					legend2.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend2);
					this.csv_chart2.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_shGroupName, new string[0]) + "\"");
					this.csv_chart2.Append(stringBuilder.ToString());
					for (int n = 0; n < arrayList.Count; n++)
					{
						System.Drawing.Color color2 = this.m_lineColor[n];
						string rack_ids2 = arrayList[n].ToString().Split(new char[]
						{
							'|'
						})[0];
						string text16 = arrayList[n].ToString().Split(new char[]
						{
							'|'
						})[1];
						for (int num7 = 0; num7 < list.Count; num7++)
						{
							dictionary[list[num7]] = -100.0;
						}
						this.csv_chart2.Append("\"" + text16 + "\"");
						dataTable2 = MultiThreadQuery.GetThermalData(2, text, text2, rack_ids2, text3, tablePrefix);
						foreach (DataRow dataRow2 in dataTable2.Rows)
						{
							try
							{
								double num8 = System.Convert.ToDouble(dataRow2["exhaustpeak"]);
								if (EcoGlobalVar.TempUnit == 1)
								{
									num8 = RackStatusAll.CtoFdegrees(num8);
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									string key2 = dataRow2["period"].ToString();
									if (dictionary.ContainsKey(key2))
									{
										dictionary[key2] = num8;
									}
								}
								else
								{
									string text17 = dataRow2["period"].ToString();
									int num9 = System.Convert.ToInt32(text17.Split(new char[]
									{
										'Q'
									})[1]);
									num9 = 3 * (num9 - 1) + 1;
									text17 = text17.Split(new char[]
									{
										'Q'
									})[0] + "-" + num9.ToString("D2");
									string key2 = text17;
									if (dictionary.ContainsKey(key2))
									{
										dictionary[key2] = num8;
									}
								}
							}
							catch
							{
							}
						}
						Series series2 = null;
						bool flag2 = false;
						for (int num10 = 0; num10 < list.Count; num10++)
						{
							if (dictionary[list[num10]] > -5.0)
							{
								if (series2 == null)
								{
									series2 = new Series();
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									series2.Points.AddXY(System.DateTime.Parse(list[num10] + str), new object[]
									{
										dictionary[list[num10]]
									});
								}
								else
								{
									series2.Points.AddXY(System.DateTime.Parse(list[num10]), new object[]
									{
										dictionary[list[num10]]
									});
								}
							}
							else
							{
								if (series2 != null)
								{
									series2.XValueType = ChartValueType.DateTime;
									series2.ChartArea = "ChartArea2";
									series2.ChartType = SeriesChartType.Line;
									series2.MarkerStyle = MarkerStyle.Circle;
									series2.MarkerSize = 3;
									series2.Color = color2;
									series2.LegendText = text16;
									series2.IsValueShownAsLabel = true;
									series2.LabelFormat = "F2";
									series2.Legend = "leg2";
									if (!flag2)
									{
										series2.IsVisibleInLegend = isVisibleInLegend;
									}
									else
									{
										series2.IsVisibleInLegend = false;
									}
									series2.BorderWidth = 1;
									flag2 = true;
									this.chart1.Series.Add(series2);
								}
								series2 = null;
							}
						}
						if (series2 != null || !flag2)
						{
							if (series2 == null)
							{
								series2 = new Series();
							}
							series2.XValueType = ChartValueType.DateTime;
							series2.ChartArea = "ChartArea2";
							series2.ChartType = SeriesChartType.Line;
							series2.MarkerStyle = MarkerStyle.Circle;
							series2.MarkerSize = 3;
							series2.Color = color2;
							series2.LegendText = text16;
							series2.IsValueShownAsLabel = true;
							series2.LabelFormat = "F2";
							series2.Legend = "leg2";
							if (!flag2)
							{
								series2.IsVisibleInLegend = isVisibleInLegend;
							}
							else
							{
								series2.IsVisibleInLegend = false;
							}
							series2.BorderWidth = 1;
							this.chart1.Series.Add(series2);
						}
						for (int num11 = 0; num11 < list.Count; num11++)
						{
							if (dictionary[list[num11]] > -5.0)
							{
								this.csv_chart2.Append("\t\"" + dictionary[list[num11]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart2.Append("\t\"NA\"");
							}
						}
						this.csv_chart2.AppendLine();
					}
					isVisibleInLegend = false;
				}
				else
				{
					this.chart1.ChartAreas[1].Visible = false;
				}
				if (this.m_pParaWindow.chkchart3_Checked())
				{
					this.chart1.ChartAreas[2].Visible = true;
					this.chart1.ChartAreas[2].Position.X = 2f;
					this.chart1.ChartAreas[2].Position.Y = (float)((80 + 420 * (array[2] - 1)) * 100) / num4;
					this.chart1.ChartAreas[2].Position.Height = height;
					this.chart1.ChartAreas[2].Position.Width = 95f;
					this.chart1.ChartAreas[2].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_ThmDiffPeak, new string[0]);
					if (EcoGlobalVar.TempUnit == 0)
					{
						this.chart1.ChartAreas[2].AxisY.Title = "°C";
					}
					else
					{
						this.chart1.ChartAreas[2].AxisY.Title = "°F";
					}
					this.chart1.ChartAreas[2].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[2].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[2].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[2].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[2].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[2].AxisX.LabelStyle.Format = "yyyy-MM-dd";
					this.chart1.ChartAreas[2].AxisX.Interval = 1.0;
					this.chart1.ChartAreas[2].AxisX.LabelStyle.Format = format;
					this.chart1.ChartAreas[2].AxisX.Interval = interval;
					this.chart1.ChartAreas[2].AxisX.IntervalType = intervalType;
					this.chart1.ChartAreas[2].AxisX.Minimum = dateTime2.ToOADate();
					this.chart1.ChartAreas[2].AxisX.Maximum = t.ToOADate();
					Legend legend3 = new Legend();
					legend3.Alignment = StringAlignment.Far;
					legend3.Docking = Docking.Top;
					legend3.LegendStyle = LegendStyle.Table;
					legend3.Name = "leg3";
					legend3.DockedToChartArea = "ChartArea3";
					legend3.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend3);
					this.csv_chart3.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_shGroupName, new string[0]) + "\"");
					this.csv_chart3.Append(stringBuilder.ToString());
					for (int num12 = 0; num12 < arrayList.Count; num12++)
					{
						System.Drawing.Color color3 = this.m_lineColor[num12];
						string rack_ids3 = arrayList[num12].ToString().Split(new char[]
						{
							'|'
						})[0];
						string text18 = arrayList[num12].ToString().Split(new char[]
						{
							'|'
						})[1];
						for (int num13 = 0; num13 < list.Count; num13++)
						{
							dictionary[list[num13]] = -100.0;
						}
						this.csv_chart3.Append("\"" + text18 + "\"");
						dataTable2 = MultiThreadQuery.GetThermalData(3, text, text2, rack_ids3, text3, tablePrefix);
						foreach (DataRow dataRow3 in dataTable2.Rows)
						{
							try
							{
								double num14 = System.Convert.ToDouble(dataRow3["diffpeak"]);
								if (EcoGlobalVar.TempUnit == 1)
								{
									num14 = RackStatusAll.CtoFdegrees(num14);
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									string key3 = dataRow3["period"].ToString();
									if (dictionary.ContainsKey(key3))
									{
										dictionary[key3] = num14;
									}
								}
								else
								{
									string text19 = dataRow3["period"].ToString();
									int num15 = System.Convert.ToInt32(text19.Split(new char[]
									{
										'Q'
									})[1]);
									num15 = 3 * (num15 - 1) + 1;
									text19 = text19.Split(new char[]
									{
										'Q'
									})[0] + "-" + num15.ToString("D2");
									string key3 = text19;
									if (dictionary.ContainsKey(key3))
									{
										dictionary[key3] = num14;
									}
								}
							}
							catch
							{
							}
						}
						Series series3 = null;
						bool flag3 = false;
						for (int num16 = 0; num16 < list.Count; num16++)
						{
							if (dictionary[list[num16]] > -5.0)
							{
								if (series3 == null)
								{
									series3 = new Series();
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									series3.Points.AddXY(System.DateTime.Parse(list[num16] + str), new object[]
									{
										dictionary[list[num16]]
									});
								}
								else
								{
									series3.Points.AddXY(System.DateTime.Parse(list[num16]), new object[]
									{
										dictionary[list[num16]]
									});
								}
							}
							else
							{
								if (series3 != null)
								{
									series3.XValueType = ChartValueType.DateTime;
									series3.ChartArea = "ChartArea3";
									series3.ChartType = SeriesChartType.Line;
									series3.MarkerStyle = MarkerStyle.Circle;
									series3.MarkerSize = 3;
									series3.Color = color3;
									series3.LegendText = text18;
									series3.IsValueShownAsLabel = true;
									series3.LabelFormat = "F2";
									series3.Legend = "leg3";
									if (!flag3)
									{
										series3.IsVisibleInLegend = isVisibleInLegend;
									}
									else
									{
										series3.IsVisibleInLegend = false;
									}
									series3.BorderWidth = 1;
									flag3 = true;
									this.chart1.Series.Add(series3);
								}
								series3 = null;
							}
						}
						if (series3 != null || !flag3)
						{
							if (series3 == null)
							{
								series3 = new Series();
							}
							series3.XValueType = ChartValueType.DateTime;
							series3.ChartArea = "ChartArea3";
							series3.ChartType = SeriesChartType.Line;
							series3.MarkerStyle = MarkerStyle.Circle;
							series3.MarkerSize = 3;
							series3.Color = color3;
							series3.LegendText = text18;
							series3.IsValueShownAsLabel = true;
							series3.LabelFormat = "F2";
							series3.Legend = "leg3";
							if (!flag3)
							{
								series3.IsVisibleInLegend = isVisibleInLegend;
							}
							else
							{
								series3.IsVisibleInLegend = false;
							}
							series3.BorderWidth = 1;
							this.chart1.Series.Add(series3);
						}
						for (int num17 = 0; num17 < list.Count; num17++)
						{
							if (dictionary[list[num17]] > -5.0)
							{
								this.csv_chart3.Append("\t\"" + dictionary[list[num17]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart3.Append("\t\"NA\"");
							}
						}
						this.csv_chart3.AppendLine();
					}
					isVisibleInLegend = false;
				}
				else
				{
					this.chart1.ChartAreas[2].Visible = false;
				}
				if (this.m_pParaWindow.chkchart4_Checked())
				{
					this.chart1.ChartAreas[3].Visible = true;
					this.chart1.ChartAreas[3].Position.X = 2f;
					this.chart1.ChartAreas[3].Position.Y = (float)((80 + 420 * (array[3] - 1)) * 100) / num4;
					this.chart1.ChartAreas[3].Position.Height = height;
					this.chart1.ChartAreas[3].Position.Width = 95f;
					this.chart1.ChartAreas[3].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_ThmRCIHi, new string[0]);
					this.chart1.ChartAreas[3].AxisY.Title = "%";
					this.chart1.ChartAreas[3].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[3].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[3].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[3].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[3].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[3].AxisX.LabelStyle.Format = format;
					this.chart1.ChartAreas[3].AxisX.Interval = interval;
					this.chart1.ChartAreas[3].AxisX.IntervalType = intervalType;
					this.chart1.ChartAreas[3].AxisX.Minimum = dateTime2.ToOADate();
					this.chart1.ChartAreas[3].AxisX.Maximum = t.ToOADate();
					Legend legend4 = new Legend();
					legend4.Alignment = StringAlignment.Far;
					legend4.Docking = Docking.Top;
					legend4.LegendStyle = LegendStyle.Table;
					legend4.Name = "leg4";
					legend4.DockedToChartArea = "ChartArea4";
					legend4.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend4);
					this.csv_chart4.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_ThmRCIHi, new string[0]) + "\"");
					this.csv_chart4.Append(stringBuilder.ToString());
					for (int num18 = 0; num18 < 1; num18++)
					{
						System.Drawing.Color color4 = this.m_lineColor[num18];
						string legendText = arrayList[num18].ToString().Split(new char[]
						{
							'|'
						})[1];
						for (int num19 = 0; num19 < list.Count; num19++)
						{
							dictionary[list[num19]] = -100.0;
						}
						this.csv_chart4.Append("\"Index Hi\"");
						string str_sql2 = string.Concat(new string[]
						{
							"select * from (select min(rci_high) as rci_hi,",
							text3,
							" as period from ",
							text5,
							" where insert_time >=#",
							text,
							"# and  insert_time <#",
							text2,
							"# group by ",
							text3,
							") as T order by period ASC"
						});
						dataTable2 = DBTools.CreateDataTable4ThermalDB(str_sql2);
						foreach (DataRow dataRow4 in dataTable2.Rows)
						{
							try
							{
								double value = System.Convert.ToDouble(dataRow4["rci_hi"]);
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									string key4 = dataRow4["period"].ToString();
									if (dictionary.ContainsKey(key4))
									{
										dictionary[key4] = value;
									}
								}
								else
								{
									string text20 = dataRow4["period"].ToString();
									int num20 = System.Convert.ToInt32(text20.Split(new char[]
									{
										'Q'
									})[1]);
									num20 = 3 * (num20 - 1) + 1;
									text20 = text20.Split(new char[]
									{
										'Q'
									})[0] + "-" + num20.ToString("D2");
									string key4 = text20;
									if (dictionary.ContainsKey(key4))
									{
										dictionary[key4] = value;
									}
								}
							}
							catch
							{
							}
						}
						Series series4 = null;
						bool flag4 = false;
						for (int num21 = 0; num21 < list.Count; num21++)
						{
							if (dictionary[list[num21]] > -5.0)
							{
								if (series4 == null)
								{
									series4 = new Series();
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									series4.Points.AddXY(System.DateTime.Parse(list[num21] + str), new object[]
									{
										dictionary[list[num21]]
									});
								}
								else
								{
									series4.Points.AddXY(System.DateTime.Parse(list[num21]), new object[]
									{
										dictionary[list[num21]]
									});
								}
							}
							else
							{
								if (series4 != null)
								{
									series4.XValueType = ChartValueType.DateTime;
									series4.ChartArea = "ChartArea4";
									series4.ChartType = SeriesChartType.Line;
									series4.MarkerStyle = MarkerStyle.Circle;
									series4.MarkerSize = 3;
									series4.Color = color4;
									series4.LegendText = legendText;
									series4.IsValueShownAsLabel = true;
									series4.LabelFormat = "N2";
									series4.Legend = "leg4";
									series4.IsVisibleInLegend = false;
									series4.BorderWidth = 1;
									flag4 = true;
									this.chart1.Series.Add(series4);
								}
								series4 = null;
							}
						}
						if (series4 != null || !flag4)
						{
							if (series4 == null)
							{
								series4 = new Series();
							}
							series4.XValueType = ChartValueType.DateTime;
							series4.ChartArea = "ChartArea4";
							series4.ChartType = SeriesChartType.Line;
							series4.MarkerStyle = MarkerStyle.Circle;
							series4.MarkerSize = 3;
							series4.Color = color4;
							series4.LegendText = legendText;
							series4.IsValueShownAsLabel = true;
							series4.LabelFormat = "N2";
							series4.Legend = "leg4";
							series4.IsVisibleInLegend = false;
							series4.BorderWidth = 1;
							this.chart1.Series.Add(series4);
						}
						for (int num22 = 0; num22 < list.Count; num22++)
						{
							if (dictionary[list[num22]] > -5.0)
							{
								this.csv_chart4.Append("\t\"" + dictionary[list[num22]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart4.Append("\t\"NA\"");
							}
						}
						this.csv_chart4.AppendLine();
					}
				}
				else
				{
					this.chart1.ChartAreas[3].Visible = false;
				}
				if (this.m_pParaWindow.chkchart5_Checked())
				{
					this.chart1.ChartAreas[4].Visible = true;
					this.chart1.ChartAreas[4].Position.X = 2f;
					this.chart1.ChartAreas[4].Position.Y = (float)((80 + 420 * (array[4] - 1)) * 100) / num4;
					this.chart1.ChartAreas[4].Position.Height = height;
					this.chart1.ChartAreas[4].Position.Width = 95f;
					this.chart1.ChartAreas[4].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_ThmRCILo, new string[0]);
					this.chart1.ChartAreas[4].AxisY.Title = "%";
					this.chart1.ChartAreas[4].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[4].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[4].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[4].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[4].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[4].AxisX.LabelStyle.Format = format;
					this.chart1.ChartAreas[4].AxisX.Interval = interval;
					this.chart1.ChartAreas[4].AxisX.IntervalType = intervalType;
					this.chart1.ChartAreas[4].AxisX.Minimum = dateTime2.ToOADate();
					this.chart1.ChartAreas[4].AxisX.Maximum = t.ToOADate();
					Legend legend5 = new Legend();
					legend5.Alignment = StringAlignment.Far;
					legend5.Docking = Docking.Top;
					legend5.LegendStyle = LegendStyle.Table;
					legend5.Name = "leg5";
					legend5.DockedToChartArea = "ChartArea5";
					legend5.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend5);
					this.csv_chart5.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_ThmRCILo, new string[0]) + "\"");
					this.csv_chart5.Append(stringBuilder.ToString());
					for (int num23 = 0; num23 < 1; num23++)
					{
						System.Drawing.Color color5 = this.m_lineColor[num23];
						string legendText2 = arrayList[num23].ToString().Split(new char[]
						{
							'|'
						})[1];
						for (int num24 = 0; num24 < list.Count; num24++)
						{
							dictionary[list[num24]] = -100.0;
						}
						this.csv_chart5.Append("\"Index Lo\"");
						string str_sql2 = string.Concat(new string[]
						{
							"select * from (select min(rci_lo) as rci_lo,",
							text3,
							" as period from ",
							text5,
							" a where insert_time >= #",
							text,
							"# and insert_time  < #",
							text2,
							"# group by ",
							text3,
							") as T order by period ASC"
						});
						dataTable2 = DBTools.CreateDataTable4ThermalDB(str_sql2);
						foreach (DataRow dataRow5 in dataTable2.Rows)
						{
							try
							{
								double value2 = System.Convert.ToDouble(dataRow5["rci_lo"]);
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									string key5 = dataRow5["period"].ToString();
									if (dictionary.ContainsKey(key5))
									{
										dictionary[key5] = value2;
									}
								}
								else
								{
									string text21 = dataRow5["period"].ToString();
									int num25 = System.Convert.ToInt32(text21.Split(new char[]
									{
										'Q'
									})[1]);
									num25 = 3 * (num25 - 1) + 1;
									text21 = text21.Split(new char[]
									{
										'Q'
									})[0] + "-" + num25.ToString("D2");
									string key5 = text21;
									if (dictionary.ContainsKey(key5))
									{
										dictionary[key5] = value2;
									}
								}
							}
							catch
							{
							}
						}
						Series series5 = null;
						bool flag5 = false;
						for (int num26 = 0; num26 < list.Count; num26++)
						{
							if (dictionary[list[num26]] > -5.0)
							{
								if (series5 == null)
								{
									series5 = new Series();
								}
								if (this.m_pParaWindow.Cboperiod_SelectedIndex != 3)
								{
									series5.Points.AddXY(System.DateTime.Parse(list[num26] + str), new object[]
									{
										dictionary[list[num26]]
									});
								}
								else
								{
									series5.Points.AddXY(System.DateTime.Parse(list[num26]), new object[]
									{
										dictionary[list[num26]]
									});
								}
							}
							else
							{
								if (series5 != null)
								{
									series5.XValueType = ChartValueType.DateTime;
									series5.ChartArea = "ChartArea5";
									series5.ChartType = SeriesChartType.Line;
									series5.MarkerStyle = MarkerStyle.Circle;
									series5.MarkerSize = 3;
									series5.Color = color5;
									series5.LegendText = legendText2;
									series5.IsValueShownAsLabel = true;
									series5.LabelFormat = "N2";
									series5.Legend = "leg5";
									series5.IsVisibleInLegend = false;
									series5.BorderWidth = 1;
									flag5 = true;
									this.chart1.Series.Add(series5);
								}
								series5 = null;
							}
						}
						if (series5 != null || !flag5)
						{
							if (series5 == null)
							{
								series5 = new Series();
							}
							series5.XValueType = ChartValueType.DateTime;
							series5.ChartArea = "ChartArea5";
							series5.ChartType = SeriesChartType.Line;
							series5.MarkerStyle = MarkerStyle.Circle;
							series5.MarkerSize = 3;
							series5.Color = color5;
							series5.LegendText = legendText2;
							series5.IsValueShownAsLabel = true;
							series5.LabelFormat = "N2";
							series5.Legend = "leg5";
							series5.IsVisibleInLegend = false;
							series5.BorderWidth = 1;
							this.chart1.Series.Add(series5);
						}
						for (int num27 = 0; num27 < list.Count; num27++)
						{
							if (dictionary[list[num27]] > -5.0)
							{
								this.csv_chart5.Append("\t\"" + dictionary[list[num27]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart5.Append("\t\"NA\"");
							}
						}
						this.csv_chart5.AppendLine();
					}
				}
				else
				{
					this.chart1.ChartAreas[4].Visible = false;
				}
				this.chart1.ResetAutoValues();
			}
		}
		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			if (!this.m_isRPTSaved)
			{
				DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Rpt_notsaved, new string[0]), MessageBoxButtons.OKCancel);
				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}
			}
			this.m_parent.showRpt(1, 0);
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = Sys_Para.GetThermalPath();
			if (text.Length == 0)
			{
				text = System.IO.Directory.GetCurrentDirectory() + "\\THReportFile\\";
			}
			System.DateTime now = System.DateTime.Now;
			int num = this.SaveToFile(text + now.ToString("yyyy-MM-dd HH-mm-ss"), this.m_pParaWindow.Txttitle);
			if (num == -1)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rpt_saveErr1, new string[0]));
				return;
			}
			if (num == -2)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			string str = text + now.ToString("yyyy-MM-dd HH-mm-ss") + "\\" + this.m_pParaWindow.Txttitle.Replace("'", "''");
			if (ReportInfo.InsertThermalReport(this.m_pParaWindow.Txttitle, this.m_pParaWindow.Txtwriter, now, str + ".html") == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			this.m_isRPTSaved = true;
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private int SaveToFile(string path, string name)
		{
			try
			{
				if (!System.IO.Directory.Exists(path))
				{
					System.IO.Directory.CreateDirectory(path);
				}
			}
			catch
			{
				int result = -1;
				return result;
			}
			try
			{
				if (this.m_pParaWindow.chkchart1_Checked() || this.m_pParaWindow.chkchart2_Checked() || this.m_pParaWindow.chkchart3_Checked() || this.m_pParaWindow.chkchart4_Checked() || this.m_pParaWindow.chkchart5_Checked())
				{
					this.chart1.SaveImage(path + "\\line.jpg", ChartImageFormat.Jpeg);
					if (this.m_pParaWindow.chkchart1_Checked())
					{
						this.exportCSV(path, name, "IntakeTempPeak", this.csv_chart1);
					}
					if (this.m_pParaWindow.chkchart2_Checked())
					{
						this.exportCSV(path, name, "ExhaustTempPeak", this.csv_chart2);
					}
					if (this.m_pParaWindow.chkchart3_Checked())
					{
						this.exportCSV(path, name, "EquipTempDiffPeak", this.csv_chart3);
					}
					if (this.m_pParaWindow.chkchart4_Checked())
					{
						this.exportCSV(path, name, "CoolingIndexHi", this.csv_chart4);
					}
					if (this.m_pParaWindow.chkchart5_Checked())
					{
						this.exportCSV(path, name, "CoolingIndexLo", this.csv_chart5);
					}
				}
			}
			catch
			{
				int result = -2;
				return result;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			stringBuilder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			stringBuilder.AppendLine("<head>");
			stringBuilder.AppendLine("<title>" + EcoLanguage.getMsg(LangRes.Rpt_saveThermAnalysis, new string[0]) + "</title>");
			stringBuilder.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
			stringBuilder.AppendLine("</head>");
			stringBuilder.AppendLine("<body >");
			if (this.m_pParaWindow.chkchart1_Checked() || this.m_pParaWindow.chkchart2_Checked() || this.m_pParaWindow.chkchart3_Checked() || this.m_pParaWindow.chkchart4_Checked() || this.m_pParaWindow.chkchart5_Checked())
			{
				stringBuilder.AppendLine("<p align='center'><img src='line.jpg'/></p>");
			}
			stringBuilder.AppendLine("</body>");
			stringBuilder.AppendLine("</html>");
			string path2 = path + "\\" + name + ".html";
			System.IO.FileStream stream = new System.IO.FileStream(path2, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.GetEncoding("UTF-8"));
			streamWriter.Flush();
			streamWriter.BaseStream.Seek(0L, System.IO.SeekOrigin.Begin);
			streamWriter.WriteLine(stringBuilder.ToString());
			streamWriter.Flush();
			streamWriter.Close();
			if (EcoLanguage.getLang() == 0)
			{
				this.exportPDF(path, name);
			}
			return 0;
		}
		private void exportCSV(string path, string name, string type, System.Text.StringBuilder csv)
		{
			commUtil.ShowInfo_DEBUG(string.Concat(new string[]
			{
				" Save --csv (",
				name,
				"_",
				type,
				")---- Start == ",
				System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
			}));
			string path2 = string.Concat(new string[]
			{
				path,
				"\\",
				name,
				"_",
				type,
				".csv"
			});
			System.IO.FileStream stream = new System.IO.FileStream(path2, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.GetEncoding("utf-16"));
			streamWriter.Flush();
			streamWriter.BaseStream.Seek(0L, System.IO.SeekOrigin.Begin);
			streamWriter.Write("﻿");
			streamWriter.Write(csv.ToString());
			streamWriter.Flush();
			streamWriter.Close();
			commUtil.ShowInfo_DEBUG(string.Concat(new string[]
			{
				" Save --csv (",
				name,
				"_",
				type,
				")---- End   == ",
				System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
			}));
		}
		private void exportPDF(string path, string name)
		{
			commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")----- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			PDF pDF = new PDF(new System.IO.BufferedStream(new System.IO.FileStream(path + "\\" + name + ".pdf", System.IO.FileMode.Create)));
			PDFjet.NET.Font font = new PDFjet.NET.Font(pDF, CoreFont.TIMES_BOLD);
			font.SetSize(7f);
			PDFjet.NET.Font font2 = new PDFjet.NET.Font(pDF, CoreFont.TIMES_ROMAN);
			font2.SetSize(7f);
			int num = 1;
			if (this.m_pParaWindow.chkchart1_Checked() || this.m_pParaWindow.chkchart2_Checked() || this.m_pParaWindow.chkchart3_Checked() || this.m_pParaWindow.chkchart4_Checked() || this.m_pParaWindow.chkchart5_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C1- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				Page page = new Page(pDF, Letter.PORTRAIT);
				pdfUtil.DrawImg(pDF, page, path + "\\line.jpg", 0f);
				pdfUtil.DrawPageNumber(pDF, page, num++);
			}
			pDF.Close();
			commUtil.ShowInfo_DEBUG(string.Concat(new string[]
			{
				" Save --pdf (",
				name,
				")----- End   == ",
				System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
				"\n"
			}));
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ThermGenRptShow));
			ChartArea chartArea = new ChartArea();
			ChartArea chartArea2 = new ChartArea();
			ChartArea chartArea3 = new ChartArea();
			ChartArea chartArea4 = new ChartArea();
			ChartArea chartArea5 = new ChartArea();
			Title title = new Title();
			Title title2 = new Title();
			Title title3 = new Title();
			this.btnPreview = new Button();
			this.btnSave = new Button();
			this.tpline = new TabPage();
			this.chart1 = new Chart();
			this.tabchart = new TabControl();
			this.tpline.SuspendLayout();
			((ISupportInitialize)this.chart1).BeginInit();
			this.tabchart.SuspendLayout();
			base.SuspendLayout();
			this.btnPreview.BackColor = System.Drawing.Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnPreview, "btnPreview");
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.UseVisualStyleBackColor = false;
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			this.btnSave.BackColor = System.Drawing.Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			componentResourceManager.ApplyResources(this.tpline, "tpline");
			this.tpline.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tpline.Controls.Add(this.chart1);
			this.tpline.Name = "tpline";
			this.chart1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.chart1.BorderlineColor = System.Drawing.Color.Black;
			this.chart1.BorderlineDashStyle = ChartDashStyle.Solid;
			chartArea.AlignmentOrientation = AreaAlignmentOrientations.None;
			chartArea.AlignmentStyle = AreaAlignmentStyles.None;
			chartArea.AxisX.Title = "ASDF";
			chartArea.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea.Name = "ChartArea1";
			chartArea.Position.Auto = false;
			chartArea.Position.Height = 18f;
			chartArea.Position.Width = 95f;
			chartArea.Position.X = 3f;
			chartArea.Position.Y = 4f;
			chartArea2.AlignmentOrientation = AreaAlignmentOrientations.None;
			chartArea2.AlignmentStyle = AreaAlignmentStyles.None;
			chartArea2.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea2.Name = "ChartArea2";
			chartArea2.Position.Auto = false;
			chartArea2.Position.Height = 18f;
			chartArea2.Position.Width = 95f;
			chartArea2.Position.X = 3f;
			chartArea2.Position.Y = 22.5f;
			chartArea3.AlignmentOrientation = AreaAlignmentOrientations.None;
			chartArea3.AlignmentStyle = AreaAlignmentStyles.None;
			chartArea3.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea3.Name = "ChartArea3";
			chartArea3.Position.Auto = false;
			chartArea3.Position.Height = 18f;
			chartArea3.Position.Width = 95f;
			chartArea3.Position.X = 3f;
			chartArea3.Position.Y = 41f;
			chartArea4.AlignmentOrientation = AreaAlignmentOrientations.None;
			chartArea4.AlignmentStyle = AreaAlignmentStyles.None;
			chartArea4.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea4.Name = "ChartArea4";
			chartArea4.Position.Auto = false;
			chartArea4.Position.Height = 18f;
			chartArea4.Position.Width = 95f;
			chartArea4.Position.X = 3f;
			chartArea4.Position.Y = 59.5f;
			chartArea5.AlignmentOrientation = AreaAlignmentOrientations.None;
			chartArea5.AlignmentStyle = AreaAlignmentStyles.None;
			chartArea5.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea5.Name = "ChartArea5";
			chartArea5.Position.Auto = false;
			chartArea5.Position.Height = 18f;
			chartArea5.Position.Width = 95f;
			chartArea5.Position.X = 3f;
			chartArea5.Position.Y = 78f;
			this.chart1.ChartAreas.Add(chartArea);
			this.chart1.ChartAreas.Add(chartArea2);
			this.chart1.ChartAreas.Add(chartArea3);
			this.chart1.ChartAreas.Add(chartArea4);
			this.chart1.ChartAreas.Add(chartArea5);
			componentResourceManager.ApplyResources(this.chart1, "chart1");
			this.chart1.Name = "chart1";
			this.chart1.Palette = ChartColorPalette.None;
			title.Alignment = ContentAlignment.TopRight;
			title.Name = "Datetime";
			title.Position.Auto = false;
			title.Position.Height = 2.5f;
			title.Position.Width = 94f;
			title.Position.X = 5f;
			title.Position.Y = 0.3f;
			title.Text = "datetime";
			title2.Alignment = ContentAlignment.TopCenter;
			title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f);
			title2.Name = "Title";
			title2.Position.Auto = false;
			title2.Position.Height = 3.5f;
			title2.Position.Width = 94f;
			title2.Position.X = 3f;
			title2.Position.Y = 1.2f;
			title2.Text = "Title";
			title3.Alignment = ContentAlignment.TopCenter;
			title3.Name = "Byname";
			title3.Position.Auto = false;
			title3.Position.Height = 2.5f;
			title3.Position.Width = 94f;
			title3.Position.X = 3f;
			title3.Position.Y = 2.5f;
			title3.Text = "Byname";
			this.chart1.Titles.Add(title);
			this.chart1.Titles.Add(title2);
			this.chart1.Titles.Add(title3);
			this.tabchart.Controls.Add(this.tpline);
			componentResourceManager.ApplyResources(this.tabchart, "tabchart");
			this.tabchart.Name = "tabchart";
			this.tabchart.SelectedIndex = 0;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			base.Controls.Add(this.tabchart);
			base.Controls.Add(this.btnPreview);
			base.Controls.Add(this.btnSave);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ThermGenRptShow";
			this.tpline.ResumeLayout(false);
			((ISupportInitialize)this.chart1).EndInit();
			this.tabchart.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
