using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.SysManPage.Tasks
{
	public class GpPower_Week : UserControl
	{
		private bool modifAll_only;
		private IContainer components;
		private TableLayoutPanel tblPanelWeekly;
		public CheckBox checkBoxOnW05;
		public CheckBox checkBoxOnW04;
		public DateTimePicker dtPickerOnW04;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label label10;
		private Label label11;
		private Label label12;
		public CheckBox checkBoxOnAll;
		public CheckBox checkBoxOnW01;
		public CheckBox checkBoxOnW02;
		public CheckBox checkBoxOnW03;
		public CheckBox checkBoxOffAll;
		public CheckBox checkBoxOffW01;
		public CheckBox checkBoxOffW02;
		public CheckBox checkBoxOffW03;
		public CheckBox checkBoxOffW04;
		public CheckBox checkBoxOffW05;
		public CheckBox checkBoxOffW06;
		public CheckBox checkBoxOnW06;
		public CheckBox checkBoxOffW07;
		public CheckBox checkBoxOnW07;
		public DateTimePicker dtPickerOnW01;
		public DateTimePicker dtPickerOnW02;
		public DateTimePicker dtPickerOnW03;
		public DateTimePicker dtPickerOnW05;
		public DateTimePicker dtPickerOnW06;
		public DateTimePicker dtPickerOnW07;
		public DateTimePicker dtPickerOffW01;
		public DateTimePicker dtPickerOffW02;
		public DateTimePicker dtPickerOffW03;
		public DateTimePicker dtPickerOffW04;
		public DateTimePicker dtPickerOffW05;
		public DateTimePicker dtPickerOffW06;
		public DateTimePicker dtPickerOffW07;
		public GpPower_Week()
		{
			this.InitializeComponent();
		}
		public void Init(string[,] arr_schedule)
		{
			if (arr_schedule[0, 0].Length == 0)
			{
				this.checkBoxOnW01.Checked = false;
			}
			else
			{
				this.checkBoxOnW01.Checked = true;
				this.dtPickerOnW01.Text = arr_schedule[0, 0];
			}
			if (arr_schedule[0, 1].Length == 0)
			{
				this.checkBoxOffW01.Checked = false;
			}
			else
			{
				this.checkBoxOffW01.Checked = true;
				this.dtPickerOffW01.Text = arr_schedule[0, 1];
			}
			if (arr_schedule[1, 0].Length == 0)
			{
				this.checkBoxOnW02.Checked = false;
			}
			else
			{
				this.checkBoxOnW02.Checked = true;
				this.dtPickerOnW02.Text = arr_schedule[1, 0];
			}
			if (arr_schedule[1, 1].Length == 0)
			{
				this.checkBoxOffW02.Checked = false;
			}
			else
			{
				this.checkBoxOffW02.Checked = true;
				this.dtPickerOffW02.Text = arr_schedule[1, 1];
			}
			if (arr_schedule[2, 0].Length == 0)
			{
				this.checkBoxOnW03.Checked = false;
			}
			else
			{
				this.checkBoxOnW03.Checked = true;
				this.dtPickerOnW03.Text = arr_schedule[2, 0];
			}
			if (arr_schedule[2, 1].Length == 0)
			{
				this.checkBoxOffW03.Checked = false;
			}
			else
			{
				this.checkBoxOffW03.Checked = true;
				this.dtPickerOffW03.Text = arr_schedule[2, 1];
			}
			if (arr_schedule[3, 0].Length == 0)
			{
				this.checkBoxOnW04.Checked = false;
			}
			else
			{
				this.checkBoxOnW04.Checked = true;
				this.dtPickerOnW04.Text = arr_schedule[3, 0];
			}
			if (arr_schedule[3, 1].Length == 0)
			{
				this.checkBoxOffW04.Checked = false;
			}
			else
			{
				this.checkBoxOffW04.Checked = true;
				this.dtPickerOffW04.Text = arr_schedule[3, 1];
			}
			if (arr_schedule[4, 0].Length == 0)
			{
				this.checkBoxOnW05.Checked = false;
			}
			else
			{
				this.checkBoxOnW05.Checked = true;
				this.dtPickerOnW05.Text = arr_schedule[4, 0];
			}
			if (arr_schedule[4, 1].Length == 0)
			{
				this.checkBoxOffW05.Checked = false;
			}
			else
			{
				this.checkBoxOffW05.Checked = true;
				this.dtPickerOffW05.Text = arr_schedule[4, 1];
			}
			if (arr_schedule[5, 0].Length == 0)
			{
				this.checkBoxOnW06.Checked = false;
			}
			else
			{
				this.checkBoxOnW06.Checked = true;
				this.dtPickerOnW06.Text = arr_schedule[5, 0];
			}
			if (arr_schedule[5, 1].Length == 0)
			{
				this.checkBoxOffW06.Checked = false;
			}
			else
			{
				this.checkBoxOffW06.Checked = true;
				this.dtPickerOffW06.Text = arr_schedule[5, 1];
			}
			if (arr_schedule[6, 0].Length == 0)
			{
				this.checkBoxOnW07.Checked = false;
			}
			else
			{
				this.checkBoxOnW07.Checked = true;
				this.dtPickerOnW07.Text = arr_schedule[6, 0];
			}
			if (arr_schedule[6, 1].Length == 0)
			{
				this.checkBoxOffW07.Checked = false;
				return;
			}
			this.checkBoxOffW07.Checked = true;
			this.dtPickerOffW07.Text = arr_schedule[6, 1];
		}
		public void getSchedule(string[,] arr_schedule)
		{
			if (this.checkBoxOnW01.Checked)
			{
				arr_schedule[0, 0] = this.dtPickerOnW01.Text + ":00";
			}
			else
			{
				arr_schedule[0, 0] = "";
			}
			if (this.checkBoxOffW01.Checked)
			{
				arr_schedule[0, 1] = this.dtPickerOffW01.Text + ":00";
			}
			else
			{
				arr_schedule[0, 1] = "";
			}
			if (this.checkBoxOnW02.Checked)
			{
				arr_schedule[1, 0] = this.dtPickerOnW02.Text + ":00";
			}
			else
			{
				arr_schedule[1, 0] = "";
			}
			if (this.checkBoxOffW02.Checked)
			{
				arr_schedule[1, 1] = this.dtPickerOffW02.Text + ":00";
			}
			else
			{
				arr_schedule[1, 1] = "";
			}
			if (this.checkBoxOnW03.Checked)
			{
				arr_schedule[2, 0] = this.dtPickerOnW03.Text + ":00";
			}
			else
			{
				arr_schedule[2, 0] = "";
			}
			if (this.checkBoxOffW03.Checked)
			{
				arr_schedule[2, 1] = this.dtPickerOffW03.Text + ":00";
			}
			else
			{
				arr_schedule[2, 1] = "";
			}
			if (this.checkBoxOnW04.Checked)
			{
				arr_schedule[3, 0] = this.dtPickerOnW04.Text + ":00";
			}
			else
			{
				arr_schedule[3, 0] = "";
			}
			if (this.checkBoxOffW04.Checked)
			{
				arr_schedule[3, 1] = this.dtPickerOffW04.Text + ":00";
			}
			else
			{
				arr_schedule[3, 1] = "";
			}
			if (this.checkBoxOnW05.Checked)
			{
				arr_schedule[4, 0] = this.dtPickerOnW05.Text + ":00";
			}
			else
			{
				arr_schedule[4, 0] = "";
			}
			if (this.checkBoxOffW05.Checked)
			{
				arr_schedule[4, 1] = this.dtPickerOffW05.Text + ":00";
			}
			else
			{
				arr_schedule[4, 1] = "";
			}
			if (this.checkBoxOnW06.Checked)
			{
				arr_schedule[5, 0] = this.dtPickerOnW06.Text + ":00";
			}
			else
			{
				arr_schedule[5, 0] = "";
			}
			if (this.checkBoxOffW06.Checked)
			{
				arr_schedule[5, 1] = this.dtPickerOffW06.Text + ":00";
			}
			else
			{
				arr_schedule[5, 1] = "";
			}
			if (this.checkBoxOnW07.Checked)
			{
				arr_schedule[6, 0] = this.dtPickerOnW07.Text + ":00";
			}
			else
			{
				arr_schedule[6, 0] = "";
			}
			if (this.checkBoxOffW07.Checked)
			{
				arr_schedule[6, 1] = this.dtPickerOffW07.Text + ":00";
				return;
			}
			arr_schedule[6, 1] = "";
		}
		private void checkBoxOnAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.modifAll_only)
			{
				bool @checked = this.checkBoxOnAll.Checked;
				this.checkBoxOnW01.Checked = @checked;
				this.checkBoxOnW02.Checked = @checked;
				this.checkBoxOnW03.Checked = @checked;
				this.checkBoxOnW04.Checked = @checked;
				this.checkBoxOnW05.Checked = @checked;
				this.checkBoxOnW06.Checked = @checked;
				this.checkBoxOnW07.Checked = @checked;
			}
			this.modifAll_only = false;
		}
		private void checkBoxOnWxx_CheckedChanged(object sender, System.EventArgs e)
		{
			bool @checked = this.checkBoxOnAll.Checked;
			if (sender.Equals(this.checkBoxOnW01))
			{
				this.dtPickerOnW01.Enabled = this.checkBoxOnW01.Checked;
				@checked = this.checkBoxOnW01.Checked;
			}
			else
			{
				if (sender.Equals(this.checkBoxOnW02))
				{
					this.dtPickerOnW02.Enabled = this.checkBoxOnW02.Checked;
					@checked = this.checkBoxOnW02.Checked;
				}
				else
				{
					if (sender.Equals(this.checkBoxOnW03))
					{
						this.dtPickerOnW03.Enabled = this.checkBoxOnW03.Checked;
						@checked = this.checkBoxOnW03.Checked;
					}
					else
					{
						if (sender.Equals(this.checkBoxOnW04))
						{
							this.dtPickerOnW04.Enabled = this.checkBoxOnW04.Checked;
							@checked = this.checkBoxOnW04.Checked;
						}
						else
						{
							if (sender.Equals(this.checkBoxOnW05))
							{
								this.dtPickerOnW05.Enabled = this.checkBoxOnW05.Checked;
								@checked = this.checkBoxOnW05.Checked;
							}
							else
							{
								if (sender.Equals(this.checkBoxOnW06))
								{
									this.dtPickerOnW06.Enabled = this.checkBoxOnW06.Checked;
									@checked = this.checkBoxOnW06.Checked;
								}
								else
								{
									if (sender.Equals(this.checkBoxOnW07))
									{
										this.dtPickerOnW07.Enabled = this.checkBoxOnW07.Checked;
										@checked = this.checkBoxOnW07.Checked;
									}
								}
							}
						}
					}
				}
			}
			if (this.checkBoxOnAll.Checked)
			{
				if (!@checked)
				{
					this.modifAll_only = true;
					this.checkBoxOnAll.Checked = false;
					return;
				}
			}
			else
			{
				if (this.checkBoxOnW01.Checked && this.checkBoxOnW02.Checked && this.checkBoxOnW03.Checked && this.checkBoxOnW04.Checked && this.checkBoxOnW05.Checked && this.checkBoxOnW06.Checked && this.checkBoxOnW07.Checked)
				{
					this.modifAll_only = true;
					this.checkBoxOnAll.Checked = true;
				}
			}
		}
		private void checkBoxOffAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!this.modifAll_only)
			{
				this.checkBoxOffW01.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW02.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW03.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW04.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW05.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW06.Checked = this.checkBoxOffAll.Checked;
				this.checkBoxOffW07.Checked = this.checkBoxOffAll.Checked;
			}
			this.modifAll_only = false;
		}
		private void checkBoxOffWxx_CheckedChanged(object sender, System.EventArgs e)
		{
			bool @checked = this.checkBoxOffAll.Checked;
			if (sender.Equals(this.checkBoxOffW01))
			{
				this.dtPickerOffW01.Enabled = this.checkBoxOffW01.Checked;
				@checked = this.checkBoxOffW01.Checked;
			}
			else
			{
				if (sender.Equals(this.checkBoxOffW02))
				{
					this.dtPickerOffW02.Enabled = this.checkBoxOffW02.Checked;
					@checked = this.checkBoxOffW02.Checked;
				}
				else
				{
					if (sender.Equals(this.checkBoxOffW03))
					{
						this.dtPickerOffW03.Enabled = this.checkBoxOffW03.Checked;
						@checked = this.checkBoxOffW03.Checked;
					}
					else
					{
						if (sender.Equals(this.checkBoxOffW04))
						{
							this.dtPickerOffW04.Enabled = this.checkBoxOffW04.Checked;
							@checked = this.checkBoxOffW04.Checked;
						}
						else
						{
							if (sender.Equals(this.checkBoxOffW05))
							{
								this.dtPickerOffW05.Enabled = this.checkBoxOffW05.Checked;
								@checked = this.checkBoxOffW05.Checked;
							}
							else
							{
								if (sender.Equals(this.checkBoxOffW06))
								{
									this.dtPickerOffW06.Enabled = this.checkBoxOffW06.Checked;
									@checked = this.checkBoxOffW06.Checked;
								}
								else
								{
									if (sender.Equals(this.checkBoxOffW07))
									{
										this.dtPickerOffW07.Enabled = this.checkBoxOffW07.Checked;
										@checked = this.checkBoxOffW07.Checked;
									}
								}
							}
						}
					}
				}
			}
			if (this.checkBoxOffAll.Checked)
			{
				if (!@checked)
				{
					this.modifAll_only = true;
					this.checkBoxOffAll.Checked = false;
					return;
				}
			}
			else
			{
				if (this.checkBoxOffW01.Checked && this.checkBoxOffW02.Checked && this.checkBoxOffW03.Checked && this.checkBoxOffW04.Checked && this.checkBoxOffW05.Checked && this.checkBoxOffW06.Checked && this.checkBoxOffW07.Checked)
				{
					this.modifAll_only = true;
					this.checkBoxOffAll.Checked = true;
				}
			}
		}
		private void checkBox_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GpPower_Week));
			this.tblPanelWeekly = new TableLayoutPanel();
			this.checkBoxOnW05 = new CheckBox();
			this.checkBoxOnW04 = new CheckBox();
			this.dtPickerOnW04 = new DateTimePicker();
			this.label4 = new Label();
			this.label5 = new Label();
			this.label6 = new Label();
			this.label7 = new Label();
			this.label8 = new Label();
			this.label9 = new Label();
			this.label10 = new Label();
			this.label11 = new Label();
			this.label12 = new Label();
			this.checkBoxOnAll = new CheckBox();
			this.checkBoxOnW01 = new CheckBox();
			this.checkBoxOnW02 = new CheckBox();
			this.checkBoxOnW03 = new CheckBox();
			this.checkBoxOffAll = new CheckBox();
			this.checkBoxOffW01 = new CheckBox();
			this.checkBoxOffW02 = new CheckBox();
			this.checkBoxOffW03 = new CheckBox();
			this.checkBoxOffW04 = new CheckBox();
			this.checkBoxOffW05 = new CheckBox();
			this.checkBoxOffW06 = new CheckBox();
			this.checkBoxOnW06 = new CheckBox();
			this.checkBoxOffW07 = new CheckBox();
			this.checkBoxOnW07 = new CheckBox();
			this.dtPickerOnW01 = new DateTimePicker();
			this.dtPickerOnW02 = new DateTimePicker();
			this.dtPickerOnW03 = new DateTimePicker();
			this.dtPickerOnW05 = new DateTimePicker();
			this.dtPickerOnW06 = new DateTimePicker();
			this.dtPickerOnW07 = new DateTimePicker();
			this.dtPickerOffW01 = new DateTimePicker();
			this.dtPickerOffW02 = new DateTimePicker();
			this.dtPickerOffW03 = new DateTimePicker();
			this.dtPickerOffW04 = new DateTimePicker();
			this.dtPickerOffW05 = new DateTimePicker();
			this.dtPickerOffW06 = new DateTimePicker();
			this.dtPickerOffW07 = new DateTimePicker();
			this.tblPanelWeekly.SuspendLayout();
			base.SuspendLayout();
			this.tblPanelWeekly.BackColor = Color.WhiteSmoke;
			componentResourceManager.ApplyResources(this.tblPanelWeekly, "tblPanelWeekly");
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW05, 1, 5);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW04, 1, 4);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW04, 2, 4);
			this.tblPanelWeekly.Controls.Add(this.label4, 0, 1);
			this.tblPanelWeekly.Controls.Add(this.label5, 0, 2);
			this.tblPanelWeekly.Controls.Add(this.label6, 0, 3);
			this.tblPanelWeekly.Controls.Add(this.label7, 0, 4);
			this.tblPanelWeekly.Controls.Add(this.label8, 0, 5);
			this.tblPanelWeekly.Controls.Add(this.label9, 0, 6);
			this.tblPanelWeekly.Controls.Add(this.label10, 0, 7);
			this.tblPanelWeekly.Controls.Add(this.label11, 2, 0);
			this.tblPanelWeekly.Controls.Add(this.label12, 4, 0);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnAll, 1, 0);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW01, 1, 1);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW02, 1, 2);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW03, 1, 3);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffAll, 3, 0);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW01, 3, 1);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW02, 3, 2);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW03, 3, 3);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW04, 3, 4);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW05, 3, 5);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW06, 3, 6);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW06, 1, 6);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOffW07, 3, 7);
			this.tblPanelWeekly.Controls.Add(this.checkBoxOnW07, 1, 7);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW01, 2, 1);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW02, 2, 2);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW03, 2, 3);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW05, 2, 5);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW06, 2, 6);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOnW07, 2, 7);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW01, 4, 1);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW02, 4, 2);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW03, 4, 3);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW04, 4, 4);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW05, 4, 5);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW06, 4, 6);
			this.tblPanelWeekly.Controls.Add(this.dtPickerOffW07, 4, 7);
			this.tblPanelWeekly.Name = "tblPanelWeekly";
			componentResourceManager.ApplyResources(this.checkBoxOnW05, "checkBoxOnW05");
			this.checkBoxOnW05.Name = "checkBoxOnW05";
			this.checkBoxOnW05.UseVisualStyleBackColor = true;
			this.checkBoxOnW05.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW05.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW04, "checkBoxOnW04");
			this.checkBoxOnW04.Name = "checkBoxOnW04";
			this.checkBoxOnW04.UseVisualStyleBackColor = true;
			this.checkBoxOnW04.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW04.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.dtPickerOnW04, "dtPickerOnW04");
			this.dtPickerOnW04.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW04.Name = "dtPickerOnW04";
			this.dtPickerOnW04.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.ForeColor = SystemColors.ControlText;
			this.label4.Name = "label4";
			componentResourceManager.ApplyResources(this.label5, "label5");
			this.label5.ForeColor = SystemColors.ControlText;
			this.label5.Name = "label5";
			componentResourceManager.ApplyResources(this.label6, "label6");
			this.label6.ForeColor = SystemColors.ControlText;
			this.label6.Name = "label6";
			componentResourceManager.ApplyResources(this.label7, "label7");
			this.label7.ForeColor = SystemColors.ControlText;
			this.label7.Name = "label7";
			componentResourceManager.ApplyResources(this.label8, "label8");
			this.label8.ForeColor = SystemColors.ControlText;
			this.label8.Name = "label8";
			componentResourceManager.ApplyResources(this.label9, "label9");
			this.label9.ForeColor = SystemColors.ControlText;
			this.label9.Name = "label9";
			componentResourceManager.ApplyResources(this.label10, "label10");
			this.label10.ForeColor = SystemColors.ControlText;
			this.label10.Name = "label10";
			componentResourceManager.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			componentResourceManager.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			componentResourceManager.ApplyResources(this.checkBoxOnAll, "checkBoxOnAll");
			this.checkBoxOnAll.Name = "checkBoxOnAll";
			this.checkBoxOnAll.UseVisualStyleBackColor = true;
			this.checkBoxOnAll.CheckedChanged += new System.EventHandler(this.checkBoxOnAll_CheckedChanged);
			this.checkBoxOnAll.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW01, "checkBoxOnW01");
			this.checkBoxOnW01.Name = "checkBoxOnW01";
			this.checkBoxOnW01.UseVisualStyleBackColor = true;
			this.checkBoxOnW01.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW01.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW02, "checkBoxOnW02");
			this.checkBoxOnW02.Name = "checkBoxOnW02";
			this.checkBoxOnW02.UseVisualStyleBackColor = true;
			this.checkBoxOnW02.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW02.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW03, "checkBoxOnW03");
			this.checkBoxOnW03.Name = "checkBoxOnW03";
			this.checkBoxOnW03.UseVisualStyleBackColor = true;
			this.checkBoxOnW03.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW03.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffAll, "checkBoxOffAll");
			this.checkBoxOffAll.Name = "checkBoxOffAll";
			this.checkBoxOffAll.UseVisualStyleBackColor = true;
			this.checkBoxOffAll.CheckedChanged += new System.EventHandler(this.checkBoxOffAll_CheckedChanged);
			this.checkBoxOffAll.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW01, "checkBoxOffW01");
			this.checkBoxOffW01.Name = "checkBoxOffW01";
			this.checkBoxOffW01.UseVisualStyleBackColor = true;
			this.checkBoxOffW01.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW01.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW02, "checkBoxOffW02");
			this.checkBoxOffW02.Name = "checkBoxOffW02";
			this.checkBoxOffW02.UseVisualStyleBackColor = true;
			this.checkBoxOffW02.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW02.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW03, "checkBoxOffW03");
			this.checkBoxOffW03.Name = "checkBoxOffW03";
			this.checkBoxOffW03.UseVisualStyleBackColor = true;
			this.checkBoxOffW03.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW03.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW04, "checkBoxOffW04");
			this.checkBoxOffW04.Name = "checkBoxOffW04";
			this.checkBoxOffW04.UseVisualStyleBackColor = true;
			this.checkBoxOffW04.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW04.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW05, "checkBoxOffW05");
			this.checkBoxOffW05.Name = "checkBoxOffW05";
			this.checkBoxOffW05.UseVisualStyleBackColor = true;
			this.checkBoxOffW05.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW05.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW06, "checkBoxOffW06");
			this.checkBoxOffW06.Name = "checkBoxOffW06";
			this.checkBoxOffW06.UseVisualStyleBackColor = true;
			this.checkBoxOffW06.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW06.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW06, "checkBoxOnW06");
			this.checkBoxOnW06.Name = "checkBoxOnW06";
			this.checkBoxOnW06.UseVisualStyleBackColor = true;
			this.checkBoxOnW06.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW06.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOffW07, "checkBoxOffW07");
			this.checkBoxOffW07.Name = "checkBoxOffW07";
			this.checkBoxOffW07.UseVisualStyleBackColor = true;
			this.checkBoxOffW07.CheckedChanged += new System.EventHandler(this.checkBoxOffWxx_CheckedChanged);
			this.checkBoxOffW07.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.checkBoxOnW07, "checkBoxOnW07");
			this.checkBoxOnW07.Name = "checkBoxOnW07";
			this.checkBoxOnW07.UseVisualStyleBackColor = true;
			this.checkBoxOnW07.CheckedChanged += new System.EventHandler(this.checkBoxOnWxx_CheckedChanged);
			this.checkBoxOnW07.Paint += new PaintEventHandler(this.checkBox_Paint);
			componentResourceManager.ApplyResources(this.dtPickerOnW01, "dtPickerOnW01");
			this.dtPickerOnW01.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW01.Name = "dtPickerOnW01";
			this.dtPickerOnW01.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW02, "dtPickerOnW02");
			this.dtPickerOnW02.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW02.Name = "dtPickerOnW02";
			this.dtPickerOnW02.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW03, "dtPickerOnW03");
			this.dtPickerOnW03.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW03.Name = "dtPickerOnW03";
			this.dtPickerOnW03.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW05, "dtPickerOnW05");
			this.dtPickerOnW05.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW05.Name = "dtPickerOnW05";
			this.dtPickerOnW05.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW06, "dtPickerOnW06");
			this.dtPickerOnW06.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW06.Name = "dtPickerOnW06";
			this.dtPickerOnW06.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOnW07, "dtPickerOnW07");
			this.dtPickerOnW07.Format = DateTimePickerFormat.Custom;
			this.dtPickerOnW07.Name = "dtPickerOnW07";
			this.dtPickerOnW07.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW01, "dtPickerOffW01");
			this.dtPickerOffW01.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW01.Name = "dtPickerOffW01";
			this.dtPickerOffW01.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW02, "dtPickerOffW02");
			this.dtPickerOffW02.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW02.Name = "dtPickerOffW02";
			this.dtPickerOffW02.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW03, "dtPickerOffW03");
			this.dtPickerOffW03.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW03.Name = "dtPickerOffW03";
			this.dtPickerOffW03.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW04, "dtPickerOffW04");
			this.dtPickerOffW04.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW04.Name = "dtPickerOffW04";
			this.dtPickerOffW04.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW05, "dtPickerOffW05");
			this.dtPickerOffW05.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW05.Name = "dtPickerOffW05";
			this.dtPickerOffW05.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW06, "dtPickerOffW06");
			this.dtPickerOffW06.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW06.Name = "dtPickerOffW06";
			this.dtPickerOffW06.ShowUpDown = true;
			componentResourceManager.ApplyResources(this.dtPickerOffW07, "dtPickerOffW07");
			this.dtPickerOffW07.Format = DateTimePickerFormat.Custom;
			this.dtPickerOffW07.Name = "dtPickerOffW07";
			this.dtPickerOffW07.ShowUpDown = true;
			base.AutoScaleMode = AutoScaleMode.None;
			base.Controls.Add(this.tblPanelWeekly);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "GpPower_Week";
			this.tblPanelWeekly.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
