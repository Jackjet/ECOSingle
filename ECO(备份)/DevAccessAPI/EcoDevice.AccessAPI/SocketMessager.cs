using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace EcoDevice.AccessAPI
{
	internal class SocketMessager
	{
		private const string ValueSpiltor = "{&}";
		private const string PropertySplitor = "{|}";
		private string target;
		private int port;
		private int dataLenth;
		private byte[] dataBytes;
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
		public int DataLenth
		{
			get
			{
				return this.dataLenth;
			}
			set
			{
				this.dataLenth = value;
			}
		}
		public byte[] DataBytes
		{
			get
			{
				return this.dataBytes;
			}
			set
			{
				this.dataBytes = value;
			}
		}
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}
		public SocketData getSocketData(int dataLenth, byte[] dataBytes, string target, int port)
		{
			return new SocketData
			{
				DataBytes = dataBytes,
				DataLenth = dataLenth,
				Target = target,
				Port = port
			};
		}
		public AbstractSocketData decode()
		{
			string @string = System.Text.Encoding.UTF8.GetString(this.dataBytes, 0, this.dataLenth);
			string[] separator = new string[]
			{
				"{|}"
			};
			string[] array = @string.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
			if (array == null || array.Length < 1)
			{
				return this.getSocketData(this.dataLenth, this.dataBytes, this.target, this.port);
			}
			string[] separator2 = new string[]
			{
				"{&}"
			};
			System.Collections.Generic.IDictionary<string, object> dictionary = new System.Collections.Generic.Dictionary<string, object>();
			string text = string.Empty;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string[] array3 = text2.Split(separator2, System.StringSplitOptions.RemoveEmptyEntries);
				if (array3 != null && array3.Length == 2)
				{
					if ("type".Equals(array3[0]))
					{
						text = array3[1];
					}
					else
					{
						dictionary.Add(array3[0], array3[1]);
					}
				}
			}
			if (text == null || text.Equals(""))
			{
				return this.getSocketData(this.dataLenth, this.dataBytes, this.target, this.port);
			}
			System.Type type = System.Type.GetType(text);
			if (type == null)
			{
				throw new System.Exception("getAssamble() is implemented incorrectly.");
			}
			AbstractSocketData abstractSocketData = null;
			try
			{
				abstractSocketData = (AbstractSocketData)ReflactUtil.newInstance(type, null);
			}
			catch (System.Exception)
			{
				throw new System.Exception("There is no default constructor in this type " + type);
			}
			System.Reflection.FieldInfo[] fieldInfos = ReflactUtil.getFieldInfos(type);
			System.Reflection.FieldInfo[] array4 = fieldInfos;
			for (int j = 0; j < array4.Length; j++)
			{
				System.Reflection.FieldInfo fieldInfo = array4[j];
				string name = fieldInfo.Name;
				if (dictionary.ContainsKey(name))
				{
					string value = (string)dictionary[name];
					if (fieldInfo.FieldType.Equals(typeof(byte)))
					{
						fieldInfo.SetValue(abstractSocketData, System.Convert.ToByte(value));
					}
					else
					{
						if (fieldInfo.FieldType.Equals(typeof(char)))
						{
							fieldInfo.SetValue(abstractSocketData, System.Convert.ToChar(value));
						}
						else
						{
							if (fieldInfo.FieldType.Equals(typeof(short)))
							{
								fieldInfo.SetValue(abstractSocketData, System.Convert.ToInt16(value));
							}
							else
							{
								if (fieldInfo.FieldType.Equals(typeof(int)))
								{
									fieldInfo.SetValue(abstractSocketData, System.Convert.ToInt32(value));
								}
								else
								{
									if (fieldInfo.FieldType.Equals(typeof(long)))
									{
										fieldInfo.SetValue(abstractSocketData, System.Convert.ToInt64(value));
									}
									else
									{
										if (fieldInfo.FieldType.Equals(typeof(double)))
										{
											fieldInfo.SetValue(abstractSocketData, System.Convert.ToDouble(value));
										}
										else
										{
											if (fieldInfo.FieldType.Equals(typeof(float)))
											{
												fieldInfo.SetValue(abstractSocketData, System.Convert.ToSingle(value));
											}
											else
											{
												if (fieldInfo.FieldType.Equals(typeof(string)))
												{
													fieldInfo.SetValue(abstractSocketData, value);
												}
												else
												{
													if (fieldInfo.FieldType.Equals(typeof(bool)))
													{
														fieldInfo.SetValue(abstractSocketData, System.Convert.ToBoolean(value));
													}
													else
													{
														if (fieldInfo.FieldType.Equals(typeof(System.DateTime)))
														{
															fieldInfo.SetValue(abstractSocketData, System.Convert.ToDateTime(value));
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
			return abstractSocketData;
		}
		public void encode(AbstractSocketData sockeData)
		{
			if (sockeData == null)
			{
				return;
			}
			System.Type type = sockeData.GetType();
			if (!AttributeHelper.needEncoded(type))
			{
				return;
			}
			string assamble = sockeData.getAssamble();
			if (string.IsNullOrEmpty(assamble))
			{
				throw new System.Exception(type.ToString() + ".getAssamble() is not implemented.");
			}
			sockeData.Type = type.ToString() + "," + assamble;
			System.Reflection.FieldInfo[] fieldInfos = ReflactUtil.getFieldInfos(type);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Reflection.FieldInfo[] array = fieldInfos;
			for (int i = 0; i < array.Length; i++)
			{
				System.Reflection.FieldInfo fieldInfo = array[i];
				if (AttributeHelper.needEncoded(fieldInfo))
				{
					string name = fieldInfo.Name;
					object value = fieldInfo.GetValue(sockeData);
					stringBuilder.Append(name + "{&}" + value).Append("{|}");
				}
			}
			this.dataBytes = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
			if (this.dataBytes == null)
			{
				this.dataBytes = new byte[0];
			}
			this.dataLenth = this.dataBytes.Length;
		}
	}
}
