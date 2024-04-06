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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.empleadosAdmin;

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para Desvinculaciones.xaml
	/// </summary>
	public partial class Desvinculaciones : UserControl
	{
		List<Desvinculacion> desvinculaciones = new List<Desvinculacion>();
		public Desvinculaciones()
		{
			InitializeComponent();
			PopulateDesvinculacionesDataGrid();
		}

		public void PopulateDesvinculacionesDataGrid()
		{
			desvinculaciones = new List<Desvinculacion>
		{
			new Desvinculacion { Id = 1, Empleado = "Juan Pérez", Fecha = DateTime.Now.AddDays(-10), Motivo = "Renuncia voluntaria", Comentarios = "Excelente desempeño durante su tiempo en la empresa.", Reconocido = true},
			new Desvinculacion { Id = 2, Empleado = "María García", Fecha = DateTime.Now.AddDays(-5), Motivo = "Despido justificado", Comentarios = "Incumplimiento reiterado de políticas de la empresa.", Reconocido = false },
			new Desvinculacion { Id = 3, Empleado = "Pedro López", Fecha = DateTime.Now.AddDays(-3), Motivo = "Jubilación", Comentarios = "Más de 30 años de servicio.", Reconocido = true }
		};

			DesvinculacionesDataGrid.ItemsSource = desvinculaciones;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = DesvinculacionesDataGrid.SelectedItem as Desvinculacion;
			if (selectedItem != null)
			{
				selectedItem.Reconocido = false;
				DesvinculacionesDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = DesvinculacionesDataGrid.SelectedItem as Desvinculacion;
			if (selectedItem != null)
			{
				selectedItem.Reconocido = true;
				DesvinculacionesDataGrid.Items.Refresh();
			}
		}


	}
}
