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
using static FrontEndWPF.empleadosAdmin;

namespace FrontEndWPF.Inventario
{
	/// <summary>
	/// Lógica de interacción para Inventario.xaml
	/// </summary>
	public partial class Inventario : Page
	{
		private DispatcherTimer timer;
		List<Inventarios> inventarios = new List<Inventarios>();
		List<Producto> productos = new List<Producto>();
		public Inventario()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			PopulateInventariosDataGrid();
			PopulateProductosDataGrid();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
        }

		public void PopulateInventariosDataGrid()
		{
			inventarios = new List<Inventarios>
		{
			new Inventarios { Id = 1, Nombre = "Producto A", Cantidad = 10, Precio = 20.5, Activo = true },
			new Inventarios { Id = 2, Nombre = "Producto B", Cantidad = 15, Precio = 15.75, Activo = false },
			new Inventarios { Id = 3, Nombre = "Producto C", Cantidad = 20, Precio = 30.0, Activo = true }
		};

			InventarioGrid.ItemsSource = inventarios;
		}

		public void PopulateProductosDataGrid()
		{
			productos = new List<Producto>
		{
			new Producto { Id = 1, Nombre = "Producto X", Categoria = "Categoría 1", Precio = 25.0, Activo = true },
			new Producto { Id = 2, Nombre = "Producto Y", Categoria = "Categoría 2", Precio = 18.5, Activo = true },
			new Producto { Id = 3, Nombre = "Producto Z", Categoria = "Categoría 1", Precio = 30.0, Activo = false }
		};

			ProductosGrid.ItemsSource = productos;
		}

		public class Inventarios
		{
			public int Id { get; set; }
			public string Nombre { get; set; }
			public int Cantidad { get; set; }
			public double Precio { get; set; }
			public bool Activo { get; set; }
		}

		public class Producto
		{
			public int Id { get; set; }
			public string Nombre { get; set; }
			public string Categoria { get; set; }
			public double Precio { get; set; }
			public bool Activo { get; set; }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var selectedValue = InventarioGrid.SelectedValue as Inventarios;
			if (selectedValue != null)
			{
				inventarios.Remove(selectedValue);
				InventarioGrid.Items.Refresh();
			}
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var selectedValue = ProductosGrid.SelectedValue as Producto;
			if (selectedValue != null)
			{
				productos.Remove(selectedValue);
				ProductosGrid.Items.Refresh();
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var nuevoInventario = new nuevoInventario();
			nuevoInventario.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			nuevoInventario.Titulo.Content = "Nuevo Item"; 
			if (nuevoInventario.ShowDialog() == true)
			{
				inventarios.Add(new Inventarios
				{
					Id = nuevoInventario.id,
					Nombre = nuevoInventario.nombre,
					Cantidad = nuevoInventario.cantidad,
					Precio = nuevoInventario.precio,
					Activo = nuevoInventario.activo
				});
				InventarioGrid.Items.Refresh();
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var selectedValue = InventarioGrid.SelectedValue as Inventarios;
			if (selectedValue != null)
			{
				var nuevoInventario = new nuevoInventario();
				nuevoInventario.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				nuevoInventario.Titulo.Content = "Editar Item";
				nuevoInventario.Nombre.Text = selectedValue.Nombre;
				nuevoInventario.Cantidad.Text = selectedValue.Cantidad.ToString();
				nuevoInventario.Precio.Text = selectedValue.Precio.ToString();
				nuevoInventario.Activo.IsChecked = selectedValue.Activo;
				if (nuevoInventario.ShowDialog() == true)
				{
					selectedValue.Nombre = nuevoInventario.nombre;
					selectedValue.Cantidad = nuevoInventario.cantidad;
					selectedValue.Precio = nuevoInventario.precio;
					selectedValue.Activo = nuevoInventario.activo;
					InventarioGrid.Items.Refresh();
				}
			}
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			var nuevoProducto = new nuevoProducto();
			nuevoProducto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			nuevoProducto.Titulo.Content = "Nuevo Producto";
			if (nuevoProducto.ShowDialog() == true)
			{
				productos.Add(new Producto
				{
					//Id = nuevoProducto.id,
					//Nombre = nuevoProducto.nombre,
					//Categoria = nuevoProducto.categoria,
					//Precio = nuevoProducto.precio,
					//Activo = nuevoProducto.activo
				});
				ProductosGrid.Items.Refresh();
			}
		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			var selectedValue = ProductosGrid.SelectedValue as Producto;
			if (selectedValue != null)
			{
				var nuevoProducto = new nuevoProducto();
				nuevoProducto.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				nuevoProducto.Titulo.Content = "Editar Producto";
				nuevoProducto.Nombre.Text = selectedValue.Nombre;
				nuevoProducto.Categoria.Text = selectedValue.Categoria;
				nuevoProducto.Precio.Text = selectedValue.Precio.ToString();
				nuevoProducto.Activo.IsChecked = selectedValue.Activo;
				if (nuevoProducto.ShowDialog() == true)
				{
					//selectedValue.Nombre = nuevoProducto.nombre;
					//selectedValue.Categoria = nuevoProducto.categoria;
					//selectedValue.Precio = nuevoProducto.precio;
					//selectedValue.Activo = nuevoProducto.activo;
					ProductosGrid.Items.Refresh();
				}
			}
		}
	}

}
