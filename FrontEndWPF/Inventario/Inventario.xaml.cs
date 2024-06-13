using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

            productosViewModel = new ProductosViewModel();
            DataContext = productosViewModel;
            ProductosGrid.ItemsSource = productosViewModel.Productos;

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





    


		public class Inventarios
		{
			public int Id { get; set; }
			public string Nombre { get; set; }
			public int Cantidad { get; set; }
			public decimal Precio { get; set; }
			public bool Activo { get; set; }
		}


        public void PopulateProductosDataGrid()
        {
            ProductosGrid.ItemsSource = productosViewModel.Productos;
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
            var selectedValue = ProductosGrid.SelectedItem as Producto;
            if (selectedValue != null)
            {
                bool eliminado = productosViewModel.EliminarProducto(selectedValue.Id);
                if (eliminado)
                {
                    MessageBox.Show("Producto eliminado exitosamente!");
                    ProductosGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el producto.");
                }
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
                productosViewModel.Productos.Add(new Producto
                {
                    //Codigo = nuevoProducto.id,
                    //Nombre = nuevoProducto.nombreProducto,
                    //Categoria = nuevoProducto.categoriaProducto,
                    //Precio = nuevoProducto.precioProducto,
                    //Activo = nuevoProducto.activoProducto
                });
                ProductosGrid.Items.Refresh();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var selectedValue = ProductosGrid.SelectedItem as Producto;
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
                    //selectedValue.Nombre = nuevoProducto.nombreProducto;
                    //selectedValue.Categoria = nuevoProducto.categoriaProducto;
                    //selectedValue.Precio = nuevoProducto.precioProducto;
                    //selectedValue.Activo = nuevoProducto.activoProducto;
                    //ProductosGrid.Items.Refresh();
                }
            }
        }
    }
}
