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
		public string rol { get; set; }
		public añadirEmpleado()
		{
			InitializeComponent();
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
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
			rol = Rol.Text;

			DialogResult = true;
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
