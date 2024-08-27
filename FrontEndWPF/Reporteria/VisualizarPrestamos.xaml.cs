using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using iText.Layout;
using Rectangle = iText.Kernel.Geom.Rectangle;
using Canvas = iText.Layout.Canvas;
using Paragraph = iText.Layout.Element.Paragraph;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Data;
using FrontEndWPF.ViewModel;
using ClosedXML.Excel;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Spreadsheet;
using Cell = iText.Layout.Element.Cell;



namespace FrontEndWPF.Reporteria
{
	/// <summary>
	/// Interaction logic for VisualizarPrestamos.xaml
	/// </summary>
	public partial class VisualizarPrestamos : UserControl
	{
		private List<Prestamo> prestamos = new List<Prestamo>();
		List<PrestamosViewModel.Pagos> pagos = new List<PrestamosViewModel.Pagos>();
		Prestamo mostrarDatos;
		private Conexion conexion = new Conexion();
		Document document;
		PrestamosViewModel prestamosViewModel = new PrestamosViewModel();
		string Fcedula;
		string Fnombre;
		int Ftelefono;
		string Fdireccion;
		string Fmensaje;
		string Fcorreo;
		bool HistorialPagos = false;

		public VisualizarPrestamos()
		{
			//Se inicializa lo que se necesita para mostrar al principio leer un .txt con FileRead,
			//LoadInicioSesiones para sacar los prestamos no le cambie el nombre :p
			//GenerarPDF genera el pdf inicial para mostrar algo
			//InitializeWebView es la caja que va a mostrar el PDF es parte de una libreria
			InitializeComponent();
			FileRead();
			LoadInicioSesiones();
			GenerarPDF();
			InitializeWebView();
		}

		//esto es parte de la inicializacion y evita que el pdf cargue y el webview no este listo y de una exepcion por lo que es obligatorio
		private async void InitializeWebView()
		{
			await webView.EnsureCoreWebView2Async(null);
		}
		//esto carga el pdf generado al webview obligatorio igual
		public void LoadPDF(string pdfPath)
		{
			if (webView.CoreWebView2 != null)
			{
				// Carga el PDF en el WebView2
				string url = new Uri(pdfPath).AbsoluteUri;
				webView.CoreWebView2.Navigate(url);
			}
			else
			{
				webView.CoreWebView2InitializationCompleted += (sender, e) =>
				{
					string url = new Uri(pdfPath).AbsoluteUri;
					webView.CoreWebView2.Navigate(url);
				};
			}
		}

		//funcion para llamar datos de la bd mi yo del pasado no le cambio el nombre por idiota 
		private void LoadInicioSesiones()
		{
			prestamos.Clear();
			Conexion conexion = new Conexion();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT Id, IdEmpleado, MontoTotal, Pendiente, Interes, Estado, FechaCreacion, Descripcion FROM Prestamos WHERE Estado = 'Activo'";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{


									Prestamo prestamo = new Prestamo()
									{
										Id = (int)reader["Id"],
										IdUsuario = (int)reader["IdEmpleado"],
										NombreUsuario = GetUserByEmpleadoId((int)reader["IdEmpleado"]),
										Descripcion = reader["Descripcion"].ToString(),
										MontoTotal = (decimal)reader["MontoTotal"],
										MontoPendiente = (decimal)reader["Pendiente"],
										InteresesMoratorios = (decimal)reader["Interes"],
										Estado = reader["Estado"].ToString(),
										FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
									};
									prestamos.Add(prestamo);
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}
		}

