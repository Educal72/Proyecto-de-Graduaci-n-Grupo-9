﻿using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
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
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Page
    {
		private DispatcherTimer timer;
		string userRole = SesionUsuario.Instance.rol;
		public MenuPrincipal()
        {
            InitializeComponent();
			user.Content = "Usuario: "+userRole;
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start(); 
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			switch (userRole)
			{
				case "Admin":
					// Codigo Rol Admin
					btnEmp.Visibility = Visibility.Visible;
					btnInv.Visibility = Visibility.Visible;
					btnPV.Visibility = Visibility.Visible;
					btnRepor.Visibility = Visibility.Visible;
					break;
				case "Cajero":
					// Codigo Rol Cajero
					btnEmp.Visibility = Visibility.Visible;
					btnInv.Visibility = Visibility.Hidden;
					btnPV.Visibility = Visibility.Visible;
					btnRepor.Visibility = Visibility.Hidden;
					break;
				case "Contador":
					// Codigo Rol Contador
					btnEmp.Visibility = Visibility.Visible;
					btnInv.Visibility = Visibility.Hidden;
					btnPV.Visibility = Visibility.Hidden;
					btnRepor.Visibility = Visibility.Visible;
					break;
				case "Salonero":
					// Codigo Rol Salonero
					btnEmp.Visibility = Visibility.Visible;
					btnInv.Visibility = Visibility.Hidden;
					btnPV.Visibility = Visibility.Hidden;
					btnRepor.Visibility = Visibility.Hidden;
					break;
				default:
					btnEmp.Visibility = Visibility.Hidden;
					btnInv.Visibility = Visibility.Hidden;
					btnPV.Visibility = Visibility.Hidden;
					btnRepor.Visibility = Visibility.Hidden;
					break;
			}

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
            } else {
				
				inicioSesionViewModel.UltimaDesconexion(resultado);
			}

            // Redireccionar a la vista de Login
            NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta(0));
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/Perfil.xaml", UriKind.Relative));
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
            NavigationService.Navigate(new Uri("Reporteria/Menu.xaml", UriKind.Relative));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			/*
			 * Dicha condición permite saber si el empleado que accede es alguien con el rol de:
			 * Administrador, Cajero, Contador y Salonero, y apartir de eso, lo redirige a la página -
			 * respectiva (esto acorde a su rol).
			 */
            if (userRole == "Admin")
            {
                NavigationService.Navigate(new Uri("Empleados/empleadosAdmin.xaml", UriKind.Relative));
            }
            else if (userRole == "Cajero")
            {
                NavigationService.Navigate(new Uri("Empleados/empleadosNoAdmin.xaml", UriKind.Relative));
            }
            else if (userRole == "Contador")
            {
                NavigationService.Navigate(new Uri("Empleados/empleadosNoAdmin.xaml", UriKind.Relative));
            }
            else if (userRole == "Salonero")
            {
                NavigationService.Navigate(new Uri("Empleados/empleadosNoAdmin.xaml", UriKind.Relative));
            }
        }

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/Configuración.xaml", UriKind.Relative));
		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Inventario/Inventario.xaml", UriKind.Relative));
		}
	}
}
