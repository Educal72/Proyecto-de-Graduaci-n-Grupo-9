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

        public PermisoTiempo()
        {
            InitializeComponent();
            this.DataContext = new PermisoDeTiempoViewModel();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
		{
            var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;
            if (selectedItem != null)
            {
                selectedItem.Estado = "No aprobado"; // Cambiar el valor del estado a una cadena representativa
                PermisoTiempoDataGrid.Items.Refresh();
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
                selectedItem.Estado = "Aprobado"; // Cambiar el valor del estado a una cadena representativa
                PermisoTiempoDataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
	}
}
