using FrontEndWPF.Empleados;
using FrontEndWPF.Index;
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
	/// Lógica de interacción para Configuración.xaml
	/// </summary>
	public partial class Configuración : Page
	{
		private DispatcherTimer timer;
		public Configuración()
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
				case "Restaurante":
					ContentArea.Content = new Restaurante();
					break;
				case "Perifericos":
					ContentArea.Content = new Impresora();
					break;
				case "Base de Datos":
					ContentArea.Content = new BaseDeDatos();
					break;
				case "Impuesto":
					ContentArea.Content = new Impuesto();
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
	}
}
