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
    /// Interaction logic for FlujosFinancieros.xaml
    /// </summary>
    public partial class FlujosFinancieros : Page
    {
        public FlujosFinancieros()
        {
            InitializeComponent();
            CargarDatos();

        }
        private void CargarDatos()
        {
            // Obtener la lista de flujos financieros desde el método ObtenerFlujosFinancieros de DatosFlujoFinanciero
            var flujos = DatosFlujoFinanciero.ObtenerFlujosFinancieros();

            // Establecer el contexto de datos del DataGrid
            dataGrid2.ItemsSource = flujos;
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public static class DatosFlujoFinanciero
    {
        public static List<FlujoFinanciero> ObtenerFlujosFinancieros()
        {
            return new List<FlujoFinanciero>()
            {
                new FlujoFinanciero()
                {
                    NumeroCaja = 1,
                    MetodosPago = "Efectivo, Tarjeta",
                    Vueltos = 50.0m,
                    Impuestos = 10.0m,
                    Estado = "Cerrado",
                    FechaInicio = new DateTime(2022, 4, 1, 9, 0, 0),
                    FechaCierre = new DateTime(2022, 4, 1, 17, 0, 0),
                    NombreCajero = "Juan Perez"
                },
                new FlujoFinanciero()
                {
                    NumeroCaja = 2,
                    MetodosPago = "Efectivo",
                    Vueltos = 20.0m,
                    Impuestos = 5.0m,
                    Estado = "Abierto",
                    FechaInicio = new DateTime(2022, 4, 2, 10, 0, 0),
                    FechaCierre = DateTime.MinValue, // Indica que aún no se ha cerrado
                    NombreCajero = "Maria Lopez"
                },
                new FlujoFinanciero()
                {
                    NumeroCaja = 3,
                    MetodosPago = "Tarjeta",
                    Vueltos = 30.0m,
                    Impuestos = 8.0m,
                    Estado = "Cerrado",
                    FechaInicio = new DateTime(2022, 4, 3, 11, 0, 0),
                    FechaCierre = new DateTime(2022, 4, 3, 16, 30, 0),
                    NombreCajero = "Pedro Ramirez"
                },
                // Agrega más flujos financieros si lo deseas
            };
        }
    }

    public class FlujoFinanciero
    {
        public int NumeroCaja { get; set; }
        public string MetodosPago { get; set; }
        public decimal Vueltos { get; set; }
        public decimal Impuestos { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }
        public string NombreCajero { get; set; }
    }
}
