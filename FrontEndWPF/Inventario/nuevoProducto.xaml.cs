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
		Conexion conexion = new Conexion();

        public object Codigo { get; internal set; }

        public nuevoProducto()
        {
            InitializeComponent();
        }

		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			// Validar Nombre
			if (string.IsNullOrWhiteSpace(Nombre.Text))
			{
				errorMessage = "El campo Nombre es obligatorio.";
				return false;
			}

			// Validar Cedula
			if (string.IsNullOrWhiteSpace(Categoria.Text))
			{
				errorMessage = "El campo Cantidad es obligatorio.";
				return false;
			}

			// Validar Salario
			if (!decimal.TryParse(Precio.Text, out _))
			{
				errorMessage = "El campo Precio debe ser un número válido.";
				return false;
			}

			bool resultado = conexion.ProductoExists(CodigoTo.Text);
			// Validar Codigo
			if (!int.TryParse(CodigoTo.Text, out _) || resultado)
			{
				errorMessage = "El codigo debe ser un número valido o ya esta asignado a otro producto.";
				return false;
			}

			// Todas las validaciones realizadas
			return true;
		}

		private void Guardar_Click(object sender, RoutedEventArgs e)
        {
			if (!ValidateInputs(out string errorMessage))
			{
				MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			// Obtener los valores de los campos
			int codigoProducto = Convert.ToInt32(CodigoTo.Text);
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
