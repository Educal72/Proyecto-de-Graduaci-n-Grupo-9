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
using static System.Net.Mime.MediaTypeNames;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para añadirEmpleado.xaml
	/// </summary>
	public partial class añadirEmpleado : Window
	{

		/* Propiedades para poder guardar todos los datos solicitados -
		 * al usuario administrador para añadir a un nuevo empleado al sistema.*/
		public string nombre_añadirEmpleado { get; set; }
		public string apellidos_añadirEmpleado { get; set; }
		public string cedula_añadirEmpleado { get; set; }
		public double salario_añadirEmpleado { get; set; }
		public string puesto_añadirEmpleado { get; set; }
		public DateTime fechaContratacion_añadirEmpleado { get; set; }
		public string correo_añadirEmpleado { get; set; }
		public string contraseña_añadirEmpleado { get; set; }
		public string telefono_añadirEmpleado { get; set; }
        public string direccion_añadirEmpleado { get; set; } //Se coloco la dirección porque en la historia: CTE001 lo mencionaba en uno de sus escenarios.
        public string rol_añadirEmpleado { get; set; } 


        //Constructor vacio.
        public añadirEmpleado()
		{
			InitializeComponent();
		}


		/* Método que se encarga de validar los espacios correspondientes en -
		 * donde el usuario administrador agregara los datos del nuevo empleado al sistema.*/
		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			// Validar Nombre:
			if (string.IsNullOrWhiteSpace(Nombre.Text) && Nombre.Text.Length > 3)
			{
				errorMessage = "El campo Nombre es obligatorio.";
				return false;
			}

            // Validar Apellidos:
            if (string.IsNullOrWhiteSpace(Apellidos.Text) && Apellidos.Text.Length > 6)
			{
				errorMessage = "El campo Apellidos es obligatorio.";
				return false;
			}

            // Validar Cedula:
            if (!int.TryParse(Cedula.Text, out _) && Cedula.Text.Length > 5)
			{
				errorMessage = "El campo Cedula es obligatorio y debe ser válido (sin guiones).";
				return false;
			}

            // Validar Salario:
            if (!double.TryParse(Salario.Text, out _))
			{
				errorMessage = "El campo Salario debe ser un número válido.";
				return false;
			}

            // Validar Puesto:
            if (string.IsNullOrWhiteSpace(Puesto.Text) || Puesto.Text.Length < 3)
			{
				errorMessage = "El campo Puesto es obligatorio.";
				return false;
			}

            // Validar Fecha de Contratación:
            if (!Fecha.SelectedDate.HasValue)
			{
				errorMessage = "El campo Fecha de Contratación es obligatorio.";
				return false;
			}

            // Validar Correo:
            int index = Correo.Text.IndexOf("@");            
            if (string.IsNullOrWhiteSpace(Correo.Text) && index > 0)
			{
				errorMessage = "El campo Correo es obligatorio.";
				return false;
			}

            /* Esta valicación en la historia CTE001, corresponde al 4rto escenario, -
             * en donde se explica que debe de dar un mensaje de advertencia porque -
             * hay datos que están haciendo conflictos en la BD. */
            Conexion conexion = new Conexion();
            var con = conexion.SelectUserCedula(Correo.Text, Convert.ToInt32(Cedula.Text));
            if (con.Count() > 0)
            {
                errorMessage = "El usuario a crear ya existe.";
                return false;
            }

            // Validar Contraseña:
            if (string.IsNullOrWhiteSpace(Contraseña.Text))
			{
				errorMessage = "El campo Contraseña es obligatorio.";
				return false;
			}

            // Validar Teléfono:
            if (!int.TryParse(Telefono.Text, out _) && Telefono.Text.Length > 8)
			{
				errorMessage = "El campo Telefono es obligatorio y debe ser válido.";
				return false;
			}

            // Validar Rol:
            if (string.IsNullOrWhiteSpace(Rol.Text))
			{
				errorMessage = "El campo Rol es obligatorio.";
				return false;
			}

			// Todas las validaciones realizadas
			return true;
		}


		/* Método en donde el usuario administrador le dara en confirmar -
		 * para poder guardar los datos que ingreso del nuevo empleado.*/
		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateInputs(out string errorMessage))
			{
                MessageBox.Show("Hay un conflicto de datos o estás omitiendo un campo obligatorio.\n"
                    + errorMessage, "Alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

			DateTime? selectedDate = Fecha.SelectedDate;
            fechaContratacion_añadirEmpleado = selectedDate!.Value;

            nombre_añadirEmpleado = Nombre.Text;
            apellidos_añadirEmpleado = Apellidos.Text;

            cedula_añadirEmpleado = Cedula.Text;
            salario_añadirEmpleado = double.Parse(Salario.Text);

            puesto_añadirEmpleado = Puesto.Text;
            correo_añadirEmpleado = Correo.Text;
            
			contraseña_añadirEmpleado = Contraseña.Text;
            telefono_añadirEmpleado = Telefono.Text;
            
			direccion_añadirEmpleado = DireccionTo.Text;
            rol_añadirEmpleado = Rol.Text;
			
			DialogResult = true;
        }


        /* Método en donde el usuario administrador le dara en confirmar -
		 * para poder cancelar los datos que ingreso del nuevo empleado.*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}


        /* Método en donde el usuario administrador le dara en el botón "Actual" -
		 * para poder guardar la fecha actual que contrato al nuevo empleado.*/
        private void FechaActual_Click(object sender, RoutedEventArgs e)
        {
            Fecha.SelectedDate = DateTime.Now;
       
        }

    }
}
