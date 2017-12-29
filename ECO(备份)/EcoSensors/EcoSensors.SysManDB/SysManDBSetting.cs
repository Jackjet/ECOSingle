using CommonAPI;
using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EventLogAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
namespace EcoSensors.SysManDB
{
	public class SysManDBSetting : UserControl
	{
		private IContainer components;
		private Panel panel1;
		private RadioButton radioButtonLocalDB;
		private RadioButton radioButtonRemoteDB;
		private TextBox textBoxMySQLIP;
		private Label label4;
		private GroupBox groupBox2;
		private Panel panel2;
		private TextBox textBox2;
		private Label label6;
		private TextBox textBox1;
		private Label label5;
		private Button btnAccessFile;
		private GroupBox groupBox3;
		private Button btnsavedb;
		private GroupBox gbDBCleanupOpt;
		private GroupBox groupBox1;
		private TextBox textBoxMySQLPassword;
		private TextBox textBoxMySQLUsername;
		private CheckBox checkBoxUseMySQL;
		private TextBox textBoxMySQLPort;
		private Label label3;
		private Label label2;
		private Label label1;
		private CheckBox cbdelOlddata;
		private Label lbMonths;
		private TextBox tbkeepmonths;
		private CheckBox cbkeepdata;
		private TextBox tbIP;
		private Label lbIP;
		private Button btnsaveopt;
		public SysManDBSetting()
		{
			this.InitializeComponent();
			this.textBoxMySQLPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxMySQLUsername.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxMySQLPassword.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbkeepmonths.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit()
		{
			this.tbIP.Text = "127.0.0.1";
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				this.textBoxMySQLPort.Text = DBUrl.CURRENT_PORT.ToString();
				this.textBoxMySQLUsername.Text = DBUrl.CURRENT_USER_NAME;
				this.textBoxMySQLPassword.Text = DBUrl.CURRENT_PWD;
				this.checkBoxUseMySQL.Checked = true;
				this.textBoxMySQLPort.Enabled = false;
				this.textBoxMySQLUsername.Enabled = false;
				this.textBoxMySQLPassword.Enabled = false;
			}
			else
			{
				this.checkBoxUseMySQL.Checked = false;
				this.textBoxMySQLPort.Enabled = false;
				this.textBoxMySQLUsername.Enabled = false;
				this.textBoxMySQLPassword.Enabled = false;
			}
			this.btnsavedb.Enabled = false;
			if (Sys_Para.GetDBOpt_keepMMflag() == 1)
			{
				this.cbkeepdata.Checked = true;
				this.tbkeepmonths.Enabled = true;
				this.tbkeepmonths.Text = System.Convert.ToString(Sys_Para.GetDBOpt_keepMM());
			}
			else
			{
				this.cbkeepdata.Checked = false;
				this.tbkeepmonths.Enabled = false;
				this.tbkeepmonths.Text = System.Convert.ToString(Sys_Para.GetDBOpt_keepMM());
			}
			if (Sys_Para.GetDBOpt_deloldflag() == 1)
			{
				this.cbdelOlddata.Checked = true;
			}
			else
			{
				this.cbdelOlddata.Checked = false;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") && !DBMaintain.IsLocalIP(DBUrl.CURRENT_HOST_PATH))
			{
				this.cbdelOlddata.Visible = false;
				return;
			}
			this.cbdelOlddata.Visible = true;
		}
		private void checkBoxUseMySQL_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.checkBoxUseMySQL.Checked)
			{
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					this.tbIP.Enabled = false;
					this.textBoxMySQLPort.Enabled = false;
					this.textBoxMySQLUsername.Enabled = false;
					this.textBoxMySQLPassword.Enabled = false;
					this.btnsavedb.Enabled = false;
					return;
				}
				this.textBoxMySQLPort.Enabled = true;
				this.textBoxMySQLPort.Text = "3306";
				this.textBoxMySQLUsername.Enabled = true;
				this.textBoxMySQLPassword.Enabled = true;
				this.btnsavedb.Enabled = true;
				return;
			}
			else
			{
				this.tbIP.Enabled = false;
				this.textBoxMySQLPort.Enabled = false;
				this.textBoxMySQLUsername.Enabled = false;
				this.textBoxMySQLPassword.Enabled = false;
				if (!DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
				{
					this.btnsavedb.Enabled = false;
					return;
				}
				this.btnsavedb.Enabled = true;
				return;
			}
		}
		private void num_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private bool paraoptCheck()
		{
			bool flag = false;
			if (this.cbkeepdata.Checked)
			{
				Ecovalidate.checkTextIsNull(this.tbkeepmonths, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbMonths.Text
					}));
					return false;
				}
				if (!Ecovalidate.Rangeint(this.tbkeepmonths, 6, 60))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbMonths.Text,
						"6",
						"60"
					}));
					return false;
				}
			}
			return true;
		}
		private bool paradbCheck()
		{
			bool flag = false;
			if (this.checkBoxUseMySQL.Checked)
			{
				if (this.tbIP.Text.Equals(string.Empty))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIP.Focus();
					this.tbIP.BackColor = Color.Red;
					return false;
				}
				try
				{
					string text = IPAddress.Parse(this.tbIP.Text).ToString();
					this.tbIP.Text = text;
				}
				catch (System.Exception)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.IPFORMAT, new string[0]));
					this.tbIP.Focus();
					this.tbIP.BackColor = Color.Red;
					bool result = false;
					return result;
				}
				this.tbIP.BackColor = Color.White;
				Ecovalidate.checkTextIsNull(this.textBoxMySQLPort, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.label1.Text
					}));
					return false;
				}
				if (!Ecovalidate.Rangeint(this.textBoxMySQLPort, 1, 65535))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.label1.Text,
						"1",
						"65535"
					}));
					return false;
				}
				Ecovalidate.checkTextIsNull(this.textBoxMySQLUsername, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.label2.Text
					}));
					return false;
				}
				string[] param = new string[]
				{
					this.tbIP.Text,
					this.textBoxMySQLPort.Text,
					this.textBoxMySQLUsername.Text,
					this.textBoxMySQLPassword.Text
				};
				progressPopup progressPopup = new progressPopup("Information", 1, EcoLanguage.getMsg(LangRes.PopProgressMsg_Checkdbconnect, new string[0]), null, new progressPopup.ProcessInThread(this.dbCheckParameter), param, 0);
				progressPopup.ShowDialog();
				object return_V = progressPopup.Return_V;
				int? num = return_V as int?;
				if (!num.HasValue || num != DebugCenter.ST_Success)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_Connectfail, new string[0]));
					return false;
				}
				return true;
			}
			return true;
		}
		private object dbCheckParameter(object para)
		{
			string[] array = (string[])para;
			string str_host = array[0];
			int i_port = System.Convert.ToInt32(array[1]);
			string str_usr = array[2];
			string str_pwd = array[3];
			int num = DBUtil.CheckMySQLParameter_NOName(str_host, i_port, str_usr, str_pwd);
			return num;
		}
		private void btnsaveopt_Click(object sender, System.EventArgs e)
		{
			if (!this.paraoptCheck())
			{
				return;
			}
			if (this.cbkeepdata.Checked)
			{
				Sys_Para.SetDBOpt_keepMMflag(1);
				int dBOpt_keepMM = System.Convert.ToInt32(this.tbkeepmonths.Text);
				Sys_Para.SetDBOpt_keepMM(dBOpt_keepMM);
			}
			else
			{
				Sys_Para.SetDBOpt_keepMMflag(0);
			}
			if (this.cbdelOlddata.Visible)
			{
				if (this.cbdelOlddata.Checked)
				{
					Sys_Para.SetDBOpt_deloldflag(1);
				}
				else
				{
					Sys_Para.SetDBOpt_deloldflag(0);
				}
			}
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0130020", new string[]
				{
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0130020", new string[0]);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void btnsavedb_Click(object sender, System.EventArgs e)
		{
			if (!DBMaintain.ConvertOldDataFinish)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.DB_inSplitMySQLTable, new string[0]));
				return;
			}
			bool @checked = this.checkBoxUseMySQL.Checked;
			string dbIP = "127.0.0.1";
			int port = 3306;
			string usrnm = "";
			string psw = "";
			bool flag = false;
			if (@checked && DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				return;
			}
			if (!this.paradbCheck())
			{
				return;
			}
			if (@checked)
			{
				dbIP = this.tbIP.Text;
				port = System.Convert.ToInt32(this.textBoxMySQLPort.Text);
				usrnm = this.textBoxMySQLUsername.Text;
				psw = this.textBoxMySQLPassword.Text;
			}
			if (!DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") && !@checked)
			{
				return;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") && @checked)
			{
				return;
			}
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.DB_ChangeCrm, new string[0]), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			int opt = 2;
			if (!flag)
			{
				long num = DBTools.EvaluateTime();
				if (num > 60L)
				{
					dbchangeoptDlg dbchangeoptDlg = new dbchangeoptDlg();
					DialogResult dialogResult2 = dbchangeoptDlg.ShowDialog();
					if (dialogResult2 != DialogResult.Yes)
					{
						opt = 1;
					}
				}
			}
			dbchangeDlg dbchangeDlg = new dbchangeDlg(@checked, dbIP, port, usrnm, psw, opt);
			dbchangeDlg.ShowDialog();
			EcoMessageBox.ShowInfo(this, EcoLanguage.getMsg(LangRes.quitEco, new string[0]));
			Program.ExitApp();
		}
		private void cbkeepdata_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.cbkeepdata.Checked)
			{
				this.tbkeepmonths.Enabled = true;
				return;
			}
			this.tbkeepmonths.Enabled = false;
		}
		private void tbIP_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '.' || keyChar == '\b')
			{
				return;
			}
			if (char.IsPunctuation(keyChar))
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManDBSetting));
			this.panel1 = new Panel();
			this.gbDBCleanupOpt = new GroupBox();
			this.btnsaveopt = new Button();
			this.cbdelOlddata = new CheckBox();
			this.lbMonths = new Label();
			this.tbkeepmonths = new TextBox();
			this.cbkeepdata = new CheckBox();
			this.groupBox1 = new GroupBox();
			this.tbIP = new TextBox();
			this.btnsavedb = new Button();
			this.lbIP = new Label();
			this.textBoxMySQLPassword = new TextBox();
			this.textBoxMySQLUsername = new TextBox();
			this.checkBoxUseMySQL = new CheckBox();
			this.textBoxMySQLPort = new TextBox();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.radioButtonLocalDB = new RadioButton();
			this.radioButtonRemoteDB = new RadioButton();
			this.textBoxMySQLIP = new TextBox();
			this.label4 = new Label();
			this.groupBox2 = new GroupBox();
			this.btnAccessFile = new Button();
			this.textBox2 = new TextBox();
			this.label6 = new Label();
			this.textBox1 = new TextBox();
			this.label5 = new Label();
			this.panel2 = new Panel();
			this.groupBox3 = new GroupBox();
			this.panel1.SuspendLayout();
			this.gbDBCleanupOpt.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.gbDBCleanupOpt);
			this.panel1.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.gbDBCleanupOpt.Controls.Add(this.btnsaveopt);
			this.gbDBCleanupOpt.Controls.Add(this.cbdelOlddata);
			this.gbDBCleanupOpt.Controls.Add(this.lbMonths);
			this.gbDBCleanupOpt.Controls.Add(this.tbkeepmonths);
			this.gbDBCleanupOpt.Controls.Add(this.cbkeepdata);
			componentResourceManager.ApplyResources(this.gbDBCleanupOpt, "gbDBCleanupOpt");
			this.gbDBCleanupOpt.Name = "gbDBCleanupOpt";
			this.gbDBCleanupOpt.TabStop = false;
			this.btnsaveopt.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsaveopt, "btnsaveopt");
			this.btnsaveopt.ForeColor = SystemColors.ControlText;
			this.btnsaveopt.Name = "btnsaveopt";
			this.btnsaveopt.UseVisualStyleBackColor = false;
			this.btnsaveopt.Click += new System.EventHandler(this.btnsaveopt_Click);
			componentResourceManager.ApplyResources(this.cbdelOlddata, "cbdelOlddata");
			this.cbdelOlddata.Name = "cbdelOlddata";
			this.cbdelOlddata.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.lbMonths, "lbMonths");
			this.lbMonths.Name = "lbMonths";
			componentResourceManager.ApplyResources(this.tbkeepmonths, "tbkeepmonths");
			this.tbkeepmonths.Name = "tbkeepmonths";
			this.tbkeepmonths.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.cbkeepdata, "cbkeepdata");
			this.cbkeepdata.Name = "cbkeepdata";
			this.cbkeepdata.UseVisualStyleBackColor = true;
			this.cbkeepdata.CheckedChanged += new System.EventHandler(this.cbkeepdata_CheckedChanged);
			this.groupBox1.Controls.Add(this.tbIP);
			this.groupBox1.Controls.Add(this.btnsavedb);
			this.groupBox1.Controls.Add(this.lbIP);
			this.groupBox1.Controls.Add(this.textBoxMySQLPassword);
			this.groupBox1.Controls.Add(this.textBoxMySQLUsername);
			this.groupBox1.Controls.Add(this.checkBoxUseMySQL);
			this.groupBox1.Controls.Add(this.textBoxMySQLPort);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.tbIP, "tbIP");
			this.tbIP.Name = "tbIP";
			this.tbIP.KeyPress += new KeyPressEventHandler(this.tbIP_KeyPress);
			this.btnsavedb.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsavedb, "btnsavedb");
			this.btnsavedb.ForeColor = SystemColors.ControlText;
			this.btnsavedb.Name = "btnsavedb";
			this.btnsavedb.UseVisualStyleBackColor = false;
			this.btnsavedb.Click += new System.EventHandler(this.btnsavedb_Click);
			componentResourceManager.ApplyResources(this.lbIP, "lbIP");
			this.lbIP.Name = "lbIP";
			componentResourceManager.ApplyResources(this.textBoxMySQLPassword, "textBoxMySQLPassword");
			this.textBoxMySQLPassword.Name = "textBoxMySQLPassword";
			componentResourceManager.ApplyResources(this.textBoxMySQLUsername, "textBoxMySQLUsername");
			this.textBoxMySQLUsername.Name = "textBoxMySQLUsername";
			componentResourceManager.ApplyResources(this.checkBoxUseMySQL, "checkBoxUseMySQL");
			this.checkBoxUseMySQL.Name = "checkBoxUseMySQL";
			this.checkBoxUseMySQL.UseVisualStyleBackColor = true;
			this.checkBoxUseMySQL.CheckedChanged += new System.EventHandler(this.checkBoxUseMySQL_CheckedChanged);
			componentResourceManager.ApplyResources(this.textBoxMySQLPort, "textBoxMySQLPort");
			this.textBoxMySQLPort.Name = "textBoxMySQLPort";
			this.textBoxMySQLPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.radioButtonLocalDB, "radioButtonLocalDB");
			this.radioButtonLocalDB.Name = "radioButtonLocalDB";
			this.radioButtonLocalDB.TabStop = true;
			this.radioButtonLocalDB.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButtonRemoteDB, "radioButtonRemoteDB");
			this.radioButtonRemoteDB.Name = "radioButtonRemoteDB";
			this.radioButtonRemoteDB.TabStop = true;
			this.radioButtonRemoteDB.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.textBoxMySQLIP, "textBoxMySQLIP");
			this.textBoxMySQLIP.Name = "textBoxMySQLIP";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.groupBox2.Controls.Add(this.btnAccessFile);
			this.groupBox2.Controls.Add(this.textBox2);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textBox1);
			this.groupBox2.Controls.Add(this.label5);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.btnAccessFile.BackColor = Color.Gainsboro;
			this.btnAccessFile.Cursor = Cursors.Hand;
			componentResourceManager.ApplyResources(this.btnAccessFile, "btnAccessFile");
			this.btnAccessFile.Name = "btnAccessFile";
			this.btnAccessFile.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.textBox2, "textBox2");
			this.textBox2.Name = "textBox2";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.textBox1, "textBox1");
			this.textBox1.Name = "textBox1";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.panel2.Controls.Add(this.groupBox3);
			this.panel2.Controls.Add(this.groupBox2);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.groupBox3.Controls.Add(this.radioButtonRemoteDB);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.radioButtonLocalDB);
			this.groupBox3.Controls.Add(this.textBoxMySQLIP);
			componentResourceManager.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.panel2);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManDBSetting";
			this.panel1.ResumeLayout(false);
			this.gbDBCleanupOpt.ResumeLayout(false);
			this.gbDBCleanupOpt.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
