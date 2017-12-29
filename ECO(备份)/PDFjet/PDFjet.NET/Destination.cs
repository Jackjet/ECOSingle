using System;
namespace PDFjet.NET
{
	public class Destination
	{
		public string name;
		public int pageObjNumber;
		public float yPosition;
		public Destination(string name, double yPosition)
		{
			this.name = name;
			this.yPosition = (float)yPosition;
		}
		public Destination(string name, float yPosition)
		{
			this.name = name;
			this.yPosition = yPosition;
		}
		internal void SetPageObjNumber(int pageObjNumber)
		{
			this.pageObjNumber = pageObjNumber;
		}
	}
}
