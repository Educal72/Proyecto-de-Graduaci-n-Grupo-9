using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FrontEndWPF.Empleados
{
	/// <summary>
	/// Lógica de interacción para Fichajes.xaml
	/// </summary>
	public partial class InicidentesEmpleado : UserControl
	{
		Conexion conexion = new Conexion();
		public ObservableCollection<Incidente> Fichajes {  get; set; }
		public InicidentesEmpleado()
		{
			InitializeComponent();
			Fichajes = new ObservableCollection<Incidente>();
			DataContext = this;
			PopulateFAQDataGrid();
		}
		public void PopulateFAQDataGrid()
		{
			List<Incidente> metricas = new List<Incidente>();
			string query = @"SELECT Id, Fecha, Descripcion, Tipo, Estado FROM Incidentes WHERE IdUsuario = @IdUsuario";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@IdUsuario", SesionUsuario.Instance.id);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						metricas.Add(new Incidente()
						{
							Id = (int)reader["Id"],
							Fecha = Convert.ToDateTime(reader["Fecha"]),
							Descripcion = reader["Descripcion"].ToString(),
							Tipo = reader["Tipo"].ToString(),
							Estado = reader["Estado"].ToString()
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			foreach (var item in metricas)
			{
				Fichajes.Add(item);
			}
		}
	}
}
