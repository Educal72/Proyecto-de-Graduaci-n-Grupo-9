using System;
using System.Collections.Generic;
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
using static FrontEndWPF.empleadosAdmin;
using static FrontEndWPF.ViewModel.FAQViewModel;

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para Metricas.xaml
	/// </summary>
	public partial class Metricas : UserControl
	{
		Conexion conexion = new Conexion();

		public Metricas(int id)
		{
			InitializeComponent();
			LlenarDataGrid(id);
			LlenarInfo(id);
		}

		private void LlenarInfo(int id)
		{
			var IdEmpleado = GetIdEmpleado(id);
			List<DatosEmpleado> metricas = new List<DatosEmpleado>();
			string query2 = @"SELECT COUNT(*) FROM Fichajes WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query2, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					int count = (int)command.ExecuteScalar();
					total.Text = count.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query3 = @"SELECT COUNT(*) FROM Incidentes WHERE IdUsuario = @IdUsuario";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query3, connection);
					command.Parameters.Add(new SqlParameter("@IdUsuario", id));
					int count2 = (int)command.ExecuteScalar();
					puntos.Text = count2.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query4 = @"SELECT COUNT(*) FROM Factura WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query4, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					int count3 = (int)command.ExecuteScalar();
					vuelto.Text = count3.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query5 = @"
                SELECT AVG(CAST(DailyCount AS FLOAT)) AS AverageFacturasPerDay
                FROM (
                    SELECT COUNT(*) AS DailyCount
                    FROM Factura
                    WHERE IdEmpleado = @IdEmpleado
                    GROUP BY CAST(FechaCreacion AS DATE)
                ) AS DailyCounts";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query5, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					object result = command.ExecuteScalar();
					double averageFacturasPerDay = result != DBNull.Value ? Convert.ToDouble(result) : 0.0;
					pagado.Text = averageFacturasPerDay + " Mesas x Dia";

				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		private void LlenarDataGrid(int id)
		{
			var IdEmpleado = GetIdEmpleado(id);
			List<DatosEmpleado> metricas = new List<DatosEmpleado>();
			string query = @"SELECT FechaSalida, Motivo FROM Desvinculacion WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						metricas.Add(new DatosEmpleado()
						{
							Tipo = "Desvinculación",
							Fecha = (DateTime)reader["FechaSalida"],
							Descripcion = reader["Motivo"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query2 = @"SELECT FechaHora, Tipo FROM Fichajes WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query2, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						metricas.Add(new DatosEmpleado()
						{
							Tipo = "Fichaje",
							Fecha = (DateTime)reader["FechaHora"],
							Descripcion = reader["Tipo"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query3 = @"SELECT Fecha, Descripcion FROM Incidentes WHERE IdUsuario = @IdUsuario";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query3, connection);
					command.Parameters.Add(new SqlParameter("@IdUsuario", id));
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						metricas.Add(new DatosEmpleado()
						{
							Tipo = "Incidente",
							Fecha = (DateTime)reader["Fecha"],
							Descripcion = reader["Descripcion"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			string query4 = @"SELECT FechaInicio, Motivo FROM PermisosDeAusencia WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query4, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						metricas.Add(new DatosEmpleado()
						{
							Tipo = "Permiso de Ausencia",
							Fecha = (DateTime)reader["FechaInicio"],
							Descripcion = reader["Motivo"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			string query5 = @"SELECT FechaInicio, Motivo FROM PermisosDeTiempo WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query5, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						metricas.Add(new DatosEmpleado()
						{
							Tipo = "Permiso de Tiempo",
							Fecha = (DateTime)reader["FechaInicio"],
							Descripcion = reader["Motivo"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			dataGrid.ItemsSource = metricas;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var navigationService = NavigationService.GetNavigationService(this);

			// Verificar si el servicio de navegación es nulo y si podemos volver atrás
			if (navigationService != null && navigationService.CanGoBack)
			{
				// Volver a la página anterior
				navigationService.GoBack();
			}
		}
		public int GetIdEmpleado(int id)
		{
			int result = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT Id FROM Empleado WHERE IdUsuario = @IdUsuario";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@IdUsuario", id);

						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									result = (int)reader["Id"];
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

			return result;
		}
	}

	public class DatosEmpleado {
		public string Tipo { get; set; }
		public DateTime Fecha { get; set; }
		public string Descripcion { get; set; }
	}

	
}

