using FrontEndWPF.Modelos;
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
using Xceed.Wpf.Toolkit.Primitives;
using static FrontEndWPF.empleadosAdmin;
using static FrontEndWPF.Modelos.UserModel;


namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para PermisoTiempoEmpleado.xaml
	/// </summary>
	public partial class DescvinculacionesEmpleado : UserControl
	{
		Conexion conexion = new Conexion();
		public ObservableCollection<EmpleadoDesvinculacion> PermisoTiempo { get; set; }
		public ConexionEmpleado conexionEmpleado = new ConexionEmpleado();
		public DescvinculacionesEmpleado()
		{
			InitializeComponent();
			PermisoTiempo = new ObservableCollection<EmpleadoDesvinculacion>();
			DataContext = this;
			LlenarDataGrid(getIdEmpleadoFromIdUsuario());
		}


		public int getIdEmpleadoFromIdUsuario()
		{

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "SELECT Id FROM Empleado WHERE IdUsuario = @IdUsuario";
					int IdUsuario;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@IdUsuario", SesionUsuario.Instance.id);
						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								IdUsuario = Convert.ToInt32(result);
								return IdUsuario;
							}
							else
							{
								Console.WriteLine("Empelado no encontrado.");
								return 0;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error: " + ex.Message);
							return 0;
						}
					}
				}
				else
				{
					return 0;

				}
			}
		}
		private void LlenarDataGrid(int id)
		{
			string query = @"SELECT FechaInicio, FechaSalida, Motivo, Comentarios, Reconocido FROM Desvinculacion WHERE IdEmpleado = @IdEmpleado";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						EmpleadoDesvinculacion metricas = new EmpleadoDesvinculacion()
						{
							FechaInicio = (DateTime)reader["FechaInicio"],
							FechaSalida = (DateTime)reader["FechaSalida"],
							Motivo = reader["Motivo"].ToString(),
							Comentarios = reader["Comentarios"].ToString(),
							Reconocido = (bool)reader["Reconocido"]
						};
						PermisoTiempo.Add(metricas);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			
			
		}
		public string GetUserByEmpleadoId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    Usuario.Nombre, 
    Usuario.Apellido
FROM 
    Usuario
JOIN 
    Empleado ON Usuario.Id = Empleado.IdUsuario
WHERE 
    Empleado.Id = @IdEmpleado";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Está seguro de eliminar el permiso de tiempo?",
					"¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{
					EmpleadoDesvinculacion faq = button.DataContext as EmpleadoDesvinculacion;
					if (faq != null)
					{
						conexionEmpleado.DeleteDesvinculaciones(faq.Id);
						LlenarDataGrid(getIdEmpleadoFromIdUsuario());
					}
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

			var nuevaDesvinculacion = new añadirDesvinculaciones();
			nuevaDesvinculacion.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			if (nuevaDesvinculacion.ShowDialog() == true)
			{
				string cedula = nuevaDesvinculacion.cedula_añadirDesvinculaciones;
				DateTime FechaInicio = nuevaDesvinculacion.fechaInicio_añadirDesvinculaciones;
				string Motivo = nuevaDesvinculacion.motivo_añadirDesvinculaciones!;

				string Comentarios = nuevaDesvinculacion.comentarios_añadirDesvinculaciones!;
				DateTime FechaSalida = nuevaDesvinculacion.fechaSalida_añadirDesvinculaciones;


				/* Si todo salio bien en el proceso del método encargado de la BD, entonces -
				 * mostrara el mensaje: "La solicitud de desvinculación del empleado -
				 * ha sido agregado exitosamente."*/
				conexionEmpleado.AddDesvinculacion(cedula, FechaInicio, Motivo, Comentarios, FechaSalida);
				MessageBox.Show("Permiso creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
				LlenarDataGrid(getIdEmpleadoFromIdUsuario());
			}
		}
	}
}
