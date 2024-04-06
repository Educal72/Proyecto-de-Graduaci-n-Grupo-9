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
	/// Lógica de interacción para PermisosAusencia.xaml
	/// </summary>
	public partial class PermisosAusencia : UserControl
	{
		List<PermisoDeAusencia> permisos = new List<PermisoDeAusencia>();
		public PermisosAusencia()
		{
			InitializeComponent();
			PopulatePermisoDeAusenciaDataGrid();
		}

		private void PopulatePermisoDeAusenciaDataGrid()
		{
			permisos = new List<PermisoDeAusencia>
	{
		new PermisoDeAusencia { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), Motivo = "Vacaciones", Aprobado = true },
		new PermisoDeAusencia { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1), Motivo = "Enfermedad", Aprobado = false },
		new PermisoDeAusencia { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Motivo = "Asuntos personales", Aprobado = true },
		new PermisoDeAusencia { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(5), Motivo = "Formación", Aprobado = true },
		new PermisoDeAusencia { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(4), Motivo = "Licencia", Aprobado = false }
	};

			PermisoDeAusenciaDataGrid.ItemsSource = permisos;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = PermisoDeAusenciaDataGrid.SelectedItem as PermisoDeAusencia;
			if (selectedItem != null) {
				selectedItem.Aprobado = false;
				PermisoDeAusenciaDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = PermisoDeAusenciaDataGrid.SelectedItem as PermisoDeAusencia;
			if (selectedItem != null)
			{
				selectedItem.Aprobado = true;
				PermisoDeAusenciaDataGrid.Items.Refresh();
			}
		}
	}
}
