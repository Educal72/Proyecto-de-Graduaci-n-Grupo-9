using DocumentFormat.OpenXml.Spreadsheet;
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
using static ClosedXML.Excel.XLPredefinedFormat;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.Index
{
	/// <summary>
	/// Lógica de interacción para Restaurante.xaml
	/// </summary>
	public partial class Impuesto : UserControl
	{
		private string Fservicio;
		private string Fiva;
		public Impuesto()
		{
			InitializeComponent();
			FileRead();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!int.TryParse(iva.Text, out int result))
			{
				MessageBox.Show("Por favor, introduzca un porcentaje IVA válido Ej. 13.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (!int.TryParse(servicio.Text, out int result2))
			{
				MessageBox.Show("Por favor, introduzca un porcentaje de Servicio válido Ej. 10.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				SaveConfigToFile(Convert.ToInt32(iva.Text), Convert.ToInt32(servicio.Text));
			}
			
		}

		private void SaveConfigToFile(int IVA, int servicio)
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Crear una carpeta específica para tu aplicación
			string appFolder = System.IO.Path.Combine(appDataPath, "YourAppName");
			if (!Directory.Exists(appFolder))
			{
				Directory.CreateDirectory(appFolder);
			}

			// Especificar la ruta completa del archivo
			string filePath = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "imp_config.txt");

			string content = $"IVA: {IVA}\n" +
							 $"Servicio: {servicio}\n";
			try
			{
				File.WriteAllText(filePath, content);
				MessageBox.Show($"Configuración guardada exitosamente", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al guardar la configuración en el archivo: {ex.Message}", "Resultado", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/imp_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/imp_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "IVA":
								Fiva = parts[1];
								break;
							case "Servicio":
								Fservicio = parts[1];
								break;
						}
					}
				}
				iva.Text = Fiva;
				servicio.Text = Fservicio;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}
	}
}
