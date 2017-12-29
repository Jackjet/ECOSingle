using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class LeafVarBinding : VarBinding
	{
		private System.Collections.Generic.Dictionary<string, object> leafVarBindinds = new System.Collections.Generic.Dictionary<string, object>();
		private MaxRepetition maxRepetition = MaxRepetition.Ten;
		public System.Collections.Generic.Dictionary<string, object> VarBindings
		{
			get
			{
				return this.leafVarBindinds;
			}
		}
		public MaxRepetition MaxRepetition
		{
			get
			{
				return this.maxRepetition;
			}
			set
			{
				this.maxRepetition = value;
			}
		}
		public LeafVarBinding Add(string variable, object value)
		{
			this.leafVarBindinds.Add(variable, value);
			return this;
		}
		public LeafVarBinding Add(string variable)
		{
			this.leafVarBindinds.Add(variable, null);
			return this;
		}
		public void Remove(string variable)
		{
			this.leafVarBindinds.Remove(variable);
		}
	}
}
