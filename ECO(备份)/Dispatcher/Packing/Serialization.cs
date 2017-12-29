using Dispatcher;
using ecoProtocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
namespace Packing
{
	public class Serialization
	{
		public const int DESER_ST_WT_TBL_NAME_OR_CARRY = 0;
		public const int DESER_ST_WT_COL_NAME = 1;
		public const int DESER_ST_WT_COL_TYPE = 2;
		public const int DESER_ST_WT_COL_UNIQUE = 3;
		public const int DESER_ST_WT_COL_DEFAULE = 4;
		public const int DESER_ST_WT_TRUNK_INFO = 5;
		public const int DESER_ST_WT_TBL_HEADER_END = 6;
		public const int DESER_ST_COLLECT_ROWS = 7;
		public static object _bundleLock = new object();
		public static Dictionary<string, uint> _BundleUpdate = new Dictionary<string, uint>();
		public string _strCarriedInfo = "";
		public string _strDelimiter = ",";
		public int _dcLayout = -1;
		public int _stateDeserialize;
		public string _nameOfTable = "";
		public long _tblSequenceNo;
		public string _statusOfTable = "";
		public long _tStart;
		public string[] _colName;
		public string[] _colType;
		public string[] _colDefault;
		public string[] _colUnique;
		public string[] _trunkSizeList;
		public DataTable _tblReceiving;
		public DataTable _tblRackInfo;
		public DataTable _tblDevice2Rack;
		public DataTable _tblUserUAC;
		public DataTable _tblPuePair;
		public DataTable _sysParamRequest;
		public DataSet _dsDBBase;
		public DataSet _dsDBIncremental;
		public DataSet _dsRealtime;
		public DataSet _dsIncremental;
		public DataSet _dsSqlRequest;
		public DataTable _dtZoneInfo;
		public DataTable _dtGroupInfo;
		public DataTable _dtAutoModel;
		public object _lockAccessLock = new object();
		private static object _mergeLock = new object();
		public void StartDecompressThread(DispatchAttribute attrib)
		{
			int arg_08_0 = attrib.type;
			attrib.cbCallBack = new DispatchCallback(this.DeserializeReceived);
			Thread thread = new Thread(new ParameterizedThreadStart(this.DecompressorThread));
			thread.Name = "Decompressor Thread";
			thread.CurrentCulture = CultureInfo.InvariantCulture;
			thread.IsBackground = true;
			Common.ElapsedTime(attrib.tStart);
			thread.Start(attrib);
		}
		public void DecompressorThread(object context)
		{
			DispatchAttribute threadInfo = (DispatchAttribute)context;
			Compression.DecompressThread(threadInfo);
		}
		private void ResetAll()
		{
			lock (this._lockAccessLock)
			{
				this.ClearDeserialize();
			}
		}
		private void ClearDeserialize()
		{
			this._strDelimiter = ",";
			this._dcLayout = -1;
			this._stateDeserialize = 0;
			this._strCarriedInfo = "";
			this._tblSequenceNo = 0L;
			this._nameOfTable = "";
			this._statusOfTable = "";
			this._tStart = (long)Environment.TickCount;
			if (this._tblReceiving != null)
			{
				this._tblReceiving.Dispose();
			}
			this._tblReceiving = null;
			if (this._tblRackInfo != null)
			{
				this._tblRackInfo.Dispose();
			}
			this._tblRackInfo = null;
			if (this._tblDevice2Rack != null)
			{
				this._tblDevice2Rack.Dispose();
			}
			this._tblDevice2Rack = null;
			if (this._tblUserUAC != null)
			{
				this._tblUserUAC.Dispose();
			}
			this._tblUserUAC = null;
			if (this._tblPuePair != null)
			{
				this._tblPuePair.Dispose();
			}
			this._tblPuePair = null;
			if (this._dsDBBase != null)
			{
				this._dsDBBase.Dispose();
			}
			this._dsDBBase = null;
			if (this._dsDBIncremental != null)
			{
				this._dsDBIncremental.Dispose();
			}
			this._dsDBIncremental = null;
			if (this._dsRealtime != null)
			{
				this._dsRealtime.Dispose();
			}
			this._dsRealtime = null;
			if (this._dsIncremental != null)
			{
				this._dsIncremental.Dispose();
			}
			this._dsIncremental = null;
			if (this._dsSqlRequest != null)
			{
				this._dsSqlRequest.Dispose();
			}
			this._dsSqlRequest = null;
			if (this._sysParamRequest != null)
			{
				this._sysParamRequest.Dispose();
			}
			this._sysParamRequest = null;
			if (this._dtZoneInfo != null)
			{
				this._dtZoneInfo.Dispose();
			}
			this._dtZoneInfo = null;
			if (this._dtGroupInfo != null)
			{
				this._dtGroupInfo.Dispose();
			}
			this._dtGroupInfo = null;
			if (this._dtAutoModel != null)
			{
				this._dtAutoModel.Dispose();
			}
			this._dtAutoModel = null;
		}
		private void DeserializeLine(string strLine)
		{
			if (strLine == null)
			{
				return;
			}
			if (this._stateDeserialize == 0 && strLine.Length > 6)
			{
				if (strLine.Substring(0, 6).Equals("######", StringComparison.CurrentCultureIgnoreCase))
				{
					this._strDelimiter = strLine.Substring(6, 1);
				}
				if (strLine.Substring(0, 6).Equals("//////", StringComparison.CurrentCultureIgnoreCase))
				{
					if (this._strCarriedInfo != null && this._strCarriedInfo != "")
					{
						this._strCarriedInfo += "\r\n";
					}
					this._strCarriedInfo += strLine.Substring(6);
				}
			}
			string[] separator = new string[]
			{
				this._strDelimiter
			};
			if (this._stateDeserialize == 0)
			{
				if (strLine == "")
				{
					return;
				}
				string[] array = strLine.Split(separator, StringSplitOptions.None);
				if (array.Length > 0)
				{
					if (!array[0].Equals("######", StringComparison.CurrentCultureIgnoreCase))
					{
						return;
					}
					if (array.Length > 1)
					{
						this._nameOfTable = array[1].Trim();
					}
					if (array.Length > 2)
					{
						this._statusOfTable = array[2].Trim();
					}
					this._stateDeserialize = 1;
					if (this._nameOfTable.Equals("RackInfo", StringComparison.CurrentCultureIgnoreCase))
					{
						if (array.Length > 3)
						{
							this._dcLayout = Convert.ToInt32(array[3]);
							Common.WriteLine("    Rack Layout = {0}", new string[]
							{
								this._dcLayout.ToString()
							});
						}
					}
					else
					{
						if (array.Length > 3)
						{
							this._tblSequenceNo = Convert.ToInt64(array[3]);
						}
					}
				}
				if (this._statusOfTable.Equals("null", StringComparison.CurrentCultureIgnoreCase))
				{
					this._tblReceiving = new DataTable();
					this._stateDeserialize = 7;
					return;
				}
				if (this._statusOfTable.Equals("nocolumn", StringComparison.CurrentCultureIgnoreCase))
				{
					this._tblReceiving = new DataTable();
					this._stateDeserialize = 7;
					return;
				}
			}
			else
			{
				if (this._stateDeserialize == 1)
				{
					this._colName = strLine.Split(separator, StringSplitOptions.None);
					this._stateDeserialize = 2;
					return;
				}
				if (this._stateDeserialize == 2)
				{
					this._colType = strLine.Split(separator, StringSplitOptions.None);
					this._stateDeserialize = 3;
					return;
				}
				if (this._stateDeserialize == 3)
				{
					this._colUnique = strLine.Split(separator, StringSplitOptions.None);
					this._stateDeserialize = 4;
					return;
				}
				if (this._stateDeserialize == 4)
				{
					this._colDefault = strLine.Split(separator, StringSplitOptions.None);
					this._stateDeserialize = 5;
					return;
				}
				if (this._stateDeserialize == 5)
				{
					this._trunkSizeList = null;
					if (!string.IsNullOrEmpty(strLine) && strLine != "-")
					{
						this._trunkSizeList = strLine.Split(separator, StringSplitOptions.RemoveEmptyEntries);
						if (this._trunkSizeList.Length <= 1)
						{
							this._trunkSizeList = null;
						}
					}
					this._stateDeserialize = 6;
					return;
				}
				if (this._stateDeserialize == 6)
				{
					if (strLine == "")
					{
						if (this._tblReceiving != null)
						{
							this._tblReceiving.Dispose();
						}
						try
						{
							Dictionary<int, string> dictionary = new Dictionary<int, string>();
							this._tblReceiving = new DataTable();
							this._tblReceiving.TableName = this._nameOfTable.Replace("rt_", "");
							for (int i = 0; i < this._colName.Length; i++)
							{
								DataColumn dataColumn = new DataColumn();
								dataColumn.DataType = Type.GetType("System." + this._colType[i]);
								dataColumn.ColumnName = this._colName[i];
								if (this._colDefault[i] != "")
								{
									dataColumn.DefaultValue = this._colDefault[i];
								}
								this._tblReceiving.Columns.Add(dataColumn);
								if (Convert.ToInt32(this._colUnique[i]) > 0)
								{
									dictionary.Add(Convert.ToInt32(this._colUnique[i]), this._colName[i]);
								}
							}
							if (dictionary.Count > 0)
							{
								DataColumn[] array2 = new DataColumn[dictionary.Count];
								for (int j = 0; j < dictionary.Count; j++)
								{
									if (dictionary.ContainsKey(j + 1))
									{
										array2[j] = this._tblReceiving.Columns[dictionary[j + 1]];
									}
								}
								this._tblReceiving.PrimaryKey = array2;
							}
						}
						catch (Exception ex)
						{
							if (this._tblReceiving != null)
							{
								this._tblReceiving.Dispose();
							}
							this._tblReceiving = null;
							Common.WriteLine("    DeserializeLine: create datatable, {0}, {1}", new string[]
							{
								strLine,
								ex.Message
							});
						}
					}
					this._stateDeserialize = 7;
					return;
				}
				if (this._stateDeserialize == 7)
				{
					if (strLine == "")
					{
						if (this._tblReceiving != null)
						{
							if (this._nameOfTable.Length > 10 && this._nameOfTable.Substring(0, 10).Equals("AUTOMODEL_", StringComparison.CurrentCultureIgnoreCase))
							{
								if (this._dtAutoModel == null)
								{
									this._dtAutoModel = new DataTable();
								}
								this._tblReceiving.TableName = this._nameOfTable.Substring(10);
								this._dtAutoModel = this._tblReceiving;
							}
							else
							{
								if (this._nameOfTable.Length > 10 && this._nameOfTable.Substring(0, 10).Equals("ZONE_INFO_", StringComparison.CurrentCultureIgnoreCase))
								{
									if (this._dtZoneInfo == null)
									{
										this._dtZoneInfo = new DataTable();
									}
									this._tblReceiving.TableName = this._nameOfTable.Substring(10);
									this._dtZoneInfo = this._tblReceiving;
								}
								else
								{
									if (this._nameOfTable.Length > 11 && this._nameOfTable.Substring(0, 11).Equals("GROUP_INFO_", StringComparison.CurrentCultureIgnoreCase))
									{
										if (this._dtGroupInfo == null)
										{
											this._dtGroupInfo = new DataTable();
										}
										this._tblReceiving.TableName = this._nameOfTable.Substring(11);
										this._dtGroupInfo = this._tblReceiving;
									}
									else
									{
										if (this._nameOfTable.Length > 10 && this._nameOfTable.Substring(0, 10).Equals("SYS_PARAM_", StringComparison.CurrentCultureIgnoreCase))
										{
											if (this._sysParamRequest == null)
											{
												this._sysParamRequest = new DataTable();
											}
											this._tblReceiving.TableName = this._nameOfTable.Substring(10);
											this._sysParamRequest = this._tblReceiving;
										}
										else
										{
											if (this._nameOfTable.Length > 10 && this._nameOfTable.Substring(0, 10).Equals("HEAT_LOAD_", StringComparison.CurrentCultureIgnoreCase))
											{
												if (this._dsSqlRequest == null)
												{
													this._dsSqlRequest = new DataSet();
												}
												this._tblReceiving.TableName = this._nameOfTable.Substring(10);
												this._dsSqlRequest.Tables.Add(this._tblReceiving);
											}
											else
											{
												if (this._nameOfTable.Length > 8 && this._nameOfTable.Substring(0, 8).Equals("SQL_REQ_", StringComparison.CurrentCultureIgnoreCase))
												{
													if (this._dsSqlRequest == null)
													{
														this._dsSqlRequest = new DataSet();
													}
													this._tblReceiving.TableName = this._nameOfTable.Substring(8);
													this._dsSqlRequest.Tables.Add(this._tblReceiving);
												}
												else
												{
													if (this._nameOfTable.Equals("UserUAC", StringComparison.CurrentCultureIgnoreCase))
													{
														if (this._tblUserUAC != null)
														{
															this._tblUserUAC.Dispose();
														}
														this._tblUserUAC = this._tblReceiving;
													}
													else
													{
														if (this._nameOfTable.Equals("Device2Rack", StringComparison.CurrentCultureIgnoreCase))
														{
															if (this._tblDevice2Rack != null)
															{
																this._tblDevice2Rack.Dispose();
															}
															this._tblDevice2Rack = this._tblReceiving;
														}
														else
														{
															if (this._nameOfTable.Equals("RackInfo", StringComparison.CurrentCultureIgnoreCase))
															{
																if (this._tblRackInfo != null)
																{
																	this._tblRackInfo.Dispose();
																}
																this._tblRackInfo = this._tblReceiving;
															}
															else
															{
																if (this._nameOfTable.Equals("rt_value_pair", StringComparison.CurrentCultureIgnoreCase))
																{
																	if (this._tblPuePair != null)
																	{
																		this._tblPuePair.Dispose();
																	}
																	this._tblPuePair = this._tblReceiving;
																}
																else
																{
																	if (this._nameOfTable.Equals("device", StringComparison.CurrentCultureIgnoreCase))
																	{
																		if (this._dsDBBase != null)
																		{
																			this._dsDBBase.Dispose();
																		}
																		this._dsDBBase = new DataSet();
																		if (this._dsDBBase != null)
																		{
																			this._dsDBBase.Tables.Add(this._tblReceiving);
																		}
																	}
																	else
																	{
																		if (this._nameOfTable.Equals("sensor", StringComparison.CurrentCultureIgnoreCase))
																		{
																			if (this._dsDBBase != null)
																			{
																				this._dsDBBase.Tables.Add(this._tblReceiving);
																			}
																		}
																		else
																		{
																			if (this._nameOfTable.Equals("port", StringComparison.CurrentCultureIgnoreCase))
																			{
																				if (this._dsDBBase != null)
																				{
																					this._dsDBBase.Tables.Add(this._tblReceiving);
																				}
																			}
																			else
																			{
																				if (this._nameOfTable.Equals("bank", StringComparison.CurrentCultureIgnoreCase))
																				{
																					if (this._dsDBBase != null)
																					{
																						this._dsDBBase.Tables.Add(this._tblReceiving);
																					}
																				}
																				else
																				{
																					if (this._nameOfTable.Equals("line", StringComparison.CurrentCultureIgnoreCase))
																					{
																						if (this._dsDBBase != null)
																						{
																							this._dsDBBase.Tables.Add(this._tblReceiving);
																						}
																					}
																					else
																					{
																						if (!this._nameOfTable.Equals("add_delete", StringComparison.CurrentCultureIgnoreCase))
																						{
																							if (this._nameOfTable.Equals("dy_device", StringComparison.CurrentCultureIgnoreCase))
																							{
																								if (this._dsDBIncremental != null)
																								{
																									this._dsDBIncremental.Dispose();
																								}
																								this._dsDBIncremental = new DataSet();
																								if (this._dsDBIncremental != null)
																								{
																									this._dsDBIncremental.Tables.Add(this._tblReceiving);
																								}
																							}
																							else
																							{
																								if (this._nameOfTable.Equals("dy_sensor", StringComparison.CurrentCultureIgnoreCase))
																								{
																									if (this._dsDBIncremental != null)
																									{
																										this._dsDBIncremental.Tables.Add(this._tblReceiving);
																									}
																								}
																								else
																								{
																									if (this._nameOfTable.Equals("dy_port", StringComparison.CurrentCultureIgnoreCase))
																									{
																										if (this._dsDBIncremental != null)
																										{
																											this._dsDBIncremental.Tables.Add(this._tblReceiving);
																										}
																									}
																									else
																									{
																										if (this._nameOfTable.Equals("dy_bank", StringComparison.CurrentCultureIgnoreCase))
																										{
																											if (this._dsDBIncremental != null)
																											{
																												this._dsDBIncremental.Tables.Add(this._tblReceiving);
																											}
																										}
																										else
																										{
																											if (this._nameOfTable.Equals("dy_line", StringComparison.CurrentCultureIgnoreCase))
																											{
																												if (this._dsDBIncremental != null)
																												{
																													this._dsDBIncremental.Tables.Add(this._tblReceiving);
																												}
																											}
																											else
																											{
																												if (!this._nameOfTable.Equals("dy_add_delete", StringComparison.CurrentCultureIgnoreCase))
																												{
																													if (this._nameOfTable.Equals("rt_device", StringComparison.CurrentCultureIgnoreCase))
																													{
																														if (this._dsRealtime != null)
																														{
																															this._dsRealtime.Dispose();
																														}
																														this._dsRealtime = new DataSet();
																														this._tblReceiving.PrimaryKey = new DataColumn[]
																														{
																															this._tblReceiving.Columns["device_id"]
																														};
																														this._dsRealtime.Tables.Add(this._tblReceiving);
																													}
																													else
																													{
																														if (this._nameOfTable.Equals("rt_sensor", StringComparison.CurrentCultureIgnoreCase))
																														{
																															if (this._dsRealtime != null)
																															{
																																this._tblReceiving.PrimaryKey = new DataColumn[]
																																{
																																	this._tblReceiving.Columns["device_id"],
																																	this._tblReceiving.Columns["sensor_type"]
																																};
																																this._dsRealtime.Tables.Add(this._tblReceiving);
																															}
																														}
																														else
																														{
																															if (this._nameOfTable.Equals("rt_port", StringComparison.CurrentCultureIgnoreCase))
																															{
																																if (this._dsRealtime != null)
																																{
																																	this._tblReceiving.PrimaryKey = new DataColumn[]
																																	{
																																		this._tblReceiving.Columns["device_id"],
																																		this._tblReceiving.Columns["port_number"],
																																		this._tblReceiving.Columns["port_id"]
																																	};
																																	this._dsRealtime.Tables.Add(this._tblReceiving);
																																}
																															}
																															else
																															{
																																if (this._nameOfTable.Equals("rt_bank", StringComparison.CurrentCultureIgnoreCase))
																																{
																																	if (this._dsRealtime != null)
																																	{
																																		this._tblReceiving.PrimaryKey = new DataColumn[]
																																		{
																																			this._tblReceiving.Columns["device_id"],
																																			this._tblReceiving.Columns["bank_id"]
																																		};
																																		this._dsRealtime.Tables.Add(this._tblReceiving);
																																	}
																																}
																																else
																																{
																																	if (this._nameOfTable.Equals("rt_line", StringComparison.CurrentCultureIgnoreCase))
																																	{
																																		if (this._dsRealtime != null)
																																		{
																																			this._tblReceiving.PrimaryKey = new DataColumn[]
																																			{
																																				this._tblReceiving.Columns["device_id"],
																																				this._tblReceiving.Columns["line_id"]
																																			};
																																			this._dsRealtime.Tables.Add(this._tblReceiving);
																																		}
																																	}
																																	else
																																	{
																																		if (this._nameOfTable.Equals(DataSetManager.tb_udDevice, StringComparison.CurrentCultureIgnoreCase))
																																		{
																																			if (this._dsIncremental != null)
																																			{
																																				this._dsIncremental.Dispose();
																																			}
																																			this._dsIncremental = new DataSet();
																																			this._tblReceiving.PrimaryKey = new DataColumn[]
																																			{
																																				this._tblReceiving.Columns["device_id"]
																																			};
																																			this._dsIncremental.Tables.Add(this._tblReceiving);
																																		}
																																		else
																																		{
																																			if (this._nameOfTable.Equals(DataSetManager.tb_udSensor, StringComparison.CurrentCultureIgnoreCase))
																																			{
																																				if (this._dsIncremental != null)
																																				{
																																					this._tblReceiving.PrimaryKey = new DataColumn[]
																																					{
																																						this._tblReceiving.Columns["device_id"],
																																						this._tblReceiving.Columns["sensor_type"]
																																					};
																																					this._dsIncremental.Tables.Add(this._tblReceiving);
																																				}
																																			}
																																			else
																																			{
																																				if (this._nameOfTable.Equals(DataSetManager.tb_udPort, StringComparison.CurrentCultureIgnoreCase))
																																				{
																																					if (this._dsIncremental != null)
																																					{
																																						this._tblReceiving.PrimaryKey = new DataColumn[]
																																						{
																																							this._tblReceiving.Columns["device_id"],
																																							this._tblReceiving.Columns["port_number"],
																																							this._tblReceiving.Columns["port_id"]
																																						};
																																						this._dsIncremental.Tables.Add(this._tblReceiving);
																																					}
																																				}
																																				else
																																				{
																																					if (this._nameOfTable.Equals(DataSetManager.tb_udBank, StringComparison.CurrentCultureIgnoreCase))
																																					{
																																						if (this._dsIncremental != null)
																																						{
																																							this._tblReceiving.PrimaryKey = new DataColumn[]
																																							{
																																								this._tblReceiving.Columns["device_id"],
																																								this._tblReceiving.Columns["bank_id"]
																																							};
																																							this._dsIncremental.Tables.Add(this._tblReceiving);
																																						}
																																					}
																																					else
																																					{
																																						if (this._nameOfTable.Equals(DataSetManager.tb_udLine, StringComparison.CurrentCultureIgnoreCase) && this._dsIncremental != null)
																																						{
																																							this._tblReceiving.PrimaryKey = new DataColumn[]
																																							{
																																								this._tblReceiving.Columns["device_id"],
																																								this._tblReceiving.Columns["line_id"]
																																							};
																																							this._dsIncremental.Tables.Add(this._tblReceiving);
																																						}
																																					}
																																				}
																																			}
																																		}
																																	}
																																}
																															}
																														}
																													}
																												}
																											}
																										}
																									}
																								}
																							}
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
							long num = Common.ElapsedTime(this._tStart);
							if (this._tblSequenceNo > 0L)
							{
								Common.WriteLine("    DeserializeLine: saved datatable #{0} [{1}], row={2}, Elapsed={3}", new string[]
								{
									this._tblSequenceNo.ToString(),
									this._nameOfTable,
									this._tblReceiving.Rows.Count.ToString(),
									num.ToString()
								});
							}
							else
							{
								Common.WriteLine("    DeserializeLine: saved datatable [{0}], row={1}, Elapsed={2}", new string[]
								{
									this._nameOfTable,
									this._tblReceiving.Rows.Count.ToString(),
									num.ToString()
								});
							}
						}
						this._stateDeserialize = 0;
						this._statusOfTable = "";
						this._nameOfTable = null;
						this._tblReceiving = null;
						this._trunkSizeList = null;
						this._colName = null;
						this._colType = null;
						this._colDefault = null;
						this._colUnique = null;
						return;
					}
					if (this._tblReceiving != null)
					{
						try
						{
							string[] array3 = strLine.Split(separator, StringSplitOptions.None);
							DataRow dataRow = this._tblReceiving.NewRow();
							for (int k = 0; k < Math.Min(this._tblReceiving.Columns.Count, array3.Length); k++)
							{
								if (this._tblReceiving.Columns[k].DataType == Type.GetType("System.DateTime"))
								{
									DateTime dateTime = DateTime.Parse(array3[k]);
									dataRow[k] = dateTime;
								}
								else
								{
									if (array3[k] != "")
									{
										dataRow[k] = array3[k];
									}
								}
							}
							this._tblReceiving.Rows.Add(dataRow);
						}
						catch (Exception ex2)
						{
							Common.WriteLine("    DeserializeLine: add row, {0}, {1} ", new string[]
							{
								strLine,
								ex2.Message
							});
						}
					}
				}
			}
		}
		private void TrunkToTableThread(object obj)
		{
			string text = "";
			TrunkToTable trunkToTable = (TrunkToTable)obj;
			try
			{
				while (true)
				{
					text = this.getLine(trunkToTable.data, trunkToTable.nEndPos, ref trunkToTable.nextReadPos);
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					string[] array = text.Split(trunkToTable.separators, StringSplitOptions.None);
					DataRow dataRow = trunkToTable.dt.NewRow();
					for (int i = 0; i < Math.Min(trunkToTable.dt.Columns.Count, array.Length); i++)
					{
						if (trunkToTable.dt.Columns[i].DataType == Type.GetType("System.DateTime"))
						{
							DateTime dateTime = DateTime.Parse(array[i]);
							dataRow[i] = dateTime;
						}
						else
						{
							if (array[i] != "")
							{
								dataRow[i] = array[i];
							}
						}
					}
					trunkToTable.dt.Rows.Add(dataRow);
				}
				trunkToTable.evtDone.Set();
			}
			catch (Exception ex)
			{
				Common.WriteLine("    Trunk Thread [{0}]: add row, {1}, {2} ", new string[]
				{
					Thread.CurrentThread.Name,
					text,
					ex.Message
				});
			}
		}
		private string getLine(byte[] data, int nEndPos, ref int nextReadPos)
		{
			if (nextReadPos == nEndPos)
			{
				return null;
			}
			string result = "";
			int index = nextReadPos;
			int num = 0;
			for (int i = nextReadPos; i < data.Length; i++)
			{
				nextReadPos++;
				if (data[i] == 10)
				{
					break;
				}
				num++;
			}
			if (num > 0)
			{
				result = Encoding.UTF8.GetString(data, index, num);
			}
			return result;
		}
		private void DeserializeReceived(DispatchAttribute attrib)
		{
			int num = 0;
			int num2 = 0;
			if (attrib.type == 1024)
			{
				ClientAPI.setRemoteCallResult(attrib.cid, attrib.data);
				return;
			}
			lock (this._lockAccessLock)
			{
				try
				{
					this.ClearDeserialize();
					while (true)
					{
						string line = this.getLine(attrib.data, attrib.data.Length, ref num2);
						if (line == null)
						{
							break;
						}
						this.DeserializeLine(line);
						if (this._stateDeserialize == 7 && this._trunkSizeList != null && this._trunkSizeList.Length > 1)
						{
							ManualResetEvent[] array = new ManualResetEvent[this._trunkSizeList.Length];
							TrunkToTable[] array2 = new TrunkToTable[this._trunkSizeList.Length];
							Thread[] array3 = new Thread[this._trunkSizeList.Length];
							for (int i = 0; i < this._trunkSizeList.Length; i++)
							{
								array[i] = new ManualResetEvent(false);
								array2[i] = new TrunkToTable();
								array2[i].data = attrib.data;
								array2[i].nextReadPos = num2;
								array2[i].nEndPos = num2 + Convert.ToInt32(this._trunkSizeList[i]);
								num2 = array2[i].nEndPos;
								array2[i].separators = new string[]
								{
									this._strDelimiter
								};
								array2[i].evtDone = array[i];
								array2[i].dt = this._tblReceiving.Clone();
								array3[i] = new Thread(new ParameterizedThreadStart(this.TrunkToTableThread));
								array3[i].Name = this._nameOfTable + "-TrunkThread-#" + i;
								array3[i].CurrentCulture = CultureInfo.InvariantCulture;
								array3[i].IsBackground = true;
								array3[i].Start(array2[i]);
							}
							WaitHandle.WaitAll(array);
							for (int j = 0; j < this._trunkSizeList.Length; j++)
							{
								this._tblReceiving.Merge(array2[j].dt);
							}
							this.DeserializeLine("");
						}
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine("    Deserialize: {0}", new string[]
					{
						ex.Message
					});
				}
				lock (Serialization._mergeLock)
				{
					num = ClientAPI.MergeToWorkingDataset(this, attrib);
				}
				long num3 = Common.ElapsedTime(this._tStart);
				Common.WriteLine("Merge End: type={0}, size={1}, Elapsed={2}", new string[]
				{
					attrib.type.ToString("X8"),
					attrib.data.Length.ToString(),
					num3.ToString()
				});
			}
			string operation = "";
			string carried = "";
			string text = "";
			string text2 = "";
			this._strCarriedInfo = this._strCarriedInfo.Replace("\r", "");
			this._strCarriedInfo = this._strCarriedInfo.Replace("\n", "");
			if (!string.IsNullOrEmpty(this._strCarriedInfo))
			{
				string[] array4 = this._strCarriedInfo.Split(new char[]
				{
					'|'
				});
				if (array4.Length > 0)
				{
					operation = array4[0];
				}
				if (array4.Length > 1)
				{
					carried = array4[1];
				}
				if (array4.Length > 2)
				{
					text = array4[2];
				}
				if (array4.Length > 3)
				{
					text2 = array4[3];
				}
			}
			bool flag3 = true;
			if (!string.IsNullOrEmpty(text))
			{
				flag3 = false;
				lock (Serialization._bundleLock)
				{
					if (!Serialization._BundleUpdate.ContainsKey(text))
					{
						Serialization._BundleUpdate.Add(text, 0u);
					}
					Dictionary<string, uint> bundleUpdate;
					string key;
					(bundleUpdate = Serialization._BundleUpdate)[key = text] = (bundleUpdate[key] | Convert.ToUInt32(num));
					if ((Serialization._BundleUpdate[text] & Convert.ToUInt32(text2)) == Convert.ToUInt32(text2))
					{
						num = Convert.ToInt32(Serialization._BundleUpdate[text]);
						flag3 = true;
						Serialization._BundleUpdate.Remove(text);
					}
					else
					{
						Common.WriteLine("Bundle partially received ({0}) ---> received:{1}, expected:{2}", new string[]
						{
							text,
							Serialization._BundleUpdate[text].ToString("X8"),
							Convert.ToUInt32(text2).ToString("X8")
						});
					}
				}
			}
			if (flag3 && num != 0)
			{
				if ((num & 128) != 0)
				{
					num |= 2;
				}
				if ((num & 64) != 0)
				{
					num |= 1;
				}
				ClientAPI.OnReceivedBroadcast((ulong)((long)num), operation, carried, text, text2);
			}
			this._sysParamRequest = null;
		}
	}
}
