using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.ViewModel.CierreViewModel;

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for GestionDeOrdenes.xaml
    /// </summary>
    public partial class GestionDeOrdenes : UserControl
	{
		Conexion conexion = new Conexion();
		List<DetalleFactura> D = new List<DetalleFactura>();
		public GestionDeOrdenes()
        {
            InitializeComponent();
            LoadOrdenes();
        }

		private void LoadOrdenes()
		{
			
			Conexion conexion = new Conexion();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = @"
                SELECT 
                    Factura.Id,
                    Factura.IdOrden,
                    Factura.CantidadPagada,
                    Factura.Impuestos,
                    Factura.Servicio,
                    Factura.FechaCreacion,
                    Factura.FechaCierre,
                    Factura.Cajero,
                    Factura.IdEmpleado,
                    Factura.IdCliente,
                    Factura.Descuento,
                    Factura.PuntosGanados,
                    Factura.MetodoPago,
                    Factura.TipoVenta,
                    Factura.Total,
                    Orden.Estado
                FROM 
                    Factura
                INNER JOIN 
                    Orden ON Factura.IdOrden = Orden.Id;";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									OrdenGestion ordenGestion = new OrdenGestion
									{
										NumeroFactura = 0, //(int)reader["Id"],
										IdOrden = 0, //(int)reader["IdOrden"],
										Cajero = "", //reader["Cajero"].ToString(),
										Salonero = "", //GetUserByEmpleadoId((int)reader["IdEmpleado"]),
										Cliente = "", //GetUserByClienteId((int)reader["IdCliente"]),
										FechaCreacion = DateTime.Now, //(DateTime)reader["FechaCreacion"],
										FechaCierre = DateTime.Now, //(DateTime)reader["FechaCierre"],
										Pagado = 0.00m, //(decimal)reader["CantidadPagada"],
										Total = 0.00m, //(decimal)reader["Total"],
										Impuesto = 0.00m, //(decimal)reader["Impuestos"],
										Servicio = 0.00m, //(decimal)reader["Servicio"],
										Estado = "", // reader["Estado"].ToString(),
										MetodoPago = "", // reader["MetodoPago"].ToString(),
										Detalles = D, //GetDetallesList((int)reader["IdOrden"]),
										Puntos = 0.00m, //(decimal)reader["PuntosGanados"],
										Tipo = "", //reader["TipoVenta"].ToString(),
										Cambio = 0.00m, //(decimal)reader["CantidadPagada"] - (decimal)reader["Total"],
										SubTotal = 0.00m, //(decimal)reader["Total"] - (decimal)reader["Servicio"] - (decimal)reader["Impuestos"] - (decimal)reader["Descuento"],
										Descuento = 0.00m, //(decimal)reader["Descuento"],
									};
									ordenesdataGrid.Items.Add(ordenGestion);
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}
		}
		public string GetUserByEmpleadoId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT Usuario.Cedula, Usuario.Nombre, Usuario.Apellido FROM Usuario JOIN Empleado ON Usuario.Id = Empleado.IdUsuario WHERE Empleado.Id = @IdEmpleado";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString() + " (" + reader["Cedula"].ToString() + ")";
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		public List<DetalleFactura> GetDetallesList(int id)
		{
			Conexion conexion = new Conexion();
			List<DetalleFactura> detalleFacturas = new List<DetalleFactura>();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    po.Cantidad,
    p.Nombre,
    p.Precio
FROM 
    ProductosOrden po
INNER JOIN 
    Productos p ON po.IdProducto = p.Id
WHERE 
    po.OrdenAsociada = @OrdenId;";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdOrden", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							DetalleFactura detalle = new DetalleFactura
							{
								Producto = reader["Nombre"].ToString(),
								Cantidad = (int)reader["Cantidad"],
								PrecioUnitario = (decimal)reader["Precio"]
							};
							detalleFacturas.Add(detalle);
						}
					}
					return detalleFacturas;
				}
			}
		}

		public string GetUserByClienteId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT Nombre, Apellidos FROM Cliente WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Id", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellidos"].ToString() + " (" + reader["Cedula"].ToString() + ")";
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		private void GenerarInforme_Click(object sender, RoutedEventArgs e)
		{

		}
	}


	public class OrdenGestion
	{
		public int NumeroFactura { get; set; }
		public int IdOrden { get; set; }
		public string Cajero { get; set; }
		public string Cliente { get; set; }
		public string Tipo { get; set; }
		public string MetodoPago { get; set; }
		public string Salonero { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaCierre { get; set; }
		public List<DetalleFactura> Detalles { get; set; }
		public decimal Puntos { get; set; }
		public decimal SubTotal { get; set; }
		public decimal Impuesto { get; set; }
		public decimal Servicio { get; set; }
		public decimal Total { get; set; }
		public decimal Descuento { get; set; }
		public decimal Pagado { get; set; }
		public decimal Cambio { get; set; }
		public string Estado { get; set; }
	}

	public class DetalleFactura
	{
		public string Producto { get; set; }
		public int Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal Total => Cantidad * PrecioUnitario;
	}
}
