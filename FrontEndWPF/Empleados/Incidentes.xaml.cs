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
	/// Lógica de interacción para Incidentes.xaml
	/// </summary>
	public partial class Incidentes : UserControl
	{
		List<Incidente> incidentes = new List<Incidente>();
		public Incidentes()
		{
			InitializeComponent();
			PopulateIncidentesDataGrid();
		}

		private void PopulateIncidentesDataGrid()
		{
			incidentes = new List<Incidente>
			{
			new Incidente { Id = 1, Fecha = DateTime.Now, Descripcion = "Discusión entre empleados", Tipo = "Interno", Estado = true },
			new Incidente { Id = 2, Fecha = DateTime.Now.AddDays(-3), Descripcion = "Queja de cliente por servicio", Tipo = "Externo", Estado = false },
			new Incidente { Id = 3, Fecha = DateTime.Now.AddDays(-5), Descripcion = "Accidente en el lugar de trabajo", Tipo = "Interno", Estado = true },
			new Incidente { Id = 4, Fecha = DateTime.Now.AddDays(-7), Descripcion = "Incumplimiento de contrato con cliente", Tipo = "Externo", Estado = false },
			new Incidente { Id = 5, Fecha = DateTime.Now.AddDays(-9), Descripcion = "Robo en la tienda", Tipo = "Externo", Estado = true }
			};

			IncidenteDataGrid.ItemsSource = incidentes;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedItem = IncidenteDataGrid.SelectedItem as Incidente;
			if (selectedItem != null)
			{
				selectedItem.Estado = true;
				IncidenteDataGrid.Items.Refresh();
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var selectedItem = IncidenteDataGrid.SelectedItem as Incidente;
			if (selectedItem != null)
			{
				selectedItem.Estado = false;
				IncidenteDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var nuevoIncidente = new nuevoIncidente();
			nuevoIncidente.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoIncidente.ShowDialog() == true)
			{
				string Tipo = nuevoIncidente.tipo;
				string Desc = nuevoIncidente.descripcion;
				DateTime Fecha = nuevoIncidente.fecha;
				bool Estado = true;

				incidentes.Add(new Incidente
				{
					Id=1,
					Tipo=Tipo,
					Descripcion=Desc,
					Fecha=Fecha,
					Estado=Estado
				});
				IncidenteDataGrid.Items.Refresh();
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var selectedItem = IncidenteDataGrid.SelectedItem as Incidente;
			if (selectedItem != null)
			{
				var editarIncidente = new editarIncidente();
				editarIncidente.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				editarIncidente.Tipo.Text = selectedItem.Tipo;
				editarIncidente.Desc.Text = selectedItem.Descripcion;
				editarIncidente.Fecha.Text = selectedItem.Fecha.ToString();
				editarIncidente.Estado.IsChecked = selectedItem.Estado;
				if (editarIncidente.ShowDialog() == true)
				{
					selectedItem.Tipo = editarIncidente.tipo;
					selectedItem.Descripcion = editarIncidente.descripcion;
					selectedItem.Fecha = editarIncidente.fecha;
					selectedItem.Estado = editarIncidente.estado;

					IncidenteDataGrid.Items.Refresh();
				}
			}
		}
	}
}
