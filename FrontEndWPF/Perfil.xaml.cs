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
	/// Lógica de interacción para Perfil.xaml
	/// </summary>
	public partial class Perfil : Page
	{
		MainWindow main = new MainWindow();
		public Perfil()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("MenuPrincipal.xaml", UriKind.Relative));
		}
    }
}
