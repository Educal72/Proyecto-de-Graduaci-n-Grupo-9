using FrontEndWPF.Empleados;
using System;
using System.Collections.Generic;
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

			ContentArea.Content = null;
			var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

			switch (selectedItem)
			{
				case "Listado de Empleados":
					ContentArea.Content = new EmployeeListControl();
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
				case "Perfiles Competenciales":
					ContentArea.Content = new PerfilesComp();
					break;
				case "Desvinculaciones":
					ContentArea.Content = new Desvinculaciones();
					break;
				case "FAQ":
					ContentArea.Content = new FAQ();
					break;
			}
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

		public class Metrica
		{
			public string Empleado { get; set; }
			public int HorasTrabajadas { get; set; }
			public int Productividad { get; set; }
			public int VentasRealizadas { get; set; }
		}
		public class Empleado
		{
			// Propiedades del empleado			
			public string? Nombre { get; set; }
			public string? PrimerApellido { get; set; }
            public string? SegundoApellido { get; set; }           
			public string? Cedula { get; set; }
            public string? Telefono { get; set; }
            public string? Correo { get; set; }
            public string? Contraseña { get; set; }
            public string IdRol { get; set; }            
			public DateTime FechaCreacion { get; set; }
            public string? Puesto { get; set; }
            public double Salario { get; set; }
			//public bool Activo { get; set; }
		}

        public class Fichajes
		{
			public int Id { get; set; }
			public int Cedula { get; set; }
			public string Nombre { get; set; }
			public string Puesto { get; set; }
			public DateTime Fecha { get; set; }
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
			public int Id { get; set; }  // Identificador único del incidente
			public string Pregunta { get; set; }  // Fecha en que ocurrió el incidente
			public string Respuesta { get; set; }  // Descripción del incidente
			public string? Documento { get; set; }

		}

		public class Desvinculacion
		{
			public int Id { get; set; } // Identificador único de la desvinculación
			public string Empleado { get; set; } // Nombre del empleado desvinculado
			public DateTime Fecha { get; set; } // Fecha de la desvinculación
			public string Motivo { get; set; } // Motivo de la desvinculación
			public string Comentarios { get; set; } // Comentarios adicionales sobre la desvinculación
			public bool Reconocido { get; set; }
		}

		public class PerfilEmpleoBuscado
		{
			public int Id { get; set; } // Identificador único del perfil de empleo buscado
			public string Titulo { get; set; } // Título del perfil de empleo
			public string Descripcion { get; set; } // Descripción del perfil de empleo
			public string NivelExperiencia { get; set; } // Nivel de experiencia requerido
			public string Requisitos { get; set; } // Requisitos del perfil de empleo
			public string Ubicacion { get; set; } // Ubicación del trabajo
			public double Salario{ get; set; } // Salario mínimo ofrecido
		}

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			ContentArea.Content = null;
			var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

			switch (selectedItem)
			{
				case "Listado de Empleados":
					ContentArea.Content = new EmployeeListControl();
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
				case "Perfiles Competenciales":
					ContentArea.Content = new PerfilesComp();
					break;
				case "Desvinculaciones":
					ContentArea.Content = new Desvinculaciones();
					break;
				case "FAQ":
					ContentArea.Content = new FAQ();
					break;
			}
		}

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

		
    }
}
