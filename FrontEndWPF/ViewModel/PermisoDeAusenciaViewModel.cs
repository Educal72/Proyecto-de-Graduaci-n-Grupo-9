using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndWPF.Modelos;


namespace FrontEndWPF.ViewModel
{
    public class PermisoDeAusenciaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PermisoDeAusencia> _permisosDeAusencia;
        public ObservableCollection<PermisoDeAusencia> PermisosDeAusencia
        {
            get { return _permisosDeAusencia; }
            set
            {
                _permisosDeAusencia = value;
                OnPropertyChanged("PermisosDeAusencia");
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

        public PermisoDeAusenciaViewModel()
        {
            _permisosDeAusencia = new ObservableCollection<PermisoDeAusencia>();
            _usuarios = new ObservableCollection<UserModel.UsuarioEmpleado>(); // Inicialización de la colección de usuarios
            LoadPermisosDeAusencia();
            LoadUsuarios(); // Cargar usuarios al inicializar el ViewModel
        }

        private void LoadPermisosDeAusencia()
        {
            _permisosDeAusencia.Clear();
            Conexion conexion = new Conexion();
            var permisosList = conexion.GetPermisosDeAusencia();

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
                _permisosDeAusencia.Add(permiso);
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

        public bool UpdateEstadoPermisoDeAusencia(int idEmpleado, string nuevoEstado)
        {
            Conexion conexion = new Conexion();
            return conexion.UpdateEstadoPermisosAusencia(idEmpleado, nuevoEstado);
        }

        public bool EliminarPermisoDeAusencia(int idEmpleado)
        {
            Conexion conexion = new Conexion();
            bool eliminado = conexion.EliminarPermisosAusencia(idEmpleado);
            if (eliminado)
            {
                var permiso = PermisosDeAusencia.FirstOrDefault(p => p.IdEmpleado == idEmpleado);
                if (permiso != null)
                {
                    PermisosDeAusencia.Remove(permiso);
                }
            }
            return eliminado;
        }

        // Nuevo método para crear permisos de tiempo
        public bool CrearPermisoAusencia(int idEmpleado, DateTime fechaInicio, DateTime fechaFin, string motivo)
        {
            try
            {
                Conexion conexion = new Conexion();
                bool creado = conexion.CrearPermisoAusencia(idEmpleado, fechaInicio, fechaFin, motivo);

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
                    _permisosDeAusencia.Add(nuevoPermiso);
                }

                return creado;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción (puedes registrar el error o mostrar un mensaje al usuario)
                Console.WriteLine("Error al crear el permiso de Ausencia: " + ex.Message);
                return false;
            }
        }

        // Método para actualizar permisos de tiempo
        public bool ActualizarPermisoAusencia(int idEmpleado, DateTime nuevaFechaInicio, DateTime nuevaFechaFin, string nuevoMotivo)
        {
            try
            {
                Conexion conexion = new Conexion();
                bool actualizado = conexion.ActualizarPermisoAusencia(idEmpleado, nuevaFechaInicio, nuevaFechaFin, nuevoMotivo);

                if (actualizado)
                {
                    // Encuentra el permiso en la colección y actualiza los valores
                    var permiso = PermisosDeAusencia.FirstOrDefault(p => p.IdEmpleado == idEmpleado);
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
                Console.WriteLine("Error al actualizar el permiso de ausencia: " + ex.Message);
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
