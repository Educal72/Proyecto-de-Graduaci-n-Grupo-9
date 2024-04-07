using FrontEndWPF.Index;
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

namespace FrontEndWPF.Inventario
{
	/// <summary>
	/// Lógica de interacción para nuevoInventario.xaml
	/// </summary>
	/// 
	public partial class nuevoInventario : Window
	{
		public int id { get; set; }
		public string nombre { get; set; }
		public int cantidad { get; set; }
		public double precio { get; set; }
		public bool activo { get; set; }

		public nuevoInventario()
		{
			InitializeComponent();
			
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
		{
			id = 1;
			nombre = Nombre.Text;
			cantidad = int.Parse(Cantidad.Text);
			precio = double.Parse(Precio.Text);
			activo = Activo.IsChecked.Value;
			DialogResult = true;
		}

		private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
