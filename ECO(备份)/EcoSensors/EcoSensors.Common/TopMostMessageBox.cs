using System;
using System.Drawing;
using System.Windows.Forms;
namespace EcoSensors.Common
{
	public static class TopMostMessageBox
	{
		public static DialogResult Show(string message, MessageBoxIcon icon)
		{
			return TopMostMessageBox.Show(message, string.Empty, MessageBoxButtons.OK, icon);
		}
		public static DialogResult Show(string message, string title, MessageBoxIcon icon)
		{
			return TopMostMessageBox.Show(message, title, MessageBoxButtons.OK, icon);
		}
		public static DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			Form form = new Form();
			form.Size = new Size(1, 1);
			form.StartPosition = FormStartPosition.Manual;
			Rectangle virtualScreen = SystemInformation.VirtualScreen;
			form.Location = new Point(virtualScreen.Bottom + 10, virtualScreen.Right + 10);
			form.Show();
			form.Focus();
			form.BringToFront();
			form.TopMost = true;
			DialogResult result = MessageBox.Show(form, message, title, buttons, icon);
			form.Dispose();
			return result;
		}
	}
}
