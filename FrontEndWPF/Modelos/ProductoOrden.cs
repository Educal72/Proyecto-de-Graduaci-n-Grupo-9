using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Modelos
{
    public class ProductoOrden
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }

}
