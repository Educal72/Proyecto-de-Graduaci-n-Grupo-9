using FrontEndWPF.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using FrontEndWPF.Modelos;
using FrontEndWPF.Empleados;

namespace FrontEndWPF.Empleados
{
    /// <summary>
    /// Lógica de interacción para PermisosAusencia.xaml
    /// </summary>
    public partial class PermisosAusencia : UserControl
    {
        private PermisoDeAusenciaViewModel _viewModel;

        public PermisosAusencia()
        {
            InitializeComponent();
            _viewModel = new PermisoDeAusenciaViewModel();
            this.DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = PermisoAusenciaDataGrid.SelectedItem as PermisoDeAusencia;
            if (selectedItem != null)
            {
                if (_viewModel.UpdateEstadoPermisoDeAusencia(selectedItem.IdEmpleado, "No aprobado"))
                {
                    selectedItem.Estado = "No aprobado"; // Cambiar el valor del estado a una cadena representativa
                    MessageBox.Show("El permiso fue rechazado.", "Rechazado", MessageBoxButton.OK, MessageBoxImage.Information);
                    PermisoAusenciaDataGrid.Items.Refresh();
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
            var selectedItem = PermisoAusenciaDataGrid.SelectedItem as PermisoDeAusencia;
            if (selectedItem != null)
            {
                if (_viewModel.UpdateEstadoPermisoDeAusencia(selectedItem.IdEmpleado, "Aprobado"))
                {
                    selectedItem.Estado = "Aprobado"; // Cambiar el valor del estado a una cadena representativa
                    MessageBox.Show("Permiso Aprobado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    PermisoAusenciaDataGrid.Items.Refresh();
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
            var selectedItem = PermisoAusenciaDataGrid.SelectedItem as PermisoDeAusencia;

            if (selectedItem != null)
            {
                // Preguntar al usuario si está seguro de que desea eliminar el registro
                var result = MessageBox.Show("¿Está seguro de que desea eliminar el permiso?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Intentar eliminar el registro de la base de datos
                    if (_viewModel.EliminarPermisoDeAusencia(selectedItem.IdEmpleado))
                    {
                        // Eliminar el registro de la colección si la eliminación en la base de datos fue exitosa
                        _viewModel.PermisosDeAusencia.Remove(selectedItem);
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
            var permisoAusenciaCrear = new PermisoAusenciaCrear();
            if (permisoAusenciaCrear.ShowDialog() == true)
            {
                _viewModel = new PermisoDeAusenciaViewModel();
                PermisoAusenciaDataGrid.ItemsSource = _viewModel.PermisosDeAusencia;
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Obtener el permiso seleccionado en el DataGrid
            var selectedItem = PermisoAusenciaDataGrid.SelectedItem as PermisoDeAusencia;

            if (selectedItem != null)
            {
                // Abrir la ventana de edición y pasar el permiso seleccionado
                PermisoAusenciaEditar editarVentana = new PermisoAusenciaEditar(selectedItem);
                if (editarVentana.ShowDialog() == true)
                {
                    _viewModel = new PermisoDeAusenciaViewModel();
                    PermisoAusenciaDataGrid.ItemsSource = _viewModel.PermisosDeAusencia;
                }
                // Refrescar el DataGrid después de cerrar la ventana de edición
                // Esto asegura que se actualicen los cambios realizados
                PermisoAusenciaDataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PermisoAusenciaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
