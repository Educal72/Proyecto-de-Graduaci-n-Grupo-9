using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontEndWPF
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        private Conexion conexion;

        public ObservableCollection<Producto> Productos { get; private set; }

        public ProductosViewModel()
        {
            conexion = new Conexion();
            Productos = new ObservableCollection<Producto>();
            LoadProductosData();
        }

        private void LoadProductosData()
        {
            var productosData = conexion.GetProductos();
            if (productosData != null && productosData.Count > 0)
            {
                foreach (var productoData in productosData)
                {
                    var producto = new Producto
                    {
                        Codigo = productoData["Codigo"].ToString(),
                        Nombre = productoData["Nombre"].ToString(),
                        Categoria = productoData["Categoria"].ToString(),
                        Precio = Convert.ToDecimal(productoData["Precio"]),
                        Activo = Convert.ToBoolean(productoData["Activo"])
                    };
                    Productos.Add(producto);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }
}
