using CommonAPI;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace DBAccessAPI
{
	public class RackEffect
	{
		private int id;
		private float RCI_High;
		private float RHI_High;
		private float RPI_High;
		private float RAI_High;
		private float RCI_Low;
		private float RHI_Low;
		private float RPI_Low;
		private float RAI_Low;
		private float RTI;
		private float Fan_Saving;
		private float Chiller_Saving;
		private float Aggressive_Saving;
		private DateTime Insert_time;
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public DateTime InsertTime
		{
			get
			{
				return this.Insert_time;
			}
			set
			{
				this.Insert_time = value;
			}
		}
		public float AggressiveSaving
		{
			get
			{
				return this.Aggressive_Saving;
			}
			set
			{
				this.Aggressive_Saving = value;
			}
		}
		public float ChillerSaving
		{
			get
			{
				return this.Chiller_Saving;
			}
			set
			{
				this.Chiller_Saving = value;
			}
		}
		public float FanSaving
		{
			get
			{
				return this.Fan_Saving;
			}
			set
			{
				this.Fan_Saving = value;
			}
		}
		public float RTIValue
		{
			get
			{
				return this.RTI;
			}
			set
			{
				this.RTI = value;
			}
		}
		public float RAIHigh
		{
			get
			{
				return this.RAI_High;
			}
			set
			{
				this.RAI_High = value;
			}
		}
		public float RPIHigh
		{
			get
			{
				return this.RPI_High;
			}
			set
			{
				this.RPI_High = value;
			}
		}
		public float RHIHigh
		{
			get
			{
				return this.RHI_High;
			}
			set
			{
				this.RHI_High = value;
			}
		}
		public float RCIHigh
		{
			get
			{
				return this.RCI_High;
			}
			set
			{
				this.RCI_High = value;
			}
		}
		public float RAILow
		{
			get
			{
				return this.RAI_Low;
			}
			set
			{
				this.RAI_Low = value;
			}
		}
		public float RPILow
		{
			get
			{
				return this.RPI_Low;
			}
			set
			{
				this.RPI_Low = value;
			}
		}
		public float RHILow
		{
			get
			{
				return this.RHI_Low;
			}
			set
			{
				this.RHI_Low = value;
			}
		}
		public float RCILow
		{
			get
			{
				return this.RCI_Low;
			}
			set
			{
				this.RCI_Low = value;
			}
		}
		public RackEffect()
		{
			this.id = -1;
		}
		public RackEffect(float f_RCI_H, float f_RCI_L, float f_RHI_H, float f_RHI_L, float f_RPI_H, float f_RPI_L, float f_RAI_H, float f_RAI_L, float f_RTI, float f_FAN, float f_Chiller, float f_Aggressive, DateTime dt_insert)
		{
			this.id = -1;
			this.RCI_High = f_RCI_H;
			this.RHI_High = f_RHI_H;
			this.RPI_High = f_RPI_H;
			this.RAI_High = f_RAI_H;
			this.RCI_Low = f_RCI_L;
			this.RHI_Low = f_RHI_L;
			this.RPI_Low = f_RPI_L;
			this.RAI_Low = f_RAI_L;
			this.RTI = f_RTI;
			this.Fan_Saving = f_FAN;
			this.Chiller_Saving = f_Chiller;
			this.Aggressive_Saving = f_Aggressive;
			this.Insert_time = dt_insert;
		}
		public static int InsertRackEffect(string str_sql, DateTime dt_inserttime)
		{
			if (DBUrl.SERVERMODE)
			{
				WorkQueue<string>.getInstance_rackeffect().WorkSequential = true;
				WorkQueue<string>.getInstance_rackeffect().EnqueueItem(str_sql);
				return 1;
			}
			if (DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				WorkQueue<string>.getInstance_rackeffect().WorkSequential = true;
				WorkQueue<string>.getInstance_rackeffect().EnqueueItem(str_sql);
				return 1;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = null;
			try
			{
				dBConn = DBConnPool.getThermalConnection();
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
				}
				dbCommand.CommandText = str_sql.Replace("'", "#");
				dbCommand.ExecuteNonQuery();
				dbCommand.Dispose();
				dBConn.Close();
				return 1;
			}
			catch (Exception ex)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				DebugCenter.GetInstance().appendToFile("DBERROR~~~~~~~~~~~Generate RackEffect Error : " + ex.Message + "\n" + ex.StackTrace);
			}
			return -1;
		}
		public static void DeleteOverallRemainLastSomeHours(int hours, DateTime currentTime)
		{
			DateTime dateTime = currentTime.AddHours((double)(-(double)hours));
			Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
			string text = "delete from rack_effect where insert_time < #" + dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "#";
			if (DBUrl.SERVERMODE || DBUrl.DB_CURRENT_TYPE.ToUpper().Equals("MYSQL"))
			{
				WorkQueue<string>.getInstance_rackeffect().WorkSequential = true;
				WorkQueue<string>.getInstance_rackeffect().EnqueueItem(text);
				return;
			}
			DBConn dBConn = null;
			DbCommand dbCommand = new OleDbCommand();
			try
			{
				dBConn = DBConnPool.getThermalConnection();
				if (dBConn != null && dBConn.con != null)
				{
					dbCommand = dBConn.con.CreateCommand();
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = text;
					dbCommand.ExecuteNonQuery();
				}
				dbCommand.Dispose();
				dBConn.Close();
			}
			catch (Exception ex)
			{
				try
				{
					dbCommand.Dispose();
				}
				catch
				{
				}
				try
				{
					dBConn.Close();
				}
				catch
				{
				}
				DebugCenter.GetInstance().appendToFile("Delete Rack effect: " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}
}
