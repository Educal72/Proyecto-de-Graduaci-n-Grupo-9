﻿using FrontEndWPF.ViewModel;
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
using FrontEndWPF.Services;
using System.Globalization;
using FrontEndWPF.ViewModel;
using System.Text.RegularExpressions;
using FrontEndWPF.Index;
using Molino_POS;
using DocumentFormat.OpenXml.Vml;


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
		List<carritoItem> carritoItems = new List<carritoItem>();
		OrdenesViewModel Orden = new OrdenesViewModel();
		FacturaViewModel Factura = new FacturaViewModel();
		FichajesViewModel fichajesViewModel = new FichajesViewModel();
		ClienteViewModel clienteViewModel = new ClienteViewModel();
		private decimal puntosDisponibles = 0;
		private bool isAsociado = false;

		private int IdUsuario;
		private int IdCliente;
		public decimal Fcambio;
		int idOrden { get; set; }
		private string Fservicio;
		private string Fiva;
		private string tipoVenta;
		private string metodoPago;
		private string cajero = SesionUsuario.Instance.nombre;
		private decimal puntosGanados;
		private decimal impuestosGenerados = 0;
		private decimal servicioI;

		public Facturacion(int idOrden)
		{
			this.idOrden = idOrden;
			InitializeComponent();
			user.Content = "Usuario: " + SesionUsuario.Instance.nombre;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			FileRead();
			LoadCarritoItems();
			pagado.Focus();
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
							case "Tipo Cambio":
								Fcambio = Convert.ToDecimal(parts[1]);
								break;
						}
					}
				}
				impuestoT.Content = "Impuesto: " + Fiva + "%";
				servicioT.Content = "Servicio: " + Fservicio + "%";
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
						var PrecioIMP = (decimal)System.Math.Round(producto.Precio - (producto.Precio * (Convert.ToDecimal(Fiva)/100.00m)), 2);
						for (int i = 0; i < producto.Cantidad; i++) { 
							impuestosGenerados = (decimal)System.Math.Round(impuestosGenerados + (producto.Precio * (Convert.ToDecimal(Fiva) / 100.00m)), 2);
						}
						carrito.Items.Add(cartItem);
						carritoItems.Add(cartItem);
						Subtotal = Subtotal + (producto.Cantidad * PrecioIMP);
					}
					impuesto.Text = impuestosGenerados.ToString();
				}
			}
				subtotal.Text = Subtotal.ToString();
				descuento.Text = puntosDisponibles.ToString();
				servicioI = ((decimal)System.Math.Round((Subtotal + impuestosGenerados) * (Convert.ToDecimal(Fservicio) / 100.00m)));
				servicio.Text = servicioI.ToString();
				Total = Subtotal + impuestosGenerados + Convert.ToDecimal(servicio.Text);
			    Total = (decimal)System.Math.Round(Total, 2);
				Total = Redondear(Total);
				total.Text = Total.ToString();
			    
			}

		static decimal Redondear(decimal value)
		{
			return (value % 5 == 0) ? value : (value + (5 - value % 5));
		}
		static decimal RedondearDolar(decimal value)
		{
			// Multiply by 10 to shift decimal places, round up to the nearest integer, then divide back
			return Math.Ceiling(value * 10) / 10;
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
			pagado.Text = total.Text;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var clienteAsociado = new clienteAsociar();
			clienteAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			clienteAsociado.WindowState = WindowState.Maximized;
			if (clienteAsociado.ShowDialog() == true)
			{
				total.Text = (Total + puntosDisponibles).ToString();
				string NombreCompleto = clienteAsociado.NombreCompleto;
				IdCliente = clienteAsociado.Cedula;
				cliente.Text = NombreCompleto;
				puntosDisponibles = clienteAsociado.puntosDisponibles;
				isAsociado = clienteAsociado.isAsociado;
				descuento.Text = "0";
				puntos.Content = "Puntos";
				
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var saloneroAsociado = new saloneroAsociar();
			saloneroAsociado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			saloneroAsociado.WindowState = WindowState.Maximized;
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
			if (!double.TryParse(pagado.Text, out double result) || Convert.ToDecimal(pagado.Text.ToString()) < Convert.ToDecimal(total.Text) || !double.TryParse(servicio.Text, out double result2))
			{
				MessageBox.Show("Por favor, introduzca una cantidad pagada valida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{

				var orden = Orden.GetOrdenById(idOrden);
				string pagadoConvertido = pagado.Text;
				decimal TotalConvertido = Convert.ToDecimal(total.Text);
				if (tipoCambio.SelectedIndex == 1) {
					var conversion = Redondear(Convert.ToDecimal(pagado.Text) * Fcambio);
					pagadoConvertido = conversion.ToString();
					var conversionTotal = Convert.ToDecimal(total.Text) * Fcambio;
					TotalConvertido = Redondear(conversionTotal);
				}
				int resultadoCrear = Factura.CrearFactura(idOrden, Convert.ToDecimal(pagadoConvertido), impuestosGenerados, Convert.ToDecimal(servicio.Text), orden.Creacion, DateTime.Now, cajero, IdUsuario, IdCliente, Convert.ToDecimal(descuento.Text), puntosGanados, metodoPago, tipoVenta, TotalConvertido);
				
                if (resultadoCrear != 0)
                {
					Factura.TerminarOrden(idOrden);
					if (isAsociado && descuento.Text != "" || descuento.Text != "0") 
					{
						puntosGanados = Redondear(Total * 0.01m);
						clienteViewModel.PuntosCliente(IdCliente, puntosGanados, Convert.ToDecimal(descuento.Text));
					}
					
                }
                resultadoFacturacion resultado = new resultadoFacturacion(isAsociado, TotalConvertido.ToString(), pagadoConvertido, carritoItems, Convert.ToDecimal(pagadoConvertido), Convert.ToInt32(Fiva), Convert.ToInt32(Fservicio), orden.Creacion, cajero, Convert.ToDecimal(descuento.Text), puntosGanados, metodoPago, tipoVenta, TotalConvertido, Subtotal, cliente.Text, resultadoCrear, salonero.Text, Convert.ToDecimal(servicio.Text), impuestosGenerados);
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
			if (!isAsociado)
			{
				MessageBox.Show("No hay un cliente asociado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				if (puntos.Content.ToString() == "Puntos" && puntosDisponibles != 0)
				{
					descuento.Text = puntosDisponibles.ToString();
					Total = Total - puntosDisponibles;
					puntosGanados = Math.Round(Total * 0.01m, 2);
					total.Text = Total.ToString();
					puntos.Content = "X";
				}
				else if (puntos.Content.ToString() == "X")
				{
					descuento.Text = "0";
					Total = Total + puntosDisponibles;
					puntosGanados = 0;
					total.Text = Total.ToString();
					puntos.Content = "Puntos";
				} else if (puntosDisponibles == 0) {
					MessageBox.Show("El cliente no tiene puntos disponibles.", "Sin Puntos", MessageBoxButton.OK, MessageBoxImage.Information);
				}

			}
		}


		private void servicio_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(servicio.Text)) { 
				servicio.Text = "0";
				servicio.CaretIndex = servicio.Text.Length;
				
			}
			total.Text = Redondear((Total - servicioI) + Convert.ToDecimal(servicio.Text)).ToString();
			//servicioI = Convert.ToDecimal(servicio.Text);
			
		}
		private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// Verificar si el texto ingresado es un número
			e.Handled = !IsTextNumeric(e.Text);
		}

		private bool IsTextNumeric(string text)
		{
			// Usar expresión regular para verificar si el texto es numérico
			Regex regex = new Regex("[^0-9]");
			return !regex.IsMatch(text);
		}

		private void Fichaje_Click(object sender, RoutedEventArgs e)
		{
			CodigoBarras barcodeWindow = new CodigoBarras(fichajesViewModel);
			barcodeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			barcodeWindow.ShowDialog(); // Abre la ventana emergente y espera su cierre
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (tipoCambio.SelectedIndex == 0) {
				totalLabel.Content = "Total: ₡";
				decimal decimalTotal = Total;
				total.Text = Redondear(Math.Round(decimalTotal, 2)).ToString();
			}
			else if (tipoCambio.SelectedIndex == 1){
				totalLabel.Content = "Total: $";
				decimal decimalTotal = Total / Fcambio;
				total.Text = RedondearDolar(decimalTotal).ToString();
			}
		}
	}
}
