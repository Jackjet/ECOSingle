using CommonAPI;
using CommonAPI.Global;
using CommonAPI.InterProcess;
using DBAccessAPI;
using EcoSensors._Lang;
using EcoSensors.Common;
using EcoSensors.Common.Thread;
using EventLogAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
namespace EcoSensors.SysManDB
{
	public class dbchangeDlg : Form
	{
		private delegate void dbRefresh(int i);
		public const int LostHistory = 1;
		public const int KeepHistory = 2;
		private IContainer components;
		private ProgressBar progressBar1;
		private Label label1;
		private Button butCancel;
		private RichTextBox richTextBox1;
		private bool b_timer_start;
		private bool b_cancel;
		private int last_percent = 1;
		private bool m_usemysql;
		private string m_dbIP;
		private int m_port;
		private string m_usrnm;
		private string m_psw;
		private int m_opt;
		private int m_normalclose = 1;
		private int m_retv;
		private int m_pbVal;
		private System.Timers.Timer setbarTimer;
		private System.Threading.Thread dbchangeThread;
		private int i_bar_per = 1;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(dbchangeDlg));
			this.progressBar1 = new ProgressBar();
			this.label1 = new Label();
			this.butCancel = new Button();
			this.richTextBox1 = new RichTextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.butCancel.BackColor = SystemColors.Control;
			this.butCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.butCancel, "butCancel");
			this.butCancel.Name = "butCancel";
			this.butCancel.UseVisualStyleBackColor = false;
			this.richTextBox1.BackColor = SystemColors.Control;
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.Name = "richTextBox1";
			base.AutoScaleMode = AutoScaleMode.None;
			base.CancelButton = this.butCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.ControlBox = false;
			base.Controls.Add(this.butCancel);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.progressBar1);
			base.Controls.Add(this.richTextBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "dbchangeDlg";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.dbchangeDlg_FormClosing);
			base.ResumeLayout(false);
		}
		public dbchangeDlg()
		{
			this.InitializeComponent();
			this.progressBar1.Visible = true;
			this.progressBar1.Minimum = 1;
			this.progressBar1.Maximum = 100;
			this.progressBar1.Value = 1;
			this.progressBar1.Step = 1;
			this.setbarTimer = new System.Timers.Timer();
			this.setbarTimer.Elapsed += new ElapsedEventHandler(this.setbarTimer_Elapsed);
			this.setbarTimer.Interval = 1000.0;
			this.setbarTimer.Start();
			this.dbchangeThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.startDBchange));
			this.dbchangeThread.IsBackground = true;
		}
		public dbchangeDlg(bool usemysql, string dbIP, int port, string usrnm, string psw, int opt)
		{
			this.InitializeComponent();
			this.m_usemysql = usemysql;
			this.m_dbIP = dbIP;
			this.m_port = port;
			this.m_usrnm = usrnm;
			this.m_psw = psw;
			this.m_opt = opt;
			this.progressBar1.Visible = true;
			this.progressBar1.Minimum = 1;
			this.progressBar1.Maximum = 100;
			this.m_pbVal = 1;
			this.progressBar1.Value = this.m_pbVal;
			this.progressBar1.Step = 1;
			AccessDBUpdate.i_percent = 1;
			this.butCancel.Enabled = false;
			this.setbarTimer = new System.Timers.Timer();
			this.setbarTimer.Elapsed += new ElapsedEventHandler(this.setbarTimer_Elapsed);
			this.setbarTimer.Interval = 1000.0;
			this.setbarTimer.Start();
			this.dbchangeThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.startDBchange));
			this.dbchangeThread.IsBackground = true;
			this.dbchangeThread.Start();
		}
		public void setbarTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (this.b_timer_start)
			{
				this.i_bar_per = AccessDBUpdate.i_percent;
				if (this.i_bar_per > this.last_percent)
				{
					this.last_percent = this.i_bar_per;
					if (this.i_bar_per == 100)
					{
						this.b_timer_start = false;
					}
					if (base.InvokeRequired)
					{
						base.Invoke(new dbchangeDlg.dbRefresh(this.setbar), new object[]
						{
							this.i_bar_per
						});
						return;
					}
					this.setbar(this.i_bar_per);
					return;
				}
			}
			else
			{
				this.last_percent = 1;
			}
		}
		private void startDBchange()
		{
			try
			{
				System.Threading.Thread.Sleep(500);
				EcoGlobalVar.ECOAppRunStatus = 2;
				Program.StopService(EcoGlobalVar.gl_ServiceName, 30000);
				System.Threading.Thread.Sleep(500);
				if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL") && this.m_usemysql)
				{
					this.m_retv = DBUrl.updatesetting(this.m_port, this.m_usrnm, this.m_psw);
				}
				else
				{
					this.m_retv = this.updatesetting(this.m_usemysql, this.m_dbIP, this.m_port, this.m_usrnm, this.m_psw);
				}
				string valuePair = ValuePairs.getValuePair("Username");
				if (!string.IsNullOrEmpty(valuePair))
				{
					LogAPI.writeEventLog("0130020", new string[]
					{
						valuePair
					});
				}
				else
				{
					LogAPI.writeEventLog("0130020", new string[0]);
				}
				InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
				if (!this.b_cancel)
				{
					Program.StartService(null);
				}
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					if (!this.b_cancel)
					{
						if (this.m_retv != DebugCenter.ST_Success)
						{
							EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
						}
						else
						{
							EcoMessageBox.ShowInfo(this, EcoLanguage.getMsg(LangRes.OPsucc, new string[0]));
						}
						this.m_normalclose = 2;
					}
					base.Close();
				};
				ControlAccess controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this, null);
			}
			catch (System.Exception)
			{
				if (this.m_normalclose == 1)
				{
					ControlAccess.ConfigControl config2 = delegate(Control control, object param)
					{
						EcoMessageBox.ShowError(this, EcoLanguage.getMsg(LangRes.OPfail, new string[0]));
						this.m_normalclose = 2;
						base.Close();
					};
					ControlAccess controlAccess2 = new ControlAccess(this, config2);
					controlAccess2.Access(this, null);
				}
			}
		}
		private void dbchangeDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.m_normalclose == 1)
			{
				if (this.butCancel.Enabled)
				{
					this.b_cancel = true;
					AccessDBUpdate.b_cancel_flag = true;
					this.m_normalclose = 3;
					InterProcessShared<System.Collections.Generic.Dictionary<string, string>>.setInterProcessKeyValue("ServiceStDBMaintain", "DBMaintainFinish");
					Program.StartService(null);
					return;
				}
				e.Cancel = true;
			}
		}
		private void setbar2(int i)
		{
		}
		private int updatesetting(bool b_mysqlenable, string str_dbip, int i_port, string str_usr, string str_pwd)
		{
			this.setbar(1);
			string arg_11_0 = System.AppDomain.CurrentDomain.BaseDirectory;
			if (!b_mysqlenable)
			{
				this.setbar(1);
				AccessDBUpdate.showOnDBChg = new AccessDBUpdate.Delegateshowbar(this.setbar2);
				if (this.m_opt == 2)
				{
					ControlAccess.ConfigControl config = delegate(Control control, object param)
					{
						dbchangeDlg dbchangeDlg = control as dbchangeDlg;
						dbchangeDlg.butCancel.Enabled = true;
					};
					ControlAccess controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
					this.b_timer_start = true;
					AccessDBUpdate.InitAccessDataDB();
					ExpiryCheck.SyncExpiredDate();
					DebugCenter.GetInstance().appendToFile("DBChange History MySQL->Access start...");
					System.DateTime now = System.DateTime.Now;
					int num = AccessDBUpdate.ExportDataDB2Access();
					System.DateTime now2 = System.DateTime.Now;
					System.TimeSpan timeSpan = now2 - now;
					DebugCenter.GetInstance().appendToFile("DBChange History MySQL->Access spend:" + timeSpan.TotalMilliseconds);
					this.b_timer_start = false;
					try
					{
						this.setbarTimer.Stop();
						this.setbarTimer.Dispose();
					}
					catch
					{
					}
					config = delegate(Control control, object param)
					{
						dbchangeDlg dbchangeDlg = control as dbchangeDlg;
						dbchangeDlg.butCancel.Enabled = false;
					};
					controlAccess = new ControlAccess(this, config);
					controlAccess.Access(this, null);
					if (num < 0)
					{
						return -4;
					}
				}
				else
				{
					AccessDBUpdate.InitAccessDataDB();
					if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
					{
						DBTools.DropMySQLDatabase(DBUrl.DB_CURRENT_NAME, DBUrl.CURRENT_HOST_PATH, DBUrl.CURRENT_PORT, DBUrl.CURRENT_USER_NAME, DBUrl.CURRENT_PWD);
					}
				}
				string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
				OleDbCommand oleDbCommand = new OleDbCommand();
				using (OleDbConnection oleDbConnection = new OleDbConnection(connectionString))
				{
					try
					{
						oleDbConnection.Open();
						oleDbCommand.Connection = oleDbConnection;
						oleDbCommand.CommandType = CommandType.Text;
						oleDbCommand.CommandText = "update dbsource set db_type='ACCESS',db_name='datadb',host_path='datadb.mdb',port= 0,user_name = 'root',pwd='root' where active_flag = 2 ";
						int num2 = oleDbCommand.ExecuteNonQuery();
						if (num2 < 0)
						{
							int result = -5;
							return result;
						}
					}
					catch (System.Exception)
					{
						int result = -5;
						return result;
					}
					finally
					{
						oleDbCommand.Dispose();
					}
				}
				DBUrl.initconfig();
				this.setbar(100);
				return DebugCenter.ST_Success;
			}
			int num3 = DBTools.InitMySQLDatabase("127.0.0.1", i_port, str_usr, str_pwd);
			if (num3 < 0)
			{
				return -2;
			}
			if (this.m_opt == 2)
			{
				ControlAccess.ConfigControl config2 = delegate(Control control, object param)
				{
					dbchangeDlg dbchangeDlg = control as dbchangeDlg;
					dbchangeDlg.butCancel.Enabled = true;
				};
				ControlAccess controlAccess2 = new ControlAccess(this, config2);
				controlAccess2.Access(this, null);
				this.b_timer_start = true;
				ExpiryCheck.SyncExpiredDate();
				DebugCenter.GetInstance().appendToFile("DBChange History Access->MySQL start...");
				System.DateTime now3 = System.DateTime.Now;
				int num4 = AccessDBUpdate.ExportDataDB2MySQL("eco", "127.0.0.1", i_port, str_usr, str_pwd);
				System.DateTime now4 = System.DateTime.Now;
				System.TimeSpan timeSpan2 = now4 - now3;
				DebugCenter.GetInstance().appendToFile("DBChange History Access->MySQL spend:" + timeSpan2.TotalMilliseconds);
				this.b_timer_start = false;
				try
				{
					this.setbarTimer.Stop();
					this.setbarTimer.Dispose();
				}
				catch
				{
				}
				config2 = delegate(Control control, object param)
				{
					dbchangeDlg dbchangeDlg = control as dbchangeDlg;
					dbchangeDlg.butCancel.Enabled = false;
				};
				controlAccess2 = new ControlAccess(this, config2);
				controlAccess2.Access(this, null);
				if (num4 < 0)
				{
					return -4;
				}
			}
			string connectionString2 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "sysdb.mdb;Jet OLEDB:Database Password=^tenec0Sensor";
			OleDbCommand oleDbCommand2 = new OleDbCommand();
			using (OleDbConnection oleDbConnection2 = new OleDbConnection(connectionString2))
			{
				try
				{
					oleDbConnection2.Open();
					oleDbCommand2.Connection = oleDbConnection2;
					oleDbCommand2.CommandType = CommandType.Text;
					oleDbCommand2.CommandText = string.Concat(new object[]
					{
						"update dbsource set db_type='MYSQL',db_name='eco',host_path='127.0.0.1',port= ",
						i_port,
						",user_name = '",
						str_usr,
						"',pwd='",
						str_pwd,
						"' where active_flag = 2 "
					});
					int num5 = oleDbCommand2.ExecuteNonQuery();
					if (num5 < 0)
					{
						int result = -5;
						return result;
					}
				}
				catch (System.Exception)
				{
					int result = -5;
					return result;
				}
				finally
				{
					oleDbCommand2.Dispose();
				}
			}
			DBUrl.initconfig();
			this.setbar(100);
			return DebugCenter.ST_Success;
		}
		private void showbar(ref int i_count)
		{
			i_count++;
			if (i_count > 500)
			{
				ControlAccess.ConfigControl config = delegate(Control control, object param)
				{
					this.m_pbVal++;
					if (this.m_pbVal >= 100)
					{
						this.m_pbVal = 1;
					}
					this.progressBar1.Value = this.m_pbVal;
				};
				ControlAccess controlAccess = new ControlAccess(this, config);
				controlAccess.Access(this, null);
				i_count = 0;
			}
		}
		private void showbar()
		{
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				this.m_pbVal++;
				if (this.m_pbVal >= 100)
				{
					this.m_pbVal = 1;
				}
				this.progressBar1.Value = this.m_pbVal;
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
		}
		private void setbar(int v)
		{
			ControlAccess.ConfigControl config = delegate(Control control, object param)
			{
				this.m_pbVal = v;
				this.progressBar1.Value = this.m_pbVal;
			};
			ControlAccess controlAccess = new ControlAccess(this, config);
			controlAccess.Access(this, null);
		}
	}
}
