using System;
using System.Collections.Generic;
namespace EcoDevice.AccessAPI
{
	public class LeafVBBuilder
	{
		public delegate void BuildVbDelegate(int index, LeafVarBinding vb);
		private const int maxNodes = 32;
		private int nodes;
		private int indexsPerRead;
		private int indexs;
		public LeafVBBuilder(int nodes, int indexs)
		{
			if (nodes <= 0 || indexs <= 0)
			{
				throw new System.ArgumentException("The nodes or ports number must be greater than 0.");
			}
			this.nodes = nodes;
			this.indexs = indexs;
			this.indexsPerRead = 32 / nodes;
		}
		public void BuildVbByIndex(System.Collections.Generic.List<LeafVarBinding> vbList, LeafVBBuilder.BuildVbDelegate portDelegate)
		{
			if (vbList == null)
			{
				vbList = new System.Collections.Generic.List<LeafVarBinding>();
			}
			int i = 1;
			if (this.nodes * this.indexs > 32)
			{
				int num = this.indexs / this.indexsPerRead;
				if (this.indexs % this.indexsPerRead > 0)
				{
					num++;
				}
				for (int j = 1; j <= num; j++)
				{
					LeafVarBinding leafVarBinding = new LeafVarBinding();
					while (i <= this.indexsPerRead * j && i <= this.indexs)
					{
						portDelegate(i, leafVarBinding);
						i++;
					}
					vbList.Add(leafVarBinding);
					if (leafVarBinding.VarBindings.Count % this.nodes != 0)
					{
						throw new System.Exception("The number of elements  is larger than " + this.nodes);
					}
				}
				return;
			}
			LeafVarBinding leafVarBinding2 = new LeafVarBinding();
			while (i <= this.indexs)
			{
				portDelegate(i, leafVarBinding2);
				i++;
			}
			vbList.Add(leafVarBinding2);
			if (leafVarBinding2.VarBindings.Count % this.nodes != 0)
			{
				throw new System.Exception("The number of elements is greater than " + this.nodes);
			}
		}
	}
}
