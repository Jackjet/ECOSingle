using CommonAPI.Global;
using CommonAPI.InterProcess;
using CommonAPI.network;
using CommonAPI.Timers;
using DBAccessAPI;
using DBAccessAPI.user;
using Dispatcher;
using EcoDevice.AccessAPI;
using ecoProtocols;
using EventLogAPI;
using Packing;
using SessionManager;
using SocketClient;
using SocketServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
namespace EcoMessenger
{
	public class EcoHandler : MessageBase
	{
		public class SerializeContext
		{
			public ecoMessage msg;
			public DispatchAttribute attrib;
			public SerializeContext()
			{
				this.msg = null;
				this.attrib = null;
			}
		}
		public class SerializeTableContext
		{
			public string name;
			public DataTable dt;
			public int nThreads;
			public int idxRowFrom;
			public int nRowCount;
			public MyMemoryStream outStream;
			public ManualResetEvent evtDone;
			public SerializeTableContext()
			{
				this.name = "";
				this.idxRowFrom = 0;
				this.nRowCount = 0;
				this.dt = null;
				this.outStream = null;
				this.evtDone = null;
			}
		}
		public delegate void DelegateOnSocketClosed(ConnectContext context, int reason);
		private const int _trunck_size = 8192;
		private static long _bsDiffTableAmountLimit = 200L;
		private static long _bsDiffTableTimerLimit = 1800000L;
		private static long _bsDiffTableMaxDelay = 10000L;
		private static long _dbStatusCode = -1L;
		private static int _isCompressing_RT = 0;
		private static TickTimer _tLastRealTimeDispatch = new TickTimer();
		private int _dcLayout = 2;
		private Dictionary<long, CommParaClass> _rackInfo;
		private DataSet _baseDataSet;
		private DataSet _baseIncremental;
		private long _tLastBaseDataSet = (long)Environment.TickCount;
		private long _tLastBaseIncremental = (long)Environment.TickCount;
		private int _compressAlgorithm = 1;
		private byte[] _baseCompressed;
		private byte[] _realtimeCompressed;
		private DataSet _realtimeDataSet;
		private DataSet _realtimeIncremental;
		private long _tLastRtDataSet = (long)Environment.TickCount;
		private long _tLastRtIncremental = (long)Environment.TickCount;
		private System.Timers.Timer _pueTimer;
		private System.Timers.Timer _diffTimer;
		private int _diffDelay;
		public static string _macMismatchList = "";
		public static object _lockAutoModel = new object();
		public static Dictionary<string, DevModelConfig> _autoModelSink = null;
		public EcoHandler.DelegateOnSocketClosed cbOnClosed;
		private void DiffTable_TimedEvent(object source, ElapsedEventArgs e)
		{
			bool bAll = false;
			lock (this._lockHandler)
			{
				long num = Common.ElapsedTime(this._tLastBaseDataSet);
				if (num >= EcoHandler._bsDiffTableTimerLimit)
				{
					bAll = true;
					this._tLastBaseDataSet = (long)Environment.TickCount;
					Common.WriteLine("    Diff. Dataset life-time update", new string[0]);
				}
				else
				{
					if (this._diffDelay <= 0)
					{
						return;
					}
				}
				Interlocked.Exchange(ref this._diffDelay, 0);
			}
			this.DispatchDiffTable(bAll);
		}
		private void PUE_TimedEvent(object source, ElapsedEventArgs e)
		{
			if (EcoHandler._dbStatusCode > 0L)
			{
				this.DispatchPUE();
			}
		}
		public void UpdateDBStatus(long statusCode)
		{
			if (statusCode > 0L)
			{
				this.OnDBReady();
				this.FirstDispatch(null);
			}
		}
		public override void OnDBReady()
		{
			if (this._isServer)
			{
				lock (this._lockHandler)
				{
					this._pueTimer = new System.Timers.Timer(10000.0);
					this._pueTimer.Elapsed += new ElapsedEventHandler(this.PUE_TimedEvent);
					this._pueTimer.Interval = (double)(1000 * Sys_Para.GetServiceDelay());
					this._pueTimer.Enabled = true;
					this._diffTimer = new System.Timers.Timer((double)EcoHandler._bsDiffTableMaxDelay);
					this._diffTimer.Elapsed += new ElapsedEventHandler(this.DiffTable_TimedEvent);
					this._diffTimer.Interval = (double)EcoHandler._bsDiffTableMaxDelay;
					this._diffTimer.Enabled = true;
					this._diffDelay = 0;
				}
			}
		}
		public override void AttachDispatcher(IConnectInterface comObj)
		{
			lock (this._lockHandler)
			{
				this._lstConnectors.Add(comObj);
			}
		}
		public override void DetachDispatcher(IConnectInterface comObj)
		{
			lock (this._lockHandler)
			{
				if (this._lstConnectors.Contains(comObj))
				{
					this._lstConnectors.Remove(comObj);
				}
			}
		}
		public override void ToAllDispatcher(DispatchAttribute attrib)
		{
			foreach (IConnectInterface current in this._lstConnectors)
			{
				current.DispatchDataset(attrib);
			}
		}
		public void UpdateCompressedData(DispatchAttribute attr)
		{
			lock (this._lockHandler)
			{
				try
				{
					if (attr != null && attr.data != null && attr.uid == 0)
					{
						if (attr.type == 2)
						{
							this._baseCompressed = new byte[attr.data.Length];
							attr.data.CopyTo(this._baseCompressed, 0);
							this._compressAlgorithm = attr.algorithm;
							Common.WriteLine("Updated BaseDB with compressed data, size={0}", new string[]
							{
								this._baseCompressed.Length.ToString()
							});
							EcoHandler._dbStatusCode = 1L;
						}
						else
						{
							if (attr.type == 1)
							{
								this._realtimeCompressed = new byte[attr.data.Length];
								attr.data.CopyTo(this._realtimeCompressed, 0);
								this._compressAlgorithm = attr.algorithm;
								Common.WriteLine("Update Realtime Data with compressed data, size={0}", new string[]
								{
									this._realtimeCompressed.Length.ToString()
								});
							}
						}
					}
				}
				catch (Exception ex)
				{
					Common.WriteLine(ex.Message, new string[0]);
				}
			}
		}
		public void OnReloadDBData(int type, object diffUpdate)
		{
			Common.WriteLine("DB update callback  @ {0}", new string[]
			{
				DateTime.Now.ToLongTimeString()
			});
			DispatchAttribute dispatchAttribute = new DispatchAttribute();
			dispatchAttribute.type = 2;
			dispatchAttribute.algorithm = 1;
			dispatchAttribute.uid = 0;
			dispatchAttribute.vid = 0;
			dispatchAttribute._msgHandler = this;
			if (diffUpdate != null)
			{
				string text = (string)diffUpdate;
				if (!string.IsNullOrEmpty(text))
				{
					char[] separator = new char[]
					{
						','
					};
					string[] array = text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length > 0)
					{
						dispatchAttribute.operation = "#UPDATE# ";
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string str = array2[i];
							DispatchAttribute expr_A1 = dispatchAttribute;
							expr_A1.operation = expr_A1.operation + "D" + str + ":*;";
						}
						dispatchAttribute.type = 128;
					}
				}
			}
			this.NewDispatchThread(null, dispatchAttribute);
		}
		public void ReloadDataSet(DispatchAttribute attrib)
		{
			DateTime now = DateTime.Now;
			Common.WriteLine("Reload DB dataset, type={0}, uid={1}, vid={2} @ {3}", new string[]
			{
				attrib.type.ToString("X8"),
				attrib.uid.ToString(),
				attrib.vid.ToString(),
				now.ToLongTimeString()
			});
			lock (this._lockDataset)
			{
				if ((attrib.type & 2) != 0 || this._baseDataSet == null)
				{
					if (attrib.reloadDB > 0)
					{
						if (this._baseDataSet != null)
						{
							this._baseDataSet.Dispose();
						}
						this._baseDataSet = DataSetManager.FastCreateDataset(null, null, null, null, null, null);
					}
					else
					{
						if (this._baseDataSet == null)
						{
							this._baseDataSet = DataSetManager.FastCreateDataset(null, null, null, null, null, null);
						}
					}
					if (this._baseIncremental != null)
					{
						this._baseIncremental.Clear();
					}
					else
					{
						this._baseIncremental = this._baseDataSet.Clone();
					}
				}
				if ((attrib.type & 4) != 0 || this._rackInfo == null)
				{
					this._dcLayout = Sys_Para.GetResolution();
					if (this._rackInfo != null)
					{
						this._rackInfo.Clear();
					}
					this._rackInfo = DeviceOperation.GetDeviceRackMapping();
				}
			}
		}
		public override void FirstDispatch(ecoMessage msg)
		{
			Common.WriteLine("First dispatch dataset @ {0}", new string[]
			{
				DateTime.Now.ToLongTimeString()
			});
			DispatchAttribute dispatchAttribute = new DispatchAttribute();
			dispatchAttribute.type = 4;
			dispatchAttribute.type |= 32;
			dispatchAttribute.algorithm = 1;
			dispatchAttribute.uid = 0;
			dispatchAttribute.vid = 0;
			dispatchAttribute._msgHandler = this;
			this.NewDispatchThread(msg, dispatchAttribute);
			this.NewDispatchThread(msg, new DispatchAttribute
			{
				type = 2,
				algorithm = 1,
				uid = 0,
				vid = 0,
				_msgHandler = this,
				reloadDB = 0
			});
		}
		public void NewDispatchThread(ecoMessage msg, DispatchAttribute attrib)
		{
			if (attrib.type == 1)
			{
				if (EcoHandler._isCompressing_RT > 0 || (EcoHandler._tLastRealTimeDispatch.isRunning() && EcoHandler._tLastRealTimeDispatch.getElapsed() < 10000L))
				{
					lock (this._lockDataset)
					{
						if (this._realtimeDataSet != null)
						{
							if (attrib.discard_if_jam > 0 && this._realtimeDataSet.Tables.Count > 0 && this._realtimeDataSet.Tables[0].Rows.Count > 0)
							{
								Common.WriteLine("Not in dispatching window, new request for realtime dataset will be ignored", new string[0]);
								return;
							}
							Common.WriteLine("All devices are offline, realtime dataset MUST be sent", new string[0]);
						}
					}
				}
				EcoHandler._isCompressing_RT = 1;
			}
			Thread thread = new Thread(new ParameterizedThreadStart(this.SerializingThread));
			thread.Name = "Serialization Thread";
			thread.CurrentCulture = CultureInfo.InvariantCulture;
			thread.IsBackground = true;
			long num = Common.ElapsedTime(attrib.tStart);
			Common.WriteLine("Serialization start, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
			{
				attrib.type.ToString("X8"),
				attrib.uid.ToString(),
				attrib.vid.ToString(),
				num.ToString()
			});
			thread.Start(new EcoHandler.SerializeContext
			{
				msg = msg,
				attrib = attrib
			});
		}
		public bool IsNumeric(string value)
		{
			string text = "0123456789";
			for (int i = 0; i < value.Length; i++)
			{
				char value2 = value[i];
				if (text.IndexOf(value2) < 0)
				{
					return false;
				}
			}
			return true;
		}
		public void BaseIncrementalTest()
		{
			string operation = "#UPDATE#D1:*;D2:S*;D3:P*;D4:B*;D1:S2;D1:P3;D1:B*;";
			this.AppendDbIncremental(operation);
		}
		public void LoadBaseIncremental(List<string> affectedDevices, Dictionary<string, List<string>> affectedSensors, Dictionary<string, List<string>> affectedPorts, Dictionary<string, List<string>> affectedBanks, Dictionary<string, List<string>> affectedLines)
		{
			DataSet dataSet = DataSetManager.FastCreateDataset(affectedDevices, affectedSensors, affectedPorts, affectedBanks, affectedLines, null);
			lock (this._lockDataset)
			{
				if (this._baseIncremental == null)
				{
					this._baseIncremental = dataSet;
				}
				else
				{
					try
					{
						for (int i = 0; i < this._baseIncremental.Tables.Count; i++)
						{
							this._baseIncremental.Tables[i].Merge(dataSet.Tables[i]);
						}
					}
					catch (Exception ex)
					{
						Common.WriteLine("UpdateBaseIncremental", new string[]
						{
							ex.Message
						});
					}
				}
			}
		}
		public int AppendDbIncremental(string operation)
		{
			int num = 0;
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = operation.ToUpper();
			text4 = text4.Replace(" ", "");
			text4 = text4.Replace("\r", "");
			text4 = text4.Replace("\n", "");
			text4 = text4.Replace("<", "");
			text4 = text4.Replace(">", "");
			text4 = text4.Replace("#ADD#", "@A");
			text4 = text4.Replace("#DELETE#", "@D");
			text4 = text4.Replace("#UPDATE#", "@U");
			char[] separator = new char[]
			{
				'@'
			};
			string[] array = text4.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text5 = array2[i];
				if (text5.Length >= 2)
				{
					if (text5.Substring(0, 1) == "A")
					{
						text += text5.Substring(1);
						if (!string.IsNullOrEmpty(text))
						{
							if (!text.EndsWith(";"))
							{
								text += ";";
							}
							num |= 2;
						}
					}
					else
					{
						if (text5.Substring(0, 1) == "D")
						{
							text2 += text5.Substring(1);
							if (!string.IsNullOrEmpty(text2))
							{
								if (!text2.EndsWith(";"))
								{
									text2 += ";";
								}
								num |= 2;
							}
						}
						else
						{
							if (text5.Substring(0, 1) == "U")
							{
								text3 += text5.Substring(1);
								if (!string.IsNullOrEmpty(text3))
								{
									if (!text3.EndsWith(";"))
									{
										text3 += ";";
									}
									num |= 128;
								}
							}
						}
					}
				}
			}
			if ((num & 2) != 0)
			{
				return 2;
			}
			if (text3 != "")
			{
				List<string> list = new List<string>();
				Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
				Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
				Dictionary<string, List<string>> dictionary3 = new Dictionary<string, List<string>>();
				Dictionary<string, List<string>> dictionary4 = new Dictionary<string, List<string>>();
				separator = new char[]
				{
					';'
				};
				string[] array3 = text3.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				string[] array4 = array3;
				for (int j = 0; j < array4.Length; j++)
				{
					string text6 = array4[j];
					string[] array5 = text6.Split(new char[]
					{
						':'
					});
					if (array5.Length == 2 && array5[0].StartsWith("D"))
					{
						string text7 = array5[0].Substring(1);
						text7 = text7.Trim();
						if (!string.IsNullOrEmpty(text7) && text7 != "*" && this.IsNumeric(text7))
						{
							if (array5[1].StartsWith("S"))
							{
								string text8 = array5[1].Substring(1);
								text8 = text8.Trim();
								if (text8 == "*" || text8 == "")
								{
									dictionary[text7] = new List<string>
									{
										"*"
									};
								}
								else
								{
									if (this.IsNumeric(text8))
									{
										if (!dictionary.ContainsKey(text7))
										{
											dictionary[text7] = new List<string>
											{
												text8
											};
										}
										else
										{
											if (!dictionary[text7].Contains("*") && !dictionary[text7].Contains(text8))
											{
												dictionary[text7].Add(text8);
											}
										}
									}
								}
							}
							else
							{
								if (array5[1].StartsWith("P"))
								{
									string text9 = array5[1].Substring(1);
									text9 = text9.Trim();
									if (text9 == "*" || text9 == "")
									{
										dictionary2[text7] = new List<string>
										{
											"*"
										};
									}
									else
									{
										if (this.IsNumeric(text9))
										{
											if (!dictionary2.ContainsKey(text7))
											{
												dictionary2[text7] = new List<string>
												{
													text9
												};
											}
											else
											{
												if (!dictionary2[text7].Contains("*") && !dictionary2[text7].Contains(text9))
												{
													dictionary2[text7].Add(text9);
												}
											}
										}
									}
								}
								else
								{
									if (array5[1].StartsWith("B"))
									{
										string text10 = array5[1].Substring(1);
										text10 = text10.Trim();
										if (text10 == "*" || text10 == "")
										{
											dictionary3[text7] = new List<string>
											{
												"*"
											};
										}
										else
										{
											if (this.IsNumeric(text10))
											{
												if (!dictionary3.ContainsKey(text7))
												{
													dictionary3[text7] = new List<string>
													{
														text10
													};
												}
												else
												{
													if (!dictionary3[text7].Contains("*") && !dictionary3[text7].Contains(text10))
													{
														dictionary3[text7].Add(text10);
													}
												}
											}
										}
									}
									else
									{
										if (array5[1].StartsWith("L"))
										{
											string text11 = array5[1].Substring(1);
											text11 = text11.Trim();
											if (text11 == "*" || text11 == "")
											{
												dictionary4[text7] = new List<string>
												{
													"*"
												};
											}
											else
											{
												if (this.IsNumeric(text11))
												{
													if (!dictionary4.ContainsKey(text7))
													{
														dictionary4[text7] = new List<string>
														{
															text11
														};
													}
													else
													{
														if (!dictionary4[text7].Contains("*") && !dictionary4[text7].Contains(text11))
														{
															dictionary4[text7].Add(text11);
														}
													}
												}
											}
										}
										else
										{
											if ((array5[1].StartsWith("*") || array5[1] == "") && !list.Contains(text7))
											{
												list.Add(text7);
												dictionary[text7] = new List<string>
												{
													"*"
												};
												dictionary2[text7] = new List<string>
												{
													"*"
												};
												dictionary3[text7] = new List<string>
												{
													"*"
												};
												dictionary4[text7] = new List<string>
												{
													"*"
												};
											}
										}
									}
								}
							}
						}
					}
				}
				int num2 = 0;
				string[] array6 = new string[]
				{
					"",
					"",
					"",
					"",
					""
				};
				if (list.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string current in list)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(current);
					}
					if (list.Count > 1)
					{
						array6[num2] = "id in (" + stringBuilder.ToString() + ")";
					}
					else
					{
						array6[num2] = "id=" + stringBuilder.ToString();
					}
				}
				num2++;
				if (dictionary.Count > 0)
				{
					array6[num2] = "";
					foreach (KeyValuePair<string, List<string>> current2 in dictionary)
					{
						List<string> value = current2.Value;
						if (value.Contains("*"))
						{
							string[] array7;
							IntPtr intPtr;
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + "(device_id=" + current2.Key + ")";
						}
						else
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							foreach (string current3 in value)
							{
								if (stringBuilder2.Length > 0)
								{
									stringBuilder2.Append(",");
								}
								stringBuilder2.Append(current3);
							}
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								string[] array7;
								IntPtr intPtr;
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							if (value.Count > 1)
							{
								string[] array7;
								string[] expr_910 = array7 = array6;
								IntPtr intPtr;
								int expr_915 = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_910[expr_915] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current2.Key,
									" and sensor_type in (",
									stringBuilder2.ToString(),
									"))"
								});
							}
							else
							{
								string[] array7;
								string[] expr_96A = array7 = array6;
								IntPtr intPtr;
								int expr_96F = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_96A[expr_96F] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current2.Key,
									" and sensor_type=",
									stringBuilder2.ToString(),
									")"
								});
							}
						}
					}
				}
				num2++;
				if (dictionary2.Count > 0)
				{
					array6[num2] = "";
					foreach (KeyValuePair<string, List<string>> current4 in dictionary2)
					{
						List<string> value2 = current4.Value;
						if (value2.Contains("*"))
						{
							string[] array7;
							IntPtr intPtr;
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + "(device_id=" + current4.Key + ")";
						}
						else
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							foreach (string current5 in value2)
							{
								if (stringBuilder3.Length > 0)
								{
									stringBuilder3.Append(",");
								}
								stringBuilder3.Append(current5);
							}
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								string[] array7;
								IntPtr intPtr;
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							if (value2.Count > 1)
							{
								string[] array7;
								string[] expr_AFF = array7 = array6;
								IntPtr intPtr;
								int expr_B04 = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_AFF[expr_B04] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current4.Key,
									" and id in (",
									stringBuilder3.ToString(),
									"))"
								});
							}
							else
							{
								string[] array7;
								string[] expr_B59 = array7 = array6;
								IntPtr intPtr;
								int expr_B5E = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_B59[expr_B5E] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current4.Key,
									" and id=",
									stringBuilder3.ToString(),
									")"
								});
							}
						}
					}
				}
				num2++;
				if (dictionary3.Count > 0)
				{
					array6[num2] = "";
					foreach (KeyValuePair<string, List<string>> current6 in dictionary3)
					{
						List<string> value3 = current6.Value;
						if (value3.Contains("*"))
						{
							string[] array7;
							IntPtr intPtr;
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + "(device_id=" + current6.Key + ")";
						}
						else
						{
							StringBuilder stringBuilder4 = new StringBuilder();
							foreach (string current7 in value3)
							{
								if (stringBuilder4.Length > 0)
								{
									stringBuilder4.Append(",");
								}
								stringBuilder4.Append(current7);
							}
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								string[] array7;
								IntPtr intPtr;
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							if (value3.Count > 1)
							{
								string[] array7;
								string[] expr_CEE = array7 = array6;
								IntPtr intPtr;
								int expr_CF3 = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_CEE[expr_CF3] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current6.Key,
									" and id in (",
									stringBuilder4.ToString(),
									"))"
								});
							}
							else
							{
								string[] array7;
								string[] expr_D48 = array7 = array6;
								IntPtr intPtr;
								int expr_D4D = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_D48[expr_D4D] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current6.Key,
									" and id=",
									stringBuilder4.ToString(),
									")"
								});
							}
						}
					}
				}
				num2++;
				if (dictionary4.Count > 0)
				{
					array6[num2] = "";
					foreach (KeyValuePair<string, List<string>> current8 in dictionary4)
					{
						List<string> value4 = current8.Value;
						if (value4.Contains("*"))
						{
							string[] array7;
							IntPtr intPtr;
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + "(device_id=" + current8.Key + ")";
						}
						else
						{
							StringBuilder stringBuilder5 = new StringBuilder();
							foreach (string current9 in value4)
							{
								if (stringBuilder5.Length > 0)
								{
									stringBuilder5.Append(",");
								}
								stringBuilder5.Append(current9);
							}
							if (!string.IsNullOrEmpty(array6[num2]))
							{
								string[] array7;
								IntPtr intPtr;
								(array7 = array6)[(int)(intPtr = (IntPtr)num2)] = array7[(int)intPtr] + " or ";
							}
							if (value4.Count > 1)
							{
								string[] array7;
								string[] expr_EDD = array7 = array6;
								IntPtr intPtr;
								int expr_EE2 = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_EDD[expr_EE2] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current8.Key,
									" and id in (",
									stringBuilder5.ToString(),
									"))"
								});
							}
							else
							{
								string[] array7;
								string[] expr_F37 = array7 = array6;
								IntPtr intPtr;
								int expr_F3C = (int)(intPtr = (IntPtr)num2);
								string text12 = array7[(int)intPtr];
								expr_F37[expr_F3C] = string.Concat(new string[]
								{
									text12,
									"(device_id=",
									current8.Key,
									" and id=",
									stringBuilder5.ToString(),
									")"
								});
							}
						}
					}
				}
				for (int k = 0; k < 5; k++)
				{
					if (array6[k] == "")
					{
						array6[k] = "(1=0)";
					}
				}
				this.LoadBaseIncremental(list, dictionary, dictionary2, dictionary3, dictionary4);
			}
			return num;
		}
		public void TableRowsThread(object obj)
		{
			EcoHandler.SerializeTableContext serializeTableContext = (EcoHandler.SerializeTableContext)obj;
			try
			{
				DataSetManager.WriteTableData(serializeTableContext.dt, serializeTableContext.idxRowFrom, serializeTableContext.nRowCount, serializeTableContext.outStream);
			}
			catch (Exception ex)
			{
				Common.WriteLine("TableRowsThread: table={0}, error:{1}", new string[]
				{
					serializeTableContext.name,
					ex.Message
				});
			}
			finally
			{
				serializeTableContext.evtDone.Set();
			}
		}
		public void DatarowToStream(string tblName, DataTable dt, int nThreads, MyMemoryStream outStream)
		{
			int count = dt.Rows.Count;
			int num = count;
			if (nThreads > 1)
			{
				num = (count + nThreads - 1) / nThreads;
			}
			if (nThreads <= 1)
			{
				DataSetManager.WriteTableData(dt, 0, 0, outStream);
				return;
			}
			Thread[] array = new Thread[nThreads];
			ManualResetEvent[] array2 = new ManualResetEvent[nThreads];
			MyMemoryStream[] array3 = new MyMemoryStream[nThreads];
			EcoHandler.SerializeTableContext[] array4 = new EcoHandler.SerializeTableContext[nThreads];
			int num2 = count;
			for (int i = 0; i < nThreads; i++)
			{
				array3[i] = new MyMemoryStream();
				array2[i] = new ManualResetEvent(false);
				array4[i] = new EcoHandler.SerializeTableContext();
				array2[i].Reset();
				array4[i].dt = dt;
				array4[i].name = tblName;
				array4[i].idxRowFrom = i * num;
				array4[i].nRowCount = Math.Min(num, num2);
				array4[i].evtDone = array2[i];
				array4[i].outStream = array3[i];
				num2 -= array4[i].nRowCount;
			}
			for (int j = 0; j < nThreads; j++)
			{
				array[j] = new Thread(new ParameterizedThreadStart(this.TableRowsThread));
				array[j].Name = string.Concat(new object[]
				{
					"Thread_",
					array4[j].name,
					"_#",
					j
				});
				array[j].CurrentCulture = CultureInfo.InvariantCulture;
				array[j].IsBackground = true;
				array[j].Start(array4[j]);
			}
			WaitHandle.WaitAll(array2);
			string text = "";
			for (int k = 0; k < nThreads; k++)
			{
				if (k > 0)
				{
					text += "|";
				}
				text += array3[k].Length;
			}
			text += "\n";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			outStream.Write(bytes, 0, bytes.Length);
			text = "\n";
			bytes = Encoding.UTF8.GetBytes(text);
			outStream.Write(bytes, 0, bytes.Length);
			for (int l = 0; l < nThreads; l++)
			{
				array3[l].WriteTo(outStream);
				array3[l].Close();
				array3[l].Dispose();
			}
		}
		public void SingleTableThread(object tblStream)
		{
			EcoHandler.SerializeTableContext serializeTableContext = (EcoHandler.SerializeTableContext)tblStream;
			try
			{
				if (serializeTableContext.nThreads > 1)
				{
					DataSetManager.WriteTableInfo(serializeTableContext.name, serializeTableContext.dt, false, serializeTableContext.outStream);
					this.DatarowToStream(serializeTableContext.name, serializeTableContext.dt, serializeTableContext.nThreads, serializeTableContext.outStream);
				}
				else
				{
					DataSetManager.WriteTableInfo(serializeTableContext.name, serializeTableContext.dt, true, serializeTableContext.outStream);
					DataSetManager.WriteTableData(serializeTableContext.dt, 0, 0, serializeTableContext.outStream);
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("SingleTableThread: table={0}, error:{1}", new string[]
				{
					serializeTableContext.name,
					ex.Message
				});
			}
			finally
			{
				serializeTableContext.evtDone.Set();
			}
		}
		public int DataSetToStream(string[] tblNames, DataSet ds, int nRowsPerThread, MyMemoryStream outStream)
		{
			int count = ds.Tables.Count;
			int num = 0;
			Thread[] array = new Thread[count];
			ManualResetEvent[] array2 = new ManualResetEvent[count];
			MyMemoryStream[] array3 = new MyMemoryStream[count];
			EcoHandler.SerializeTableContext[] array4 = new EcoHandler.SerializeTableContext[count];
			for (int i = 0; i < count; i++)
			{
				array3[i] = new MyMemoryStream();
				array2[i] = new ManualResetEvent(false);
				array4[i] = new EcoHandler.SerializeTableContext();
				array2[i].Reset();
				array4[i].dt = ds.Tables[i];
				array4[i].name = tblNames[i];
				array4[i].nThreads = 0;
				if (array4[i].dt.Rows.Count > nRowsPerThread && nRowsPerThread > 1)
				{
					array4[i].nThreads = (array4[i].dt.Rows.Count + nRowsPerThread - 1) / nRowsPerThread;
				}
				array4[i].evtDone = array2[i];
				array4[i].outStream = array3[i];
			}
			for (int j = 0; j < count; j++)
			{
				array[j] = new Thread(new ParameterizedThreadStart(this.SingleTableThread));
				array[j].Name = "Thread_" + array4[j].name;
				array[j].CurrentCulture = CultureInfo.InvariantCulture;
				array[j].IsBackground = true;
				array[j].Start(array4[j]);
			}
			WaitHandle.WaitAll(array2);
			for (int k = 0; k < count; k++)
			{
				num += (int)array3[k].Length;
				array3[k].WriteTo(outStream);
				array3[k].Close();
				array3[k].Dispose();
			}
			return num;
		}
		public void SerializingThread(object context)
		{
			EcoHandler.SerializeContext serializeContext = (EcoHandler.SerializeContext)context;
			ecoMessage msg = serializeContext.msg;
			DispatchAttribute attrib = serializeContext.attrib;
			byte[] array = null;
			long num = Common.ElapsedTime(attrib.tStart);
			Common.WriteLine("    Dataset serializing, type={0}, Elapsed={1}", new string[]
			{
				attrib.type.ToString("X8"),
				num.ToString()
			});
			int num2 = 0;
			if (attrib.operation != null && attrib.operation != "")
			{
				num2 |= this.AppendDbIncremental(attrib.operation);
				bool flag = true;
				lock (this._lockDataset)
				{
					try
					{
						int count = this._baseIncremental.Tables[0].Rows.Count;
						if ((long)count >= EcoHandler._bsDiffTableAmountLimit)
						{
							num2 &= -129;
							num2 |= 2;
							attrib.type &= -129;
							attrib.type |= 2;
							flag = false;
							Common.WriteLine("    Diff. Dataset amount-limit update", new string[0]);
						}
					}
					catch (Exception)
					{
					}
				}
				if (flag)
				{
					if (attrib._msgHandler != null)
					{
						Interlocked.Increment(ref ((EcoHandler)attrib._msgHandler)._diffDelay);
					}
					Common.WriteLine("    Diff. Dataset delayed", new string[0]);
					if ((attrib.type & 1) != 0)
					{
						EcoHandler._isCompressing_RT = 0;
					}
					return;
				}
			}
			if ((attrib.type & 32) != 0)
			{
				num2 |= 32;
			}
			if ((attrib.type & 8) != 0)
			{
				num2 |= 8;
			}
			if ((attrib.type & 4) != 0)
			{
				this.ReloadDataSet(attrib);
				num2 |= 4;
			}
			if ((attrib.type & 2) != 0)
			{
				this.ReloadDataSet(attrib);
				num2 |= 2;
			}
			if ((attrib.type & 1) != 0)
			{
				num2 |= 1;
			}
			if ((attrib.type & 64) != 0)
			{
				num2 |= 64;
			}
			if ((attrib.type & 128) != 0)
			{
				num2 |= 128;
			}
			if ((attrib.type & 256) != 0)
			{
				num2 |= 256;
			}
			if ((attrib.type & 512) != 0)
			{
				num2 |= 512;
			}
			if ((attrib.type & 2048) != 0)
			{
				num2 |= 2048;
			}
			if ((attrib.type & 4096) != 0)
			{
				num2 |= 4096;
			}
			if (num2 == 0)
			{
				if ((attrib.type & 1) != 0)
				{
					EcoHandler._isCompressing_RT = 0;
				}
				return;
			}
			MyMemoryStream myMemoryStream = new MyMemoryStream();
			try
			{
				string text = string.Concat(new string[]
				{
					attrib.operation,
					"|",
					attrib.attached,
					"|",
					attrib.guid,
					"|",
					attrib.alltype
				});
				if (!text.Equals("|||"))
				{
					string text2 = text.Trim();
					text2 = text2.Replace("\r", "\n");
					while (text2.IndexOf("\n\n") >= 0)
					{
						text2 = text2.Replace("\n\n", "");
					}
					string[] array2 = text2.Split(new char[]
					{
						'\n'
					});
					if (array2.Length > 0)
					{
						string text3 = "";
						string[] array3 = array2;
						for (int i = 0; i < array3.Length; i++)
						{
							string text4 = array3[i];
							if (text4.Trim() != "")
							{
								text3 = text3 + "//////" + text4 + "\r\n";
							}
						}
						if (text3 != "")
						{
							text3 += "\r\n";
							byte[] bytes = Encoding.UTF8.GetBytes(text3);
							myMemoryStream.Write(bytes, 0, bytes.Length);
						}
					}
				}
				if ((num2 & 8) != 0)
				{
					DataSetManager.WriteUserUAC(myMemoryStream, null);
				}
				if ((num2 & 4096) != 0)
				{
					string[] sTime = new string[]
					{
						"",
						"",
						"",
						"",
						"",
						"",
						"",
						""
					};
					double[] array4 = new double[8];
					double[] pue = array4;
					int[] array5 = new int[2];
					int[] array6 = array5;
					AppData.LoadPUEData(ref pue, ref array6, ref sTime);
					DataSetManager.WritePueValuePairs(pue, sTime, myMemoryStream, null);
				}
				if ((num2 & 2048) != 0)
				{
					DataSetManager.WriteStreamByAutoModel(myMemoryStream, null);
				}
				if ((num2 & 256) != 0)
				{
					DataSetManager.WriteZoneInfo(myMemoryStream, null);
				}
				if ((num2 & 512) != 0)
				{
					DataSetManager.WriteGroupInfo(myMemoryStream, null);
				}
				if ((num2 & 4) != 0)
				{
					DataSetManager.WriteRackInfo(this._dcLayout, myMemoryStream, null);
					DataSetManager.WriteDevice2Rack(myMemoryStream, null);
				}
				if ((num2 & 32) != 0)
				{
					DataSetManager.WriteSystemSettings(myMemoryStream, null);
				}
				if ((num2 & 2) != 0 && this._baseDataSet != null)
				{
					DataSet dataSet = null;
					lock (this._lockDataset)
					{
						dataSet = this._baseDataSet.Copy();
						this._tLastBaseDataSet = (long)Environment.TickCount;
						this._tLastBaseIncremental = (long)Environment.TickCount;
					}
					if (dataSet != null)
					{
						string[] tblNames = new string[]
						{
							"device",
							"sensor",
							"port",
							"bank",
							"line"
						};
						this.DataSetToStream(tblNames, dataSet, 8192, myMemoryStream);
						dataSet.Dispose();
					}
				}
				if ((num2 & 128) != 0 && this._baseIncremental != null)
				{
					DataSet dataSet2 = null;
					lock (this._lockDataset)
					{
						dataSet2 = this._baseIncremental.Copy();
						this._tLastBaseIncremental = (long)Environment.TickCount;
					}
					if (dataSet2 != null)
					{
						string[] tblNames2 = new string[]
						{
							"dy_device",
							"dy_sensor",
							"dy_port",
							"dy_bank",
							"dy_line"
						};
						this.DataSetToStream(tblNames2, dataSet2, 8192, myMemoryStream);
						dataSet2.Dispose();
					}
				}
				if ((num2 & 1) != 0 && this._realtimeDataSet != null)
				{
					string[] sTime2 = new string[]
					{
						"",
						"",
						"",
						"",
						"",
						"",
						"",
						""
					};
					double[] array7 = new double[8];
					double[] pue2 = array7;
					int[] array8 = new int[2];
					int[] array9 = array8;
					AppData.LoadPUEData(ref pue2, ref array9, ref sTime2);
					DataSetManager.WritePueValuePairs(pue2, sTime2, myMemoryStream, null);
					DataSet dataSet3 = null;
					lock (this._lockDataset)
					{
						dataSet3 = this._realtimeDataSet.Copy();
						this._tLastRtDataSet = (long)Environment.TickCount;
					}
					if (dataSet3 != null)
					{
						string[] tblNames3 = new string[]
						{
							"rt_device",
							"rt_sensor",
							"rt_port",
							"rt_bank",
							"rt_line"
						};
						this.DataSetToStream(tblNames3, dataSet3, 8192, myMemoryStream);
						dataSet3.Dispose();
					}
				}
				if ((num2 & 64) != 0 && this._realtimeIncremental != null)
				{
					DataSet dataSet4 = null;
					lock (this._lockDataset)
					{
						dataSet4 = this._realtimeIncremental.Copy();
						this._tLastRtIncremental = (long)Environment.TickCount;
					}
					if (dataSet4 != null)
					{
						string[] tblNames4 = new string[]
						{
							"ud_device",
							"ud_sensor",
							"ud_port",
							"ud_bank",
							"ud_line"
						};
						this.DataSetToStream(tblNames4, dataSet4, 8192, myMemoryStream);
						dataSet4.Dispose();
					}
				}
				if (myMemoryStream.Length > 0L)
				{
					array = myMemoryStream.ToArray();
				}
				myMemoryStream.Close();
				myMemoryStream.Dispose();
				num = Common.ElapsedTime(attrib.tStart);
				Common.WriteLine("    Dataset ready, type={0}, Elapsed={1}", new string[]
				{
					attrib.type.ToString("X8"),
					num.ToString()
				});
				if (array != null && array.Length > 0)
				{
					attrib.algorithm = 1;
					attrib.data = array;
					if (msg != null)
					{
						attrib.owner = msg._from;
						attrib.cbCallBack = new DispatchCallback(msg._from.DispatchDataset);
					}
					else
					{
						attrib.cbCallBack = new DispatchCallback(this.ToAllDispatcher);
					}
					if (attrib.type == 1 || attrib.type == 2)
					{
						attrib.cbCacheProcess = new DispatchCallback(this.UpdateCompressedData);
					}
					Compression.CompressThread(attrib);
				}
				else
				{
					DateTime now = DateTime.Now;
					Common.WriteLine("    nothing to dispatch, type={0}, uid={1}, vid={2} @ {3}", new string[]
					{
						attrib.type.ToString("X8"),
						attrib.uid.ToString(),
						attrib.vid.ToString(),
						now.ToLongTimeString()
					});
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("SerializingThread: " + ex.Message, new string[0]);
			}
			finally
			{
				if (myMemoryStream != null)
				{
					myMemoryStream.Close();
					myMemoryStream.Dispose();
				}
				num = Common.ElapsedTime(attrib.tStart);
				Common.WriteLine("Serialization End, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
				{
					attrib.type.ToString("X8"),
					attrib.uid.ToString(),
					attrib.vid.ToString(),
					num.ToString()
				});
				if ((attrib.type & 1) != 0)
				{
					EcoHandler._isCompressing_RT = 0;
					EcoHandler._tLastRealTimeDispatch.Start();
				}
			}
		}
		public void StartDeviceIncrementalThread(Dictionary<string, string> request)
		{
			this.AppendDeviceIncremental(request);
		}
		public void AppendDeviceIncremental(Dictionary<string, string> request)
		{
			if (this._realtimeDataSet == null)
			{
				return;
			}
			long tLast = (long)Environment.TickCount;
			if (!request.ContainsKey("operation"))
			{
				return;
			}
			if (!request.ContainsKey("targets"))
			{
				return;
			}
			string text = request["operation"];
			if (text == null || text == "")
			{
				return;
			}
			text = text.Trim();
			text = text.ToLower();
			string text2 = request["targets"];
			if (text2 == null || text2 == "")
			{
				return;
			}
			text2 = text2.Trim();
			text2 = text2.ToLower();
			DataTable dataTable = null;
			DataRow[] array = null;
			lock (this._lockDataset)
			{
				dataTable = this._realtimeDataSet.Tables[2].Clone();
			}
			string[] array2 = text2.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array2.Length; i++)
			{
				int num = array2[i].IndexOf(":");
				if (num >= 0)
				{
					string text3 = array2[i].Substring(0, num);
					string text4 = array2[i].Substring(num + 1);
					lock (this._lockDataset)
					{
						if (text4.Equals("all"))
						{
							array = this._realtimeDataSet.Tables[2].Select("device_id=" + text3);
						}
						else
						{
							array = this._realtimeDataSet.Tables[2].Select(string.Concat(new string[]
							{
								"device_id=",
								text3,
								" and port_number in (",
								text4,
								")"
							}));
						}
					}
					if (array != null && array.Length > 0)
					{
						DataRow[] array3 = array;
						for (int j = 0; j < array3.Length; j++)
						{
							DataRow row = array3[j];
							dataTable.ImportRow(row);
						}
					}
				}
			}
			foreach (DataRow dataRow in dataTable.Rows)
			{
				dataRow["port_state"] = OutletStatus.Pending.ToString();
			}
			Common.ElapsedTime(tLast);
			lock (this._lockDataset)
			{
				if (this._realtimeIncremental == null)
				{
					this._realtimeIncremental = this._realtimeDataSet.Clone();
				}
				if (dataTable.Rows.Count > 0 && this._realtimeIncremental != null && this._realtimeIncremental.Tables.Count > 2)
				{
					this._realtimeIncremental.Tables[2].Merge(dataTable);
				}
			}
			dataTable.Dispose();
			Common.ElapsedTime(tLast);
			if (text.IndexOf("more") < 0)
			{
				this.NewDispatchThread(null, new DispatchAttribute
				{
					type = 64,
					algorithm = 1,
					uid = 0,
					vid = 0,
					_msgHandler = this
				});
			}
		}
		public void DispatchDiffTable(bool bAll)
		{
			DispatchAttribute dispatchAttribute = new DispatchAttribute();
			dispatchAttribute.type = (bAll ? 2 : 128);
			dispatchAttribute.algorithm = 1;
			dispatchAttribute.uid = 0;
			dispatchAttribute.vid = 0;
			dispatchAttribute._msgHandler = this;
			try
			{
				Common.WriteLine(string.Format("Dispatching diff table: count={0}, all={1}", this._baseIncremental.Tables[0].Rows.Count, bAll), new string[0]);
			}
			catch (Exception)
			{
			}
			this.NewDispatchThread(null, dispatchAttribute);
		}
		public void BroadcastMACConflict(IDictionary<string, string> macMismatch)
		{
			string text = "";
			if (macMismatch != null)
			{
				foreach (KeyValuePair<string, string> current in macMismatch)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += ";";
					}
					text = text + current.Key + "," + current.Value;
				}
			}
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(EcoHandler._macMismatchList))
			{
				return;
			}
			if (text.Equals(EcoHandler._macMismatchList, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}
			EcoHandler._macMismatchList = text;
			this.ToAllDispatcher(new DispatchAttribute
			{
				type = 8192,
				uid = 0,
				vid = 0,
				operation = "MACConflict",
				attached = EcoHandler._macMismatchList
			});
		}
		public void DispatchPUE()
		{
			this.NewDispatchThread(null, new DispatchAttribute
			{
				type = 4096,
				algorithm = 1,
				uid = 0,
				vid = 0,
				_msgHandler = this
			});
		}
		public void OnAutoDiscovery(string modeKey, DevModelConfig cfg)
		{
			lock (EcoHandler._lockAutoModel)
			{
				DevModelConfig value = default(DevModelConfig);
				value.modelName = cfg.modelName;
				value.firmwareVer = cfg.firmwareVer;
				value.autoBasicInfo = cfg.autoBasicInfo;
				value.autoRatingInfo = cfg.autoRatingInfo;
				if (EcoHandler._autoModelSink == null)
				{
					EcoHandler._autoModelSink = DataSetManager.getAutoModelList();
				}
				else
				{
					EcoHandler._autoModelSink[modeKey] = value;
				}
			}
			this.NewDispatchThread(null, new DispatchAttribute
			{
				type = 2048,
				algorithm = 1,
				uid = 0,
				vid = 0,
				_msgHandler = this
			});
		}
		public void SendUrgency(Socket sock, int uid, string op)
		{
			byte[] array = this.BuildUrgencyPacket(sock, uid, op);
			if (array == null)
			{
				return;
			}
			foreach (IConnectInterface current in this._lstConnectors)
			{
				current.SendUrgency(sock, uid, array);
			}
		}
		public void BroadcastPending(DataTable pendingOutlets)
		{
			if (pendingOutlets == null || pendingOutlets.Rows.Count <= 0)
			{
				return;
			}
			try
			{
				pendingOutlets.PrimaryKey = new DataColumn[]
				{
					pendingOutlets.Columns["device_id"],
					pendingOutlets.Columns["port_number"]
				};
				DataView dataView = new DataView(pendingOutlets);
				dataView.Sort = "device_id ASC, port_number ASC";
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (DataRowView dataRowView in dataView)
				{
					int num2 = Convert.ToInt32(dataRowView[0]);
					if (num2 != num && stringBuilder2.Length > 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(";");
						}
						stringBuilder.Append(num.ToString() + ":" + stringBuilder2.ToString());
						stringBuilder2.Clear();
						if (stringBuilder.Length >= 65000)
						{
							Dictionary<string, string> dictionary = new Dictionary<string, string>();
							dictionary["operation"] = "pending";
							dictionary["targets"] = stringBuilder.ToString();
							Common.WriteLine("BroadcastPending: {0}", new string[]
							{
								stringBuilder.ToString()
							});
							this.StartDeviceIncrementalThread(dictionary);
							stringBuilder.Clear();
						}
					}
					num = num2;
					int num3 = Convert.ToInt32(dataRowView[1]);
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Append(",");
					}
					stringBuilder2.Append(num3.ToString());
				}
				if (stringBuilder2.Length > 0)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(num.ToString() + ":" + stringBuilder2.ToString());
					stringBuilder2.Clear();
				}
				if (stringBuilder.Length > 0)
				{
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					dictionary2["operation"] = "pending";
					dictionary2["targets"] = stringBuilder.ToString();
					Common.WriteLine("BroadcastPending: {0}", new string[]
					{
						stringBuilder.ToString()
					});
					this.StartDeviceIncrementalThread(dictionary2);
				}
			}
			catch (Exception)
			{
			}
		}
		public void DifferentialValues(DataSet rtDiff)
		{
			if (rtDiff == null)
			{
				return;
			}
			try
			{
				if (rtDiff.Tables.Count >= 4)
				{
					for (int i = 0; i < rtDiff.Tables.Count; i++)
					{
						if (rtDiff.Tables[i].Columns.Contains("insert_time"))
						{
							rtDiff.Tables[i].Columns.Remove("insert_time");
						}
					}
					if (rtDiff.Tables[1].Columns.Contains("sensor_location"))
					{
						rtDiff.Tables[1].Columns.Remove("sensor_location");
					}
					if (rtDiff.Tables[2].Columns.Contains("port_nm"))
					{
						rtDiff.Tables[2].Columns.Remove("port_nm");
					}
					if (rtDiff.Tables[3].Columns.Contains("bank_nm"))
					{
						rtDiff.Tables[3].Columns.Remove("bank_nm");
					}
					if (rtDiff.Tables[3].Columns.Contains("bank_number"))
					{
						rtDiff.Tables[3].Columns.Remove("bank_number");
					}
					rtDiff.Tables[0].PrimaryKey = new DataColumn[]
					{
						rtDiff.Tables[0].Columns["device_id"]
					};
					rtDiff.Tables[1].PrimaryKey = new DataColumn[]
					{
						rtDiff.Tables[1].Columns["device_id"],
						rtDiff.Tables[1].Columns["sensor_type"]
					};
					rtDiff.Tables[2].PrimaryKey = new DataColumn[]
					{
						rtDiff.Tables[2].Columns["device_id"],
						rtDiff.Tables[2].Columns["port_number"],
						rtDiff.Tables[2].Columns["port_id"]
					};
					rtDiff.Tables[3].PrimaryKey = new DataColumn[]
					{
						rtDiff.Tables[3].Columns["device_id"],
						rtDiff.Tables[3].Columns["bank_id"]
					};
					if (rtDiff.Tables.Count > 4)
					{
						rtDiff.Tables[4].PrimaryKey = new DataColumn[]
						{
							rtDiff.Tables[4].Columns["device_id"],
							rtDiff.Tables[4].Columns["line_id"]
						};
					}
					lock (this._lockDataset)
					{
						if (this._realtimeIncremental == null)
						{
							this._realtimeIncremental = rtDiff;
						}
						else
						{
							this._realtimeIncremental = this._realtimeDataSet.Clone();
							for (int j = 0; j < this._realtimeIncremental.Tables.Count; j++)
							{
								this._realtimeIncremental.Tables[j].Merge(rtDiff.Tables[j], false, MissingSchemaAction.AddWithKey);
							}
						}
					}
					this.NewDispatchThread(null, new DispatchAttribute
					{
						type = 64,
						algorithm = 1,
						uid = 0,
						vid = 0,
						_msgHandler = this
					});
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("DifferentialValues: {0}", new string[]
				{
					ex.Message
				});
			}
		}
		public DataSet GetDelayOffDevice(DataSet rtLastRealtime, List<int> keepLastValues)
		{
			if (rtLastRealtime == null || rtLastRealtime.Tables.Count <= 0 || keepLastValues == null || keepLastValues.Count <= 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			DataSet dataSet = null;
			try
			{
				if (rtLastRealtime != null && keepLastValues.Count > 0)
				{
					foreach (int current in keepLastValues)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(current.ToString());
					}
					dataSet = new DataSet();
					for (int i = 0; i < rtLastRealtime.Tables.Count; i++)
					{
						DataTable dataTable = rtLastRealtime.Tables[i].Clone();
						DataRow[] array;
						if (keepLastValues.Count == 1)
						{
							array = rtLastRealtime.Tables[i].Select("device_id=" + stringBuilder);
						}
						else
						{
							array = rtLastRealtime.Tables[i].Select("device_id in (" + stringBuilder + ")");
						}
						DataRow[] array2 = array;
						for (int j = 0; j < array2.Length; j++)
						{
							DataRow row = array2[j];
							dataTable.ImportRow(row);
						}
						dataSet.Tables.Add(dataTable);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("GetDelayOffDevice: {0}", new string[]
				{
					ex.Message
				});
				return null;
			}
			return dataSet;
		}
		public void DispatchRealtimeData(DataSet rtData, List<int> keepLastValues)
		{
			DataSet dataSet = null;
			bool flag = true;
			lock (this._lockHandler)
			{
				if (this._pueTimer != null)
				{
					if (rtData == null)
					{
						this._pueTimer.Interval = (double)(1000 * Sys_Para.GetServiceDelay());
						this._pueTimer.Enabled = true;
					}
					else
					{
						this._pueTimer.Enabled = false;
					}
				}
			}
			lock (this._lockDataset)
			{
				if (rtData == null)
				{
					flag = false;
					if (this._realtimeDataSet != null)
					{
						for (int i = 0; i < this._realtimeDataSet.Tables.Count; i++)
						{
							this._realtimeDataSet.Tables[i].Clear();
						}
					}
				}
				else
				{
					if (this._realtimeDataSet != null)
					{
						if (SessionAPI.getSessionCount() <= 0)
						{
							Common.WriteLine("No client connected, realtime dataset was ignored", new string[0]);
							return;
						}
						if (keepLastValues != null && keepLastValues.Count > 0)
						{
							dataSet = this.GetDelayOffDevice(this._realtimeDataSet, keepLastValues);
							Common.WriteLine("Offline delayed: count={0}", new string[]
							{
								dataSet.Tables[0].Rows.Count.ToString()
							});
						}
						this._realtimeDataSet.Dispose();
					}
					this._realtimeDataSet = rtData;
					try
					{
						if (this._realtimeDataSet.Tables.Count >= 4)
						{
							for (int j = 0; j < this._realtimeDataSet.Tables.Count; j++)
							{
								if (this._realtimeDataSet.Tables[j].Columns.Contains("insert_time"))
								{
									this._realtimeDataSet.Tables[j].Columns.Remove("insert_time");
								}
							}
							if (this._realtimeDataSet.Tables[1].Columns.Contains("sensor_location"))
							{
								this._realtimeDataSet.Tables[1].Columns.Remove("sensor_location");
							}
							if (this._realtimeDataSet.Tables[2].Columns.Contains("port_nm"))
							{
								this._realtimeDataSet.Tables[2].Columns.Remove("port_nm");
							}
							if (this._realtimeDataSet.Tables[3].Columns.Contains("bank_nm"))
							{
								this._realtimeDataSet.Tables[3].Columns.Remove("bank_nm");
							}
							if (this._realtimeDataSet.Tables[3].Columns.Contains("bank_number"))
							{
								this._realtimeDataSet.Tables[3].Columns.Remove("bank_number");
							}
							this._realtimeDataSet.Tables[0].PrimaryKey = new DataColumn[]
							{
								this._realtimeDataSet.Tables[0].Columns["device_id"]
							};
							this._realtimeDataSet.Tables[1].PrimaryKey = new DataColumn[]
							{
								this._realtimeDataSet.Tables[1].Columns["device_id"],
								this._realtimeDataSet.Tables[1].Columns["sensor_type"]
							};
							this._realtimeDataSet.Tables[2].PrimaryKey = new DataColumn[]
							{
								this._realtimeDataSet.Tables[2].Columns["device_id"],
								this._realtimeDataSet.Tables[2].Columns["port_number"],
								this._realtimeDataSet.Tables[2].Columns["port_id"]
							};
							this._realtimeDataSet.Tables[3].PrimaryKey = new DataColumn[]
							{
								this._realtimeDataSet.Tables[3].Columns["device_id"],
								this._realtimeDataSet.Tables[3].Columns["bank_id"]
							};
							if (this._realtimeDataSet.Tables.Count > 4)
							{
								this._realtimeDataSet.Tables[4].PrimaryKey = new DataColumn[]
								{
									this._realtimeDataSet.Tables[4].Columns["device_id"],
									this._realtimeDataSet.Tables[4].Columns["line_id"]
								};
							}
							if (dataSet != null && dataSet.Tables.Count > 0)
							{
								for (int k = 0; k < dataSet.Tables.Count; k++)
								{
									this._realtimeDataSet.Tables[k].Merge(dataSet.Tables[k], false, MissingSchemaAction.AddWithKey);
								}
								dataSet.Dispose();
							}
						}
					}
					catch (Exception ex)
					{
						Common.WriteLine("DispatchRealtimeData: {0}", new string[]
						{
							ex.Message
						});
					}
				}
				if (this._realtimeDataSet == null)
				{
					return;
				}
				if (this._realtimeIncremental != null)
				{
					this._realtimeIncremental.Dispose();
				}
				this._realtimeIncremental = this._realtimeDataSet.Clone();
			}
			DispatchAttribute dispatchAttribute = new DispatchAttribute();
			dispatchAttribute.type = 1;
			dispatchAttribute.algorithm = 1;
			dispatchAttribute.uid = 0;
			dispatchAttribute.vid = 0;
			dispatchAttribute._msgHandler = this;
			if (flag)
			{
				dispatchAttribute.discard_if_jam = 1;
			}
			this.NewDispatchThread(null, dispatchAttribute);
		}
		public override void OnClose(ConnectContext ctx)
		{
			if (this.cbOnClosed != null)
			{
				Common.WriteLine("    Message Handler Onclosed(), uid={0}", new string[]
				{
					ctx._uid.ToString()
				});
				this.cbOnClosed(ctx, -2);
			}
		}
		protected void DispatchOnDemand(ecoMessage msg, DispatchAttribute attrib)
		{
			this.NewDispatchThread(msg, attrib);
		}
		public bool Sync_StartPullingServeThread(ecoMessage msg, DispatchAttribute attrib)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.Sync_PullingServeThread));
			thread.Name = "Pulling Serve Thread";
			thread.CurrentCulture = CultureInfo.InvariantCulture;
			thread.Priority = ThreadPriority.Highest;
			thread.IsBackground = true;
			long num = Common.ElapsedTime(attrib.tStart);
			Common.WriteLine("Pull Serving Start, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
			{
				attrib.type.ToString("X8"),
				attrib.uid.ToString(),
				attrib.vid.ToString(),
				num.ToString()
			});
			thread.Start(new EcoHandler.SerializeContext
			{
				msg = msg,
				attrib = attrib
			});
			return true;
		}
		public void Sync_PullingServeThread(object context)
		{
			EcoHandler.SerializeContext serializeContext = (EcoHandler.SerializeContext)context;
			ecoMessage msg = serializeContext.msg;
			DispatchAttribute dispatchAttribute = serializeContext.attrib.getCopy();
			UpdateTracker trackerClone = SessionAPI.getTrackerClone((long)serializeContext.attrib.uid, true);
			UserAccessRights userRights = SessionAPI.getUserRights((long)serializeContext.attrib.uid);
			if (trackerClone._UpdateBindings.Count > 0)
			{
				dispatchAttribute = trackerClone._UpdateBindings[0];
				trackerClone._UpdateBindings.RemoveAt(0);
			}
			dispatchAttribute.type |= (int)trackerClone._modifiedSinceLastRequest;
			byte[] array = this.Sync_GetSyncUpdatePack(dispatchAttribute, userRights);
			if (array != null)
			{
				dispatchAttribute.data = array;
				dispatchAttribute.cbCallBack = new DispatchCallback(msg._from.DispatchDataset);
				Compression.CompressThread(dispatchAttribute);
			}
			long num = Common.ElapsedTime(dispatchAttribute.tStart);
			Common.WriteLine("Pull Serving End with Attachment, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
			{
				dispatchAttribute.type.ToString("X8"),
				dispatchAttribute.uid.ToString(),
				dispatchAttribute.vid.ToString(),
				num.ToString()
			});
			trackerClone._modifiedSinceLastRequest &= (uint)(~(uint)dispatchAttribute.type);
		}
		protected byte[] Sync_GetSyncUpdatePack(DispatchAttribute attrib, UserAccessRights userRights)
		{
			byte[] result = null;
			if (attrib.type != 0)
			{
				Common.WriteLine("Pull Data, type={0}, uid={1}, vid={2}", new string[]
				{
					attrib.type.ToString("X8"),
					attrib.uid.ToString(),
					attrib.vid.ToString()
				});
				result = this.Sync_PackUserDataset(attrib, userRights);
			}
			else
			{
				Common.WriteLine("Pull Empty, type={0}, uid={1}, vid={2}", new string[]
				{
					attrib.type.ToString("X8"),
					attrib.uid.ToString(),
					attrib.vid.ToString()
				});
			}
			return result;
		}
		public byte[] Sync_PackUserDataset(DispatchAttribute attrib, UserAccessRights uac)
		{
			long num = (long)attrib.type;
			long num2 = 0L;
			byte[] result = null;
			if ((num & 8L) != 0L)
			{
				num2 |= 8L;
				num2 |= 2L;
				num2 |= 1L;
			}
			if ((num & 8192L) != 0L)
			{
				num2 |= 8192L;
			}
			if ((num & 32L) != 0L)
			{
				num2 |= 32L;
			}
			if ((num & 4L) != 0L)
			{
				num2 |= 4L;
				num2 |= 2L;
			}
			if ((num & 2L) != 0L)
			{
				num2 |= 2L;
			}
			if ((num & 1L) != 0L)
			{
				num2 |= 1L;
			}
			if ((num & 64L) != 0L)
			{
				num2 |= 64L;
			}
			if ((num & 128L) != 0L)
			{
				num2 |= 128L;
			}
			if ((num & 256L) != 0L)
			{
				num2 |= 256L;
			}
			if ((num & 512L) != 0L)
			{
				num2 |= 512L;
			}
			if ((num & 2048L) != 0L)
			{
				num2 |= 2048L;
			}
			MyMemoryStream myMemoryStream = new MyMemoryStream();
			try
			{
				string text = string.Concat(new string[]
				{
					attrib.operation,
					"|",
					attrib.attached,
					"|",
					attrib.guid,
					"|",
					attrib.alltype
				});
				if (!text.Equals("|||"))
				{
					string text2 = text.Trim();
					text2 = text2.Replace("\r", "\n");
					while (text2.IndexOf("\n\n") >= 0)
					{
						text2 = text2.Replace("\n\n", "");
					}
					string[] array = text2.Split(new char[]
					{
						'\n'
					});
					if (array.Length > 0)
					{
						string text3 = "";
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text4 = array2[i];
							if (text4.Trim() != "")
							{
								text3 = text3 + "//////" + text4 + "\r\n";
							}
						}
						if (text3 != "")
						{
							text3 += "\r\n";
							byte[] bytes = Encoding.UTF8.GetBytes(text3);
							myMemoryStream.Write(bytes, 0, bytes.Length);
						}
					}
				}
				if ((num2 & 8L) != 0L)
				{
					DataSetManager.WriteUserUAC(myMemoryStream, uac);
				}
				if ((num2 & 2048L) != 0L)
				{
					DataSetManager.WriteStreamByAutoModel(myMemoryStream, uac);
				}
				if ((num2 & 256L) != 0L)
				{
					DataSetManager.WriteZoneInfo(myMemoryStream, uac);
				}
				if ((num2 & 512L) != 0L)
				{
					DataSetManager.WriteGroupInfo(myMemoryStream, uac);
				}
				if ((num2 & 4L) != 0L)
				{
					DataSetManager.WriteRackInfo(this._dcLayout, myMemoryStream, uac);
					DataSetManager.WriteDevice2Rack(myMemoryStream, uac);
				}
				if ((num2 & 32L) != 0L)
				{
					DataSetManager.WriteSystemSettings(myMemoryStream, uac);
				}
				if ((num2 & 2L) != 0L)
				{
					DataSet dataSet = DataSetManager.FastCreateDataset(null, null, null, null, null, uac);
					string[] tblNames = new string[]
					{
						"device",
						"sensor",
						"port",
						"bank",
						"line"
					};
					this.DataSetToStream(tblNames, dataSet, 8192, myMemoryStream);
					dataSet.Dispose();
				}
				else
				{
					if ((num2 & 128L) != 0L)
					{
						DataSet dataSet2 = null;
						lock (this._lockDataset)
						{
							if (this._baseIncremental != null)
							{
								dataSet2 = this._baseIncremental.Copy();
							}
						}
						if (dataSet2 != null)
						{
							DataSet dataSet3 = this.Sync_GetAuthorizedData(dataSet2, uac);
							if (dataSet3 != null)
							{
								string[] tblNames2 = new string[]
								{
									"dy_device",
									"dy_sensor",
									"dy_port",
									"dy_bank",
									"dy_line"
								};
								this.DataSetToStream(tblNames2, dataSet3, 8192, myMemoryStream);
							}
							dataSet2.Dispose();
						}
					}
				}
				if ((num2 & 1L) != 0L)
				{
					DataSet dataSet4 = null;
					lock (this._lockDataset)
					{
						if (this._realtimeDataSet != null)
						{
							dataSet4 = this._realtimeDataSet.Copy();
						}
					}
					if (dataSet4 != null)
					{
						DataSet dataSet5 = this.Sync_GetAuthorizedData(dataSet4, uac);
						if (dataSet5 != null)
						{
							string[] tblNames3 = new string[]
							{
								"rt_device",
								"rt_sensor",
								"rt_port",
								"rt_bank",
								"rt_line"
							};
							this.DataSetToStream(tblNames3, dataSet5, 8192, myMemoryStream);
						}
						dataSet4.Dispose();
					}
				}
				else
				{
					if ((num2 & 64L) != 0L)
					{
						DataSet dataSet6 = null;
						lock (this._lockDataset)
						{
							if (this._realtimeIncremental != null)
							{
								dataSet6 = this._realtimeIncremental.Copy();
							}
						}
						if (dataSet6 != null)
						{
							DataSet dataSet7 = this.Sync_GetAuthorizedData(dataSet6, uac);
							if (dataSet7 != null)
							{
								string[] tblNames4 = new string[]
								{
									"ud_device",
									"ud_sensor",
									"ud_port",
									"ud_bank",
									"ud_line"
								};
								this.DataSetToStream(tblNames4, dataSet7, 8192, myMemoryStream);
							}
							dataSet6.Dispose();
						}
					}
				}
				if (myMemoryStream.Length > 0L)
				{
					result = myMemoryStream.ToArray();
				}
				myMemoryStream.Close();
				myMemoryStream.Dispose();
			}
			catch (Exception ex)
			{
				Common.WriteLine("Sync_PackUserDataset: " + ex.Message, new string[0]);
			}
			return result;
		}
		protected DataSet Sync_GetAuthorizedData(DataSet dsSnapshot, UserAccessRights uac)
		{
			DataSet dataSet = dsSnapshot.Clone();
			if (uac == null)
			{
				return dataSet;
			}
			try
			{
				for (int i = 0; i < dsSnapshot.Tables.Count; i++)
				{
					foreach (DataRow dataRow in dsSnapshot.Tables[i].Rows)
					{
						long key = (long)Convert.ToInt32(dataRow["device_id"]);
						if (uac._authDevicePortList.ContainsKey(key) && i != 1 && (i == 0 || uac._authDevicePortList[key].Count == 0 || (i == 2 && uac._authDevicePortList[key].Contains((long)Convert.ToInt32(dataRow["port_id"])))))
						{
							dataSet.Tables[i].ImportRow(dataRow);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("Sync_GetAuthorizedData: " + ex.Message, new string[0]);
			}
			return dataSet;
		}
		protected Dictionary<string, string> getValuePairs(byte[] data, int off, int len)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string text = "";
			string text2 = "";
			Stack<string> stack = new Stack<string>();
			try
			{
				MemoryStream input = new MemoryStream(data, off, len);
				using (XmlReader xmlReader = XmlReader.Create(input))
				{
					while (xmlReader.Read())
					{
						switch (xmlReader.NodeType)
						{
						case XmlNodeType.Element:
						{
							text = xmlReader.Name;
							stack.Push(text);
							text2 = "";
							string text3 = "";
							foreach (string current in stack)
							{
								if (text3.Length > 0)
								{
									text3 += ".";
								}
								text3 += current;
							}
							if (stack.Count > 1)
							{
							}
							break;
						}
						case XmlNodeType.Text:
							text2 = xmlReader.Value;
							break;
						case XmlNodeType.EndElement:
							dictionary[text] = text2.Trim();
							stack.Pop();
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return dictionary;
		}
		protected bool ParseUserInfo(byte[] data, int off, int len)
		{
			ValuePairs.setValuePair("uid", "-1");
			ValuePairs.setValuePair("vid", "-1");
			Dictionary<string, string> valuePairs = this.getValuePairs(data, off, len);
			foreach (KeyValuePair<string, string> current in valuePairs)
			{
				ValuePairs.setValuePair(current.Key, current.Value.Trim());
			}
			string valuePair = ValuePairs.getValuePair("uid");
			return Convert.ToInt32(valuePair) > 0;
		}
		protected override void ClientProcessing(ecoMessage msg)
		{
			if (msg == null)
			{
				return;
			}
			BaseTCPClient<EcoContext, EcoHandler> baseTCPClient = (BaseTCPClient<EcoContext, EcoHandler>)msg._from;
			EcoContext ecoContext = (EcoContext)msg._c;
			ulong header = msg._header;
			byte[] array = (byte[])msg._attached;
			if (baseTCPClient == null)
			{
				return;
			}
			List<byte[]> list = new List<byte[]>();
			uint packetType = ecoServerProtocol.getPacketType(header);
			if ((ulong)ecoServerProtocol.getPacketType(header) == 35471uL)
			{
				int num = 0;
				int num2 = (int)array[num];
				if (num2 >= 128)
				{
					num2 -= 256;
				}
				msg._from.setLoginState(num2);
				if (num2 == 1)
				{
					num++;
					int num3 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num));
					num += 2;
					int uid = -1;
					int vid = -1;
					if (num3 > 0 && this.ParseUserInfo(array, num, num3))
					{
						uid = Convert.ToInt32(ValuePairs.getValuePair("uid"));
						vid = Convert.ToInt32(ValuePairs.getValuePair("vid"));
					}
					Common.WriteLine("    Login acked: status={0}, uid={1}, vid={2}", new string[]
					{
						num2.ToString(),
						uid.ToString(),
						vid.ToString()
					});
					msg._from.UpdateUID(uid, vid);
				}
				else
				{
					Common.WriteLine("    Login rejected: code={0}", new string[]
					{
						num2.ToString()
					});
				}
			}
			else
			{
				if ((ulong)packetType == 35457uL)
				{
					int num4 = (int)array[0];
					int num5 = (num4 == 0) ? 1 : -1;
					msg._from.setLoginState(num5);
					if (num5 > 0)
					{
						int vid2 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 1));
						Common.WriteLine("    Login with uid acked: vid={0}, status={1}", new string[]
						{
							vid2.ToString(),
							num4.ToString()
						});
						msg._from.UpdateVID(vid2);
					}
					else
					{
						Common.WriteLine("    Login with uid acked: status={1}", new string[]
						{
							num4.ToString()
						});
					}
				}
				else
				{
					if ((ulong)packetType == 32uL)
					{
						int startIndex = 0;
						Common.WriteLine("    Be kicked: uid={0}", new string[]
						{
							((int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, startIndex))).ToString()
						});
					}
					else
					{
						if ((ulong)packetType == 35459uL)
						{
							Common.WriteLine("    Request ack from ecoServer", new string[0]);
							int num6 = (int)array[0];
							int num7 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 1));
							if (num6 != 0 && (num7 & 4) == 0 && (num7 & 32) == 0 && (num7 & 2) != 0)
							{
							}
						}
						else
						{
							if ((ulong)packetType == 35333uL)
							{
								Common.WriteLine("    Error Message from ecoServer", new string[0]);
								int num8 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
								ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
								if (num8 == 1)
								{
									ClientAPI.OnClosed(ecoContext, -3);
								}
							}
							else
							{
								if ((ulong)packetType == 260uL)
								{
									int num9 = 0;
									ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num9));
									num9 += 2;
									ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num9));
									num9 += 2;
									int num10 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num9));
									num9 += 2;
									int num11 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num9));
									num9 += 2;
									if (num11 > 0)
									{
										string @string = Encoding.UTF8.GetString(array, num9, num11);
										string[] array2 = @string.Split(new char[]
										{
											'|'
										});
										if (array2.Length >= 4)
										{
											string operation = array2[0];
											string carried = array2[1];
											string guid = array2[2];
											string alltype = array2[3];
											ClientAPI.OnReceivedBroadcast((ulong)((long)num10), operation, carried, guid, alltype);
										}
									}
								}
							}
						}
					}
				}
			}
			try
			{
				if (msg != null && msg._from != null)
				{
					foreach (byte[] current in list)
					{
						msg._from.AsyncSend(ecoContext, current);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("ClientProcessing: " + ex.Message, new string[0]);
			}
		}
		public bool StartRemoteCallThread(ecoMessage msg, DispatchAttribute attrib)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.RemoteCallThread));
			thread.Name = "Remote Call Thread";
			thread.CurrentCulture = CultureInfo.InvariantCulture;
			thread.Priority = ThreadPriority.Highest;
			thread.IsBackground = true;
			long num = Common.ElapsedTime(attrib.tStart);
			Common.WriteLine("Remote call start, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
			{
				attrib.type.ToString("X8"),
				attrib.uid.ToString(),
				attrib.vid.ToString(),
				num.ToString()
			});
			thread.Start(new EcoHandler.SerializeContext
			{
				msg = msg,
				attrib = attrib
			});
			return true;
		}
		public void RemoteCallThread(object context)
		{
			EcoHandler.SerializeContext serializeContext = (EcoHandler.SerializeContext)context;
			ecoMessage msg = serializeContext.msg;
			DispatchAttribute attrib = serializeContext.attrib;
			string value = "";
			string para = "";
			int num = attrib.request.IndexOf(',');
			if (num >= 0)
			{
				value = attrib.request.Substring(0, num);
				para = attrib.request.Substring(num + 1);
			}
			byte[] array = AppData.AppProtResponse_Srv(Convert.ToInt32(value), para, attrib.uid);
			if (array != null)
			{
				attrib.data = array;
				if (msg != null && msg._from != null)
				{
					attrib.cbCallBack = new DispatchCallback(msg._from.DispatchDataset);
				}
				else
				{
					attrib.cbCallBack = new DispatchCallback(this.ToAllDispatcher);
				}
				Compression.CompressThread(attrib);
			}
			long num2 = Common.ElapsedTime(attrib.tStart);
			Common.WriteLine("Remote Call End, type={0}, uid={1}, vid={2}, Elapsed={3}", new string[]
			{
				attrib.type.ToString("X8"),
				attrib.uid.ToString(),
				attrib.vid.ToString(),
				num2.ToString()
			});
		}
		private void AppendResponse(List<byte[]> responseArgs, int type, int code, byte[] additional)
		{
			ulong value = 0uL;
			int num = 0;
			if (additional != null && additional.Length > 0)
			{
				num = additional.Length;
			}
			ecoServerProtocol.setPacketPrefix(ref value);
			ecoServerProtocol.setPacketType(ref value, (ulong)((long)type));
			ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(3 + num)));
			value = ecoServerProtocol.swap64(value);
			byte[] bytes = BitConverter.GetBytes(value);
			byte[] array = new byte[9 + num];
			int num2 = 0;
			Array.Copy(bytes, 0, array, num2, 8);
			num2 += 8;
			array[num2] = (byte)code;
			num2++;
			if (num > 0)
			{
				Array.Copy(additional, 0, array, num2, num);
				num2 += num;
			}
			responseArgs.Add(array);
		}
		public string getUserInfo(string user, long uid, long vid, string cookie, bool isTrial, int DaysRemaining)
		{
			string text = "<?xml version=\"1.0\"?>\r\n";
			text += "<UserInfo>\r\n";
			UserInfo userByName = UserMaintain.getUserByName(user);
			if (userByName == null)
			{
				return null;
			}
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"<UserID>",
				userByName.UserID,
				"</UserID>"
			});
			text = text + "<UserName>" + userByName.UserName + "</UserName>";
			object obj2 = text;
			text = string.Concat(new object[]
			{
				obj2,
				"<UserType>",
				userByName.UserType,
				"</UserType>"
			});
			object obj3 = text;
			text = string.Concat(new object[]
			{
				obj3,
				"<UserRight>",
				userByName.UserRight,
				"</UserRight>"
			});
			text = text + "<UserPortNM>" + userByName.UserPortNM + "</UserPortNM>";
			text = text + "<UserDevice>" + userByName.UserDevice + "</UserDevice>";
			text = text + "<UserGroup>" + userByName.UserGroup + "</UserGroup>";
			object obj4 = text;
			text = string.Concat(new object[]
			{
				obj4,
				"<UserStatus>",
				userByName.UserStatus,
				"</UserStatus>"
			});
			text = text + "<user>" + userByName.UserName + "</user>";
			object obj5 = text;
			text = string.Concat(new object[]
			{
				obj5,
				"<uid>",
				uid,
				"</uid>"
			});
			object obj6 = text;
			text = string.Concat(new object[]
			{
				obj6,
				"<vid>",
				vid,
				"</vid>"
			});
			text = text + "<cookie>" + cookie + "</cookie>";
			text = text + "<trial>" + (isTrial ? "1" : "0") + "</trial>";
			object obj7 = text;
			text = string.Concat(new object[]
			{
				obj7,
				"<remaining_days>",
				DaysRemaining,
				"</remaining_days>"
			});
			int cURRENT_PORT = DBUrl.CURRENT_PORT;
			object obj8 = text;
			text = string.Concat(new object[]
			{
				obj8,
				"<dbport>",
				cURRENT_PORT,
				"</dbport>"
			});
			string dB_CURRENT_NAME = DBUrl.DB_CURRENT_NAME;
			text = text + "<serverid>" + dB_CURRENT_NAME + "</serverid>";
			string cURRENT_HOST_PATH = DBUrl.CURRENT_HOST_PATH;
			text = text + "<dbserver>" + cURRENT_HOST_PATH + "</dbserver>";
			string str = "";
			text = text + "<dbuser>" + str + "</dbuser>";
			string str2 = "";
			text = text + "<dbpass>" + str2 + "</dbpass>";
			bool globalEvent = InterProcessEvent.getGlobalEvent("Database_Maintaining", false);
			if (globalEvent)
			{
				text += "<Database_Maintaining>1</Database_Maintaining>";
			}
			else
			{
				text += "<Database_Maintaining>0</Database_Maintaining>";
			}
			return text + "</UserInfo>\r\n";
		}
		protected int LoginCheck(EcoContext c, byte[] packet, ref byte[] buffer)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string master = "";
			string remote = "";
			string text4 = "";
			string client_cookie = "";
			int num = 0;
			int len = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, num));
			num += 2;
			Dictionary<string, string> valuePairs = this.getValuePairs(packet, num, len);
			foreach (KeyValuePair<string, string> current in valuePairs)
			{
				if (current.Key.Equals("AllInOne", StringComparison.InvariantCultureIgnoreCase))
				{
					string value = current.Value;
					string[] array = value.Split(new char[]
					{
						'\n'
					});
					if (array.Length > 0)
					{
						text = array[0].Trim();
					}
					if (array.Length > 1)
					{
						text2 = array[1].Trim();
					}
					if (array.Length > 2)
					{
						text3 = array[2].Trim();
					}
					if (array.Length > 3)
					{
						master = array[3].Trim();
					}
					if (array.Length > 4)
					{
						remote = array[4].Trim();
					}
					if (array.Length > 5)
					{
						text4 = array[5].Trim();
					}
					if (array.Length > 6)
					{
						client_cookie = array[6].Trim();
						break;
					}
					break;
				}
			}
			string text5 = "";
			string text6 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			bool flag = false;
			int num2;
			if (!text3.Equals(text6))
			{
				c._userName = "";
				c._uid = -1L;
				c._vid = -1L;
				num2 = -3;
				Common.WriteLine("Server: client version[{0}] not match with [{1}]", new string[]
				{
					text3,
					text6
				});
			}
			else
			{
				if (EcoHandler._dbStatusCode <= 0L)
				{
					c._userName = "";
					c._uid = -1L;
					c._vid = -1L;
					num2 = -4;
					Common.WriteLine("Server: Authentication Failed", new string[0]);
				}
				else
				{
					if (UserMaintain.checkuser(text, text2))
					{
						long uID = SessionAPI.getUID(text, text2, text4, client_cookie, master, remote, c._sock);
						if (uID < 0L)
						{
							c._userName = "";
							c._uid = -1L;
							c._vid = -1L;
							num2 = (int)uID;
							if (num2 == -5)
							{
								Common.WriteLine("Wrong Serial Number: " + text4, new string[0]);
							}
							else
							{
								if (num2 == -6)
								{
									Common.WriteLine("Serial Number is already in use: " + text4, new string[0]);
								}
								else
								{
									if (num2 == -7)
									{
										Common.WriteLine("Connection limited: " + text4, new string[0]);
									}
									else
									{
										if (num2 == -8)
										{
											Common.WriteLine("Serial Number is old: " + text4, new string[0]);
										}
										else
										{
											Common.WriteLine("Server: Authentication Failed", new string[0]);
										}
									}
								}
							}
						}
						else
						{
							bool isTrial = SessionAPI.isTrial(uID);
							int remainingDays = SessionAPI.getRemainingDays(uID);
							string cookie = SessionAPI.getCookie(uID);
							text5 = this.getUserInfo(text, uID, 0L, cookie, isTrial, remainingDays);
							if (text5 != null)
							{
								UserInfo userByName = UserMaintain.getUserByName(text);
								if (userByName != null)
								{
									SessionAPI.Authorized(text, uID, (long)userByName.UserType, userByName.UserGroup, userByName.UserDevice);
								}
								c._userName = text;
								c._uid = uID;
								c._vid = 0L;
								num2 = 1;
								Common.WriteLine("Server: Login Success", new string[0]);
								flag = true;
							}
							else
							{
								SessionAPI.Delete(uID, true);
								c._userName = "";
								c._uid = -1L;
								c._vid = -1L;
								num2 = -2;
								Common.WriteLine("Server: Authentication Failed", new string[0]);
							}
						}
					}
					else
					{
						c._userName = "";
						c._uid = -1L;
						c._vid = -1L;
						num2 = -2;
						Common.WriteLine("Server: Authentication Failed", new string[0]);
					}
				}
			}
			if (!flag)
			{
				LogAPI.writeEventLog("0220001", new string[]
				{
					text
				});
			}
			int num3 = 0;
			byte[] array2 = null;
			if (!string.IsNullOrEmpty(text5))
			{
				array2 = Encoding.UTF8.GetBytes(text5);
				num3 = array2.Length;
			}
			num = 0;
			buffer = new byte[2 + num3];
			byte[] bytes = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)num3));
			Array.Copy(bytes, 0, buffer, num, 2);
			num += 2;
			if (num3 > 0)
			{
				Array.Copy(array2, 0, buffer, num, num3);
				num += num3;
			}
			return num2;
		}
		protected string getOperationByTag(string tagList, string allOperations)
		{
			string result = "";
			string[] array = tagList.Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string arg_26_0 = array2[i];
			}
			return result;
		}
		protected override void ServerProcessing(ecoMessage msg)
		{
			if (msg == null)
			{
				return;
			}
			try
			{
				BaseTCPServer<EcoContext, EcoHandler> baseTCPServer = (BaseTCPServer<EcoContext, EcoHandler>)msg._from;
				EcoContext ecoContext = (EcoContext)msg._c;
				ulong header = msg._header;
				byte[] array = (byte[])msg._attached;
				if (baseTCPServer != null)
				{
					List<byte[]> list = new List<byte[]>();
					uint packetType = ecoServerProtocol.getPacketType(header);
					if ((ulong)packetType == 385uL)
					{
						Common.WriteLine("    Connect ack from ecoServer", new string[0]);
						this.FirstDispatch(msg);
					}
					else
					{
						if ((ulong)ecoServerProtocol.getPacketType(header) == 1uL)
						{
							int num = 0;
							int num2 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num));
							num += 2;
							ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num));
							num += 2;
							int num3 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, num));
							num += 2;
							if (num3 > 0)
							{
								Dictionary<string, string> valuePairs = this.getValuePairs(array, num, num3);
								if (valuePairs.ContainsKey("RestartListener"))
								{
									bool flag = false;
									string[] array2 = valuePairs["RestartListener"].Split(new char[]
									{
										','
									});
									if (array2.Length >= 2)
									{
										Convert.ToInt32(array2[0]);
										int num4 = Convert.ToInt32(array2[1]);
										if (num4 > 0 && !NetworkShareAccesser.TcpPortInUse(num4))
										{
											flag = true;
										}
									}
									if (flag)
									{
										this.ToAllDispatcher(new DispatchAttribute
										{
											type = 8192,
											uid = 0,
											vid = 0,
											operation = "RestartListener",
											attached = valuePairs["RestartListener"]
										});
										DispatchAPI.Command4Service(valuePairs);
									}
								}
								else
								{
									DispatchAPI.Command4Service(valuePairs);
								}
							}
							SessionAPI.Update((long)num2);
						}
						else
						{
							if ((ulong)ecoServerProtocol.getPacketType(header) == 35343uL)
							{
								byte[] additional = null;
								int code = this.LoginCheck(ecoContext, array, ref additional);
								this.AppendResponse(list, 35471, code, additional);
							}
							else
							{
								if ((ulong)packetType == 35329uL)
								{
									Common.WriteLine("Server: Login with uid Success", new string[0]);
									int num5 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
									ecoContext._uid = (long)num5;
									ecoContext._vid = 2L;
									byte[] array3 = new byte[4];
									Array.Clear(array3, 0, array3.Length);
									byte[] bytes = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)ecoContext._vid));
									Array.Copy(bytes, 0, array3, 0, 2);
									this.AppendResponse(list, 35457, 0, array3);
								}
								else
								{
									if ((ulong)packetType == 35330uL)
									{
										Common.WriteLine("    Logout from ecoClient", new string[0]);
										int num6 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
										int num7 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
										SessionAPI.Logout((long)num6, (long)num7);
										this.AppendResponse(list, 35458, 0, null);
									}
									else
									{
										if ((ulong)packetType == 35331uL)
										{
											int num8 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
											int vid = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
											int num9 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 4));
											Common.WriteLine("Request from ecoClient: uid={0}, vid={1}, type={2}", new string[]
											{
												num8.ToString(),
												vid.ToString(),
												num9.ToString("X8")
											});
											SessionAPI.Update((long)num8);
											int num10 = num9;
											if (msg != null && msg._from != null && (num10 & 1) != 0 && this._realtimeCompressed != null)
											{
												DispatchAttribute dispatchAttribute = new DispatchAttribute();
												dispatchAttribute.type = 1;
												dispatchAttribute.uid = num8;
												dispatchAttribute.vid = vid;
												dispatchAttribute._msgHandler = this;
												Common.WriteLine("    Dispatch from cache: uid={0}, vid={1}, type={2}", new string[]
												{
													num8.ToString(),
													vid.ToString(),
													dispatchAttribute.type.ToString("X8")
												});
												dispatchAttribute.data = this._realtimeCompressed;
												dispatchAttribute.algorithm = this._compressAlgorithm;
												msg._from.DispatchDataset(dispatchAttribute);
												num10 &= -2;
											}
											if (msg != null && msg._from != null && (num10 & 2) != 0 && this._baseCompressed != null)
											{
												DispatchAttribute dispatchAttribute2 = new DispatchAttribute();
												dispatchAttribute2.type = 2;
												dispatchAttribute2.uid = num8;
												dispatchAttribute2.vid = vid;
												dispatchAttribute2._msgHandler = this;
												Common.WriteLine("    Dispatch from cache: uid={0}, vid={1}, type={2}", new string[]
												{
													num8.ToString(),
													vid.ToString(),
													dispatchAttribute2.type.ToString("X8")
												});
												dispatchAttribute2.data = this._baseCompressed;
												dispatchAttribute2.algorithm = this._compressAlgorithm;
												msg._from.DispatchDataset(dispatchAttribute2);
												num10 &= -3;
											}
											if (msg != null && msg._from != null && (num10 & 16384) != 0)
											{
												DispatchAttribute dispatchAttribute3 = new DispatchAttribute();
												dispatchAttribute3.type = 16384;
												dispatchAttribute3.uid = num8;
												dispatchAttribute3.vid = vid;
												dispatchAttribute3._msgHandler = this;
												Common.WriteLine("    Dispatch for pull: uid={0}, vid={1}, type={2}", new string[]
												{
													num8.ToString(),
													vid.ToString(),
													dispatchAttribute3.type.ToString("X8")
												});
												this.Sync_StartPullingServeThread(msg, dispatchAttribute3);
												num10 &= -16385;
											}
											if (num10 != 0)
											{
												DispatchAttribute dispatchAttribute4 = new DispatchAttribute();
												dispatchAttribute4.type = num10;
												dispatchAttribute4.uid = num8;
												dispatchAttribute4.vid = vid;
												dispatchAttribute4._msgHandler = this;
												Common.WriteLine("    Dispatch for other: uid={0}, vid={1}, type={2}", new string[]
												{
													num8.ToString(),
													vid.ToString(),
													dispatchAttribute4.type.ToString("X8")
												});
												this.DispatchOnDemand(msg, dispatchAttribute4);
											}
										}
										else
										{
											if ((ulong)packetType == 35332uL)
											{
												int num11 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
												int vid2 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
												int num12 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 4));
												int num13 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 6));
												Dictionary<string, string> dictionary = null;
												if (num13 > 0)
												{
													dictionary = this.getValuePairs(array, 8, num13);
												}
												string text = "";
												DateTime now = DateTime.Now;
												int num14 = num12;
												if ((num14 & 8192) != 0)
												{
													DispatchAttribute dispatchAttribute5 = new DispatchAttribute();
													dispatchAttribute5.type = 8192;
													dispatchAttribute5.uid = num11;
													dispatchAttribute5.vid = vid2;
													dispatchAttribute5._msgHandler = this;
													if (dictionary != null)
													{
														if (dictionary.ContainsKey("attached"))
														{
															dispatchAttribute5.attached = dictionary["attached"];
														}
														if (dictionary.ContainsKey("operation"))
														{
															dispatchAttribute5.operation = dictionary["operation"];
														}
														if (dictionary.ContainsKey("guid"))
														{
															dispatchAttribute5.guid = dictionary["guid"];
														}
														if (dictionary.ContainsKey("alltype"))
														{
															dispatchAttribute5.alltype = dictionary["alltype"];
														}
														dispatchAttribute5.uid = 0;
														dispatchAttribute5.vid = 0;
													}
													Common.WriteLine("Broadcast: type={0}, uid={1}, vid={2}, msg=[{3}] @ {4}", new string[]
													{
														dispatchAttribute5.type.ToString("X8"),
														num11.ToString(),
														vid2.ToString(),
														text,
														now.ToLongTimeString()
													});
													this.ToAllDispatcher(dispatchAttribute5);
													num14 &= -8193;
												}
												if (num11 != 0)
												{
													SessionAPI.Update((long)num11);
													if ((num14 & 1024) != 0)
													{
														DispatchAttribute dispatchAttribute6 = new DispatchAttribute();
														dispatchAttribute6.type = 1024;
														dispatchAttribute6.uid = num11;
														dispatchAttribute6.vid = vid2;
														dispatchAttribute6._msgHandler = this;
														Common.WriteLine("Remote Call: type={0}, uid={1}, vid={2}, msg=[{3}] @ {4}", new string[]
														{
															dispatchAttribute6.type.ToString("X8"),
															num11.ToString(),
															vid2.ToString(),
															text,
															now.ToLongTimeString()
														});
														Dictionary<string, string> valuePairs2 = this.getValuePairs(array, 8, num13);
														text = valuePairs2["pid"] + "," + valuePairs2["parameter"];
														dispatchAttribute6.cid = Convert.ToInt64(valuePairs2["cid"]);
														dispatchAttribute6.algorithm = Convert.ToInt32(valuePairs2["compress"]);
														dispatchAttribute6.request = text;
														this.StartRemoteCallThread(msg, dispatchAttribute6);
														num14 &= -1025;
													}
												}
												if ((num14 & 64) != 0)
												{
													Common.WriteLine("Power opertion: type={0}, uid={1}, vid={2}, msg=[{3}] @ {4}", new string[]
													{
														64.ToString("X8"),
														num11.ToString(),
														vid2.ToString(),
														text,
														now.ToLongTimeString()
													});
													Dictionary<string, string> valuePairs3 = this.getValuePairs(array, 8, num13);
													this.StartDeviceIncrementalThread(valuePairs3);
													num14 &= -65;
												}
												if ((num14 & 2) != 0)
												{
													DispatchAttribute dispatchAttribute7 = new DispatchAttribute();
													dispatchAttribute7.type = 2;
													dispatchAttribute7.uid = num11;
													dispatchAttribute7.vid = vid2;
													dispatchAttribute7._msgHandler = this;
													if (dictionary != null)
													{
														if (dictionary.ContainsKey("attached"))
														{
															dispatchAttribute7.attached = dictionary["attached"];
														}
														if (dictionary.ContainsKey("operation"))
														{
															dispatchAttribute7.operation = dictionary["operation"];
														}
														if (dictionary.ContainsKey("guid"))
														{
															dispatchAttribute7.guid = dictionary["guid"];
														}
														if (dictionary.ContainsKey("alltype"))
														{
															dispatchAttribute7.alltype = dictionary["alltype"];
														}
													}
													Common.WriteLine("Base Info: uid={0}, vid={1}, type={2}", new string[]
													{
														num11.ToString(),
														vid2.ToString(),
														dispatchAttribute7.type.ToString("X8")
													});
													this.NewDispatchThread(msg, dispatchAttribute7);
													num14 &= -3;
												}
												if ((num14 & 1) != 0)
												{
													DispatchAttribute dispatchAttribute8 = new DispatchAttribute();
													dispatchAttribute8.type = 1;
													dispatchAttribute8.uid = num11;
													dispatchAttribute8.vid = vid2;
													dispatchAttribute8._msgHandler = this;
													if (dictionary != null)
													{
														if (dictionary.ContainsKey("attached"))
														{
															dispatchAttribute8.attached = dictionary["attached"];
														}
														if (dictionary.ContainsKey("operation"))
														{
															dispatchAttribute8.operation = dictionary["operation"];
														}
														if (dictionary.ContainsKey("guid"))
														{
															dispatchAttribute8.guid = dictionary["guid"];
														}
														if (dictionary.ContainsKey("alltype"))
														{
															dispatchAttribute8.alltype = dictionary["alltype"];
														}
													}
													Common.WriteLine("Realtime Data: uid={0}, vid={1}, type={2}", new string[]
													{
														num11.ToString(),
														vid2.ToString(),
														dispatchAttribute8.type.ToString("X8")
													});
													this.NewDispatchThread(msg, dispatchAttribute8);
													num14 &= -2;
												}
												if (num14 != 0)
												{
													if ((num14 & 2048) != 0)
													{
														Dictionary<string, string> valuePairs4 = this.getValuePairs(array, 8, num13);
														try
														{
															string text2 = valuePairs4["model_key"];
															DevModelConfig value = default(DevModelConfig);
															value.modelName = valuePairs4["modelName"];
															value.firmwareVer = valuePairs4["firmwareVer"];
															value.autoBasicInfo = valuePairs4["autoBasicInfo"];
															value.autoRatingInfo = valuePairs4["autoRatingInfo"];
															lock (EcoHandler._lockAutoModel)
															{
																if (!string.IsNullOrEmpty(text2))
																{
																	if (EcoHandler._autoModelSink == null)
																	{
																		EcoHandler._autoModelSink = DataSetManager.getAutoModelList();
																	}
																	else
																	{
																		EcoHandler._autoModelSink[text2] = value;
																	}
																}
																DevAccessCfg.GetInstance().LoadAutoDeviceModelConfig();
															}
														}
														catch (Exception)
														{
														}
													}
													DispatchAttribute dispatchAttribute9 = new DispatchAttribute();
													dispatchAttribute9.type = num14;
													dispatchAttribute9.uid = num11;
													dispatchAttribute9.vid = vid2;
													dispatchAttribute9._msgHandler = this;
													Common.WriteLine("Change Notify: type={0}, uid={1}, vid={2}, msg=[{3}] @ {4}", new string[]
													{
														dispatchAttribute9.type.ToString("X8"),
														num11.ToString(),
														vid2.ToString(),
														text,
														now.ToLongTimeString()
													});
													dispatchAttribute9.attached = "";
													dispatchAttribute9.operation = "";
													dispatchAttribute9.guid = "";
													dispatchAttribute9.alltype = "";
													if (dictionary != null)
													{
														if (dictionary.ContainsKey("attached"))
														{
															dispatchAttribute9.attached = dictionary["attached"];
														}
														if (dictionary.ContainsKey("operation"))
														{
															dispatchAttribute9.operation = dictionary["operation"];
														}
														if (dictionary.ContainsKey("guid"))
														{
															dispatchAttribute9.guid = dictionary["guid"];
														}
														if (dictionary.ContainsKey("alltype"))
														{
															dispatchAttribute9.alltype = dictionary["alltype"];
														}
													}
													this.DispatchOnDemand(null, dispatchAttribute9);
												}
												this.AppendResponse(list, 35460, 0, null);
											}
											else
											{
												if ((ulong)packetType == 259uL)
												{
													int num15 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
													int type = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
													ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 4));
													SessionAPI.Update((long)num15);
													DateTime now2 = DateTime.Now;
													Common.WriteLine("Message from web via ecoServer: type={0}, uid={1} @ {2}", new string[]
													{
														type.ToString("X8"),
														num15.ToString(),
														now2.ToLongTimeString()
													});
													if (msg != null && msg._from != null)
													{
														DispatchAttribute dispatchAttribute10 = new DispatchAttribute();
														dispatchAttribute10.algorithm = 1;
														dispatchAttribute10.type = type;
														dispatchAttribute10.uid = 0;
														dispatchAttribute10.vid = 0;
														msg._from.DispatchDataset(dispatchAttribute10);
													}
													this.AppendResponse(list, 387, 1, null);
												}
												else
												{
													if ((ulong)packetType == 386uL)
													{
														if (ecoContext != null && ecoContext._uid != 0L)
														{
															SessionAPI.Update(ecoContext._uid);
														}
													}
													else
													{
														if ((ulong)packetType == 35328uL)
														{
															int num16 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
															SessionAPI.Update((long)num16);
															this.AppendResponse(list, 35456, 0, null);
														}
														else
														{
															if ((ulong)packetType == 35330uL)
															{
																Common.WriteLine("    Logout from ecoClient", new string[0]);
																int num17 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 0));
																int num18 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(array, 2));
																SessionAPI.Logout((long)num17, (long)num18);
																this.AppendResponse(list, 35458, 0, null);
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
					foreach (byte[] current in list)
					{
						baseTCPServer.AsyncSend(ecoContext, current);
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("ServerProcessing: " + ex.Message, new string[0]);
			}
		}
		public override void AutoPull4UAC(ConnectContext c)
		{
			try
			{
				ulong header = 0uL;
				ecoServerProtocol.setPacketPrefix(ref header);
				ecoServerProtocol.setPacketType(ref header, 35331uL);
				ecoServerProtocol.setPacketLen(ref header, 8uL);
				byte[] array = new byte[6];
				int num = 0;
				byte[] bytes = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)c._uid));
				Array.Copy(bytes, 0, array, num, 2);
				num += 2;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)c._vid));
				Array.Copy(bytes2, 0, array, num, 2);
				num += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16(16384));
				Array.Copy(bytes3, 0, array, num, 2);
				num += 2;
				this.PutMesssage(c._owner, c, 6, header, array);
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
		}
		public override byte[] BuildMsg4Service(int uid, int vid, string msg)
		{
			try
			{
				Common.WriteLine("    Send service management: " + msg, new string[0]);
				int num = 0;
				byte[] array = null;
				if (msg.Length > 0)
				{
					array = Encoding.UTF8.GetBytes(msg);
					num = array.Length;
				}
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				int num2 = 6 + num;
				ecoServerProtocol.setPacketType(ref value, 1uL);
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(2 + num2)));
				value = ecoServerProtocol.swap64(value);
				byte[] bytes = BitConverter.GetBytes(value);
				int num3 = 0;
				byte[] array2 = new byte[8 + num2];
				Array.Copy(bytes, 0, array2, num3, 8);
				num3 += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
				Array.Copy(bytes2, 0, array2, num3, 2);
				num3 += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)vid));
				Array.Copy(bytes3, 0, array2, num3, 2);
				num3 += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)num));
				Array.Copy(bytes4, 0, array2, num3, 2);
				num3 += 2;
				if (num > 0)
				{
					Array.Copy(array, 0, array2, num3, num);
					num3 += num;
				}
				if (array2.Length > 0)
				{
					return array2;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public override byte[] BuildLogin(object cfgClient)
		{
			try
			{
				ClientConfig clientConfig = (ClientConfig)cfgClient;
				int uid = clientConfig.uid;
				int arg_14_0 = clientConfig.vid;
				string text = "<Login>\r\n";
				text += string.Format("<AllInOne>{0}</AllInOne>\r\n", clientConfig.info4Login);
				text += "</Login>";
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				int num;
				if (uid < 0)
				{
					num = 2;
					ecoServerProtocol.setPacketType(ref value, 257uL);
				}
				else
				{
					if (uid > 0)
					{
						num = 2;
						ecoServerProtocol.setPacketType(ref value, 35329uL);
					}
					else
					{
						num = 2 + bytes.Length;
						ecoServerProtocol.setPacketType(ref value, 35343uL);
					}
				}
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(2 + num)));
				value = ecoServerProtocol.swap64(value);
				byte[] bytes2 = BitConverter.GetBytes(value);
				byte[] array = new byte[8 + num];
				int num2 = 0;
				Array.Copy(bytes2, 0, array, num2, 8);
				num2 += 8;
				if (uid < 0)
				{
					int num3 = 0;
					byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)num3));
					Array.Copy(bytes3, 0, array, num2, 2);
					num2 += 2;
				}
				else
				{
					if (uid > 0)
					{
						Common.WriteLine("    Send login: uid={0}", new string[]
						{
							uid.ToString()
						});
						byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
						Array.Copy(bytes3, 0, array, num2, 2);
						num2 += 2;
					}
					else
					{
						Common.WriteLine("    Send auth: info={0}", new string[]
						{
							clientConfig.info4Login
						});
						byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)bytes.Length));
						Array.Copy(bytes4, 0, array, num2, 2);
						num2 += 2;
						Array.Copy(bytes, 0, array, num2, bytes.Length);
						num2 += bytes.Length;
					}
				}
				if (array.Length > 0)
				{
					return array;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public override byte[] BuildLogout(int uid, int vid)
		{
			try
			{
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 35330uL);
				ecoServerProtocol.setPacketLen(ref value, 6uL);
				value = ecoServerProtocol.swap64(value);
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[12];
				int num = 0;
				Array.Copy(bytes, 0, array, num, 8);
				num += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
				Array.Copy(bytes2, 0, array, num, 2);
				num += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)vid));
				Array.Copy(bytes3, 0, array, num, 2);
				num += 2;
				Common.WriteLine("    Send logout: uid={0}", new string[]
				{
					uid.ToString()
				});
				if (array.Length > 0)
				{
					return array;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public byte[] BuildUrgencyPacket(Socket sock, int uid, string op)
		{
			try
			{
				ulong value;
				if (op.Equals("kick", StringComparison.InvariantCultureIgnoreCase))
				{
					value = 32uL;
				}
				else
				{
					if (!op.Equals("servicewilldown", StringComparison.InvariantCultureIgnoreCase))
					{
						byte[] result = null;
						return result;
					}
					value = 48uL;
				}
				ulong value2 = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value2);
				ecoServerProtocol.setPacketType(ref value2, value);
				int num = 2;
				ecoServerProtocol.setPacketLen(ref value2, (ulong)((long)(2 + num)));
				value2 = ecoServerProtocol.swap64(value2);
				byte[] bytes = BitConverter.GetBytes(value2);
				byte[] array = new byte[8 + num];
				int num2 = 0;
				Array.Copy(bytes, 0, array, num2, 8);
				num2 += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
				Array.Copy(bytes2, 0, array, num2, 2);
				num2 += 2;
				if (array.Length > 0)
				{
					byte[] result = array;
					return result;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public override byte[] BuildKeepAlive(int uid, int vid)
		{
			try
			{
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				int num;
				if (uid < 0)
				{
					ecoServerProtocol.setPacketType(ref value, 256uL);
					num = 2;
				}
				else
				{
					ecoServerProtocol.setPacketType(ref value, 35328uL);
					num = 4;
				}
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(2 + num)));
				value = ecoServerProtocol.swap64(value);
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[8 + num];
				int num2 = 0;
				Array.Copy(bytes, 0, array, num2, 8);
				num2 += 8;
				if (uid < 0)
				{
					byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16(0));
					Array.Copy(bytes2, 0, array, num2, 2);
					num2 += 2;
				}
				else
				{
					byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
					Array.Copy(bytes3, 0, array, num2, 2);
					num2 += 2;
					byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)vid));
					Array.Copy(bytes4, 0, array, num2, 2);
					num2 += 2;
				}
				if (array.Length > 0)
				{
					return array;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public override byte[] BuildRequest(int uid, int vid, int dataType)
		{
			try
			{
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 35331uL);
				ecoServerProtocol.setPacketLen(ref value, 8uL);
				value = ecoServerProtocol.swap64(value);
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[14];
				int num = 0;
				Array.Copy(bytes, 0, array, num, 8);
				num += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
				Array.Copy(bytes2, 0, array, num, 2);
				num += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)vid));
				Array.Copy(bytes3, 0, array, num, 2);
				num += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)dataType));
				Array.Copy(bytes4, 0, array, num, 2);
				num += 2;
				return array;
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
			return null;
		}
		public override byte[] BuildMessage(int uid, int vid, int type, bool broadcast, byte[] message)
		{
			try
			{
				if (broadcast)
				{
					uid = 0;
					vid = 0;
				}
				int num = 0;
				if (message != null && message.Length > 0)
				{
					num += message.Length;
				}
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 35332uL);
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(10 + num)));
				value = ecoServerProtocol.swap64(value);
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[16 + num];
				int num2 = 0;
				Array.Copy(bytes, 0, array, num2, 8);
				num2 += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)uid));
				Array.Copy(bytes2, 0, array, num2, 2);
				num2 += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)vid));
				Array.Copy(bytes3, 0, array, num2, 2);
				num2 += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)type));
				Array.Copy(bytes4, 0, array, num2, 2);
				num2 += 2;
				byte[] bytes5 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)num));
				Array.Copy(bytes5, 0, array, num2, 2);
				num2 += 2;
				if (message != null && message.Length > 0)
				{
					Array.Copy(message, 0, array, num2, message.Length);
					num2 += message.Length;
				}
				return array;
			}
			catch (Exception ex)
			{
				Common.WriteLine("SendMessage failed: {0}", new string[]
				{
					ex.Message
				});
			}
			return null;
		}
		public override byte[] BuildBroadcast(DispatchAttribute attr)
		{
			try
			{
				ulong value = 0uL;
				string s = string.Concat(new string[]
				{
					attr.operation,
					"|",
					attr.attached,
					"|",
					attr.guid,
					"|",
					attr.alltype
				});
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 260uL);
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(10 + bytes.Length)));
				value = ecoServerProtocol.swap64(value);
				int num = 0;
				byte[] bytes2 = BitConverter.GetBytes(value);
				byte[] array = new byte[16 + bytes.Length];
				Array.Copy(bytes2, 0, array, num, 8);
				num += 8;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16(0));
				Array.Copy(bytes3, 0, array, num, 2);
				num += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16(0));
				Array.Copy(bytes4, 0, array, num, 2);
				num += 2;
				byte[] bytes5 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.type));
				Array.Copy(bytes5, 0, array, num, 2);
				num += 2;
				byte[] bytes6 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)bytes.Length));
				Array.Copy(bytes6, 0, array, num, 2);
				num += 2;
				Array.Copy(bytes, 0, array, num, bytes.Length);
				num += bytes.Length;
				Common.WriteLine("    Disptach broadcast: total {0} bytes", new string[]
				{
					bytes.Length.ToString()
				});
				return array;
			}
			catch (Exception ex)
			{
				Common.WriteLine("Disptach broadcast failed: {0}", new string[]
				{
					ex.Message
				});
			}
			return null;
		}
		public override byte[] BuildFirstDispatch(DispatchAttribute attr)
		{
			try
			{
				attr.block_no = 0;
				attr.dispatchPointer = 0;
				ulong value = 0uL;
				MD5 mD = MD5.Create();
				byte[] array = mD.ComputeHash(attr.data, 0, attr.data.Length);
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 258uL);
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(21 + array.Length)));
				value = ecoServerProtocol.swap64(value);
				int num = 0;
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array2 = new byte[27 + array.Length];
				Array.Copy(bytes, 0, array2, num, 8);
				num += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.uid));
				Array.Copy(bytes2, 0, array2, num, 2);
				num += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.vid));
				Array.Copy(bytes3, 0, array2, num, 2);
				num += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.type));
				Array.Copy(bytes4, 0, array2, num, 2);
				num += 2;
				array2[num] = 1;
				num++;
				ushort value2 = 0;
				byte[] bytes5 = BitConverter.GetBytes(ecoServerProtocol.swap16(value2));
				Array.Copy(bytes5, 0, array2, num, 2);
				num += 2;
				uint value3 = (uint)attr.data.Length;
				byte[] bytes6 = BitConverter.GetBytes(ecoServerProtocol.swap32(value3));
				Array.Copy(bytes6, 0, array2, num, 4);
				num += 4;
				value2 = (ushort)(4 + array.Length);
				bytes5 = BitConverter.GetBytes(ecoServerProtocol.swap16(value2));
				Array.Copy(bytes5, 0, array2, num, 2);
				num += 2;
				uint algorithm = (uint)attr.algorithm;
				byte[] bytes7 = BitConverter.GetBytes(ecoServerProtocol.swap32(algorithm));
				Array.Copy(bytes7, 0, array2, num, 4);
				num += 4;
				Array.Copy(array, 0, array2, num, array.Length);
				num += array.Length;
				attr.block_no++;
				long num2 = Common.ElapsedTime(attr.tStart);
				Common.WriteLine("    Disptach first block: type={0}, uid={1}, total {2} bytes, Elapsed={3}", new string[]
				{
					attr.type.ToString("X8"),
					attr.uid.ToString(),
					attr.data.Length.ToString(),
					num2.ToString()
				});
				return array2;
			}
			catch (Exception ex)
			{
				Common.WriteLine("DisptachDataSetStart failed: {0}", new string[]
				{
					ex.Message
				});
			}
			return null;
		}
		public override byte[] BuildNextDispatch(DispatchAttribute attr)
		{
			try
			{
				int num = Math.Min(4096, attr.data.Length - attr.dispatchPointer);
				ulong value = 0uL;
				ecoServerProtocol.setPacketPrefix(ref value);
				ecoServerProtocol.setPacketType(ref value, 258uL);
				ecoServerProtocol.setPacketLen(ref value, (ulong)((long)(13 + num)));
				value = ecoServerProtocol.swap64(value);
				int num2 = 0;
				byte[] bytes = BitConverter.GetBytes(value);
				byte[] array = new byte[19 + num];
				Array.Copy(bytes, 0, array, num2, 8);
				num2 += 8;
				byte[] bytes2 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.uid));
				Array.Copy(bytes2, 0, array, num2, 2);
				num2 += 2;
				byte[] bytes3 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.vid));
				Array.Copy(bytes3, 0, array, num2, 2);
				num2 += 2;
				byte[] bytes4 = BitConverter.GetBytes(ecoServerProtocol.swap16((ushort)attr.type));
				Array.Copy(bytes4, 0, array, num2, 2);
				num2 += 2;
				array[num2] = 0;
				if (attr.data.Length - attr.dispatchPointer == num)
				{
					array[num2] = 2;
				}
				num2++;
				ushort value2 = (ushort)attr.block_no;
				byte[] bytes5 = BitConverter.GetBytes(ecoServerProtocol.swap16(value2));
				Array.Copy(bytes5, 0, array, num2, 2);
				num2 += 2;
				ushort value3 = (ushort)num;
				byte[] bytes6 = BitConverter.GetBytes(ecoServerProtocol.swap16(value3));
				Array.Copy(bytes6, 0, array, num2, 2);
				num2 += 2;
				Array.Copy(attr.data, attr.dispatchPointer, array, num2, num);
				if (attr.data.Length - attr.dispatchPointer == num)
				{
					long num3 = Common.ElapsedTime(attr.tStart);
					Common.WriteLine("    Disptach last block: type={0}, block={1} @ {2}, {3} bytes, Elapsed={4}", new string[]
					{
						attr.type.ToString("X8"),
						attr.block_no.ToString(),
						attr.dispatchPointer.ToString(),
						num.ToString(),
						num3.ToString()
					});
				}
				attr.block_no++;
				attr.dispatchPointer += num;
				return array;
			}
			catch (Exception ex)
			{
				Common.WriteLine("DispatchNext failed: {0}", new string[]
				{
					ex.Message
				});
			}
			return null;
		}
	}
}
