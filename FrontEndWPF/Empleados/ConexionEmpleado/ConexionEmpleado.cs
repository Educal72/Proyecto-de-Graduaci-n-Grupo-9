using FrontEndWPF.Index;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    public class ConexionEmpleado
    {  
        //Instancia de la clase: Conexión.
        Conexion conexion = new Conexion();
        
        //Variables globales estaticas.
        static int Si_Prosigue = 0;
        static int No_Prosigue = 0;
        static int permisoLeerDesv;
        static int permisoCrearDesv;
        static int permisoEliminarDesv;
        static string? nombreUsuario;


        //Constructor vacio.
        public ConexionEmpleado()
        {
            NombreUsuario(nombreUsuario!);
        }


        /*|==========================================================| ZONA DE MÉTODOS DE OBTENCIÓN DE RESPUESTAS |==========================================================*/

        /* Este método sirve para poder validar si los métodos enfocados al CRUD y otros más en la parte -
         * de la BD, se ejecuto con normalidad ó se detecto algún error en especifico.*/
        public void ProseguirAñadirEmpleado(int si, int no)
        {
            var caminoAñadir = new EmployeeListControl();
            var caminoDesv = new Desvinculaciones();

            if (si == 4)
            {
                caminoAñadir.ProseguirDatoGuardado(true);
                caminoDesv.ProseguirDatoGuardado(true);
            }

            if (no == 1)
            {
                caminoAñadir.ProseguirDatoGuardado(false);
                caminoDesv.ProseguirDatoGuardado(false);
            }

        }


        /* Este método se encarga de guardar el nombre del usuario que esta ingresando en el sistema, -
         * esto desde el login.*/
        public void NombreUsuario(string nombreActual)
        {
            nombreUsuario = nombreActual;
            permisoLeerDesv = VerificarPermisoLeer(GetUserIdByName(nombreActual!));
            permisoCrearDesv = VerificarPermisoCrear(GetUserIdByName(nombreActual!));
            permisoEliminarDesv = VerificarPermisoEliminar(GetUserIdByName(nombreActual!));
        }


        /* Este método sirve para traer el número entero correspondiente al permiso de leer desvinculaciones -
         * por parte del usuario. */
        public int PermisoLeer()
        {
            return permisoLeerDesv;
        }


        /* Este método sirve para traer el número entero correspondiente al permiso de crear desvinculaciones -
         * por parte del usuario. */
        public int PermisoCrear()
        {
            return permisoCrearDesv;
        }


        /* Este método sirve para traer el número entero correspondiente al permiso de eliminar desvinculaciones -
         * por parte del usuario. */
        public int PermisoEliminar()
        {
            return permisoEliminarDesv;
        }


        /*|==========================================================| ZONA DE MÉTODOS OBTENCIÓN DE DATOS |==========================================================*/

        /* Este método sirve para poder obtener el id de la tabla de: ControlPlanillas, -
         * esto por medio del correo y la cédula que se esta pasando por parámetro.*/
        public int GetUserIdByEmailCedula(string correo, string cedula)
        {
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT Id FROM ControlPlanillas WHERE Correo = @Correo AND Cedula = @Cedula";

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
                            Si_Prosigue = 1;
                            return userId;
                        }
                        else
                        {
                            No_Prosigue = 1;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Console.WriteLine("Planilla no Existe.");
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        No_Prosigue = 1;
                        conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        Console.WriteLine("Error: " + ex.Message);
                        return 0;
                    }
                }
            }
        }


        /* Este método sirve para poder obtener el id de la tabla de: Usuario, -
         * esto por medio del correo y la cédula que se esta pasando por parámetro.*/
        public int GetUserId(string correo, string cedula)
        {
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
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
                            Si_Prosigue = 1;
                            return userId;
                        }
                        else
                        {
                            No_Prosigue = 1;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            MessageBox.Show("El id del usuario no fue encontrado.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        No_Prosigue = 1;
                        conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                        "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                        "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                }
            }
        }


        /* Este método sirve para poder obtener el id de la tabla de: Usuario, -
         * esto por medio del nombre y la cédula que se esta pasando por parámetro.*/
        public int GetIdPermisosAutorizacion(string nombre, string cedula)
        {
            var conexion_Empleado = new ConexionEmpleado();
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT Id FROM Usuario WHERE Nombre = @Nombre AND Cedula = @Cedula";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@Nombre", nombre);
                    userCommand.Parameters.AddWithValue("@Cedula", cedula);
                    try
                    {
                        object result = userCommand.ExecuteScalar();
                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                            Si_Prosigue = 1;
                            return userId;
                        }
                        else
                        {
                            No_Prosigue = 1;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            MessageBox.Show("El id del usuario no fue encontrado.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        No_Prosigue = 1;
                        conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                        "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos, muchas gracias.",
                        "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                }
            }
        }


        /* Este método sirve para poder obtener el id de la tabla de: Usuario, -
         * esto por medio del correo y la cédula que se esta pasando por parámetro, -
         * esto para la tabla de incidentes.*/
        public int GetUserIdByEmai(string correo)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT Id FROM Usuario WHERE Correo = @Correo";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@Correo", correo);
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
                            Console.WriteLine("Usuario no existe para el incidente.");
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


        /* Este método sirve para poder obtener el id de la tabla de: Usuario, -
         * esto por medio del nombre que se esta pasando por parámetro.*/
        public int GetUserIdByName(string nombreUsuario)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT Id FROM Usuario WHERE Nombre = @Nombre";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@Nombre", nombreUsuario);
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
                            Console.WriteLine("Usuario no existe.");
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


        /* Este método sirve para verificar si tiene permisos suficientes el usuario, 
         * el cual su id se esta pasando por parámetro.*/
        public int VerificarPermisoLeer(int idUsuario)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT LeerDesvinculacion FROM Autorizacion WHERE IdUsuario = @IdUsuario";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
                            Console.WriteLine("No tiene permiso.");
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


        /* Este método sirve para verificar si tiene permisos suficientes el usuario, 
         * el cual su id se esta pasando por parámetro.*/
        public int VerificarPermisoCrear(int idUsuario)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT CrearDesvinculacion FROM Autorizacion WHERE IdUsuario = @IdUsuario";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
                            Console.WriteLine("No tiene permiso.");
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


        /* Este método sirve para verificar si tiene permisos suficientes el usuario, 
         * el cual su id se esta pasando por parámetro.*/
        public int VerificarPermisoEliminar(int idUsuario)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                int userId;
                string query = "SELECT EliminarDesvinculacion FROM Autorizacion WHERE IdUsuario = @IdUsuario";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
                            Console.WriteLine("No tiene permiso.");
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


        /* Este método sirve para obtener todos los registros de la tabla de: ControlPlanillas, -
         * esto para poder hacer la validación de la cédula en la clase de: "añadirPlanilla".*/
        public Dictionary<string, object> SelectUserCedula(string correo, int cedula)
        {
            var result = new Dictionary<string, object>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT * FROM ControlPlanillas WHERE Correo = @Correo AND Cedula = @Cedula";
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

                    conexion.CloseConnection(connection);
                }
            }

            return result;
        }


        /* Método que obtiene el Id del rol por el nombre que se le pasando -
         * por parámetro.*/
        public int ObtenerID_X_Rol(string Rol)
        {
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                string roleQuery = "Exec ObtenerID_X_Rol @Rol";
                int roleId = -1;
                using (SqlCommand roleCommand = new SqlCommand(roleQuery, connection))
                {
                    roleCommand.Parameters.AddWithValue("@Rol", Rol);
                    try
                    {
                        object result = roleCommand.ExecuteScalar();
                        if (result != null)
                        {
                            roleId = Convert.ToInt32(result);
                            Si_Prosigue = 1;
                            return roleId;
                        }
                        else
                        {
                            No_Prosigue = 1;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            MessageBox.Show("El rol no fue encontrado.", "¡Error!",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        No_Prosigue = 1;
                        conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                            "\nSi el error persiste, contacte con el soporte, muchas gracias.", "¡Error!: " + ex.Message,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                }
            }
        }


        /* Método que obtiene el nombre del rol por el Id que se le pasando -
         * por parámetro.*/
        public string ObtenerNombreRol_X_ID(int idRol)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                string userId;
                string query = "SELECT Nombre FROM Roles WHERE Id = @idRol";

                using (SqlCommand userCommand = new SqlCommand(query, connection))
                {
                    userCommand.Parameters.AddWithValue("@idRol", idRol);
                    try
                    {
                        object result = userCommand.ExecuteScalar();
                        if (result != null)
                        {
                            userId = result.ToString()!;
                            return userId;
                        }
                        else
                        {
                            Console.WriteLine("Nombre del rol no existe.");
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
        }


        /* Método que trae todos los nombres de los usuarios -
         * registrados y lo pone en una lista.*/
        public List<string> Usuarios()
        {
            List<string> TodosLosUsuarios = new List<string>();
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Nombre FROM Usuario";
                    string NombreUsuarios;

                    using (SqlCommand roleCommand = new SqlCommand(query, connection))
                    {
                        try
                        {
                            SqlDataReader reader = roleCommand.ExecuteReader();

                            while (reader.Read())
                            {
                                NombreUsuarios = reader["Nombre"].ToString()!;
                                TodosLosUsuarios.Add(NombreUsuarios);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                                "\nSi el error persiste, contacte con el soporte, muchas gracias.", "¡Error!: " + ex.Message,
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            return TodosLosUsuarios;
        }


        /* Método que se encarga de dar el ID del usuario -
         * que se le esta pasando por párametro.*/
        public string getIdUsuario(string Usuario)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Id FROM Usuario WHERE Nombre = @Nombre AND Apellido = @Apellido";
                    string idUsuario;
                    var SplitString = SplitStringAtFirstSpace(Usuario);
                    string Nombre = SplitString.Item1;
                    string Apellido = SplitString.Item2;

                    using (SqlCommand roleCommand = new SqlCommand(query, connection))
                    {
                        roleCommand.Parameters.AddWithValue("@Nombre", Nombre);
						roleCommand.Parameters.AddWithValue("@Apellido", Apellido);

						try
                        {
                            object result = roleCommand.ExecuteScalar();
                            if (result != null)
                            {
                                idUsuario = result.ToString()!;
                                return idUsuario;
                            }
                            else
                            {
                                MessageBox.Show("El id del usuario no fue encontrado.", "¡Error!",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return "";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                                "\nSi el error persiste, contacte con el soporte, muchas gracias.", "¡Error!: " + ex.Message,
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return "";
                        }//Fin del Try Catch.
                    }//Fin del usign (SqlCommand roleCommand = new SqlCommand(query, connection)).
                }//Fin del if (connection != null).
                else
                {
                    return "";

                }//Fin del else.
            }//Fin del using (SqlConnection connection = OpenConnection()).
        }//Fin del método.

		public string getIdUsuario2(string Usuario)
		{
			using (SqlConnection connection = conexion.OpenConnection())
			{
				if (connection != null)
				{
					string query = "SELECT Id FROM Usuario WHERE Nombre = @Nombre";
					string idUsuario;
					var SplitString = SplitStringAtFirstSpace(Usuario);
					string Nombre = SplitString.Item1;
					string Apellido = SplitString.Item2;

					using (SqlCommand roleCommand = new SqlCommand(query, connection))
					{
						roleCommand.Parameters.AddWithValue("@Nombre", Nombre);

						try
						{
							object result = roleCommand.ExecuteScalar();
							if (result != null)
							{
								idUsuario = result.ToString()!;
								return idUsuario;
							}
							else
							{
								MessageBox.Show("El id del usuario no fue encontrado.", "¡Error!",
									MessageBoxButton.OK, MessageBoxImage.Error);
								return "";
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
								"\nSi el error persiste, contacte con el soporte, muchas gracias.", "¡Error!: " + ex.Message,
								MessageBoxButton.OK, MessageBoxImage.Error);
							return "";
						}//Fin del Try Catch.
					}//Fin del usign (SqlCommand roleCommand = new SqlCommand(query, connection)).
				}//Fin del if (connection != null).
				else
				{
					return "";

				}//Fin del else.
			}//Fin del using (SqlConnection connection = OpenConnection()).
		}//Fin del método.

		public (string, string) SplitStringAtFirstSpace(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return (string.Empty, string.Empty); // Retornar cadena vacía si la entrada está vacía o es null
			}

			int index = input.IndexOf(' ');

			if (index == -1)
			{
				return (input, string.Empty); // Retornar toda la cadena como la primera parte si no hay espacio
			}

			string firstPart = input.Substring(0, index);
			string secondPart = input.Substring(index + 1);

			return (firstPart, secondPart);
		}

		/* Método que trae el nombre del usuario por el id -
         * que se pasa por parámetro de entrada.*/
		public string UsuarioPorID(int id)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Nombre, Apellido FROM Usuario Where Id = @ID";
                    string NombreUsuario;

                    using (SqlCommand roleCommand = new SqlCommand(query, connection))
                    {
                        roleCommand.Parameters.AddWithValue("ID", id);
                        try
                        {

                            using (SqlDataReader reader = roleCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        NombreUsuario = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
                                        return NombreUsuario;
                                    }
                                }
                            
                            else
                            {
                                MessageBox.Show("No hay un usuario con ese id proporcionado, intentelo de nuevo por favor.", "¡Error!",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                Console.WriteLine("Nombres de los usuarios no se pudo hacer.");
                                return "";
                            }
                                return "";
                        }
                        } 
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ocurrio un error interno, intentelo de nuevo " +
                                "\nSi el error persiste, contacte con el soporte, muchas gracias.", "¡Error!: " + ex.Message,
                                MessageBoxButton.OK, MessageBoxImage.Error);
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


        /*|==========================================================| ZONA DE MÉTODOS TIPO CRUD |==========================================================*/

        /* Método que sirve para enviar los datos que se quiere actualizar de un empleado, -
         * esto hacia la base de datos que esta en SQL Server. */
        public bool UpdateEmployee(string cedula, string nombre, string apellido, string puesto,
            DateTime fechaContratacion, string correo, string telefono, bool activo, string rol,
            string direccion, string oldCedula, string oldCorreo)
        {

            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();
            
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    /* Aquí lo que hace es obtener el IdUsuario a través del método: GetUserId().
					 * El cuál, tomara la cédula y correo (antes de que el usuario los hubiera modificado) para -
					 * así poder obtener el IdUsuario correspondiente.
					 * 
					 * Lo mismo pasa con el IdRol, nada más que aqui se obtiene el Id del rol a partir del nombre -
					 * que se le esta mandando.
					 * 
					 * También en la BD, se encuentran los procedimientos almacenados con documentación integrada, -
					 * por lo que si quieren ver su funcionamiento un poco más detallado, entonces pueden checar -
					 * dicho procedimiento en la BD. */
                    int idUsuario = GetUserId(oldCorreo, oldCedula);
                    int idRol = ObtenerID_X_Rol(rol);

                    string query = "Exec ActualizarListadoEmpleados @IdUsuario, @Cedula, @Nombre, @Apellido, @Puesto, @FechaContratacion, @Correo, @Telefono, @Activo, @Direccion, @IdRol";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
                        command.Parameters.AddWithValue("@Puesto", puesto);
                        command.Parameters.AddWithValue("@FechaContratacion", fechaContratacion);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Activo", activo);
                        command.Parameters.AddWithValue("@IdRol", idRol);
                        command.Parameters.AddWithValue("@Direccion", direccion);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
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
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que sirve para enviar los datos que se quiere eliminar de un empleado, -
		 * esto hacia la base de datos que esta en SQL Server. */
        public bool DeleteEmployee(string rol, string correo, string cedula)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    /* Aquí sucede algo similar al del método de actualizar un empleado, -
					 * solo que aquí es para eliminar a un empleado es especifico. */
                    int idUsuario = GetUserId(correo, cedula);
                    int idrol = ObtenerID_X_Rol(rol);

                    string query = "Exec EliminarListadoEmpleados @IdRol, @IdUsuario";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdRol", idrol);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                            
                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
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
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que sirve para enviar los datos que se quieren agregar de una solicitud -
         * de desvinculación, esto hacia la base de datos que esta en SQL Server. */
        public bool AddDesvinculacion(string nombre, string apellido, DateTime fechaInicio, string motivo,
            string comentarios, DateTime fechaSalida)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query1 = "Exec CreacionSolicitudDesvinculacion @Nombre, @Apellido, @FechaInicio, @Motivo, @Comentarios, @FechaSalida";

                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellido);
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                        command.Parameters.AddWithValue("@Motivo", motivo);
                        command.Parameters.AddWithValue("@Comentarios", comentarios);
                        command.Parameters.AddWithValue("@FechaSalida", fechaSalida);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                            
                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("La creación de la solicitud de desvinculación no se pudo completar en este momento debido a un problema técnico, intentelo de nuevo más tarde. " +
                            "\nSi el error persiste, contacte con el soporte o con el departamento de recursos humanos para obtener asistencia adicional, muchas gracias.",
                            "¡Error!: " + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que sirve para enviar los datos que se quieren eliminar de una solicitud -
         * de desvinculación, esto hacia la base de datos que esta en SQL Server. */
        public bool DeleteDesvinculaciones(int id)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec EliminarSolicitudDesvinculacion @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("La solicitud de eliminación sobre la desvinculación seleccionada no se pudo completar en este momento debido a un problema técnico." +
                            "\nIntentelo de nuevo más tarde, si el problema persiste, entonces contacte al soporte técnico, muchas gracias.", "¡Error!: " + ex.Message,
                            MessageBoxButton.OK, MessageBoxImage.Error);

                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que marca el estado de una desvinculación como: "True (reconocido) ó -
         * False (no reconocido)" en la BD.*/
        public bool ReconocerDesvinculacion(bool estado, int idDesvinculacion)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec MarcarEstadoDesvinculacion @Estado, @IdDesvinculacion";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Estado", estado);
                        command.Parameters.AddWithValue("@IdDesvinculacion", idDesvinculacion);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                        }
                        catch (Exception ex)
                        {
                            No_Prosigue = 1;
                            MessageBox.Show("Hubo un error interno en la marcación debido a un problema técnico." +
                            "\nIntentelo de nuevo más tarde, si el problema persiste, entonces contacte al soporte técnico, muchas gracias.", "¡Error!: " + ex.Message,
                            MessageBoxButton.OK, MessageBoxImage.Error);

                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
                            Si_Prosigue = 0;
                            No_Prosigue = 0;
                            return false;
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que agrega una nueva planilla en la BD.*/
        public bool AgregarPlanilla(string nombre, string apellidos, string cedula, string puesto,
            string correo, DateTime fechaCreacion, double salario)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "EXEC CrearControlPlanillas @Nombre, @Apellido, @Cedula, @Puesto, @Correo, @FechaCreacion, @Salario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellidos);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Puesto", puesto);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@FechaCreacion", fechaCreacion);
                        command.Parameters.AddWithValue("@Salario", salario);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un problema en la inserción de los datos, intentelo de nuevo por favor.",
                                "¡Error!" + ex, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }


                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que actualiza una nueva planilla en la BD.*/
        public bool ActualizarPlanilla(string nombre, string apellidos, string cedula,
            string puesto, string correo, DateTime fechaCreacion, decimal salario, string oldCorreo,
            string oldCedula)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    int idplanilla = GetUserIdByEmailCedula(oldCorreo, oldCedula);
                    string query = "Exec ActualizarControlPlanillas @IdPlanilla, @Nombre, @Apellido, @Cedula, @Puesto, @Correo, @FechaCreacion, @Salario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlanilla", idplanilla);
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Apellido", apellidos);
                        command.Parameters.AddWithValue("@Cedula", cedula);
                        command.Parameters.AddWithValue("@Puesto", puesto);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@FechaCreacion", fechaCreacion);
                        command.Parameters.AddWithValue("@Salario", salario);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error en la actualización, intentelo de nuevo por favor.",
                                "¡Error! \n" + ex + "\n", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que elimina una nueva planilla en la BD.*/
        public bool EliminarPlanilla(string identificaCorreo,
            string identificaCedula)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    int idPlanilla = GetUserIdByEmailCedula(identificaCorreo, identificaCedula);
                    string query = "Exec EliminarControlPlanillas @IdPlanilla";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlanilla", idPlanilla);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error en la eliminación, intentelo de nuevo por favor.",
                                "¡Error! \n" + ex + "\n", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que agrega un nuevo incidente en la BD.*/
        public bool AgregarIncidente(DateTime fecha, string hora, string descripcion,
            string tipoIncidente, string Usuario)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    int idUsuario = Convert.ToInt32(Usuario);
                    string query = "EXEC CrearIncidentes @Fecha, @Hora, @Descripcion, @Tipo, @IdUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Fecha", fecha);
                        command.Parameters.AddWithValue("@Hora", hora);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@Tipo", tipoIncidente);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un problema en la inserción de los datos, intentelo de nuevo por favor.",
                                "¡Error!" + ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }


                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que marca un incidente como: "Resuelto ó No resuelto" en la BD.*/
        public bool MarcarIncidente(string estado, string Usuario)
        {
            bool success = false;
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    int idUsuario = Convert.ToInt32(Usuario);
                    
                    
                    string query = "Exec MarcarResueltoIncidente @Estado, @IdUsuario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Estado", estado);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error en la marcación, intentelo de nuevo por favor.",
                                "¡Error! \n" + ex + "\n", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que agrega un nuevo perfil competencial a la BD.*/
        public bool AgregarPerfilCompetencial(string titulo, string descripcion, string experiencia,
            string requisitos, string ubicacion, double salario)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "EXEC CrearPerfilCompetencial @Titulo, @Descripcion, @Experiencia, @Requisitos, @Ubicacion, @Salario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Titulo", titulo);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@Experiencia", experiencia);
                        command.Parameters.AddWithValue("@Requisitos", requisitos);
                        command.Parameters.AddWithValue("@Ubicacion", ubicacion);
                        command.Parameters.AddWithValue("@Salario", salario);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("La creación del perfil competencial no se pudo completar en este momento debido a un problema técnico. " +
                                "\nSi el problema persiste, se sugiere intentarlo más tarde o contactar al soporte técnico respectivamente.",
                                "¡Error!" + ex, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que actualiza un perfil competencial que fue seleccionado y lo lleva a la BD.*/
        public bool ActualizarPerfilCompetencial(int idPerfilCompetencial, string titulo, string descripcion, string experiencia,
            string requisitos, string ubicacion, double salario)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec ActualizarPerfilCompetencial @Id, @Titulo, @Descripcion, @Experiencia, @Requisitos, @Ubicacion, @Salario";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idPerfilCompetencial);
                        command.Parameters.AddWithValue("@Titulo", titulo);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@Experiencia", experiencia);
                        command.Parameters.AddWithValue("@Requisitos", requisitos);
                        command.Parameters.AddWithValue("@Ubicacion", ubicacion);
                        command.Parameters.AddWithValue("@Salario", salario);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("La actualización del perfil competencial no se pudo completar en este momento debido a un problema técnico. " +
                                "\nSi el problema persiste, se sugiere intentarlo más tarde o contactar al soporte técnico respectivamente.",
                                "¡Error! \n" + ex + "\n", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /* Método que elimina un perfil competencial que fue seleccionado y lo lleva a la BD.*/
        public bool EliminarPerfilCompetencial(int idPerfilCompetencial)
        {
            bool success = false;

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "Exec EliminarPerfilCompetencial @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idPerfilCompetencial);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("La eliminación del perfil competencial no se pudo completar en este momento debido a un problema técnico. " +
                                "\nSi el problema persiste, se sugiere intentarlo más tarde o contactar al soporte técnico respectivamente.",
                                "¡Error! \n" + ex + "\n", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    conexion.CloseConnection(connection);
                }
            }

            return success;
        }


        /*|==========================================================| ZONA DE MÉTODOS DE PERMISOS |==========================================================*/

        /* Método que sirve para enviar los datos que se quieren para asignar permisos de autorización a un empleado, -
         * esto hacia la base de datos que esta en SQL Server. */
        public bool RegistrarPermisoAutorizacion(string nombre, string cedula, bool leerDesv, bool crearDesv, bool eliminarDesv)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    /* Aquí lo que hace es obtener el IdUsuario a través del método: GetIdPermisosAutorizacion().
					 * El cuál, tomara el nombre y la cédula para así poder obtener el IdUsuario correspondiente.
                     *
					 * También en la BD, se encuentran los procedimientos almacenados con documentación integrada, -
					 * por lo que si quieren ver su funcionamiento un poco más detallado, entonces pueden checar -
					 * dicho procedimiento en la BD.*/
                    int idUsuario = GetIdPermisosAutorizacion(nombre, cedula);
                    string query = "Exec PermisosAutorizacion @IdUsuario, @LeerDesv, @CrearDesv, @EliminarDesv";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@LeerDesv", leerDesv);
                        command.Parameters.AddWithValue("@CrearDesv", crearDesv);
                        command.Parameters.AddWithValue("@EliminarDesv", eliminarDesv);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;
                            
                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
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
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


        /* Método que sirve para enviar los datos que se quieren para asignar permisos de autorización a un empleado, -
         * esto hacia la base de datos que esta en SQL Server. */
        public bool ActualizarPermisoAutorizacion(string nombre, string cedula, bool leerDesv, bool crearDesv, bool eliminarDesv)
        {
            bool success = false;
            var conexion_Empleado = new ConexionEmpleado();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    /* Aquí lo que hace es obtener el IdUsuario a través del método: GetIdPermisosAutorizacion().
					 * El cuál, tomara el nombre y la cédula para así poder obtener el IdUsuario correspondiente.
                     *
					 * También en la BD, se encuentran los procedimientos almacenados con documentación integrada, -
					 * por lo que si quieren ver su funcionamiento un poco más detallado, entonces pueden checar -
					 * dicho procedimiento en la BD.*/
                    int idUsuario = GetIdPermisosAutorizacion(nombre, cedula);
                    string query = "Exec ActualizarPermisosAutorizacion @IdUsuario, @LeerDesv, @CrearDesv, @EliminarDesv";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        command.Parameters.AddWithValue("@LeerDesv", leerDesv);
                        command.Parameters.AddWithValue("@CrearDesv", crearDesv);
                        command.Parameters.AddWithValue("@EliminarDesv", eliminarDesv);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            success = rowsAffected > 0;

                            Si_Prosigue = 4;
                            conexion_Empleado.ProseguirAñadirEmpleado(Si_Prosigue, No_Prosigue);
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
                    conexion.CloseConnection(connection);
                }
            }
            Si_Prosigue = 0;
            No_Prosigue = 0;
            return success;
        }


    }//Fin de la clase: ConexionEmpleado.
}//Fin del namespace: FrontEndWPF.