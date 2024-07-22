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

namespace FrontEndWPF
{
    public partial class PermisoTiempoCrear : Window
    {
        public PermisoTiempoCrear()
        {
            InitializeComponent();
        }

        private void Button_Click_Crear(object sender, RoutedEventArgs e)
        {
            // Aquí puedes agregar la lógica para guardar los datos
            var fechaInicio = FechaInicioPicker.SelectedDate;
            var fechaFin = FechaFinPicker.SelectedDate;
            var motivo = MotivoTextBox.Text;

            // Aquí deberías llamar al método para guardar los datos en la base de datos

            MessageBox.Show("Permiso de tiempo creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            // Cerrar la ventana después de crear el permiso
            this.Close();
        }

        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana sin hacer cambios
            this.Close();
        }
    }
}
