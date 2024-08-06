using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private ICollectionView customerView;


		public ControlPlanillas()
        {
            InitializeComponent();
            conexion.OpenConnection();
            LoadData();
            PopulateFiltroComboBox();
        }

        private void LoadData()
        {
            string query = @"SELECT 
    Usuario.Id,
    Usuario.Nombre,
    Usuario.Apellido,
    Usuario.Cedula,
    Empleado.Puesto,
    Usuario.Telefono,
    Empleado.FechaContratacion,
    Empleado.Salario
FROM 
    Empleado
JOIN 
    Usuario ON Empleado.IdUsuario = Usuario.Id;";

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
                            IdUsuario = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString()! + " " + reader["Apellido"].ToString()!,
                            Apellidos = reader["Apellido"].ToString()!,                            
                            Cedula = reader["Cedula"].ToString()!,
                            Puesto = reader["Puesto"].ToString()!,
                            Telefono = reader["Telefono"].ToString()!,
                            FechaCreacion = Convert.ToDateTime(reader["FechaContratacion"]),
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


        //private void Button_Añadir(object sender, RoutedEventArgs e)
        //{
        //    var nuevaPlanilla = new añadirPlanilla();
        //    nuevaPlanilla.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    if (nuevaPlanilla.ShowDialog() == true)
        //    {
        //        string Nombre = nuevaPlanilla.nombre;
        //        string Apellidos = nuevaPlanilla.apellidos;
        //        string Cedula = nuevaPlanilla.cedula;
        //        string Puesto = nuevaPlanilla.puesto;
        //        string Correo = nuevaPlanilla.correo;
        //        DateTime Fecha = nuevaPlanilla.fechacreacion;
        //        double Salario = nuevaPlanilla.salario;

        //        var ResultadoPlanilla = conexionEmpleado.AgregarPlanilla(Nombre, Apellidos, Cedula, Puesto, Correo, Fecha, Salario);
        //        if (ResultadoPlanilla)
        //        {
        //            MessageBox.Show("Se inserto correctamente la planilla, muchas gracias.",
        //                       "¡Inserción exitosa!", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Hubo un error en la inserción, intentelo de nuevo por favor.", "¡Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        LoadData();
        //    }
        //}

        private void Button_Editar(object sender, RoutedEventArgs e)
        {
            ControlPlanilla? selectedPlanilla =
                PlanillaDataGrid.SelectedItem as ControlPlanilla;

            if (selectedPlanilla != null)
            {
                string oldCedula = selectedPlanilla.Cedula!;
                var editarPlanilla = new editarPlanilla();
                editarPlanilla.WindowStartupLocation =
                WindowStartupLocation.CenterScreen;

                editarPlanilla.Nombre.Text = selectedPlanilla.Nombre;
                editarPlanilla.Apellidos.Text = selectedPlanilla.Apellidos;                
                editarPlanilla.Cedula.Text = selectedPlanilla.Cedula;
                editarPlanilla.Puesto.Text = selectedPlanilla.Puesto;
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


                    PlanillaDataGrid.Items.Refresh();
                   var ResultadoPlanilla = conexionEmpleado.ActualizarPlanilla(selectedPlanilla.Salario, selectedPlanilla.IdUsuario, selectedPlanilla.FechaCreacion);
                    
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
			else
			{
				MessageBox.Show("Por favor, seleccione un elemento de la lista.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		//    private void Button_Eliminar(object sender, RoutedEventArgs e)
		//    {

		//        ControlPlanilla? selectedPlanilla =
		//            PlanillaDataGrid.SelectedItem as ControlPlanilla;


		//        if (selectedPlanilla != null)
		//        {
		//MessageBoxResult result = MessageBox.Show("¿Quieres eliminar esta planilla?",
		//"¡Advertencia!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

		//            if (result == MessageBoxResult.Yes)
		//            {
		//                var ResultadoPlanilla = conexionEmpleado.EliminarPlanilla(selectedPlanilla.Correo, selectedPlanilla.Cedula);
		//                LoadData();
		//            }  

		//        }
		//    }

		private void PopulateFiltroComboBox()
		{
			string query = @"
            SELECT 
                Cedula, 
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
						string cedula = reader["Cedula"].ToString();
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

		private bool FilterCustomers(object item)
		{
			ControlPlanilla planilla = item as ControlPlanilla;
			if (planilla == null)
				return false;

			string clienteCedulaMin = planilla.Cedula.ToLower();
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
					customerView = CollectionViewSource.GetDefaultView(PlanillaDataGrid.Items);

				customerView.Filter = FilterCustomers; // Aplicar el filtro personalizado

			}

		}
	}
}
