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

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para resultadoFacturacion.xaml
	/// </summary>
	public partial class resultadoFacturacion : Window
	{
		public resultadoFacturacion(string cantidadPagar, string cantidadPagada)
		{
			InitializeComponent();
			pagado.Text = cantidadPagada;
			total.Text = cantidadPagar;
			vuelto.Text = (double.Parse(cantidadPagada) - double.Parse(cantidadPagar)).ToString();
			puntos.Text = (double.Parse(cantidadPagar) * 0.01).ToString();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
			if (mainWindow != null)
			{
				mainWindow.ChangePageToPuntoVenta();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
			if (mainWindow != null)
			{
				mainWindow.ChangePageToPuntoVenta();
			}
		}
	}
}
