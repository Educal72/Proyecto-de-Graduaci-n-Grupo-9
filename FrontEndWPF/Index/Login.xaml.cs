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

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para ExtraPagina.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
			InitializeComponent();

        }
		

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (correo.Text == "" || correo.Text == "Admin")
			{
				MenuPrincipal menuPrincipal = new MenuPrincipal();
				menuPrincipal.user.Content = "Usuario: Admin";
				NavigationService.Navigate(menuPrincipal);
			}
			else if (correo.Text == "Cajero")
			{
				MenuPrincipal menuPrincipal = new MenuPrincipal();
				menuPrincipal.user.Content = "Usuario: Cajero";
				menuPrincipal.btnEmp.Visibility = Visibility.Hidden;
				menuPrincipal.btnInv.Visibility = Visibility.Hidden;
				menuPrincipal.btnPV.Visibility = Visibility.Visible;
				menuPrincipal.btnRepor.Visibility = Visibility.Hidden;
				NavigationService.Navigate(menuPrincipal);
			}
			else if (correo.Text == "Contador")
			{
				MenuPrincipal menuPrincipal = new MenuPrincipal();
				menuPrincipal.user.Content = "Usuario: Contador";
				menuPrincipal.btnEmp.Visibility = Visibility.Hidden;
				menuPrincipal.btnInv.Visibility = Visibility.Hidden;
				menuPrincipal.btnPV.Visibility = Visibility.Hidden;
				menuPrincipal.btnRepor.Visibility = Visibility.Visible;
				NavigationService.Navigate(menuPrincipal);
			}
			else {
				MenuPrincipal menuPrincipal = new MenuPrincipal();
				menuPrincipal.user.Content = "Usuario: Salonero";
				menuPrincipal.btnEmp.Visibility = Visibility.Visible;
				menuPrincipal.btnInv.Visibility = Visibility.Hidden;
				menuPrincipal.btnPV.Visibility = Visibility.Hidden;
				menuPrincipal.btnRepor.Visibility = Visibility.Hidden;
				NavigationService.Navigate(menuPrincipal);
			}
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/RecuperarContraseña.xaml", UriKind.Relative));
		}
	}
}
