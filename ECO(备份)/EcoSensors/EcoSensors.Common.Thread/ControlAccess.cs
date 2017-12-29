using System;
using System.Windows.Forms;
namespace EcoSensors.Common.Thread
{
	public class ControlAccess
	{
		public delegate void ConfigControl(Control control, object obj);
		private ContainerControl form;
		private ControlAccess.ConfigControl config;
		public ControlAccess(ContainerControl form, ControlAccess.ConfigControl config)
		{
			this.form = form;
			this.config = config;
		}
		public void Access(Control control, object obj)
		{
			if (!control.InvokeRequired)
			{
				this.config(control, obj);
				return;
			}
			if (this.form == null)
			{
				return;
			}
			ControlAccess.ConfigControl method = new ControlAccess.ConfigControl(this.Access);
			this.form.Invoke(method, new object[]
			{
				control,
				obj
			});
		}
	}
}
