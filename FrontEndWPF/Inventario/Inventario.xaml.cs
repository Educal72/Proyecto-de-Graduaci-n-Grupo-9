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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using FrontEndWPF.Index;
using FrontEndWPF.ViewModel;


namespace FrontEndWPF.Inventario
{

    public partial class Inventario : Page
    {
        private DispatcherTimer timer;
        private ProductosViewModel productosViewModel;
        private ProductosViewModel viewModel;
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
            viewModel = new ProductosViewModel();
            DataContext = viewModel;
            DataContext = productosViewModel;
            ProductosGrid.ItemsSource = productosViewModel.Productos;
            PopulateInventariosDataGrid();
            PopulateProductosDataGrid();
			user.Content = "Usuario: " + SesionUsuario.Instance.nombre;
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
			string query = @"SELECT Id, Nombre, Cantidad, Precio, Activo FROM Inventario ORDER BY Activo DESC";

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
							Cantidad = Convert.ToInt32(reader["Cantidad"]),
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
			ProductosGrid.ItemsSource = productosViewModel.Productos;
		}


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedValue = InventarioGrid.SelectedValue as InventarioM;
            if (selectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista para eliminar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Salir del método si no hay ningún elemento seleccionado
            }

            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea eliminar este inventario?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool resultQuery = conexion.DeleteInventario(selectedValue.Id);
                PopulateInventariosDataGrid();
                if (resultQuery)
                {
                    MessageBox.Show("El inventario fue eliminado exitosamente", "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el inventario", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedValue = ProductosGrid.SelectedItem as Producto;
            if (selectedValue == null)
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.", "Selección requerida", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; // Salir del método si no hay ningún producto seleccionado
            }

            MessageBoxResult result = MessageBox.Show("¿Está seguro que desea desactivar este producto?", "Confirmar Eliminación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool eliminado = productosViewModel.EliminarProducto(selectedValue.Id);
                if (eliminado)
                {
                    MessageBox.Show("Producto desactivado exitosamente!");
                    ProductosGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Error al desactivar el producto.");
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var nuevoInventario = new nuevoInventario();
			nuevoInventario.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			nuevoInventario.Titulo.Content = "Nuevo Inventario";
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
            if (selectedValue == null)
            {
                MessageBox.Show("Seleccione un un elemento de la lista para poder editar.", "Editar Inventario", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nuevoInventario = new nuevoInventario();
            nuevoInventario.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            nuevoInventario.Titulo.Content = "Editar Inventario";
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
				productosViewModel.LoadProductosData();
                PopulateProductosDataGrid();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var selectedValue = ProductosGrid.SelectedItem as Producto;
            if (selectedValue != null)
            {
                var editarProductoWindow = new EditarProductoWindow(selectedValue);
                editarProductoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                if (editarProductoWindow.ShowDialog() == true)
                {
                    // Actualiza el producto en el ViewModel
                    viewModel.ActualizarProducto(
                        selectedValue.Id,
                        int.Parse(editarProductoWindow.CodigoTextBox.Text),
                        editarProductoWindow.NombreTextBox.Text,
                        editarProductoWindow.CategoriaTextBox.Text,
                        decimal.Parse(editarProductoWindow.PrecioTextBox.Text),
                        editarProductoWindow.ActivoCheckBox.IsChecked.GetValueOrDefault()
                    );
                    ProductosGrid.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para editar.", "Editar Producto", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

		private void Fichaje_Click(object sender, RoutedEventArgs e)
		{
			FichajesViewModel fichajesViewModel = new FichajesViewModel();
			CodigoBarras barcodeWindow = new CodigoBarras(fichajesViewModel);
			barcodeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			barcodeWindow.ShowDialog(); // Abre la ventana emergente y espera su cierre
		}

		private void ToggleButton_Checked(object sender, RoutedEventArgs e)
		{

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(codigo.Text, out int result)) {
                productosViewModel.GetProductoEspecifico(Convert.ToInt32(codigo.Text));
                ProductosGrid.ItemsSource = productosViewModel.Productos; } 
            else {
                MessageBox.Show("Número de Código no válido.", "Error", MessageBoxButton.OK,MessageBoxImage.Error );
            } 

		}

		private void Button_Click_8(object sender, RoutedEventArgs e)
		{
			productosViewModel = new ProductosViewModel();
            productosViewModel.LoadProductosData();
            PopulateProductosDataGrid();
			ProductosGrid.Items.Refresh();
		}
	}
}
