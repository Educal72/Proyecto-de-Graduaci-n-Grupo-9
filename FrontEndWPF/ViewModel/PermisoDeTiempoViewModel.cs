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

        public PermisoDeTiempoViewModel()
        {
            _permisosDeTiempo = new ObservableCollection<PermisoDeTiempo>();
            LoadPermisosDeTiempo();
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}