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
using static FrontEndWPF.PuntoVenta;

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
			new Order { Id = 1, Estado = "Pendiente", CreationTime = DateTime.Now,
						Productos = new List<carritoItem>{ new carritoItem{Id=1, Nombre="Producto 1", Cantidad=2, Precio = 1299.00 },
													   new carritoItem{Id=2, Nombre="Producto 2", Cantidad=1, Precio = 1399.00 }}},
			new Order { Id = 2, Estado = "Pago", CreationTime = DateTime.Now.AddDays(-1),
						Productos = new List<carritoItem>{ new carritoItem{Id=3, Nombre="Producto 3", Cantidad=3, Precio = 1499.00 },
													   new carritoItem{Id=4, Nombre="Producto 4", Cantidad=4, Precio = 1599.00 }}},
			new Order { Id = 3, Estado = "Pendiente", CreationTime = DateTime.Now.AddDays(-2),
						Productos = new List<carritoItem>{ new carritoItem{Id = 5, Nombre="Producto 5", Cantidad=5, Precio = 1899.00 },
													   new carritoItem{Id=6, Nombre="Producto 6", Cantidad=6, Precio = 1099.00 }}},
		};

			DataContext = this;
		}

		private void SortOrders()
		{
			var sortedOrders = Orders.OrderByDescending(o => o.Estado == "Pendiente")
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

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta());
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new Facturacion());
			}
		}
	}

	public class Order
	{
		public int Id { get; set; }
		public string Estado { get; set; }
		public DateTime CreationTime { get; set; }
		public List<carritoItem> Productos { get; set; }
	}

	public class carritoItem
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public double Precio { get; set; }
		public int Cantidad { get; set; }
	}
}

