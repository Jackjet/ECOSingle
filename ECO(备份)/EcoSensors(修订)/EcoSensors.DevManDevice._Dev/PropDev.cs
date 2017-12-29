using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice._Dev
{
	public class PropDev : UserControl
	{
		private IContainer components;
		private RadioButton rbdev1;
		private RadioButton rbdev2;
		private PropDev1 propDev1;
		private PropDev2 propDev2;
		public PropDev()
		{
			this.InitializeComponent();
			this.rbdev1.Checked = true;
		}
		public void pageInit(DevManDevice pParent, int devID, bool onlinest)
		{
			this.propDev1.pageInit(pParent, devID, onlinest);
			this.rbdev2.Visible = true;
			this.propDev2.pageInit(devID, onlinest);
		}
		public void TimerProc(bool onlinest, int haveThresholdChange)
		{
			this.propDev1.TimerProc(onlinest, haveThresholdChange);
			this.propDev2.TimerProc(onlinest, haveThresholdChange);
		}
		private void rbdev1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rbdev1.Checked)
			{
				this.propDev1.Visible = true;
				this.propDev2.Visible = false;
				return;
			}
			this.propDev1.Visible = false;
			this.propDev2.Visible = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PropDev));
			this.rbdev1 = new RadioButton();
			this.rbdev2 = new RadioButton();
			this.propDev2 = new PropDev2();
			this.propDev1 = new PropDev1();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.rbdev1, "rbdev1");
			this.rbdev1.Name = "rbdev1";
			this.rbdev1.TabStop = true;
			this.rbdev1.UseVisualStyleBackColor = true;
			this.rbdev1.CheckedChanged += new System.EventHandler(this.rbdev1_CheckedChanged);
			componentResourceManager.ApplyResources(this.rbdev2, "rbdev2");
			this.rbdev2.Name = "rbdev2";
			this.rbdev2.TabStop = true;
			this.rbdev2.UseVisualStyleBackColor = true;
			this.propDev2.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propDev2, "propDev2");
			this.propDev2.Name = "propDev2";
			this.propDev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.propDev1, "propDev1");
			this.propDev1.Name = "propDev1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.propDev2);
			base.Controls.Add(this.propDev1);
			base.Controls.Add(this.rbdev2);
			base.Controls.Add(this.rbdev1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "PropDev";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
