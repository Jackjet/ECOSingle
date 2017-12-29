using System;
namespace PDFjet.NET
{
	public class Annotation
	{
		internal string uri;
		internal string key;
		internal double x1;
		internal double y1;
		internal double x2;
		internal double y2;
		public Annotation(string uri, string key, double x1, double y1, double x2, double y2)
		{
			this.uri = uri;
			this.key = key;
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}
	}
}
