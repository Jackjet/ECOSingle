using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public interface SnmpSession
	{
		SnmpConfig SnmpConfig
		{
			get;
		}
		System.Collections.Generic.Dictionary<string, string> Get(LeafVarBinding varBindings);
		System.Collections.Generic.Dictionary<string, string> Get(System.Collections.Generic.List<LeafVarBinding> varBindings);
		System.Collections.Generic.Dictionary<string, string> GetNext(LeafVarBinding varBindings);
		System.Collections.Generic.Dictionary<string, string> Set(LeafVarBinding varBindings);
		System.Collections.Generic.Dictionary<string, string> GetBulk(string startVariable);
		System.Collections.Generic.Dictionary<string, string> GetBulk(string startVariable, MaxRepetition maxRepetition);
		System.Collections.Generic.Dictionary<string, string> GetTable(TableVarBinding tableVariable);
		System.Collections.Generic.Dictionary<string, string> Walk(System.Collections.Generic.List<VarBinding> variables);
	}
}
