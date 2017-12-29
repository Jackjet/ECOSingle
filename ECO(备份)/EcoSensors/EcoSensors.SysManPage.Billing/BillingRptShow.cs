using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using PDFjet.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class BillingRptShow : UserControl
	{
		private IContainer components;
		private DataGridView dgvBilling;
		private Label lbTitle;
		private Button btnPreview;
		private Button btnSave;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private Label lbRPT_title;
		public static int AnalysisIndex_devIDs = 0;
		public static int AnalysisIndex_gpID = 1;
		public static int AnalysisIndex_gpNM = 2;
		public static int AnalysisIndex_gpTP = 3;
		public static int AnalysisIndex_portIDs = 4;
		private static int[][] TableWidth = new int[][]
		{
			new int[]
			{
				200,
				100,
				100,
				100,
				100,
				100,
				100,
				100,
				0,
				0,
				0
			},
			new int[]
			{
				150,
				90,
				90,
				90,
				90,
				90,
				100,
				100,
				100,
				0
			},
			new int[]
			{
				140,
				140,
				80,
				80,
				80,
				80,
				100,
				100,
				100,
				0
			},
			new int[]
			{
				140,
				140,
				70,
				80,
				70,
				70,
				70,
				80,
				100,
				80
			}
		};
		private Billing m_parent;
		private BillingRptParaClass m_pParaClass;
		private bool m_isRPTSaved;
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BillingRptShow));
			this.dgvBilling = new DataGridView();
			this.lbTitle = new Label();
			this.btnPreview = new Button();
			this.btnSave = new Button();
			this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
			this.lbRPT_title = new Label();
			((ISupportInitialize)this.dgvBilling).BeginInit();
			base.SuspendLayout();
			this.dgvBilling.AllowUserToAddRows = false;
			this.dgvBilling.AllowUserToDeleteRows = false;
			this.dgvBilling.AllowUserToResizeRows = false;
			this.dgvBilling.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvBilling.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvBilling, "dgvBilling");
			this.dgvBilling.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvBilling.MultiSelect = false;
			this.dgvBilling.Name = "dgvBilling";
			this.dgvBilling.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			this.dgvBilling.RowHeadersVisible = false;
			this.dgvBilling.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvBilling.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvBilling.StandardTab = true;
			this.dgvBilling.TabStop = false;
			componentResourceManager.ApplyResources(this.lbTitle, "lbTitle");
			this.lbTitle.Name = "lbTitle";
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
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
			this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			this.dataGridViewTextBoxColumn5.ReadOnly = true;
			this.dataGridViewTextBoxColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
			this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			this.dataGridViewTextBoxColumn6.ReadOnly = true;
			this.dataGridViewTextBoxColumn6.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
			this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			this.dataGridViewTextBoxColumn7.ReadOnly = true;
			this.dataGridViewTextBoxColumn7.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
			this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			this.dataGridViewTextBoxColumn8.ReadOnly = true;
			this.dataGridViewTextBoxColumn8.SortMode = DataGridViewColumnSortMode.NotSortable;
			componentResourceManager.ApplyResources(this.lbRPT_title, "lbRPT_title");
			this.lbRPT_title.Name = "lbRPT_title";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			base.Controls.Add(this.lbRPT_title);
			base.Controls.Add(this.btnPreview);
			base.Controls.Add(this.btnSave);
			base.Controls.Add(this.dgvBilling);
			base.Controls.Add(this.lbTitle);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "BillingRptShow";
			((ISupportInitialize)this.dgvBilling).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public BillingRptShow()
		{
			this.InitializeComponent();
		}
		public void pageInit(Billing parent, BillingRptParaClass pPara)
		{
			this.m_parent = parent;
			this.m_pParaClass = pPara;
			this.m_isRPTSaved = false;
			this.CreateChart();
		}
		private void CreateChart()
		{
			string msg = EcoLanguage.getMsg(LangRes.Rpt_BillTitle, new string[]
			{
				this.m_pParaClass.Txtwriter,
				this.m_pParaClass.Dtptime
			});
			this.lbRPT_title.Text = msg;
			DataTable dataTableRst = this.m_pParaClass.DataTableRst;
			this.dgvBilling.DataSource = null;
			this.dgvBilling.DataSource = dataTableRst;
			int tableType_index = this.m_pParaClass.tableType_index;
			for (int i = 0; i < this.dgvBilling.Columns.Count; i++)
			{
				this.dgvBilling.Columns[i].Width = BillingRptShow.TableWidth[tableType_index][i];
				this.dgvBilling.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				if (i != 0 && (this.m_pParaClass.RptType != 2 || i != 1))
				{
					this.dgvBilling.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
				}
			}
			for (int j = 0; j < this.dgvBilling.Rows.Count; j++)
			{
				for (int k = 0; k < this.dgvBilling.Columns.Count; k++)
				{
					string text = this.dgvBilling.Rows[j].Cells[k].Value.ToString();
					if (text.Equals(EcoLanguage.getMsg(LangRes.BILLTB_Subtotal, new string[0])))
					{
						this.dgvBilling.Rows[j].Cells[k].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
					}
				}
			}
			this.dgvBilling.Columns[this.dgvBilling.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
			string text = Sys_Para.GetBillPath();
			if (text.Length == 0)
			{
				text = System.IO.Directory.GetCurrentDirectory() + "\\BillReportFile\\";
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
			if (ReportInfo.InsertBillReport(this.m_pParaClass.Txttitle, this.m_pParaClass.Txtwriter, now, str + ".html") == 0)
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
				return -1;
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
			stringBuilder.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
			stringBuilder.AppendLine("<head>");
			stringBuilder.AppendLine("<title>" + EcoLanguage.getMsg(LangRes.Rpt_saveBillingInformation, new string[0]) + "</title>");
			stringBuilder.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />");
			stringBuilder.AppendLine("</head>");
			stringBuilder.AppendLine("<body >");
			string msg = EcoLanguage.getMsg(LangRes.Rpt_BillTitle, new string[]
			{
				this.m_pParaClass.Txtwriter,
				this.m_pParaClass.Dtptime
			});
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"<p><table align='center' width='900px' border='1'><tr height='30px' ><td align='center' style='font-weight:bold;font-size:15pt;' colspan='",
				this.dgvBilling.ColumnCount,
				"'>",
				this.lbTitle.Text
			}));
			stringBuilder.AppendLine("<br><div align='right' style='font-weight:bold;font-size:10pt;'>" + msg + "</div>");
			stringBuilder.AppendLine("</td></tr>");
			for (int i = 0; i < this.dgvBilling.ColumnCount; i++)
			{
				stringBuilder.AppendLine("<td align='center'>" + this.dgvBilling.Columns[i].HeaderText + "</td>");
				if (i > 0)
				{
					stringBuilder2.Append("\t");
				}
				stringBuilder2.Append("\"" + this.dgvBilling.Columns[i].HeaderText + "\"");
			}
			stringBuilder2.AppendLine();
			for (int j = 0; j < this.dgvBilling.Rows.Count; j++)
			{
				stringBuilder.AppendLine("<tr height='30px'>");
				for (int k = 0; k < this.dgvBilling.ColumnCount; k++)
				{
					string text = this.dgvBilling.Rows[j].Cells[k].Value.ToString();
					if (text.Length == 0)
					{
						stringBuilder.AppendLine("<td>&nbsp;</td>");
					}
					else
					{
						if (this.dgvBilling.Rows[j].Cells[k].Style.Alignment == DataGridViewContentAlignment.MiddleRight || this.dgvBilling.Columns[k].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
						{
							stringBuilder.AppendLine("<td align='Right'>" + text + "</td>");
						}
						else
						{
							stringBuilder.AppendLine("<td>" + text + "</td>");
						}
					}
					if (k > 0)
					{
						stringBuilder2.Append("\t");
					}
					stringBuilder2.Append("\"" + text + "\"");
				}
				stringBuilder.AppendLine("</tr>");
				stringBuilder2.AppendLine();
			}
			stringBuilder.AppendLine("</table></p>");
			this.exportCSV(path, name, "Billing", stringBuilder2);
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
			Page page = new Page(pDF, Letter.PORTRAIT);
			pdfUtil.DrawTitle(pDF, page, this.lbTitle.Text, 0f, 12f, 1048576);
			string msg = EcoLanguage.getMsg(LangRes.Rpt_BillTitle, new string[]
			{
				this.m_pParaClass.Txtwriter,
				this.m_pParaClass.Dtptime
			});
			pdfUtil.DrawTitle(pDF, page, msg, 30f, 10f, 2097152);
			System.Collections.Generic.List<System.Collections.Generic.List<Cell>> list = new System.Collections.Generic.List<System.Collections.Generic.List<Cell>>();
			System.Collections.Generic.List<Cell> list2 = new System.Collections.Generic.List<Cell>();
			for (int i = 0; i < this.dgvBilling.ColumnCount; i++)
			{
				Cell cell = new Cell(font, this.dgvBilling.Columns[i].HeaderText);
				cell.SetTextAlignment(1048576);
				list2.Add(cell);
			}
			list.Add(list2);
			for (int j = 0; j < this.dgvBilling.Rows.Count; j++)
			{
				list2 = new System.Collections.Generic.List<Cell>();
				for (int k = 0; k < this.dgvBilling.ColumnCount; k++)
				{
					Cell cell = new Cell(font2, this.dgvBilling.Rows[j].Cells[k].Value.ToString());
					if (this.dgvBilling.Rows[j].Cells[k].Style.Alignment == DataGridViewContentAlignment.MiddleRight || this.dgvBilling.Columns[k].DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
					{
						cell.SetTextAlignment(2097152);
					}
					list2.Add(cell);
				}
				list.Add(list2);
			}
			System.Collections.Generic.List<float> list3 = new System.Collections.Generic.List<float>();
			int tableType_index = this.m_pParaClass.tableType_index;
			for (int l = 0; l < this.dgvBilling.Columns.Count; l++)
			{
				float item = pdfUtil.PDFpageeffwidth / (float)this.dgvBilling.Size.Width * (float)BillingRptShow.TableWidth[tableType_index][l];
				list3.Add(item);
			}
			Page page2 = pdfUtil.DrawTable(pDF, page, ref num, list, Table.DATA_HAS_1_HEADER_ROWS, 55f, list3);
			pdfUtil.DrawPageNumber(pDF, page2, num++);
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
	}
}
