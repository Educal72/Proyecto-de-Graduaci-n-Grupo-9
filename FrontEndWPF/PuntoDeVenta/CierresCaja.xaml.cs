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
using static FrontEndWPF.ViewModel.CierreViewModel;

namespace FrontEndWPF.PuntoDeVenta
{
	/// <summary>
	/// Lógica de interacción para CierresCaja.xaml
	/// </summary>
	public partial class CierresCaja : Window
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
		public CierresCaja(Cierre cierre)
		{
			InitializeComponent();
			FileReadImpresora();
			FileRead();
			ImprimirFactura(cierre);
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

		public void ImprimirFactura(Cierre cierre)
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
				DrawCenteredTextBold(cierre.NombreUsuario, 12, true);
				//DrawLeftRightText($"Apertura: {MetodoPago.Text}", "", 12, typeface);
				//DrawLeftRightText($"Cierre: {Recepcionista.Text}", "", 12, typeface);
				//DrawLeftRightText($"Salonero: {Salonero.Text}", "", 12, typeface);
				//DrawLeftRightText($"Cliente: {Cliente.Text}", "", 12, typeface);
				DrawLeftRightText($"Estado de Caja: {cierre.Estado}", "", 12, typeface);
				DrawLeftRightText($"Apertura: {cierre.FechaApertura}", "", 12, typeface);
				DrawLeftRightText($"Cierre: {cierre.CierreFecha}", "", 12, typeface);
				y += 10;
				DrawCenteredTextBold("----------------------------------------------------", 12, true);
				DrawLeftRightText("Fondos Apertura", cierre.FondosApertura.ToString(), 12, boldTypeface);
				DrawLeftRightText("Pagado Tarjeta", cierre.PagadoTarjeta.ToString(), 12, boldTypeface);
				DrawLeftRightText("Pagado Efectivo", cierre.PagadoEfectivo.ToString(), 12, boldTypeface);
				DrawLeftRightText("Egresos Gabinete", "-" + cierre.Egresos.ToString(), 12, boldTypeface);
				DrawLeftRightText("Ingresos Gabinete", cierre.Ingresos.ToString(), 12, boldTypeface);
				DrawLeftRightText("Total IVA 13%", cierre.TotalIVA.ToString(), 12, boldTypeface);
				DrawLeftRightText("Total Servicio 10%", cierre.TotalServicios.ToString(), 12, boldTypeface);
				DrawLeftRightText("Anuladas", "-" + cierre.Anulado.ToString(), 12, boldTypeface);
				DrawLeftRightText("Fondos Cierre", cierre.FondosCierre.ToString(), 12, boldTypeface);
				DrawLeftRightText("Total Ganado", cierre.TotalFinal.ToString(), 12, boldTypeface);
				DrawCenteredTextBold("----------------------------------------------------", 12, true);
				y += 10;
				DrawCenteredTextBold("Detalles de Pedidos", 12, true);
				DrawLeftRightText("Restaurante", cierre.TotalRestaurante.ToString(), 12, boldTypeface);
				DrawLeftRightText("Para Llevar", cierre.TotalLlevar.ToString(), 12, boldTypeface);
				DrawLeftRightText("Uber Eats", cierre.TotalUberEats.ToString(), 12, boldTypeface);
				DrawLeftRightText("PedidosYa", cierre.TotalPedidosYa.ToString(), 12, boldTypeface);
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
	}
}
