using DBAccessAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManRack
{
	public class rackDeviceList : Form
	{
		private IContainer components;
		private ListBox lbDevice;
		private Label labrackNm;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(rackDeviceList));
			this.lbDevice = new ListBox();
			this.labrackNm = new Label();
			base.SuspendLayout();
			this.lbDevice.BackColor = Color.LightGray;
			this.lbDevice.BorderStyle = BorderStyle.FixedSingle;
			this.lbDevice.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.lbDevice, "lbDevice");
			this.lbDevice.Name = "lbDevice";
			this.labrackNm.BackColor = Color.LightGray;
			componentResourceManager.ApplyResources(this.labrackNm, "labrackNm");
			this.labrackNm.Name = "labrackNm";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.LightGray;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lbDevice);
			base.Controls.Add(this.labrackNm);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "rackDeviceList";
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
		}
		public rackDeviceList()
		{
			this.InitializeComponent();
		}
		public rackDeviceList(string rackNm, string devIDs)
		{
			this.InitializeComponent();
			this.initPage(rackNm, devIDs);
		}
		private void initPage(string rackNm, string devIDs)
		{
			int num = 212;
			this.labrackNm.Text = rackNm;
			using (Graphics graphics = base.CreateGraphics())
			{
				SizeF sizeF = graphics.MeasureString(rackNm, this.labrackNm.Font);
				int num2 = (int)System.Math.Ceiling((double)sizeF.Width);
				if (num2 > 135)
				{
					this.labrackNm.Width = (int)System.Math.Ceiling((double)(sizeF.Width + 5f));
				}
				else
				{
					this.labrackNm.Width = 135;
				}
				if (num2 + 77 > num)
				{
					num = num2 + 77;
				}
			}
			string[] array = devIDs.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!text.Equals(string.Empty))
				{
					DeviceInfo deviceByID = DeviceOperation.getDeviceByID(System.Convert.ToInt32(text));
					string text2 = deviceByID.DeviceName + ":" + deviceByID.DeviceIP;
					using (Graphics graphics2 = base.CreateGraphics())
					{
						int num2 = (int)System.Math.Ceiling((double)graphics2.MeasureString(text2, this.lbDevice.Font).Width);
						if (num2 + 8 > num)
						{
							num = num2 + 8;
						}
					}
					this.lbDevice.Items.Add(text2);
				}
			}
			this.lbDevice.Width = num - 8;
			base.Width = num;
		}
	}
}
