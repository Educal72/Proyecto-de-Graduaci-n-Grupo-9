using FrontEndWPF.Reporteria;
using FrontEndWPF.ViewModel;
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
using static FrontEndWPF.Facturacion;
using static FrontEndWPF.PuntoVenta;
using static FrontEndWPF.ViewModel.OrdenesViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para PuntoVenta.xaml
    /// </summary>
    public partial class PuntoVenta : UserControl
    {
		private DispatcherTimer timer;


		ProductosViewModel productos = new ProductosViewModel();
		OrdenesViewModel ordenes = new OrdenesViewModel();
		int ordenId = 0;
		public PuntoVenta(int orden)
        {
			InitializeComponent();
			productos.LoadProductosDataActivo();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			foreach (var category in productos.Productos.Select(p => p.Categoria).Distinct())
			{
				CategoryListBox.Items.Add(category);
			}
			buscar.Focus();
			ordenId = orden;
			LoadOrden(ordenId);
		}

		public async void LoadOrden(int ordenId) {
			if (ordenId != 0)
			{

				var productosOrden = ordenes.GetProductosByOrdenId(ordenId);

				foreach (var productoOrden in productosOrden)
				{
					carritoItem nuevoProducto = new carritoItem
					{
						Nombre = productoOrden.Nombre,
						Cantidad = productoOrden.Cantidad,
						Precio = productoOrden.Precio,
						Id = productoOrden.ProductoId,

					};

					carrito.Items.Add(nuevoProducto);
				}
			}
		}
		
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void ProductIdTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SearchProduct();
				e.Handled = true;
			}
		}

		private void SearchProduct()
		{
			int productoCodigo;
			if (int.TryParse(buscar.Text, out productoCodigo))
			{
				var productoId = ordenes.GetProductIdByCodigo(productoCodigo);
				var product = productos.Productos.FirstOrDefault(p => p.Id == productoId);
				carritoItem existingProduct = carrito.Items.OfType<carritoItem>().FirstOrDefault(p => p.Id == productoId);
				if (product != null)
				{
					if (existingProduct != null)
					{
						existingProduct.Cantidad += 1;
						buscar.Text = "";
						carrito.Items.Refresh();
					}
					else { 

						var cartItem = new carritoItem
					{
						Id = product.Id,
						Nombre = product.Nombre,
						Precio = product.Precio,
						Cantidad = 1
					};

					carrito.Items.Add(cartItem);
					buscar.Text = "";
					carrito.Items.Refresh();
					}
					
				}
				else
				{
					MessageBox.Show("No se encontro el producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				MessageBox.Show("Por favor, introduzca un ID valido!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void CategoryButton_Click(object sender, RoutedEventArgs e)
		{
			ProductWrapPanel.Children.Clear();
			string selectedCategory = ((Button)sender).Content.ToString();
			var categoryProducts = productos.Productos.Where(p => p.Categoria == selectedCategory);
			foreach (var product in categoryProducts)
			{
				var button = new Button
				{
					Content = product.Nombre,
					Style = (Style)FindResource("ProductButtonStyle")
				};
				button.Click += ProductButton_Click;
				ProductWrapPanel.Children.Add(button);
			}
		}

		private void ProductButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			string productName = button.Content.ToString();
			var selectedProduct = productos.Productos.FirstOrDefault(p => p.Nombre == productName);
			carritoItem existingProduct = carrito.Items.OfType<carritoItem>().FirstOrDefault(p => p.Id == selectedProduct.Id);
			if (selectedProduct != null)
			{
				if (existingProduct != null)
				{
					existingProduct.Cantidad += 1;
					buscar.Text = "";
					carrito.Items.Refresh();
				}
				else
				{
					var cartItem = new carritoItem
					{
						Id = selectedProduct.Id,
						Nombre = selectedProduct.Nombre,
						Precio = selectedProduct.Precio,
						Cantidad = 1
					};
					carrito.Items.Add(cartItem);
				}

			}
		}

		
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new MenuPrincipal());
			}
		}
		

		public class Product
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public string Category { get; set; }
			public decimal Price { get; set; }
		}
		public class carritoItem
		{
			public int Id { get; set; }
			public string Nombre { get; set; }
			public decimal Precio { get; set; }
			public int Cantidad { get; set; }
		}

		public class nuevaOrden
		{
			public DateTime FechaCreacion { get; set; }
			public string Estado { get; set; }
		}


		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			dynamic selectedItem = carrito.SelectedItem;
			if (selectedItem != null)
			{
				selectedItem.Cantidad += 1;
				carrito.Items.Refresh();
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var selectedItem = carrito.SelectedItem as carritoItem;
			if (selectedItem != null)
			{
				selectedItem.Cantidad -= 1;


				if (selectedItem.Cantidad<= 0)
				{
					carrito.Items.Remove(selectedItem);
				}
				else
				{
					carrito.Items.Refresh();
				}
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var selectedItem = carrito.SelectedItem as carritoItem;
			
				if (selectedItem != null)
				{
					carrito.Items.Remove(selectedItem);

				}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var nuevoItem = new nuevoItemVentana();
			nuevoItem.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoItem.ShowDialog() == true)
			{
				string itemName = nuevoItem.Nombre;
				decimal itemPrice = nuevoItem.Precio;
				carritoItem existingProduct = carrito.Items.OfType<carritoItem>().FirstOrDefault(p => p.Nombre == itemName);
				if (existingProduct != null ) {
					existingProduct.Cantidad += 1;
					carrito.Items.Refresh();
				} else { 
				int idProducto = ordenes.InsertarProductoUnico(itemName, "Historial", itemPrice, false);
				carrito.Items.Add(new carritoItem { Id=idProducto ,Nombre = itemName, Precio = itemPrice, Cantidad = 1 });
				}
				
				
				
			}
		}

		private async void Button_Click_5(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
            if (carrito.Items.Count == 0)
            {
				MessageBox.Show("¡La orden esta vacia!", "Orden Invalida", MessageBoxButton.OK, MessageBoxImage.Error);
            }else {

				if (ordenId != 0 ) {
					ordenes.EliminarProductoOrden(ordenId);
					var productosOrden = new List<ProductoOrden>();
					var carritoItems = carrito.Items.OfType<carritoItem>().ToList();

					foreach (var item in carritoItems)
					{
						var productoOrden = new ProductoOrden
						{
							OrdenId = ordenId,
							ProductoId = item.Id,
							Cantidad = item.Cantidad
						};

						productosOrden.Add(productoOrden);
					}
					ordenes.CrearProductoOrden(productosOrden);
					if (parentWindow != null && parentWindow is MainWindow mainWindow)
					{
						mainWindow.mainFrame.Navigate(new ordenesListado());
					}

				} else {
					OrdenesViewModel ordenesViewModel = new OrdenesViewModel();

					var nuevaOrden = new OrdenesViewModel.Orden
					{
						Creacion = DateTime.Now,
						Estado = "Activa"
					};

					int ordenId = ordenesViewModel.CrearOrdenAsync(nuevaOrden);

					var productosOrden = new List<ProductoOrden>();
					var carritoItems = carrito.Items.OfType<carritoItem>().ToList();

					foreach (var item in carritoItems)
					{
						var productoOrden = new ProductoOrden
						{
							OrdenId = ordenId,
							ProductoId = item.Id,
							Cantidad = item.Cantidad
						};

						productosOrden.Add(productoOrden);
					}
					ordenesViewModel.CrearProductoOrden(productosOrden);
					if (parentWindow != null && parentWindow is MainWindow mainWindow)
					{
						mainWindow.mainFrame.Navigate(new ordenesListado());
					}
				}

			}
            
		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new ordenesListado());
			}
		}
	}
}
