using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FrontEndWPF.Inventario
{
    public partial class Inventario : Page
    {
        private DispatcherTimer timer;
        private ProductosViewModel productosViewModel;
        private ProductosViewModel viewModel;


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
            var inventarios = new List<Inventarios>
            {
                new Inventarios { Id = 1, Nombre = "Producto A", Cantidad = 10, Precio = 20.5, Activo = true },
                new Inventarios { Id = 2, Nombre = "Producto B", Cantidad = 15, Precio = 15.75, Activo = false },
                new Inventarios { Id = 3, Nombre = "Producto C", Cantidad = 20, Precio = 30.0, Activo = true }
            };

            InventarioGrid.ItemsSource = inventarios;
        }

        public void PopulateProductosDataGrid()
        {
            ProductosGrid.ItemsSource = productosViewModel.Productos;
        }

        public class Inventarios
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
            public double Precio { get; set; }
            public bool Activo { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedValue = InventarioGrid.SelectedItem as Inventarios;
            if (selectedValue != null)
            {
                var inventarios = InventarioGrid.ItemsSource as List<Inventarios>;
                inventarios.Remove(selectedValue);
                InventarioGrid.Items.Refresh();
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
                var inventarios = InventarioGrid.ItemsSource as List<Inventarios>;
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
            var selectedValue = InventarioGrid.SelectedItem as Inventarios;
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

                    // Refresca la vista
                    MessageBox.Show("Producto actualizado exitosamente!");
                    ProductosGrid.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para editar.", "Editar Producto", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
