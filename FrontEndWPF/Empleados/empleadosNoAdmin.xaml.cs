using FrontEndWPF.Empleados;
using FrontEndWPF.ViewModel;
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
    /// Lógica de interacción para empleadosNoAdmin.xaml
    /// </summary>
    public partial class empleadosNoAdmin : Page
    {
        private DispatcherTimer timer;

        //Variable estatica booleana.
        static bool a;

        public empleadosNoAdmin()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			user.Content = "Usuario: " + SesionUsuario.Instance.nombre;

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
			InicioSesionViewModel inicioSesionViewModel = new InicioSesionViewModel();

			//// Obtener el IdUsuario del usuario actual
			int idUsuario = SesionUsuario.Instance.id;
			int resultado = inicioSesionViewModel.ExisteInicioSesion(idUsuario);
			//// Actualizar la última desconexión
			if (resultado == 0)
			{
				MessageBox.Show("Error al actualizar la última desconexión", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{

				inicioSesionViewModel.UltimaDesconexion(resultado);
			}

			// Redireccionar a la vista de Login
			NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
		}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/Perfil.xaml", UriKind.Relative));
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentArea.Content = null;
            var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            switch (selectedItem)
            {
				case "Fichajes":
					ContentArea.Content = new FichajesEmpleado();
					break;
				case "Permisos de Tiempo":
					ContentArea.Content = new PermisoTiempoEmpleado();
					break;
				case "Permisos de Ausencia":
					ContentArea.Content = new PermisoAusenciaEmpleado();
					break;
				case "Incidentes":
					ContentArea.Content = new InicidentesEmpleado();
					break;
				case "Desvinculaciones":
					ContentArea.Content = new DescvinculacionesEmpleado();
					break;
				case "FAQ":
					ContentArea.Content = new FAQ();
					break;
			}
        }

	
	}
}
