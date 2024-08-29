using FrontEndWPF.Empleados;
using FrontEndWPF.Index;
using FrontEndWPF.Modelos;
using FrontEndWPF.PuntoDeVenta;
using FrontEndWPF.ViewModel;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
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
using static FrontEndWPF.Modelos.UserModel;
using static System.Collections.Specialized.BitVector32;
using Image = iText.Layout.Element.Image;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;
using static FrontEndWPF.ViewModel.CierreViewModel;
using DocumentFormat.OpenXml.Spreadsheet;


namespace FrontEndWPF.Reporteria
{
    /// <summary>
    /// Interaction logic for InformeNormativa.xaml
    /// </summary>
    public partial class VisualizarCierres : UserControl
    {
        Conexion conexion = new Conexion();
        CierreViewModel cierreViewModel = new CierreViewModel();
		List<Cierre> cierresList = new List<Cierre>();
        public VisualizarCierres()
        {
            InitializeComponent();
			LoadCierres();
		}

		private void LoadCierres()
		{
			cierresList.Clear();
			cierresDataGrid.Items.Clear();
			Conexion conexion = new Conexion();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT * FROM CierreCaja WHERE Estado = 'Cerrado'";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									Cierre cierresCaja = new Cierre
									{
										Id = (int)reader["Id"],
										IdUsuario = (int)reader["IdUsuario"],
										NombreUsuario = cierreViewModel.GetUserById((int)reader["IdUsuario"]),
										FechaApertura = (DateTime)reader["FechaApertura"],
										CierreFecha = (DateTime)reader["Cierre"],
										PagadoEfectivo = (decimal)reader["PagadoEfectivo"],
										PagadoTarjeta = (decimal)reader["PagadoTarjeta"],
										TotalFinal = (decimal)reader["TotalFinal"],
										TotalIVA = (decimal)reader["TotalIVA"],
										TotalServicios = (decimal)reader["TotalServicios"],
										TotalRestaurante = (decimal)reader["TotalRestaurante"],
										TotalLlevar = (decimal)reader["TotalLlevar"],
										TotalPedidosYa = (decimal)reader["TotalPedidosYa"],
										TotalUberEats = (decimal)reader["TotalUberEats"],
										FondosApertura = (decimal)reader["FondosApertura"],
										Egresos = (decimal)reader["Egresos"],
										Ingresos = (decimal)reader["Ingresos"],
										Anulado = (decimal)reader["Anulado"],
										Estado = reader["Estado"].ToString(),
										FondosCierre = (decimal)reader["FondosCierre"],
									};
									cierresList.Add(cierresCaja);
									cierresDataGrid.Items.Add(cierresCaja);
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}
		}


		private void LoadCierresFiltro(DateTime from, DateTime where)
		{
			cierresList.Clear();
			cierresDataGrid.Items.Clear();
			Conexion conexion = new Conexion();
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT * FROM CierreCaja WHERE Cierre BETWEEN @From AND @Where";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@From", from);
						command.Parameters.AddWithValue("@Where", where);
						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									Cierre cierresCaja = new Cierre
									{
										Id = (int)reader["Id"],
										IdUsuario = (int)reader["IdUsuario"],
										NombreUsuario = cierreViewModel.GetUserById((int)reader["IdUsuario"]),
										FechaApertura = (DateTime)reader["FechaApertura"],
										CierreFecha = (DateTime)reader["Cierre"],
										PagadoEfectivo = (decimal)reader["PagadoEfectivo"],
										PagadoTarjeta = (decimal)reader["PagadoTarjeta"],
										TotalFinal = (decimal)reader["TotalFinal"],
										TotalIVA = (decimal)reader["TotalIVA"],
										TotalServicios = (decimal)reader["TotalServicios"],
										TotalRestaurante = (decimal)reader["TotalRestaurante"],
										TotalLlevar = (decimal)reader["TotalLlevar"],
										TotalPedidosYa = (decimal)reader["TotalPedidosYa"],
										TotalUberEats = (decimal)reader["TotalUberEats"],
										FondosApertura = (decimal)reader["FondosApertura"],
										Egresos = (decimal)reader["Egresos"],
										Ingresos = (decimal)reader["Ingresos"],
										Anulado = (decimal)reader["Anulado"],
										Estado = reader["Estado"].ToString(),
										FondosCierre = (decimal)reader["FondosCierre"],
									};
									cierresList.Add(cierresCaja);
									cierresDataGrid.Items.Add(cierresCaja);
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Error executing query: " + ex.Message);
						}
					}

					conexion.CloseConnection(connection);
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			LoadCierres();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (from.SelectedDate == null)
			{
				MessageBox.Show("Por favor, seleccione una fecha inicial.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (!DateTime.TryParse(from.SelectedDate.ToString(), out DateTime result))
			{
				MessageBox.Show("Por favor, introduzca una fecha inicial válida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (where.SelectedDate == null)
			{
				MessageBox.Show("Por favor, seleccione una fecha final.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (!DateTime.TryParse(where.SelectedDate.ToString(), out DateTime result2))
			{
				MessageBox.Show("Por favor, introduzca una fecha final válida.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				// Both dates are valid, pass them to LoadCierresFiltro method
				LoadCierresFiltro(result, result2);
			}
		}

		private void Generar_Informe_Click(object sender, RoutedEventArgs e)
		{
			Cierre selectedCierre = cierresDataGrid.SelectedItem as Cierre;
			MessageBoxResult result = MessageBox.Show("¿Está seguro que desea imprimir el cierre seleccionado?", "Confirmar Impresión", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (selectedCierre != null && result == MessageBoxResult.Yes) {
				CierresCaja cierresCaja = new CierresCaja(selectedCierre);
			}
		}
	}
}
