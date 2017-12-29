using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class AssignNameParaDlg : Form
	{
		public const int ASSNAME_PARA_Cancel = -1;
		public const int ASSNAME_PARA_MODELRACK = 1;
		public const int ASSNAME_PARA_MODEL = 2;
		private IContainer components;
		private RadioButton rbMrack;
		private RadioButton rbModel;
		private Button butAssign;
		private Button butCancel;
		private GroupBox groupBox1;
		private int m_asspara = -1;
		public int AssignPara
		{
			get
			{
				return this.m_asspara;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AssignNameParaDlg));
			this.rbMrack = new RadioButton();
			this.rbModel = new RadioButton();
			this.butAssign = new Button();
			this.butCancel = new Button();
			this.groupBox1 = new GroupBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.rbMrack, "rbMrack");
			this.rbMrack.Checked = true;
			this.rbMrack.Name = "rbMrack";
			this.rbMrack.TabStop = true;
			this.rbMrack.Tag = "Model_Rack";
			this.rbMrack.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbModel, "rbModel");
			this.rbModel.Name = "rbModel";
			this.rbModel.Tag = "Model";
			this.rbModel.UseVisualStyleBackColor = true;
			this.butAssign.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.butAssign, "butAssign");
			this.butAssign.Name = "butAssign";
			this.butAssign.UseVisualStyleBackColor = false;
			this.butAssign.Click += new System.EventHandler(this.butAssign_Click);
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.BackColor = SystemColors.Control;
			this.butCancel.DialogResult = DialogResult.Cancel;
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = false;
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.rbMrack);
			base.Controls.Add(this.rbModel);
			base.Controls.Add(this.butAssign);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AssignNameParaDlg";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public AssignNameParaDlg()
		{
			this.InitializeComponent();
		}
		private void butAssign_Click(object sender, System.EventArgs e)
		{
			if (this.rbMrack.Checked)
			{
				this.m_asspara = 1;
			}
			else
			{
				if (this.rbModel.Checked)
				{
					this.m_asspara = 2;
				}
			}
			base.Close();
			base.Dispose();
		}
	}
}
