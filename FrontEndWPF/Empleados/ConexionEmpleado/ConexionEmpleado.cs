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
















    }
}