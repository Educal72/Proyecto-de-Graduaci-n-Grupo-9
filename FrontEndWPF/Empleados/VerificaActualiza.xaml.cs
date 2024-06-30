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

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para VerificaActualiza.xaml
    /// </summary>
    public partial class VerificaActualiza : Window
    {
        editarEmpleado editarEmpleado = new editarEmpleado();

        public VerificaActualiza()
        {
            InitializeComponent();
        }

        /* [4.1]
         * Este método se ejecuta cuando se da en guardar en la ventana llamada:
         * VerificaActualiza.
         * 
         * Consiste en validar si se está presionando una de las 2 opciones que -
         * se le presentan al usuario, uno para el campo: Si, y otro para el -
         * campo: No, y dependiendo de cual seleccione, estos mandaran un true -
         * o un false a un método llamado: TranscuroDeVerificacionDeActualizar -
         * que se encuentra en la clase llamada: editarEmpleado, por eso también -
         * se hace una instancia aqui de esa clase.
         */
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (ActivoSi.IsChecked == true)
            {
                editarEmpleado.TranscursoDeVerificacionDeActualizar(true);
                DialogResult = true; 
                MessageBox.Show("Empleado actualizado exitosamente.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else if (ActivoNo.IsChecked == true)
            {
                editarEmpleado.TranscursoDeVerificacionDeActualizar(false);
                DialogResult = false;
            }
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}