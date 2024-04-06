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

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para PermisoTiempo.xaml
	/// </summary>
	public partial class PermisoTiempo : UserControl
	{
		List<PermisoDeTiempo> permisos = new List<PermisoDeTiempo>();
		public PermisoTiempo()
		{
			InitializeComponent();
			PopulateDataGrid();
		}

		public void PopulateDataGrid()
		{
			permisos = new List<PermisoDeTiempo>
			{
			new PermisoDeTiempo { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), Motivo = "Inicio de Sesión", Aprobado = true },
			new PermisoDeTiempo { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1), Motivo = "Cerrado de Sesión", Aprobado = false },
			new PermisoDeTiempo { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(3), Motivo = "Inicio de Sesión", Aprobado = true },
			new PermisoDeTiempo { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(5), Motivo = "Inicio de Sesión", Aprobado = true },
			new PermisoDeTiempo { Empleado = "Nombre Apellidos", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(4), Motivo = "Cerrado de Sesión", Aprobado = false } 
			};

			PermisoTiempoDataGrid.ItemsSource = permisos;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;
			if (selectedItem != null)
			{
				selectedItem.Aprobado = false;
				PermisoTiempoDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = PermisoTiempoDataGrid.SelectedItem as PermisoDeTiempo;
			if (selectedItem != null)
			{
				selectedItem.Aprobado = true;
				PermisoTiempoDataGrid.Items.Refresh();
			}
		}
	}
}
