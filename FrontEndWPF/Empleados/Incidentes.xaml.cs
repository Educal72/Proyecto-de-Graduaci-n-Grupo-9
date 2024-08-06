using FrontEndWPF.Empleados;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Lógica de interacción para Incidentes.xaml
    /// </summary>
    public partial class Incidentes : UserControl
    {
        //Instancias a las clases: Conexion y ConexionEmpleado.
        Conexion conexion = new Conexion();
        ConexionEmpleado conexionEmpleado = new ConexionEmpleado();
		private ICollectionView customerView;

		/* -------------------
         * Variables Globales:
         * -------------------
         * 
         * Estas son variables de tipo booleanas que nos ayudaran -
         * a poder saber si el usuario quiere ver los registros de -
         * los incidentes de tipo: Interno ó Externo.
         */
		static bool validacionTipoInterno;
        static bool validacionTipoExterno;


        //Constructor de tipo vacio de la clase: Incidentes.
        public Incidentes()
        {
            InitializeComponent();
            conexion.OpenConnection();
			PopulateFiltroComboBox();
			LoadDataInterno();
			//MessageBox.Show("Bienvenido(a) al apartado de incidentes", "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
		}


        /* 
         * Métodos que están encargados para mostrar en pantalla los datos que -
         * son de tipo interno y externo. 
         */
        private void LoadDataInterno()
        {
            string query = @"
                SELECT Id, Fecha, Hora, Descripcion, Tipo, Estado, IdUsuario
                FROM Incidentes ";

            List<Modelos.UserModel.Incidente> incidentes = new List<Modelos.UserModel.Incidente>();

            using (SqlConnection connection = conexion.OpenConnection())
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        incidentes.Add(new Modelos.UserModel.Incidente
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
							Usuario = conexionEmpleado.UsuarioPorID(Convert.ToInt32(reader["IdUsuario"].ToString())),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Hora = reader["Hora"].ToString()!,
                            Descripcion = reader["Descripcion"].ToString()!,
                            Tipo = reader["Tipo"].ToString()!,
                            Estado = reader["Estado"].ToString()!
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
                IncidenteDataGrid.ItemsSource = incidentes;
            }

            /* 
             * Esto se hace para saber si hay datos con el tipo: Interno, -
             * esto con el motivo de avisarle al usuario de que no hay datos de -
             * incidentes de tipo internos.
             */
        }

		private void PopulateFiltroComboBox()
		{
			string query = @"
            SELECT 
                Id, 
                Nombre, 
                Apellido
            FROM 
                Usuario"
			;

			combo.Items.Add(new ComboBoxItem
			{
				Content = "Todos",
				Tag = 1 // Usar el Tag para almacenar la cédula asociada
			});

			using (SqlConnection connection = conexion.OpenConnection())
			{
				try
				{
					SqlCommand command = new SqlCommand(query, connection);
					SqlDataReader reader = command.ExecuteReader();

					while (reader.Read())
					{
						string cedula = reader["Id"].ToString();
						string nombre = reader["Nombre"].ToString();
						string primerApellido = reader["Apellido"].ToString();

						// Formatear la opción del ComboBox
						string displayText = $"{nombre} {primerApellido}";

						// Crear un ComboBoxItem con el texto y agregarlo al ComboBox
						combo.Items.Add(new ComboBoxItem
						{
							Content = displayText,
							Tag = cedula // Usar el Tag para almacenar la cédula asociada
						});
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.Message);
				}
			}
		}

		/* Método relacionado al botón de: Nuevo, el cual crea un nuevo -
         registro de incidente.*/
		private void Button_Nuevo(object sender, RoutedEventArgs e)
        {
            /*
             * Aqui valida si el usuario esta creando un nuevo incidente, -
             * dentro del tipo: Interno.
             */
            //if (validacionTipoInterno != false)
            //{
                var nuevoIncidente = new añadirIncidente();
                nuevoIncidente.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                if (nuevoIncidente.ShowDialog() == true)
                {
                    DateTime Fecha = nuevoIncidente.fecha_añadirIncidente;
                    string Hora = nuevoIncidente.hora_añadirIncidente;
                    string Descripcion = nuevoIncidente.descripcion_añadirIncidente;
                    string Tipo = nuevoIncidente.tipo_añadirIncidente;
                    string Usuario = nuevoIncidente.Usuario_añadirIncidente;

                    /*
                     * Aquí se manda por parámetro al método llamado: AgregarIncidente, -
                     * esto con la finalidad de que pueda ser enviado y registrado a la -
                     * base de datos que esta en SQL Server.
                     */
                    conexionEmpleado.AgregarIncidente(Fecha, Hora, Descripcion, Tipo, conexionEmpleado.getIdUsuario(Usuario));
                    LoadDataInterno();
                }
            //}
            /*
             * Aqui valida si el usuario esta creando un nuevo incidente, -
             * dentro del tipo: Externo.
             */
            //else if (validacionTipoExterno != false)
            //{
                
            //}
            /*
             * Aqui muestra un mensaje de aviso en dado caso que el usuario le de al botón de -
             * "Nuevo" sin haber seleccionado alguno de las dos opciones (en este caso: Interno ó Externo) -
             * que se le estan brindando.
             * 
             * Esto se hace por buenas prácticas de seguridad.
             */
            //else
            //{
            //    MessageBox.Show("No seleccionaste que tipo incidente deseas ver.",
            //                    "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }


        /* Método relacionado al botón de: Si, el cual marca el incidente seleccionado -
         * como "Resuelto".*/
        private void Si_Click(object sender, RoutedEventArgs e)
        {
            /* Esto sirve para poder detectar si el usuario esta -
             * seleccionando un incidente que se le muestra en pantalla.*/
            Modelos.UserModel.Incidente selectedIncidente = IncidenteDataGrid.SelectedItem as Modelos.UserModel.Incidente;
            if (selectedIncidente != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Quieres marcar este dato como resuelto?",
                    "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);


                /* Aqui lo que hace es que si el usuario selecciono un incidente y le dio al botón -
                 * para poder continuar, entonces marcara el incidente como "Resuelto".
                 * 
                 * Caso contrario, entonces mandara un mensaje, mencionando que se cancelo la solicitud -
                 * de marcación. */
                if (result == MessageBoxResult.Yes && selectedIncidente != null)
                {
                    string Estado = "Resuelto";
                    conexionEmpleado.MarcarIncidente(Estado, selectedIncidente.Id.ToString());
                    LoadDataInterno();
                }
                else
                {
                    MessageBox.Show("Se cancelo la marcación, muchas gracias.", "¡Aviso!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}


        /* Método relacionado al botón de: No, el cual marca el incidente seleccionado -
         * como "No resuelto".*/
        private void No_Click(object sender, RoutedEventArgs e)
        {
            /* Esto sirve para poder detectar si el usuario esta -
             * seleccionando un incidente que se le muestra en pantalla.*/
            Modelos.UserModel.Incidente selectedIncidente = IncidenteDataGrid.SelectedItem as Modelos.UserModel.Incidente;
            if (selectedIncidente != null)
            {
                MessageBoxResult result = MessageBox.Show("¿Quieres marcar este dato como no resuelto?",
                    "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);


                /* Aqui lo que hace es que si el usuario selecciono un incidente y le dio al botón -
                 * para poder continuar, entonces marcara el incidente como "No resuelto".
                 * 
                 * Caso contrario, entonces mandara un mensaje, mencionando que se cancelo la solicitud -
                 * de marcación. */
                if (result == MessageBoxResult.Yes && selectedIncidente != null)
                {
                    string Estado = "No resuelto";
                    string Usuario = selectedIncidente.Usuario;
                    conexionEmpleado.MarcarIncidente(Estado, selectedIncidente.Id.ToString());
                    LoadDataInterno();
                }
                else
                {
                    MessageBox.Show("Se cancelo la marcación, muchas gracias.", "¡Aviso!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}


        /* Método relacionado al botón de: Interno, el cual mandara un true para indicar que se -
           quiere ver los incidentes de tipo: Interno, y luego de eso, manda a llamar el método -
           que se encarga de mostrar dichos incidentes internos.*/
        private void Interno_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de ver los registros de incidentes internos?",
                "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                validacionTipoInterno = true;
                LoadDataInterno();
            }
        }


        /* Método relacionado al botón de: Externo, el cual mandara un true para indicar que se -
           quiere ver los incidentes de tipo: Externo, y luego de eso, manda a llamar el método -
           que se encarga de mostrar dichos incidentes externos.*/
        private void Externo_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de ver los registros de incidentes externos?",
                "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                validacionTipoExterno = true;
            }
        }

		private bool FilterCustomers(object item)
		{
			Modelos.UserModel.Incidente incidentes = item as Modelos.UserModel.Incidente;
			if (incidentes == null)
				return false;

			string clienteCedulaMin = incidentes.IdUsuario.ToString();
			if (combo.SelectedItem is ComboBoxItem selectedItem)
			{
				if (!string.IsNullOrEmpty(selectedItem.Tag.ToString()) && clienteCedulaMin != selectedItem.Tag.ToString())
					return false;
			}

			return true;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (combo.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() == "Todos")
			{
				if (customerView != null)
				{
					customerView.Filter = null; // Eliminar el filtro para mostrar todas las entradas
				}
			}
			else
			{
				if (customerView == null)
					customerView = CollectionViewSource.GetDefaultView(IncidenteDataGrid.Items);

				customerView.Filter = FilterCustomers; // Aplicar el filtro personalizado
			}

		}

	}
}
