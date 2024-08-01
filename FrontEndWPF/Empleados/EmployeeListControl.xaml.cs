using FrontEndWPF.Empleados;
using FrontEndWPF.Index;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.Primitives;
using static FrontEndWPF.empleadosAdmin;
using static FrontEndWPF.Modelos.UserModel;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Application = System.Windows.Application;

namespace FrontEndWPF
{

    /// <summary>
    /// Lógica de interacción para EmployeeListControl.xaml
    /// </summary>
    public partial class EmployeeListControl : UserControl
    {
        //Intancias a las clases: Conexion, ConexionEmpleado y empleadosAdmin.
        Conexion conexion = new Conexion();
        ConexionEmpleado conexionEmpleado = new ConexionEmpleado();
        empleadosAdmin empleadosAdmin = new empleadosAdmin();

        //Lista global para los permisos.
        static List<PermisosAutorizacion>? listaPermisos;

        //Variable global estatica.
        static bool SeProsigue;
        static bool Permiso1;
        static bool Permiso2;
        static int Id;
        static bool LeerPermiso;
        static bool CrearPermiso;
        static bool EliminarPermiso;

        //Variable tipo MessageBoxResult.
        static MessageBoxResult result;


        //Constructor vacio.
        public EmployeeListControl()
        {
            empleadosAdmin.Enviar(false);
            InitializeComponent();
            conexion.OpenConnection();
            LoadData();
        }


        /* 0.1 */
        private void LoadData()
		{
            /*
			* Aquí se agrego la dirección y el activo, la dirección -
			* fue porque uno de los escenarios que presenta la historia: CTE001 -
			* lo menciona, y el activo se agrego porque cuando se agrega un nuevo empleado -
			* se necesita saber si esta laborando en el negocio, cosa que debería estarlo -
			* porque se esta agregando al sistema, por ende, debe estar activo al inicio.
			*/
            string query = @"
                SELECT u.Id, u.Nombre, u.PrimerApellido, u.SegundoApellido, u.Cedula, u.Telefono, u.Correo, u.Contraseña, u.IdRol, u.FechaCreacion, e.Puesto, e.Salario, e.Direccion, e.Activo
                FROM Usuario u
                JOIN Empleado e ON u.Id = e.IdUsuario";

			List<UsuarioEmpleado> usuariosEmpleados = new List<UsuarioEmpleado>();

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						usuariosEmpleados.Add(new UsuarioEmpleado
						{
							Id = Convert.ToInt32(reader["Id"]),
							Nombre = reader["Nombre"].ToString(),
							PrimerApellido = reader["PrimerApellido"].ToString(),
							SegundoApellido = reader["SegundoApellido"].ToString(),
							Cedula = reader["Cedula"].ToString(),
							Telefono = reader["Telefono"].ToString(),
							Correo = reader["Correo"].ToString(),
							Contraseña = reader["Contraseña"].ToString(),
							IdRol = Convert.ToInt32(reader["IdRol"]),
							NombreRol = conexion.getRoleName(Convert.ToInt32(reader["IdRol"])),
							FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
							Puesto = reader["Puesto"].ToString(),
							Salario = Convert.ToDecimal(reader["Salario"]),
                            Direccion = reader["Direccion"].ToString(),
                            Activo = Convert.ToBoolean(reader["Activo"])
                        });
						
	}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}

