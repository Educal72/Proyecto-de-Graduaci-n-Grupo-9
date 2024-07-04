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

        private async void LoadOrders()
        {
            var ordenesConProductos = ordenesViewModel.GetOrdenesConProductos();
            Orders = new ObservableCollection<Order>();

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

                var order = new Order
                {
                    Id = orden.Id,
                    Estado = orden.Estado,
                    CreationTime = orden.Creacion,
                    Productos = productosOrdenes,
                };

                Orders.Add(order);
            }

            Orders = new ObservableCollection<Order>(Orders.OrderByDescending(o => o.CreationTime));
            OrdersDataGrid.ItemsSource = Orders;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                ordenesViewModel.EliminarOrden(selectedOrder.Id);
                LoadOrders();
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
            SelectedOrderService selectedOrderService = new SelectedOrderService();
            var selectedOrder = OrdersDataGrid.SelectedItem as Order; 
            if (selectedOrder != null)
            {
                NavigationService.Navigate(new Facturacion(selectedOrder.Id));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null && parentWindow is MainWindow mainWindow)
            {
                mainWindow.mainFrame.Navigate(new ordenesListado());
            }
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
}
