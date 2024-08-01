using FrontEndWPF.Empleados;
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
using System.Windows.Threading;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para empleadosNoAdmin.xaml
    /// </summary>
    public partial class empleadosNoAdmin : Page
    {
        private DispatcherTimer timer;

        //Variable estatica booleana.
        static bool a;

        public empleadosNoAdmin()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        

        /* Este método sirve para poder saber si el usuario administrador quiere o no entrar -
         * a algún apartado de la sección de empleados.*/
        public void Enviar(bool dato)
        {
            a = dato;
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/Perfil.xaml", UriKind.Relative));
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentArea.Content = null;
            var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            switch (selectedItem)
            {
                case "Desvinculaciones":
                    ContentArea.Content = new Desvinculaciones();
                    if (a == true)
                    {
                        ContentArea.Content = new PantallaOscura();
                    }
                    break;
            }
        }

    }
}
