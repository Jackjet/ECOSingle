using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	internal class RSBlock
	{
		private int totalCount;
		private int dataCount;
		private RSBlock(int totalCount, int dataCount)
		{
			this.totalCount = totalCount;
			this.dataCount = dataCount;
		}
		public int GetDataCount()
		{
			return this.dataCount;
		}
		public int GetTotalCount()
		{
			return this.totalCount;
		}
		public static RSBlock[] GetRSBlocks(int errorCorrectLevel)
		{
			int[] rsBlockTable = RSBlock.GetRsBlockTable(errorCorrectLevel);
			int num = rsBlockTable.Length / 3;
			List<RSBlock> list = new List<RSBlock>();
			for (int i = 0; i < num; i++)
			{
				int num2 = rsBlockTable[3 * i];
				int num3 = rsBlockTable[3 * i + 1];
				int num4 = rsBlockTable[3 * i + 2];
				for (int j = 0; j < num2; j++)
				{
					list.Add(new RSBlock(num3, num4));
				}
			}
			return list.ToArray();
		}
		private static int[] GetRsBlockTable(int errorCorrectLevel)
		{
			switch (errorCorrectLevel)
			{
			case 0:
				return new int[]
				{
					2,
					50,
					32
				};
			case 1:
				return new int[]
				{
					1,
					100,
					80
				};
			case 2:
				return new int[]
				{
					4,
					25,
					9
				};
			case 3:
				return new int[]
				{
					2,
					50,
					24
				};
			default:
				return null;
			}
		}
	}
}
