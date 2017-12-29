using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManZone
{
	public class ZoneInfoDlg : Form
	{
		private IContainer components;
		private Label labColor;
		private Button butColor;
		private TextBox tbZoneNm;
		private Button butCancel;
		private Button butSave;
		private Label lbZoneNm;
		private ColorDialog colorDialog1;
		private Label label1;
		private GroupBox groupBox1;
		private ManZone m_parent2;
		private long m_zoneID;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ZoneInfoDlg));
			this.labColor = new Label();
			this.butColor = new Button();
			this.tbZoneNm = new TextBox();
			this.butCancel = new Button();
			this.butSave = new Button();
			this.lbZoneNm = new Label();
			this.colorDialog1 = new ColorDialog();
			this.label1 = new Label();
			this.groupBox1 = new GroupBox();
			base.SuspendLayout();
			this.labColor.BackColor = Color.YellowGreen;
			this.labColor.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.labColor, "labColor");
			this.labColor.Name = "labColor";
			componentResourceManager.ApplyResources(this.butColor, "butColor");
			this.butColor.Name = "butColor";
			this.butColor.UseVisualStyleBackColor = true;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			this.tbZoneNm.AcceptsReturn = true;
			componentResourceManager.ApplyResources(this.tbZoneNm, "tbZoneNm");
			this.tbZoneNm.Name = "tbZoneNm";
			this.tbZoneNm.KeyPress += new KeyPressEventHandler(this.tbZoneNm_KeyPress);
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.butSave, "butSave");
			this.butSave.Name = "butSave";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			componentResourceManager.ApplyResources(this.lbZoneNm, "lbZoneNm");
			this.lbZoneNm.Name = "lbZoneNm";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.label1);
			base.Controls.Add(this.labColor);
			base.Controls.Add(this.butColor);
			base.Controls.Add(this.tbZoneNm);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.butSave);
			base.Controls.Add(this.lbZoneNm);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ZoneInfoDlg";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ZoneInfoDlg()
		{
			this.InitializeComponent();
			this.tbZoneNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public ZoneInfoDlg(ManZone pparent, long zoneID)
		{
			this.InitializeComponent();
			this.tbZoneNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_parent2 = pparent;
			this.m_zoneID = zoneID;
			if (this.m_zoneID != -1L)
			{
				this.initPage(zoneID);
			}
		}
		private void initPage(long zoneID)
		{
			ZoneInfo zoneByID = ZoneInfo.getZoneByID(zoneID);
			if (zoneByID == null)
			{
				return;
			}
			this.tbZoneNm.Text = zoneByID.ZoneName;
			this.labColor.BackColor = Color.FromArgb(System.Convert.ToInt32(zoneByID.ZoneColor));
		}
		private void butSave_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbZoneNm, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbZoneNm.Text
				}));
				return;
			}
			if (this.zoneNmExisted())
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Zone_nmdup, new string[]
				{
					this.tbZoneNm.Text
				}));
				this.tbZoneNm.Focus();
				return;
			}
			if (this.colorExisted())
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Zone_colordup, new string[]
				{
					this.tbZoneNm.Text
				}));
				return;
			}
			int num;
			if (this.m_zoneID == -1L)
			{
				num = this.m_parent2.addZone(this.tbZoneNm.Text, this.labColor.BackColor);
			}
			else
			{
				num = this.m_parent2.modifyZone(this.m_zoneID, this.tbZoneNm.Text, this.labColor.BackColor);
			}
			if (num == 1)
			{
				base.Close();
			}
		}
		private bool zoneNmExisted()
		{
			return !ZoneInfo.CheckName(this.m_zoneID, this.tbZoneNm.Text);
		}
		private bool colorExisted()
		{
			return this.labColor.BackColor.Equals(Color.Blue) || this.labColor.BackColor.Equals(Color.Yellow) || this.labColor.BackColor.Equals(Color.White) || !ZoneInfo.CheckColor(this.m_zoneID, this.labColor.BackColor.ToArgb());
		}
		private void tbZoneNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == ' ' || keyChar == '_')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void butColor_Click(object sender, System.EventArgs e)
		{
			DialogResult dialogResult = this.colorDialog1.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				this.labColor.BackColor = this.colorDialog1.Color;
			}
		}
	}
}
