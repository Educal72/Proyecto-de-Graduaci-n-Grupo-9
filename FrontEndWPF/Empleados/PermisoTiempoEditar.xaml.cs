using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FrontEndWPF.Modelos;
using FrontEndWPF.ViewModel;
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF
{
    public partial class PermisoTiempoEditar : Window
    {
        private PermisoDeTiempoViewModel viewModel;
        private PermisoDeTiempo permisoActual; // Permiso actual que se está editando

        public PermisoTiempoEditar(PermisoDeTiempo permiso)
        {
            InitializeComponent();
            viewModel = new PermisoDeTiempoViewModel();
            DataContext = viewModel; // Enlaza el ViewModel con el DataContext
            permisoActual = permiso;

            // Inicializa los campos de la ventana con los datos del permiso actual
            InicializarCampos();

            // Deshabilita el ComboBox si el ComboBox no es necesario para editar el empleado
            UsuarioComboBox.IsEnabled = false;
        }

        private void InicializarCampos()
        {
            // Configurar los campos del formulario con los datos del permiso actual
            FechaInicioPicker.SelectedDate = permisoActual.FechaInicio;
            FechaFinPicker.SelectedDate = permisoActual.FechaFin;
            MotivoTextBox.Text = permisoActual.Motivo;

            // Configura el ComboBox para mostrar el empleado seleccionado
            // Debes asegurarte de que `Usuarios` esté cargado antes de asignar `SelectedValue`
            UsuarioComboBox.ItemsSource = viewModel.Usuarios;
            UsuarioComboBox.DisplayMemberPath = "Display"; // Campo para mostrar en el ComboBox
            UsuarioComboBox.SelectedValuePath = "Id"; // Campo que se usará como valor
            UsuarioComboBox.SelectedValue = permisoActual.IdEmpleado;
        }

        private void Button_Click_Actualizar(object sender, RoutedEventArgs e)
        {
            // Obtener los datos del formulario
            var fechaInicio = FechaInicioPicker.SelectedDate;
            var fechaFin = FechaFinPicker.SelectedDate;
            var motivo = MotivoTextBox.Text;

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

            // Obtener el Id del usuario seleccionado (debería ser el mismo que el permiso actual)
            int idEmpleado = permisoActual.IdEmpleado;

            // Llamar al método de actualización en el ViewModel con los parámetros correctos
            bool resultado = viewModel.ActualizarPermisoTiempo(idEmpleado, fechaInicio.Value, fechaFin.Value, motivo);

            if (resultado)
            {
                MessageBox.Show("Permiso de tiempo actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                this.Close(); // Cerrar la ventana después de actualizar el permiso
            }
            else
            {
                MessageBox.Show("Hubo un error al actualizar el permiso de tiempo. Inténtelo nuevamente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_Cancelar(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            // Cerrar la ventana sin hacer cambios
            this.Close();
        }
    }
}
