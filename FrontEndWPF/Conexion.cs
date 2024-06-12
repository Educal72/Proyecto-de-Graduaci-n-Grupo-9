using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Security.Cryptography;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Shapes;
using System.Reflection.Metadata.Ecma335;

namespace FrontEndWPF
{
	public class Conexion
	{
		private string connectionString;
		private string server;
		private string puerto;
		private string nombre;
		private string usuario;
		private string contraseña;

		public Conexion()
		{
			if (!File.Exists(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt"))
			{
				return;
			}
			try
			{
				// Leer todas las líneas del archivo
				string[] lines = File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/db_config.txt");

				// Parsear el contenido del archivo
				foreach (string line in lines)
				{
					string[] parts = line.Split(new[] { ": " }, StringSplitOptions.None);
					if (parts.Length == 2)
					{
						switch (parts[0])
						{
							case "Server":
								server = parts[1];
								break;
							case "Puerto":
								puerto = parts[1];
								break;
							case "Nombre":
								nombre = parts[1];
								break;
							case "Usuario":
								usuario = parts[1];
								break;
							case "Contraseña":
								contraseña = parts[1];
								break;
						}
					}
				}

				// Opcional: Mostrar los valores leídos (para depuración)
				connectionString = $"Server={server};Database={nombre};User Id={usuario};Password={contraseña};";
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al leer la configuración del archivo: {ex.Message}");
			}
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
		public Dictionary<string, object> GetUserByEmail(string correo)
		{
			var result = new Dictionary<string, object>();

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT * FROM Usuario WHERE Correo = @Correo";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Correo", correo);

						try
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									for (int i = 0; i < reader.FieldCount; i++)
									{
										string fieldName = reader.GetName(i);
										if (!reader.IsDBNull(i))
										{
											result[fieldName] = reader.GetValue(i);
										}
										else
										{
											result[fieldName] = null; // Otra opción sería asignar un valor predeterminado, como string.Empty
										}
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

        public List<Dictionary<string, object>> GetProductos()
        {
            var productos = new List<Dictionary<string, object>>();

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Codigo, Nombre, Categoria, Precio, Activo FROM Productos";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var producto = new Dictionary<string, object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string fieldName = reader.GetName(i);
                                        if (!reader.IsDBNull(i))
                                        {
                                            producto[fieldName] = reader.GetValue(i);
                                        }
                                        else
                                        {
                                            producto[fieldName] = null; // Otra opción sería asignar un valor predeterminado, como string.Empty
                                        }
                                    }
                                    productos.Add(producto);
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

            return productos;
        }

        public Dictionary<string, object> SelectUserCedula(string correo, int cedula)
		{
			var result = new Dictionary<string, object>();

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT * FROM Usuario WHERE Correo = @Correo AND Cedula = @Cedula";
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Correo", correo);
						command.Parameters.AddWithValue("@Cedula", cedula);

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
						command.Parameters.AddWithValue("@Contraseña", HashPassword(contraseña));
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

        public bool InsertarProducto(int codigo ,string nombre, string categoria, decimal precio, bool activo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "INSERT INTO Productos (Codigo, Nombre, Categoria, Precio, Activo) VALUES (@Codigo, @Nombre, @Categoria, @Precio, @Activo)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Codigo", codigo);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Categoria", categoria);
                        command.Parameters.AddWithValue("@Precio", precio);
                        command.Parameters.AddWithValue("@Activo", activo);

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

        public bool ActualizarUsuario(string correo, string nombre, string primerApellido, string segundoApellido, string cedula, string telefono, string contraseña, string rol)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    // Obtener el ID del rol
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
                                Console.WriteLine("Rol no encontrado.");
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al ejecutar la consulta de rol: " + ex.Message);
                            return false;
                        }
                    }

                    // Actualizar el usuario
                    string query = "UPDATE Usuario SET Nombre = @Nombre, PrimerApellido = @PrimerApellido, SegundoApellido = @SegundoApellido, " +
                                   "Cedula = @Cedula, Telefono = @Telefono, Contraseña = @Contraseña, IdRol = @IdRol WHERE Correo = @Correo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@PrimerApellido", primerApellido);
                        command.Parameters.AddWithValue("@SegundoApellido", segundoApellido);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Contraseña", HashPassword(contraseña));
                        command.Parameters.AddWithValue("@IdRol", roleId);
                        command.Parameters.AddWithValue("@Correo", correo);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error al ejecutar la consulta de actualización: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }


        public string HashPassword(string password)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

				// Convert byte array to a string
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
		public string getRoleName(int Id)
		{

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "SELECT Nombre FROM roles WHERE Id = @Rol";
					string roleId;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@Rol", Id);
						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								roleId = result.ToString();
								return roleId;
							}
							else
							{
								Console.WriteLine("Role not found.");
								return "";
							}
						}
						catch (Exception ex)
						{
							Console.WriteLine("Error: " + ex.Message);
							return "";
						}
					}
				}
				else {
					return "";
				
				}
			}
		}
	}
}
	

