using System;
using System.Data;
namespace EcoDevice.AccessAPI
{
	public class PresentValue
	{
		private DataTable deviceTable = new DataTable("device");
		private DataTable sensorTable = new DataTable("sensor");
		private DataTable outletTable = new DataTable("outlet");
		private DataTable bankTable = new DataTable("bank");
		private DataTable lineTable = new DataTable("line");
		public DataTable DeviceTable
		{
			get
			{
				return this.deviceTable;
			}
			set
			{
				this.deviceTable = value;
			}
		}
		public DataTable SensorTable
		{
			get
			{
				return this.sensorTable;
			}
			set
			{
				this.sensorTable = value;
			}
		}
		public DataTable OutletTable
		{
			get
			{
				return this.outletTable;
			}
			set
			{
				this.outletTable = value;
			}
		}
		public DataTable BankTable
		{
			get
			{
				return this.bankTable;
			}
			set
			{
				this.bankTable = value;
			}
		}
		public DataTable LineTable
		{
			get
			{
				return this.lineTable;
			}
			set
			{
				this.lineTable = value;
			}
		}
		public PresentValue Clone()
		{
			return new PresentValue
			{
				deviceTable = this.deviceTable.Copy(),
				sensorTable = this.sensorTable.Copy(),
				outletTable = this.outletTable.Copy(),
				bankTable = this.bankTable.Copy(),
				lineTable = this.lineTable.Copy()
			};
		}
		public PresentValue()
		{
			this.initialDeviceTable();
			this.initialSensorTable();
			this.initialOutletTable();
			this.initialBankTable();
			this.initialLineTable();
		}
		private void initialOutletTable()
		{
			this.outletTable.Columns.Add("device_id", typeof(int));
			this.outletTable.Columns.Add("port_id", typeof(int));
			this.outletTable.Columns.Add("port_number", typeof(int));
			this.outletTable.Columns.Add("port_nm", typeof(string));
			this.outletTable.Columns.Add("voltage_value", typeof(float));
			this.outletTable.Columns.Add("current_value", typeof(float));
			this.outletTable.Columns.Add("power_value", typeof(float));
			this.outletTable.Columns.Add("power_consumption", typeof(float));
			this.outletTable.Columns.Add("port_state", typeof(string));
			this.outletTable.Columns.Add("insert_time", typeof(System.DateTime));
		}
		private void initialSensorTable()
		{
			this.sensorTable.Columns.Add("device_id", typeof(int));
			this.sensorTable.Columns.Add("humidity", typeof(float));
			this.sensorTable.Columns.Add("temperature", typeof(float));
			this.sensorTable.Columns.Add("press_value", typeof(float));
			this.sensorTable.Columns.Add("sensor_type", typeof(short));
			this.sensorTable.Columns.Add("sensor_location", typeof(short));
			this.sensorTable.Columns.Add("insert_time", typeof(System.DateTime));
		}
		private void initialDeviceTable()
		{
			this.deviceTable.Columns.Add("device_id", typeof(int));
			this.deviceTable.Columns.Add("voltage_value", typeof(float));
			this.deviceTable.Columns.Add("current_value", typeof(float));
			this.deviceTable.Columns.Add("power_value", typeof(float));
			this.deviceTable.Columns.Add("power_consumption", typeof(float));
			this.deviceTable.Columns.Add("device_state", typeof(int));
			this.deviceTable.Columns.Add("doorsensor_status", typeof(short));
			this.deviceTable.Columns.Add("leakcurrent_status", typeof(short));
			this.deviceTable.Columns.Add("insert_time", typeof(System.DateTime));
		}
		private void initialBankTable()
		{
			this.bankTable.Columns.Add("device_id", typeof(int));
			this.bankTable.Columns.Add("bank_id", typeof(int));
			this.bankTable.Columns.Add("bank_number", typeof(int));
			this.bankTable.Columns.Add("bank_nm", typeof(string));
			this.bankTable.Columns.Add("voltage_value", typeof(float));
			this.bankTable.Columns.Add("current_value", typeof(float));
			this.bankTable.Columns.Add("power_value", typeof(float));
			this.bankTable.Columns.Add("power_consumption", typeof(float));
			this.bankTable.Columns.Add("bank_state", typeof(string));
			this.bankTable.Columns.Add("insert_time", typeof(System.DateTime));
		}
		private void initialLineTable()
		{
			this.lineTable.Columns.Add("device_id", typeof(int));
			this.lineTable.Columns.Add("line_id", typeof(int));
			this.lineTable.Columns.Add("line_number", typeof(int));
			this.lineTable.Columns.Add("voltage_value", typeof(float));
			this.lineTable.Columns.Add("current_value", typeof(float));
			this.lineTable.Columns.Add("power_value", typeof(float));
			this.lineTable.Columns.Add("insert_time", typeof(System.DateTime));
		}
		public float getDeviceVoltage(int deviceID, float voltage)
		{
			float result = voltage;
			if (deviceID < 0)
			{
				DataRow[] array = this.outletTable.Select("port_id > 0");
				if (array != null && array.Length > 0)
				{
					int num = array.Length;
					System.Random random = new System.Random((int)System.DateTime.Now.Ticks);
					int num2 = random.Next(0, num - 1);
					int num3 = System.Convert.ToInt32(array[num2]["device_id"]);
					DataRow[] array2 = this.deviceTable.Select("device_id=" + num3);
					if (array2 != null && array2.Length > 0)
					{
						result = System.Convert.ToSingle(array2[0]["voltage_value"]);
					}
				}
			}
			else
			{
				DataRow[] array3 = this.deviceTable.Select("device_id=" + deviceID);
				if (array3 != null && array3.Length > 0)
				{
					result = System.Convert.ToSingle(array3[0]["voltage_value"]);
				}
			}
			return result;
		}
		public bool isDevOnline(int deviceID)
		{
			DataRow[] array = this.deviceTable.Select("device_id=" + deviceID);
			return array != null && array.Length > 0;
		}
	}
}
