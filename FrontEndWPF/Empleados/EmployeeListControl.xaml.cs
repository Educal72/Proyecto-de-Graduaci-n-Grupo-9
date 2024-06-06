using FrontEndWPF.Empleados;
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
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para EmployeeListControl.xaml
	/// </summary>
	public partial class EmployeeListControl : UserControl
	{
		List<Empleado> employees = new List<Empleado>();
		Conexion conexion = new Conexion();

		public EmployeeListControl()
		{
			InitializeComponent();
			conexion.OpenConnection();
			LoadData();
		}

		private void LoadData()
		{
			
			string query = @"
                SELECT u.Nombre, u.PrimerApellido, u.SegundoApellido, u.Cedula, u.Telefono, u.Correo, u.Contraseña, u.IdRol, u.FechaCreacion, e.Puesto, e.Salario
                FROM Usuario u
                JOIN Empleado e ON u.Id = e.IdUsuario";

			List<UsuarioEmpleado> usuariosEmpleados = new List<UsuarioEmpleado>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						usuariosEmpleados.Add(new UsuarioEmpleado
						{
							Nombre = reader["Nombre"].ToString(),
							PrimerApellido = reader["PrimerApellido"].ToString(),
							SegundoApellido = reader["SegundoApellido"].ToString(),
							Cedula = reader["Cedula"].ToString(),
							Telefono = reader["Telefono"].ToString(),
							Correo = reader["Correo"].ToString(),
							Contraseña = reader["Contraseña"].ToString(),
							IdRol = Convert.ToInt32(reader["IdRol"]),
							FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
							Puesto = reader["Puesto"].ToString(),
							Salario = Convert.ToDecimal(reader["Salario"])
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			EmployeeDataGrid.ItemsSource = usuariosEmpleados;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevoEmpleado = new añadirEmpleado();
			nuevoEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoEmpleado.ShowDialog() == true)
			{

				string Cedula = nuevoEmpleado.cedula;
				string Nombre = nuevoEmpleado.nombre;
				string Apellidos = nuevoEmpleado.apellidos;
				string Puesto = nuevoEmpleado.puesto;
				DateTime Fecha = nuevoEmpleado.fechaContratación;
				double Salario = nuevoEmpleado.salario;
				string Correo = nuevoEmpleado.correo;
				string Contraseña = nuevoEmpleado.contraseña;
				string Telefono = nuevoEmpleado.telefono;
				string Rol = nuevoEmpleado.rol;

				conexion.AddUser(Nombre, Apellidos, Apellidos, Cedula, Telefono, Correo, Contraseña, Rol, Fecha, Puesto, Salario);
				LoadData();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				employees.Remove(selectedEmpleado);
				EmployeeDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				var editarEmpleado = new editarEmpleado();
				editarEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				editarEmpleado.Cedula.Text = selectedEmpleado.Cedula;
				editarEmpleado.Nombre.Text = selectedEmpleado.Nombre;
				editarEmpleado.Apellidos.Text = selectedEmpleado.Apellidos;
				editarEmpleado.Puesto.Text = selectedEmpleado.Puesto;
				editarEmpleado.Fecha.SelectedDate = selectedEmpleado.FechaContratacion;
				editarEmpleado.Salario.Text = selectedEmpleado.Salario.ToString();
				editarEmpleado.Correo.Text = selectedEmpleado.CorreoElectronico;
				editarEmpleado.Contraseña.Text = selectedEmpleado.Contraseña;
				editarEmpleado.Telefono.Text = selectedEmpleado.Telefono;
				editarEmpleado.Activo.IsChecked = selectedEmpleado.Activo;
				editarEmpleado.Rol.Text = selectedEmpleado.Rol;
				if (editarEmpleado.ShowDialog() == true)
				{
					selectedEmpleado.Cedula = editarEmpleado.cedula;
					selectedEmpleado.Nombre = editarEmpleado.nombre;
					selectedEmpleado.Apellidos = editarEmpleado.apellidos;
					selectedEmpleado.Puesto = editarEmpleado.puesto;
					selectedEmpleado.FechaContratacion = editarEmpleado.fechaContratación;
					selectedEmpleado.Salario = editarEmpleado.salario;
					selectedEmpleado.CorreoElectronico = editarEmpleado.correo;
					selectedEmpleado.Contraseña = editarEmpleado.contraseña;
					selectedEmpleado.Telefono = editarEmpleado.telefono;
					selectedEmpleado.Activo = editarEmpleado.Activo.IsChecked ?? false;
					selectedEmpleado.Rol = editarEmpleado.rol;
					EmployeeDataGrid.Items.Refresh();
				}
			}
		}


		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
				if (mainWindow != null)
				{
					mainWindow.ChangePageToMetricas();
				}

			}
		}
	}
	}

