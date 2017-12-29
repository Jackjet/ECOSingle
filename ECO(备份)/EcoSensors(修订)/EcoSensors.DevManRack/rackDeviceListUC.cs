using DBAccessAPI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManRack
{
	public class rackDeviceListUC : UserControl
	{
		private IContainer components;
		private ListBox lbDevice;
		private Label labrackNm;
		private long rackID;
		private string m_rackNm;
		public long RacKID
		{
			get
			{
				return this.rackID;
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
			this.lbDevice = new ListBox();
			this.labrackNm = new Label();
			base.SuspendLayout();
			this.lbDevice.BackColor = Color.LightGray;
			this.lbDevice.BorderStyle = BorderStyle.FixedSingle;
			this.lbDevice.FormattingEnabled = true;
			this.lbDevice.ItemHeight = 15;
			this.lbDevice.Location = new Point(4, 37);
			this.lbDevice.Margin = new Padding(4, 3, 4, 3);
			this.lbDevice.Name = "lbDevice";
			this.lbDevice.Size = new Size(204, 152);
			this.lbDevice.TabIndex = 5;
			this.labrackNm.BackColor = Color.LightGray;
			this.labrackNm.ImeMode = ImeMode.NoControl;
			this.labrackNm.Location = new Point(38, 7);
			this.labrackNm.Margin = new Padding(4, 0, 4, 0);
			this.labrackNm.Name = "labrackNm";
			this.labrackNm.Size = new Size(135, 22);
			this.labrackNm.TabIndex = 4;
			this.labrackNm.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.LightGray;
			base.Controls.Add(this.lbDevice);
			base.Controls.Add(this.labrackNm);
			this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "rackDeviceListUC";
			base.Size = new Size(212, 196);
			base.ResumeLayout(false);
		}
		public rackDeviceListUC()
		{
			this.InitializeComponent();
		}
		public rackDeviceListUC(long r_id, string rackNm, string devIDs)
		{
			this.InitializeComponent();
			this.rackID = r_id;
			this.m_rackNm = rackNm;
			this.initPage(rackNm, devIDs);
		}
		private void initPage(string rackNm, string devIDs)
		{
			Program.m_IdleCounter = 0;
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
