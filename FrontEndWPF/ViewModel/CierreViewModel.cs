using DocumentFormat.OpenXml.Drawing.Diagrams;
using FrontEndWPF.PuntoDeVenta;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static FrontEndWPF.Reporteria.VisualizarPrestamos;

namespace FrontEndWPF.ViewModel
{

	public class CierreViewModel
	{
		Conexion conexion = new Conexion();
		string CajaId;
		DateTime CierreFecha;
		decimal PagadoEfectivo;
		decimal PagadoTarjeta;
		decimal TotalFinal;
		decimal TotalIVA;
		decimal TotalServicios;
		decimal TotalRestaurante;
		decimal TotalParaLlevar;
		decimal TotalPedidosYa;
		decimal TotalUberEats;
		decimal Anulado;

		int Id;
		int  IdUsuario;
		DateTime FechaApertura;
		decimal FondosApertura;
		decimal Egresos;
		decimal Ingresos;
		decimal FondosCierre;

		public bool ExisteCierreAbierto()
		{
			bool hasEntries = false;

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT COUNT(*) FROM CierreCaja WHERE Estado = 'Abierta'";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							int count = (int)command.ExecuteScalar();
							hasEntries = count > 0;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}

			return hasEntries;
		}

		public DateTime GetFechaActualCierre()
		{
			FileRead();
			DateTime hasEntries;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "SELECT FechaApertura FROM CierreCaja WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id", CajaId);

