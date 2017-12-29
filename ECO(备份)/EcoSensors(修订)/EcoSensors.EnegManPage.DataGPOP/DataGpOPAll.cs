using DBAccessAPI;
using DBAccessAPI.user;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.DataGPOP
{
	public class DataGpOPAll : UserControl
	{
		private delegate void UIRefresh();
		public const int Interval_AfterChange = 10000;
		public const int Interval_NoChange = 1000;
		private const int TreeST_Normal = 0;
		private const int TreeST_NoGroup = 1;
		public const int MSG_DEVTREE_GENSEL = 63000;
		public const int MSG_GROUPTREE_EMPTY = 63001;
		private const int IMG_folderclosed = 0;
		private const int IMG_folderopen = 1;
		private const int IMG_deviceoff = 2;
		private const int IMG_deviceon = 3;
		public const int TreeRefreshST_nonewDT = -1;
		public const int TreeRefreshST_havedismatch = 1;
		public const int TreeRefreshST_newDT = 2;
		private IContainer components;
		private SplitContainer splitContainer1;
		private TreeView tvDataGp;
		private ImageList imageList1;
		private DataGpOPGroup dataGpOPGroup1;
		private DataGpOPDev dataGpOPDev1;
		private System.Timers.Timer dTimer;
		private TreeNode m_gptypeZoneNode;
		private TreeNode m_gptypeRackNode;
		private TreeNode m_gptypeDevNode;
		private TreeNode m_gptypeOutletNode;
		private string m_lastSelectedTag;
		private int refreshFlg = 64;
		public int GroupTreeOpFlg
		{
			get
			{
				return this.refreshFlg;
			}
			set
			{
				this.refreshFlg = value;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataGpOPAll));
			this.splitContainer1 = new SplitContainer();
			this.tvDataGp = new TreeView();
			this.imageList1 = new ImageList(this.components);
			this.dataGpOPGroup1 = new DataGpOPGroup();
			this.dataGpOPDev1 = new DataGpOPDev();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvDataGp);
			this.splitContainer1.Panel2.Controls.Add(this.dataGpOPDev1);
			this.splitContainer1.Panel2.Controls.Add(this.dataGpOPGroup1);
			this.tvDataGp.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.tvDataGp, "tvDataGp");
			this.tvDataGp.HideSelection = false;
			this.tvDataGp.ImageList = this.imageList1;
			this.tvDataGp.Name = "tvDataGp";
			this.tvDataGp.AfterCollapse += new TreeViewEventHandler(this.tvDataGp_AfterCollapse);
			this.tvDataGp.AfterExpand += new TreeViewEventHandler(this.tvDataGp_AfterExpand);
			this.tvDataGp.AfterSelect += new TreeViewEventHandler(this.tvDataGp_AfterSelect);
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "folderclosed.gif");
			this.imageList1.Images.SetKeyName(1, "folderopen.gif");
			this.imageList1.Images.SetKeyName(2, "deviceoff.gif");
			this.imageList1.Images.SetKeyName(3, "deviceon.gif");
			this.dataGpOPGroup1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dataGpOPGroup1, "dataGpOPGroup1");
			this.dataGpOPGroup1.Name = "dataGpOPGroup1";
			this.dataGpOPDev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.dataGpOPDev1, "dataGpOPDev1");
			this.dataGpOPDev1.Name = "dataGpOPDev1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DataGpOPAll";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public DataGpOPAll()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_DataGpOPAll = this;
			this.m_gptypeZoneNode = new TreeNode();
			this.m_gptypeZoneNode.Text = EcoLanguage.getMsg(LangRes.Group_TPZone, new string[0]);
			this.m_gptypeZoneNode.Name = "";
			this.m_gptypeZoneNode.Tag = "TPZone";
			this.m_gptypeZoneNode.ImageIndex = 0;
			this.m_gptypeZoneNode.SelectedImageIndex = 0;
			this.m_gptypeRackNode = new TreeNode();
			this.m_gptypeRackNode.Text = EcoLanguage.getMsg(LangRes.Group_TPRack, new string[0]);
			this.m_gptypeRackNode.Name = "";
			this.m_gptypeRackNode.Tag = "TPRack";
			this.m_gptypeRackNode.ImageIndex = 0;
			this.m_gptypeRackNode.SelectedImageIndex = 0;
			this.m_gptypeDevNode = new TreeNode();
			this.m_gptypeDevNode.Text = EcoLanguage.getMsg(LangRes.Group_TPDev, new string[0]);
			this.m_gptypeDevNode.Name = "";
			this.m_gptypeDevNode.Tag = "TPDev";
			this.m_gptypeDevNode.ImageIndex = 0;
			this.m_gptypeDevNode.SelectedImageIndex = 0;
			this.m_gptypeOutletNode = new TreeNode();
			this.m_gptypeOutletNode.Text = EcoLanguage.getMsg(LangRes.Group_TPOutlet, new string[0]);
			this.m_gptypeOutletNode.Name = "";
			this.m_gptypeOutletNode.Tag = "TPOutlet";
			this.m_gptypeOutletNode.ImageIndex = 0;
			this.m_gptypeOutletNode.SelectedImageIndex = 0;
			this.InitTimer();
			ClientAPI.PendingChanged += new System.EventHandler<ClientAPI.PendingStatusArgs>(this.UI_OnPendingChanged);
			ClientAPI.RealtimeChanged += new System.EventHandler<ClientAPI.RealtimeDataArgs>(this.UI_OnRealtimeChanged);
		}
		private void UI_OnPendingChanged(object sender, ClientAPI.PendingStatusArgs e)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new Action(()=>
				{
					this.UI_OnPendingChanged(sender, e);
				}));
				return;
			}
			this.theTimeout(null, null);
		}
		private void UI_OnRealtimeChanged(object sender, ClientAPI.RealtimeDataArgs e)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new Action(()=>
				{
					this.UI_OnRealtimeChanged(sender, e);
				}));
				return;
			}
			this.theTimeout(null, null);
		}
		public void pageInit()
		{
			this.m_lastSelectedTag = null;
			this.GroupTreeOpFlg = 64;
			this.treeDataInit();
			this.dTimer.Interval = 100.0;
		}
		private void InitTimer()
		{
			if (this.dTimer == null)
			{
				this.dTimer = new System.Timers.Timer();
				this.dTimer.Elapsed += new ElapsedEventHandler(this.theTimeout);
				this.dTimer.Interval = 100.0;
				this.dTimer.AutoReset = true;
				this.dTimer.Enabled = false;
			}
		}
		public void starTimer()
		{
			if (this.dTimer != null)
			{
				this.dTimer.Enabled = true;
			}
		}
		public void endTimer()
		{
			if (this.dTimer != null)
			{
				this.dTimer.Enabled = false;
			}
		}
		private void setTimer(int Interval)
		{
			this.dTimer.Interval = (double)Interval;
		}
		private void theTimeout(object source, ElapsedEventArgs e)
		{
			if (!this.dTimer.Enabled)
			{
				return;
			}
			if (base.InvokeRequired)
			{
				base.Invoke(new DataGpOPAll.UIRefresh(this.deviceTimerRefresh));
				return;
			}
			this.deviceTimerRefresh();
		}
		private void deviceTimerRefresh()
		{
			this.dTimer.Interval = 30000.0;
			int num = this.RefreshTree();
			if (num == 1)
			{
				if (this.treeDataInit() == 1)
				{
					this.endTimer();
					return;
				}
			}
			else
			{
				if (num == -1)
				{
					this.setTimer(1000);
					return;
				}
			}
			try
			{
				TreeNode selectedNode = this.tvDataGp.SelectedNode;
				string text = selectedNode.Tag.ToString();
				if (selectedNode.Level == 0)
				{
					this.dataGpOPGroup1.pageInit("", 1, this);
					return;
				}
				if (selectedNode.Level == 1)
				{
					this.dataGpOPGroup1.pageInit(text, 1, this);
				}
				else
				{
					if (selectedNode.Level == 2)
					{
						this.dataGpOPDev1.pageInit(text, selectedNode.Text, this);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine("DataGpOPAll deviceTimerRefresh error!" + ex.Message);
			}
			this.setTimer(10000);
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63000)
			{
				this.treeDataInit();
				return;
			}
			base.DefWndProc(ref m);
		}
		private int treeDataInit()
		{
			this.GroupTreeOpFlg = 0;
			TreeNode treeNode = null;
			long num = 0L;
			long lastSelectedGpID = 0L;
			try
			{
				this.tvDataGp.Nodes.Clear();
				this.m_gptypeZoneNode.Nodes.Clear();
				this.m_gptypeRackNode.Nodes.Clear();
				this.m_gptypeDevNode.Nodes.Clear();
				this.m_gptypeOutletNode.Nodes.Clear();
				this.m_gptypeZoneNode.ImageIndex = 0;
				this.m_gptypeZoneNode.SelectedImageIndex = 0;
				this.m_gptypeRackNode.ImageIndex = 0;
				this.m_gptypeRackNode.SelectedImageIndex = 0;
				this.m_gptypeDevNode.ImageIndex = 0;
				this.m_gptypeDevNode.SelectedImageIndex = 0;
				this.m_gptypeOutletNode.ImageIndex = 0;
				this.m_gptypeOutletNode.SelectedImageIndex = 0;
				if (this.m_lastSelectedTag == "TPZone")
				{
					treeNode = this.m_gptypeZoneNode;
				}
				else
				{
					if (this.m_lastSelectedTag == "TPRack")
					{
						treeNode = this.m_gptypeRackNode;
					}
					else
					{
						if (this.m_lastSelectedTag == "TPDev")
						{
							treeNode = this.m_gptypeDevNode;
						}
						else
						{
							if (this.m_lastSelectedTag == "TPOutlet")
							{
								treeNode = this.m_gptypeOutletNode;
							}
							else
							{
								if (this.m_lastSelectedTag != null)
								{
									if (this.m_lastSelectedTag.IndexOf('@') < 0)
									{
										num = System.Convert.ToInt64(this.m_lastSelectedTag);
									}
									else
									{
										lastSelectedGpID = System.Convert.ToInt64(this.m_lastSelectedTag.Substring(0, this.m_lastSelectedTag.IndexOf('@')));
									}
								}
							}
						}
					}
				}
				UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
				if (gl_LoginUser.UserType != 0)
				{
					string userGroup = gl_LoginUser.UserGroup;
					System.Collections.Generic.List<GroupInfo> list = ClientAPI.getGroupCopy();
					if (list == null)
					{
						list = new System.Collections.Generic.List<GroupInfo>();
					}
					string[] array = userGroup.Split(new string[]
					{
						","
					}, System.StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string value = array2[i];
						long num2 = System.Convert.ToInt64(value);
						GroupInfo groupInfo = null;
						foreach (GroupInfo current in list)
						{
							if (current.ID == num2)
							{
								groupInfo = current;
								break;
							}
						}
						if (groupInfo != null)
						{
							TreeNode treeNode2 = new TreeNode();
							treeNode2.Text = groupInfo.GroupName;
							treeNode2.Tag = groupInfo.ID;
							treeNode2.ImageIndex = 0;
							treeNode2.SelectedImageIndex = 0;
							if (num == groupInfo.ID)
							{
								treeNode = treeNode2;
							}
							string groupType;
							switch (groupType = groupInfo.GroupType)
							{
							case "zone":
								this.m_gptypeZoneNode.Nodes.Add(treeNode2);
								this.appendDeviceNode(treeNode2, groupInfo, lastSelectedGpID, ref treeNode);
								break;
							case "rack":
							case "allrack":
								this.m_gptypeRackNode.Nodes.Add(treeNode2);
								this.appendDeviceNode(treeNode2, groupInfo, lastSelectedGpID, ref treeNode);
								break;
							case "dev":
							case "alldev":
								this.m_gptypeDevNode.Nodes.Add(treeNode2);
								this.appendDeviceNode(treeNode2, groupInfo, lastSelectedGpID, ref treeNode);
								break;
							case "outlet":
							case "alloutlet":
								this.m_gptypeOutletNode.Nodes.Add(treeNode2);
								this.appendDeviceNode(treeNode2, groupInfo, lastSelectedGpID, ref treeNode);
								break;
							}
						}
					}
					if (this.m_gptypeZoneNode.Nodes.Count != 0)
					{
						this.tvDataGp.Nodes.Add(this.m_gptypeZoneNode);
					}
					if (this.m_gptypeRackNode.Nodes.Count != 0)
					{
						this.tvDataGp.Nodes.Add(this.m_gptypeRackNode);
					}
					if (this.m_gptypeDevNode.Nodes.Count != 0)
					{
						this.tvDataGp.Nodes.Add(this.m_gptypeDevNode);
					}
					if (this.m_gptypeOutletNode.Nodes.Count != 0)
					{
						this.tvDataGp.Nodes.Add(this.m_gptypeOutletNode);
					}
				}
				else
				{
					System.Collections.Generic.List<GroupInfo> list2 = ClientAPI.getGroupCopy();
					if (list2 == null)
					{
						list2 = new System.Collections.Generic.List<GroupInfo>();
					}
					this.tvDataGp.Nodes.Add(this.m_gptypeZoneNode);
					this.tvDataGp.Nodes.Add(this.m_gptypeRackNode);
					this.tvDataGp.Nodes.Add(this.m_gptypeDevNode);
					this.tvDataGp.Nodes.Add(this.m_gptypeOutletNode);
					foreach (GroupInfo current2 in list2)
					{
						TreeNode treeNode3 = new TreeNode();
						treeNode3.Text = current2.GroupName;
						treeNode3.Tag = current2.ID;
						treeNode3.ImageIndex = 0;
						treeNode3.SelectedImageIndex = 0;
						if (num == current2.ID)
						{
							treeNode = treeNode3;
						}
						string groupType2;
						switch (groupType2 = current2.GroupType)
						{
						case "zone":
							this.m_gptypeZoneNode.Nodes.Add(treeNode3);
							this.appendDeviceNode(treeNode3, current2, lastSelectedGpID, ref treeNode);
							break;
						case "rack":
						case "allrack":
							this.m_gptypeRackNode.Nodes.Add(treeNode3);
							this.appendDeviceNode(treeNode3, current2, lastSelectedGpID, ref treeNode);
							break;
						case "dev":
						case "alldev":
							this.m_gptypeDevNode.Nodes.Add(treeNode3);
							this.appendDeviceNode(treeNode3, current2, lastSelectedGpID, ref treeNode);
							break;
						case "outlet":
						case "alloutlet":
							this.m_gptypeOutletNode.Nodes.Add(treeNode3);
							this.appendDeviceNode(treeNode3, current2, lastSelectedGpID, ref treeNode);
							break;
						}
					}
				}
				if (this.tvDataGp.Nodes.Count == 0)
				{
					return 1;
				}
				if (treeNode == null)
				{
					TreeNode treeNode4 = this.tvDataGp.Nodes[0];
					if (treeNode4.Nodes.Count > 0)
					{
						this.tvDataGp.SelectedNode = treeNode4.Nodes[0];
					}
					else
					{
						this.tvDataGp.SelectedNode = treeNode4;
					}
				}
				else
				{
					if (treeNode.Nodes.Count > 0)
					{
						treeNode = treeNode.Nodes[0];
					}
					this.tvDataGp.SelectedNode = treeNode;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("DataGpOPAll (treeDataInit) " + ex.Message);
			}
			return 0;
		}
		private void appendDeviceNode(TreeNode gpNode, GroupInfo foundGroup, long lastSelectedGpID, ref TreeNode willSelectedNode)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			string groupType;
			switch (groupType = foundGroup.GroupType)
			{
			case "zone":
				if (foundGroup.GetMemberList() != null && foundGroup.GetMemberList().Length > 0)
				{
					string text = ClientAPI.getRacklistByZonelist(foundGroup.GetMemberList());
					text = commUtil.uniqueIDs(text);
					if (text.Length > 0)
					{
						text = text.Substring(0, text.Length - 1);
						DataRow[] dataRows = ClientAPI.getDataRows(0, "rack_id in (" + text + ")", "");
						DataRow[] array = dataRows;
						for (int i = 0; i < array.Length; i++)
						{
							DataRow dataRow = array[i];
							dictionary.Add(dataRow["device_id"].ToString(), "");
						}
					}
				}
				break;
			case "rack":
			case "allrack":
				if (foundGroup.GetMemberList() != null && foundGroup.GetMemberList().Length > 0)
				{
					DataRow[] dataRows = ClientAPI.getDataRows(0, "rack_id in (" + foundGroup.GetMemberList() + ")", "");
					DataRow[] array2 = dataRows;
					for (int j = 0; j < array2.Length; j++)
					{
						DataRow dataRow2 = array2[j];
						dictionary.Add(dataRow2["device_id"].ToString(), "");
					}
				}
				break;
			case "dev":
			case "alldev":
				if (foundGroup.GetMemberList() != null && foundGroup.GetMemberList().Length > 0)
				{
					string[] array3 = foundGroup.GetMemberList().Split(new string[]
					{
						","
					}, System.StringSplitOptions.RemoveEmptyEntries);
					string[] array4 = array3;
					for (int k = 0; k < array4.Length; k++)
					{
						string key = array4[k];
						dictionary.Add(key, "");
					}
				}
				break;
			case "outlet":
			case "alloutlet":
			{
				new System.Collections.Generic.Dictionary<string, string>();
				DataSet dataSet = ClientAPI.getDataSet(0);
				if (dataSet == null)
				{
					return;
				}
				if (foundGroup.GetMemberList() != null && foundGroup.GetMemberList().Length != 0)
				{
					DataTable dataTable = dataSet.Tables[2];
					string[] array5 = foundGroup.GetMemberList().Split(new string[]
					{
						","
					}, System.StringSplitOptions.RemoveEmptyEntries);
					System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>();
					string[] array6 = array5;
					for (int l = 0; l < array6.Length; l++)
					{
						string text2 = array6[l];
						dictionary2.Add(text2, text2);
					}
					foreach (DataRow dataRow3 in dataTable.Rows)
					{
						string text3 = System.Convert.ToString(dataRow3["port_id"]);
						if (dictionary2.ContainsKey(text3))
						{
							string key2 = dataRow3["device_id"].ToString();
							if (dictionary.ContainsKey(key2))
							{
								string text4 = dictionary[key2];
								text4 = text4 + "," + text3;
								dictionary[key2] = text4;
							}
							else
							{
								dictionary.Add(key2, text3);
							}
						}
					}
				}
				break;
			}
			}
			DataRow[] dataRows2 = ClientAPI.getDataRows(0, "", "device_nm ASC");
			DataRow[] array7 = dataRows2;
			for (int m = 0; m < array7.Length; m++)
			{
				DataRow dataRow4 = array7[m];
				string text5 = System.Convert.ToString(dataRow4["device_id"]);
				if (dictionary.ContainsKey(text5))
				{
					long num2 = System.Convert.ToInt64(text5);
					string text6 = (string)dataRow4["device_nm"];
					TreeNode treeNode = new TreeNode();
					treeNode.Text = text6;
					treeNode.Tag = text5 + "@" + dictionary[text5];
					if (!ClientAPI.IsDeviceOnline(System.Convert.ToInt32(text5)))
					{
						treeNode.ImageIndex = 2;
						treeNode.SelectedImageIndex = 2;
					}
					else
					{
						treeNode.ImageIndex = 3;
						treeNode.SelectedImageIndex = 3;
					}
					gpNode.Nodes.Add(treeNode);
					if (lastSelectedGpID == num2)
					{
						willSelectedNode = treeNode;
					}
				}
			}
		}
		private void tvDataGp_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.endTimer();
			this.m_lastSelectedTag = e.Node.Tag.ToString();
			string text = e.Node.Tag.ToString();
			if (e.Node.Level != 0)
			{
				if (e.Node.Level == 1)
				{
					this.dataGpOPGroup1.Visible = true;
					this.dataGpOPDev1.Visible = false;
					this.dataGpOPGroup1.pageInit(text, 0, this);
				}
				else
				{
					if (e.Node.Level == 2)
					{
						this.dataGpOPGroup1.Visible = false;
						this.dataGpOPDev1.Visible = true;
						this.dataGpOPDev1.pageInit(text, e.Node.Text, this);
					}
				}
				this.starTimer();
				return;
			}
			if (e.Node.Nodes.Count > 0)
			{
				TreeNode selectedNode = e.Node.Nodes[0];
				this.tvDataGp.SelectedNode = selectedNode;
				return;
			}
			this.dataGpOPGroup1.pageInit("", 0, this);
			this.dataGpOPGroup1.Visible = true;
			this.dataGpOPDev1.Visible = false;
		}
		private void tvDataGp_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Level == 0 || node.Level == 1)
			{
				node.ImageIndex = 0;
				node.SelectedImageIndex = 0;
				this.tvDataGp.Refresh();
			}
		}
		private void tvDataGp_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Level == 0 || node.Level == 1)
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
				this.tvDataGp.Refresh();
			}
		}
		private int RefreshTree()
		{
			if (this.GroupTreeOpFlg == 64)
			{
				return 1;
			}
			if (ClientAPI.getDataSet(1) == null)
			{
				return -1;
			}
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			foreach (TreeNode treeNode in this.tvDataGp.Nodes)
			{
				foreach (TreeNode treeNode2 in treeNode.Nodes)
				{
					foreach (TreeNode treeNode3 in treeNode2.Nodes)
					{
						string text = treeNode3.Tag.ToString();
						text = text.Substring(0, text.IndexOf('@'));
						int num = System.Convert.ToInt32(text);
						if (!ClientAPI.IsDeviceExisted(num))
						{
							int result = 1;
							return result;
						}
						if (!devicRackZoneRelation.ContainsKey(num))
						{
							int result = 1;
							return result;
						}
						ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[num];
						if (!deviceWithZoneRackInfo.device_nm.Equals(treeNode3.Text))
						{
							treeNode3.Text = deviceWithZoneRackInfo.device_nm;
						}
						if (!ClientAPI.IsDeviceOnline(num))
						{
							treeNode3.ImageIndex = 2;
							treeNode3.SelectedImageIndex = 2;
						}
						else
						{
							treeNode3.ImageIndex = 3;
							treeNode3.SelectedImageIndex = 3;
						}
					}
				}
			}
			return 2;
		}
	}
}
