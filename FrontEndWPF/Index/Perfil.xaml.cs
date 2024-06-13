using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para Perfil.xaml
    /// </summary>
    public partial class Perfil : Page
    {
        private DispatcherTimer timer;
        private string userEmail;
        public string userRol = SesionUsuario.Instance.rol;
        public Conexion conexion = new Conexion();

        public Perfil()
        {
            InitializeComponent();
            foreach (ComboBoxItem item in comboRol.Items)
            {

                if (item.Content.ToString() == userRol)
                {
                    // Set the matched item as selected
                    comboRol.SelectedItem = item;
                    break; // Exit loop once found
                }
            }

            // Establecer el DataContext con el ViewModel
            DataContext = new PerfilViewModel();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Instrucciones enviadas al correo electrónico, favor revisarlo.", "Recuperación de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //var actualizarUsuario = new PerfilViewModel(); Esta pendiente revisar el modelo.
            //conexion.OpenConnection();
            conexion.ActualizarUsuario(correo.Text, Nombre.Text, primerApellido.Text, segundoApellido.Text, Cedula.Text, Telefono.Text, comboRol.Text);
            MessageBox.Show("Producto agregado Exitosamente!");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
        }
    }
}
