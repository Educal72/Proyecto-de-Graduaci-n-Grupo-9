using FrontEndWPF.Reporteria;
using FrontEndWPF.ViewModel;
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
using System.Windows.Shapes;
using static FrontEndWPF.Reporteria.VisualizarPrestamos;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class verHistorialPagos : Window
    {

		Conexion conexion = new Conexion();
		VisualizarPrestamos visualizarPrestamos = new VisualizarPrestamos();
		public int IdPrestamo_verHistorialPagos { get; set; }
		public Reporteria.VisualizarPrestamos.Prestamo Prestamo_verHistorialPagos { get; set; }
		public verHistorialPagos()
        {
            InitializeComponent();
			PopulateFiltroComboBox();
			DataContext = this;
		}

		private void PopulateFiltroComboBox()
		{
			string query = @"SELECT DISTINCT u.Nombre, u.Apellido, u.Cedula
							FROM Prestamos p
							JOIN Empleado e ON p.IdEmpleado = e.Id
							JOIN Usuario u ON e.IdUsuario = u.Id
							WHERE p.Estado = 'Activo';"
			;

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						string cedula = reader["Cedula"].ToString();
						string nombre = reader["Nombre"].ToString();
						string Apellido = reader["Apellido"].ToString();

						// Formatear la opción del ComboBox
						string displayText = $"{nombre} {Apellido} ({cedula})";

						// Crear un ComboBoxItem con el texto y agregarlo al ComboBox
						Usuarios_ComboBox.Items.Add(new ComboBoxItem
						{
							Content = displayText,
							Tag = cedula // Usar el Tag para almacenar la cédula asociada
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		public int GetEmpleadoIdByCedula(int cedula)
		{
			Conexion conexion = new Conexion();
			int IdEmpleado = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"
        SELECT e.Id
        FROM Usuario u
        JOIN Empleado e ON u.Id = e.IdUsuario
        WHERE u.Cedula = @Cedula";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							IdEmpleado = (int)reader["Id"];

							return IdEmpleado;
						}
					}
				}
			}
			return IdEmpleado;
		}

		private void Usuarios_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Descripcion_ComboBox.Items.Clear();
			Descripcion_ComboBox.IsEnabled = true;
			int empleadoId = 0;
			if (Usuarios_ComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
				empleadoId = GetEmpleadoIdByCedula(Convert.ToInt32(selectedItem.Tag));
			}
			string query = @"SELECT Id, Descripcion
                    FROM Prestamos
                    WHERE IdEmpleado = @IdEmpleado AND Estado = 'Activo';"
			;
			if (empleadoId != 0)
			{
				using (SqlConnection connection = conexion.OpenConnection())
				{
					try
					{
						SqlCommand command = new SqlCommand(query, connection);
						command.Parameters.Add(new SqlParameter("@IdEmpleado", empleadoId));
						SqlDataReader reader = command.ExecuteReader();

						while (reader.Read())
						{
							string descripcion = reader["Descripcion"].ToString();
							string id = reader["Id"].ToString();

							// Formatear la opción del ComboBox

							// Crear un ComboBoxItem con el texto y agregarlo al ComboBox
							Descripcion_ComboBox.Items.Add(new ComboBoxItem
							{
								Content = descripcion,
								Tag = id // Usar el Tag para almacenar la cédula asociada
							});
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error: " + ex.Message);
					}
				}
			}
			/* Método relacionado al ComboBox, este se encarga de poder guardar -
			 * los usuarios que estan registrados en el sistema.*/

		}

		private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			//Validar Descripcion:
			if (Descripcion_ComboBox.Text == "" || Descripcion_ComboBox.Text == null)
			{
				errorMessage = "El campo Descripcion es obligatorio.";
				return false;

			}

			//Validar Usuario:
			if (Usuarios_ComboBox.Text == "" || Usuarios_ComboBox.Text == null)
			{
				errorMessage = "El campo Usuario es obligatorio.";
				return false;

			}
			// Todas las validaciones realizadas
			return true;
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateInputs(out string errorMessage))
			{
				MessageBox.Show(errorMessage);
				return;
			}
			if (Descripcion_ComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
				IdPrestamo_verHistorialPagos = Convert.ToInt32(selectedItem.Tag);
				using (SqlConnection connection = conexion.OpenConnection())
				{
					if (connection != null)
					{
						string query = "SELECT Id, IdEmpleado, MontoTotal, Pendiente, Interes, Estado, FechaCreacion, Descripcion FROM Prestamos WHERE Id = @Id";
						using (SqlCommand command = new SqlCommand(query, connection))
						{
							try
							{
								command.Parameters.Add(new SqlParameter("@Id", selectedItem.Tag));
								using (SqlDataReader reader = command.ExecuteReader())
								{
									while (reader.Read())
									{
										Prestamo prestamo = new Prestamo()
										{
											Id = (int)reader["Id"],
											IdUsuario = (int)reader["IdEmpleado"],
											NombreUsuario = visualizarPrestamos.GetUserByEmpleadoId((int)reader["IdEmpleado"]),
											Descripcion = reader["Descripcion"].ToString(),
											MontoTotal = (decimal)reader["MontoTotal"],
											MontoPendiente = (decimal)reader["Pendiente"],
											InteresesMoratorios = (decimal)reader["Interes"],
											Estado = reader["Estado"].ToString(),
											FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
										};
										Prestamo_verHistorialPagos = prestamo;
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
			}
			DialogResult = true;
		}


	}
}
