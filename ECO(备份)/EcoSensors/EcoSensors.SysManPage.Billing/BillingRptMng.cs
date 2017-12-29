using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class BillingRptMng : UserControl
	{
		private bool ShowAll;
		private IContainer components;
		private Button btndel;
		private DataGridView dgvwReport;
		private Panel panellist;
		private GroupBox groupBoxSearch;
		private TextBox txtkey;
		private Button btnAll;
		private ComboBox cboby;
		private Label label365;
		private DateTimePicker dtpFrom;
		private Label label3;
		private Label label363;
		private DateTimePicker dtpTo;
		private Button btnSearch;
		private Label label1;
		private GroupBox groupBoxLoc;
		private Button btnSave;
		private Label lbpath;
		private Button btnBrowse;
		private TextBox txtpath;
		private Label lblTitle;
		private DataGridViewTextBoxColumn No;
		private DataGridViewTextBoxColumn id;
		private DataGridViewTextBoxColumn Title;
		private DataGridViewTextBoxColumn Writer;
		private DataGridViewTextBoxColumn ReportTime;
		private DataGridViewTextBoxColumn ReportPath;
		private DataGridViewLinkColumn Reports;
		private DataGridViewLinkColumn Folder;
		public BillingRptMng()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			this.panellist.Visible = true;
			this.dtpFrom.Text = System.DateTime.Now.AddDays((double)(1 - System.DateTime.Now.Day)).ToString("yyyy-MM-dd 00:00:00");
			this.dtpTo.Text = System.DateTime.Now.AddDays((double)(1 - System.DateTime.Now.Day)).AddMonths(1).AddDays(-1.0).ToString("yyyy-MM-dd 23:59:59");
			this.cboby.SelectedIndex = 0;
			this.txtpath.Text = Sys_Para.GetBillPath();
			if (this.txtpath.Text.Length == 0)
			{
				this.txtpath.Text = System.IO.Directory.GetCurrentDirectory() + "\\BillReportFile\\";
			}
			this.DataBind(this.cboby.SelectedIndex.ToString());
		}
		private void DataBind(string type)
		{
			string text = this.dtpFrom.Text;
			string text2 = this.dtpTo.Text;
			string str = this.txtkey.Text.Replace("'", "''");
			string str_sql;
			if (type != null)
			{
				if (type == "0")
				{
					str_sql = string.Concat(new string[]
					{
						"select id,Title,Writer,ReportTime,ReportPath from reportbill where ReportTime between #",
						text,
						"# and #",
						text2,
						"# order by ReportTime desc"
					});
					goto IL_D1;
				}
				if (type == "1")
				{
					str_sql = "select id,Title,Writer,ReportTime,ReportPath from reportbill where title like '%" + str + "%' order by ReportTime desc";
					goto IL_D1;
				}
				if (type == "2")
				{
					str_sql = "select id,Title,Writer,ReportTime,ReportPath from reportbill where writer like '%" + str + "%' order by ReportTime desc";
					goto IL_D1;
				}
			}
			str_sql = "select id,Title,Writer,ReportTime,ReportPath from reportbill order by ReportTime desc";
			IL_D1:
			DataTable dataTable = DBTools.CreateDataTable4SysDB(str_sql);
			int num = 1;
			this.dgvwReport.Rows.Clear();
			foreach (DataRow dataRow in dataTable.Rows)
			{
				System.DateTime dateTime = (System.DateTime)dataRow["ReportTime"];
				object[] values = new object[]
				{
					num++,
					dataRow["id"].ToString(),
					dataRow["Title"].ToString(),
					dataRow["Writer"].ToString(),
					dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
					dataRow["ReportPath"].ToString(),
					EcoLanguage.getMsg(LangRes.RptMng_OpenHTML, new string[0]),
					EcoLanguage.getMsg(LangRes.RptMng_OpenFolder, new string[0])
				};
				this.dgvwReport.Rows.Add(values);
			}
			this.dgvwReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvwReport.ClearSelection();
		}
		private void dgvwReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				switch (e.ColumnIndex)
				{
				case 6:
				{
					string text = this.dgvwReport.Rows[e.RowIndex].Cells["ReportPath"].Value.ToString();
					if (System.IO.File.Exists(text))
					{
						Process.Start("explorer.exe", text);
						return;
					}
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.RptMng_notExist, new string[0]));
					return;
				}
				case 7:
				{
					string text = this.dgvwReport.Rows[e.RowIndex].Cells["ReportPath"].Value.ToString();
					string directoryName = System.IO.Path.GetDirectoryName(text);
					if (System.IO.Directory.Exists(directoryName))
					{
						Process.Start(directoryName);
						return;
					}
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.RptMng_notExist, new string[0]));
					break;
				}
				default:
					return;
				}
			}
		}
		private void btndel_Click(object sender, System.EventArgs e)
		{
			if (this.dgvwReport.SelectedRows.Count > 0)
			{
				if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.RptMng_delCrm, new string[0]), MessageBoxButtons.OKCancel).Equals(DialogResult.OK))
				{
					string text = this.dgvwReport.SelectedRows[0].Cells["ReportPath"].Value.ToString();
					string path = text.Substring(0, text.LastIndexOf('\\'));
					if (System.IO.Directory.Exists(path))
					{
						try
						{
							System.IO.Directory.Delete(path, true);
						}
						catch
						{
							System.Console.WriteLine("Delete report fail,it is being used by another process");
							EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
							return;
						}
					}
					string sql = "delete from reportbill where id=" + this.dgvwReport.SelectedRows[0].Cells[1].Value;
					int num = DBTools.executeSql(sql);
					if (num > 0)
					{
						EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
					}
					if (this.ShowAll)
					{
						this.DataBind("");
						return;
					}
					this.DataBind(this.cboby.SelectedIndex.ToString());
					return;
				}
			}
			else
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_selectneed, new string[0]));
			}
		}
		private void btnAll_Click(object sender, System.EventArgs e)
		{
			this.DataBind("");
			this.ShowAll = true;
		}
		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = this.txtpath.Text;
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = folderBrowserDialog.SelectedPath;
				if (selectedPath.Substring(selectedPath.Length - 1) != "\\")
				{
					this.txtpath.Text = selectedPath + "\\";
					return;
				}
				this.txtpath.Text = selectedPath;
			}
		}
		private void cboby_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.cboby.SelectedIndex.ToString() == "0")
			{
				this.dtpFrom.Enabled = true;
				this.dtpTo.Enabled = true;
				this.txtkey.Enabled = false;
				return;
			}
			this.dtpFrom.Enabled = false;
			this.dtpTo.Enabled = false;
			this.txtkey.Enabled = true;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = this.txtpath.Text;
			if (text == "")
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbpath.Text
				}));
				return;
			}
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.Substring(i, 1).ToCharArray()[0];
				if (c > '~')
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.RptMng_pathErr1, new string[0]));
					return;
				}
			}
			int num = Sys_Para.UpdateBillPath(this.txtpath.Text.Replace("'", "''"));
			if (num > 0)
			{
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			string text = this.cboby.SelectedIndex.ToString();
			if (text != "0" && this.txtkey.Text == "")
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.RptMng_needKeyword, new string[0]));
				return;
			}
			this.DataBind(text);
			this.ShowAll = false;
		}
		private void txtkey_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BillingRptMng));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			this.btndel = new Button();
			this.dgvwReport = new DataGridView();
			this.panellist = new Panel();
			this.groupBoxSearch = new GroupBox();
			this.txtkey = new TextBox();
			this.btnAll = new Button();
			this.cboby = new ComboBox();
			this.label365 = new Label();
			this.dtpFrom = new DateTimePicker();
			this.label3 = new Label();
			this.label363 = new Label();
			this.dtpTo = new DateTimePicker();
			this.btnSearch = new Button();
			this.label1 = new Label();
			this.groupBoxLoc = new GroupBox();
			this.btnSave = new Button();
			this.lbpath = new Label();
			this.btnBrowse = new Button();
			this.txtpath = new TextBox();
			this.lblTitle = new Label();
			this.No = new DataGridViewTextBoxColumn();
			this.id = new DataGridViewTextBoxColumn();
			this.Title = new DataGridViewTextBoxColumn();
			this.Writer = new DataGridViewTextBoxColumn();
			this.ReportTime = new DataGridViewTextBoxColumn();
			this.ReportPath = new DataGridViewTextBoxColumn();
			this.Reports = new DataGridViewLinkColumn();
			this.Folder = new DataGridViewLinkColumn();
			((ISupportInitialize)this.dgvwReport).BeginInit();
			this.panellist.SuspendLayout();
			this.groupBoxSearch.SuspendLayout();
			this.groupBoxLoc.SuspendLayout();
			base.SuspendLayout();
			this.btndel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btndel, "btndel");
			this.btndel.Name = "btndel";
			this.btndel.UseVisualStyleBackColor = false;
			this.btndel.Click += new System.EventHandler(this.btndel_Click);
			this.dgvwReport.AllowUserToAddRows = false;
			this.dgvwReport.AllowUserToDeleteRows = false;
			this.dgvwReport.AllowUserToResizeRows = false;
			this.dgvwReport.BackgroundColor = Color.WhiteSmoke;
			this.dgvwReport.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			this.dgvwReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.Gainsboro;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvwReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dgvwReport, "dgvwReport");
			this.dgvwReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvwReport.Columns.AddRange(new DataGridViewColumn[]
			{
				this.No,
				this.id,
				this.Title,
				this.Writer,
				this.ReportTime,
				this.ReportPath,
				this.Reports,
				this.Folder
			});
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = Color.Transparent;
			dataGridViewCellStyle2.SelectionForeColor = Color.Transparent;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvwReport.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvwReport.EditMode = DataGridViewEditMode.EditOnF2;
			this.dgvwReport.GridColor = Color.White;
			this.dgvwReport.MultiSelect = false;
			this.dgvwReport.Name = "dgvwReport";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = Color.Gainsboro;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvwReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvwReport.RowHeadersVisible = false;
			dataGridViewCellStyle4.SelectionBackColor = SystemColors.MenuHighlight;
			dataGridViewCellStyle4.SelectionForeColor = SystemColors.ControlText;
			this.dgvwReport.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvwReport.RowTemplate.Height = 25;
			this.dgvwReport.RowTemplate.ReadOnly = true;
			this.dgvwReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvwReport.ShowCellErrors = false;
			this.dgvwReport.ShowCellToolTips = false;
			this.dgvwReport.ShowEditingIcon = false;
			this.dgvwReport.ShowRowErrors = false;
			this.dgvwReport.StandardTab = true;
			this.dgvwReport.TabStop = false;
			this.dgvwReport.CellContentClick += new DataGridViewCellEventHandler(this.dgvwReport_CellContentClick);
			this.panellist.Controls.Add(this.groupBoxSearch);
			this.panellist.Controls.Add(this.groupBoxLoc);
			this.panellist.Controls.Add(this.btndel);
			this.panellist.Controls.Add(this.dgvwReport);
			this.panellist.Controls.Add(this.lblTitle);
			componentResourceManager.ApplyResources(this.panellist, "panellist");
			this.panellist.Name = "panellist";
			this.groupBoxSearch.Controls.Add(this.txtkey);
			this.groupBoxSearch.Controls.Add(this.btnAll);
			this.groupBoxSearch.Controls.Add(this.cboby);
			this.groupBoxSearch.Controls.Add(this.label365);
			this.groupBoxSearch.Controls.Add(this.dtpFrom);
			this.groupBoxSearch.Controls.Add(this.label3);
			this.groupBoxSearch.Controls.Add(this.label363);
			this.groupBoxSearch.Controls.Add(this.dtpTo);
			this.groupBoxSearch.Controls.Add(this.btnSearch);
			this.groupBoxSearch.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.groupBoxSearch, "groupBoxSearch");
			this.groupBoxSearch.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBoxSearch.Name = "groupBoxSearch";
			this.groupBoxSearch.TabStop = false;
			componentResourceManager.ApplyResources(this.txtkey, "txtkey");
			this.txtkey.Name = "txtkey";
			this.txtkey.KeyPress += new KeyPressEventHandler(this.txtkey_KeyPress);
			this.btnAll.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnAll, "btnAll");
			this.btnAll.ForeColor = SystemColors.ControlText;
			this.btnAll.Name = "btnAll";
			this.btnAll.UseVisualStyleBackColor = false;
			this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
			this.cboby.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.cboby, "cboby");
			this.cboby.FormattingEnabled = true;
			this.cboby.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("cboby.Items"),
				componentResourceManager.GetString("cboby.Items1"),
				componentResourceManager.GetString("cboby.Items2")
			});
			this.cboby.Name = "cboby";
			this.cboby.SelectedIndexChanged += new System.EventHandler(this.cboby_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label365, "label365");
			this.label365.ForeColor = SystemColors.ControlText;
			this.label365.Name = "label365";
			componentResourceManager.ApplyResources(this.dtpFrom, "dtpFrom");
			this.dtpFrom.Format = DateTimePickerFormat.Custom;
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.ForeColor = SystemColors.ControlText;
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label363, "label363");
			this.label363.ForeColor = SystemColors.ControlText;
			this.label363.Name = "label363";
			componentResourceManager.ApplyResources(this.dtpTo, "dtpTo");
			this.dtpTo.Format = DateTimePickerFormat.Custom;
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.ShowUpDown = true;
			this.btnSearch.BackColor = Color.Gainsboro;
			this.btnSearch.FlatAppearance.BorderColor = Color.White;
			this.btnSearch.FlatAppearance.BorderSize = 0;
			this.btnSearch.FlatAppearance.MouseDownBackColor = Color.White;
			this.btnSearch.FlatAppearance.MouseOverBackColor = Color.White;
			componentResourceManager.ApplyResources(this.btnSearch, "btnSearch");
			this.btnSearch.ForeColor = SystemColors.ControlText;
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			this.groupBoxLoc.Controls.Add(this.btnSave);
			this.groupBoxLoc.Controls.Add(this.lbpath);
			this.groupBoxLoc.Controls.Add(this.btnBrowse);
			this.groupBoxLoc.Controls.Add(this.txtpath);
			componentResourceManager.ApplyResources(this.groupBoxLoc, "groupBoxLoc");
			this.groupBoxLoc.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBoxLoc.Name = "groupBoxLoc";
			this.groupBoxLoc.TabStop = false;
			this.btnSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.ForeColor = SystemColors.ControlText;
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			componentResourceManager.ApplyResources(this.lbpath, "lbpath");
			this.lbpath.ForeColor = SystemColors.ControlText;
			this.lbpath.Name = "lbpath";
			this.btnBrowse.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.ForeColor = SystemColors.ControlText;
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = false;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			this.txtpath.BackColor = SystemColors.ControlLightLight;
			componentResourceManager.ApplyResources(this.txtpath, "txtpath");
			this.txtpath.Name = "txtpath";
			this.txtpath.ReadOnly = true;
			this.lblTitle.BackColor = Color.Gainsboro;
			this.lblTitle.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
			this.lblTitle.ForeColor = Color.FromArgb(20, 73, 160);
			this.lblTitle.Name = "lblTitle";
			this.No.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.No, "No");
			this.No.Name = "No";
			componentResourceManager.ApplyResources(this.id, "id");
			this.id.Name = "id";
			this.Title.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.Title, "Title");
			this.Title.Name = "Title";
			this.Writer.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.Writer, "Writer");
			this.Writer.Name = "Writer";
			this.ReportTime.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			componentResourceManager.ApplyResources(this.ReportTime, "ReportTime");
			this.ReportTime.Name = "ReportTime";
			componentResourceManager.ApplyResources(this.ReportPath, "ReportPath");
			this.ReportPath.Name = "ReportPath";
			this.Reports.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Reports, "Reports");
			this.Reports.Name = "Reports";
			this.Reports.Resizable = DataGridViewTriState.True;
			this.Reports.SortMode = DataGridViewColumnSortMode.Automatic;
			this.Folder.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			componentResourceManager.ApplyResources(this.Folder, "Folder");
			this.Folder.Name = "Folder";
			this.Folder.SortMode = DataGridViewColumnSortMode.Automatic;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panellist);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "BillingRptMng";
			((ISupportInitialize)this.dgvwReport).EndInit();
			this.panellist.ResumeLayout(false);
			this.groupBoxSearch.ResumeLayout(false);
			this.groupBoxSearch.PerformLayout();
			this.groupBoxLoc.ResumeLayout(false);
			this.groupBoxLoc.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
