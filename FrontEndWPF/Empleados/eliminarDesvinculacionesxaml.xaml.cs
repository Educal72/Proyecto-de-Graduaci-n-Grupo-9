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
    /// Lógica de interacción para eliminarDesvinculacionesxaml.xaml
    /// </summary>
    public partial class eliminarDesvinculacionesxaml : Window
    {       
        public string? empleado { get; set; } // Nombre del empleado.
        public DateTime fechaInicio { get; set; } // Fecha de inicio.
        public string? motivo { get; set; } // Motivo de la desvinculación
        public string? comentarios { get; set; } // Comentarios adicionales sobre la desvinculación
        public DateTime fechaSalida { get; set; } // Fecha de la desvinculación
        public bool reconocido { get; set; } //Sirve para poder dar una aprobación o no a la solicitud.

        public static bool validacion1;

        public eliminarDesvinculacionesxaml()
        {
            InitializeComponent();
        }

        /*
         * Método en el cual recibe la respuesta a la -
         * verificación de eliminar una solicitud de desvinculación.
         */
        public void TranscursoDeVerificacion(bool RespuestaPregunta)
        {
            validacion1 = RespuestaPregunta;
        }

        /*
         * Método que trabaja en equipo con el método: -
         * TranscursoDeVerificacion, para asi poder hacer la validación -
         * respectiva, osea, saber si el empleado quiere o no eliminar su -
         * solicitud de desvinculación.
         */
        private void Eliminar_Desvinculacion_Click(object sender, RoutedEventArgs e)
        {
            var VerificaDesvinculacionesEmpleados = new VerificaDesvinculacionesEmpleados();
            VerificaDesvinculacionesEmpleados.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (VerificaDesvinculacionesEmpleados.ShowDialog() == true)
            {
                if (validacion1 == true)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
        }

        private void Cancelar_Desvinculacion_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


    }
}
