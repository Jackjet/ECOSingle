using CommonAPI.Global;
using CommonAPI.InterProcess;
using EcoMessenger;
using ecoProtocols;
using SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
namespace Dispatcher
{
	public static class ServicesAPI
	{
		public delegate void DelegateOnAccepted(Socket sock);
		public delegate void DelegateOnDisconnected(Socket sock);
		private static string _sCodebase = AppDomain.CurrentDomain.BaseDirectory;
		private static BaseTCPServer<EcoContext, EcoHandler> _ecoListener = null;
		private static BillingHandler _billingHandler = null;
		private static BaseTCPServer<BillingContext, BillingHandler> _billingService = null;
		public static ServicesAPI.DelegateOnAccepted cbOnAccepted = null;
		public static ServicesAPI.DelegateOnDisconnected cbOnDisconnected = null;
		public static bool StartEcoService(bool ssl, int port, EcoHandler handle)
		{
			Common.WriteLine("StartEcoService... ssl=" + ssl.ToString() + ", port=" + port.ToString(), new string[0]);
			bool flag = false;
			try
			{
				ServerConfig serverConfig = new ServerConfig();
				serverConfig.ssl = ssl;
				serverConfig.MaxListen = 100;
				serverConfig.BufferSize = 4096;
				serverConfig.Protocol = ecoServerProtocol.ServerTypes.TCP;
				serverConfig.serverName = "Eco Server";
				serverConfig.tokenList = "";
				serverConfig.MaxConnect = 3000;
				serverConfig.port = 7979;
				if (port != 0)
				{
					serverConfig.port = port;
				}
				if (serverConfig.ssl)
				{
					if (File.Exists(ServicesAPI._sCodebase + "MonitorServers.pfx"))
					{
						serverConfig.TlsCertificate = new X509Certificate2(ServicesAPI._sCodebase + "MonitorServers.pfx", "ecoserver");
						ServicesAPI._ecoListener = new ServerSSL<EcoContext, EcoHandler>(serverConfig);
					}
					else
					{
						Common.WriteLine("Server Certificate is lost: {0}", new string[]
						{
							ServicesAPI._sCodebase + "MonitorServers.pfx"
						});
					}
				}
				else
				{
					ServicesAPI._ecoListener = new BaseTCPServer<EcoContext, EcoHandler>(serverConfig);
				}
				if (ServicesAPI._ecoListener != null)
				{
					if (port == 0)
					{
						Random random = new Random();
						for (int i = 128; i > 0; i++)
						{
							int usePort = 30000 + random.Next(35534);
							flag = ServicesAPI._ecoListener.Start(handle, usePort);
							if (flag)
							{
								InterProcessShared<Dictionary<string, string>>.setInterProcessKeyValue("Listen_Port", usePort.ToString());
								ValuePairs.setValuePair("ServicePort", usePort.ToString());
								ValuePairs.SaveValueKeyToRegistry(true);
								break;
							}
						}
					}
					else
					{
						flag = ServicesAPI._ecoListener.Start(handle, -1);
						if (flag)
						{
							ValuePairs.setValuePair("ServicePort", port.ToString());
							ValuePairs.SaveValueKeyToRegistry(true);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine("StartEcoService: {0}", new string[]
				{
					ex.Message
				});
			}
			return flag;
		}
		public static void StopEcoService()
		{
			Common.WriteLine("StopEcoService", new string[0]);
			if (ServicesAPI._ecoListener != null)
			{
				ServicesAPI._ecoListener.Stop();
			}
			ServicesAPI._ecoListener = null;
		}
		public static bool RestartListener(bool newSSL, int newPort, EcoHandler handle)
		{
			ServicesAPI.StopEcoService();
			bool flag = ServicesAPI.StartEcoService(newSSL, newPort, handle);
			if (flag)
			{
				Common.WriteLine(string.Concat(new object[]
				{
					"Eco Listener is restarted successfully, ssl=",
					newSSL,
					", port=",
					newPort
				}), new string[0]);
			}
			else
			{
				Common.WriteLine(string.Concat(new object[]
				{
					"Failed to restart Eco Listener, ssl=",
					newSSL,
					", port=",
					newPort
				}), new string[0]);
			}
			return flag;
		}
		public static void OnAccepted(Socket sock)
		{
			Common.WriteLine("OnAccepted: {0}", new string[]
			{
				sock.LocalEndPoint.ToString()
			});
			if (ServicesAPI.cbOnAccepted != null)
			{
				ServicesAPI.cbOnAccepted(sock);
			}
		}
		public static void OnDisconnected(Socket sock)
		{
			if (ServicesAPI.cbOnDisconnected != null)
			{
				ServicesAPI.cbOnDisconnected(sock);
			}
		}
		public static bool StartBillingService(ServerConfig cfg, int replacePort, string token)
		{
			Common.WriteLine("StartEnquiryServer", new string[0]);
			ServicesAPI._billingHandler = new BillingHandler();
			ServicesAPI._billingHandler.Start(true);
			ServerConfig serverConfig = cfg;
			if (cfg == null)
			{
				serverConfig = new ServerConfig();
				serverConfig.ssl = false;
				serverConfig.MaxListen = 100;
				serverConfig.BufferSize = 4096;
				serverConfig.Protocol = ecoServerProtocol.ServerTypes.TCP;
				serverConfig.serverName = "Billing Server";
				serverConfig.port = 8888;
				serverConfig.MaxConnect = 3000;
				if (replacePort > 0)
				{
					serverConfig.port = replacePort;
				}
				serverConfig.tokenList = "12345678";
				if (token != null && token != string.Empty)
				{
					serverConfig.tokenList = token;
				}
			}
			if (serverConfig.Protocol == ecoServerProtocol.ServerTypes.TCP)
			{
				ServicesAPI._billingService = new BaseTCPServer<BillingContext, BillingHandler>(serverConfig);
				if (ServicesAPI._billingService != null)
				{
					ServicesAPI._billingService.setAuthorizedString(true, serverConfig.tokenList);
					if (ServicesAPI._billingService.Start(ServicesAPI._billingHandler, -1))
					{
						Common.WriteLine("Billing Listener is started successfully, port=" + serverConfig.port, new string[0]);
						return true;
					}
				}
				Common.WriteLine("Failed to start Billing Listener, port=" + serverConfig.port, new string[0]);
				return false;
			}
			throw new InvalidOperationException("Protocol not supported.");
		}
		public static void StopBillingService()
		{
			try
			{
				Common.WriteLine("StopBillingService", new string[0]);
				if (ServicesAPI._billingHandler != null)
				{
					ServicesAPI._billingHandler.Stop();
				}
				ServicesAPI._billingHandler = null;
				if (ServicesAPI._billingService != null)
				{
					ServicesAPI._billingService.Stop();
				}
				ServicesAPI._billingService = null;
			}
			catch (Exception ex)
			{
				Common.WriteLine("StopBillingService: {0}", new string[]
				{
					ex.Message
				});
			}
		}
	}
}
