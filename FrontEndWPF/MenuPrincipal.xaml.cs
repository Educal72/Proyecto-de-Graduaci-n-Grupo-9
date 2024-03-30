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
		public MenuPrincipal()
        {
            InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start(); 
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}
		
		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//rect1.Width = e.NewSize.Width;
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("PuntoVenta.xaml", UriKind.Relative));
		}
	}
}
