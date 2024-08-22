using FrontEndWPF.Empleados;
using FrontEndWPF.Index;
using FrontEndWPF.Modelos;
using FrontEndWPF.ViewModel;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.Modelos.UserModel;
using static FrontEndWPF.ViewModel.AuditoriaViewModel;
using static System.Collections.Specialized.BitVector32;
using Image = iText.Layout.Element.Image;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;
namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for InformeNormativa.xaml
    /// </summary>
    public partial class InformeNormativa : UserControl
    {
        Conexion conexion = new Conexion();
        InicioSesionViewModel Pagina = new InicioSesionViewModel();


        static List<InfoAuditoria>? entidadAuditorias;
        static List<Impuestos_InfoAuditoria>? listaImpuestos;
        static List<Ingresos_InfoAuditoria>? listaIngresos;

        static List<Gastos_InfoAuditoria>? listaGastos;
        static List<Retenciones_InfoAuditoria>? listaRetenciones;
        
        static List<EstadoFinanciero_InfoAuditoria>? listaEstadoFinanciero;
        static List<DatosEmpleado_InfoAuditoria>? listaDatosEmpleado;

        static string Impuestos;
        static string Ingresos;
        static string Gastos;
        static string Retenciones;
        static string EstadoFinanciero;
        static string DatosEmpleado;
        static bool seProsigue;
        /*
     * INGRESOS -> TOTAL DE FACTURA
     * LOS GASTOS -> MONTO TOTAL Y EL INVERTIDO.
     * RETENCIONES -> SOLO LA SUMA DE LOS SALARIOS DE LOS EMPLEADOS
     * ESTADO FINANCIERO -> INGRESOS - GASTOS.
     * DATOS DEL EMPLEADO.
     */
        public InformeNormativa()
        {
            InitializeComponent();
            seProsigue = true;
            ImpuestosDataGridTextColumn.Visibility = Visibility.Collapsed;
            IngresosDataGridTextColumn.Visibility = Visibility.Collapsed;
            GastosDataGridTextColumn.Visibility = Visibility.Collapsed;

            RetencionesDataGridTextColumn.Visibility = Visibility.Collapsed;
            EstadoFinancieroDataGridTextColumn.Visibility = Visibility.Collapsed;
            DatosEmpleadoDataGridTextColumn.Visibility = Visibility.Collapsed;
        }


        public void CargarDatos()
        {
            if(seProsigue == true)
            {
                ImpuestosDataGridTextColumn.Visibility = Visibility.Visible;
                IngresosDataGridTextColumn.Visibility = Visibility.Visible;
                GastosDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (listaImpuestos != null)
            {
                foreach (var listas in listaImpuestos!)
                {
                    Impuestos = listas.Impuestos!;
                }
            }

            if (listaIngresos != null)
            {
                foreach (var listas in listaIngresos!)
                {
                    Ingresos = listas.Ingresos!;
                }
            }

            if (listaGastos != null)
            {
                foreach (var listas in listaGastos!)
                {
                    Gastos = listas.Gastos!;
                }
            }

            if (listaRetenciones != null)
            {
                foreach (var listas in listaRetenciones!)
                {
                    Retenciones = listas.Retenciones!;
                }
            }

            if (listaEstadoFinanciero != null)
            {
                foreach (var listas in listaEstadoFinanciero!)
                {
                    EstadoFinanciero = listas.EstadoFinanciero!;
                }
            }

            if (listaDatosEmpleado != null)
            {
                foreach (var listas in listaDatosEmpleado!)
                {
                    DatosEmpleado = listas.DatosEmpleado!;
                }
            }


            string query = @"
                SELECT 
                    FACT.Impuestos,
	                FACT.Total,
                    PRE.MontoTotal,
	                INV.MontoInvertido
                FROM
	                Factura FACT INNER JOIN Prestamos PRE
	                ON FACT.Id = PRE.Id INNER JOIN Inversiones INV
                    ON PRE.Id = INV.Id;";

            List<InfoAuditoria> entidadAuditoria = new List<InfoAuditoria>();
            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        entidadAuditoria.Add(new InfoAuditoria
                        {
                            Impuestos = Convert.ToString(Impuestos),
                            Ingresos = Convert.ToString(Ingresos),
                            Gastos = Convert.ToString(Gastos),
                            Retenciones = Convert.ToString(Retenciones),
                            EstadoFinanciero = Convert.ToString(EstadoFinanciero),
                            DatosEmpleado = Convert.ToString(DatosEmpleado)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            normativaDataGrid.ItemsSource = entidadAuditoria;
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.Impuestos != null);
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.Ingresos != null);
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.Gastos != null);
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.Retenciones != null);
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.EstadoFinanciero != null);
            entidadAuditorias = entidadAuditoria.FindAll(itemLista => itemLista.DatosEmpleado != null);

        }

        public void NoCargaDatos()
        {
            Impuestos = null;
            Ingresos = null;
            Gastos = null;
            Retenciones = null;
            EstadoFinanciero = null;
            DatosEmpleado = null;

            if (listaImpuestos != null && listaImpuestos.Count != 0)
            {
                listaImpuestos!.Clear();
            }

            if (listaIngresos != null && listaIngresos.Count != 0)
            {
                listaIngresos!.Clear();
            }

            if (listaGastos != null && listaGastos.Count != 0)
            {
                listaGastos!.Clear();
            }

            if (listaRetenciones != null && listaRetenciones.Count != 0)
            {
                listaRetenciones!.Clear();
            }

            if (listaEstadoFinanciero != null && listaEstadoFinanciero.Count != 0)
            {
                listaEstadoFinanciero!.Clear();
            }

            if (listaDatosEmpleado != null && listaDatosEmpleado.Count != 0)
            {
                listaDatosEmpleado!.Clear();
            }

            normativaDataGrid.ItemsSource = null;
            ImpuestosDataGridTextColumn.Visibility = Visibility.Collapsed;
            IngresosDataGridTextColumn.Visibility = Visibility.Collapsed;
            GastosDataGridTextColumn.Visibility = Visibility.Collapsed;
            RetencionesDataGridTextColumn.Visibility = Visibility.Collapsed;
            EstadoFinancieroDataGridTextColumn.Visibility = Visibility.Collapsed;
            DatosEmpleadoDataGridTextColumn.Visibility = Visibility.Collapsed;


        }



        private void Generar_Informe_Click(object sender, RoutedEventArgs e)
        {
            CargarDatos();
        }

        private void Cambiar_Informe_Click(object sender, RoutedEventArgs e)
        {
            InfoAuditoria? selectedInfoNormativa = normativaDataGrid.SelectedItem as InfoAuditoria;
            if (selectedInfoNormativa == null)
            {
                MessageBox.Show("Por favor, seleccione un informe antes de proceder.", "¡Advertencia!", MessageBoxButton.OK,
                     MessageBoxImage.Warning);
                return;
            }


            if (selectedInfoNormativa != null)
            {
                var cambiarInformeNormativa = new CambiarInforme();
                cambiarInformeNormativa.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                cambiarInformeNormativa.Impuestos.Text = selectedInfoNormativa.RespImpuestos;
                cambiarInformeNormativa.Ingresos.Text = selectedInfoNormativa.RespIngresos;
                cambiarInformeNormativa.Gastos.Text = selectedInfoNormativa.RespGastos;
                cambiarInformeNormativa.Retenciones.Text = selectedInfoNormativa.RespRetenciones;
                cambiarInformeNormativa.Estado_Financiero.Text = selectedInfoNormativa.RespEstadoFinanciero;
                cambiarInformeNormativa.Datos_Empleado.Text = selectedInfoNormativa.RespDatosEmpleado;


                if (cambiarInformeNormativa.ShowDialog() == true)
                {
                    selectedInfoNormativa.RespImpuestos = cambiarInformeNormativa.impuestos_cambiarInforme;
                    selectedInfoNormativa.RespIngresos = cambiarInformeNormativa.ingresos_cambiarInforme;
                    selectedInfoNormativa.RespGastos = cambiarInformeNormativa.gastos_cambiarInforme;
                    selectedInfoNormativa.RespRetenciones = cambiarInformeNormativa.retenciones_cambiarInforme;
                    selectedInfoNormativa.RespEstadoFinanciero = cambiarInformeNormativa.estadoFinanciero_cambiarInforme;
                    selectedInfoNormativa.RespDatosEmpleado = cambiarInformeNormativa.datosEmpleado_cambiarInforme;

                    if (selectedInfoNormativa.RespImpuestos is not null)
                    {
                        ManejoFiltrosImpuestos(selectedInfoNormativa.RespImpuestos);
                    }

                    if (selectedInfoNormativa.RespIngresos is not null)
                    {
                        ManejoFiltrosIngresos(selectedInfoNormativa.RespIngresos);
                    }

                    if (selectedInfoNormativa.RespGastos is not null)
                    {
                        ManejoFiltrosGastos(selectedInfoNormativa.RespGastos);
                    }

                    if (selectedInfoNormativa.RespRetenciones is not null)
                    {
                        ManejoFiltrosRetenciones(selectedInfoNormativa.RespRetenciones);
                    }

                    if (selectedInfoNormativa.RespEstadoFinanciero is not null)
                    {
                        ManejoFiltrosEstadoFinanciero(selectedInfoNormativa.RespEstadoFinanciero);
                    }

                    if (selectedInfoNormativa.RespDatosEmpleado is not null)
                    {
                        ManejoFiltrosDatosEmpleado(selectedInfoNormativa.RespDatosEmpleado);
                    }

                    CargarDatos();
                }
            }
        }


        public void ManejoFiltrosImpuestos(string RespImpuestos)
        {
            if (RespImpuestos.Equals("Total"))
            {
                string query = @"
                    SELECT 
	                    SUM(Impuestos) as Impuestos
                    FROM
	                    Factura;";

                List<Impuestos_InfoAuditoria> impuestos_InfoAuditorias = new List<Impuestos_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal TotalImpuestos = Convert.ToDecimal(reader["Impuestos"]);
                            impuestos_InfoAuditorias.Add(new Impuestos_InfoAuditoria
                            {
                                Impuestos = Convert.ToString(TotalImpuestos)
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                listaImpuestos = impuestos_InfoAuditorias.FindAll(itemLista => itemLista.Impuestos != null);
            }


            if (RespImpuestos.Equals("Mostrar"))
            {
                seProsigue = false;
                ImpuestosDataGridTextColumn.Visibility = Visibility.Visible;
            }


            if (RespImpuestos.Equals("No mostrar"))
            {
                seProsigue = false;
                ImpuestosDataGridTextColumn.Visibility = Visibility.Collapsed;
            }
        }

        public void ManejoFiltrosIngresos(string RespIngresos)
        {

            if (RespIngresos.Equals("Total"))
            {

                string query = @"
                    SELECT 
	                    SUM(Total) as TotalIngresos
                    FROM
	                    Factura;";

                List<Ingresos_InfoAuditoria> ingresos_InfoAuditorias = new List<Ingresos_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal TotalIngresos = Convert.ToDecimal(reader["TotalIngresos"]);
                            ingresos_InfoAuditorias.Add(new Ingresos_InfoAuditoria
                            {
                                Ingresos = Convert.ToString(TotalIngresos)
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                listaIngresos = ingresos_InfoAuditorias.FindAll(itemLista => itemLista.Ingresos != null);
            }

            if (RespIngresos.Equals("Mostrar"))
            {
                seProsigue = false;
                IngresosDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (RespIngresos.Equals("No mostrar"))
            {
                seProsigue = false;
                IngresosDataGridTextColumn.Visibility = Visibility.Collapsed;
            }

        }



        public void ManejoFiltrosGastos(string RespGastos)
        {

            if (RespGastos.Equals("Total"))
            {
                string query = @"
                SELECT 
                    PRE.MontoTotal,
	                INV.MontoInvertido
                FROM
	                Inversiones INV INNER JOIN Prestamos PRE
                    ON PRE.Id = INV.Id;";

                List<Gastos_InfoAuditoria> gastos_InfoAuditorias = new List<Gastos_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal TotalGastos = Convert.ToDecimal(reader["MontoTotal"]) + 
                                Convert.ToDecimal(reader["MontoInvertido"]);

                            gastos_InfoAuditorias.Add(new Gastos_InfoAuditoria
                            {
                                Gastos = Convert.ToString(TotalGastos)
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                listaGastos = gastos_InfoAuditorias.FindAll(itemLista => itemLista.Gastos != null);
            }

            if (RespGastos.Equals("Mostrar"))
            {
                seProsigue = false;
                GastosDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (RespGastos.Equals("No mostrar"))
            {
                seProsigue = false;
                GastosDataGridTextColumn.Visibility = Visibility.Collapsed;
            }

        }

        public void ManejoFiltrosRetenciones(string RespRetenciones)
        {
            if (RespRetenciones.Equals("Total"))
            {
                string query = @"
                    SELECT
	                    SUM(Salario) SalarioEmpleados
                    FROM
	                    Empleado EMP;";

                List<Retenciones_InfoAuditoria> retenciones_InfoAuditorias = new List<Retenciones_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal TotalSalarios = Convert.ToDecimal(reader["SalarioEmpleados"]);
                            retenciones_InfoAuditorias.Add(new Retenciones_InfoAuditoria
                            {
                                Retenciones = Convert.ToString(TotalSalarios)
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                listaRetenciones = retenciones_InfoAuditorias.FindAll(itemLista => itemLista.Retenciones != null);

            }

            if (RespRetenciones.Equals("Mostrar"))
            {
                RetencionesDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (RespRetenciones.Equals("No mostrar"))
            {
                RetencionesDataGridTextColumn.Visibility = Visibility.Collapsed;
            }

        }



        public void ManejoFiltrosEstadoFinanciero(string RespEstadoFinanciero)
        {

            if (RespEstadoFinanciero.Equals("Total"))
            {
                string query = @"
                    SELECT
	                    FACT.Total,
                        PRE.MontoTotal,
	                    INV.MontoInvertido
                    FROM
	                    Factura FACT INNER JOIN Prestamos PRE
                        ON FACT.Id = PRE.Id INNER JOIN Inversiones INV 
	                    ON PRE.Id = INV.Id ;";

                List<EstadoFinanciero_InfoAuditoria> estadoFinanciero_InfoAuditorias = new List<EstadoFinanciero_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            decimal TotalIngresos = Convert.ToDecimal(reader["Total"]);
                            decimal TotalGastos = Convert.ToDecimal(reader["MontoTotal"]) + Convert.ToDecimal(reader["MontoInvertido"]);
                            decimal TotalFinanciero = TotalIngresos - TotalGastos;

                            estadoFinanciero_InfoAuditorias.Add(new EstadoFinanciero_InfoAuditoria
                            {
                                EstadoFinanciero = Convert.ToString(TotalFinanciero)
                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                listaEstadoFinanciero = estadoFinanciero_InfoAuditorias.FindAll(itemLista => itemLista.EstadoFinanciero != null);
            }

            if (RespEstadoFinanciero.Equals("Mostrar"))
            {
                EstadoFinancieroDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (RespEstadoFinanciero.Equals("No mostrar"))
            {
                EstadoFinancieroDataGridTextColumn.Visibility = Visibility.Collapsed;
            }
        }

        public void ManejoFiltrosDatosEmpleado(string RespDatosEmpleados)
        {

            if (RespDatosEmpleados.Equals("Total"))
            {
                string query = @"
                    SELECT
                        US.Nombre,
	                    US.Apellido,
	                    US.Cedula,
	                    US.Telefono,
                        US.Correo,
                        EMP.Puesto
                        
                    FROM
	                    Empleado EMP INNER JOIN Usuario US
	                    ON EMP.IdUsuario = US.Id;";

                List<DatosEmpleado_InfoAuditoria> datosEmpleado_InfoAuditorias = new List<DatosEmpleado_InfoAuditoria>();
                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            datosEmpleado_InfoAuditorias.Add(new DatosEmpleado_InfoAuditoria
                            {
                                DatosEmpleado = "Empleados: \n" + reader["Nombre"] + " " + reader["Apellido"] + "\nTeléfono: " + reader["Telefono"] + "\nPuesto: " +
                                reader["Puesto"] + "\nCorreo: " + reader["Correo"]

                            });

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

                listaDatosEmpleado = datosEmpleado_InfoAuditorias.FindAll(itemLista => itemLista.DatosEmpleado != null);

            }

            if (RespDatosEmpleados.Equals("Mostrar"))
            {
                DatosEmpleadoDataGridTextColumn.Visibility = Visibility.Visible;
            }

            if (RespDatosEmpleados.Equals("No mostrar"))
            {
                DatosEmpleadoDataGridTextColumn.Visibility = Visibility.Collapsed;
            }

        }


        private void Descartar_Informe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Está seguro de descartar el informe",
            "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                NoCargaDatos();
            }
        }

        private void DescargarNormativa_Click(object sender, RoutedEventArgs e)
        {
            // Ruta base para guardar el archivo PDF
            string rutaBase = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string nombreArchivoBase = "Informe - Normativa";
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
                iText.Layout.Element.Image image = new Image(imageData)
                    .ScaleToFit(120, 120); // Ajustar tamaño de la imagen

                // Crear una tabla para el encabezado con dos columnas
                Table headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }));
                headerTable.SetWidth(UnitValue.CreatePercentValue(100));

                // Primera celda con los subtítulos
                Cell textCell = new Cell()
                    .Add(new Paragraph("Informe de normativa").SetFont(boldFont).SetFontSize(14))
                    .Add(new Paragraph("Empresa: El Molino").SetFont(normalFont).SetFontSize(12))
                    .Add(new Paragraph($"Fecha de Impresión: {DateTime.Now:yyyy-MM-dd}").SetFont(normalFont).SetFontSize(12))
                    .Add(new Paragraph($"Hora de Impresión: {DateTime.Now:hh:mm:ss tt}").SetFont(normalFont).SetFontSize(12))
                    .Add(new Paragraph("\n")); // Espacio                
                textCell.SetPadding(0);
                headerTable.AddCell(textCell);

                // Segunda celda con la imagen
                Cell imageCell = new Cell().Add(image);
                imageCell.SetPaddingRight(20);
                headerTable.AddCell(imageCell);

                // Añadir la tabla de encabezado al documento
                document.Add(headerTable);

                // Encabezados de la tabla
                var table = new Table(6); // 4 columnas
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total de impuestos").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total de ingresos").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Total de gastos").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Retenciones").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Estado financiero").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Datos de empleados").SetFont(boldFont)));

                // Datos de la tabla
                if (entidadAuditorias != null)
                {
                    foreach (var listas in entidadAuditorias!)
                    {
                        if (listas.Impuestos != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.Impuestos).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                        if (listas.Ingresos != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.Ingresos).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                        if (listas.Gastos != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.Gastos).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                        if (listas.Retenciones != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.Retenciones).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                        if (listas.EstadoFinanciero != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.EstadoFinanciero).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                        if (listas.DatosEmpleado != null)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(listas.DatosEmpleado).SetFont(normalFont)));
                        }
                        else
                        {
                            table.AddCell(new Cell().Add(new Paragraph("N/A").SetFont(normalFont)));
                        }


                    }
                }

                document.Add(table);

                // Añadir números de página
                Pagina.AddPageNumbers(pdf);
            }
            entidadAuditorias.Clear();
            MessageBox.Show($"El informe se ha generado y guardado en: {archivoPdf}", "Informe de Normativa - Generado", MessageBoxButton.OK, MessageBoxImage.Information);
        }




    }
}
