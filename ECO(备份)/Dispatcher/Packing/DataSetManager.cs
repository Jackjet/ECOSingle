using CommonAPI;
using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoMessenger;
using ecoProtocols;
using SessionManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
namespace Packing
{
	public class DataSetManager
	{
		public enum OutletStatus
		{
			OFF = 1,
			ON,
			Pending,
			Fault,
			NoAuth,
			NA,
			POP
		}
		public enum BankStatus
		{
			OFF = 1,
			ON,
			Reboot
		}
		private struct ColumnDefine
		{
			public string colName;
			public string colType;
			public ColumnDefine(string name, string type)
			{
				this.colName = name;
				this.colType = type;
			}
		}
		public const string _strDelimiter = "|";
		public const string tb_Device2Rack = "Device2Rack";
		public const string tb_RackInfo = "RackInfo";
		public const string tb_UserUAC = "UserUAC";
		public const string tb_Device = "device";
		public const string tb_Sensor = "sensor";
		public const string tb_Port = "port";
		public const string tb_Bank = "bank";
		public const string tb_Line = "line";
		public const string tb_Delete = "add_delete";
		public const string tb_dyDevice = "dy_device";
		public const string tb_dySensor = "dy_sensor";
		public const string tb_dyPort = "dy_port";
		public const string tb_dyBank = "dy_bank";
		public const string tb_dyLine = "dy_line";
		public const string tb_dyDelete = "dy_add_delete";
		public const string tb_rtDevice = "rt_device";
		public const string tb_rtSensor = "rt_sensor";
		public const string tb_rtPort = "rt_port";
		public const string tb_rtBank = "rt_bank";
		public const string tb_rtLine = "rt_line";
		public const string tb_rackid = "rack_id";
		public const string tb_racknm = "rack_nm";
		public const string tb_devID = "device_id";
		public const string tb_lineID = "line_id";
		public const string tb_authPorts = "authorized_ports";
		public const string tb_Vvol = "voltage_value";
		public const string tb_Vcurrent = "current_value";
		public const string tb_Vpower = "power_value";
		public const string tb_VpowerD = "power_consumption";
		public const string tb_devSt = "device_state";
		public const string tb_devDoorSt = "doorsensor_status";
		public const string tb_Vhumi = "humidity";
		public const string tb_Vtempt = "temperature";
		public const string tb_Vpress = "press_value";
		public const string tb_bankNo = "bank_number";
		public const string tb_banknm = "bank_nm";
		public const string tb_bankSt = "bank_state";
		public const string tb_devIP = "device_ip";
		public const string tb_devnm = "device_nm";
		public const string tb_devmodel = "model_nm";
		public const string tb_devFWVer = "fw_version";
		public const string tb_maxvol = "max_voltage";
		public const string tb_minvol = "min_voltage";
		public const string tb_maxpowerD = "max_power_diss";
		public const string tb_minpowerD = "min_power_diss";
		public const string tb_maxpower = "max_power";
		public const string tb_minpower = "min_power";
		public const string tb_maxcurrent = "max_current";
		public const string tb_mincurrent = "min_current";
		public const string tb_ssID = "sensor_id";
		public const string tb_ssnm = "sensor_nm";
		public const string tb_maxhumi = "max_humidity";
		public const string tb_minhumi = "min_humidity";
		public const string tb_maxtempt = "max_temperature";
		public const string tb_mintempt = "min_temperature";
		public const string tb_maxpress = "max_press";
		public const string tb_minpress = "min_press";
		public const string tb_ssNo = "sensor_type";
		public const string tb_ssLoct = "sensor_location";
		public const string tb_portID = "port_id";
		public const string tb_portNo = "port_number";
		public const string tb_portnm = "port_nm";
		public const string tb_portSt = "port_state";
		public const string tb_bankID = "bank_id";
		public const string tb_colRackInfoSx = "rack_sx";
		public const string tb_colRackInfoSy = "rack_sy";
		public const string tb_colRackInfoEx = "rack_ex";
		public const string tb_colRackInfoEy = "rack_ey";
		public const string tb_colRackInfoDevice = "rack_device";
		public const string tb_colRackInfoReserved = "rack_reserved";
		public const string tb_ValuePair = "rt_value_pair";
		public const string tb_keyName = "key_name";
		public const string tb_keyValue = "key_value";
		public const string tb_keyMaxZoneNum = "MaxZoneNum";
		public const string tb_keyMaxRackNum = "MaxRackNum";
		public const string tb_keyMaxDevNum = "MaxDevNum";
		public const string tb_keySupportISG = "SupportISG";
		public const string tb_keySupportBP = "SupportBP";
		public const string KEY_PueISG = "PUE_ISG";
		public const string KEY_AtenPDU = "ATEN_PDU";
		public const string KEY_EnablePowerOP = "ENABLE_POWER_OP";
		public const string KEY_TempUnit = "TempUnit";
		public const string KEY_CurCurrency = "CurCurrency";
		public const string KEY_co2kg = "co2kg";
		public const string KEY_RackFullNameFlag = "RackFullNameFlag";
		public const string tb_HeatLoad = "HeatLoad";
		public const string col_HeatKey = "HeatKey";
		public const string col_HeatValue = "HeatValue";
		public const string tb_SysParam = "SysParam";
		public const string col_SysKey = "SysKey";
		public const string col_SysValue = "SysValue";
		public static int PDUData_FreshFLG_Yes = 1;
		public static int PDUData_FreshFLG_NO = 2;
		public static int PDUData_Doorst_close = 0;
		public static int PDUData_Doorst_open = 1;
		public static int PDUData_Doorst_noattach = 2;
		public static string ValueEmpty = "-1000";
		public static string tb_udDevice = "ud_device";
		public static string tb_udSensor = "ud_sensor";
		public static string tb_udPort = "ud_port";
		public static string tb_udBank = "ud_bank";
		public static string tb_udLine = "ud_line";
		public static object _lockRack = new object();
		public static ArrayList _LastAllRacks = null;
		private static long _table_sequence = 0L;
		private static readonly DataSetManager.ColumnDefine[] colDevice = new DataSetManager.ColumnDefine[]
		{
			new DataSetManager.ColumnDefine("device_id", "System.Int32"),
			new DataSetManager.ColumnDefine("device_ip", "System.String"),
			new DataSetManager.ColumnDefine("device_nm", "System.String"),
			new DataSetManager.ColumnDefine("model_nm", "System.String"),
			new DataSetManager.ColumnDefine("fw_version", "System.String"),
			new DataSetManager.ColumnDefine("max_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("min_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("max_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("min_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("max_power", "System.Single"),
			new DataSetManager.ColumnDefine("min_power", "System.Single"),
			new DataSetManager.ColumnDefine("max_current", "System.Single"),
			new DataSetManager.ColumnDefine("min_current", "System.Single"),
			new DataSetManager.ColumnDefine("rack_id", "System.Int32")
		};
		private static readonly DataSetManager.ColumnDefine[] colSensor = new DataSetManager.ColumnDefine[]
		{
			new DataSetManager.ColumnDefine("sensor_id", "System.Int32"),
			new DataSetManager.ColumnDefine("device_id", "System.Int32"),
			new DataSetManager.ColumnDefine("sensor_nm", "System.String"),
			new DataSetManager.ColumnDefine("max_humidity", "System.Single"),
			new DataSetManager.ColumnDefine("min_humidity", "System.Single"),
			new DataSetManager.ColumnDefine("max_temperature", "System.Single"),
			new DataSetManager.ColumnDefine("min_temperature", "System.Single"),
			new DataSetManager.ColumnDefine("max_press", "System.Single"),
			new DataSetManager.ColumnDefine("min_press", "System.Single"),
			new DataSetManager.ColumnDefine("sensor_type", "System.Int16"),
			new DataSetManager.ColumnDefine("sensor_location", "System.Int16")
		};
		private static readonly DataSetManager.ColumnDefine[] colPort = new DataSetManager.ColumnDefine[]
		{
			new DataSetManager.ColumnDefine("port_id", "System.Int32"),
			new DataSetManager.ColumnDefine("device_id", "System.Int32"),
			new DataSetManager.ColumnDefine("port_number", "System.Int32"),
			new DataSetManager.ColumnDefine("port_nm", "System.String"),
			new DataSetManager.ColumnDefine("max_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("min_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("max_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("min_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("max_power", "System.Single"),
			new DataSetManager.ColumnDefine("min_power", "System.Single"),
			new DataSetManager.ColumnDefine("max_current", "System.Single"),
			new DataSetManager.ColumnDefine("min_current", "System.Single")
		};
		private static readonly DataSetManager.ColumnDefine[] colBank = new DataSetManager.ColumnDefine[]
		{
			new DataSetManager.ColumnDefine("bank_id", "System.Int32"),
			new DataSetManager.ColumnDefine("device_id", "System.Int32"),
			new DataSetManager.ColumnDefine("bank_number", "System.String"),
			new DataSetManager.ColumnDefine("bank_nm", "System.String"),
			new DataSetManager.ColumnDefine("max_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("min_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("max_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("min_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("max_power", "System.Single"),
			new DataSetManager.ColumnDefine("min_power", "System.Single"),
			new DataSetManager.ColumnDefine("max_current", "System.Single"),
			new DataSetManager.ColumnDefine("min_current", "System.Single")
		};
		private static readonly DataSetManager.ColumnDefine[] colLine = new DataSetManager.ColumnDefine[]
		{
			new DataSetManager.ColumnDefine("line_id", "System.Int32"),
			new DataSetManager.ColumnDefine("device_id", "System.Int32"),
			new DataSetManager.ColumnDefine("line_number", "System.Int32"),
			new DataSetManager.ColumnDefine("line_name", "System.String"),
			new DataSetManager.ColumnDefine("max_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("min_voltage", "System.Single"),
			new DataSetManager.ColumnDefine("max_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("min_power_diss", "System.Single"),
			new DataSetManager.ColumnDefine("max_power", "System.Single"),
			new DataSetManager.ColumnDefine("min_power", "System.Single"),
			new DataSetManager.ColumnDefine("max_current", "System.Single"),
			new DataSetManager.ColumnDefine("min_current", "System.Single")
		};
		public static void CultureTest()
		{
		}
		public static void DumperTest()
		{
		}
		public static void WriteFile(string filename, string separator_line, string log)
		{
			StreamWriter streamWriter;
			if (!File.Exists(filename))
			{
				streamWriter = File.CreateText(filename);
			}
			else
			{
				streamWriter = File.AppendText(filename);
			}
			streamWriter.Write("{0}\r\n", separator_line);
			streamWriter.Write("{0}", log);
			streamWriter.Close();
		}
		public static Dictionary<string, DevModelConfig> getAutoModelList()
		{
			Dictionary<string, DevModelConfig> dictionary = new Dictionary<string, DevModelConfig>();
			List<Dictionary<string, string>> deviceDefine = DeviceOperation.GetDeviceDefine();
			foreach (Dictionary<string, string> current in deviceDefine)
			{
				string text = current["modelname"];
				string text2 = current["version"];
				string text3 = current["basic"];
				string text4 = current["extra"];
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "";
					}
					if (string.IsNullOrEmpty(text4))
					{
						text4 = "";
					}
					string key = text + "-" + text2;
					dictionary[key] = new DevModelConfig
					{
						modelName = text,
						firmwareVer = text2,
						autoBasicInfo = text3,
						autoRatingInfo = text4
					};
				}
			}
			return dictionary;
		}
		public static void WriteStreamByAutoModel(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			long tLast = (long)Environment.TickCount;
			Dictionary<string, DevModelConfig> autoModelList = DataSetManager.getAutoModelList();
			string text = "null";
			if (autoModelList == null)
			{
				text = "null";
			}
			else
			{
				if (autoModelList.Count == 0)
				{
					text = "empty";
				}
				else
				{
					text = autoModelList.Count.ToString();
				}
			}
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"AUTOMODEL_update",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}{3}{4}{5}{6}\n", new object[]
			{
				"modelName",
				"|",
				"firmwareVer",
				"|",
				"autoBasicInfo",
				"|",
				"autoRatingInfo"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("String{0}String{1}String{2}String\n", "|", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}2{1}0{2}0\n", "|", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}\n", "|", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			foreach (KeyValuePair<string, DevModelConfig> current in autoModelList)
			{
				s = string.Format("{0}{1}{2}{3}{4}{5}{6}\n", new object[]
				{
					current.Value.modelName,
					"|",
					current.Value.firmwareVer,
					"|",
					current.Value.autoBasicInfo,
					"|",
					current.Value.autoRatingInfo
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			Common.WriteLine("    Auto Model Elapsed: {0}, items={1}", new string[]
			{
				Common.ElapsedTime(tLast).ToString(),
				text
			});
		}
		public static void WriteZoneInfo(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			long tLast = (long)Environment.TickCount;
			ArrayList allZone = ZoneInfo.getAllZone();
			Common.WriteLine("XXXXXXXXXXXXXXXXXXXXX Read All Zone: elapsed={0}", new string[]
			{
				Common.ElapsedTime(tLast).ToString()
			});
			string text;
			if (allZone == null)
			{
				text = "null";
			}
			else
			{
				if (allZone.Count == 0)
				{
					text = "empty";
				}
				else
				{
					text = allZone.Count.ToString();
				}
			}
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"ZONE_INFO_ZoneInfo",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
			{
				"id",
				"|",
				"zone_nm",
				"|",
				"racks",
				"|",
				"sx",
				"|",
				"sy",
				"|",
				"ex",
				"|",
				"ey",
				"|",
				"color"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("Int32{0}String{1}String{2}Int32{3}Int32{4}Int32{5}Int32{6}String\n", new object[]
			{
				"|",
				"|",
				"|",
				"|",
				"|",
				"|",
				"|"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}0{1}0{2}0{3}0{4}0{5}0{6}0\n", new object[]
			{
				"|",
				"|",
				"|",
				"|",
				"|",
				"|",
				"|"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}{3}{4}{5}{6}\n", new object[]
			{
				"|",
				"|",
				"|",
				"|",
				"|",
				"|",
				"|"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			for (int i = 0; i < allZone.Count; i++)
			{
				ZoneInfo zoneInfo = (ZoneInfo)allZone[i];
				string text2 = zoneInfo.RackInfo;
				if (text2.IndexOf("|") == -1)
				{
					if (uac != null && uac._authZoneList != null)
					{
						if (!uac._authZoneList.ContainsKey(zoneInfo.ZoneID))
						{
							goto IL_43E;
						}
						text2 = uac.getAuthorized("rack", text2);
					}
					s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
					{
						zoneInfo.ZoneID,
						"|",
						zoneInfo.ZoneName,
						"|",
						(text2 == null) ? "" : text2,
						"|",
						zoneInfo.StartPointX,
						"|",
						zoneInfo.StartPointY,
						"|",
						zoneInfo.EndPointX,
						"|",
						zoneInfo.EndPointY,
						"|",
						zoneInfo.ZoneColor
					});
					bytes = Encoding.UTF8.GetBytes(s);
					tblStream.Write(bytes, 0, bytes.Length);
				}
				IL_43E:;
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static void WriteGroupInfo(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			string s;
			byte[] bytes;
			lock (DataSetManager._lockRack)
			{
				long tLast = (long)Environment.TickCount;
				List<GroupInfo> allGroup = GroupInfo.GetAllGroup();
				Common.WriteLine("XXXXXXXXXXXXXXXXXXXXX Read All Group: elapsed={0}", new string[]
				{
					Common.ElapsedTime(tLast).ToString()
				});
				string text;
				if (allGroup == null)
				{
					text = "null";
				}
				else
				{
					if (allGroup.Count == 0)
					{
						text = "empty";
					}
					else
					{
						text = allGroup.Count.ToString();
					}
				}
				s = string.Format("######{0}{1}{2}{3}\n", new object[]
				{
					"|",
					"GROUP_INFO_GroupInfo",
					"|",
					text
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
				{
					"id",
					"|",
					"groupname",
					"|",
					"grouptype",
					"|",
					"linecolor",
					"|",
					"isselect",
					"|",
					"thermalflag",
					"|",
					"billflag",
					"|",
					"members"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("Int32{0}String{1}String{2}String{3}Int32{4}Int32{5}Int32{6}String\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("1{0}0{1}0{2}0{3}0{4}0{5}0{6}0\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("{0}{1}{2}{3}{4}{5}{6}\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = "-\n";
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = "\n";
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				for (int i = 0; i < allGroup.Count; i++)
				{
					GroupInfo groupInfo = allGroup[i];
					string text2 = groupInfo.Members;
					if (text2 == null)
					{
						text2 = "";
					}
					if (text2.IndexOf("|") == -1)
					{
						if (uac != null && uac._authGroupList != null)
						{
							if (!uac._authGroupList.ContainsKey(groupInfo.ID))
							{
								goto IL_50B;
							}
							if (groupInfo.GroupType == "zone")
							{
								text2 = uac.getAuthorized("zone", text2);
							}
							else
							{
								if (groupInfo.GroupType == "rack" || groupInfo.GroupType == "allrack")
								{
									text2 = uac.getAuthorized("rack", text2);
								}
								else
								{
									if (groupInfo.GroupType == "dev" || groupInfo.GroupType == "alldev")
									{
										text2 = uac.getAuthorized("device", text2);
									}
									else
									{
										if (groupInfo.GroupType == "outlet" || groupInfo.GroupType == "alloutlet")
										{
											text2 = uac.getAuthorized("port", text2);
										}
									}
								}
							}
						}
						s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
						{
							groupInfo.ID,
							"|",
							groupInfo.GroupName,
							"|",
							groupInfo.GroupType,
							"|",
							groupInfo.LineColor,
							"|",
							groupInfo.SelectedFlag,
							"|",
							groupInfo.ThermalFlag,
							"|",
							groupInfo.BillFlag,
							"|",
							(text2 == null) ? "" : text2
						});
						bytes = Encoding.UTF8.GetBytes(s);
						tblStream.Write(bytes, 0, bytes.Length);
					}
					IL_50B:;
				}
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static void WriteSystemSettings(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			string text = "9";
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"SYS_PARAM_SysParam",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}\n", "SysKey", "|", "SysValue");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("String{0}String\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}0\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int num = DevAccessCfg.GetInstance().getmaxZoneNum();
			s = string.Format("{0}{1}{2}\n", "MaxZoneNum", "|", num);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int num2 = DevAccessCfg.GetInstance().getmaxRackNum();
			s = string.Format("{0}{1}{2}\n", "MaxRackNum", "|", num2);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int num3 = DevAccessCfg.GetInstance().getmaxDevNum();
			s = string.Format("{0}{1}{2}\n", "MaxDevNum", "|", num3);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			bool flag = DevAccessCfg.GetInstance().isISGsupport();
			s = string.Format("{0}{1}{2}\n", "SupportISG", "|", flag ? 1 : 0);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			bool flag2 = DevAccessCfg.GetInstance().isBillprotsupport();
			s = string.Format("{0}{1}{2}\n", "SupportBP", "|", flag2 ? 1 : 0);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int temperatureUnit = Sys_Para.GetTemperatureUnit();
			s = string.Format("{0}{1}{2}\n", "TempUnit", "|", temperatureUnit);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			string currency = Sys_Para.GetCurrency();
			s = string.Format("{0}{1}{2}\n", "CurCurrency", "|", currency);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}\n", "co2kg", "|", Sys_Para.GetCO2KG().ToString("F2"));
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int rackFullNameflag = Sys_Para.GetRackFullNameflag();
			s = string.Format("{0}{1}{2}\n", "RackFullNameFlag", "|", rackFullNameflag);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int iSGFlag = Sys_Para.GetISGFlag();
			s = string.Format("{0}{1}{2}\n", "PUE_ISG", "|", iSGFlag);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int iTPowerFlag = Sys_Para.GetITPowerFlag();
			s = string.Format("{0}{1}{2}\n", "ATEN_PDU", "|", iTPowerFlag);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			int num4 = Sys_Para.GetEnablePowerControlFlag() ? 1 : 0;
			s = string.Format("{0}{1}{2}\n", "ENABLE_POWER_OP", "|", num4);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			string text2 = EcoHandler._macMismatchList;
			text2 = text2.Replace(";", "+");
			text2 = text2.Replace(",", "_");
			s = string.Format("{0}{1}{2}\n", "MAC_CONFLICT", "|", text2);
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static void WritePueValuePairs(double[] pue, string[] sTime, MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			string text = "8";
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"rt_value_pair",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}\n", "key_name", "|", "key_value");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("String{0}String\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}0\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			if (pue != null)
			{
				s = string.Format("CurrentIT{0}{1}\n", "|", pue[0]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_CurrentIT{0}{1}\n", "|", sTime[0]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("CurrentTotal{0}{1}\n", "|", pue[1]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_CurrentTotal{0}{1}\n", "|", sTime[1]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("HourIT{0}{1}\n", "|", pue[2]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_HourIT{0}{1}\n", "|", sTime[2]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("HourTotal{0}{1}\n", "|", pue[3]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_HourTotal{0}{1}\n", "|", sTime[3]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("DayIT{0}{1}\n", "|", pue[4]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_DayIT{0}{1}\n", "|", sTime[4]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("DayTotal{0}{1}\n", "|", pue[5]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_DayTotal{0}{1}\n", "|", sTime[5]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("WeekIT{0}{1}\n", "|", pue[6]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_WeekIT{0}{1}\n", "|", sTime[6]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("WeekTotal{0}{1}\n", "|", pue[7]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("T_WeekTotal{0}{1}\n", "|", sTime[7]);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static ArrayList getCurRacks()
		{
			ArrayList result;
			lock (DataSetManager._lockRack)
			{
				result = DeepCloneHelper.DeepClone<ArrayList>(DataSetManager._LastAllRacks);
			}
			return result;
		}
		public static void WriteRackInfo(int dcLayout, MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			lock (DataSetManager._lockRack)
			{
				long tLast = (long)Environment.TickCount;
				DataSetManager._LastAllRacks = RackInfo.GetAllRack_NoEmpty();
				Common.WriteLine("XXXXXXXXXXXXXXXXXXXXX Read All Rack: elapsed={0}", new string[]
				{
					Common.ElapsedTime(tLast).ToString()
				});
				string text;
				if (DataSetManager._LastAllRacks == null)
				{
					text = "null";
				}
				else
				{
					if (DataSetManager._LastAllRacks.Count == 0)
					{
						text = "empty";
					}
					else
					{
						text = DataSetManager._LastAllRacks.Count.ToString();
					}
				}
				string s = string.Format("######{0}{1}{2}{3}{4}{5}\n", new object[]
				{
					"|",
					"RackInfo",
					"|",
					text,
					"|",
					dcLayout
				});
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
				{
					"rack_id",
					"|",
					"rack_nm",
					"|",
					"rack_sx",
					"|",
					"rack_sy",
					"|",
					"rack_ex",
					"|",
					"rack_ey",
					"|",
					"rack_device",
					"|",
					"rack_reserved"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("UInt32{0}String{1}Int32{2}Int32{3}Int32{4}Int32{5}String{6}String\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("1{0}0{1}0{2}0{3}0{4}0{5}0{6}0\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = string.Format("{0}{1}{2}{3}{4}{5}{6}\n", new object[]
				{
					"|",
					"|",
					"|",
					"|",
					"|",
					"|",
					"|"
				});
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = "-\n";
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				s = "\n";
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				for (int i = 0; i < DataSetManager._LastAllRacks.Count; i++)
				{
					RackInfo rackInfo = (RackInfo)DataSetManager._LastAllRacks[i];
					string text2 = rackInfo.DeviceInfo;
					if (text2.IndexOf("|") == -1)
					{
						if (uac != null && uac._authRackDeviceList != null)
						{
							if (!uac._authRackDeviceList.ContainsKey(rackInfo.RackID))
							{
								goto IL_46B;
							}
							text2 = uac.getAuthorized("device", text2);
						}
						s = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}\n", new object[]
						{
							rackInfo.RackID,
							"|",
							rackInfo.OriginalName,
							"|",
							rackInfo.StartPoint_X,
							"|",
							rackInfo.StartPoint_Y,
							"|",
							rackInfo.EndPoint_X,
							"|",
							rackInfo.EndPoint_Y,
							"|",
							(text2 == null) ? "" : text2,
							"|",
							rackInfo.RackFullName
						});
						bytes = Encoding.UTF8.GetBytes(s);
						tblStream.Write(bytes, 0, bytes.Length);
					}
					IL_46B:;
				}
				s = "\n";
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
			}
		}
		public static void WriteDevice2Rack(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			long tLast = (long)Environment.TickCount;
			Dictionary<long, CommParaClass> deviceRackMapping = DeviceOperation.GetDeviceRackMapping();
			Common.WriteLine("XXXXXXXXXXXXXXXXXXXXX Read All RackMap: elapsed={0}", new string[]
			{
				Common.ElapsedTime(tLast).ToString()
			});
			string text;
			if (deviceRackMapping == null)
			{
				text = "null";
			}
			else
			{
				if (deviceRackMapping.Count == 0)
				{
					text = "empty";
				}
				else
				{
					text = deviceRackMapping.Count.ToString();
				}
			}
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"Device2Rack",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}{3}{4}\n", new object[]
			{
				"device_id",
				"|",
				"rack_id",
				"|",
				"rack_nm"
			});
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("UInt32{0}UInt32{1}String\n", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}0{1}0\n", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("0{0}0{1}0\n", "|", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			foreach (KeyValuePair<long, CommParaClass> current in deviceRackMapping)
			{
				if (uac == null || uac._authDevicePortList == null || uac._authDevicePortList.ContainsKey(current.Key))
				{
					s = string.Format("{0}{1}{2}{3}{4}\n", new object[]
					{
						current.Key,
						"|",
						current.Value.Long_First,
						"|",
						current.Value.String_First
					});
					bytes = Encoding.UTF8.GetBytes(s);
					tblStream.Write(bytes, 0, bytes.Length);
				}
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static void WriteUserUAC(MyMemoryStream tblStream, UserAccessRights uac = null)
		{
			int arg_05_0 = Environment.TickCount;
			Common.WriteLine("XXXXXXXXXXXXXXXXXXXXX Read UAC", new string[0]);
			string text = "0";
			if (uac != null && uac._authDevicePortList.Count > 0)
			{
				text = (1 + uac._authDevicePortList.Count).ToString();
			}
			string s = string.Format("######{0}{1}{2}{3}\n", new object[]
			{
				"|",
				"UserUAC",
				"|",
				text
			});
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("{0}{1}{2}\n", "device_id", "|", "authorized_ports");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("UInt32{0}String\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("1{0}0\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = string.Format("0{0}\n", "|");
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "-\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
			if (uac != null)
			{
				string arg = string.Format("UserType:{0};DeviceList:{1};GroupList:{2}", uac._usrerType, uac._DeviceList, uac._GroupList);
				s = string.Format("0{0}{1}\n", "|", arg);
				bytes = Encoding.UTF8.GetBytes(s);
				tblStream.Write(bytes, 0, bytes.Length);
				if (uac._authDevicePortList.Count > 0)
				{
					foreach (KeyValuePair<long, List<long>> current in uac._authDevicePortList)
					{
						StringBuilder stringBuilder = new StringBuilder();
						foreach (long current2 in current.Value)
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(",");
							}
							stringBuilder.Append(current2.ToString());
						}
						s = string.Format("{0}{1}{2}\n", current.Key, "|", stringBuilder.ToString());
						bytes = Encoding.UTF8.GetBytes(s);
						tblStream.Write(bytes, 0, bytes.Length);
					}
				}
			}
			s = "\n";
			bytes = Encoding.UTF8.GetBytes(s);
			tblStream.Write(bytes, 0, bytes.Length);
		}
		public static void WriteTableInfo(string tblName, DataTable tbl, bool bWithEndLine, MyMemoryStream tblStream)
		{
			try
			{
				string text;
				if (tbl == null)
				{
					text = "null";
				}
				else
				{
					if (tbl.Columns.Count == 0)
					{
						text = "nocolumn";
					}
					else
					{
						if (tbl.Rows.Count == 0)
						{
							text = "empty";
						}
						else
						{
							text = tbl.Rows.Count.ToString();
						}
					}
				}
				Interlocked.Increment(ref DataSetManager._table_sequence);
				string text2 = string.Format("######{0}{1}{2}{3}{4}{5}\n", new object[]
				{
					"|",
					tblName,
					"|",
					text,
					"|",
					DataSetManager._table_sequence
				});
				byte[] bytes = Encoding.UTF8.GetBytes(text2);
				tblStream.Write(bytes, 0, bytes.Length);
				if (tbl == null || tbl.Columns.Count == 0)
				{
					text2 = "\n";
					bytes = Encoding.UTF8.GetBytes(text2);
					tblStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					text2 = "";
					foreach (DataColumn dataColumn in tbl.Columns)
					{
						text2 += "|";
						text2 += dataColumn.ColumnName;
					}
					if (text2 != "")
					{
						text2 = text2.Substring("|".Length);
					}
					text2 += "\n";
					bytes = Encoding.UTF8.GetBytes(text2);
					tblStream.Write(bytes, 0, bytes.Length);
					text2 = "";
					foreach (DataColumn dataColumn2 in tbl.Columns)
					{
						text2 += "|";
						text2 += dataColumn2.DataType;
					}
					if (text2 != "")
					{
						text2 = text2.Substring("|".Length);
					}
					text2 += "\n";
					text2 = text2.Replace("System.", "");
					text2 = text2.Replace("Int16", "Int32");
					bytes = Encoding.UTF8.GetBytes(text2);
					tblStream.Write(bytes, 0, bytes.Length);
					text2 = "";
					List<string> list = new List<string>();
					for (int i = 0; i < tbl.PrimaryKey.Length; i++)
					{
						list.Add(tbl.PrimaryKey[i].ColumnName);
					}
					foreach (DataColumn dataColumn3 in tbl.Columns)
					{
						text2 += "|";
						string str = "0";
						for (int j = 0; j < list.Count; j++)
						{
							if (dataColumn3.ColumnName.Equals(list[j], StringComparison.CurrentCultureIgnoreCase))
							{
								str = string.Format("{0}", j + 1);
								break;
							}
						}
						text2 += str;
					}
					if (text2 != "")
					{
						text2 = text2.Substring("|".Length);
					}
					text2 += "\n";
					bytes = Encoding.UTF8.GetBytes(text2);
					tblStream.Write(bytes, 0, bytes.Length);
					text2 = "";
					foreach (DataColumn dataColumn4 in tbl.Columns)
					{
						text2 += "|";
						text2 += dataColumn4.DefaultValue;
					}
					if (text2 != "")
					{
						text2 = text2.Substring("|".Length);
					}
					text2 += "\n";
					bytes = Encoding.UTF8.GetBytes(text2);
					tblStream.Write(bytes, 0, bytes.Length);
					if (bWithEndLine)
					{
						text2 = "-\n";
						bytes = Encoding.UTF8.GetBytes(text2);
						tblStream.Write(bytes, 0, bytes.Length);
						text2 = "\n";
						bytes = Encoding.UTF8.GetBytes(text2);
						tblStream.Write(bytes, 0, bytes.Length);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("WriteTableInfo: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		public static void WriteTableData(DataTable tbl, int fromRow, int nRowCount, MyMemoryStream tblStream)
		{
			try
			{
				if (tbl == null || tbl.Columns.Count == 0)
				{
					string text = "\n";
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					tblStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					int num = nRowCount;
					if (nRowCount <= 0)
					{
						fromRow = 0;
						num = tbl.Rows.Count;
					}
					num = Math.Min(num, tbl.Rows.Count - fromRow);
					for (int i = fromRow; i < fromRow + num; i++)
					{
						string text = "";
						for (int j = 0; j < tbl.Columns.Count; j++)
						{
							text += "|";
							if (tbl.Columns[j].DataType == Type.GetType("System.DateTime"))
							{
								text += ((DateTime)tbl.Rows[i].ItemArray[j]).ToString("yyyy-MM-dd HH:mm:ss");
							}
							else
							{
								text += tbl.Rows[i].ItemArray[j].ToString();
							}
						}
						if (text != "")
						{
							text = text.Substring("|".Length);
						}
						text += "\n";
						byte[] bytes = Encoding.UTF8.GetBytes(text);
						tblStream.Write(bytes, 0, bytes.Length);
					}
					if (nRowCount == 0 || fromRow + nRowCount >= tbl.Rows.Count)
					{
						string text = "\n";
						byte[] bytes = Encoding.UTF8.GetBytes(text);
						tblStream.Write(bytes, 0, bytes.Length);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("WriteTableData: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		public static DataSet FastCreateDataset(List<string> affectedDevices, Dictionary<string, List<string>> affectedSensors, Dictionary<string, List<string>> affectedPorts, Dictionary<string, List<string>> affectedBanks, Dictionary<string, List<string>> affectedLines, UserAccessRights uac = null)
		{
			DataSet dataSet = new DataSet();
			int num = 0;
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "device";
				DataSetManager.ColumnDefine[] array = DataSetManager.colDevice;
				for (int i = 0; i < array.Length; i++)
				{
					DataSetManager.ColumnDefine columnDefine = array[i];
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType(columnDefine.colType);
					dataColumn.ColumnName = columnDefine.colName;
					dataTable.Columns.Add(dataColumn);
				}
				dataTable.PrimaryKey = new DataColumn[]
				{
					dataTable.Columns["device_id"]
				};
				dataSet.Tables.Add(dataTable);
				Hashtable deviceCache = DBCache.GetDeviceCache();
				if (deviceCache != null && deviceCache.Count > 0)
				{
					if (affectedDevices == null)
					{
						ICollection values = deviceCache.Values;
						IEnumerator enumerator = values.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DeviceInfo deviceInfo = (DeviceInfo)enumerator.Current;
								if (uac == null || uac._authDevicePortList.ContainsKey((long)deviceInfo.DeviceID))
								{
									DataRow dataRow = dataSet.Tables[num].NewRow();
									dataRow["device_id"] = deviceInfo.DeviceID;
									dataRow["device_ip"] = deviceInfo.DeviceIP;
									dataRow["device_nm"] = deviceInfo.DeviceName;
									dataRow["model_nm"] = deviceInfo.ModelNm;
									dataRow["fw_version"] = deviceInfo.FWVersion;
									dataRow["max_voltage"] = deviceInfo.Max_voltage;
									dataRow["min_voltage"] = deviceInfo.Min_voltage;
									dataRow["max_power_diss"] = deviceInfo.Max_power_diss;
									dataRow["min_power_diss"] = deviceInfo.Min_power_diss;
									dataRow["max_power"] = deviceInfo.Max_power;
									dataRow["min_power"] = deviceInfo.Min_power;
									dataRow["max_current"] = deviceInfo.Max_current;
									dataRow["min_current"] = deviceInfo.Min_current;
									dataRow["rack_id"] = deviceInfo.RackID;
									dataSet.Tables[num].Rows.Add(dataRow);
								}
							}
							goto IL_47C;
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					foreach (string current in affectedDevices)
					{
						if (deviceCache.ContainsKey(Convert.ToInt32(current)))
						{
							DeviceInfo deviceInfo2 = (DeviceInfo)deviceCache[Convert.ToInt32(current)];
							if (uac == null || uac._authDevicePortList.ContainsKey((long)deviceInfo2.DeviceID))
							{
								DataRow dataRow2 = dataSet.Tables[num].NewRow();
								dataRow2["device_id"] = deviceInfo2.DeviceID;
								dataRow2["device_ip"] = deviceInfo2.DeviceIP;
								dataRow2["device_nm"] = deviceInfo2.DeviceName;
								dataRow2["model_nm"] = deviceInfo2.ModelNm;
								dataRow2["fw_version"] = deviceInfo2.FWVersion;
								dataRow2["max_voltage"] = deviceInfo2.Max_voltage;
								dataRow2["min_voltage"] = deviceInfo2.Min_voltage;
								dataRow2["max_power_diss"] = deviceInfo2.Max_power_diss;
								dataRow2["min_power_diss"] = deviceInfo2.Min_power_diss;
								dataRow2["max_power"] = deviceInfo2.Max_power;
								dataRow2["min_power"] = deviceInfo2.Min_power;
								dataRow2["max_current"] = deviceInfo2.Max_current;
								dataRow2["min_current"] = deviceInfo2.Min_current;
								dataRow2["rack_id"] = deviceInfo2.RackID;
								dataSet.Tables[num].Rows.Add(dataRow2);
							}
						}
					}
				}
				IL_47C:;
			}
			catch (Exception ex)
			{
				Common.WriteLine("FastCreateDataset-device: " + ex.StackTrace, new string[0]);
			}
			num++;
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "sensor";
				DataSetManager.ColumnDefine[] array = DataSetManager.colSensor;
				for (int i = 0; i < array.Length; i++)
				{
					DataSetManager.ColumnDefine columnDefine2 = array[i];
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType(columnDefine2.colType);
					dataColumn.ColumnName = columnDefine2.colName;
					dataTable.Columns.Add(dataColumn);
				}
				dataTable.PrimaryKey = new DataColumn[]
				{
					dataTable.Columns["device_id"],
					dataTable.Columns["sensor_type"]
				};
				dataSet.Tables.Add(dataTable);
				Hashtable sensorCache = DBCache.GetSensorCache();
				if (sensorCache != null && sensorCache.Count > 0)
				{
					if (affectedSensors == null)
					{
						ICollection values2 = sensorCache.Values;
						IEnumerator enumerator = values2.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								SensorInfo sensorInfo = (SensorInfo)enumerator.Current;
								if (uac == null)
								{
									DataRow dataRow3 = dataSet.Tables[num].NewRow();
									dataRow3["sensor_id"] = sensorInfo.ID;
									dataRow3["device_id"] = sensorInfo.Device_ID;
									dataRow3["sensor_nm"] = sensorInfo.SensorName;
									dataRow3["max_humidity"] = sensorInfo.Max_humidity;
									dataRow3["min_humidity"] = sensorInfo.Min_humidity;
									dataRow3["max_temperature"] = sensorInfo.Max_temperature;
									dataRow3["min_temperature"] = sensorInfo.Min_temperature;
									dataRow3["max_press"] = sensorInfo.Max_press;
									dataRow3["min_press"] = sensorInfo.Min_press;
									dataRow3["sensor_type"] = sensorInfo.Type;
									dataRow3["sensor_location"] = sensorInfo.Location;
									dataSet.Tables[num].Rows.Add(dataRow3);
								}
							}
							goto IL_9A6;
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					Hashtable deviceSensorMap = DBCache.GetDeviceSensorMap();
					if (deviceSensorMap != null && deviceSensorMap.Count > 0)
					{
						List<int> list = new List<int>();
						foreach (KeyValuePair<string, List<string>> current2 in affectedSensors)
						{
							int num2 = Convert.ToInt32(current2.Key);
							if (uac == null)
							{
								List<string> value = current2.Value;
								if (value.Contains("*"))
								{
									if (!deviceSensorMap.ContainsKey(num2))
									{
										continue;
									}
									List<int> list2 = (List<int>)deviceSensorMap[num2];
									using (List<int>.Enumerator enumerator4 = list2.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											int current3 = enumerator4.Current;
											list.Add(current3);
										}
										continue;
									}
								}
								foreach (string current4 in value)
								{
									list.Add(Convert.ToInt32(current4));
								}
							}
						}
						foreach (int current5 in list)
						{
							SensorInfo sensorInfo2 = (SensorInfo)sensorCache[current5];
							if (sensorInfo2 != null && (uac == null || uac._authDevicePortList.ContainsKey((long)sensorInfo2.Device_ID)))
							{
								DataRow dataRow4 = dataSet.Tables[num].NewRow();
								dataRow4["sensor_id"] = sensorInfo2.ID;
								dataRow4["device_id"] = sensorInfo2.Device_ID;
								dataRow4["sensor_nm"] = sensorInfo2.SensorName;
								dataRow4["max_humidity"] = sensorInfo2.Max_humidity;
								dataRow4["min_humidity"] = sensorInfo2.Min_humidity;
								dataRow4["max_temperature"] = sensorInfo2.Max_temperature;
								dataRow4["min_temperature"] = sensorInfo2.Min_temperature;
								dataRow4["max_press"] = sensorInfo2.Max_press;
								dataRow4["min_press"] = sensorInfo2.Min_press;
								dataRow4["sensor_type"] = sensorInfo2.Type;
								dataRow4["sensor_location"] = sensorInfo2.Location;
								dataSet.Tables[num].Rows.Add(dataRow4);
							}
						}
					}
				}
				IL_9A6:;
			}
			catch (Exception ex2)
			{
				Common.WriteLine("FastCreateDataset-sensor: " + ex2.StackTrace, new string[0]);
			}
			num++;
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "port";
				DataSetManager.ColumnDefine[] array = DataSetManager.colPort;
				for (int i = 0; i < array.Length; i++)
				{
					DataSetManager.ColumnDefine columnDefine3 = array[i];
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType(columnDefine3.colType);
					dataColumn.ColumnName = columnDefine3.colName;
					dataTable.Columns.Add(dataColumn);
				}
				dataTable.PrimaryKey = new DataColumn[]
				{
					dataTable.Columns["device_id"],
					dataTable.Columns["port_number"],
					dataTable.Columns["port_id"]
				};
				dataSet.Tables.Add(dataTable);
				Hashtable portCache = DBCache.GetPortCache();
				if (portCache != null && portCache.Count > 0)
				{
					if (affectedPorts == null)
					{
						ICollection values3 = portCache.Values;
						IEnumerator enumerator = values3.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PortInfo portInfo = (PortInfo)enumerator.Current;
								if (uac == null || (uac._authDevicePortList.ContainsKey((long)portInfo.DeviceID) && (uac._authDevicePortList[(long)portInfo.DeviceID].Count <= 0 || uac._authDevicePortList[(long)portInfo.DeviceID].Contains((long)portInfo.ID))))
								{
									DataRow dataRow5 = dataSet.Tables[num].NewRow();
									dataRow5["port_id"] = portInfo.ID;
									dataRow5["device_id"] = portInfo.DeviceID;
									dataRow5["port_number"] = portInfo.PortNum;
									dataRow5["port_nm"] = portInfo.PortName;
									dataRow5["max_voltage"] = portInfo.Max_voltage;
									dataRow5["min_voltage"] = portInfo.Min_voltage;
									dataRow5["max_power_diss"] = portInfo.Max_power_diss;
									dataRow5["min_power_diss"] = portInfo.Min_power_diss;
									dataRow5["max_power"] = portInfo.Max_power;
									dataRow5["min_power"] = portInfo.Min_power;
									dataRow5["max_current"] = portInfo.Max_current;
									dataRow5["min_current"] = portInfo.Min_current;
									dataSet.Tables[num].Rows.Add(dataRow5);
								}
							}
							goto IL_1052;
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					List<int> list3 = new List<int>();
					Hashtable devicePortMap = DBCache.GetDevicePortMap();
					if (devicePortMap != null && devicePortMap.Count > 0)
					{
						foreach (KeyValuePair<string, List<string>> current6 in affectedPorts)
						{
							int num3 = Convert.ToInt32(current6.Key);
							if (uac == null || uac._authDevicePortList.ContainsKey((long)num3))
							{
								List<string> value2 = current6.Value;
								if (value2.Contains("*"))
								{
									if (!devicePortMap.ContainsKey(num3))
									{
										continue;
									}
									List<int> list4 = (List<int>)devicePortMap[num3];
									using (List<int>.Enumerator enumerator4 = list4.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											int current7 = enumerator4.Current;
											if (uac == null || !uac._authDevicePortList.ContainsKey((long)num3) || uac._authDevicePortList[(long)num3].Count <= 0 || uac._authDevicePortList[(long)num3].Contains((long)current7))
											{
												list3.Add(current7);
											}
										}
										continue;
									}
								}
								foreach (string current8 in value2)
								{
									if (uac == null || !uac._authDevicePortList.ContainsKey((long)num3) || uac._authDevicePortList[(long)num3].Count <= 0 || uac._authDevicePortList[(long)num3].Contains((long)Convert.ToInt32(current8)))
									{
										list3.Add(Convert.ToInt32(current8));
									}
								}
							}
						}
					}
					foreach (int current9 in list3)
					{
						PortInfo portInfo2 = (PortInfo)portCache[Convert.ToInt32(current9)];
						if (uac == null || (uac._authDevicePortList.ContainsKey((long)portInfo2.DeviceID) && (uac._authDevicePortList[(long)portInfo2.DeviceID].Count <= 0 || uac._authDevicePortList[(long)portInfo2.DeviceID].Contains((long)portInfo2.ID))))
						{
							DataRow dataRow6 = dataSet.Tables[num].NewRow();
							dataRow6["port_id"] = portInfo2.ID;
							dataRow6["device_id"] = portInfo2.DeviceID;
							dataRow6["port_number"] = portInfo2.PortNum;
							dataRow6["port_nm"] = portInfo2.PortName;
							dataRow6["max_voltage"] = portInfo2.Max_voltage;
							dataRow6["min_voltage"] = portInfo2.Min_voltage;
							dataRow6["max_power_diss"] = portInfo2.Max_power_diss;
							dataRow6["min_power_diss"] = portInfo2.Min_power_diss;
							dataRow6["max_power"] = portInfo2.Max_power;
							dataRow6["min_power"] = portInfo2.Min_power;
							dataRow6["max_current"] = portInfo2.Max_current;
							dataRow6["min_current"] = portInfo2.Min_current;
							dataSet.Tables[num].Rows.Add(dataRow6);
						}
					}
				}
				IL_1052:;
			}
			catch (Exception ex3)
			{
				Common.WriteLine("FastCreateDataset-port: " + ex3.StackTrace, new string[0]);
			}
			num++;
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "bank";
				DataSetManager.ColumnDefine[] array = DataSetManager.colBank;
				for (int i = 0; i < array.Length; i++)
				{
					DataSetManager.ColumnDefine columnDefine4 = array[i];
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType(columnDefine4.colType);
					dataColumn.ColumnName = columnDefine4.colName;
					dataTable.Columns.Add(dataColumn);
				}
				dataTable.PrimaryKey = new DataColumn[]
				{
					dataTable.Columns["device_id"],
					dataTable.Columns["bank_id"]
				};
				dataSet.Tables.Add(dataTable);
				Hashtable bankCache = DBCache.GetBankCache();
				if (bankCache != null && bankCache.Count > 0)
				{
					if (affectedBanks == null)
					{
						ICollection values4 = bankCache.Values;
						IEnumerator enumerator = values4.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								BankInfo bankInfo = (BankInfo)enumerator.Current;
								if (uac == null || (uac._authDevicePortList.ContainsKey((long)bankInfo.DeviceID) && uac._authDevicePortList[(long)bankInfo.DeviceID].Count <= 0))
								{
									DataRow dataRow7 = dataSet.Tables[num].NewRow();
									dataRow7["bank_id"] = bankInfo.ID;
									dataRow7["device_id"] = bankInfo.DeviceID;
									dataRow7["bank_number"] = bankInfo.PortLists;
									dataRow7["bank_nm"] = bankInfo.BankName;
									dataRow7["max_voltage"] = bankInfo.Max_voltage;
									dataRow7["min_voltage"] = bankInfo.Min_voltage;
									dataRow7["max_power_diss"] = bankInfo.Max_power_diss;
									dataRow7["min_power_diss"] = bankInfo.Min_power_diss;
									dataRow7["max_power"] = bankInfo.Max_power;
									dataRow7["min_power"] = bankInfo.Min_power;
									dataRow7["max_current"] = bankInfo.Max_current;
									dataRow7["min_current"] = bankInfo.Min_current;
									dataSet.Tables[num].Rows.Add(dataRow7);
								}
							}
							goto IL_161F;
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					List<int> list5 = new List<int>();
					Hashtable deviceBankMap = DBCache.GetDeviceBankMap();
					if (deviceBankMap != null && deviceBankMap.Count > 0)
					{
						foreach (KeyValuePair<string, List<string>> current10 in affectedBanks)
						{
							int num4 = Convert.ToInt32(current10.Key);
							if (uac == null || (uac._authDevicePortList.ContainsKey((long)num4) && uac._authDevicePortList[(long)num4].Count <= 0))
							{
								List<string> value3 = current10.Value;
								if (value3.Contains("*"))
								{
									if (!deviceBankMap.ContainsKey(num4))
									{
										continue;
									}
									List<int> list6 = (List<int>)deviceBankMap[num4];
									using (List<int>.Enumerator enumerator4 = list6.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											int current11 = enumerator4.Current;
											list5.Add(current11);
										}
										continue;
									}
								}
								foreach (string current12 in value3)
								{
									list5.Add(Convert.ToInt32(current12));
								}
							}
						}
					}
					foreach (int current13 in list5)
					{
						BankInfo bankInfo2 = (BankInfo)bankCache[Convert.ToInt32(current13)];
						if (uac == null || (uac._authDevicePortList.ContainsKey((long)bankInfo2.DeviceID) && uac._authDevicePortList[(long)bankInfo2.DeviceID].Count <= 0))
						{
							DataRow dataRow8 = dataSet.Tables[num].NewRow();
							dataRow8["bank_id"] = bankInfo2.ID;
							dataRow8["device_id"] = bankInfo2.DeviceID;
							dataRow8["bank_number"] = bankInfo2.PortLists;
							dataRow8["bank_nm"] = bankInfo2.BankName;
							dataRow8["max_voltage"] = bankInfo2.Max_voltage;
							dataRow8["min_voltage"] = bankInfo2.Min_voltage;
							dataRow8["max_power_diss"] = bankInfo2.Max_power_diss;
							dataRow8["min_power_diss"] = bankInfo2.Min_power_diss;
							dataRow8["max_power"] = bankInfo2.Max_power;
							dataRow8["min_power"] = bankInfo2.Min_power;
							dataRow8["max_current"] = bankInfo2.Max_current;
							dataRow8["min_current"] = bankInfo2.Min_current;
							dataSet.Tables[num].Rows.Add(dataRow8);
						}
					}
				}
				IL_161F:;
			}
			catch (Exception ex4)
			{
				Common.WriteLine("FastCreateDataset-bank: " + ex4.StackTrace, new string[0]);
			}
			num++;
			try
			{
				DataTable dataTable = new DataTable();
				dataTable.TableName = "line";
				DataSetManager.ColumnDefine[] array = DataSetManager.colLine;
				for (int i = 0; i < array.Length; i++)
				{
					DataSetManager.ColumnDefine columnDefine5 = array[i];
					DataColumn dataColumn = new DataColumn();
					dataColumn.DataType = Type.GetType(columnDefine5.colType);
					dataColumn.ColumnName = columnDefine5.colName;
					dataTable.Columns.Add(dataColumn);
				}
				dataTable.PrimaryKey = new DataColumn[]
				{
					dataTable.Columns["device_id"],
					dataTable.Columns["line_id"]
				};
				dataSet.Tables.Add(dataTable);
				Hashtable lineCache = DBCache.GetLineCache();
				if (lineCache != null && lineCache.Count > 0)
				{
					if (affectedLines == null)
					{
						ICollection values5 = lineCache.Values;
						IEnumerator enumerator = values5.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								LineInfo lineInfo = (LineInfo)enumerator.Current;
								if (uac == null || (uac._authDevicePortList.ContainsKey((long)lineInfo.DeviceID) && uac._authDevicePortList[(long)lineInfo.DeviceID].Count <= 0))
								{
									DataRow dataRow9 = dataSet.Tables[num].NewRow();
									dataRow9["line_id"] = lineInfo.ID;
									dataRow9["device_id"] = lineInfo.DeviceID;
									dataRow9["line_number"] = lineInfo.LineNumber;
									dataRow9["line_name"] = lineInfo.LineName;
									dataRow9["max_voltage"] = lineInfo.Max_voltage;
									dataRow9["min_voltage"] = lineInfo.Min_voltage;
									dataRow9["max_power"] = lineInfo.Max_power;
									dataRow9["min_power"] = lineInfo.Min_power;
									dataRow9["max_current"] = lineInfo.Max_current;
									dataRow9["min_current"] = lineInfo.Min_current;
									dataSet.Tables[num].Rows.Add(dataRow9);
								}
							}
							goto IL_1B98;
						}
						finally
						{
							IDisposable disposable = enumerator as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					List<int> list7 = new List<int>();
					Hashtable deviceLineMap = DBCache.GetDeviceLineMap();
					if (deviceLineMap != null && deviceLineMap.Count > 0)
					{
						foreach (KeyValuePair<string, List<string>> current14 in affectedLines)
						{
							int num5 = Convert.ToInt32(current14.Key);
							if (uac == null || (uac._authDevicePortList.ContainsKey((long)num5) && uac._authDevicePortList[(long)num5].Count <= 0))
							{
								List<string> value4 = current14.Value;
								if (value4.Contains("*"))
								{
									if (!deviceLineMap.ContainsKey(num5))
									{
										continue;
									}
									List<int> list8 = (List<int>)deviceLineMap[num5];
									using (List<int>.Enumerator enumerator4 = list8.GetEnumerator())
									{
										while (enumerator4.MoveNext())
										{
											int current15 = enumerator4.Current;
											list7.Add(current15);
										}
										continue;
									}
								}
								foreach (string current16 in value4)
								{
									list7.Add(Convert.ToInt32(current16));
								}
							}
						}
					}
					foreach (int current17 in list7)
					{
						LineInfo lineInfo2 = (LineInfo)lineCache[Convert.ToInt32(current17)];
						if (uac == null || (uac._authDevicePortList.ContainsKey((long)lineInfo2.DeviceID) && uac._authDevicePortList[(long)lineInfo2.DeviceID].Count <= 0))
						{
							DataRow dataRow10 = dataSet.Tables[num].NewRow();
							dataRow10["line_id"] = lineInfo2.ID;
							dataRow10["device_id"] = lineInfo2.DeviceID;
							dataRow10["line_number"] = lineInfo2.LineNumber;
							dataRow10["line_name"] = lineInfo2.LineName;
							dataRow10["max_voltage"] = lineInfo2.Max_voltage;
							dataRow10["min_voltage"] = lineInfo2.Min_voltage;
							dataRow10["max_power"] = lineInfo2.Max_power;
							dataRow10["min_power"] = lineInfo2.Min_power;
							dataRow10["max_current"] = lineInfo2.Max_current;
							dataRow10["min_current"] = lineInfo2.Min_current;
							dataSet.Tables[num].Rows.Add(dataRow10);
						}
					}
				}
				IL_1B98:;
			}
			catch (Exception ex5)
			{
				Common.WriteLine("FastCreateDataset-line: " + ex5.StackTrace, new string[0]);
			}
			return dataSet;
		}
	}
}
