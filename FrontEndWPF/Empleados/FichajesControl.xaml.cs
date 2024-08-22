using FrontEndWPF.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.empleadosAdmin;

namespace FrontEndWPF
{
	/// <summary>
	/// Lógica de interacción para FichajesControl.xaml
	/// </summary>
	public partial class FichajesControl : UserControl
	{

		Conexion conexion = new Conexion();
		private StringBuilder barcode = new StringBuilder();
		FichajesViewModel fichajesViewModel = new FichajesViewModel();
		private ICollectionView customerView;
		public FichajesControl()
		{
			InitializeComponent();
			PopulateFiltroComboBox();
			PopulateFichajesDataGrid();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cedula.Focus();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				string scannedCode = barcode.ToString().Trim();
				barcode.Clear();
				ProcessScannedCode(scannedCode);
				e.Handled = true;
			}
			else
			{
				barcode.Append(e.Key.ToString().Replace("D", ""));
			}
		}

		private void ProcessScannedCode(string scannedCode)
		{
			try
			{
				fichajesViewModel.CrearFichaje(Convert.ToInt32(scannedCode));
				PopulateFichajesDataGrid();
				cedula.Clear();
			}
			catch (Exception ex) {
				MessageBox.Show("Cédula en formato inválido o de empleado inexistente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void PopulateFichajesDataGrid()
		{
			List<Fichajes> fichajes = new List<Fichajes>();
			string query = @"
            SELECT 
                Fichajes.FechaHora,
				Fichajes.FechaSalida,
                Fichajes.Tipo,
                Usuario.Nombre,
                Usuario.Apellido,
				Usuario.Cedula
            FROM 
                Fichajes
            JOIN 
                Empleado ON Fichajes.IdEmpleado = Empleado.Id
            JOIN 
                Usuario ON Empleado.IdUsuario = Usuario.Id";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						fichajes.Add(new Fichajes()
						{
							Cedula = reader["Cedula"].ToString(),
							Nombre = reader["Nombre"].ToString() + " " +reader["Apellido"].ToString(),
							Fecha = (DateTime)reader["FechaHora"],
							FechaSalida = reader["FechaSalida"] != DBNull.Value ? (DateTime?)reader["FechaSalida"] : null,
							Tipo = reader["Tipo"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
			FichajesDataGrid.ItemsSource = fichajes;
		}

		private void PopulateFiltroComboBox()
		{
			string query = @"
            SELECT 
                Cedula, 
                Nombre, 
                Apellido
            FROM 
                Usuario"
            ;

			combo.Items.Add(new ComboBoxItem
			{
				Content = "Todos",
				Tag = 1 // Usar el Tag para almacenar la cédula asociada
			});

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						string cedula = reader["Cedula"].ToString();
						string nombre = reader["Nombre"].ToString();
						string primerApellido = reader["Apellido"].ToString();

						// Formatear la opción del ComboBox
						string displayText = $"{nombre} {primerApellido}";

						// Crear un ComboBoxItem con el texto y agregarlo al ComboBox
						combo.Items.Add(new ComboBoxItem
						{
							Content = displayText,
							Tag = cedula // Usar el Tag para almacenar la cédula asociada
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		private bool FilterCustomers(object item)
		{
			Fichajes fichajes = item as Fichajes;
			if (fichajes == null)
			return false;

			string clienteCedulaMin = fichajes.Cedula.ToLower();
			if (combo.SelectedItem is ComboBoxItem selectedItem)
			{
				if (!string.IsNullOrEmpty(selectedItem.Tag.ToString()) && clienteCedulaMin != selectedItem.Tag.ToString())
				return false;
			}

			return true;
		}


		private void cedula_TextChanged(object sender, TextChangedEventArgs e)
		{
			cedula.Focus();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (combo.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() == "Todos")
			{
				if (customerView != null)
				{
					customerView.Filter = null; // Eliminar el filtro para mostrar todas las entradas
					cedula.Focus();
				}
			}
			else
			{
				if (customerView == null)
					customerView = CollectionViewSource.GetDefaultView(FichajesDataGrid.Items);

				customerView.Filter = FilterCustomers; // Aplicar el filtro personalizado
				cedula.Focus();
			}

			cedula.Focus(); // Mover el foco al TextBox
		}
	}

}
