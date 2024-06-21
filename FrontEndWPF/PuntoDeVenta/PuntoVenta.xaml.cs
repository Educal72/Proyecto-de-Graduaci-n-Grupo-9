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
    /// Lógica de interacción para PuntoVenta.xaml
    /// </summary>
    public partial class PuntoVenta : UserControl
    {
		private DispatcherTimer timer;

		private List<Product> products = new List<Product>
		{
			new Product { Id=1, Name = "Producto 1", Category = "Category 1", Price = 1299.00 },
			new Product { Id=2, Name = "Producto 2", Category = "Category 2", Price = 1399.00 },
			new Product { Id=3, Name = "Producto 3", Category = "Category 1", Price = 1499.00 },
			new Product { Id=4, Name = "Producto 4", Category = "Category 2", Price = 1599.00 },
			new Product { Id=5, Name = "Producto 5", Category = "Category 1", Price = 1899.00 },
			new Product { Id=6, Name = "Producto 6", Category = "Category 3" , Price = 1099.00},
			new Product { Id=7, Name = "Producto 7", Category = "Category 1", Price = 1099.00 },
			new Product { Id=8, Name = "Producto 8", Category = "Category 2" , Price = 999.00},
			new Product { Id=11, Name = "Producto 11", Category = "Category 2" , Price = 999.00},
			new Product { Id=9, Name = "Producto 9", Category = "Category 4" , Price = 1299.00},
			new Product { Id=10, Name = "Producto 10", Category = "Category 2", Price = 1299.00 },
			new Product { Id=14, Name = "Producto 14", Category = "Category 2", Price = 1299.00 },
			new Product { Id=16, Name = "Producto 16", Category = "Category 2", Price = 1299.00 },
			new Product { Id=96, Name = "Producto 96", Category = "Category 2", Price = 1299.00 },
			new Product { Id=78, Name = "Producto 78", Category = "Category 2", Price = 1299.00 },
			new Product { Id=89, Name = "Producto 89", Category = "Category 2", Price = 1299.00 },
			new Product { Id=18, Name = "Producto 18", Category = "Category 2", Price = 1299.00 },
			new Product { Id=28, Name = "Producto 28", Category = "Category 2", Price = 1299.00 },
			new Product { Id=16, Name = "Producto 16", Category = "Category 2", Price = 1299.00 },
			new Product { Id=15, Name = "Producto 15", Category = "Category 2", Price = 1299.00 },

		};

		public PuntoVenta()
        {
            InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			foreach (var category in products.Select(p => p.Category).Distinct())
			{
				CategoryListBox.Items.Add(category);
			}
			buscar.Focus();
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
			int productId;
			if (int.TryParse(buscar.Text, out productId))
			{
				Product product = products.FirstOrDefault(p => p.Id == productId);
				carritoItem existingProduct = carrito.Items.OfType<carritoItem>().FirstOrDefault(p => p.Id == productId);
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
						Nombre = product.Name,
						Precio = product.Price,
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
			var categoryProducts = products.Where(p => p.Category == selectedCategory);
			foreach (var product in categoryProducts)
			{
				var button = new Button
				{
					Content = product.Name,
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
			Product selectedProduct = products.FirstOrDefault(p => p.Name == productName);
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
						Nombre = selectedProduct.Name,
						Precio = selectedProduct.Price,
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
			public double Price { get; set; }
		}
		public class carritoItem
		{
			public int Id { get; set; }
			public string Nombre { get; set; }
			public double Precio { get; set; }
			public int Cantidad { get; set; }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			dynamic selectedItem = carrito.SelectedItem;
			if (selectedItem != null)
			{
				selectedItem.Cantidad += 1;
				carrito.Items.Refresh();
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
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var selectedItem = carrito.SelectedItem as carritoItem;
			
				if (selectedItem != null)
				{
					carrito.Items.Remove(selectedItem);

				}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var nuevoItem = new nuevoItemVentana();
			nuevoItem.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoItem.ShowDialog() == true)
			{

				string itemName = nuevoItem.Nombre;
				double itemPrice = nuevoItem.Precio;


				carrito.Items.Add(new carritoItem { Nombre = itemName, Precio = itemPrice, Cantidad = 1 });
			}
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (parentWindow != null && parentWindow is MainWindow mainWindow)
			{
				mainWindow.mainFrame.Navigate(new ordenesListado());
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
