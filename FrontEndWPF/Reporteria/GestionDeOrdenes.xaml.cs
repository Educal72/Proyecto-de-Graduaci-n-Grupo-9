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
    /// Interaction logic for GestionDeOrdenes.xaml
    /// </summary>
    public partial class GestionDeOrdenes : UserControl
	{
        public GestionDeOrdenes()
        {
            InitializeComponent();
            CargarDatos();

        }

        private void CargarDatos()
        {
            // Obtener la lista de órdenes desde el método ObtenerOrdenes de DatosOrden
            var ordenes = DatosOrden.ObtenerOrdenes();

            // Establecer el contexto de datos del DataGrid
            dataGrid1.ItemsSource = ordenes;
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GenerarInforme_Click(object sender, RoutedEventArgs e)
        {
            // Aquí puedes colocar la lógica para abrir el modal con las opciones de selección de fecha
            // Por ejemplo:
            // abrirModalSeleccionFecha();
            //MessageBox.Show("Botón Generar Informe clickeado");
            informePopup.IsOpen = true; // Abrir Modal

        }

        private void CerrarInformePopup_Click(object sender, RoutedEventArgs e)
        {
            informePopup.IsOpen = false;
        }
    }
    public static class DatosOrden
    {
        public static List<Orden> ObtenerOrdenes()
        {
            return new List<Orden>()
            {
                new Orden()
                {
                    NumeroOrden = 1,
                    ProductosIncluidos = "Producto A, Producto B",
                    FechaOrden = new DateTime(2022, 4, 1),
                    Monto = 100.50m,
                    Estado = "Completada",
                    NombreCajero = "Juan Perez"
                },
                new Orden()
                {
                    NumeroOrden = 2,
                    ProductosIncluidos = "Producto C, Producto D",
                    FechaOrden = new DateTime(2022, 4, 2),
                    Monto = 75.25m,
                    Estado = "Pendiente",
                    NombreCajero = "Maria Lopez"
                },
                new Orden()
                {
                    NumeroOrden = 3,
                    ProductosIncluidos = "Producto E, Producto F",
                    FechaOrden = new DateTime(2022, 4, 3),
                    Monto = 120.75m,
                    Estado = "Completada",
                    NombreCajero = "Pedro Ramirez"
                },
            };
        }
    }

    public class Orden
    {
        public int NumeroOrden { get; set; }
        public string ProductosIncluidos { get; set; }
        public DateTime FechaOrden { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string NombreCajero { get; set; }
    }
}
