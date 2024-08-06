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
	public partial class verCliente : Window
	{
		public string nombre { get; set; }
		public string cedula { get; set; }
		public string apellidos { get; set; }
		public string correo { get; set; }
		public string telefono { get; set; }
		public bool asociar { get; set; }

		public verCliente(string oldNombre, string oldCedula, string oldApellidos, string oldCorreo, string oldTelefono, bool oldAsociar)
		{
			InitializeComponent();
			Nombre.Text = oldNombre;
			Cedula.Text = oldCedula;
			Apellidos.Text = oldApellidos;
			Correo.Text = oldCorreo;
			Telefono.Text = oldTelefono;
			Asociar.IsChecked = oldAsociar;
			Cedula.IsEnabled = false;

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (!Nombre.Text.Equals("") && !Cedula.Text.Equals("") && !Apellidos.Text.Equals("") && !Correo.Text.Equals("") && !Telefono.Text.Equals("")) {
				nombre = Nombre.Text;
				cedula = Cedula.Text;
				apellidos = Apellidos.Text;
				correo = Correo.Text;
				telefono = Telefono.Text;
				if (Asociar.IsChecked == true) {
					asociar = true;
				} else {
					asociar = false;
				}
			DialogResult = true;
			Close();
			} else {
				MessageBox.Show("Por favor, introduzca toda la información solicitada.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
