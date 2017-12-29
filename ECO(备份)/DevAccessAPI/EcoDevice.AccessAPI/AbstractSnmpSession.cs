using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Net;
namespace EcoDevice.AccessAPI
{
	public abstract class AbstractSnmpSession : SnmpSession
	{
		private SnmpConfig config;
		public SnmpConfig SnmpConfig
		{
			get
			{
				return this.config;
			}
		}
		public AbstractSnmpSession(SnmpConfig config)
		{
			if (config == null)
			{
				throw new System.ArgumentNullException("SnmpConfig is null");
			}
			this.config = config;
		}
		private System.Collections.Generic.Dictionary<string, string> SendPacket(SnmpOperationType pduType, VarBinding variables)
		{
			if (variables == null)
			{
				throw new System.ArgumentNullException("The variables for the " + pduType + " operation is null.");
			}
			return this.SendPacket(pduType, new System.Collections.Generic.List<VarBinding>
			{
				variables
			});
		}
		private void configBulkPdu(Pdu pdu, MaxRepetition maxRepetion)
		{
			pdu.NonRepeaters = 0;
			pdu.MaxRepetitions = (int)maxRepetion;
		}
		private System.Collections.Generic.Dictionary<string, string> SendPacket(SnmpOperationType pduType, System.Collections.Generic.List<VarBinding> variables)
		{
			if (variables == null || variables.Count < 1)
			{
				throw new System.ArgumentNullException("The variables for the " + pduType + " operation is null or empty.");
			}
			UdpTarget udpTarget = null;
			IAgentParameters agentParameters = null;
			Pdu pdu = null;
			System.Collections.Generic.Dictionary<string, string> result3;
			try
			{
				udpTarget = new UdpTarget(System.Net.IPAddress.Parse(this.config.AgentIp), this.config.Port, this.config.Timeout, this.config.Retry);
				if (this.config.Version == SnmpVersionType.Ver3)
				{
					agentParameters = new SecureAgentParameters();
					SecureAgentParameters secureAgentParameters = agentParameters as SecureAgentParameters;
					if (!udpTarget.Discovery(secureAgentParameters))
					{
						throw new SnmpException("Discovery failed: The device with ip(" + this.config.AgentIp + ") is unreachable.");
					}
					pdu = new ScopedPdu();
					SnmpV3Config snmpV3Config = this.config as SnmpV3Config;
					secureAgentParameters.SecurityName.Set(snmpV3Config.UserName);
					secureAgentParameters.Authentication = (AuthenticationDigests)snmpV3Config.Authentication;
					secureAgentParameters.AuthenticationSecret.Set(snmpV3Config.AuthSecret);
					secureAgentParameters.Privacy = (PrivacyProtocols)snmpV3Config.Privacy;
					secureAgentParameters.PrivacySecret.Set(snmpV3Config.PrivacySecret);
					secureAgentParameters.Reportable = true;
				}
				else
				{
					if (this.config.Version == SnmpVersionType.Ver1)
					{
						OctetString community = new OctetString(((SnmpV1Config)this.config).Community);
						agentParameters = new AgentParameters(SnmpVersion.Ver1, community);
					}
					else
					{
						OctetString community = new OctetString(((SnmpV2Config)this.config).Community);
						agentParameters = new AgentParameters(SnmpVersion.Ver2, community);
					}
					pdu = new Pdu();
				}
				DictionaryUtil dictionaryUtil = new DictionaryUtil();
				foreach (VarBinding current in variables)
				{
					try
					{
						if (current is LeafVarBinding)
						{
							if (pduType.Equals(SnmpOperationType.GetTable) || pduType.Equals(SnmpOperationType.Walk))
							{
								pdu.Type = PduType.Get;
							}
							else
							{
								pdu.Type = (PduType)pduType;
								if (pduType.Equals(SnmpOperationType.GetBulk))
								{
									this.configBulkPdu(pdu, current.MaxRepetition);
								}
							}
							System.Collections.Generic.Dictionary<string, string> result = this.ReceiveResponseWithLeafVB((LeafVarBinding)current, pdu, udpTarget, agentParameters);
							dictionaryUtil.Add(result);
						}
						else
						{
							if (agentParameters.Version == SnmpVersion.Ver1)
							{
								pdu.Type = PduType.GetNext;
							}
							else
							{
								pdu.Type = PduType.GetBulk;
								this.configBulkPdu(pdu, current.MaxRepetition);
							}
							System.Collections.Generic.Dictionary<string, string> result2 = this.ReceiveResponseWithTableVB((TableVarBinding)current, pdu, udpTarget, agentParameters);
							dictionaryUtil.Add(result2);
						}
					}
					catch (System.Exception ex)
					{
						if (!ex.Message.Contains("Invalid ASN.1 type encountered 0x00. Unable to continue decoding."))
						{
							throw new SnmpException(ex.Message);
						}
					}
				}
				result3 = dictionaryUtil.Result;
			}
			catch (System.Exception ex2)
			{
				throw new SnmpException(ex2.Message);
			}
			finally
			{
				if (udpTarget != null)
				{
					udpTarget.Close();
				}
			}
			return result3;
		}
		private System.Collections.Generic.Dictionary<string, string> ReceiveResponseWithLeafVB(LeafVarBinding leafVb, Pdu pdu, UdpTarget target, IAgentParameters param)
		{
			if (leafVb.VarBindings.Count < 1)
			{
				throw new System.ArgumentNullException("The variables for the " + pdu.Type + " opertion is emtpy.");
			}
			pdu.VbList.Clear();
			this.configPduVb(leafVb, pdu);
			SnmpPacket snmpPacket = target.Request(pdu, param);
			this.validateResponse(snmpPacket, pdu);
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			foreach (Vb current in snmpPacket.Pdu.VbList)
			{
				if (current.Value.Type != SnmpConstants.SMI_NOSUCHINSTANCE && current.Value.Type != SnmpConstants.SMI_NOSUCHOBJECT && current.Value.Type != SnmpConstants.SMI_ENDOFMIBVIEW)
				{
					string key = current.Oid.ToString();
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, current.Value.ToString());
					}
				}
			}
			return dictionary;
		}
		private void configPduVb(LeafVarBinding leafVb, Pdu pdu)
		{
			System.Collections.Generic.IEnumerator<string> enumerator = leafVb.VarBindings.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (pdu.Type == PduType.Set)
				{
					object obj = leafVb.VarBindings[current];
					AsnType value = null;
					if (obj is string)
					{
						value = new OctetString(System.Convert.ToString(obj));
					}
					else
					{
						if (obj is int)
						{
							value = new Integer32(System.Convert.ToInt32(obj));
						}
						else
						{
							if (obj is System.Net.IPAddress)
							{
								value = new IpAddress(obj as System.Net.IPAddress);
							}
						}
					}
					pdu.VbList.Add(new Oid(current), value);
				}
				else
				{
					pdu.VbList.Add(current);
				}
			}
		}
		private System.Collections.Generic.Dictionary<string, string> ReceiveResponseWithTableVB(TableVarBinding tableVb, Pdu pdu, UdpTarget target, IAgentParameters param)
		{
			if (string.IsNullOrEmpty(tableVb.TableEntryOid))
			{
				throw new System.ArgumentNullException("The TableEntryOid can not be null or empty.");
			}
			Oid oid = new Oid(tableVb.TableEntryOid);
			Oid oid2 = null;
			if (string.IsNullOrEmpty(tableVb.ColumnOid))
			{
				oid2 = (Oid)oid.Clone();
			}
			else
			{
				oid2 = new Oid(tableVb.ColumnOid);
			}
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			while (oid2 != null)
			{
				pdu.VbList.Clear();
				pdu.VbList.Add(oid2);
				SnmpPacket snmpPacket = target.Request(pdu, param);
				this.validateResponse(snmpPacket, pdu);
				foreach (Vb current in snmpPacket.Pdu.VbList)
				{
					if (!oid.IsRootOf(current.Oid))
					{
						oid2 = null;
						break;
					}
					if (current.Value.Type == SnmpConstants.SMI_ENDOFMIBVIEW)
					{
						oid2 = null;
						break;
					}
					string key = current.Oid.ToString();
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, current.Value.ToString());
						oid2 = current.Oid;
					}
				}
			}
			return dictionary;
		}
		private void validateResponse(SnmpPacket response, Pdu pdu)
		{
			if (response == null)
			{
				throw new SnmpException("Request failed: There is no response to this " + pdu.Type + " request.");
			}
			if (response.Pdu.ErrorStatus != 0 && response.Pdu.ErrorStatus != 2)
			{
				throw new SnmpException("Receive failed with error status <" + response.Pdu.ErrorStatus + ">.");
			}
			if (response.Pdu.Type == PduType.Report)
			{
				throw new SnmpException("Report response.");
			}
		}
		public System.Collections.Generic.Dictionary<string, string> Get(LeafVarBinding varBinding)
		{
			return this.SendPacket(SnmpOperationType.Get, varBinding);
		}
		public System.Collections.Generic.Dictionary<string, string> Get(System.Collections.Generic.List<LeafVarBinding> varBindings)
		{
			System.Collections.Generic.List<VarBinding> list = new System.Collections.Generic.List<VarBinding>();
			foreach (LeafVarBinding current in varBindings)
			{
				list.Add(current);
			}
			return this.SendPacket(SnmpOperationType.Get, list);
		}
		public System.Collections.Generic.Dictionary<string, string> GetNext(LeafVarBinding varBinding)
		{
			return this.SendPacket(SnmpOperationType.GetNext, varBinding);
		}
		public System.Collections.Generic.Dictionary<string, string> Set(LeafVarBinding varBinding)
		{
			return this.SendPacket(SnmpOperationType.Set, varBinding);
		}
		public virtual System.Collections.Generic.Dictionary<string, string> GetBulk(string startVariable)
		{
			if (string.IsNullOrEmpty(startVariable))
			{
				throw new System.ArgumentNullException("The startVariable for the " + SnmpOperationType.GetBulk + " operation is null or empty.");
			}
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(startVariable);
			return this.SendPacket(SnmpOperationType.GetBulk, leafVarBinding);
		}
		public System.Collections.Generic.Dictionary<string, string> GetTable(TableVarBinding tableVariable)
		{
			return this.SendPacket(SnmpOperationType.GetTable, tableVariable);
		}
		public System.Collections.Generic.Dictionary<string, string> Walk(System.Collections.Generic.List<VarBinding> variables)
		{
			return this.SendPacket(SnmpOperationType.Walk, variables);
		}
		public System.Collections.Generic.Dictionary<string, string> GetBulk(string startVariable, MaxRepetition maxRepetition)
		{
			if (string.IsNullOrEmpty(startVariable))
			{
				throw new System.ArgumentNullException("The startVariable for the " + SnmpOperationType.GetBulk + " operation is null or empty.");
			}
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(startVariable);
			leafVarBinding.MaxRepetition = maxRepetition;
			return this.SendPacket(SnmpOperationType.GetBulk, leafVarBinding);
		}
	}
}
