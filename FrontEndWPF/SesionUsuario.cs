using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FrontEndWPF
{
    public class SesionUsuario
    {
        private static SesionUsuario instance = null;
        private static readonly object padlock = new object();
        public string correo { get; set; }
        public string rol { get; set; }
        public string nombre { get; set; }
        public int id { get; set; }
        Conexion conexion = new Conexion();
        public SesionUsuario()
        {
        }

        public static SesionUsuario Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SesionUsuario();
                    }
                    return instance;
                }
            }
        }

        public void CrearUsuarioGenerico() {
			string query = @"
        INSERT INTO [dbo].[Cliente]
               ([Cedula]
               ,[Nombre]
               ,[Apellidos]
               ,[CorreoElectronico]
               ,[Telefono]
               ,[Asociado]
               ,[Puntos])
         VALUES
               (0
               ,'Generico'
               ,'N/A'
               ,'generico@email.com'
               ,0
               ,0
               ,0)";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.ExecuteNonQuery();
			}
		}

		public void InsertarRoles()
		{
			string query = @"
        INSERT INTO [dbo].[Roles] (Nombre)
        VALUES 
            ('Admin'),
            ('Cajero'),
            ('Salonero'),
            ('Contador')";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.ExecuteNonQuery();
			}
		}
		public void InsertarAutorizacion(int idUsuario)
		{
			string connectionString = "your_connection_string_here";
			string query = @"
        INSERT INTO [dbo].[Autorizacion]
               ([IdUsuario]
               ,[LeerDesvinculacion]
               ,[CrearDesvinculacion]
               ,[EliminarDesvinculacion])
         VALUES
               (@IdUsuario
               ,1
               ,1
               ,1)";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@IdUsuario", idUsuario);
				command.ExecuteNonQuery();
			}
		}

		public void InsertarAutorizacionEmpleado(int idUsuario)
		{
			string connectionString = "your_connection_string_here";
			string query = @"
        INSERT INTO [dbo].[Autorizacion]
               ([IdUsuario]
               ,[LeerDesvinculacion]
               ,[CrearDesvinculacion]
               ,[EliminarDesvinculacion])
         VALUES
               (@IdUsuario
               ,0
               ,0
               ,0)";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@IdUsuario", idUsuario);
				command.ExecuteNonQuery();
			}
		}
		public bool SeleccionarTodasLasAutorizaciones()
		{
			string query = "SELECT * FROM [dbo].[Autorizacion]";

			using (SqlConnection connection = conexion.OpenConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);

				using (SqlDataReader reader = command.ExecuteReader())
				{
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
				}
			}
		}
	}
}