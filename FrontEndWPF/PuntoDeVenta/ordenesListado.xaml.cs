using FrontEndWPF.Services;
using FrontEndWPF.ViewModel;
using FrontEndWPF.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using FrontEndWPF;
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

		OrdenesViewModel ordenesViewModel = new OrdenesViewModel();
		public ordenesListado()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			LoadOrders();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}
		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta(0));
			}
		}

		private async void LoadOrders()
		{
			List<Order> Ordenes = new List<Order>();
			var ordenesConProductos = ordenesViewModel.GetOrdenesConProductos();
			Orders = new ObservableCollection<Order>();
			OrdersDataGrid.ItemsSource = Ordenes;
			foreach (var entry in ordenesConProductos)
			{
				List<ProductosOrden> productosOrdenes = new List<ProductosOrden>();
				var orden = entry.Key;
				var productos = entry.Value;
				foreach (var producto in productos)
				{
					var productoOrden = new ProductosOrden
					{
						Id = producto.Id,
						OrdenId = producto.OrdenId,
						ProductoId = producto.ProductoId,
						Cantidad = producto.Cantidad,
						Nombre = producto.Nombre,
					};
					productosOrdenes.Add(productoOrden);
				}
				var Orders = new Order
				{
					Id = orden.Id,
					Estado = orden.Estado,
					CreationTime = orden.Creacion,
					Productos = productosOrdenes,
				};
				Ordenes.Add(Orders);
				
			}

			Ordenes.OrderByDescending(o => o.CreationTime).ToList();
			OrdersDataGrid.ItemsSource = Ordenes;
		}


		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Order selectedOrder = OrdersDataGrid.SelectedItem as Order;
			if (selectedOrder != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar esta orden?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					ordenesViewModel.EliminarOrden(selectedOrder.Id);
					LoadOrders();
				}
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{

			var selectedItem = OrdersDataGrid.SelectedItem as Order;
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow && selectedItem != null)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta(selectedItem.Id));
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new PuntoVenta(0));
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			var selectedOrder = OrdersDataGrid.SelectedItem as Order;
			if (parentWindow != null && parentWindow is MainWindow mainWindow && selectedOrder != null)
			{
				mainWindow.mainFrame.Navigate(new Facturacion(selectedOrder.Id));
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Anular(object sender, RoutedEventArgs e)
		{
			Order selectedOrder = OrdersDataGrid.SelectedItem as Order;
			if (selectedOrder != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Está seguro que desea anular esta orden?", "Confirmar Anulación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					ordenesViewModel.AnularOrden(selectedOrder.Id);
					LoadOrders();
				}
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}

	public class ProductosOrden
	{
		public int Id { get; set; }
		public int OrdenId { get; set; }
		public int ProductoId { get; set; }
		public string Nombre { get; set; }
		public int Cantidad { get; set; }
	}

	public class Order
	{
		public int Id { get; set; }
		public string Estado { get; set; }
		public DateTime CreationTime { get; set; }
		public List<ProductosOrden> Productos { get; set; }
	}

	public class carritoItem
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public double Precio { get; set; }
		public int Cantidad { get; set; }
	}

}
