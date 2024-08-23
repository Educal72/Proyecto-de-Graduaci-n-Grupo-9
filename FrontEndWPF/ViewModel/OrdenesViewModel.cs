using FrontEndWPF.Reporteria;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FrontEndWPF.PuntoVenta;

namespace FrontEndWPF.ViewModel
{
	class OrdenesViewModel
	{
		Conexion conexion = new Conexion();

		public class ProductoOrden
		{
			public int Id { get; set; }
			public int OrdenId { get; set; }
			public int ProductoId { get; set; }
			public int Cantidad { get; set; }
		}

		public class ProductosOrden
		{
			public int Id { get; set; }
			public int OrdenId { get; set; }
			public int ProductoId { get; set; }
			public string Nombre { get; set; }
			public decimal Precio { get; set; }
			public int Cantidad { get; set; }
		}


		public class Orden
		{
			public int Id { get; set; }
			public DateTime Creacion { get; set; }
			public string Estado { get; set; }
		}

		public int CrearOrdenAsync(Orden orden)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO Orden (FechaHoraCreacion, Estado) OUTPUT INSERTED.Id VALUES (@FechaHoraCreacion, @Estado)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@FechaHoraCreacion", SqlDbType.DateTime) { Value = orden.Creacion });
					command.Parameters.Add(new SqlParameter("@Estado", SqlDbType.NVarChar) { Value = orden.Estado });
					int newOrderId = (int) command.ExecuteScalar();
					return newOrderId;
				}
			}
		}

		public int InsertarProductoUnico(string nombre, string categoria, decimal precio, bool activo)
		{
			bool success = false;

			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO Productos (Nombre, Categoria, Precio, Activo) OUTPUT INSERTED.Id VALUES (@Nombre, @Categoria, @Precio, @Activo)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Nombre", nombre);
					command.Parameters.AddWithValue("@Categoria", categoria);
					command.Parameters.AddWithValue("@Precio", precio);
					command.Parameters.AddWithValue("@Activo", activo);
					int newProductId = (int)command.ExecuteScalar();
					return newProductId;
				}
			}
		}

		public void CrearProductoOrden(List<ProductoOrden> productos)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				foreach (var producto in productos)
				{
					string query = "INSERT INTO ProductosOrden (IdProducto, Cantidad, OrdenAsociada) VALUES (@IdProducto, @Cantidad, @OrdenAsociada)";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.Add(new SqlParameter("@IdProducto", producto.ProductoId ));
						command.Parameters.Add(new SqlParameter("@Cantidad", producto.Cantidad ));
						command.Parameters.Add(new SqlParameter("@OrdenAsociada", producto.OrdenId ));

						command.ExecuteNonQuery();
					}
				}
			}
		}

		public List<Orden> GetAllOrdenes()
		{
			var ordenes = new List<Orden>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				using (var command = new SqlCommand("SELECT Id, FechaHoraCreacion, Estado FROM Orden WHERE Estado != @Estado AND Estado != @Estado2 AND Estado != @Estado3", connection))
				{
					command.Parameters.Add(new SqlParameter("@Estado", "Eliminada"));
					command.Parameters.Add(new SqlParameter("@Estado2", "Facturada"));
					command.Parameters.Add(new SqlParameter("@Estado3", "Anulada"));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							ordenes.Add(new OrdenesViewModel.Orden
							{
								Id = reader.GetInt32(0),
								Creacion = reader.GetDateTime(1),
								Estado = reader.GetString(2)
							});
						}
					}
				}
			}

			return ordenes;
		}

		public List<ProductosOrden> GetProductosByOrdenId(int ordenId)
		{
			var productos = new List<ProductosOrden>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				var query = @"
            SELECT 
                po.Id, 
                po.IdProducto, 
                p.Nombre AS NombreProducto, 
				p.Precio AS Precio,
                po.Cantidad, 
                po.OrdenAsociada 
            FROM 
                ProductosOrden po
            JOIN 
                Productos p ON po.IdProducto = p.Id
            WHERE 
                po.OrdenAsociada = @OrdenId";

				using (var command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@OrdenId", ordenId));

					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							productos.Add(new ProductosOrden
							{
								Id = reader.GetInt32(0),
								ProductoId = reader.GetInt32(1),
								Nombre = reader.GetString(2),
								Precio = reader.GetDecimal(3),
								Cantidad = reader.GetInt32(4),
								OrdenId = reader.GetInt32(5)
							});
						}
					}
				}
			}

			return productos;
		}

		public Dictionary<Orden, List<ProductosOrden>> GetOrdenesConProductos()
		{
			var ordenesConProductos = new Dictionary<Orden, List<ProductosOrden>>();

			var ordenes = GetAllOrdenes();

			foreach (var orden in ordenes)
			{
				var productos = GetProductosByOrdenId(orden.Id);
				ordenesConProductos.Add(orden, productos);
			}

			return ordenesConProductos;
		}

		public void EliminarOrden(int id)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Orden SET Estado = @Estado WHERE Id = @Id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Estado", "Eliminada");
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}
		public void EliminarProductoOrden(int idProductoOrden)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{

				string query = @"DELETE FROM ProductosOrden
                             WHERE OrdenAsociada = @IdProductoOrden";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@IdProductoOrden", idProductoOrden);

					command.ExecuteNonQuery();
				}
			}
		}

		public Dictionary<string, object> GetOrdenConProductosById(int id)
		{
			var orden = GetOrdenById(id);
			var productos = GetProductosByOrdenId(orden.Id);
			return new Dictionary<string, object>
		{
			{ "Orden", orden },
			{ "Productos", productos }
		};
		}

		public Orden GetOrdenById(int id)
		{
			var orden = new Orden();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				using (var command = new SqlCommand("SELECT Id, FechaHoraCreacion, Estado FROM Orden WHERE Estado != @Estado AND Id = @Id", connection))
				{
					command.Parameters.Add(new SqlParameter("@Estado", "Eliminada"));
					command.Parameters.Add(new SqlParameter("@Id", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							orden.Id = reader.GetInt32(0);
							orden.Creacion = reader.GetDateTime(1);
							orden.Estado = reader.GetString(2);
						}
					}
				}
			}

			return orden;
		}

		public int GetProductIdByCodigo(int codigo)
		{
			int resultado = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				using (var command = new SqlCommand("SELECT Id FROM Productos WHERE Codigo = @Codigo", connection))
				{
					command.Parameters.Add(new SqlParameter("@Codigo", codigo));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							resultado = reader.GetInt32(0);
						}
					}
				}
			}

			return resultado;
		}

		public void AnularOrden(int id)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Orden SET Estado = @Estado WHERE Id = @Id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Estado", "Anulada");
					command.Parameters.AddWithValue("@Id", id);
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
