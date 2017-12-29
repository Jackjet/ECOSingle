using EcoSensors.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
namespace EcoSensors.EnegManPage.DashBoard.meter
{
	public class meter : UserControl
	{
		private IContainer components;
		private Label lbName;
		private Label lbValue;
		private Panel panel3;
		private PictureBox pB1;
		private float AimAngle;
		private float CurAngle;
		private Timer timerMeter = new Timer();
		private Point positionRect = new Point(0, 0);
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
			this.lbName = new Label();
			this.lbValue = new Label();
			this.panel3 = new Panel();
			this.pB1 = new PictureBox();
			this.panel3.SuspendLayout();
			((ISupportInitialize)this.pB1).BeginInit();
			base.SuspendLayout();
			this.lbName.BackColor = Color.Transparent;
			this.lbName.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbName.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbName.Location = new Point(100, 16);
			this.lbName.Margin = new Padding(5, 0, 5, 0);
			this.lbName.Name = "lbName";
			this.lbName.Size = new Size(65, 14);
			this.lbName.TabIndex = 4;
			this.lbName.Text = "RCI Hi";
			this.lbName.TextAlign = ContentAlignment.MiddleCenter;
			this.lbValue.BackColor = Color.Transparent;
			this.lbValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.lbValue.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbValue.Location = new Point(100, 52);
			this.lbValue.Margin = new Padding(0);
			this.lbValue.Name = "lbValue";
			this.lbValue.Size = new Size(65, 14);
			this.lbValue.TabIndex = 6;
			this.lbValue.Text = "999.99%";
			this.lbValue.TextAlign = ContentAlignment.MiddleCenter;
			this.panel3.BackColor = Color.Transparent;
			this.panel3.BackgroundImage = Resources.meter_RCI;
			this.panel3.Controls.Add(this.pB1);
			this.panel3.Location = new Point(0, 0);
			this.panel3.Margin = new Padding(4, 3, 4, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new Size(100, 100);
			this.panel3.TabIndex = 4;
			this.pB1.BackColor = Color.Transparent;
			this.pB1.BackgroundImage = Resources.meter_back;
			this.pB1.Image = Resources.meter_dialdot;
			this.pB1.Location = new Point(0, 0);
			this.pB1.Margin = new Padding(5, 3, 5, 3);
			this.pB1.Name = "pB1";
			this.pB1.Size = new Size(100, 100);
			this.pB1.TabIndex = 2;
			this.pB1.TabStop = false;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackgroundImage = Resources.meter_background;
			this.BackgroundImageLayout = ImageLayout.Stretch;
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.lbValue);
			base.Controls.Add(this.lbName);
			this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Margin = new Padding(4, 3, 4, 3);
			base.Name = "meter";
			base.Size = new Size(165, 100);
			this.panel3.ResumeLayout(false);
			((ISupportInitialize)this.pB1).EndInit();
			base.ResumeLayout(false);
		}
		public meter()
		{
			this.InitializeComponent();
			this.timerMeter.Interval = 100;
			this.timerMeter.Tick += new System.EventHandler(this.timerMeter_Tick);
			this.DoubleBuffered = true;
			this.BackColor = Color.Transparent;
			base.Paint += new PaintEventHandler(this.Meter_Paint);
		}
		private void timerMeter_Tick(object sender, System.EventArgs e)
		{
			if (this.CurAngle > this.AimAngle)
			{
				float num = (float)System.Math.Max(1.0, System.Math.Round((double)System.Math.Abs(System.Math.Abs(this.AimAngle) - System.Math.Abs(this.CurAngle)) / 10.0));
				float num2 = this.CurAngle - num;
				if (num2 <= this.AimAngle)
				{
					this.timerMeter.Enabled = false;
					this.CurAngle = this.AimAngle;
				}
				else
				{
					this.CurAngle = num2;
				}
			}
			else
			{
				if (this.AimAngle > 360f)
				{
				}
				float num3 = (float)System.Math.Max(1.0, System.Math.Round((double)System.Math.Abs(System.Math.Abs(this.AimAngle) - System.Math.Abs(this.CurAngle)) / 10.0));
				float num2 = this.CurAngle + num3;
				if (num2 >= this.AimAngle)
				{
					this.timerMeter.Enabled = false;
					this.CurAngle = this.AimAngle;
				}
				else
				{
					if (num2 >= -360f && num2 <= 360f)
					{
						this.CurAngle = num2;
					}
					else
					{
						this.CurAngle = 0f;
						this.AimAngle = 0f;
					}
				}
			}
			base.Invalidate();
		}
		private void Meter_Paint(object sender, PaintEventArgs e)
		{
			int num = 0;
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.ResetTransform();
			e.Graphics.TranslateTransform((float)this.positionRect.X + 50f, (float)(this.positionRect.Y + num) + 50f);
			e.Graphics.RotateTransform(this.CurAngle);
			e.Graphics.DrawImage(Resources.meter_dial, -5, -50, 10, 100);
			e.Graphics.ResetTransform();
		}
		public void init(string strLable, double v)
		{
			string str = v.ToString("F2");
			this.lbName.Text = strLable;
			this.lbValue.Text = str + "%";
			float num = (float)v;
			if (num < 0f)
			{
				num = 0f;
			}
			else
			{
				if (num > 100f)
				{
					num = 100f;
				}
			}
			if (num < 90f)
			{
				this.AimAngle = 2f * num;
			}
			else
			{
				if (num < 95f)
				{
					this.AimAngle = 180f + 90f * (num - 90f) / 5f;
				}
				else
				{
					this.AimAngle = 270f + 90f * (num - 95f) / 5f;
				}
			}
			this.timerMeter.Enabled = true;
		}
	}
}
