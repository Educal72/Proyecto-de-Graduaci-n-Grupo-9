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
	/// Lógica de interacción para Perfil.xaml
	/// </summary>
	public partial class Perfil : Page
	{
		MainWindow main = new MainWindow();
		private DispatcherTimer timer;
		public Perfil()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Instrucciones enviadas al correo electrónico, favor revisarlo.", "Recuperación de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
		}
    }
}
