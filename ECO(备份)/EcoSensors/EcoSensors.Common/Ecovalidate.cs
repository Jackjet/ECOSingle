using EcoDevice.AccessAPI;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	internal class Ecovalidate
	{
		public static bool ValidDevName(string checkStr)
		{
			Regex regex = new Regex("^[a-zA-Z0-9_ ]+$");
			return regex.IsMatch(checkStr);
		}
		public static bool validEmail(string checkStr)
		{
			string pattern = "^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";
			Regex regex = new Regex(pattern);
			return regex.IsMatch(checkStr);
		}
		public static void checkTextIsNull(TextBox tb, ref bool flg)
		{
			tb.Text = tb.Text.Trim();
			if (tb.Text.Equals(string.Empty))
			{
				tb.Focus();
				flg = true;
			}
		}
		public static bool Rangeint(TextBox tb, int minV, int maxV)
		{
			int num = 0;
			try
			{
				num = System.Convert.ToInt32(tb.Text);
			}
			catch (System.Exception)
			{
				tb.Focus();
				bool result = false;
				return result;
			}
			if (num < minV || num > maxV)
			{
				tb.Focus();
				return false;
			}
			return true;
		}
		public static bool RangeDouble(TextBox tb, double minV, double maxV)
		{
			double num = 0.0;
			try
			{
				num = (double)System.Convert.ToSingle(tb.Text);
			}
			catch (System.Exception)
			{
				tb.Focus();
				bool result = false;
				return result;
			}
			if (num < minV || num > maxV)
			{
				tb.Focus();
				return false;
			}
			return true;
		}
		public static bool minlength(TextBox tb, int minLen)
		{
			tb.Text = tb.Text.Trim();
			if (tb.Text.Length < minLen)
			{
				tb.Focus();
				return false;
			}
			return true;
		}
		public static void checkEmpty(DevModelConfig devcfg, TextBox tb, ref bool flg, int selNo = -1)
		{
		}
		public static void checkThresholdValue(TextBox tb, Label lbrange, bool enableEmpty, ref bool flg)
		{
			if (!tb.Enabled)
			{
				return;
			}
			if (!tb.Text.Equals(string.Empty))
			{
				try
				{
					System.Convert.ToSingle(tb.Text);
				}
				catch (System.FormatException)
				{
					tb.BackColor = Color.Red;
					tb.Focus();
					flg = false;
					return;
				}
				float num = System.Convert.ToSingle(tb.Text);
				string numberDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				char c = numberDecimalSeparator[0];
				string[] array = System.Convert.ToString(num).Split(new char[]
				{
					c
				});
				if (array.Length <= 1 || array[1].Length <= 1)
				{
					string text = lbrange.Text;
					text = text.Substring(1, text.Length - 2);
					string key;
					switch (key = tb.Tag.ToString())
					{
					case "current":
					case "voltage":
					case "power":
					case "powerDiss":
					case "humidity":
					case "temperature":
					case "press":
					case "ocurrent":
					case "ovoltage":
					case "opower":
					case "opowerDiss":
					case "bcurrent":
					case "bvoltage":
					case "bpower":
					case "bpowerDiss":
					case "lcurrent":
					case "lvoltage":
					case "lpower":
					case "lpowerDiss":
						if (num >= System.Convert.ToSingle(devcfgUtil.Rang_min(text)) && num <= System.Convert.ToSingle(devcfgUtil.Rang_max(text)))
						{
							tb.BackColor = Color.White;
							return;
						}
						tb.BackColor = Color.Red;
						tb.Focus();
						flg = false;
						break;

						return;
					}
					return;
				}
				tb.BackColor = Color.Red;
				tb.Focus();
				flg = false;
				return;
			}
			if (enableEmpty)
			{
				return;
			}
			tb.BackColor = Color.Red;
			tb.Focus();
			flg = false;
		}
		public static void checkThresholdMaxMixValue(TextBox maxtb, TextBox mintb, ref bool flg)
		{
			if (maxtb.Text.Equals(string.Empty) || mintb.Text.Equals(string.Empty))
			{
				return;
			}
			if (System.Convert.ToDouble(maxtb.Text) <= System.Convert.ToDouble(mintb.Text))
			{
				maxtb.BackColor = Color.Red;
				mintb.BackColor = Color.Red;
				mintb.Focus();
				flg = false;
			}
		}
		public static bool NumberFormat_double(TextBox tb)
		{
			try
			{
				System.Convert.ToDouble(tb.Text);
			}
			catch (System.Exception)
			{
				tb.Focus();
				return false;
			}
			return true;
		}
		public static bool IsAddressValid(string addrString)
		{
			IPAddress iPAddress;
			return IPAddress.TryParse(addrString, out iPAddress);
		}
		public static bool inputCheck_float(TextBox tb, char inputchar, int precise)
		{
			string numberDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			char c = numberDecimalSeparator[0];
			if (inputchar == '\b')
			{
				return true;
			}
			if (inputchar != '-' && inputchar != c && (inputchar < '0' || inputchar > '9'))
			{
				return false;
			}
			string text = tb.Text;
			int selectionStart = tb.SelectionStart;
			string text2 = text.Substring(0, selectionStart);
			text2 += inputchar;
			text2 += text.Substring(selectionStart + tb.SelectionLength);
			string[] array = text2.Split(new char[]
			{
				'-'
			});
			if (array.Length > 2)
			{
				return false;
			}
			if (text2.Contains('-') && text2[0] != '-')
			{
				return false;
			}
			string[] array2 = text2.Split(new char[]
			{
				c
			});
			return array2.Length <= 2 && (array2.Length <= 1 || array2[1].Length <= precise);
		}
	}
}
