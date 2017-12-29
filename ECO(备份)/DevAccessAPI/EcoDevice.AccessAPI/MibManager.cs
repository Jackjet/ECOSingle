using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	internal static class MibManager
	{
		public static LeafVarBinding GetDiscoveryRequest_ALL()
		{
			LeafVarBinding leafVarBinding = new LeafVarBinding();
			leafVarBinding.Add(DeviceBaseMib.DeviceName);
			leafVarBinding.Add(DeviceBaseMib.FWversion);
			leafVarBinding.Add(DeviceBaseMib.Mac);
			leafVarBinding.Add(DeviceBaseMib.ModelName);
			leafVarBinding.Add(DashboardMib.DashboradRackname);
			leafVarBinding.Add(EatonPDUBaseMib.DeviceName);
			leafVarBinding.Add(EatonPDUBaseMib.FWversion);
			leafVarBinding.Add(EatonPDUBaseMib.Mac);
			leafVarBinding.Add(EatonPDUBaseMib.ModelName);
			leafVarBinding.Add(EatonPDUBaseMib_M2.DeviceName);
			leafVarBinding.Add(EatonPDUBaseMib_M2.FWversion);
			leafVarBinding.Add(EatonPDUBaseMib_M2.Mac);
			leafVarBinding.Add(EatonPDUBaseMib_M2.ModelName);
			return leafVarBinding;
		}
		public static PropertiesMessage GetDiscoveryMessage_ALL(System.Collections.Generic.Dictionary<string, string> variables)
		{
			if (variables == null || variables.Count < 1)
			{
				return null;
			}
			PropertiesMessage propertiesMessage = new PropertiesMessage();
			System.Collections.Generic.IEnumerator<string> enumerator = variables.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				string text = variables[current];
				if (!text.Equals("Null"))
				{
					if (current.StartsWith(DeviceBaseMib.DeviceName) || current.StartsWith(EatonPDUBaseMib.DeviceName) || current.StartsWith(EatonPDUBaseMib_M2.DeviceName))
					{
						if ("\0".Equals(text) || string.IsNullOrEmpty(text))
						{
							text = string.Empty;
						}
						propertiesMessage.DeviceName = text;
					}
					else
					{
						if (current.StartsWith(DeviceBaseMib.FWversion) || current.StartsWith(EatonPDUBaseMib.FWversion) || current.StartsWith(EatonPDUBaseMib_M2.FWversion))
						{
							propertiesMessage.FirwWareVersion = text;
						}
						else
						{
							if (current.StartsWith(DeviceBaseMib.Mac) || current.StartsWith(EatonPDUBaseMib.Mac) || current.StartsWith(EatonPDUBaseMib_M2.Mac))
							{
								propertiesMessage.MacAddress = text.Replace(" ", ":").Replace("-", ":");
							}
							else
							{
								if (current.StartsWith(DeviceBaseMib.ModelName) || current.StartsWith(EatonPDUBaseMib.ModelName) || current.StartsWith(EatonPDUBaseMib_M2.ModelName))
								{
									if ("\0".Equals(text) || string.IsNullOrEmpty(text))
									{
										return null;
									}
									propertiesMessage.ModelName = text;
								}
								else
								{
									if (!current.StartsWith(DashboardMib.DashboradRackname))
									{
										return null;
									}
									propertiesMessage.DashboardRackname = text;
								}
							}
						}
					}
				}
			}
			if (propertiesMessage.DashboardRackname == null)
			{
				propertiesMessage.DashboardRackname = "";
			}
			propertiesMessage.CreateTime = System.DateTime.Now;
			return propertiesMessage;
		}
	}
}
