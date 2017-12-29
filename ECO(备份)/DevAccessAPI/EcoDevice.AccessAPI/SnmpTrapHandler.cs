using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Text;
namespace EcoDevice.AccessAPI
{
	public class SnmpTrapHandler
	{
		private System.Collections.Generic.List<UsmConfig> usmConfigs = new System.Collections.Generic.List<UsmConfig>();
		public System.Collections.Generic.List<UsmConfig> UsmConfigList
		{
			get
			{
				return this.usmConfigs;
			}
		}
		public TrapMessage Handler(AbstractSocketData socketData)
		{
			if (socketData == null)
			{
				return null;
			}
			TrapMessage result;
			try
			{
				int dataLenth = socketData.DataLenth;
				byte[] dataBytes = socketData.DataBytes;
				int protocolVersion = SnmpPacket.GetProtocolVersion(dataBytes, dataLenth);
				TrapMessage trapMessage;
				SnmpPacket snmpPacket;
				if (protocolVersion == 0)
				{
					trapMessage = new TrapV1Message();
					snmpPacket = new SnmpV1TrapPacket();
					((SnmpV1TrapPacket)snmpPacket).decode(dataBytes, dataLenth);
				}
				else
				{
					if (protocolVersion == 1)
					{
						trapMessage = new TrapV2Message();
						snmpPacket = new SnmpV2Packet();
						((SnmpV2Packet)snmpPacket).decode(dataBytes, dataLenth);
						if (snmpPacket.Pdu.Type != PduType.V2Trap)
						{
							throw new SnmpException("Invalid SNMP version 2 packet type received.");
						}
					}
					else
					{
						trapMessage = new TrapV3Message();
						snmpPacket = new SnmpV3Packet();
						UserSecurityModel uSM = ((SnmpV3Packet)snmpPacket).GetUSM(dataBytes, dataLenth);
						if (uSM.EngineId.Length <= 0)
						{
							throw new SnmpException("Invalid packet. Authoritative engine id is not set.");
						}
						if (uSM.SecurityName.Length <= 0)
						{
							throw new SnmpException("Invalid packet. Security name is not set.");
						}
						if (this.usmConfigs.Count > 0)
						{
							UsmConfig usmConfig = this.FindPeer(uSM.EngineId.ToString(), uSM.SecurityName.ToString());
							if (usmConfig == null)
							{
								throw new SnmpException("SNMP packet from unknown peer.");
							}
							((SnmpV3Packet)snmpPacket).USM.Authentication = (AuthenticationDigests)usmConfig.Authentication;
							((SnmpV3Packet)snmpPacket).USM.Privacy = (PrivacyProtocols)usmConfig.Privacy;
							if (usmConfig.Privacy != Privacy.None)
							{
								((SnmpV3Packet)snmpPacket).USM.PrivacySecret.Set(usmConfig.PrivacySecret);
							}
							if (usmConfig.Authentication != Authentication.None)
							{
								((SnmpV3Packet)snmpPacket).USM.AuthenticationSecret.Set(usmConfig.AuthenticationSecret);
							}
						}
						((SnmpV3Packet)snmpPacket).decode(dataBytes, dataLenth);
						if (snmpPacket.Pdu.Type != PduType.V2Trap)
						{
							throw new SnmpException("Invalid SNMP version 3 packet type received.");
						}
					}
				}
				trapMessage.AgentIpAddress = socketData.Target;
				trapMessage.Port = socketData.Port;
				SnmpTrapHandler.configTrap(trapMessage, protocolVersion, snmpPacket);
				this.configVb(trapMessage, protocolVersion, snmpPacket);
				result = trapMessage;
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine(ex.Message);
				result = null;
			}
			return result;
		}
		private void configVb(TrapMessage trap, int ver, SnmpPacket packet)
		{
			VbCollection vbList;
			if (ver == 0)
			{
				vbList = ((SnmpV1TrapPacket)packet).TrapPdu.VbList;
			}
			else
			{
				vbList = packet.Pdu.VbList;
			}
			new System.Text.StringBuilder();
			foreach (Vb current in vbList)
			{
				if (!trap.VarBindings.ContainsKey(current.Oid.ToString()))
				{
					trap.VarBindings.Add(current.Oid.ToString(), current.Value.ToString());
				}
			}
		}
		private UsmConfig FindPeer(string engineId, string securityName)
		{
			if (engineId != null)
			{
				foreach (UsmConfig current in this.usmConfigs)
				{
					if (current.IsMatch(engineId, securityName))
					{
						UsmConfig result = current;
						return result;
					}
				}
			}
			foreach (UsmConfig current2 in this.usmConfigs)
			{
				if (current2.IsMatch(securityName))
				{
					UsmConfig result = current2;
					return result;
				}
			}
			return null;
		}
		private static void configTrap(TrapMessage trap, int ver, SnmpPacket packet)
		{
			trap.SnmpVersion = (SnmpVersionType)ver;
			trap.ReceiveTime = System.DateTime.Now;
			if (trap.SnmpVersion == SnmpVersionType.Ver1)
			{
				((TrapV1Message)trap).SnmpVersion = (SnmpVersionType)ver;
				((TrapV1Message)trap).Enterprise = ((SnmpV1TrapPacket)packet).TrapPdu.Enterprise.ToString();
				((TrapV1Message)trap).GenericTrap = ((SnmpV1TrapPacket)packet).TrapPdu.Generic.ToString();
				((TrapV1Message)trap).Specific = ((SnmpV1TrapPacket)packet).TrapPdu.Specific.ToString();
				((TrapV1Message)trap).TimeStamp = ((SnmpV1TrapPacket)packet).TrapPdu.TimeStamp.ToString();
				((TrapV1Message)trap).Community = ((SnmpV1TrapPacket)packet).Community.ToString();
				return;
			}
			TrapDesc trapDesc = new TrapDesc();
			trapDesc.TrapObjectID = packet.Pdu.TrapObjectID.ToString();
			trapDesc.TrapSysUpTime = packet.Pdu.TrapSysUpTime.ToString();
			trapDesc.ErrorIndex = packet.Pdu.ErrorIndex.ToString();
			trapDesc.ErrorStatus = packet.Pdu.ErrorStatus.ToString();
			trapDesc.ErrorStatusString = SnmpError.ErrorMessage(packet.Pdu.ErrorStatus);
			if (trap.SnmpVersion == SnmpVersionType.Ver2)
			{
				((TrapV2Message)trap).Community = ((SnmpV2Packet)packet).Community.ToString();
				((TrapV2Message)trap).TrapDesc = trapDesc;
				return;
			}
			((TrapV3Message)trap).EngineId = ((SnmpV3Packet)packet).USM.EngineId.ToString();
			((TrapV3Message)trap).SecurityName = ((SnmpV3Packet)packet).USM.SecurityName.ToString();
			((TrapV3Message)trap).TrapDesc = trapDesc;
		}
	}
}
