using DBAccessAPI;
using EcoDevice.AccessAPI;
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
namespace EcoSensors.EnegManPage.Analysis
{
	public class EGenRptShow : UserControl
	{
		public static int AnalysisIndex_devIDs = 0;
		public static int AnalysisIndex_gpID = 1;
		public static int AnalysisIndex_gpNM = 2;
		public static int AnalysisIndex_gpTP = 3;
		public static int AnalysisIndex_portIDs = 4;
		public static int AnalysisIndex_invaliddevIDs = 5;
		public static int AnalysisIndex_gpPUE = 6;
		private static System.Drawing.Color[] DFTLine_Color = new System.Drawing.Color[]
		{
			System.Drawing.Color.Red,
			System.Drawing.Color.Blue,
			System.Drawing.Color.Orange,
			System.Drawing.Color.FromArgb(162, 215, 48)
		};
		private string m_dgvinventory_Col6Txt;
		private string m_dgvinventory_Col7Txt;
		private EnegAnalysis m_parent;
		private EGenRptParaClass m_pParaClass;
		private float m_energyV;
		private bool m_isRPTSaved;
		private System.Text.StringBuilder csv_chart1 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart2 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart3 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart4 = new System.Text.StringBuilder();
		private System.Text.StringBuilder csv_chart5 = new System.Text.StringBuilder();
		private IContainer components;
		private TabControl tabchart;
		private TabPage tpline;
		private Chart chart1;
		private TabPage tpsaving;
		private System.Windows.Forms.TextBox txtaction;
		private Chart chtsaving;
		private TabPage tpcapacity;
		private Panel palcapacity4;
		private Label labcapacity4;
		private DataGridView dgvcapacity4;
		private Chart chtcapacity4;
		private Panel palcapacity3;
		private Label labcapacity3;
		private DataGridView dgvcapacity3;
		private Chart chtcapacity3;
		private Panel palcapacity2;
		private Label labcapacity2;
		private DataGridView dgvcapacity2;
		private Chart chtcapacity2;
		private Panel palcapacity1;
		private Label labcapacity1;
		private DataGridView dgvcapacity1;
		private Chart chtcapacity1;
		private TabPage tplist;
		private DataGridView dgvinventory;
		private Label label4;
		private Button btnPreview;
		private Button btnSave;
		private TabPage tppue;
		private DataGridView dgvPUE;
		private DataGridViewTextBoxColumn gpnm;
		private DataGridViewTextBoxColumn totalpower;
		private DataGridViewTextBoxColumn itpower;
		private DataGridViewTextBoxColumn pue;
		private Label labelPUE;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Rack_Name;
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewTextBoxColumn Column3;
		private DataGridViewTextBoxColumn Power_diss;
		private DataGridViewTextBoxColumn Column4;
		private DataGridViewTextBoxColumn Column5;
		private DataGridViewTextBoxColumn Column6;
		public EGenRptShow()
		{
			this.InitializeComponent();
			this.m_dgvinventory_Col6Txt = this.dgvinventory.Columns[6].HeaderText;
			this.m_dgvinventory_Col7Txt = this.dgvinventory.Columns[7].HeaderText;
		}
		public void pageInit(EnegAnalysis parent, EGenRptParaClass pParaClass, System.Collections.Generic.List<object> ret_obj)
		{
			this.m_parent = parent;
			this.m_pParaClass = pParaClass;
			this.m_isRPTSaved = false;
			this.m_energyV = Sys_Para.GetEnergyValue();
			this.chart1.Height = 55;
			this.chart1.Legends.Clear();
			this.chart1.Series.Clear();
			this.csv_chart1 = new System.Text.StringBuilder();
			this.csv_chart2 = new System.Text.StringBuilder();
			this.csv_chart3 = new System.Text.StringBuilder();
			this.csv_chart4 = new System.Text.StringBuilder();
			this.csv_chart5 = new System.Text.StringBuilder();
			this.tabchart.TabPages.Clear();
			this.CreateChart_useprepareData(ret_obj);
		}
		private void CreateChart_useprepareData(System.Collections.Generic.List<object> ret_obj)
		{
			DataTable[] array = new DataTable[]
			{
				ret_obj[0] as DataTable,
				ret_obj[1] as DataTable,
				ret_obj[2] as DataTable,
				ret_obj[3] as DataTable
			};
			DataTable[] array2 = new DataTable[]
			{
				ret_obj[4] as DataTable,
				ret_obj[5] as DataTable,
				ret_obj[6] as DataTable,
				ret_obj[7] as DataTable
			};
			DataTable[] array3 = new DataTable[]
			{
				ret_obj[8] as DataTable,
				ret_obj[9] as DataTable,
				ret_obj[10] as DataTable,
				ret_obj[11] as DataTable
			};
			DataTable[] array4 = new DataTable[]
			{
				ret_obj[12] as DataTable,
				ret_obj[13] as DataTable,
				ret_obj[14] as DataTable,
				ret_obj[15] as DataTable
			};
			string txttitle = this.m_pParaClass.Txttitle;
			string dtptime = this.m_pParaClass.Dtptime;
			string arg_138_0 = this.m_pParaClass.Txtwriter;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.Dictionary<string, double> dictionary = new System.Collections.Generic.Dictionary<string, double>();
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			stringBuilder.Append("\"" + EcoLanguage.getMsg(LangRes.Rpt_shGroupName, new string[0]) + "\"");
			switch (this.m_pParaClass.Cboperiod_SelectedIndex)
			{
			case 0:
			{
				System.DateTime t = this.m_pParaClass.DTbegin;
				while (t < this.m_pParaClass.DTend)
				{
					string text = t.ToString(this.m_pParaClass.AxisX_LabelStyleFormat);
					stringBuilder.Append("\t\"" + text + "\"");
					dictionary.Add(text, -100.0);
					list.Add(text);
					t = t.AddHours(1.0);
				}
				break;
			}
			case 1:
			{
				System.DateTime t = this.m_pParaClass.DTbegin;
				while (t < this.m_pParaClass.DTend)
				{
					string text2 = t.ToString(this.m_pParaClass.AxisX_LabelStyleFormat);
					stringBuilder.Append("\t\"" + text2 + "\"");
					dictionary.Add(text2, -100.0);
					list.Add(text2);
					t = t.AddDays(1.0);
				}
				break;
			}
			case 2:
			{
				System.DateTime t = this.m_pParaClass.DTbegin;
				while (t < this.m_pParaClass.DTend)
				{
					string text3 = t.ToString(this.m_pParaClass.AxisX_LabelStyleFormat);
					stringBuilder.Append("\t\"" + text3 + "\"");
					dictionary.Add(text3, -100.0);
					list.Add(text3);
					t = t.AddMonths(1);
				}
				break;
			}
			case 3:
			{
				System.DateTime t = this.m_pParaClass.DTbegin;
				while (t < this.m_pParaClass.DTend)
				{
					string text4 = t.ToString(this.m_pParaClass.AxisX_LabelStyleFormat);
					stringBuilder.Append("\t\"" + text4 + "\"");
					dictionary.Add(text4, -100.0);
					list.Add(text4);
					t = t.AddMonths(3);
				}
				break;
			}
			}
			stringBuilder.AppendLine();
			this.m_pParaClass.DTbegin.ToString("yyyy-MM-dd HH:mm:ss");
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			System.Collections.ArrayList gppara_list = this.m_pParaClass.gppara_list;
			if (this.m_pParaClass.chkchart1_Checked() || this.m_pParaClass.chkchart2_Checked() || this.m_pParaClass.chkchart3_Checked() || this.m_pParaClass.chkchart4_Checked() || this.m_pParaClass.chkchart5_Checked())
			{
				this.tabchart.TabPages.Add(this.tpline);
				bool isVisibleInLegend = true;
				this.chart1.Titles[0].Text = EcoLanguage.getMsg(LangRes.Rpt_shdate, new string[0]) + ":" + dtptime;
				this.chart1.Titles[1].Text = txttitle;
				this.chart1.Titles[2].Text = this.m_pParaClass.report_from;
				this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
				this.chart1.Titles[1].Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, FontStyle.Regular);
				this.chart1.Titles[2].Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
				int num4 = 0;
				int[] array5 = new int[5];
				if (this.m_pParaClass.chkchart1_Checked())
				{
					num4++;
					array5[0] = num4;
				}
				else
				{
					array5[0] = 0;
				}
				if (this.m_pParaClass.chkchart2_Checked())
				{
					num4++;
					array5[1] = num4;
				}
				else
				{
					array5[1] = 0;
				}
				if (this.m_pParaClass.chkchart3_Checked())
				{
					num4++;
					array5[2] = num4;
				}
				else
				{
					array5[2] = 0;
				}
				if (this.m_pParaClass.chkchart4_Checked())
				{
					num4++;
					array5[3] = num4;
				}
				else
				{
					array5[3] = 0;
				}
				if (this.m_pParaClass.chkchart5_Checked())
				{
					num4++;
					array5[4] = num4;
				}
				else
				{
					array5[4] = 0;
				}
				float num5 = (float)(80 + 400 * num4 + 70);
				float height = 40000f / num5;
				this.chart1.Titles[0].Position.Y = 600f / num5;
				this.chart1.Titles[1].Position.Y = 2400f / num5;
				this.chart1.Titles[1].Position.Height = 7000f / num5;
				this.chart1.Titles[2].Position.Y = 5000f / num5;
				this.chart1.Height = (int)num5;
				if (this.m_pParaClass.chkchart1_Checked())
				{
					this.chart1.ChartAreas[0].Visible = true;
					this.chart1.ChartAreas[0].Position.X = 2f;
					this.chart1.ChartAreas[0].Position.Y = (float)((80 + 400 * (array5[0] - 1)) * 100) / num5;
					this.chart1.ChartAreas[0].Position.Height = height;
					this.chart1.ChartAreas[0].Position.Width = 95f;
					this.chart1.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shITUsage, new string[0]);
					this.chart1.ChartAreas[0].AxisY.Title = "kWh";
					this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = this.m_pParaClass.AxisX_LabelStyleFormat;
					this.chart1.ChartAreas[0].AxisX.Interval = this.m_pParaClass.AxisX_Interval;
					this.chart1.ChartAreas[0].AxisX.IntervalType = this.m_pParaClass.AxisX_IntervalType;
					this.chart1.ChartAreas[0].AxisX.Minimum = this.m_pParaClass.DTbegin_minus1.ToOADate();
					this.chart1.ChartAreas[0].AxisX.Maximum = this.m_pParaClass.DTend.ToOADate();
					Legend legend = new Legend();
					legend.Alignment = StringAlignment.Far;
					legend.Docking = Docking.Top;
					legend.LegendStyle = LegendStyle.Table;
					legend.BorderWidth = 2;
					legend.Name = "leg1";
					legend.DockedToChartArea = "ChartArea1";
					legend.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend);
					this.csv_chart1.Append(stringBuilder.ToString());
					for (int i = 0; i < gppara_list.Count; i++)
					{
						string[] array6 = gppara_list[i].ToString().Split(new char[]
						{
							'|'
						});
						string arg_99A_0 = array6[EGenRptShow.AnalysisIndex_devIDs];
						string text5 = array6[EGenRptShow.AnalysisIndex_gpNM];
						string arg_9AD_0 = array6[EGenRptShow.AnalysisIndex_gpTP];
						string arg_9B6_0 = array6[EGenRptShow.AnalysisIndex_portIDs];
						for (int j = 0; j < list.Count; j++)
						{
							dictionary[list[j]] = -100.0;
						}
						this.csv_chart1.Append("\"" + text5 + "\"");
						if (array[i] == null)
						{
							System.Console.WriteLine(" chart1 - groupPDTable error !! j=" + i);
						}
						System.Drawing.Color color = EGenRptShow.DFTLine_Color[i];
						foreach (DataRow dataRow in array[i].Rows)
						{
							try
							{
								double value = System.Convert.ToDouble(dataRow["power_consumption"]);
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									string key = dataRow["period"].ToString();
									if (dictionary.ContainsKey(key))
									{
										dictionary[key] = value;
									}
								}
								else
								{
									string text6 = dataRow["period"].ToString();
									int num6 = System.Convert.ToInt32(text6.Split(new char[]
									{
										'Q'
									})[1]);
									num6 = 3 * (num6 - 1) + 1;
									text6 = text6.Split(new char[]
									{
										'Q'
									})[0] + "-" + num6.ToString("D2");
									string key = text6;
									if (dictionary.ContainsKey(key))
									{
										dictionary[key] = value;
									}
								}
							}
							catch
							{
							}
						}
						Series series = null;
						bool flag = false;
						for (int k = 0; k < list.Count; k++)
						{
							if (dictionary[list[k]] > -5.0)
							{
								if (series == null)
								{
									series = new Series();
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									series.Points.AddXY(System.DateTime.Parse(list[k] + this.m_pParaClass.extra_str), new object[]
									{
										dictionary[list[k]]
									});
								}
								else
								{
									series.Points.AddXY(System.DateTime.Parse(list[k]), new object[]
									{
										dictionary[list[k]]
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
									series.LegendText = text5;
									series.IsValueShownAsLabel = true;
									series.LabelFormat = "F4";
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
							series.LegendText = text5;
							series.IsValueShownAsLabel = true;
							series.LabelFormat = "F4";
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
						for (int l = 0; l < list.Count; l++)
						{
							if (dictionary[list[l]] > -5.0)
							{
								this.csv_chart1.Append("\t\"" + dictionary[list[l]].ToString("F4") + "\"");
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
				if (this.m_pParaClass.chkchart2_Checked())
				{
					this.chart1.ChartAreas[1].Visible = true;
					this.chart1.ChartAreas[1].Position.X = 2f;
					this.chart1.ChartAreas[1].Position.Y = (float)((80 + 420 * (array5[1] - 1)) * 100) / num5;
					this.chart1.ChartAreas[1].Position.Height = height;
					this.chart1.ChartAreas[1].Position.Width = 95f;
					this.chart1.ChartAreas[1].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shITLoad, new string[0]);
					this.chart1.ChartAreas[1].AxisY.Title = "kW";
					this.chart1.ChartAreas[1].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[1].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[1].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[1].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[1].AxisX.LabelStyle.Format = this.m_pParaClass.AxisX_LabelStyleFormat;
					this.chart1.ChartAreas[1].AxisX.Interval = this.m_pParaClass.AxisX_Interval;
					this.chart1.ChartAreas[1].AxisX.IntervalType = this.m_pParaClass.AxisX_IntervalType;
					this.chart1.ChartAreas[1].AxisX.Minimum = this.m_pParaClass.DTbegin_minus1.ToOADate();
					this.chart1.ChartAreas[1].AxisX.Maximum = this.m_pParaClass.DTend.ToOADate();
					Legend legend2 = new Legend();
					legend2.Alignment = StringAlignment.Far;
					legend2.Docking = Docking.Top;
					legend2.LegendStyle = LegendStyle.Table;
					legend2.Name = "leg2";
					legend2.DockedToChartArea = "ChartArea2";
					legend2.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend2);
					this.csv_chart2.Append(stringBuilder.ToString());
					for (int m = 0; m < gppara_list.Count; m++)
					{
						string[] array7 = gppara_list[m].ToString().Split(new char[]
						{
							'|'
						});
						string arg_1168_0 = array7[EGenRptShow.AnalysisIndex_devIDs];
						string text7 = array7[EGenRptShow.AnalysisIndex_gpNM];
						string arg_117B_0 = array7[EGenRptShow.AnalysisIndex_gpTP];
						string arg_1184_0 = array7[EGenRptShow.AnalysisIndex_portIDs];
						System.Drawing.Color color2 = EGenRptShow.DFTLine_Color[m];
						for (int n = 0; n < list.Count; n++)
						{
							dictionary[list[n]] = -100.0;
						}
						this.csv_chart2.Append("\"" + text7 + "\"");
						if (array2[m] == null)
						{
							System.Console.WriteLine(" chart2 - groupPPeakTable error !! j=" + m);
						}
						if (array[m] == null)
						{
							System.Console.WriteLine(" chart2 - groupPDTable error !! j=" + m);
						}
						System.Collections.Generic.Dictionary<string, double> dictionary2 = new System.Collections.Generic.Dictionary<string, double>();
						for (int num7 = 0; num7 < array[m].Rows.Count; num7++)
						{
							double num8 = System.Convert.ToDouble(array[m].Rows[num7]["power_consumption"]);
							dictionary2.Add(System.Convert.ToString(array[m].Rows[num7]["period"]), num8);
						}
						foreach (DataRow dataRow2 in array2[m].Rows)
						{
							try
							{
								double num9 = System.Convert.ToDouble(dataRow2["power"]);
								string key2 = dataRow2["period"].ToString();
								double spendMinutes = this.getSpendMinutes(this.m_pParaClass.Cboperiod_SelectedIndex, dataRow2["period"].ToString());
								if (dictionary2.ContainsKey(key2))
								{
									double num8 = dictionary2[key2];
									double num10 = num8 * 60.0 / spendMinutes;
									if (num9 < num10)
									{
										num9 = this.calPeakPower(num10, num9);
									}
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									string key3 = dataRow2["period"].ToString();
									if (dictionary.ContainsKey(key3))
									{
										dictionary[key3] = num9;
									}
								}
								else
								{
									string text8 = dataRow2["period"].ToString();
									int num11 = System.Convert.ToInt32(text8.Split(new char[]
									{
										'Q'
									})[1]);
									num11 = 3 * (num11 - 1) + 1;
									text8 = text8.Split(new char[]
									{
										'Q'
									})[0] + "-" + num11.ToString("D2");
									string key3 = text8;
									if (dictionary.ContainsKey(key3))
									{
										dictionary[key3] = num9;
									}
								}
							}
							catch
							{
							}
						}
						Series series2 = null;
						bool flag2 = false;
						for (int num12 = 0; num12 < list.Count; num12++)
						{
							if (dictionary[list[num12]] > -5.0)
							{
								if (series2 == null)
								{
									series2 = new Series();
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									series2.Points.AddXY(System.DateTime.Parse(list[num12] + this.m_pParaClass.extra_str), new object[]
									{
										dictionary[list[num12]]
									});
								}
								else
								{
									series2.Points.AddXY(System.DateTime.Parse(list[num12]), new object[]
									{
										dictionary[list[num12]]
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
									series2.LegendText = text7;
									series2.IsValueShownAsLabel = true;
									series2.LabelFormat = "F4";
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
							series2.LegendText = text7;
							series2.IsValueShownAsLabel = true;
							series2.LabelFormat = "F4";
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
						for (int num13 = 0; num13 < list.Count; num13++)
						{
							if (dictionary[list[num13]] > -5.0)
							{
								this.csv_chart2.Append("\t\"" + dictionary[list[num13]].ToString("F4") + "\"");
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
				if (this.m_pParaClass.chkchart3_Checked())
				{
					num2 = this.m_pParaClass.Co2_elec();
					this.chart1.ChartAreas[2].Visible = true;
					this.chart1.ChartAreas[2].Position.X = 2f;
					this.chart1.ChartAreas[2].Position.Y = (float)((80 + 420 * (array5[2] - 1)) * 100) / num5;
					this.chart1.ChartAreas[2].Position.Height = height;
					this.chart1.ChartAreas[2].Position.Width = 95f;
					this.chart1.ChartAreas[2].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shITCO2Equ, new string[0]);
					this.chart1.ChartAreas[2].AxisY.Title = "KG";
					this.chart1.ChartAreas[2].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[2].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[2].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[2].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[2].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[2].AxisX.LabelStyle.Format = this.m_pParaClass.AxisX_LabelStyleFormat;
					this.chart1.ChartAreas[2].AxisX.Interval = this.m_pParaClass.AxisX_Interval;
					this.chart1.ChartAreas[2].AxisX.IntervalType = this.m_pParaClass.AxisX_IntervalType;
					this.chart1.ChartAreas[2].AxisX.Minimum = this.m_pParaClass.DTbegin_minus1.ToOADate();
					this.chart1.ChartAreas[2].AxisX.Maximum = this.m_pParaClass.DTend.ToOADate();
					Legend legend3 = new Legend();
					legend3.Alignment = StringAlignment.Far;
					legend3.Docking = Docking.Top;
					legend3.LegendStyle = LegendStyle.Table;
					legend3.Name = "leg3";
					legend3.DockedToChartArea = "ChartArea3";
					legend3.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend3);
					this.csv_chart3.Append(stringBuilder.ToString());
					for (int num14 = 0; num14 < gppara_list.Count; num14++)
					{
						string[] array8 = gppara_list[num14].ToString().Split(new char[]
						{
							'|'
						});
						string arg_1A3C_0 = array8[EGenRptShow.AnalysisIndex_devIDs];
						string text9 = array8[EGenRptShow.AnalysisIndex_gpNM];
						string arg_1A4F_0 = array8[EGenRptShow.AnalysisIndex_gpTP];
						string arg_1A58_0 = array8[EGenRptShow.AnalysisIndex_portIDs];
						for (int num15 = 0; num15 < list.Count; num15++)
						{
							dictionary[list[num15]] = -100.0;
						}
						this.csv_chart3.Append("\"" + text9 + "\"");
						if (array[num14] == null)
						{
							System.Console.WriteLine(" chart3 - groupPDTable error !! j=" + num14);
						}
						System.Drawing.Color color3 = EGenRptShow.DFTLine_Color[num14];
						foreach (DataRow dataRow3 in array[num14].Rows)
						{
							try
							{
								double value2 = System.Convert.ToDouble(dataRow3["power_consumption"]) * num2;
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									string key4 = dataRow3["period"].ToString();
									if (dictionary.ContainsKey(key4))
									{
										dictionary[key4] = value2;
									}
								}
								else
								{
									string text10 = dataRow3["period"].ToString();
									int num16 = System.Convert.ToInt32(text10.Split(new char[]
									{
										'Q'
									})[1]);
									num16 = 3 * (num16 - 1) + 1;
									text10 = text10.Split(new char[]
									{
										'Q'
									})[0] + "-" + num16.ToString("D2");
									string key4 = text10;
									if (dictionary.ContainsKey(key4))
									{
										dictionary[key4] = value2;
									}
								}
							}
							catch
							{
							}
						}
						Series series3 = null;
						bool flag3 = false;
						for (int num17 = 0; num17 < list.Count; num17++)
						{
							if (dictionary[list[num17]] > -5.0)
							{
								if (series3 == null)
								{
									series3 = new Series();
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									series3.Points.AddXY(System.DateTime.Parse(list[num17] + this.m_pParaClass.extra_str), new object[]
									{
										dictionary[list[num17]]
									});
								}
								else
								{
									series3.Points.AddXY(System.DateTime.Parse(list[num17]), new object[]
									{
										dictionary[list[num17]]
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
									series3.LegendText = text9;
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
							series3.LegendText = text9;
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
						for (int num18 = 0; num18 < list.Count; num18++)
						{
							if (dictionary[list[num18]] > -5.0)
							{
								this.csv_chart3.Append("\t\"" + dictionary[list[num18]].ToString("F4") + "\"");
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
				if (this.m_pParaClass.chkchart4_Checked())
				{
					num = this.m_pParaClass.price_elec();
					this.chart1.ChartAreas[3].Visible = true;
					this.chart1.ChartAreas[3].Position.X = 2f;
					this.chart1.ChartAreas[3].Position.Y = (float)((80 + 420 * (array5[3] - 1)) * 100) / num5;
					this.chart1.ChartAreas[3].Position.Height = height;
					this.chart1.ChartAreas[3].Position.Width = 95f;
					this.chart1.ChartAreas[3].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shITPowerconsum, new string[0]);
					this.chart1.ChartAreas[3].AxisY.Title = EcoGlobalVar.CurCurrency;
					this.chart1.ChartAreas[3].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[3].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[3].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[3].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[3].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[3].AxisX.LabelStyle.Format = this.m_pParaClass.AxisX_LabelStyleFormat;
					this.chart1.ChartAreas[3].AxisX.Interval = this.m_pParaClass.AxisX_Interval;
					this.chart1.ChartAreas[3].AxisX.IntervalType = this.m_pParaClass.AxisX_IntervalType;
					this.chart1.ChartAreas[3].AxisX.Minimum = this.m_pParaClass.DTbegin_minus1.ToOADate();
					this.chart1.ChartAreas[3].AxisX.Maximum = this.m_pParaClass.DTend.ToOADate();
					Legend legend4 = new Legend();
					legend4.Alignment = StringAlignment.Far;
					legend4.Docking = Docking.Top;
					legend4.LegendStyle = LegendStyle.Table;
					legend4.Name = "leg4";
					legend4.DockedToChartArea = "ChartArea4";
					legend4.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend4);
					this.csv_chart4.Append(stringBuilder.ToString());
					for (int num19 = 0; num19 < gppara_list.Count; num19++)
					{
						string[] array9 = gppara_list[num19].ToString().Split(new char[]
						{
							'|'
						});
						string arg_221A_0 = array9[EGenRptShow.AnalysisIndex_devIDs];
						string text11 = array9[EGenRptShow.AnalysisIndex_gpNM];
						string arg_222D_0 = array9[EGenRptShow.AnalysisIndex_gpTP];
						string arg_2236_0 = array9[EGenRptShow.AnalysisIndex_portIDs];
						for (int num20 = 0; num20 < list.Count; num20++)
						{
							dictionary[list[num20]] = -100.0;
						}
						this.csv_chart4.Append("\"" + text11 + "\"");
						if (array[num19] == null)
						{
							System.Console.WriteLine(" chart4 - groupPDTable error !! j=" + num19);
						}
						System.Drawing.Color color4 = EGenRptShow.DFTLine_Color[num19];
						foreach (DataRow dataRow4 in array[num19].Rows)
						{
							try
							{
								double value3 = System.Convert.ToDouble(dataRow4["power_consumption"]) * num;
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									string key5 = dataRow4["period"].ToString();
									if (dictionary.ContainsKey(key5))
									{
										dictionary[key5] = value3;
									}
								}
								else
								{
									string text12 = dataRow4["period"].ToString();
									int num21 = System.Convert.ToInt32(text12.Split(new char[]
									{
										'Q'
									})[1]);
									num21 = 3 * (num21 - 1) + 1;
									text12 = text12.Split(new char[]
									{
										'Q'
									})[0] + "-" + num21.ToString("D2");
									string key5 = text12;
									if (dictionary.ContainsKey(key5))
									{
										dictionary[key5] = value3;
									}
								}
							}
							catch
							{
							}
						}
						Series series4 = null;
						bool flag4 = false;
						for (int num22 = 0; num22 < list.Count; num22++)
						{
							if (dictionary[list[num22]] > -5.0)
							{
								if (series4 == null)
								{
									series4 = new Series();
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									series4.Points.AddXY(System.DateTime.Parse(list[num22] + this.m_pParaClass.extra_str), new object[]
									{
										dictionary[list[num22]]
									});
								}
								else
								{
									series4.Points.AddXY(System.DateTime.Parse(list[num22]), new object[]
									{
										dictionary[list[num22]]
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
									series4.MarkerStyle = MarkerStyle.Cross;
									series4.MarkerSize = 3;
									series4.Color = color4;
									series4.LegendText = text11;
									series4.IsValueShownAsLabel = true;
									series4.LabelFormat = "N2";
									series4.Legend = "leg4";
									if (!flag4)
									{
										series4.IsVisibleInLegend = isVisibleInLegend;
									}
									else
									{
										series4.IsVisibleInLegend = false;
									}
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
							series4.MarkerStyle = MarkerStyle.Cross;
							series4.MarkerSize = 3;
							series4.Color = color4;
							series4.LegendText = text11;
							series4.IsValueShownAsLabel = true;
							series4.LabelFormat = "N2";
							series4.Legend = "leg4";
							if (!flag4)
							{
								series4.IsVisibleInLegend = isVisibleInLegend;
							}
							else
							{
								series4.IsVisibleInLegend = false;
							}
							series4.BorderWidth = 1;
							this.chart1.Series.Add(series4);
						}
						for (int num23 = 0; num23 < list.Count; num23++)
						{
							if (dictionary[list[num23]] > -5.0)
							{
								this.csv_chart4.Append("\t\"" + dictionary[list[num23]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart4.Append("\t\"NA\"");
							}
						}
						this.csv_chart4.AppendLine();
					}
					isVisibleInLegend = false;
				}
				else
				{
					this.chart1.ChartAreas[3].Visible = false;
				}
				if (this.m_pParaClass.chkchart5_Checked())
				{
					num3 = this.m_pParaClass.price_Co2();
					num2 = this.m_pParaClass.Co2_elec();
					this.chart1.ChartAreas[4].Visible = true;
					this.chart1.ChartAreas[4].Position.X = 2f;
					this.chart1.ChartAreas[4].Position.Y = (float)((80 + 420 * (array5[4] - 1)) * 100) / num5;
					this.chart1.ChartAreas[4].Position.Height = height;
					this.chart1.ChartAreas[4].Position.Width = 95f;
					this.chart1.ChartAreas[4].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shITCO2Emi, new string[0]);
					this.chart1.ChartAreas[4].AxisY.Title = EcoGlobalVar.CurCurrency;
					this.chart1.ChartAreas[4].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[4].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					this.chart1.ChartAreas[4].AxisY.IsStartedFromZero = false;
					this.chart1.ChartAreas[4].AxisX.MajorGrid.Enabled = false;
					this.chart1.ChartAreas[4].AxisY.MajorGrid.Enabled = true;
					this.chart1.ChartAreas[4].AxisX.LabelStyle.Format = this.m_pParaClass.AxisX_LabelStyleFormat;
					this.chart1.ChartAreas[4].AxisX.Interval = this.m_pParaClass.AxisX_Interval;
					this.chart1.ChartAreas[4].AxisX.IntervalType = this.m_pParaClass.AxisX_IntervalType;
					this.chart1.ChartAreas[4].AxisX.Minimum = this.m_pParaClass.DTbegin_minus1.ToOADate();
					this.chart1.ChartAreas[4].AxisX.Maximum = this.m_pParaClass.DTend.ToOADate();
					Legend legend5 = new Legend();
					legend5.Alignment = StringAlignment.Far;
					legend5.Docking = Docking.Top;
					legend5.LegendStyle = LegendStyle.Table;
					legend5.Name = "leg5";
					legend5.DockedToChartArea = "ChartArea5";
					legend5.IsDockedInsideChartArea = false;
					this.chart1.Legends.Add(legend5);
					this.csv_chart5.Append(stringBuilder.ToString());
					for (int num24 = 0; num24 < gppara_list.Count; num24++)
					{
						string[] array10 = gppara_list[num24].ToString().Split(new char[]
						{
							'|'
						});
						string arg_2A05_0 = array10[EGenRptShow.AnalysisIndex_devIDs];
						string text13 = array10[EGenRptShow.AnalysisIndex_gpNM];
						string arg_2A18_0 = array10[EGenRptShow.AnalysisIndex_gpTP];
						string arg_2A21_0 = array10[EGenRptShow.AnalysisIndex_portIDs];
						for (int num25 = 0; num25 < list.Count; num25++)
						{
							dictionary[list[num25]] = -100.0;
						}
						this.csv_chart5.Append("\"" + text13 + "\"");
						if (array[num24] == null)
						{
							System.Console.WriteLine(" chart5 - groupPDTable error !! j=" + num24);
						}
						System.Drawing.Color color5 = EGenRptShow.DFTLine_Color[num24];
						foreach (DataRow dataRow5 in array[num24].Rows)
						{
							try
							{
								double value4 = System.Convert.ToDouble(dataRow5["power_consumption"]) * num3 * num2 / 1000.0;
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									string key6 = dataRow5["period"].ToString();
									if (dictionary.ContainsKey(key6))
									{
										dictionary[key6] = value4;
									}
								}
								else
								{
									string text14 = dataRow5["period"].ToString();
									int num26 = System.Convert.ToInt32(text14.Split(new char[]
									{
										'Q'
									})[1]);
									num26 = 3 * (num26 - 1) + 1;
									text14 = text14.Split(new char[]
									{
										'Q'
									})[0] + "-" + num26.ToString("D2");
									string key6 = text14;
									if (dictionary.ContainsKey(key6))
									{
										dictionary[key6] = value4;
									}
								}
							}
							catch
							{
							}
						}
						Series series5 = null;
						bool flag5 = false;
						for (int num27 = 0; num27 < list.Count; num27++)
						{
							if (dictionary[list[num27]] > -5.0)
							{
								if (series5 == null)
								{
									series5 = new Series();
								}
								if (this.m_pParaClass.Cboperiod_SelectedIndex != 3)
								{
									series5.Points.AddXY(System.DateTime.Parse(list[num27] + this.m_pParaClass.extra_str), new object[]
									{
										dictionary[list[num27]]
									});
								}
								else
								{
									series5.Points.AddXY(System.DateTime.Parse(list[num27]), new object[]
									{
										dictionary[list[num27]]
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
									series5.LegendText = text13;
									series5.IsValueShownAsLabel = true;
									series5.LabelFormat = "N2";
									series5.Legend = "leg5";
									if (!flag5)
									{
										series5.IsVisibleInLegend = isVisibleInLegend;
									}
									else
									{
										series5.IsVisibleInLegend = false;
									}
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
							series5.LegendText = text13;
							series5.IsValueShownAsLabel = true;
							series5.LabelFormat = "N2";
							series5.Legend = "leg5";
							if (!flag5)
							{
								series5.IsVisibleInLegend = isVisibleInLegend;
							}
							else
							{
								series5.IsVisibleInLegend = false;
							}
							series5.BorderWidth = 1;
							this.chart1.Series.Add(series5);
						}
						for (int num28 = 0; num28 < list.Count; num28++)
						{
							if (dictionary[list[num28]] > -5.0)
							{
								this.csv_chart5.Append("\t\"" + dictionary[list[num28]].ToString("F4") + "\"");
							}
							else
							{
								this.csv_chart5.Append("\t\"NA\"");
							}
						}
						this.csv_chart5.AppendLine();
					}
					isVisibleInLegend = false;
				}
				else
				{
					this.chart1.ChartAreas[4].Visible = false;
				}
				this.chart1.ResetAutoValues();
			}
			if (this.m_pParaClass.chkchart6_Checked())
			{
				this.tabchart.TabPages.Add(this.tpsaving);
				double num29 = 0.0;
				double num30 = 0.0;
				double num31 = 0.0;
				double num32 = 0.0;
				double num33 = 0.0;
				double num34 = 27.0;
				double num35 = 32.0;
				double num36 = -99999.0;
				double num37 = -99999.0;
				double num38 = -99999.0;
				double num39 = -99999.0;
				double num40 = 0.0;
				double num41 = 0.0;
				double num42 = 0.0;
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				stringBuilder2.AppendLine(EcoLanguage.getMsg(LangRes.Rpt_shAction, new string[0]));
				string str = "";
				System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary3 = ret_obj[16] as System.Collections.Generic.Dictionary<long, RackStatusOne>;
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary3)
				{
					RackStatusOne value5 = current.Value;
					num29 += value5.Power;
				}
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary3)
				{
					RackStatusOne value5 = current2.Value;
					if (value5.TIntake_min - value5.TFloor < 0.0 || value5.TEquipk < 0.0 || value5.TFloor_avg - value5.TIntake_diff <= 0.0)
					{
						str = str + value5.RackName + ";";
					}
					if (value5.IntakeSSnum == 0 || value5.ExhaustSSnum == 0 || value5.FloorSSnum == 0 || value5.TEquipk_avg <= 0.0 || value5.TFloor_avg <= 0.0 || value5.TFloor_avg - value5.TIntake_diff <= 0.0 || value5.Power == 0.0)
					{
						value5.Power = 0.0;
					}
					else
					{
						num32 += (value5.TFloor_avg - value5.TIntake_diff) * (value5.Power / num29);
						num33 += value5.TEquipk * (value5.Power / num29);
						num30 += value5.VEquipk;
					}
				}
				if (num33 != 0.0 && num32 != 0.0)
				{
					num36 = num32 / num33;
					double num43 = num30 / num36;
					foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current3 in dictionary3)
					{
						RackStatusOne value5 = current3.Value;
						if (value5.Power > 0.0)
						{
							double num44 = num43 * value5.Power / num29;
							num31 += value5.TFloor_avg * num44 / num43;
						}
					}
					num36 *= 100.0;
					num40 = System.Math.Pow(num36 / 100.0, 2.9) * 100.0;
					if (num40 >= 100.0)
					{
						num40 = 100.0;
					}
					num41 = ((num31 < 0.0) ? 0.0 : (100.0 - (num34 - num31) * 2.0));
					num42 = ((num31 < 0.0) ? 0.0 : (100.0 - (num35 - num31) * 2.0));
				}
				this.chtsaving.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
				this.chtsaving.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
				this.chtsaving.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shFanSav, new string[0]);
				this.chtsaving.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
				this.chtsaving.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
				this.chtsaving.ChartAreas[0].AxisY.LabelStyle = new LabelStyle
				{
					Format = "{N2}"
				};
				this.chtsaving.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
				this.chtsaving.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
				this.chtsaving.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
				this.chtsaving.ChartAreas[0].AxisY.Interval = 10.0;
				this.chtsaving.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
				this.chtsaving.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
				this.chtsaving.ChartAreas[0].AxisX.LabelStyle.Format = "";
				this.chtsaving.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
				this.chtsaving.ChartAreas[0].AxisY.IsStartedFromZero = true;
				this.chtsaving.ChartAreas[0].AxisX.IsStartedFromZero = false;
				this.chtsaving.ChartAreas[1].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
				this.chtsaving.ChartAreas[1].AlignmentStyle = AreaAlignmentStyles.Cursor;
				this.chtsaving.ChartAreas[1].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shChilSav, new string[0]);
				this.chtsaving.ChartAreas[1].AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
				this.chtsaving.ChartAreas[1].AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
				this.chtsaving.ChartAreas[1].AxisY.LabelStyle = new LabelStyle
				{
					Format = "{N2}"
				};
				this.chtsaving.ChartAreas[1].AxisX.TitleAlignment = StringAlignment.Center;
				this.chtsaving.ChartAreas[1].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
				this.chtsaving.ChartAreas[1].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
				this.chtsaving.ChartAreas[1].AxisY.Interval = 10.0;
				this.chtsaving.ChartAreas[1].AxisY.MajorGrid.Enabled = true;
				this.chtsaving.ChartAreas[1].AxisX.MajorGrid.Enabled = false;
				this.chtsaving.ChartAreas[1].AxisX.LabelStyle.Format = "";
				this.chtsaving.ChartAreas[1].AxisY.MajorTickMark.Enabled = false;
				this.chtsaving.ChartAreas[1].AxisY.IsStartedFromZero = true;
				this.chtsaving.ChartAreas[1].AxisX.IsStartedFromZero = false;
				this.chtsaving.Series.Clear();
				this.chtsaving.Legends.Clear();
				Series series6 = new Series();
				series6.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shMeasured, new string[0]), new object[]
				{
					100
				});
				series6.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shPotential, new string[0]), new object[]
				{
					num40
				});
				series6.XValueType = ChartValueType.String;
				series6.ChartArea = "ChartArea1";
				series6.ChartType = SeriesChartType.Column;
				series6.Color = System.Drawing.Color.FromArgb(162, 215, 48);
				series6.BorderWidth = 1;
				this.chtsaving.Series.Add(series6);
				Series series7 = new Series();
				series7.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shMeasured, new string[0]), new object[]
				{
					100
				});
				series7.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shPotential, new string[0]), new object[]
				{
					num41
				});
				series7.Points.AddXY(EcoLanguage.getMsg(LangRes.Rpt_shAggressive, new string[0]), new object[]
				{
					num42
				});
				series7.XValueType = ChartValueType.String;
				series7.ChartArea = "ChartArea2";
				series7.ChartType = SeriesChartType.Column;
				series7.Color = System.Drawing.Color.Blue;
				series7.BorderWidth = 1;
				this.chtsaving.Series.Add(series7);
				DataTable dataTable = DBTools.CreateDataTable4ThermalDB("select RCI_High,RCI_Low,RPI_High,RPI_Low,RAI_High,RAI_Low,RHI_High,RHI_Low from rack_effect where insert_time= (select max(insert_time) from  rack_effect)");
				if (dataTable.Rows.Count > 0)
				{
					if (System.Convert.ToString(dataTable.Rows[0]["RCI_High"]) != "")
					{
						num38 = System.Convert.ToDouble(dataTable.Rows[0]["RCI_High"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RPI_High"]) != "")
					{
						num37 = System.Convert.ToDouble(dataTable.Rows[0]["RPI_High"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RAI_High"]) != "")
					{
						System.Convert.ToDouble(dataTable.Rows[0]["RAI_High"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RHI_High"]) != "")
					{
						System.Convert.ToDouble(dataTable.Rows[0]["RHI_High"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RCI_Low"]) != "")
					{
						System.Convert.ToDouble(dataTable.Rows[0]["RCI_Low"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RPI_Low"]) != "")
					{
						System.Convert.ToDouble(dataTable.Rows[0]["RPI_Low"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RAI_Low"]) != "")
					{
						num39 = System.Convert.ToDouble(dataTable.Rows[0]["RAI_Low"]);
					}
					if (System.Convert.ToString(dataTable.Rows[0]["RHI_Low"]) != "")
					{
						System.Convert.ToDouble(dataTable.Rows[0]["RHI_Low"]);
					}
				}
				if (num37 < 90.0 && num37 != -99999.0)
				{
					stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrPress, new string[0]));
				}
				if (num39 < 50.0 && num39 != -99999.0)
				{
					stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrAirRatio, new string[0]));
				}
				if (num36 < 1.0 && num36 != -99999.0)
				{
					if (num36 != 0.0)
					{
						stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrOverVentilate, new string[0]) + " " + ((1.0 / num36 - 1.0) * 100.0).ToString("F2"));
					}
					else
					{
						stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrOverVentilate, new string[0]) + " 100");
					}
				}
				if (num38 < 90.0 && num38 != -99999.0)
				{
					stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrSupplyAir, new string[0]));
				}
				if ((num36 < 90.0 && num36 != -99999.0) || (num36 > 110.0 && num36 != -99999.0))
				{
					stringBuilder2.AppendLine("  " + EcoLanguage.getMsg(LangRes.Rpt_shErrImproveAir, new string[0]));
				}
				this.txtaction.Text = "";
				this.txtaction.Text = stringBuilder2.ToString();
			}
			if (this.m_pParaClass.chkchart7_Checked())
			{
				this.tabchart.TabPages.Add(this.tpcapacity);
				int num45 = (int)System.Convert.ToInt16(this.m_pParaClass.Cboduration);
				int cboperiod_SelectedIndex = this.m_pParaClass.Cboperiod_SelectedIndex;
				this.palcapacity1.Visible = false;
				this.palcapacity2.Visible = false;
				this.palcapacity3.Visible = false;
				this.palcapacity4.Visible = false;
				System.Collections.Generic.Dictionary<long, CommParaClass> dictionary4 = null;
				for (int num46 = 0; num46 < gppara_list.Count; num46++)
				{
					string[] array11 = gppara_list[num46].ToString().Split(new char[]
					{
						'|'
					});
					string device_id = array11[EGenRptShow.AnalysisIndex_devIDs];
					string str2 = array11[EGenRptShow.AnalysisIndex_gpNM];
					string a = array11[EGenRptShow.AnalysisIndex_gpTP];
					string arg_3DDE_0 = array11[EGenRptShow.AnalysisIndex_portIDs];
					if (array2[num46] == null)
					{
						System.Console.WriteLine(" chart7 - groupPPeakTable error !! j=" + num46);
					}
					System.Collections.Generic.Dictionary<string, double> dictionary5 = new System.Collections.Generic.Dictionary<string, double>();
					for (int num47 = 0; num47 < array2[num46].Rows.Count; num47++)
					{
						double num48 = System.Convert.ToDouble(array2[num46].Rows[num47]["power"]);
						dictionary5.Add(System.Convert.ToString(array2[num46].Rows[num47]["period"]), num48);
					}
					if (array[num46] == null)
					{
						System.Console.WriteLine(" chart7 - groupPDTable error !! j=" + num46);
					}
					System.Collections.Generic.Dictionary<string, double> dictionary6 = new System.Collections.Generic.Dictionary<string, double>();
					for (int num49 = 0; num49 < array[num46].Rows.Count; num49++)
					{
						double num50 = System.Convert.ToDouble(array[num46].Rows[num49]["power_consumption"]);
						dictionary6.Add(System.Convert.ToString(array[num46].Rows[num49]["period"]), num50);
					}
					double num51 = 0.0;
					if (a == "zone" || a == "rack" || a == "allrack" || a == "dev" || a == "alldev")
					{
						if (dictionary4 == null)
						{
							dictionary4 = DeviceOperation.GetDeviceModelMapping();
						}
						num51 = this.getSumCapacity_dev(device_id, dictionary4);
					}
					else
					{
						if (a == "alloutlet" || a == "outlet")
						{
							num51 = -100.0;
						}
					}
					DataTable dataTable2 = new DataTable();
					dataTable2.Columns.Add("Capacity", typeof(string));
					dataTable2.Columns.Add("Average_Power", typeof(string));
					dataTable2.Columns.Add("Growth_avg", typeof(string));
					dataTable2.Columns.Add("Peak_Power", typeof(string));
					dataTable2.Columns.Add("Growth_Peak", typeof(string));
					dataTable2.Columns.Add("Time_period", typeof(string));
					double num52 = 0.0;
					double num53 = 0.0;
					for (int num54 = 0; num54 < num45; num54++)
					{
						System.DateTime curdatatime;
						string text15;
						if (cboperiod_SelectedIndex == 0)
						{
							curdatatime = this.m_pParaClass.DTbegin.AddHours((double)num54);
							text15 = curdatatime.ToString("yyyy-MM-dd HH");
						}
						else
						{
							if (cboperiod_SelectedIndex == 1)
							{
								curdatatime = this.m_pParaClass.DTbegin.AddDays((double)num54);
								text15 = curdatatime.ToString("yyyy-MM-dd");
							}
							else
							{
								if (cboperiod_SelectedIndex == 2)
								{
									curdatatime = this.m_pParaClass.DTbegin.AddMonths(num54);
									text15 = curdatatime.ToString("yyyy-MM");
								}
								else
								{
									curdatatime = this.m_pParaClass.DTbegin.AddMonths(3 * num54);
									int num55 = (curdatatime.Month - 1) / 3 + 1;
									text15 = curdatatime.Year.ToString() + "Q" + num55.ToString();
								}
							}
						}
						double spendMinutes2 = this.getSpendMinutes(cboperiod_SelectedIndex, curdatatime);
						DataRow dataRow6 = dataTable2.NewRow();
						if (num51 >= 0.0)
						{
							dataRow6["Capacity"] = num51.ToString("F2");
						}
						else
						{
							dataRow6["Capacity"] = "N/A";
						}
						double num56 = 0.0;
						if (dictionary6.ContainsKey(text15))
						{
							double num50 = dictionary6[text15];
							num56 = num50 * 60.0 / spendMinutes2;
							dataRow6["Average_Power"] = num56.ToString("F4");
							if (num54 == 0)
							{
								dataRow6["Growth_avg"] = "0.00%";
							}
							else
							{
								double num57;
								if (num52 != 0.0)
								{
									num57 = (num56 - num52) * 100.0 / num52;
								}
								else
								{
									num57 = 0.0;
								}
								dataRow6["Growth_avg"] = num57.ToString("F2") + "%";
							}
							num52 = num56;
						}
						else
						{
							dataRow6["Average_Power"] = "0.00";
							dataRow6["Growth_avg"] = "0.00%";
						}
						if (dictionary5.ContainsKey(text15))
						{
							double num48 = dictionary5[text15];
							if (num48 < num56)
							{
								num48 = this.calPeakPower(num56, num48);
							}
							dataRow6["Peak_Power"] = num48.ToString("F4");
							if (num54 == 0)
							{
								dataRow6["Growth_Peak"] = "0.00%";
							}
							else
							{
								double num58;
								if (num53 != 0.0)
								{
									num58 = (num48 - num53) * 100.0 / num53;
								}
								else
								{
									num58 = 0.0;
								}
								dataRow6["Growth_Peak"] = num58.ToString("F2") + "%";
							}
							num53 = num48;
						}
						else
						{
							dataRow6["Peak_Power"] = "0.00";
							dataRow6["Growth_Peak"] = "0.00%";
						}
						dataRow6["Time_period"] = text15;
						dataTable2.Rows.Add(dataRow6);
					}
					Panel panel = (Panel)this.tpcapacity.Controls.Find("palcapacity" + (num46 + 1), false)[0];
					Label label = (Label)panel.Controls.Find("labcapacity" + (num46 + 1), false)[0];
					DataGridView dataGridView = (DataGridView)panel.Controls.Find("dgvcapacity" + (num46 + 1), false)[0];
					Chart chart = (Chart)panel.Controls.Find("chtcapacity" + (num46 + 1), false)[0];
					panel.Visible = true;
					label.Text = str2 + " " + EcoLanguage.getMsg(LangRes.Rpt_shITPowercapacity, new string[0]);
					int x = (738 - label.Size.Width) / 2;
					label.Location = new System.Drawing.Point(x, 8);
					dataGridView.Columns.Clear();
					dataGridView.Rows.Clear();
					for (int num59 = 0; num59 < dataTable2.Rows.Count + 1; num59++)
					{
						dataGridView.Columns.Add("col" + num59.ToString(), "col" + num59.ToString());
					}
					for (int num60 = 0; num60 < dataTable2.Columns.Count; num60++)
					{
						string[] array12 = new string[dataTable2.Rows.Count + 1];
						if (num60 == 0)
						{
							array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shCapacity, new string[0]) + "(kW)";
						}
						else
						{
							if (num60 == 1)
							{
								array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shAvgPower, new string[0]) + "(kW)";
							}
							else
							{
								if (num60 == 2)
								{
									array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shGrowthAvg, new string[0]);
								}
								else
								{
									if (num60 == 3)
									{
										array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shPeakPower, new string[0]) + "(kW)";
									}
									else
									{
										if (num60 == 4)
										{
											array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shGrowthPeak, new string[0]);
										}
										else
										{
											if (num60 == 5)
											{
												array12[0] = EcoLanguage.getMsg(LangRes.Rpt_shTimePeriod, new string[0]);
											}
										}
									}
								}
							}
						}
						for (int num61 = 1; num61 < array12.Length; num61++)
						{
							if (num60 == 0)
							{
								array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Capacity"]);
							}
							else
							{
								if (num60 == 1)
								{
									array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Average_Power"]);
								}
								else
								{
									if (num60 == 2)
									{
										array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Growth_avg"]);
									}
									else
									{
										if (num60 == 3)
										{
											array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Peak_Power"]);
										}
										else
										{
											if (num60 == 4)
											{
												array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Growth_Peak"]);
											}
											else
											{
												if (num60 == 5)
												{
													array12[num61] = System.Convert.ToString(dataTable2.Rows[num61 - 1]["Time_period"]);
												}
											}
										}
									}
								}
							}
						}
						dataGridView.Rows.Add(array12);
					}
					chart.ChartAreas.Clear();
					chart.ChartAreas.Add(new ChartArea());
					chart.ChartAreas[0].BackColor = System.Drawing.Color.WhiteSmoke;
					chart.ChartAreas[0].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
					chart.ChartAreas[0].AlignmentStyle = AreaAlignmentStyles.Cursor;
					chart.ChartAreas[0].AxisX.Title = EcoLanguage.getMsg(LangRes.Rpt_shTimePeriod, new string[0]);
					chart.ChartAreas[0].AxisY.Title = "kW";
					chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
					chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
					chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(64, 64, 64, 64);
					chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
					chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
					chart.ChartAreas[0].AxisY.IsStartedFromZero = true;
					chart.ChartAreas[0].AxisX.IsStartedFromZero = false;
					chart.ChartAreas[0].AxisY.Maximum = double.NaN;
					chart.Series.Clear();
					chart.Legends.Clear();
					Legend legend6 = new Legend();
					legend6.Alignment = StringAlignment.Far;
					legend6.Docking = Docking.Top;
					legend6.LegendStyle = LegendStyle.Table;
					legend6.Name = "leg1";
					legend6.DockedToChartArea = "ChartArea1";
					legend6.IsDockedInsideChartArea = false;
					chart.Legends.Add(legend6);
					Series series8 = new Series();
					DataView dataView = new DataView(dataTable2);
					dataView.Sort = "Time_period asc";
					series8.Points.DataBindXY(dataView, "Time_period", dataView, "Capacity");
					series8.XValueType = ChartValueType.String;
					series8.ChartArea = "ChartArea1";
					series8.ChartType = SeriesChartType.Column;
					series8.Color = System.Drawing.Color.FromArgb(153, 204, 255);
					series8.BorderWidth = 2;
					series8.LegendText = EcoLanguage.getMsg(LangRes.Rpt_shCapacity, new string[0]);
					series8.Legend = "leg1";
					series8.IsVisibleInLegend = true;
					chart.Series.Add(series8);
					Series series9 = new Series();
					dataView.Sort = "Time_period asc";
					series9.Points.DataBindXY(dataView, "Time_period", dataView, "Peak_Power");
					series9.XValueType = ChartValueType.String;
					series9.ChartArea = "ChartArea1";
					series9.ChartType = SeriesChartType.Column;
					series9.Color = System.Drawing.Color.FromArgb(204, 153, 255);
					series9.BorderWidth = 2;
					series9.LegendText = EcoLanguage.getMsg(LangRes.Rpt_shPeakPower, new string[0]);
					series9.Legend = "leg1";
					series9.IsVisibleInLegend = true;
					chart.Series.Add(series9);
					Series series10 = new Series();
					dataView.Sort = "Time_period asc";
					series10.Points.DataBindXY(dataView, "Time_period", dataView, "Average_Power");
					series10.XValueType = ChartValueType.String;
					series10.ChartArea = "ChartArea1";
					series10.ChartType = SeriesChartType.Column;
					series10.Color = System.Drawing.Color.FromArgb(255, 255, 153);
					series10.BorderWidth = 2;
					series10.LegendText = EcoLanguage.getMsg(LangRes.Rpt_shAvgPower, new string[0]);
					series10.Legend = "leg1";
					series10.IsVisibleInLegend = true;
					chart.Series.Add(series10);
				}
			}
			if (this.m_pParaClass.chkchart8_Checked())
			{
				System.Collections.Generic.Dictionary<long, CommParaClass> deviceRackMapping = DeviceOperation.GetDeviceRackMapping();
				this.tabchart.TabPages.Add(this.tplist);
				num = this.m_pParaClass.price_elec();
				num2 = this.m_pParaClass.Co2_elec();
				num3 = this.m_pParaClass.price_Co2();
				DataTable dataTable3 = new DataTable();
				dataTable3.Columns.Add("Group", typeof(string));
				dataTable3.Columns.Add("Rack_Name", typeof(string));
				dataTable3.Columns.Add("Outlet", typeof(string));
				dataTable3.Columns.Add("Peak_Watts", typeof(double));
				dataTable3.Columns.Add("Power_Dissipation", typeof(double));
				dataTable3.Columns.Add("Peak_CO2", typeof(double));
				dataTable3.Columns.Add("Power_Cost", typeof(double));
				dataTable3.Columns.Add("CO2_Cost", typeof(double));
				for (int num62 = 0; num62 < gppara_list.Count; num62++)
				{
					string[] array13 = gppara_list[num62].ToString().Split(new char[]
					{
						'|'
					});
					string arg_4C9D_0 = array13[EGenRptShow.AnalysisIndex_devIDs];
					string text16 = array13[EGenRptShow.AnalysisIndex_gpNM];
					string arg_4CB0_0 = array13[EGenRptShow.AnalysisIndex_gpTP];
					string arg_4CB9_0 = array13[EGenRptShow.AnalysisIndex_portIDs];
					string arg_4CC2_0 = array13[EGenRptShow.AnalysisIndex_invaliddevIDs];
					System.DateTime now = System.DateTime.Now;
					if (array3[num62] == null)
					{
						System.Console.WriteLine(" chart8 - groupOutLetPowerAndName error !! j=" + num62);
					}
					System.DateTime now2 = System.DateTime.Now;
					System.TimeSpan timeSpan = now2 - now;
					System.Collections.Generic.Dictionary<string, string> dictionary7 = new System.Collections.Generic.Dictionary<string, string>();
					for (int num63 = 0; num63 < array3[num62].Rows.Count; num63++)
					{
						string key7 = System.Convert.ToString(array3[num62].Rows[num63]["pdu_id"]) + "-" + System.Convert.ToString(array3[num62].Rows[num63]["server_id"]);
						string value6 = System.Convert.ToString(array3[num62].Rows[num63]["server_name"]) + "|" + System.Convert.ToDouble(array3[num62].Rows[num63]["power"]);
						dictionary7.Add(key7, value6);
					}
					System.DateTime arg_4DCB_0 = System.DateTime.Now;
					if (array4[num62] == null)
					{
						System.Console.WriteLine(" chart8 - groupOutLetPDAndName error !! j=" + num62);
					}
					System.DateTime now3 = System.DateTime.Now;
					System.Collections.Generic.Dictionary<string, double> dictionary8 = new System.Collections.Generic.Dictionary<string, double>();
					for (int num64 = 0; num64 < array4[num62].Rows.Count; num64++)
					{
						string key8 = System.Convert.ToString(array4[num62].Rows[num64]["pdu_id"]) + "-" + System.Convert.ToString(array4[num62].Rows[num64]["server_id"]);
						double value7 = System.Convert.ToDouble(array4[num62].Rows[num64]["power_consumption"]);
						dictionary8.Add(key8, value7);
					}
					System.DateTime now4 = System.DateTime.Now;
					timeSpan = now4 - now3;
					commUtil.ShowInfo_DEBUG(string.Concat(new object[]
					{
						" GetRackNameAndDeviceID --- j=",
						num62,
						" ",
						now4.ToString("yyyy-MM-dd HH:mm:ss:fff"),
						"  Spend = ",
						timeSpan.TotalMilliseconds
					}));
					double minutesinscope = this.getMinutesinscope(this.m_pParaClass.Cboperiod_SelectedIndex, this.m_pParaClass.strBegin, this.m_pParaClass.strEnd);
					foreach (System.Collections.Generic.KeyValuePair<string, string> current4 in dictionary7)
					{
						string key9 = current4.Key;
						double num65 = 0.0;
						if (dictionary8.ContainsKey(key9))
						{
							num65 = System.Convert.ToDouble(dictionary8[key9]);
						}
						string text17 = System.Convert.ToString(current4.Value).Split(new char[]
						{
							'|'
						})[0];
						if (text17 == "\0")
						{
							text17 = "";
						}
						double curpeak_power = System.Convert.ToDouble(current4.Value.Split(new char[]
						{
							'|'
						})[1]);
						double curavg_power = num65 * 60.0 / minutesinscope;
						curpeak_power = this.calPeakPower(curavg_power, curpeak_power);
						string text18 = "";
						long key10 = System.Convert.ToInt64(key9.Split(new char[]
						{
							'-'
						})[0]);
						if (deviceRackMapping.ContainsKey(key10))
						{
							text18 = deviceRackMapping[key10].String_First;
						}
						dataTable3.Rows.Add(new object[]
						{
							text16,
							text18,
							text17,
							curpeak_power.ToString("F4"),
							num65.ToString("F4"),
							(num65 * num2).ToString("F2"),
							(num65 * num).ToString("F2"),
							(num65 * num2 * num3 / 1000.0).ToString("F2")
						});
					}
					System.DateTime now5 = System.DateTime.Now;
					timeSpan = now5 - now4;
					commUtil.ShowInfo_DEBUG(string.Concat(new object[]
					{
						" Fill table ---------- j=",
						num62,
						" ",
						now5.ToString("yyyy-MM-dd HH:mm:ss:fff"),
						"  Spend = ",
						timeSpan.TotalMilliseconds
					}));
				}
				commUtil.ShowInfo_DEBUG(" Start  Draw chart 8------- " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				this.dgvinventory.AutoGenerateColumns = false;
				this.dgvinventory.DataSource = null;
				this.dgvinventory.DataSource = dataTable3;
				this.dgvinventory.Columns[0].DataPropertyName = "Group";
				this.dgvinventory.Columns[1].DataPropertyName = "Rack_Name";
				this.dgvinventory.Columns[2].DataPropertyName = "Outlet";
				this.dgvinventory.Columns[3].DataPropertyName = "Peak_Watts";
				this.dgvinventory.Columns[4].DataPropertyName = "Power_Dissipation";
				this.dgvinventory.Columns[5].DataPropertyName = "Peak_CO2";
				this.dgvinventory.Columns[6].DataPropertyName = "Power_Cost";
				this.dgvinventory.Columns[6].HeaderText = string.Format(this.m_dgvinventory_Col6Txt, EcoGlobalVar.CurCurrency);
				this.dgvinventory.Columns[7].DataPropertyName = "CO2_Cost";
				this.dgvinventory.Columns[7].HeaderText = string.Format(this.m_dgvinventory_Col7Txt, EcoGlobalVar.CurCurrency);
				commUtil.ShowInfo_DEBUG(" Finish Draw chart 8------- " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			}
			if (this.m_pParaClass.chkchart9_Checked())
			{
				this.tabchart.TabPages.Add(this.tppue);
				DataTable dataTable4 = new DataTable();
				DataColumn dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "gpnm";
				dataTable4.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Double");
				dataColumn.ColumnName = "totalpower";
				dataTable4.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Double");
				dataColumn.ColumnName = "itpower";
				dataTable4.Columns.Add(dataColumn);
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Double");
				dataColumn.ColumnName = "pue";
				dataTable4.Columns.Add(dataColumn);
				for (int num66 = 0; num66 < gppara_list.Count; num66++)
				{
					string[] array14 = gppara_list[num66].ToString().Split(new char[]
					{
						'|'
					});
					string arg_5413_0 = array14[EGenRptShow.AnalysisIndex_devIDs];
					string value8 = array14[EGenRptShow.AnalysisIndex_gpNM];
					string arg_5426_0 = array14[EGenRptShow.AnalysisIndex_gpTP];
					string arg_542F_0 = array14[EGenRptShow.AnalysisIndex_portIDs];
					string value9 = array14[EGenRptShow.AnalysisIndex_gpPUE];
					if (array[num66] == null)
					{
						System.Console.WriteLine(" chart9 - groupPDTable error !! j=" + num66);
					}
					double num67 = 0.0;
					foreach (DataRow dataRow7 in array[num66].Rows)
					{
						try
						{
							num67 += System.Convert.ToDouble(dataRow7["power_consumption"]);
						}
						catch
						{
						}
					}
					double num68 = System.Convert.ToDouble(value9);
					DataRow dataRow8 = dataTable4.NewRow();
					dataRow8["gpnm"] = value8;
					dataRow8["totalpower"] = num68.ToString("F4");
					dataRow8["itpower"] = num67.ToString("F4");
					if (num67 > 0.0)
					{
						dataRow8["pue"] = (num68 / num67).ToString("F4");
					}
					else
					{
						dataRow8["pue"] = 0;
					}
					dataTable4.Rows.Add(dataRow8);
				}
				this.dgvPUE.AutoGenerateColumns = false;
				this.dgvPUE.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
				this.dgvPUE.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, FontStyle.Regular);
				this.dgvPUE.DataSource = dataTable4.DefaultView;
				this.dgvPUE.Columns[0].DataPropertyName = "gpnm";
				this.dgvPUE.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dgvPUE.Columns[1].DataPropertyName = "totalpower";
				this.dgvPUE.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dgvPUE.Columns[2].DataPropertyName = "itpower";
				this.dgvPUE.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dgvPUE.Columns[3].DataPropertyName = "pue";
				this.dgvPUE.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}
			commUtil.ShowInfo_DEBUG(" Finish Draw All chart----- " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\n");
		}
		private double getSpendMinutes(int rptPeriod, string strdatatime)
		{
			switch (rptPeriod)
			{
			case 0:
				return 60.0;
			case 1:
				return 1440.0;
			case 2:
			{
				System.DateTime dateTime = System.Convert.ToDateTime(strdatatime);
				System.DateTime d = new System.DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
				System.DateTime d2 = d.AddMonths(1);
				return (d2 - d).TotalMinutes;
			}
			default:
			{
				int num = System.Convert.ToInt32(strdatatime.Split(new char[]
				{
					'Q'
				})[1]);
				num = 3 * (num - 1) + 1;
				int year = System.Convert.ToInt32(strdatatime.Split(new char[]
				{
					'Q'
				})[0]);
				System.DateTime d = new System.DateTime(year, num, 1, 0, 0, 0);
				System.DateTime d2 = d.AddMonths(3);
				return (d2 - d).TotalMinutes;
			}
			}
		}
		private double getSpendMinutes(int rptPeriod, System.DateTime curdatatime)
		{
			switch (rptPeriod)
			{
			case 0:
				return 60.0;
			case 1:
				return 1440.0;
			case 2:
			{
				double num = (double)System.DateTime.DaysInMonth(curdatatime.Year, curdatatime.Month);
				return num * 24.0 * 60.0;
			}
			default:
			{
				int month = curdatatime.Month;
				int month2 = (month - 1) / 3 * 3 + 1;
				System.DateTime d = new System.DateTime(curdatatime.Year, month2, 1, 0, 0, 0);
				System.DateTime d2 = d.AddMonths(3);
				return (d2 - d).TotalMinutes;
			}
			}
		}
		private double getMinutesinscope(int rptPeriod, string strBegin, string strEnd)
		{
			System.DateTime d = System.Convert.ToDateTime(strBegin);
			System.DateTime d2 = System.Convert.ToDateTime(strEnd);
			return (d2 - d).TotalMinutes;
		}
		private double getSumCapacity_dev(string device_id, System.Collections.Generic.Dictionary<long, CommParaClass> devID2Model)
		{
			double num = 0.0;
			string[] array = device_id.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				long key = System.Convert.ToInt64(value);
				if (devID2Model.ContainsKey(key))
				{
					CommParaClass commParaClass = devID2Model[key];
					string string_First = commParaClass.String_First;
					string string_ = commParaClass.String_2;
					string text = DevAccessCfg.GetInstance().getDeviceModelConfig(string_First, string_).devcapacity;
					if (!text.Contains("V"))
					{
						num += (double)System.Convert.ToInt32(text);
					}
					else
					{
						text = text.Replace("V", "");
						num += (double)((float)System.Convert.ToInt32(text) * this.m_energyV);
					}
				}
			}
			return num / 1000.0;
		}
		private double getSumCapacity_port(string portids, System.Collections.Generic.Dictionary<long, CommParaClass> portID2Model)
		{
			double num = 0.0;
			string[] array = portids.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				long key = System.Convert.ToInt64(value);
				if (portID2Model.ContainsKey(key))
				{
					CommParaClass commParaClass = portID2Model[key];
					string string_First = commParaClass.String_First;
					string string_ = commParaClass.String_2;
					int integer_First = commParaClass.Integer_First;
					string text = DevAccessCfg.GetInstance().getDeviceModelConfig(string_First, string_).devcapacity;
					if (!text.Contains("V"))
					{
						num += (double)(System.Convert.ToInt32(text) / integer_First);
					}
					else
					{
						text = text.Replace("V", "");
						num += (double)((float)System.Convert.ToInt32(text) * this.m_energyV / (float)integer_First);
					}
				}
			}
			return num / 1000.0;
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
			string text = Sys_Para.GetDefinePath();
			if (text.Length == 0)
			{
				text = System.IO.Directory.GetCurrentDirectory() + "\\PWReportFile\\";
			}
			System.DateTime now = System.DateTime.Now;
			int num = this.SaveToFile(text + now.ToString("yyyy-MM-dd HH-mm-ss"), this.m_pParaClass.Txttitle);
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
			string str = text + now.ToString("yyyy-MM-dd HH-mm-ss") + "\\" + this.m_pParaClass.Txttitle.Replace("'", "''");
			if (ReportInfo.InsertReport(this.m_pParaClass.Txttitle, this.m_pParaClass.Txtwriter, now, str + ".html") == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			this.m_isRPTSaved = true;
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private int SaveToFile(string path, string name)
		{
			this.m_pParaClass.m_savepath = path;
			System.Collections.ArrayList gppara_list = this.m_pParaClass.gppara_list;
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
				commUtil.ShowInfo_DEBUG(" Save ------- jpg == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				if (this.m_pParaClass.chkchart1_Checked() || this.m_pParaClass.chkchart2_Checked() || this.m_pParaClass.chkchart3_Checked() || this.m_pParaClass.chkchart4_Checked() || this.m_pParaClass.chkchart5_Checked())
				{
					this.chart1.SaveImage(path + "\\line.jpg", ChartImageFormat.Jpeg);
					if (this.m_pParaClass.chkchart1_Checked())
					{
						this.exportCSV(path, name, "ITPowerUsage", this.csv_chart1);
					}
					if (this.m_pParaClass.chkchart2_Checked())
					{
						this.exportCSV(path, name, "ITPowerLoad", this.csv_chart2);
					}
					if (this.m_pParaClass.chkchart3_Checked())
					{
						this.exportCSV(path, name, "ITCO2Equivalent", this.csv_chart3);
					}
					if (this.m_pParaClass.chkchart4_Checked())
					{
						this.exportCSV(path, name, "ITPowerConsumptionCost", this.csv_chart4);
					}
					if (this.m_pParaClass.chkchart5_Checked())
					{
						this.exportCSV(path, name, "ITCO2EmissionCost", this.csv_chart5);
					}
				}
				if (this.m_pParaClass.chkchart6_Checked())
				{
					this.chtsaving.SaveImage(path + "\\saving.jpg", ChartImageFormat.Jpeg);
				}
				if (this.m_pParaClass.chkchart7_Checked())
				{
					if (gppara_list.Count >= 1)
					{
						this.chtcapacity1.SaveImage(path + "\\capacity1.jpg", ChartImageFormat.Jpeg);
					}
					if (gppara_list.Count >= 2)
					{
						this.chtcapacity2.SaveImage(path + "\\capacity2.jpg", ChartImageFormat.Jpeg);
					}
					if (gppara_list.Count >= 3)
					{
						this.chtcapacity3.SaveImage(path + "\\capacity3.jpg", ChartImageFormat.Jpeg);
					}
					if (gppara_list.Count >= 4)
					{
						this.chtcapacity4.SaveImage(path + "\\capacity4.jpg", ChartImageFormat.Jpeg);
					}
				}
				commUtil.ShowInfo_DEBUG(" Save --End-- jpg == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			}
			catch
			{
				int result = -2;
				return result;
			}
			progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_saving, new string[0]), null, new progressPopup.ProcessInThread(this.exportProc), this.m_pParaClass, 0);
			progressPopup.ShowDialog();
			return 0;
		}
		private object exportProc(object param)
		{
			string savepath = this.m_pParaClass.m_savepath;
			string txttitle = this.m_pParaClass.Txttitle;
			System.Collections.ArrayList gppara_list = this.m_pParaClass.gppara_list;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			stringBuilder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			stringBuilder.AppendLine("<head>");
			stringBuilder.AppendLine("<title>" + EcoLanguage.getMsg(LangRes.Rpt_savePowerAnalysis, new string[0]) + "</title>");
			stringBuilder.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
			stringBuilder.AppendLine("</head>");
			stringBuilder.AppendLine("<body >");
			if (this.m_pParaClass.chkchart1_Checked() || this.m_pParaClass.chkchart2_Checked() || this.m_pParaClass.chkchart3_Checked() || this.m_pParaClass.chkchart4_Checked() || this.m_pParaClass.chkchart5_Checked())
			{
				stringBuilder.AppendLine("<p align='center'><img src='line.jpg'/></p>");
			}
			if (this.m_pParaClass.chkchart6_Checked())
			{
				stringBuilder.AppendLine("<p align='center' style='font-weight:bold;font-size:15pt;'>" + EcoLanguage.getMsg(LangRes.Rpt_saveSuggestion, new string[0]) + "</p>");
				stringBuilder.AppendLine("<p align='center'><img src='saving.jpg'/></p>");
				string[] array = this.txtaction.Text.Replace("\r\n", "|").Split(new char[]
				{
					'|'
				});
				stringBuilder.AppendLine("<p align='center'><table  width='500'>");
				for (int i = 0; i < array.Length; i++)
				{
					if (i == 0)
					{
						stringBuilder.AppendLine("<tr height='25px' align='left'><td style='font-weight:bold;font-size:13pt;'>" + array[i] + "</td></tr>");
					}
					else
					{
						stringBuilder.AppendLine("<tr height='25px' align='left'><td>&nbsp;&nbsp;&nbsp;&nbsp" + array[i] + "</td></tr>");
					}
				}
				stringBuilder.AppendLine("</table></p>");
			}
			if (this.m_pParaClass.chkchart7_Checked())
			{
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				for (int j = 0; j < gppara_list.Count; j++)
				{
					Panel panel = (Panel)this.tpcapacity.Controls.Find("palcapacity" + (j + 1), false)[0];
					Label label = (Label)panel.Controls.Find("labcapacity" + (j + 1), false)[0];
					DataGridView dataGridView = (DataGridView)panel.Controls.Find("dgvcapacity" + (j + 1), false)[0];
					string str = "capacity" + (j + 1) + ".jpg";
					string[] array2 = gppara_list[j].ToString().Split(new char[]
					{
						'|'
					});
					if (j == 0)
					{
						stringBuilder2.Append("\"" + dataGridView.Rows[5].Cells[0].Value + "\"");
						stringBuilder2.Append("\t\"" + EcoLanguage.getMsg(LangRes.Rpt_shGroupName, new string[0]) + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[0].Cells[0].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[1].Cells[0].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[2].Cells[0].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[3].Cells[0].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[4].Cells[0].Value + "\"");
						stringBuilder2.AppendLine();
					}
					string str2 = array2[EGenRptShow.AnalysisIndex_gpNM];
					for (int k = 1; k < dataGridView.ColumnCount; k++)
					{
						stringBuilder2.Append("\"" + dataGridView.Rows[5].Cells[k].Value + "\"");
						stringBuilder2.Append("\t\"" + str2 + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[0].Cells[k].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[1].Cells[k].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[2].Cells[k].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[3].Cells[k].Value + "\"");
						stringBuilder2.Append("\t\"" + dataGridView.Rows[4].Cells[k].Value + "\"");
						stringBuilder2.AppendLine();
					}
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"<p><table align='center'  border='1'><tr height='30px' align='center'><td style='font-weight:bold;font-size:13pt;' colspan='",
						dataGridView.ColumnCount,
						"'>",
						label.Text,
						"</td></tr>"
					}));
					for (int l = 0; l < dataGridView.Rows.Count; l++)
					{
						stringBuilder.AppendLine("<tr height='30px'>");
						for (int m = 0; m < dataGridView.ColumnCount; m++)
						{
							stringBuilder.AppendLine("<td>" + dataGridView.Rows[l].Cells[m].Value + "</td>");
						}
						stringBuilder.AppendLine("</tr>");
					}
					stringBuilder.AppendLine("</table></p>");
					stringBuilder.AppendLine("<p align='center'><img src='" + str + "'/></p>");
				}
				this.exportCSV(savepath, txttitle, "Capacity", stringBuilder2);
			}
			if (this.m_pParaClass.chkchart8_Checked())
			{
				System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"<p><table align='center' width='900px' border='1'><tr height='30px' ><td align='center' style='font-weight:bold;font-size:15pt;' colspan='",
					this.dgvinventory.ColumnCount,
					"'>",
					this.label4.Text,
					"</td></tr>"
				}));
				for (int n = 0; n < this.dgvinventory.ColumnCount; n++)
				{
					stringBuilder.AppendLine("<td align='center'>" + this.dgvinventory.Columns[n].HeaderText + "</td>");
					if (n > 0)
					{
						stringBuilder3.Append("\t");
					}
					stringBuilder3.Append("\"" + this.dgvinventory.Columns[n].HeaderText + "\"");
				}
				stringBuilder3.AppendLine();
				for (int num = 0; num < this.dgvinventory.Rows.Count; num++)
				{
					stringBuilder.AppendLine("<tr height='30px'>");
					for (int num2 = 0; num2 < this.dgvinventory.ColumnCount; num2++)
					{
						if (this.dgvinventory.Rows[num].Cells[num2].Value.ToString().Length == 0)
						{
							stringBuilder.AppendLine("<td>&nbsp;</td>");
						}
						else
						{
							stringBuilder.AppendLine("<td>" + this.dgvinventory.Rows[num].Cells[num2].Value + "</td>");
						}
						if (num2 > 0)
						{
							stringBuilder3.Append("\t");
						}
						stringBuilder3.Append("\"" + this.dgvinventory.Rows[num].Cells[num2].Value + "\"");
					}
					stringBuilder.AppendLine("</tr>");
					stringBuilder3.AppendLine();
				}
				stringBuilder.AppendLine("</table></p>");
				this.exportCSV(savepath, txttitle, "Inventory", stringBuilder3);
			}
			if (this.m_pParaClass.chkchart9_Checked())
			{
				System.Text.StringBuilder stringBuilder4 = new System.Text.StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"<p><table align='center' width='900px' border='1'><tr height='30px' ><td align='center' style='font-weight:bold;font-size:15pt;' colspan='",
					this.dgvPUE.ColumnCount,
					"'>",
					this.labelPUE.Text,
					"</td></tr>"
				}));
				for (int num3 = 0; num3 < this.dgvPUE.ColumnCount; num3++)
				{
					stringBuilder.AppendLine("<td align='center'>" + this.dgvPUE.Columns[num3].HeaderText + "</td>");
					if (num3 > 0)
					{
						stringBuilder4.Append("\t");
					}
					stringBuilder4.Append("\"" + this.dgvPUE.Columns[num3].HeaderText + "\"");
				}
				stringBuilder4.AppendLine();
				for (int num4 = 0; num4 < this.dgvPUE.Rows.Count; num4++)
				{
					stringBuilder.AppendLine("<tr height='30px'>");
					for (int num5 = 0; num5 < this.dgvPUE.ColumnCount; num5++)
					{
						if (this.dgvPUE.Rows[num4].Cells[num5].Value.ToString().Length == 0)
						{
							stringBuilder.AppendLine("<td>&nbsp;</td>");
						}
						else
						{
							stringBuilder.AppendLine("<td>" + this.dgvPUE.Rows[num4].Cells[num5].Value + "</td>");
						}
						if (num5 > 0)
						{
							stringBuilder4.Append("\t");
						}
						stringBuilder4.Append("\"" + this.dgvPUE.Rows[num4].Cells[num5].Value + "\"");
					}
					stringBuilder.AppendLine("</tr>");
					stringBuilder4.AppendLine();
				}
				stringBuilder.AppendLine("</table></p>");
				this.exportCSV(savepath, txttitle, "PUE", stringBuilder4);
			}
			stringBuilder.AppendLine("</body>");
			stringBuilder.AppendLine("</html>");
			commUtil.ShowInfo_DEBUG(" Save --html (" + txttitle + ")---- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			string path = savepath + "\\" + txttitle + ".html";
			System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.GetEncoding("UTF-8"));
			streamWriter.Flush();
			streamWriter.BaseStream.Seek(0L, System.IO.SeekOrigin.Begin);
			streamWriter.WriteLine(stringBuilder.ToString());
			streamWriter.Flush();
			streamWriter.Close();
			commUtil.ShowInfo_DEBUG(" Save --html (" + txttitle + ")---- End   == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
			if (EcoLanguage.getLang() == 0)
			{
				this.exportPDF(savepath, txttitle);
			}
			return null;
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
			System.Collections.ArrayList gppara_list = this.m_pParaClass.gppara_list;
			PDF pDF = new PDF(new System.IO.BufferedStream(new System.IO.FileStream(path + "\\" + name + ".pdf", System.IO.FileMode.Create)));
			PDFjet.NET.Font font = new PDFjet.NET.Font(pDF, CoreFont.TIMES_BOLD);
			font.SetSize(7f);
			PDFjet.NET.Font font2 = new PDFjet.NET.Font(pDF, CoreFont.TIMES_ROMAN);
			font2.SetSize(7f);
			int num = 1;
			if (this.m_pParaClass.chkchart1_Checked() || this.m_pParaClass.chkchart2_Checked() || this.m_pParaClass.chkchart3_Checked() || this.m_pParaClass.chkchart4_Checked() || this.m_pParaClass.chkchart5_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C1- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				Page page = new Page(pDF, Letter.PORTRAIT);
				pdfUtil.DrawImg(pDF, page, path + "\\line.jpg", 0f);
				pdfUtil.DrawPageNumber(pDF, page, num++);
			}
			if (this.m_pParaClass.chkchart6_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C6- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				Page page = new Page(pDF, Letter.PORTRAIT);
				pdfUtil.DrawTitle(pDF, page, EcoLanguage.getMsg(LangRes.Rpt_saveSuggestion, new string[0]), 0f, 12f, 1048576);
				float num2 = pdfUtil.DrawImg(pDF, page, path + "\\saving.jpg", 30f);
				System.Collections.Generic.List<System.Collections.Generic.List<Cell>> list = new System.Collections.Generic.List<System.Collections.Generic.List<Cell>>();
				string[] array = this.txtaction.Text.Replace("\r\n", "|").Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					System.Collections.Generic.List<Cell> list2 = new System.Collections.Generic.List<Cell>();
					string text = array[i];
					if (text.Length != 0)
					{
						text = text.Replace(";", "; ");
						Cell cell;
						if (i == 0)
						{
							cell = new Cell(font, text);
						}
						else
						{
							cell = new Cell(font2, "    " + text);
						}
						cell.SetTopPadding(5f);
						cell.SetBottomPadding(5f);
						cell.SetLeftPadding(2f);
						cell.SetRightPadding(2f);
						list2.Add(cell);
						list.Add(list2);
					}
				}
				System.Collections.Generic.List<float> list3 = new System.Collections.Generic.List<float>();
				list3.Add(pdfUtil.PDFpageeffwidth);
				Page page2 = pdfUtil.DrawTable(pDF, page, ref num, list, Table.DATA_HAS_0_HEADER_ROWS, 30f + num2 + 5f, list3);
				pdfUtil.DrawPageNumber(pDF, page2, num++);
			}
			if (this.m_pParaClass.chkchart7_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C7- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				for (int j = 0; j < gppara_list.Count; j++)
				{
					Panel panel = (Panel)this.tpcapacity.Controls.Find("palcapacity" + (j + 1), false)[0];
					Label label = (Label)panel.Controls.Find("labcapacity" + (j + 1), false)[0];
					DataGridView dataGridView = (DataGridView)panel.Controls.Find("dgvcapacity" + (j + 1), false)[0];
					string str = "capacity" + (j + 1) + ".jpg";
					Page page = new Page(pDF, Letter.PORTRAIT);
					pdfUtil.DrawTitle(pDF, page, label.Text, 0f, 12f, 1048576);
					float num3 = pdfUtil.DrawImg(pDF, page, path + "\\" + str, 30f);
					gppara_list[j].ToString().Split(new char[]
					{
						'|'
					});
					System.Collections.Generic.List<System.Collections.Generic.List<Cell>> list4 = new System.Collections.Generic.List<System.Collections.Generic.List<Cell>>();
					list4.Add(new System.Collections.Generic.List<Cell>
					{
						new Cell(font, dataGridView.Rows[5].Cells[0].Value.ToString()),
						new Cell(font, dataGridView.Rows[0].Cells[0].Value.ToString()),
						new Cell(font, dataGridView.Rows[1].Cells[0].Value.ToString()),
						new Cell(font, dataGridView.Rows[2].Cells[0].Value.ToString()),
						new Cell(font, dataGridView.Rows[3].Cells[0].Value.ToString()),
						new Cell(font, dataGridView.Rows[4].Cells[0].Value.ToString())
					});
					for (int k = 1; k < dataGridView.ColumnCount; k++)
					{
						list4.Add(new System.Collections.Generic.List<Cell>
						{
							new Cell(font2, dataGridView.Rows[5].Cells[k].Value.ToString()),
							new Cell(font2, dataGridView.Rows[0].Cells[k].Value.ToString()),
							new Cell(font2, dataGridView.Rows[1].Cells[k].Value.ToString()),
							new Cell(font2, dataGridView.Rows[2].Cells[k].Value.ToString()),
							new Cell(font2, dataGridView.Rows[3].Cells[k].Value.ToString()),
							new Cell(font2, dataGridView.Rows[4].Cells[k].Value.ToString())
						});
					}
					System.Collections.Generic.List<float> list5 = new System.Collections.Generic.List<float>();
					list5.Add(80f);
					list5.Add(84f);
					list5.Add(84f);
					list5.Add(84f);
					list5.Add(84f);
					list5.Add(pdfUtil.PDFpageeffwidth - 416f);
					Page page3 = pdfUtil.DrawTable(pDF, page, ref num, list4, Table.DATA_HAS_1_HEADER_ROWS, 30f + num3 + 5f, list5);
					pdfUtil.DrawPageNumber(pDF, page3, num++);
				}
			}
			if (this.m_pParaClass.chkchart8_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C8- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				Page page = new Page(pDF, Letter.PORTRAIT);
				pdfUtil.DrawTitle(pDF, page, this.label4.Text, 0f, 12f, 1048576);
				System.Collections.Generic.List<System.Collections.Generic.List<Cell>> list6 = new System.Collections.Generic.List<System.Collections.Generic.List<Cell>>();
				System.Collections.Generic.List<Cell> list7 = new System.Collections.Generic.List<Cell>();
				for (int l = 0; l < this.dgvinventory.ColumnCount; l++)
				{
					list7.Add(new Cell(font, this.dgvinventory.Columns[l].HeaderText));
				}
				list6.Add(list7);
				for (int m = 0; m < this.dgvinventory.Rows.Count; m++)
				{
					list7 = new System.Collections.Generic.List<Cell>();
					for (int n = 0; n < this.dgvinventory.ColumnCount; n++)
					{
						list7.Add(new Cell(font2, this.dgvinventory.Rows[m].Cells[n].Value.ToString()));
					}
					list6.Add(list7);
				}
				System.Collections.Generic.List<float> list8 = new System.Collections.Generic.List<float>();
				list8.Add(80f);
				list8.Add(80f);
				list8.Add(80f);
				list8.Add(50f);
				list8.Add(60f);
				list8.Add(50f);
				list8.Add(50f);
				list8.Add(pdfUtil.PDFpageeffwidth - 450f);
				Page page4 = pdfUtil.DrawTable(pDF, page, ref num, list6, Table.DATA_HAS_1_HEADER_ROWS, 35f, list8);
				pdfUtil.DrawPageNumber(pDF, page4, num++);
			}
			if (this.m_pParaClass.chkchart9_Checked())
			{
				commUtil.ShowInfo_DEBUG(" Save --pdf (" + name + ")--C9- Start == " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
				Page page = new Page(pDF, Letter.PORTRAIT);
				pdfUtil.DrawTitle(pDF, page, this.labelPUE.Text, 0f, 12f, 1048576);
				System.Collections.Generic.List<System.Collections.Generic.List<Cell>> list9 = new System.Collections.Generic.List<System.Collections.Generic.List<Cell>>();
				System.Collections.Generic.List<Cell> list10 = new System.Collections.Generic.List<Cell>();
				for (int num4 = 0; num4 < this.dgvPUE.ColumnCount; num4++)
				{
					list10.Add(new Cell(font, this.dgvPUE.Columns[num4].HeaderText));
				}
				list9.Add(list10);
				for (int num5 = 0; num5 < this.dgvPUE.Rows.Count; num5++)
				{
					list10 = new System.Collections.Generic.List<Cell>();
					for (int num6 = 0; num6 < this.dgvPUE.ColumnCount; num6++)
					{
						list10.Add(new Cell(font2, this.dgvPUE.Rows[num5].Cells[num6].Value.ToString()));
					}
					list9.Add(list10);
				}
				System.Collections.Generic.List<float> list11 = new System.Collections.Generic.List<float>();
				list11.Add(125f);
				list11.Add(125f);
				list11.Add(125f);
				list11.Add(pdfUtil.PDFpageeffwidth - 375f);
				Page page5 = pdfUtil.DrawTable(pDF, page, ref num, list9, Table.DATA_HAS_1_HEADER_ROWS, 35f, list11);
				pdfUtil.DrawPageNumber(pDF, page5, num++);
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
		private double calPeakPower(double curavg_power, double curpeak_power)
		{
			if (curpeak_power >= curavg_power)
			{
				return curpeak_power;
			}
			switch (EcoGlobalVar.gl_PeakPowerMethod)
			{
			case 1:
				return curavg_power;
			case 2:
				return curpeak_power;
			}
			curpeak_power = curavg_power + (curavg_power - curpeak_power);
			if (curpeak_power > curavg_power * 1.1)
			{
				curpeak_power = curavg_power * 1.1;
			}
			return curpeak_power;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EGenRptShow));
			ChartArea chartArea = new ChartArea();
			ChartArea chartArea2 = new ChartArea();
			ChartArea chartArea3 = new ChartArea();
			ChartArea chartArea4 = new ChartArea();
			ChartArea chartArea5 = new ChartArea();
			Title title = new Title();
			Title title2 = new Title();
			Title title3 = new Title();
			ChartArea chartArea6 = new ChartArea();
			ChartArea chartArea7 = new ChartArea();
			ChartArea chartArea8 = new ChartArea();
			ChartArea chartArea9 = new ChartArea();
			ChartArea chartArea10 = new ChartArea();
			ChartArea chartArea11 = new ChartArea();
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			this.tabchart = new TabControl();
			this.tpline = new TabPage();
			this.chart1 = new Chart();
			this.tpsaving = new TabPage();
			this.txtaction = new System.Windows.Forms.TextBox();
			this.chtsaving = new Chart();
			this.tpcapacity = new TabPage();
			this.palcapacity4 = new Panel();
			this.labcapacity4 = new Label();
			this.dgvcapacity4 = new DataGridView();
			this.chtcapacity4 = new Chart();
			this.palcapacity3 = new Panel();
			this.labcapacity3 = new Label();
			this.dgvcapacity3 = new DataGridView();
			this.chtcapacity3 = new Chart();
			this.palcapacity2 = new Panel();
			this.labcapacity2 = new Label();
			this.dgvcapacity2 = new DataGridView();
			this.chtcapacity2 = new Chart();
			this.palcapacity1 = new Panel();
			this.chtcapacity1 = new Chart();
			this.labcapacity1 = new Label();
			this.dgvcapacity1 = new DataGridView();
			this.tplist = new TabPage();
			this.dgvinventory = new DataGridView();
			this.Column1 = new DataGridViewTextBoxColumn();
			this.Rack_Name = new DataGridViewTextBoxColumn();
			this.Column2 = new DataGridViewTextBoxColumn();
			this.Column3 = new DataGridViewTextBoxColumn();
			this.Power_diss = new DataGridViewTextBoxColumn();
			this.Column4 = new DataGridViewTextBoxColumn();
			this.Column5 = new DataGridViewTextBoxColumn();
			this.Column6 = new DataGridViewTextBoxColumn();
			this.label4 = new Label();
			this.tppue = new TabPage();
			this.dgvPUE = new DataGridView();
			this.gpnm = new DataGridViewTextBoxColumn();
			this.totalpower = new DataGridViewTextBoxColumn();
			this.itpower = new DataGridViewTextBoxColumn();
			this.pue = new DataGridViewTextBoxColumn();
			this.labelPUE = new Label();
			this.btnPreview = new Button();
			this.btnSave = new Button();
			this.tabchart.SuspendLayout();
			this.tpline.SuspendLayout();
			((ISupportInitialize)this.chart1).BeginInit();
			this.tpsaving.SuspendLayout();
			((ISupportInitialize)this.chtsaving).BeginInit();
			this.tpcapacity.SuspendLayout();
			this.palcapacity4.SuspendLayout();
			((ISupportInitialize)this.dgvcapacity4).BeginInit();
			((ISupportInitialize)this.chtcapacity4).BeginInit();
			this.palcapacity3.SuspendLayout();
			((ISupportInitialize)this.dgvcapacity3).BeginInit();
			((ISupportInitialize)this.chtcapacity3).BeginInit();
			this.palcapacity2.SuspendLayout();
			((ISupportInitialize)this.dgvcapacity2).BeginInit();
			((ISupportInitialize)this.chtcapacity2).BeginInit();
			this.palcapacity1.SuspendLayout();
			((ISupportInitialize)this.chtcapacity1).BeginInit();
			((ISupportInitialize)this.dgvcapacity1).BeginInit();
			this.tplist.SuspendLayout();
			((ISupportInitialize)this.dgvinventory).BeginInit();
			this.tppue.SuspendLayout();
			((ISupportInitialize)this.dgvPUE).BeginInit();
			base.SuspendLayout();
			this.tabchart.Controls.Add(this.tpline);
			this.tabchart.Controls.Add(this.tpsaving);
			this.tabchart.Controls.Add(this.tpcapacity);
			this.tabchart.Controls.Add(this.tplist);
			this.tabchart.Controls.Add(this.tppue);
			componentResourceManager.ApplyResources(this.tabchart, "tabchart");
			this.tabchart.Name = "tabchart";
			this.tabchart.SelectedIndex = 0;
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
			componentResourceManager.ApplyResources(this.tpsaving, "tpsaving");
			this.tpsaving.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tpsaving.Controls.Add(this.txtaction);
			this.tpsaving.Controls.Add(this.chtsaving);
			this.tpsaving.Name = "tpsaving";
			this.txtaction.BackColor = System.Drawing.Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.txtaction, "txtaction");
			this.txtaction.Name = "txtaction";
			this.txtaction.ReadOnly = true;
			this.chtsaving.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea6.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea6.Name = "ChartArea1";
			chartArea6.Position.Auto = false;
			chartArea6.Position.Height = 98f;
			chartArea6.Position.Width = 49f;
			chartArea6.Position.X = 1f;
			chartArea6.Position.Y = 1f;
			chartArea7.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea7.Name = "ChartArea2";
			chartArea7.Position.Auto = false;
			chartArea7.Position.Height = 98f;
			chartArea7.Position.Width = 49f;
			chartArea7.Position.X = 50f;
			chartArea7.Position.Y = 1f;
			this.chtsaving.ChartAreas.Add(chartArea6);
			this.chtsaving.ChartAreas.Add(chartArea7);
			componentResourceManager.ApplyResources(this.chtsaving, "chtsaving");
			this.chtsaving.Name = "chtsaving";
			componentResourceManager.ApplyResources(this.tpcapacity, "tpcapacity");
			this.tpcapacity.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tpcapacity.Controls.Add(this.palcapacity4);
			this.tpcapacity.Controls.Add(this.palcapacity3);
			this.tpcapacity.Controls.Add(this.palcapacity2);
			this.tpcapacity.Controls.Add(this.palcapacity1);
			this.tpcapacity.Name = "tpcapacity";
			this.palcapacity4.Controls.Add(this.labcapacity4);
			this.palcapacity4.Controls.Add(this.dgvcapacity4);
			this.palcapacity4.Controls.Add(this.chtcapacity4);
			componentResourceManager.ApplyResources(this.palcapacity4, "palcapacity4");
			this.palcapacity4.Name = "palcapacity4";
			componentResourceManager.ApplyResources(this.labcapacity4, "labcapacity4");
			this.labcapacity4.Name = "labcapacity4";
			this.dgvcapacity4.AllowUserToAddRows = false;
			this.dgvcapacity4.AllowUserToResizeRows = false;
			this.dgvcapacity4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvcapacity4.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.dgvcapacity4.BorderStyle = BorderStyle.None;
			this.dgvcapacity4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvcapacity4.ColumnHeadersVisible = false;
			componentResourceManager.ApplyResources(this.dgvcapacity4, "dgvcapacity4");
			this.dgvcapacity4.Name = "dgvcapacity4";
			this.dgvcapacity4.ReadOnly = true;
			this.dgvcapacity4.RowHeadersVisible = false;
			this.dgvcapacity4.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dgvcapacity4.RowTemplate.Height = 30;
			this.chtcapacity4.BackColor = System.Drawing.Color.WhiteSmoke;
			this.chtcapacity4.BorderlineColor = System.Drawing.Color.WhiteSmoke;
			chartArea8.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea8.Name = "ChartArea1";
			this.chtcapacity4.ChartAreas.Add(chartArea8);
			componentResourceManager.ApplyResources(this.chtcapacity4, "chtcapacity4");
			this.chtcapacity4.Name = "chtcapacity4";
			this.palcapacity3.Controls.Add(this.labcapacity3);
			this.palcapacity3.Controls.Add(this.dgvcapacity3);
			this.palcapacity3.Controls.Add(this.chtcapacity3);
			componentResourceManager.ApplyResources(this.palcapacity3, "palcapacity3");
			this.palcapacity3.Name = "palcapacity3";
			componentResourceManager.ApplyResources(this.labcapacity3, "labcapacity3");
			this.labcapacity3.Name = "labcapacity3";
			this.dgvcapacity3.AllowUserToAddRows = false;
			this.dgvcapacity3.AllowUserToResizeRows = false;
			this.dgvcapacity3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvcapacity3.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.dgvcapacity3.BorderStyle = BorderStyle.None;
			this.dgvcapacity3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvcapacity3.ColumnHeadersVisible = false;
			componentResourceManager.ApplyResources(this.dgvcapacity3, "dgvcapacity3");
			this.dgvcapacity3.Name = "dgvcapacity3";
			this.dgvcapacity3.ReadOnly = true;
			this.dgvcapacity3.RowHeadersVisible = false;
			this.dgvcapacity3.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dgvcapacity3.RowTemplate.Height = 30;
			this.chtcapacity3.BackColor = System.Drawing.Color.WhiteSmoke;
			this.chtcapacity3.BorderlineColor = System.Drawing.Color.WhiteSmoke;
			chartArea9.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea9.Name = "ChartArea1";
			this.chtcapacity3.ChartAreas.Add(chartArea9);
			componentResourceManager.ApplyResources(this.chtcapacity3, "chtcapacity3");
			this.chtcapacity3.Name = "chtcapacity3";
			this.palcapacity2.Controls.Add(this.labcapacity2);
			this.palcapacity2.Controls.Add(this.dgvcapacity2);
			this.palcapacity2.Controls.Add(this.chtcapacity2);
			componentResourceManager.ApplyResources(this.palcapacity2, "palcapacity2");
			this.palcapacity2.Name = "palcapacity2";
			componentResourceManager.ApplyResources(this.labcapacity2, "labcapacity2");
			this.labcapacity2.Name = "labcapacity2";
			this.dgvcapacity2.AllowUserToAddRows = false;
			this.dgvcapacity2.AllowUserToResizeRows = false;
			this.dgvcapacity2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvcapacity2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.dgvcapacity2.BorderStyle = BorderStyle.None;
			this.dgvcapacity2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvcapacity2.ColumnHeadersVisible = false;
			componentResourceManager.ApplyResources(this.dgvcapacity2, "dgvcapacity2");
			this.dgvcapacity2.Name = "dgvcapacity2";
			this.dgvcapacity2.ReadOnly = true;
			this.dgvcapacity2.RowHeadersVisible = false;
			this.dgvcapacity2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dgvcapacity2.RowTemplate.Height = 30;
			this.chtcapacity2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.chtcapacity2.BorderlineColor = System.Drawing.Color.WhiteSmoke;
			chartArea10.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea10.Name = "ChartArea1";
			this.chtcapacity2.ChartAreas.Add(chartArea10);
			componentResourceManager.ApplyResources(this.chtcapacity2, "chtcapacity2");
			this.chtcapacity2.Name = "chtcapacity2";
			this.palcapacity1.Controls.Add(this.chtcapacity1);
			this.palcapacity1.Controls.Add(this.labcapacity1);
			this.palcapacity1.Controls.Add(this.dgvcapacity1);
			componentResourceManager.ApplyResources(this.palcapacity1, "palcapacity1");
			this.palcapacity1.Name = "palcapacity1";
			this.chtcapacity1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.chtcapacity1.BorderlineColor = System.Drawing.Color.WhiteSmoke;
			chartArea11.BackColor = System.Drawing.Color.WhiteSmoke;
			chartArea11.Name = "ChartArea1";
			this.chtcapacity1.ChartAreas.Add(chartArea11);
			componentResourceManager.ApplyResources(this.chtcapacity1, "chtcapacity1");
			this.chtcapacity1.Name = "chtcapacity1";
			componentResourceManager.ApplyResources(this.labcapacity1, "labcapacity1");
			this.labcapacity1.Name = "labcapacity1";
			this.dgvcapacity1.AllowUserToAddRows = false;
			this.dgvcapacity1.AllowUserToResizeRows = false;
			this.dgvcapacity1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.dgvcapacity1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.dgvcapacity1.BorderStyle = BorderStyle.None;
			this.dgvcapacity1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvcapacity1.ColumnHeadersVisible = false;
			componentResourceManager.ApplyResources(this.dgvcapacity1, "dgvcapacity1");
			this.dgvcapacity1.Name = "dgvcapacity1";
			this.dgvcapacity1.ReadOnly = true;
			this.dgvcapacity1.RowHeadersVisible = false;
			this.dgvcapacity1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.dgvcapacity1.RowTemplate.Height = 30;
			this.dgvcapacity1.StandardTab = true;
			this.dgvcapacity1.TabStop = false;
			componentResourceManager.ApplyResources(this.tplist, "tplist");
			this.tplist.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tplist.Controls.Add(this.dgvinventory);
			this.tplist.Controls.Add(this.label4);
			this.tplist.Name = "tplist";
			this.dgvinventory.AllowUserToAddRows = false;
			this.dgvinventory.AllowUserToDeleteRows = false;
			this.dgvinventory.AllowUserToResizeRows = false;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = System.Drawing.Color.Gainsboro;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvinventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvinventory, "dgvinventory");
			this.dgvinventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvinventory.Columns.AddRange(new DataGridViewColumn[]
			{
				this.Column1,
				this.Rack_Name,
				this.Column2,
				this.Column3,
				this.Power_diss,
				this.Column4,
				this.Column5,
				this.Column6
			});
			this.dgvinventory.MultiSelect = false;
			this.dgvinventory.Name = "dgvinventory";
			this.dgvinventory.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			this.dgvinventory.RowHeadersVisible = false;
			this.dgvinventory.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvinventory.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvinventory.StandardTab = true;
			this.dgvinventory.TabStop = false;
			componentResourceManager.ApplyResources(this.Column1, "Column1");
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Rack_Name, "Rack_Name");
			this.Rack_Name.Name = "Rack_Name";
			this.Rack_Name.ReadOnly = true;
			this.Rack_Name.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Column2, "Column2");
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Column3, "Column3");
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Power_diss, "Power_diss");
			this.Power_diss.Name = "Power_diss";
			this.Power_diss.ReadOnly = true;
			this.Power_diss.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Column4, "Column4");
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			this.Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Column5, "Column5");
			this.Column5.Name = "Column5";
			this.Column5.ReadOnly = true;
			this.Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.Column6, "Column6");
			this.Column6.Name = "Column6";
			this.Column6.ReadOnly = true;
			this.Column6.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.tppue, "tppue");
			this.tppue.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tppue.Controls.Add(this.dgvPUE);
			this.tppue.Controls.Add(this.labelPUE);
			this.tppue.Name = "tppue";
			this.dgvPUE.AllowUserToAddRows = false;
			this.dgvPUE.AllowUserToDeleteRows = false;
			this.dgvPUE.AllowUserToResizeRows = false;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			this.dgvPUE.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			componentResourceManager.ApplyResources(this.dgvPUE, "dgvPUE");
			this.dgvPUE.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvPUE.Columns.AddRange(new DataGridViewColumn[]
			{
				this.gpnm,
				this.totalpower,
				this.itpower,
				this.pue
			});
			this.dgvPUE.MultiSelect = false;
			this.dgvPUE.Name = "dgvPUE";
			this.dgvPUE.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			this.dgvPUE.RowHeadersVisible = false;
			this.dgvPUE.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvPUE.SelectionMode = DataGridViewSelectionMode.CellSelect;
			this.dgvPUE.StandardTab = true;
			this.dgvPUE.TabStop = false;
			componentResourceManager.ApplyResources(this.gpnm, "gpnm");
			this.gpnm.Name = "gpnm";
			this.gpnm.ReadOnly = true;
			this.gpnm.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.totalpower, "totalpower");
			this.totalpower.Name = "totalpower";
			this.totalpower.ReadOnly = true;
			this.totalpower.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.itpower, "itpower");
			this.itpower.Name = "itpower";
			this.itpower.ReadOnly = true;
			this.itpower.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.pue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.pue, "pue");
			this.pue.Name = "pue";
			this.pue.ReadOnly = true;
			this.pue.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.labelPUE, "labelPUE");
			this.labelPUE.Name = "labelPUE";
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
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			base.Controls.Add(this.btnPreview);
			base.Controls.Add(this.btnSave);
			base.Controls.Add(this.tabchart);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "EGenRptShow";
			this.tabchart.ResumeLayout(false);
			this.tpline.ResumeLayout(false);
			((ISupportInitialize)this.chart1).EndInit();
			this.tpsaving.ResumeLayout(false);
			this.tpsaving.PerformLayout();
			((ISupportInitialize)this.chtsaving).EndInit();
			this.tpcapacity.ResumeLayout(false);
			this.palcapacity4.ResumeLayout(false);
			this.palcapacity4.PerformLayout();
			((ISupportInitialize)this.dgvcapacity4).EndInit();
			((ISupportInitialize)this.chtcapacity4).EndInit();
			this.palcapacity3.ResumeLayout(false);
			this.palcapacity3.PerformLayout();
			((ISupportInitialize)this.dgvcapacity3).EndInit();
			((ISupportInitialize)this.chtcapacity3).EndInit();
			this.palcapacity2.ResumeLayout(false);
			this.palcapacity2.PerformLayout();
			((ISupportInitialize)this.dgvcapacity2).EndInit();
			((ISupportInitialize)this.chtcapacity2).EndInit();
			this.palcapacity1.ResumeLayout(false);
			this.palcapacity1.PerformLayout();
			((ISupportInitialize)this.chtcapacity1).EndInit();
			((ISupportInitialize)this.dgvcapacity1).EndInit();
			this.tplist.ResumeLayout(false);
			this.tplist.PerformLayout();
			((ISupportInitialize)this.dgvinventory).EndInit();
			this.tppue.ResumeLayout(false);
			this.tppue.PerformLayout();
			((ISupportInitialize)this.dgvPUE).EndInit();
			base.ResumeLayout(false);
		}
	}
}
