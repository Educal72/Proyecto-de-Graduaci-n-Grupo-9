using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF.ViewModel
{
    class ClienteViewModel
    {
		Conexion conexion = new Conexion();

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

		public void CrearCliente(int cedula, string nombre, string apellidos, string correo, int telefono, int asociado, decimal puntos)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO Cliente (Cedula, Nombre, Apellidos, CorreoElectronico, Telefono, Asociado, Puntos) VALUES (@Cedula, @Nombre, @Apellidos, @CorreoElectronico, @Telefono, @Asociado, @Puntos)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula ));
					command.Parameters.Add(new SqlParameter("@Nombre", nombre ));
					command.Parameters.Add(new SqlParameter("@Apellidos", apellidos));
					command.Parameters.Add(new SqlParameter("@CorreoElectronico", correo));
					command.Parameters.Add(new SqlParameter("@Telefono", telefono));
					command.Parameters.Add(new SqlParameter("@Asociado", asociado));
					command.Parameters.Add(new SqlParameter("@Puntos", puntos));
					command.ExecuteScalar();
				}
			}
		}

		public void EditarCliente(int cedula, string nombre, string apellidos, string correo, int telefono, bool asociado)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Cliente SET Nombre = @Nombre, Apellidos = @Apellidos, CorreoElectronico = @CorreoElectronico, Telefono = @Telefono, Asociado = @Asociado WHERE Cedula = @Cedula";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula));
					command.Parameters.Add(new SqlParameter("@Nombre", nombre));
					command.Parameters.Add(new SqlParameter("@Apellidos", apellidos));
					command.Parameters.Add(new SqlParameter("@CorreoElectronico", correo));
					command.Parameters.Add(new SqlParameter("@Telefono", telefono));
					command.Parameters.Add(new SqlParameter("@Asociado", asociado));
					command.ExecuteNonQuery();
				}
			}
		}

		public void EliminarCliente(int id)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "DELETE Cliente WHERE Cedula = @Cedula";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Cedula", id);
					command.ExecuteNonQuery();
				}
			}
		}

		public void PuntosCliente(int cedula, decimal puntos, decimal puntosUsados)
		{
			decimal result;
			decimal nuevosPuntos;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query2 = "SELECT Puntos FROM Cliente WHERE Cedula = @Cedula";

				using (SqlCommand command2 = new SqlCommand(query2, connection))
				{
					command2.Parameters.Add(new SqlParameter("@Cedula", cedula));
					result = (decimal)command2.ExecuteScalar();
				}
			}
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Cliente SET Puntos = @Puntos WHERE Cedula = @Cedula";
				if (puntosUsados != 0) {
					nuevosPuntos = 0.00m;
				} else
				{
					nuevosPuntos = result + puntos;
				}
				 
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula));
					command.Parameters.Add(new SqlParameter("@Puntos", nuevosPuntos));
					command.ExecuteNonQuery();
				}
			}
		}
	}
}
