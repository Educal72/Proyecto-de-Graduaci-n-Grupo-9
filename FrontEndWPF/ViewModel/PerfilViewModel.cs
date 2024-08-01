using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FrontEndWPF
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private Conexion conexion;

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(); }
        }

        private string primerApellido;
        public string PrimerApellido
        {
            get => primerApellido;
            set { primerApellido = value; OnPropertyChanged(); }
        }

        private string segundoApellido;
        public string SegundoApellido
        {
            get => segundoApellido;
            set { segundoApellido = value; OnPropertyChanged(); }
        }

        private string cedula;
        public string Cedula
        {
            get => cedula;
            set { cedula = value; OnPropertyChanged(); }
        }

        private string telefono;
        public string Telefono
        {
            get => telefono;
            set { telefono = value; OnPropertyChanged(); }
        }

        private string correo;
        public string Correo
        {
            get => correo;
            set { correo = value; OnPropertyChanged(); }
        }

        private string contraseña;
        public string Contraseña
        {
            get => contraseña;
            set { contraseña = value; OnPropertyChanged(); }
        }

        private string rol;
        public string Rol
        {
            get => rol;
            set { rol = value; OnPropertyChanged(); }
        }


        public PerfilViewModel()
        {
            conexion = new Conexion();
            LoadUserData();
        }

        private void LoadUserData()
        {
            string userEmail = SesionUsuario.Instance.correo;
            var userData = conexion.GetUserByEmail(userEmail);
            if (userData != null)
            {
                Nombre = userData["Nombre"].ToString();
                PrimerApellido = userData["Apellido"].ToString();
                Cedula = userData["Cedula"].ToString();
                Telefono = userData["Telefono"].ToString();
                Correo = userData["Correo"].ToString();
                Rol = conexion.getRoleName(Convert.ToInt32(userData["IdRol"]));
            }
        }

        public void ActualizarUsuario()
        {
            bool actualizado = conexion.ActualizarUsuario(Correo, Nombre, PrimerApellido, SegundoApellido, Cedula, Telefono, Rol);
            if (actualizado)
            {
                // Usuario actualizado exitosamente
            }
            else
            {
                // Error al actualizar el usuario
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
