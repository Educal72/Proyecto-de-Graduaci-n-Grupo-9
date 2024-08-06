using FrontEndWPF.Empleados;
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

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para PerfileComp.xaml
    /// </summary>
    public partial class PerfilesComp : UserControl
    {
        //Instancias a las clases: Conexion y ConexionEmpleado.
        Conexion conexion = new Conexion();
        ConexionEmpleado conexionEmpleado = new ConexionEmpleado();


        public PerfilesComp()
        {
            InitializeComponent();
            LoadData();
        }


        /* 
         * Método que está encargado de mostrar en pantalla -
         * los datos de los perfiles competenciales. */
        private void LoadData()
        {
            string query = @"EXEC LeerPerfilesCompetenciales";

            List<PerfilCompentencial> perfilesCompetenciales = new List<PerfilCompentencial>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        perfilesCompetenciales.Add(new PerfilCompentencial
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Titulo = reader["Titulo"].ToString()!,
                            Descripcion = reader["Descripcion"].ToString()!,
                            Experiencia = reader["Experiencia"].ToString()!,
                            Requisitos = reader["Requisitos"].ToString()!,
                            Ubicacion = reader["Ubicacion"].ToString()!,
                            Salario = Convert.ToDouble(reader["Salario"].ToString())!
                            /* 
                             * Nota:
                             *      Se coloco un ! en cada comando ToString(), esto para -
                             *      poder aceptar si los campos llegaran a ser nulos.
                             */
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            PerfilesDataGrid.ItemsSource = perfilesCompetenciales;
        }


        /* Método que esta relacionado al botón de: Añadir.
		 * Este lo que se enfocara hacer es que va a crear -
		 * los perfiles competenciales que coloca el usuario -
		 * administrador. */
        private void Añadir_Click(object sender, RoutedEventArgs e)
        {
            var nuevoperfilCompetencial = new añadirPerfilCompetencial();
            nuevoperfilCompetencial.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (nuevoperfilCompetencial.ShowDialog() == true)
            {                
                string Titulo = nuevoperfilCompetencial.titulo_añadirPerfilCompetencial;
                string Descripcion = nuevoperfilCompetencial.descripcion_añadirPerfilCompetencial;
                string Experiencia = nuevoperfilCompetencial.experiencia_añadirPerfilCompetencial;
                string Requisitos = nuevoperfilCompetencial.requisitos_añadirPerfilCompetencial;
                string Ubicacion = nuevoperfilCompetencial.ubicacion_añadirPerfilCompetencial;
                double Salario = nuevoperfilCompetencial.salario_añadirPerfilCompetencial;

                /*
                 * Aquí se manda por parámetro al método llamado: AgregarPerfilCompetencial, -
                 * esto con la finalidad de que pueda ser enviado y registrado a la -
                 * base de datos que esta en SQL Server.
                 */
                conexionEmpleado.AgregarPerfilCompetencial(Titulo, Descripcion, Experiencia, 
                    Requisitos, Ubicacion, Salario);
                LoadData();
            }
        }


        /* Método que esta relacionado al botón de: Editar.
		 * Este lo que se enfocara hacer es que va a editar -
		 * los perfiles competenciales que seleccione el usuario -
		 * administrador. */
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            PerfilCompentencial? selectedPerfilCompetencial = PerfilesDataGrid.SelectedItem as PerfilCompentencial;

            if (selectedPerfilCompetencial != null)
            {
                var editarPerfilCompetencial = new editarPerfilCompetencial();
                editarPerfilCompetencial.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                editarPerfilCompetencial.Titulo_TextBox.Text = selectedPerfilCompetencial.Titulo;
                editarPerfilCompetencial.Descripcion_TextBox.Text = selectedPerfilCompetencial.Descripcion;
                editarPerfilCompetencial.Experiencia_TextBox.Text = selectedPerfilCompetencial.Experiencia;
                editarPerfilCompetencial.Requisitos_TextBox.Text = selectedPerfilCompetencial.Requisitos;
                editarPerfilCompetencial.Ubicacion_TextBox.Text = selectedPerfilCompetencial.Ubicacion;
                editarPerfilCompetencial.Salario_TextBox.Text = selectedPerfilCompetencial.Salario.ToString();

                if (editarPerfilCompetencial.ShowDialog() == true)
                {
                    selectedPerfilCompetencial.Titulo = editarPerfilCompetencial.titulo_editarPerfilCompetencial;
                    selectedPerfilCompetencial.Descripcion = editarPerfilCompetencial.descripcion_editarPerfilCompetencial;
                    selectedPerfilCompetencial.Experiencia = editarPerfilCompetencial.experiencia_editarPerfilCompetencial;
                    selectedPerfilCompetencial.Requisitos = editarPerfilCompetencial.requisitos_editarPerfilCompetencial;
                    selectedPerfilCompetencial.Ubicacion = editarPerfilCompetencial.ubicacion_editarPerfilCompetencial;
                    selectedPerfilCompetencial.Salario = editarPerfilCompetencial.salario_editarPerfilCompetencial;

                    PerfilesDataGrid.Items.Refresh();

                    var ResultadoPerfilCompetencial = conexionEmpleado.ActualizarPerfilCompetencial(selectedPerfilCompetencial.Id, selectedPerfilCompetencial.Titulo, selectedPerfilCompetencial.Descripcion,
                        selectedPerfilCompetencial.Experiencia, selectedPerfilCompetencial.Requisitos, selectedPerfilCompetencial.Ubicacion, selectedPerfilCompetencial.Salario);

                    if (ResultadoPerfilCompetencial)
                    {
                        MessageBox.Show("Se actualizo correctamente el perfil competencial, muchas gracias.",
                                   "¡Actualización exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error en la actualización, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}


        /* Método que esta relacionado al botón de: Eliminar.
		 * Este lo que se enfocara hacer es que va a eliminar -
		 * los perfiles competenciales que seleccione el usuario -
		 * administrador. */
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            PerfilCompentencial? selectedPerfilCompetencial = PerfilesDataGrid.SelectedItem as PerfilCompentencial;

            if (selectedPerfilCompetencial != null)
            {
                var result = MessageBox.Show("¿Está seguro de que desea eliminar este perfil?",
								   "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) { 
                        var ResultadoPerfilCompetencial = 
                        conexionEmpleado.EliminarPerfilCompetencial(selectedPerfilCompetencial.Id);

                    if (ResultadoPerfilCompetencial)
                    {
                        MessageBox.Show("Se elimino correctamente el perfil competencial, muchas gracias.",
                                   "¡Eliminación exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error en la eliminación del perfil competencial, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
				
                }
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}

		}
        }
    }


