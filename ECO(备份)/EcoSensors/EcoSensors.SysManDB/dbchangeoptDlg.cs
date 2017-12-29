using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManDB
{
	public class dbchangeoptDlg : Form
	{
		private IContainer components;
		private RadioButton rbKeepHistory;
		private RadioButton rbLostHistory;
		private Button butNext;
		private RichTextBox richTextBox1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(dbchangeoptDlg));
			this.rbKeepHistory = new RadioButton();
			this.rbLostHistory = new RadioButton();
			this.butNext = new Button();
			this.richTextBox1 = new RichTextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.rbKeepHistory, "rbKeepHistory");
			this.rbKeepHistory.Name = "rbKeepHistory";
			this.rbKeepHistory.TabStop = true;
			this.rbKeepHistory.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbLostHistory, "rbLostHistory");
			this.rbLostHistory.Name = "rbLostHistory";
			this.rbLostHistory.TabStop = true;
			this.rbLostHistory.UseVisualStyleBackColor = true;
			this.butNext.BackColor = SystemColors.Control;
			this.butNext.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butNext, "butNext");
			this.butNext.Name = "butNext";
			this.butNext.UseVisualStyleBackColor = false;
			this.butNext.Click += new System.EventHandler(this.butNext_Click);
			this.richTextBox1.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			base.AutoScaleMode = AutoScaleMode.None;
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.butNext);
			base.Controls.Add(this.rbLostHistory);
			base.Controls.Add(this.rbKeepHistory);
			base.Controls.Add(this.richTextBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dbchangeoptDlg";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
		}
		public dbchangeoptDlg()
		{
			this.InitializeComponent();
		}
		private void butNext_Click(object sender, System.EventArgs e)
		{
			if (this.rbKeepHistory.Checked)
			{
				base.DialogResult = DialogResult.Yes;
				return;
			}
			base.DialogResult = DialogResult.No;
		}
		private void richTextBox1_TextChanged(object sender, System.EventArgs e)
		{
		}
	}
}
