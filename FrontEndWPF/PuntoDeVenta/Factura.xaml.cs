using FrontEndWPF.Index;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.PuntoDeVenta
{
	/// <summary>
	/// Lógica de interacción para Factura.xaml
	/// </summary>
	public partial class Factura : Window
	{
		private string Fnombre;
		private string Fcedula;
		private int Ftelefono;
		private string Fmensaje;
		private string Fcorreo;
		private string Fdireccion;
		private string Fiva;
		private string Fservicio;
		private string NombreImpresora;
		private string DescargarComo;
		private string CodigoDeControl;
		private decimal BalanceInicial;
		public Factura(FacturaDoc factura)
		{
			InitializeComponent();
			FileRead();
			FileReadImpresora();
			LlenarFactura(factura);
		}

		private void LlenarFactura(FacturaDoc factura)
		{
			NumeroFactura.Text = factura.NumeroFactura;
			Recepcionista.Text = factura.Recepcionista;
			Cliente.Text = factura.Cliente;
			Fecha.Text = $"{factura.Fecha:dd/MM/yyyy HH:mm}";
			
			TipoOrden.Text = factura.Tipo;
			MetodoPago.Text = factura.MetodoPago;
			PuntosGanados.Text = factura.Puntos.ToString();

			foreach (var detalle in factura.Detalles)
			{
				var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
				stackPanel.Children.Add(new TextBlock { Text = $"{detalle.Producto} ({detalle.Cantidad} x {detalle.PrecioUnitario:C})", Width = 200 });
				stackPanel.Children.Add(new TextBlock { Text = detalle.Total.ToString("C"), HorizontalAlignment = HorizontalAlignment.Right, Width = 50 });
				DetallesStackPanel.Children.Add(stackPanel);
			}

			SubTotal.Text = factura.SubTotal.ToString("C");
			Impuesto.Text = factura.Impuesto.ToString("C");
			Total.Text = factura.Total.ToString("C");
			Pagado.Text = factura.Pagado.ToString("C");
			Cambio.Text = factura.Cambio.ToString("C");
			Servicio.Text = factura.Servicio.ToString("C");
			Salonero.Text = factura.Salonero;
			
		}

		public void FileReadImpresora()
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
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}

		public void ImprimirFactura(FacturaDoc factura)
		{
			string printerName = NombreImpresora;
			PrintDialog printDialog = new PrintDialog();
			var printQueue = LocalPrintServer.GetDefaultPrintQueue();

			var printServer = new LocalPrintServer();
			var printer = printServer.GetPrintQueue(printerName);
			printDialog.PrintQueue = printer;
			// Crear un DrawingVisual para dibujar la factura
			var ticket = new DrawingVisual();
				using (var drawingContext = ticket.RenderOpen())
				{
					var typeface = new Typeface("Courier New");
					var boldTypeface = new Typeface(new FontFamily("Courier New"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
					var brush = Brushes.Black;
					double y = 0;

				Action<string, double> DrawCenteredText = (text, fontSize) =>
				{
					double maxWidth = printDialog.PrintableAreaWidth;
					DrawWrappedText(text, fontSize, maxWidth, typeface, y, (line, lineFontSize) =>
					{
						var formattedText = new FormattedText(line,
							System.Globalization.CultureInfo.InvariantCulture,
							FlowDirection.LeftToRight,
							typeface,
							lineFontSize,
							brush);

						drawingContext.DrawText(formattedText, new Point((maxWidth - formattedText.Width) / 2, y));
						y += formattedText.Height + 5;
					});
				};

				Action<string, double, bool> DrawCenteredTextBold = (text, fontSize, isBold) =>
				{
					var tf = isBold ? boldTypeface : typeface;
					var formattedText = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, tf, fontSize, brush);
					drawingContext.DrawText(formattedText, new Point((printDialog.PrintableAreaWidth - formattedText.Width) / 2, y));
					y += formattedText.Height + 5;
				};



				Action<string, string, double, Typeface> DrawLeftRightText = (leftText, rightText, fontSize, tf) =>
					{
						var formattedLeftText = new FormattedText(leftText, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, tf, fontSize, brush);
						var formattedRightText = new FormattedText(rightText, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, tf, fontSize, brush);
						drawingContext.DrawText(formattedLeftText, new Point(0, y));
						drawingContext.DrawText(formattedRightText, new Point(printDialog.PrintableAreaWidth - formattedRightText.Width, y));
						y += formattedLeftText.Height + 5;
					};

					var bitmap = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/323715758_861857158399502_6847062045596271805_n-removebg-preview.png", UriKind.RelativeOrAbsolute));
					drawingContext.DrawImage(bitmap, new Rect((printDialog.PrintableAreaWidth - bitmap.Width) / 2, y, bitmap.Width, bitmap.Height));
					y += bitmap.Height + 10;

					DrawCenteredTextBold(Fnombre, 16, true);
					DrawCenteredTextBold(Fcedula, 16, true);
					DrawCenteredText(Fdireccion, 12);
					DrawCenteredText(Fcorreo, 12);
					DrawCenteredText(Ftelefono.ToString(), 12);

					DrawLeftRightText($"Nº Factura: {NumeroFactura.Text}","", 12,typeface);
					DrawLeftRightText($"Metodo de Pago: {MetodoPago.Text}", "", 12, typeface);
					DrawLeftRightText($"Recepción: {Recepcionista.Text}", "", 12, typeface);
					DrawLeftRightText($"Salonero: {Salonero.Text}", "", 12, typeface);
					DrawLeftRightText($"Cliente: {Cliente.Text}", "", 12, typeface);
					DrawLeftRightText($"Fecha: {Fecha.Text}", "", 12, typeface);
					DrawCenteredTextBold(TipoOrden.Text, 12, true);
					DrawCenteredTextBold("----------------------------------------------------", 12, true);
					y += 10;

					DrawLeftRightText("Producto", "Cantidad x Precio", 12, boldTypeface);
					foreach (var detalle in factura.Detalles)
					{
						DrawLeftRightText(detalle.Producto, $"{detalle.Cantidad} x {detalle.PrecioUnitario:C}", 12, typeface);
					}
					DrawCenteredTextBold("----------------------------------------------------", 12, true);
					y += 10;

					DrawLeftRightText("Sub-total:", SubTotal.Text, 12, typeface);
					DrawLeftRightText($"I.V.A: {Fiva}%", Impuesto.Text, 12, typeface);
					DrawLeftRightText($"Servicio: {Fservicio}%", Servicio.Text, 12, typeface);
					DrawLeftRightText("Total:", Total.Text, 12, typeface);
					DrawLeftRightText("Puntos Ganados:", PuntosGanados.Text, 12, typeface);
					DrawLeftRightText("Pagado:", Pagado.Text, 12, typeface);
					DrawLeftRightText("Cambio:", Cambio.Text, 12, typeface);
					y += 10;

					DrawCenteredText("-------------------------", 12);
					DrawCenteredText(Fmensaje, 14);
					DrawCenteredText("Autorizada mediante la resolución N° DGT-R-033-2019 del 27 de junio de 2019", 12);
					DrawCenteredText("-------------------------", 12);
				}
				printDialog.PrintVisual(ticket, "Factura");
		}

		public void FileRead()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/rest_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/rest_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "Nombre":
								Fnombre = parts[1];
								break;
							case "Cedula":
								Fcedula = parts[1];
								break;
							case "Telefono":
								Ftelefono = Convert.ToInt32(parts[1]);
								break;
							case "Correo":
								Fcorreo = parts[1];
								break;
							case "Mensaje":
								Fmensaje = parts[1];
								break;
							case "Direccion":
								Fdireccion = parts[1];
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

		private void DrawWrappedText(string text, double fontSize, double maxWidth, Typeface typeface, double startY, Action<string, double> drawText)
		{
			var words = text.Split(' ');
			var line = string.Empty;
			double currentWidth = 0;
			double lineHeight = fontSize * 1.2; // Aproximación de altura de línea
			double y = startY;

			foreach (var word in words)
			{
				var testLine = string.IsNullOrEmpty(line) ? word : $"{line} {word}";
				var formattedText = new FormattedText(testLine,
					System.Globalization.CultureInfo.InvariantCulture,
					FlowDirection.LeftToRight,
					typeface,
					fontSize,
					Brushes.Black);

				if (formattedText.Width > maxWidth)
				{
					drawText(line, fontSize);
					line = word;
					y += lineHeight;
				}
				else
				{
					line = testLine;
				}
			}

			if (!string.IsNullOrEmpty(line))
			{
				drawText(line, fontSize);
			}
		}

		public void FileReadImp()
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
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}

		public class FacturaDoc
		{
			public string NumeroFactura { get; set; }
			public string Recepcionista { get; set; }
			public string Cliente { get; set; }
			public string Tipo { get; set; }
			public string MetodoPago { get; set; }
			public string Salonero { get; set; }
			public DateTime Fecha { get; set; }
			public List<DetalleFactura> Detalles { get; set; }
			public decimal Puntos { get; set; }
			public decimal SubTotal { get; set; }
			public decimal Impuesto { get; set; }
			public decimal Servicio { get; set; }
			public decimal Total { get; set; }
			public decimal Pagado { get; set; }
			public decimal Cambio { get; set; }

		}

		public class DetalleFactura
		{
			public string Producto { get; set; }
			public int Cantidad { get; set; }
			public decimal PrecioUnitario { get; set; }
			public decimal Total => Cantidad * PrecioUnitario;
		}
	}
}
