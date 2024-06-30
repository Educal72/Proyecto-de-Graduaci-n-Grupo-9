using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    public partial class EditarProductoWindow : Window
    {
        public Producto Producto { get; set; }
		Conexion conexion = new Conexion();

        public EditarProductoWindow(Producto producto)
        {
            InitializeComponent();
            Producto = producto;
            DataContext = Producto;

            AceptarButton.Click += AceptarButton_Click;
            CancelarButton.Click += CancelarButton_Click;
            // Inicializar los campos con los datos del producto
            CodigoTextBox.Text = Producto.Codigo.ToString();
            NombreTextBox.Text = Producto.Nombre;
            CategoriaTextBox.Text = Producto.Categoria;
            PrecioTextBox.Text = Producto.Precio.ToString();
            ActivoCheckBox.IsChecked = Producto.Activo;
        }

		private bool ValidateInputs(out string errorMessage)
		{
			errorMessage = string.Empty;

			// Validar Nombre
			if (string.IsNullOrWhiteSpace(NombreTextBox.Text))
			{
				errorMessage = "El campo Nombre es obligatorio.";
				return false;
			}

			// Validar Cedula
			if (string.IsNullOrWhiteSpace(CategoriaTextBox.Text))
			{
				errorMessage = "El campo Cantidad es obligatorio.";
				return false;
			}

			// Validar Salario
			if (!decimal.TryParse(PrecioTextBox.Text, out _))
			{
				errorMessage = "El campo Precio debe ser un número válido.";
				return false;
			}

			if (Producto.Codigo != Convert.ToInt32(CodigoTextBox.Text)) {
				bool resultado = conexion.ProductoExists(CodigoTextBox.Text);
				if (!int.TryParse(CodigoTextBox.Text, out _) || resultado)
				{
					errorMessage = "El codigo debe ser un número valido o ya esta asignado a otro producto.";
					return false;
				}
			}
			return true;
		}

		private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
			if (!ValidateInputs(out string errorMessage))
			{
				MessageBox.Show(errorMessage);
				return;
			}
			Producto.Codigo = Convert.ToInt32(CodigoTextBox.Text);
            Producto.Nombre = NombreTextBox.Text;
            Producto.Categoria = CategoriaTextBox.Text;
            Producto.Precio = decimal.Parse(PrecioTextBox.Text);
            Producto.Activo = ActivoCheckBox.IsChecked.GetValueOrDefault();
            DialogResult = true;
        }

        private void CancelarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
