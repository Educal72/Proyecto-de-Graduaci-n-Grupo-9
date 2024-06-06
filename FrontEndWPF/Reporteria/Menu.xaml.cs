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
            comboBox.Items.Add("Informes");
            comboBox.Items.Add("Registro Financiero");
            comboBox.Items.Add("Auditoria");
            comboBox.Items.Add("Normativas");
            comboBox.SelectedIndex = 1;
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


		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem as string == "Informes")
            {
                MenuListBox.Visibility = Visibility.Visible;
                listBox = MenuListBox;
            }
            else
            {
                MenuListBox.Visibility = Visibility.Collapsed;
            }

            if (comboBox.SelectedItem as string == "Registro Financiero")
            {
                MenuListBox1.Visibility = Visibility.Visible;
                listBox = MenuListBox1;
            }
            else
            {
                MenuListBox1.Visibility = Visibility.Collapsed;
            }

            if (comboBox.SelectedItem as string == "Auditoria")
            {
                MenuListBox2.Visibility = Visibility.Visible;
                listBox = MenuListBox2;
            }
            else
            {
                MenuListBox2.Visibility = Visibility.Collapsed;
            }

            if (comboBox.SelectedItem as string == "Normativas")
            {
                MenuListBox3.Visibility = Visibility.Visible;
                listBox = MenuListBox3;
            }
            else
            {
                MenuListBox3.Visibility = Visibility.Collapsed;
            }
        }
        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var selectedItem = (listBox.SelectedItem as ListBoxItem)?.Content.ToString();



            switch (selectedItem)
            {
                case "Inicios de sesion":
                    MainFrame.Navigate(new InicioDeSesion());
                    break;
                case "Gestion de Ordenes":
                    MainFrame.Navigate(new GestionDeOrdenes());
                    break;
                case "Flujos Financieros":
                    MainFrame.Navigate(new FlujosFinancieros());
                    break;
                case "Visualizar Prestamos":
                    MainFrame.Navigate(new VisualizarPrestamos());
                    break;
                case "Visualizar Financiamientos":
                    MainFrame.Navigate(new VisualizarFinanciamientos());
                    break;
                case "Visualizar Inversiones":
                    MainFrame.Navigate(new VisualizarInversiones());
                    break;
                case "Visualizar Auditorias":
                    MainFrame.Navigate(new InformeAuditoria());
                    break;
                case "Crear Informe de Auditoria":
                    MainFrame.Navigate(new CrearInformeAuditoria());
                    break;
                case "Visualizar Normativas":
                    MainFrame.Navigate(new InformeNormativa());
                    break;
                case "Crear Informe de Normativa":
                    MainFrame.Navigate(new CrearNormativa());
                    break;
                case "Crear Financiamiento":
                    MainFrame.Navigate(new CrearFinanciamiento());
                    break;
                case "Crear Prestamo":
                    MainFrame.Navigate(new CrearPrestamos());
                    break;
                case "Crear Inversion":
                    MainFrame.Navigate(new CrearInversion());
                    break;
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index/MenuPrincipal.xaml", UriKind.Relative));
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            comboBox.Items.Clear(); // Elimina el TextBox cuando el ComboBox obtiene el foco
        }



    }
}
