using System;
using System.Windows;
using System.Windows.Media.Imaging;
using FrontEndWPF;
using FrontEndWPF.Empleados;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using System.IO;
using System.Windows.Input;
using System.IO.Ports;
using System.Text;
using Linearstar.Windows.RawInput;
using System.Reflection.Metadata;
using System.Windows.Interop;
using System.Threading;
using System.Collections.Concurrent;
using System.Net;


namespace FrontEndWPF
{
	public partial class MainWindow : Window
	{
		public string Usuario;

		

		public MainWindow()
		{
			InitializeComponent();
			WindowState = WindowState.Maximized;
			Uri iconUri = new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/icono.ico", UriKind.RelativeOrAbsolute);
			this.Icon = BitmapFrame.Create(iconUri);
			Login Pagina2 = new Login();
			mainFrame.Navigate(Pagina2);

		}

		

		public void ChangePageToPuntoVenta()
		{
			mainFrame.Navigate(new PuntoVenta(0));
		}

		public void ChangePageToMetricas(int id)
		{
			empleadosAdmin empleadosAdmin = new empleadosAdmin();
			empleadosAdmin.ContentArea.Content = new Metricas(id);
			mainFrame.Navigate(empleadosAdmin);
		}

		
	}
}
