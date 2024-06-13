using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontEndWPF
{
    public class ProductosViewModel : INotifyPropertyChanged
    {
        private Conexion conexion;

        public ObservableCollection<Producto> Productos { get; set; }

        public ProductosViewModel()
        {
            conexion = new Conexion();
            Productos = new ObservableCollection<Producto>();
            LoadProductosData();
        }

        public void LoadProductosData()
        {
            var productosData = conexion.GetProductos();
            if (productosData != null && productosData.Count > 0)
            {
                foreach (var productoData in productosData)
                {
                    var producto = new Producto
                    {
                        Id = Convert.ToInt32(productoData["Id"]),
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

        public bool EliminarProducto(int id)
        {
            bool eliminado = conexion.EliminarProducto(id);
            if (eliminado)
            {
                var producto = Productos.FirstOrDefault(p => p.Id == id);
                if (producto != null)
                {
                    Productos.Remove(producto);
                }
                OnPropertyChanged(nameof(Productos));
            }
            return eliminado;
        }

        public bool ActualizarProducto(int id, int codigo, string nombre, string categoria, decimal precio, bool activo)
        {
            bool actualizado = conexion.ActualizarProducto(id, codigo, nombre, categoria, precio, activo);
            if (actualizado)
            {
                var producto = Productos.FirstOrDefault(p => p.Id == id);
                if (producto != null)
                {
                    producto.Codigo = codigo.ToString();
                    producto.Nombre = nombre;
                    producto.Categoria = categoria;
                    producto.Precio = precio;
                    producto.Activo = activo;
                    OnPropertyChanged(nameof(Productos));
                }
            }
            return actualizado;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Producto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
    }
}