					hasEntries = (DateTime)command.ExecuteScalar();
					conexion.CloseConnection(connection);
					return hasEntries;
				}
			}
		}

		public int CrearCierre(decimal fondosApertura, int id) {

			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO CierreCaja (IdUsuario, FechaApertura, FondosApertura, Estado, FondosCierre, Ingresos, Egresos) OUTPUT Inserted.Id VALUES (@IdUsuario, @FechaApertura, @FondosApertura, @Estado, @FondosCierre,@Ingresos,@Egresos)";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@IdUsuario", id);
					command.Parameters.AddWithValue("@FechaApertura", DateTime.Now);
					command.Parameters.AddWithValue("@FondosApertura", fondosApertura);
					command.Parameters.AddWithValue("@FondosCierre", fondosApertura);
					command.Parameters.AddWithValue("@Estado", "Abierta");
					command.Parameters.AddWithValue("@Egresos", 0);
					command.Parameters.AddWithValue("@Ingresos", 0);
					int newOrderId = (int)command.ExecuteScalar();
					return newOrderId;
				}
			}
		}

		public void TerminarCierre()
		{
			FileRead();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
					SUM(CASE WHEN MetodoPago = 'Efectivo' THEN Total ELSE 0 END) AS SumaEfectivo,
					SUM(CASE WHEN MetodoPago = 'Tarjeta' THEN Total ELSE 0 END) AS SumaTarjeta,
					SUM(Impuestos) AS SumaImpuestos,
					SUM(Total) AS Total,
					SUM(Servicio) AS SumaServicio,
					SUM(CASE WHEN TipoVenta = 'Restaurante' THEN Total ELSE 0 END) AS SumaRestaurante,
					SUM(CASE WHEN TipoVenta = 'PedidosYa' THEN Total ELSE 0 END) AS SumaPedidosYa,
					SUM(CASE WHEN TipoVenta = 'Para Llevar' THEN Total ELSE 0 END) AS SumaParaLlevar,
					SUM(CASE WHEN TipoVenta = 'Uber Eats' THEN Total ELSE 0 END) AS SumaUberEats
						FROM Factura
						JOIN Orden ON Factura.IdOrden = Orden.Id
						WHERE FechaCreacion > @FechaCreacion
						AND Orden.Estado = 'Facturada';";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@FechaCreacion", GetFechaActualCierre());
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							PagadoEfectivo = (decimal)reader["SumaEfectivo"];
							PagadoTarjeta = (decimal)reader["SumaTarjeta"];
							TotalFinal = (decimal)reader["Total"];
							TotalIVA = (decimal)reader["SumaImpuestos"];
							TotalServicios = (decimal)reader["SumaServicio"];
							TotalRestaurante = (decimal)reader["SumaRestaurante"];
							TotalParaLlevar = (decimal)reader["SumaParaLlevar"];
							TotalPedidosYa = (decimal)reader["SumaPedidosYa"];
							TotalUberEats = (decimal)reader["SumaUberEats"];
						}
					}
				}
			}

			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query2 = @"SELECT 
								Id,
								IdUsuario,
								FechaApertura,
								FondosApertura,
								Egresos,
								Ingresos,
								Estado,
								FondosCierre
								FROM CierreCaja WHERE Id = @Id;";

				using (SqlCommand command2 = new SqlCommand(query2, connection))
				{
					command2.Parameters.AddWithValue("@Id", CajaId);
					using (SqlDataReader reader2 = command2.ExecuteReader())
					{
						while (reader2.Read())
						{
							Id = (int)reader2["Id"];
							IdUsuario = (int)reader2["IdUsuario"];
							FechaApertura = (DateTime)reader2["FechaApertura"];
							FondosApertura = (decimal)reader2["FondosApertura"];
							Egresos = (decimal)reader2["Egresos"];
							Ingresos = (decimal)reader2["Ingresos"];
							FondosCierre = (decimal)reader2["FondosCierre"];
						}
					}
				}

				string query3 = @"SELECT 
				SUM(Factura.Total) AS SumaAnuladas
					FROM 
				Factura
					JOIN 
				Orden ON Factura.IdOrden = Orden.Id
					WHERE 
				Orden.Estado = 'Anulada';";

				using (SqlCommand command3 = new SqlCommand(query3, connection))
				{
					using (SqlDataReader reader3 = command3.ExecuteReader())
					{
						while (reader3.Read())
						{
							decimal sumaAnulada = reader3["SumaAnuladas"] != DBNull.Value
	? (decimal)reader3["SumaAnuladas"]
	: 0;
						}
					}
				}
				TotalFinal = TotalFinal - Egresos + Ingresos;
				Cierre cierre = new Cierre
				{
					Id = Id,
					IdUsuario = IdUsuario,
					NombreUsuario = GetUserById(),
					FechaApertura = FechaApertura,
					CierreFecha = DateTime.Now,
					FondosApertura = FondosApertura,
					PagadoEfectivo = PagadoEfectivo,
					PagadoTarjeta = PagadoTarjeta,
					Ingresos = Ingresos,
					Egresos = Egresos,
					TotalFinal = TotalFinal,
					TotalIVA = TotalIVA,
					TotalServicios = TotalServicios,
					TotalRestaurante = TotalRestaurante,
					TotalLlevar = TotalParaLlevar,
					TotalPedidosYa = TotalPedidosYa,
					TotalUberEats = TotalUberEats,
					Anulado = Anulado,
					FondosCierre = FondosCierre,
					Estado = "Cerrada"
				};
				CierresCaja cierresCaja = new CierresCaja(cierre);
				CerrarCiere();
			}
		}

		public void CerrarCiere()
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE CierreCaja SET Estado = @Estado WHERE Id = @Id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Estado", "Cerrado");
					command.Parameters.AddWithValue("@Id", CajaId);
					command.ExecuteNonQuery();
				}
			}
		}

		public bool CheckEgreso(decimal dinero)
		{
			FileRead();
			decimal gabineteActual = 0;
				using (SqlConnection connection = conexion.OpenConnection())
				{
					string query = "SELECT FondosCierre FROM CierreCaja WHERE Id = @Id";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Id", CajaId);
						gabineteActual = (decimal)command.ExecuteScalar();
					}
				}
			if (gabineteActual < dinero)
			{
				return false;
			}
			else {
				return true;
			}

		}

		public void Gabinete(string tipo, decimal dinero)
		{
			FileRead();
			decimal gabineteActual = 0;
			if (tipo == "Egreso") {
				using (SqlConnection connection = conexion.OpenConnection())
				{
					string query = "SELECT Egresos FROM CierreCaja WHERE Id = @Id";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Id", CajaId);
						gabineteActual = (decimal)command.ExecuteScalar();
					}
					decimal gabineteNuevo = gabineteActual + dinero;
					string query2 = "UPDATE CierreCaja SET Egresos = @Egresos, FondosCierre = FondosCierre - @Egresos WHERE Id = @Id";
					using (SqlCommand command2 = new SqlCommand(query2, connection))
					{
						command2.Parameters.AddWithValue("@Id", CajaId);
						command2.Parameters.AddWithValue("@Egresos", gabineteNuevo);
						command2.ExecuteNonQuery();
					}
				}
			} else if (tipo =="Ingreso") {
				using (SqlConnection connection = conexion.OpenConnection())
				{
					string query = "SELECT Ingresos FROM CierreCaja WHERE Id = @Id";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Id", CajaId);
						gabineteActual = (decimal)command.ExecuteScalar();
					}
					decimal gabineteNuevo = gabineteActual + dinero;
					string query2 = "UPDATE CierreCaja SET Ingresos = @Ingresos, FondosCierre = FondosCierre + @Ingresos WHERE Id = @Id";
					using (SqlCommand command2 = new SqlCommand(query2, connection))
					{
						command2.Parameters.AddWithValue("@Id", CajaId);
						command2.Parameters.AddWithValue("@Ingresos", gabineteNuevo);
						command2.ExecuteNonQuery();
					}
				}
			}
		}
		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/caja.txt"))
			{
				return;
		}
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/caja.txt");
				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "Id":
								CajaId = parts[1];
								break;
						}
					}
				}
		}

		private string GetUserById()
		{
			string displayText = SesionUsuario.Instance.id.ToString();
			string query = @"
            SELECT 
                Nombre, 
                Apellido
            FROM 
                Usuario
WHERE Id = @Id"
			;

			using (SqlConnection connection = conexion.OpenConnection())
			{
					SqlCommand command = new SqlCommand(query, connection);

				command.Parameters.AddWithValue("@Id", SesionUsuario.Instance.id);
				SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						string nombre = reader["Nombre"].ToString();
						string Apellido = reader["Apellido"].ToString();
						displayText = $"{nombre} {Apellido}";
					}
				return displayText;
			}
		}

		public class Cierre
		{
			public int Id { get; set; }
			public int IdUsuario { get; set; }
			public string? NombreUsuario { get; set; }
			public DateTime FechaApertura { get; set; }
			public DateTime CierreFecha { get; set; }
			public decimal FondosApertura { get; set; }
			public decimal PagadoEfectivo { get; set; }
			public decimal PagadoTarjeta { get; set; }
			public decimal Egresos { get; set; }
			public decimal Ingresos { get; set; }
			public decimal TotalFinal { get; set; }
			public decimal TotalIVA { get; set; }
			public decimal TotalServicios { get; set; }
			public decimal TotalRestaurante { get; set; }
			public decimal TotalLlevar { get; set; }
			public decimal TotalPedidosYa { get; set; }
			public decimal TotalUberEats { get; set; }
			public decimal Anulado { get; set; }
			public decimal FondosCierre { get; set; }
			public string Estado { get; set; }
		}
	}
}
