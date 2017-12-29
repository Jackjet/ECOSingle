using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.DevManDCFloorGrid;
using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace EcoSensors.DevManRack
{
	public class RackInfoDlg : Form
	{
		private IContainer components;
		private Button butDone;
		private Button butCancel;
		private TextBox tbColumn;
		private Label lbColumn;
		private TextBox tbRow;
		private Label lbRow;
		private RadioButton rbV;
		private RadioButton rbH;
		private TextBox tbRackNm;
		private Label lbRackNm;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private TextBox tbRackFNm;
		private Label lbRackFNm;
		private ManRack m_Parent2;
		private long m_rackID;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(RackInfoDlg));
			this.butDone = new Button();
			this.butCancel = new Button();
			this.tbColumn = new TextBox();
			this.lbColumn = new Label();
			this.tbRow = new TextBox();
			this.lbRow = new Label();
			this.rbV = new RadioButton();
			this.rbH = new RadioButton();
			this.tbRackNm = new TextBox();
			this.lbRackNm = new Label();
			this.groupBox1 = new GroupBox();
			this.groupBox2 = new GroupBox();
			this.tbRackFNm = new TextBox();
			this.lbRackFNm = new Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.butDone, "butDone");
			this.butDone.Name = "butDone";
			this.butDone.UseVisualStyleBackColor = true;
			this.butDone.Click += new System.EventHandler(this.butDone_Click);
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tbColumn, "tbColumn");
			this.tbColumn.Name = "tbColumn";
			this.tbColumn.KeyPress += new KeyPressEventHandler(this.point_KeyPress);
			componentResourceManager.ApplyResources(this.lbColumn, "lbColumn");
			this.lbColumn.Name = "lbColumn";
			componentResourceManager.ApplyResources(this.tbRow, "tbRow");
			this.tbRow.Name = "tbRow";
			this.tbRow.KeyPress += new KeyPressEventHandler(this.point_KeyPress);
			componentResourceManager.ApplyResources(this.lbRow, "lbRow");
			this.lbRow.Name = "lbRow";
			componentResourceManager.ApplyResources(this.rbV, "rbV");
			this.rbV.Name = "rbV";
			this.rbV.TabStop = true;
			this.rbV.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.rbH, "rbH");
			this.rbH.Checked = true;
			this.rbH.Name = "rbH";
			this.rbH.TabStop = true;
			this.rbH.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.tbRackNm, "tbRackNm");
			this.tbRackNm.Name = "tbRackNm";
			this.tbRackNm.KeyPress += new KeyPressEventHandler(this.rackNm_KeyPress);
			componentResourceManager.ApplyResources(this.lbRackNm, "lbRackNm");
			this.lbRackNm.Name = "lbRackNm";
			this.groupBox1.Controls.Add(this.lbRow);
			this.groupBox1.Controls.Add(this.tbRow);
			this.groupBox1.Controls.Add(this.lbColumn);
			this.groupBox1.Controls.Add(this.tbColumn);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.groupBox2.Controls.Add(this.rbH);
			this.groupBox2.Controls.Add(this.rbV);
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.tbRackFNm, "tbRackFNm");
			this.tbRackFNm.Name = "tbRackFNm";
			componentResourceManager.ApplyResources(this.lbRackFNm, "lbRackFNm");
			this.lbRackFNm.Name = "lbRackFNm";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.tbRackFNm);
			base.Controls.Add(this.lbRackFNm);
			base.Controls.Add(this.butDone);
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.tbRackNm);
			base.Controls.Add(this.lbRackNm);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RackInfoDlg";
			base.ShowInTaskbar = false;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public RackInfoDlg()
		{
			this.InitializeComponent();
			this.tbRackNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbRow.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbColumn.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
		}
		public RackInfoDlg(ManRack parent, long rackID)
		{
			this.InitializeComponent();
			this.tbRackNm.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbRow.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.tbColumn.ContextMenuStrip = EcoGlobalVar.nullcontextMenuStrip;
			this.m_Parent2 = parent;
			this.m_rackID = rackID;
			if (Sys_Para.GetRackFullNameflag() == 1)
			{
				this.lbRackFNm.Visible = true;
				this.tbRackFNm.Visible = true;
			}
			else
			{
				this.lbRackFNm.Visible = false;
				this.tbRackFNm.Visible = false;
			}
			if (rackID == -1L)
			{
				return;
			}
			this.initPage(rackID);
		}
		private void initPage(long rackID)
		{
			RackInfo rackByID = RackInfo.getRackByID(rackID);
			if (rackByID == null)
			{
				return;
			}
			int num = rackByID.StartPoint_X + 1;
			int num2 = rackByID.StartPoint_Y + 1;
			int num3 = rackByID.EndPoint_X + 1;
			int num4 = 0;
			if (num != num3)
			{
				num4 = 1;
			}
			this.tbRackNm.Text = rackByID.OriginalName;
			this.tbRackFNm.Text = rackByID.RackFullName;
			this.tbRow.Text = num.ToString();
			this.tbColumn.Text = num2.ToString();
			if (num4 == 0)
			{
				this.rbH.Checked = true;
				return;
			}
			this.rbV.Checked = true;
		}
		private void butDone_Click(object sender, System.EventArgs e)
		{
			if (!this.checkValue())
			{
				return;
			}
			string text = this.tbRackNm.Text.Trim();
			string text2 = text;
			if (this.tbRackFNm.Visible)
			{
				text2 = this.tbRackFNm.Text.Trim();
			}
			int row = (int)(System.Convert.ToInt16(this.tbRow.Text) - 1);
			int column = (int)(System.Convert.ToInt16(this.tbColumn.Text) - 1);
			string direction = this.rbH.Checked ? "H" : "V";
			bool flag;
			if (this.m_rackID == -1L)
			{
				flag = this.m_Parent2.addRack(column, row, direction, text, text2);
			}
			else
			{
				flag = this.m_Parent2.modifyRack(this.m_rackID, column, row, direction, text, text2);
			}
			if (flag)
			{
				base.Close();
			}
		}
		private bool checkValue()
		{
			string text = this.tbRow.Text;
			string text2 = this.tbColumn.Text;
			string text3 = this.rbH.Checked ? "H" : "V";
			bool flag = false;
			Ecovalidate.checkTextIsNull(this.tbRackNm, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbRackNm.Text
				}));
				return false;
			}
			string text4 = this.tbRackNm.Text;
			Ecovalidate.checkTextIsNull(this.tbRow, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbRow.Text
				}));
				return false;
			}
			int cur_DCRows = DevManFloorGrids.Cur_DCRows;
			int cur_DCColumns = DevManFloorGrids.Cur_DCColumns;
			if (!Ecovalidate.Rangeint(this.tbRow, 1, cur_DCRows))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbRow.Text,
					"1",
					cur_DCRows.ToString()
				}));
				return false;
			}
			Ecovalidate.checkTextIsNull(this.tbColumn, ref flag);
			if (flag)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Required, new string[]
				{
					this.lbColumn.Text
				}));
				return false;
			}
			if (!Ecovalidate.Rangeint(this.tbColumn, 1, cur_DCColumns))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Range, new string[]
				{
					this.lbColumn.Text,
					"1",
					cur_DCColumns.ToString()
				}));
				return false;
			}
			if ((int)System.Convert.ToInt16(text) == cur_DCRows && text3.Equals("V"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
				this.tbRow.Focus();
				return false;
			}
			if ((int)System.Convert.ToInt16(text2) == cur_DCColumns && text3.Equals("H"))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
				this.tbColumn.Focus();
				return false;
			}
			if (!RackInfo.CheckOriginalName(this.m_rackID, text4))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_nmdup, new string[]
				{
					text4
				}));
				this.tbRackNm.Focus();
				return false;
			}
			return true;
		}
		private void rackNm_KeyPress(object sender, KeyPressEventArgs e)
		{
			char keyChar = e.KeyChar;
			if ((keyChar >= '0' && keyChar <= '9') || (keyChar >= 'A' && keyChar <= 'Z') || (keyChar >= 'a' && keyChar <= 'z'))
			{
				return;
			}
			if (keyChar == '_' || keyChar == ' ')
			{
				return;
			}
			if (keyChar == '\b')
			{
				return;
			}
			e.Handled = true;
		}
		private void point_KeyPress(object sender, KeyPressEventArgs e)
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
