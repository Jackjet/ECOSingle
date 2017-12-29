using CommonAPI.network;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using InSnergyAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.settings
{
	public class SysOthers : UserControl
	{
		private IContainer components;
		private GroupBox gbBillProt;
		private Label lbBPlistenPort;
		private TextBox tbBPlistenport;
		private Label lbBPsecuritystr;
		private TextBox tbBPsecuritystr;
		private Button butSysparaSave;
		private CheckBox cbEnableBP;
		private TextBox tbManagerPort;
		private Label lbManagerPort;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysOthers));
			this.gbBillProt = new GroupBox();
			this.cbEnableBP = new CheckBox();
			this.lbBPlistenPort = new Label();
			this.tbBPlistenport = new TextBox();
			this.lbBPsecuritystr = new Label();
			this.tbBPsecuritystr = new TextBox();
			this.butSysparaSave = new Button();
			this.tbManagerPort = new TextBox();
			this.lbManagerPort = new Label();
			this.gbBillProt.SuspendLayout();
			base.SuspendLayout();
			this.gbBillProt.Controls.Add(this.cbEnableBP);
			this.gbBillProt.Controls.Add(this.lbBPlistenPort);
			this.gbBillProt.Controls.Add(this.tbBPlistenport);
			this.gbBillProt.Controls.Add(this.lbBPsecuritystr);
			this.gbBillProt.Controls.Add(this.tbBPsecuritystr);
			componentResourceManager.ApplyResources(this.gbBillProt, "gbBillProt");
			this.gbBillProt.ForeColor = Color.FromArgb(20, 73, 160);
			this.gbBillProt.Name = "gbBillProt";
			this.gbBillProt.TabStop = false;
			componentResourceManager.ApplyResources(this.cbEnableBP, "cbEnableBP");
			this.cbEnableBP.Name = "cbEnableBP";
			this.cbEnableBP.UseVisualStyleBackColor = true;
			this.cbEnableBP.CheckedChanged += new System.EventHandler(this.cbEnableBP_CheckedChanged);
			componentResourceManager.ApplyResources(this.lbBPlistenPort, "lbBPlistenPort");
			this.lbBPlistenPort.ForeColor = Color.Black;
			this.lbBPlistenPort.Name = "lbBPlistenPort";
			componentResourceManager.ApplyResources(this.tbBPlistenport, "tbBPlistenport");
			this.tbBPlistenport.ForeColor = Color.Black;
			this.tbBPlistenport.Name = "tbBPlistenport";
			this.tbBPlistenport.KeyPress += new KeyPressEventHandler(this.tbBPlistenport_KeyPress);
			componentResourceManager.ApplyResources(this.lbBPsecuritystr, "lbBPsecuritystr");
			this.lbBPsecuritystr.ForeColor = Color.Black;
			this.lbBPsecuritystr.Name = "lbBPsecuritystr";
			componentResourceManager.ApplyResources(this.tbBPsecuritystr, "tbBPsecuritystr");
			this.tbBPsecuritystr.ForeColor = Color.Black;
			this.tbBPsecuritystr.Name = "tbBPsecuritystr";
			this.tbBPsecuritystr.KeyPress += new KeyPressEventHandler(this.tbBPsecuritystr_KeyPress);
			this.butSysparaSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSysparaSave, "butSysparaSave");
			this.butSysparaSave.ForeColor = SystemColors.ControlText;
			this.butSysparaSave.Name = "butSysparaSave";
			this.butSysparaSave.UseVisualStyleBackColor = false;
			this.butSysparaSave.Click += new System.EventHandler(this.butSysparaSave_Click);
			componentResourceManager.ApplyResources(this.tbManagerPort, "tbManagerPort");
			this.tbManagerPort.Name = "tbManagerPort";
			this.tbManagerPort.KeyPress += new KeyPressEventHandler(this.tbManagerPort_KeyPress);
			componentResourceManager.ApplyResources(this.lbManagerPort, "lbManagerPort");
			this.lbManagerPort.Name = "lbManagerPort";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.tbManagerPort);
			base.Controls.Add(this.lbManagerPort);
			base.Controls.Add(this.butSysparaSave);
			base.Controls.Add(this.gbBillProt);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysOthers";
			this.gbBillProt.ResumeLayout(false);
			this.gbBillProt.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public SysOthers()
		{
			this.InitializeComponent();
			this.tbBPlistenport.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbBPsecuritystr.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public void pageInit()
		{
			if (Sys_Para.GetBPFlag() == 0)
			{
				this.cbEnableBP.Checked = false;
				this.tbBPlistenport.Enabled = false;
				this.tbBPlistenport.Text = System.Convert.ToString(Sys_Para.GetBPPort());
				this.tbBPsecuritystr.Enabled = false;
				this.tbBPsecuritystr.Text = Sys_Para.GetBPSecurity();
			}
			else
			{
				this.cbEnableBP.Checked = true;
				this.tbBPlistenport.Enabled = true;
				this.tbBPlistenport.Text = System.Convert.ToString(Sys_Para.GetBPPort());
				this.tbBPsecuritystr.Enabled = true;
				this.tbBPsecuritystr.Text = Sys_Para.GetBPSecurity();
			}
			this.lbManagerPort.Hide();
			this.tbManagerPort.Hide();
		}
		private void cbEnableBP_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.cbEnableBP.Checked)
			{
				this.tbBPlistenport.Enabled = false;
				this.tbBPsecuritystr.Enabled = false;
				return;
			}
			this.tbBPlistenport.Enabled = true;
			this.tbBPsecuritystr.Enabled = true;
		}
		private void butSysparaSave_Click(object sender, System.EventArgs e)
		{
			int bPFlag = Sys_Para.GetBPFlag();
			int num = 0;
			int bPPort = Sys_Para.GetBPPort();
			int num2 = bPPort;
			string bPSecurity = Sys_Para.GetBPSecurity();
			string text = Sys_Para.GetBPSecurity();
			bool flag = false;
			if (this.cbEnableBP.Checked)
			{
				num = 1;
				Ecovalidate.checkTextIsNull(this.tbBPlistenport, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbBPlistenPort.Text
					}));
					return;
				}
				if (!Ecovalidate.Rangeint(this.tbBPlistenport, 1, 65535))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbBPlistenPort.Text,
						"1",
						"65535"
					}));
					return;
				}
				num2 = System.Convert.ToInt32(this.tbBPlistenport.Text);
				if (bPPort != num2)
				{
					bool flag2 = NetworkShareAccesser.TcpPortInUse(num2);
					if (flag2)
					{
						this.tbBPlistenport.Focus();
						EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Portconflict, new string[]
						{
							this.tbBPlistenport.Text
						}));
						return;
					}
				}
				Ecovalidate.checkTextIsNull(this.tbBPsecuritystr, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbBPsecuritystr.Text
					}));
					return;
				}
				text = this.tbBPsecuritystr.Text;
			}
			if (bPFlag != num || bPPort != num2 || !bPSecurity.Equals(text))
			{
				Sys_Para.SetBPFlag(num);
				Sys_Para.SetBPPort(num2);
				Sys_Para.SetBPSecurity(text);
				InSnergyService.RestartBillingProtocol(num == 1, num2, text);
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void tbBPlistenport_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\b' && (e.KeyChar > '9' || e.KeyChar < '0'))
			{
				e.Handled = true;
			}
		}
		private void tbBPsecuritystr_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void tbManagerPort_KeyPress(object sender, KeyPressEventArgs e)
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
	}
}
