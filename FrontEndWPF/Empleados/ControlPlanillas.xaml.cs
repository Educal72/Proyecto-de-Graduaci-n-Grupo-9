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
using static FrontEndWPF.Modelos.UserModel;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para ControlPlanillas.xaml
    /// </summary>
    public partial class ControlPlanillas : UserControl
    {
        /*
         * Instancias para poder usar los procedimientos almacenados -
         * ubicados en los métodos de las clases: Conexión y ConexiónEmpleado.
         */
        Conexion conexion = new Conexion();
        ConexionEmpleado conexionEmpleado = new ConexionEmpleado();


        public ControlPlanillas()
        {
            InitializeComponent();
            conexion.OpenConnection();
            LoadData();
        }

        private void LoadData()
        {
            string query = @"SELECT Nombre, Apellido, Cedula, Puesto, Correo, FechaCreacion, Salario
                             FROM ControlPlanillas";

            List<ControlPlanilla> planillas = new List<ControlPlanilla>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        planillas.Add(new ControlPlanilla
                        {
                            Nombre = reader["Nombre"].ToString()!,
                            Apellidos = reader["Apellido"].ToString()!,                            
                            Cedula = reader["Cedula"].ToString()!,
                            Puesto = reader["Puesto"].ToString()!,
                            Correo = reader["Correo"].ToString()!,
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            Salario = Convert.ToDecimal(reader["Salario"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            PlanillaDataGrid.ItemsSource = planillas;
        }


        private void Button_Añadir(object sender, RoutedEventArgs e)
        {
            var nuevaPlanilla = new añadirPlanilla();
            nuevaPlanilla.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (nuevaPlanilla.ShowDialog() == true)
            {
                string Nombre = nuevaPlanilla.nombre;
                string Apellidos = nuevaPlanilla.apellidos;
                string Cedula = nuevaPlanilla.cedula;
                string Puesto = nuevaPlanilla.puesto;
                string Correo = nuevaPlanilla.correo;
                DateTime Fecha = nuevaPlanilla.fechacreacion;
                double Salario = nuevaPlanilla.salario;

                var ResultadoPlanilla = conexionEmpleado.AgregarPlanilla(Nombre, Apellidos, Cedula, Puesto, Correo, Fecha, Salario);
                if (ResultadoPlanilla)
                {
                    MessageBox.Show("Se inserto correctamente la planilla, muchas gracias.",
                               "¡Inserción exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Hubo un error en la inserción, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadData();
            }
        }

        private void Button_Editar(object sender, RoutedEventArgs e)
        {
            ControlPlanilla? selectedPlanilla =
                PlanillaDataGrid.SelectedItem as ControlPlanilla;

            if (selectedPlanilla != null)
            {
                string oldCorreo = selectedPlanilla.Correo!;
                string oldCedula = selectedPlanilla.Cedula!;
                var editarPlanilla = new editarPlanilla();
                editarPlanilla.WindowStartupLocation =
                WindowStartupLocation.CenterScreen;

                editarPlanilla.Nombre.Text = selectedPlanilla.Nombre;
                editarPlanilla.Apellidos.Text = selectedPlanilla.Apellidos;                
                editarPlanilla.Cedula.Text = selectedPlanilla.Cedula;
                editarPlanilla.Puesto.Text = selectedPlanilla.Puesto;
                editarPlanilla.Correo.Text = selectedPlanilla.Correo;
                editarPlanilla.Fecha.SelectedDate = selectedPlanilla.FechaCreacion;
                editarPlanilla.Salario.Text = selectedPlanilla.Salario.ToString();
                

                if (editarPlanilla.ShowDialog() == true)
                {
                    selectedPlanilla.Cedula = editarPlanilla.cedula;
                    selectedPlanilla.Nombre = editarPlanilla.nombre;
                    selectedPlanilla.Apellidos = editarPlanilla.apellidos;                
                    selectedPlanilla.Puesto = editarPlanilla.puesto;
                    selectedPlanilla.FechaCreacion = editarPlanilla.fechacreacion;
                    selectedPlanilla.Salario = Convert.ToDecimal(editarPlanilla.salario);
                    selectedPlanilla.Correo = editarPlanilla.correo;


                    PlanillaDataGrid.Items.Refresh();
                   var ResultadoPlanilla = conexionEmpleado.ActualizarPlanilla(selectedPlanilla.Nombre, selectedPlanilla.Apellidos,
                        selectedPlanilla.Cedula, selectedPlanilla.Puesto, selectedPlanilla.Correo,
                        selectedPlanilla.FechaCreacion, selectedPlanilla.Salario, oldCorreo, oldCedula);
                    
                    if (ResultadoPlanilla)
                    {
                        MessageBox.Show("Se actualizo correctamente la planilla, muchas gracias.",
                                   "¡Actualización exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error en la actualización, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void Button_Eliminar(object sender, RoutedEventArgs e)
        {

            ControlPlanilla? selectedPlanilla =
                PlanillaDataGrid.SelectedItem as ControlPlanilla;


            if (selectedPlanilla != null)
            {
                string identificaCorreo = selectedPlanilla.Correo;
                string identificaCedula = selectedPlanilla.Cedula;

                var eliminarPlanilla = new eliminarPlanilla();
                eliminarPlanilla.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                eliminarPlanilla.Nombre.Text = selectedPlanilla.Nombre;
                eliminarPlanilla.Apellidos.Text = selectedPlanilla.Apellidos;       
                eliminarPlanilla.Cedula.Text = selectedPlanilla.Cedula;
                eliminarPlanilla.Puesto.Text = selectedPlanilla.Puesto;
                eliminarPlanilla.Correo.Text = selectedPlanilla.Correo;
                eliminarPlanilla.Fecha.SelectedDate = selectedPlanilla.FechaCreacion;
                eliminarPlanilla.Salario.Text = selectedPlanilla.Salario.ToString();
                
                if (eliminarPlanilla.ShowDialog() == true)
                {
                    PlanillaDataGrid.Items.Refresh();
                    var ResultadoPlanilla =  conexionEmpleado.EliminarPlanilla(identificaCorreo, identificaCedula);
                    if (ResultadoPlanilla)
                    {
                        MessageBox.Show("Se elimino correctamente la planilla, muchas gracias.",
                                   "¡Eliminación exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error en la eliminación de la planilla, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }

            }
        }
    }
}
