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
    /// Lógica de interacción para editarPlanilla.xaml
    /// </summary>
    public partial class editarPlanilla : Window
    {
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? cedula { get; set; }
        public string? puesto { get; set; }
        public string? correo { get; set; }
        public DateTime fechacreacion { get; set; }
        public double salario { get; set; }

        public editarPlanilla()
        {
            InitializeComponent();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Quieres editar esta planilla?",
                "¡Confirmación!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
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
            else
            {
                MessageBox.Show("Se cancelo la solicitud, muchas gracias",
                    "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = false;
            }

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se cancelo la solicitud, muchas gracias",
                "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = false;
        }

        private void Button_Actual(object sender, RoutedEventArgs e)
        {
            Fecha.SelectedDate = DateTime.Now;
        }


    }
}
