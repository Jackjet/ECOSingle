using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Billing
{
	public class BillingSettings : UserControl
	{
		private IContainer components;
		private TextBox tb1rate;
		private Label lbCurrency1;
		private RadioButton rbBilling1rate;
		private RadioButton rbBilling2rate;
		private TableLayoutPanel tblPanel2rate;
		private Label lbPeak;
		private Label lbFrom;
		private Label lbnonPeak;
		private Label lbDuration;
		private TextBox tb2Rate2;
		public DateTimePicker dtPicker2from1;
		private Label lbRCurrency2;
		private TextBox tb2Rate1;
		private Button butSysparaSave;
		private GroupBox groupBox1;
		private TextBox tbduration;
		private string m_lbCurrency1txt;
		private string m_lbRCurrencytxt;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BillingSettings));
			this.tb1rate = new TextBox();
			this.lbCurrency1 = new Label();
			this.rbBilling1rate = new RadioButton();
			this.rbBilling2rate = new RadioButton();
			this.tblPanel2rate = new TableLayoutPanel();
			this.tb2Rate2 = new TextBox();
			this.dtPicker2from1 = new DateTimePicker();
			this.lbFrom = new Label();
			this.lbnonPeak = new Label();
			this.lbPeak = new Label();
			this.lbDuration = new Label();
			this.lbRCurrency2 = new Label();
			this.tb2Rate1 = new TextBox();
			this.tbduration = new TextBox();
			this.butSysparaSave = new Button();
			this.groupBox1 = new GroupBox();
			this.tblPanel2rate.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tb1rate, "tb1rate");
			this.tb1rate.ForeColor = Color.Black;
			this.tb1rate.Name = "tb1rate";
			this.tb1rate.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.lbCurrency1, "lbCurrency1");
			this.lbCurrency1.ForeColor = Color.Black;
			this.lbCurrency1.Name = "lbCurrency1";
			componentResourceManager.ApplyResources(this.rbBilling1rate, "rbBilling1rate");
			this.rbBilling1rate.ForeColor = Color.Black;
			this.rbBilling1rate.Name = "rbBilling1rate";
			this.rbBilling1rate.TabStop = true;
			this.rbBilling1rate.UseVisualStyleBackColor = true;
			this.rbBilling1rate.CheckedChanged += new System.EventHandler(this.rbBillingrateType_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbBilling2rate, "rbBilling2rate");
			this.rbBilling2rate.ForeColor = Color.Black;
			this.rbBilling2rate.Name = "rbBilling2rate";
			this.rbBilling2rate.TabStop = true;
			this.rbBilling2rate.UseVisualStyleBackColor = true;
			this.rbBilling2rate.CheckedChanged += new System.EventHandler(this.rbBillingrateType_CheckedChanged);
			componentResourceManager.ApplyResources(this.tblPanel2rate, "tblPanel2rate");
			this.tblPanel2rate.Controls.Add(this.tb2Rate2, 3, 2);
			this.tblPanel2rate.Controls.Add(this.dtPicker2from1, 1, 1);
			this.tblPanel2rate.Controls.Add(this.lbFrom, 1, 0);
			this.tblPanel2rate.Controls.Add(this.lbnonPeak, 0, 2);
			this.tblPanel2rate.Controls.Add(this.lbPeak, 0, 1);
			this.tblPanel2rate.Controls.Add(this.lbDuration, 2, 0);
			this.tblPanel2rate.Controls.Add(this.lbRCurrency2, 3, 0);
			this.tblPanel2rate.Controls.Add(this.tb2Rate1, 3, 1);
			this.tblPanel2rate.Controls.Add(this.tbduration, 2, 1);
			this.tblPanel2rate.Name = "tblPanel2rate";
			componentResourceManager.ApplyResources(this.tb2Rate2, "tb2Rate2");
			this.tb2Rate2.ForeColor = Color.Black;
			this.tb2Rate2.Name = "tb2Rate2";
			this.tb2Rate2.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.dtPicker2from1, "dtPicker2from1");
			this.dtPicker2from1.Format = DateTimePickerFormat.Custom;
			this.dtPicker2from1.Name = "dtPicker2from1";
			this.dtPicker2from1.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.lbFrom, "lbFrom");
			this.lbFrom.ForeColor = SystemColors.ControlText;
			this.lbFrom.Name = "lbFrom";
			componentResourceManager.ApplyResources(this.lbnonPeak, "lbnonPeak");
			this.lbnonPeak.ForeColor = SystemColors.ControlText;
			this.lbnonPeak.Name = "lbnonPeak";
			componentResourceManager.ApplyResources(this.lbPeak, "lbPeak");
			this.lbPeak.ForeColor = SystemColors.ControlText;
			this.lbPeak.Name = "lbPeak";
			componentResourceManager.ApplyResources(this.lbDuration, "lbDuration");
			this.lbDuration.ForeColor = SystemColors.ControlText;
			this.lbDuration.Name = "lbDuration";
			componentResourceManager.ApplyResources(this.lbRCurrency2, "lbRCurrency2");
			this.lbRCurrency2.ForeColor = SystemColors.ControlText;
			this.lbRCurrency2.Name = "lbRCurrency2";
			componentResourceManager.ApplyResources(this.tb2Rate1, "tb2Rate1");
			this.tb2Rate1.ForeColor = Color.Black;
			this.tb2Rate1.Name = "tb2Rate1";
			this.tb2Rate1.KeyPress += new KeyPressEventHandler(this.double_KeyPress);
			componentResourceManager.ApplyResources(this.tbduration, "tbduration");
			this.tbduration.ForeColor = Color.Black;
			this.tbduration.Name = "tbduration";
			this.tbduration.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
			this.butSysparaSave.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butSysparaSave, "butSysparaSave");
			this.butSysparaSave.ForeColor = SystemColors.ControlText;
			this.butSysparaSave.Name = "butSysparaSave";
			this.butSysparaSave.UseVisualStyleBackColor = false;
			this.butSysparaSave.Click += new System.EventHandler(this.butSysparaSave_Click);
			this.groupBox1.Controls.Add(this.rbBilling2rate);
			this.groupBox1.Controls.Add(this.rbBilling1rate);
			this.groupBox1.Controls.Add(this.tblPanel2rate);
			this.groupBox1.Controls.Add(this.lbCurrency1);
			this.groupBox1.Controls.Add(this.tb1rate);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.ForeColor = Color.FromArgb(20, 73, 160);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.butSysparaSave);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "BillingSettings";
			this.tblPanel2rate.ResumeLayout(false);
			this.tblPanel2rate.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
		public BillingSettings()
		{
			this.InitializeComponent();
			this.tb1rate.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tb2Rate1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tb2Rate2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_lbCurrency1txt = this.lbCurrency1.Text;
			this.m_lbRCurrencytxt = this.lbRCurrency2.Text;
		}
		public void pageInit()
		{
			this.lbCurrency1.Text = string.Format(this.m_lbCurrency1txt, EcoGlobalVar.CurCurrency);
			this.lbRCurrency2.Text = string.Format(this.m_lbRCurrencytxt, EcoGlobalVar.CurCurrency);
			if (Sys_Para.GetBill_ratetype() == 0)
			{
				this.rbBilling1rate.Checked = true;
			}
			else
			{
				this.rbBilling2rate.Checked = true;
			}
			float bill_1rate = Sys_Para.GetBill_1rate();
			if (bill_1rate < 0f)
			{
				this.tb1rate.Text = "";
			}
			else
			{
				this.tb1rate.Text = bill_1rate.ToString("F2");
			}
			this.dtPicker2from1.Text = Sys_Para.GetBill_2from1();
			this.tbduration.Text = Sys_Para.GetBill_2duration1().ToString();
			float bill_2rate = Sys_Para.GetBill_2rate1();
			if (bill_2rate < 0f)
			{
				this.tb2Rate1.Text = "";
			}
			else
			{
				this.tb2Rate1.Text = bill_2rate.ToString("F2");
			}
			float bill_2rate2 = Sys_Para.GetBill_2rate2();
			if (bill_2rate2 < 0f)
			{
				this.tb2Rate2.Text = "";
				return;
			}
			this.tb2Rate2.Text = bill_2rate2.ToString("F2");
		}
		private void rbBillingrateType_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbBilling1rate.Checked)
			{
				this.tb1rate.Enabled = true;
				this.dtPicker2from1.Enabled = false;
				this.tbduration.Enabled = false;
				this.tb2Rate1.Enabled = false;
				this.tb2Rate2.Enabled = false;
				return;
			}
			this.tb1rate.Enabled = false;
			this.dtPicker2from1.Enabled = true;
			this.tbduration.Enabled = true;
			this.tb2Rate1.Enabled = true;
			this.tb2Rate2.Enabled = true;
		}
		private bool paraCheck()
		{
			bool flag = false;
			if (this.rbBilling1rate.Checked)
			{
				Ecovalidate.checkTextIsNull(this.tb1rate, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.rbBilling1rate.Text
					}));
					return false;
				}
				if (!Ecovalidate.NumberFormat_double(this.tb1rate))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return false;
				}
			}
			else
			{
				Ecovalidate.checkTextIsNull(this.tbduration, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbDuration.Text
					}));
					return false;
				}
				if (!Ecovalidate.Rangeint(this.tbduration, 1, 23))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
					{
						this.lbDuration.Text,
						"1",
						"23"
					}));
					return false;
				}
				Ecovalidate.checkTextIsNull(this.tb2Rate1, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbPeak.Text
					}));
					return false;
				}
				if (!Ecovalidate.NumberFormat_double(this.tb2Rate1))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return false;
				}
				Ecovalidate.checkTextIsNull(this.tb2Rate2, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.lbnonPeak.Text
					}));
					return false;
				}
				if (!Ecovalidate.NumberFormat_double(this.tb2Rate2))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return false;
				}
			}
			return true;
		}
		private void butSysparaSave_Click(object sender, System.EventArgs e)
		{
			if (!this.paraCheck())
			{
				return;
			}
			if (this.rbBilling1rate.Checked)
			{
				float bill_1rate = System.Convert.ToSingle(this.tb1rate.Text);
				if (Sys_Para.SetBill_ratetype(0) < 0 || Sys_Para.SetBill_1rate(bill_1rate) < 0)
				{
					EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
					return;
				}
			}
			else
			{
				float bill_2rate = System.Convert.ToSingle(this.tb2Rate1.Text);
				float bill_2rate2 = System.Convert.ToSingle(this.tb2Rate2.Text);
				string bill_2from = this.dtPicker2from1.Text + ":00:00";
				int bill_2duration = System.Convert.ToInt32(this.tbduration.Text);
				if (Sys_Para.SetBill_ratetype(1) < 0 || Sys_Para.SetBill_2from1(bill_2from) < 0 || Sys_Para.SetBill_2duration1(bill_2duration) < 0 || Sys_Para.SetBill_2rate1(bill_2rate) < 0 || Sys_Para.SetBill_2rate2(bill_2rate2) < 0)
				{
					EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
					return;
				}
			}
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
		}
		private void double_KeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (TextBox)sender;
			bool flag = Ecovalidate.inputCheck_float(tb, e.KeyChar, 2);
			if (flag)
			{
				return;
			}
			e.Handled = true;
		}
		private void int_KeyPress(object sender, KeyPressEventArgs e)
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
