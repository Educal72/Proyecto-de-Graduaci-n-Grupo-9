using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static FrontEndWPF.Modelos.UserModel;


namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para eliminarEmpleado.xaml
    /// </summary>
    public partial class eliminarEmpleado : Window
    {

        Conexion conexion = new Conexion();
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string cedula { get; set; }
        public double salario { get; set; }
        public string puesto { get; set; }
        public DateTime fechaContratación { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public string telefono { get; set; }
        public string rol { get; set; }

        public static bool validacion;

        public eliminarEmpleado()
        {
            InitializeComponent();
        }

        /*
         * Este es el método que se menciona en la clase de: Verificaccion, -
         * el cual toma el dato que se le esta mandando.
         * 
         * VER el comentario 3.1
         */
        public void TranscursoDeVerificacion(bool RespuestaPregunta)
        {
            validacion = RespuestaPregunta;
        }

        /*
         * Este método es el que le sigue después del TranscursoDeVerificacion, -
         * este llama la ventana: Verificacionn para poder ver cual opción escogió el usuario.
         * 
         * Si resulta que el usuario no quiere eliminar un dato, entonces en el método lo sabrá -
         * gracias a una condición colocada en dicho método, haciendo que si uno no quiere eliminar -
         * un dato, entonces no se procese dicha petición de eliminación, así, evitando posibles -
         * eliminaciones accidentales.
         */
        public void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            var Verificacionn = new Verificacionn();
            Verificacionn.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
            if (Verificacionn.ShowDialog() == true)
            {
                if(validacion == true)
                {
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
        }
        
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
 
    }
}
