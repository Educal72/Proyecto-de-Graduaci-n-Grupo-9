using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectoGrupo9
{
	public class RoundedPanel : Panel
	{
		public int Radius { get; set; } = 10; // Adjust the radius as needed

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using (Pen pen = new Pen(this.BackColor, 1))
			{
				pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
				e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
			}

			// Draw rounded rectangle border
			using (Pen borderPen = new Pen(Color.Black, 2))
			{
				borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
				e.Graphics.DrawArc(borderPen, 0, 0, Radius * 2, Radius * 2, 180, 90);
				e.Graphics.DrawLine(borderPen, Radius, 0, this.Width - Radius, 0);
				e.Graphics.DrawArc(borderPen, this.Width - Radius * 2, 0, Radius * 2, Radius * 2, 270, 90);
				e.Graphics.DrawLine(borderPen, this.Width, Radius, this.Width, this.Height - Radius);
				e.Graphics.DrawArc(borderPen, this.Width - Radius * 2, this.Height - Radius * 2, Radius * 2, Radius * 2, 0, 90);
				e.Graphics.DrawLine(borderPen, this.Width - Radius, this.Height, Radius, this.Height);
				e.Graphics.DrawArc(borderPen, 0, this.Height - Radius * 2, Radius * 2, Radius * 2, 90, 90);
				e.Graphics.DrawLine(borderPen, 0, this.Height - Radius, 0, Radius);
			}
		}
	}
}
