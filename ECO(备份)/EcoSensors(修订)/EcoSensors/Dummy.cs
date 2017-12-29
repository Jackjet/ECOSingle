using EcoSensors.MainForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors
{
	public class Dummy : Form
	{
		private IContainer components;
		private Button butTest;
		private Panel panelTemp1;
		private DataGridView dataGridView1;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Column2;
		private DataGridViewTextBoxColumn Column3;
		private Label label1;
		private OpenFileDialog openFileDialog1;
		private Timer timer1;
		private SplitContainer splitContainer1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TreeView treeView1;
		private FlowLayoutPanel flowLayoutPanelDevManPage;
		private Panel panelTemp;
		public Dummy()
		{
			this.InitializeComponent();
		}
		private void butTest_Click(object sender, System.EventArgs e)
		{
            MainForm.MainForm mainForm = new MainForm.MainForm();
			mainForm.Show();
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
		}
		private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
		{
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
			TreeNode treeNode = new TreeNode("Node3");
			TreeNode treeNode2 = new TreeNode("Node4");
			TreeNode treeNode3 = new TreeNode("Node5");
			TreeNode treeNode4 = new TreeNode("Node0", new TreeNode[]
			{
				treeNode,
				treeNode2,
				treeNode3
			});
			TreeNode treeNode5 = new TreeNode("Node6");
			TreeNode treeNode6 = new TreeNode("Node7");
			TreeNode treeNode7 = new TreeNode("Node8");
			TreeNode treeNode8 = new TreeNode("Node9");
			TreeNode treeNode9 = new TreeNode("Node10");
			TreeNode treeNode10 = new TreeNode("Node1", new TreeNode[]
			{
				treeNode5,
				treeNode6,
				treeNode7,
				treeNode8,
				treeNode9
			});
			TreeNode treeNode11 = new TreeNode("Node2");
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			this.butTest = new Button();
			this.panelTemp1 = new Panel();
			this.splitContainer1 = new SplitContainer();
			this.treeView1 = new TreeView();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new TabPage();
			this.tabPage2 = new TabPage();
			this.label1 = new Label();
			this.dataGridView1 = new DataGridView();
			this.Column1 = new DataGridViewTextBoxColumn();
			this.Column2 = new DataGridViewTextBoxColumn();
			this.Column3 = new DataGridViewTextBoxColumn();
			this.openFileDialog1 = new OpenFileDialog();
			this.timer1 = new Timer(this.components);
			this.flowLayoutPanelDevManPage = new FlowLayoutPanel();
			this.panelTemp = new Panel();
			this.panelTemp1.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			((ISupportInitialize)this.dataGridView1).BeginInit();
			this.flowLayoutPanelDevManPage.SuspendLayout();
			base.SuspendLayout();
			this.butTest.Location = new Point(0, 0);
			this.butTest.Margin = new Padding(0);
			this.butTest.Name = "butTest";
			this.butTest.Size = new Size(100, 27);
			this.butTest.TabIndex = 1;
			this.butTest.Text = "Test";
			this.butTest.UseVisualStyleBackColor = true;
			this.butTest.Click += new System.EventHandler(this.butTest_Click);
			this.panelTemp1.AutoSize = true;
			this.panelTemp1.BackColor = Color.White;
			this.panelTemp1.BorderStyle = BorderStyle.FixedSingle;
			this.panelTemp1.Controls.Add(this.splitContainer1);
			this.panelTemp1.Location = new Point(184, 70);
			this.panelTemp1.Name = "panelTemp1";
			this.panelTemp1.Size = new Size(820, 548);
			this.panelTemp1.TabIndex = 0;
			this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.AutoScroll = true;
			this.splitContainer1.Panel1.BackColor = Color.Transparent;
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
			this.splitContainer1.Panel1MinSize = 0;
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
			this.splitContainer1.Panel2.Paint += new PaintEventHandler(this.splitContainer1_Panel2_Paint);
			this.splitContainer1.Panel2MinSize = 0;
			this.splitContainer1.Size = new Size(818, 546);
			this.splitContainer1.SplitterDistance = 200;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 0;
			this.treeView1.Location = new Point(23, 125);
			this.treeView1.Name = "treeView1";
			treeNode.Name = "Node3";
			treeNode.Text = "Node3";
			treeNode2.Name = "Node4";
			treeNode2.Text = "Node4";
			treeNode3.Name = "Node5";
			treeNode3.Text = "Node5";
			treeNode4.Name = "Node0";
			treeNode4.Text = "Node0";
			treeNode5.Name = "Node6";
			treeNode5.Text = "Node6";
			treeNode6.Name = "Node7";
			treeNode6.Text = "Node7";
			treeNode7.Name = "Node8";
			treeNode7.Text = "Node8";
			treeNode8.Name = "Node9";
			treeNode8.Text = "Node9";
			treeNode9.Name = "Node10";
			treeNode9.Text = "Node10";
			treeNode10.Name = "Node1";
			treeNode10.Text = "Node1";
			treeNode11.Name = "Node2";
			treeNode11.Text = "Node2";
			this.treeView1.Nodes.AddRange(new TreeNode[]
			{
				treeNode4,
				treeNode10,
				treeNode11
			});
			this.treeView1.Size = new Size(155, 264);
			this.treeView1.TabIndex = 1;
			this.tabControl1.Appearance = TabAppearance.FlatButtons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = DockStyle.Fill;
			this.tabControl1.Location = new Point(0, 0);
			this.tabControl1.Margin = new Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(198, 544);
			this.tabControl1.TabIndex = 0;
			this.tabPage1.BackColor = Color.WhiteSmoke;
			this.tabPage1.BorderStyle = BorderStyle.Fixed3D;
			this.tabPage1.Location = new Point(4, 26);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(190, 514);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage2.Location = new Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(190, 515);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.BackColor = Color.Gainsboro;
			this.label1.Location = new Point(60, 19);
			this.label1.Margin = new Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(28, 14);
			this.label1.TabIndex = 1;
			this.label1.Text = "No.";
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.dataGridView1.BackgroundColor = Color.WhiteSmoke;
			this.dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = Color.Gainsboro;
			dataGridViewCellStyle.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dataGridView1.ColumnHeadersHeight = 27;
			this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
			{
				this.Column1,
				this.Column2,
				this.Column3
			});
			this.dataGridView1.GridColor = SystemColors.ControlLight;
			this.dataGridView1.Location = new Point(52, 16);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new Size(564, 155);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellContentClick += new DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			this.Column2.HeaderText = "Column2";
			this.Column2.Name = "Column2";
			this.Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			this.Column3.HeaderText = "Column3";
			this.Column3.Name = "Column3";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.flowLayoutPanelDevManPage.BackColor = Color.FromArgb(255, 128, 0);
			this.flowLayoutPanelDevManPage.Controls.Add(this.butTest);
			this.flowLayoutPanelDevManPage.Dock = DockStyle.Fill;
			this.flowLayoutPanelDevManPage.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.flowLayoutPanelDevManPage.Location = new Point(0, 0);
			this.flowLayoutPanelDevManPage.Margin = new Padding(0);
			this.flowLayoutPanelDevManPage.MaximumSize = new Size(1180, 27);
			this.flowLayoutPanelDevManPage.MinimumSize = new Size(1016, 27);
			this.flowLayoutPanelDevManPage.Name = "flowLayoutPanelDevManPage";
			this.flowLayoutPanelDevManPage.Size = new Size(1016, 27);
			this.flowLayoutPanelDevManPage.TabIndex = 4;
			this.panelTemp.BackColor = Color.White;
			this.panelTemp.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.panelTemp.Location = new Point(0, 85);
			this.panelTemp.Margin = new Padding(0);
			this.panelTemp.MaximumSize = new Size(1016, 662);
			this.panelTemp.MinimumSize = new Size(1016, 662);
			this.panelTemp.Name = "panelTemp";
			this.panelTemp.Size = new Size(1016, 662);
			this.panelTemp.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(8f, 14f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(1016, 741);
			base.Controls.Add(this.flowLayoutPanelDevManPage);
			base.Controls.Add(this.panelTemp1);
			base.Controls.Add(this.panelTemp);
			this.Font = new Font("Verdana", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Margin = new Padding(4, 3, 4, 3);
			this.MinimumSize = new Size(1024, 768);
			base.Name = "Dummy";
			this.Text = "Dummy Fomr for Test";
			this.panelTemp1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			((ISupportInitialize)this.dataGridView1).EndInit();
			this.flowLayoutPanelDevManPage.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
