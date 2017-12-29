using System;
namespace CommonAPI.Tools
{
	public class SNDecoder
	{
		private static byte[] m_transTable = new byte[]
		{
			52,
			66,
			78,
			80,
			71,
			56,
			74,
			75,
			53,
			65,
			72,
			54,
			77,
			86,
			87,
			85,
			76,
			70,
			67,
			88,
			55,
			57,
			69,
			68,
			48,
			49,
			51,
			81,
			50,
			82,
			84,
			89
		};
		private static byte[,] KeytableMaster_1 = new byte[,]
		{

			{
				75,
				84,
				52,
				54,
				78
			},

			{
				57,
				69,
				78,
				82,
				52
			},

			{
				67,
				53,
				74,
				48,
				85
			},

			{
				71,
				51,
				85,
				66,
				74
			},

			{
				88,
				65,
				54,
				52,
				82
			},

			{
				87,
				49,
				82,
				78,
				54
			},

			{
				72,
				70,
				48,
				74,
				66
			},

			{
				50,
				56,
				66,
				85,
				48
			},

			{
				65,
				88,
				68,
				80,
				77
			},

			{
				49,
				87,
				77,
				76,
				68
			},

			{
				70,
				72,
				55,
				89,
				86
			},

			{
				56,
				50,
				86,
				81,
				55
			},

			{
				84,
				75,
				80,
				68,
				76
			},

			{
				69,
				57,
				76,
				77,
				80
			},

			{
				53,
				67,
				89,
				55,
				81
			},

			{
				51,
				71,
				81,
				86,
				89
			},

			{
				82,
				77,
				87,
				57,
				88
			},

			{
				54,
				68,
				88,
				75,
				87
			},

			{
				66,
				86,
				50,
				71,
				72
			},

			{
				48,
				55,
				72,
				67,
				50
			},

			{
				78,
				76,
				57,
				87,
				75
			},

			{
				52,
				80,
				75,
				88,
				57
			},

			{
				85,
				81,
				71,
				50,
				67
			},

			{
				74,
				89,
				67,
				72,
				71
			},

			{
				76,
				78,
				69,
				49,
				84
			},

			{
				80,
				52,
				84,
				65,
				69
			},

			{
				81,
				85,
				51,
				56,
				53
			},

			{
				89,
				74,
				53,
				70,
				51
			},

			{
				77,
				82,
				49,
				69,
				65
			},

			{
				68,
				54,
				65,
				84,
				49
			},

			{
				86,
				66,
				56,
				51,
				70
			},

			{
				55,
				48,
				70,
				53,
				56
			}
		};
		private static byte[,] KeytableMaster_2 = new byte[,]
		{

			{
				75,
				81,
				77,
				89,
				78
			},

			{
				57,
				89,
				68,
				81,
				52
			},

			{
				67,
				76,
				86,
				80,
				85
			},

			{
				71,
				80,
				55,
				76,
				74
			},

			{
				88,
				86,
				76,
				55,
				82
			},

			{
				87,
				55,
				80,
				86,
				54
			},

			{
				72,
				77,
				81,
				68,
				66
			},

			{
				50,
				68,
				89,
				77,
				48
			},

			{
				65,
				66,
				78,
				48,
				77
			},

			{
				49,
				48,
				52,
				66,
				68
			},

			{
				70,
				82,
				85,
				54,
				86
			},

			{
				56,
				54,
				74,
				82,
				55
			},

			{
				84,
				85,
				82,
				74,
				76
			},

			{
				69,
				74,
				54,
				85,
				80
			},

			{
				53,
				78,
				66,
				52,
				81
			},

			{
				51,
				52,
				48,
				78,
				89
			},

			{
				82,
				70,
				84,
				56,
				88
			},

			{
				54,
				56,
				69,
				70,
				87
			},

			{
				66,
				65,
				53,
				49,
				72
			},

			{
				48,
				49,
				51,
				65,
				50
			},

			{
				78,
				53,
				65,
				51,
				75
			},

			{
				52,
				51,
				49,
				53,
				57
			},

			{
				85,
				84,
				70,
				69,
				67
			},

			{
				74,
				69,
				56,
				84,
				71
			},

			{
				76,
				67,
				88,
				71,
				84
			},

			{
				80,
				71,
				87,
				67,
				69
			},

			{
				81,
				75,
				72,
				57,
				53
			},

			{
				89,
				57,
				50,
				75,
				51
			},

			{
				77,
				72,
				75,
				50,
				65
			},

			{
				68,
				50,
				57,
				72,
				49
			},

			{
				86,
				88,
				67,
				87,
				70
			},

			{
				55,
				87,
				71,
				88,
				56
			}
		};
		private static byte[,] KeytableClient_1 = new byte[,]
		{

			{
				75,
				65,
				69,
				85,
				87
			},

			{
				57,
				49,
				84,
				74,
				88
			},

			{
				67,
				70,
				51,
				78,
				50
			},

			{
				71,
				56,
				53,
				52,
				72
			},

			{
				88,
				84,
				49,
				66,
				57
			},

			{
				87,
				69,
				65,
				48,
				75
			},

			{
				72,
				53,
				56,
				82,
				71
			},

			{
				50,
				51,
				70,
				54,
				67
			},

			{
				65,
				75,
				87,
				86,
				69
			},

			{
				49,
				57,
				88,
				55,
				84
			},

			{
				70,
				67,
				50,
				77,
				51
			},

			{
				56,
				71,
				72,
				68,
				53
			},

			{
				84,
				88,
				57,
				81,
				49
			},

			{
				69,
				87,
				75,
				89,
				65
			},

			{
				53,
				72,
				71,
				76,
				56
			},

			{
				51,
				50,
				67,
				80,
				70
			},

			{
				82,
				76,
				68,
				72,
				52
			},

			{
				54,
				80,
				77,
				50,
				78
			},

			{
				66,
				81,
				55,
				88,
				74
			},

			{
				48,
				89,
				86,
				87,
				85
			},

			{
				78,
				77,
				80,
				67,
				54
			},

			{
				52,
				68,
				76,
				71,
				82
			},

			{
				85,
				86,
				89,
				75,
				48
			},

			{
				74,
				55,
				81,
				57,
				66
			},

			{
				76,
				82,
				52,
				53,
				68
			},

			{
				80,
				54,
				78,
				51,
				77
			},

			{
				81,
				66,
				74,
				84,
				55
			},

			{
				89,
				48,
				85,
				69,
				86
			},

			{
				77,
				78,
				54,
				70,
				80
			},

			{
				68,
				52,
				82,
				56,
				76
			},

			{
				86,
				85,
				48,
				65,
				89
			},

			{
				55,
				74,
				66,
				49,
				81
			}
		};
		private static byte[,] KeytableClient_2 = new byte[,]
		{

			{
				75,
				65,
				80,
				56,
				71
			},

			{
				57,
				49,
				76,
				70,
				67
			},

			{
				67,
				70,
				89,
				49,
				57
			},

			{
				71,
				56,
				81,
				65,
				75
			},

			{
				88,
				84,
				68,
				51,
				50
			},

			{
				87,
				69,
				77,
				53,
				72
			},

			{
				72,
				53,
				55,
				69,
				87
			},

			{
				50,
				51,
				86,
				84,
				88
			},

			{
				65,
				75,
				54,
				71,
				56
			},

			{
				49,
				57,
				82,
				67,
				70
			},

			{
				70,
				67,
				48,
				57,
				49
			},

			{
				56,
				71,
				66,
				75,
				65
			},

			{
				84,
				88,
				52,
				50,
				51
			},

			{
				69,
				87,
				78,
				72,
				53
			},

			{
				53,
				72,
				74,
				87,
				69
			},

			{
				51,
				50,
				85,
				88,
				84
			},

			{
				82,
				76,
				49,
				89,
				48
			},

			{
				54,
				80,
				65,
				81,
				66
			},

			{
				66,
				81,
				56,
				80,
				54
			},

			{
				48,
				89,
				70,
				76,
				82
			},

			{
				78,
				77,
				69,
				55,
				74
			},

			{
				52,
				68,
				84,
				86,
				85
			},

			{
				85,
				86,
				51,
				68,
				52
			},

			{
				74,
				55,
				53,
				77,
				78
			},

			{
				76,
				82,
				57,
				48,
				89
			},

			{
				80,
				54,
				75,
				66,
				81
			},

			{
				81,
				66,
				71,
				54,
				80
			},

			{
				89,
				48,
				67,
				82,
				76
			},

			{
				77,
				78,
				87,
				74,
				55
			},

			{
				68,
				52,
				88,
				85,
				86
			},

			{
				86,
				85,
				50,
				52,
				68
			},

			{
				55,
				74,
				72,
				78,
				77
			}
		};
		private static byte[,] seq = new byte[,]
		{

			{
				4,
				6,
				2,
				5,
				0,
				1,
				8,
				3,
				7,
				9
			},

			{
				2,
				5,
				8,
				1,
				4,
				3,
				0,
				6,
				7,
				9
			},

			{
				7,
				3,
				9,
				2,
				5,
				6,
				8,
				4,
				1,
				0
			},

			{
				3,
				1,
				2,
				8,
				7,
				9,
				4,
				0,
				5,
				6
			},

			{
				6,
				0,
				1,
				3,
				2,
				8,
				4,
				7,
				5,
				9
			},

			{
				9,
				1,
				5,
				7,
				4,
				2,
				3,
				0,
				6,
				8
			},

			{
				6,
				2,
				0,
				3,
				7,
				4,
				1,
				5,
				9,
				8
			},

			{
				9,
				7,
				8,
				0,
				6,
				5,
				3,
				1,
				4,
				2
			},

			{
				3,
				7,
				2,
				5,
				0,
				8,
				4,
				6,
				9,
				1
			},

			{
				3,
				0,
				4,
				5,
				7,
				1,
				8,
				9,
				6,
				2
			},

			{
				4,
				9,
				5,
				0,
				6,
				7,
				1,
				2,
				8,
				3
			},

			{
				6,
				8,
				7,
				1,
				5,
				2,
				9,
				4,
				0,
				3
			},

			{
				2,
				1,
				7,
				4,
				5,
				9,
				8,
				6,
				0,
				3
			},

			{
				7,
				1,
				8,
				0,
				3,
				4,
				5,
				2,
				6,
				9
			},

			{
				7,
				0,
				2,
				4,
				5,
				1,
				8,
				3,
				6,
				9
			},

			{
				2,
				0,
				8,
				6,
				5,
				4,
				1,
				7,
				3,
				9
			},

			{
				6,
				4,
				0,
				5,
				3,
				8,
				1,
				7,
				9,
				2
			},

			{
				9,
				1,
				8,
				0,
				6,
				3,
				4,
				2,
				7,
				5
			},

			{
				7,
				0,
				3,
				8,
				4,
				9,
				1,
				5,
				2,
				6
			},

			{
				8,
				9,
				3,
				1,
				4,
				6,
				0,
				5,
				7,
				2
			},

			{
				3,
				1,
				0,
				9,
				5,
				6,
				2,
				4,
				8,
				7
			},

			{
				9,
				5,
				0,
				4,
				1,
				6,
				8,
				2,
				7,
				3
			},

			{
				3,
				6,
				9,
				5,
				4,
				0,
				8,
				2,
				1,
				7
			},

			{
				2,
				3,
				1,
				0,
				5,
				9,
				7,
				6,
				4,
				8
			},

			{
				0,
				1,
				6,
				5,
				7,
				8,
				9,
				2,
				3,
				4
			},

			{
				2,
				6,
				0,
				3,
				5,
				9,
				7,
				8,
				4,
				1
			},

			{
				2,
				8,
				5,
				3,
				9,
				4,
				1,
				7,
				0,
				6
			},

			{
				8,
				4,
				2,
				1,
				0,
				5,
				9,
				7,
				6,
				3
			},

			{
				0,
				9,
				4,
				2,
				5,
				1,
				7,
				6,
				8,
				3
			},

			{
				3,
				7,
				4,
				8,
				5,
				6,
				9,
				0,
				2,
				1
			},

			{
				4,
				8,
				2,
				1,
				6,
				9,
				7,
				3,
				5,
				0
			},

			{
				5,
				4,
				8,
				6,
				1,
				0,
				9,
				3,
				7,
				2
			}
		};
		private static string[] m_trialSN = new string[]
		{
			"DD2BX-VW219-QT8U4-AVPY0",
			"PVUK4-8HDG4-FTW5X-KGPX7",
			"NTX3T-2BCY2-W3FE4-C4FRD",
			"Q6P2F-MGTPW-N8BG6-JNX1P",
			"YPMD7-0FXLN-FRW6L-J7FUC",
			"19LT8-R1Y0X-J7TW0-68HRU",
			"Q47JU-BC3TV-0W2DT-HTD4R",
			"TU8RU-84DVF-GWN5H-Q8J3K",
			"PPJBQ-X7QJA-61BWG-KVQYA",
			"MHR49-RLLFC-PW1DA-QEE49"
		};
		public unsafe static int isSNOK(string inputSN, int SNtype)
		{
			string[] trialSN = SNDecoder.m_trialSN;
			for (int i = 0; i < trialSN.Length; i++)
			{
				string text = trialSN[i];
				if (text.Equals(inputSN))
				{
					return 1;
				}
			}
			byte[] array = new byte[11];
			byte* ptr = null;
			byte* ptr2 = null;
			byte b = 0;
			byte[] array2 = new byte[25];
			string text2 = inputSN.Replace("-", "");
			char[] array3 = text2.ToCharArray();
			for (int j = 0; j < 20; j++)
			{
				array2[j] = (byte)array3[j];
			}
			byte b2 = SNDecoder.FindFromTable(array2[19]);
			ushort num2;
			ushort num = num2 = 0;
			ptr = (byte*)(&num);
			ptr2 = (byte*)(&num2);
			byte b3 = SNDecoder.FindFromTable(array2[15]);
			*ptr = b3;
			*ptr2 = b3;
			b3 = SNDecoder.FindFromTable(array2[16]);
			ptr[(int)1 / 1] = b3;
			ushort num3 = (ushort)b3;
			num3 = (ushort)(num3 << 5);
			num2 |= num3;
			b3 = SNDecoder.FindFromTable(array2[17]);
            ptr[(int)2 / 1] = b3;
			num3 = (ushort)b3;
			num3 = (ushort)(num3 << 10);
			num2 |= num3;
			b3 = SNDecoder.FindFromTable(array2[18]);
            ptr[(int)3 / 1] = b3;
			num3 = (ushort)b3;
			num3 = (ushort)(num3 << 15);
			num2 |= num3;
			byte b4 = 0;
			for (int j = 0; j < 15; j++)
			{
				b4 = (byte)((int)b4 ^ ((int)array2[j] ^ j + 20));
			}
			b4 &= 31;
			if (b2 != b4)
			{
				return -1;
			}
			array[10] = 0;
			int num4 = 0;
			int num5 = 0;
			for (int j = 0; j < 15; j++)
			{
				if (((int)num2 & 1 << j) == 0)
				{
					if (num4 == 0)
					{
						num4++;
					}
					else
					{
						if (num4 == 1)
						{
							num4++;
						}
						else
						{
							if (num4 == 2)
							{
								num4++;
							}
							else
							{
								if (num4 == 3)
								{
									num4++;
								}
								else
								{
									if (num4 == 4)
									{
										num5 = (int)SNDecoder.FindFromTable(array2[j]);
										num4++;
									}
								}
							}
						}
					}
				}
			}
			num4 = 0;
			int num6 = 0;
			int num7 = 0;
			b4 = 0;
			for (int j = 0; j < 15; j++)
			{
				if (((int)num2 & 1 << j) != 0)
				{
					int num8 = (int)SNDecoder.seq[num5, num6];
					array[num8] = array2[j];
					b4 += array[num8];
					num6++;
				}
				else
				{
					if (num4 == 0)
					{
						b2 = SNDecoder.FindFromTable(array2[j]);
						num4++;
					}
					else
					{
						if (num4 == 1)
						{
							b3 = SNDecoder.FindFromTable(array2[j]);
							num4++;
						}
						else
						{
							if (num4 == 2)
							{
								b = SNDecoder.FindFromTable(array2[j]);
								num7 = j;
								num4++;
							}
						}
					}
				}
			}
			b4 &= 31;
			if (b4 != b3)
			{
				return -2;
			}
			ptr2 = (byte*)(&num);
			b3 = 0;
			b4 = *ptr2;
			b3 ^= b4;
			b4 = ptr2[(int)1 / 1];
			b3 ^= b4;
            b4 = ptr2[(int)2 / 1];
			b3 ^= b4;
            b4 = ptr2[(int)3 / 1];
			b3 ^= b4;
			b3 &= 31;
			if (b2 != b3)
			{
				return -3;
			}
			b2 = 0;
			for (int j = 0; j < 19; j++)
			{
				if (j != num7)
				{
					b2 = (byte)((int)b2 ^ ((int)array2[j] ^ j + 66));
				}
			}
			b2 &= 31;
			if (b2 != b)
			{
				return -4;
			}
			int num9;
			if (SNtype == 1)
			{
				num9 = SNDecoder.iskeyexisted(array, SNDecoder.KeytableMaster_1, SNDecoder.KeytableMaster_2);
			}
			else
			{
				num9 = SNDecoder.iskeyexisted(array, SNDecoder.KeytableClient_1, SNDecoder.KeytableClient_2);
			}
			if (num9 < 0)
			{
				return num9;
			}
			return 0;
		}
		private static byte FindFromTable(byte findchar)
		{
			for (int i = 0; i < 32; i++)
			{
				if (SNDecoder.m_transTable[i] == findchar)
				{
					return (byte)i;
				}
			}
			return 255;
		}
		private static int iskeyexisted(byte[] StrFromKey, byte[,] Keytable_1, byte[,] Keytable_2)
		{
			int num = 0;
			while (num < 32 && (StrFromKey[0] != Keytable_1[num, 0] || StrFromKey[1] != Keytable_1[num, 1] || StrFromKey[2] != Keytable_1[num, 2] || StrFromKey[3] != Keytable_1[num, 3] || StrFromKey[4] != Keytable_1[num, 4]))
			{
				num++;
			}
			if (num >= 32)
			{
				return -5;
			}
			num = 0;
			while (num < 32 && (StrFromKey[5] != Keytable_2[num, 0] || StrFromKey[6] != Keytable_2[num, 1] || StrFromKey[7] != Keytable_2[num, 2] || StrFromKey[8] != Keytable_2[num, 3] || StrFromKey[9] != Keytable_2[num, 4]))
			{
				num++;
			}
			if (num >= 32)
			{
				return -6;
			}
			return 0;
		}
	}
}
