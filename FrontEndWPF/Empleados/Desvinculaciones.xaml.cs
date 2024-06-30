using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FrontEndWPF.empleadosAdmin;
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF.Empleados
{
    /// <summary>
    /// Lógica de interacción para Desvinculaciones.xaml
    /// </summary>
    public partial class Desvinculaciones : UserControl
    {
        Conexion conexion = new Conexion();
        public Desvinculaciones()
        {
            InitializeComponent();
            conexion.OpenConnection();
            LoadDataDesv();

        }

        /* 
         * Método para poder leer y mostrar los datos de desvinculación.
         */
        private void LoadDataDesv()
        {
            /*
             * Aqui se trae los datos para poder mostrar la desvinculación, -
             * esto por medio de dos INNER JOINS a las tablas: Desvinculacion, -
             * Empleado y Usuario.
             */
            string query = @"
            SELECT 
                Desv.Id,
			    Desv.IdEmpleado,
			    CONCAT(Us.Nombre, ' ', Us.PrimerApellido)  as 'Empleado',
			    Desv.FechaInicio,
			    Desv.Motivo,
			    Desv.Comentarios,
			    Desv.FechaSalida,
			    Desv.Reconocido

	        FROM
		        Desvinculacion Desv INNER JOIN Empleado Emp 
		        ON Desv.IdEmpleado = Emp.Id 
		        INNER JOIN Usuario Us 
		        ON Emp.IdUsuario = Us.Id";

            List<EmpleadoDesvinculacion> usuariosEmpleados = new List<EmpleadoDesvinculacion>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        usuariosEmpleados.Add(new EmpleadoDesvinculacion
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Empleado = reader["Empleado"].ToString(),
                            FechaInicio = reader["FechaInicio"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FechaInicio"]),
                            Motivo = reader["Motivo"].ToString(),
                            Comentarios = reader["Comentarios"].ToString(),
                            FechaSalida = reader["FechaSalida"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FechaSalida"]),
                            Reconocido = reader["Reconocido"] == DBNull.Value ? false : Convert.ToBoolean(reader["Reconocido"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            DesvinculacionesDataGrid.ItemsSource = usuariosEmpleados;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de marcar este empleado como no reconocido?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Marcar al empleado como no reconocido
                    selectedItem.Reconocido = false;
                    DesvinculacionesDataGrid.Items.Refresh();
                    MessageBox.Show("Empleado marcado como no reconocido.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Si el usuario responde No, no se realiza ninguna acción adicional
            }
            else
            {
                // Mostrar un mensaje de advertencia si no se selecciona ningún empleado
                MessageBox.Show("Por favor, seleccione un empleado antes de proceder.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedItem = DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Desea reconocer al empleado?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    selectedItem.Reconocido = true;
                    DesvinculacionesDataGrid.Items.Refresh();
                    MessageBox.Show("Empleado reconocido exitosamente.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // No se necesita hacer nada si el usuario selecciona "No"
            }
            else
            {
                // Mostrar un mensaje de advertencia si no se selecciona ningún empleado
                MessageBox.Show("Por favor, seleccione un empleado antes de proceder.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /* 
         * Método para añadir una solicitud de desvinculación.
         */
        private void Añadir_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            var nuevaDesvinculacion = new añadirDesvinculaciones();
            nuevaDesvinculacion.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (nuevaDesvinculacion.ShowDialog() == true)
            {
                DateTime FechaInicio = nuevaDesvinculacion.fechaInicio;
                string Motivo = nuevaDesvinculacion.motivo;
                string Comentarios = nuevaDesvinculacion.comentarios;
                DateTime FechaSalida = nuevaDesvinculacion.fechaSalida;

                conexion.AddDesvinculacion(FechaInicio, Motivo, Comentarios, FechaSalida);
                LoadDataDesv();
            }
        }


        /* 
         * Método para eliminar una solicitud de desvinculación.
         */
        private void Eliminar_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            EmpleadoDesvinculacion? selectedDesvinculacion =
                DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;

            if (selectedDesvinculacion != null)
            {
                var eliminarDesvinculacion = new eliminarDesvinculacionesxaml();
                eliminarDesvinculacion.WindowStartupLocation =
                    WindowStartupLocation.CenterScreen;

                eliminarDesvinculacion.EmpleadoTextBox.Text = selectedDesvinculacion.Empleado;
                eliminarDesvinculacion.FechaInicioPicker.SelectedDate = selectedDesvinculacion.FechaInicio;
                eliminarDesvinculacion.MotivoTextbox.Text = selectedDesvinculacion.Motivo;
                eliminarDesvinculacion.ComentariosTextbox.Text = selectedDesvinculacion.Comentarios;
                eliminarDesvinculacion.FechaSalidaPicker.SelectedDate = selectedDesvinculacion.FechaSalida;

                if (eliminarDesvinculacion.ShowDialog() == true)
                {
                    DesvinculacionesDataGrid.Items.Refresh();
                    conexion.DeleteDesvinculaciones(selectedDesvinculacion.Id);
                    LoadDataDesv();
                }
            }
        }

		
    }
}
