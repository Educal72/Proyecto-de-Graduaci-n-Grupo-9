using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FrontEndWPF.Reporteria.VisualizarPrestamos;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.ViewModel
{
	
	public class PrestamosViewModel
	{
		Conexion conexion = new Conexion();
		public bool CrearPrestamo(int idEmpleado, string descripcion, double interes, double prestado, DateTime fecha)
		{
			double pendiente = prestado + (prestado * (interes / 100)); 
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO Prestamos (IdEmpleado, Descripcion, Interes, MontoTotal, FechaCreacion, Pendiente, Estado) VALUES (@IdEmpleado, @Descripcion, @Interes, @Prestado, @Fecha, @Pendiente, @Estado)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", idEmpleado));
					command.Parameters.Add(new SqlParameter("@Descripcion", descripcion));
					command.Parameters.Add(new SqlParameter("@Interes", interes));
					command.Parameters.Add(new SqlParameter("@Prestado", prestado));
					command.Parameters.Add(new SqlParameter("@Fecha", fecha));
					command.Parameters.Add(new SqlParameter("@Pendiente", pendiente));
					command.Parameters.Add(new SqlParameter("@Estado", "Activo"));
					int rowsAffected = (int)command.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						return true;
					}
					else
					{
						return false;
					}

				}
			}
		}

		public bool CrearPago(int idPrestamo, double cantidadpagada, DateTime fecha)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO PagosPrestamos (IdPrestamo, CantidadPagada, Fecha) VALUES (@IdPrestamo, @CantidadPagada, @Fecha)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdPrestamo", idPrestamo));
					command.Parameters.Add(new SqlParameter("@CantidadPagada", cantidadpagada));
					command.Parameters.Add(new SqlParameter("@Fecha", fecha));
					int rowsAffected = (int)command.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						double pendiente = GetPendienteFromPrestamo(idPrestamo);
						double nuevoPendiente = pendiente - cantidadpagada; 
						string query2 = "UPDATE Prestamos SET Pendiente = @Pendiente WHERE Id = @Id";
						using (SqlCommand command2 = new SqlCommand(query2, connection))
						{
							command2.Parameters.Add(new SqlParameter("@Id", idPrestamo));
							command2.Parameters.Add(new SqlParameter("@Pendiente", nuevoPendiente));
							int rowsAffected2 = (int)command2.ExecuteNonQuery();
							if (rowsAffected2 > 0)
							{
								double restante = GetPendienteFromPrestamo(idPrestamo);
								if (restante == 0.00)
								{
									string query3 = "UPDATE Prestamos SET Estado = 'Inactivo' WHERE Id = @Id";
									using (SqlCommand command3 = new SqlCommand(query3, connection))
									{
										command3.Parameters.Add(new SqlParameter("@Id", idPrestamo));
										int rowsAffected3 = (int)command3.ExecuteNonQuery();
										if (rowsAffected3 > 0)
										{
											return true;
										}
										else
										{
											return false;
										}
									}
								}
								else {
									return true;
								}	
							}
							else
							{
								return false;
							}

						}
					}
					else
					{
						return false;
					}

				}
			}


		}

		public double GetPendienteFromPrestamo(int id)
		{
			Conexion conexion = new Conexion();
			double Pendiente = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT Pendiente FROM Prestamos WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Id", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							Pendiente = Convert.ToDouble(reader["Pendiente"]);
							return Pendiente;
						}
					}
				}
			}
			return Pendiente;
		}

		public List<Pagos> LoadPagos(int IdPrestamo)
		{
			List<Pagos> pagos = new List<Pagos>();
			Conexion conexion = new Conexion();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT CantidadPagada, Fecha FROM PagosPrestamos WHERE IdPrestamo = @IdPrestamo";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							command.Parameters.Add(new SqlParameter("@IdPrestamo", IdPrestamo));
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									Pagos pago = new Pagos()
									{
										CantidadPagada = (decimal)reader["CantidadPagada"],
										Fecha = Convert.ToDateTime(reader["Fecha"])
									};
									pagos.Add(pago);
								}
								return pagos;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}
					conexion.CloseConnection(connection);
				}
				return pagos;
			}
		}
		public class Pagos
		{
			public decimal CantidadPagada { get; set; }
			public DateTime Fecha { get; set; }
		}
	}
}
