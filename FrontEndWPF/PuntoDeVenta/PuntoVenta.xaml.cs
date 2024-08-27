using DocumentFormat.OpenXml.Drawing.Diagrams;
using FrontEndWPF.Index;
using FrontEndWPF.PuntoDeVenta;
using FrontEndWPF.Reporteria;
using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
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
using static ClosedXML.Excel.XLPredefinedFormat;
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
		public FichajesViewModel fichajesViewModel = new FichajesViewModel();
		private StringBuilder barcode = new StringBuilder();
		ProductosViewModel productos = new ProductosViewModel();
		OrdenesViewModel ordenes = new OrdenesViewModel();
		CierreViewModel cierreViewModel = new CierreViewModel();
		string nombreImpresora;
		string codigoControl;
		string descargarComo;
		string codigoDeControl;
		decimal balanceInicial;

		int ordenId = 0;
		public PuntoVenta(int orden)
		{
			InitializeComponent();
			productos.LoadProductosDataActivo();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			foreach (var category in productos.Productos.Select(p => p.Categoria).Distinct())
			{
				CategoryListBox.Items.Add(category);
			}
			buscar.Focus();
			ordenId = orden;
			LoadOrden(ordenId);
			user.Content = "Usuario: " + SesionUsuario.Instance.nombre;
		}

		public async void LoadOrden(int ordenId)
		{
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
			fecha.Content = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
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
					else
					{

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
					Style = (System.Windows.Style)FindResource("ProductButtonStyle")
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
			public System.DateTime FechaCreacion { get; set; }
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


				if (selectedItem.Cantidad <= 0)
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
				if (existingProduct != null)
				{
					existingProduct.Cantidad += 1;
					carrito.Items.Refresh();
				}
				else
				{
					int idProducto = ordenes.InsertarProductoUnico(itemName, "Historial", itemPrice, false);
					carrito.Items.Add(new carritoItem { Id = idProducto, Nombre = itemName, Precio = itemPrice, Cantidad = 1 });
				}



			}
		}

		private async void Button_Click_5(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			if (carrito.Items.Count == 0)
			{
				MessageBox.Show("¡La orden esta vacia!", "Orden Invalida", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{

				if (ordenId != 0)
				{
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

				}
				else
				{
					OrdenesViewModel ordenesViewModel = new OrdenesViewModel();

					var nuevaOrden = new OrdenesViewModel.Orden
					{
						Creacion = System.DateTime.Now,
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

		private void Fichaje_Click(object sender, RoutedEventArgs e)
		{

			CodigoBarras barcodeWindow = new CodigoBarras(fichajesViewModel);
			barcodeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			barcodeWindow.ShowDialog(); // Abre la ventana emergente y espera su cierre
		}

		private void Gabinete_Click(object sender, RoutedEventArgs e)
		{
			FileReadImpresora();
			Gabinete nuevoItem = new Gabinete();
			nuevoItem.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoItem.ShowDialog() == true)
			{
				var tipo = nuevoItem.tipo;
				MessageBox.Show("La operación de " + tipo + " fue un exito", tipo + " Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
				string openDrawerCommand = "\x1B\x70\x01\x32\x32";
				RawPrinterHelper.SendStringToPrinter(nombreImpresora, openDrawerCommand);
			}
		}

		private void PrintDirect(byte[] command, string printerName)
		{
			// Create a connection to the printer
			using (var printServer = new LocalPrintServer())
			using (var printQueue = printServer.GetPrintQueue(printerName))
			using (var printJob = printQueue.AddJob("Open Cash Drawer"))
			{
				using (var writer = new BinaryWriter(printJob.JobStream))
				{
					writer.Write(command);
				}
			}

		}
		private void Cierre_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("¿Está seguro que desea hacer el cierre de caja?", "Confirmar Cierre", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (result == MessageBoxResult.Yes)
			{
				if (cierreViewModel.TerminarCierre())
				{
					MessageBox.Show("El cierre se realizó correctamente.", "Cierre de Caja", MessageBoxButton.OK, MessageBoxImage.Information);
					Window parentWindow = Window.GetWindow(this);
					if (parentWindow != null && parentWindow is MainWindow mainWindow)
					{
						mainWindow.mainFrame.Navigate(new MenuPrincipal());
					}
				}
				else
				{
					MessageBox.Show("Error durante el cierre.", "Cierre de Caja", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}
		public void FileReadImpresora()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/impresora_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/impresora_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "NombreImpresora":
								nombreImpresora = parts[1];
								break;
							case "DescargarComo":
								descargarComo = parts[1];
								break;
							case "CodigoDeControl":
								codigoDeControl = parts[1];
								break;
							case "BalanceInicial":
								balanceInicial = Convert.ToDecimal(parts[1]);
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
		}
	}
}
