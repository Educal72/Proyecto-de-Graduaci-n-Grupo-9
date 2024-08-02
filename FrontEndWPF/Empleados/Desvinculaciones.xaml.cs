using FrontEndWPF.Inventario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Lógica de interacción para Desvinculaciones.xaml
    /// </summary>
    public partial class Desvinculaciones : UserControl
    {
        /* Intancias a las clases: Conexion, ConexionEmpleado, -
         * empleadosAdmin y empleadosNoAdmin.*/
        Conexion conexion = new Conexion();
        ConexionEmpleado conexionEmpleado = new ConexionEmpleado();
        empleadosAdmin empleadosAdmin = new empleadosAdmin();
        empleadosNoAdmin empleadosNoAdmin = new empleadosNoAdmin();


        //Variables globales estaticas.
        static bool SeProsigue;
        static int permisoLeer;
        static int permisoCrear;
        static int permisoEliminar;


        //Variable tipo MessageBoxResult.
        static MessageBoxResult result;


        //Constructor vacio:
        public Desvinculaciones()
        {
            empleadosAdmin.Enviar(false);
            empleadosNoAdmin.Enviar(false);
            InitializeComponent();
            conexion.OpenConnection();
            LoadDataDesv();
        }


        /* Método para poder leer y mostrar los datos de desvinculación.*/
        private void LoadDataDesv()
        {
            /* [1.1]
             * Aqui es para poder saber si el usuario con el rol de empleado -
             * (ya sea cajero, salonero o contador) o administrador tienen los -
             * permisos para poder leer los datos de las desvinculaciones. */
            permisoLeer = conexionEmpleado.PermisoLeer();

            if (permisoLeer == 1)
            {
                /* Aqui se trae los datos para poder mostrar la desvinculación, -
                 * esto por medio de un procedimiento almacenado llamado: -
                 * LeerSolicitudDesvinculacion */
                string query = @"Exec LeerSolicitudDesvinculacion";
                
                List<EmpleadoDesvinculacion> desvinculacionEmpleados = new List<EmpleadoDesvinculacion>();

                using (SqlConnection connection = conexion.OpenConnection())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            desvinculacionEmpleados.Add(new EmpleadoDesvinculacion
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString() + " " + reader["Apellido"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                Motivo = reader["Motivo"].ToString()!,
                                Comentarios = reader["Comentarios"].ToString(),
                                FechaSalida = Convert.ToDateTime(reader["FechaSalida"]),
                                Reconocido = reader["Reconocido"] == DBNull.Value ? false : Convert.ToBoolean(reader["Reconocido"])
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("La lectura de las solicitudes de desvinculación no se pudo completar en este momento debido a un problema técnico." +
                            "\nIntentelo de nuevo más tarde, ó, puede contactar al soporte técnico si el problema persiste, muchas gracias.", "¡Error!: " + ex.Message,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        empleadosAdmin.Enviar(true);
                        empleadosNoAdmin.Enviar(true);
                    }

                }

                DesvinculacionesDataGrid.ItemsSource = desvinculacionEmpleados;
            }
            else
            {
                MessageBox.Show("La acción no está permitida debido a la falta de autorización." +
                    "\nIntentelo de nuevo más tarde, o puede contactar al soporte técnico si el problema persiste, muchas gracias.", "¡Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                empleadosAdmin.Enviar(true);
                empleadosNoAdmin.Enviar(true);
            }

        }


        /* Este método sirve para poder guardar la respuesta que esta en -
         * la variable: "dato", esto porque el resultado, determinara si se -
         * presenta el MessageBox que contiene el mensaje: "Se agrego o -
         * se elimino la desvinculación" en la pantalla final. */
        public void ProseguirDatoGuardado(bool dato)
        {
            if (dato == true)
            {
                SeProsigue = dato;
            }

            if (dato == false)
            {
                SeProsigue = false;
            }

        }


        /* Método para añadir una solicitud de desvinculación. */
        private void Añadir_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            /* Aqui es lo mismo que el permisoLeer, pero adecuado al permiso de -
             * creación de solicitudes de desvinculación.
             * 
             * Nota: Puede ver la explicación en el comentario con esta -
             * indicación: "[1.1]", el cual se encuentra en esta misma clase. */
            permisoCrear = conexionEmpleado.PermisoCrear();
            if (permisoCrear == 1)
            {
                var nuevaDesvinculacion = new añadirDesvinculaciones();
                nuevaDesvinculacion.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                if (nuevaDesvinculacion.ShowDialog() == true)
                {
                    string Nombre = nuevaDesvinculacion.nombre_añadirDesvinculaciones!;
                    string Apellido = nuevaDesvinculacion.apellido_añadirDesvinculaciones!;
                    
                    DateTime FechaInicio = nuevaDesvinculacion.fechaInicio_añadirDesvinculaciones;
                    string Motivo = nuevaDesvinculacion.motivo_añadirDesvinculaciones!;
                    
                    string Comentarios = nuevaDesvinculacion.comentarios_añadirDesvinculaciones!;
                    DateTime FechaSalida = nuevaDesvinculacion.fechaSalida_añadirDesvinculaciones;


                    /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                     * mostrara el mensaje: "La solicitud de desvinculación del empleado -
                     * ha sido agregado exitosamente."*/
                    conexionEmpleado.AddDesvinculacion(Nombre, Apellido, FechaInicio, Motivo, Comentarios, FechaSalida);
                    if (SeProsigue == true)
                    {
                        MessageBox.Show($"La solicitud de desvinculación del empleado: {Nombre + " " + Apellido} ha sido agregado exitosamente." +
                        "\nAhora pasara por un proceso de revisión y procesamiento en el departamento de recursos humanos ó al supervisor encargado, muchas gracias.",
                        "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    LoadDataDesv();
                }
            }
            else
            {
                MessageBox.Show("La acción no está permitida debido a la falta de autorización." +
                    "\nIntentelo de nuevo más tarde, o puede contactar al soporte técnico si el problema persiste, muchas gracias.", "¡Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        /* Método para eliminar una solicitud de desvinculación. */
        private void Eliminar_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            /* Aqui es lo mismo que el permisoLeer, pero adecuado al permiso de -
             * eliminación de solicitudes de desvinculación.
             * 
             * Nota: Puede ver la explicación en el comentario con esta -
             * indicación: "[1.1]", el cual se encuentra en esta misma clase. */
            permisoEliminar = conexionEmpleado.PermisoEliminar();
            if (permisoEliminar == 1)
            {
                EmpleadoDesvinculacion? selectedDesvinculacion = DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;

                /* Aquí consiste en que si la persona tiene seleccionado un dato de la tabla de -
                 * información que quiere eliminar, entonces puede hacer la acción correspondiente - 
                 * (además de ver la información del dato que quiere eliminar), caso contrario -
                 * entonces no dejara hacer la acción. */
                if (selectedDesvinculacion == null)
                {
                    MessageBox.Show("Por favor, seleccione una desvinculación antes de proceder.",
                        "¡Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (selectedDesvinculacion != null)
                {
                    /* Aquí lo que hace es que muestra un mensaje por medio de un MessageBox, -
                     * para luego, si es que el usuario presiona que si quiere eliminar la desvinculación, -
                     * entonces procedera a hacer la eliminación y luego carga de nuevo los datos. */
                    MessageBoxResult result = MessageBox.Show("¿Está seguro de eliminar la información de la solicitud de desvinculación?", "¡Confirmación!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        DesvinculacionesDataGrid.Items.Refresh();
                        conexionEmpleado.DeleteDesvinculaciones(selectedDesvinculacion.Id);

                        /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                         * mostrara el mensaje: "La solicitud de desvinculación, ha sido eliminada -
                         * exitosamente". */
                        if (SeProsigue == true)
                        {
                            MessageBox.Show("La solicitud de desvinculación, ha sido eliminada exitosamente.",
                                "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        LoadDataDesv();
                    }
                }
            }
            else
            {
                MessageBox.Show("La acción no está permitida debido a la falta de autorización." +
                    "\nIntentelo de nuevo más tarde, o puede contactar al soporte técnico si el problema persiste, muchas gracias.", "¡Error!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /* Método que sirve para marcar una solicitud de desvinculación en -
         * el estado: "Reconocido". */
        private void Si_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            /* Aqui lo que se hace es verificar si el usuario con rol de empleado o -
             * administrador, estan seleccionando una solicitud de desvinculación en -
             * especifico.*/
            var selectedItem = DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;
            
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Desea reconocer al empleado?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                /* Aqui lo que hace es que si el usuario (empleado o administrador) dice que si a la -
                 * opción del MessageBox, entonces realizara el proceso para marcar el estado como: "Reconocido". */
                if (result == MessageBoxResult.Yes && selectedItem != null)
                {
                    bool Reconocido = selectedItem.Reconocido = true;
                    int IdDesvinculacion = selectedItem.Id;

                    /* Aqui manda al método que esta encargado como modelo hacia la BD, el id de la -
                     * desvinculación seleccionada y si el estado, que en este caso es: "Reconocido".
                     * 
                     * Nota: En la BD y en los métodos correspondientes, Reconocido es interpretado por un: True (o un 1), -
                     * esto se debe a que es de tipo booleano. */
                    conexionEmpleado.ReconocerDesvinculacion(Reconocido, IdDesvinculacion);
                    DesvinculacionesDataGrid.Items.Refresh();
                    MessageBox.Show("La solicitud de desvinculación ha sido reconocido exitosamente.", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Se cancelo la marcación, muchas gracias.", "¡Aviso!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                //Muestra un mensaje de advertencia si no se selecciona ninguna desvinculación.
                MessageBox.Show("Por favor, seleccione una desvinculación antes de proceder.", 
                    "¡Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        /* Método que sirve para marcar una solicitud de desvinculación en -
        * el estado: No reconocido. */
        private void No_Desvinculaciones_Click(object sender, RoutedEventArgs e)
        {
            /* Aqui lo que se hace es verificar si el usuario con rol de empleado o -
             * administrador, estan seleccionando una solicitud de desvinculación en -
             * especifico.*/
            var selectedItem = DesvinculacionesDataGrid.SelectedItem as EmpleadoDesvinculacion;
            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de marcar este empleado como no reconocido?", "¡Confirmación!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                /* Aqui lo que hace es que si el usuario (empleado o administrador) dice que si a la -
                 * opción del MessageBox, entonces realizara el proceso para marcar el estado como: "No reconocido". */
                if (result == MessageBoxResult.Yes && selectedItem != null)
                {
                    bool Reconocido = selectedItem.Reconocido = false;
                    int IdDesvinculacion = selectedItem.Id;


                    /* Aqui manda al método que esta encargado como modelo hacia la BD, el id de la -
                     * desvinculación seleccionada y si el estado, que en este caso es: "No reconocido".
                     * 
                     * Nota: En la BD y en los métodos correspondientes, No reconocido es interpretado -
                     * por un: False (o un 0), esto se debe a que es de tipo booleano. */
                    conexionEmpleado.ReconocerDesvinculacion(Reconocido, IdDesvinculacion);
                    DesvinculacionesDataGrid.Items.Refresh();
                    MessageBox.Show("La solicitud de desvinculación no fue reconocido.", "¡Confirmación!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Se cancelo la marcación, muchas gracias.", "¡Aviso!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            else
            {
                //Muestra un mensaje de advertencia si no se selecciona ninguna desvinculación.
                MessageBox.Show("Por favor, seleccione una desvinculación antes de proceder.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



    }
}
