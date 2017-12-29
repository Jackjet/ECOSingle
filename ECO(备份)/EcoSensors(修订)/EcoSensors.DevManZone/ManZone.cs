using CommonAPI.Global;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.component;
using EcoSensors.DevManDCFloorGrid;
using EventLogAPI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.DevManZone
{
	public class ManZone : UserControl
	{
		private delegate void deviceDate();
		private IContainer components;
		private Button butModify;
		private Button butDel;
		private TreeView tvZone;
		private Button butAdd;
		private DevManFloorGrids devManFloorGrids1;
		private Color ColorRack_notinZone = Color.Silver;
		private Color ColorRack_inZone_Sel = Color.Blue;
		private Color ColorRack_inZone_noSel = Color.Lime;
		private Color Color_DragBlick_rect = Color.Yellow;
		private Color ColorLabel_ = Color.White;
		private bool ininit = true;
		private bool inSelection;
		private int zonedef_startR = -1;
		private int zonedef_startC = -1;
		private int zonedef_endR = -1;
		private int zonedef_endC = -1;
		private bool bickerFlg;
		private System.Timers.Timer dTimer;
		private DataGridView dgvSetDevice;
		private static int testSeqNO = 1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ManZone));
			this.butModify = new Button();
			this.butDel = new Button();
			this.tvZone = new TreeView();
			this.butAdd = new Button();
			this.devManFloorGrids1 = new DevManFloorGrids();
			base.SuspendLayout();
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
			this.tvZone.HideSelection = false;
			componentResourceManager.ApplyResources(this.tvZone, "tvZone");
			this.tvZone.Name = "tvZone";
			this.tvZone.BeforeSelect += new TreeViewCancelEventHandler(this.tvZone_BeforeSelect);
			this.tvZone.AfterSelect += new TreeViewEventHandler(this.tvZone_AfterSelect);
			this.butAdd.BackColor = Color.Gainsboro;
			componentResourceManager.ApplyResources(this.butAdd, "butAdd");
			this.butAdd.Name = "butAdd";
			this.butAdd.UseVisualStyleBackColor = false;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			this.devManFloorGrids1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devManFloorGrids1, "devManFloorGrids1");
			this.devManFloorGrids1.Name = "devManFloorGrids1";
			this.devManFloorGrids1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.devManFloorGrids1);
			base.Controls.Add(this.butModify);
			base.Controls.Add(this.butDel);
			base.Controls.Add(this.tvZone);
			base.Controls.Add(this.butAdd);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ManZone";
			base.ResumeLayout(false);
		}
		public ManZone()
		{
			this.InitializeComponent();
			this.dgvSetDevice = this.devManFloorGrids1.getRackCtrl();
			this.dTimer = new System.Timers.Timer();
			this.dTimer.Elapsed += new ElapsedEventHandler(this.timerEvent);
			this.dTimer.Interval = 500.0;
			this.dTimer.AutoReset = true;
			this.dTimer.Enabled = false;
		}
		public void init()
		{
			this.ininit = true;
			this.treeMenuInit();
			this.zonerackInit();
			if (this.tvZone.Nodes.Count > 0)
			{
				this.tvZone.SelectedNode = this.tvZone.Nodes[0];
				this.tvZone.Nodes[0].Checked = true;
			}
			else
			{
				this.butDel.Enabled = false;
				this.butModify.Enabled = false;
			}
			this.ininit = false;
		}
		public void startBicker()
		{
			if (this.tvZone.Nodes.Count == 0 || this.tvZone.SelectedNode == null)
			{
				return;
			}
			if (this.tvZone.SelectedNode.Tag == null)
			{
				return;
			}
			this.zoneBicker();
			string text = this.tvZone.SelectedNode.Tag.ToString();
			string[] array = text.Split(new char[]
			{
				'|'
			});
			string text2 = array[0];
			string text3 = array[1];
			string[] array2 = text2.Split(new char[]
			{
				','
			});
			int num = (int)System.Convert.ToInt16(array2[0]);
			int num2 = (int)System.Convert.ToInt16(array2[1]);
			string[] array3 = text3.Split(new char[]
			{
				','
			});
			int num3 = (int)System.Convert.ToInt16(array3[0]);
			int num4 = (int)System.Convert.ToInt16(array3[1]);
			int num5 = (num - num3) * (num2 - num4);
			if (num5 < 0)
			{
				num5 = -num5;
			}
			if (num5 < 1000)
			{
				num5 = 1000;
			}
			else
			{
				if (num5 > 3000)
				{
					num5 = 3000;
				}
			}
			this.dTimer.Interval = (double)num5;
			this.dTimer.Enabled = true;
		}
		public void endBicker()
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new ManZone.deviceDate(this.zoneBickerRevert));
			}
			else
			{
				this.zoneBickerRevert();
			}
			this.dTimer.Enabled = false;
		}
		private void timerEvent(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new ManZone.deviceDate(this.zoneBicker));
				return;
			}
			this.zoneBicker();
		}
		private void zoneBicker()
		{
			if (this.tvZone.SelectedNode == null)
			{
				return;
			}
			if (this.tvZone.SelectedNode.Tag == null)
			{
				return;
			}
			this.dTimer.Interval = 30000.0;
			string text = this.tvZone.SelectedNode.Tag.ToString();
			string[] array = text.Split(new char[]
			{
				'|'
			});
			string text2 = array[0];
			string text3 = array[1];
			string value = array[2];
			Color zoneColor = Color.FromArgb(System.Convert.ToInt32(value));
			string[] array2 = text2.Split(new char[]
			{
				','
			});
			int num = (int)System.Convert.ToInt16(array2[0]);
			int num2 = (int)System.Convert.ToInt16(array2[1]);
			string[] array3 = text3.Split(new char[]
			{
				','
			});
			int num3 = (int)System.Convert.ToInt16(array3[0]);
			int num4 = (int)System.Convert.ToInt16(array3[1]);
			if (!this.bickerFlg)
			{
				zoneColor = this.Color_DragBlick_rect;
				this.setZoneRect(num, num2, num3, num4, zoneColor, this.ColorRack_inZone_Sel, ManZone.testSeqNO++);
				this.bickerFlg = true;
			}
			else
			{
				this.setZoneRect(num, num2, num3, num4, zoneColor, this.ColorRack_inZone_Sel, ManZone.testSeqNO++);
				this.bickerFlg = false;
			}
			if (ManZone.testSeqNO <= 0)
			{
				ManZone.testSeqNO = 1;
			}
			int num5 = (num - num3) * (num2 - num4) / 2;
			if (num5 < 0)
			{
				num5 = -num5;
			}
			if (num5 < 500)
			{
				num5 = 500;
			}
			if (!this.bickerFlg)
			{
				num5 += 500;
			}
			this.dTimer.Interval = (double)num5;
		}
		private void setZoneRect(int startR, int startC, int endR, int endC, Color zoneColor, Color rackColor, int SeqNO)
		{
			int num;
			int num2;
			if (startR > endR)
			{
				num = endR;
				num2 = startR;
			}
			else
			{
				num = startR;
				num2 = endR;
			}
			int num3;
			int num4;
			if (startC > endC)
			{
				num3 = endC;
				num4 = startC;
			}
			else
			{
				num3 = startC;
				num4 = endC;
			}
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j];
					if (dataGridViewSpanCell.GetRowSpan() != 0 || dataGridViewSpanCell.GetColumnSpan() != 0)
					{
						dataGridViewSpanCell.BackColor = rackColor;
						if (dataGridViewSpanCell.GetColumnSpan() < 0 && j == num3)
						{
							DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j - 1];
							Color backColor = dataGridViewSpanCell2.BackColor;
							if (backColor != rackColor)
							{
								dataGridViewSpanCell2.BackColor = rackColor;
								this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
							}
						}
						else
						{
							if (dataGridViewSpanCell.GetColumnSpan() > 0 && j == num4)
							{
								DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j + 1];
								Color backColor = dataGridViewSpanCell2.BackColor;
								if (backColor != rackColor)
								{
									dataGridViewSpanCell2.BackColor = rackColor;
									this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
								}
							}
							else
							{
								if (dataGridViewSpanCell.GetRowSpan() < 0 && i == num)
								{
									DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i - 1].Cells[j];
									Color backColor = dataGridViewSpanCell2.BackColor;
									if (backColor != rackColor)
									{
										dataGridViewSpanCell2.BackColor = rackColor;
										this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
									}
								}
								else
								{
									if (dataGridViewSpanCell.GetRowSpan() > 0 && i == num2)
									{
										DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i + 1].Cells[j];
										Color backColor = dataGridViewSpanCell2.BackColor;
										if (backColor != rackColor)
										{
											dataGridViewSpanCell2.BackColor = rackColor;
											this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell2);
										}
									}
								}
							}
						}
					}
					else
					{
						dataGridViewSpanCell.BackColor = zoneColor;
					}
					this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
				}
			}
		}
		private void treeMenuInit()
		{
			this.tvZone.Nodes.Clear();
			System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			for (int i = 0; i < allZone.Count; i++)
			{
				ZoneInfo zoneInfo = (ZoneInfo)allZone[i];
				string zoneName = zoneInfo.ZoneName;
				string rackInfo = zoneInfo.RackInfo;
				string text = zoneInfo.StartPointX.ToString() + "," + zoneInfo.StartPointY.ToString();
				string text2 = zoneInfo.EndPointX.ToString() + "," + zoneInfo.EndPointY.ToString();
				string zoneColor = zoneInfo.ZoneColor;
				TreeNode treeNode = new TreeNode();
				treeNode.Text = zoneName;
				treeNode.Name = rackInfo;
				treeNode.Tag = string.Concat(new string[]
				{
					text,
					"|",
					text2,
					"|",
					zoneColor,
					"|",
					zoneInfo.ZoneID.ToString()
				});
				string[] source = rackInfo.Split(new char[]
				{
					','
				});
				foreach (RackInfo rackInfo2 in allRack_NoEmpty)
				{
					if (source.Contains(rackInfo2.RackID.ToString()) && rackInfo2 != null && !rackInfo2.DeviceInfo.Equals(string.Empty))
					{
						string displayRackName = rackInfo2.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
						TreeNode treeNode2 = new TreeNode();
						treeNode2.Text = displayRackName;
						treeNode2.Name = "rack";
						treeNode.Nodes.Add(treeNode2);
					}
				}
				this.tvZone.Nodes.Add(treeNode);
			}
		}
		private void treeMenuSelect(string zoneNm)
		{
			foreach (TreeNode treeNode in this.tvZone.Nodes)
			{
				if (treeNode.Text.Equals(zoneNm))
				{
					this.tvZone.SelectedNode = treeNode;
					break;
				}
			}
		}
		private void zonerackInit()
		{
			this.dgvSetDevice = this.devManFloorGrids1.getRackCtrl();
			this.dgvSetDevice.MultiSelect = false;
			this.dgvSetDevice.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			this.dgvSetDevice.DefaultCellStyle.SelectionBackColor = this.Color_DragBlick_rect;
			this.dgvSetDevice.DefaultCellStyle.SelectionForeColor = Color.White;
			this.dgvSetDevice.ShowCellToolTips = true;
			this.dgvSetDevice.CellMouseClick -= new DataGridViewCellMouseEventHandler(this.dgvSetDevice_CellMouseClick);
			this.dgvSetDevice.CellMouseClick += new DataGridViewCellMouseEventHandler(this.dgvSetDevice_CellMouseClick);
			this.dgvSetDevice.SelectionChanged -= new System.EventHandler(this.dgvSetDevice_SelectionChanged);
			this.dgvSetDevice.SelectionChanged += new System.EventHandler(this.dgvSetDevice_SelectionChanged);
			this.dgvSetDevice.MouseDown -= new MouseEventHandler(this.dgvSetDevice_MouseDown);
			this.dgvSetDevice.MouseDown += new MouseEventHandler(this.dgvSetDevice_MouseDown);
			this.inSelection = false;
			this.zonedef_startR = -1;
			this.zonedef_startC = -1;
			this.zonedef_endR = -1;
			this.zonedef_endC = -1;
			this.dgvSetDevice.MouseMove -= new MouseEventHandler(this.dgvSetDevice_MouseMove);
			this.dgvSetDevice.MouseMove += new MouseEventHandler(this.dgvSetDevice_MouseMove);
			this.dgvSetDevice.MouseUp -= new MouseEventHandler(this.dgvSetDevice_MouseUP);
			this.dgvSetDevice.MouseUp += new MouseEventHandler(this.dgvSetDevice_MouseUP);
			System.GC.Collect();
			System.Collections.ArrayList allRack_NoEmpty = RackInfo.GetAllRack_NoEmpty();
			for (int i = 0; i < allRack_NoEmpty.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRack_NoEmpty[i];
				string displayRackName = rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag);
				string tag = rackInfo.RackID.ToString();
				int startPoint_X = rackInfo.StartPoint_X;
				int startPoint_Y = rackInfo.StartPoint_Y;
				int endPoint_X = rackInfo.EndPoint_X;
				int arg_1C3_0 = rackInfo.EndPoint_Y;
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
				dataGridViewSpanCell.Tag = tag;
				dataGridViewSpanCell.Value = displayRackName;
				dataGridViewSpanCell.BackColor = this.ColorRack_notinZone;
				dataGridViewSpanCell.SelectionBackColor = this.ColorRack_inZone_Sel;
				if (text.Equals("H"))
				{
					dataGridViewSpanCell.SetColumnSpan(1);
					dataGridViewSpanCell.ToolTipText = displayRackName;
					DataGridViewSpanCell dataGridViewSpanCell2 = (DataGridViewSpanCell)this.dgvSetDevice.Rows[startPoint_X].Cells[startPoint_Y + 1];
					dataGridViewSpanCell2.Tag = tag;
					dataGridViewSpanCell2.Value = displayRackName;
					dataGridViewSpanCell2.BackColor = this.ColorRack_notinZone;
					dataGridViewSpanCell2.SelectionBackColor = this.ColorRack_inZone_Sel;
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
						dataGridViewSpanCell3.Tag = tag;
						dataGridViewSpanCell3.Value = displayRackName;
						dataGridViewSpanCell3.BackColor = this.ColorRack_notinZone;
						dataGridViewSpanCell3.SelectionBackColor = this.ColorRack_inZone_Sel;
						dataGridViewSpanCell3.SetRowSpan(-1);
						dataGridViewSpanCell3.ToolTipText = displayRackName;
					}
				}
			}
			this.gridinit(this.dgvSetDevice);
			this.ZoneDisplayAll();
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
						dataGridViewSpanCell.SelectionBackColor = this.Color_DragBlick_rect;
						dataGridViewSpanCell.ToolTipText = "Drag and drop to define a zone";
					}
				}
			}
		}
		private void ZoneDisplayAll()
		{
			System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
			for (int i = 0; i < allZone.Count; i++)
			{
				ZoneInfo zoneInfo = (ZoneInfo)allZone[i];
				string arg_1D_0 = zoneInfo.ZoneName;
				string arg_24_0 = zoneInfo.RackInfo;
				string zoneColor = zoneInfo.ZoneColor;
				Color zoneColor2 = Color.FromArgb(System.Convert.ToInt32(zoneColor));
				int startPointX = zoneInfo.StartPointX;
				int startPointY = zoneInfo.StartPointY;
				int endPointX = zoneInfo.EndPointX;
				int endPointY = zoneInfo.EndPointY;
				this.setZoneRect(startPointX, startPointY, endPointX, endPointY, zoneColor2, this.ColorRack_inZone_noSel, 0);
			}
		}
		private void tvZone_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			this.endBicker();
			this.zonedef_cancel();
			if (e.Node != null)
			{
				object arg_1F_0 = e.Node.Tag;
			}
		}
		private void tvZone_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string name = e.Node.Name;
			string text = e.Node.Text;
			if (name.Equals("rack"))
			{
				foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSetDevice.Rows)
				{
					foreach (DataGridViewSpanCell dataGridViewSpanCell in dataGridViewRow.Cells)
					{
						if ((dataGridViewSpanCell.GetRowSpan() != 0 || dataGridViewSpanCell.GetColumnSpan() != 0) && dataGridViewSpanCell.Value.ToString().Equals(text))
						{
							dataGridViewSpanCell.BackColor = this.ColorRack_inZone_Sel;
							DataGridViewSpanCell cellsameRack = this.getCellsameRack(dataGridViewSpanCell);
							cellsameRack.BackColor = this.ColorRack_inZone_Sel;
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
							this.dgvSetDevice.InvalidateCell(cellsameRack);
							break;
						}
					}
				}
				this.butAdd.Enabled = false;
				this.butDel.Enabled = false;
				this.butModify.Enabled = false;
				return;
			}
			this.butDel.Enabled = true;
			this.butModify.Enabled = true;
			if (!this.ininit)
			{
				this.startBicker();
			}
		}
		private DataGridViewSpanCell getCellsameRack(DataGridViewSpanCell srccell)
		{
			DataGridViewSpanCell result = null;
			if (srccell.GetColumnSpan() < 0)
			{
				result = (DataGridViewSpanCell)this.dgvSetDevice.Rows[srccell.RowIndex].Cells[srccell.ColumnIndex - 1];
			}
			else
			{
				if (srccell.GetColumnSpan() > 0)
				{
					result = (DataGridViewSpanCell)this.dgvSetDevice.Rows[srccell.RowIndex].Cells[srccell.ColumnIndex + 1];
				}
				else
				{
					if (srccell.GetRowSpan() < 0)
					{
						result = (DataGridViewSpanCell)this.dgvSetDevice.Rows[srccell.RowIndex - 1].Cells[srccell.ColumnIndex];
					}
					else
					{
						if (srccell.GetRowSpan() > 0)
						{
							result = (DataGridViewSpanCell)this.dgvSetDevice.Rows[srccell.RowIndex + 1].Cells[srccell.ColumnIndex];
						}
					}
				}
			}
			return result;
		}
		private void zoneBickerRevert()
		{
			if (this.tvZone.SelectedNode == null)
			{
				return;
			}
			string name = this.tvZone.SelectedNode.Name;
			if (name.Equals("rack"))
			{
				string text = this.tvZone.SelectedNode.Text;
				foreach (DataGridViewRow dataGridViewRow in (System.Collections.IEnumerable)this.dgvSetDevice.Rows)
				{
					foreach (DataGridViewSpanCell dataGridViewSpanCell in dataGridViewRow.Cells)
					{
						if ((dataGridViewSpanCell.GetRowSpan() != 0 || dataGridViewSpanCell.GetColumnSpan() != 0) && dataGridViewSpanCell.Value.ToString().Equals(text))
						{
							dataGridViewSpanCell.BackColor = this.ColorRack_inZone_noSel;
							DataGridViewSpanCell cellsameRack = this.getCellsameRack(dataGridViewSpanCell);
							cellsameRack.BackColor = this.ColorRack_inZone_noSel;
							this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
							this.dgvSetDevice.InvalidateCell(cellsameRack);
							break;
						}
					}
				}
				return;
			}
			string text2 = this.tvZone.SelectedNode.Tag.ToString();
			string[] array = text2.Split(new char[]
			{
				'|'
			});
			string text3 = array[0];
			string text4 = array[1];
			string value = array[2];
			Color zoneColor = Color.FromArgb(System.Convert.ToInt32(value));
			string[] array2 = text3.Split(new char[]
			{
				','
			});
			int startR = (int)System.Convert.ToInt16(array2[0]);
			int startC = (int)System.Convert.ToInt16(array2[1]);
			string[] array3 = text4.Split(new char[]
			{
				','
			});
			int endR = (int)System.Convert.ToInt16(array3[0]);
			int endC = (int)System.Convert.ToInt16(array3[1]);
			this.setZoneRect(startR, startC, endR, endC, zoneColor, this.ColorRack_inZone_noSel, 0);
			this.bickerFlg = false;
		}
		private void dgvSetDevice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[e.RowIndex].Cells[e.ColumnIndex];
			string value = dataGridViewSpanCell.Value.ToString();
			TreeNode[] array = this.tvZone.Nodes.Find("rack", true);
			TreeNode[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TreeNode treeNode = array2[i];
				if (treeNode.Text.Equals(value))
				{
					this.tvZone.SelectedNode = treeNode;
				}
			}
		}
		private void dgvSetDevice_SelectionChanged(object sender, System.EventArgs e)
		{
			for (int i = 0; i < this.dgvSetDevice.SelectedCells.Count; i++)
			{
				DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.SelectedCells[i];
				dataGridViewSpanCell.Selected = false;
			}
		}
		private void dgvSetDevice_MouseDown(object sender, MouseEventArgs e)
		{
			this.endBicker();
			this.butAdd.Enabled = false;
			this.zonedef_cancel();
			DataGridView.HitTestInfo hitTestInfo = this.dgvSetDevice.HitTest(e.X, e.Y);
			this.zonedef_startR = hitTestInfo.RowIndex;
			if (this.zonedef_startR < 0)
			{
				return;
			}
			this.zonedef_startC = hitTestInfo.ColumnIndex;
			if (this.zonedef_startC < 0)
			{
				return;
			}
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			this.inSelection = true;
			this.zonedef_endR = -1;
			this.zonedef_endC = -1;
		}
		private void dgvSetDevice_MouseMove(object sender, MouseEventArgs e)
		{
			if (!this.inSelection)
			{
				return;
			}
			if (this.zonedef_startR < 0 || this.zonedef_startC < 0)
			{
				return;
			}
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			DataGridView.HitTestInfo hitTestInfo = this.dgvSetDevice.HitTest(e.X, e.Y);
			int columnIndex = hitTestInfo.ColumnIndex;
			int rowIndex = hitTestInfo.RowIndex;
			if (columnIndex < 0 || rowIndex < 0)
			{
				return;
			}
			this.zonedef_select(false);
			if (this.zonedef_startR != rowIndex || this.zonedef_startC != columnIndex)
			{
				this.zonedef_endR = rowIndex;
				this.zonedef_endC = columnIndex;
				this.zonedef_select(true);
				this.butAdd.Enabled = true;
				return;
			}
			this.butAdd.Enabled = false;
		}
		private void dgvSetDevice_MouseUP(object sender, MouseEventArgs e)
		{
			if (this.zonedef_startR < 0 || this.zonedef_startC < 0)
			{
				return;
			}
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			this.inSelection = false;
		}
		private void zonedef_select(bool ifselect)
		{
			if (this.zonedef_startR >= 0 && this.zonedef_startC >= 0 && this.zonedef_endR >= 0 && this.zonedef_endC >= 0)
			{
				int num = System.Math.Min(this.zonedef_startR, this.zonedef_endR);
				int num2 = System.Math.Max(this.zonedef_startR, this.zonedef_endR);
				int num3 = System.Math.Min(this.zonedef_startC, this.zonedef_endC);
				int num4 = System.Math.Max(this.zonedef_startC, this.zonedef_endC);
				for (int i = num; i <= num2; i++)
				{
					for (int j = num3; j <= num4; j++)
					{
						DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j];
						dataGridViewSpanCell.ifSelected = ifselect;
						this.dgvSetDevice.InvalidateCell(dataGridViewSpanCell);
						DataGridViewSpanCell cellsameRack = this.getCellsameRack(dataGridViewSpanCell);
						if (cellsameRack != null)
						{
							cellsameRack.ifSelected = ifselect;
							this.dgvSetDevice.InvalidateCell(cellsameRack);
						}
					}
				}
			}
		}
		private void zonedef_cancel()
		{
			this.zonedef_select(false);
			this.inSelection = false;
			this.dgvSetDevice.ClearSelection();
			this.zonedef_startR = (this.zonedef_startC = -1);
			this.zonedef_endR = (this.zonedef_endC = -1);
		}
		private void butAdd_Click(object sender, System.EventArgs e)
		{
			System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
			if (allZone.Count >= EcoGlobalVar.gl_maxZoneNum)
			{
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.Zone_MaxNum, new string[]
				{
					EcoGlobalVar.gl_maxZoneNum.ToString()
				}));
				return;
			}
			ZoneInfoDlg zoneInfoDlg = new ZoneInfoDlg(this, -1L);
			zoneInfoDlg.ShowDialog(this);
		}
		private void butModify_Click(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = this.tvZone.SelectedNode;
			if (selectedNode == null)
			{
				return;
			}
			string text = selectedNode.Tag.ToString();
			string[] array = text.Split(new char[]
			{
				'|'
			});
			string value = array[3];
			ZoneInfoDlg zoneInfoDlg = new ZoneInfoDlg(this, System.Convert.ToInt64(value));
			zoneInfoDlg.ShowDialog(this);
		}
		private void butDel_Click(object sender, System.EventArgs e)
		{
			TreeNode selectedNode = this.tvZone.SelectedNode;
			if (selectedNode == null)
			{
				return;
			}
			string text = selectedNode.Text;
			DialogResult dialogResult = EcoMessageBox.ShowWarning(EcoLanguage.getMsg(LangRes.Zone_delCrm, new string[]
			{
				text
			}), MessageBoxButtons.OKCancel);
			if (dialogResult == DialogResult.Cancel)
			{
				return;
			}
			ZoneInfo.DeleteByName(text);
			string valuePair = ValuePairs.getValuePair("Username");
			if (!string.IsNullOrEmpty(valuePair))
			{
				LogAPI.writeEventLog("0430021", new string[]
				{
					text,
					valuePair
				});
			}
			else
			{
				LogAPI.writeEventLog("0430021", new string[]
				{
					text
				});
			}
			EcoGlobalVar.setDashBoardFlg(776uL, "", 64);
			this.treeMenuInit();
			this.zonerackInit();
			this.butAdd.Enabled = false;
			EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
		}
		public int addZone(string zoneNm, Color color)
		{
			string text = "";
			int num = System.Math.Min(this.zonedef_startR, this.zonedef_endR);
			int num2 = System.Math.Max(this.zonedef_startR, this.zonedef_endR);
			int num3 = System.Math.Min(this.zonedef_startC, this.zonedef_endC);
			int num4 = System.Math.Max(this.zonedef_startC, this.zonedef_endC);
			for (int i = num; i <= num2; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					DataGridViewSpanCell dataGridViewSpanCell = (DataGridViewSpanCell)this.dgvSetDevice.Rows[i].Cells[j];
					if (dataGridViewSpanCell.GetRowSpan() != 0 || dataGridViewSpanCell.GetColumnSpan() != 0)
					{
						string str = dataGridViewSpanCell.Tag.ToString();
						text = text + str + ",";
					}
				}
			}
			text = commUtil.uniqueIDs(text);
			if (text.Length > 0)
			{
				text = text.Substring(0, text.Length - 1);
			}
			this.zonedef_cancel();
			int result = ZoneInfo.CreateZoneInfo(zoneNm, text, num, num3, num2, num4, color.ToArgb().ToString());
			switch (result)
			{
			case -2:
			case -1:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				break;
			case 1:
			{
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0430020", new string[]
					{
						zoneNm,
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0430020", new string[]
					{
						zoneNm
					});
				}
				EcoGlobalVar.setDashBoardFlg(256uL, "", 0);
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				this.treeMenuInit();
				this.treeMenuSelect(zoneNm);
				this.butAdd.Enabled = false;
				EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
				break;
			}
			}
			return result;
		}
		public int modifyZone(long zoneID, string zoneNm, Color color)
		{
			ZoneInfo zoneByID = ZoneInfo.getZoneByID(zoneID);
			zoneByID.ZoneName = zoneNm;
			zoneByID.ZoneColor = color.ToArgb().ToString();
			int result = zoneByID.UpdateZone();
			switch (result)
			{
			case -2:
			case -1:
				EcoMessageBox.ShowError(EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
				break;
			case 1:
			{
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0430022", new string[]
					{
						zoneNm,
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0430022", new string[]
					{
						zoneNm
					});
				}
				EcoGlobalVar.setDashBoardFlg(256uL, "", 0);
				EcoMessageBox.ShowInfo(EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
				this.treeMenuInit();
				this.treeMenuSelect(zoneNm);
				this.butAdd.Enabled = false;
				EcoGlobalVar.gl_DevManPage.FlushFlg_ZoneBoard = 1;
				break;
			}
			}
			return result;
		}
	}
}
