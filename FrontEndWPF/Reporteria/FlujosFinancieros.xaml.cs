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
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using System.Windows.Data;
using FrontEndWPF.ViewModel;
using ClosedXML.Excel;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Spreadsheet;
using Cell = iText.Layout.Element.Cell;
using FrontEndWPF.Modelos;
using static FrontEndWPF.Reporteria.VisualizarPrestamos;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for FlujosFinancieros.xaml
    /// </summary>
    public partial class FlujosFinancieros : UserControl
	{
        private List<Modelos.Prestamos> prestamos = new List<Modelos.Prestamos>();
        private List<Modelos.Inversion> inversiones = new List<Modelos.Inversion>();
        private List<Modelos.Financiamiento> financiamientos = new List<Modelos.Financiamiento>();

        private Conexion conexion = new Conexion();
        Document document;
        FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
        string Fcedula;
        string Fnombre;
        int Ftelefono;
        string Fdireccion;
        string Fmensaje;
        string Fcorreo;
        private FlujosFinancierosViewModel viewModel;

        public FlujosFinancieros()
        {
            InitializeComponent();
            viewModel = new FlujosFinancierosViewModel();
            LoadNombresEmpresa();
            LoadFinanciera();
            LoadNombreUsuario();
            prestamos =
            flujosFinancierosViewModel.GetAllPrestamos();
            FileRead();
            //LoadInicioSesiones();
            GenerarPDF();
            GenerarPDFALLInversiones();
            GenerarPDFALLFinanciamientos();
            InitializeWebView();
          
        }

        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async(null);
        }

        private void LoadNombresEmpresa()
        {
            List<string> nombresEmpresa = viewModel.GetAllNombresEmpresa();
            NombreEmpresaComboBox.ItemsSource = nombresEmpresa;
        }

        private void LoadFinanciera()
        {
            List<string> nombresFinanciera = viewModel.GetAllNombresFinancieras();
            FinancieraComboBox.ItemsSource = nombresFinanciera;
        }

        private void LoadNombreUsuario()
        {
            List<int> idEmpleados = viewModel.GetAllIdEmpleados();
            List<string> idEmpleadosString = new List<string>();

			foreach (var id in idEmpleados) {
                idEmpleadosString.Add(GetUserByEmpleadoId(id));
            }
            // Convertir la lista de enteros a una lista de cadenas
            //List<string> idEmpleadosString = idEmpleados.ConvertAll(id => id.ToString());


            PrestamoEmpleadoComboBox.ItemsSource = idEmpleadosString;
        }

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
                var table = new iText.Layout.Element.Table(6); // 4 columnas
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Monto Total").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Pendiente").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Interes").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Estado").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha Creacion").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Descripcion").SetFont(boldFont)));
                // Datos de la tabla
                foreach (var sesion in prestamos)
                {
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.MontoTotal.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Pendiente.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Interes.ToString()}").SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Estado.ToString()}").SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.FechaCreacion.ToString("yyyy-MM-dd")).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Descripcion.ToString()}").SetFont(normalFont)));

                }

                document.Add(table);
                // Añadir números de página
                AddPageNumbers(pdf);

            }
            LoadPDF(archivoPdf);
            //MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe Generado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

		public string GetUserByEmpleadoId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    Usuario.Cedula, 
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
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString() + " ("+ reader["Cedula"].ToString()+")";
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}

		public void GenerarPDFALLInversiones()
        {
            // Ruta base para guardar el archivo PDF
            string rutaBase = Path.GetTempPath();
            string nombreArchivoBase = "InformeInversiones";
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
                    .Add(new iText.Layout.Element.Paragraph("Informe de Inversiones").SetFont(boldFont).SetFontSize(14))
                    .Add(new iText.Layout.Element.Paragraph($"Empresa: {Fnombre}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Total de Registros: {inversiones.Count}").SetFont(normalFont).SetFontSize(12))
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
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Financiera").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Monto Invertido").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha Creacion").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Ganancia Mensual").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha Finalizacion").SetFont(boldFont)));
                // Datos de la tabla
                foreach (var sesion in inversiones)
                {
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Financiera.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.MontoInvertido.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.FechaCreacion.ToString("yyyy-MM-dd")).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.GananciaMensual.ToString()}").SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.FechaFinalizacion?.ToString("yyyy-MM-dd") ?? "N/A").SetFont(normalFont)));

                }

                document.Add(table);
                // Añadir números de página
                AddPageNumbers(pdf);

            }
            LoadPDF(archivoPdf);
            //MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe Generado", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void GenerarPDFALLFinanciamientos()
        {
            // Ruta base para guardar el archivo PDF
            string rutaBase = Path.GetTempPath();
            string nombreArchivoBase = "InformeFinanciamientos";
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
                    .Add(new iText.Layout.Element.Paragraph("Informe de Financiamientos").SetFont(boldFont).SetFontSize(14))
                    .Add(new iText.Layout.Element.Paragraph($"Empresa: {Fnombre}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
                    .Add(new iText.Layout.Element.Paragraph($"Total de Registros: {financiamientos.Count}").SetFont(normalFont).SetFontSize(12))
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
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Nombre Empresa").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Motivo").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Fecha").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Estado").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Monto").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Cancelado").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new iText.Layout.Element.Paragraph("Interes").SetFont(boldFont)));
                // Datos de la tabla
                foreach (var sesion in financiamientos)
                {
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.NombreEmpresa.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Motivo.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Fecha.ToString("yyyy-MM-dd")).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph(sesion.Estado.ToString()).SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Monto.ToString()}").SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Cancelado.ToString()}").SetFont(normalFont)));
                    table.AddCell(new Cell().Add(new iText.Layout.Element.Paragraph($"₡ {sesion.Interes.ToString()}").SetFont(normalFont)));
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




        private void RefreshWebView(object prestamosData)
        {
            // Actualiza la fuente del WebView2 con los datos obtenidos
            // Puedes crear HTML dinámico o cargar un PDF basado en los datos.
            // webView.Source = new Uri("ruta_al_archivo_o_contenido_html_generado");
        }



        //private void PrestamosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
        //    var selectedMethod = ((ComboBoxItem)PrestamosComboBox.SelectedItem).Content.ToString();

        //    switch (selectedMethod)
        //    {
        //        case "GetAllPrestamos":
        //            // Llama al método para obtener todos los préstamos y actualiza el WebView2
        //            prestamos = flujosFinancierosViewModel.GetAllPrestamos();
        //            GenerarPDF();
        //            break;
        //    }
        //}

        //private void InversionesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
        //    var selectedMethod = ((ComboBoxItem)InversionesComboBox.SelectedItem).Content.ToString();

        //    switch (selectedMethod)
        //    {
        //        case "GetAllInversiones":
        //            // Llama al método para obtener todos los préstamos y actualiza el WebView2
        //            inversiones = flujosFinancierosViewModel.GetAllInversiones();
        //            GenerarPDFALLInversiones();
        //            break;
        //    }
        //}

        //private void FinanciamientosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
        //    var selectedMethod = ((ComboBoxItem)FinanciamientosComboBox.SelectedItem).Content.ToString();

        //    switch (selectedMethod)
        //    {
        //        case "GetAllFinanciamientos":
        //            // Llama al método para obtener todos los préstamos y actualiza el WebView2
        //            financiamientos = flujosFinancierosViewModel.GetAllFinanciamientos();
        //            GenerarPDFALLFinanciamientos();
        //            break;
        //    }
        //}

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            GenerarPDF();
        }

        private void NombreEmpresaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
            //var selectedEmpresa = ((ComboBoxItem)NombreEmpresaComboBox.SelectedItem).Content.ToString();
            string selectedEmpresa = NombreEmpresaComboBox.SelectedItem as string;

            if (selectedEmpresa != null)
            {
                // Aquí puedes manejar la selección, por ejemplo, cargar financiamientos relacionados
                financiamientos = flujosFinancierosViewModel.GetFinanciamientosByNombreEmpresa(selectedEmpresa);
                GenerarPDFALLFinanciamientos();
            }
        }

        private void FinancieraComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
            //var selectedEmpresa = ((ComboBoxItem)NombreEmpresaComboBox.SelectedItem).Content.ToString();
            string selectedFinanciera = FinancieraComboBox.SelectedItem as string;

            if (selectedFinanciera != null)
            {
                // Aquí puedes manejar la selección, por ejemplo, cargar financiamientos relacionados
                inversiones = flujosFinancierosViewModel.GetInversionesByFinanciera(selectedFinanciera);
                GenerarPDFALLInversiones();
            }
        }

        private void PrestamoEmpleadoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el Id del empleado seleccionado
            if (PrestamoEmpleadoComboBox.SelectedItem != null)
            {
                string selectedIdString = PrestamoEmpleadoComboBox.SelectedItem as string;
                int selectedId;

                // Intenta convertir el string a int
                if (int.TryParse(selectedIdString, out selectedId))
                {
                    // Llama al método para obtener los préstamos por Id
                    var prestamos = flujosFinancierosViewModel.GetPrestamosByIdUsuario(selectedId);
                    GenerarPDF();  
                }
                else
                {
                    // Maneja el caso en que la conversión falle
                    MessageBox.Show("El ID seleccionado no es válido.");
                }
            }
        }

        private void verPrestamos(object sender, RoutedEventArgs e)
        {
            FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
            //var selectedMethod = ((ComboBoxItem)PrestamosComboBox.SelectedItem).Content.ToString();

            // Llama al método para obtener todos los préstamos y actualiza el WebView2
            prestamos = flujosFinancierosViewModel.GetAllPrestamos();
            GenerarPDF();

        }

        private void verInversiones(object sender, RoutedEventArgs e)
        {
            FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
            //var selectedMethod = ((ComboBoxItem)PrestamosComboBox.SelectedItem).Content.ToString();

            // Llama al método para obtener todos los préstamos y actualiza el WebView2
            inversiones = flujosFinancierosViewModel.GetAllInversiones();
            GenerarPDFALLInversiones();
        }

        private void verFinanciamientos(object sender, RoutedEventArgs e)
        {
            FlujosFinancierosViewModel flujosFinancierosViewModel = new FlujosFinancierosViewModel();
            //var selectedMethod = ((ComboBoxItem)PrestamosComboBox.SelectedItem).Content.ToString();

            // Llama al método para obtener todos los préstamos y actualiza el WebView2
            financiamientos = flujosFinancierosViewModel.GetAllFinanciamientos();
            GenerarPDFALLFinanciamientos();
        }
    }
}



