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

namespace FrontEndWPF.Index
{
    /// <summary>
    /// Lógica de interacción para RecuperarContraseña.xaml
    /// </summary>
    public partial class RecuperarContraseña : Page
    {
        public RecuperarContraseña()
        {
            InitializeComponent();
        }

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("Instrucciones enviadas al correo electrónico, favor revisarlo.", "Recuperación de Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
			NavigationService.Navigate(new Uri("Index/Login.xaml", UriKind.Relative));
		}
    }
}
