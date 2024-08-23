using FrontEndWPF.ViewModel;
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
using System.Windows.Shapes;

namespace FrontEndWPF.Index
{
	/// <summary>
	/// Lógica de interacción para CodigoBarras.xaml
	/// </summary>
	public partial class CodigoBarras : Window
	{
		private StringBuilder barcode = new StringBuilder();
		private FichajesViewModel fichajesViewModel;

		public CodigoBarras(FichajesViewModel fichajesViewModel)
		{
			InitializeComponent();
			this.fichajesViewModel = fichajesViewModel;
			this.KeyDown += BarcodeWindow_KeyDown; // Asociar el evento KeyDown
		}

		private void BarcodeWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				string scannedCode = barcode.ToString().Trim();
				barcode.Clear();

				try
				{
					fichajesViewModel.CrearFichaje(Convert.ToInt32(scannedCode));
					MessageBox.Show("Fichaje creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
					this.Close(); // Cierra la ventana cuando se ha realizado el fichaje
				}
				catch (Exception ex)
				{
					MessageBox.Show("Cédula en formato inválido o de empleado inexistente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					this.Close(); // Cierra la ventana en caso de error
				}
			}
			else
			{
				// Acumular el código escaneado
				barcode.Append(e.Key.ToString().Replace("D", ""));
			}
		}

		private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.Close(); // Cierra la ventana si se presiona "Cancelar"
		}
	}
}
