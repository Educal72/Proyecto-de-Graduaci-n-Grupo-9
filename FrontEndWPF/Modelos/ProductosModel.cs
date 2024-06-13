using System;

namespace FrontEndWPF.Models
{
    public class ProductosModel
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }
}
