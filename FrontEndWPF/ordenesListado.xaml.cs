using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Lógica de interacción para ordenesListadoxaml.xaml
	/// </summary>
	public partial class ordenesListado : Page
	{
		private DispatcherTimer timer;
		public ObservableCollection<Order> Orders { get; set; }
		public ordenesListado()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			LoadOrders();
			SortOrders();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta());
			}
		}

		private void LoadOrders()
		{
			Orders = new ObservableCollection<Order>
			{
				new Order { Id = 1, Estado = "Active", CreationTime = DateTime.Now },
				new Order { Id = 2, Estado = "Inactive", CreationTime = DateTime.Now.AddDays(-1) },
				new Order { Id = 3, Estado = "Active", CreationTime = DateTime.Now.AddDays(-2) },
			};

			DataContext = this;
		}

		private void SortOrders()
		{
			var sortedOrders = Orders.OrderByDescending(o => o.Estado == "Active")
								 .ThenByDescending(o => o.CreationTime)
								 .ToList();

			
			Orders.Clear();
			foreach (var order in sortedOrders)
			{
				Orders.Add(order);
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Order selectedOrder = OrdersDataGrid.SelectedItem as Order;
			if (selectedOrder != null)
			{
				Orders.Remove(selectedOrder);
				
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta());
			}
		}
	}

	public class Order
	{
		public int Id { get; set; }
		public string Estado { get; set; }
		public DateTime CreationTime { get; set; }
	}
}

