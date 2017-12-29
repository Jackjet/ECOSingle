using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Properties;
using EventLogAPI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.LogSysLog
{
	public class LogSysLog : UserControl
	{
		private static int goPage;
		private static int nTatalLogs;
		private static int nPerPageLogs;
		private static int nCurrentPage;
		private static int nTotalPage;
		private int SaveSuccess;
		private static int SaveRange;
		private IContainer components;
		private Panel panellist;
		private Label lblTitle;
		private Button btnExport;
		private Label label2;
		private Label label1;
		private TextBox txtkey;
		private Button btnSearch;
		private DateTimePicker dtpLogtimeTo;
		private Label label363;
		private DateTimePicker dtpLogtimeFrom;
		private Label label365;
		private Label lab_key;
		private Button btnLogSearchView;
		private Button btnLastPage;
		private Button btnNextPage;
		private Button btnPreviewPage;
		private Button btnFirstPage;
		private TextBox txtGoPage;
		private Label labelPage;
		private GroupBox groupBox1;
		private RadioButton rbInclude;
		private RadioButton rbExclude;
		private RadioButton rbAll;
		private DataGridView dataGridViewLogs;
		private PictureBox pictureBoxLoading;
		private Label txtPageCount;
		private CheckBox checkBox_SaveAll;
		public LogSysLog()
		{
			this.InitializeComponent();
		}
		public void pageInit()
		{
			LogSetting logSetting = new LogSetting();
			this.rbAll.Checked = true;
			this.dtpLogtimeFrom.Enabled = false;
			this.dtpLogtimeTo.Enabled = false;
			this.dtpLogtimeFrom.Value = System.DateTime.Now;
			this.dtpLogtimeTo.Value = System.DateTime.Now;
			LogSysLog.goPage = 0;
			this.txtGoPage.Text = "";
			this.txtkey.Text = "";
			this.txtPageCount.Text = "";
			LogSysLog.SaveRange = 0;
			LogSysLog.nPerPageLogs = logSetting.PageSize;
			if (LogSysLog.nPerPageLogs == 0)
			{
				LogSysLog.nPerPageLogs = 17;
			}
			LogSysLog.nTatalLogs = 0;
			LogSysLog.nCurrentPage = 1;
			LogSysLog.nTotalPage = 0;
			this.panellist.Update();
			this.btnSearch_Click(this, null);
			int arg_C9_0 = LogSysLog.nTatalLogs;
		}
		private void displayFoundLogs(DataTable dtab_logs)
		{
			string text;
			if (LogSysLog.nTotalPage == 0)
			{
				text = "0 / 0";
				this.btnExport.Enabled = false;
			}
			else
			{
				text = LogSysLog.nCurrentPage.ToString() + " / " + LogSysLog.nTotalPage.ToString();
				this.btnExport.Enabled = true;
			}
			this.txtPageCount.Text = text;
			this.txtPageCount.Update();
			this.txtGoPage.Text = LogSysLog.goPage.ToString();
			this.txtGoPage.Update();
			if (LogSysLog.nTotalPage > 1)
			{
				this.txtGoPage.Enabled = true;
				if (LogSysLog.goPage != 0)
				{
					this.btnLogSearchView.Enabled = true;
				}
				else
				{
					this.btnLogSearchView.Enabled = false;
				}
				if (LogSysLog.nCurrentPage == 1)
				{
					this.btnFirstPage.Enabled = false;
					this.btnPreviewPage.Enabled = false;
					this.btnNextPage.Enabled = true;
					this.btnLastPage.Enabled = true;
				}
				else
				{
					if (LogSysLog.nCurrentPage == LogSysLog.nTotalPage)
					{
						this.btnFirstPage.Enabled = true;
						this.btnPreviewPage.Enabled = true;
						this.btnNextPage.Enabled = false;
						this.btnLastPage.Enabled = false;
					}
					else
					{
						this.btnFirstPage.Enabled = true;
						this.btnPreviewPage.Enabled = true;
						this.btnNextPage.Enabled = true;
						this.btnLastPage.Enabled = true;
					}
				}
			}
			else
			{
				this.txtGoPage.Enabled = false;
				this.btnLogSearchView.Enabled = false;
				this.btnFirstPage.Enabled = false;
				this.btnPreviewPage.Enabled = false;
				this.btnNextPage.Enabled = false;
				this.btnLastPage.Enabled = false;
			}
			if (dtab_logs == null)
			{
				this.dataGridViewLogs.DataSource = null;
			}
			else
			{
				this.dataGridViewLogs.DataSource = dtab_logs;
				this.dataGridViewLogs.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dataGridViewLogs.Columns[0].Width = 60;
				this.dataGridViewLogs.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dataGridViewLogs.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dataGridViewLogs.Columns[2].Width = 100;
				this.dataGridViewLogs.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				this.dataGridViewLogs.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				this.dataGridViewLogs.Columns[4].Width = 150;
				this.dataGridViewLogs.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				this.dataGridViewLogs.Columns[5].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			}
			this.dataGridViewLogs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			this.dataGridViewLogs.Update();
		}
		private void rbAll_CheckedChanged(object sender, System.EventArgs e)
		{
			this.dtpLogtimeFrom.Enabled = false;
			this.dtpLogtimeTo.Enabled = false;
		}
		private void rbInclude_CheckedChanged(object sender, System.EventArgs e)
		{
			this.dtpLogtimeFrom.Enabled = true;
			this.dtpLogtimeTo.Enabled = true;
		}
		private void rbExclude_CheckedChanged(object sender, System.EventArgs e)
		{
			this.dtpLogtimeFrom.Enabled = true;
			this.dtpLogtimeTo.Enabled = true;
		}
		private void txtGoPage_TextChanged(object sender, System.EventArgs e)
		{
			if (this.txtGoPage.TextLength == 0)
			{
				LogSysLog.goPage = 0;
			}
			else
			{
				int num;
				if (int.TryParse(this.txtGoPage.Text, out num))
				{
					LogSysLog.goPage = num;
				}
			}
			if (LogSysLog.goPage != 0)
			{
				this.txtGoPage.Text = LogSysLog.goPage.ToString();
				this.txtGoPage.Select(this.txtGoPage.Text.Length, 1);
				this.btnLogSearchView.Enabled = true;
				return;
			}
			this.txtGoPage.Text = "";
			this.btnLogSearchView.Enabled = false;
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			string value;
			if (this.txtkey.TextLength != 0)
			{
				value = this.txtkey.Text.Trim();
			}
			else
			{
				value = null;
			}
			int num;
			if (this.rbInclude.Checked)
			{
				num = 1;
			}
			else
			{
				if (this.rbExclude.Checked)
				{
					num = 2;
				}
				else
				{
					num = 0;
					this.dtpLogtimeFrom.Value = System.DateTime.Now;
					this.dtpLogtimeTo.Value = System.DateTime.Now;
				}
			}
			System.DateTime value2 = this.dtpLogtimeFrom.Value;
			System.DateTime value3 = this.dtpLogtimeTo.Value;
			LogSysLog.nTatalLogs = 0;
			LogSysLog.nCurrentPage = 1;
			LogSysLog.nTotalPage = 0;
			int rowCount = LogInfo.GetRowCount(num, value2, value3);
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			arrayList.Add(value);
			arrayList.Add(num);
			arrayList.Add(value2);
			arrayList.Add(value3);
			DataTable dtab_logs;
			if (rowCount > 50000)
			{
				progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_searchLog, new string[0]), null, new progressPopup.ProcessInThread(this.searchLogProc), arrayList, 0);
				progressPopup.ShowDialog();
				object obj = progressPopup.Return_V;
				dtab_logs = (obj as DataTable);
			}
			else
			{
				object obj = this.searchLogProc(arrayList);
				dtab_logs = (obj as DataTable);
			}
			this.displayFoundLogs(dtab_logs);
		}
		private object searchLogProc(object param)
		{
			DataTable result = null;
			System.Collections.ArrayList arrayList = param as System.Collections.ArrayList;
			string s_keyword = arrayList[0] as string;
			int i_range_type = (int)arrayList[1];
			System.DateTime dt_start = (System.DateTime)arrayList[2];
			System.DateTime dt_end = (System.DateTime)arrayList[3];
			LogSysLog.nTatalLogs = LogAPI.searchLogs(s_keyword, i_range_type, dt_start, dt_end);
			if (LogSysLog.nTatalLogs > 0)
			{
				LogSysLog.nTotalPage = LogSysLog.nTatalLogs / LogSysLog.nPerPageLogs;
				if (LogSysLog.nTatalLogs % LogSysLog.nPerPageLogs != 0)
				{
					LogSysLog.nTotalPage++;
				}
				result = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			}
			return result;
		}
		private void btnLogSearchView_Click(object sender, System.EventArgs e)
		{
			this.dataGridViewLogs.DataSource = null;
			this.dataGridViewLogs.Update();
			int arg_1E_0 = LogSysLog.nCurrentPage;
			if (LogSysLog.goPage >= LogSysLog.nTotalPage)
			{
				LogSysLog.goPage = LogSysLog.nTotalPage;
				LogSysLog.nCurrentPage = LogSysLog.nTotalPage;
			}
			else
			{
				if (LogSysLog.goPage < 1)
				{
					LogSysLog.goPage = 1;
					LogSysLog.nCurrentPage = 1;
				}
				else
				{
					LogSysLog.nCurrentPage = LogSysLog.goPage;
				}
			}
			DataTable dtab_logs = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			this.displayFoundLogs(dtab_logs);
		}
		private object getOnePageLogProc(object param)
		{
			return LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
		}
		private void btnFirstPage_Click(object sender, System.EventArgs e)
		{
			LogSysLog.nCurrentPage = 1;
			DataTable dtab_logs = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			this.displayFoundLogs(dtab_logs);
		}
		private void btnPreviewPage_Click(object sender, System.EventArgs e)
		{
			LogSysLog.nCurrentPage--;
			DataTable dtab_logs = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			this.displayFoundLogs(dtab_logs);
		}
		private void btnNextPage_Click(object sender, System.EventArgs e)
		{
			LogSysLog.nCurrentPage++;
			DataTable dtab_logs = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			this.displayFoundLogs(dtab_logs);
		}
		private void btnLastPage_Click(object sender, System.EventArgs e)
		{
			LogSysLog.nCurrentPage = LogSysLog.nTotalPage;
			DataTable dtab_logs = LogAPI.dispOnePageLogs(LogSysLog.nCurrentPage, LogSysLog.nPerPageLogs);
			this.displayFoundLogs(dtab_logs);
		}
		private void btnExport_Click(object sender, System.EventArgs e)
		{
			System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.SaveFile));
			thread.SetApartmentState(System.Threading.ApartmentState.STA);
			if (thread.ThreadState != System.Threading.ThreadState.Running)
			{
				thread.Start();
				thread.Join();
			}
			if (this.SaveSuccess == 1)
			{
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			}
			else
			{
				if (this.SaveSuccess == 2)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				}
			}
			this.SaveSuccess = 0;
		}
		private void SaveFile()
		{
			switch (EcoLanguage.getLang())
			{
			case 0:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			case 1:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("de");
				break;
			case 2:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("es");
				break;
			case 3:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr");
				break;
			case 4:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("it");
				break;
			case 5:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ja");
				break;
			case 6:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ko");
				break;
			case 7:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("pt");
				break;
			case 8:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
				break;
			case 9:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHS");
				break;
			case 10:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CHT");
				break;
			default:
				System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
				break;
			}
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;
			saveFileDialog.CreatePrompt = true;
			saveFileDialog.Title = "Excel";
			if (this.checkBox_SaveAll.Checked)
			{
				LogSysLog.SaveRange = 1;
			}
			else
			{
				LogSysLog.SaveRange = 0;
			}
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				System.IO.Stream stream;
				try
				{
					stream = saveFileDialog.OpenFile();
				}
				catch (System.Exception)
				{
					this.SaveSuccess = 2;
					return;
				}
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(stream, System.Text.Encoding.Unicode);
				string text = "";
				try
				{
					for (int i = 0; i < this.dataGridViewLogs.Columns.Count; i++)
					{
						string name = this.dataGridViewLogs.Columns[i].Name;
						if (text.Length == 0)
						{
							text += name;
						}
						else
						{
							text = text + "\t" + name;
						}
					}
					streamWriter.WriteLine(text);
					if (LogSysLog.SaveRange == 0)
					{
						for (int j = 0; j < this.dataGridViewLogs.Rows.Count; j++)
						{
							string text2 = "";
							for (int k = 0; k < this.dataGridViewLogs.Columns.Count; k++)
							{
								if (k > 0)
								{
									text2 += "\t";
								}
								if (this.dataGridViewLogs.Rows[j].Cells[k].Value == null)
								{
									text2 = (text2 ?? "");
								}
								else
								{
									text2 += this.dataGridViewLogs.Rows[j].Cells[k].Value.ToString().Trim();
								}
							}
							streamWriter.WriteLine(text2);
						}
					}
					else
					{
						DataTable allLogs = LogAPI.getAllLogs();
						for (int l = 0; l < allLogs.Rows.Count; l++)
						{
							string text3 = "";
							for (int m = 0; m < allLogs.Columns.Count; m++)
							{
								if (m > 0)
								{
									text3 += "\t";
								}
								if (allLogs.Rows[l][m].ToString() == null)
								{
									text3 = (text3 ?? "");
								}
								else
								{
									text3 += allLogs.Rows[l][m].ToString().Trim();
								}
							}
							streamWriter.WriteLine(text3);
						}
					}
					streamWriter.Close();
					stream.Close();
					this.SaveSuccess = 1;
				}
				catch (System.Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
				finally
				{
					streamWriter.Close();
					stream.Close();
				}
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LogSysLog));
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.panellist = new Panel();
			this.checkBox_SaveAll = new CheckBox();
			this.txtPageCount = new Label();
			this.pictureBoxLoading = new PictureBox();
			this.lblTitle = new Label();
			this.btnExport = new Button();
			this.label2 = new Label();
			this.label1 = new Label();
			this.txtkey = new TextBox();
			this.btnSearch = new Button();
			this.dtpLogtimeTo = new DateTimePicker();
			this.label363 = new Label();
			this.dtpLogtimeFrom = new DateTimePicker();
			this.label365 = new Label();
			this.lab_key = new Label();
			this.btnLogSearchView = new Button();
			this.btnLastPage = new Button();
			this.btnNextPage = new Button();
			this.btnPreviewPage = new Button();
			this.btnFirstPage = new Button();
			this.txtGoPage = new TextBox();
			this.labelPage = new Label();
			this.groupBox1 = new GroupBox();
			this.rbInclude = new RadioButton();
			this.rbExclude = new RadioButton();
			this.rbAll = new RadioButton();
			this.dataGridViewLogs = new DataGridView();
			this.panellist.SuspendLayout();
			((ISupportInitialize)this.pictureBoxLoading).BeginInit();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.dataGridViewLogs).BeginInit();
			base.SuspendLayout();
			this.panellist.Controls.Add(this.checkBox_SaveAll);
			this.panellist.Controls.Add(this.txtPageCount);
			this.panellist.Controls.Add(this.pictureBoxLoading);
			this.panellist.Controls.Add(this.lblTitle);
			this.panellist.Controls.Add(this.btnExport);
			this.panellist.Controls.Add(this.label2);
			this.panellist.Controls.Add(this.label1);
			this.panellist.Controls.Add(this.txtkey);
			this.panellist.Controls.Add(this.btnSearch);
			this.panellist.Controls.Add(this.dtpLogtimeTo);
			this.panellist.Controls.Add(this.label363);
			this.panellist.Controls.Add(this.dtpLogtimeFrom);
			this.panellist.Controls.Add(this.label365);
			this.panellist.Controls.Add(this.lab_key);
			this.panellist.Controls.Add(this.btnLogSearchView);
			this.panellist.Controls.Add(this.btnLastPage);
			this.panellist.Controls.Add(this.btnNextPage);
			this.panellist.Controls.Add(this.btnPreviewPage);
			this.panellist.Controls.Add(this.btnFirstPage);
			this.panellist.Controls.Add(this.txtGoPage);
			this.panellist.Controls.Add(this.labelPage);
			this.panellist.Controls.Add(this.groupBox1);
			this.panellist.Controls.Add(this.dataGridViewLogs);
			componentResourceManager.ApplyResources(this.panellist, "panellist");
			this.panellist.Name = "panellist";
			componentResourceManager.ApplyResources(this.checkBox_SaveAll, "checkBox_SaveAll");
			this.checkBox_SaveAll.Name = "checkBox_SaveAll";
			this.checkBox_SaveAll.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.txtPageCount, "txtPageCount");
			this.txtPageCount.Name = "txtPageCount";
			this.pictureBoxLoading.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.pictureBoxLoading, "pictureBoxLoading");
			this.pictureBoxLoading.Image = Resources.loader;
			this.pictureBoxLoading.Name = "pictureBoxLoading";
			this.pictureBoxLoading.TabStop = false;
			this.lblTitle.BackColor = Color.Gainsboro;
			this.lblTitle.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
			this.lblTitle.ForeColor = Color.FromArgb(20, 73, 160);
			this.lblTitle.Name = "lblTitle";
			this.btnExport.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnExport, "btnExport");
			this.btnExport.Image = Resources.l_save;
			this.btnExport.Name = "btnExport";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.txtkey, "txtkey");
			this.txtkey.Name = "txtkey";
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
			componentResourceManager.ApplyResources(this.dtpLogtimeTo, "dtpLogtimeTo");
			this.dtpLogtimeTo.Format = DateTimePickerFormat.Custom;
			this.dtpLogtimeTo.Name = "dtpLogtimeTo";
			this.dtpLogtimeTo.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label363, "label363");
			this.label363.ForeColor = SystemColors.ControlText;
			this.label363.Name = "label363";
			componentResourceManager.ApplyResources(this.dtpLogtimeFrom, "dtpLogtimeFrom");
			this.dtpLogtimeFrom.Format = DateTimePickerFormat.Custom;
			this.dtpLogtimeFrom.Name = "dtpLogtimeFrom";
			this.dtpLogtimeFrom.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label365, "label365");
			this.label365.ForeColor = SystemColors.ControlText;
			this.label365.Name = "label365";
			componentResourceManager.ApplyResources(this.lab_key, "lab_key");
			this.lab_key.ForeColor = SystemColors.ControlText;
			this.lab_key.Name = "lab_key";
			this.btnLogSearchView.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnLogSearchView, "btnLogSearchView");
			this.btnLogSearchView.Image = Resources.l_search;
			this.btnLogSearchView.Name = "btnLogSearchView";
			this.btnLogSearchView.UseVisualStyleBackColor = true;
			this.btnLogSearchView.Click += new System.EventHandler(this.btnLogSearchView_Click);
			this.btnLastPage.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnLastPage, "btnLastPage");
			this.btnLastPage.Image = Resources.l_goLast;
			this.btnLastPage.Name = "btnLastPage";
			this.btnLastPage.UseVisualStyleBackColor = true;
			this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
			this.btnNextPage.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnNextPage, "btnNextPage");
			this.btnNextPage.Image = Resources.l_goNext;
			this.btnNextPage.Name = "btnNextPage";
			this.btnNextPage.UseVisualStyleBackColor = true;
			this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
			this.btnPreviewPage.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnPreviewPage, "btnPreviewPage");
			this.btnPreviewPage.Image = Resources.l_goPrev;
			this.btnPreviewPage.Name = "btnPreviewPage";
			this.btnPreviewPage.UseVisualStyleBackColor = true;
			this.btnPreviewPage.Click += new System.EventHandler(this.btnPreviewPage_Click);
			this.btnFirstPage.FlatAppearance.BorderSize = 0;
			componentResourceManager.ApplyResources(this.btnFirstPage, "btnFirstPage");
			this.btnFirstPage.Image = Resources.l_goFirst;
			this.btnFirstPage.Name = "btnFirstPage";
			this.btnFirstPage.UseVisualStyleBackColor = true;
			this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
			componentResourceManager.ApplyResources(this.txtGoPage, "txtGoPage");
			this.txtGoPage.Name = "txtGoPage";
			this.txtGoPage.TextChanged += new System.EventHandler(this.txtGoPage_TextChanged);
			componentResourceManager.ApplyResources(this.labelPage, "labelPage");
			this.labelPage.Name = "labelPage";
			this.groupBox1.Controls.Add(this.rbInclude);
			this.groupBox1.Controls.Add(this.rbExclude);
			this.groupBox1.Controls.Add(this.rbAll);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.rbInclude, "rbInclude");
			this.rbInclude.ForeColor = SystemColors.ControlText;
			this.rbInclude.Name = "rbInclude";
			this.rbInclude.TabStop = true;
			this.rbInclude.UseVisualStyleBackColor = true;
			this.rbInclude.CheckedChanged += new System.EventHandler(this.rbInclude_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbExclude, "rbExclude");
			this.rbExclude.ForeColor = SystemColors.ControlText;
			this.rbExclude.Name = "rbExclude";
			this.rbExclude.TabStop = true;
			this.rbExclude.UseVisualStyleBackColor = true;
			this.rbExclude.CheckedChanged += new System.EventHandler(this.rbExclude_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbAll, "rbAll");
			this.rbAll.ForeColor = SystemColors.ControlText;
			this.rbAll.Name = "rbAll";
			this.rbAll.TabStop = true;
			this.rbAll.UseVisualStyleBackColor = true;
			this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
			this.dataGridViewLogs.AllowUserToAddRows = false;
			this.dataGridViewLogs.BackgroundColor = Color.WhiteSmoke;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle.BackColor = Color.Gainsboro;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 9f);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dataGridViewLogs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			componentResourceManager.ApplyResources(this.dataGridViewLogs, "dataGridViewLogs");
			this.dataGridViewLogs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dataGridViewLogs.Name = "dataGridViewLogs";
			this.dataGridViewLogs.RowHeadersVisible = false;
			this.dataGridViewLogs.RowTemplate.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
			this.dataGridViewLogs.RowTemplate.ReadOnly = true;
			this.dataGridViewLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridViewLogs.StandardTab = true;
			this.dataGridViewLogs.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panellist);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LogSysLog";
			this.panellist.ResumeLayout(false);
			this.panellist.PerformLayout();
			((ISupportInitialize)this.pictureBoxLoading).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.dataGridViewLogs).EndInit();
			base.ResumeLayout(false);
		}
	}
}
