using CommonAPI;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.Login
{
	public class mysqlsetting : Form
	{
		private int m_DBport;
		private string m_usrnm;
		private string m_psw;
		private string m_mySQLVer = "";
		private IContainer components;
		private Panel panel1;
		private Button btnsave;
		private GroupBox groupBox1;
		private TextBox textBoxMySQLPassword;
		private TextBox textBoxMySQLUsername;
		private TextBox textBoxMySQLPort;
		private Label label3;
		private Label label2;
		private Label label1;
		private Button btncancel;
		private Label label4;
		public int DBPort
		{
			get
			{
				return this.m_DBport;
			}
		}
		public string DBusrnm
		{
			get
			{
				return this.m_usrnm;
			}
		}
		public string DBPsw
		{
			get
			{
				return this.m_psw;
			}
		}
		public string mySQLVer
		{
			get
			{
				return this.m_mySQLVer;
			}
		}
		public mysqlsetting(string parastr)
		{
			this.InitializeComponent();
			string[] array = parastr.Split(new string[]
			{
				","
			}, System.StringSplitOptions.RemoveEmptyEntries);
			this.textBoxMySQLPort.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxMySQLUsername.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxMySQLPassword.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxMySQLPort.Text = array[2];
			this.textBoxMySQLUsername.Text = array[3];
			this.textBoxMySQLPassword.Text = array[4];
			if (array.Length > 6)
			{
				this.m_mySQLVer = array[6];
				return;
			}
			this.m_mySQLVer = "";
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
		private bool paraCheck()
		{
			bool flag = false;
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
			return true;
		}
		private void btncancel_Click(object sender, System.EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}
		private void btnsave_Click(object sender, System.EventArgs e)
		{
			if (!this.paraCheck())
			{
				return;
			}
			int num = System.Convert.ToInt32(this.textBoxMySQLPort.Text);
			string text = this.textBoxMySQLUsername.Text;
			string text2 = this.textBoxMySQLPassword.Text;
			int num2 = DBUtil.CheckMySQLParameter_NOName("127.0.0.1", num, text, text2);
			if (num2 != DebugCenter.ST_Success)
			{
				EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.DB_Connectfail, new string[0]));
				return;
			}
			this.m_DBport = num;
			this.m_usrnm = text;
			this.m_psw = text2;
			base.DialogResult = DialogResult.OK;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(mysqlsetting));
			this.panel1 = new Panel();
			this.btncancel = new Button();
			this.btnsave = new Button();
			this.groupBox1 = new GroupBox();
			this.textBoxMySQLPassword = new TextBox();
			this.textBoxMySQLUsername = new TextBox();
			this.textBoxMySQLPort = new TextBox();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.label4 = new Label();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.btncancel);
			this.panel1.Controls.Add(this.btnsave);
			this.panel1.Controls.Add(this.groupBox1);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.btncancel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btncancel, "btncancel");
			this.btncancel.ForeColor = SystemColors.ControlText;
			this.btncancel.Name = "btncancel";
			this.btncancel.UseVisualStyleBackColor = false;
			this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
			this.btnsave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.btnsave, "btnsave");
			this.btnsave.ForeColor = SystemColors.ControlText;
			this.btnsave.Name = "btnsave";
			this.btnsave.UseVisualStyleBackColor = false;
			this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
			this.groupBox1.Controls.Add(this.textBoxMySQLPassword);
			this.groupBox1.Controls.Add(this.textBoxMySQLUsername);
			this.groupBox1.Controls.Add(this.textBoxMySQLPort);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.textBoxMySQLPassword, "textBoxMySQLPassword");
			this.textBoxMySQLPassword.Name = "textBoxMySQLPassword";
			componentResourceManager.ApplyResources(this.textBoxMySQLUsername, "textBoxMySQLUsername");
			this.textBoxMySQLUsername.Name = "textBoxMySQLUsername";
			componentResourceManager.ApplyResources(this.textBoxMySQLPort, "textBoxMySQLPort");
			this.textBoxMySQLPort.Name = "textBoxMySQLPort";
			this.textBoxMySQLPort.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label4);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "mysqlsetting";
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
