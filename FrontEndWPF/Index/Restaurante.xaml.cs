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

namespace FrontEndWPF.Index
{
	/// <summary>
	/// Lógica de interacción para Restaurante.xaml
	/// </summary>
	public partial class Restaurante : UserControl
	{
		private string Fnombre;
		private string Fced;
		private int Ftelefono;
		private string Fmensaje;
		private string Fcorreo;
		private string Fdireccion;

		public Restaurante()
		{
			InitializeComponent();
			FileRead();
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			SaveConfigToFile(nombre.Text, cedula.Text, Convert.ToInt32(telefono.Text), correo.Text, mensaje.Text, direccion.Text);
		}

		private void SaveConfigToFile(string nombre, string ced, int telefono, string correo, string mensaje,string direccion)
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Crear una carpeta específica para tu aplicación
			string appFolder = System.IO.Path.Combine(appDataPath, "YourAppName");
			if (!Directory.Exists(appFolder))
			{
				Directory.CreateDirectory(appFolder);
			}

			// Especificar la ruta completa del archivo
			string filePath = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "rest_config.txt");

			string content = $"Nombre: {nombre}\n" +
							 $"Cedula: {ced}\n" +
							 $"Telefono: {telefono}\n" +
							 $"Correo: {correo}\n" +
							 $"Mensaje: {mensaje}\n" +
							 $"Direccion: {direccion}\n";
			try
			{
				File.WriteAllText(filePath, content);
				MessageBox.Show($"Configuración guardada exitosamente en el archivo!\nRuta: {filePath}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al guardar la configuración en el archivo: {ex.Message}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/rest_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/rest_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "Nombre":
								Fnombre = parts[1];
								break;
							case "Cedula":
								Fced = parts[1];
								break;
							case "Telefono":
								Ftelefono = Convert.ToInt32(parts[1]);
								break;
							case "Correo":
								Fcorreo = parts[1];
								break;
							case "Mensaje":
								Fmensaje = parts[1];
								break;
							case "Direccion":
								Fdireccion = parts[1];
								break;
						}
					}
				}
				nombre.Text = Fnombre;
				cedula.Text = Fced;
				telefono.Text = Ftelefono.ToString();
				correo.Text = Fcorreo;
				mensaje.Text = Fmensaje;
				direccion.Text = Fdireccion;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}

		
	}
}
