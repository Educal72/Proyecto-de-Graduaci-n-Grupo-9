using FrontEndWPF.Index;
using System;
using System.Collections.Generic;
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
    public partial class añadirIncidente : Window
    {        
        public DateTime fecha_añadirIncidente { get; set; }
        public string? hora_añadirIncidente { get; set; }
        public string? descripcion_añadirIncidente { get; set; }
        public string? tipo_añadirIncidente { get; set; }
        public string? estado_añadirIncidente { get; set; }
        public string? Usuario_añadirIncidente { get; set; }
        

        //Instancia a la clase: Conexion.
        ConexionEmpleado ConexionEmpleado = new ConexionEmpleado();


        //Constructor de tipo vacio de la clase: añadirIncidente.
        public añadirIncidente()
        {
            InitializeComponent();
            
            /* Estos dos comandos lo que hacen es ayudar al ComboBox a encontrar -
             * los usuarios que están registrados en el sistema, para asi, mostrarlos -
             * en pantalla.*/
            DataContext = this;
            NombreUsuarios.AddRange(ConexionEmpleado.Usuarios());
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
            if (string.IsNullOrWhiteSpace(Hora_TextBox.Text) && Hora_TextBox.Text.Length > 4)
            {
                errorMessage = "El campo hora es obligatorio.";
                return false;
            }

            // Validar Descripción
            if (string.IsNullOrWhiteSpace(Descripcion_TextBox.Text) && Descripcion_TextBox.Text.Length > 6)
            {
                errorMessage = "El campo Descripción es obligatorio.";
                return false;
            }

            // Validar Tipo
            if (string.IsNullOrWhiteSpace(TipoIncidente_TextBox.Text) && TipoIncidente_TextBox.Text.Length > 7)
            {
                errorMessage = "El campo tipo es obligatorio.";
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
            fecha_añadirIncidente = selectedDate.Value;
            hora_añadirIncidente = Hora_TextBox.Text;
            descripcion_añadirIncidente = Descripcion_TextBox.Text;
            tipo_añadirIncidente = TipoIncidente_TextBox.Text;                        
            Usuario_añadirIncidente = Usuarios_ComboBox.Text;
            MessageBox.Show("Incidente registrado exitosamente.", "¡Aviso!", 
                MessageBoxButton.OK, MessageBoxImage.Information);
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


        /* Método relacionado al ComboBox, este se encarga de poder guardar -
         * los usuarios que estan registrados en el sistema.*/
        public List<string> NombreUsuarios { get; } = new List<string>();
    }
}
