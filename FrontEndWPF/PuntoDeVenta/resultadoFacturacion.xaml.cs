using FrontEndWPF.Index;
using FrontEndWPF.PuntoDeVenta;
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
using System.Windows.Shapes;
using static FrontEndWPF.PuntoDeVenta.Factura;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para resultadoFacturacion.xaml
	/// </summary>
	public partial class resultadoFacturacion : Window
	{
		List<Facturacion.carritoItem> carritoItems;
		decimal cantidadpagada;
		int impuestos;
		int servicio;
		DateTime fechacreacion;
		string cajero;
		decimal descuento;
		decimal puntosganados;
		string metodopago;
		string tipo;
		decimal totalPagar;
		decimal subtotal;
		string cliente;
		int facturaId;
		string salonero;
		decimal servicioD;
		decimal impuestosGenerados;
		decimal Fiva;
		decimal Fservicio;

		public resultadoFacturacion(bool isAsociado, string cantidadPagar, string cantidadPagada, List<Facturacion.carritoItem> carrito, decimal CantidadPagada, int Impuestos, int Servicio, DateTime FechaCreacion, string Cajero, decimal Descuento, decimal PuntosGanados, string MetodoPago, string TipoVenta, decimal Total, decimal Subtotal, string Cliente, int Factura, string Salonero, decimal ServicioD, decimal ImpuestosGenerados)
		{
			InitializeComponent();
			FileRead();
			pagado.Text = cantidadPagada;
			total.Text = cantidadPagar;
			vuelto.Text = (double.Parse(cantidadPagada) - double.Parse(cantidadPagar)).ToString();
			if (isAsociado && Descuento == 0.00m) {
				decimal puntosGanados = (Convert.ToDecimal(cantidadPagar) * 0.01m);
				puntos.Text = Redondear(puntosGanados).ToString("F2");
			} else {
				puntos.Text = "0";
			}
			
			carritoItems = carrito;
			cantidadpagada = CantidadPagada;
			impuestos = Impuestos;
			servicio = Servicio;
			fechacreacion = FechaCreacion;
			cajero = Cajero;
			descuento = Descuento;
			puntosganados = PuntosGanados;
			metodopago = MetodoPago;
			tipo = TipoVenta;
			totalPagar = Total;
			subtotal = Subtotal;
			cliente = Cliente;
			facturaId = Factura;
			salonero = Salonero;
			servicioD = ServicioD;
			impuestosGenerados = ImpuestosGenerados;
			GenerarFactura();
		}
		static decimal Redondear(decimal value)
		{
			return (value % 5 == 0) ? value : (value + (5 - value % 5));
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
			List<DetalleFactura> detalleFacturas = new List<DetalleFactura>();
			foreach (var item in carritoItems) {
				var PrecioIMP = (decimal)System.Math.Round(item.Precio - (item.Precio * 0.13m));
				var detalle = new DetalleFactura()
				{
					Producto = item.Nombre,
					Cantidad = item.Cantidad,
					PrecioUnitario = PrecioIMP
				};
				detalleFacturas.Add(detalle);
			}

			var factura = new FacturaDoc
			{
				NumeroFactura = facturaId.ToString(),
				Recepcionista = cajero,
				Cliente = cliente,
				Fecha = fechacreacion,
				Detalles = detalleFacturas,
				SubTotal = subtotal,
				Impuesto = impuestosGenerados,
				Total = totalPagar,
				Pagado = cantidadpagada,
				Cambio = Convert.ToDecimal(vuelto.Text),
				Puntos = puntosganados,
				Tipo = tipo,
				MetodoPago = metodopago,
				Salonero = salonero,
				Servicio = servicioD
			};

			var ventana = new Factura(factura);
			ventana.ImprimirFactura(factura);
			this.Close();
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
			if (mainWindow != null)
			{
				mainWindow.ChangePageToPuntoVenta();
			}
		}

		private void GenerarFactura()
		{
			List<DetalleFactura> detalleFacturas = new List<DetalleFactura>();
			foreach (var item in carritoItems)
			{
				var PrecioIMP = (decimal)System.Math.Round(item.Precio - (item.Precio * (Fiva/100)));
				var detalle = new DetalleFactura()
				{
					Producto = item.Nombre,
					Cantidad = item.Cantidad,
					PrecioUnitario = PrecioIMP
				};
				detalleFacturas.Add(detalle);
			}

			var factura = new FacturaDoc
			{
				NumeroFactura = facturaId.ToString(),
				Recepcionista = cajero,
				Cliente = cliente,
				Fecha = fechacreacion,
				Detalles = detalleFacturas,
				SubTotal = subtotal,
				Impuesto = impuestosGenerados,
				Total = totalPagar,
				Pagado = cantidadpagada,
				Cambio = Convert.ToDecimal(vuelto.Text),
				Puntos = puntosganados,
				Tipo = tipo,
				MetodoPago = metodopago,
				Salonero = salonero,
				Servicio = servicioD
			};

			var ventana = new Factura(factura);
			ventana.ImprimirFactura(factura);
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
								Fiva = Convert.ToDecimal(parts[1]);
								break;
							case "Servicio":
								Fservicio = Convert.ToDecimal(parts[1]);
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
