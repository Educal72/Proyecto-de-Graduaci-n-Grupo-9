using FrontEndWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
	/// Lógica de interacción para clienteAsociar.xaml
	/// </summary>
	public partial class clienteAsociar : Window
	{
		private DispatcherTimer timer;
		Conexion conexion = new Conexion();
		ClienteViewModel clienteViewModel = new ClienteViewModel();
		private ICollectionView customerView;
		public string NombreCompleto { get; set; }
		public int Cedula { get; set; }
		public bool isAsociado { get; set; }
		public decimal puntosDisponibles { get; set; }
		public clienteAsociar()
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
			LoadData();
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			fecha.Content = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
		}

		private void LoadData()
		{
			string query = @"
                SELECT Cedula, Nombre, Apellidos,
				Telefono, CorreoElectronico,
				Asociado, Puntos
                FROM Cliente";

			List<Cliente> usuariosEmpleados = new List<Cliente>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						usuariosEmpleados.Add(new Cliente
						{

							Nombre = reader["Nombre"].ToString(),
							Apellidos = reader["Apellidos"].ToString(),
							CorreoElectronico = reader["CorreoElectronico"].ToString(),
							Cedula = reader["Cedula"].ToString(),
							Telefono = reader["Telefono"].ToString(),
							Puntos = Convert.ToInt32(reader["Puntos"]),
							Asociado = (bool)reader["Asociado"]
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			CustomerDataGrid.ItemsSource = usuariosEmpleados;
		}


		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Cliente selectedCustomer = (Cliente)CustomerDataGrid.SelectedItem;

			if (selectedCustomer != null)
			{
				NombreCompleto = selectedCustomer.Nombre +" "+ selectedCustomer.Apellidos;
				Cedula = Convert.ToInt32(selectedCustomer.Cedula);
				isAsociado = selectedCustomer.Asociado;
				puntosDisponibles = selectedCustomer.Puntos;
				DialogResult = true;
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
		public class Cliente
		{
			public string Cedula { get; set; }
			public string Nombre { get; set; }
			public string Apellidos { get; set; }
			public string CorreoElectronico { get; set; }
			public string Telefono { get; set; }
			public bool Asociado { get; set; }
			public decimal Puntos { get; set; }
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

			return true;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (customerView != null)
			{
				customerView.Filter = null;

				cedulaF.Text = "";
				
			}
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			nuevoCliente nuevoCliente = new nuevoCliente();
			nuevoCliente.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoCliente.ShowDialog() == true)
			{
				int cedula = Convert.ToInt32(nuevoCliente.cedula);
				string nombre = nuevoCliente.nombre;
				string apellidos = nuevoCliente.apellidos;
				string correo = nuevoCliente.correo;
				int telefono = Convert.ToInt32(nuevoCliente.telefono);
				int asociado;
				if (nuevoCliente.asociar == true) {
					asociado = 1;
				} else {
					asociado = 0;
				}
				clienteViewModel.CrearCliente(cedula, nombre, apellidos, correo, telefono, asociado, 0);
				LoadData();
			}
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var selectedItem = CustomerDataGrid.SelectedItem as Cliente;
			if (selectedItem != null) { 
			verCliente verCliente = new verCliente(selectedItem.Nombre, selectedItem.Cedula, selectedItem.Apellidos, selectedItem.CorreoElectronico, selectedItem.Telefono, selectedItem.Asociado) ;
			verCliente.WindowStartupLocation = WindowStartupLocation.CenterScreen;
	
				if (verCliente.ShowDialog() == true)
			{
				clienteViewModel.EditarCliente(Convert.ToInt32(verCliente.cedula), verCliente.nombre, verCliente.apellidos, verCliente.correo, Convert.ToInt32(verCliente.telefono),verCliente.asociar);
				LoadData();
			}
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}

		}

		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			var selectedItem = CustomerDataGrid.SelectedItem as Cliente;
			if (selectedItem != null)
			{
				MessageBoxResult result = MessageBox.Show("¿Seguro que desea eliminar a este cliente?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
				{

					if (selectedItem.Cedula != "0" && selectedItem.Nombre != "Generico")
					{
						clienteViewModel.EliminarCliente(Convert.ToInt32(selectedItem.Cedula));
						LoadData();
					}
					else
					{
						MessageBox.Show("El cliente que desea eliminar es inválido", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
					}

				}
			}
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}
	}
