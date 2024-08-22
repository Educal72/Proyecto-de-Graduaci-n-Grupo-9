using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FrontEndWPF.Modelos;
using FrontEndWPF.ViewModel;
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF.Empleados
{
    /// <summary>
    /// Interaction logic for PermisoAusenciaCrear.xaml
    /// </summary>
    public partial class PermisoAusenciaCrear : Window
    {
        private PermisoDeAusenciaViewModel viewModel;
        private List<UsuarioEmpleado> usuarios; // Lista de usuarios
        Conexion conexion = new Conexion();
        public PermisoAusenciaCrear()
        {
            InitializeComponent();
            viewModel = new PermisoDeAusenciaViewModel();
            DataContext = viewModel; // Enlaza el ViewModel con el DataContext
            CargarUsuarios(); // Cargar los usuarios al inicializar la ventana
			if (SesionUsuario.Instance.rol == "Salonero")
			{

				object IdEmpleado = conexion.getIdEmpleadoFromIdUsuario(SesionUsuario.Instance.id);

				foreach (UsuarioEmpleado item in UsuarioComboBox.Items)
				{
					if (item.Id != null && item.Id.Equals(IdEmpleado))
					{
						UsuarioComboBox.SelectedItem = item;
						break;
					}

				}
				UsuarioComboBox.IsEnabled = false;
			}
		}
        private void CargarUsuarios()
        {
            // Llama al método dropdownUsuarios del ViewModel para obtener la lista de usuarios
            Conexion conexion = new Conexion();
            usuarios = conexion.DropdownUsuarios();

            // Configura el ComboBox con la lista de usuarios
            UsuarioComboBox.ItemsSource = usuarios;
            UsuarioComboBox.DisplayMemberPath = "Display"; // Campo para mostrar en el ComboBox
            UsuarioComboBox.SelectedValuePath = "Id"; // Campo que se usará como valor
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

            if (fechaInicio == null || fechaFin == null || string.IsNullOrWhiteSpace(motivo))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (fechaFin < fechaInicio)
            {
                MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener el Id del usuario seleccionado
            int idEmpleado = (int)UsuarioComboBox.SelectedValue;

            // Llamar al método de creación en el ViewModel con los parámetros correctos
            bool resultado = viewModel.CrearPermisoAusencia(idEmpleado, fechaInicio.Value, fechaFin.Value, motivo);

            if (resultado)
            {
                MessageBox.Show("Permiso de ausencia creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close(); // Cerrar la ventana después de crear el permiso
            }
            else
            {
                MessageBox.Show("Hubo un error al crear el permiso de ausencia. Inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana sin hacer cambios
            DialogResult = false;
            this.Close();
        }
    }
}
