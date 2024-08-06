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

namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
		private DispatcherTimer timer;
		public ListBox listBox {  get; set; }
        
        public Menu()
        {
            InitializeComponent();
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

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

			ContentArea.Content = null;
			var selectedItem = (MenuListBox.SelectedItem as ListBoxItem)?.Content.ToString();
			switch (selectedItem)
            {
                case "Inicios de Sesión":
					ContentArea.Content = new InicioDeSesion();
                    break;
                case "Gestión de Ordenes":
					ContentArea.Content = new GestionDeOrdenes();
                    break;
                case "Flujos Financieros":
					ContentArea.Content = new FlujosFinancieros();
                    break;
                case "Préstamos":
					ContentArea.Content = new VisualizarPrestamos();
                    break;
                case "Financiamientos":
					ContentArea.Content = new VisualizarFinanciamientos();
                    break;
                case "Inversiones":
					ContentArea.Content = new VisualizarInversiones();
                    break;
                case "Auditorías":
					ContentArea.Content = new InformeAuditoria();
                    break;
                case "Normativa":
					ContentArea.Content = new InformeNormativa();
                    break;
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
        }

    }
}
