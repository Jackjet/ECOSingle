using CommonAPI.InterProcess;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace EcoSensors.Login
{
	public class restoredb : Form
	{
		private IContainer components;
		private TabControl tpDbMaintenance;
		private TabPage tpDbImport;
		private TextBox tbImportFile;
		private Button btnImportFile;
		private Label lbImportFile;
		private Button btnDbImport;
		private Label label1;
		private Button btnCancel;
		private int m_exitPara = 1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(restoredb));
			this.tpDbMaintenance = new TabControl();
			this.tpDbImport = new TabPage();
			this.tbImportFile = new TextBox();
			this.btnImportFile = new Button();
			this.lbImportFile = new Label();
			this.btnDbImport = new Button();
			this.label1 = new Label();
			this.btnCancel = new Button();
			this.tpDbMaintenance.SuspendLayout();
			this.tpDbImport.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tpDbMaintenance, "tpDbMaintenance");
			this.tpDbMaintenance.Controls.Add(this.tpDbImport);
			this.tpDbMaintenance.Name = "tpDbMaintenance";
			this.tpDbMaintenance.SelectedIndex = 0;
			this.tpDbImport.BackColor = Color.WhiteSmoke;
			this.tpDbImport.BorderStyle = BorderStyle.FixedSingle;
			this.tpDbImport.Controls.Add(this.btnCancel);
			this.tpDbImport.Controls.Add(this.tbImportFile);
			this.tpDbImport.Controls.Add(this.btnImportFile);
			this.tpDbImport.Controls.Add(this.lbImportFile);
			this.tpDbImport.Controls.Add(this.btnDbImport);
			componentResourceManager.ApplyResources(this.tpDbImport, "tpDbImport");
			this.tpDbImport.Name = "tpDbImport";
			componentResourceManager.ApplyResources(this.tbImportFile, "tbImportFile");
			this.tbImportFile.Name = "tbImportFile";
			this.btnImportFile.BackColor = Color.Gainsboro;
			this.btnImportFile.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnImportFile, "btnImportFile");
			this.btnImportFile.Name = "btnImportFile";
			this.btnImportFile.UseVisualStyleBackColor = false;
			this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
			componentResourceManager.ApplyResources(this.lbImportFile, "lbImportFile");
			this.lbImportFile.Name = "lbImportFile";
			this.btnDbImport.BackColor = Color.Gainsboro;
			this.btnDbImport.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnDbImport, "btnDbImport");
			this.btnDbImport.Name = "btnDbImport";
			this.btnDbImport.UseVisualStyleBackColor = false;
			this.btnDbImport.Click += new System.EventHandler(this.btnDbImport_Click);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = SystemColors.ControlText;
			this.label1.Name = "label1";
			this.btnCancel.BackColor = Color.Gainsboro;
			this.btnCancel.Cursor = Cursors.Hand;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label1);
			base.Controls.Add(this.tpDbMaintenance);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "restoredb";
			base.FormClosing += new FormClosingEventHandler(this.restoredb_FormClosing);
			this.tpDbMaintenance.ResumeLayout(false);
			this.tpDbImport.ResumeLayout(false);
			this.tpDbImport.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public restoredb(int exitPara)
		{
			this.InitializeComponent();
			this.m_exitPara = exitPara;
		}
		private void btnImportFile_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = this.tbImportFile.Text;
			openFileDialog.Filter = "bak files (*.bak)|*.bak|All Files (*.*)|*.*";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.tbImportFile.Text = openFileDialog.FileName;
			}
		}
		private void btnDbImport_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbImportFile, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbImportFile.Text
				}));
				return;
			}
			if (!System.IO.File.Exists(this.tbImportFile.Text))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.File_unexist, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			DBTools.ProgramBar_Percent = 1;
			progressPopup progressPopup = new progressPopup("Information", 2, EcoLanguage.getMsg(LangRes.PopProgressMsg_Checkfile, new string[0]), null, new progressPopup.ProcessInThread(this.dbCheckImportFile), this.tbImportFile.Text, new progressPopup.ProgramBarThread(this.dbCheckImportFileBar), 0);
			progressPopup.ShowDialog();
			object return_V = progressPopup.Return_V;
			string text = return_V as string;
			if (text == null || text.Length == 0)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.File_illegal, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			if (text.StartsWith("DISKSIZELOW"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBImportFail2, new string[0]));
				this.tbImportFile.Focus();
				return;
			}
			if (text.StartsWith("UNZIP_ERROR"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBunziperror, new string[0]));
				return;
			}
			string text2 = "MYSQLVERSIONERROR;";
			if (text.StartsWith(text2))
			{
				string text3 = text.Substring(text2.Length);
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.needHighVerMySQL, new string[]
				{
					text3
				}));
				return;
			}
			if (text.StartsWith("DBVERSION_ERROR"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DBImportLowVerError, new string[0]));
				return;
			}
			if (text.StartsWith("MYSQL_CONNECT_ERROR"))
			{
				mysqlsetting mysqlsetting = new mysqlsetting(text);
				DialogResult dialogResult = mysqlsetting.ShowDialog();
				if (dialogResult != DialogResult.OK)
				{
					return;
				}
				string text4 = DBUtil.CheckMySQLVersion("127.0.0.1", mysqlsetting.DBPort, mysqlsetting.DBusrnm, mysqlsetting.DBPsw, mysqlsetting.mySQLVer);
				if (text4.Length > 0)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.needHighVerMySQL, new string[]
					{
						text4
					}));
					return;
				}
				string[] array = text.Split(new string[]
				{
					","
				}, System.StringSplitOptions.RemoveEmptyEntries);
				string text5 = array[5];
				text = string.Concat(new object[]
				{
					"127.0.0.1,",
					mysqlsetting.DBPort,
					",",
					mysqlsetting.DBusrnm,
					",",
					mysqlsetting.DBPsw,
					",",
					text5,
					",RESET"
				});
			}
			DialogResult dialogResult2 = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DB_ChangeCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult2 == DialogResult.Cancel)
			{
				return;
			}
			DBTools.ProgramBar_Percent = 1;
			progressPopup = new progressPopup("Information", 2, EcoLanguage.getMsg(LangRes.PopProgressMsg_ImportDB, new string[0]), null, new progressPopup.ProcessInThread(this.dbImportPro), text, new progressPopup.ProgramBarThread(this.dbImportProBar), 0);
			progressPopup.ShowDialog();
			return_V = progressPopup.Return_V;
			int? num = return_V as int?;
			if (!num.HasValue || num < 0)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				return;
			}
			base.DialogResult = DialogResult.OK;
		}
		private object dbCheckImportFileBar(ref int ibarper, ref string sbartype)
		{
			ibarper = DBTools.ProgramBar_Percent;
			int arg_0C_0 = DBTools.ProgramBar_Type;
			string text = "";
			sbartype = text;
			return "";
		}
		private object dbCheckImportFile(object filename)
		{
			return DBTools.CheckDatabase((string)filename);
		}
		private object dbImportProBar(ref int ibarper, ref string sbartype)
		{
			ibarper = DBTools.ProgramBar_Percent;
			int arg_0C_0 = DBTools.ProgramBar_Type;
			string text = "";
			sbartype = text;
			return "";
		}
		private object dbImportPro(object param)
		{
			string str_file = (string)param;
			EcoGlobalVar.ECOAppRunStatus = 2;
			Program.StopService(EcoGlobalVar.gl_ServiceName, 30000);
			System.Threading.Thread.Sleep(3000);
			bool flag = false;
			try
			{
				flag = DBTools.ImportDatabase(str_file);
			}
			catch (System.Exception)
			{
			}
			InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
			EcoGlobalVar.ECOAppRunStatus = 1;
			if (!flag)
			{
				return -1;
			}
			return 1;
		}
		private void restoredb_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (base.DialogResult != DialogResult.OK && this.m_exitPara == 1)
			{
				if (EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Login_quit, new string[0]), MessageBoxButtons.YesNo) == DialogResult.No)
				{
					e.Cancel = true;
					return;
				}
				Program.ExitApp();
			}
		}
	}
}
