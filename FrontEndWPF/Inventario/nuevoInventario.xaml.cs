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
		public decimal precio { get; set; }
		public bool activo { get; set; }

		public nuevoInventario()
		{
			InitializeComponent();
			
		}

		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			// Validar Nombre
			if (string.IsNullOrWhiteSpace(Nombre.Text))
			{
				errorMessage = "El campo Nombre es obligatorio.";
				return false;
			}

			// Validar Cedula
			if (!int.TryParse(Cantidad.Text, out _))
			{
				errorMessage = "El campo Cantidad es obligatorio y debe ser un número válido.";
				return false;
			}

			// Validar Salario
			if (!decimal.TryParse(Precio.Text, out _))
			{
				errorMessage = "El campo Precio debe ser un número válido.";
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
			nombre = Nombre.Text;
			cantidad = int.Parse(Cantidad.Text);
			precio = decimal.Parse(Precio.Text);
			activo = Activo.IsChecked.Value;
			DialogResult = true;
            MessageBox.Show("Inventario guardado exitosamente.");
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
