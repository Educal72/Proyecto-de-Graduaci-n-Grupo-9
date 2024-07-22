using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrontEndWPF.Modelos;
using FrontEndWPF.ViewModel;
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF
{
    public partial class PermisoTiempoCrear : Window
    {
        private PermisoDeTiempoViewModel viewModel;
        private List<UsuarioEmpleado> usuarios; // Lista de usuarios

        public PermisoTiempoCrear()
        {
            InitializeComponent();
            viewModel = new PermisoDeTiempoViewModel();
            CargarUsuarios(); // Cargar los usuarios al inicializar la ventana
        }

        private void CargarUsuarios()
        {
            // Llama al método dropdownUsuarios del ViewModel para obtener la lista de usuarios
            Conexion conexion = new Conexion();
            usuarios = conexion.DropdownUsuarios();

            // Configura el ComboBox con la lista de usuarios
            UsuarioComboBox.ItemsSource = usuarios;
            UsuarioComboBox.DisplayMemberPath = "Nombre"; // O el campo que desees mostrar
            UsuarioComboBox.SelectedValuePath = "Cedula"; // O el campo que desees usar como valor
        }

        private void Button_Click_Crear(object sender, RoutedEventArgs e)
        {
            // Obtener los datos del formulario
            var fechaInicio = FechaInicioPicker.SelectedDate;
            var fechaFin = FechaFinPicker.SelectedDate;
            var motivo = MotivoTextBox.Text;
            var usuarioSeleccionado = UsuarioComboBox.SelectedItem as UsuarioEmpleado;

            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un usuario.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Aquí puedes agregar la lógica para guardar los datos
            // Aquí deberías llamar al método para guardar los datos en la base de datos, incluyendo el usuario seleccionado

            MessageBox.Show("Permiso de tiempo creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            // Cerrar la ventana después de crear el permiso
            this.Close();
        }

        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana sin hacer cambios
            this.Close();
        }
    }
}
