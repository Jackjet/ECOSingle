using System;
using System.Collections.Generic;
namespace PDFjet.NET
{
	public class OptionalContentGroup
	{
		internal string name;
		internal int ocgNumber;
		internal int objNumber;
		internal bool visible;
		internal bool printable;
		internal bool exportable;
		private List<IDrawable> components;
		public OptionalContentGroup(string name)
		{
			this.name = name;
			this.components = new List<IDrawable>();
		}
		public void Add(IDrawable d)
		{
			this.components.Add(d);
		}
		public void SetVisible(bool visible)
		{
			this.visible = visible;
		}
		public void SetPrintable(bool printable)
		{
			this.printable = printable;
		}
		public void SetExportable(bool exportable)
		{
			this.exportable = exportable;
		}
		public void DrawOn(Page p)
		{
			if (this.components.Count > 0)
			{
				p.pdf.groups.Add(this);
				this.ocgNumber = p.pdf.groups.Count;
				p.pdf.Newobj();
				p.pdf.Append("<<\n");
				p.pdf.Append("/Type /OCG\n");
				p.pdf.Append("/Name (" + this.name + ")\n");
				p.pdf.Append(">>\n");
				p.pdf.Endobj();
				this.objNumber = p.pdf.objNumber;
				p.Append("/OC /OC");
				p.Append(this.ocgNumber);
				p.Append(" BDC\n");
				foreach (IDrawable current in this.components)
				{
					current.DrawOn(p);
				}
				p.Append("\nEMC\n");
			}
		}
	}
}
