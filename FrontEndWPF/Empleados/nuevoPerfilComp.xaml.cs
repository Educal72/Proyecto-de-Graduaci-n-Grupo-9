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
	/// Lógica de interacción para nuevoPerfilComp.xaml
	/// </summary>
	/// 
	
	public partial class nuevoPerfilComp : Window
	{
		public string titulo { get; set; }
		public string descripcion { get; set; }
		public string experiencia { get; set; }
		public string requisitos { get; set; }
		public string ubicación { get; set; }
		public double salario { get; set; }

		public nuevoPerfilComp()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			titulo = Titulo.Text;
			descripcion = Desc.Text;
			experiencia = Exp.Text;
			requisitos = Req.Text;
			ubicación = Ubicacion.Text;
			salario = double.Parse(Salario.Text);
			DialogResult = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
