using DBAccessAPI;
using System;
using System.Collections;
using System.Linq;
namespace EcoSensors.DevManDevice
{
	public class commDev
	{
		public static bool updateZoneforRack(RackInfo pRack)
		{
			if (pRack == null)
			{
				return false;
			}
			System.Collections.ArrayList allZone = ZoneInfo.getAllZone();
			bool result = false;
			string text = pRack.RackID.ToString();
			int startPoint_X = pRack.StartPoint_X;
			int startPoint_Y = pRack.StartPoint_Y;
			int endPoint_X = pRack.EndPoint_X;
			int arg_3A_0 = pRack.EndPoint_Y;
			int num;
			if (startPoint_X == endPoint_X)
			{
				num = 0;
			}
			else
			{
				num = 1;
			}
			foreach (ZoneInfo zoneInfo in allZone)
			{
				string rackInfo = zoneInfo.RackInfo;
				string[] array = rackInfo.Split(new char[]
				{
					','
				});
				int startPointX = zoneInfo.StartPointX;
				int startPointY = zoneInfo.StartPointY;
				int endPointX = zoneInfo.EndPointX;
				int endPointY = zoneInfo.EndPointY;
				bool flag = false;
				string text2 = "";
				bool flag2 = false;
				if (((startPoint_X <= startPointX && startPoint_X >= endPointX) || (startPoint_X >= startPointX && startPoint_X <= endPointX)) && ((startPoint_Y <= startPointY && startPoint_Y >= endPointY) || (startPoint_Y >= startPointY && startPoint_Y <= endPointY)))
				{
					flag2 = true;
				}
				else
				{
					if (num == 0)
					{
						if (((startPoint_X <= startPointX && startPoint_X >= endPointX) || (startPoint_X >= startPointX && startPoint_X <= endPointX)) && ((startPoint_Y + 1 <= startPointY && startPoint_Y + 1 >= endPointY) || (startPoint_Y + 1 >= startPointY && startPoint_Y + 1 <= endPointY)))
						{
							flag2 = true;
						}
					}
					else
					{
						if (num == 1 && ((startPoint_X + 1 <= startPointX && startPoint_X + 1 >= endPointX) || (startPoint_X + 1 >= startPointX && startPoint_X + 1 <= endPointX)) && ((startPoint_Y <= startPointY && startPoint_Y >= endPointY) || (startPoint_Y >= startPointY && startPoint_Y <= endPointY)))
						{
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					if (array.Contains(text))
					{
						continue;
					}
					if (array.Length == 1 && array[0].Equals(string.Empty))
					{
						text2 = text;
					}
					else
					{
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text3 = array2[i];
							if (text2.Equals(string.Empty))
							{
								text2 = text3;
							}
							else
							{
								text2 = text2 + "," + text3;
							}
						}
						if (text2.Equals(string.Empty))
						{
							text2 = text;
						}
						else
						{
							text2 = text2 + "," + text;
						}
					}
					flag = true;
				}
				else
				{
					if (array.Contains(text))
					{
						string[] array3 = array;
						for (int j = 0; j < array3.Length; j++)
						{
							string text4 = array3[j];
							if (!text4.Equals(text))
							{
								if (text2.Equals(string.Empty))
								{
									text2 = text4;
								}
								else
								{
									text2 = text2 + "," + text4;
								}
							}
						}
						flag = true;
					}
				}
				if (flag)
				{
					result = true;
					zoneInfo.RackInfo = text2;
					zoneInfo.UpdateZone();
				}
			}
			return result;
		}
	}
}
