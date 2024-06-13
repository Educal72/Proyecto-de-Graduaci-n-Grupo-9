using System.Windows;

namespace FrontEndWPF
{
    public partial class EditarProductoWindow : Window
    {
        public Producto Producto { get; set; }

        public EditarProductoWindow(Producto producto)
        {
            InitializeComponent();
            Producto = producto;
            DataContext = Producto;

            AceptarButton.Click += AceptarButton_Click;
            CancelarButton.Click += CancelarButton_Click;
            // Inicializar los campos con los datos del producto
            CodigoTextBox.Text = Producto.Codigo;
            NombreTextBox.Text = Producto.Nombre;
            CategoriaTextBox.Text = Producto.Categoria;
            PrecioTextBox.Text = Producto.Precio.ToString();
            ActivoCheckBox.IsChecked = Producto.Activo;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            Producto.Codigo = CodigoTextBox.Text;
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
