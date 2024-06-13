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
        public empleadosNoAdmin()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        private DispatcherTimer timer;

        private void MenuListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

            ContentArea.Content = null;
            var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            switch (selectedItem)
            {
                case "Desvinculaciones":
                    ContentArea.Content = new Desvinculaciones();
                    break;
            }
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

        public class Metrica
        {
            public string Empleado { get; set; }
            public int HorasTrabajadas { get; set; }
            public int Productividad { get; set; }
            public int VentasRealizadas { get; set; }
        }
     
        public class Desvinculacion
        {
            public int Id { get; set; } // Identificador único de la desvinculación
            public string Empleado { get; set; } // Nombre del empleado desvinculado
            public DateTime Fecha { get; set; } // Fecha de la desvinculación
            public string Motivo { get; set; } // Motivo de la desvinculación
            public string Comentarios { get; set; } // Comentarios adicionales sobre la desvinculación
            public bool Reconocido { get; set; }
        }

        public class PerfilEmpleoBuscado
        {
            public int Id { get; set; } // Identificador único del perfil de empleo buscado
            public string Titulo { get; set; } // Título del perfil de empleo
            public string Descripcion { get; set; } // Descripción del perfil de empleo
            public string NivelExperiencia { get; set; } // Nivel de experiencia requerido
            public string Requisitos { get; set; } // Requisitos del perfil de empleo
            public string Ubicacion { get; set; } // Ubicación del trabajo
            public double Salario { get; set; } // Salario mínimo ofrecido
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentArea.Content = null;
            var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();

            switch (selectedItem)
            {
                case "Desvinculaciones":
                    ContentArea.Content = new Desvinculaciones();
                    break;
            }
        }

    }
}
