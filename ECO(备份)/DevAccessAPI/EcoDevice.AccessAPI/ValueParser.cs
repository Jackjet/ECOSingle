using CommonAPI;
using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class ValueParser
	{
		private PresentValue presentValue;
		private static string yyyyMMddHHmmss = "yyyy-MM-dd HH:mm:ss";
		public ValueParser(PresentValue presentValue)
		{
			this.presentValue = presentValue;
		}
		public ValueParser() : this(new PresentValue())
		{
		}
		public PresentValue GetPresentValue(System.Collections.Generic.List<ValueMessage> values, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, OutletMapping>> outletIdMapper, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, BankMapping>> bankIdMapper, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, SensorMapping>> sensorIdMapper, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, LineMapping>> lineIdMapper)
		{
			if (values == null || values.Count < 1)
			{
				return new PresentValue();
			}
			if (this.presentValue == null)
			{
				this.presentValue = new PresentValue();
			}
			foreach (ValueMessage current in values)
			{
				try
				{
					this.setDeviceTable(current.CreateTime, current.DeviceValue, current.DeviceID);
					this.setSensorTable(current.CreateTime, current.SensorValue, current.DeviceID, current.SensorNums, current.DeviceMac, sensorIdMapper);
					this.setOutletTable(current.CreateTime, current.OutletValue, current.DeviceID, current.PortNums, current.DeviceMac, outletIdMapper);
					this.setBankTable(current.CreateTime, current.BankValue, current.DeviceID, current.BankNums, current.DeviceMac, bankIdMapper);
					this.setLineTable(current.CreateTime, current.LineValue, current.DeviceID, current.LineNums, current.DeviceMac, lineIdMapper);
				}
				catch (System.Exception ex)
				{
					DebugCenter.GetInstance().appendToFile("Skip device: " + current.DeviceMac + ", " + ex.Message);
				}
			}
			return this.presentValue;
		}
		private void setOutletTable(System.DateTime dateTime, System.Collections.Generic.Dictionary<int, OutletValueEntry> dictionary, int deviceId, int portNumbers, string mac, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, OutletMapping>> outletIdMapper)
		{
			try
			{
				if (dictionary == null || dictionary.Count < 1)
				{
					dictionary = new System.Collections.Generic.Dictionary<int, OutletValueEntry>();
					for (int i = 1; i <= portNumbers; i++)
					{
						dictionary.Add(i, new OutletValueEntry(i));
					}
				}
				System.Collections.Generic.IEnumerator<int> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (outletIdMapper.ContainsKey(mac) && outletIdMapper[mac].ContainsKey(current))
					{
						OutletValueEntry outletValueEntry = dictionary[current];
						OutletAutoValue outletAutoValue = new OutletAutoValue();
						outletAutoValue.PortState = outletValueEntry.OutletStatus.ToString();
						OutletMapping outletMapping = outletIdMapper[mac][current];
						outletAutoValue.PortId = outletMapping.OutletId;
						outletAutoValue.Current = this.ParseDeviceValue(outletValueEntry.Current);
						outletAutoValue.Voltage = this.ParseDeviceValue(outletValueEntry.Voltage);
						outletAutoValue.Power = this.ParseDeviceValue(outletValueEntry.Power);
						outletAutoValue.PowerDissipation = this.ParseDeviceValue(outletValueEntry.PowerDissipation);
						outletAutoValue.InsertTime = this.parseSecondTime(dateTime);
						this.presentValue.OutletTable.Rows.Add(new object[]
						{
							deviceId,
							outletAutoValue.PortId,
							outletMapping.OutletNumber,
							outletMapping.OutletName,
							outletAutoValue.Voltage,
							outletAutoValue.Current,
							outletAutoValue.Power,
							outletAutoValue.PowerDissipation,
							outletAutoValue.PortState,
							outletAutoValue.InsertTime
						});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		private void setBankTable(System.DateTime dateTime, System.Collections.Generic.Dictionary<int, BankValueEntry> dictionary, int deviceId, int bankNumbers, string mac, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, BankMapping>> bankIdMapper)
		{
			try
			{
				if (dictionary == null || dictionary.Count < 1)
				{
					dictionary = new System.Collections.Generic.Dictionary<int, BankValueEntry>();
					for (int i = 1; i <= bankNumbers; i++)
					{
						dictionary.Add(i, new BankValueEntry(i));
					}
				}
				System.Collections.Generic.IEnumerator<int> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (bankIdMapper.ContainsKey(mac) && bankIdMapper[mac].ContainsKey(current))
					{
						BankValueEntry bankValueEntry = dictionary[current];
						BankAutoValue bankAutoValue = new BankAutoValue();
						bankAutoValue.BankState = bankValueEntry.BankStatus.ToString();
						BankMapping bankMapping = bankIdMapper[mac][current];
						bankAutoValue.BankId = bankMapping.BankId;
						bankAutoValue.Current = this.ParseDeviceValue(bankValueEntry.Current);
						bankAutoValue.Voltage = this.ParseDeviceValue(bankValueEntry.Voltage);
						bankAutoValue.Power = this.ParseDeviceValue(bankValueEntry.Power);
						bankAutoValue.PowerDissipation = this.ParseDeviceValue(bankValueEntry.PowerDissipation);
						bankAutoValue.InsertTime = this.parseSecondTime(dateTime);
						this.presentValue.BankTable.Rows.Add(new object[]
						{
							deviceId,
							bankAutoValue.BankId,
							bankMapping.BankNumber,
							bankMapping.BankName,
							bankAutoValue.Voltage,
							bankAutoValue.Current,
							bankAutoValue.Power,
							bankAutoValue.PowerDissipation,
							bankAutoValue.BankState,
							bankAutoValue.InsertTime
						});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		private void setLineTable(System.DateTime dateTime, System.Collections.Generic.Dictionary<int, LineValueEntry> dictionary, int deviceId, int lineNumbers, string mac, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, LineMapping>> lineIdMapper)
		{
			try
			{
				if (dictionary == null || dictionary.Count < 1)
				{
					dictionary = new System.Collections.Generic.Dictionary<int, LineValueEntry>();
					for (int i = 1; i <= lineNumbers; i++)
					{
						dictionary.Add(i, new LineValueEntry(i));
					}
				}
				System.Collections.Generic.IEnumerator<int> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (lineIdMapper.ContainsKey(mac) && lineIdMapper[mac].ContainsKey(current))
					{
						LineValueEntry lineValueEntry = dictionary[current];
						LineAutoValue lineAutoValue = new LineAutoValue();
						LineMapping lineMapping = lineIdMapper[mac][current];
						lineAutoValue.LineId = lineMapping.LineId;
						lineAutoValue.Current = this.ParseDeviceValue(lineValueEntry.Current);
						lineAutoValue.Voltage = this.ParseDeviceValue(lineValueEntry.Voltage);
						lineAutoValue.Power = this.ParseDeviceValue(lineValueEntry.Power);
						lineAutoValue.InsertTime = this.parseSecondTime(dateTime);
						this.presentValue.LineTable.Rows.Add(new object[]
						{
							deviceId,
							lineAutoValue.LineId,
							lineMapping.LineNumber,
							lineAutoValue.Voltage,
							lineAutoValue.Current,
							lineAutoValue.Power,
							lineAutoValue.InsertTime
						});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		private void setSensorTable(System.DateTime dateTime, System.Collections.Generic.Dictionary<int, SensorValueEntry> dictionary, int deviceId, int sensorNumbers, string mac, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, SensorMapping>> sensorIdMapper)
		{
			try
			{
				if (dictionary == null || dictionary.Count < 1)
				{
					dictionary = new System.Collections.Generic.Dictionary<int, SensorValueEntry>();
					for (int i = 1; i <= sensorNumbers; i++)
					{
						dictionary.Add(i, new SensorValueEntry(i));
					}
				}
				System.Collections.Generic.IEnumerator<int> enumerator = dictionary.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					if (sensorIdMapper.ContainsKey(mac) && sensorIdMapper[mac].ContainsKey(current))
					{
						SensorValueEntry sensorValueEntry = dictionary[current];
						SensorAutoValue sensorAutoValue = new SensorAutoValue();
						SensorMapping sensorMapping = sensorIdMapper[mac][current];
						int sensorLocation = sensorMapping.SensorLocation;
						sensorAutoValue.DeviceId = deviceId;
						sensorAutoValue.Humidity = this.ParseDeviceValue(sensorValueEntry.Humidity);
						sensorAutoValue.InsertTime = this.parseSecondTime(dateTime);
						sensorAutoValue.Press = this.ParseDeviceValue(sensorValueEntry.Pressure);
						sensorAutoValue.Temperature = this.ParseDeviceValue(sensorValueEntry.Temperature);
						sensorAutoValue.Type = current;
						this.presentValue.SensorTable.Rows.Add(new object[]
						{
							sensorAutoValue.DeviceId,
							sensorAutoValue.Humidity,
							sensorAutoValue.Temperature,
							sensorAutoValue.Press,
							sensorAutoValue.Type,
							sensorLocation,
							sensorAutoValue.InsertTime
						});
					}
				}
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		private void setDeviceTable(System.DateTime dateTime, DeviceValueEntry deviceValueEntry, int deviceId)
		{
			try
			{
				DeviceAutoValue deviceAutoValue = new DeviceAutoValue();
				if (deviceValueEntry == null)
				{
					deviceValueEntry = new DeviceValueEntry();
				}
				else
				{
					deviceAutoValue.DeviceState = 1;
				}
				deviceAutoValue.Current = this.ParseDeviceValue(deviceValueEntry.Current);
				deviceAutoValue.Power = this.ParseDeviceValue(deviceValueEntry.Power);
				deviceAutoValue.Voltage = this.ParseDeviceValue(deviceValueEntry.Voltage);
				deviceAutoValue.PowerDissipation = this.ParseDeviceValue(deviceValueEntry.PowerDissipation);
				deviceAutoValue.DoorSensorStatus = deviceValueEntry.DoorSensorStatus;
				deviceAutoValue.LeakCurrentStatus = deviceValueEntry.LeakCurrentStatus;
				deviceAutoValue.DeviceId = deviceId;
				deviceAutoValue.InsertTime = this.parseSecondTime(dateTime);
				this.presentValue.DeviceTable.Rows.Add(new object[]
				{
					deviceAutoValue.DeviceId,
					deviceAutoValue.Voltage,
					deviceAutoValue.Current,
					deviceAutoValue.Power,
					deviceAutoValue.PowerDissipation,
					deviceAutoValue.DeviceState,
					deviceAutoValue.DoorSensorStatus,
					deviceAutoValue.LeakCurrentStatus,
					deviceAutoValue.InsertTime
				});
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		private float ParseDeviceValue(string deviceValue)
		{
			return this.ParseDeviceValue(deviceValue, -500f);
		}
		private float ParseDeviceValue(string deviceValue, float defaultValue)
		{
			if (string.IsNullOrEmpty(deviceValue))
			{
				return defaultValue;
			}
			float result;
			try
			{
				result = float.Parse(deviceValue);
			}
			catch (System.Exception)
			{
				result = defaultValue;
			}
			return result;
		}
		private System.DateTime parseSecondTime(System.DateTime time)
		{
			return System.DateTime.Parse(time.ToString(ValueParser.yyyyMMddHHmmss));
		}
		private float getOutletLastPdValue(string ip, int outletNum, System.Collections.Generic.IDictionary<string, System.Collections.Generic.IDictionary<int, float>> lastOutletPdValue)
		{
			try
			{
				return System.Convert.ToSingle(lastOutletPdValue[ip][outletNum].ToString("0.00"));
			}
			catch (System.Exception)
			{
			}
			return 0f;
		}
		private float getDeviceLastPdValue(string ip, System.Collections.Generic.IDictionary<string, float> lastDevicePdValue)
		{
			float result;
			try
			{
				if (lastDevicePdValue == null || lastDevicePdValue.Count < 1 || !lastDevicePdValue.ContainsKey(ip))
				{
					result = 0f;
				}
				else
				{
					result = System.Convert.ToSingle(lastDevicePdValue[ip].ToString("0.00"));
				}
			}
			catch (System.Exception)
			{
				result = 0f;
			}
			return result;
		}
	}
}
