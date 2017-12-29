using Dispatcher;
using Packing;
using SessionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
namespace ecoProtocols
{
	public class EcoContext : ConnectContext
	{
		public static int _nextUID = 1;
		public override List<byte[]> PacketReceived(byte[] packet)
		{
			List<byte[]> list = new List<byte[]>();
			uint packetType = ecoServerProtocol.getPacketType(this._header);
			if ((ulong)ecoServerProtocol.getPacketType(this._header) == 35328uL)
			{
				int num = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 0));
				ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 2));
				SessionAPI.Update((long)num);
				this.AppendResponse(list, 35456, 0, null);
			}
			else
			{
				if ((ulong)ecoServerProtocol.getPacketType(this._header) == 1uL)
				{
					this.AppendResponse(list, 129, 0, null);
					if (this._owner != null)
					{
						this._owner.ReportMessage(this._owner, this, this._header, packet);
					}
				}
				else
				{
					if ((ulong)ecoServerProtocol.getPacketType(this._header) == 35343uL)
					{
						if (this._owner != null)
						{
							this._owner.ReportMessage(this._owner, this, this._header, packet);
						}
					}
					else
					{
						if ((ulong)ecoServerProtocol.getPacketType(this._header) == 35329uL)
						{
							if (this._owner != null)
							{
								this._owner.ReportMessage(this._owner, this, this._header, packet);
							}
						}
						else
						{
							if ((ulong)packetType == 35330uL)
							{
								Common.WriteLine("    Logout from ecoClient", new string[0]);
								int num2 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, 0));
								int num3 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, 2));
								SessionAPI.Logout((long)num2, (long)num3);
								this.AppendResponse(list, 35458, 0, null);
							}
							else
							{
								if ((ulong)ecoServerProtocol.getPacketType(this._header) == 35471uL)
								{
									if (this._owner != null)
									{
										this._owner.ReportMessage(this._owner, this, this._header, packet);
									}
								}
								else
								{
									if ((ulong)packetType == 35331uL)
									{
										if (this._owner != null)
										{
											this._owner.ReportMessage(this._owner, this, this._header, packet);
										}
									}
									else
									{
										if ((ulong)packetType == 35332uL)
										{
											if (this._owner != null)
											{
												this._owner.ReportMessage(this._owner, this, this._header, packet);
											}
										}
										else
										{
											if ((ulong)packetType != 384uL)
											{
												if ((ulong)packetType == 385uL)
												{
													if (this._owner != null)
													{
														this._owner.ReportMessage(this._owner, this, this._header, packet);
													}
												}
												else
												{
													if ((ulong)packetType == 259uL)
													{
														if (this._owner != null)
														{
															this._owner.ReportMessage(this._owner, this, this._header, packet);
														}
													}
													else
													{
														if ((ulong)packetType == 35456uL)
														{
															int num4 = (int)this._receiveBuffer[0];
															if (num4 > 0 && this._owner != null)
															{
																this._owner.ReportMessage(this._owner, this, this._header, packet);
															}
														}
														else
														{
															if ((ulong)packetType == 35457uL)
															{
																if (this._owner != null)
																{
																	this._owner.ReportMessage(this._owner, this, this._header, packet);
																}
															}
															else
															{
																if ((ulong)packetType == 35459uL)
																{
																	Common.WriteLine("    Request ack from ecoServer", new string[0]);
																	int num5 = (int)this._receiveBuffer[0];
																	int num6 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 1));
																	if (num5 != 0 && (num6 == 4 || num6 == 32 || num6 == 2) && this._owner != null)
																	{
																		this._owner.ReportMessage(this._owner, this, this._header, packet);
																	}
																}
																else
																{
																	if ((ulong)packetType == 35458uL)
																	{
																		Common.WriteLine("    Logout ack from ecoServer", new string[0]);
																	}
																	else
																	{
																		if ((ulong)packetType == 35460uL)
																		{
																			Common.WriteLine("    Message ack from ecoServer", new string[0]);
																		}
																		else
																		{
																			if ((ulong)packetType == 35333uL)
																			{
																				Common.WriteLine("    Error Message from ecoServer", new string[0]);
																				this.AppendResponse(list, 35461, 0, null);
																				int num7 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 0));
																				ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 2));
																				if (num7 == 1 && this._owner != null)
																				{
																					this._owner.ReportMessage(this._owner, this, this._header, packet);
																				}
																			}
																			else
																			{
																				if ((ulong)packetType == 386uL)
																				{
																					if (this._owner != null)
																					{
																						this._owner.ReportMessage(this._owner, this, this._header, packet);
																					}
																				}
																				else
																				{
																					if ((ulong)packetType == 32uL)
																					{
																						int startIndex = 0;
																						int num8 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, startIndex));
																						base.setKicked(true);
																						Common.WriteLine("    Be kicked: uid={0}", new string[]
																						{
																							num8.ToString()
																						});
																						ClientAPI.OnClosed(this, -2);
																					}
																					else
																					{
																						if ((ulong)packetType == 48uL)
																						{
																							int startIndex2 = 0;
																							int num9 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, startIndex2));
																							base.setServiceWillDown(true);
																							Common.WriteLine("    Service will be down: uid={0}", new string[]
																							{
																								num9.ToString()
																							});
																							ClientAPI.OnClosed(this, -2);
																						}
																						else
																						{
																							if ((ulong)packetType == 260uL)
																							{
																								if (this._owner != null)
																								{
																									this._owner.ReportMessage(this._owner, this, this._header, packet);
																								}
																							}
																							else
																							{
																								if ((ulong)packetType == 258uL)
																								{
																									this.AppendResponse(list, 386, 0, null);
																									if (this._vid >= 0L)
																									{
																										int num10 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 0));
																										ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 2));
																										ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, 4));
																										if (num10 > 0)
																										{
																											this.ReceiveDataset(ref this._sqlContext, packet);
																										}
																										else
																										{
																											this.ReceiveDataset(ref this._dsContext, packet);
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
			return list;
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
		private void ReceiveDataset(ref ConnectContext.SerializeContext context, byte[] packet)
		{
			try
			{
				int num = 0;
				this._headBytes = BitConverter.GetBytes(ecoServerProtocol.swap64(this._header));
				int uid = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, num));
				num += 2;
				int vid = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, num));
				num += 2;
				int type = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(this._receiveBuffer, num));
				num += 2;
				int num2 = (int)packet[num];
				num++;
				int num4;
				if ((num2 & 1) != 0)
				{
					DateTime arg_77_0 = DateTime.Now;
					Common.WriteLine("Dataset from server: type={0}, uid={1}, vid={2}", new string[]
					{
						type.ToString("X8"),
						uid.ToString(),
						vid.ToString()
					});
					ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, num));
					num += 2;
					uint num3 = ecoServerProtocol.swap32((uint)BitConverter.ToInt32(packet, num));
					num += 4;
					num4 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, num));
					num += 2;
					uint num5 = ecoServerProtocol.swap32((uint)BitConverter.ToInt32(packet, num));
					num += 4;
					context._compress = (int)(num5 & 15u);
					num4 -= 4;
					context._hashDataSet = new byte[num4];
					Array.Copy(packet, num, context._hashDataSet, 0, num4);
					num4 = 0;
					context._dataSize = 0;
					context._dataBuffer = new byte[num3];
					context._tPacketStart = (long)Environment.TickCount;
					Common.WriteLine("    DataSet begin, uid={0}, vid={1}, type={2}, total={3}, Elapsed=0", new string[]
					{
						uid.ToString(),
						vid.ToString(),
						type.ToString("X8"),
						num3.ToString()
					});
				}
				else
				{
					ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, num));
					num += 2;
					num4 = (int)ecoServerProtocol.swap16((ushort)BitConverter.ToInt16(packet, num));
					num += 2;
				}
				if (context._dataBuffer != null && packet != null && num4 > 0)
				{
					Array.Copy(packet, num, context._dataBuffer, context._dataSize, num4);
					context._dataSize += num4;
				}
				if ((num2 & 2) != 0)
				{
					if (context._dataBuffer != null)
					{
						long num6 = Common.ElapsedTime(context._tPacketStart);
						bool flag = false;
						MD5 mD = MD5.Create();
						byte[] first = mD.ComputeHash(context._dataBuffer, 0, context._dataBuffer.Length);
						if (context._hashDataSet != null && !first.SequenceEqual(context._hashDataSet))
						{
							flag = true;
						}
						DateTime arg_255_0 = DateTime.Now;
						if (flag)
						{
							Common.WriteLine("    DataSet crashed: uid={0}, vid={1}, type={2}, size={3}, elapsed: {4}", new string[]
							{
								uid.ToString(),
								vid.ToString(),
								type.ToString("X8"),
								context._dataBuffer.Length.ToString(),
								num6.ToString()
							});
						}
						else
						{
							Common.WriteLine("    DataSet end: uid={0}, vid={1}, type={2}, size={3}, elapsed: {4}", new string[]
							{
								uid.ToString(),
								vid.ToString(),
								type.ToString("X8"),
								context._dataBuffer.Length.ToString(),
								num6.ToString()
							});
						}
						DispatchAttribute dispatchAttribute = new DispatchAttribute();
						dispatchAttribute.uid = uid;
						dispatchAttribute.vid = vid;
						dispatchAttribute.type = type;
						dispatchAttribute.algorithm = context._compress;
						dispatchAttribute.data = context._dataBuffer;
						dispatchAttribute.owner = this._owner;
						Serialization serialization = new Serialization();
						serialization.StartDecompressThread(dispatchAttribute);
					}
					context._dataSize = 0;
					context._dataBuffer = null;
				}
			}
			catch (Exception ex)
			{
				Common.WriteLine(ex.Message, new string[0]);
			}
		}
	}
}
