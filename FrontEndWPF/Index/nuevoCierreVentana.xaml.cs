using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Molino_POS;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class nuevoCierreVentana : Window
    {
		public decimal Precio { get; set; }
		private static readonly HttpClient client = new HttpClient();
		public string Fservicio;
		public string Fiva;
		public decimal Fcambio;

		public nuevoCierreVentana()
        {
            InitializeComponent();
			FileRead();
			FetchDataButton_Click();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!double.TryParse(precioItem.Text, out double result))
			{
				MessageBox.Show("Por favor, introduzca un número válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (!decimal.TryParse(cambioItem.Text, out decimal result3))
			{
				MessageBox.Show("Por favor, introduzca un tipo de cambio válido Ej. 10.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				SaveConfigToFile(Convert.ToInt32(Fiva), Convert.ToInt32(Fservicio), Convert.ToDecimal(cambioItem.Text));
				Precio = decimal.Parse(precioItem.Text);
				DialogResult = true;
			}
			
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private async void FetchDataButton_Click()
		{
			ExtractorTipoCambioC service = new ExtractorTipoCambioC();
			var tipoCambio = await service.ObtenerTipoDeCambioConAngleSharpAsync();
			cambioItem.Text = tipoCambio.ToString();
		}
		
		private void SaveConfigToFile(int IVA, int servicio, decimal tipoCambio)
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
							 $"Servicio: {servicio}\n" +
							 $"Tipo Cambio: {tipoCambio}\n";
			try
			{
				File.WriteAllText(filePath, content);
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
							case "Tipo Cambio":
								Fcambio = Convert.ToDecimal(parts[1]);
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}
	}
}
