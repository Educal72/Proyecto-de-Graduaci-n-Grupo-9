using FrontEndWPF.Modelos;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using iText.Kernel.Font;
using iText.Kernel.Events;
using System;
using iText.IO.Font.Constants;
using iText.Kernel.Geom;
using iText.Layout.Borders;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.IO.Image;
using System.Data.SqlClient;
using HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;



namespace FrontEndWPF.ViewModel
{
    public class InicioSesionViewModel : INotifyPropertyChanged
    {
        private Conexion conexion = new Conexion();
        private ObservableCollection<InicioSesion> _inicioSesiones;
        private ObservableCollection<InicioSesion> _registros;

        public ObservableCollection<InicioSesion> InicioSesiones
        {
            get { return _inicioSesiones; }
            set
            {
                _inicioSesiones = value;
                OnPropertyChanged("InicioSesiones");
            }
        }

        public ObservableCollection<InicioSesion> Registros
        {
            get { return _registros; }
            set
            {
                _registros = value;
                OnPropertyChanged("Registros");
            }
        }

        public InicioSesionViewModel()
        {
            _inicioSesiones = new ObservableCollection<InicioSesion>();
            _registros = new ObservableCollection<InicioSesion>();
            LoadInicioSesiones();
        }

        private void LoadInicioSesiones()
        {
            _inicioSesiones.Clear();
            Conexion conexion = new Conexion();
            var sesionesList = conexion.ListarInicioSesion();

            foreach (var sesionDict in sesionesList)
            {
                InicioSesion sesion = new InicioSesion
                {
                    IdUsuario = (int)sesionDict["IdUsuario"],
                    FechaIngreso = (DateTime)sesionDict["FechaIngreso"],
                    FechaInicioSesion = (DateTime)sesionDict["FechaInicioSesion"],
                    UltimaDesconexion = sesionDict["UltimaDesconexion"] != null ? (DateTime)sesionDict["UltimaDesconexion"] : (DateTime?)null
                };
                _inicioSesiones.Add(sesion);
            }
        }

        public void CargarRegistrosPorFecha(DateTime fecha)
        {
            _registros.Clear();
            Conexion conexion = new Conexion();
            var registrosList = conexion.BuscarInicioSesionPorFecha(fecha);

            foreach (var registroDict in registrosList)
            {
                InicioSesion registro = new InicioSesion
                {
                    IdUsuario = (int)registroDict["IdUsuario"],
                    FechaIngreso = (DateTime)registroDict["FechaIngreso"],
                    FechaInicioSesion = (DateTime)registroDict["FechaInicioSesion"],
                    UltimaDesconexion = registroDict.ContainsKey("UltimaDesconexion")
                                        ? (DateTime?)registroDict["UltimaDesconexion"]
                                        : null
                };
                _registros.Add(registro);
            }
        }

		public void GenerarPDF()
		{
			// Ruta base para guardar el archivo PDF
			string rutaBase = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string nombreArchivoBase = "InformeInicioSesion";
			string extensionArchivo = ".pdf";

			// Encontrar el siguiente número consecutivo disponible
			int numeroConsecutivo = 1;
			string archivoPdf;
			do
			{
				archivoPdf = System.IO.Path.Combine(rutaBase, $"{nombreArchivoBase}_{numeroConsecutivo}{extensionArchivo}");
				numeroConsecutivo++;
			}
			while (File.Exists(archivoPdf));

			// Generar el archivo PDF
			using (var writer = new PdfWriter(archivoPdf))
			using (var pdf = new PdfDocument(writer))
			using (var document = new Document(pdf))
			{
				// Crear fuentes para negrita y normal
				PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
				PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

				// Ruta de la imagen
				string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/323715758_861857158399502_6847062045596271805_n-removebg-preview.png";
				ImageData imageData = ImageDataFactory.Create(imagePath);
				Image image = new Image(imageData)
					.ScaleToFit(120, 120); // Ajustar tamaño de la imagen

				// Crear una tabla para el encabezado con dos columnas
				Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }));
				headerTable.SetWidth(UnitValue.CreatePercentValue(100));

