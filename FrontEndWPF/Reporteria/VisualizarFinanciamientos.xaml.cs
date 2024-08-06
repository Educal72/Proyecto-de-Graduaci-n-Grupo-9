using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FrontEndWPF.Reporteria
{
    public partial class VisualizarFinanciamientos : UserControl
	{
        public VisualizarFinanciamientos()
        {
            InitializeComponent();
            CargarDatosFinanciamiento();
        }

        private void CargarDatosFinanciamiento()
        {
            // Obtener la lista de financiamientos desde el método ObtenerFinanciamientos de DatosFinanciamientos
            var financiamientos = DatosFinanciamientos.ObtenerFinanciamientos();

            // Establecer el contexto de datos del DataGrid
            financiamientosDataGrid.ItemsSource = financiamientos;
        }

        private void Descargar_Click(object sender, RoutedEventArgs e)
        {
            // Agregar código para descargar el financiamiento seleccionado
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Implementar la lógica para filtrar los financiamientos activos
        }

        private void financiamientosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Implementar la lógica para mostrar los detalles del financiamiento seleccionado
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            // Implementar la lógica para abrir la ventana de edición
            MessageBox.Show("Editar financiamiento seleccionado.");
        }
    }

    public static class DatosFinanciamientos
    {
        public static List<Financiamiento> ObtenerFinanciamientos()
        {
            return new List<Financiamiento>()
            {
                new Financiamiento()
                {
                    NombreEmpresa = "Empresa A",
                    Motivo = "Compra de equipo",
                    FechaFinanciacion = new DateTime(2022, 1, 10),
                    Estado = "Activo",
                    MontoPendiente = 5000.00m,
                    MontoCancelado = 2000.00m,
                    Intereses = 300.00m
                },
                new Financiamiento()
                {
                    NombreEmpresa = "Empresa B",
                    Motivo = "Expansión de instalaciones",
                    FechaFinanciacion = new DateTime(2022, 2, 15),
                    Estado = "Inactivo",
                    MontoPendiente = 10000.00m,
                    MontoCancelado = 6000.00m,
                    Intereses = 800.00m
                },
                new Financiamiento()
                {
                    NombreEmpresa = "Empresa C",
                    Motivo = "Inversión en tecnología",
                    FechaFinanciacion = new DateTime(2022, 3, 20),
                    Estado = "Activo",
                    MontoPendiente = 7500.00m,
                    MontoCancelado = 3000.00m,
                    Intereses = 450.00m
                },
                // Agrega más financiamientos si lo deseas
            };
        }
    }

    public class Financiamiento
    {
        public string NombreEmpresa { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaFinanciacion { get; set; }
        public string Estado { get; set; }
        public decimal MontoPendiente { get; set; }
        public decimal MontoCancelado { get; set; }
        public decimal Intereses { get; set; }
    }
}
