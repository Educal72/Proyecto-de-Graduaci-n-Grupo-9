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
	/// Lógica de interacción para añadirEmpleado.xaml
	/// </summary>
	public partial class editarEmpleado : Window
	{
		public string nombre { get; set; }
		public string apellidos { get; set; }
		public string cedula { get; set; }
		public double salario { get; set; }
		public string puesto { get; set; }
		public DateTime fechaContratación { get; set; }
		public string correo { get; set; }
		public string contraseña { get; set; }
		public string telefono { get; set; }
        public string direccion { get; set; }
        public bool activo { get; set; }
        public string rol { get; set; }

        public static bool validacionActualizar;

        public editarEmpleado()
		{
			InitializeComponent();
		}


        /*
         * Este es el método que se menciona en la clase de: VerificaActualiza, -
         * el cual toma el dato que se le esta mandando.
         * 
         * VER el comentario 4.1
         */
        public void TranscursoDeVerificacionDeActualizar(bool RespuestaPregunta)
        {
            validacionActualizar = RespuestaPregunta;
        }

        /*
         * Este método es el que le sigue después del TranscursoDeVerificacionDeActualizar, -
         * este llama la ventana: VerificacionActualizacion para poder ver cual opción escogió -
         * el usuario.
         * 
         * Si resulta que el usuario no quiere actualizar un dato, entonces en el método lo sabrá -
         * gracias a una condición colocada en dicho método, haciendo que si uno no quiere actualizar -
         * un dato, entonces no se procese dicha petición de actualización, así, evitando posibles -
         * actualizaciones accidentales.
         */
        public void Guardar_Click(object sender, RoutedEventArgs e)
        {
            var VerificacionActualizacion = new VerificaActualiza();
            VerificacionActualizacion.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (VerificacionActualizacion.ShowDialog() == true)
            {
                if (validacionActualizar == true)
                {
                    DateTime? selectedDate = Fecha.SelectedDate;
                    nombre = Nombre.Text;
                    apellidos = Apellidos.Text;
                    cedula = Cedula.Text;
                    salario = double.Parse(Salario.Text);
                    puesto = Puesto.Text;
                    fechaContratación = selectedDate.Value;
                    correo = Correo.Text;
                    contraseña = Contraseña.Text;
                    telefono = Telefono.Text;
                    direccion = DireccionTexto.Text;
                    activo = (bool)Activo.IsChecked;
                    rol = RolMuestra.Text;
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Fecha.SelectedDate = DateTime.Now;
		}
	}
}