				// Primera celda con los subtítulos
				Cell textCell = new Cell()
					.Add(new Paragraph("Informes de Inicios de Sesión").SetFont(boldFont).SetFontSize(14))
					.Add(new Paragraph("Empresa: El Molino").SetFont(normalFont).SetFontSize(12))
					.Add(new Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
					.Add(new Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
					.Add(new Paragraph($"Total de Registros: {_inicioSesiones.Count}").SetFont(normalFont).SetFontSize(12))
					.Add(new Paragraph("\n")); // Espacio
				textCell.SetBorder(Border.NO_BORDER);
				textCell.SetPadding(0);
				headerTable.AddCell(textCell);

				// Segunda celda con la imagen
				Cell imageCell = new Cell().Add(image);
				imageCell.SetBorder(Border.NO_BORDER);
				imageCell.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
				imageCell.SetVerticalAlignment(VerticalAlignment.TOP);
				imageCell.SetPaddingRight(20);
				headerTable.AddCell(imageCell);

				// Añadir la tabla de encabezado al documento
				document.Add(headerTable);

				// Encabezados de la tabla
				var table = new Table(4); // 4 columnas
				table.AddHeaderCell(new Cell().Add(new Paragraph("Id Usuario").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha de Ingreso").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new Paragraph("Fecha de Inicio de Sesión").SetFont(boldFont)));
				table.AddHeaderCell(new Cell().Add(new Paragraph("Última Fecha de Desconexión").SetFont(boldFont)));

				// Datos de la tabla
				foreach (var sesion in _inicioSesiones)
				{
					table.AddCell(new Cell().Add(new Paragraph(sesion.IdUsuario.ToString()).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new Paragraph(sesion.FechaIngreso.ToString("yyyy-MM-dd")).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new Paragraph(sesion.FechaInicioSesion.ToString("yyyy-MM-dd")).SetFont(normalFont)));
					table.AddCell(new Cell().Add(new Paragraph(sesion.UltimaDesconexion.HasValue ?
						sesion.UltimaDesconexion.Value.ToString("yyyy-MM-dd") : "N/A").SetFont(normalFont)));
				}

				document.Add(table);

				// Añadir números de página
				AddPageNumbers(pdf);
			}

			MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe Generado", MessageBoxButton.OK, MessageBoxImage.Information);
		}



		public void AddPageNumbers(PdfDocument pdfDoc)
        {
            int numberOfPages = pdfDoc.GetNumberOfPages();
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            for (int i = 1; i <= numberOfPages; i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                // Crear el Canvas para el PdfCanvas y la página
                Canvas canvas = new Canvas(pdfCanvas, pageSize); // Nota: El constructor solo necesita PdfCanvas y Rectangle

                // Especificar el nombre completo para evitar el conflicto de nombres
                canvas.ShowTextAligned(new Paragraph($"Página {i}").SetFont(font).SetFontSize(12),
                    pageSize.GetWidth() - 50, 20, i, iText.Layout.Properties.TextAlignment.RIGHT, iText.Layout.Properties.VerticalAlignment.BOTTOM, 0);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public bool CrearRegistroInicio(int IdUsuario, DateTime FechaIngreso, DateTime FechaInicioSesion)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "INSERT INTO InicioSesion (IdUsuario, FechaIngreso, FechaInicioSesion) VALUES (@IdUsuario, @FechaIngreso, @FechaInicioSesion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdUsuario", IdUsuario));
                    command.Parameters.Add(new SqlParameter("@FechaIngreso", FechaIngreso));
                    command.Parameters.Add(new SqlParameter("@FechaInicioSesion", FechaInicioSesion));
                    int rowsAffected = (int)command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
        }
        public bool UltimaDesconexion(int id)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                string query = "UPDATE InicioSesion SET UltimaDesconexion = @UltimaDesconexion WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", id));
                    command.Parameters.Add(new SqlParameter("@UltimaDesconexion", DateTime.Now));
                    int rowsAffected = (int)command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
		public int ExisteInicioSesion(int IdUsuario)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"
    SELECT 
        Id
    FROM 
        InicioSesion
    WHERE 
        IdUsuario = @IdUsuario AND
        UltimaDesconexion IS NULL";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdUsuario", IdUsuario));
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							int entradaId = reader.GetInt32(0);
							return entradaId;
						}
						else
						{
							int entradaId = 0;
							return entradaId;
						}
					}

				}
			}
		}
	}
}