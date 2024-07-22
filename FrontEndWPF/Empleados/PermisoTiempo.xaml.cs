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
                    MessageBox.Show("El permiso fue rechazado.");
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
                    MessageBox.Show("¡Permiso Aprobado exitosamente!");
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
	}
}
