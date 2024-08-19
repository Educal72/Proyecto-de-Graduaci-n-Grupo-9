﻿using FrontEndWPF.Index;
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
    /// Lógica de interacción para añadirIncidente.xaml
    /// </summary>
    public partial class añadirPrestamo : Window
    {        
        Conexion conexion = new Conexion();
        public DateTime fecha_añadirPrestamo { get; set; }
        public double prestado_añadirPrestamo { get; set; }
        public string descripcion_añadirPrestamo { get; set; }
        public double interes_añadirPrestamo { get; set; }
        public string estado_añadirPrestamo { get; set; }
        public int empleado_añadirPrestamo { get; set; }
        

        //Instancia a la clase: Conexion.
        ConexionEmpleado ConexionEmpleado = new ConexionEmpleado();


        //Constructor de tipo vacio de la clase: añadirIncidente.
        public añadirPrestamo()
        {
            InitializeComponent();
            PopulateFiltroComboBox();
            /* Estos dos comandos lo que hacen es ayudar al ComboBox a encontrar -
             * los usuarios que están registrados en el sistema, para asi, mostrarlos -
             * en pantalla.*/
            DataContext = this;
            
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
						Usuarios_ComboBox.Items.Add(new ComboBoxItem
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

		/* Método que sirve para poder hacer las validaciones -
         * de los campos que estan en: añadirIncidente.xaml.*/
		private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validar Fecha
            if (!Fecha_TextBox.SelectedDate.HasValue)
            {
                errorMessage = "El campo Fecha es obligatorio.";
                return false;
            }

            // Validar Hora
            if (!double.TryParse(Prestado_TextBox.Text, out double result))
            {
                errorMessage = "El campo Prestado es obligatorio y debe ser un número válido.";
                return false;
            }

			if (!int.TryParse(Interes_TextBox.Text, out int result2))
			{
				errorMessage = "El campo Interes es obligatorio y debe ser un número válido.";
				return false;
			}

			// Validar Descripción
			if (string.IsNullOrWhiteSpace(Descripcion_TextBox.Text) && Descripcion_TextBox.Text.Length > 6)
            {
                errorMessage = "El campo Descripción es obligatorio.";
                return false;
            }

            //Validar Usuario:
            if (Usuarios_ComboBox.Text == "" || Usuarios_ComboBox.Text == null)
            {
                errorMessage = "El campo Usuario es obligatorio.";
                return false;

            }

			// Todas las validaciones realizadas
			return true;
        }


        /* Método relacionado al botón: Guardar.
         * El cual se encarga de guardar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }
            DateTime? selectedDate = Fecha_TextBox.SelectedDate;
			fecha_añadirPrestamo = selectedDate.Value;
			prestado_añadirPrestamo = Convert.ToDouble(Prestado_TextBox.Text);
			descripcion_añadirPrestamo = Descripcion_TextBox.Text;
			interes_añadirPrestamo = Convert.ToDouble(Interes_TextBox.Text);
			if (Usuarios_ComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
                int empleadoId = GetEmpleadoIdByCedula(Convert.ToInt32(selectedItem.Tag));
				empleado_añadirPrestamo = empleadoId;
			}
            //MessageBox.Show("Prestamo registrado exitosamente.", "¡Aviso!", 
                //MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            
        }


        /* Método relacionado al botón: Cancelar.
         * El cual se encarga de cancelar los datos que están siendo -
         * ingresados por el usuario.*/
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        /* Método relacionado al botón: Actual.
         * El cual se encarga de guardar la fecha actual.*/
        private void FechaActual_Click(object sender, RoutedEventArgs e)
        {
            Fecha_TextBox.SelectedDate = DateTime.Now;
        }

		public int GetEmpleadoIdByCedula(int cedula)
		{
			Conexion conexion = new Conexion();
			int IdEmpleado = 0;
			using (SqlConnection connection = conexion.OpenConnection())
			{
				string query = @"
                SELECT e.Id
                FROM Usuario u
                JOIN Empleado e ON u.Id = e.IdUsuario
                WHERE u.Cedula = @Cedula";
				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.Add(new SqlParameter("@Cedula", cedula));
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
                            IdEmpleado = (int)reader["Id"];

                            return IdEmpleado;
						}
					}
				}
			}
			return IdEmpleado;
		}

		/* Método relacionado al ComboBox, este se encarga de poder guardar -
         * los usuarios que estan registrados en el sistema.*/

	}
}
