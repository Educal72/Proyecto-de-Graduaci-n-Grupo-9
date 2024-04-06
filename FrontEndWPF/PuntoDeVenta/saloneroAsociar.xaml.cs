using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para saloneroAsociar.xaml
	/// </summary>
	public partial class saloneroAsociar : Window
	{
		private DispatcherTimer timer;
		private ICollectionView customerView;
		public string NombreCompleto { get; set; }
		public saloneroAsociar()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			LoadCustomers();
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void LoadCustomers()
		{
			var cliente1 = new Cliente
			{
				Cedula = "1122334455",
				Nombre = "Salonero 1",
				Apellidos = "Salonero 1",
				CorreoElectronico = "salonero1@gmail.com",
				Telefono = "11223344",
			};
			var cliente2 = new Cliente
			{
				Cedula = "1234567890",
				Nombre = "John",
				Apellidos = "Doe",
				CorreoElectronico = "john.doe@example.com",
				Telefono = "123-456-7890",
			};
			var cliente3 = new Cliente
			{
				Cedula = "0987654321",
				Nombre = "Jane",
				Apellidos = "Smith",
				CorreoElectronico = "jane.smith@example.com",
				Telefono = "987-654-3210",
			};
			CustomerDataGrid.Items.Add(cliente1);
			CustomerDataGrid.Items.Add(cliente2);
			CustomerDataGrid.Items.Add(cliente3);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Cliente selectedCustomer = (Cliente)CustomerDataGrid.SelectedItem;

			if (selectedCustomer != null)
			{
				NombreCompleto = selectedCustomer.Nombre + " " + selectedCustomer.Apellidos;
				DialogResult = true;
			}
		}
		public class Cliente
		{
			public string Cedula { get; set; }
			public string Nombre { get; set; }
			public string Apellidos { get; set; }
			public string CorreoElectronico { get; set; }
			public string Telefono { get; set; }
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (customerView == null)
				customerView = CollectionViewSource.GetDefaultView(CustomerDataGrid.Items);

			customerView.Filter = FilterCustomers;
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (customerView == null)
					customerView = CollectionViewSource.GetDefaultView(CustomerDataGrid.Items);

				customerView.Filter = FilterCustomers;
			}
		}

		private bool FilterCustomers(object item)
		{
			Cliente cliente = item as Cliente;
			if (cliente == null)
				return false;
			string clienteCedulaMin = cliente.Cedula.ToLower();
			string clienteNombreMin = cliente.Nombre.ToLower();
			if (!string.IsNullOrEmpty(cedulaF.Text) && !clienteCedulaMin.Contains(cedulaF.Text.ToLower()))
				return false;

			if (!string.IsNullOrEmpty(nombreF.Text) && !clienteNombreMin.Contains(nombreF.Text.ToLower()))
				return false;

			return true;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (customerView != null)
			{
				customerView.Filter = null;

				cedulaF.Text = "";
				nombreF.Text = "";
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
