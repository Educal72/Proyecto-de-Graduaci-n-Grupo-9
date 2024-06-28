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
		public string nombre { get; set; }
		public string apellidos { get; set; }
		public string cedula { get; set; }
		public double salario { get; set; }
		public string puesto { get; set; }
		public DateTime fechaContratación { get; set; }
		public string correo { get; set; }
		public string contraseña { get; set; }
		public string telefono { get; set; }
        public string direccion { get; set; } //Se coloco la dirección porque en la historia: CTE001 lo mencionaba en uno de sus escenarios.
        public string rol { get; set; }


        public añadirEmpleado()
		{
			InitializeComponent();
		}

		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			// Validar Nombre
			if (string.IsNullOrWhiteSpace(Nombre.Text) && Nombre.Text.Length > 3)
			{
				errorMessage = "El campo Nombre es obligatorio.";
				return false;
			}

			// Validar Apellidos
			if (string.IsNullOrWhiteSpace(Apellidos.Text) && Apellidos.Text.Length > 6)
			{
				errorMessage = "El campo Apellidos es obligatorio.";
				return false;
			}

			// Validar Cedula
			if (!int.TryParse(Cedula.Text, out _) && Cedula.Text.Length > 5)
			{
				errorMessage = "El campo Cedula es obligatorio y debe ser válido (sin guiones).";
				return false;
			}

			// Validar Salario
			if (!double.TryParse(Salario.Text, out _))
			{
				errorMessage = "El campo Salario debe ser un número válido.";
				return false;
			}

			// Validar Puesto
			if (string.IsNullOrWhiteSpace(Puesto.Text) || Puesto.Text.Length < 3)
			{
				errorMessage = "El campo Puesto es obligatorio.";
				return false;
			}

			// Validar Fecha de Contratación
			if (!Fecha.SelectedDate.HasValue)
			{
				errorMessage = "El campo Fecha de Contratación es obligatorio.";
				return false;
			}

			int index = Correo.Text.IndexOf("@");
			// Validar Correo
			if (string.IsNullOrWhiteSpace(Correo.Text) && index > 0)
			{
				errorMessage = "El campo Correo es obligatorio.";
				return false;
			}

			// Validar Contraseña
			if (string.IsNullOrWhiteSpace(Contraseña.Text))
			{
				errorMessage = "El campo Contraseña es obligatorio.";
				return false;
			}

			// Validar Telefono
			if (!int.TryParse(Telefono.Text, out _) && Telefono.Text.Length > 8)
			{
				errorMessage = "El campo Telefono es obligatorio y debe ser válido.";
				return false;
			}

			// Validar Rol
			if (string.IsNullOrWhiteSpace(Rol.Text))
			{
				errorMessage = "El campo Rol es obligatorio.";
				return false;
			}
			Conexion conexion = new Conexion();
			var con = conexion.SelectUserCedula(Correo.Text, Convert.ToInt32(Cedula.Text));
			if (con.Count() > 0)
			{
				errorMessage = "El usuario a crear ya existe.";
				return false;
			}

				// Todas las validaciones realizadas
				return true;
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateInputs(out string errorMessage))
			{
				MessageBox.Show(errorMessage);
				return;
			}
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
            direccion = DireccionTo.Text; //Toma el dato que esta en el espacio llamado Direccion, el cual aparece cuando se quiere agregar un nuevo empleado.
            rol = Rol.Text;
			DialogResult = true;
            MessageBox.Show("Empleado guardado exitosamente.");


        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Fecha.SelectedDate = DateTime.Now;
		}

        private void FechaActual_Click(object sender, RoutedEventArgs e)
        {
            Fecha.SelectedDate = DateTime.Now;
        }
    }
}
