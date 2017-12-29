using DBAccessAPI;
using Dispatcher;
using EcoSensors._Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.DevManDevice
{
	public class DevManDevice : UserControl
	{
		private delegate void devRefresh();
		public const int MSG_DEVTREE_GENSEL = 63000;
		public const string TV_DevRoot = "DevRoot";
		private const int IMG_folderclosed = 0;
		private const int IMG_folderopen = 1;
		private const int IMG_deviceoff = 2;
		private const int IMG_deviceon = 3;
		private IContainer components;
		private SplitContainer splitContainer1;
		private TreeView tvDev;
		private DevManAllDev devManAllDev1;
		private DevMan_DevTB devMan_Dev1;
		private ImageList imageList1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DevManDevice));
			this.splitContainer1 = new SplitContainer();
			this.tvDev = new TreeView();
			this.imageList1 = new ImageList(this.components);
			this.devMan_Dev1 = new DevMan_DevTB();
			this.devManAllDev1 = new DevManAllDev();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvDev);
			this.splitContainer1.Panel2.Controls.Add(this.devMan_Dev1);
			this.splitContainer1.Panel2.Controls.Add(this.devManAllDev1);
			this.tvDev.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.tvDev, "tvDev");
			this.tvDev.HideSelection = false;
			this.tvDev.ImageList = this.imageList1;
			this.tvDev.Name = "tvDev";
			this.tvDev.Nodes.AddRange(new TreeNode[]
			{
				(TreeNode)componentResourceManager.GetObject("tvDev.Nodes")
			});
			this.tvDev.AfterCollapse += new TreeViewEventHandler(this.tvDev_AfterCollapse);
			this.tvDev.AfterExpand += new TreeViewEventHandler(this.tvDev_AfterExpand);
			this.tvDev.AfterSelect += new TreeViewEventHandler(this.tvDev_AfterSelect);
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "folderclosed.gif");
			this.imageList1.Images.SetKeyName(1, "folderopen.gif");
			this.imageList1.Images.SetKeyName(2, "deviceoff.gif");
			this.imageList1.Images.SetKeyName(3, "deviceon.gif");
			this.devMan_Dev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devMan_Dev1, "devMan_Dev1");
			this.devMan_Dev1.Name = "devMan_Dev1";
			this.devManAllDev1.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.devManAllDev1, "devManAllDev1");
			this.devManAllDev1.Name = "devManAllDev1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "DevManDevice";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public DevManDevice()
		{
			this.InitializeComponent();
			EcoGlobalVar.gl_DevManCtrl = this;
			this.InitTimer();
		}
		public void pageInit()
		{
			this.devManAllDev1.clearFilter();
			this.DevtreeInit();
			this.tvDev.SelectedNode = this.tvDev.Nodes[0];
			this.tvDev.ExpandAll();
			DeviceInfo.SetRefreshFlag(2);
			this.dTimer.Interval = 100.0;
			this.starTimer();
		}
		private void DevtreeInit(string devName)
		{
			this.endTimer();
			if (devName.Equals("+1"))
			{
				TreeNode selectedNode = this.tvDev.SelectedNode;
				TreeNode treeNode = selectedNode.NextNode;
				if (treeNode == null)
				{
					treeNode = selectedNode.PrevNode;
					if (treeNode == null)
					{
						devName = "DevRoot";
					}
					else
					{
						devName = treeNode.Text;
					}
				}
				else
				{
					devName = treeNode.Text;
				}
			}
			this.DevtreeInit();
			this.starTimer();
			if (this.tvDev.Nodes[0].Text.Equals(devName))
			{
				this.tvDev.SelectedNode = this.tvDev.Nodes[0];
				this.tvDev.ExpandAll();
				return;
			}
			foreach (TreeNode treeNode2 in this.tvDev.Nodes[0].Nodes)
			{
				if (treeNode2.Text.Equals(devName))
				{
					this.tvDev.SelectedNode = treeNode2;
					this.tvDev.Nodes[0].ImageIndex = 1;
					this.tvDev.Nodes[0].SelectedImageIndex = 1;
					this.tvDev.Refresh();
					return;
				}
			}
			this.tvDev.SelectedNode = this.tvDev.Nodes[0];
			this.tvDev.ExpandAll();
		}
		private void DevtreeInit()
		{
			this.tvDev.Nodes.Clear();
			System.GC.Collect();
			TreeNode treeNode = new TreeNode();
			treeNode.Text = EcoLanguage.getMsg(LangRes.DevRoot, new string[0]);
			treeNode.Name = "DevRoot";
			treeNode.Tag = "DevRoot";
			treeNode.ImageIndex = 0;
			treeNode.SelectedImageIndex = 0;
			this.tvDev.Nodes.Add(treeNode);
			System.Collections.Generic.List<DeviceInfo> allDevice = DeviceOperation.GetAllDevice();
			for (int i = 0; i < allDevice.Count; i++)
			{
				DeviceInfo deviceInfo = allDevice[i];
				string deviceName = deviceInfo.DeviceName;
				TreeNode treeNode2 = new TreeNode();
				treeNode2.Text = deviceName;
				treeNode2.Name = deviceInfo.DeviceID.ToString();
				treeNode2.Tag = deviceInfo.Mac;
				if (!ClientAPI.IsDeviceOnline(deviceInfo.DeviceID))
				{
					treeNode2.ImageIndex = 2;
					treeNode2.SelectedImageIndex = 2;
				}
				else
				{
					treeNode2.ImageIndex = 3;
					treeNode2.SelectedImageIndex = 3;
				}
				treeNode.Nodes.Add(treeNode2);
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
				base.Invoke(new DevManDevice.devRefresh(this.deviceTimerRefresh));
				return;
			}
			this.deviceTimerRefresh();
		}
		private void deviceTimerRefresh()
		{
			this.dTimer.Interval = 10000.0;
			DataSet dataSet = ClientAPI.getDataSet(1);
			if (dataSet != null)
			{
				int haveThresholdChange = this.refreshTree();
				TreeNode selectedNode = this.tvDev.SelectedNode;
				string text = selectedNode.Tag.ToString();
				if (text.Equals("DevRoot"))
				{
					this.devManAllDev1.TimerProc(haveThresholdChange);
					return;
				}
				this.devMan_Dev1.TimerProc(haveThresholdChange);
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
		public int refreshTree()
		{
			int result = 0;
			TreeNode treeNode = this.tvDev.Nodes[0];
			int refreshFlag = DeviceInfo.GetRefreshFlag();
			foreach (TreeNode treeNode2 in treeNode.Nodes)
			{
				string name = treeNode2.Name;
				string arg_50_0 = (string)treeNode2.Tag;
				if (!ClientAPI.IsDeviceOnline(System.Convert.ToInt32(name)))
				{
					treeNode2.ImageIndex = 2;
					treeNode2.SelectedImageIndex = 2;
				}
				else
				{
					treeNode2.ImageIndex = 3;
					treeNode2.SelectedImageIndex = 3;
				}
				if (refreshFlag == 1)
				{
					result = 1;
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(System.Convert.ToInt32(name));
					string text = treeNode2.Text;
					if (!text.Equals(deviceByID.DeviceName))
					{
						treeNode2.Text = deviceByID.DeviceName;
					}
				}
			}
			DeviceInfo.SetRefreshFlag(2);
			return result;
		}
		private void tvDev_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string text = e.Node.Tag.ToString();
			string text2 = e.Node.Text;
			string a;
			if ((a = text) != null && a == "DevRoot")
			{
				this.devMan_Dev1.Visible = false;
				this.devManAllDev1.pageInit(this, e.Node);
				this.devManAllDev1.Visible = true;
				return;
			}
			this.devManAllDev1.Visible = false;
			string name = e.Node.Name;
			bool onlinest = ClientAPI.IsDeviceOnline(System.Convert.ToInt32(name));
			this.devMan_Dev1.pageInit(this, System.Convert.ToInt32(name), text2, onlinest);
			this.devMan_Dev1.Visible = true;
			this.tvDev.Focus();
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63000)
			{
				string devName = System.Runtime.InteropServices.Marshal.PtrToStringUni(m.LParam);
				this.DevtreeInit(devName);
				return;
			}
			base.DefWndProc(ref m);
		}
		private void tvDev_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name == "DevRoot")
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
				this.tvDev.Refresh();
			}
		}
		private void tvDev_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Name == "DevRoot")
			{
				node.ImageIndex = 0;
				node.SelectedImageIndex = 0;
				this.tvDev.Refresh();
			}
		}
	}
}
