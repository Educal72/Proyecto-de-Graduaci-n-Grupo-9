using System;
using System.Windows;
using FrontEndWPF;
using static FrontEndWPF.PuntoVenta;

namespace FrontEndWPF.Inventario
{
    public partial class nuevoProducto : Window
    {
        private int i;
        internal object id;
        internal object nombreProducto;
        internal object categoriaProducto;
        internal object precioProducto;
        internal object activoProducto;
		ProductosViewModel productosViewModel = new ProductosViewModel();

		public nuevoProducto()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los valores de los campos
            int codigoProducto = 12;
            string nombreProducto = Nombre.Text;
            string categoriaProducto = Categoria.Text;
            decimal precioProducto = Convert.ToDecimal(Precio.Text);
            bool activoProducto = Activo.IsChecked ?? false; // Obtener el estado del CheckBox

            // Llamar al método para realizar la inserción en la base de datos
            Conexion conexion = new Conexion();
            bool success = conexion.InsertarProducto(codigoProducto, nombreProducto, categoriaProducto, precioProducto, activoProducto);

            if (success)
            {
				var producto = new Producto
				{
					Codigo = codigoProducto.ToString(),
					Nombre = nombreProducto,
					Categoria = categoriaProducto,
					Precio = precioProducto,
					Activo = activoProducto
				};
				productosViewModel.Productos.Add(producto);
				MessageBox.Show("Producto guardado exitosamente.");
            }
            else
            {
                MessageBox.Show("Hubo un error al guardar el producto.");
            }

            // Cerrar la ventana después de guardar
            this.DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana sin guardar
            this.DialogResult = false;
        }
    }
}
