using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.component;
using EcoSensors.DevManDCFloorGrid;
using EcoSensors.DevManDevice;
using EventLogAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManRack
{
	public class ManRack : UserControl
	{
		private IContainer components;
		private ListBox lbRack;
		private Button butModify;
		private Button butDel;
		private Button butNew;
		private DevManFloorGrids devManFloorGrids1;
		private Color ColorRack_noDev = Color.Silver;
		private Color ColorRack_selected = Color.DarkCyan;
		private Color ColorRack_haveDev = Color.Lime;
		private Color ColorLabel_ = Color.WhiteSmoke;
		private rackDeviceListUC rdl;
		private Popup toolTip1;
		private Rectangle dragBoxFromMouseDown = Rectangle.Empty;
		private int rowIndexFromMouseDown;
		private int colIndexFromMouseDown;
		private DataGridView dgvSetDevice;
		private System.Collections.ArrayList m_AllRacks;
		private int m_Curselindex;
		private System.Collections.Generic.Dictionary<long, RackInfo> m_MapID2Rack = new System.Collections.Generic.Dictionary<long, RackInfo>();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ManRack));
			this.lbRack = new ListBox();
			this.butModify = new Button();
			this.butDel = new Button();
			this.butNew = new Button();
			this.devManFloorGrids1 = new DevManFloorGrids();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.lbRack, "lbRack");
			this.lbRack.FormattingEnabled = true;
			this.lbRack.Name = "lbRack";
			this.lbRack.SelectedIndexChanged += new System.EventHandler(this.lbRack_SelectedIndexChanged);
			this.butModify.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butModify, "butModify");
			this.butModify.Name = "butModify";
			this.butModify.UseVisualStyleBackColor = false;
			this.butModify.Click += new System.EventHandler(this.butModify_Click);
			this.butDel.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butDel, "butDel");
			this.butDel.Name = "butDel";
			this.butDel.UseVisualStyleBackColor = false;
			this.butDel.Click += new System.EventHandler(this.butDel_Click);
			this.butNew.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butNew, "butNew");
			this.butNew.Name = "butNew";
			this.butNew.UseVisualStyleBackColor = false;
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			this.devManFloorGrids1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devManFloorGrids1, "devManFloorGrids1");
			this.devManFloorGrids1.Name = "devManFloorGrids1";
			this.devManFloorGrids1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.devManFloorGrids1);
			base.Controls.Add(this.lbRack);
			base.Controls.Add(this.butModify);
			base.Controls.Add(this.butDel);
			base.Controls.Add(this.butNew);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ManRack";
			base.VisibleChanged += new System.EventHandler(this.ManRack_VisibleChanged);
			base.ResumeLayout(false);
		}
		public ManRack()
		{
			this.InitializeComponent();
			this.dgvSetDevice = this.devManFloorGrids1.getRackCtrl();
		}
		public void init()
		{
			this.m_AllRacks = RackInfo.getAllRack();
			this.m_MapID2Rack.Clear();
			this.dgvSetDevice = this.devManFloorGrids1.getRackCtrl();
			this.dgvSetDevice.MultiSelect = false;
			this.dgvSetDevice.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			this.dgvSetDevice.DefaultCellStyle.SelectionBackColor = this.dgvSetDevice.DefaultCellStyle.BackColor;
			this.dgvSetDevice.DefaultCellStyle.SelectionForeColor = this.dgvSetDevice.DefaultCellStyle.ForeColor;
			this.dgvSetDevice.CellMouseEnter -= new DataGridViewCellEventHandler(this.Rack_CellMouseEnter);
			this.dgvSetDevice.CellMouseEnter += new DataGridViewCellEventHandler(this.Rack_CellMouseEnter);
			this.dgvSetDevice.CellMouseLeave -= new DataGridViewCellEventHandler(this.Rack_CellMouseLeave);
			this.dgvSetDevice.CellMouseLeave += new DataGridViewCellEventHandler(this.Rack_CellMouseLeave);
			this.dgvSetDevice.MouseLeave -= new System.EventHandler(this.dgvSetDevice_MouseLeave);
			this.dgvSetDevice.MouseLeave += new System.EventHandler(this.dgvSetDevice_MouseLeave);
			this.dgvSetDevice.ShowCellToolTips = true;
			this.dgvSetDevice.AllowDrop = true;
			this.dgvSetDevice.MouseDown -= new MouseEventHandler(this.Rack_MouseDown);
			this.dgvSetDevice.MouseDown += new MouseEventHandler(this.Rack_MouseDown);
			this.dgvSetDevice.DragEnter -= new DragEventHandler(this.Rack_DragEnter);
			this.dgvSetDevice.DragEnter += new DragEventHandler(this.Rack_DragEnter);
			this.dgvSetDevice.DragDrop -= new DragEventHandler(this.Rack_DragDrop);
			this.dgvSetDevice.DragDrop += new DragEventHandler(this.Rack_DragDrop);
			this.lbRack.Items.Clear();
			System.GC.Collect();
			this.dgvSetDevice.SuspendLayout();
			for (int i = 0; i < this.m_AllRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)this.m_AllRacks[i];
				this.m_MapID2Rack.Add(rackInfo.RackID, rackInfo);
				string deviceInfo = rackInfo.DeviceInfo;
				string displayRackName = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
				long rackID = rackInfo.RackID;
				this.lbRack.Items.Add(displayRackName);
				int startPoint_X = rackInfo.StartPoint_X;
				int startPoint_Y = rackInfo.StartPoint_Y;
				int endPoint_X = rackInfo.EndPoint_X;
				int arg_241_0 = rackInfo.EndPoint_Y;
				string text;
				if (startPoint_X == endPoint_X)
				{
					text = "H";
				}
				else
				{
					text = "V";
				}
				DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[startPoint_X].Cells[startPoint_Y];
				dataGridViewSpanCell.Tag = rackID;
				dataGridViewSpanCell.Value = deviceInfo;
				if (deviceInfo.Length > 0)
				{
					dataGridViewSpanCell.BackColor = this.ColorRack_haveDev;
				}
				else
				{
					dataGridViewSpanCell.BackColor = this.ColorRack_noDev;
				}
				if (text.Equals("H"))
				{
					dataGridViewSpanCell.SetColumnSpan(1);
					dataGridViewSpanCell.ToolTipText = displayRackName;
					DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[startPoint_X].Cells[startPoint_Y + 1];
					dataGridViewSpanCell2.Tag = rackID;
					dataGridViewSpanCell2.Value = deviceInfo;
					dataGridViewSpanCell2.BackColor = dataGridViewSpanCell.BackColor;
					dataGridViewSpanCell2.SetColumnSpan(-1);
					dataGridViewSpanCell2.ToolTipText = displayRackName;
				}
				else
				{
					if (text.Equals("V"))
					{
						dataGridViewSpanCell.SetRowSpan(1);
						dataGridViewSpanCell.ToolTipText = displayRackName;
						DataGridViewSpanCell dataGridViewSpanCell3 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[startPoint_X + 1].Cells[startPoint_Y];
						dataGridViewSpanCell3.Tag = rackID;
						dataGridViewSpanCell3.Value = deviceInfo;
						dataGridViewSpanCell3.BackColor = dataGridViewSpanCell.BackColor;
						dataGridViewSpanCell3.SetRowSpan(-1);
						dataGridViewSpanCell3.ToolTipText = displayRackName;
					}
				}
			}
			this.gridinit(this.dgvSetDevice);
			this.dgvSetDevice.ResumeLayout();
			if (this.lbRack.Items.Count > 0)
			{
				this.lbRack.SelectedIndex = 0;
				return;
			}
			this.butDel.Enabled = false;
			this.butModify.Enabled = false;
		}
		private void gridinit(DataGridView tbpanel)
		{
			for (int i = 0; i < tbpanel.RowCount; i++)
			{
				for (int j = 0; j < tbpanel.ColumnCount; j++)
				{
					DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j];
					if (dataGridViewSpanCell.GetRowSpan() == 0 && dataGridViewSpanCell.GetColumnSpan() == 0)
					{
						dataGridViewSpanCell.Value = "";
						dataGridViewSpanCell.BackColor = this.ColorLabel_;
						dataGridViewSpanCell.ToolTipText = "You can drag and move the rack";
					}
				}
			}
		}
		private void initRackList()
		{
			this.m_AllRacks = RackInfo.getAllRack();
			this.m_MapID2Rack.Clear();
			this.lbRack.Items.Clear();
			for (int i = 0; i < this.m_AllRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)this.m_AllRacks[i];
				this.m_MapID2Rack.Add(rackInfo.RackID, rackInfo);
				string displayRackName = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
				this.lbRack.Items.Add(displayRackName);
			}
			if (this.lbRack.Items.Count > 0)
			{
				this.lbRack.SelectedIndex = 0;
			}
		}
		private void Rack_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			int columnIndex = e.ColumnIndex;
			if (columnIndex < 0)
			{
				return;
			}
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex].Cells[columnIndex];
			if (dataGridViewSpanCell.GetColumnSpan() == 0 && dataGridViewSpanCell.GetRowSpan() == 0)
			{
				if (this.toolTip1 != null)
				{
					this.rdl = null;
					this.toolTip1.Close();
				}
				return;
			}
			long num = (long)dataGridViewSpanCell.Tag;
			string devIDs = dataGridViewSpanCell.Value.ToString();
			if (this.rdl != null)
			{
				if (this.rdl.RacKID == num)
				{
					return;
				}
				this.rdl = null;
			}
			if (this.toolTip1 != null)
			{
				this.toolTip1.Close();
			}
			RackInfo rackInfo = this.m_MapID2Rack[num];
			this.rdl = new rackDeviceListUC(num, rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag), devIDs);
			Screen[] allScreens = Screen.AllScreens;
			int width = allScreens[0].Bounds.Width;
			int height = allScreens[0].Bounds.Height;
			int x = Control.MousePosition.X + 1;
			int y = Control.MousePosition.Y + 1;
			if (width - Control.MousePosition.X < this.rdl.Size.Width)
			{
				x = Control.MousePosition.X - this.rdl.Size.Width;
			}
			if (height - Control.MousePosition.Y < this.rdl.Size.Height)
			{
				y = Control.MousePosition.Y - this.rdl.Size.Height;
			}
			Rectangle area = new Rectangle(x, y, 1, 1);
			this.toolTip1 = new Popup(this.rdl);
			this.toolTip1.AutoClose = false;
			this.toolTip1.FocusOnOpen = false;
			this.toolTip1.ShowingAnimation = (this.toolTip1.HidingAnimation = PopupAnimations.Blend);
			this.toolTip1.Show(area);
		}
		private void Rack_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void dgvSetDevice_MouseLeave(object sender, System.EventArgs e)
		{
			if (this.toolTip1 == null)
			{
				return;
			}
			this.rdl = null;
			this.toolTip1.Close();
		}
		private void Rack_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.toolTip1 != null)
			{
				this.rdl = null;
				this.toolTip1.Close();
			}
			DataGridView.HitTestInfo hitTestInfo = this.dgvSetDevice.HitTest(e.X, e.Y);
			this.rowIndexFromMouseDown = hitTestInfo.RowIndex;
			if (this.rowIndexFromMouseDown < 0)
			{
				return;
			}
			this.colIndexFromMouseDown = hitTestInfo.ColumnIndex;
			if (this.colIndexFromMouseDown < 0)
			{
				return;
			}
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[this.rowIndexFromMouseDown].Cells[this.colIndexFromMouseDown];
			if (dataGridViewSpanCell.GetColumnSpan() == 0 && dataGridViewSpanCell.GetRowSpan() == 0)
			{
				this.dragBoxFromMouseDown = Rectangle.Empty;
				return;
			}
			if (dataGridViewSpanCell.GetColumnSpan() < 0)
			{
				this.colIndexFromMouseDown--;
			}
			if (dataGridViewSpanCell.GetRowSpan() < 0)
			{
				this.rowIndexFromMouseDown--;
			}
			long key = (long)dataGridViewSpanCell.Tag;
			RackInfo rackInfo = this.m_MapID2Rack[key];
			this.lbRack.SelectedItem = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			if (this.dragBoxFromMouseDown != Rectangle.Empty && this.dragBoxFromMouseDown.Contains(e.X, e.Y))
			{
				return;
			}
			Size dragSize = SystemInformation.DragSize;
			this.dragBoxFromMouseDown = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
			this.dgvSetDevice.DoDragDrop(this.dgvSetDevice.Rows[this.rowIndexFromMouseDown].Cells[this.colIndexFromMouseDown], DragDropEffects.Move);
		}
		private void Rack_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}
		private void Rack_DragDrop(object sender, DragEventArgs e)
		{
			Point point = this.dgvSetDevice.PointToClient(new Point(e.X, e.Y));
			DataGridView.HitTestInfo hitTestInfo = this.dgvSetDevice.HitTest(point.X, point.Y);
			if (this.dragBoxFromMouseDown == Rectangle.Empty)
			{
				return;
			}
			this.dragBoxFromMouseDown = Rectangle.Empty;
			int rowIndex = hitTestInfo.RowIndex;
			if (rowIndex < 0)
			{
				return;
			}
			int columnIndex = hitTestInfo.ColumnIndex;
			if (columnIndex < 0)
			{
				return;
			}
			int num = this.colIndexFromMouseDown;
			int num2 = this.rowIndexFromMouseDown;
			if (num == columnIndex && num2 == rowIndex)
			{
				return;
			}
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[this.rowIndexFromMouseDown].Cells[this.colIndexFromMouseDown];
			DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex].Cells[columnIndex];
			string text;
			if (dataGridViewSpanCell.GetRowSpan() != 0)
			{
				text = "V";
			}
			else
			{
				text = "H";
			}
			if (text.Equals("H") && num2 == rowIndex && num + 1 == columnIndex)
			{
				return;
			}
			if (text.Equals("V") && num2 + 1 == rowIndex && num == columnIndex)
			{
				return;
			}
			System.Console.WriteLine(string.Concat(new object[]
			{
				"in Rack_DragDrop, From ",
				this.rowIndexFromMouseDown,
				":",
				this.colIndexFromMouseDown,
				"->",
				rowIndex,
				":",
				columnIndex
			}));
			if (!this.checkMove(text, rowIndex, columnIndex, (long)dataGridViewSpanCell.Tag))
			{
				return;
			}
			long num3 = (long)dataGridViewSpanCell.Tag;
			string toolTipText = dataGridViewSpanCell.ToolTipText;
			string value = dataGridViewSpanCell.Value.ToString();
			Color backColor = dataGridViewSpanCell.BackColor;
			DataGridViewSpanCell dataGridViewSpanCell3;
			DataGridViewSpanCell dataGridViewSpanCell4;
			if (text.Equals("V"))
			{
				dataGridViewSpanCell.Tag = null;
				dataGridViewSpanCell.Value = "";
				dataGridViewSpanCell.ToolTipText = "You can drag and move the rack";
				dataGridViewSpanCell.SetRowSpan(0);
				dataGridViewSpanCell.BackColor = this.ColorLabel_;
				dataGridViewSpanCell3 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[this.rowIndexFromMouseDown + 1].Cells[this.colIndexFromMouseDown];
				dataGridViewSpanCell3.Tag = null;
				dataGridViewSpanCell3.Value = "";
				dataGridViewSpanCell3.ToolTipText = "You can drag and move the rack";
				dataGridViewSpanCell3.SetRowSpan(0);
				dataGridViewSpanCell3.BackColor = this.ColorLabel_;
				dataGridViewSpanCell2.Tag = num3;
				dataGridViewSpanCell2.Value = value;
				dataGridViewSpanCell2.ToolTipText = toolTipText;
				dataGridViewSpanCell2.SetRowSpan(1);
				dataGridViewSpanCell2.BackColor = backColor;
				dataGridViewSpanCell4 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex + 1].Cells[columnIndex];
				dataGridViewSpanCell4.Tag = num3;
				dataGridViewSpanCell4.Value = value;
				dataGridViewSpanCell4.ToolTipText = toolTipText;
				dataGridViewSpanCell4.SetRowSpan(-1);
				dataGridViewSpanCell4.BackColor = backColor;
			}
			else
			{
				dataGridViewSpanCell.Tag = null;
				dataGridViewSpanCell.Value = "";
				dataGridViewSpanCell.ToolTipText = "You can drag and move the rack";
				dataGridViewSpanCell.SetColumnSpan(0);
				dataGridViewSpanCell.BackColor = this.ColorLabel_;
				dataGridViewSpanCell3 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[this.rowIndexFromMouseDown].Cells[this.colIndexFromMouseDown + 1];
				dataGridViewSpanCell3.Tag = null;
				dataGridViewSpanCell3.Value = "";
				dataGridViewSpanCell3.ToolTipText = "You can drag and move the rack";
				dataGridViewSpanCell3.SetColumnSpan(0);
				dataGridViewSpanCell3.BackColor = this.ColorLabel_;
				dataGridViewSpanCell2.Tag = num3;
				dataGridViewSpanCell2.Value = value;
				dataGridViewSpanCell2.ToolTipText = toolTipText;
				dataGridViewSpanCell2.SetColumnSpan(1);
				dataGridViewSpanCell2.BackColor = backColor;
				dataGridViewSpanCell4 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex].Cells[columnIndex + 1];
				dataGridViewSpanCell4.Tag = num3;
				dataGridViewSpanCell4.Value = value;
				dataGridViewSpanCell2.ToolTipText = toolTipText;
				dataGridViewSpanCell4.SetColumnSpan(-1);
				dataGridViewSpanCell4.BackColor = backColor;
			}
			RackInfo rackInfo = this.m_MapID2Rack[num3];
			if (text.Equals("H"))
			{
				rackInfo.EndPoint_X = rowIndex;
				rackInfo.EndPoint_Y = columnIndex + 1;
			}
			else
			{
				rackInfo.EndPoint_X = rowIndex + 1;
				rackInfo.EndPoint_Y = columnIndex;
			}
			rackInfo.StartPoint_X = rowIndex;
			rackInfo.StartPoint_Y = columnIndex;
			rackInfo.UpdateRack();
			bool flag = commDev.updateZoneforRack(rackInfo);
			EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
			ulong num4 = 4uL;
			int num5 = 0;
			if (flag)
			{
				num4 |= 264uL;
				num5 |= 64;
			}
			num5 |= 1;
			EcoGlobalVar.setDashBoardFlg(num4, "", num5);
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell3);
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell4);
		}
		private bool checkMove(string direction, int targetRow, int targetColumn, long oldRackID)
		{
			if (targetRow > this.dgvSetDevice.RowCount - 1)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
				return false;
			}
			if (targetColumn > this.dgvSetDevice.ColumnCount - 1)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
				return false;
			}
			if (this.checkPointHaveRack(targetRow, targetColumn, oldRackID))
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail2, new string[0]));
				return false;
			}
			if (direction.Equals("H"))
			{
				if (targetColumn + 1 > this.dgvSetDevice.ColumnCount - 1)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
					return false;
				}
				if (this.checkPointHaveRack(targetRow, targetColumn + 1, oldRackID))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail2, new string[0]));
					return false;
				}
			}
			else
			{
				if (targetRow + 1 > this.dgvSetDevice.RowCount - 1)
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail1, new string[0]));
					return false;
				}
				if (this.checkPointHaveRack(targetRow + 1, targetColumn, oldRackID))
				{
					EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_Fail2, new string[0]));
					return false;
				}
			}
			return true;
		}
		private bool checkPointHaveRack(int row, int column, long oldRackID)
		{
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[row].Cells[column];
			return dataGridViewSpanCell != null && (dataGridViewSpanCell.GetColumnSpan() != 0 || dataGridViewSpanCell.GetRowSpan() != 0) && (oldRackID < 0L || (long)dataGridViewSpanCell.Tag != oldRackID);
		}
		private void lbRack_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.lbRack.SelectedItem == null)
			{
				return;
			}
			int curselindex = this.m_Curselindex;
			RackInfo rackInfo = null;
			if (curselindex >= this.m_AllRacks.Count)
			{
				rackInfo = null;
			}
			else
			{
				rackInfo = (RackInfo)this.m_AllRacks[curselindex];
			}
			this.m_Curselindex = this.lbRack.SelectedIndex;
			RackInfo rackInfo2 = (RackInfo)this.m_AllRacks[this.m_Curselindex];
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSetDevice.Rows)
			{
				foreach (DataGridViewSpanCell dataGridViewSpanCell in dataGridViewRow.Cells)
				{
					if (dataGridViewSpanCell.GetColumnSpan() != 0 || dataGridViewSpanCell.GetRowSpan() != 0)
					{
						if (dataGridViewSpanCell.Tag == null)
						{
							commUtil.ShowInfo_DEBUG("ERROR. cell.Tag == null {in lbRack_SelectedIndexChanged}");
						}
						else
						{
							if ((long)dataGridViewSpanCell.Tag == rackInfo2.RackID)
							{
								dataGridViewSpanCell.BackColor = this.ColorRack_selected;
								this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
								if (dataGridViewSpanCell.Value.ToString().Equals(""))
								{
									this.butDel.Enabled = true;
								}
								else
								{
									this.butDel.Enabled = false;
								}
								this.butModify.Enabled = true;
							}
							else
							{
								if (rackInfo != null && (long)dataGridViewSpanCell.Tag == rackInfo.RackID)
								{
									if (!dataGridViewSpanCell.Value.ToString().Equals(""))
									{
										dataGridViewSpanCell.BackColor = this.ColorRack_haveDev;
										this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
									}
									else
									{
										dataGridViewSpanCell.BackColor = this.ColorRack_noDev;
										this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
									}
								}
							}
						}
					}
				}
			}
		}
		private void butNew_Click(object sender, System.EventArgs e)
		{
			this.closetips();
			if (this.lbRack.Items.Count >= EcoGlobalVar.gl_maxRackNum)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Rack_MaxNum, new string[]
				{
					EcoGlobalVar.gl_maxRackNum.ToString()
				}));
				return;
			}
			RackInfoDlg rackInfoDlg = new RackInfoDlg(this, -1L);
			rackInfoDlg.ShowDialog(this);
		}
		public bool addRack(int column, int row, string direction, string rackNm, string rackFNm)
		{
			if (!this.checkMove(direction, row, column, -1L))
			{
				return false;
			}
			int i_ex;
			int i_ey;
			if (direction.Equals("H"))
			{
				i_ex = row;
				i_ey = column + 1;
			}
			else
			{
				i_ex = row + 1;
				i_ey = column;
			}
			long num = RackInfo.CreateRackInfo(rackNm, "", row, column, i_ex, i_ey, rackFNm);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0430010", new string[]
				{
					rackNm,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0430010", new string[]
				{
					rackNm
				});
			}
			this.initRackList();
			bool flag = commDev.updateZoneforRack(this.m_MapID2Rack[num]);
			this.addRackUI(column, row, direction, rackNm, "", num);
			RackInfo rackInfo = this.m_MapID2Rack[num];
			this.lbRack.SelectedItem = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			ulong num2 = 516uL;
			int appAction = 0;
			if (flag)
			{
				num2 |= 264uL;
			}
			EcoGlobalVar.setDashBoardFlg(num2, "", appAction);
			return true;
		}
		private void addRackUI(int column, int row, string direction, string rackNm, string devIDs, long rackID)
		{
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[row].Cells[column];
			dataGridViewSpanCell.BackColor = this.ColorRack_noDev;
			dataGridViewSpanCell.Value = devIDs;
			dataGridViewSpanCell.Tag = rackID;
			if (direction.Equals("H"))
			{
				dataGridViewSpanCell.SetColumnSpan(1);
				dataGridViewSpanCell.ToolTipText = rackNm;
				DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[row].Cells[column + 1];
				dataGridViewSpanCell2.Tag = rackID;
				dataGridViewSpanCell2.Value = devIDs;
				dataGridViewSpanCell2.BackColor = this.ColorRack_noDev;
				dataGridViewSpanCell2.SetColumnSpan(-1);
				dataGridViewSpanCell2.ToolTipText = rackNm;
				this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
				this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
				return;
			}
			dataGridViewSpanCell.SetRowSpan(1);
			dataGridViewSpanCell.ToolTipText = rackNm;
			DataGridViewSpanCell dataGridViewSpanCell3 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[row + 1].Cells[column];
			dataGridViewSpanCell3.Tag = rackID;
			dataGridViewSpanCell3.Value = devIDs;
			dataGridViewSpanCell3.BackColor = this.ColorRack_noDev;
			dataGridViewSpanCell3.SetRowSpan(-1);
			dataGridViewSpanCell3.ToolTipText = rackNm;
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
			this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell3);
		}
		public bool modifyRack(long rackId, int column, int row, string direction, string rackOrigNm, string rackFuNm)
		{
			RackInfo rackByID = RackInfo.getRackByID(rackId);
			if (rackByID == null)
			{
				return true;
			}
			if (!this.checkMove(direction, row, column, rackId))
			{
				return false;
			}
			string devIDs = this.delRackUi(rackId);
			int num;
			int num2;
			if (direction.Equals("H"))
			{
				num = row;
				num2 = column + 1;
			}
			else
			{
				num = row + 1;
				num2 = column;
			}
			bool flag = false;
			bool flag2 = false;
			if (!rackByID.OriginalName.Equals(rackOrigNm) || !rackByID.RackFullName.Equals(rackFuNm))
			{
				flag = true;
			}
			if (rackByID.StartPoint_X != row || rackByID.StartPoint_Y != column || rackByID.EndPoint_X != num || rackByID.EndPoint_Y != num2)
			{
				flag2 = true;
			}
			rackByID.OriginalName = rackOrigNm;
			rackByID.RackFullName = rackFuNm;
			rackByID.StartPoint_X = row;
			rackByID.StartPoint_Y = column;
			rackByID.EndPoint_X = num;
			rackByID.EndPoint_Y = num2;
			rackByID.UpdateRack();
			this.addRackUI(column, row, direction, rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag), devIDs, rackId);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0430012", new string[]
				{
					rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag),
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0430012", new string[]
				{
					rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag)
				});
			}
			bool flag3 = false;
			if (flag2)
			{
				flag3 = commDev.updateZoneforRack(rackByID);
			}
			EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
			ulong num3 = 4uL;
			int num4 = 0;
			if (flag3)
			{
				num3 |= 264uL;
				num4 |= 64;
			}
			if (flag2)
			{
				EcoGlobalVar.setDashBoardFlg(num3, "", num4 | 1);
			}
			else
			{
				if (flag)
				{
					EcoGlobalVar.setDashBoardFlg(num3, "", num4);
				}
			}
			this.initRackList();
			this.lbRack.SelectedItem = rackByID.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
			return true;
		}
		private void butDel_Click(object sender, System.EventArgs e)
		{
			if (this.lbRack.SelectedItem == null)
			{
				return;
			}
			this.closetips();
			int selectedIndex = this.lbRack.SelectedIndex;
			string text = this.lbRack.SelectedItem.ToString();
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Rack_delCrm, new string[]
			{
				text
			}), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			this.lbRack.Items.RemoveAt(selectedIndex);
			RackInfo rackInfo = (RackInfo)this.m_AllRacks[selectedIndex];
			this.m_AllRacks.RemoveAt(selectedIndex);
			this.m_MapID2Rack.Remove(rackInfo.RackID);
			this.delRackUi(rackInfo.RackID);
			RackInfo.DeleteByID(rackInfo.RackID);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0430011", new string[]
				{
					text,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0430011", new string[]
				{
					text
				});
			}
			EcoGlobalVar.setDashBoardFlg(780uL, "", 64);
			if (this.lbRack.Items.Count == 0)
			{
				this.butDel.Enabled = false;
				this.butModify.Enabled = false;
				return;
			}
			if (selectedIndex < this.lbRack.Items.Count)
			{
				this.lbRack.SelectedIndex = selectedIndex;
				return;
			}
			this.lbRack.SelectedIndex = this.lbRack.Items.Count - 1;
		}
		private string delRackUi(long rackID)
		{
			foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSetDevice.Rows)
			{
				foreach (DataGridViewSpanCell dataGridViewSpanCell in dataGridViewRow.Cells)
				{
					if ((dataGridViewSpanCell.GetColumnSpan() != 0 || dataGridViewSpanCell.GetRowSpan() != 0) && dataGridViewSpanCell.Tag != null && (long)dataGridViewSpanCell.Tag == rackID)
					{
						string result = dataGridViewSpanCell.Value.ToString();
						int columnIndex = dataGridViewSpanCell.ColumnIndex;
						int rowIndex = dataGridViewSpanCell.RowIndex;
						if (dataGridViewSpanCell.GetRowSpan() != 0)
						{
							dataGridViewSpanCell.SetRowSpan(0);
							dataGridViewSpanCell.Tag = null;
							dataGridViewSpanCell.Value = "";
							dataGridViewSpanCell.ToolTipText = "You can drag and move the rack";
							dataGridViewSpanCell.BackColor = this.ColorLabel_;
							DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex + 1].Cells[columnIndex];
							dataGridViewSpanCell2.SetRowSpan(0);
							dataGridViewSpanCell2.Tag = null;
							dataGridViewSpanCell2.Value = "";
							dataGridViewSpanCell2.ToolTipText = "You can drag and move the rack";
							dataGridViewSpanCell2.BackColor = this.ColorLabel_;
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
						}
						else
						{
							dataGridViewSpanCell.SetColumnSpan(0);
							dataGridViewSpanCell.Tag = null;
							dataGridViewSpanCell.Value = "";
							dataGridViewSpanCell.ToolTipText = "You can drag and move the rack";
							dataGridViewSpanCell.BackColor = this.ColorLabel_;
							DataGridViewSpanCell dataGridViewSpanCell3 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[rowIndex].Cells[columnIndex + 1];
							dataGridViewSpanCell3.SetColumnSpan(0);
							dataGridViewSpanCell3.Tag = null;
							dataGridViewSpanCell3.Value = "";
							dataGridViewSpanCell3.ToolTipText = "You can drag and move the rack";
							dataGridViewSpanCell3.BackColor = this.ColorLabel_;
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell3);
						}
						return result;
					}
				}
			}
			return "";
		}
		private void butModify_Click(object sender, System.EventArgs e)
		{
			if (this.lbRack.SelectedItem == null)
			{
				return;
			}
			int selectedIndex = this.lbRack.SelectedIndex;
			RackInfo rackInfo = (RackInfo)this.m_AllRacks[selectedIndex];
			if (rackInfo == null)
			{
				return;
			}
			this.closetips();
			RackInfoDlg rackInfoDlg = new RackInfoDlg(this, rackInfo.RackID);
			rackInfoDlg.ShowDialog(this);
		}
		public void closetips()
		{
			if (this.toolTip1 != null)
			{
				this.rdl = null;
				this.toolTip1.Close();
			}
		}
		private void ManRack_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!base.Visible && this.toolTip1 != null)
			{
				this.rdl = null;
				this.toolTip1.Close();
			}
		}
	}
}
