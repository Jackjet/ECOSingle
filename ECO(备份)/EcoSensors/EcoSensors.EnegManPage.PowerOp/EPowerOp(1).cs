using DBAccessAPI.user;
using Dispatcher;
using EcoSensors._Lang;
using EcoSensors.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.PowerOp
{
	public class EPowerOp : UserControl
	{
		public const int MSG_DEVTREE_GENSEL = 63000;
		public const int RefreshST_nonewDT = -1;
		public const int RefreshST_havedismatch = 1;
		public const int RefreshST_newDT = 2;
		private const string TV_DevRoot = "DevRoot";
		private const string TV_Dev = "Dev";
		private const int IMG_folderclosed = 0;
		private const int IMG_folderopen = 1;
		private const int IMG_deviceoff = 2;
		private const int IMG_deviceon = 3;
		private TreeNode m_AlldevicesNode;
		private IContainer components;
		private SplitContainer splitContainer1;
		private TreeView tvPowerControl;
		private EPowerOpDev ePowerOpDev1;
		private ImageList imageList1;
		public EPowerOp()
		{
			this.InitializeComponent();
			this.m_AlldevicesNode = new TreeNode();
			this.m_AlldevicesNode.Text = EcoLanguage.getMsg(LangRes.DevRoot, new string[0]);
			this.m_AlldevicesNode.Name = "DevRoot";
			this.m_AlldevicesNode.Tag = "DevRoot";
			this.m_AlldevicesNode.ImageIndex = 0;
			this.m_AlldevicesNode.SelectedImageIndex = 0;
			this.tvPowerControl.Nodes.Add(this.m_AlldevicesNode);
		}
		public void pageInit()
		{
			this.treeDataInit();
		}
		private void treeDataInit()
		{
			System.DateTime now = System.DateTime.Now;
			commUtil.ShowInfo_DEBUG("treeDataInit ---1 " + now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
			try
			{
				this.m_AlldevicesNode.Nodes.Clear();
				UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
				if (gl_LoginUser.UserType == 0)
				{
					DataRow[] dataRows = ClientAPI.getDataRows(0, "", "device_nm ASC");
					DataRow[] array = dataRows;
					for (int i = 0; i < array.Length; i++)
					{
						DataRow dataRow = array[i];
						string text = (string)dataRow["device_nm"];
						string text2 = System.Convert.ToString(dataRow["device_id"]);
						TreeNode treeNode = new TreeNode();
						treeNode.Text = text;
						treeNode.Tag = text2;
						if (!ClientAPI.IsDeviceOnline(System.Convert.ToInt32(text2)))
						{
							treeNode.ImageIndex = 2;
							treeNode.SelectedImageIndex = 2;
						}
						else
						{
							treeNode.ImageIndex = 3;
							treeNode.SelectedImageIndex = 3;
						}
						this.m_AlldevicesNode.Nodes.Add(treeNode);
					}
				}
				else
				{
					DataRow[] dataRows2 = ClientAPI.getDataRows(0, "", "device_nm ASC");
					string userDevice = gl_LoginUser.UserDevice;
					string[] source = userDevice.Split(new char[]
					{
						','
					});
					DataRow[] array2 = dataRows2;
					for (int j = 0; j < array2.Length; j++)
					{
						DataRow dataRow2 = array2[j];
						string text3 = System.Convert.ToString(dataRow2["device_id"]);
						if (source.Contains(text3))
						{
							string text4 = (string)dataRow2["device_nm"];
							TreeNode treeNode2 = new TreeNode(text4);
							treeNode2.Text = text4;
							treeNode2.Tag = text3;
							if (!ClientAPI.IsDeviceOnline(System.Convert.ToInt32(text3)))
							{
								treeNode2.ImageIndex = 2;
								treeNode2.SelectedImageIndex = 2;
							}
							else
							{
								treeNode2.ImageIndex = 3;
								treeNode2.SelectedImageIndex = 3;
							}
							this.m_AlldevicesNode.Nodes.Add(treeNode2);
						}
					}
				}
				System.DateTime now2 = System.DateTime.Now;
				commUtil.ShowInfo_DEBUG("--treeDataInit--  Spend:" + (now2 - now).TotalMilliseconds);
				if (this.m_AlldevicesNode.Nodes.Count > 0)
				{
					this.tvPowerControl.SelectedNode = this.m_AlldevicesNode.Nodes[0];
				}
				else
				{
					this.ePowerOpDev1.pageInit(-1, "", this);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		protected override void DefWndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 63000)
			{
				System.Runtime.InteropServices.Marshal.PtrToStringUni(m.LParam);
				this.treeDataInit();
				return;
			}
			base.DefWndProc(ref m);
		}
		public int refreshTree()
		{
			if (ClientAPI.getDataSet(1) == null)
			{
				return -1;
			}
			TreeNode treeNode = this.tvPowerControl.Nodes[0];
			int count = treeNode.Nodes.Count;
			int deviceCount = this.getDeviceCount();
			if (count != deviceCount)
			{
				return 1;
			}
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			foreach (TreeNode treeNode2 in treeNode.Nodes)
			{
				string value = (string)treeNode2.Tag;
				int num = System.Convert.ToInt32(value);
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
				if (!deviceWithZoneRackInfo.device_nm.Equals(treeNode2.Text))
				{
					treeNode2.Text = deviceWithZoneRackInfo.device_nm;
				}
				if (!ClientAPI.IsDeviceOnline(num))
				{
					treeNode2.ImageIndex = 2;
					treeNode2.SelectedImageIndex = 2;
				}
				else
				{
					treeNode2.ImageIndex = 3;
					treeNode2.SelectedImageIndex = 3;
				}
			}
			return 2;
		}
		private int getDeviceCount()
		{
			UserInfo gl_LoginUser = EcoGlobalVar.gl_LoginUser;
			if (gl_LoginUser.UserType == 0)
			{
				return ClientAPI.getDeviceCount();
			}
			DataRow[] dataRows = ClientAPI.getDataRows(0, "", "device_nm ASC");
			string userDevice = gl_LoginUser.UserDevice;
			string[] source = userDevice.Split(new char[]
			{
				','
			});
			int num = 0;
			DataRow[] array = dataRows;
			for (int i = 0; i < array.Length; i++)
			{
				DataRow dataRow = array[i];
				string value = System.Convert.ToString(dataRow["device_id"]);
				if (source.Contains(value))
				{
					num++;
				}
			}
			return num;
		}
		private void tvPowerControl_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string text = e.Node.Text;
			string text2 = e.Node.Tag.ToString();
			if (text2.Equals("DevRoot"))
			{
				return;
			}
			this.ePowerOpDev1.pageInit(System.Convert.ToInt32(text2), text, this);
		}
		private void tvPowerControl_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			string arg_0B_0 = e.Node.Text;
			string text = e.Node.Tag.ToString();
			if (text.Equals("DevRoot"))
			{
				e.Cancel = true;
			}
		}
		private void tvPowerControl_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Tag == "DevRoot")
			{
				node.ImageIndex = 0;
				node.SelectedImageIndex = 0;
				this.tvPowerControl.Refresh();
			}
		}
		private void tvPowerControl_AfterExpand(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			if (node.Tag == "DevRoot")
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
				this.tvPowerControl.Refresh();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EPowerOp));
			this.splitContainer1 = new SplitContainer();
			this.tvPowerControl = new TreeView();
			this.imageList1 = new ImageList(this.components);
			this.ePowerOpDev1 = new EPowerOpDev();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.BorderStyle = BorderStyle.Fixed3D;
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvPowerControl);
			this.splitContainer1.Panel2.Controls.Add(this.ePowerOpDev1);
			this.tvPowerControl.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.tvPowerControl, "tvPowerControl");
			this.tvPowerControl.HideSelection = false;
			this.tvPowerControl.ImageList = this.imageList1;
			this.tvPowerControl.Name = "tvPowerControl";
			this.tvPowerControl.AfterCollapse += new TreeViewEventHandler(this.tvPowerControl_AfterCollapse);
			this.tvPowerControl.AfterExpand += new TreeViewEventHandler(this.tvPowerControl_AfterExpand);
			this.tvPowerControl.BeforeSelect += new TreeViewCancelEventHandler(this.tvPowerControl_BeforeSelect);
			this.tvPowerControl.AfterSelect += new TreeViewEventHandler(this.tvPowerControl_AfterSelect);
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "folderclosed.gif");
			this.imageList1.Images.SetKeyName(1, "folderopen.gif");
			this.imageList1.Images.SetKeyName(2, "deviceoff.gif");
			this.imageList1.Images.SetKeyName(3, "deviceon.gif");
			componentResourceManager.ApplyResources(this.ePowerOpDev1, "ePowerOpDev1");
			this.ePowerOpDev1.BackColor = Color.WhiteSmoke;
			this.ePowerOpDev1.Name = "ePowerOpDev1";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "EPowerOp";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
