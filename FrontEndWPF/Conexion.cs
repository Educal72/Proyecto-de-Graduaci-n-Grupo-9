using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace FrontEndWPF
{
	public class Conexion
	{
		private string connectionString;

		// Constructor to initialize the connection string
		public Conexion(string server = "localhost", string database = "ProyectoBD_Molino", string userId = "Admin", string password = "admin123")
		{
			connectionString = $"Server={server};Database={database};User Id={userId};Password={password};";
		}

		public SqlConnection OpenConnection()
		{
			SqlConnection connection = new SqlConnection(connectionString);
			try
			{
				connection.Open();
				Console.WriteLine("Connection opened successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error opening connection: " + ex.Message);
			}
			return connection;
		}

		public void CloseConnection(SqlConnection connection)
		{
			if (connection != null && connection.State == System.Data.ConnectionState.Open)
			{
				connection.Close();
				Console.WriteLine("Connection closed successfully.");
			}
		}

		public void InsertData(string table, Dictionary<string, object> data)
		{
			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					string columns = string.Join(", ", data.Keys);
					string parameters = string.Join(", ", data.Keys.Select(key => "@" + key));

					string query = $"INSERT INTO {table} ({columns}) VALUES ({parameters})";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						foreach (var pair in data)
						{
							command.Parameters.AddWithValue("@" + pair.Key, pair.Value);
						}

						try
						{
							int rowsAffected = command.ExecuteNonQuery();
							Console.WriteLine($"Rows affected: {rowsAffected}");
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					CloseConnection(connection);
				}
			}
		}

		public Dictionary<string, object> SelectUser(string correo, string contraseña)
		{
			var result = new Dictionary<string, object>();

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT * FROM Usuario WHERE Correo = @Correo AND Contraseña = @Contraseña";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Correo", correo);
						command.Parameters.AddWithValue("@Contraseña", contraseña);

						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									for (int i = 0; i < reader.FieldCount; i++)
									{
										result[reader.GetName(i)] = reader.GetValue(i);
									}
								}
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					CloseConnection(connection);
				}
			}

			return result;
		}

		public bool HasEntries()
		{
			bool hasEntries = false;

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT COUNT(*) FROM Usuario";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						try
						{
							int count = (int)command.ExecuteScalar();
							hasEntries = count > 0;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing query: " + ex.Message);
						}
					}

					CloseConnection(connection);
				}
			}

			return hasEntries;
		}

		public bool AddUser(string nombre, string primerApellido, string segundoApellido, string cedula, string telefono, string correo, string contraseña, string rol, DateTime fechaCreacion, string puesto, double salario)
		{
			bool success = false;

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					// Find the role ID
					string roleQuery = "SELECT Id FROM roles WHERE Nombre = @Rol";
					int roleId = -1;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@Rol", rol);
						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								roleId = Convert.ToInt32(result);
							}
							else
							{
								Console.WriteLine("Role not found.");
								return false;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing role query: " + ex.Message);
							return false;
						}
					}

					// Insert the new user
					string query = "INSERT INTO Usuario (Nombre, PrimerApellido, SegundoApellido, Cedula, Telefono, Correo, Contraseña, IdRol, FechaCreacion) " +
								   "VALUES (@Nombre, @PrimerApellido, @SegundoApellido, @Cedula, @Telefono, @Correo, @Contraseña, @IdRol, @FechaCreacion)";

					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Nombre", nombre);
						command.Parameters.AddWithValue("@PrimerApellido", primerApellido);
						command.Parameters.AddWithValue("@SegundoApellido", segundoApellido);
						command.Parameters.AddWithValue("@Cedula", cedula);
						command.Parameters.AddWithValue("@Telefono", telefono);
						command.Parameters.AddWithValue("@Correo", correo);
						command.Parameters.AddWithValue("@Contraseña", contraseña);
						command.Parameters.AddWithValue("@IdRol", roleId);
						command.Parameters.AddWithValue("@FechaCreacion", fechaCreacion);

						try
						{
							int rowsAffected = command.ExecuteNonQuery();
							success = rowsAffected > 0;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing insert query: " + ex.Message);
						}
					}

					// Find the role ID
					string userQuery = "SELECT Id FROM Usuario WHERE Cedula = @Cedula";
					int userId = -1;

					using (SqlCommand userCommand = new SqlCommand(userQuery, connection))
					{
						userCommand.Parameters.AddWithValue("@Cedula", cedula);
						try
						{
							object result = userCommand.ExecuteScalar();
							if (result != null)
							{
								userId = Convert.ToInt32(result);
							}
							else
							{
								Console.WriteLine("User not found.");
								return false;
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing role query: " + ex.Message);
							return false;
						}
					}

					string query2 = "INSERT INTO Empleado (Puesto, FechaContratacion, Salario, FechaDespido, IdUsuario) " +
								   "VALUES (@Puesto, @FechaContratacion, @Salario, @FechaDespido, @IdUsuario)";

					using (SqlCommand command = new SqlCommand(query2, connection))
					{
						command.Parameters.AddWithValue("@Puesto", puesto);
						command.Parameters.AddWithValue("@FechaContratacion", fechaCreacion);
						command.Parameters.AddWithValue("@Salario", salario);
						command.Parameters.AddWithValue("@FechaDespido", fechaCreacion);
						command.Parameters.AddWithValue("@IdUsuario", userId);

						try
						{
							int rowsAffected = command.ExecuteNonQuery();
							success = rowsAffected > 0;
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error executing insert query: " + ex.Message);
						}
					}

					CloseConnection(connection);
				}
			}

			return success;
		}
	}

           
	}

