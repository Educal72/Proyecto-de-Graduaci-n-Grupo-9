using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.Services
{
    public class Producto
    {
        public int Cantidad { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public List<Producto> Productos { get; set; }
        public string CreationTime { get; set; }
        public string Estado { get; set; }
    }

    public class SelectedOrderService
    {
        public static Order SelectedOrder { get; set; }
    }
}
