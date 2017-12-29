using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManDB
{
	public class SysManDB : UserControl
	{
		private IContainer components;
		private FlowLayoutPanel flowLayoutPanelSysManPage;
		private Button butSysParaDBCap;
		private Button butSysParaDBMaint;
		private SysManDBCap sysManDBCap1;
		private SysManDBMaint sysManDBMaint1;
		private Panel panel1;
		private Panel panel2;
		private Button butSysParaDBSet;
		private SysManDBSetting sysManDBSetting1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SysManDB));
			this.flowLayoutPanelSysManPage = new FlowLayoutPanel();
			this.butSysParaDBSet = new Button();
			this.butSysParaDBCap = new Button();
			this.butSysParaDBMaint = new Button();
			this.panel1 = new Panel();
			this.panel2 = new Panel();
			this.sysManDBCap1 = new SysManDBCap();
			this.sysManDBSetting1 = new SysManDBSetting();
			this.sysManDBMaint1 = new SysManDBMaint();
			this.flowLayoutPanelSysManPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.flowLayoutPanelSysManPage.BackColor = Color.Gainsboro;
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysParaDBSet);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysParaDBCap);
			this.flowLayoutPanelSysManPage.Controls.Add(this.butSysParaDBMaint);
			componentResourceManager.ApplyResources(this.flowLayoutPanelSysManPage, "flowLayoutPanelSysManPage");
			this.flowLayoutPanelSysManPage.Name = "flowLayoutPanelSysManPage";
			componentResourceManager.ApplyResources(this.butSysParaDBSet, "butSysParaDBSet");
			this.butSysParaDBSet.MinimumSize = new Size(150, 27);
			this.butSysParaDBSet.Name = "butSysParaDBSet";
			this.butSysParaDBSet.Tag = "DB Settings";
			this.butSysParaDBSet.UseVisualStyleBackColor = true;
			this.butSysParaDBSet.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysParaDBCap, "butSysParaDBCap");
			this.butSysParaDBCap.MinimumSize = new Size(150, 27);
			this.butSysParaDBCap.Name = "butSysParaDBCap";
			this.butSysParaDBCap.Tag = "DB Capacity";
			this.butSysParaDBCap.UseVisualStyleBackColor = true;
			this.butSysParaDBCap.Click += new System.EventHandler(this.comm_butClick);
			componentResourceManager.ApplyResources(this.butSysParaDBMaint, "butSysParaDBMaint");
			this.butSysParaDBMaint.MinimumSize = new Size(150, 27);
			this.butSysParaDBMaint.Name = "butSysParaDBMaint";
			this.butSysParaDBMaint.Tag = "DB Maintenance";
			this.butSysParaDBMaint.UseVisualStyleBackColor = true;
			this.butSysParaDBMaint.Click += new System.EventHandler(this.comm_butClick);
			this.panel1.BackColor = Color.Gainsboro;
			this.panel1.Controls.Add(this.flowLayoutPanelSysManPage);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.panel2.Controls.Add(this.sysManDBCap1);
			this.panel2.Controls.Add(this.sysManDBSetting1);
			this.panel2.Controls.Add(this.sysManDBMaint1);
			componentResourceManager.ApplyResources(this.panel2, "panel2");
			this.panel2.Name = "panel2";
			this.sysManDBCap1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManDBCap1, "sysManDBCap1");
			this.sysManDBCap1.Name = "sysManDBCap1";
			this.sysManDBSetting1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManDBSetting1, "sysManDBSetting1");
			this.sysManDBSetting1.Name = "sysManDBSetting1";
			this.sysManDBMaint1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.sysManDBMaint1, "sysManDBMaint1");
			this.sysManDBMaint1.Name = "sysManDBMaint1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.panel2);
			base.Controls.Add(this.panel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "SysManDB";
			this.flowLayoutPanelSysManPage.ResumeLayout(false);
			this.flowLayoutPanelSysManPage.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public SysManDB()
		{
			this.InitializeComponent();
		}
		public void pageInit(int selIndex)
		{
			switch (selIndex)
			{
			case 0:
				this.comm_butClick(this.butSysParaDBSet, null);
				return;
			case 1:
				this.comm_butClick(this.butSysParaDBCap, null);
				return;
			case 2:
				this.comm_butClick(this.butSysParaDBMaint, null);
				return;
			default:
				return;
			}
		}
		private void comm_butClick(object sender, System.EventArgs e)
		{
			this.sysManDBCap1.Visible = false;
			this.sysManDBMaint1.Visible = false;
			this.sysManDBSetting1.Visible = false;
			EcoGlobalVar.gl_SysDBCapCtrl.endDBcapTimer();
			foreach (Control control in this.flowLayoutPanelSysManPage.Controls)
			{
				Font font = control.Font;
				if (control.Tag.Equals(((Button)sender).Tag))
				{
					((Button)sender).Font = new Font(font.FontFamily, font.Size, FontStyle.Bold);
					string name = ((Button)sender).Name;
					if (name.Equals(this.butSysParaDBCap.Name))
					{
						this.sysManDBCap1.Visible = true;
						this.sysManDBCap1.pageInit();
					}
					else
					{
						if (name.Equals(this.butSysParaDBMaint.Name))
						{
							this.sysManDBMaint1.Visible = true;
							this.sysManDBMaint1.pageInit();
						}
						else
						{
							if (name.Equals(this.butSysParaDBSet.Name))
							{
								this.sysManDBSetting1.Visible = true;
								this.sysManDBSetting1.pageInit();
							}
						}
					}
				}
				else
				{
					((Button)control).Font = new Font(font.FontFamily, font.Size, FontStyle.Regular);
				}
			}
		}
	}
}
