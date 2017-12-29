using DBAccessAPI;
using EcoSensors._Lang;
using InSnergyAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.DevManPage.OtherDevices
{
	public class OtherDevices : UserControl
	{
		private delegate void devRefresh();
		public const int MSG_DEVTREE_GENSEL = 63000;
		public const string TV_OtherDevRoot = "DevRoot";
		private const string TV_GateWay = "GateWay";
		private const string TV_BrPanel = "BrPanel";
		private const int IMG_folderclosed = 0;
		private const int IMG_folderopen = 1;
		private const int IMG_gatewayoff = 2;
		private const int IMG_gatewayon = 3;
		private const int IMG_branchoff = 4;
		private const int IMG_branchon = 5;
		private IContainer components;
		private TreeView tvDevOther;
		private SplitContainer splitContainer1;
		private ImageList imageList1;
		private OtherDevicesAllDev otherDevicesAllDev1;
		private OtherDeviceISGSubMeter otherDeviceISGSubMeter1;
		private OtherDeviceISGBrPanel otherDeviceISGBrPanel1;
		private System.Timers.Timer dTimer;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OtherDevices));
			this.tvDevOther = new TreeView();
			this.imageList1 = new ImageList(this.components);
			this.splitContainer1 = new SplitContainer();
			this.otherDeviceISGSubMeter1 = new OtherDeviceISGSubMeter();
			this.otherDeviceISGBrPanel1 = new OtherDeviceISGBrPanel();
			this.otherDevicesAllDev1 = new OtherDevicesAllDev();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.tvDevOther.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.tvDevOther, "tvDevOther");
			this.tvDevOther.HideSelection = false;
			this.tvDevOther.ImageList = this.imageList1;
			this.tvDevOther.Name = "tvDevOther";
			this.tvDevOther.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("tvDevOther.Nodes")
			});
			this.tvDevOther.AfterCollapse += new TreeViewEventHandler(this.tvDevOther_AfterCollapse);
			this.tvDevOther.AfterExpand += new TreeViewEventHandler(this.tvDevOther_AfterExpand);
			this.tvDevOther.AfterSelect += new TreeViewEventHandler(this.tvDevOther_AfterSelect);
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "folderclosed.gif");
			this.imageList1.Images.SetKeyName(1, "folderopen.gif");
			this.imageList1.Images.SetKeyName(2, "gatewayoff.gif");
			this.imageList1.Images.SetKeyName(3, "gatewayon.gif");
			this.imageList1.Images.SetKeyName(4, "ammeteroff.gif");
			this.imageList1.Images.SetKeyName(5, "ammeteron.gif");
			this.imageList1.Images.SetKeyName(6, "Bank.png");
			this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvDevOther);
			this.splitContainer1.Panel2.Controls.Add(this.otherDeviceISGSubMeter1);
			this.splitContainer1.Panel2.Controls.Add(this.otherDeviceISGBrPanel1);
			this.splitContainer1.Panel2.Controls.Add(this.otherDevicesAllDev1);
			this.otherDeviceISGSubMeter1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.otherDeviceISGSubMeter1, "otherDeviceISGSubMeter1");
			this.otherDeviceISGSubMeter1.Name = "otherDeviceISGSubMeter1";
			this.otherDeviceISGBrPanel1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.otherDeviceISGBrPanel1, "otherDeviceISGBrPanel1");
			this.otherDeviceISGBrPanel1.Name = "otherDeviceISGBrPanel1";
			this.otherDevicesAllDev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.otherDevicesAllDev1, "otherDevicesAllDev1");
			this.otherDevicesAllDev1.Name = "otherDevicesAllDev1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "OtherDevices";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public OtherDevices()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_otherDevCtrl = this;
			this.InitTimer();
		}
		public void pageInit()
		{
			this.DevtreeInit();
			this.tvDevOther.SelectedNode = this.tvDevOther.Nodes[0];
			this.dTimer.Interval = 100.0;
			this.starTimer();
		}
		private void DevtreeInit(string objID)
		{
			this.DevtreeInit();
			if (this.tvDevOther.Nodes[0].Name.Equals(objID))
			{
				this.tvDevOther.SelectedNode = this.tvDevOther.Nodes[0];
				return;
			}
			int num = this.devtreesel(this.tvDevOther.Nodes[0].Nodes, objID);
			if (num == 1)
			{
				return;
			}
			this.tvDevOther.SelectedNode = this.tvDevOther.Nodes[0];
		}
		private int devtreesel(TreeNodeCollection Nodes, string objID)
		{
			foreach (TreeNode treeNode in Nodes)
			{
				if (treeNode.Name.Equals(objID))
				{
					this.tvDevOther.SelectedNode = treeNode;
					this.tvDevOther.Nodes[0].ImageIndex = 1;
					this.tvDevOther.Nodes[0].SelectedImageIndex = 1;
					this.tvDevOther.Refresh();
					int result = 1;
					return result;
				}
				int num = this.devtreesel(treeNode.Nodes, objID);
				if (num == 1)
				{
					int result = 1;
					return result;
				}
			}
			return 0;
		}
		private void DevtreeInit()
		{
			InSnergyService.UpdateLocalTree();
			this.tvDevOther.Nodes.Clear();
			TreeNode treeNode = new TreeNode();
			treeNode.Text = EcoLanguage.getMsg(LangRes.OtherDevRoot, new string[0]);
			treeNode.Name = "DevRoot";
			treeNode.Tag = "DevRoot";
			treeNode.ImageIndex = 0;
			treeNode.SelectedImageIndex = 0;
			this.tvDevOther.Nodes.Add(treeNode);
			System.Collections.Generic.List<InSnergyGateway> allGateWay = InSnergyGateway.GetAllGateWay();
			foreach (InSnergyGateway current in allGateWay)
			{
				string gatewayName = current.GatewayName;
				TreeNode treeNode2 = new TreeNode();
				treeNode2.Text = gatewayName;
				treeNode2.Name = current.GatewayID;
				treeNode2.Tag = "GateWay";
				if (InSnergyService.IsGatewayOnline(current.GatewayID))
				{
					treeNode2.ImageIndex = 3;
					treeNode2.SelectedImageIndex = 3;
				}
				else
				{
					treeNode2.ImageIndex = 2;
					treeNode2.SelectedImageIndex = 2;
				}
				treeNode.Nodes.Add(treeNode2);
				foreach (Branch current2 in current.BranchList)
				{
					TreeNode treeNode3 = new TreeNode();
					treeNode3.Text = current2.BranchName;
					treeNode3.Name = current2.GatewayID + ":" + current2.BranchID;
					treeNode3.Tag = "BrPanel";
					if (InSnergyService.IsBranchOnline(current2.GatewayID, current2.BranchID))
					{
						treeNode3.ImageIndex = 5;
						treeNode3.SelectedImageIndex = 5;
					}
					else
					{
						treeNode3.ImageIndex = 4;
						treeNode3.SelectedImageIndex = 4;
					}
					treeNode2.Nodes.Add(treeNode3);
				}
			}
		}
		private void InitTimer()
		{
			if (this.dTimer == null)
			{
				this.dTimer = new System.Timers.Timer();
				this.dTimer.Elapsed += new ElapsedEventHandler(this.theout);
				this.dTimer.Interval = 100.0;
				this.dTimer.AutoReset = true;
				this.dTimer.Enabled = false;
			}
		}
		private void theout(object source, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new OtherDevices.devRefresh(this.deviceTimerRefresh));
				return;
			}
			this.deviceTimerRefresh();
		}
		private void deviceTimerRefresh()
		{
			this.dTimer.Interval = 10000.0;
			InSnergyService.UpdateLocalTree();
			this.refreshTree(this.tvDevOther.Nodes[0].Nodes);
			TreeNode selectedNode = this.tvDevOther.SelectedNode;
			string text = selectedNode.Tag.ToString();
			if (text.Equals("DevRoot"))
			{
				this.otherDevicesAllDev1.TimerProc();
				return;
			}
			if (text.Equals("GateWay"))
			{
				this.otherDeviceISGBrPanel1.TimerProc();
				return;
			}
			if (text.Equals("BrPanel"))
			{
				this.otherDeviceISGSubMeter1.TimerProc();
			}
		}
		private void starTimer()
		{
			this.dTimer.Enabled = true;
		}
		public void endTimer()
		{
			if (this.dTimer != null)
			{
				this.dTimer.Enabled = false;
			}
		}
		public void refreshTree(TreeNodeCollection Nodes)
		{
			foreach (TreeNode treeNode in Nodes)
			{
				string name = treeNode.Name;
				string text = (string)treeNode.Tag;
				if (text.Equals("GateWay"))
				{
					if (!InSnergyService.IsGatewayOnline(name))
					{
						treeNode.ImageIndex = 2;
						treeNode.SelectedImageIndex = 2;
					}
					else
					{
						treeNode.ImageIndex = 3;
						treeNode.SelectedImageIndex = 3;
					}
				}
				else
				{
					if (text.Equals("BrPanel"))
					{
						string gw = name.Split(new char[]
						{
							':'
						})[0];
						string branch = name.Split(new char[]
						{
							':'
						})[1];
						if (InSnergyService.IsBranchOnline(gw, branch))
						{
							treeNode.ImageIndex = 5;
							treeNode.SelectedImageIndex = 5;
						}
						else
						{
							treeNode.ImageIndex = 4;
							treeNode.SelectedImageIndex = 4;
						}
					}
				}
				this.refreshTree(treeNode.Nodes);
			}
		}
		private void tvDevOther_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string text = e.Node.Tag.ToString();
			string text2 = e.Node.Text;
			this.otherDevicesAllDev1.Visible = false;
			this.otherDeviceISGBrPanel1.Visible = false;
			this.otherDeviceISGSubMeter1.Visible = false;
			string a;
			if ((a = text) != null)
			{
				if (a == "DevRoot")
				{
					this.otherDevicesAllDev1.pageInit(this, e.Node);
					this.otherDevicesAllDev1.Visible = true;
					return;
				}
				if (a == "GateWay")
				{
					string name = e.Node.Name;
					this.otherDeviceISGBrPanel1.pageInit(this, name, text2);
					this.otherDeviceISGBrPanel1.Visible = true;
					this.tvDevOther.Focus();
					return;
				}
				if (!(a == "BrPanel"))
				{
					return;
				}
				string name2 = e.Node.Name;
				string gatewayID = name2.Split(new char[]
				{
					':'
				})[0];
				string branchID = name2.Split(new char[]
				{
					':'
				})[1];
				this.otherDeviceISGSubMeter1.pageInit(this, gatewayID, branchID);
				this.otherDeviceISGSubMeter1.Visible = true;
				this.tvDevOther.Focus();
			}
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63000)
			{
				string objID = System.Runtime.InteropServices.Marshal.PtrToStringUni(m.LParam);
				this.DevtreeInit(objID);
				return;
			}
			base.DefWndProc(ref m);
		}
		private void tvDevOther_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name == "DevRoot")
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
				this.tvDevOther.Refresh();
			}
		}
		private void tvDevOther_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name == "DevRoot")
			{
				node.ImageIndex = 0;
				node.SelectedImageIndex = 0;
				this.tvDevOther.Refresh();
			}
		}
	}
}
