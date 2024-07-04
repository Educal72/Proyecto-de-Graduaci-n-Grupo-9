using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using static FrontEndWPF.PuntoVenta;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FrontEndWPF.Services;
using System.Globalization;
using FrontEndWPF.ViewModel;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para Facturacion.xaml
	/// </summary>
	public partial class Facturacion : Page
	{
		double Subtotal;
		double Total;
		private DispatcherTimer timer;
        private const double ServicioPorcentaje = 0.10;
        private const double ImpuestoPorcentaje = 0.13;
        private FacturacionViewModel viewModel;

        public Facturacion(int orderId)
		{
			InitializeComponent();
            viewModel = new FacturacionViewModel();
            this.DataContext = viewModel;
            viewModel.LoadProductosCarrito(orderId);

            timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			//LoadCarritoItems();
			pagado.Focus();

            LoadSelectedOrder();
            CalculateTotals();
        }
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		    private void LoadSelectedOrder()
    {
        var selectedOrder = SelectedOrderService.SelectedOrder;
        if (selectedOrder != null)
        {
            carrito.ItemsSource = selectedOrder.Productos;
        }
    }

    private void CalculateTotals()
    {
            var productos = carrito.ItemsSource as IEnumerable<Producto>;
            if (productos != null)
            {
                double subtotal = (double)productos.Sum(p => p.Cantidad * p.Precio);
                subtotalTextBox.Text = subtotal.ToString("C");
                servicioTextBox.Text = (subtotal * ServicioPorcentaje).ToString("C");
                impuestoTextBox.Text = (subtotal * ImpuestoPorcentaje).ToString("C");
                UpdateTotal();
            }
        }

    private void descuentoTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        double subtotal = double.Parse(subtotalTextBox.Text, NumberStyles.Currency);
        double servicio = double.Parse(servicioTextBox.Text, NumberStyles.Currency);
        double impuesto = double.Parse(impuestoTextBox.Text, NumberStyles.Currency);
        double descuento = 0;

        if (double.TryParse(descuentoTextBox.Text, out double parsedDescuento))
        {
            descuento = parsedDescuento;
        }

        double total = subtotal + servicio + impuesto - descuento;
        totalTextBox.Text = total.ToString("C");
    }
		//private void LoadCarritoItems()
		//{
		//	var cartItem = new carritoItem
		//	{
		//		Nombre = "Pizza",
		//		Precio = 2100.00,
		//		Cantidad = 2
		//	};
		//	var cartItem2 = new carritoItem
		//	{
		//		Nombre = "Hamburguesa",
		//		Precio = 2900.00,
		//		Cantidad = 1
		//	};
		//	var cartItem3 = new carritoItem
		//	{
		//		Nombre = "Ensalada",
		//		Precio = 1200.00,
		//		Cantidad = 3
		//	};
		//	carrito.Items.Add(cartItem);
		//	carrito.Items.Add(cartItem2);
		//	carrito.Items.Add(cartItem3);
		//	Subtotal = (cartItem.Cantidad*cartItem.Precio) + (cartItem2.Cantidad * cartItem2.Precio) + (cartItem3.Cantidad * cartItem3.Precio);
		//	subtotal.Text = Subtotal.ToString();
		//	impuesto.Text = "13";
		//	descuento.Text = "0";
		//	servicio.Text = "10";
		//	Total = Subtotal + Subtotal * 0.10;
		//	total.Text = Total.ToString();
		//}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new ordenesListado());
			}
		}

		public class carritoItem
		{
			public string Nombre { get; set; }
			public double Precio { get; set; }
			public int Cantidad { get; set; }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			pagado.Text = totalTextBox.ToString();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var clienteAsociado = new clienteAsociar();
			clienteAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			clienteAsociado.WindowState = WindowState.Maximized;
			if (clienteAsociado.ShowDialog() == true)
			{
				string NombreCompleto = clienteAsociado.NombreCompleto;
				cliente.Text = NombreCompleto;
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var saloneroAsociado = new saloneroAsociar();
			saloneroAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (saloneroAsociado.ShowDialog() == true)
			{
				string NombreCompleto = saloneroAsociado.NombreCompleto;
				salonero.Text = NombreCompleto;
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			if (!double.TryParse(pagado.Text, out double result))
			{
				MessageBox.Show("Por favor, introduzca una cantidad pagada valida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				resultadoFacturacion resultado = new resultadoFacturacion(totalTextBox.Text, pagado.Text);
				resultado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				resultado.Show();

			}
		}
	}
}
