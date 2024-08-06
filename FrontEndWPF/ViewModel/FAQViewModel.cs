using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

namespace FrontEndWPF.ViewModel
{


	public class FAQViewModel
	{
		Conexion conexion = new Conexion();
		public class FAQ
		{
			public int Id { get; set; }
			public string Pregunta { get; set; }
			public string Respuesta { get; set; }
			public string Nombre { get; set; }
			public byte[]? Documento { get; set; }
		}

		public bool CrearFAQ(string pregunta, string respuesta, byte[] documento, string nombre)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "INSERT INTO FAQ (Pregunta, Respuesta, Documento, NombreDoc) VALUES (@Pregunta, @Respuesta, @Documento, @NombreDoc)";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Pregunta", pregunta));
					command.Parameters.Add(new SqlParameter("@Respuesta", respuesta));
					command.Parameters.Add(new SqlParameter("@Documento", documento));
					command.Parameters.Add(new SqlParameter("@NombreDoc", nombre));
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
		public (string, byte[]) GetDocumento(int id)
		{
			string nombre = "";
			byte[] documento = null;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "SELECT NombreDoc, Documento FROM FAQ WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Id", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							nombre = reader["NombreDoc"].ToString();
							documento = (byte[])reader["Documento"];
							return (nombre, documento);
						}
					}
				}
			}
			return (nombre, documento);
		}

		public bool EditarFAQ(int id, string pregunta, string respuesta, byte[] documento, string nombre)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = "UPDATE FAQ SET Pregunta = @Pregunta, Respuesta = @Respuesta, NombreDoc = @NombreDoc, Documento = @Documento WHERE Id = @Id";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Pregunta", pregunta));
					command.Parameters.Add(new SqlParameter("@Respuesta", respuesta));
					command.Parameters.Add(new SqlParameter("@NombreDoc", nombre));
					command.Parameters.Add(new SqlParameter("@Documento", documento));
					command.Parameters.Add(new SqlParameter("@Id", id));
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
		public void EliminarFAQ(int id)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{

				string query = @"DELETE FROM FAQ
                             WHERE Id = @Id";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@Id", id);

					command.ExecuteNonQuery();
				}
			}
		}
	}
}

	