			EmployeeDataGrid.ItemsSource = usuariosEmpleados;
		}


        /* Aqui lo que hace es cargar todos los permisos de la BD hacia la entidad.
         * 
         * Luego que hace la carga de datos hacia la entidad, esta va a hacer almacenada -
         * en otra lista, el cual es global y se llama: "listaPermisos", pero antes de que -
         * guarde los datos en esa lista se hace un proceso de condición. Para eso, en C#, -
         * ofrece una sentencia exclusiva en las listas, el cual se llama: "FindAll()", esto -
         * lo que hace es que devuelve todos los elementos que coincida con el predicado Lambda -
         * colocado, ahora, investigando en internet, Lambda funciona para hacer condiciones -
         * compactas en métodos como Find, FindAll, Where, etc de forma precisa y consisa, por -
         * lo que es una herramienta muy poderosa para hacer diferentes acciones que uno desea en -
         * una lista.
         * 
         * Con esto mencionado, entonces se coloca una variable llamada: "itemLista", el cual funge -
         * como un parámetro (no necesariamente tiene que tener este nombre que le coloque, puede ser otro), -
         * vealo como los que se colocan en un foreach, for, etc. Luego se coloca una expresión, -
         * el cual, para este caso, necesitamos traer el IdUsuario que esta asociado a ese elemento -
         * de datos, luego de eso, se coloca una condición, mencionando que si es igual a la variable llamada: -
         * "id" (el cual se esta pasando como parámetro de entrada), entonces que guarde ese dato.
         * 
         * También, la razón por la cual se usa FindAll, es que devuelve todos los elementos relacionados -
         * a la condición que uno le coloque, a comparación de Find, que solo devuelve el primer elemento -
         * y ya, algo similar al ExecuteScalar que ofrece Sql, por lo que nos ayuda bastante a almacenar -
         * los permisos que contiene ese IdUsuario.
         * 
         * Además, la razón por la cual pido por párametro de entrada un id (el cual trae el idUsuario que -
         * esta asociado a los permisos, ver [1.3] que esta en esta misma clase, es porque en la lista -
         * necesitamos saber si ese elemento de datos que esta trayendo (osea, el IdUsuario, LeerDesvinculacion, -
         * etc) es el mismo que esta siendo enviado a este método, porque asi entonces, si es igual, quiere decir -
         * que si son los permisos del empleado que uno selecciono en la visualización de datos, por eso se pide -
         * ese id, para asi, poder evitar traer permisos mezclados o erroneos al empleado que uno esta seleccionando -
         * para agregarle nuevos permisos, ó, modificar sus permisos actuales. */
        private void LoadDataPermisos(int id)
        {
            string query = @"EXEC LeerPermisos";
            List<PermisosAutorizacion> permisosAutorizaciones = new List<PermisosAutorizacion>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        permisosAutorizaciones.Add(new PermisosAutorizacion
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                            LeerDesvinculacion = Convert.ToBoolean(reader["LeerDesvinculacion"]),
                            CrearDesvinculacion = Convert.ToBoolean(reader["CrearDesvinculacion"]),
                            EliminarDesvinculacion = Convert.ToBoolean(reader["EliminarDesvinculacion"])
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un problema con la lectura de los permisos.", "¡Error!: " + ex,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            /* Guarda los permisos y el IdUsuario siempre y cuando sea igual a la -
             * variable: "id" que se pasa como parámetro de entrada. */
            listaPermisos = permisosAutorizaciones.FindAll(itemLista => itemLista.IdUsuario == id);
        }


        /* Este método sirve para poder guardar la respuesta que esta en -
         * la variable: dato, esto porque el resultado, determinara si se -
         * presenta el MessageBox que contiene el mensaje: "Se actualizo o -
         * elimino el empleado exitosamente" en la pantalla final. */
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


        /* Método para poder añadir un nuevo empleado.
		 * 0.2 */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nuevoEmpleado = new añadirEmpleado();
            nuevoEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (nuevoEmpleado.ShowDialog() == true)
            {
                string Cedula = nuevoEmpleado.cedula_añadirEmpleado;
                string Nombre = nuevoEmpleado.nombre_añadirEmpleado;

                string Apellidos = nuevoEmpleado.apellidos_añadirEmpleado;
                string Puesto = nuevoEmpleado.puesto_añadirEmpleado;

                DateTime Fecha = nuevoEmpleado.fechaContratacion_añadirEmpleado;
                double Salario = nuevoEmpleado.salario_añadirEmpleado;

                string Correo = nuevoEmpleado.correo_añadirEmpleado;
                string Contraseña = nuevoEmpleado.contraseña_añadirEmpleado;

                string Telefono = nuevoEmpleado.telefono_añadirEmpleado;
                string Direccion = nuevoEmpleado.direccion_añadirEmpleado;
                string Rol = nuevoEmpleado.rol_añadirEmpleado;

                /* Aquí se manda por parámetro al método llamado: AddUser, -
				 * esto con la finalidad de que pueda ser enviado y registrado a la -
				 * base de datos que esta en SQL Server. */
                conexion.AddUser(Nombre, Apellidos, Cedula, Telefono, Correo, Contraseña, Rol, Fecha, Puesto, Salario, Direccion);
                if (SeProsigue == true)
                {
                    MessageBox.Show("Empleado guardado exitosamente.", "Añadir empleado.",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadData();
            }
        }


        /* Método que sirve para poder editar a un empleado -
         * del sistema (y de la BD). */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /* Aqui pasa lo mismo que en el método enfocado para eliminar -
			 * a un empleado, [VER el comentario: 1.1] */
            UsuarioEmpleadoSoloMostrar? selectedEmpleadoMostrar = EmployeeDataGrid.SelectedItem as UsuarioEmpleadoSoloMostrar;

            /* Aquí tiene un comportamiento similar al del método de eliminar -
			 * [VER el comentario 1.2], salgo con la clara diferencia que aquí -
			 * está enfocado en actualizar (o editar) a un empleado que esta -
			 * siendo seleccionado. */
            if (selectedEmpleadoMostrar == null)
            {
                MessageBox.Show("Por favor, seleccione un empleado antes de proceder.", "¡Advertencia!", MessageBoxButton.OK,
                     MessageBoxImage.Warning);
                return;
            }


            if (selectedEmpleadoMostrar != null)
            {
                MessageBoxResult result =
                    MessageBox.Show("¿Está seguro de actualizar la información del empleado(a)?",
                    "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    /* Aqui pinta los datos del empleado seleccionado, además de guardar el correo y la cédula para -
                     * obtener el id del empleado al cual se esta actualizando. */
                    string oldCorreo = selectedEmpleadoMostrar.Correo!;
                    string oldCedula = selectedEmpleadoMostrar.Cedula!;

                    var editarEmpleado = new editarEmpleado();
                    editarEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    editarEmpleado.Cedula.Text = selectedEmpleadoMostrar.Cedula;
                    editarEmpleado.Nombre.Text = selectedEmpleadoMostrar.Nombre;

                    editarEmpleado.Apellidos.Text = selectedEmpleadoMostrar.Apellido;
                    editarEmpleado.Puesto.Text = selectedEmpleadoMostrar.Puesto;

                    editarEmpleado.Fecha.SelectedDate = selectedEmpleadoMostrar.FechaCreacion;
                    editarEmpleado.Correo.Text = selectedEmpleadoMostrar.Correo;

                    editarEmpleado.Telefono.Text = selectedEmpleadoMostrar.Telefono;
                    editarEmpleado.Activo.IsChecked = selectedEmpleadoMostrar.Activo;

                    editarEmpleado.RolMuestra.Text = selectedEmpleadoMostrar.Rol;
                    editarEmpleado.Direccion_TextBox.Text = selectedEmpleadoMostrar.Direccion;


                    /* Aqui es donde el usuario pondra los nuevos datos que desee modificar del -
                     * empleado seleccionado.*/
                    if (editarEmpleado.ShowDialog() == true)
                    {
                        selectedEmpleadoMostrar.Cedula = editarEmpleado.cedula_editarEmpleado;
                        selectedEmpleadoMostrar.Nombre = editarEmpleado.nombre_editarEmpleado;

                        selectedEmpleadoMostrar.Apellido = editarEmpleado.apellidos_editarEmpleado;
                        selectedEmpleadoMostrar.Puesto = editarEmpleado.puesto_editarEmpleado;

                        selectedEmpleadoMostrar.FechaCreacion = editarEmpleado.fechaContratacion_editarEmpleado;
                        selectedEmpleadoMostrar.Correo = editarEmpleado.correo_editarEmpleado;

                        selectedEmpleadoMostrar.Telefono = editarEmpleado.telefono_editarEmpleado;
                        selectedEmpleadoMostrar.Activo = editarEmpleado.activo_editarEmpleado;

                        selectedEmpleadoMostrar.Rol = editarEmpleado.rol_editarEmpleado;
                        selectedEmpleadoMostrar.Direccion = editarEmpleado.direccion_editarEmpleado;


                        /* Refresca los campos y los manda al método correspondiente -
                         * para ser procesado a la BD, luego de eso, procede nuevamente -
                         * a cargar los datos de los empleados.*/
                        EmployeeDataGrid.Items.Refresh();
                        conexionEmpleado.UpdateEmployee(selectedEmpleadoMostrar.Cedula, selectedEmpleadoMostrar.Nombre,
                            selectedEmpleadoMostrar.Apellido!, selectedEmpleadoMostrar.Puesto, selectedEmpleadoMostrar.FechaCreacion, selectedEmpleadoMostrar.Correo,
                            selectedEmpleadoMostrar.Telefono, selectedEmpleadoMostrar.Activo, selectedEmpleadoMostrar.Rol, selectedEmpleadoMostrar.Direccion, oldCedula, oldCorreo);


                        /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                         * mostrara el mensaje: "Empleado actualizado exitosamente."*/
                        if (SeProsigue == true)
                        {
                            MessageBox.Show("Empleado actualizado exitosamente.", "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        LoadData();
                    }
                }
            }
        }



        /* Método que sirve para poder eliminar de manera -
		 * permanente un empleado del sistema (y de la BD).*/
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /* [1.1]
			 * Se hace una instancia a la clase: UsuarioEmpleadoSoloMostrar, -
			 * esto se debe a que en dicha clase es donde se están -
			 * guardando los datos ingresados, además que es en dicha -
			 * clase que el sistema puede detectar correctamente los datos.
			 * 
			 * Nota: El EmployeeDataGrid, en Windows WPF, es el que se relaciona -
			 * con los datos que uno coloca en un formulario o campos de datos, -
			 * este puede tener diferentes nombres, por lo que no necesariamente -
			 * va a llamarse: "EmployeeDataGrid". */
            UsuarioEmpleadoSoloMostrar? selectedEmpleadoMostrar = EmployeeDataGrid.SelectedItem as UsuarioEmpleadoSoloMostrar;


            /* [1.2]
			 * Aquí consiste en que si la persona tiene seleccionado un empleado N, que quiere -
			 * eliminar, entonces puede hacer la acción correspondiente (además de ver la infor-
			 * mación del dato que quiere eliminar), caso contrario entonces no dejara hacer la acción. */
            if (selectedEmpleadoMostrar == null)
            {
                MessageBox.Show("Por favor, seleccione un empleado antes de proceder.", "¡Advertencia!", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }


            if (selectedEmpleadoMostrar != null)
            {
                /* Aquí lo que hace es que muestra un mensaje por medio de un MessageBox, -
                 * para luego, si es que el usuario presiona que si quiere eliminar el empleado, -
                 * entonces procedera a eliminar el empleado y luego carga de nuevo los datos.
                 * 
                 * Nota: Aquí se toma el IdRol, ya que dicho Id indica el rol que tiene -
                 * el usuario, el cual (por la historia CTE001 y CTE002) es de un usuario -
                 * con rol de administrador, por eso se pide el rol. */
                MessageBoxResult result = MessageBox.Show("¿Está seguro de eliminar la información del empleado(a)?",
                    "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    EmployeeDataGrid.Items.Refresh();
                    conexionEmpleado.DeleteEmployee(selectedEmpleadoMostrar.Rol!, selectedEmpleadoMostrar.Correo!, selectedEmpleadoMostrar.Cedula!);

                    /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                     * mostrara el mensaje: "Empleado eliminado exitosamente."*/
                    if (SeProsigue == true)
                    {
                        MessageBox.Show("Empleado eliminado exitosamente.", "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    LoadData();
                }
            }
        }


        /* Este método sirve para poder detectar si el usuario quiere -
         * guardar un nuevo permiso inicial, ó, quiere actualizar dichos -
         * permisos */
        public void PermisoValidacion(bool dato1, bool dato2)
        {
            if (dato1 == true)
            {
                Permiso1 = dato1;
                Permiso2 = dato2;
            }

            if (dato2 == true)
            {
                Permiso1 = dato1;
                Permiso2 = dato2;
            }

        }


        /* Este método es para los permisos de los empleados (ya sean adminis -
         * tradores como empleados).
         *
         * Esto se hace para poder evitar que algún empleado (ya sea cajero, salonero -
         * o contador) ó administrador pueda realizar ciertas acciones que son muy delicadas -
         * para el sistema, como pueden la modificación, eliminación o lectura de datos -
         * sensibles, por lo que esto es una buena práctica para poder segmentar las funciones -
         * que tiene cada usuario en el sistema. */
        private void Permisos_Button(object sender, RoutedEventArgs e)
        {
            UsuarioEmpleadoSoloMostrar? selectedEmpleadoMostrar = EmployeeDataGrid.SelectedItem as UsuarioEmpleadoSoloMostrar;


            if (selectedEmpleadoMostrar == null)
            {
                MessageBox.Show("Por favor, seleccione un empleado antes de proceder.",
                    "¡Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (selectedEmpleadoMostrar != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Está seguro de ir a la información de los permisos de autorización del empleado(a)?",
                    "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                /* Aquí lo que hace es que si el usuario administrador presiona que si, -
                 * entonces hara lo demás para los permisos.
                 * 
                 * Dentro de la condición lo que se hace es que primero se presentan los datos: -
                 * Nombre y Cedula del empleado, los cuales mostraran al empleado que uno selecciono -
                 * (estos no se podran modificar, ya que estan configurados como solo lectura desde la -
                 * clase correspondiente, el cual se llama: permisosAutorizacion.xaml), luego, manda -
                 * a un método llamado: LoadDataPermisos, el cual tiene como proposito de cargar todos -
                 * los permisos que se encuentran registrados en la BD, pero para que lo haga bien, se -
                 * le manda un dato entero, el cual, gracias al método: "GetIdPermisosAutorizacion", -
                 * obtendremos el IdUsuario para poder traer los permisos correspondientes al empleado -
                 * que uno selecciono con anterioridad.
                 * 
                 * Siguiendo con la explicación, después de eso se hace un foreach, esto para poder -
                 * recorrer toda la lista que tiene almacenado los permisos, luego de eso, los almacenan -
                 * en variables globales del mismo tipo (los cuales son booleanos) para que se puedan -
                 * presentar en la pantalla, eso es gracias a los comandos:
                 *     -> permisosAutorizacion.Permiso_Leer_Desvinculacion.IsChecked = LeerPermiso;
                 *     -> permisosAutorizacion.Permiso_Crear_Desvinculacion.IsChecked = CrearPermiso;
                 *     -> permisosAutorizacion.Permiso_Eliminar_Desvinculacion.IsChecked = EliminarPermiso;
                 *     -> Otros más, si es que se necesitaran mostrar.
                 *
                 * Luego de eso, el usuario administrador podra colocarle los permisos que corresponde a -
                 * ese empleado, y si el administrador lo quiere registrar, entonces le da en guardar y -
                 * entra en la primera condición que tiene la variable: "Permiso1", pero si en realidad -
                 * quiere actualizar los permisos, entonces ahi entraria a la segunda condición que tiene -
                 * la variable: "Permiso2" y realiza todo el procedimiento correspondiente para que sea -
                 * enviado a la BD. */
                if (result == MessageBoxResult.Yes)
                {
                    var permisosAutorizacion = new permisosAutorizacion();
                    var entidadP = new PermisosAutorizacion();

                    permisosAutorizacion.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    permisosAutorizacion.Nombre_TextBox.Text = selectedEmpleadoMostrar.Nombre;
                    permisosAutorizacion.Cedula_TextBox.Text = selectedEmpleadoMostrar.Cedula;

                    
                    /* [1.2] */
                    LoadDataPermisos(conexionEmpleado.GetIdPermisosAutorizacion(selectedEmpleadoMostrar.Nombre!,
                        selectedEmpleadoMostrar.Cedula!));
                    

                    foreach (var cantidadPermisos in listaPermisos!)
                    {
                        LeerPermiso = cantidadPermisos.LeerDesvinculacion;
                        CrearPermiso = cantidadPermisos.CrearDesvinculacion;
                        EliminarPermiso = cantidadPermisos.EliminarDesvinculacion;
                    }

                    permisosAutorizacion.Permiso_Leer_Desvinculacion.IsChecked = LeerPermiso;
                    permisosAutorizacion.Permiso_Crear_Desvinculacion.IsChecked = CrearPermiso;
                    permisosAutorizacion.Permiso_Eliminar_Desvinculacion.IsChecked = EliminarPermiso;


                    /* Aqui es donde el usuario pondra los permisos que desee -
                     * para el empleado seleccionado.*/
                    if (permisosAutorizacion.ShowDialog() == true)
                    {
                        LeerPermiso = permisosAutorizacion.desvinculacion_permiso_leer_permisosAutorizacion;
                        CrearPermiso = permisosAutorizacion.desvinculacion_permiso_crear_permisosAutorizacion;
                        EliminarPermiso = permisosAutorizacion.desvinculacion_permiso_eliminar_permisosAutorizacion;


                        if (Permiso1 == true)
                        {
                            EmployeeDataGrid.Items.Refresh();
                            conexionEmpleado.RegistrarPermisoAutorizacion(selectedEmpleadoMostrar.Nombre!, selectedEmpleadoMostrar.Cedula!,
                                LeerPermiso, CrearPermiso, EliminarPermiso);


                            /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                             * mostrara el mensaje: "Permisos asignados exitosamente."*/
                            if (SeProsigue == true)
                            {
                                MessageBox.Show("Permisos asignados exitosamente.", "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            LoadData();

                        }
                        else if (Permiso2 == true)
                        {
                            EmployeeDataGrid.Items.Refresh();
                            conexionEmpleado.ActualizarPermisoAutorizacion(selectedEmpleadoMostrar.Nombre!, selectedEmpleadoMostrar.Cedula!,
                                LeerPermiso, CrearPermiso, EliminarPermiso);


                            /* Si todo salio bien en el proceso del método encargado de la BD, entonces -
                             * mostrara el mensaje: "Permisos acualizados exitosamente."*/
                            if (SeProsigue == true)
                            {
                                MessageBox.Show("Permisos acualizados exitosamente.", "¡Confirmación!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            LoadData();
                        }


                     }
                 }
            }

		}

		/* 
		 * Este método es para la historia laboral del empleado.
		 */
		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			var selectedEmpleado = EmployeeDataGrid.SelectedItem as UsuarioEmpleado;
			if (selectedEmpleado != null)
			{
				MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
				if (mainWindow != null)
				{
					mainWindow.ChangePageToMetricas(selectedEmpleado.Id);
				}

			}
		}
	}
	}


