using CommonAPI.Global;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.MainForm
{
	public class IdleTmDlg : Form
	{
		private IContainer components;
		private Button butCancel;
		private Button butSave;
		private Label UnitIdleTimeout;
		private TextBox tbIdleTimeOut;
		private Label lbidletimeout;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IdleTmDlg));
			this.butCancel = new Button();
			this.butSave = new Button();
			this.UnitIdleTimeout = new Label();
			this.tbIdleTimeOut = new TextBox();
			this.lbidletimeout = new Label();
			base.SuspendLayout();
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			componentResourceManager.ApplyResources(this.UnitIdleTimeout, "UnitIdleTimeout");
			this.UnitIdleTimeout.ForeColor = Color.Black;
			this.UnitIdleTimeout.Name = "UnitIdleTimeout";
			componentResourceManager.ApplyResources(this.tbIdleTimeOut, "tbIdleTimeOut");
			this.tbIdleTimeOut.ForeColor = Color.Black;
			this.tbIdleTimeOut.Name = "tbIdleTimeOut";
			this.tbIdleTimeOut.KeyPress += new KeyPressEventHandler(this.digit_KeyPress);
			componentResourceManager.ApplyResources(this.lbidletimeout, "lbidletimeout");
			this.lbidletimeout.ForeColor = Color.Black;
			this.lbidletimeout.Name = "lbidletimeout";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.UnitIdleTimeout);
			base.Controls.Add(this.tbIdleTimeOut);
			base.Controls.Add(this.lbidletimeout);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.butSave);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "IdleTmDlg";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public IdleTmDlg()
		{
			this.InitializeComponent();
			this.tbIdleTimeOut.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			int idleTimeout = ValuePairs.getIdleTimeout(true);
			this.tbIdleTimeOut.Text = System.Convert.ToString(idleTimeout);
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbIdleTimeOut, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbidletimeout.Text
				}));
				return;
			}
			if (!Ecovalidate.Rangeint(this.tbIdleTimeOut, 0, 30))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbidletimeout.Text,
					"0",
					"30"
				}));
				return;
			}
			int num = System.Convert.ToInt32(this.tbIdleTimeOut.Text);
			ValuePairs.setIdleTimeout(num, true);
			Program.m_IdleTimeSet = num * Program.m_IdleTimeFact;
			EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
			base.DialogResult = DialogResult.OK;
			base.Close();
		}
		private void digit_KeyPress(object sender, KeyPressEventArgs e)
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
