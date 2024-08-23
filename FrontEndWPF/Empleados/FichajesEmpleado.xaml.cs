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
	public partial class FichajesEmpleado : UserControl
	{
		Conexion conexion = new Conexion();
		public ObservableCollection<Fichajes> Fichajes {  get; set; }
		public FichajesEmpleado()
		{
			InitializeComponent();
			Fichajes = new ObservableCollection<Fichajes>();
			DataContext = this;
			PopulateFAQDataGrid();
		}
		public int getIdEmpleadoFromIdUsuario()
		{

			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "SELECT Id FROM Empleado WHERE IdUsuario = @IdUsuario";
					int IdUsuario;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@IdUsuario", SesionUsuario.Instance.id);
						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								IdUsuario = Convert.ToInt32(result);
								return IdUsuario;
							}
							else
							{
								Console.WriteLine("Empelado no encontrado.");
								return 0;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error: " + ex.Message);
							return 0;
						}
					}
				}
				else
				{
					return 0;

				}
			}
		}
		public void PopulateFAQDataGrid()
		{
			List<Fichajes> fichajes = new List<Fichajes>();
			string query = @"SELECT 
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
    Usuario ON Empleado.IdUsuario = Usuario.Id
WHERE
    Fichajes.IdEmpleado = @IdEmpleado
ORDER BY 
    ABS(DATEDIFF(DAY, Fichajes.FechaHora, GETDATE())) ASC;";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					int IdUsuario = getIdEmpleadoFromIdUsuario();
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@IdEmpleado", IdUsuario);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						fichajes.Add(new Fichajes()
						{
							Cedula = reader["Cedula"].ToString(),
							Nombre = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString(),
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

			foreach (var item in fichajes)
			{
				Fichajes.Add(item);
			}
		}
	}
}
