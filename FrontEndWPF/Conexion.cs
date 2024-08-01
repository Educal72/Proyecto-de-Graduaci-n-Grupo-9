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
using FrontEndWPF.Index;
using FrontEndWPF.Modelos;
using static FrontEndWPF.Modelos.UserModel;

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
        static int Si_Prosigue = 0;
        static int No_Prosigue = 0;

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
                    string query = "SELECT Id, Codigo, Nombre, Categoria, Precio, Activo FROM Productos";
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
                                            producto[fieldName] = null;
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

        public List<Dictionary<string, object>> GetPermisosDeTiempo()
        {
            var permisos = new List<Dictionary<string, object>>();

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT IdEmpleado, FechaInicio, FechaFin, Motivo, Estado FROM PermisosDeTiempo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var permiso = new Dictionary<string, object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string fieldName = reader.GetName(i);
                                        if (!reader.IsDBNull(i))
                                        {
                                            permiso[fieldName] = reader.GetValue(i);
                                        }
                                        else
                                        {
                                            permiso[fieldName] = null;
                                        }
                                    }
                                    permisos.Add(permiso);
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

            return permisos;
        }

        public List<Dictionary<string, object>> GetPermisosDeAusencia()
        {
            var permisos = new List<Dictionary<string, object>>();

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT IdEmpleado, FechaInicio, FechaFin, Motivo, Estado FROM PermisosDeAusencia";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var permiso = new Dictionary<string, object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        string fieldName = reader.GetName(i);
                                        if (!reader.IsDBNull(i))
                                        {
                                            permiso[fieldName] = reader.GetValue(i);
                                        }
                                        else
                                        {
                                            permiso[fieldName] = null;
                                        }
                                    }
                                    permisos.Add(permiso);
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

            return permisos;
        }
        public List<Dictionary<string, object>> GetProductosActivos()
        {
            var productos = new List<Dictionary<string, object>>();

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Id, Codigo, Nombre, Categoria, Precio, Activo FROM Productos Where Activo = 1";
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
                                            producto[fieldName] = null;
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

        public bool EliminarProducto(int id)
        {
            bool eliminado = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "DELETE FROM Productos WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                eliminado = true;
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

            return eliminado;
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

        /* Método que sirve para poder añadir nuevos empleados a la BD. */
        public bool AddUser(string nombre, string apellido, string cedula, string telefono, string correo, string contraseña, string rol,
            DateTime fechaCreacion, string puesto, double salario, string direccion)
        {
            var conexion_Empleado = new ConexionEmpleado();
            bool success = false;
            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    //Obtiene el ID a través del rol que se envia por parámetro.
                    string roleQuery = "Exec ObtenerID_X_Rol @Rol";
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
                                Si_Prosigue = 1;
                            }
                            else
                            {
                                No_Prosigue = 1;
                                MessageBox.Show("El rol no fue encontrado.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                                Si_Prosigue = 0;
                                No_Prosigue = 0;
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                            "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                            "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }


                    /*==============================================================================================================================================*/


                    //Inserta un nuevo usuario en la base de datos, esto a través de los datos enviados por los párametros:
                    string query = "Exec CrearUsuarios @Nombre, @Apellido, @Cedula, @Telefono, @Correo, @Contraseña, @IdRol, @FechaCreacion";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
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
                            Si_Prosigue = 2;
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                            "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                            "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }


                    /*==============================================================================================================================================*/


                    //Obtiene el ID a través de la cédula que se envia por parámetro.
                    string userQuery = "Exec ObtenerID_X_Cedula @Cedula";
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
                                Si_Prosigue = 3;
                            }
                            else
                            {
                                No_Prosigue = 1;
                                MessageBox.Show("La cédula no fue encontrada.", "¡Error!",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                                conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                                Si_Prosigue = 0;
                                No_Prosigue = 0;
                                return false;
                            }
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                            "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                            "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }


                    /*==============================================================================================================================================*/


                    /* Nota Importante:
                     * Si notaron que en los métodos loadData y para añadir empleado se especificaba el campo activo, -
                     * pero no se usaba como parámetro [VER 0.1 y 0.2] es porque cuando se crea un nuevo empleado, aquí en -
                     * visual, se le da directamente el dato 1 (que significa que si esta activo) para evitar un error en la lectura -
                     * de los datos (tanto en el programa, como en la BD), esto también porque a nivel de lógica del negocio, si yo como -
                     * 3ra persona (digamos un administrador) estoy agregando un nuevo empleado que acaba de ser contratado, dicho empleado -
                     * entonces debe estar activo, ya que esta trabajando en el negocio, por eso no se coloca el activo como parámetro. */
                    string query2 = "Exec CrearEmpleados @Puesto, @FechaContratacion, @Salario, @FechaDespido, @IdUsuario, @Activo, @Direccion";
                    using (SqlCommand command = new SqlCommand(query2, connection))
                    {
                        command.Parameters.AddWithValue("@Puesto", puesto);
                        command.Parameters.AddWithValue("@FechaContratacion", fechaCreacion);
                        command.Parameters.AddWithValue("@Salario", salario);
                        command.Parameters.AddWithValue("@FechaDespido", fechaCreacion);
                        command.Parameters.AddWithValue("@IdUsuario", userId);
                        command.Parameters.AddWithValue("@Activo", 1);
                        command.Parameters.AddWithValue("@Direccion", direccion);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                            Si_Prosigue = 4;
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                            "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                            "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }

                    CloseConnection(connection);
                }
            }

            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }

        public bool InsertarProducto(int codigo, string nombre, string categoria, decimal precio, bool activo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    try
                    {
                        string query = "INSERT INTO Productos (Codigo, Nombre, Categoria, Precio, Activo) VALUES (@Codigo, @Nombre, @Categoria, @Precio, @Activo)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Codigo", codigo);
                            command.Parameters.AddWithValue("@Nombre", nombre);
                            command.Parameters.AddWithValue("@Categoria", categoria);
                            command.Parameters.AddWithValue("@Precio", precio);
                            command.Parameters.AddWithValue("@Activo", activo);

                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing insert query: " + ex.Message);
                        // Lanza la excepción para que quien llama pueda manejarla adecuadamente.
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection);
                    }
                }
            }

            return success;
        }

        public bool CrearPermisoTiempo(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    try
                    {
                        string query = "INSERT INTO PermisosDeTiempo (IdEmpleado, FechaInicio, FechaFin, Motivo, Estado) VALUES (@IdEmpleado, @FechaInicio, @FechaFin, @Motivo, @Estado)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                            command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                            command.Parameters.AddWithValue("@FechaFin", fechaFin);
                            command.Parameters.AddWithValue("@Motivo", motivo);
                            command.Parameters.AddWithValue("@Estado", "Pendiente"); // Estado por defecto

                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing insert query: " + ex.Message);
                        // Lanza la excepción para que quien llama pueda manejarla adecuadamente.
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection);
                    }
                }
            }

            return success;
        }

        public bool CrearPermisoAusencia(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    try
                    {
                        string query = "INSERT INTO PermisosDeAusencia (IdEmpleado, FechaInicio, FechaFin, Motivo, Estado) VALUES (@IdEmpleado, @FechaInicio, @FechaFin, @Motivo, @Estado)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                            command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                            command.Parameters.AddWithValue("@FechaFin", fechaFin);
                            command.Parameters.AddWithValue("@Motivo", motivo);
                            command.Parameters.AddWithValue("@Estado", "Pendiente"); // Estado por defecto

                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing insert query: " + ex.Message);
                        // Lanza la excepción para que quien llama pueda manejarla adecuadamente.
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection);
                    }
                }
            }

            return success;
        }
        public bool ActualizarProducto(int id, int codigo, string nombre, string categoria, decimal precio, bool activo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "UPDATE Productos SET Codigo = @Codigo, Nombre = @Nombre, Categoria = @Categoria, Precio = @Precio, Activo = @Activo WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
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
                            Console.WriteLine("Error executing update query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }


        public bool ActualizarUsuario(string correo, string nombre, string primerApellido, string segundoApellido, string cedula, string telefono, string rol)
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
                    string actualizarquery = "UPDATE Usuario SET Nombre = @Nombre, PrimerApellido = @PrimerApellido, SegundoApellido = @SegundoApellido, " +
                                   "Cedula = @Cedula, Telefono = @Telefono, IdRol = @IdRol WHERE Correo = @Correo";

                    using (SqlCommand command = new SqlCommand(actualizarquery, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@PrimerApellido", primerApellido);
                        command.Parameters.AddWithValue("@SegundoApellido", segundoApellido);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Telefono", telefono);
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
        public bool UpdateEstadoPermisosTiempo(int idEmpleado, string nuevoEstado)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "UPDATE PermisosDeTiempo SET Estado = @Estado WHERE IdEmpleado = @IdEmpleado";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@Estado", nuevoEstado);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing update query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }

        public bool UpdateEstadoPermisosAusencia(int idEmpleado, string nuevoEstado)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "UPDATE PermisosDeAusencia SET Estado = @Estado WHERE IdEmpleado = @IdEmpleado";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@Estado", nuevoEstado);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing update query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }

        public bool EliminarPermisosTiempo(int idEmpleado)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "DELETE FROM PermisosDeTiempo WHERE IdEmpleado = @IdEmpleado";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing delete query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }

        public bool EliminarPermisosAusencia(int idEmpleado)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "DELETE FROM PermisosDeAusencia WHERE IdEmpleado = @IdEmpleado";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing delete query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }
        public List<UsuarioEmpleado> DropdownUsuarios()
        {
            List<UsuarioEmpleado> usuarios = new List<UsuarioEmpleado>();

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Id, Nombre, Apellido FROM Usuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var IdEmpleado = getIdEmpleadoFromIdUsuario(Convert.ToInt32(reader["Id"]));
                                    UsuarioEmpleado usuario = new UsuarioEmpleado
                                    {
                                        Id = IdEmpleado, // Asegúrate de convertir el ID a entero
                                        Nombre = reader["Nombre"].ToString()!,
                                        Apellido = reader["Apellido"].ToString()!
                                    };

                                    usuarios.Add(usuario);
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

            return usuarios;
        }

        public bool ActualizarPermisoTiempo(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    // Query para actualizar los permisos de tiempo
                    string query = "UPDATE PermisosDeTiempo SET FechaInicio = @FechaInicio, FechaFin = @FechaFin, Motivo = @Motivo WHERE IdEmpleado = @IdEmpleado";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Añade los parámetros al comando SQL
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", fechaFin);
                        command.Parameters.AddWithValue("@Motivo", motivo);

                        try
                        {
                            // Ejecuta la consulta y obtiene el número de filas afectadas
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0; // Si se afectó al menos una fila, el éxito es true
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing update query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }

        public bool ActualizarPermisoAusencia(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    // Query para actualizar los permisos de tiempo
                    string query = "UPDATE PermisosDeAusencia SET FechaInicio = @FechaInicio, FechaFin = @FechaFin, Motivo = @Motivo WHERE IdEmpleado = @IdEmpleado";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Añade los parámetros al comando SQL
                        command.Parameters.AddWithValue("@IdEmpleado", idEmpleado);
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", fechaFin);
                        command.Parameters.AddWithValue("@Motivo", motivo);

                        try
                        {
                            // Ejecuta la consulta y obtiene el número de filas afectadas
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0; // Si se afectó al menos una fila, el éxito es true
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error executing update query: " + ex.Message);
                        }
                    }

                    CloseConnection(connection);
                }
            }

            return success;
        }

        /* 
		 * Método que sirve para enviar los datos que se quiere actualizar de un empleado -
		 * hacia la base de datos que esta en SQL Server.
		 */
        public bool UpdateEmployee(string cedula, string nombre,
            string primerApellido, string segundoApellido,
            string puesto, DateTime fechaContratacion, decimal salario,
            string correo, string contraseña, string telefono, bool activo,
            string direccion, int idrol, string oldCedula, string oldCorreo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    /*
					 * Aquí lo que hace es incrementar el idusuario en 1 en 1, -
					 * esto porque en el comando donde se manda la instrucción de ejecución -
					 * a la BD (el cual se llama: query), tiene un procedimiento almacenado que -
					 * necesita dicho campo, además que este es necesario para realizar una actualización -
					 * que se encuentra dentro del procedimiento almacenado que se esta utilizando.
					 * 
					 * [2.1]
					 * También la variable idusuario es global para que se pueda tomar en cuenta el incremento -
					 * que se esta indicando en el comando (idusuario++), además porque dicho dato en la BD es un -
					 * IDENTITY que va de uno en uno.
					 * 
					 * También, en la BD, se encuentran los procedimientos almacenados con documentación integrada, -
					 * por lo que si quieren ver más detallado su funcionamiento, entonces pueden checar dicho procedimiento -
					 * en la BD.
					 */
                    int idUsuario = GetUserIdByEmailCedula(oldCorreo, oldCedula);
                    string query = "Exec ActualizarListadoEmpleados @IdUsuario, @Cedula, @Nombre, @PrimerApellido, @SegundoApellido, @Contraseña, @Puesto, @FechaContratacion, @Salario, @Correo, @Telefono, @Direccion, @Activo, @IdRol";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@PrimerApellido", primerApellido);
                        command.Parameters.AddWithValue("@SegundoApellido", segundoApellido);
                        command.Parameters.AddWithValue("@Contraseña", contraseña);
                        command.Parameters.AddWithValue("@Puesto", puesto);
                        command.Parameters.AddWithValue("@FechaContratacion", fechaContratacion);
                        command.Parameters.AddWithValue("@Salario", salario);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Activo", activo);
                        command.Parameters.AddWithValue("@IdRol", idrol);

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

        /* 
		 * Método que sirve para enviar los datos que se quiere eliminar de un empleado -
		 * hacia la base de datos que esta en SQL Server.
		 */
        public bool DeleteEmployee(int idrol, string correo, string cedula)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    /* 
					 * Aquí sucede algo similar al del método de actualizar un empleado, -
					 * solo que aquí es para eliminar un empleado.
					 * 
					 * Nota: VER el comentario [2.1]. 
					 */
                    int idUsuario = GetUserIdByEmailCedula(correo, cedula);
                    string query = "Exec EliminarListadoEmpleados @IdRol, @IdUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdRol", idrol);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

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

        public bool AddDesvinculacion(DateTime FechaInicio, string Motivo,
                                      string Comentarios, DateTime FechaSalida)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query1 = "Exec CreacionSolicitudDesvinculacion @FechaInicio, @Motivo, @Comentarios, @FechaSalida";
                    
                }
            return true;
            }
        }
        /* Método que valida si el correo existe o no existe en la base de datos -
         * de Molino Central de Coronado. Esto por medio de un procedimiento -
         * almacenado conocido como: RecuperarContraseña, el cual es un procedimiento -
         * almacenado que es compartido con el método: InsertarNuevaContraseña(). */
        public bool ValidarCorreo(string correo)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec RecuperarContraseña @Correo, @PermiteComando, @ContraseñaNueva";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@PermiteComando", 0);
                        command.Parameters.AddWithValue("@ContraseñaNueva", string.Empty);

                        object ValidaDato = command.ExecuteScalar();
                        success = (ValidaDato != null && ValidaDato != DBNull.Value);

                        if (success == false)
                        {
                            MessageBox.Show("El correo no existe dentro de Molino Central de Coronado.",
                                "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    CloseConnection(connection);
                }
            }

            return success;
        }

        /* Método que sirve para poder registar la nueva contraseña que esta colocando el usuario, -
         * además de su correo asociado, también se valida si la contraseña y el correo no son datos nulos, -
         * esto por motivos de buenas prácticas, después de esto, si la contraseña y el correo no son nulos, -
         * estos son enviados al procedimiento almacenado conocido como: RecuperarContraseña, el cual es un -
         * procedimiento almacenado que es compartido con el método: ValidarCorreo(). */
        public bool InsertarNuevaContraseña(string correo, string contraseña)
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec RecuperarContraseña @Correo, @PermiteComando, @ContraseñaNueva";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (contraseña != string.Empty && correo != string.Empty)
                        {
                            command.Parameters.AddWithValue("@Correo", correo);
                            command.Parameters.AddWithValue("@PermiteComando", 1);
                            command.Parameters.AddWithValue("@ContraseñaNueva", HashPassword(contraseña));
                            try
                            {
                                int rowsAffected = command.ExecuteNonQuery();
                                success = rowsAffected > 0;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error executing insert query: " + ex.Message);
                            }

                            MessageBox.Show("El cambio de contraseña fue totalmente exitoso, inicie sesión nuevamente, gracias.",
                                "Cambio de contraseña.", MessageBoxButton.OK, MessageBoxImage.Information);
                            Envio(true);
                        }
                        else
                        {
                            MessageBox.Show("Parece que hubo un error durante el proceso de cambiar su contraseña, intentelo de nuevo.",
                            "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                            Envio(false);
                        }

                    }
                    CloseConnection(connection);
                }
            }

            return success;
        }

        public void Envio(bool dato)
        {
            /*
            * Intancia a la clase de: CambioContraseña.
            */
            var cambioContraseña = new CambioContraseña();
            cambioContraseña.ErrorEnvio(dato);
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
                else
                {
                    return "";

                }
            }
        }

		public int getIdEmpleadoFromIdUsuario(int Id)
		{

			using (SqlConnection connection = OpenConnection())
			{
				if (connection != null)
				{
					// Find the role name
					string roleQuery = "SELECT Id FROM Empleado WHERE IdUsuario = @IdUsuario";
					int IdUsuario;

					using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
					{
						roleCommand.Parameters.AddWithValue("@IdUsuario", Id);
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
		public bool AddInventario(string nombre, int cantidad, decimal precio, bool activo)
        {
            bool success = false;
            string query = "INSERT INTO Inventario (Nombre, Cantidad, Precio, Activo) " +
                           "VALUES (@Nombre, @Cantidad, @Precio, @Activo)";

            using (SqlConnection connection = OpenConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@Precio", precio);
                    command.Parameters.AddWithValue("@Activo", activo);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al añadir: " + ex.Message);
                    }
                }
                CloseConnection(connection);
            }

            return success;
        }

        public bool EditInventario(int id, string nombre, int cantidad, decimal precio, bool activo)
        {
            bool success = false;
            string query = "UPDATE Inventario SET Nombre = @Nombre, Cantidad = @Cantidad, Precio = @Precio, Activo = @Activo WHERE Id = @Id";

            using (SqlConnection connection = OpenConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Nombre", nombre);
                    command.Parameters.AddWithValue("@Cantidad", cantidad);
                    command.Parameters.AddWithValue("@Precio", precio);
                    command.Parameters.AddWithValue("@Activo", activo);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing update query: " + ex.Message);
                    }
                }
            }

            return success;
        }


        public bool DeleteInventario(int id)
        {
            bool success = false;
            string query = "DELETE FROM Inventario WHERE Id = @Id";

            using (SqlConnection connection = OpenConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        success = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al eliminar: " + ex.Message);
                    }
                }
                CloseConnection(connection);
            }

            return success;
        }

        public Dictionary<string, object> GetInventarioById(int id)
        {
            var result = new Dictionary<string, object>();
            string query = "SELECT Id, Nombre, Cantidad, Precio, Activo FROM Inventario WHERE Id = @Id";

            using (SqlConnection connection = OpenConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

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
            }

            return result;
        }

        public bool ProductoExists(string codigo)
        {
            using (SqlConnection connection = OpenConnection())
            {
                string query = "SELECT COUNT(1) FROM Productos WHERE Codigo = @Codigo";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Codigo", codigo);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public int GetUserIdByEmailCedula(string correo, string cedula)
        {
            using (SqlConnection connection = OpenConnection())
            {
                int userId;
                string query = "SELECT Id FROM Usuario WHERE Correo = @Correo AND Cedula = @Cedula";
                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@Correo", correo);
                    userCommand.Parameters.AddWithValue("@Cedula", cedula);
                    try
                    {
                        object result = userCommand.ExecuteScalar();
                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                            return userId;
                        }
                        else
                        {
                            Console.WriteLine("Usuario no Existe.");
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
        }

        public bool InsertarOrden()
        {
            bool success = false;

            using (SqlConnection connection = OpenConnection())
            {
                if (connection != null)
                {
                    try
                    {
                        string query = "INSERT INTO Orden (FechaHoraCreacion, Estado) VALUES (@Fecha, @Estado)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                            command.Parameters.AddWithValue("@Estado", true);
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }

                        string query2 = "INSERT INTO ProductosOrden (FechaHoraCreacion, Estado) VALUES (@Fecha, @Estado)";
                        using (SqlCommand command = new SqlCommand(query2, connection))
                        {
                            command.Parameters.AddWithValue("@Fecha", DateTime.Now);
                            command.Parameters.AddWithValue("@Estado", true);
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing insert query: " + ex.Message);
                        // Lanza la excepción para que quien llama pueda manejarla adecuadamente.
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection);
                    }
                }
            }

            return success;
        }
    }
}
           


