using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para Facturacion.xaml
	/// </summary>
	public partial class Facturacion : Page
	{
		decimal Subtotal;
		decimal Total;
		private DispatcherTimer timer;
		OrdenesViewModel Orden = new OrdenesViewModel();
		FacturaViewModel Factura = new FacturaViewModel();
		private decimal puntosDisponibles = 0;
		private bool isAsociado = false;

		private int IdUsuario;
		private int IdCliente;
		int idOrden { get; set; }
		private string Fservicio;
		private string Fiva;
		private string tipoVenta;
		private string metodoPago;
		private string cajero = SesionUsuario.Instance.nombre;
		private decimal puntosGanados;

		public Facturacion(int idOrden)
		{
			this.idOrden = idOrden;
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			LoadCarritoItems();
			pagado.Focus();
			FileRead();
			RestauranteButton.IsChecked = true;
			tipoVenta = "Restaurante";
			EfectivoButton.IsChecked = true;
			metodoPago = "Efectivo";
			IdCliente = 0;
			IdUsuario = SesionUsuario.Instance.id;
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
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
				impuesto.Text = Fiva;
				servicio.Text = Fservicio;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}

		private void LoadCarritoItems()
		{
			var orden = Orden.GetOrdenConProductosById(idOrden);
			if (orden != null && orden.ContainsKey("Productos")) {
				var productos = orden["Productos"] as List<OrdenesViewModel.ProductosOrden>;
				if (productos != null)
				{
					foreach (var producto in productos)
					{
						var cartItem = new carritoItem
						{
							Nombre = producto.Nombre,
							Precio = producto.Precio,
							Cantidad = producto.Cantidad
						};
						carrito.Items.Add(cartItem);
						Subtotal = Subtotal + (producto.Cantidad * producto.Precio);

					}
				}
			}
				subtotal.Text = Subtotal.ToString();
				descuento.Text = puntosDisponibles.ToString();
				Total = Subtotal + Subtotal * 0.10m;
			    Total = (decimal)System.Math.Round(Total, 2);
				total.Text = Total.ToString();
			} 

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
			public decimal Precio { get; set; }
			public int Cantidad { get; set; }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			pagado.Text = Total.ToString();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var clienteAsociado = new clienteAsociar();
			clienteAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			clienteAsociado.WindowState = WindowState.Maximized;
			if (clienteAsociado.ShowDialog() == true)
			{
				string NombreCompleto = clienteAsociado.NombreCompleto;
				IdCliente = clienteAsociado.Cedula;
				cliente.Text = NombreCompleto;

				if (clienteAsociado.isAsociado) {
					puntosDisponibles = clienteAsociado.puntosDisponibles;
					isAsociado = clienteAsociado.isAsociado;
				}
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var saloneroAsociado = new saloneroAsociar();
			saloneroAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (saloneroAsociado.ShowDialog() == true)
			{
				string NombreCompleto = saloneroAsociado.NombreCompleto;
				IdUsuario = saloneroAsociado.IdUsuario;
				salonero.Text = NombreCompleto;
				puntosGanados = Total * 0.01m;
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			if (!double.TryParse(pagado.Text, out double result) || Convert.ToDecimal(pagado.Text.ToString()) < Total)
			{
				MessageBox.Show("Por favor, introduzca una cantidad pagada valida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				var orden = Orden.GetOrdenById(idOrden);
				bool resultadoCrear = Factura.CrearFactura(idOrden, Convert.ToDecimal(pagado.Text), Convert.ToInt32(Fiva), Convert.ToInt32(Fservicio), orden.Creacion, DateTime.Now, cajero, IdUsuario, IdCliente, Convert.ToDecimal(descuento.Text), puntosGanados, metodoPago, tipoVenta, Total);
                if (resultadoCrear)
                {
					Factura.TerminarOrden(idOrden);
                }
                resultadoFacturacion resultado = new resultadoFacturacion(total.Text, pagado.Text);
				resultado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				resultado.Show();

			}
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			var checkedButton = sender as ToggleButton;

			if (UberEatsButton == checkedButton)
			{
				tipoVenta = "Uber Eats";
			}
			else if (PedidosYaButton == checkedButton)
			{
				tipoVenta = "PedidosYa";
			}
			else if (RestauranteButton == checkedButton)
			{
				tipoVenta = "Restaurante";
			}
			else {
				tipoVenta = "Para Llevar";
			}
		}

		private void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
		{
			var checkedButton = sender as ToggleButton;

			if (EfectivoButton == checkedButton)
			{
				metodoPago = "Efectivo";
			}
			else
			{
				metodoPago = "Tarjeta";
			}
			
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			if (!isAsociado) {
				MessageBox.Show("No hay un cliente asociado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			} else { 
				descuento.Text = puntosDisponibles.ToString();
				Total = Total - puntosDisponibles;
				puntosGanados = Total * 0.01m;
				total.Text = Total.ToString();
			}
			
		}
	}
}
