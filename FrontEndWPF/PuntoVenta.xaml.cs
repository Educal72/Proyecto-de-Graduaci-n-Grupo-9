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
    /// Lógica de interacción para PuntoVenta.xaml
    /// </summary>
    public partial class PuntoVenta : UserControl
    {
		private DispatcherTimer timer;

		private List<Product> products = new List<Product>
		{
			new Product { Name = "Product 1", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1" , Price = 1299.00},
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 2", Category = "Category 1" , Price = 1299.00},
			new Product { Name = "Product 2", Category = "Category 1" , Price = 1299.00},
			new Product { Name = "Product 2", Category = "Category 1", Price = 1299.00 },
			new Product { Name = "Product 3", Category = "Category 2", Price = 1299.00},
            // Add more products and categories as needed
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
		}
		
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void CategoryButton_Click(object sender, RoutedEventArgs e)
		{
			ProductWrapPanel.Children.Clear(); // Clear existing buttons
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
			if (selectedProduct != null)
			{
				// Create a new CartItem object
				var cartItem = new carritoItem
				{
					Nombre = selectedProduct.Name,
					Precio = selectedProduct.Price,
					Cantidad = 1
				};

				// Add the new CartItem to the DataGrid
				carrito.Items.Add(cartItem);
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
			public string Name { get; set; }
			public string Category { get; set; }
			public double Price { get; set; }
		}
		public class carritoItem
		{
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
	}
}
