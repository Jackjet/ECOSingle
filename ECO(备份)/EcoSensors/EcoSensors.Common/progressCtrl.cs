using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	public class progressCtrl : UserControl
	{
		private IContainer components;
		private ProgressBar progressBar1;
		public progressCtrl()
		{
			this.InitializeComponent();
		}
		public void setprog_para(int min, int max, int step)
		{
			this.progressBar1.Minimum = min;
			this.progressBar1.Maximum = max;
			this.progressBar1.Step = step;
			this.progressBar1.Value = min;
		}
		public void setprog_performstep()
		{
			this.progressBar1.PerformStep();
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
			this.progressBar1 = new ProgressBar();
			base.SuspendLayout();
			this.progressBar1.Location = new Point(399, 311);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new Size(218, 13);
			this.progressBar1.TabIndex = 79;
			base.AutoScaleDimensions = new SizeF(8f, 14f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.progressBar1);
			this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Margin = new Padding(4, 3, 4, 3);
			base.Name = "progressCtrl";
			base.Size = new Size(1016, 635);
			base.ResumeLayout(false);
		}
	}
}
