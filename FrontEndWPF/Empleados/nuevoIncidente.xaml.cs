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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para nuevoIncidente.xaml
	/// </summary>
	public partial class nuevoIncidente : Window
	{
		public string tipo { get; set; }
		public string descripcion { get; set; }
		public DateTime fecha { get; set; }


		public nuevoIncidente()
		{
			InitializeComponent();
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			DateTime? selectedDate = Fecha.SelectedDate;
			tipo = Tipo.Text;
			descripcion = Desc.Text;
			fecha = selectedDate.Value;
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
