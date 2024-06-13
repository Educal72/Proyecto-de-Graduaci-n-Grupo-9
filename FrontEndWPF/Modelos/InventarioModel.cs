using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
	public class InventarioModel
	{
		public class InventarioM() { 
			public int Id { get; set; }
			public string Nombre { get; set; }
			public int Cantidad { get; set; }
			public decimal Precio { get; set; }
			public bool Activo { get; set; }
		}
		
	}
}
