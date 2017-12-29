using MySql.Data.MySqlClient;
using System;
namespace MySql.Data.Types
{
	internal struct MySqlTimeSpan : IMySqlValue
	{
		private TimeSpan mValue;
		private bool isNull;
		public bool IsNull
		{
			get
			{
				return this.isNull;
			}
		}
		MySqlDbType IMySqlValue.MySqlDbType
		{
			get
			{
				return MySqlDbType.Time;
			}
		}
		object IMySqlValue.Value
		{
			get
			{
				return this.mValue;
			}
		}
		public TimeSpan Value
		{
			get
			{
				return this.mValue;
			}
		}
		Type IMySqlValue.SystemType
		{
			get
			{
				return typeof(TimeSpan);
			}
		}
		string IMySqlValue.MySqlTypeName
		{
			get
			{
				return "TIME";
			}
		}
		public MySqlTimeSpan(bool isNull)
		{
			this.isNull = isNull;
			this.mValue = TimeSpan.MinValue;
		}
		public MySqlTimeSpan(TimeSpan val)
		{
			this.isNull = false;
			this.mValue = val;
		}
		void IMySqlValue.WriteValue(MySqlPacket packet, bool binary, object val, int length)
		{
			if (!(val is TimeSpan))
			{
				throw new MySqlException("Only TimeSpan objects can be serialized by MySqlTimeSpan");
			}
			TimeSpan timeSpan = (TimeSpan)val;
			bool flag = timeSpan.TotalMilliseconds < 0.0;
			timeSpan = timeSpan.Duration();
			if (binary)
			{
				if (timeSpan.Milliseconds > 0)
				{
					packet.WriteByte(12);
				}
				else
				{
					packet.WriteByte(8);
				}
				packet.WriteByte(flag ? 1 : 0);
				packet.WriteInteger((long)timeSpan.Days, 4);
				packet.WriteByte((byte)timeSpan.Hours);
				packet.WriteByte((byte)timeSpan.Minutes);
				packet.WriteByte((byte)timeSpan.Seconds);
				if (timeSpan.Milliseconds > 0)
				{
					long v = (long)(timeSpan.Milliseconds * 1000);
					packet.WriteInteger(v, 4);
					return;
				}
			}
			else
			{
				string v2 = string.Format("'{0}{1} {2:00}:{3:00}:{4:00}.{5:000000}'", new object[]
				{
					flag ? "-" : "",
					timeSpan.Days,
					timeSpan.Hours,
					timeSpan.Minutes,
					timeSpan.Seconds,
					timeSpan.Milliseconds * 1000
				});
				packet.WriteStringNoNull(v2);
			}
		}
		IMySqlValue IMySqlValue.ReadValue(MySqlPacket packet, long length, bool nullVal)
		{
			if (nullVal)
			{
				return new MySqlTimeSpan(true);
			}
			if (length >= 0L)
			{
				string s = packet.ReadString(length);
				this.ParseMySql(s);
				return this;
			}
			long num = (long)((ulong)packet.ReadByte());
			int num2 = 0;
			if (num > 0L)
			{
				num2 = (int)packet.ReadByte();
			}
			this.isNull = false;
			if (num == 0L)
			{
				this.isNull = true;
			}
			else
			{
				if (num == 5L)
				{
					this.mValue = new TimeSpan(packet.ReadInteger(4), 0, 0, 0);
				}
				else
				{
					if (num == 8L)
					{
						this.mValue = new TimeSpan(packet.ReadInteger(4), (int)packet.ReadByte(), (int)packet.ReadByte(), (int)packet.ReadByte());
					}
					else
					{
						this.mValue = new TimeSpan(packet.ReadInteger(4), (int)packet.ReadByte(), (int)packet.ReadByte(), (int)packet.ReadByte(), packet.ReadInteger(4) / 1000000);
					}
				}
			}
			if (num2 == 1)
			{
				this.mValue = this.mValue.Negate();
			}
			return this;
		}
		void IMySqlValue.SkipValue(MySqlPacket packet)
		{
			int num = (int)packet.ReadByte();
			packet.Position += num;
		}
		internal static void SetDSInfo(MySqlSchemaCollection sc)
		{
			MySqlSchemaRow mySqlSchemaRow = sc.AddRow();
			mySqlSchemaRow["TypeName"] = "TIME";
			mySqlSchemaRow["ProviderDbType"] = MySqlDbType.Time;
			mySqlSchemaRow["ColumnSize"] = 0;
			mySqlSchemaRow["CreateFormat"] = "TIME";
			mySqlSchemaRow["CreateParameters"] = null;
			mySqlSchemaRow["DataType"] = "System.TimeSpan";
			mySqlSchemaRow["IsAutoincrementable"] = false;
			mySqlSchemaRow["IsBestMatch"] = true;
			mySqlSchemaRow["IsCaseSensitive"] = false;
			mySqlSchemaRow["IsFixedLength"] = true;
			mySqlSchemaRow["IsFixedPrecisionScale"] = true;
			mySqlSchemaRow["IsLong"] = false;
			mySqlSchemaRow["IsNullable"] = true;
			mySqlSchemaRow["IsSearchable"] = true;
			mySqlSchemaRow["IsSearchableWithLike"] = false;
			mySqlSchemaRow["IsUnsigned"] = false;
			mySqlSchemaRow["MaximumScale"] = 0;
			mySqlSchemaRow["MinimumScale"] = 0;
			mySqlSchemaRow["IsConcurrencyType"] = DBNull.Value;
			mySqlSchemaRow["IsLiteralSupported"] = false;
			mySqlSchemaRow["LiteralPrefix"] = null;
			mySqlSchemaRow["LiteralSuffix"] = null;
			mySqlSchemaRow["NativeDataType"] = null;
		}
		public override string ToString()
		{
			return string.Format("{0} {1:00}:{2:00}:{3:00}", new object[]
			{
				this.mValue.Days,
				this.mValue.Hours,
				this.mValue.Minutes,
				this.mValue.Seconds
			});
		}
		private void ParseMySql(string s)
		{
			string[] array = s.Split(new char[]
			{
				':',
				'.'
			});
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			int num3 = int.Parse(array[2]);
			int num4 = 0;
			if (array.Length > 3)
			{
				array[3] = array[3].PadRight(6, '0');
				num4 = int.Parse(array[3]) / 1000;
			}
			if (num < 0 || array[0].StartsWith("-", StringComparison.Ordinal))
			{
				num2 *= -1;
				num3 *= -1;
				num4 *= -1;
			}
			int num5 = num / 24;
			num -= num5 * 24;
			this.mValue = new TimeSpan(num5, num, num2, num3, num4);
			this.isNull = false;
		}
	}
}
