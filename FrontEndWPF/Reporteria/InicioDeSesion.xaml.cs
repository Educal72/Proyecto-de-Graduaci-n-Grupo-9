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
    /// Interaction logic for InicioDeSesion.xaml
    /// </summary>
    public partial class InicioDeSesion : Page
    {
        public InicioDeSesion()
        {
            InitializeComponent();
            CargarDatos1();
        }

        private void CargarDatos1()
        {
            // Obtener la lista de empleados desde el método ObtenerEmpleados de DatosEmpleado
            var empleados = DatosEmpleado.ObtenerEmpleados();

            // Establecer el contexto de datos del DataGrid
            dataGrid.ItemsSource = empleados;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            // Abrir el modal
            modalPopup.IsOpen = true;
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {
            // Código para actualizar los datos
            // Puedes acceder a los datos modificados desde las TextBoxes en el modal
            // y realizar las operaciones necesarias para actualizar los datos en tu sistema
            // Una vez que hayas terminado, cierra el modal
            modalPopup.IsOpen = false;
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

    public static class DatosEmpleado
    {
        public static List<Empleado> ObtenerEmpleados()
        {
            return new List<Empleado>()
            {
                new Empleado()
                {
                    Nombre = "Juan Perez",
                    RolAsignado = "Administrador",
                    FechaIngreso = new DateTime(2022, 1, 10),
                    FechaInicioSesion = new DateTime(2022, 1, 10, 9, 0, 0),
                    UltimaFechaDesconexion = new DateTime(2022, 1, 10, 17, 30, 0)
                },
                new Empleado()
                {
                    Nombre = "Maria Lopez",
                    RolAsignado = "Analista",
                    FechaIngreso = new DateTime(2022, 2, 15),
                    FechaInicioSesion = new DateTime(2022, 2, 15, 8, 30, 0),
                    UltimaFechaDesconexion = new DateTime(2022, 2, 15, 18, 0, 0)
                },
                new Empleado()
                {
                    Nombre = "Pedro Ramirez",
                    RolAsignado = "Desarrollador",
                    FechaIngreso = new DateTime(2022, 3, 20),
                    FechaInicioSesion = new DateTime(2022, 3, 20, 10, 15, 0),
                    UltimaFechaDesconexion = new DateTime(2022, 3, 20, 16, 45, 0)
                },
                // Agrega más empleados si lo deseas
            };
        }
    }

    public class Empleado
    {
        public string Nombre { get; set; }
        public string RolAsignado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaInicioSesion { get; set; }
        public DateTime UltimaFechaDesconexion { get; set; }
    }
}
