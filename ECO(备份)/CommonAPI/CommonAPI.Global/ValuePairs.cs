using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml;
namespace CommonAPI.Global
{
	public class ValuePairs
	{
		private const string regkey_SerialNo = "SerialNo";
		public const string regkey_esSrvIP = "MasterIP";
		public const string regkey_MngPort = "ServicePort";
		public const string regkey_ErrCode = "ServiceStatus";
		private const string regkey_IdleTimeout = "IdleTimeout";
		public const string regpath_master32 = "SOFTWARE\\ATEN\\ecoSensors";
		public const string regpath_master64 = "SOFTWARE\\Wow6432Node\\ATEN\\ecoSensors";
		public const string regpath_client32 = "SOFTWARE\\ATEN\\ecoSensorsClient";
		public const string regpath_client64 = "SOFTWARE\\Wow6432Node\\ATEN\\ecoSensorsClient";
		private static string sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static Dictionary<string, string> _cmdline = new Dictionary<string, string>();
		private static object _pairLock = new object();
		public static int getValuePairsCount()
		{
			int count;
			lock (ValuePairs._pairLock)
			{
				count = ValuePairs._cmdline.Count;
			}
			return count;
		}
		public static string getSerialNo(bool forRemote = true)
		{
			string result;
			lock (ValuePairs._pairLock)
			{
				string text = "";
				string name = "SOFTWARE\\ATEN\\ecoSensors";
				if (forRemote)
				{
					name = "SOFTWARE\\ATEN\\ecoSensorsClient";
				}
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
				if (registryKey != null && registryKey.GetValue("SerialNo") != null)
				{
					text = registryKey.GetValue("SerialNo").ToString();
				}
				result = text;
			}
			return result;
		}
		public static int getIdleTimeout(bool islocal)
		{
			int result;
			lock (ValuePairs._pairLock)
			{
				int num = 0;
				RegistryKey registryKey;
				if (islocal)
				{
					string name = "SOFTWARE\\ATEN\\ecoSensors";
					registryKey = Registry.LocalMachine.OpenSubKey(name, true);
					if (registryKey == null)
					{
						result = num;
						return result;
					}
				}
				else
				{
					string name = "SOFTWARE\\ATEN\\ecoSensorsClient";
					registryKey = Registry.CurrentUser.OpenSubKey(name, true);
					if (registryKey == null)
					{
						result = num;
						return result;
					}
				}
				if (registryKey.GetValue("IdleTimeout") != null)
				{
					num = (int)registryKey.GetValue("IdleTimeout");
				}
				registryKey.Close();
				result = num;
			}
			return result;
		}
		public static void setIdleTimeout(int IdleTime, bool islocal)
		{
			lock (ValuePairs._pairLock)
			{
				RegistryKey registryKey;
				if (islocal)
				{
					string text = "SOFTWARE\\ATEN\\ecoSensors";
					registryKey = Registry.LocalMachine.OpenSubKey(text, true);
					if (registryKey == null)
					{
						registryKey = Registry.LocalMachine.CreateSubKey(text);
					}
					if (registryKey != null)
					{
						registryKey.SetValue("IdleTimeout", IdleTime);
					}
				}
				else
				{
					string text = "SOFTWARE\\ATEN\\ecoSensorsClient";
					registryKey = Registry.CurrentUser.OpenSubKey(text, true);
					if (registryKey == null)
					{
						registryKey = Registry.CurrentUser.CreateSubKey(text);
					}
					if (registryKey != null)
					{
						registryKey.SetValue("IdleTimeout", IdleTime);
					}
				}
				registryKey.Close();
			}
		}
		public static string getValuePair(string key)
		{
			string result;
			lock (ValuePairs._pairLock)
			{
				string text = null;
				if (ValuePairs._cmdline != null && ValuePairs._cmdline.ContainsKey(key.ToLower()))
				{
					text = ValuePairs._cmdline[key.ToLower()];
				}
				result = text;
			}
			return result;
		}
		public static void setValuePair(string key, string value)
		{
			lock (ValuePairs._pairLock)
			{
				if (ValuePairs._cmdline != null)
				{
					ValuePairs._cmdline[key.ToLower()] = value;
				}
			}
		}
		public static bool isMyIP(string serverIP)
		{
			bool result;
			lock (ValuePairs._pairLock)
			{
				bool flag2 = false;
				try
				{
					if (serverIP.Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					if (serverIP.Equals("127.0.0.1", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					if (serverIP.Equals("0:0:0:0:0:0:0:1", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					if (serverIP.Equals("0:0:0:0:0:0:0:0", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					if (serverIP.Equals("::1", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					if (serverIP.Equals("::", StringComparison.CurrentCultureIgnoreCase))
					{
						result = true;
						return result;
					}
					IPAddress iPAddress = IPAddress.Parse(serverIP);
					NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
					for (int i = 0; i < allNetworkInterfaces.Length; i++)
					{
						NetworkInterface networkInterface = allNetworkInterfaces[i];
						if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
						{
							Console.WriteLine(networkInterface.Name);
							foreach (UnicastIPAddressInformation current in networkInterface.GetIPProperties().UnicastAddresses)
							{
								if (current.Address.AddressFamily == AddressFamily.InterNetwork && current.Address.ToString() == iPAddress.ToString())
								{
									flag2 = true;
									break;
								}
							}
						}
						if (flag2)
						{
							break;
						}
					}
				}
				catch (Exception)
				{
				}
				result = flag2;
			}
			return result;
		}
		private static byte[] A_B_C_D(string str)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(str);
			char c = '\0';
			for (int i = 0; i < bytes.Length; i++)
			{
				byte[] arg_21_0 = bytes;
				int arg_21_1 = i;
				char arg_1F_0 = (char)(bytes[i] - 1);
				char expr_1A = c;
				c = Convert.ToChar( expr_1A + '\u0001');
				arg_21_0[arg_21_1] = (byte)(arg_1F_0 ^ expr_1A);
			}
			return bytes;
		}
		private static string A_B_C_D_Reverse(byte[] ac)
		{
			char c = '\0';
			for (int i = 0; i < ac.Length; i++)
			{
				int arg_15_1 = i;
				char arg_11_0 = (char)ac[i];
				char expr_0C = c;
				c = Convert.ToChar( expr_0C + '\u0001');
				ac[arg_15_1] = (byte)((arg_11_0 ^ expr_0C) + '\u0001');
			}
			return Encoding.ASCII.GetString(ac);
		}
		public static Dictionary<string, string> LoadValueKeyFromXML(string file)
		{
			Dictionary<string, string> result;
			lock (ValuePairs._pairLock)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string text = "";
				string text2 = "";
				Stack<string> stack = new Stack<string>();
				try
				{
					string text3 = ValuePairs.sCodebase + "\\" + file;
					if (File.Exists(text3))
					{
						using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(text3))
						{
							while (xmlReader.Read())
							{
								switch (xmlReader.NodeType)
								{
								case XmlNodeType.Element:
								{
									text = xmlReader.Name;
									stack.Push(text);
									string text4 = "";
									using (Stack<string>.Enumerator enumerator = stack.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											string current = enumerator.Current;
											if (text4.Length > 0)
											{
												text4 += ".";
											}
											text4 += current;
										}
										continue;
									}
									break;
								}
								case XmlNodeType.Attribute:
								case XmlNodeType.CDATA:
								case XmlNodeType.EntityReference:
								case XmlNodeType.Entity:
								case XmlNodeType.ProcessingInstruction:
								case XmlNodeType.Comment:
								case XmlNodeType.Document:
								case XmlNodeType.DocumentType:
								case XmlNodeType.DocumentFragment:
								case XmlNodeType.Notation:
								case XmlNodeType.Whitespace:
								case XmlNodeType.SignificantWhitespace:
								case XmlNodeType.EndEntity:
								case XmlNodeType.XmlDeclaration:
									continue;
								case XmlNodeType.Text:
									break;
								case XmlNodeType.EndElement:
									dictionary[text] = text2.Trim();
									stack.Pop();
									continue;
								default:
									continue;
								}
								text2 = xmlReader.Value;
							}
						}
					}
				}
				catch (Exception)
				{
				}
				result = dictionary;
			}
			return result;
		}
		public static Dictionary<string, string> LoadValueKeyFromRegistry(bool islocal)
		{
			Dictionary<string, string> result;
			lock (ValuePairs._pairLock)
			{
				ValuePairs._cmdline.Clear();
				try
				{
					ValuePairs._cmdline["MasterIP".ToLower()] = "127.0.0.1";
					if (islocal)
					{
						string name = "SOFTWARE\\ATEN\\ecoSensors";
						string name2 = "SOFTWARE\\Wow6432Node\\ATEN\\ecoSensors";
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
						if (registryKey == null)
						{
							registryKey = Registry.LocalMachine.OpenSubKey(name2);
						}
						if (registryKey == null)
						{
							DebugCenter.GetInstance().appendToFile("Failed to read registry");
							result = null;
							return result;
						}
						if (registryKey.GetValue("ServicePort") != null)
						{
							string text = registryKey.GetValue("ServicePort").ToString();
							if (text != null)
							{
								ValuePairs._cmdline["ServicePort".ToLower()] = text;
							}
							DebugCenter.GetInstance().appendToFile("Listen port=" + text);
						}
						registryKey.Close();
					}
					else
					{
						ValuePairs._cmdline["MasterIP".ToLower()] = "";
						string text2 = "";
						string name = "SOFTWARE\\ATEN\\ecoSensorsClient";
						string name2 = "SOFTWARE\\Wow6432Node\\ATEN\\ecoSensorsClient";
						RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name);
						if (registryKey == null)
						{
							registryKey = Registry.CurrentUser.OpenSubKey(name2);
						}
						if (registryKey == null)
						{
							DebugCenter.GetInstance().appendToFile("Failed to read registry: Client -> currentuser");
						}
						else
						{
							if (registryKey.GetValue("MasterIP") != null)
							{
								string text3 = registryKey.GetValue("MasterIP").ToString();
								if (text3 != null)
								{
									ValuePairs._cmdline["MasterIP".ToLower()] = text3;
								}
								DebugCenter.GetInstance().appendToFile("Server IP=" + text3);
							}
							if (registryKey.GetValue("ServicePort") != null)
							{
								string text4 = registryKey.GetValue("ServicePort").ToString();
								if (text4 != null)
								{
									text2 = text4;
								}
								DebugCenter.GetInstance().appendToFile("Listen port=" + text2);
							}
						}
						if (text2.Length == 0)
						{
							RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(name);
							if (registryKey2 == null)
							{
								registryKey2 = Registry.LocalMachine.OpenSubKey(name2);
							}
							if (registryKey2 == null)
							{
								DebugCenter.GetInstance().appendToFile("Failed to read registry: Client -> localmachine");
							}
							else
							{
								string text5 = registryKey2.GetValue("ServicePort").ToString();
								if (text5 != null)
								{
									text2 = text5;
								}
								DebugCenter.GetInstance().appendToFile("Listen port=" + text5);
								registryKey2.Close();
							}
						}
						ValuePairs._cmdline["ServicePort".ToLower()] = text2;
						registryKey.Close();
					}
				}
				catch (Exception ex)
				{
					DebugCenter.GetInstance().appendToFile(ex.Message);
				}
				result = ValuePairs._cmdline;
			}
			return result;
		}
		public static void SaveValueKeyToRegistry(bool islocal)
		{
			lock (ValuePairs._pairLock)
			{
				try
				{
					RegistryKey registryKey;
					if (islocal)
					{
						string text = "SOFTWARE\\ATEN\\ecoSensors";
						registryKey = Registry.LocalMachine.OpenSubKey(text, true);
						if (registryKey == null)
						{
							registryKey = Registry.LocalMachine.CreateSubKey(text);
						}
					}
					else
					{
						string text = "SOFTWARE\\ATEN\\ecoSensorsClient";
						registryKey = Registry.CurrentUser.OpenSubKey(text, true);
						if (registryKey == null)
						{
							registryKey = Registry.CurrentUser.CreateSubKey(text);
						}
					}
					if (registryKey != null)
					{
						string value = ValuePairs._cmdline["ServicePort".ToLower()];
						if (!islocal)
						{
							string value2 = ValuePairs._cmdline["MasterIP".ToLower()];
							registryKey.SetValue("MasterIP", value2);
						}
						registryKey.SetValue("ServicePort", value);
						registryKey.Close();
					}
				}
				catch (Exception)
				{
				}
			}
		}
		private static string regString4Java(string str)
		{
			string text = "";
			char[] array = str.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if (c >= 'A' && c <= 'Z')
				{
					text = text + "/" + c;
				}
				else
				{
					text += c;
				}
			}
			return text;
		}
	}
}
