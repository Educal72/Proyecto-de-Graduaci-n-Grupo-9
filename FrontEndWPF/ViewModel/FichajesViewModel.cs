using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEndWPF.ViewModel
{
	public class FichajesViewModel
	{
		Conexion conexion = new Conexion();
		public bool CrearFichaje(int cedula)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				var Ids = GetUserIdByCedula(cedula);
				int existeFichaje = ExisteFichajeDiaActual(Ids.Item2);
				if (existeFichaje == 0)
				{
					string query = "INSERT INTO Fichajes (IdEmpleado, FechaHora, Tipo) VALUES (@IdEmpleado, @FechaHora, @Tipo)";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.Add(new SqlParameter("@IdEmpleado", Ids.Item2));
						command.Parameters.Add(new SqlParameter("@FechaHora", DateTime.Now));
						command.Parameters.Add(new SqlParameter("@Tipo", "Entrada"));
						int rowsAffected = (int)command.ExecuteNonQuery();
						if (rowsAffected > 0)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				else {
					CerrarFichaje(existeFichaje);
					return true;
				}
				
			}
		}

		public bool CerrarFichaje(int IdFichaje)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE Fichajes SET Tipo = @Tipo, FechaSalida = @Fecha WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Id", IdFichaje));
					command.Parameters.Add(new SqlParameter("@Tipo", "Salida"));
					command.Parameters.Add(new SqlParameter("@Fecha", DateTime.Now));
					int rowsAffected = (int)command.ExecuteNonQuery();
					if (rowsAffected > 0)
					{
						return true;
					}
					else
					{
						return false;
					}

				}
			}
		}
		public int ExisteFichajeDiaActual(int IdEmpleado)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"
            SELECT 
                Id
            FROM 
                Fichajes
            WHERE 
                IdEmpleado = @IdEmpleado 
                AND CONVERT(date, FechaHora) = CONVERT(date, GETDATE())";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", IdEmpleado));
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							int entradaId = reader.GetInt32(0);
							return entradaId;
						}
						else
						{
							int entradaId = 0;
							return entradaId;
						}
					}

				}
			}
		}
		public (int, int) GetUserIdByCedula(int cedula)
		{
			int IdUsuario = 0;
			int IdEmpleado = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"
            SELECT 
                Usuario.Id AS UsuarioId, 
                Empleado.Id AS EmpleadoId
            FROM 
                Usuario
            JOIN 
                Empleado ON Usuario.Id = Empleado.IdUsuario
            WHERE 
                Usuario.Cedula = @Cedula";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							IdUsuario = (int)reader["UsuarioId"];
							IdEmpleado = (int)reader["EmpleadoId"];
							return (IdUsuario, IdEmpleado);
						}
					}
				}
			}
			return (IdUsuario, IdEmpleado);
		}
	}
}
