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

namespace FrontEndWPF
{
    /// <summary>
    /// Lógica de interacción para añadirPerfilCompetencial.xaml
    /// </summary>
    public partial class añadirPerfilCompetencial : Window
    {
        public string? titulo_añadirPerfilCompetencial { get; set; }
        public string? descripcion_añadirPerfilCompetencial { get; set; }
        public string? experiencia_añadirPerfilCompetencial { get; set; }
        public string? requisitos_añadirPerfilCompetencial { get; set; }
        public string? ubicacion_añadirPerfilCompetencial { get; set; }
        public double salario_añadirPerfilCompetencial { get; set; }


        //Instancia a la clase: Conexion.
        ConexionEmpleado ConexionEmpleado = new ConexionEmpleado();


        public añadirPerfilCompetencial()
        {
            InitializeComponent();
        }


        /* Método que sirve para poder hacer las validaciones -
         * de los campos que estan en: añadirPerfilCompetencial.xaml.*/
        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validar Titulo
            if (string.IsNullOrWhiteSpace(Titulo_TextBox.Text) && Titulo_TextBox.Text.Length > 4)
            {
                errorMessage = "El campo titulo es obligatorio.";
                return false;
            }

            // Validar Descripción
            if (string.IsNullOrWhiteSpace(Descripcion_TextBox.Text) && Descripcion_TextBox.Text.Length > 6)
            {
                errorMessage = "El campo Descripción es obligatorio.";
                return false;
            }

            // Validar Experiencia
            if (string.IsNullOrWhiteSpace(Experiencia_TextBox.Text) && Experiencia_TextBox.Text.Length > 7)
            {
                errorMessage = "El campo experiencia es obligatorio.";
                return false;
            }

            //Validar Requisitos:
            if (string.IsNullOrWhiteSpace(Requisitos_TextBox.Text) && Requisitos_TextBox.Text.Length > 7)
            {
                errorMessage = "El campo requisitos es obligatorio.";
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
            titulo_añadirPerfilCompetencial = Titulo_TextBox.Text;
            descripcion_añadirPerfilCompetencial = Descripcion_TextBox.Text;
            experiencia_añadirPerfilCompetencial = Experiencia_TextBox.Text;
            requisitos_añadirPerfilCompetencial = Requisitos_TextBox.Text;
            ubicacion_añadirPerfilCompetencial = Ubicacion_TextBox.Text;
            salario_añadirPerfilCompetencial = Convert.ToDouble(Salario_TextBox.Text);
            MessageBox.Show("Perfil competencial registrado exitosamente.", "¡Aviso!",
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

    }
}
