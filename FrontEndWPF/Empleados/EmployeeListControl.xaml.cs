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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
		List<Empleado> employees = new List<Empleado>();
		Conexion conexion = new Conexion();


		public EmployeeListControl()
		{
			InitializeComponent();
			conexion.OpenConnection();
			LoadData();
		}

		/* 
		 * 0.1
		 */
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
                SELECT u.Nombre, u.PrimerApellido, u.SegundoApellido, u.Cedula, u.Telefono, u.Correo, u.Contraseña, u.IdRol, u.FechaCreacion, e.Puesto, e.Salario, e.Direccion, e.Activo
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
							Nombre = reader["Nombre"].ToString(),
							PrimerApellido = reader["PrimerApellido"].ToString(),
							SegundoApellido = reader["SegundoApellido"].ToString(),
							Cedula = reader["Cedula"].ToString(),
							Telefono = reader["Telefono"].ToString(),
							Correo = reader["Correo"].ToString(),
							Contraseña = reader["Contraseña"].ToString(),
							IdRol = Convert.ToInt32(reader["IdRol"]),
							FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
							Puesto = reader["Puesto"].ToString(),
							Salario = ((double)Convert.ToDecimal(reader["Salario"])),
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

		/*
		 * Método para poder añadir un nuevo empleado.
		 * 0.2
		 */
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var nuevoEmpleado = new añadirEmpleado();
			nuevoEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			if (nuevoEmpleado.ShowDialog() == true)
			{

				string Cedula = nuevoEmpleado.cedula;
				string Nombre = nuevoEmpleado.nombre;
				string Apellidos = nuevoEmpleado.apellidos;
				string Puesto = nuevoEmpleado.puesto;
				DateTime Fecha = nuevoEmpleado.fechaContratación;
				double Salario = nuevoEmpleado.salario;
				string Correo = nuevoEmpleado.correo;
				string Contraseña = nuevoEmpleado.contraseña;
				string Telefono = nuevoEmpleado.telefono;
				string Direccion = nuevoEmpleado.direccion;
				string Rol = nuevoEmpleado.rol;

				/*
				 * Aquí se manda por parámetro al método llamado: AddUser, -
				 * esto con la finalidad de que pueda ser enviado y registrado a la -
				 * base de datos que esta en SQL Server.
				 */
				conexion.AddUser(Nombre, Apellidos, Apellidos, Cedula, Telefono, Correo, Contraseña, Rol, Fecha, Puesto, Salario, Direccion);
				LoadData();
			}
		}


		/*
		 * Método que sirve para poder eliminar de manera -
		 * permanente un empleado del sistema (y de la BD).
		 */
		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			/* [1.1]
			 * Se hace una instancia a la clase: UsuarioEmpleado, -
			 * esto se debe a que en dicha clase es donde se están -
			 * guardando los datos ingresados, además que es en dicha -
			 * clase que el sistema (con ayuda del EmployeeDataGrid -
			 * [el cual, en Windows WPF, es el que se relaciona con los -
			 * datos que uno coloca en un formulario o campos de datos]) -
			 * puede dectectar correctamente los datos.
			 */
            UsuarioEmpleado? selectedEmpleado = EmployeeDataGrid.SelectedItem as UsuarioEmpleado;

			/* [1.2]
			 * Aquí consiste en que si la persona tiene seleccionado un dato de la tabla de -
			 * información que quiere eliminar, entonces puede hacer la acción correspondiente - 
			 * (además de ver la información del dato que quiere eliminar), caso contrario -
			 * entonces no dejara hacer la acción.
			 */
            if (selectedEmpleado != null)
			{
				/*
				 * Aquí lo que se hace es ubicar la ventana que fue creada para -
				 * eliminar un empleado, el cual se llama: eliminarEmpleado, cuando -
				 * lo encuentra, entonces lo muestra junto con la información del dato -
				 * del empleado al cual quiere ver.
				 */
				var eliminarEmpleado = new eliminarEmpleado();
                eliminarEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                eliminarEmpleado.Cedula.Text = selectedEmpleado.Cedula;
                eliminarEmpleado.Nombre.Text = selectedEmpleado.Nombre;
                eliminarEmpleado.Apellidos.Text = selectedEmpleado.PrimerApellido;
                eliminarEmpleado.Apellidos.Text = selectedEmpleado.SegundoApellido;
                eliminarEmpleado.Puesto.Text = selectedEmpleado.Puesto;
                eliminarEmpleado.Fecha.SelectedDate = selectedEmpleado.FechaCreacion;
                eliminarEmpleado.Salario.Text = selectedEmpleado.Salario.ToString();
                eliminarEmpleado.Correo.Text = selectedEmpleado.Correo;
                eliminarEmpleado.Contraseña.Text = selectedEmpleado.Contraseña;
                eliminarEmpleado.Telefono.Text = selectedEmpleado.Telefono;
                eliminarEmpleado.Activo.Text = selectedEmpleado.Activo.ToString();
                eliminarEmpleado.RolMuestra.Text = selectedEmpleado.IdRol.ToString();
				if (eliminarEmpleado.ShowDialog() == true)
				{
                    /*
					 * Aquí lo que hace es que muestra los datos en la ventana -
					 * para luego (si es que el usuario presiona la acción respectiva) -
					 * la variable conexion (que es la clase en donde se maneja el envió -
					 * de datos a la BD) manda a llamar el método para poder eliminar el -
					 * empleado.
					 * 
					 * Nota: Aquí se toma el IdRol, ya que dicho Id indica el rol que tiene -
					 * el usuario, el cual (por la historia CTE001 y CTE002) es de un usuario -
					 * con rol de administrador, por eso se pide el rol.
					 */
                    EmployeeDataGrid.Items.Refresh();
					conexion.DeleteEmployee(selectedEmpleado.IdRol);
					LoadData();
				}
                    
			}
		}


        /*
		 * Método que sirve para poder editar a un empleado -
		 * del sistema (y de la BD).
		 */
        private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			/*
			 * Aqui pasa lo mismo que en el método enfocado para eliminar -
			 * a un empleado, [VER el comentario: 1.1]
			 */
            UsuarioEmpleado? selectedEmpleado = EmployeeDataGrid.SelectedItem as UsuarioEmpleado;

			/*
			 * Aquí tiene un comportamiento similar al del método de eliminar -
			 * [VER el comentario 1.2], salgo con la clara diferencia que aquí -
			 * está enfocado en actualizar (o editar) a un empleado que esta -
			 * siendo seleccionado.
			 */
			if (selectedEmpleado != null)
			{
				var editarEmpleado = new editarEmpleado();
				editarEmpleado.WindowStartupLocation = WindowStartupLocation.CenterScreen;
				editarEmpleado.Cedula.Text = selectedEmpleado.Cedula;
				editarEmpleado.Nombre.Text = selectedEmpleado.Nombre;
				editarEmpleado.Apellidos.Text = selectedEmpleado.PrimerApellido;
				editarEmpleado.Apellidos.Text = selectedEmpleado.SegundoApellido;
				editarEmpleado.Puesto.Text = selectedEmpleado.Puesto;
				editarEmpleado.Fecha.SelectedDate = selectedEmpleado.FechaCreacion;
				editarEmpleado.Salario.Text = selectedEmpleado.Salario.ToString();
				editarEmpleado.Correo.Text = selectedEmpleado.Correo;
				editarEmpleado.Contraseña.Text = selectedEmpleado.Contraseña;
				editarEmpleado.Telefono.Text = selectedEmpleado.Telefono;
                editarEmpleado.DireccionTexto.Text = selectedEmpleado.Direccion;
                editarEmpleado.Activo.IsChecked = selectedEmpleado.Activo;
				editarEmpleado.RolMuestra.Text = selectedEmpleado.IdRol.ToString();
                if (editarEmpleado.ShowDialog() == true)
				{
                    selectedEmpleado.Cedula = editarEmpleado.cedula;
                    selectedEmpleado.Nombre = editarEmpleado.nombre;
                    editarEmpleado.Apellidos.Text = selectedEmpleado.PrimerApellido;
                    editarEmpleado.Apellidos.Text = selectedEmpleado.SegundoApellido;
                    selectedEmpleado.Puesto = editarEmpleado.puesto;
					selectedEmpleado.FechaCreacion = editarEmpleado.fechaContratación;
					selectedEmpleado.Salario = editarEmpleado.salario;
					selectedEmpleado.Correo = editarEmpleado.correo;
                    selectedEmpleado.Contraseña = editarEmpleado.contraseña;
                    selectedEmpleado.Telefono = editarEmpleado.telefono;
					selectedEmpleado.Activo = editarEmpleado.Activo.IsChecked ?? false;
					selectedEmpleado.Direccion = editarEmpleado.direccion;
                    selectedEmpleado.IdRol = Convert.ToInt32(editarEmpleado.rol);
                    EmployeeDataGrid.Items.Refresh();
                    conexion.UpdateEmployee(selectedEmpleado.Cedula, selectedEmpleado.Nombre,
						selectedEmpleado.PrimerApellido, selectedEmpleado.SegundoApellido,
                        selectedEmpleado.Puesto, selectedEmpleado.FechaCreacion,
                        selectedEmpleado.Salario, selectedEmpleado.Correo,
                        selectedEmpleado.Contraseña, selectedEmpleado.Telefono,
                        selectedEmpleado.Activo, selectedEmpleado.Direccion, selectedEmpleado.IdRol);                    
                    LoadData();
				}
			}
		}

		/* 
		 * Este método es para la historia laboral del empleado.
		 */
		private void Button_Click_6(object sender, RoutedEventArgs e)
		{
			Empleado selectedEmpleado = EmployeeDataGrid.SelectedItem as Empleado;
			if (selectedEmpleado != null)
			{
				MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
				if (mainWindow != null)
				{
					mainWindow.ChangePageToMetricas();
				}

			}
		}
	}
	}

