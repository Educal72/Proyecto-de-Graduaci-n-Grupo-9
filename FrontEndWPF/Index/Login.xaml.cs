using System;
using System.Collections.Generic;
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
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para ExtraPagina.xaml
	/// </summary>
	public partial class Login : Page
	{
		Conexion conexion = new Conexion();
		public Login()
		{
			InitializeComponent();
			conexion.OpenConnection();
			if (conexion.HasEntries())
			{
				Opcion1.Content = "¿Olvidaste tu contraseña?";
			}
			else
			{
				Opcion1.Content = "Crear Usuario Admin";
			}
		}


		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var con = conexion.SelectUser(correo.Text, cont.Password);
			if (con.Count() > 0)
			{
				MainWindow mainWindow = new MainWindow();
				mainWindow.Usuario = con["Correo"].ToString();
				NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
			}
			else
			{
				MessageBox.Show("Usuario o Contraseña Incorrecta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (conexion.HasEntries())
			{
				NavigationService.Navigate(new Uri("Index/RecuperarContraseña.xaml", UriKind.Relative));
			}
			else
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
					NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
				}

			}
		}
	}
}

