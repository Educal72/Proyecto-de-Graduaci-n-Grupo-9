using FrontEndWPF.Index;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class BaseDeDatosPrimera : Window
    {
		public string Servidor {  get; set; }
		public string Puerto { get; set; }
		public string NombreBD { get; set; }
		public string Usuario { get; set; }
		public string Contraseña { get; set; }

		private string Fserver;
		private string Fpuerto;
		private string Fnombre;
		private string Fusuario;
		private string Fcontraseña;

		public BaseDeDatosPrimera()
        {
            InitializeComponent();
			FileRead();
        }

		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "Server":
								Fserver = parts[1];
								break;
							case "Puerto":
								Fpuerto = parts[1];
								break;
							case "Nombre":
								Fnombre = parts[1];
								break;
							case "Usuario":
								Fusuario = parts[1];
								break;
							case "Contraseña":
								Fcontraseña = parts[1];
								break;
						}
					}
				}
				servidor.Text = Fserver;
				puerto.Text = Fpuerto;
				nombre.Text = Fnombre;
				usuario.Text = Fusuario;
				contraseña.Text = Fcontraseña;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
}

		private bool ValidateInputs(string server, string puerto, string nombre, string usuario, string contraseña, out string errorMessage)
		{
			errorMessage = string.Empty;

			if (string.IsNullOrWhiteSpace(server))
			{
				errorMessage = "El campo Servidor es obligatorio.";
				return false;
			}

			if (string.IsNullOrWhiteSpace(puerto))
			{
				errorMessage = "El campo Puerto es obligatorio.";
				return false;
			}

			if (string.IsNullOrWhiteSpace(nombre))
			{
				errorMessage = "El campo Nombre de la base de datos es obligatorio.";
				return false;
			}

			if (string.IsNullOrWhiteSpace(usuario))
			{
				errorMessage = "El campo Usuario es obligatorio.";
				return false;
			}

			if (string.IsNullOrWhiteSpace(contraseña))
			{
				errorMessage = "El campo Contraseña es obligatorio.";
				return false;
			}

			// Todas las validaciones pasaron
			return true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateInputs(servidor.Text, puerto.Text, nombre.Text, usuario.Text, contraseña.Text, out string errorMessage))
			{
				MessageBox.Show(errorMessage);
				return;
			}
			Servidor = servidor.Text;
			Puerto = puerto.Text;
			NombreBD = nombre.Text;
			Usuario = usuario.Text;
			Contraseña = contraseña.Text;
			DialogResult = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var connectionString = $"Server={servidor.Text};Database={nombre.Text};User Id={usuario.Text};Password={contraseña.Text};";
			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				connection.Open();
				MessageBox.Show("Conexión Exitosa.");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error en la Conexión: " + ex.Message);
				
			}
		}
	}
}
