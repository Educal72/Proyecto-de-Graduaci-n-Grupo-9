using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static FrontEndWPF.Modelos.UserModel;
using static FrontEndWPF.Modelos.InventarioModel;

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
		Conexion conexion = new Conexion();
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
			string query = @"SELECT Id, Nombre, Cantidad, Precio, Activo FROM Inventario";

			List<InventarioM> inventarios = new List<InventarioM>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						inventarios.Add(new InventarioM() 
						{
							Id = Convert.ToInt32(reader["Id"]),
							Nombre = reader["Nombre"].ToString(),
							Cantidad = Convert.ToInt32(reader["Id"]),
							Precio = Convert.ToDecimal(reader["Precio"]),
							Activo = Convert.ToBoolean(reader["Activo"])
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

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
			public decimal Precio { get; set; }
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
			var selectedValue = InventarioGrid.SelectedValue as InventarioM;
			
			if (selectedValue != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Esta seguro que desea eliminar esta entrada del inventario?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes) {
					bool resultQuery = conexion.DeleteInventario(selectedValue.Id);
					PopulateInventariosDataGrid();
					if (resultQuery)
					{
						MessageBox.Show("La entrada fue eliminada exitosamente", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
					}
					else {
						MessageBox.Show("La entrada no se pudo eliminar", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
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
				string Nombre = nuevoInventario.nombre;
				int Cantidad = nuevoInventario.cantidad;
				decimal Precio = nuevoInventario.precio;
				bool Activo = nuevoInventario.activo;
				conexion.AddInventario(Nombre, Cantidad, Precio, Activo);
				PopulateInventariosDataGrid();
			}
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var selectedValue = InventarioGrid.SelectedValue as InventarioM;
			if (selectedValue != null)
			{
				var nuevoInventario = new nuevoInventario();
				nuevoInventario.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				nuevoInventario.Titulo.Content = "Editar Item";
				var inventarioSeleccionado = conexion.GetInventarioById(selectedValue.Id);
				nuevoInventario.Nombre.Text = inventarioSeleccionado["Nombre"].ToString();
				nuevoInventario.Cantidad.Text = inventarioSeleccionado["Cantidad"].ToString();
				nuevoInventario.Precio.Text = inventarioSeleccionado["Precio"].ToString();
				nuevoInventario.Activo.IsChecked = Convert.ToBoolean(inventarioSeleccionado["Activo"]);
				if (nuevoInventario.ShowDialog() == true)
				{

					string Nombre = nuevoInventario.nombre;
					int Cantidad = nuevoInventario.cantidad;
					decimal Precio = nuevoInventario.precio;
					bool Activo = nuevoInventario.activo;
					conexion.EditInventario(Convert.ToInt32(inventarioSeleccionado["Id"]), Nombre, Cantidad, Precio, Activo);
					PopulateInventariosDataGrid();
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
					Id = nuevoProducto.id,
					Nombre = nuevoProducto.nombre,
					Categoria = nuevoProducto.categoria,
					Precio = nuevoProducto.precio,
					Activo = nuevoProducto.activo
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
					selectedValue.Nombre = nuevoProducto.nombre;
					selectedValue.Categoria = nuevoProducto.categoria;
					selectedValue.Precio = nuevoProducto.precio;
					selectedValue.Activo = nuevoProducto.activo;
					ProductosGrid.Items.Refresh();
				}
			}
		}
	}

}
