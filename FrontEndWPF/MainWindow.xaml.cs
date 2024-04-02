using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FrontEndWPF;

namespace FrontEndWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			WindowState = WindowState.Maximized;
			Uri iconUri = new Uri("D:\\Visual Studio Apps\\FrontEnd\\FrontEndWPF\\icono.ico", UriKind.RelativeOrAbsolute);
			this.Icon = BitmapFrame.Create(iconUri);
			Login Pagina2 = new Login();
			mainFrame.Navigate(Pagina2);
		}
		public void ChangePageToPuntoVenta()
		{
			mainFrame.Navigate(new PuntoVenta());
		}
	}
}