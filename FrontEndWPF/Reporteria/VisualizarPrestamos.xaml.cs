using System;
using System.Collections.Generic;
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

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for VisualizarPrestamos.xaml
    /// </summary>
    public partial class VisualizarPrestamos : Page
    {
        public VisualizarPrestamos()
        {
            InitializeComponent();
            CargarDatosPrestamos();

        }
        private void CargarDatosPrestamos()
        {
            // Obtener la lista de préstamos desde el método ObtenerPrestamos de DatosPrestamos
            var prestamos = DatosPrestamos.ObtenerPrestamos();

            // Establecer el contexto de datos del DataGrid
            prestamosDataGrid.ItemsSource = prestamos;
        }

        private void Descargar_Click(object sender, RoutedEventArgs e)
        {
            // Agregar código para descargar el préstamo seleccionado
        }
    }

    public static class DatosPrestamos
    {
        public static List<Prestamo> ObtenerPrestamos()
        {
            return new List<Prestamo>()
            {
                new Prestamo()
                {
                    Entidad = "Entidad A",
                    MontoTotal = 10000.00m,
                    MontoPendiente = 5000.00m,
                    InteresesMoratorios = 200.00m,
                    Estado = "Activo",
                    FechaCreacion = new DateTime(2022, 1, 10)
                },
                new Prestamo()
                {
                    Entidad = "Entidad B",
                    MontoTotal = 15000.00m,
                    MontoPendiente = 7500.00m,
                    InteresesMoratorios = 300.00m,
                    Estado = "Activo",
                    FechaCreacion = new DateTime(2022, 2, 15)
                },
                new Prestamo()
                {
                    Entidad = "Entidad C",
                    MontoTotal = 20000.00m,
                    MontoPendiente = 10000.00m,
                    InteresesMoratorios = 400.00m,
                    Estado = "Inactivo",
                    FechaCreacion = new DateTime(2022, 3, 20)
                }
                // Agrega más préstamos si lo deseas
            };
        }
    }

    public class Prestamo
    {
        public string Entidad { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPendiente { get; set; }
        public decimal InteresesMoratorios { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
