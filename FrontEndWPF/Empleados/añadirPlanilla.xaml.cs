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
    /// Lógica de interacción para añadirPlanilla.xaml
    /// </summary>
    public partial class añadirPlanilla : Window
    {
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? cedula { get; set; }
        public string? puesto { get; set; }
        public string? correo { get; set; }
        public DateTime fechacreacion { get; set; }
        public double salario { get; set; }
               
        public añadirPlanilla()
        {
            InitializeComponent();
        }

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            // Validar Nombre
            if (string.IsNullOrWhiteSpace(Nombre.Text) && Nombre.Text.Length > 3)
            {
                errorMessage = "El campo Nombre es obligatorio.";
                return false;
            }

            // Validar Apellidos
            if (string.IsNullOrWhiteSpace(Apellidos.Text) && Apellidos.Text.Length > 6)
            {
                errorMessage = "El campo Apellidos es obligatorio.";
                return false;
            }

            // Validar Cedula
            if (!int.TryParse(Cedula.Text, out _) && Cedula.Text.Length > 5)
            {
                errorMessage = "El campo Cedula es obligatorio y debe ser válido (sin guiones).";
                return false;
            }

            // Validar Salario
            if (!double.TryParse(Salario.Text, out _))
            {
                errorMessage = "El campo Salario debe ser un número válido.";
                return false;
            }

            // Validar Puesto
            if (string.IsNullOrWhiteSpace(Puesto.Text) || Puesto.Text.Length < 3)
            {
                errorMessage = "El campo Puesto es obligatorio.";
                return false;
            }

            // Validar Fecha de Creacion
            if (!Fecha.SelectedDate.HasValue)
            {
                errorMessage = "El campo Fecha de Creación es obligatorio.";
                return false;
            }

            // Validar Correo
            int index = Correo.Text.IndexOf("@");            
            if (string.IsNullOrWhiteSpace(Correo.Text) && index > 0)
            {
                errorMessage = "El campo Correo es obligatorio.";
                return false;
            }
            
            ConexionEmpleado conexionEmpleado = new ConexionEmpleado();
            var con = conexionEmpleado.SelectUserCedula(Correo.Text, Convert.ToInt32(Cedula.Text));
            if (con.Count() > 0)
            {
                errorMessage = "La planilla a crear ya existe.";
                return false;
            }

            // Todas las validaciones realizadas
            return true;
        }


        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }
            DateTime? selectedDate = Fecha.SelectedDate;
            nombre = Nombre.Text;
            apellidos = Apellidos.Text;
            cedula = Cedula.Text;            
            puesto = Puesto.Text;
            correo = Correo.Text;
            fechacreacion = selectedDate.Value;            
            salario = double.Parse(Salario.Text);
            DialogResult = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Actual(object sender, RoutedEventArgs e)
        {
            Fecha.SelectedDate = DateTime.Now;
        }
    }
}
