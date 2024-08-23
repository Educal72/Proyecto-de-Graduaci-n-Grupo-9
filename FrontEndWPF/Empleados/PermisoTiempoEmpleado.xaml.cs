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
using PermisoDeTiempo = FrontEndWPF.Modelos.PermisoDeTiempo;

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para PermisoTiempoEmpleado.xaml
	/// </summary>
	public partial class PermisoTiempoEmpleado : UserControl
	{
		Conexion conexion = new Conexion();
		PermisoDeTiempoViewModel viewModel = new PermisoDeTiempoViewModel();
		public ObservableCollection<PermisoDeTiempo> PermisoTiempo { get; set; }
		public PermisoTiempoEmpleado()
		{
			InitializeComponent();
			PermisoTiempo = new ObservableCollection<PermisoDeTiempo>();
			DataContext = this;
			LoadPermisosDeTiempo();
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

		private void LoadPermisosDeTiempo()
		{
			PermisoTiempo.Clear();
			Conexion conexion = new Conexion();
			var permisosList = conexion.GetPermisosDeTiempoEspecifico(getIdEmpleadoFromIdUsuario());

			foreach (var permisoDict in permisosList)
			{
				PermisoDeTiempo permiso = new PermisoDeTiempo
				{
					Id = (int)permisoDict["Id"],
					IdEmpleado = (int)permisoDict["IdEmpleado"],
					NombreCompleto = GetUserByEmpleadoId((int)permisoDict["IdEmpleado"]),
					FechaInicio = (DateTime)permisoDict["FechaInicio"],
					FechaFin = (DateTime)permisoDict["FechaFin"],
					Motivo = (string)permisoDict["Motivo"],
					Estado = permisoDict["Estado"].ToString() // Asegurarse de que Estado se maneje como string
				};
				PermisoTiempo.Add(permiso);
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
					PermisoDeTiempo faq = button.DataContext as PermisoDeTiempo;
					if (faq != null)
					{
						viewModel.EliminarPermisoDeTiempo(faq.Id);
						LoadPermisosDeTiempo();
					}
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var permisoTiempoCrear = new PermisoTiempoCrear();
			permisoTiempoCrear.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (permisoTiempoCrear.ShowDialog() == true)
			{
				MessageBox.Show("Permiso creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
				LoadPermisosDeTiempo();
			}
		}
	}
}
