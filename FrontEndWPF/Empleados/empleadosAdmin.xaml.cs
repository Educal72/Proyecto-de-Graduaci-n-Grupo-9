using FrontEndWPF.Empleados;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
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
        
		//Variable estatica booleana.
        static bool a; 

		public empleadosAdmin()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			switch (SesionUsuario.Instance.rol)
			{
				case "Admin":
					ListadodeEmpleados.Visibility = Visibility.Visible;
					FichajesItem.Visibility = Visibility.Visible;
					PermisosdeTiempo.Visibility = Visibility.Visible;
					PermisosdeAusencia.Visibility = Visibility.Visible;
					Incidentes.Visibility = Visibility.Visible;
					PerfilesCompetenciales.Visibility = Visibility.Visible;
					Desvinculaciones.Visibility = Visibility.Visible;
					FAQ.Visibility = Visibility.Visible;
					ControldePlanillas.Visibility = Visibility.Visible;
					break;
				case "Cajero":
					ListadodeEmpleados.Visibility = Visibility.Hidden;
					FichajesItem.Visibility = Visibility.Visible;
					PermisosdeTiempo.Visibility = Visibility.Visible;
					PermisosdeAusencia.Visibility = Visibility.Visible;
					Incidentes.Visibility = Visibility.Visible;
					PerfilesCompetenciales.Visibility = Visibility.Hidden;
					Desvinculaciones.Visibility = Visibility.Visible;
					FAQ.Visibility = Visibility.Visible;
					ControldePlanillas.Visibility = Visibility.Hidden;
					break;
				case "Salonero":
					ListadodeEmpleados.Visibility = Visibility.Hidden;
					FichajesItem.Visibility = Visibility.Visible;
					PermisosdeTiempo.Visibility = Visibility.Visible;
					PermisosdeAusencia.Visibility = Visibility.Visible;
					Incidentes.Visibility = Visibility.Visible;
					PerfilesCompetenciales.Visibility = Visibility.Hidden;
					Desvinculaciones.Visibility = Visibility.Visible;
					FAQ.Visibility = Visibility.Visible;
					ControldePlanillas.Visibility = Visibility.Hidden;
					break;
				case "Contador":
					ListadodeEmpleados.Visibility = Visibility.Visible;
					FichajesItem.Visibility = Visibility.Visible;
					PermisosdeTiempo.Visibility = Visibility.Visible;
					PermisosdeAusencia.Visibility = Visibility.Visible;
					Incidentes.Visibility = Visibility.Visible;
					PerfilesCompetenciales.Visibility = Visibility.Visible;
					Desvinculaciones.Visibility = Visibility.Visible;
					FAQ.Visibility = Visibility.Visible;
					ControldePlanillas.Visibility = Visibility.Visible;
					break;
				default:
					ListadodeEmpleados.Visibility = Visibility.Visible;
					FichajesItem.Visibility = Visibility.Visible;
					PermisosdeTiempo.Visibility = Visibility.Visible;
					PermisosdeAusencia.Visibility = Visibility.Visible;
					Incidentes.Visibility = Visibility.Visible;
					PerfilesCompetenciales.Visibility = Visibility.Visible;
					Desvinculaciones.Visibility = Visibility.Visible;
					FAQ.Visibility = Visibility.Visible;
					ControldePlanillas.Visibility = Visibility.Visible;
					break;
			}
		}

		/* Este método sirve para poder saber si el usuario administrador quiere o no entrar -
		 * a algún apartado de la sección de empleados.*/
		public void Enviar(bool dato)
		{
			a = dato;
		}
		
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/Perfil.xaml", UriKind.Relative));
		}

        public class Fichajes
		{
			public string Cedula { get; set; }
			public string Nombre { get; set; }
			public DateTime Fecha { get; set; }
			public DateTime? FechaSalida { get; set; }
			public string Tipo { get; set; }
											 
		}

		public class HistorialLaboral
		{
			public DateTime FechaInicio { get; set; }
			public DateTime? FechaFin { get; set; } // El '?' indica que puede ser nulo
			public string Empresa { get; set; }
			public string Cargo { get; set; }
		}
		
		public class PermisoDeAusencia
		{
			public string Empleado { get; set; } 
			public DateTime FechaInicio { get; set; } 
			public DateTime FechaFin { get; set; } 
			public string Motivo { get; set; }  
			public bool Aprobado { get; set; } 
		}

		public class PermisoDeTiempo
		{
			public int Id { get; set; }  // Identificador único del permiso
			public string Empleado { get; set; }  // Nombre del empleado solicitante
			public DateTime FechaInicio { get; set; }  // Fecha de inicio del permiso
			public DateTime FechaFin { get; set; }  // Fecha de fin del permiso
			public string Motivo { get; set; }  // Motivo del permiso
			public bool Aprobado { get; set; }  // Estado del permiso (aprobado o no)
		}

		public class Incidente
		{
			public int Id { get; set; }  // Identificador único del incidente
			public DateTime Fecha { get; set; }  // Fecha en que ocurrió el incidente
			public string Descripcion { get; set; }  // Descripción del incidente
			public string Tipo { get; set; }  // Tipo de incidente (por ejemplo, "Entre Empleados", "Con Cliente")
			public bool Estado { get; set; }
		}
		public class FAQS
		{
			public int Id { get; set; }
			public string Pregunta { get; set; }  // Fecha en que ocurrió el incidente
			public string Respuesta { get; set; }  // Descripción del incidente
			public string Nombre { get; set; }
			public byte[]? Documento { get; set; }
		}

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			ContentArea.Content = null;
			var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

			switch (selectedItem)
			{
				case "Listado de Empleados":
					ContentArea.Content = new EmployeeListControl();
                    if (a == true)
                    {
                        ContentArea.Content = new PantallaOscura();
                    }
                    break;
				case "Fichajes":
					ContentArea.Content = new FichajesControl();
					break;
				case "Permisos de Tiempo":
					ContentArea.Content = new PermisoTiempo();
					break;
				case "Permisos de Ausencia":
					ContentArea.Content = new PermisosAusencia();
					break;
				case "Incidentes":
					ContentArea.Content = new Incidentes();
					break;
				case "Talento Humano":
					ContentArea.Content = new PerfilesComp();
					break;
				case "Desvinculaciones":
					ContentArea.Content = new Desvinculaciones();
                    break;
				case "FAQ":
					ContentArea.Content = new FAQ();
					break;
				case "Control de Planillas":
					ContentArea.Content = new ControlPlanillas();
					break;
			}
		}

		private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
		{

        }
    }
}
