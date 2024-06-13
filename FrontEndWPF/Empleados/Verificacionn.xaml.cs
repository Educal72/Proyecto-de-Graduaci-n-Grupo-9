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
    /// Lógica de interacción para Verificacion.xaml
    /// </summary>
    public partial class Verificacionn : Window
    {
        eliminarEmpleado eliminarEmpleado = new eliminarEmpleado();

        public Verificacionn()
        {
            InitializeComponent();
        }

        /* [3.1]
         * Este método se ejecuta cuando se da en guardar en la ventana llamada:
         * Verificacionn.
         * 
         * Consiste en validar si se está presionando una de las 2 opciones que -
         * se le presentan al usuario, uno para el campo: Si, y otro para el -
         * campo: No, y dependiendo de cual seleccione, estos mandaran un true -
         * o un false a un método llamado: TranscursoDeVerificacion que se encuentra -
         * en la clase llamada: eliminarEmpleado, por eso también se hace una instancia -
         * aquí de esa clase.
         */
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if(ActivoSi.IsChecked == true)
            {  
                eliminarEmpleado.TranscursoDeVerificacion(true);
                DialogResult = true;
            }
            else if (ActivoNo.IsChecked == true) {                
                eliminarEmpleado.TranscursoDeVerificacion(false);
                DialogResult = false;
            }           
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }
}
