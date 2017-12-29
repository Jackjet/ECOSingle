using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	internal class DictionaryUtil
	{
		private System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> results = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>();
		public System.Collections.Generic.Dictionary<string, string> Result
		{
			get
			{
				System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
				foreach (System.Collections.Generic.Dictionary<string, string> current in this.results)
				{
					System.Collections.Generic.IEnumerator<string> enumerator2 = current.Keys.GetEnumerator();
					while (enumerator2.MoveNext())
					{
						if (!dictionary.ContainsKey(enumerator2.Current))
						{
							dictionary.Add(enumerator2.Current, current[enumerator2.Current]);
						}
					}
				}
				return dictionary;
			}
		}
		public void Add(System.Collections.Generic.Dictionary<string, string> result)
		{
			if (result == null)
			{
				throw new System.ArgumentNullException("The response value is null.");
			}
			this.results.Add(result);
		}
	}
}
