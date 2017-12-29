using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.Analysis
{
	public class EGenRptSetPUE : Form
	{
		private IContainer components;
		private TableLayoutPanel tableLayoutPanel1;
		private TextBox textBoxTP4;
		private Label labelGT3;
		private TextBox textBoxTP3;
		private Label labelGN4;
		private TextBox textBoxTP2;
		private Label labelGN3;
		private Label labelGN2;
		private Label labelGT2;
		private Label label3;
		private Label labelGT1;
		private Label label2;
		private TextBox textBoxTP1;
		private Label label1;
		private Label labelGN1;
		private Label labelGT4;
		private Button buttonOK;
		private Button butCancel;
		private ListView.ListViewItemCollection m_Grouplist;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EGenRptSetPUE));
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.textBoxTP4 = new TextBox();
			this.labelGT3 = new Label();
			this.textBoxTP3 = new TextBox();
			this.labelGN4 = new Label();
			this.textBoxTP2 = new TextBox();
			this.labelGN3 = new Label();
			this.labelGN2 = new Label();
			this.labelGT2 = new Label();
			this.label3 = new Label();
			this.labelGT1 = new Label();
			this.label2 = new Label();
			this.textBoxTP1 = new TextBox();
			this.label1 = new Label();
			this.labelGN1 = new Label();
			this.labelGT4 = new Label();
			this.buttonOK = new Button();
			this.butCancel = new Button();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.textBoxTP4, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelGT3, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.textBoxTP3, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelGN4, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.textBoxTP2, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelGN3, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelGN2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelGT2, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelGT1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxTP1, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelGN1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelGT4, 1, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.textBoxTP4, "textBoxTP4");
			this.textBoxTP4.Name = "textBoxTP4";
			this.textBoxTP4.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.labelGT3, "labelGT3");
			this.labelGT3.Name = "labelGT3";
			componentResourceManager.ApplyResources(this.textBoxTP3, "textBoxTP3");
			this.textBoxTP3.Name = "textBoxTP3";
			this.textBoxTP3.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.labelGN4, "labelGN4");
			this.labelGN4.Name = "labelGN4";
			componentResourceManager.ApplyResources(this.textBoxTP2, "textBoxTP2");
			this.textBoxTP2.Name = "textBoxTP2";
			this.textBoxTP2.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.labelGN3, "labelGN3");
			this.labelGN3.Name = "labelGN3";
			componentResourceManager.ApplyResources(this.labelGN2, "labelGN2");
			this.labelGN2.Name = "labelGN2";
			componentResourceManager.ApplyResources(this.labelGT2, "labelGT2");
			this.labelGT2.Name = "labelGT2";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.labelGT1, "labelGT1");
			this.labelGT1.Name = "labelGT1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.textBoxTP1, "textBoxTP1");
			this.textBoxTP1.Name = "textBoxTP1";
			this.textBoxTP1.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.labelGN1, "labelGN1");
			this.labelGN1.Name = "labelGN1";
			componentResourceManager.ApplyResources(this.labelGT4, "labelGT4");
			this.labelGT4.Name = "labelGT4";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Name = "EGenRptSetPUE";
			base.ShowInTaskbar = false;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
		public EGenRptSetPUE()
		{
			this.InitializeComponent();
			this.textBoxTP1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public EGenRptSetPUE(ListView.ListViewItemCollection Grouplist)
		{
			this.InitializeComponent();
			this.textBoxTP1.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP2.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP3.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.textBoxTP4.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_Grouplist = Grouplist;
			for (int i = 0; i < this.m_Grouplist.Count; i++)
			{
				ListViewItem listViewItem = this.m_Grouplist[i];
				switch (i)
				{
				case 0:
					this.labelGN1.Text = listViewItem.SubItems[1].Text;
					this.labelGT1.Text = listViewItem.SubItems[0].Text;
					this.labelGN1.Visible = true;
					this.labelGT1.Visible = true;
					this.textBoxTP1.Visible = true;
					break;
				case 1:
					this.labelGN2.Text = listViewItem.SubItems[1].Text;
					this.labelGT2.Text = listViewItem.SubItems[0].Text;
					this.labelGN2.Visible = true;
					this.labelGT2.Visible = true;
					this.textBoxTP2.Visible = true;
					break;
				case 2:
					this.labelGN3.Text = listViewItem.SubItems[1].Text;
					this.labelGT3.Text = listViewItem.SubItems[0].Text;
					this.labelGN3.Visible = true;
					this.labelGT3.Visible = true;
					this.textBoxTP3.Visible = true;
					break;
				case 3:
					this.labelGN4.Text = listViewItem.SubItems[1].Text;
					this.labelGT4.Text = listViewItem.SubItems[0].Text;
					this.labelGN4.Visible = true;
					this.labelGT4.Visible = true;
					this.textBoxTP4.Visible = true;
					break;
				}
			}
		}
		private void num_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if (keyChar >= '0' && keyChar <= '9')
			{
				return;
			}
			if (keyChar == '.' || keyChar == ',' || keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			bool flag = false;
			if (this.textBoxTP1.Visible)
			{
				Ecovalidate.checkTextIsNull(this.textBoxTP1, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.labelGN1.Text
					}));
					this.textBoxTP1.Focus();
					return;
				}
				if (!Ecovalidate.NumberFormat_double(this.textBoxTP1))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return;
				}
			}
			if (this.textBoxTP2.Visible)
			{
				Ecovalidate.checkTextIsNull(this.textBoxTP2, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.labelGN2.Text
					}));
					this.textBoxTP2.Focus();
					return;
				}
				if (!Ecovalidate.NumberFormat_double(this.textBoxTP2))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return;
				}
			}
			if (this.textBoxTP3.Visible)
			{
				Ecovalidate.checkTextIsNull(this.textBoxTP3, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.labelGN3.Text
					}));
					this.textBoxTP3.Focus();
					return;
				}
				if (!Ecovalidate.NumberFormat_double(this.textBoxTP3))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return;
				}
			}
			if (this.textBoxTP4.Visible)
			{
				Ecovalidate.checkTextIsNull(this.textBoxTP4, ref flag);
				if (flag)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
					{
						this.labelGN4.Text
					}));
					this.textBoxTP4.Focus();
					return;
				}
				if (!Ecovalidate.NumberFormat_double(this.textBoxTP4))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Comm_invalidNumber, new string[0]));
					return;
				}
			}
			for (int i = 0; i < this.m_Grouplist.Count; i++)
			{
				ListViewItem listViewItem = this.m_Grouplist[i];
				switch (i)
				{
				case 0:
				{
					string text = listViewItem.Tag.ToString();
					string[] array = text.Split(new char[]
					{
						'|'
					});
					string tag = string.Concat(new string[]
					{
						array[0],
						"|",
						array[1],
						"|",
						array[2],
						"|",
						this.textBoxTP1.Text
					});
					listViewItem.Tag = tag;
					break;
				}
				case 1:
				{
					string text = listViewItem.Tag.ToString();
					string[] array = text.Split(new char[]
					{
						'|'
					});
					string tag = string.Concat(new string[]
					{
						array[0],
						"|",
						array[1],
						"|",
						array[2],
						"|",
						this.textBoxTP2.Text
					});
					listViewItem.Tag = tag;
					break;
				}
				case 2:
				{
					string text = listViewItem.Tag.ToString();
					string[] array = text.Split(new char[]
					{
						'|'
					});
					string tag = string.Concat(new string[]
					{
						array[0],
						"|",
						array[1],
						"|",
						array[2],
						"|",
						this.textBoxTP3.Text
					});
					listViewItem.Tag = tag;
					break;
				}
				case 3:
				{
					string text = listViewItem.Tag.ToString();
					string[] array = text.Split(new char[]
					{
						'|'
					});
					string tag = string.Concat(new string[]
					{
						array[0],
						"|",
						array[1],
						"|",
						array[2],
						"|",
						this.textBoxTP4.Text
					});
					listViewItem.Tag = tag;
					break;
				}
				}
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
			base.Dispose();
		}
	}
}
