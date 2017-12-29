using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
namespace EcoSensors.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0"), System.Runtime.CompilerServices.CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}
		private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
		{
		}
		private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
		{
		}
	}
}
