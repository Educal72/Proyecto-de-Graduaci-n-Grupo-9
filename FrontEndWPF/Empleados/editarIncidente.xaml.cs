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
	/// Lógica de interacción para editarIncidente.xaml
	/// </summary>
	/// 

	public partial class editarIncidente : Window
	{
		public string tipo { get; set; }
		public string descripcion { get; set; }
		public DateTime fecha { get; set; }
		public bool estado { get; set; }
		public editarIncidente()
		{
			InitializeComponent();
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			DateTime? selectedDate = Fecha.SelectedDate;
			tipo = Tipo.Text;
			descripcion = Desc.Text;
			fecha = selectedDate.Value;
			if (Estado.IsChecked == true) {
				estado = true;
			}
			else {
				estado = false;
			}
			
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
