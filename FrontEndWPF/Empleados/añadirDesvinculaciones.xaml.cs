using FrontEndWPF.Index;
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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para añadirDesvinculaciones.xaml
    /// </summary>

    public partial class añadirDesvinculaciones : Window
    {
        public string? empleado { get; set; } // Nombre del empleado que quiere hacer la desvinculación.
        public DateTime fechaInicio { get; set; } // Fecha de inicio.
        public string? motivo { get; set; } // Motivo de la desvinculación
        public string? comentarios { get; set; } // Comentarios adicionales sobre la desvinculación
        public DateTime fechaSalida { get; set; } // Fecha de la desvinculación


        public añadirDesvinculaciones()
        {
            InitializeComponent();
        }


        /*
         * Método en donde se guardan los datos que el empleado esta colocando.
         */
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            empleado = EmpleadoTextBox.Text;
            DateTime? selectedDateInicio = FechaInicioPicker.SelectedDate;
            fechaInicio = selectedDateInicio.Value;
            motivo = MotivoTextbox.Text;
            comentarios = ComentariosTextbox.Text;
            DateTime? selectedDateFinal = FechaSalidaPicker.SelectedDate;
            fechaSalida = selectedDateFinal.Value;

            // Realizar el proceso de desvinculación aquí (guardar en la base de datos, por ejemplo)

            // Mostrar mensaje de confirmación
            MessageBox.Show($"El empleado {empleado} ha sido desvinculado exitosamente.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
