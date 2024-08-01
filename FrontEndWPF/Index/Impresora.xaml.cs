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
	public partial class Impresora : UserControl
	{
		private string NombreImpresora;
		private string DescargarComo;
		private string CodigoDeControl;
		private decimal BalanceInicial;

		public Impresora()
		{
			InitializeComponent();
			FileRead();
		}
		

		private void SaveConfigToFile(string NombreImpresora, string DescargarComo, string CodigoDeControl, decimal BalanceInicial)
		{
			string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Crear una carpeta específica para tu aplicación
			string appFolder = System.IO.Path.Combine(appDataPath, "YourAppName");
			if (!Directory.Exists(appFolder))
			{
				Directory.CreateDirectory(appFolder);
			}

			// Especificar la ruta completa del archivo
			string filePath = System.IO.Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "impresora_config.txt");

			string content = $"NombreImpresora: {NombreImpresora}\n" +
							 $"DescargarComo: {DescargarComo}\n" +
							 $"CodigoDeControl: {CodigoDeControl}\n" +
							 $"BalanceInicial: {BalanceInicial}\n";
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
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/impresora_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/impresora_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "NombreImpresora":
								NombreImpresora = parts[1];
								break;
							case "DescargarComo":
								DescargarComo = parts[1];
								break;
							case "CodigoDeControl":
								CodigoDeControl = parts[1];
								break;
							case "BalanceInicial":
								BalanceInicial = Convert.ToDecimal(parts[1]);
								break;
						}
					}
				}

				nombre.Text = NombreImpresora;
				descargar.Text = DescargarComo;
				codigo.Text = CodigoDeControl;
				balanceinicial.Text = BalanceInicial.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			SaveConfigToFile(nombre.Text.ToString(), descargar.Text.ToString(), codigo.Text.ToString(), Convert.ToDecimal(balanceinicial.Text));
		}
	}
}
