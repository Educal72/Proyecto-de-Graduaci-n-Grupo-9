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
        Conexion conexion = new Conexion();

        /*|==========================================================| ZONA DE MÉTODOS OBTENCIÓN DE DATOS |==========================================================*/
        public int GetUserIdByEmailCedula(string correo, string cedula)
        {
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
                            return userId;
                        }
                        else
                        {
                            Console.WriteLine("Planilla no Existe.");
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
                    string query = "SELECT Id FROM Usuario WHERE Nombre = @Nombre";
                    string idUsuario;

                    using (SqlCommand roleCommand = new SqlCommand(query, connection))
                    {
                        roleCommand.Parameters.AddWithValue("@Nombre", Usuario);

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


        /* Método que trae el nombre del usuario por el id -
         * que se pasa por parámetro de entrada.*/
        public string UsuarioPorID(int id)
        {
            using (SqlConnection connection = conexion.OpenConnection())
            {
                if (connection != null)
                {
                    string query = "SELECT Nombre FROM Usuario Where Id = @ID";
                    string NombreUsuario;

                    using (SqlCommand roleCommand = new SqlCommand(query, connection))
                    {
                        roleCommand.Parameters.AddWithValue("ID", id);
                        try
                        {
                            object result = roleCommand.ExecuteScalar();
                            if (result != null)
                            {
                                NombreUsuario = result.ToString()!;
                                return NombreUsuario;
                            }
                            else
                            {
                                MessageBox.Show("No hay un usuario con ese id proporcionado, intentelo de nuevo por favor.", "¡Error!", 
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                Console.WriteLine("Nombres de los usuarios no se pudo hacer.");
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
                                "¡Error!" + ex, MessageBoxButton.OK, MessageBoxImage.Error);
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


    }//Fin de la clase: ConexionEmpleado.
}//Fin del namespace: FrontEndWPF.