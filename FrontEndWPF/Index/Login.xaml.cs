using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;
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
			if (conexion.HasEntries() || !File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt"))
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
			var con = conexion.SelectUser(correo.Text, conexion.HashPassword(cont.Password));
			if (con.Count() > 0)
			{
				MainWindow mainWindow = new MainWindow();
				mainWindow.Usuario = con["Correo"].ToString();
				if (con != null ) {
					SesionUsuario.Instance.correo = con["Correo"].ToString();
					conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
					SesionUsuario.Instance.rol = conexion.getRoleName(Convert.ToInt32(con["IdRol"]));
				}
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
		private void SaveConfigToFile(string server, string puerto, string nombre, string usuario, string contraseña)
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Crear una carpeta específica para tu aplicación
			string appFolder = System.IO.Path.Combine(appDataPath, "YourAppName");
			if (!Directory.Exists(appFolder))
			{
				Directory.CreateDirectory(appFolder);
			}

			// Especificar la ruta completa del archivo
			string filePath = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "db_config.txt");

			string content = $"Server: {server}\n" +
							 $"Puerto: {puerto}\n" +
							 $"Nombre: {nombre}\n" +
							 $"Usuario: {usuario}\n" +
							 $"Contraseña: {contraseña}\n";

			try
			{
				File.WriteAllText(filePath, content);
				MessageBox.Show($"Configuración guardada exitosamente en el archivo!\nRuta: {filePath}");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al guardar la configuración en el archivo: {ex.Message}");
			}
		}

		private void BDButton_Click(object sender, RoutedEventArgs e)
		{
			var conexionBd = new BaseDeDatosPrimera();
			conexionBd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (conexionBd.ShowDialog() == true)
			{

				string Servidor = conexionBd.Servidor;
				string Puerto = conexionBd.Puerto;
				string NombreBD = conexionBd.NombreBD;
				string Usuario = conexionBd.Usuario;
				string Contraseña = conexionBd.Contraseña;
				SaveConfigToFile(Servidor, Puerto, NombreBD, Usuario, Contraseña);
				NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
			}
		}
	}
}

