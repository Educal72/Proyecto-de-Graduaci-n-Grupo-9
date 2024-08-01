using FrontEndWPF.ViewModel;
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
        private InicioSesionViewModel viewModel;


        public InicioDeSesion()
        {
            InitializeComponent();
            DataContext = new InicioSesionViewModel();
        }


        private void CargarDatos1()
        {
            // Obtener la lista de empleados desde el método ObtenerEmpleados de DatosEmpleado

            // Establecer el contexto de datos del DataGrid
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

            modalPopup.IsOpen = false;
        }

        private void GenerarInforme_Click(object sender, RoutedEventArgs e)
        {

            var viewModel = (InicioSesionViewModel)DataContext;

            if (viewModel == null)
            {
                MessageBox.Show("Error: No se pudo obtener el ViewModel.");
                return;
            }

            viewModel.GenerarPDF();
        }

        private void CerrarInformePopup_Click(object sender, RoutedEventArgs e)
        {
            informePopup.IsOpen = false;
        }

        private void ActualizarInforme_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (InicioSesionViewModel)DataContext;

            if (viewModel == null)
            {
                MessageBox.Show("Error: No se pudo obtener el ViewModel.");
                return;
            }

            if (fechaInforme.SelectedDate.HasValue)
            {
                DateTime fechaSeleccionada = fechaInforme.SelectedDate.Value;
                MessageBox.Show($"Buscando registros para la fecha: {fechaSeleccionada.ToShortDateString()}");
                viewModel.CargarRegistrosPorFecha(fechaSeleccionada);
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show(
                    "No se ha seleccionado una fecha. ¿Desea continuar sin filtrar los registros por fecha?",
                    "Confirmar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (resultado == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Cargando todos los registros.");
                    viewModel.CargarRegistrosPorFecha(DateTime.MinValue); // Puedes manejar el caso de `DateTime.MinValue` como prefieras
                }
            }
        }
    }
}
