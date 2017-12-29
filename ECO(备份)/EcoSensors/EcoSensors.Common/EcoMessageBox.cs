using System;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	internal class EcoMessageBox
	{
		public static void ShowError(IWin32Window owner, string text)
		{
			MessageBox.Show(owner, text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		public static void ShowError(string text)
		{
			MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		public static void ShowInfo(IWin32Window owner, string text)
		{
			MessageBox.Show(owner, text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		public static void ShowInfo(string text)
		{
			MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		public static DialogResult ShowWarning(string text, MessageBoxButtons buttons)
		{
			return MessageBox.Show(null, text, "Warning", buttons, MessageBoxIcon.Exclamation);
		}
	}
}
