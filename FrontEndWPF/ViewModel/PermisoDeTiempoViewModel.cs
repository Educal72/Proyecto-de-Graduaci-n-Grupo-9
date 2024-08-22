using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndWPF.Modelos;
using System.Data.SqlClient;

namespace FrontEndWPF.ViewModel
{
    public class PermisoDeTiempoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PermisoDeAusencia> _permisosDeTiempo;
        public ObservableCollection<PermisoDeAusencia> PermisosDeTiempo
        {
            get { return _permisosDeTiempo; }
            set
            {
                _permisosDeTiempo = value;
                OnPropertyChanged("PermisosDeTiempo");
            }
        }

        // Nueva propiedad para los usuarios
        private ObservableCollection<UserModel.UsuarioEmpleado> _usuarios;
        public ObservableCollection<UserModel.UsuarioEmpleado> Usuarios
        {
            get { return _usuarios; }
            set
            {
                _usuarios = value;
                OnPropertyChanged("Usuarios");
            }
        }

        public PermisoDeTiempoViewModel()
        {
            _permisosDeTiempo = new ObservableCollection<PermisoDeAusencia>();
            _usuarios = new ObservableCollection<UserModel.UsuarioEmpleado>(); // Inicialización de la colección de usuarios
            LoadPermisosDeTiempo();
            LoadUsuarios(); // Cargar usuarios al inicializar el ViewModel
        }

        private void LoadPermisosDeTiempo()
        {
            _permisosDeTiempo.Clear();
            Conexion conexion = new Conexion();
            var permisosList = conexion.GetPermisosDeTiempo();

            foreach (var permisoDict in permisosList)
            {
                PermisoDeAusencia permiso = new PermisoDeAusencia
                {
                    Id = (int)permisoDict["Id"],
					IdEmpleado = (int)permisoDict["IdEmpleado"],
                    NombreCompleto = GetUserByEmpleadoId((int)permisoDict["IdEmpleado"]),
                    FechaInicio = (DateTime)permisoDict["FechaInicio"],
                    FechaFin = (DateTime)permisoDict["FechaFin"],
                    Motivo = (string)permisoDict["Motivo"],
                    Estado = permisoDict["Estado"].ToString() // Asegurarse de que Estado se maneje como string
                };
                _permisosDeTiempo.Add(permiso);
            }
        }

        private void LoadUsuarios()
        {
            Conexion conexion = new Conexion();
            var usuariosList = conexion.DropdownUsuarios(); // Llamada al nuevo método DropdownUsuarios

            foreach (var usuario in usuariosList)
            {
                Usuarios.Add(usuario); // Añadir usuarios a la colección
            }
        }

        public bool UpdateEstadoPermisoDeTiempo(int idEmpleado, string nuevoEstado)
        {
            Conexion conexion = new Conexion();
            return conexion.UpdateEstadoPermisosTiempo(idEmpleado, nuevoEstado);
        }

        public bool EliminarPermisoDeTiempo(int idEmpleado)
        {
            Conexion conexion = new Conexion();
            bool eliminado = conexion.EliminarPermisosTiempo(idEmpleado);
            if (eliminado)
            {
                var permiso = PermisosDeTiempo.FirstOrDefault(p => p.IdEmpleado == idEmpleado);
                if (permiso != null)
                {
                    PermisosDeTiempo.Remove(permiso);
                }
            }
            return eliminado;
        }

        // Nuevo método para crear permisos de tiempo
        public bool CrearPermisoTiempo(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            try
            {
                Conexion conexion = new Conexion();
                bool creado = conexion.CrearPermisoTiempo(idEmpleado, fechaInicio, fechaFin, motivo);

                if (creado)
                {
                    PermisoDeAusencia nuevoPermiso = new PermisoDeAusencia
                    {
                        IdEmpleado = idEmpleado,
                        FechaInicio = fechaInicio,
                        FechaFin = fechaFin,
                        Motivo = motivo,
                        Estado = "Pendiente" // Estado por defecto
                    };
                    _permisosDeTiempo.Add(nuevoPermiso);
                }

                return creado;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción (puedes registrar el error o mostrar un mensaje al usuario)
                Console.WriteLine("Error al crear el permiso de tiempo: " + ex.Message);
                return false;
            }
        }

        // Método para actualizar permisos de tiempo
        public bool ActualizarPermisoTiempo(int idEmpleado, DateTime nuevaFechaInicio, DateTime nuevaFechaFin, string nuevoMotivo)
        {
            try
            {
                Conexion conexion = new Conexion();
                bool actualizado = conexion.ActualizarPermisoTiempo(idEmpleado, nuevaFechaInicio, nuevaFechaFin, nuevoMotivo);

                if (actualizado)
                {
                    // Encuentra el permiso en la colección y actualiza los valores
                    var permiso = PermisosDeTiempo.FirstOrDefault(p => p.IdEmpleado == idEmpleado);
                    if (permiso != null)
                    {
                        permiso.FechaInicio = nuevaFechaInicio;
                        permiso.FechaFin = nuevaFechaFin;
                        permiso.Motivo = nuevoMotivo;
                        // El estado se mantiene igual
                    }
                }

                return actualizado;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Error al actualizar el permiso de tiempo: " + ex.Message);
                // Considera registrar el error en un archivo o base de datos, o mostrar un mensaje al usuario
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
		public string GetUserByEmpleadoId(int id)
		{
			Conexion conexion = new Conexion();
			string NombreCompleto = "";
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"SELECT 
    Usuario.Nombre, 
    Usuario.Apellido
FROM 
    Usuario
JOIN 
    Empleado ON Usuario.Id = Empleado.IdUsuario
WHERE 
    Empleado.Id = @IdEmpleado";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@IdEmpleado", id));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							NombreCompleto = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString();
							return NombreCompleto;
						}
					}
				}
			}
			return NombreCompleto;
		}
	}
}