		//esto genera el pdf en base a IniciosDeSesion
		public void GenerarPDF()
		{
			// Ruta base para guardar el archivo PDF
			string rutaBase = Path.GetTempPath();
			string nombreArchivoBase = "InformePrestamos";
			string extensionArchivo = ".pdf";
			string archivoPdf = System.IO.Path.Combine(rutaBase, $"{nombreArchivoBase}_{Guid.NewGuid()}{extensionArchivo}");

			// Generar el archivo PDF
			using (var writer = new PdfWriter(archivoPdf))
			using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
			using (document = new Document(pdf))
			{
				// Crear fuentes para negrita y normal
				PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
				PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

				// Ruta de la imagen
				string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/323715758_861857158399502_6847062045596271805_n-removebg-preview.png";
				ImageData imageData = ImageDataFactory.Create(imagePath);
				iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData)
					.ScaleToFit(120, 120); // Ajustar tamaño de la imagen

				// Crear una tabla para el encabezado con dos columnas
				iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }));
				headerTable.SetWidth(UnitValue.CreatePercentValue(100));

				// Primera celda con los subtítulos
				Cell textCell = new Cell()
					.Add(new iText.Layout.Element.Paragraph("Informe de Préstamos").SetFont(boldFont).SetFontSize(14))
					.Add(new iText.Layout.Element.Paragraph($"Empresa: {Fnombre}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Total de Registros: {prestamos.Count}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph("\n")); // Espacio
				textCell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
				textCell.SetPadding(0);
				headerTable.AddCell(textCell);

				// Segunda celda con la imagen
				Cell imageCell = new Cell().Add(image);
				imageCell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
				imageCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
				imageCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
				imageCell.SetPaddingRight(20);
				headerTable.AddCell(imageCell);

				// Añadir la tabla de encabezado al documento
				document.Add(headerTable);

				// Encabezados de la tabla
				var table = new iText.Layout.Element.Table(7); // 4 columnas
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Nombre de Empleado").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Descripción").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Total Prestado").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Pendiente").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Interes %").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Estado").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha de Creación").SetFont(boldFont)));

				// Datos de la tabla
				foreach (var sesion in prestamos)
				{
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.NombreUsuario.ToString()).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Descripcion.ToString()).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.MontoTotal.ToString()}").SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.MontoPendiente.ToString()}").SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.InteresesMoratorios.ToString() +"%").SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Estado.ToString()).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.FechaCreacion.ToString("yyyy-MM-dd")).SetFont(normalFont)));
				}

				document.Add(table);
				// Añadir números de página
				AddPageNumbers(pdf);

			}
			LoadPDF(archivoPdf);
			//MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe Generado", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private void AddPageNumbers(iText.Kernel.Pdf.PdfDocument pdfDoc)
		{
			int numberOfPages = pdfDoc.GetNumberOfPages();
			PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

			for (int i = 1; i <= numberOfPages; i++)
			{
				iText.Kernel.Pdf.PdfPage page = pdfDoc.GetPage(i);
				Rectangle pageSize = page.GetPageSize();
				PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

				// Crear el Canvas para el PdfCanvas y la página
				iText.Layout.Canvas canvas = new Canvas(pdfCanvas, pageSize); // Nota: El constructor solo necesita PdfCanvas y Rectangle

				// Especificar el nombre completo para evitar el conflicto de nombres
				canvas.ShowTextAligned(new Paragraph($"Página {i}").SetFont(font).SetFontSize(12),
					pageSize.GetWidth() - 50, 20, i, iText.Layout.Properties.TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
			}
		}

		//esta funcion consigue el nombre completo de un usuario en base al idempleado
		public string GetUserByEmpleadoId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    Usuario.Nombre, 
    Usuario.Apellido
FROM 
    Usuario
JOIN 
    Empleado ON Usuario.Id = Empleado.IdUsuario
WHERE 
    Empleado.Id = @IdEmpleado";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		public class Prestamo
		{
			public int Id { get; set; }
			public int IdUsuario { get; set; }
			public string? NombreUsuario { get; set; }
			public decimal MontoTotal { get; set; }
			public decimal MontoPendiente { get; set; }
			public decimal InteresesMoratorios { get; set; }
			public string Estado { get; set; }
			public string Descripcion { get; set; }
			public DateTime FechaCreacion { get; set; }
		}

		//boton para crear un nuevo prestamo
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevoPrestamo = new añadirPrestamo();
			nuevoPrestamo.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoPrestamo.ShowDialog() == true)
			{
				DateTime Fecha = nuevoPrestamo.fecha_añadirPrestamo;
				double Interes = nuevoPrestamo.interes_añadirPrestamo;
				string Descripcion = nuevoPrestamo.descripcion_añadirPrestamo;
				double Prestado = nuevoPrestamo.prestado_añadirPrestamo;
				int Usuario = nuevoPrestamo.empleado_añadirPrestamo;

				/*
				 * Aquí se manda por parámetro al método llamado: AgregarIncidente, -
				 * esto con la finalidad de que pueda ser enviado y registrado a la -
				 * base de datos que esta en SQL Server.
				 */
				prestamosViewModel.CrearPrestamo(Usuario, Descripcion, Interes, Prestado, Fecha);
				LoadInicioSesiones();
				GenerarPDF();

				//conexionEmpleado.AgregarIncidente(Fecha, Hora, Descripcion, Tipo, conexionEmpleado.getIdUsuario(Usuario));
			}
		}

		//boton para hacer un pago a un prestamo
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var nuevoPago = new añadirPago();
			nuevoPago.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoPago.ShowDialog() == true)
			{
				DateTime Fecha = nuevoPago.fecha_añadirPago;
				double CantidadPagada = nuevoPago.cantidadPagada_añadirPago;
				int Prestamo = nuevoPago.idPrestamo_añadirPago;

				/*
				 * Aquí se manda por parámetro al método llamado: AgregarIncidente, -
				 * esto con la finalidad de que pueda ser enviado y registrado a la -
				 * base de datos que esta en SQL Server.
				 */
				prestamosViewModel.CrearPago(Prestamo, CantidadPagada, Fecha);
				LoadInicioSesiones();
				GenerarPDF();

				//conexionEmpleado.AgregarIncidente(Fecha, Hora, Descripcion, Tipo, conexionEmpleado.getIdUsuario(Usuario));
			}
		}

		//togglebutton para mostrar los prestamos inactivos
		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			prestamos.Clear();
			Conexion conexion = new Conexion();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT Id, IdEmpleado, MontoTotal, Pendiente, Interes, Estado, FechaCreacion, Descripcion FROM Prestamos";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{


									Prestamo prestamo = new Prestamo()
									{
										Id = (int)reader["Id"],
										IdUsuario = (int)reader["IdEmpleado"],
										NombreUsuario = GetUserByEmpleadoId((int)reader["IdEmpleado"]),
										Descripcion = reader["Descripcion"].ToString(),
										MontoTotal = (decimal)reader["MontoTotal"],
										MontoPendiente = (decimal)reader["Pendiente"],
										InteresesMoratorios = (decimal)reader["Interes"],
										Estado = reader["Estado"].ToString(),
										FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
									};
									prestamos.Add(prestamo);
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}
					GenerarPDF();
					conexion.CloseConnection(connection);
				}
			}
		}

		//esta ventana lo que hace es que regenera otro pdf por si el contador quiere ver el historial de pagos que se han hecho a un prestamo en especifico
		private void verHistorialPagos(object sender, RoutedEventArgs e)
		{
			var verHistorialPagos = new verHistorialPagos();
			verHistorialPagos.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (verHistorialPagos.ShowDialog() == true)
			{
				int IdPrestamo = verHistorialPagos.IdPrestamo_verHistorialPagos;
				mostrarDatos = verHistorialPagos.Prestamo_verHistorialPagos;
				/*
				 * Aquí se manda por parámetro al método llamado: AgregarIncidente, -
				 * esto con la finalidad de que pueda ser enviado y registrado a la -
				 * base de datos que esta en SQL Server.
				 */

				inactivos.IsEnabled = false;
				pagos = prestamosViewModel.LoadPagos(IdPrestamo);
				GenerarPDFPagos(pagos, mostrarDatos);
				HistorialPagos = true;
			}
		}

		//esto genera dicho pdf nuevo
		public void GenerarPDFPagos(List<PrestamosViewModel.Pagos> pagos, Prestamo prestamo)
		{
			// Ruta base para guardar el archivo PDF
			string rutaBase = Path.GetTempPath();
			string nombreArchivoBase = "InformePagos";
			string extensionArchivo = ".pdf";
			string archivoPdf = System.IO.Path.Combine(rutaBase, $"{nombreArchivoBase}_{Guid.NewGuid()}{extensionArchivo}");

			// Generar el archivo PDF
			using (var writer = new PdfWriter(archivoPdf))
			using (var pdf = new iText.Kernel.Pdf.PdfDocument(writer))
			using (document = new Document(pdf))
			{
				// Crear fuentes para negrita y normal
				PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
				PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

				// Ruta de la imagen
				string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/323715758_861857158399502_6847062045596271805_n-removebg-preview.png";
				ImageData imageData = ImageDataFactory.Create(imagePath);
				iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData)
					.ScaleToFit(120, 120); // Ajustar tamaño de la imagen

				// Crear una tabla para el encabezado con dos columnas
				iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }));
				headerTable.SetWidth(UnitValue.CreatePercentValue(100));

				// Primera celda con los subtítulos
				Cell textCell = new Cell()
					.Add(new iText.Layout.Element.Paragraph("Informe de Pagos").SetFont(boldFont).SetFontSize(14))
					.Add(new iText.Layout.Element.Paragraph($"Empresa: {Fnombre}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Deudor: {prestamo.NombreUsuario}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Descripción del Préstamo: {prestamo.Descripcion}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Fecha de Creación del Préstamo: {prestamo.FechaCreacion.ToString("yyyy-MM-dd")}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph($"Total de Registros: {prestamos.Count}").SetFont(normalFont).SetFontSize(12))
					.Add(new iText.Layout.Element.Paragraph("\n")); // Espacio
				textCell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
				textCell.SetPadding(0);
				headerTable.AddCell(textCell);

				// Segunda celda con la imagen
				Cell imageCell = new Cell().Add(image);
				imageCell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
				imageCell.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
				imageCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
				imageCell.SetPaddingRight(20);
				headerTable.AddCell(imageCell);

				// Añadir la tabla de encabezado al documento
				document.Add(headerTable);

				// Encabezados de la tabla
				var table = new iText.Layout.Element.Table(2); // 4 columnas
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Cantidad Pagada").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha").SetFont(boldFont)));

				// Datos de la tabla
				foreach (var sesion in pagos)
				{
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.CantidadPagada.ToString()}").SetFont(normalFont)));
					table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Fecha.ToString("yyyy-MM-dd")).SetFont(normalFont)));
				}
				table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph("Pendiente").SetFont(boldFont)));
				table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(prestamo.MontoPendiente.ToString()).SetFont(normalFont)));
				table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph("Total").SetFont(boldFont)));
				table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph((prestamo.MontoTotal + (prestamo.MontoTotal * prestamo.InteresesMoratorios / 100.00m)).ToString()).SetFont(normalFont)));

				document.Add(table);
				// Añadir números de página
				AddPageNumbers(pdf);

			}
			LoadPDF(archivoPdf);
			//MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe Generado", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		//esto lee los datos de un txt para sacar el nombre del restaurante
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

		//esto limpia el pdf para mostrar el pdf original
		private void Limpiar_Click(object sender, RoutedEventArgs e)
		{
			inactivos.IsEnabled = true;
			LoadInicioSesiones();
			GenerarPDF();
			HistorialPagos = false;
		}

		//esta descarga el pdf en formato excel y dependiendo del pdf que se este mostrando es el excel que descarga

		private void Descargar_Excel(object sender, RoutedEventArgs e)
		{
			if (!HistorialPagos)
			{

				var workbook = new XLWorkbook();
				var worksheet = workbook.Worksheets.Add("Prestamos");

				// Agregar encabezados de columnas
				worksheet.Cell(1, 1).Value = "Nombre de Usuario";
				worksheet.Cell(1, 2).Value = "Descripcion";
				worksheet.Cell(1, 3).Value = "Prestado";
				worksheet.Cell(1, 4).Value = "Pendiente";
				worksheet.Cell(1, 5).Value = "Interes";
				worksheet.Cell(1, 6).Value = "Estado";
				worksheet.Cell(1, 7).Value = "Fecha de Creación";

				for (int i = 1; i <= 4; i++)
				{
					worksheet.Cell(i, 9).Style.Font.Bold = true;
				}
				for (int i = 1; i <= 7; i++)
				{
					worksheet.Cell(1, i).Style.Font.Bold = true;
				}
				worksheet.Column(1).Width = 40;
				worksheet.Column(2).Width = 40;
				worksheet.Column(3).Width = 15;
				worksheet.Column(4).Width = 15;
				worksheet.Column(5).Width = 10;
				worksheet.Column(6).Width = 10;
				worksheet.Column(7).Width = 25;
				worksheet.Column(9).Width = 25;
				worksheet.Column(10).Width = 30;
				worksheet.Cell(1, 9).Value = "INFORME DE PRÉSTAMOS";
				worksheet.Cell(2, 9).Value = "Empresa: ";
				worksheet.Cell(3, 9).Value = "Fecha de Impresión: ";
				worksheet.Cell(4, 9).Value = "Total Registros: ";
				worksheet.Cell(2, 10).Value = Fnombre;
				worksheet.Cell(3, 10).Value = DateTime.Now.ToString("yyyy-MM-dd");
				worksheet.Cell(4, 10).Value = prestamos.Count;

				// Agregar datos a las columnas
				for (int i = 0; i < prestamos.Count; i++)
				{
					var prestamo = prestamos[i];
					worksheet.Cell(i + 2, 1).Value = prestamo.NombreUsuario;
					worksheet.Cell(i + 2, 2).Value = prestamo.Descripcion;
					worksheet.Cell(i + 2, 3).Value = prestamo.MontoTotal;
					worksheet.Cell(i + 2, 4).Value = prestamo.MontoPendiente;
					worksheet.Cell(i + 2, 5).Value = prestamo.InteresesMoratorios;
					worksheet.Cell(i + 2, 6).Value = prestamo.Estado;
					worksheet.Cell(i + 2, 7).Value = prestamo.FechaCreacion.ToString("yyyy-MM-dd");
				}


				// Guardar el archivo
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Filter = "Excel Files|*.xlsx",
					Title = "Guardar Archivo Excel",
					FileName = ""
				};
				if (saveFileDialog.ShowDialog() == true)
				{
					using (var stream = new MemoryStream())
					{
						workbook.SaveAs(stream);
						File.WriteAllBytes(saveFileDialog.FileName, stream.ToArray());
					}
				}
			}
			else {
				var workbook = new XLWorkbook();
				var worksheet = workbook.Worksheets.Add("Pagos");

				// Agregar encabezados de columnas
				worksheet.Cell(1, 1).Value = "Cantidad Pagada";
				worksheet.Cell(1, 2).Value = "Fecha de Pago";

				for (int i = 1; i <= 2; i++)
				{
					worksheet.Cell(1, i).Style.Font.Bold = true;
				}
				for (int i = 1; i <= 7; i++)
				{
					worksheet.Cell(i, 4).Style.Font.Bold = true;
				}
				worksheet.Cell(1, 4).Value = "INFORME DE PAGOS DE PRÉSTAMOS";
				worksheet.Cell(2, 4).Value = "Empresa: ";
				worksheet.Cell(3, 4).Value = "Deudor: ";
				worksheet.Cell(4, 4).Value = "Descripción del Préstamo: ";
				worksheet.Cell(5, 4).Value = "Fecha de Creación del Préstamo: ";
				worksheet.Cell(6, 4).Value = "Fecha de Impresión: ";
				worksheet.Cell(7, 4).Value = "Total Registros: ";
				worksheet.Column(1).Width = 25;
				worksheet.Column(2).Width = 25;
				worksheet.Column(4).Width = 35;
				worksheet.Column(5).Width = 35;
				worksheet.Cell(2, 5).Value = Fnombre;
				worksheet.Cell(3, 5).Value = mostrarDatos.NombreUsuario;
				worksheet.Cell(4, 5).Value = mostrarDatos.Descripcion;
				worksheet.Cell(5, 5).Value = mostrarDatos.FechaCreacion.ToString("yyyy-MM-dd");
				worksheet.Cell(6, 5).Value = DateTime.Now.ToString("yyyy-MM-dd");
				worksheet.Cell(7, 5).Value = pagos.Count;

				// Agregar datos a las columnas
				for (int i = 0; i < pagos.Count; i++)
				{
					var pagosList = pagos[i];
					worksheet.Cell(i + 2, 1).Value = pagosList.CantidadPagada;
					worksheet.Cell(i + 2, 2).Value = pagosList.Fecha.ToString("yyyy-MM-dd");
				}
				worksheet.Cell(pagos.Count + 2, 1).Value = "Pendiente";
				worksheet.Cell(pagos.Count + 2, 1).Style.Font.Bold = true;
				worksheet.Cell(pagos.Count + 2, 2).Value = mostrarDatos.MontoPendiente;
				worksheet.Cell(pagos.Count + 3, 1).Value = "Monto Original";
				worksheet.Cell(pagos.Count + 3, 1).Style.Font.Bold = true;
				worksheet.Cell(pagos.Count + 3, 2).Value = mostrarDatos.MontoTotal + (mostrarDatos.MontoTotal * mostrarDatos.InteresesMoratorios / 100.00m);
				// Guardar el archivo
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Filter = "Excel Files|*.xlsx",
					Title = "Guardar Archivo Excel",
					FileName = mostrarDatos.NombreUsuario + "_" + mostrarDatos.Descripcion
				};
				if (saveFileDialog.ShowDialog() == true)
				{
					using (var stream = new MemoryStream())
					{
						workbook.SaveAs(stream);
						File.WriteAllBytes(saveFileDialog.FileName, stream.ToArray());
					}
				}
			}
		}
		//este es el filtro de mostrar inactivos pero si se vuelve a hacer click deja de mostrar los inactivos
		private void MyToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			LoadInicioSesiones();
			GenerarPDF();
		}
	}
}
