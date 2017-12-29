using EcoSensors.Common.component;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.DevManDCFloorGrid
{
	public class DevManFloorGrids : UserControl
	{
		private IContainer components;
		private Label label30;
		private Label label31;
		private Label label32;
		private Label label33;
		private Label label34;
		private Label label35;
		private Label label36;
		private Label label13;
		private Label label1;
		private DataGridView dgvSetDevice;
		private Label dcRlebel7;
		private Label dcRlebel5;
		private Label dcRlebel4;
		private Label dcRlebel3;
		private Label dcRlebel2;
		private Label dcRlebel1;
		private Label dcLlebel7;
		private Label dcLlebel6;
		private Label dcLlebel5;
		private Label dcLlebel4;
		private Label dcLlebel3;
		private Label dcLlebel2;
		private Label dcLlebel1;
		private Label dcBlebel10;
		private Label dcBlebel9;
		private Label dcBlebel8;
		private Label dcBlebel7;
		private Label dcBlebel6;
		private Label dcBlebel5;
		private Label dcBlebel4;
		private Label dcBlebel3;
		private Label dcBlebel1;
		private Label dcBlebel2;
		private Label dcTlebel10;
		private Label dcTlebel9;
		private Label dcTlebel8;
		private Label dcTlebel7;
		private Label dcTlebel6;
		private Label dcTlebel5;
		private Label dcTlebel4;
		private Label dcTlebel3;
		private Label dcTlebel1;
		private Label dcTlebel2;
		private Label dcRlebel6;
		private static readonly int[] columns = new int[]
		{
			45,
			72,
			90
		};
		private static readonly int[] rows = new int[]
		{
			30,
			48,
			60
		};
		private static readonly int[] pixels = new int[]
		{
			16,
			10,
			8
		};
		public static readonly string[,] lebelTxt;
		public static readonly int[,] xPos;
		public static readonly int[,] yPos;
		public static int Cur_DCRows
		{
			get
			{
				int dCLayoutType = EcoGlobalVar.DCLayoutType;
				return DevManFloorGrids.rows[dCLayoutType];
			}
		}
		public static int Cur_DCColumns
		{
			get
			{
				int dCLayoutType = EcoGlobalVar.DCLayoutType;
				return DevManFloorGrids.columns[dCLayoutType];
			}
		}
		public static int Cur_Pixel
		{
			get
			{
				int dCLayoutType = EcoGlobalVar.DCLayoutType;
				return DevManFloorGrids.pixels[dCLayoutType];
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
			this.label30 = new Label();
			this.label31 = new Label();
			this.label32 = new Label();
			this.label33 = new Label();
			this.label34 = new Label();
			this.label35 = new Label();
			this.label36 = new Label();
			this.label13 = new Label();
			this.label1 = new Label();
			this.dgvSetDevice = new DataGridView();
			this.dcRlebel7 = new Label();
			this.dcRlebel5 = new Label();
			this.dcRlebel4 = new Label();
			this.dcRlebel3 = new Label();
			this.dcRlebel2 = new Label();
			this.dcRlebel1 = new Label();
			this.dcLlebel7 = new Label();
			this.dcLlebel6 = new Label();
			this.dcLlebel5 = new Label();
			this.dcLlebel4 = new Label();
			this.dcLlebel3 = new Label();
			this.dcLlebel2 = new Label();
			this.dcLlebel1 = new Label();
			this.dcBlebel10 = new Label();
			this.dcBlebel9 = new Label();
			this.dcBlebel8 = new Label();
			this.dcBlebel7 = new Label();
			this.dcBlebel6 = new Label();
			this.dcBlebel5 = new Label();
			this.dcBlebel4 = new Label();
			this.dcBlebel3 = new Label();
			this.dcBlebel1 = new Label();
			this.dcBlebel2 = new Label();
			this.dcTlebel10 = new Label();
			this.dcTlebel9 = new Label();
			this.dcTlebel8 = new Label();
			this.dcTlebel7 = new Label();
			this.dcTlebel6 = new Label();
			this.dcTlebel5 = new Label();
			this.dcTlebel4 = new Label();
			this.dcTlebel3 = new Label();
			this.dcTlebel1 = new Label();
			this.dcTlebel2 = new Label();
			this.dcRlebel6 = new Label();
			((ISupportInitialize)this.dgvSetDevice).BeginInit();
			base.SuspendLayout();
			this.label30.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label30.ImeMode = ImeMode.NoControl;
			this.label30.Location = new Point(775, 455);
			this.label30.Margin = new Padding(1);
			this.label30.Name = "label30";
			this.label30.Size = new Size(24, 14);
			this.label30.TabIndex = 161;
			this.label30.Text = "30";
			this.label30.TextAlign = ContentAlignment.BottomCenter;
			this.label30.Visible = false;
			this.label31.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label31.ImeMode = ImeMode.NoControl;
			this.label31.Location = new Point(775, 380);
			this.label31.Margin = new Padding(1);
			this.label31.Name = "label31";
			this.label31.Size = new Size(24, 14);
			this.label31.TabIndex = 160;
			this.label31.Text = "25";
			this.label31.TextAlign = ContentAlignment.BottomCenter;
			this.label31.Visible = false;
			this.label32.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label32.ImeMode = ImeMode.NoControl;
			this.label32.Location = new Point(775, 304);
			this.label32.Margin = new Padding(1);
			this.label32.Name = "label32";
			this.label32.Size = new Size(24, 14);
			this.label32.TabIndex = 159;
			this.label32.Text = "20";
			this.label32.TextAlign = ContentAlignment.BottomCenter;
			this.label32.Visible = false;
			this.label33.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label33.ImeMode = ImeMode.NoControl;
			this.label33.Location = new Point(775, 230);
			this.label33.Margin = new Padding(1);
			this.label33.Name = "label33";
			this.label33.Size = new Size(24, 14);
			this.label33.TabIndex = 158;
			this.label33.Text = "15";
			this.label33.TextAlign = ContentAlignment.BottomCenter;
			this.label33.Visible = false;
			this.label34.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label34.ImeMode = ImeMode.NoControl;
			this.label34.Location = new Point(775, 155);
			this.label34.Margin = new Padding(1);
			this.label34.Name = "label34";
			this.label34.Size = new Size(24, 14);
			this.label34.TabIndex = 157;
			this.label34.Text = "10";
			this.label34.TextAlign = ContentAlignment.BottomCenter;
			this.label34.Visible = false;
			this.label35.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label35.ImeMode = ImeMode.NoControl;
			this.label35.Location = new Point(775, 80);
			this.label35.Margin = new Padding(1);
			this.label35.Name = "label35";
			this.label35.Size = new Size(24, 14);
			this.label35.TabIndex = 156;
			this.label35.Text = "5";
			this.label35.TextAlign = ContentAlignment.BottomCenter;
			this.label35.Visible = false;
			this.label36.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label36.ImeMode = ImeMode.NoControl;
			this.label36.Location = new Point(775, 20);
			this.label36.Margin = new Padding(1);
			this.label36.Name = "label36";
			this.label36.Size = new Size(24, 14);
			this.label36.TabIndex = 155;
			this.label36.Text = "1";
			this.label36.TextAlign = ContentAlignment.BottomCenter;
			this.label36.Visible = false;
			this.label13.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label13.ImeMode = ImeMode.NoControl;
			this.label13.Location = new Point(755, 470);
			this.label13.Margin = new Padding(1);
			this.label13.Name = "label13";
			this.label13.Size = new Size(24, 14);
			this.label13.TabIndex = 146;
			this.label13.Text = "50";
			this.label13.TextAlign = ContentAlignment.BottomCenter;
			this.label13.Visible = false;
			this.label1.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label1.ImeMode = ImeMode.NoControl;
			this.label1.Location = new Point(755, 4);
			this.label1.Margin = new Padding(1);
			this.label1.Name = "label1";
			this.label1.Size = new Size(24, 14);
			this.label1.TabIndex = 135;
			this.label1.Text = "50";
			this.label1.TextAlign = ContentAlignment.BottomCenter;
			this.label1.Visible = false;
			this.dgvSetDevice.AllowUserToAddRows = false;
			this.dgvSetDevice.AllowUserToDeleteRows = false;
			this.dgvSetDevice.AllowUserToResizeColumns = false;
			this.dgvSetDevice.AllowUserToResizeRows = false;
			this.dgvSetDevice.BackgroundColor = Color.White;
			this.dgvSetDevice.ColumnHeadersHeight = 15;
			this.dgvSetDevice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvSetDevice.ColumnHeadersVisible = false;
			this.dgvSetDevice.Location = new Point(16, 14);
			this.dgvSetDevice.Margin = new Padding(1);
			this.dgvSetDevice.Name = "dgvSetDevice";
			this.dgvSetDevice.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			this.dgvSetDevice.RowHeadersVisible = false;
			this.dgvSetDevice.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgvSetDevice.RowTemplate.Height = 15;
			this.dgvSetDevice.RowTemplate.ReadOnly = true;
			this.dgvSetDevice.ScrollBars = ScrollBars.None;
			this.dgvSetDevice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dgvSetDevice.Size = new Size(723, 483);
			this.dgvSetDevice.StandardTab = true;
			this.dgvSetDevice.TabIndex = 169;
			this.dgvSetDevice.TabStop = false;
			this.dcRlebel7.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel7.ImeMode = ImeMode.NoControl;
			this.dcRlebel7.Location = new Point(739, 483);
			this.dcRlebel7.Margin = new Padding(0);
			this.dcRlebel7.Name = "dcRlebel7";
			this.dcRlebel7.Size = new Size(19, 11);
			this.dcRlebel7.TabIndex = 203;
			this.dcRlebel7.Text = "60";
			this.dcRlebel7.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel5.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel5.ImeMode = ImeMode.NoControl;
			this.dcRlebel5.Location = new Point(739, 327);
			this.dcRlebel5.Margin = new Padding(0);
			this.dcRlebel5.Name = "dcRlebel5";
			this.dcRlebel5.Size = new Size(19, 11);
			this.dcRlebel5.TabIndex = 201;
			this.dcRlebel5.Text = "40";
			this.dcRlebel5.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel4.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel4.ImeMode = ImeMode.NoControl;
			this.dcRlebel4.Location = new Point(739, 249);
			this.dcRlebel4.Margin = new Padding(0);
			this.dcRlebel4.Name = "dcRlebel4";
			this.dcRlebel4.Size = new Size(19, 11);
			this.dcRlebel4.TabIndex = 200;
			this.dcRlebel4.Text = "30";
			this.dcRlebel4.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel3.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel3.ImeMode = ImeMode.NoControl;
			this.dcRlebel3.Location = new Point(739, 171);
			this.dcRlebel3.Margin = new Padding(0);
			this.dcRlebel3.Name = "dcRlebel3";
			this.dcRlebel3.Size = new Size(19, 11);
			this.dcRlebel3.TabIndex = 199;
			this.dcRlebel3.Text = "20";
			this.dcRlebel3.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel2.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel2.ImeMode = ImeMode.NoControl;
			this.dcRlebel2.Location = new Point(739, 93);
			this.dcRlebel2.Margin = new Padding(0);
			this.dcRlebel2.Name = "dcRlebel2";
			this.dcRlebel2.Size = new Size(19, 11);
			this.dcRlebel2.TabIndex = 198;
			this.dcRlebel2.Text = "10";
			this.dcRlebel2.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel1.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel1.ImeMode = ImeMode.NoControl;
			this.dcRlebel1.Location = new Point(739, 15);
			this.dcRlebel1.Margin = new Padding(0);
			this.dcRlebel1.Name = "dcRlebel1";
			this.dcRlebel1.Size = new Size(19, 11);
			this.dcRlebel1.TabIndex = 197;
			this.dcRlebel1.Text = "1";
			this.dcRlebel1.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel7.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel7.ImeMode = ImeMode.NoControl;
			this.dcLlebel7.Location = new Point(-3, 483);
			this.dcLlebel7.Margin = new Padding(0);
			this.dcLlebel7.Name = "dcLlebel7";
			this.dcLlebel7.Size = new Size(19, 11);
			this.dcLlebel7.TabIndex = 196;
			this.dcLlebel7.Text = "60";
			this.dcLlebel7.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel6.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel6.ImeMode = ImeMode.NoControl;
			this.dcLlebel6.Location = new Point(-3, 405);
			this.dcLlebel6.Margin = new Padding(0);
			this.dcLlebel6.Name = "dcLlebel6";
			this.dcLlebel6.Size = new Size(19, 11);
			this.dcLlebel6.TabIndex = 195;
			this.dcLlebel6.Text = "50";
			this.dcLlebel6.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel5.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel5.ImeMode = ImeMode.NoControl;
			this.dcLlebel5.Location = new Point(-3, 327);
			this.dcLlebel5.Margin = new Padding(0);
			this.dcLlebel5.Name = "dcLlebel5";
			this.dcLlebel5.Size = new Size(19, 11);
			this.dcLlebel5.TabIndex = 194;
			this.dcLlebel5.Text = "40";
			this.dcLlebel5.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel4.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel4.ImeMode = ImeMode.NoControl;
			this.dcLlebel4.Location = new Point(-3, 249);
			this.dcLlebel4.Margin = new Padding(0);
			this.dcLlebel4.Name = "dcLlebel4";
			this.dcLlebel4.Size = new Size(19, 11);
			this.dcLlebel4.TabIndex = 193;
			this.dcLlebel4.Text = "30";
			this.dcLlebel4.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel3.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel3.ImeMode = ImeMode.NoControl;
			this.dcLlebel3.Location = new Point(-3, 171);
			this.dcLlebel3.Margin = new Padding(0);
			this.dcLlebel3.Name = "dcLlebel3";
			this.dcLlebel3.Size = new Size(19, 11);
			this.dcLlebel3.TabIndex = 192;
			this.dcLlebel3.Text = "20";
			this.dcLlebel3.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel2.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel2.ImeMode = ImeMode.NoControl;
			this.dcLlebel2.Location = new Point(-3, 93);
			this.dcLlebel2.Margin = new Padding(0);
			this.dcLlebel2.Name = "dcLlebel2";
			this.dcLlebel2.Size = new Size(19, 11);
			this.dcLlebel2.TabIndex = 191;
			this.dcLlebel2.Text = "10";
			this.dcLlebel2.TextAlign = ContentAlignment.MiddleCenter;
			this.dcLlebel1.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcLlebel1.ImeMode = ImeMode.NoControl;
			this.dcLlebel1.Location = new Point(-3, 15);
			this.dcLlebel1.Margin = new Padding(0);
			this.dcLlebel1.Name = "dcLlebel1";
			this.dcLlebel1.Size = new Size(19, 11);
			this.dcLlebel1.TabIndex = 190;
			this.dcLlebel1.Text = "1";
			this.dcLlebel1.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel10.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel10.ImeMode = ImeMode.NoControl;
			this.dcBlebel10.Location = new Point(721, 498);
			this.dcBlebel10.Margin = new Padding(0);
			this.dcBlebel10.Name = "dcBlebel10";
			this.dcBlebel10.Size = new Size(19, 11);
			this.dcBlebel10.TabIndex = 189;
			this.dcBlebel10.Text = "90";
			this.dcBlebel10.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel9.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel9.ImeMode = ImeMode.NoControl;
			this.dcBlebel9.Location = new Point(642, 498);
			this.dcBlebel9.Margin = new Padding(0);
			this.dcBlebel9.Name = "dcBlebel9";
			this.dcBlebel9.Size = new Size(19, 11);
			this.dcBlebel9.TabIndex = 188;
			this.dcBlebel9.Text = "80";
			this.dcBlebel9.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel8.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel8.ImeMode = ImeMode.NoControl;
			this.dcBlebel8.Location = new Point(563, 498);
			this.dcBlebel8.Margin = new Padding(0);
			this.dcBlebel8.Name = "dcBlebel8";
			this.dcBlebel8.Size = new Size(19, 11);
			this.dcBlebel8.TabIndex = 187;
			this.dcBlebel8.Text = "70";
			this.dcBlebel8.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel7.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel7.ImeMode = ImeMode.NoControl;
			this.dcBlebel7.Location = new Point(484, 498);
			this.dcBlebel7.Margin = new Padding(0);
			this.dcBlebel7.Name = "dcBlebel7";
			this.dcBlebel7.Size = new Size(19, 11);
			this.dcBlebel7.TabIndex = 186;
			this.dcBlebel7.Text = "60";
			this.dcBlebel7.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel6.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel6.ImeMode = ImeMode.NoControl;
			this.dcBlebel6.Location = new Point(405, 498);
			this.dcBlebel6.Margin = new Padding(0);
			this.dcBlebel6.Name = "dcBlebel6";
			this.dcBlebel6.Size = new Size(19, 11);
			this.dcBlebel6.TabIndex = 185;
			this.dcBlebel6.Text = "50";
			this.dcBlebel6.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel5.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel5.ImeMode = ImeMode.NoControl;
			this.dcBlebel5.Location = new Point(326, 498);
			this.dcBlebel5.Margin = new Padding(0);
			this.dcBlebel5.Name = "dcBlebel5";
			this.dcBlebel5.Size = new Size(19, 11);
			this.dcBlebel5.TabIndex = 184;
			this.dcBlebel5.Text = "40";
			this.dcBlebel5.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel4.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel4.ImeMode = ImeMode.NoControl;
			this.dcBlebel4.Location = new Point(247, 498);
			this.dcBlebel4.Margin = new Padding(0);
			this.dcBlebel4.Name = "dcBlebel4";
			this.dcBlebel4.Size = new Size(19, 11);
			this.dcBlebel4.TabIndex = 183;
			this.dcBlebel4.Text = "30";
			this.dcBlebel4.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel3.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel3.ImeMode = ImeMode.NoControl;
			this.dcBlebel3.Location = new Point(168, 498);
			this.dcBlebel3.Margin = new Padding(0);
			this.dcBlebel3.Name = "dcBlebel3";
			this.dcBlebel3.Size = new Size(19, 11);
			this.dcBlebel3.TabIndex = 182;
			this.dcBlebel3.Text = "20";
			this.dcBlebel3.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel1.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel1.ImeMode = ImeMode.NoControl;
			this.dcBlebel1.Location = new Point(14, 498);
			this.dcBlebel1.Margin = new Padding(0);
			this.dcBlebel1.Name = "dcBlebel1";
			this.dcBlebel1.Size = new Size(19, 11);
			this.dcBlebel1.TabIndex = 180;
			this.dcBlebel1.Text = "1";
			this.dcBlebel1.TextAlign = ContentAlignment.MiddleCenter;
			this.dcBlebel2.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcBlebel2.ImeMode = ImeMode.NoControl;
			this.dcBlebel2.Location = new Point(89, 498);
			this.dcBlebel2.Margin = new Padding(0);
			this.dcBlebel2.Name = "dcBlebel2";
			this.dcBlebel2.Size = new Size(19, 11);
			this.dcBlebel2.TabIndex = 181;
			this.dcBlebel2.Text = "10";
			this.dcBlebel2.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel10.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel10.ImeMode = ImeMode.NoControl;
			this.dcTlebel10.Location = new Point(721, 1);
			this.dcTlebel10.Margin = new Padding(0);
			this.dcTlebel10.Name = "dcTlebel10";
			this.dcTlebel10.Size = new Size(19, 11);
			this.dcTlebel10.TabIndex = 179;
			this.dcTlebel10.Text = "90";
			this.dcTlebel10.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel9.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel9.ImeMode = ImeMode.NoControl;
			this.dcTlebel9.Location = new Point(642, 1);
			this.dcTlebel9.Margin = new Padding(0);
			this.dcTlebel9.Name = "dcTlebel9";
			this.dcTlebel9.Size = new Size(19, 11);
			this.dcTlebel9.TabIndex = 178;
			this.dcTlebel9.Text = "80";
			this.dcTlebel9.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel8.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel8.ImeMode = ImeMode.NoControl;
			this.dcTlebel8.Location = new Point(563, 1);
			this.dcTlebel8.Margin = new Padding(0);
			this.dcTlebel8.Name = "dcTlebel8";
			this.dcTlebel8.Size = new Size(19, 11);
			this.dcTlebel8.TabIndex = 177;
			this.dcTlebel8.Text = "70";
			this.dcTlebel8.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel7.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel7.ImeMode = ImeMode.NoControl;
			this.dcTlebel7.Location = new Point(484, 1);
			this.dcTlebel7.Margin = new Padding(0);
			this.dcTlebel7.Name = "dcTlebel7";
			this.dcTlebel7.Size = new Size(19, 11);
			this.dcTlebel7.TabIndex = 176;
			this.dcTlebel7.Text = "60";
			this.dcTlebel7.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel6.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel6.ImeMode = ImeMode.NoControl;
			this.dcTlebel6.Location = new Point(405, 1);
			this.dcTlebel6.Margin = new Padding(0);
			this.dcTlebel6.Name = "dcTlebel6";
			this.dcTlebel6.Size = new Size(19, 11);
			this.dcTlebel6.TabIndex = 175;
			this.dcTlebel6.Text = "50";
			this.dcTlebel6.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel5.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel5.ImeMode = ImeMode.NoControl;
			this.dcTlebel5.Location = new Point(326, 1);
			this.dcTlebel5.Margin = new Padding(0);
			this.dcTlebel5.Name = "dcTlebel5";
			this.dcTlebel5.Size = new Size(19, 11);
			this.dcTlebel5.TabIndex = 174;
			this.dcTlebel5.Text = "40";
			this.dcTlebel5.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel4.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel4.ImeMode = ImeMode.NoControl;
			this.dcTlebel4.Location = new Point(247, 1);
			this.dcTlebel4.Margin = new Padding(0);
			this.dcTlebel4.Name = "dcTlebel4";
			this.dcTlebel4.Size = new Size(19, 11);
			this.dcTlebel4.TabIndex = 173;
			this.dcTlebel4.Text = "30";
			this.dcTlebel4.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel3.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel3.ImeMode = ImeMode.NoControl;
			this.dcTlebel3.Location = new Point(168, 1);
			this.dcTlebel3.Margin = new Padding(0);
			this.dcTlebel3.Name = "dcTlebel3";
			this.dcTlebel3.Size = new Size(19, 11);
			this.dcTlebel3.TabIndex = 172;
			this.dcTlebel3.Text = "20";
			this.dcTlebel3.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel1.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel1.ImeMode = ImeMode.NoControl;
			this.dcTlebel1.Location = new Point(14, 1);
			this.dcTlebel1.Margin = new Padding(0);
			this.dcTlebel1.Name = "dcTlebel1";
			this.dcTlebel1.Size = new Size(19, 11);
			this.dcTlebel1.TabIndex = 170;
			this.dcTlebel1.Text = "1";
			this.dcTlebel1.TextAlign = ContentAlignment.MiddleCenter;
			this.dcTlebel2.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcTlebel2.ImeMode = ImeMode.NoControl;
			this.dcTlebel2.Location = new Point(89, 1);
			this.dcTlebel2.Margin = new Padding(0);
			this.dcTlebel2.Name = "dcTlebel2";
			this.dcTlebel2.Size = new Size(19, 11);
			this.dcTlebel2.TabIndex = 171;
			this.dcTlebel2.Text = "10";
			this.dcTlebel2.TextAlign = ContentAlignment.MiddleCenter;
			this.dcRlebel6.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.dcRlebel6.ImeMode = ImeMode.NoControl;
			this.dcRlebel6.Location = new Point(739, 405);
			this.dcRlebel6.Margin = new Padding(0);
			this.dcRlebel6.Name = "dcRlebel6";
			this.dcRlebel6.Size = new Size(19, 11);
			this.dcRlebel6.TabIndex = 202;
			this.dcRlebel6.Text = "50";
			this.dcRlebel6.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleMode = AutoScaleMode.None;
			this.BackColor = Color.WhiteSmoke;
			base.Controls.Add(this.dcRlebel7);
			base.Controls.Add(this.dcRlebel5);
			base.Controls.Add(this.dcRlebel4);
			base.Controls.Add(this.dcRlebel3);
			base.Controls.Add(this.dcRlebel2);
			base.Controls.Add(this.dcRlebel1);
			base.Controls.Add(this.dcLlebel7);
			base.Controls.Add(this.dcLlebel6);
			base.Controls.Add(this.dcLlebel5);
			base.Controls.Add(this.dcLlebel4);
			base.Controls.Add(this.dcLlebel3);
			base.Controls.Add(this.dcLlebel2);
			base.Controls.Add(this.dcLlebel1);
			base.Controls.Add(this.dcBlebel10);
			base.Controls.Add(this.dcBlebel9);
			base.Controls.Add(this.dcBlebel8);
			base.Controls.Add(this.dcBlebel7);
			base.Controls.Add(this.dcBlebel6);
			base.Controls.Add(this.dcBlebel5);
			base.Controls.Add(this.dcBlebel4);
			base.Controls.Add(this.dcBlebel3);
			base.Controls.Add(this.dcBlebel1);
			base.Controls.Add(this.dcBlebel2);
			base.Controls.Add(this.dcTlebel10);
			base.Controls.Add(this.dcTlebel9);
			base.Controls.Add(this.dcTlebel8);
			base.Controls.Add(this.dcTlebel7);
			base.Controls.Add(this.dcTlebel6);
			base.Controls.Add(this.dcTlebel5);
			base.Controls.Add(this.dcTlebel4);
			base.Controls.Add(this.dcTlebel3);
			base.Controls.Add(this.dcTlebel1);
			base.Controls.Add(this.dcTlebel2);
			base.Controls.Add(this.dcRlebel6);
			base.Controls.Add(this.dgvSetDevice);
			base.Controls.Add(this.label30);
			base.Controls.Add(this.label31);
			base.Controls.Add(this.label32);
			base.Controls.Add(this.label33);
			base.Controls.Add(this.label34);
			base.Controls.Add(this.label35);
			base.Controls.Add(this.label36);
			base.Controls.Add(this.label13);
			base.Controls.Add(this.label1);
			this.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Margin = new Padding(4, 3, 4, 3);
			base.Name = "DevManFloorGrids";
			base.Size = new Size(755, 510);
			((ISupportInitialize)this.dgvSetDevice).EndInit();
			base.ResumeLayout(false);
		}
		public DevManFloorGrids()
		{
			this.InitializeComponent();
			int dCLayoutType = EcoGlobalVar.DCLayoutType;
			for (int i = 0; i < DevManFloorGrids.columns[dCLayoutType]; i++)
			{
				DataGridViewSpanColumn dataGridViewSpanColumn = new DataGridViewSpanColumn();
				dataGridViewSpanColumn.Name = (i + 1).ToString();
				dataGridViewSpanColumn.Resizable = DataGridViewTriState.False;
				dataGridViewSpanColumn.Width = DevManFloorGrids.pixels[dCLayoutType];
				this.dgvSetDevice.Columns.Add(dataGridViewSpanColumn);
			}
		}
		public DataGridView getRackCtrl()
		{
			int num = EcoGlobalVar.DCLayoutType;
			if (num >= 3)
			{
				num = 0;
			}
			this.dgvSetDevice.Rows.Clear();
			this.dgvSetDevice.Columns.Clear();
			for (int i = 0; i < DevManFloorGrids.columns[num]; i++)
			{
				DataGridViewSpanColumn dataGridViewSpanColumn = new DataGridViewSpanColumn();
				dataGridViewSpanColumn.Name = (i + 1).ToString();
				dataGridViewSpanColumn.Resizable = DataGridViewTriState.False;
				dataGridViewSpanColumn.Width = DevManFloorGrids.pixels[num];
				this.dgvSetDevice.Columns.Add(dataGridViewSpanColumn);
			}
			for (int j = 0; j < DevManFloorGrids.rows[num]; j++)
			{
				DataGridViewRow dataGridViewRow = new DataGridViewRow();
				for (int k = 0; k < DevManFloorGrids.columns[num]; k++)
				{
					DataGridViewSpanCell dataGridViewSpanCell = new DataGridViewSpanCell();
					dataGridViewSpanCell.Tag = "";
					dataGridViewSpanCell.Value = "";
					dataGridViewRow.Cells.Add(dataGridViewSpanCell);
				}
				dataGridViewRow.Height = DevManFloorGrids.pixels[num];
				this.dgvSetDevice.Rows.Add(dataGridViewRow);
			}
			this.initLabelPosi();
			this.dgvSetDevice.ReadOnly = true;
			return this.dgvSetDevice;
		}
		private void initLabelPosi()
		{
			int dCLayoutType = EcoGlobalVar.DCLayoutType;
			this.dcTlebel1.Text = (this.dcBlebel1.Text = DevManFloorGrids.lebelTxt[0, dCLayoutType]);
			this.dcTlebel2.Text = (this.dcBlebel2.Text = DevManFloorGrids.lebelTxt[1, dCLayoutType]);
			this.dcTlebel3.Text = (this.dcBlebel3.Text = DevManFloorGrids.lebelTxt[2, dCLayoutType]);
			this.dcTlebel4.Text = (this.dcBlebel4.Text = DevManFloorGrids.lebelTxt[3, dCLayoutType]);
			this.dcTlebel5.Text = (this.dcBlebel5.Text = DevManFloorGrids.lebelTxt[4, dCLayoutType]);
			this.dcTlebel6.Text = (this.dcBlebel6.Text = DevManFloorGrids.lebelTxt[5, dCLayoutType]);
			this.dcTlebel7.Text = (this.dcBlebel7.Text = DevManFloorGrids.lebelTxt[6, dCLayoutType]);
			this.dcTlebel8.Text = (this.dcBlebel8.Text = DevManFloorGrids.lebelTxt[7, dCLayoutType]);
			this.dcTlebel9.Text = (this.dcBlebel9.Text = DevManFloorGrids.lebelTxt[8, dCLayoutType]);
			this.dcTlebel10.Text = (this.dcBlebel10.Text = DevManFloorGrids.lebelTxt[9, dCLayoutType]);
			this.dcLlebel1.Text = (this.dcRlebel1.Text = DevManFloorGrids.lebelTxt[0, dCLayoutType]);
			this.dcLlebel2.Text = (this.dcRlebel2.Text = DevManFloorGrids.lebelTxt[1, dCLayoutType]);
			this.dcLlebel3.Text = (this.dcRlebel3.Text = DevManFloorGrids.lebelTxt[2, dCLayoutType]);
			this.dcLlebel4.Text = (this.dcRlebel4.Text = DevManFloorGrids.lebelTxt[3, dCLayoutType]);
			this.dcLlebel5.Text = (this.dcRlebel5.Text = DevManFloorGrids.lebelTxt[4, dCLayoutType]);
			this.dcLlebel6.Text = (this.dcRlebel6.Text = DevManFloorGrids.lebelTxt[5, dCLayoutType]);
			this.dcLlebel7.Text = (this.dcRlebel7.Text = DevManFloorGrids.lebelTxt[6, dCLayoutType]);
			this.dcTlebel1.Left = (this.dcBlebel1.Left = DevManFloorGrids.xPos[0, dCLayoutType]);
			this.dcTlebel2.Left = (this.dcBlebel2.Left = DevManFloorGrids.xPos[1, dCLayoutType]);
			this.dcTlebel3.Left = (this.dcBlebel3.Left = DevManFloorGrids.xPos[2, dCLayoutType]);
			this.dcTlebel4.Left = (this.dcBlebel4.Left = DevManFloorGrids.xPos[3, dCLayoutType]);
			this.dcTlebel5.Left = (this.dcBlebel5.Left = DevManFloorGrids.xPos[4, dCLayoutType]);
			this.dcTlebel6.Left = (this.dcBlebel6.Left = DevManFloorGrids.xPos[5, dCLayoutType]);
			this.dcTlebel7.Left = (this.dcBlebel7.Left = DevManFloorGrids.xPos[6, dCLayoutType]);
			this.dcTlebel8.Left = (this.dcBlebel8.Left = DevManFloorGrids.xPos[7, dCLayoutType]);
			this.dcTlebel9.Left = (this.dcBlebel9.Left = DevManFloorGrids.xPos[8, dCLayoutType]);
			this.dcTlebel10.Left = (this.dcBlebel10.Left = DevManFloorGrids.xPos[9, dCLayoutType]);
			this.dcLlebel1.Top = (this.dcRlebel1.Top = DevManFloorGrids.yPos[0, dCLayoutType]);
			this.dcLlebel2.Top = (this.dcRlebel2.Top = DevManFloorGrids.yPos[1, dCLayoutType]);
			this.dcLlebel3.Top = (this.dcRlebel3.Top = DevManFloorGrids.yPos[2, dCLayoutType]);
			this.dcLlebel4.Top = (this.dcRlebel4.Top = DevManFloorGrids.yPos[3, dCLayoutType]);
			this.dcLlebel5.Top = (this.dcRlebel5.Top = DevManFloorGrids.yPos[4, dCLayoutType]);
			this.dcLlebel6.Top = (this.dcRlebel6.Top = DevManFloorGrids.yPos[5, dCLayoutType]);
			this.dcLlebel7.Top = (this.dcRlebel7.Top = DevManFloorGrids.yPos[6, dCLayoutType]);
		}
		public static int get_DCRows(int type)
		{
			return DevManFloorGrids.rows[type];
		}
		public static int get_DCColumns(int type)
		{
			return DevManFloorGrids.columns[type];
		}
		static DevManFloorGrids()
		{
			// Note: this type is marked as 'beforefieldinit'.
			string[,] array = new string[10, 3];
			array[0, 0] = "1";
			array[0, 1] = "1";
			array[0, 2] = "1";
			array[1, 0] = "5";
			array[1, 1] = "8";
			array[1, 2] = "10";
			array[2, 0] = "10";
			array[2, 1] = "16";
			array[2, 2] = "20";
			array[3, 0] = "15";
			array[3, 1] = "24";
			array[3, 2] = "30";
			array[4, 0] = "20";
			array[4, 1] = "32";
			array[4, 2] = "40";
			array[5, 0] = "25";
			array[5, 1] = "40";
			array[5, 2] = "50";
			array[6, 0] = "30";
			array[6, 1] = "48";
			array[6, 2] = "60";
			array[7, 0] = "35";
			array[7, 1] = "56";
			array[7, 2] = "70";
			array[8, 0] = "40";
			array[8, 1] = "64";
			array[8, 2] = "80";
			array[9, 0] = "45";
			array[9, 1] = "72";
			array[9, 2] = "90";
			DevManFloorGrids.lebelTxt = array;
			DevManFloorGrids.xPos = new int[,]
			{

				{
					18,
					14,
					13
				},

				{
					82,
					84,
					84
				},

				{
					161,
					164,
					165
				},

				{
					241,
					244,
					245
				},

				{
					321,
					324,
					325
				},

				{
					401,
					404,
					405
				},

				{
					481,
					484,
					485
				},

				{
					561,
					564,
					565
				},

				{
					641,
					644,
					645
				},

				{
					721,
					724,
					725
				}
			};
			DevManFloorGrids.yPos = new int[,]
			{

				{
					18,
					14,
					13
				},

				{
					82,
					84,
					84
				},

				{
					161,
					164,
					165
				},

				{
					241,
					244,
					245
				},

				{
					321,
					324,
					325
				},

				{
					401,
					404,
					405
				},

				{
					481,
					484,
					485
				}
			};
		}
	}
}
