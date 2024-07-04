using System;
using System.Collections.Generic;
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
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para saloneroAsociar.xaml
	/// </summary>
	public partial class saloneroAsociar : Window
	{
		private DispatcherTimer timer;
		Conexion conexion = new Conexion();
		private ICollectionView customerView;
		public string NombreCompleto { get; set; }
		public int IdUsuario { get; set; }
		public saloneroAsociar()
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
                SELECT IdUsuario, u.Nombre, u.PrimerApellido, u.SegundoApellido, u.Cedula, u.Telefono, u.Correo, u.Contraseña, u.IdRol, u.FechaCreacion, e.Puesto, e.Salario, e.Direccion, e.Activo
                FROM Usuario u
                JOIN Empleado e ON u.Id = e.IdUsuario";

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
							Id = Convert.ToInt32(reader["IdUsuario"]),
							Nombre = reader["Nombre"].ToString(),
							Apellidos = reader["PrimerApellido"].ToString() + " " + reader["SegundoApellido"].ToString(),
							CorreoElectronico = reader["Correo"].ToString(),
							Cedula = reader["Cedula"].ToString(),
							Telefono = reader["Telefono"].ToString()
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
				NombreCompleto = selectedCustomer.Nombre + " " + selectedCustomer.Apellidos;
				IdUsuario = selectedCustomer.Id;
				DialogResult = true;
			}
		}
		public class Cliente
		{
			public int Id { get; set; }
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
