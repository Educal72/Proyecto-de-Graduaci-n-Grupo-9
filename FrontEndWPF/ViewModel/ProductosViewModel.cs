using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using static FrontEndWPF.Modelos.InventarioModel;

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
            Productos.Clear();
            var productosData = conexion.GetProductos();
            if (productosData != null && productosData.Count > 0)
            {
                foreach (var productoData in productosData)
                {
                    var producto = new Producto
                    {
                        Id = Convert.ToInt32(productoData["Id"]),
                        Codigo = Convert.ToInt32(productoData["Codigo"]),
                        Nombre = productoData["Nombre"].ToString(),
                        Categoria = productoData["Categoria"].ToString(),
                        Precio = Convert.ToDecimal(productoData["Precio"]),
                        Activo = Convert.ToBoolean(productoData["Activo"])
                    };
                    Productos.Add(producto);
                }
            }
        }

		public void LoadProductosDataActivo()
		{
			Productos.Clear();
			var productosData = conexion.GetProductosActivos();
			if (productosData != null && productosData.Count > 0)
			{
				foreach (var productoData in productosData)
				{
					var producto = new Producto
					{
						Id = Convert.ToInt32(productoData["Id"]),
						Codigo = Convert.ToInt32(productoData["Codigo"]),
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
                LoadProductosData();
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
                    producto.Codigo = codigo;
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

		public ObservableCollection<Producto> GetProductoEspecifico(int Codigo)
		{
            
			var productos = new List<Producto>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT Id, Codigo, Nombre, Categoria, Precio, Activo FROM Productos WHERE Codigo = @Codigo;";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							command.Parameters.AddWithValue("@Codigo", Codigo);
							using (SqlDataReader reader = command.ExecuteReader())
							{
                                Productos.Clear();
								while (reader.Read())
								{
									Producto producto = new Producto()
									{
										Id = Convert.ToInt32(reader["Id"]),
										Codigo = Convert.ToInt32(reader["Codigo"]),
										Nombre = reader["Nombre"].ToString(),
										Categoria = reader["Categoria"].ToString(),
										Precio = Convert.ToDecimal(reader["Precio"]),
										Activo = Convert.ToBoolean(reader["Activo"])
									};
									Productos.Add(producto);
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}

			return Productos;
		}
	}

    public class Producto
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        public decimal Cantidad { get; internal set; }
    }
}
