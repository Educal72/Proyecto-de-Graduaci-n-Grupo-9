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
using FrontEndWPF.Modelos;
using FrontEndWPF.Empleados;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para PermisoTiempo.xaml
	/// </summary>
	public partial class PermisoTiempo : UserControl
	{
        private PermisoDeTiempoViewModel _viewModel;

        public PermisoTiempo()
        {
            InitializeComponent();
            _viewModel = new PermisoDeTiempoViewModel();
            this.DataContext = _viewModel;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
		{
            var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;
            if (selectedItem != null)
            {
                if (_viewModel.UpdateEstadoPermisoDeTiempo(selectedItem.IdEmpleado, "No aprobado"))
                {
                    selectedItem.Estado = "No aprobado"; // Cambiar el valor del estado a una cadena representativa
                    MessageBox.Show("El permiso fue rechazado.", "Rechazado", MessageBoxButton.OK, MessageBoxImage.Information);
                    PermisoTiempoDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el estado en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
            var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;
            if (selectedItem != null)
            {
                if (_viewModel.UpdateEstadoPermisoDeTiempo(selectedItem.IdEmpleado, "Aprobado"))
                {
                    selectedItem.Estado = "Aprobado"; // Cambiar el valor del estado a una cadena representativa
                    MessageBox.Show("Permiso Aprobado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    PermisoTiempoDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el estado en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;

            if (selectedItem != null)
            {
                // Preguntar al usuario si está seguro de que desea eliminar el registro
                var result = MessageBox.Show("¿Está seguro de que desea eliminar el permiso?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Intentar eliminar el registro de la base de datos
                    if (_viewModel.EliminarPermisoDeTiempo(selectedItem.IdEmpleado))
                    {
                        // Eliminar el registro de la colección si la eliminación en la base de datos fue exitosa
                        _viewModel.PermisosDeTiempo.Remove(selectedItem);
                        MessageBox.Show("Permiso eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el permiso en la base de datos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                // Si el usuario selecciona No, simplemente no hacer nada y cerrar el cuadro de diálogo
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var permisoTiempoCrear = new PermisoTiempoCrear();
            if (permisoTiempoCrear.ShowDialog() == true)
            {
                _viewModel = new PermisoDeTiempoViewModel();
                PermisoTiempoDataGrid.ItemsSource = _viewModel._permisosDeTiempo;
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Obtener el permiso seleccionado en el DataGrid
            var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;

            if (selectedItem != null)
            {
                // Abrir la ventana de edición y pasar el permiso seleccionado
                PermisoTiempoEditar editarVentana = new PermisoTiempoEditar(selectedItem);
                if (editarVentana.ShowDialog() == true)
                {
                    _viewModel = new PermisoDeTiempoViewModel();
                    PermisoTiempoDataGrid.ItemsSource = _viewModel._permisosDeTiempo;
                }
                // Refrescar el DataGrid después de cerrar la ventana de edición
                // Esto asegura que se actualicen los cambios realizados
                PermisoTiempoDataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PermisoTiempoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
