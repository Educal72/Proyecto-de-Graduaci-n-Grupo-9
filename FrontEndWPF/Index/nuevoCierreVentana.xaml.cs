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
    /// Lógica de interacción para nuevoItemVentana.xaml
    /// </summary>
    public partial class nuevoCierreVentana : Window
    {
		public decimal Precio { get; set; }

		public nuevoCierreVentana()
        {
            InitializeComponent();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!double.TryParse(precioItem.Text, out double result))
			{
				MessageBox.Show("Por favor, introduzca un número válido.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else {
				Precio = decimal.Parse(precioItem.Text);
				DialogResult = true;
			}
			
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
