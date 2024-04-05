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
using System.Windows.Threading;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para empleadosAdmin.xaml
	/// </summary>
	public partial class empleadosAdmin : Page
	{
		private DispatcherTimer timer;
		public empleadosAdmin()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}
		private void MenuListBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
			// Clear existing content
			ContentArea.Content = null;

			// Get the selected menu item
			var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

			// Load appropriate content based on the selected menu item
			switch (selectedItem)
			{
				case "Listado de Empleados":
					ContentArea.Content = new EmployeeListControl();
					break;
				case "Fichajes":
					ContentArea.Content = new FichajesControl();
					break;
					// Add cases for other menu items as needed
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("MenuPrincipal.xaml", UriKind.Relative));
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Perfil.xaml", UriKind.Relative));
		}
		public class Empleado
		{
			// Propiedades del empleado
			public string Cedula { get; set; }
			public string Nombre { get; set; }
			public string Apellidos { get; set; }
			public string Puesto { get; set; }
			public DateTime FechaContratacion { get; set; }
			public double Salario { get; set; }
			public string CorreoElectronico { get; set; }
			public string Telefono { get; set; }
			public string Contraseña { get; set;}
			public bool Activo { get; set; }
		}

		public class Fichajes
		{
			public int Id { get; set; }
			public int EmpleadoId { get; set; }
			public DateTime Fecha { get; set; }
			public string Tipo { get; set; } // Entrada, Salida, etc.
											 // Add other properties as needed
		}
	}
}
