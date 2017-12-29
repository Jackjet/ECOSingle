using EcoSensors.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
namespace EcoSensors.Monitor
{
	public class MonMeter : UserControl
	{
		public const int meterTYPE_PUE = 1;
		public const int meterTYPE_RTI = 2;
		public const int meterTYPE_RCI = 3;
		private IContainer components;
		private Label lbright;
		private Panel panel3;
		private PictureBox pB1;
		private Label lbValue;
		private Label lbName;
		private Label lbleft;
		private float AimAngle;
		private float CurAngle;
		private Timer timerMeter = new Timer();
		private Point positionRect = new Point(15, 15);
		private int m_metertype = 1;
		private bool showPointer = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MonMeter));
			this.lbright = new Label();
			this.panel3 = new Panel();
			this.pB1 = new PictureBox();
			this.lbValue = new Label();
			this.lbName = new Label();
			this.lbleft = new Label();
			this.panel3.SuspendLayout();
			((ISupportInitialize)this.pB1).BeginInit();
			base.SuspendLayout();
			this.lbright.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.lbright, "lbright");
			this.lbright.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbright.Name = "lbright";
			this.panel3.BackColor = Color.Transparent;
			this.panel3.BackgroundImage = Resources.meter_RCI;
			this.panel3.Controls.Add(this.pB1);
			componentResourceManager.ApplyResources(this.panel3, "panel3");
			this.panel3.Name = "panel3";
			this.pB1.BackColor = Color.Transparent;
			this.pB1.BackgroundImage = Resources.meter_back;
			this.pB1.Image = Resources.meter_dialdot;
			componentResourceManager.ApplyResources(this.pB1, "pB1");
			this.pB1.Name = "pB1";
			this.pB1.TabStop = false;
			this.lbValue.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.lbValue, "lbValue");
			this.lbValue.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbValue.Name = "lbValue";
			this.lbName.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.lbName, "lbName");
			this.lbName.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbName.Name = "lbName";
			this.lbleft.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.lbleft, "lbleft");
			this.lbleft.ForeColor = Color.FromArgb(230, 230, 230);
			this.lbleft.Name = "lbleft";
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackgroundImage = Resources.meter_background;
			componentResourceManager.ApplyResources(this, "$this");
			base.Controls.Add(this.lbright);
			base.Controls.Add(this.lbleft);
			base.Controls.Add(this.panel3);
			base.Controls.Add(this.lbValue);
			base.Controls.Add(this.lbName);
			base.Name = "MonMeter";
			this.panel3.ResumeLayout(false);
			((ISupportInitialize)this.pB1).EndInit();
			base.ResumeLayout(false);
		}
		public MonMeter()
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
			if (this.showPointer)
			{
				e.Graphics.TranslateTransform((float)this.positionRect.X + 50f, (float)(this.positionRect.Y + num) + 50f);
				e.Graphics.RotateTransform(this.CurAngle);
				e.Graphics.DrawImage(Resources.meter_dial, -5, -50, 10, 100);
				e.Graphics.ResetTransform();
			}
		}
		public void init_lable(int metertype, string strLable, string strLeft, string strRight, string strValue, Bitmap scalebmp)
		{
			this.m_metertype = metertype;
			this.lbName.Text = strLable;
			this.lbleft.Text = strLeft;
			this.lbright.Text = strRight;
			this.lbValue.Text = strValue;
			this.panel3.BackgroundImage = scalebmp;
		}
		public void init_value(double v)
		{
			string text = v.ToString("F2");
			this.lbValue.Text = text;
			switch (this.m_metertype)
			{
			case 1:
			{
				float num = (float)v;
				if (num < 1f)
				{
					num = 1f;
				}
				else
				{
					if (num > 5f)
					{
						num = 5f;
					}
				}
				if ((double)num < 2.5)
				{
					this.AimAngle = 60f * (num - 1f);
				}
				else
				{
					if (num < 3f)
					{
						this.AimAngle = 90f + 60f * (num - 2.5f);
					}
					else
					{
						this.AimAngle = 120f + 75f * (num - 3f);
					}
				}
				this.showPointer = true;
				this.timerMeter.Enabled = true;
				return;
			}
			case 2:
			{
				Label expr_CE = this.lbValue;
				expr_CE.Text += "%";
				float num = (float)v;
				if (num < 50f)
				{
					num = 50f;
				}
				else
				{
					if (num > 150f)
					{
						num = 150f;
					}
				}
				if (num < 80f)
				{
					this.AimAngle = -150f + 2f * (num - 50f);
				}
				else
				{
					if (num < 85f)
					{
						this.AimAngle = -90f + 6f * (num - 80f);
					}
					else
					{
						if (num < 100f)
						{
							this.AimAngle = -60f + 4f * (num - 85f);
						}
						else
						{
							if (num < 115f)
							{
								this.AimAngle = 0f + 4f * (num - 100f);
							}
							else
							{
								if (num < 120f)
								{
									this.AimAngle = 60f + 6f * (num - 115f);
								}
								else
								{
									this.AimAngle = 90f + 2f * (num - 120f);
								}
							}
						}
					}
				}
				this.showPointer = true;
				this.timerMeter.Enabled = true;
				return;
			}
			case 3:
			{
				Label expr_1EC = this.lbValue;
				expr_1EC.Text += "%";
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
				this.showPointer = true;
				this.timerMeter.Enabled = true;
				return;
			}
			default:
				return;
			}
		}
		public void init_valueErr()
		{
			switch (this.m_metertype)
			{
			case 1:
				this.lbValue.Text = "";
				this.showPointer = false;
				this.timerMeter.Enabled = true;
				return;
			case 2:
				this.lbValue.Text = "%";
				this.showPointer = false;
				this.timerMeter.Enabled = true;
				return;
			case 3:
				this.lbValue.Text = "%";
				this.showPointer = false;
				this.timerMeter.Enabled = true;
				return;
			default:
				return;
			}
		}
	}
}
