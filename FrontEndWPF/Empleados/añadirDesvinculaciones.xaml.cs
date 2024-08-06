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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para añadirDesvinculaciones.xaml
    /// </summary>

    public partial class añadirDesvinculaciones : Window
    {
        /* Propiedades que guardan los datos de las desvinculaciones que fueron digitadas -
         * por el usuario con rol de: Administrador ó Empleado.*/
        public string? nombre_añadirDesvinculaciones { get; set; }
        public string? apellido_añadirDesvinculaciones { get; set; }
        public DateTime fechaInicio_añadirDesvinculaciones { get; set; }
        public string? motivo_añadirDesvinculaciones { get; set; }
        public string? comentarios_añadirDesvinculaciones { get; set; }
		public string? cedula_añadirDesvinculaciones { get; set; }
		public DateTime fechaSalida_añadirDesvinculaciones { get; set; }
        Conexion conexion  = new Conexion();

        //Constructor vacio.
        public añadirDesvinculaciones()
        {
            InitializeComponent();
            PopulateFiltroComboBox();
        }

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
						string Apellido = reader["Apellido"].ToString();

						// Formatear la opción del ComboBox
						string displayText = $"{nombre} {Apellido} ({cedula})";

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


		/* Método que se encarga de validar los espacios correspondientes en -
		 * donde el usuario administrador o empleado agregara los datos de la -
		 * nueva solicitud de desvinculación en el sistema.*/
		private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;


            // Validar Fecha de inicio:
            if (!FechaInicio_Picker.SelectedDate.HasValue)
            {
                errorMessage = "El campo Fecha de Inicio es obligatorio.";
                return false;
            }

            // Validar Motivo:
            if (string.IsNullOrWhiteSpace(Motivo_Textbox.Text) && Motivo_Textbox.Text.Length > 6)
            {
                errorMessage = "El campo Motivo es obligatorio.";
                return false;
            }

            // Validar Comentarios:
            if (string.IsNullOrWhiteSpace(Comentarios_Textbox.Text) && Comentarios_Textbox.Text.Length > 6)
            {
                errorMessage = "El campo Comentarios es obligatorio.";
                return false;
            }

            // Validar Fecha de salida:
            if (!FechaSalida_Picker.SelectedDate.HasValue)
            {
                errorMessage = "El campo Fecha de Salida es obligatorio.";
                return false;
            }

            // Todas las validaciones realizadas
            return true;
        }


        /*  Método en donde se guardan los datos que el usuario administrador ó -
         *  empleado esta colocando. Además, este método esta enlazado al botón -
         *  de: "Guardar".*/
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show("Hay un conflicto de datos o estás omitiendo un campo obligatorio.\n"
                    + errorMessage, "Alerta", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (combo.SelectedItem is ComboBoxItem selectedItem)
            {
                // Recupera el Tag del ítem seleccionado
                cedula_añadirDesvinculaciones = selectedItem.Tag.ToString();
			}
            DateTime? selectedDateInicio = FechaInicio_Picker.SelectedDate;
            fechaInicio_añadirDesvinculaciones = selectedDateInicio!.Value;
            
            motivo_añadirDesvinculaciones = Motivo_Textbox.Text;
            comentarios_añadirDesvinculaciones = Comentarios_Textbox.Text;
            
            DateTime? selectedDateFinal = FechaSalida_Picker.SelectedDate;
            fechaSalida_añadirDesvinculaciones = selectedDateFinal!.Value;

            DialogResult = true;
        }


        /*  Método en donde se cancelan los datos que el usuario administrador ó -
         *  empleado esta colocando. Además, este método esta enlazado al botón -
         *  de: "Cancelar".*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        /* Método en donde el usuario administrador o empleado le dara en el botón "Actual" -
         * para poder guardar la fecha actual de la nueva desvinculación.*/
        private void FechaActual_Inicio_Click(object sender, RoutedEventArgs e)
        {
            FechaInicio_Picker.SelectedDate = DateTime.Now;
        }


        /* Método en donde el usuario administrador o empleado le dara en el botón "Actual" -
         * para poder guardar la fecha actual de la nueva desvinculación.*/
        private void FechaActual_Salida_Click(object sender, RoutedEventArgs e)
        {
           FechaSalida_Picker.SelectedDate = DateTime.Now;
        }
    }
}
