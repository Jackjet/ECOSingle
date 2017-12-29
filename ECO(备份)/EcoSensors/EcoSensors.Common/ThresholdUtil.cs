using DBAccessAPI;
using EcoDevice.AccessAPI;
using EcoSensors.EnegManPage.DashBoard;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	internal class ThresholdUtil
	{
		public static void SetUIEdit(TextBox tb, bool editable, float v, int isTemperature = 0, string fmt = "F1")
		{
			if (!editable || v == -600f)
			{
				tb.BackColor = Color.Silver;
				tb.Enabled = false;
				tb.ReadOnly = true;
				tb.Text = "";
				return;
			}
			tb.BackColor = Color.White;
			tb.Enabled = true;
			tb.ReadOnly = false;
			if (isTemperature == 0 || EcoGlobalVar.TempUnit == 0)
			{
				tb.Text = (v.ToString().Equals((-300).ToString()) ? "" : v.ToString(fmt));
				return;
			}
			tb.Text = (v.ToString().Equals((-300).ToString()) ? "" : RackStatusAll.CtoFdegrees((double)v).ToString(fmt));
		}
		public static void UI2Dev(int thflg, DeviceThreshold threshold)
		{
			if ((thflg & 1) != 0)
			{
				threshold.MinCurrentMT = -500f;
			}
			if ((thflg & 2) != 0)
			{
				threshold.MaxCurrentMT = -500f;
			}
			if ((thflg & 4) != 0)
			{
				threshold.MinVoltageMT = -500f;
			}
			if ((thflg & 8) != 0)
			{
				threshold.MaxVoltageMT = -500f;
			}
			if ((thflg & 16) != 0)
			{
				threshold.MinPowerMT = -500f;
			}
			if ((thflg & 32) != 0)
			{
				threshold.MaxPowerMT = -500f;
			}
			if ((thflg & 128) != 0)
			{
				threshold.MaxPowerDissMT = -500f;
			}
		}
		public static void UI2Dev(int thflg, BankThreshold threshold)
		{
			if ((thflg & 1) != 0)
			{
				threshold.MinCurrentMt = -500f;
			}
			if ((thflg & 2) != 0)
			{
				threshold.MaxCurrentMT = -500f;
			}
			if ((thflg & 4) != 0)
			{
				threshold.MinVoltageMT = -500f;
			}
			if ((thflg & 8) != 0)
			{
				threshold.MaxVoltageMT = -500f;
			}
			if ((thflg & 16) != 0)
			{
				threshold.MinPowerMT = -500f;
			}
			if ((thflg & 32) != 0)
			{
				threshold.MaxPowerMT = -500f;
			}
			if ((thflg & 128) != 0)
			{
				threshold.MaxPowerDissMT = -500f;
			}
		}
		public static void UI2Dev(int thflg, LineThreshold threshold)
		{
			if ((thflg & 1) != 0)
			{
				threshold.MinCurrentMt = -500f;
			}
			if ((thflg & 2) != 0)
			{
				threshold.MaxCurrentMT = -500f;
			}
			if ((thflg & 4) != 0)
			{
				threshold.MinVoltageMT = -500f;
			}
			if ((thflg & 8) != 0)
			{
				threshold.MaxVoltageMT = -500f;
			}
			if ((thflg & 16) != 0)
			{
				threshold.MinPowerMT = -500f;
			}
			if ((thflg & 32) != 0)
			{
				threshold.MaxPowerMT = -500f;
			}
		}
		public static void UI2Dev(int thflg, OutletThreshold threshold)
		{
			if ((thflg & 1) != 0)
			{
				threshold.MinCurrentMt = -500f;
			}
			if ((thflg & 2) != 0)
			{
				threshold.MaxCurrentMT = -500f;
			}
			if ((thflg & 4) != 0)
			{
				threshold.MinVoltageMT = -500f;
			}
			if ((thflg & 8) != 0)
			{
				threshold.MaxVoltageMT = -500f;
			}
			if ((thflg & 16) != 0)
			{
				threshold.MinPowerMT = -500f;
			}
			if ((thflg & 32) != 0)
			{
				threshold.MaxPowerMT = -500f;
			}
			if ((thflg & 128) != 0)
			{
				threshold.MaxPowerDissMT = -500f;
			}
		}
		public static void UI2Dev(int thflg, SensorThreshold threshold, SensorInfo dbSS)
		{
			if ((thflg & 1) != 0)
			{
				threshold.MinTemperatureMT = -500f;
			}
			else
			{
				if (dbSS.Min_temperature == -600f)
				{
					threshold.MinTemperatureMT = -600f;
				}
			}
			if ((thflg & 2) != 0)
			{
				threshold.MaxTemperatureMT = -500f;
			}
			else
			{
				if (dbSS.Max_temperature == -600f)
				{
					threshold.MaxTemperatureMT = -600f;
				}
			}
			if ((thflg & 4) != 0)
			{
				threshold.MinHumidityMT = -500f;
			}
			else
			{
				if (dbSS.Min_humidity == -600f)
				{
					threshold.MinHumidityMT = -600f;
				}
			}
			if ((thflg & 8) != 0)
			{
				threshold.MaxHumidityMT = -500f;
			}
			else
			{
				if (dbSS.Max_humidity == -600f)
				{
					threshold.MaxHumidityMT = -600f;
				}
			}
			if ((thflg & 16) != 0)
			{
				threshold.MinPressMT = -500f;
			}
			else
			{
				if (dbSS.Min_press == -600f)
				{
					threshold.MinPressMT = -600f;
				}
			}
			if ((thflg & 32) != 0)
			{
				threshold.MaxPressMT = -500f;
				return;
			}
			if (dbSS.Max_press == -600f)
			{
				threshold.MaxPressMT = -600f;
			}
		}
		public static float UI2DB(TextBox tbBox, float oldV, int isTemperature = 0)
		{
			if (!tbBox.Enabled)
			{
				return oldV;
			}
			if (tbBox.Text.Equals(string.Empty))
			{
				return -300f;
			}
			if (isTemperature == 0 || EcoGlobalVar.TempUnit == 0)
			{
				return System.Convert.ToSingle(tbBox.Text);
			}
			return (float)RackStatusAll.FtoCdegrees((double)System.Convert.ToSingle(tbBox.Text));
		}
	}
}
