using CommonAPI;
using DBAccessAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
namespace EcoDevice.AccessAPI
{
	public class DefaultSnmpExecutors : SnmpExecutors
	{
		private System.Collections.Generic.List<SnmpConfiger> snmpConfigs;
		private System.Collections.Generic.List<SnmpConfiger> retryconfigs = new System.Collections.Generic.List<SnmpConfiger>();
		public static object lockGroup = new object();
		public static object lockRestore = new object();
		public bool OnLine
		{
			get
			{
				SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
				System.Collections.Generic.List<bool> results = systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
				{
					System.Collections.Generic.List<bool> list = (System.Collections.Generic.List<bool>)coll;
					SnmpConfiger snmpConfig = (SnmpConfiger)obj;
					SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfig);
					bool flag = false;
					try
					{
						flag = snmpExecutor.Online;
					}
					catch (System.Exception)
					{
					}
					finally
					{
						lock (coll)
						{
							if (list.Count > 0)
							{
								list.Add(flag && list[list.Count - 1]);
							}
							else
							{
								list.Add(flag);
							}
						}
					}
				});
				return results.Count >= 1 && results[results.Count - 1];
			}
		}
		public System.Collections.Generic.List<SnmpConfiger> getRetryList()
		{
			return this.retryconfigs;
		}
		public DefaultSnmpExecutors(System.Collections.Generic.List<SnmpConfiger> snmpConfigs)
		{
			this.snmpConfigs = snmpConfigs;
		}
		public void GetProperties(ExecutorCallBack callBack)
		{
			foreach (SnmpConfiger current in this.snmpConfigs)
			{
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(current);
				snmpExecutor.GetProperties(callBack);
			}
		}
		public System.Collections.Generic.List<PropertiesMessage> GetProperties_ALL()
		{
			System.DateTime createTime = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				PropertiesMessage propertiesMessage = null;
				try
				{
					propertiesMessage = snmpExecutor.GetProperties_ALL();
					propertiesMessage.CreateTime = createTime;
					propertiesMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					propertiesMessage.PortNums = snmpConfiger.PortNumbers;
					propertiesMessage.PerPortReading = snmpConfiger.PerPortReading;
					propertiesMessage.Switchable = snmpConfiger.Switchable;
					propertiesMessage.SensorNums = snmpConfiger.SensorNumber;
				}
				catch (System.Exception)
				{
				}
				if (propertiesMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<PropertiesMessage>)col).Add(propertiesMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<PropertiesMessage> GetProperties_ATEN()
		{
			System.DateTime createTime = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				PropertiesMessage propertiesMessage = null;
				try
				{
					propertiesMessage = snmpExecutor.GetProperties_ATEN();
					propertiesMessage.CreateTime = createTime;
					propertiesMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					propertiesMessage.PortNums = snmpConfiger.PortNumbers;
					propertiesMessage.PerPortReading = snmpConfiger.PerPortReading;
					propertiesMessage.Switchable = snmpConfiger.Switchable;
					propertiesMessage.SensorNums = snmpConfiger.SensorNumber;
				}
				catch (System.Exception)
				{
				}
				if (propertiesMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<PropertiesMessage>)col).Add(propertiesMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<PropertiesMessage> GetEatonPDUProperties()
		{
			System.DateTime createTime = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				PropertiesMessage propertiesMessage = null;
				try
				{
					propertiesMessage = snmpExecutor.GetEatonPDUProperties();
					propertiesMessage.CreateTime = createTime;
					propertiesMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					propertiesMessage.PortNums = snmpConfiger.PortNumbers;
					propertiesMessage.PerPortReading = snmpConfiger.PerPortReading;
					propertiesMessage.Switchable = snmpConfiger.Switchable;
					propertiesMessage.SensorNums = snmpConfiger.SensorNumber;
				}
				catch (System.Exception)
				{
				}
				if (propertiesMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<PropertiesMessage>)col).Add(propertiesMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<PropertiesMessage> GetEatonPDUProperties_M2()
		{
			System.DateTime createTime = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				PropertiesMessage propertiesMessage = null;
				try
				{
					propertiesMessage = snmpExecutor.GetEatonPDUProperties_M2();
					propertiesMessage.CreateTime = createTime;
					propertiesMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					propertiesMessage.PortNums = snmpConfiger.PortNumbers;
					propertiesMessage.PerPortReading = snmpConfiger.PerPortReading;
					propertiesMessage.Switchable = snmpConfiger.Switchable;
					propertiesMessage.SensorNums = snmpConfiger.SensorNumber;
				}
				catch (System.Exception)
				{
				}
				if (propertiesMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<PropertiesMessage>)col).Add(propertiesMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<PropertiesMessage> GetApcPDUProperties()
		{
			System.DateTime createTime = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				PropertiesMessage propertiesMessage = null;
				try
				{
					propertiesMessage = snmpExecutor.GetApcPDUProperties();
					propertiesMessage.CreateTime = createTime;
					propertiesMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					propertiesMessage.PortNums = snmpConfiger.PortNumbers;
					propertiesMessage.PerPortReading = snmpConfiger.PerPortReading;
					propertiesMessage.Switchable = snmpConfiger.Switchable;
					propertiesMessage.SensorNums = snmpConfiger.SensorNumber;
				}
				catch (System.Exception)
				{
				}
				if (propertiesMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<PropertiesMessage>)col).Add(propertiesMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<PropertiesMessage> GetChainProperties()
		{
			System.DateTime arg_05_0 = System.DateTime.Now;
			SystemThreadPool<SnmpConfiger, PropertiesMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, PropertiesMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfig = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfig);
				System.Collections.Generic.List<PropertiesMessage> list = new System.Collections.Generic.List<PropertiesMessage>();
				try
				{
					list = snmpExecutor.GetChainProperties();
				}
				catch (System.Exception)
				{
				}
				if (list != null && list.Count > 0)
				{
					lock (col)
					{
						foreach (PropertiesMessage current in list)
						{
							((System.Collections.Generic.List<PropertiesMessage>)col).Add(current);
						}
					}
				}
			});
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsEatonPDU_M2()
		{
			System.DateTime createTime = System.DateTime.Now;
			System.Collections.Generic.List<ThresholdMessage> list = new System.Collections.Generic.List<ThresholdMessage>();
			SystemThreadPool<SnmpConfiger, ThresholdMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ThresholdMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				ThresholdMessage thresholdMessage = null;
				try
				{
					thresholdMessage = snmpExecutor.GetThresholdsEatonPDU_M2();
					thresholdMessage.CreateTime = createTime;
					thresholdMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					thresholdMessage.PortNums = snmpConfiger.PortNumbers;
					thresholdMessage.PerPortReading = snmpConfiger.PerPortReading;
					thresholdMessage.Switchable = snmpConfiger.Switchable;
					thresholdMessage.SensorNums = snmpConfiger.SensorNumber;
					thresholdMessage.BankNums = snmpConfiger.BankNumber;
					thresholdMessage.DeviceID = snmpConfiger.DeviceID;
					thresholdMessage.DeviceMac = snmpConfiger.DeviceMac;
				}
				catch (System.Exception)
				{
				}
				if (thresholdMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<ThresholdMessage>)col).Add(thresholdMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholdsApcPDU()
		{
			System.DateTime createTime = System.DateTime.Now;
			System.Collections.Generic.List<ThresholdMessage> list = new System.Collections.Generic.List<ThresholdMessage>();
			SystemThreadPool<SnmpConfiger, ThresholdMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ThresholdMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				ThresholdMessage thresholdMessage = null;
				try
				{
					thresholdMessage = snmpExecutor.GetThresholdsApcPDU();
					thresholdMessage.CreateTime = createTime;
					thresholdMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					thresholdMessage.PortNums = snmpConfiger.PortNumbers;
					thresholdMessage.PerPortReading = snmpConfiger.PerPortReading;
					thresholdMessage.Switchable = snmpConfiger.Switchable;
					thresholdMessage.SensorNums = snmpConfiger.SensorNumber;
					thresholdMessage.BankNums = snmpConfiger.BankNumber;
					thresholdMessage.DeviceID = snmpConfiger.DeviceID;
					thresholdMessage.DeviceMac = snmpConfiger.DeviceMac;
				}
				catch (System.Exception)
				{
				}
				if (thresholdMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<ThresholdMessage>)col).Add(thresholdMessage);
					}
				}
			});
		}
		public System.Collections.Generic.List<ValueMessage> GetVaules()
		{
			System.Threading.ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum * 2, ThreadUtil.CompletionPortNum * 2);
			int num;
			int num2;
			System.Threading.ThreadPool.GetMaxThreads(out num, out num2);
			System.DateTime createTime = System.DateTime.Now;
			System.Collections.Generic.List<ValueMessage> list = new System.Collections.Generic.List<ValueMessage>();
			SystemThreadPool<SnmpConfiger, ValueMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ValueMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				ValueMessage valueMessage = null;
				try
				{
					if (snmpConfiger.DevModelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
					{
						valueMessage = snmpExecutor.GetValuesEatonPDU_M2();
					}
					else
					{
						if (snmpConfiger.DevModelConfig.commonThresholdFlag == Constant.APC_PDU)
						{
							valueMessage = snmpExecutor.GetValuesApcPDU();
						}
						else
						{
							valueMessage = snmpExecutor.GetValues();
						}
					}
				}
				catch (System.Exception)
				{
				}
				if (valueMessage == null)
				{
					valueMessage = new ValueMessage();
				}
				valueMessage.CreateTime = createTime;
				valueMessage.ModelName = snmpConfiger.DevModel;
				valueMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
				valueMessage.PortNums = snmpConfiger.PortNumbers;
				valueMessage.PerPortReading = snmpConfiger.PerPortReading;
				valueMessage.Switchable = snmpConfiger.Switchable;
				valueMessage.SensorNums = snmpConfiger.SensorNumber;
				valueMessage.BankNums = snmpConfiger.BankNumber;
				valueMessage.DeviceID = snmpConfiger.DeviceID;
				valueMessage.DeviceMac = snmpConfiger.DeviceMac;
				lock (col)
				{
					((System.Collections.Generic.List<ValueMessage>)col).Add(valueMessage);
				}
			});
		}
		public System.Collections.Generic.List<ValueMessage> GetChainVaules()
		{
			System.Threading.ThreadPool.SetMaxThreads(ThreadUtil.WorkThreadNum * 2, ThreadUtil.CompletionPortNum * 2);
			int num;
			int num2;
			System.Threading.ThreadPool.GetMaxThreads(out num, out num2);
			System.DateTime arg_22_0 = System.DateTime.Now;
			System.Collections.Generic.List<ValueMessage> list = new System.Collections.Generic.List<ValueMessage>();
			SystemThreadPool<SnmpConfiger, ValueMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ValueMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfig = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfig);
				System.Collections.Generic.List<ValueMessage> list2 = new System.Collections.Generic.List<ValueMessage>();
				try
				{
					list2 = snmpExecutor.GetChainValues();
				}
				catch (System.Exception)
				{
				}
				if (list2 != null && list2.Count > 0)
				{
					lock (col)
					{
						foreach (ValueMessage current in list2)
						{
							((System.Collections.Generic.List<ValueMessage>)col).Add(current);
						}
					}
				}
			});
		}
		public System.Collections.Generic.List<ThresholdMessage> GetThresholds()
		{
			this.retryconfigs.Clear();
			System.DateTime createTime = System.DateTime.Now;
			System.Collections.Generic.List<ThresholdMessage> list = new System.Collections.Generic.List<ThresholdMessage>();
			SystemThreadPool<SnmpConfiger, ThresholdMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ThresholdMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				ThresholdMessage thresholdMessage = null;
				try
				{
					if (snmpConfiger.DevModelConfig.commonThresholdFlag == Constant.EatonPDU_M2)
					{
						thresholdMessage = snmpExecutor.GetThresholdsEatonPDU_M2();
					}
					else
					{
						if (snmpConfiger.DevModelConfig.commonThresholdFlag == Constant.APC_PDU)
						{
							thresholdMessage = snmpExecutor.GetThresholdsApcPDU();
						}
						else
						{
							thresholdMessage = snmpExecutor.GetThresholds();
						}
					}
					thresholdMessage.CreateTime = createTime;
					thresholdMessage.IpAddress = snmpConfiger.SnmpConfig.AgentIp;
					thresholdMessage.PortNums = snmpConfiger.PortNumbers;
					thresholdMessage.PerPortReading = snmpConfiger.PerPortReading;
					thresholdMessage.Switchable = snmpConfiger.Switchable;
					thresholdMessage.SensorNums = snmpConfiger.SensorNumber;
					thresholdMessage.BankNums = snmpConfiger.BankNumber;
					thresholdMessage.DeviceID = snmpConfiger.DeviceID;
					thresholdMessage.DeviceMac = snmpConfiger.DeviceMac;
				}
				catch (System.Exception)
				{
				}
				if (thresholdMessage != null)
				{
					lock (col)
					{
						((System.Collections.Generic.List<ThresholdMessage>)col).Add(thresholdMessage);
						return;
					}
				}
				this.retryconfigs.Add(snmpConfiger);
			});
		}
		public System.Collections.Generic.List<ThresholdMessage> GetChainThresholds()
		{
			System.DateTime arg_05_0 = System.DateTime.Now;
			System.Collections.Generic.List<ThresholdMessage> list = new System.Collections.Generic.List<ThresholdMessage>();
			SystemThreadPool<SnmpConfiger, ThresholdMessage> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, ThresholdMessage>(this.snmpConfigs);
			return systemThreadPool.GetResults(delegate(System.Collections.ICollection col, object obj)
			{
				SnmpConfiger snmpConfig = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfig);
				System.Collections.Generic.List<ThresholdMessage> list2 = new System.Collections.Generic.List<ThresholdMessage>();
				try
				{
					list2 = snmpExecutor.GetChainThresholds();
				}
				catch (System.Exception)
				{
				}
				if (list2 != null && list2.Count > 0)
				{
					lock (col)
					{
						foreach (ThresholdMessage current in list2)
						{
							((System.Collections.Generic.List<ThresholdMessage>)col).Add(current);
						}
					}
				}
			});
		}
		public void UpdateTrapReceiver(Sys_Para pSys, TrapEnabled open)
		{
			if (pSys.TrapPort <= 0 || pSys.TrapPort > 65535)
			{
				return;
			}
			DebugCenter debug = DebugCenter.GetInstance();
			debug.appendToFile("*****=====***** Begin to send trap receiver config to device *****=====*****");
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				System.Collections.Generic.List<System.Net.IPAddress> managerLocalIpAddress = this.getManagerLocalIpAddress();
				System.Net.IPAddress iPAddress = managerLocalIpAddress[0];
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				if (snmpConfiger.DevModelConfig.commonThresholdFlag != Constant.EatonPDU_M2 && snmpConfiger.DevModelConfig.commonThresholdFlag != Constant.APC_PDU && !snmpConfiger.SnmpConfig.AgentIp.Equals("127.0.0.1"))
				{
					try
					{
						if (managerLocalIpAddress.Count > 1)
						{
							iPAddress = this.getLocalSameNetworkIP(snmpConfiger.SnmpConfig.AgentIp);
							if (iPAddress.ToString().Equals("127.0.0.1"))
							{
								int deviceHttpPort = snmpExecutor.DeviceHttpPort;
								if (deviceHttpPort > 0)
								{
									iPAddress = this.getLocalConnectibleIP(snmpConfiger.SnmpConfig.AgentIp, deviceHttpPort);
									if (iPAddress.ToString().Equals("127.0.0.1"))
									{
										debug.appendToFile("Failed to get connectiable IP, Device IP: " + snmpConfiger.SnmpConfig.AgentIp);
										if (iPAddress.ToString().Equals("127.0.0.1"))
										{
											iPAddress = managerLocalIpAddress[0];
										}
									}
								}
								else
								{
									debug.appendToFile(string.Concat(new object[]
									{
										"Failed to get device http port: ",
										deviceHttpPort,
										", Device IP: ",
										snmpConfiger.SnmpConfig.AgentIp
									}));
									iPAddress = managerLocalIpAddress[0];
								}
							}
						}
						TrapReceiverConfiguration trapReceiverConfig = snmpExecutor.GetTrapReceiverConfig(1);
						if (trapReceiverConfig != null && (trapReceiverConfig.Enabled != open || trapReceiverConfig.TrapVersion != pSys.TrapSnmpVersion || trapReceiverConfig.TrapPort != pSys.TrapPort || trapReceiverConfig.ReceiverIp.ToString() != iPAddress.ToString()))
						{
							snmpExecutor.ConfigTrapReceiver(new TrapReceiverConfiguration(1)
							{
								AgentVersion = (int)snmpConfiger.SnmpConfig.Version,
								Enabled = open,
								ReceiverIp = iPAddress,
								TrapPort = pSys.TrapPort,
								TrapVersion = pSys.TrapSnmpVersion,
								Community = pSys.TrapUserName,
								Username = pSys.TrapUserName,
								AuthPassword = pSys.TrapAuthenPwd,
								PrivPassword = pSys.TrapPrivacyPwd
							});
						}
					}
					catch (System.Exception)
					{
					}
				}
			});
		}
		private System.Collections.Generic.List<System.Net.IPAddress> getManagerLocalIpAddress()
		{
			System.Collections.Generic.List<System.Net.IPAddress> list = new System.Collections.Generic.List<System.Net.IPAddress>();
			System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
			System.Net.IPAddress[] addressList = hostEntry.AddressList;
			for (int i = 0; i < addressList.Length; i++)
			{
				System.Net.IPAddress iPAddress = addressList[i];
				if (iPAddress.AddressFamily.ToString() == "InterNetwork")
				{
					System.Net.IPAddress item = System.Net.IPAddress.Parse(iPAddress.ToString());
					list.Add(item);
				}
			}
			return list;
		}
		private System.Net.IPAddress getLocalSameNetworkIP(string deviceIP)
		{
			string ipString = "127.0.0.1";
			System.Net.IPAddress iPAddress = System.Net.IPAddress.Parse(deviceIP);
			System.Net.NetworkInformation.NetworkInterface[] allNetworkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				System.Net.NetworkInformation.NetworkInterface networkInterface = allNetworkInterfaces[i];
				if (networkInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
				{
					foreach (System.Net.NetworkInformation.UnicastIPAddressInformation current in networkInterface.GetIPProperties().UnicastAddresses)
					{
						if (current.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && current.IPv4Mask != null && (iPAddress.Address & current.IPv4Mask.Address) == (current.Address.Address & current.IPv4Mask.Address))
						{
							return current.Address;
						}
					}
				}
			}
			return System.Net.IPAddress.Parse(ipString);
		}
		private System.Net.IPAddress getLocalConnectibleIP(string remoteIP, int remotePort)
		{
			string text = "127.0.0.1";
			System.Net.Sockets.Socket socket = null;
			try
			{
				socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
				System.IAsyncResult asyncResult = socket.BeginConnect(remoteIP, remotePort, null, null);
				bool flag = asyncResult.AsyncWaitHandle.WaitOne(5000, true);
				if (flag && socket.Connected)
				{
					text = this.GetLocalEndPoint(socket);
					int length = text.IndexOf(":");
					text = text.Substring(0, length);
				}
			}
			catch (System.Exception)
			{
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			return System.Net.IPAddress.Parse(text);
		}
		private string GetLocalEndPoint(System.Net.Sockets.Socket s)
		{
			System.Net.NetworkInformation.IPGlobalProperties iPGlobalProperties = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties();
			System.Net.NetworkInformation.TcpConnectionInformation[] activeTcpConnections = iPGlobalProperties.GetActiveTcpConnections();
			System.Net.IPEndPoint iPEndPoint = (System.Net.IPEndPoint)s.RemoteEndPoint;
			System.Net.NetworkInformation.TcpConnectionInformation[] array = activeTcpConnections;
			for (int i = 0; i < array.Length; i++)
			{
				System.Net.NetworkInformation.TcpConnectionInformation tcpConnectionInformation = array[i];
				if (iPEndPoint.ToString() == tcpConnectionInformation.RemoteEndPoint.ToString())
				{
					return new System.Net.IPEndPoint(tcpConnectionInformation.LocalEndPoint.Address, ((System.Net.IPEndPoint)s.LocalEndPoint).Port).ToString();
				}
			}
			return "127.0.0.1:0";
		}
		public string[] TurnOnGroupOutlets()
		{
			string failDID = "";
			string successDID = "";
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string text = "";
				bool flag = false;
				try
				{
					foreach (int current in snmpConfiger.GroupOutlets)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += current;
					}
					flag = snmpExecutor.TurnOnGroupOutlets(snmpConfiger.GroupOutlets);
				}
				catch (System.Exception)
				{
				}
				lock (DefaultSnmpExecutors.lockGroup)
				{
					if (flag)
					{
						if (!string.IsNullOrEmpty(text))
						{
							object successDID;
							if (!string.IsNullOrEmpty(successDID))
							{
								successDID += ";";
							}
							successDID = successDID;
							successDID = string.Concat(new object[]
							{
								successDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(text))
						{
							object failDID;
							if (!string.IsNullOrEmpty(failDID))
							{
								failDID += ";";
							}
							failDID = failDID;
							failDID = string.Concat(new object[]
							{
								failDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
				}
			});
			return new string[]
			{
				failDID,
				successDID
			};
		}
		public string[] TurnOffGroupOutlets()
		{
			string failDID = "";
			string successDID = "";
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string text = "";
				bool flag = false;
				try
				{
					foreach (int current in snmpConfiger.GroupOutlets)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += current;
					}
					flag = snmpExecutor.TurnOffGroupOutlets(snmpConfiger.GroupOutlets);
				}
				catch (System.Exception)
				{
				}
				lock (DefaultSnmpExecutors.lockGroup)
				{
					if (flag)
					{
						if (!string.IsNullOrEmpty(text))
						{
							object successDID;
							if (!string.IsNullOrEmpty(successDID))
							{
								successDID += ";";
							}
							successDID = successDID;
							successDID = string.Concat(new object[]
							{
								successDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(text))
						{
							object failDID;
							if (!string.IsNullOrEmpty(failDID))
							{
								failDID += ";";
							}
							failDID = failDID;
							failDID = string.Concat(new object[]
							{
								failDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
				}
			});
			return new string[]
			{
				failDID,
				successDID
			};
		}
		public string[] RebootGroupOutlets()
		{
			string failDID = "";
			string successDID = "";
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				string text = "";
				bool flag = false;
				try
				{
					foreach (int current in snmpConfiger.GroupOutlets)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += ",";
						}
						text += current;
					}
					flag = snmpExecutor.RebootGroupOutlets(snmpConfiger.GroupOutlets);
				}
				catch (System.Exception)
				{
				}
				lock (DefaultSnmpExecutors.lockGroup)
				{
					if (flag)
					{
						if (!string.IsNullOrEmpty(text))
						{
							object successDID;
							if (!string.IsNullOrEmpty(successDID))
							{
								successDID += ";";
							}
							successDID = successDID;
							successDID = string.Concat(new object[]
							{
								successDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(text))
						{
							object failDID;
							if (!string.IsNullOrEmpty(failDID))
							{
								failDID += ";";
							}
							failDID = failDID;
							failDID = string.Concat(new object[]
							{
								failDID,
								snmpConfiger.DeviceID,
								":",
								text
							});
						}
					}
				}
			});
			return new string[]
			{
				failDID,
				successDID
			};
		}
		public string[] GetStatusGroupOutlets()
		{
			string failDID = "";
			string successDID = "";
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				try
				{
					System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
					for (int i = 1; i <= snmpConfiger.DevModelConfig.portNum; i++)
					{
						if (snmpConfiger.DevModelConfig.isOutletSwitchable(i - 1))
						{
							list.Add(i);
						}
					}
					if (snmpExecutor.GetDeviceOutletsStatus(list) == null)
					{
						lock (DefaultSnmpExecutors.lockGroup)
						{
							failDID = failDID + snmpConfiger.DeviceID + ",";
							goto IL_DA;
						}
					}
					lock (DefaultSnmpExecutors.lockGroup)
					{
						successDID = successDID + snmpConfiger.DeviceID + ",";
					}
					IL_DA:;
				}
				catch (System.Exception)
				{
					lock (DefaultSnmpExecutors.lockGroup)
					{
						failDID = failDID + snmpConfiger.DeviceID + ",";
					}
				}
			});
			return new string[]
			{
				failDID,
				successDID
			};
		}
		public string[] RestoreThresholds2Device()
		{
			string failMAC = "";
			string successDID = "";
			string misMAC = "";
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				try
				{
					bool flag = false;
					int num = snmpExecutor.CheckDeviceMac(snmpConfiger.DeviceMac);
					if (num >= 0)
					{
						flag = snmpExecutor.UpdateDeviceName(snmpConfiger.RestoreThresholds.DeviceName);
					}
					else
					{
						if (num == -1)
						{
							lock (DefaultSnmpExecutors.lockRestore)
							{
								misMAC = misMAC + snmpConfiger.DeviceMac + ",";
							}
						}
					}
					if (flag)
					{
						DevicePOPSettings devicePOPSettings = new DevicePOPSettings();
						devicePOPSettings.copyPOPSetings(snmpConfiger.RestoreThresholds.DeviceThreshold);
						snmpExecutor.SetDevicePOPSettings(devicePOPSettings);
						snmpExecutor.SetDeviceThreshold(snmpConfiger.RestoreThresholds.DeviceThreshold);
						for (int i = 0; i < snmpConfiger.RestoreThresholds.SensorThreshold.Count; i++)
						{
							snmpExecutor.SetSensorThreshold(snmpConfiger.RestoreThresholds.SensorThreshold[i + 1]);
						}
						for (int j = 0; j < snmpConfiger.RestoreThresholds.BankThreshold.Count; j++)
						{
							snmpExecutor.SetBankThreshold(snmpConfiger.RestoreThresholds.BankThreshold[j + 1]);
						}
						for (int k = 0; k < snmpConfiger.RestoreThresholds.OutletThreshold.Count; k++)
						{
							snmpExecutor.SetOutletThreshold(snmpConfiger.RestoreThresholds.OutletThreshold[k + 1]);
						}
						for (int l = 0; l < snmpConfiger.RestoreThresholds.LineThreshold.Count; l++)
						{
							snmpExecutor.SetLineThreshold(snmpConfiger.RestoreThresholds.LineThreshold[l + 1]);
						}
					}
					if (!flag)
					{
						lock (DefaultSnmpExecutors.lockRestore)
						{
							failMAC = failMAC + snmpConfiger.DeviceMac + ",";
							goto IL_217;
						}
					}
					lock (DefaultSnmpExecutors.lockRestore)
					{
						successDID = successDID + snmpConfiger.DeviceID + ",";
					}
					IL_217:;
				}
				catch (System.Exception ex)
				{
					lock (DefaultSnmpExecutors.lockRestore)
					{
						failMAC = failMAC + snmpConfiger.DeviceMac + ",";
					}
					DebugCenter instance = DebugCenter.GetInstance();
					instance.appendToFile("Failed to restore config to device: " + snmpConfiger.RestoreThresholds.DeviceName + ", " + ex.Message);
				}
			});
			return new string[]
			{
				failMAC,
				successDID,
				misMAC
			};
		}
		public System.Collections.Generic.List<SnmpConfiger> UpdateDeviceVoltages(float voltage)
		{
			System.Collections.Generic.List<SnmpConfiger> failConfigs = new System.Collections.Generic.List<SnmpConfiger>();
			SystemThreadPool<SnmpConfiger, bool> systemThreadPool = new SystemLargeThreadPool<SnmpConfiger, bool>(this.snmpConfigs);
			systemThreadPool.GetResults(delegate(System.Collections.ICollection coll, object obj)
			{
				SnmpConfiger snmpConfiger = (SnmpConfiger)obj;
				SnmpExecutor snmpExecutor = new DefaultSnmpExecutor(snmpConfiger);
				try
				{
					if (!snmpExecutor.UpdateDeviceVoltage(voltage))
					{
						lock (DefaultSnmpExecutors.lockRestore)
						{
							failConfigs.Add(snmpConfiger);
						}
					}
				}
				catch (System.Exception)
				{
					lock (DefaultSnmpExecutors.lockRestore)
					{
						failConfigs.Add(snmpConfiger);
					}
				}
			});
			return failConfigs;
		}
	}
}
