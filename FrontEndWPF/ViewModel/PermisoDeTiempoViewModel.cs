using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontEndWPF.Modelos;

namespace FrontEndWPF.ViewModel
{
    public class PermisoDeTiempoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PermisoDeTiempo> _permisosDeTiempo;
        public ObservableCollection<PermisoDeTiempo> PermisosDeTiempo
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
            _permisosDeTiempo = new ObservableCollection<PermisoDeTiempo>();
            _usuarios = new ObservableCollection<UserModel.UsuarioEmpleado>(); // Inicialización de la colección de usuarios
            LoadPermisosDeTiempo();
            LoadUsuarios(); // Cargar usuarios al inicializar el ViewModel
        }

        private void LoadPermisosDeTiempo()
        {
            Conexion conexion = new Conexion();
            var permisosList = conexion.GetPermisosDeTiempo();

            foreach (var permisoDict in permisosList)
            {
                PermisoDeTiempo permiso = new PermisoDeTiempo
                {
                    IdEmpleado = (int)permisoDict["IdEmpleado"],
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
                    PermisoDeTiempo nuevoPermiso = new PermisoDeTiempo
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
                // Manejo de la excepción (puedes registrar el error o mostrar un mensaje al usuario)
                Console.WriteLine("Error al actualizar el permiso de tiempo: " + ex.Message);
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
