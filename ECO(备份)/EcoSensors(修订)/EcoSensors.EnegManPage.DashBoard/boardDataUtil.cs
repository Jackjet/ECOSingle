using CommonAPI.CultureTransfer;
using DBAccessAPI;
using Dispatcher;
using EcoDevice.AccessAPI;
using EcoSensors.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
namespace EcoSensors.EnegManPage.DashBoard
{
	internal class boardDataUtil
	{
		public const int dsindex_Dev = 0;
		public const int dsindex_SS = 1;
		public const int dsindex_Port = 2;
		public const int dsindex_Bank = 3;
		public const int dsindex_Line = 4;
		public const string tb_rackid = "rack_id";
		public const string tb_devID = "device_id";
		public const string tb_devIP = "device_ip";
		public const string tb_devnm = "device_nm";
		public const string tb_maxvol = "max_voltage";
		public const string tb_minvol = "min_voltage";
		public const string tb_maxpowerD = "max_power_diss";
		public const string tb_minpowerD = "min_power_diss";
		public const string tb_maxpower = "max_power";
		public const string tb_minpower = "min_power";
		public const string tb_maxcurrent = "max_current";
		public const string tb_mincurrent = "min_current";
		public const string tb_Vvol = "voltage_value";
		public const string tb_Vcurrent = "current_value";
		public const string tb_Vpower = "power_value";
		public const string tb_VpowerD = "power_consumption";
		public const string tb_VLeakCurrent = "leakcurrent_status";
		public const string tb_devSt = "device_state";
		public const string tb_devDoorSt = "doorsensor_status";
		public const string tb_ssID = "sensor_id";
		public const string tb_ssnm = "sensor_nm";
		public const string tb_Vhumi = "humidity";
		public const string tb_Vtempt = "temperature";
		public const string tb_Vpress = "press_value";
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
		public const string tb_bankNo = "bank_number";
		public const string tb_banknm = "bank_nm";
		public const string tb_bankSt = "bank_state";
		public const string tb_lineID = "line_id";
		public const string tb_lineNo = "line_number";
		private static Color dft_COLOR = Color.FromArgb(162, 215, 48);
		public static int PDUData_FreshFLG_Yes = 1;
		public static int PDUData_FreshFLG_NO = 2;
		public static int PDUData_Doorst_close = 0;
		public static int PDUData_Doorst_open = 1;
		public static int PDUData_Doorst_noattach = 2;
		public static System.Collections.Generic.Dictionary<long, Color> getAllRacksColor(string SelBoardTag, System.Collections.ArrayList allRacks, DataSet autodata_ds, ref string ret_para)
		{
			RackStatusAll.Total_Value = 0.0;
			RackStatusAll.fetal_error = false;
			System.Collections.Generic.Dictionary<long, Color> result = new System.Collections.Generic.Dictionary<long, Color>();
			RackStatusAll.Board_Tag = SelBoardTag;
			switch (SelBoardTag)
			{
			case "0:0":
				result = boardDataUtil.sensorsThresholdStatus(allRacks, autodata_ds);
				break;
			case "0:9":
				result = boardDataUtil.DoorColorStatus(allRacks, autodata_ds);
				break;
			case "0:1":
				RackStatusAll.MinValue = 0.0;
				RackStatusAll.MaxValue = 100.0;
				result = RackStatusAll.RackColor(boardDataUtil.AvailablePower(allRacks, autodata_ds));
				break;
			case "1:0":
			{
				string str = "";
				string str2 = "";
				result = RackStatusAll.RackColor(boardDataUtil.heatLoadDissipation(allRacks, ref str, ref str2));
				ret_para = str + "#" + str2;
				break;
			}
			case "1:1":
				RackStatusAll.MinValue = 500.0;
				RackStatusAll.MaxValue = 4500.0;
				result = RackStatusAll.RackColor(boardDataUtil.heatLoadDensityOSD(allRacks, autodata_ds));
				break;
			case "2:0":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 45.0;
				result = RackStatusAll.RackColor(boardDataUtil.coldIntakeTemperature(allRacks, autodata_ds));
				break;
			case "2:1":
				RackStatusAll.MinValue = 2.0;
				RackStatusAll.MaxValue = 20.0;
				result = RackStatusAll.RackColor(boardDataUtil.intakeDiffTemp(allRacks, autodata_ds));
				break;
			case "2:2":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 45.0;
				result = RackStatusAll.RackColor(boardDataUtil.exhaustTemperature(allRacks, autodata_ds));
				break;
			case "2:3":
				RackStatusAll.MinValue = 0.0;
				RackStatusAll.MaxValue = 60.0;
				result = RackStatusAll.RackColor(boardDataUtil.hotExhaustTemperature(allRacks, autodata_ds));
				break;
			case "2:4":
				RackStatusAll.MinValue = 5.0;
				RackStatusAll.MaxValue = 55.0;
				result = RackStatusAll.RackColor(boardDataUtil.coldHotAcrossTempRise(allRacks, autodata_ds));
				break;
			case "3:0":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 200.0;
				result = RackStatusAll.RackColor(boardDataUtil.intakeDiffPressure(allRacks, autodata_ds));
				break;
			case "3:1":
				RackStatusAll.MinValue = 500.0;
				RackStatusAll.MaxValue = 2500.0;
				result = RackStatusAll.RackColor(boardDataUtil.heatLoadAirflowAcross(allRacks, autodata_ds));
				break;
			case "3:2":
				RackStatusAll.MinValue = 500.0;
				RackStatusAll.MaxValue = 2500.0;
				result = RackStatusAll.RackColor(boardDataUtil.floorPlenumAirflow(allRacks, autodata_ds));
				break;
			case "3:4":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 50.0;
				result = RackStatusAll.RackColor(boardDataUtil.hotRecirculation(allRacks, autodata_ds));
				break;
			case "3:5":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 50.0;
				result = RackStatusAll.RackColor(boardDataUtil.coldBypass(allRacks, autodata_ds));
				break;
			case "4:0":
				RackStatusAll.MinValue = 10.0;
				RackStatusAll.MaxValue = 90.0;
				result = RackStatusAll.RackColor(boardDataUtil.coldIntakeRelative(allRacks, autodata_ds));
				break;
			case "4:1":
				RackStatusAll.MinValue = 0.0;
				RackStatusAll.MaxValue = 15.0;
				result = RackStatusAll.RackColor(boardDataUtil.coldIntakeDewPointTemperature(allRacks, autodata_ds));
				break;
			}
			return result;
		}
		public static System.Collections.Generic.Dictionary<long, Color> sensorsThresholdStatus(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			double num = 0.0;
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				Color silver = boardDataUtil.dft_COLOR;
				Color left = boardDataUtil.dft_COLOR;
				Color left2 = boardDataUtil.dft_COLOR;
				Color left3 = boardDataUtil.dft_COLOR;
				Color left4 = boardDataUtil.dft_COLOR;
				string arg_56_0 = rackInfo.DeviceInfo;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"power_value <>'",
					-500,
					"' and power_value <>'",
					-1000,
					"' "
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					num += ecoConvert.f2d(array2[k]["power_value"]);
				}
				if (dataTable.Select("device_state=0").Length > 0)
				{
					silver = Color.Silver;
					dictionary.Add(rackInfo.RackID, Color.Silver);
				}
				else
				{
					for (int l = 0; l < dataTable.Rows.Count; l++)
					{
						if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable.Rows[l]["device_id"])))
						{
							ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable.Rows[l]["device_id"])];
							string device_model = deviceWithZoneRackInfo.device_model;
							string fw_version = deviceWithZoneRackInfo.fw_version;
							DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
							int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "dev");
							boardDataUtil.getDevThresholdColor(dataTable.Rows[l], uIthEdidflg, ref silver, deviceModelConfig.leakCurrent);
							if (silver == Color.Silver || silver == Color.Red)
							{
								break;
							}
						}
					}
					if (silver == Color.Silver)
					{
						dictionary.Add(rackInfo.RackID, Color.Silver);
					}
					else
					{
						DataTable dataTable2 = ds.Tables[1].Clone();
						DataRow[] array3 = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
						for (int m = 0; m < array3.Length; m++)
						{
							dataTable2.ImportRow(array3[m]);
						}
						for (int n = 0; n < dataTable2.Rows.Count; n++)
						{
							if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable2.Rows[n]["device_id"])))
							{
								ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable2.Rows[n]["device_id"])];
								string device_model2 = deviceWithZoneRackInfo.device_model;
								string fw_version2 = deviceWithZoneRackInfo.fw_version;
								DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model2, fw_version2);
								int uIthEdidflg2 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig2, "ss");
								boardDataUtil.getSensorThresholdColor(dataTable2.Rows[n], uIthEdidflg2, ref left);
								if (left == Color.Silver || left == Color.Red)
								{
									break;
								}
							}
						}
						if (left == Color.Silver)
						{
							dictionary.Add(rackInfo.RackID, Color.Silver);
						}
						else
						{
							DataTable dataTable3 = ds.Tables[2].Clone();
							DataRow[] array4 = ds.Tables[2].Select("rack_id= " + rackInfo.RackID);
							for (int num2 = 0; num2 < array4.Length; num2++)
							{
								dataTable3.ImportRow(array4[num2]);
							}
							for (int num3 = 0; num3 < dataTable3.Rows.Count; num3++)
							{
								if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable3.Rows[num3]["device_id"])))
								{
									ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable3.Rows[num3]["device_id"])];
									string device_model3 = deviceWithZoneRackInfo.device_model;
									string fw_version3 = deviceWithZoneRackInfo.fw_version;
									DevModelConfig deviceModelConfig3 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model3, fw_version3);
									if (deviceModelConfig3.perportreading == 2)
									{
										string a = System.Convert.ToString(dataTable3.Rows[num3]["port_state"]);
										if (!(a != OutletStatus.ON.ToString()) || !(a != OutletStatus.NA.ToString()))
										{
											int uIthEdidflg3 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig3, "port");
											boardDataUtil.getPortThresholdColor(dataTable3.Rows[num3], uIthEdidflg3, ref left2);
											if (left2 == Color.Silver)
											{
												dictionary.Add(rackInfo.RackID, Color.Silver);
												break;
											}
										}
									}
								}
							}
							if (!(left2 == Color.Silver))
							{
								DataTable dataTable4 = ds.Tables[3].Clone();
								DataRow[] array5 = ds.Tables[3].Select(string.Concat(new object[]
								{
									"rack_id= ",
									rackInfo.RackID,
									" and bank_state='",
									BankStatus.ON.ToString(),
									"'"
								}));
								for (int num4 = 0; num4 < array5.Length; num4++)
								{
									dataTable4.ImportRow(array5[num4]);
								}
								for (int num5 = 0; num5 < dataTable4.Rows.Count; num5++)
								{
									if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable4.Rows[num5]["device_id"])))
									{
										ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable4.Rows[num5]["device_id"])];
										string device_model4 = deviceWithZoneRackInfo.device_model;
										string fw_version4 = deviceWithZoneRackInfo.fw_version;
										DevModelConfig deviceModelConfig4 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model4, fw_version4);
										if (deviceModelConfig4.perbankReading == 2)
										{
											int uIthEdidflg4 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig4, "bank");
											boardDataUtil.getBankThresholdColor(dataTable4.Rows[num5], uIthEdidflg4, ref left3);
											if (left3 == Color.Silver)
											{
												dictionary.Add(rackInfo.RackID, Color.Silver);
												break;
											}
										}
									}
								}
								if (!(left3 == Color.Silver))
								{
									DataTable dataTable5 = ds.Tables[4].Clone();
									DataRow[] array6 = ds.Tables[4].Select("rack_id= " + rackInfo.RackID);
									for (int num6 = 0; num6 < array6.Length; num6++)
									{
										dataTable5.ImportRow(array6[num6]);
									}
									for (int num7 = 0; num7 < dataTable5.Rows.Count; num7++)
									{
										if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable5.Rows[num7]["device_id"])))
										{
											ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable5.Rows[num7]["device_id"])];
											string device_model5 = deviceWithZoneRackInfo.device_model;
											string fw_version5 = deviceWithZoneRackInfo.fw_version;
											DevModelConfig deviceModelConfig5 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model5, fw_version5);
											if (deviceModelConfig5.perlineReading == Constant.YES)
											{
												int uIthEdidflg5 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig5, "line");
												boardDataUtil.getLineThresholdColor(dataTable5.Rows[num7], uIthEdidflg5, ref left4);
												if (left4 == Color.Silver)
												{
													dictionary.Add(rackInfo.RackID, Color.Silver);
													break;
												}
											}
										}
									}
									if (!(left4 == Color.Silver))
									{
										try
										{
											if (silver == Color.Red || left == Color.Red || left2 == Color.Red || left3 == Color.Red || left4 == Color.Red)
											{
												dictionary.Add(rackInfo.RackID, Color.Red);
											}
											else
											{
												if (silver == Color.Orange || left == Color.Orange || left2 == Color.Orange || left3 == Color.Orange || left4 == Color.Orange)
												{
													dictionary.Add(rackInfo.RackID, Color.Orange);
												}
												else
												{
													dictionary.Add(rackInfo.RackID, boardDataUtil.dft_COLOR);
												}
											}
										}
										catch (System.Exception)
										{
										}
									}
								}
							}
						}
					}
				}
			}
			RackStatusAll.Total_Value = num;
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, Color> UACsensorsThresholdStatus(System.Collections.Generic.Dictionary<long, System.Collections.Generic.List<long>> UACDevPort, System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				Color silver = boardDataUtil.dft_COLOR;
				Color left = boardDataUtil.dft_COLOR;
				Color left2 = boardDataUtil.dft_COLOR;
				Color left3 = boardDataUtil.dft_COLOR;
				string arg_44_0 = rackInfo.DeviceInfo;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				if (dataTable.Select("device_state=0").Length > 0)
				{
					silver = Color.Silver;
					dictionary.Add(rackInfo.RackID, Color.Silver);
				}
				else
				{
					for (int k = 0; k < dataTable.Rows.Count; k++)
					{
						int num = System.Convert.ToInt32(dataTable.Rows[k]["device_id"]);
						if (devicRackZoneRelation.ContainsKey(num) && UACDevPort.ContainsKey((long)num) && UACDevPort[(long)num].Count == 0)
						{
							ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[num];
							string device_model = deviceWithZoneRackInfo.device_model;
							string fw_version = deviceWithZoneRackInfo.fw_version;
							DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
							int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "dev");
							boardDataUtil.getDevThresholdColor(dataTable.Rows[k], uIthEdidflg, ref silver, deviceModelConfig.leakCurrent);
							if (silver == Color.Silver || silver == Color.Red)
							{
								break;
							}
						}
					}
					if (silver == Color.Silver)
					{
						dictionary.Add(rackInfo.RackID, Color.Silver);
					}
					else
					{
						DataTable dataTable2 = ds.Tables[2].Clone();
						DataRow[] array2 = ds.Tables[2].Select("rack_id= " + rackInfo.RackID);
						for (int l = 0; l < array2.Length; l++)
						{
							dataTable2.ImportRow(array2[l]);
						}
						for (int m = 0; m < dataTable2.Rows.Count; m++)
						{
							int num = System.Convert.ToInt32(dataTable2.Rows[m]["device_id"]);
							if (devicRackZoneRelation.ContainsKey(num))
							{
								ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[num];
								string device_model2 = deviceWithZoneRackInfo.device_model;
								string fw_version2 = deviceWithZoneRackInfo.fw_version;
								DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model2, fw_version2);
								if (deviceModelConfig2.perportreading == 2)
								{
									string a = System.Convert.ToString(dataTable2.Rows[m]["port_state"]);
									if (!(a != OutletStatus.ON.ToString()) || !(a != OutletStatus.NA.ToString()))
									{
										int uIthEdidflg2 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig2, "port");
										boardDataUtil.getPortThresholdColor(dataTable2.Rows[m], uIthEdidflg2, ref left2);
										if (left2 == Color.Silver)
										{
											dictionary.Add(rackInfo.RackID, Color.Silver);
											break;
										}
									}
								}
							}
						}
						if (!(left2 == Color.Silver))
						{
							DataTable dataTable3 = ds.Tables[3].Clone();
							DataRow[] array3 = ds.Tables[3].Select(string.Concat(new object[]
							{
								"rack_id= ",
								rackInfo.RackID,
								" and bank_state='",
								BankStatus.ON.ToString(),
								"'"
							}));
							for (int n = 0; n < array3.Length; n++)
							{
								dataTable3.ImportRow(array3[n]);
							}
							for (int num2 = 0; num2 < dataTable3.Rows.Count; num2++)
							{
								int num = System.Convert.ToInt32(dataTable3.Rows[num2]["device_id"]);
								if (devicRackZoneRelation.ContainsKey(num) && UACDevPort.ContainsKey((long)num) && UACDevPort[(long)num].Count == 0)
								{
									ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable3.Rows[num2]["device_id"])];
									string device_model3 = deviceWithZoneRackInfo.device_model;
									string fw_version3 = deviceWithZoneRackInfo.fw_version;
									DevModelConfig deviceModelConfig3 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model3, fw_version3);
									if (deviceModelConfig3.perbankReading == 2)
									{
										int uIthEdidflg3 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig3, "bank");
										boardDataUtil.getBankThresholdColor(dataTable3.Rows[num2], uIthEdidflg3, ref left3);
										if (left3 == Color.Silver)
										{
											dictionary.Add(rackInfo.RackID, Color.Silver);
											break;
										}
									}
								}
							}
							if (!(left3 == Color.Silver))
							{
								try
								{
									if (silver == Color.Red || left == Color.Red || left2 == Color.Red || left3 == Color.Red)
									{
										dictionary.Add(rackInfo.RackID, Color.Red);
									}
									else
									{
										if (silver == Color.Orange || left == Color.Orange || left2 == Color.Orange || left3 == Color.Orange)
										{
											dictionary.Add(rackInfo.RackID, Color.Orange);
										}
										else
										{
											dictionary.Add(rackInfo.RackID, boardDataUtil.dft_COLOR);
										}
									}
								}
								catch (System.Exception)
								{
								}
							}
						}
					}
				}
			}
			return dictionary;
		}
		public static Color[] StatusAlarmColor(DataSet ds)
		{
			Color[] array = new Color[]
			{
				boardDataUtil.dft_COLOR,
				boardDataUtil.dft_COLOR,
				boardDataUtil.dft_COLOR,
				boardDataUtil.dft_COLOR
			};
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			System.DateTime now = System.DateTime.Now;
			DataTable dataTable = ds.Tables[0];
			if (dataTable.Rows.Count <= 0 || dataTable.Select("device_state =0").Length > 0)
			{
				array[0] = Color.Silver;
				array[1] = Color.Silver;
				array[2] = Color.Silver;
				array[3] = Color.Silver;
				System.DateTime now2 = System.DateTime.Now;
				System.TimeSpan timeSpan = now2 - now;
				commUtil.ShowInfo_DEBUG(string.Concat(new object[]
				{
					"%%% StatusAlarmColor - ",
					now2.ToString("yyyy-MM-dd HH:mm:ss:fff"),
					"  Spend = ",
					timeSpan.TotalMilliseconds
				}));
				return array;
			}
			DataTable dataTable2 = ds.Tables[1];
			if (dataTable2.Rows.Count <= 0)
			{
				array[1] = Color.Silver;
				array[2] = Color.Silver;
				array[3] = Color.Silver;
			}
			else
			{
				for (int i = 0; i < dataTable2.Rows.Count; i++)
				{
					if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable2.Rows[i]["device_id"])))
					{
						ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable2.Rows[i]["device_id"])];
						string device_model = deviceWithZoneRackInfo.device_model;
						string fw_version = deviceWithZoneRackInfo.fw_version;
						DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
						int uIthEdidflg = devcfgUtil.UIThresholdEditFlg(deviceModelConfig, "ss");
						if (array[1] != Color.Silver && array[1] != Color.Red)
						{
							boardDataUtil.getSensorThresholdColor_Temp(dataTable2.Rows[i], uIthEdidflg, ref array[1]);
						}
						if (array[2] != Color.Silver && array[2] != Color.Red)
						{
							boardDataUtil.getSensorThresholdColor_Press(dataTable2.Rows[i], uIthEdidflg, ref array[2]);
						}
						if (array[3] != Color.Silver && array[3] != Color.Red)
						{
							boardDataUtil.getSensorThresholdColor_Humidity(dataTable2.Rows[i], uIthEdidflg, ref array[3]);
						}
					}
				}
			}
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable.Rows[j]["device_id"])))
				{
					ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable.Rows[j]["device_id"])];
					string device_model2 = deviceWithZoneRackInfo.device_model;
					string fw_version2 = deviceWithZoneRackInfo.fw_version;
					DevModelConfig deviceModelConfig2 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model2, fw_version2);
					int uIthEdidflg2 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig2, "dev");
					boardDataUtil.getDevThresholdColor(dataTable.Rows[j], uIthEdidflg2, ref array[0], deviceModelConfig2.leakCurrent);
					if (array[0] == Color.Silver)
					{
						return array;
					}
				}
			}
			DataTable dataTable3 = ds.Tables[2];
			for (int k = 0; k < dataTable3.Rows.Count; k++)
			{
				if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable3.Rows[k]["device_id"])))
				{
					ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable3.Rows[k]["device_id"])];
					string device_model3 = deviceWithZoneRackInfo.device_model;
					string fw_version3 = deviceWithZoneRackInfo.fw_version;
					DevModelConfig deviceModelConfig3 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model3, fw_version3);
					if (deviceModelConfig3.perportreading == 2)
					{
						string a = System.Convert.ToString(dataTable3.Rows[k]["port_state"]);
						if (!(a != OutletStatus.ON.ToString()) || !(a != OutletStatus.NA.ToString()))
						{
							int uIthEdidflg3 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig3, "port");
							boardDataUtil.getPortThresholdColor(dataTable3.Rows[k], uIthEdidflg3, ref array[0]);
							if (array[0] == Color.Silver)
							{
								return array;
							}
						}
					}
				}
			}
			DataTable dataTable4 = ds.Tables[3];
			for (int l = 0; l < dataTable4.Rows.Count; l++)
			{
				if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable4.Rows[l]["device_id"])))
				{
					ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable4.Rows[l]["device_id"])];
					string device_model4 = deviceWithZoneRackInfo.device_model;
					string fw_version4 = deviceWithZoneRackInfo.fw_version;
					DevModelConfig deviceModelConfig4 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model4, fw_version4);
					if (deviceModelConfig4.perbankReading == 2)
					{
						string a2 = System.Convert.ToString(dataTable4.Rows[l]["bank_state"]);
						if (!(a2 != BankStatus.ON.ToString()))
						{
							int uIthEdidflg4 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig4, "bank");
							boardDataUtil.getBankThresholdColor(dataTable4.Rows[l], uIthEdidflg4, ref array[0]);
							if (array[0] == Color.Silver)
							{
								return array;
							}
						}
					}
				}
			}
			DataTable dataTable5 = ds.Tables[4];
			for (int m = 0; m < dataTable5.Rows.Count; m++)
			{
				if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(dataTable5.Rows[m]["device_id"])))
				{
					ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(dataTable5.Rows[m]["device_id"])];
					string device_model5 = deviceWithZoneRackInfo.device_model;
					string fw_version5 = deviceWithZoneRackInfo.fw_version;
					DevModelConfig deviceModelConfig5 = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model5, fw_version5);
					if (deviceModelConfig5.perlineReading == Constant.YES)
					{
						int uIthEdidflg5 = devcfgUtil.UIThresholdEditFlg(deviceModelConfig5, "line");
						boardDataUtil.getLineThresholdColor(dataTable5.Rows[m], uIthEdidflg5, ref array[0]);
						if (array[0] == Color.Silver)
						{
							return array;
						}
					}
				}
			}
			return array;
		}
		private static void getDevThresholdColor(DataRow curRow, int UIthEdidflg, ref Color PDUstatu, int lineLeakOpt)
		{
			if (PDUstatu == Color.Red || PDUstatu == Color.Silver)
			{
				return;
			}
			if (lineLeakOpt == Constant.YES && (int)curRow["leakcurrent_status"] == 2)
			{
				PDUstatu = Color.Red;
				return;
			}
			double num = ecoConvert.f2d(curRow["voltage_value"]);
			double num2 = ecoConvert.f2d(curRow["current_value"]);
			double num3 = ecoConvert.f2d(curRow["power_value"]);
			double num4 = ecoConvert.f2d(curRow["power_consumption"]);
			if (devcfgUtil.haveMeasureCurrent(UIthEdidflg) && num2 == -500.0)
			{
				PDUstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasureVoltage(UIthEdidflg) && num == -500.0)
			{
				PDUstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePower(UIthEdidflg) && num3 == -500.0)
			{
				PDUstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePowerD(UIthEdidflg) && num4 == -500.0)
			{
				PDUstatu = Color.Red;
				return;
			}
			double min_v = ecoConvert.f2d(curRow["min_current"]);
			double max_v = ecoConvert.f2d(curRow["max_current"]);
			double min_v2 = ecoConvert.f2d(curRow["min_voltage"]);
			double max_v2 = ecoConvert.f2d(curRow["max_voltage"]);
			double min_v3 = ecoConvert.f2d(curRow["min_power"]);
			double max_v3 = ecoConvert.f2d(curRow["max_power"]);
			double min_v4 = ecoConvert.f2d(curRow["min_power_diss"]);
			double max_v4 = ecoConvert.f2d(curRow["max_power_diss"]);
			if (PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(1, num2, UIthEdidflg, min_v, max_v, ref PDUstatu);
			}
			if (PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(2, num, UIthEdidflg, min_v2, max_v2, ref PDUstatu);
			}
			if (PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(3, num3, UIthEdidflg, min_v3, max_v3, ref PDUstatu);
			}
			if (PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(4, num4, UIthEdidflg, min_v4, max_v4, ref PDUstatu);
			}
			if (PDUstatu == Color.Orange || PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(1, num2, UIthEdidflg, min_v, max_v, ref PDUstatu);
			}
			if (PDUstatu == Color.Orange || PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(2, num, UIthEdidflg, min_v2, max_v2, ref PDUstatu);
			}
			if (PDUstatu == Color.Orange || PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(3, num3, UIthEdidflg, min_v3, max_v3, ref PDUstatu);
			}
			if (PDUstatu == Color.Orange || PDUstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(4, num4, UIthEdidflg, min_v4, max_v4, ref PDUstatu);
			}
		}
		private static void getSensorThresholdColor(DataRow curRow, int UIthEdidflg, ref Color Sensorstatu)
		{
			boardDataUtil.getSensorThresholdColor_Temp(curRow, UIthEdidflg, ref Sensorstatu);
			boardDataUtil.getSensorThresholdColor_Press(curRow, UIthEdidflg, ref Sensorstatu);
			boardDataUtil.getSensorThresholdColor_Humidity(curRow, UIthEdidflg, ref Sensorstatu);
		}
		private static void getSensorThresholdColor_Temp(DataRow curRow, int UIthEdidflg, ref Color Sensorstatu)
		{
			if (Sensorstatu == Color.Red || Sensorstatu == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["temperature"]);
			double num2 = ecoConvert.f2d(curRow["max_temperature"]);
			double num3 = ecoConvert.f2d(curRow["min_temperature"]);
			if (devcfgUtil.haveUIEditV(UIthEdidflg, 1, 2, num3, num2) && num == -500.0)
			{
				Sensorstatu = Color.Red;
				return;
			}
			if (Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(5, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
			if (Sensorstatu == Color.Orange || Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(5, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
		}
		private static void getSensorThresholdColor_Press(DataRow curRow, int UIthEdidflg, ref Color Sensorstatu)
		{
			if (Sensorstatu == Color.Red || Sensorstatu == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["press_value"]);
			double num2 = ecoConvert.f2d(curRow["max_press"]);
			double num3 = ecoConvert.f2d(curRow["min_press"]);
			if (devcfgUtil.haveUIEditV(UIthEdidflg, 16, 32, num3, num2) && num == -500.0)
			{
				Sensorstatu = Color.Red;
				return;
			}
			if (Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(7, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
			if (Sensorstatu == Color.Orange || Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(7, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
		}
		private static void getSensorThresholdColor_Humidity(DataRow curRow, int UIthEdidflg, ref Color Sensorstatu)
		{
			if (Sensorstatu == Color.Red || Sensorstatu == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["humidity"]);
			double num2 = ecoConvert.f2d(curRow["max_humidity"]);
			double num3 = ecoConvert.f2d(curRow["min_humidity"]);
			if (devcfgUtil.haveUIEditV(UIthEdidflg, 4, 8, num3, num2) && num == -500.0)
			{
				Sensorstatu = Color.Red;
				return;
			}
			if (Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(6, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
			if (Sensorstatu == Color.Orange || Sensorstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(6, num, UIthEdidflg, num3, num2, ref Sensorstatu);
			}
		}
		private static void getPortThresholdColor(DataRow curRow, int UIthEdidflg, ref Color Portstatu)
		{
			if (Portstatu == Color.Red || Portstatu == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["voltage_value"]);
			double num2 = ecoConvert.f2d(curRow["current_value"]);
			double num3 = ecoConvert.f2d(curRow["power_value"]);
			double num4 = ecoConvert.f2d(curRow["power_consumption"]);
			if (devcfgUtil.haveMeasureCurrent(UIthEdidflg) && num2 == -500.0)
			{
				Portstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasureVoltage(UIthEdidflg) && num == -500.0)
			{
				Portstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePower(UIthEdidflg) && num3 == -500.0)
			{
				Portstatu = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePowerD(UIthEdidflg) && num4 == -500.0)
			{
				Portstatu = Color.Red;
				return;
			}
			double min_v = ecoConvert.f2d(curRow["min_current"]);
			double max_v = ecoConvert.f2d(curRow["max_current"]);
			double min_v2 = ecoConvert.f2d(curRow["min_voltage"]);
			double max_v2 = ecoConvert.f2d(curRow["max_voltage"]);
			double min_v3 = ecoConvert.f2d(curRow["min_power"]);
			double max_v3 = ecoConvert.f2d(curRow["max_power"]);
			double min_v4 = ecoConvert.f2d(curRow["min_power_diss"]);
			double max_v4 = ecoConvert.f2d(curRow["max_power_diss"]);
			if (Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(1, num2, UIthEdidflg, min_v, max_v, ref Portstatu);
			}
			if (Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(2, num, UIthEdidflg, min_v2, max_v2, ref Portstatu);
			}
			if (Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(3, num3, UIthEdidflg, min_v3, max_v3, ref Portstatu);
			}
			if (Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(4, num4, UIthEdidflg, min_v4, max_v4, ref Portstatu);
			}
			if (Portstatu == Color.Orange || Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(1, num2, UIthEdidflg, min_v, max_v, ref Portstatu);
			}
			if (Portstatu == Color.Orange || Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(2, num, UIthEdidflg, min_v2, max_v2, ref Portstatu);
			}
			if (Portstatu == Color.Orange || Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(3, num3, UIthEdidflg, min_v3, max_v3, ref Portstatu);
			}
			if (Portstatu == Color.Orange || Portstatu == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(4, num4, UIthEdidflg, min_v4, max_v4, ref Portstatu);
			}
		}
		private static void getBankThresholdColor(DataRow curRow, int UIthEdidflg, ref Color Bankstatus)
		{
			if (Bankstatus == Color.Red || Bankstatus == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["voltage_value"]);
			double num2 = ecoConvert.f2d(curRow["current_value"]);
			double num3 = ecoConvert.f2d(curRow["power_value"]);
			double num4 = ecoConvert.f2d(curRow["power_consumption"]);
			if (devcfgUtil.haveMeasureCurrent(UIthEdidflg) && num2 == -500.0)
			{
				Bankstatus = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasureVoltage(UIthEdidflg) && num == -500.0)
			{
				Bankstatus = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePower(UIthEdidflg) && num3 == -500.0)
			{
				Bankstatus = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePowerD(UIthEdidflg) && num4 == -500.0)
			{
				Bankstatus = Color.Red;
				return;
			}
			double min_v = ecoConvert.f2d(curRow["min_current"]);
			double max_v = ecoConvert.f2d(curRow["max_current"]);
			double min_v2 = ecoConvert.f2d(curRow["min_voltage"]);
			double max_v2 = ecoConvert.f2d(curRow["max_voltage"]);
			double min_v3 = ecoConvert.f2d(curRow["min_power"]);
			double max_v3 = ecoConvert.f2d(curRow["max_power"]);
			double min_v4 = ecoConvert.f2d(curRow["min_power_diss"]);
			double max_v4 = ecoConvert.f2d(curRow["max_power_diss"]);
			if (Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(1, num2, UIthEdidflg, min_v, max_v, ref Bankstatus);
			}
			if (Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(2, num, UIthEdidflg, min_v2, max_v2, ref Bankstatus);
			}
			if (Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(3, num3, UIthEdidflg, min_v3, max_v3, ref Bankstatus);
			}
			if (Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(4, num4, UIthEdidflg, min_v4, max_v4, ref Bankstatus);
			}
			if (Bankstatus == Color.Orange || Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(1, num2, UIthEdidflg, min_v, max_v, ref Bankstatus);
			}
			if (Bankstatus == Color.Orange || Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(2, num, UIthEdidflg, min_v2, max_v2, ref Bankstatus);
			}
			if (Bankstatus == Color.Orange || Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(3, num3, UIthEdidflg, min_v3, max_v3, ref Bankstatus);
			}
			if (Bankstatus == Color.Orange || Bankstatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(4, num4, UIthEdidflg, min_v4, max_v4, ref Bankstatus);
			}
		}
		private static void getLineThresholdColor(DataRow curRow, int UIthEdidflg, ref Color Linestatus)
		{
			if (Linestatus == Color.Red || Linestatus == Color.Silver)
			{
				return;
			}
			double num = ecoConvert.f2d(curRow["voltage_value"]);
			double num2 = ecoConvert.f2d(curRow["current_value"]);
			double num3 = ecoConvert.f2d(curRow["power_value"]);
			if (devcfgUtil.haveMeasureCurrent(UIthEdidflg) && num2 == -500.0)
			{
				Linestatus = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasureVoltage(UIthEdidflg) && num == -500.0)
			{
				Linestatus = Color.Red;
				return;
			}
			if (devcfgUtil.haveMeasurePower(UIthEdidflg) && num3 == -500.0)
			{
				Linestatus = Color.Red;
				return;
			}
			double min_v = ecoConvert.f2d(curRow["min_current"]);
			double max_v = ecoConvert.f2d(curRow["max_current"]);
			double min_v2 = ecoConvert.f2d(curRow["min_voltage"]);
			double max_v2 = ecoConvert.f2d(curRow["max_voltage"]);
			double min_v3 = ecoConvert.f2d(curRow["min_power"]);
			double max_v3 = ecoConvert.f2d(curRow["max_power"]);
			if (Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(1, num2, UIthEdidflg, min_v, max_v, ref Linestatus);
			}
			if (Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(2, num, UIthEdidflg, min_v2, max_v2, ref Linestatus);
			}
			if (Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Orange(3, num3, UIthEdidflg, min_v3, max_v3, ref Linestatus);
			}
			if (Linestatus == Color.Orange || Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(1, num2, UIthEdidflg, min_v, max_v, ref Linestatus);
			}
			if (Linestatus == Color.Orange || Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(2, num, UIthEdidflg, min_v2, max_v2, ref Linestatus);
			}
			if (Linestatus == Color.Orange || Linestatus == boardDataUtil.dft_COLOR)
			{
				boardDataUtil.commThresholdColor_Red(3, num3, UIthEdidflg, min_v3, max_v3, ref Linestatus);
			}
		}
		private static System.Collections.Generic.Dictionary<long, Color> DoorColorStatus(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, Color> dictionary = new System.Collections.Generic.Dictionary<long, Color>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select("doorsensor_status = '" + boardDataUtil.PDUData_Doorst_open + "'");
				if (array2.Length > 0)
				{
					dictionary.Add(rackInfo.RackID, Color.Orange);
				}
				else
				{
					array2 = dataTable.Select("doorsensor_status = '" + boardDataUtil.PDUData_Doorst_close + "'");
					if (array2.Length > 0)
					{
						dictionary.Add(rackInfo.RackID, boardDataUtil.dft_COLOR);
					}
					else
					{
						dictionary.Add(rackInfo.RackID, Color.Silver);
					}
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> AvailablePower(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			System.Collections.Generic.Dictionary<int, ClientAPI.DeviceWithZoneRackInfo> devicRackZoneRelation = ClientAPI.GetDevicRackZoneRelation();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_28_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"current_value <>'",
					-500,
					"' and current_value <>'",
					-1000,
					"'"
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					if (devicRackZoneRelation.ContainsKey(System.Convert.ToInt32(array2[k]["device_id"])))
					{
						ClientAPI.DeviceWithZoneRackInfo deviceWithZoneRackInfo = devicRackZoneRelation[System.Convert.ToInt32(array2[k]["device_id"])];
						string device_model = deviceWithZoneRackInfo.device_model;
						string fw_version = deviceWithZoneRackInfo.fw_version;
						DevModelConfig deviceModelConfig = DevAccessCfg.GetInstance().getDeviceModelConfig(device_model, fw_version);
						double num3 = devcfgUtil.ampMax(deviceModelConfig, "dev");
						if (num3 > 0.0)
						{
							flag = true;
							num += ecoConvert.f2d(array2[k]["current_value"]);
							num2 += devcfgUtil.ampMax(deviceModelConfig, "dev");
						}
					}
				}
				if (!flag)
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
				else
				{
					double num4 = (num2 - num) / num2 * 100.0;
					RackStatusAll.Total_Value += num4;
					dictionary.Add(rackInfo.RackID, num4.ToString("F2"));
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> heatLoadDissipation(System.Collections.ArrayList allRacks, ref string strPeriod, ref string strcurrentTime)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = boardDataUtil.DispatchAPI_CreateDictonary(RackStatusAll.Power_dissipation_period, allRacks, ref strPeriod, ref strcurrentTime);
			System.Collections.Generic.Dictionary<long, string> dictionary2 = new System.Collections.Generic.Dictionary<long, string>();
			foreach (System.Collections.Generic.KeyValuePair<long, string> current in dictionary)
			{
				string value = current.Value;
				if (value.Equals("Error"))
				{
					dictionary2.Add(current.Key, "Error");
				}
				else
				{
					double num = CultureTransfer.ToDouble(value);
					dictionary2.Add(current.Key, num.ToString());
					RackStatusAll.Total_Value += num;
				}
			}
			return dictionary2;
		}
		private static System.Collections.Generic.Dictionary<long, string> heatLoadDensityOSD(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				bool flag = false;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"power_value <>'",
					-500,
					"' and power_value <>'",
					-1000,
					"'"
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					num += ecoConvert.f2d(array2[k]["power_value"]);
				}
				if (flag)
				{
					RackStatusAll.Total_Value += num;
					dictionary.Add(rackInfo.RackID, num.ToString("F4"));
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> coldIntakeTemperature(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					num += ecoConvert.f2d(array2[k]["temperature"]);
					num2 += 1.0;
				}
				if (!flag)
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
				else
				{
					RackStatusAll.Total_Value += num / num2;
					dictionary.Add(rackInfo.RackID, (num / num2).ToString("F2"));
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> intakeDiffTemp(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					double num3 = ecoConvert.f2d(array2[k]["temperature"]);
					if (k == 0)
					{
						num = num3;
						num2 = num3;
					}
					else
					{
						if (num3 > num)
						{
							num = num3;
						}
						else
						{
							if (num3 < num2)
							{
								num2 = num3;
							}
						}
					}
				}
				if (flag)
				{
					RackStatusAll.Total_Value += num - num2;
					dictionary.Add(rackInfo.RackID, (num - num2).ToString("F2"));
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> exhaustTemperature(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					1
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					double num3 = ecoConvert.f2d(array2[k]["temperature"]);
					if (k == 0)
					{
						num = num3;
						num2 = num3;
					}
					else
					{
						if (num3 > num)
						{
							num = num3;
						}
						else
						{
							if (num3 < num2)
							{
								num2 = num3;
							}
						}
					}
				}
				if (flag)
				{
					RackStatusAll.Total_Value += num - num2;
					dictionary.Add(rackInfo.RackID, (num - num2).ToString("F2"));
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> hotExhaustTemperature(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				bool flag = false;
				int num2 = 0;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					1
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					num += ecoConvert.f2d(array2[k]["temperature"]);
					num2++;
				}
				if (!flag)
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
				else
				{
					RackStatusAll.Total_Value += num / (double)num2;
					dictionary.Add(rackInfo.RackID, (num / (double)num2).ToString("F2"));
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> coldHotAcrossTempRise(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = false;
				bool flag2 = false;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					double num3 = ecoConvert.f2d(array2[k]["temperature"]);
					if (k == 0)
					{
						num = num3;
					}
					else
					{
						if (num3 < num)
						{
							num = num3;
						}
					}
				}
				array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					1
				}));
				for (int l = 0; l < array2.Length; l++)
				{
					flag2 = true;
					double num3 = ecoConvert.f2d(array2[l]["temperature"]);
					if (l == 0)
					{
						num2 = num3;
					}
					else
					{
						if (num3 > num2)
						{
							num2 = num3;
						}
					}
				}
				if (flag && flag2)
				{
					RackStatusAll.Total_Value += num2 - num;
					dictionary.Add(rackInfo.RackID, (num2 - num).ToString("F2"));
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, string> intakeDiffPressure(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				int num2 = 0;
				bool flag = false;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"press_value <>'",
					-500,
					"' and press_value <>'",
					-1000,
					"' and sensor_location=",
					2
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					flag = true;
					num += ecoConvert.f2d(array2[k]["press_value"]);
					num2++;
				}
				if (flag)
				{
					RackStatusAll.Total_Value += num / (double)num2;
					dictionary.Add(rackInfo.RackID, (num / (double)num2).ToString("F2"));
				}
				else
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> heatLoadAirflowAcross(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary2 = boardDataUtil.RackCal_infor(allRacks, ds);
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary2)
			{
				RackStatusOne value = current.Value;
				if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.TEquipk_avg <= 0.0 || value.Power == 0.0)
				{
					dictionary.Add(current.Key, "Error");
				}
				else
				{
					dictionary.Add(current.Key, value.VEquipk.ToString("F2"));
					RackStatusAll.Total_Value += value.VEquipk;
				}
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> floorPlenumAirflow(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary2 = boardDataUtil.RackCal_infor(allRacks, ds);
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			int num5 = 0;
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary2)
			{
				RackStatusOne value = current.Value;
				num3 += value.Power;
			}
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary2)
			{
				RackStatusOne value = current2.Value;
				if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.FloorSSnum == 0 || value.TEquipk_avg <= 0.0 || value.TFloor_avg <= 0.0 || value.TFloor_avg - value.TIntake_diff <= 0.0 || value.Power == 0.0)
				{
					value.Power = 0.0;
				}
				else
				{
					num5++;
					num += (value.TFloor_avg - value.TIntake_diff) * (value.Power / num3);
					num2 += value.TEquipk * (value.Power / num3);
					num4 += value.VEquipk;
				}
			}
			if (num == 0.0 || num2 == 0.0)
			{
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current3 in dictionary2)
				{
					dictionary.Add(current3.Key, "Error");
				}
				RackStatusAll.fetal_error = true;
			}
			else
			{
				double num6 = num / num2;
				double num7 = num4 / num6;
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current4 in dictionary2)
				{
					RackStatusOne value = current4.Value;
					if (value.Power == 0.0)
					{
						dictionary.Add(current4.Key, "Error");
					}
					else
					{
						double num8 = num7 * value.Power / num3;
						dictionary.Add(current4.Key, num8.ToString("F4"));
						RackStatusAll.Total_Value += num8;
					}
				}
				RackStatusAll.fetal_error = false;
			}
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, RackStatusOne> RackCal_infor(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary = new System.Collections.Generic.Dictionary<long, RackStatusOne>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				DataTable dataTable = ds.Tables[0].Clone();
				DataRow[] array = ds.Tables[0].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"power_value <>'",
					-500,
					"' and power_value <>'",
					-1000,
					"' "
				}));
				for (int k = 0; k < array2.Length; k++)
				{
					num += ecoConvert.f2d(array2[k]["power_value"]);
				}
				double num2 = 0.0;
				double num3 = 0.0;
				double num4 = 0.0;
				double num5 = 0.0;
				double num6 = 0.0;
				double floor_Tavg = 0.0;
				DataTable dataTable2 = ds.Tables[1].Clone();
				DataRow[] array3 = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int l = 0; l < array3.Length; l++)
				{
					dataTable2.ImportRow(array3[l]);
				}
				array2 = dataTable2.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				double num7 = 0.0;
				int num8 = array2.Length;
				for (int m = 0; m < num8; m++)
				{
					double num9 = ecoConvert.f2d(array2[m]["temperature"]);
					num7 += num9;
					if (m == 0)
					{
						num2 = num9;
						num3 = num9;
					}
					else
					{
						if (num9 < num2)
						{
							num2 = num9;
						}
						else
						{
							if (num9 > num3)
							{
								num3 = num9;
							}
						}
					}
				}
				if (num8 > 0)
				{
					num4 = num7 / (double)num8;
				}
				array2 = dataTable2.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					1
				}));
				double num10 = 0.0;
				int num11 = array2.Length;
				for (int n = 0; n < num11; n++)
				{
					double num9 = ecoConvert.f2d(array2[n]["temperature"]);
					num10 += num9;
					if (n == 0)
					{
						num5 = num9;
					}
					if (num9 > num5)
					{
						num5 = num9;
					}
				}
				if (num11 > 0)
				{
					num6 = num10 / (double)num11;
				}
				array2 = dataTable2.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and sensor_location=",
					2
				}));
				double num12 = 0.0;
				int num13 = array2.Length;
				for (int num14 = 0; num14 < num13; num14++)
				{
					double num9 = ecoConvert.f2d(array2[num14]["temperature"]);
					num12 += num9;
				}
				if (num13 > 0)
				{
					floor_Tavg = num12 / (double)num13;
				}
				double num15 = num * 3412.3 / 1000.0;
				double vEquipk = num15 / ((num6 - num4) * 1.08);
				dictionary.Add(rackInfo.RackID, new RackStatusOne(rackInfo.GetDisplayRackName(EcoGlobalVar.RackFullNameFlag), num, num8, num2, num3, num4, num11, num5, num6, num13, floor_Tavg, vEquipk));
			}
			return dictionary;
		}
		private static System.Collections.Generic.Dictionary<long, string> hotRecirculation(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary2 = boardDataUtil.RackCal_infor(allRacks, ds);
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary2)
			{
				RackStatusOne value = current.Value;
				num3 += value.Power;
			}
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary2)
			{
				RackStatusOne value = current2.Value;
				if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.FloorSSnum == 0 || value.TEquipk_avg <= 0.0 || value.TFloor_avg <= 0.0 || value.TFloor_avg - value.TIntake_diff <= 0.0 || value.Power == 0.0)
				{
					value.Power = 0.0;
				}
				else
				{
					num += (value.TFloor_avg - value.TIntake_diff) * (value.Power / num3);
					num2 += value.TEquipk * (value.Power / num3);
					num4 += value.VEquipk;
				}
			}
			if (num == 0.0 || num2 == 0.0)
			{
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current3 in dictionary2)
				{
					dictionary.Add(current3.Key, "Error");
				}
				RackStatusAll.fetal_error = true;
			}
			else
			{
				double num5 = num / num2;
				double num6 = num4 / num5;
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current4 in dictionary2)
				{
					RackStatusOne value = current4.Value;
					if (value.Power == 0.0)
					{
						dictionary.Add(current4.Key, "Error");
					}
					else
					{
						double num7 = num6 * value.Power / num3;
						double num8;
						if (num7 < value.VEquipk)
						{
							num8 = (value.VEquipk - num7) * 100.0 / value.VEquipk;
							dictionary.Add(current4.Key, num8.ToString("F2"));
						}
						else
						{
							num8 = 0.0;
							dictionary.Add(current4.Key, "0");
						}
						RackStatusAll.Total_Value += num8;
					}
				}
				RackStatusAll.fetal_error = false;
			}
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, string> coldBypass(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			System.Collections.Generic.Dictionary<long, RackStatusOne> dictionary2 = boardDataUtil.RackCal_infor(allRacks, ds);
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current in dictionary2)
			{
				RackStatusOne value = current.Value;
				num3 += value.Power;
			}
			foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current2 in dictionary2)
			{
				RackStatusOne value = current2.Value;
				if (value.IntakeSSnum == 0 || value.ExhaustSSnum == 0 || value.FloorSSnum == 0 || value.TEquipk_avg <= 0.0 || value.TFloor_avg <= 0.0 || value.TFloor_avg - value.TIntake_diff <= 0.0 || value.Power == 0.0)
				{
					value.Power = 0.0;
				}
				else
				{
					num += (value.TFloor_avg - value.TIntake_diff) * (value.Power / num3);
					num2 += value.TEquipk * (value.Power / num3);
					num4 += value.VEquipk;
				}
			}
			if (num == 0.0 || num2 == 0.0)
			{
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current3 in dictionary2)
				{
					dictionary.Add(current3.Key, "Error");
				}
				RackStatusAll.fetal_error = true;
			}
			else
			{
				double num5 = num / num2;
				double num6 = num4 / num5;
				foreach (System.Collections.Generic.KeyValuePair<long, RackStatusOne> current4 in dictionary2)
				{
					RackStatusOne value = current4.Value;
					if (value.Power == 0.0)
					{
						dictionary.Add(current4.Key, "Error");
					}
					else
					{
						double num7 = num6 * value.Power / num3;
						double num8;
						if (value.VEquipk < num7)
						{
							num8 = (num7 - value.VEquipk) * 100.0 / num7;
							dictionary.Add(current4.Key, num8.ToString("F2"));
						}
						else
						{
							num8 = 0.0;
							dictionary.Add(current4.Key, "0");
						}
						RackStatusAll.Total_Value += num8;
					}
				}
				RackStatusAll.fetal_error = false;
			}
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, string> coldIntakeRelative(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"humidity <>'",
					-500,
					"' and humidity <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				int num2 = array2.Length;
				for (int k = 0; k < num2; k++)
				{
					num += ecoConvert.f2d(array2[k]["humidity"]);
				}
				if (num2 <= 0)
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
				else
				{
					RackStatusAll.Total_Value += num / (double)num2;
					dictionary.Add(rackInfo.RackID, (num / (double)num2).ToString("F2"));
				}
			}
			return dictionary;
		}
		public static System.Collections.Generic.Dictionary<long, string> coldIntakeDewPointTemperature(System.Collections.ArrayList allRacks, DataSet ds)
		{
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			for (int i = 0; i < allRacks.Count; i++)
			{
				RackInfo rackInfo = (RackInfo)allRacks[i];
				string arg_20_0 = rackInfo.DeviceInfo;
				double num = 0.0;
				DataTable dataTable = ds.Tables[1].Clone();
				DataRow[] array = ds.Tables[1].Select("rack_id= " + rackInfo.RackID);
				for (int j = 0; j < array.Length; j++)
				{
					dataTable.ImportRow(array[j]);
				}
				DataRow[] array2 = dataTable.Select(string.Concat(new object[]
				{
					"temperature <>'",
					-500,
					"' and temperature <>'",
					-1000,
					"' and humidity <>'",
					-500,
					"' and humidity <>'",
					-1000,
					"' and sensor_location=",
					0
				}));
				int num2 = array2.Length;
				for (int k = 0; k < num2; k++)
				{
					double num3 = ecoConvert.f2d(array2[k]["humidity"]);
					double num4 = ecoConvert.f2d(array2[k]["temperature"]);
					if (num3 == 0.0)
					{
						num += 0.0;
					}
					else
					{
						double num5 = 17.27 * num4 / (237.7 + num4) + System.Math.Log(num3 / 100.0, 2.7182818284590451);
						num += 237.7 * num5 / (17.27 - num5);
					}
				}
				if (num2 <= 0)
				{
					dictionary.Add(rackInfo.RackID, "Error");
				}
				else
				{
					RackStatusAll.Total_Value += num / (double)num2;
					dictionary.Add(rackInfo.RackID, (num / (double)num2).ToString("F2"));
				}
			}
			return dictionary;
		}
		public static DataTable DispatchAPI_CreateDataTable(string sendID, string sql)
		{
			commUtil.ShowInfo_DEBUG("DispatchAPI_CreateDataTable ---1 " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
			DataTable dataTable = new DataTable();
			dataTable = (DataTable)ClientAPI.RemoteCall(1, 1, sql, 10000);
			if (dataTable == null)
			{
				return new DataTable();
			}
			return dataTable;
		}
		public static System.Collections.Generic.Dictionary<long, string> DispatchAPI_CreateDictonary(int Power_dissipation_period, System.Collections.ArrayList allRacks, ref string strPeriod, ref string strcurrentTime)
		{
			commUtil.ShowInfo_DEBUG("DispatchAPI_CreateDictonary ---1 " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff"));
			strPeriod = "";
			strcurrentTime = "";
			System.Collections.Generic.Dictionary<long, string> dictionary = new System.Collections.Generic.Dictionary<long, string>();
			dictionary = (System.Collections.Generic.Dictionary<long, string>)ClientAPI.RemoteCall(2, 1, Power_dissipation_period.ToString(), 10000);
			if (dictionary == null)
			{
				strPeriod = System.DateTime.Now.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
				strcurrentTime = System.DateTime.Now.ToString("yyyy/MM/dd HH", System.Globalization.DateTimeFormatInfo.InvariantInfo);
				return new System.Collections.Generic.Dictionary<long, string>();
			}
			string text = dictionary[0L];
			string[] array = text.Split(new string[]
			{
				"#"
			}, System.StringSplitOptions.RemoveEmptyEntries);
			strPeriod = array[0];
			strcurrentTime = array[1];
			dictionary.Remove(0L);
			return dictionary;
		}
		private static void commThresholdColor_Orange(int measure_tp, double measure_v, int UIthEdidflg, double min_v, double max_v, ref Color ref_color)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			switch (measure_tp)
			{
			case 1:
				flag = devcfgUtil.haveMeasureCurrent(UIthEdidflg);
				num = 1;
				num2 = 2;
				min_v = -300.0;
				break;
			case 2:
				flag = devcfgUtil.haveMeasureVoltage(UIthEdidflg);
				num = 4;
				num2 = 8;
				break;
			case 3:
				flag = devcfgUtil.haveMeasurePower(UIthEdidflg);
				num = 16;
				num2 = 32;
				min_v = -300.0;
				break;
			case 4:
				flag = devcfgUtil.haveMeasurePowerD(UIthEdidflg);
				num = 64;
				num2 = 128;
				break;
			case 5:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 1, 2, min_v, max_v);
				num = 1;
				num2 = 2;
				break;
			case 6:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 4, 8, min_v, max_v);
				num = 4;
				num2 = 8;
				break;
			case 7:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 16, 32, min_v, max_v);
				num = 16;
				num2 = 32;
				break;
			}
			if (measure_v != -1000.0 && flag)
			{
				int num3 = 0;
				if ((UIthEdidflg & num) == 0 && min_v != -300.0 && min_v != -600.0)
				{
					num3 |= 1;
				}
				if ((UIthEdidflg & num2) == 0 && max_v != -300.0 && max_v != -600.0)
				{
					num3 |= 2;
				}
				if (num3 == 3)
				{
					if ((measure_v >= min_v && measure_v < RackStatusAll.MinWarningthreshold(max_v, min_v)) || (RackStatusAll.MaxWarningthreshold(max_v, min_v) < measure_v && measure_v <= max_v))
					{
						ref_color = Color.Orange;
						return;
					}
				}
				else
				{
					if (num3 == 1)
					{
						if (measure_v >= min_v && measure_v < 1.15 * min_v)
						{
							ref_color = Color.Orange;
							return;
						}
					}
					else
					{
						if (num3 == 2 && 0.85 * max_v < measure_v && measure_v <= max_v)
						{
							ref_color = Color.Orange;
						}
					}
				}
			}
		}
		private static void commThresholdColor_Red(int measure_tp, double measure_v, int UIthEdidflg, double min_v, double max_v, ref Color ref_color)
		{
			int num = 0;
			int num2 = 0;
			bool flag = false;
			switch (measure_tp)
			{
			case 1:
				flag = devcfgUtil.haveMeasureCurrent(UIthEdidflg);
				num = 1;
				num2 = 2;
				break;
			case 2:
				flag = devcfgUtil.haveMeasureVoltage(UIthEdidflg);
				num = 4;
				num2 = 8;
				break;
			case 3:
				flag = devcfgUtil.haveMeasurePower(UIthEdidflg);
				num = 16;
				num2 = 32;
				break;
			case 4:
				flag = devcfgUtil.haveMeasurePowerD(UIthEdidflg);
				num = 64;
				num2 = 128;
				break;
			case 5:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 1, 2, min_v, max_v);
				num = 1;
				num2 = 2;
				break;
			case 6:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 4, 8, min_v, max_v);
				num = 4;
				num2 = 8;
				break;
			case 7:
				flag = devcfgUtil.haveUIEditV(UIthEdidflg, 16, 32, min_v, max_v);
				num = 16;
				num2 = 32;
				break;
			}
			if (measure_v != -1000.0 && flag)
			{
				int num3 = 0;
				if ((UIthEdidflg & num) == 0 && min_v != -300.0 && min_v != -600.0)
				{
					num3 |= 1;
				}
				if ((UIthEdidflg & num2) == 0 && max_v != -300.0 && max_v != -600.0)
				{
					num3 |= 2;
				}
				if (num3 == 3)
				{
					if (measure_v > max_v || measure_v < min_v)
					{
						ref_color = Color.Red;
						return;
					}
				}
				else
				{
					if (num3 == 1)
					{
						if (measure_v < min_v)
						{
							ref_color = Color.Red;
							return;
						}
					}
					else
					{
						if (num3 == 2 && measure_v > max_v)
						{
							ref_color = Color.Red;
						}
					}
				}
			}
		}
	}
}
